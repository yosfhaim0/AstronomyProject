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
            containerRegistry.Register<IGalleryImageOfTheDayService, GalleryImageOfTheDayService>();
            containerRegistry.RegisterForNavigation<ImageOfTheDayView>(nameof(ImageOfTheDayView));
        }

        private MyConfigurations GetConfigurations()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"appsettings.json");
            var jsonString = File.ReadAllText(path);
            var configurations = JsonConvert.DeserializeObject<MyConfigurations>(jsonString);
            return configurations;



        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
    }

}
