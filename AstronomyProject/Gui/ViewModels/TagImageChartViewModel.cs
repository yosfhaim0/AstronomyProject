using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gui.ViewModels
{
    public class TagImageChartViewModel : ViewModelBase
    {
        public ObservableCollection<ISeries> TagsSeries { get; set; } = new();

        public ObservableCollection<Axis> XAxes { get; set; } = new();

        public ObservableCollection<Axis> YAxes { get; set; } = new();

        public TagImageChartViewModel()
        {

        }

        public void NewMediaIn(Tuple<List<ISeries>, List<Axis>, List<Axis>> tuple)
        {
            TagsSeries.Clear();
            TagsSeries.AddRange(tuple.Item1);

            YAxes.Clear();
            YAxes.AddRange(tuple.Item2);

            XAxes.Clear();
            XAxes.AddRange(tuple.Item3);
        }       
    }
}