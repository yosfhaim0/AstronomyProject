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
    /// Interaction logic for BulletedItem.xaml
    /// </summary>
    public partial class BulletedItem : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("BulletText", typeof(string), typeof(BulletedItem));

        public string BulletText
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public BulletedItem()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
