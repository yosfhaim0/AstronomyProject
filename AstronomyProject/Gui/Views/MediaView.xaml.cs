using Gui.ViewModels;
using Models;
using Prism;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gui.Views
{
    /// <summary>
    /// Interaction logic for MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl
    {
        public MediaGroupe SelectedMedia
        {
            get { return (MediaGroupe)GetValue(SelectedMediaProperty); }
            set 
            { 
                SetValue(SelectedMediaProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for SelectedMedia.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedMediaProperty =
            DependencyProperty.Register("SelectedMedia", typeof(MediaGroupe), typeof(MediaView) );


        MedialViewModel viewModel;


        public MediaView()
        {
            InitializeComponent();
            var app = Application.Current as PrismApplicationBase;
            viewModel = app.Container.Resolve<MedialViewModel>();
            //vm.SelectedMedia = SelectedMedia;
            viewModel.PlayRequested += (s, e) =>
            {
                media.Play();
            };
            viewModel.PauseRequested += (s, e) =>
            {
                media.Pause();
            };
            viewModel.StopRequested += (s, e) =>
            {
                media.Stop();
            };
        }
    }
}
