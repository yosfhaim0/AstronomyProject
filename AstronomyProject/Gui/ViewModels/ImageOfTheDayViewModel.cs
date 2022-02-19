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

        public ObservableCollection<ImageOfTheDay> Gallery { get; set; } = new();

        private ImageOfTheDay _selectedImage;
        public ImageOfTheDay SelectedImage
        {
            get { return _selectedImage; }
            set 
            { 
                SetProperty(ref _selectedImage, value); 
            }
        }


        DelegateCommand _load;
        public DelegateCommand Load => _load ??=
            new DelegateCommand(async () =>
            {
                SelectedImage = await _gallery.GetTodayImage();
                var gallery = await _gallery.GetGallery();
                Gallery.Clear();
                Gallery.AddRange(gallery);
            });
    }
}
