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
    /// Interaction logic for FieldItem.xaml
    /// </summary>
    public partial class FieldItem : UserControl
    {


        public string FieldName
        {
            get { return (string)GetValue(FieldNameProperty); }
            set { SetValue(FieldNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FieldName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FieldNameProperty =
            DependencyProperty.Register("FieldName", typeof(string), typeof(FieldItem));



        public string FieldValue
        {
            get { return (string)GetValue(FieldValueProperty); }
            set { SetValue(FieldValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FieldValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FieldValueProperty =
            DependencyProperty.Register("FieldValue", typeof(string), typeof(FieldItem));


        public FieldItem()
        {
            InitializeComponent();
            //DataContext = this;
        }
    }
}
