using System.Windows;
using System.Windows.Controls;
using Prism;
using Prism.Ioc;

namespace Gui.Views
{
    /// <summary>
    /// Interaction logic for SearchMedia.xaml
    /// </summary>
    public partial class SearchMediaView : UserControl
    {
        public SearchMediaView()
        {
            var app = Application.Current as PrismApplicationBase;
            var vm = app.Container.Resolve<ViewModels.SearchMediaViewModel>();
            DataContext = vm;
            InitializeComponent();
            vm.Play += () =>
            {
                media.Play();
            };
            vm.Pause += () =>
            {
                media.Pause();
            };
            vm.Stop += () =>
            {
                media.Stop();
            };
            vm.Mute += () =>
            {
                media.IsMuted = !media.IsMuted;
            };
        }
    }
}
