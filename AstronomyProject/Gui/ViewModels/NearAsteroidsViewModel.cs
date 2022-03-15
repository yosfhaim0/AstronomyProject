using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DomainModel.Services;
using Prism.Commands;
using Prism.Mvvm;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;

namespace Gui.ViewModels
{
    public class NearAsteroidsViewModel : ViewModelBase
    {
        readonly INearAsteroidService _nearAsteroidService;

        public NearAsteroidsViewModel(INearAsteroidService nearAsteroidService)
        {
            _nearAsteroidService = nearAsteroidService;
        }

        DelegateCommand _load;
        public DelegateCommand Load => _load ??= new DelegateCommand(
            async () =>
            {
                if (NearAsteroids.Any())
                {
                    return;
                }
                IsLoading = true;
                var asteroids = await _nearAsteroidService.GetNearAsteroids();
                NearAsteroids.Clear();
                NearAsteroids.AddRange(asteroids);
                AsteroidsGreterThen.Clear();
                AsteroidsGreterThen.AddRange(asteroids);
                LoadPieSeries();
                LoadSpeedSerise();
                IsLoading = false;
            });

        private DateTime? _fromDate;
        public DateTime? FromDate
        {
            get => _fromDate;
            set => SetProperty(ref _fromDate, value);
        }

        private DateTime? _toDate;
        public DateTime? ToDate
        {
            get => _toDate;
            set => SetProperty(ref _toDate, value);
        }

        DelegateCommand _searchBetweenDates;
        public DelegateCommand SearchBetweenDates => _searchBetweenDates ??= new DelegateCommand(
            async () =>
            {
                IsLoading = true;
                var asteroids = await _nearAsteroidService
                .SearchNearAsteroids(FromDate.Value, ToDate.Value);

                NearAsteroids.Clear();
                NearAsteroids.AddRange(asteroids);

                AsteroidsGreterThen.Clear();
                AsteroidsGreterThen.AddRange(asteroids);

                LoadPieSeries();
                LoadSpeedSerise();
                IsLoading = false;
            });


        public ObservableCollection<ISeries> RiskInformation { get; set; } = new();

        public ObservableCollection<NearAsteroid> NearAsteroids { get; set; } = new();

        private double _diameter;
        public double Diameter
        {
            get => _diameter;
            set => SetProperty(ref _diameter, value);
        }

        public ObservableCollection<NearAsteroid> AsteroidsGreterThen { get; set; } = new();

        DelegateCommand _filterByDiameter;
        public DelegateCommand FilterByDiameter => _filterByDiameter ??= new DelegateCommand(
             () =>
            {
                AsteroidsGreterThen.Clear();
                AsteroidsGreterThen.AddRange(NearAsteroids.Where(a => a.EstimatedDiameterMin >= Diameter));

            });


        private NearAsteroid _selectedAstroeid = new();
        public NearAsteroid SelectedAstroeid
        {
            get => _selectedAstroeid;
            set
            {
                if (value == null) return;
                SetProperty(ref _selectedAstroeid, value);
                CloseApproach = new(_selectedAstroeid?.CloseApproachs);
                LoadCloseApproachSeries();
            }
        }

        private ObservableCollection<CloseApproach> _closeApproach = new();
        public ObservableCollection<CloseApproach> CloseApproach
        {
            get => _closeApproach;
            set => SetProperty(ref _closeApproach, value);
        }

        private void LoadPieSeries()
        {
            var sec = new ObservableCollection<ISeries>
            {
                new PieSeries<int>
                {
                    Name = "Sentry objects",
                    Values = new int[]{ NearAsteroids.Where(x => x.IsSentryObject).Count() }
                },
                new PieSeries<int>
                {
                    Name = "Potentially hazardous",
                    Values = new int[]{ NearAsteroids.Where(x => x.IsPotentiallyHazardousAsteroid).Count() }
                },
                new PieSeries<int>
                {
                    Name = "Not dangerous at all",
                    Values = new int[]{ NearAsteroids.Where(x => !x.IsPotentiallyHazardousAsteroid && !x.IsSentryObject).Count() }
                },

            };
            RiskInformation.Clear();
            RiskInformation.AddRange(sec);
        }

        private void LoadSpeedSerise()
        {
            var series = new List<ISeries>();
            foreach (var a in NearAsteroids)
            {
                series.Add(new ColumnSeries<double>
                {
                    Name = a.Name,
                    Values = new ObservableCollection<double>(a
                    .CloseApproachs
                    .Select(c => c.RelativeVelocity))
                });
            }

            SpeedInfo.Clear();
            SpeedInfo.AddRange(series);
        }

        private void LoadCloseApproachSeries()
        {
            var series = new List<ISeries>
            {
                new ColumnSeries<DateTimePoint>
                {
                    Values =
                    new ObservableCollection<DateTimePoint>(from c in CloseApproach
                                                            select new DateTimePoint(c.CloseApproachDate, c.RelativeVelocity))
                }
            };

            CloseApproachSeries.Clear();
            CloseApproachSeries.AddRange(series);
        }

        public ObservableCollection<ISeries> CloseApproachSeries { get; set; } = new();

        public ObservableCollection<ICartesianAxis> XAxesDateTime { get; set; } = new()
        {
            new Axis
            {
                Labeler = value => new DateTime((long)value).ToString("dd/MM/yyyy"),
                LabelsRotation = 15,

                // in this case we want our columns with a width of 1 day, we can get that number
                // using the following syntax
                UnitWidth = TimeSpan.FromDays(1).Ticks, // mark

                // The MinStep property forces the separator to be greater than 1 day.
                MinStep = TimeSpan.FromDays(1).Ticks // mark

                // if the difference between our points is in hours then we would:
                // UnitWidth = TimeSpan.FromHours(1).Ticks,

                // since all the months and years have a different number of days
                // we can use the average, it would not cause any visible error in the user interface
                //Months: TimeSpan.FromDays(30.4375).Ticks
                //Years: TimeSpan.FromDays(365.25).Ticks
            }
        };

        public ObservableCollection<ISeries> SpeedInfo { get; set; } = new();


        //public List<Axis> YAxes { get; set; } = new List<Axis>
        //{
        //    new()
        //    {
        //        // Now the Y axis we will display labels as currency
        //        // LiveCharts provides some common formatters
        //        // in this case we are using the currency formatter.
        //        Labeler = Labelers.Currency

        //        // you could also build your own currency formatter
        //        // for example:
        //        // Labeler = (value) => value.ToString("C")

        //        // But the one that LiveCharts provides creates shorter labels when
        //        // the amount is in millions or trillions
        //    }
        //};
    }
}
