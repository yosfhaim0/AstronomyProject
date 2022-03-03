using ApiRequests.Nasa;
using DataAccess.UnitOfWork;
using DomainModel.Services;
using Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace Gui.ViewModels
{
    public class ImageOfTheDayViewModel : BindableBase
    {
        readonly IGalleryImageOfTheDayService _gallery;

        public ImageOfTheDayViewModel(IGalleryImageOfTheDayService gallery)
        {
            _gallery = gallery;
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public ObservableCollection<ImageOfTheDay> Gallery { get; set; } = new();

        private ImageOfTheDay _selectedImage;
        public ImageOfTheDay SelectedImage
        {
            get { return _selectedImage ?? Gallery?.FirstOrDefault(); }
            set
            {
                SetProperty(ref _selectedImage, value);
            }
        }

        private DelegateCommand _moveNext;

        public DelegateCommand MoveNext => _moveNext ??= new DelegateCommand(
            () =>
            {
                var index = Gallery.IndexOf(SelectedImage);
                index = index + 1 < Gallery.Count ? index + 1 : 0;
                SelectedImage = Gallery[index];
            });

        private DelegateCommand _movePrev;
        public DelegateCommand MovePrev => _movePrev ??= new DelegateCommand(
            () =>
            {
                var index = Gallery.IndexOf(SelectedImage);
                index = index - 1 > 0 ? index - 1 : Gallery.Count - 1;
                SelectedImage = Gallery[index];
            });

        private DelegateCommand _goToTodayImage;
        public DelegateCommand GoToTodayImage => _goToTodayImage ??= new DelegateCommand(
            () =>
            {
                SelectedImage = Gallery.FirstOrDefault(i => i.Date.Date == DateTime.Today.Date);
            });

        DelegateCommand _load;
        public DelegateCommand Load => _load ??= new DelegateCommand(
            async () =>
            {
                if (Gallery.Any())
                {
                    return;
                }
                IsLoading = true;
                SelectedImage = await _gallery.GetTodayImage();
                var gallery = await _gallery.GetGallery();
                Gallery.Clear();
                Gallery.AddRange(gallery);
                IsLoading = false;
            });


    }
}
