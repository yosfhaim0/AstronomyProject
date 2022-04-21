using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.LiveCharts
{
    public class ChartBuilder
    {
        Chart _chart = new();

        public Chart Build()
        {
            if (_chart.Series != null)
            {
                return _chart;
            }
            throw new DataMisalignedException();
        }

        public ChartBuilder SetColumnSeries<T>(IEnumerable<T> values, string name, double maxBarWidth = 10)
        {
            _chart.Series = new ObservableCollection<ISeries>()
            {
                new ColumnSeries<T>
                {
                    Values = new ObservableCollection<T>(values),
                    Name = name,
                    MaxBarWidth = maxBarWidth,
                }
            };
            return this;
        }

        public ChartBuilder SetColumnSeries<T>(IEnumerable<IEnumerable<T>> values, IEnumerable<string> names, double maxBarWidth = 10)
        {
            _chart.Series = new ObservableCollection<ISeries>();
            foreach (var item in values.Zip(names))
            {
                _chart.Series.Add(new ColumnSeries<T>
                {
                    Values = new ObservableCollection<T>(item.First),
                    Name = item.Second,
                    MaxBarWidth = maxBarWidth
                });
            }
            return this;
        }

        public ChartBuilder SetRowSeries<T>(IEnumerable<T> values, string name, double maxBarWidth = 10)
        {
            _chart.Series = new ObservableCollection<ISeries>()
            {
                new RowSeries<T>
                {
                    Values = new ObservableCollection<T>(values),
                    Name = name,
                    MaxBarWidth = maxBarWidth,
                }
            };
            return this;
        }

        public ChartBuilder SetRowSeries<T>(IEnumerable<IEnumerable<T>> values, IEnumerable<string> names, double maxBarWidth = 10)
        {
            _chart.Series = new ObservableCollection<ISeries>();
            foreach (var item in values.Zip(names))
            {
                _chart.Series.Add(new RowSeries<T>
                {
                    Values = new ObservableCollection<T>(item.First),
                    Name = item.Second,
                    MaxBarWidth = maxBarWidth
                });
            }
            return this;
        }

        public ChartBuilder SetPieSeries<T>(IEnumerable<IEnumerable<T>> values, IEnumerable<string> names)
        {
            _chart.Series = new ObservableCollection<ISeries>();
            foreach (var item in values.Zip(names))
            {
                _chart.Series.Add(new PieSeries<T>
                {
                    Values = new ObservableCollection<T>(item.First),
                    Name = item.Second,
                });
            }
            return this;
        }

        public ChartBuilder SetPieSeries<T>(IEnumerable<T> values, string name)
        {
            _chart.Series = new ObservableCollection<ISeries>()
            {
                new PieSeries<T>
                {
                    Values = new ObservableCollection<T>(values),
                    Name = name,
                }
            };
            return this;
        }


        public ChartBuilder SetXAxes(Func<double, string> labeler = null, List<string> labels = null, double textSize = 12, double nameTextSize = 12)
        {
            if (labeler == null && labels == null)
            {
                throw new ArgumentNullException($"{nameof(labeler)}, {nameof(labels)}");
            }
            _chart.XAxes = new ObservableCollection<Axis>(GetAxis(labeler, labels, textSize, nameTextSize)); ;
            return this;
        }

        public ChartBuilder SetYAxes(Func<double, string> labeler = null, List<string> labels = null, double textSize = 12, double nameTextSize = 12)
        {
            if (labeler == null && labels == null)
            {
                throw new ArgumentNullException($"{nameof(labeler)}, {nameof(labels)}");
            }
            _chart.YAxes = new ObservableCollection<Axis>(GetAxis(labeler, labels, textSize, nameTextSize));
            return this;
        }

        private static List<Axis> GetAxis(Func<double, string> labeler, List<string> labels, double textSize, double nameTextSize)
        {
            List<Axis> axes = new();
            if (labeler == null)
            {
                axes.Add(new Axis { Labels = labels, TextSize = textSize, NameTextSize = nameTextSize });
            }
            else if (labels == null)
            {
                axes.Add(new Axis { Labeler = labeler, TextSize = textSize, NameTextSize = nameTextSize });
            }

            return axes;
        }

        public ChartBuilder SetXAxesDateTime()
        {
            _chart.XAxes = new ObservableCollection<Axis>
            {
                new Axis
                {
                    Labeler = value => new DateTime((long)value).ToString("dd/MM/yyyy"),
                    LabelsRotation = 15,
                    TextSize= 15,
                    UnitWidth = TimeSpan.FromDays(1).Ticks,
                    MinStep = TimeSpan.FromDays(1).Ticks,
                }
            };
            return this;
        }
    }
}
