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
                var img = await nasaApi.GetImageOfTheDay();
                Images.Clear();
                Images.Add(img);
            });
    }
}
