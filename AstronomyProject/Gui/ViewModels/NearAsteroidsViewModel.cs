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

namespace Gui.ViewModels
{
    public class NearAsteroidsViewModel : ViewModelBase
    {
        readonly INearAsteroidService _nearAsteroidService;

        public NearAsteroidsViewModel(INearAsteroidService nearAsteroidService)
        {
            _nearAsteroidService = nearAsteroidService;
        }

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
                TotalAsteroids = NearAsteroids.Count;
                LoadPieSeries();
                LoadSpeedSerise();
                IsLoading = false;
            });


        public ObservableCollection<ISeries> RiskInformation { get; set; } = new();

        public ObservableCollection<NearAsteroid> NearAsteroids { get; set; } = new();

        private int _totalAsteroids;
        public int TotalAsteroids
        {
            get => _totalAsteroids; 
            set => SetProperty(ref _totalAsteroids, value); 
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
            foreach(var a in NearAsteroids)
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

        public ObservableCollection<ISeries> SpeedInfo { get; set; } = new();


        public ObservableCollection<Axis> XAxes { get; set; } = new();

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
