using DomainModel.DataAccessFactory;
using DomainModel.Services;
using Models.Configurations;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Unity;
using System;
using System.IO;
using System.Windows;
using Gui.Views;
using Gui.Dialogs;
using System.Threading;
using System.Reflection;

namespace Gui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            LoadHeavyAssembly(typeof(CefSharp.Wpf.ChromiumWebBrowser));
            LoadHeavyAssembly(typeof(LiveChartsCore.ISeries));
            LoadHeavyAssembly(typeof(LiveChartsCore.SkiaSharpView.Axis));

            containerRegistry.RegisterInstance(GetConfigurations());

            containerRegistry.RegisterScoped<IDataAccessFactory, DataAccessFactory>();

            #region Register services from the domain model
            containerRegistry.RegisterScoped<IImageOfTheDayService, ImageOfTheDayService>();
            containerRegistry.RegisterScoped<INearAsteroidService, NearAsteroidService>();
            containerRegistry.RegisterScoped<IMediaService, MediaService>();
            #endregion

            containerRegistry.Register<IDialogService, DialogService>();
            containerRegistry.Register<ChartDialog>();

            #region Register views for navigation
            containerRegistry.RegisterForNavigation<ImageOfTheDayView>();
            containerRegistry.RegisterForNavigation<HomeView>();
            containerRegistry.RegisterForNavigation<SearchMediaView>();
            containerRegistry.RegisterForNavigation<NearAsteroidsView>();
            containerRegistry.RegisterForNavigation<EightPlanetsView>();

            #endregion
        }

        private void LoadHeavyAssembly(Type type)
        {
            string assemblyString = type.Assembly.FullName.ToString();
            new Thread(
                () =>
                {
                    try
                    {
                        Assembly.Load(assemblyString);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                })
            {
                Priority = ThreadPriority.Lowest
            }.Start();
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
