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
        public ObservableCollection<NASAImageOfTheDay> Images { get; set; } = new();
        DelegateCommand _load;
        public DelegateCommand load => _load ??=
            new DelegateCommand(async () =>
            {
                NasaApi nasaApi = new NasaApi();
                var img = await nasaApi.GetImageOfTheDay();
                Images.Clear();
                Images.Add(img);
            });

        //public ImageOfTheDayViewModel(NASAImageOfTheDay nASAImageOfTheDay)
        //{

        //    _imageOfTheDay = nASAImageOfTheDay;
        //}

        //private string _imageUrl;

        //public string ImageUrl
        //{
        //    get { return ImageOfTheDay.ImageUrl; }
        //    //set { SetProperty(ref _imageUrl, value); }
        //}

        //private string _explanation;

        //public string Explanation
        //{
        //    get { return ImageOfTheDay.Explanation; }
        //    //set { SetProperty(ref _explanation, value); }
        //}

        //private DateTime _date;

        //public DateTime Date
        //{
        //    get { return ImageOfTheDay.Date; }
        //    //set { SetProperty(ref _date, value); }
        //}
        //private string _title;

        //public string Title
        //{
        //    get { return ImageOfTheDay.Title; }
        //    //set { SetProperty(ref _title, value); }
        //




    }
}
