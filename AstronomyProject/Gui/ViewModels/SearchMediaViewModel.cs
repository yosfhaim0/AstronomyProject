using DomainModel.Services;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Models;
using Prism.Commands;
using Prism.Mvvm;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gui.ViewModels
{
    public class SearchMediaViewModel : ViewModelBase
    {

        readonly IMediaService _mediaService;
        public SearchMediaViewModel(IMediaService mediaService)
        {
            _mediaService = mediaService;

        }

        private void SetImaggaGraph()
        {
            if (_selectedMedia.Tags == null) return;
            Series = new ObservableCollection<ISeries>
            {
                new ColumnSeries<double>
                {
                    Name = _selectedMedia.Title,
                    Values = _selectedMedia.Tags.Select(x=>x.Confidence).ToList(),
                }
            };

            XAxes = new List<Axis>
            {
                new()
                {
                    Labels = _selectedMedia.Tags.Select(t=>t.Tag).ToList(),
                }
            };

            YAxes = new List<Axis>
            {
                new()
                {
                    Labeler =(value) =>value.ToString(),
                }
            };
        }

        private string _searchWord;
        public string SearchWord
        {
            get { return _searchWord; }
            set
            {
                IsSelected = false;
                SetProperty(ref _searchWord, value);
            }
        }

        bool _isSelected = false;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }


        private MediaGroupe _selectedMedia;
        public MediaGroupe SelectedMedia
        {
            get { return _selectedMedia ?? Medias?.FirstOrDefault(); }
            set
            {
                IsSelected = true;
                SetProperty(ref _selectedMedia, value);
                SetImaggaGraph();
            }
        }
        public IEnumerable<ISeries> Series { get; set; }

        public List<Axis> XAxes { get; set; }

        public List<Axis> YAxes { get; set; }


        public ObservableCollection<MediaGroupe> Medias { get; set; } = new();

        private DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ??= new DelegateCommand(
            async () =>
            {
                if (!string.IsNullOrEmpty(SearchWord))
                {
                    IsLoading = true;
                    IsSelected = false;
                    var medias = await _mediaService.SearchMedia(SearchWord);
                    Medias.Clear();
                    Medias.AddRange(medias);
                    IsLoading = false;
                }
            },
            () =>
            {
                return !IsLoading;
            });



    }
}
