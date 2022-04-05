using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.ViewModels
{
    public class ImaggaTaggingGraphViewModel : ViewModelBase
    {
        public ImaggaTaggingGraphViewModel()
        {

            Series = new ObservableCollection<ISeries>
            {
                new ColumnSeries<double>
                {
                    Name = MediaGroupe.Title,
                    Values = MediaGroupe.Tags.Select(x=>x.Confidence).ToList(),
                }
            };

            XAxes = new List<Axis>
            {
                new()
                {
                   
                    Labels = MediaGroupe.Tags.Select(t=>t.Tag).ToList(),
                }
            };

            YAxes = new List<Axis>
            {
                new()
                {
                    // Now the Y axis we will display labels as currency
                    // LiveCharts provides some common formatters
                    // in this case we are using the currency formatter.
                    Labeler =(value) =>value.ToString(),

                    // you could also build your own currency formatter
                    // for example:
                    // Labeler = (value) => value.ToString("C")

                    // But the one that LiveCharts provides creates shorter labels when
                    // the amount is in millions or trillions
                }
            };
        }
        private MediaGroupe _mediaGroupe;
        public MediaGroupe MediaGroupe
        {
            get { return _mediaGroupe; }
            set
            {
                SetProperty(ref _mediaGroupe, value);
            }
        }

        
    }
}
