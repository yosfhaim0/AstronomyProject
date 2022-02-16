using ApiRequests.Nasa;
using DataAccess.DbContexts;
using DataAccess.UnitOfWork;
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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var con = @"Data Source=(localdb)\MSSQLLocalDB;Database=AstronomyDB;Trusted_Connection=True;";
            containerRegistry.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            containerRegistry.RegisterInstance(new DbContextFactory(con));
        }

        protected override Window CreateShell()
        {
            //var a = Container.Resolve<NASAImageOfTheDay>();
            var main = Container.Resolve<MainWindow>();
            return main;
        }
    }

}
