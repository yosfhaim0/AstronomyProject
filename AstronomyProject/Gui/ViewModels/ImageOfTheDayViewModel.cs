using ApiRequests.Nasa;
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
        public ObservableCollection<ImageOfTheDay> Images { get; set; } = new();
        DelegateCommand _load;
        
        public DelegateCommand load => _load ??=
            new DelegateCommand(async () =>
            {
                NasaApi nasaApi = new NasaApi();
                var imgDto = await nasaApi.GetImageOfTheDay();
                var img = imgDto.CopyPropertiesToNew(typeof(ImageOfTheDay)) as ImageOfTheDay;
                Images.Clear();
                Images.Add(img);
            });
    }
}
