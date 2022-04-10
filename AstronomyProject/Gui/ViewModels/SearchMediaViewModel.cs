using DomainModel.Services;
using Gui.Dialogs;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Models;
using Prism.Commands;
using Prism.Events;
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
        readonly ChartDialog _chartDialog;

        public SearchMediaViewModel(IMediaService mediaService, IDialogService dialogService, ChartDialog chartDialog)
        {
            _mediaService = mediaService;
            _dialogService = dialogService;
            _chartDialog = chartDialog;
        }


        private string _searchWord;
        public string SearchWord
        {
            get { return _searchWord; }
            set
            {
                if(SetProperty(ref _searchWord, value))
                {
                    IsSelected = false;
                };
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


        public ObservableCollection<MediaGroupe> Medias { get; set; } = new();

        private DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ??= new DelegateCommand(
            async () =>
            {
                if (!string.IsNullOrEmpty(SearchWord))
                {
                    IsLoading = true;

                    try
                    {
                        var medias = await _mediaService.SearchMedia(SearchWord);
                        Medias.Clear();
                        Medias.AddRange(medias);
                        IsSelected = false;
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


        bool _isIoadingTagChart = false;
        public bool IsIoadingTagChart
        {
            get => _isIoadingTagChart;
            set => SetProperty(ref _isIoadingTagChart, value);
        }

        private DelegateCommand _getTagsCommand;
        public DelegateCommand GetTagsCommand => _getTagsCommand ??= new(
            async () =>
            {
                IsIoadingTagChart = true;
                try
                {
                    await _chartDialog.ShowChartByMedia(SelectedMedia);
                }
                catch (Exception ex)
                {
                    _dialogService.ShowDialog("Erorr", ex.Message);
                }
                IsIoadingTagChart = false;
            },
            () => !IsIoadingTagChart);

        

        private DelegateCommand _seeMoreCommand;
        public DelegateCommand SeeMoreCommand => _seeMoreCommand ??= new(
            async () =>
            {
                if (!string.IsNullOrEmpty(SearchWord))
                {
                    IsLoading = true;

                    try
                    {
                        var medias = await _mediaService.SearchMedia(SearchWord, Medias.Count);
                        Medias.AddRange(medias);
                        IsSelected = false;
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

        private bool _isMuted = false;
        public bool IsMuted
        {
            get { return _isMuted; }
            set { SetProperty(ref _isMuted, value); }
        }


        public event EventHandler PlayRequested;

        public event EventHandler PauseRequested;

        public event EventHandler StopRequested;

        private DelegateCommand _play;
        public DelegateCommand PlayCommand => _play ??= new(
            () =>
            {
                PlayRequested?.Invoke(this, EventArgs.Empty);
            });

        private DelegateCommand _pause;
        public DelegateCommand PauseCommand => _pause ??= new(
            () =>
            {
                PauseRequested?.Invoke(this, EventArgs.Empty);
            });

        private DelegateCommand _stop;
        public DelegateCommand StopCommand => _stop ??= new(
            () =>
            {
                StopRequested?.Invoke(this, EventArgs.Empty);
            });

        private DelegateCommand _mute;
        public DelegateCommand MuteCommand => _mute ??= new(
            () =>
            {
                IsMuted = !IsMuted;
            });


    }
}
