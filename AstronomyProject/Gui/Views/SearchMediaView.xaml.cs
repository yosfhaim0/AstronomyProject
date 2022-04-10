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
            InitializeComponent();
            var app = Application.Current as PrismApplicationBase;
            var vm = app.Container.Resolve<ViewModels.SearchMediaViewModel>();
            DataContext = vm;
            vm.PlayRequested += (s, e) =>
            {
                media.Play();
            };
            vm.PauseRequested += (s, e) =>
            {
                media.Pause();
            };
            vm.StopRequested += (s, e) =>
            {
                media.Stop();
            };
        }
    }
}
