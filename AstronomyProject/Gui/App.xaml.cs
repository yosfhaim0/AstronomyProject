﻿using DomainModel.DbFactory;
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
using DataAccess.DbContexts;
using DataAccess.UnitOfWork;

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
            
            containerRegistry.RegisterSingleton<IDbFactory ,DbFactory>();

            #region Register services from the domain model
            containerRegistry.RegisterSingleton<IGalleryImageOfTheDayService, GalleryImageOfTheDayService>();
            containerRegistry.RegisterSingleton<INearAsteroidService, NearAsteroidService>();
            containerRegistry.RegisterSingleton<IMediaService, MediaService>();
            #endregion

            #region Register views for navigation
            containerRegistry.RegisterForNavigation<ImageOfTheDayView>();
            containerRegistry.RegisterForNavigation<HomeView>();
            containerRegistry.RegisterForNavigation<SearchMediaView>();
            containerRegistry.RegisterForNavigation<NearAsteroidsView>();
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
