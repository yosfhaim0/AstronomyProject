using ApiRequests.Nasa;
using Models;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Gui
{
    class temp
    {
        //public string MyProperty { get; set; }
        //async Task fun()
        //{
        //    NasaApi nasaApi = new NasaApi();
        //    MyProperty = await nasaApi.GetImageOfTheDay();
        //}
    }
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        protected override async void RegisterTypes(IContainerRegistry containerRegistry)
        {

            NasaApi nasaApi = new NasaApi();
            var a = await nasaApi.GetImageOfTheDay();
            containerRegistry.RegisterInstance(a);//NASAImageOfTheDay

        }

        protected override Window CreateShell()
        {
            //var a = Container.Resolve<NASAImageOfTheDay>();
            var main = Container.Resolve<MainWindow>();
            return main;
        }
    }

}
