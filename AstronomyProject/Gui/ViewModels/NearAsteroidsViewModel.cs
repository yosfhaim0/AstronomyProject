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
using Gui.Dialogs;

namespace Gui.ViewModels
{
    public class NearAsteroidsViewModel : ViewModelBase , INavigationAware
    {
        readonly INearAsteroidService _nearAsteroidService;
        IDialogService _dialogService;

        public NearAsteroidsViewModel(INearAsteroidService nearAsteroidService, IDialogService dialogService)
        {
            _nearAsteroidService = nearAsteroidService;
            _dialogService = dialogService;
        }

        private async Task Search()
        {
            IsLoading = true;
            try
            {
                var asteroids = await _nearAsteroidService
                        .SearchNearAsteroids(FromDate.Value, ToDate.Value);
                OnLoading(asteroids);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _dialogService.ShowDialog("Erorr", ex.ParamName);

            }

            IsLoading = false;
        }

        private DateTime? _fromDate = DateTime.Today.AddDays(-3);
        public DateTime? FromDate
        {
            get => _fromDate;
            set => SetProperty(ref _fromDate, value);
        }

        private DateTime? _toDate = DateTime.Today;
        public DateTime? ToDate
        {
            get => _toDate;
            set => SetProperty(ref _toDate, value);
        }

        private int _totalCount;

        public int TotalCount
        {
            get => _totalCount; 
            set => SetProperty(ref _totalCount, value); 
        }

        DelegateCommand _load;
        public DelegateCommand Load => _load ??= new DelegateCommand(
            async () =>
            {
                if (IsLoading) 
                    return;
                await Search();
            });

        DelegateCommand _searchBetweenDates;
        public DelegateCommand SearchBetweenDates => _searchBetweenDates ??= new DelegateCommand(
            async () =>
            {
                await Search();
            });

        private void OnLoading(IEnumerable<NearAsteroid> asteroids)
        {
            _allAsteroids.Clear();
            _allAsteroids.AddRange(asteroids);

            TotalCount = _allAsteroids.Count;

            Asteroids.Clear();
            Asteroids.AddRange(asteroids);

            SetRiskInfoSeries();
        }

        readonly List<NearAsteroid> _allAsteroids = new();

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
                SetRelativeVelocitySeries();
                SetMissDistanceSeries();
                SetOrbitingBodySeries();
            }
        }

        private ObservableCollection<CloseApproach> _closeApproach = new();
        public ObservableCollection<CloseApproach> CloseApproach
        {
            get => _closeApproach;
            set => SetProperty(ref _closeApproach, value);
        }

        public ObservableCollection<ISeries> RiskInformation { get; set; } = new();

        private void SetRiskInfoSeries()
        {
            var series = new ObservableCollection<ISeries>
            {
                new PieSeries<int>
                {
                    Name = "Sentry objects",
                    Values = new int[]{ _allAsteroids.Count(x => x.IsSentryObject) }
                },
                new PieSeries<int>
                {
                    Name = "Potentially hazardous",
                    Values = new int[]{ _allAsteroids.Count(x => x.IsPotentiallyHazardousAsteroid) }
                },
                new PieSeries<int>
                {
                    Name = "Not dangerous at all",
                    Values = new int[]{ _allAsteroids.Count(x => !x.IsPotentiallyHazardousAsteroid && !x.IsSentryObject) }
                },

            };
            RiskInformation.Clear();
            RiskInformation.AddRange(series);
        }

        private IEnumerable<CloseApproachGroupByOrbitingBody> GroupByOrbitingBody()
        {
            return from c in CloseApproach
                   group c by c.OrbitingBody into ob
                   select new CloseApproachGroupByOrbitingBody
                   {
                       OrbitingBody = ob.Key,
                       Values = ob.AsEnumerable()
                   };
        }

        public ObservableCollection<ISeries> OrbitingBodySeries { get; set; } = new();

        private void SetOrbitingBodySeries()
        {
            var groupByOB = GroupByOrbitingBody();
            var series = new List<ISeries>();
            foreach (var c in groupByOB)
            {
                series.Add(new PieSeries<int>
                {
                    Values = new ObservableCollection<int> { c.Values.Count() },
                    Name = c.OrbitingBody,
                    InnerRadius = 70
                });
            }
            OrbitingBodySeries.Clear();
            OrbitingBodySeries.AddRange(series);
        }

        public ObservableCollection<ISeries> RelativeVelocitySeries { get; set; } = new();

        private void SetRelativeVelocitySeries()
        {
            var groupByOB = GroupByOrbitingBody();
            var series = new List<ISeries>();
            foreach (var c in groupByOB)
            {
                series.Add(new ColumnSeries<DateTimePoint>
                {
                    Values = new ObservableCollection<DateTimePoint>(from r in c.Values
                                                                     select new DateTimePoint(r.CloseApproachDate, r.RelativeVelocity)),
                    Name = $"{c.OrbitingBody}: {c.Values.Count()}"
                });
            }

            RelativeVelocitySeries.Clear();
            RelativeVelocitySeries.AddRange(series);
        }

        public ObservableCollection<ISeries> MissDistanceSeries { get; set; } = new();

        private void SetMissDistanceSeries()
        {
            var groupByOB = GroupByOrbitingBody();
            var series = new List<ISeries>();
            foreach (var c in groupByOB)
            {
                series.Add(new ColumnSeries<DateTimePoint>
                {
                    
                    Values = new ObservableCollection<DateTimePoint>(from m in c.Values
                                                                         select new DateTimePoint(m.CloseApproachDate, m.MissDistance)),
                    Name = $"{c.OrbitingBody}: {c.Values.Count()}"
                    
                });
            }

            MissDistanceSeries.Clear();
            MissDistanceSeries.AddRange(series);
        }

        public ObservableCollection<ICartesianAxis> XAxesDateTime { get; set; } = new()
        {
            new Axis
            {
                Labeler = value => new DateTime((long)value).ToString("dd/MM/yyyy"),
                LabelsRotation = 15,
                TextSize= 15,
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks, 
            }
        };

        public void OnNavigatedTo(NavigationContext navigationContext) { }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

    }

    class CloseApproachGroupByOrbitingBody
    {
        public string OrbitingBody { get; set; }

        public IEnumerable<CloseApproach> Values { get; set; }
    }
}
