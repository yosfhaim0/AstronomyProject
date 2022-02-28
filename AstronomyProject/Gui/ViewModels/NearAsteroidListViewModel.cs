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
    public class NearAsteroidListViewModel : BindableBase
    {
        readonly INearAsteroidService _nearAsteroidService;

        public NearAsteroidListViewModel(INearAsteroidService nearAsteroidService)
        {
            _nearAsteroidService = nearAsteroidService;
        }

        public ObservableCollection<ISeries> Series { get; set; } = new();

        public ObservableCollection<NearAsteroid> NearAsteroids { get; set; } = new();

        private int _totalAsteroids;
        public int TotalAsteroids
        {
            get { return _totalAsteroids; }
            set { SetProperty(ref _totalAsteroids, value); }
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
                var potentiallyHazardous = await _nearAsteroidService.GetPotentiallyHazardous();
                NearAsteroids.Clear();
                NearAsteroids.AddRange(potentiallyHazardous);
                TotalAsteroids = NearAsteroids.Count;
                LoadPieSeries();
                IsLoading = false;
            });

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
            Series.Clear();
            Series.AddRange(sec);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
    }
}
