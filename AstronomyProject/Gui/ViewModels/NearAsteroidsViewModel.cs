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
using Prism.Regions;

namespace Gui.ViewModels
{
    public class NearAsteroidsViewModel : ViewModelBase , INavigationAware
    {
        readonly INearAsteroidService _nearAsteroidService;

        public NearAsteroidsViewModel(INearAsteroidService nearAsteroidService )
        {
            _nearAsteroidService = nearAsteroidService;
        }

        DelegateCommand _load;
        public DelegateCommand Load => _load ??= new DelegateCommand(
            async () =>
            {
                if (_allAsteroids.Any())
                {
                    return;
                }
                IsLoading = true;
                var asteroids = await _nearAsteroidService.GetNearAsteroids();
                
                OnLoading(asteroids);
                
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

                OnLoading(asteroids);
                
                IsLoading = false;
            });

        private void OnLoading(IEnumerable<NearAsteroid> asteroids)
        {
            _allAsteroids.Clear();
            _allAsteroids.AddRange(asteroids);

            Asteroids.Clear();
            Asteroids.AddRange(asteroids);

            LoadPieSeries();
        }

        public ObservableCollection<ISeries> RiskInformation { get; set; } = new();

        private List<NearAsteroid> _allAsteroids = new();

        private double _diameter;
        public double Diameter
        {
            get => _diameter;
            set
            {
                SetProperty(ref _diameter, value);
            }
        }

        public ObservableCollection<NearAsteroid> Asteroids { get; set; } = new();

        DelegateCommand _filterByDiameter;
        public DelegateCommand FilterByDiameter => _filterByDiameter ??= new DelegateCommand(
             () =>
            {
                Asteroids.Clear();
                Asteroids.AddRange(_allAsteroids.Where(a => a.EstimatedDiameterMin >= Diameter));

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
                LoadRelativeVelocitySeries();
                LoadMissDistanceSeries();
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
                    Values = new int[]{ _allAsteroids.Where(x => x.IsSentryObject).Count() }
                },
                new PieSeries<int>
                {
                    Name = "Potentially hazardous",
                    Values = new int[]{ _allAsteroids.Where(x => x.IsPotentiallyHazardousAsteroid).Count() }
                },
                new PieSeries<int>
                {
                    Name = "Not dangerous at all",
                    Values = new int[]{ _allAsteroids.Where(x => !x.IsPotentiallyHazardousAsteroid && !x.IsSentryObject).Count() }
                },

            };
            RiskInformation.Clear();
            RiskInformation.AddRange(sec);
        }

        private void LoadRelativeVelocitySeries()
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

            RelativeVelocitySeries.Clear();
            RelativeVelocitySeries.AddRange(series);
        }

        public ObservableCollection<ISeries> RelativeVelocitySeries { get; set; } = new();

        public ObservableCollection<ICartesianAxis> RelativeVelocityXAxesDateTime { get; set; } = new()
        {
            new Axis
            {
                Labeler = value => new DateTime((long)value).ToString("dd/MM/yyyy"),
                LabelsRotation = 15,
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

        private void LoadMissDistanceSeries()
        {
            var series = new List<ISeries>
            {
                new ColumnSeries<DateTimePoint>
                {
                    Values =
                    new ObservableCollection<DateTimePoint>(from c in CloseApproach
                                                            select new DateTimePoint(c.CloseApproachDate, c.MissDistance))
                }
            };

            MissDistanceSeries.Clear();
            MissDistanceSeries.AddRange(series);
        }

        public ObservableCollection<ISeries> MissDistanceSeries { get; set; } = new();

        public ObservableCollection<ICartesianAxis> MissDistanceXAxesDateTime { get; set; } = new()
        {
            new Axis
            {
                Labeler = value => new DateTime((long)value).ToString("dd/MM/yyyy"),
                LabelsRotation = 15,
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

        public void OnNavigatedTo(NavigationContext navigationContext) { }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }


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
