using Gui.LiveCharts;

namespace Gui.ViewModels
{
    public class TagImageChartViewModel : ViewModelBase
    {
        private Chart _tagsChart;

        public Chart TagsChart
        {
            get { return _tagsChart; }
            set { SetProperty(ref _tagsChart, value); }
        } 
    }
}