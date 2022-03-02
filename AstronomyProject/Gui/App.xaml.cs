using DomainModel.DbFactory;
using DomainModel.Services;
using Models.Configurations;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Gui.Views;

namespace Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    { 
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance(GetConfigurations());
            
            containerRegistry.RegisterSingleton<IDbFactory, DbFactory>();
            
            #region Register services from the domain model
            containerRegistry.Register<IGalleryImageOfTheDayService, GalleryImageOfTheDayService>();
            containerRegistry.Register<INearAsteroidService, NearAsteroidService>();
            containerRegistry.RegisterSingleton<SaveAllToDB>();
            #endregion

            #region Register views for navigation
            containerRegistry.RegisterForNavigation<ImageOfTheDayView>(nameof(ImageOfTheDayView));
            containerRegistry.RegisterForNavigation<HomeView>(nameof(HomeView));
            containerRegistry.RegisterForNavigation<SearchMediaView>(nameof(SearchMediaView)); 
            containerRegistry.RegisterForNavigation<NearAsteroidListView>(nameof(NearAsteroidListView));
            #endregion
        }

        private MyConfigurations GetConfigurations()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"appsettings.json");
            var jsonString = File.ReadAllText(path);
            var configurations = JsonConvert.DeserializeObject<MyConfigurations>(jsonString);
            return configurations;
        }

        //protected override async void OnExit(ExitEventArgs e)
        //{
        //    await Container.Resolve<SaveAllToDB>().SaveAsync();
        //    Current.Shutdown();
        //    base.OnExit(e);
        //}

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }

}
