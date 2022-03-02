using DomainModel.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gui.ViewModels
{
    public class SearchMediaViewModel : BindableBase
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

        private string _content;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
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
