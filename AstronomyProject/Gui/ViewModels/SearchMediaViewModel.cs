using DomainModel.Services;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
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

        private string _searchWord;
        public string SearchWord
        {
            get { return _searchWord; }
            set { SetProperty(ref _searchWord, value); }
        }

        private string _selectedImage;
        public string SelectedImage
        {
            get { return _selectedImage ?? Content?.FirstOrDefault(); }
            set
            {
                SetProperty(ref _selectedImage, value);
            }
        }

        private List<string> _content;
        public List<string> Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand => _searchCommand ??= new DelegateCommand(
            async () =>
            {
                if (!string.IsNullOrEmpty(SearchWord))
                {
                    IsLoading = true;
                    Content = await _mediaService.SearchMedia(SearchWord);
                    IsLoading = false;
                }
            },
            () =>
            {
                return !IsLoading;
            });



    }
}
