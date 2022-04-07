using DomainModel.Services;
using Gui.Dialogs;
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

        readonly IDialogService _dialogService;

        public SearchMediaViewModel(IMediaService mediaService, IDialogService dialogService)
        {
            _mediaService = mediaService;
            _dialogService = dialogService;
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
            }
        }

        public ObservableCollection<ISeries> TagsSeries { get; set; } = new();

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
                    try
                    {
                        var medias = await _mediaService.SearchMedia(SearchWord);
                        Medias.Clear();
                        Medias.AddRange(medias);
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowDialog("Erorr", ex.Message);
                    }
                    IsLoading = false;
                }
            },
            () =>
            {
                return !IsLoading;
            });


        private DelegateCommand _getTagsCommand;
        public DelegateCommand GetTagsCommand => _getTagsCommand ??= new DelegateCommand(
            async () =>
            {
                if (SelectedMedia.Tags.Any())
                {
                    SetImaggaGraph();
                    return;
                }

                IsLoading = true;
                try
                {
                    var tags = await _mediaService
            .GetMediaTags(SelectedMedia);
                    SelectedMedia.Tags = tags?.ToList();
                    SetImaggaGraph();
                }
                catch (Exception ex)
                {
                    _dialogService.ShowDialog("Erorr", ex.Message);
                }
                IsLoading = false;

            });

        private void SetImaggaGraph()
        {
            TagsSeries.Clear();
            TagsSeries.AddRange(new List<ISeries>
            {
                new ColumnSeries<double>
                {
                    Name = "Confidence",
                    Values = new ObservableCollection<double>(SelectedMedia
                    .Tags
                    .Select(x => x.Confidence)),
                }
            });

            XAxes = new List<Axis>
            {
                new()
                {
                    Labels = SelectedMedia.Tags.Select(t => t.Tag).ToList(),
                }
            };

            YAxes = new List<Axis>
            {
                new()
                {
                    Labeler =(value) => value.ToString(),
                }
            };
        }



    }
}
