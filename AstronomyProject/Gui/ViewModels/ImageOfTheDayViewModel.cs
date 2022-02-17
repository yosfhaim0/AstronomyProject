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

        public ObservableCollection<ImageOfTheDay> Images { get; set; } = new();
        
        DelegateCommand _load;
        public DelegateCommand Load => _load ??=
            new DelegateCommand(async () =>
            {
                var img = await _gallery.GetTodayImage();
                Images.Clear();
                Images.Add(img);
            });
    }
}
