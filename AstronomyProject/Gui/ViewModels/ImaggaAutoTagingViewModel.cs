using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.ViewModels
{
    internal class ImaggaAutoTagingViewModel
    {
        public IEnumerable<ISeries> Series { get; set; } = new ObservableCollection<ISeries>
        {
            new StepLineSeries<double?>
            {
                Values = new ObservableCollection<double?> { 2, 1, 3, 4, 3, 4, 6 },
                Fill = null
            }
        };
    }
}
