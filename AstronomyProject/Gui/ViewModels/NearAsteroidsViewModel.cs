using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DomainModel.Services;
using Prism.Commands;
using LiveChartsCore.Defaults;
using Gui.Dialogs;
using Gui.LiveCharts;

namespace Gui.ViewModels
{
    public class NearAsteroidsViewModel : ViewModelBase
    {
        readonly INearAsteroidService _nearAsteroidService;
        readonly IDialogService _dialogService;

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

        readonly List<NearAsteroid> _allAsteroids = new();

        public ObservableCollection<NearAsteroid> Asteroids { get; set; } = new();

        private NearAsteroid _selectedAstroeid = new();
        public NearAsteroid SelectedAstroeid
        {
            get => _selectedAstroeid;
            set
            {
                if (value == null) return;
                SetProperty(ref _selectedAstroeid, value);
                CloseApproach = new(_selectedAstroeid?.CloseApproachs);
                FilterDatesCommand.Execute();
            }
        }

        private ObservableCollection<CloseApproach> _closeApproach = new();
        public ObservableCollection<CloseApproach> CloseApproach
        {
            get => _closeApproach;
            set => SetProperty(ref _closeApproach, value);
        }

        DelegateCommand _filterByDiameter;
        public DelegateCommand FilterByDiameter => _filterByDiameter ??= new DelegateCommand(
             () =>
            {
                Asteroids.Clear();
                Asteroids.AddRange(_allAsteroids.Where(a => a.EstimatedDiameterMin >= Diameter));

            });

        private double _diameter;
        public double Diameter
        {
            get => _diameter;
            set
            {
                SetProperty(ref _diameter, value);
            }
        }

        private string _selectedFilterOfCloseApproach = "All";
        public string SelectedFilterOfCloseApproach
        {
            get { return _selectedFilterOfCloseApproach; }
            set { SetProperty(ref _selectedFilterOfCloseApproach, value); }
        }

        DelegateCommand _load;
        public DelegateCommand Load => _load ??= new DelegateCommand(
            async () =>
            {
                if (IsActive)
                {
                    return;
                }
                try
                {
                    await Search();
                    IsActive = true;
                }
                catch (Exception ex)
                {
                    _dialogService.ShowDialog("Erorr", ex.Message);
                }
            });

        DelegateCommand _searchBetweenDates;
        public DelegateCommand SearchBetweenDates => _searchBetweenDates ??= new DelegateCommand(
            async () =>
            {
                try
                {
                    await Search();
                }
                catch (Exception ex)
                {
                    _dialogService.ShowDialog("Erorr", ex.Message);
                }
            });

        private void OnLoading(IEnumerable<NearAsteroid> asteroids)
        {
            _allAsteroids.Clear();
            _allAsteroids.AddRange(asteroids);

            TotalCount = _allAsteroids.Count;

            Asteroids.Clear();
            Asteroids.AddRange(asteroids);

            SelectedAstroeid = Asteroids.First();

            SetRiskInfoChart();
        }

        private DelegateCommand _filterDatesCommand;
        public DelegateCommand FilterDatesCommand => _filterDatesCommand ??= new (
         () =>
         {
             CloseApproach.Clear();
             switch (SelectedFilterOfCloseApproach)
             {
                 case "All":
                     CloseApproach.AddRange(SelectedAstroeid.CloseApproachs);
                     break;
                 case "Between the search dates":
                     CloseApproach.AddRange(SelectedAstroeid
                         .CloseApproachs
                         .Where(c => c.CloseApproachDate >= FromDate
                         && c.CloseApproachDate <= ToDate));
                     break;
                 case "Last week":
                     CloseApproach.AddRange(SelectedAstroeid
                         .CloseApproachs
                         .Where(c => c.CloseApproachDate >= DateTime.Now.AddDays(-7)
                         && c.CloseApproachDate <= DateTime.Now));
                     break;
                 case "Last month":
                     CloseApproach.AddRange(SelectedAstroeid
                         .CloseApproachs
                         .Where(c => c.CloseApproachDate >= DateTime.Now.AddDays(-30)
                         && c.CloseApproachDate <= DateTime.Now));
                     break;
                 case "Last year":
                     CloseApproach.AddRange(SelectedAstroeid
                         .CloseApproachs
                         .Where(c => c.CloseApproachDate >= DateTime.Now.AddDays(-365)
                         && c.CloseApproachDate <= DateTime.Now));
                     break;
             }
             SetRelativeVelocityChart();
             SetMissDistanceChart();
         });

        private Chart _riskInformationChart;
        public Chart RiskInformationChart
        {
            get { return _riskInformationChart; }
            set { SetProperty(ref _riskInformationChart, value); }
        }

        private void SetRiskInfoChart()
        {
            RiskInformationChart = new ChartBuilder()
                .SetPieSeries(new List<int[]>
                {
                    new int[]{ _allAsteroids.Count(x => x.IsSentryObject) },
                    new int[]{ _allAsteroids.Count(x => x.IsPotentiallyHazardousAsteroid) },
                    new int[]{ _allAsteroids.Count(x => !x.IsPotentiallyHazardousAsteroid && !x.IsSentryObject) }

                }, new string[] 
                { 
                    "Sentry objects",
                    "Potentially hazardous",
                    "Not dangerous at all"
                }).Build();
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

        Chart _relativeVelocityChart;
        public Chart RelativeVelocityChart 
        { 
            get => _relativeVelocityChart; 
            set => SetProperty(ref _relativeVelocityChart, value); 
        }

        private void SetRelativeVelocityChart()
        {
            var groupByOB = GroupByOrbitingBody();
            RelativeVelocityChart = new ChartBuilder()
                .SetXAxesDateTime()
                .SetColumnSeries(groupByOB
                .Select(c => c.Values
                .Select(v => new DateTimePoint(v.CloseApproachDate, v.RelativeVelocity))),
                groupByOB.Select(c => $"{c.OrbitingBody}: {c.Values.Count()}"))
                .Build();
        }

        Chart _missDistanceChart;
        public Chart MissDistanceChart
        {
            get => _missDistanceChart;
            set => SetProperty(ref _missDistanceChart, value);
        }

        private void SetMissDistanceChart()
        {
            var groupByOB = GroupByOrbitingBody();
            MissDistanceChart = new ChartBuilder()
                        .SetXAxesDateTime()
                        .SetColumnSeries(groupByOB
                        .Select(c => c.Values
                        .Select(v => new DateTimePoint(v.CloseApproachDate, v.MissDistance))),
                        groupByOB.Select(c => $"{c.OrbitingBody}: {c.Values.Count()}"))
                        .Build();

        }
    }

    class CloseApproachGroupByOrbitingBody
    {
        public string OrbitingBody { get; set; }

        public IEnumerable<CloseApproach> Values { get; set; }
    }
}
