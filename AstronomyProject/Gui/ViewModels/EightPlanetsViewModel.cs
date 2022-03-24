using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using DomainModel.Services;
using System.Windows.Controls;
using LiveCharts.Configurations;
using LiveCharts;
using Models;
using LiveCharts.Helpers;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Measure;
using System.Reflection;
using LiveChartsCore.Kernel.Sketches;

namespace Gui.ViewModels
{
    public class EightPlanetsViewModel : ViewModelBase
    {
        readonly EightPlanets _eightPlanetsInfo;
        List<Planet> PlanetList;
        public Planet SelectedPlanet { get; set; }
        public EightPlanetsViewModel(EightPlanets eightPlanetsInfo)
        {
            _eightPlanetsInfo = eightPlanetsInfo;
            PlanetList = _eightPlanetsInfo.GetEightPlanetsInfo();
            Series = new();
            foreach (var p in PlanetList)
            {

                Series.Add(new ColumnSeries<double>
                {
                    Values = new ObservableCollection<double> { p.distanceFromSun },
                    Name = p.name,


                    //DataLabelsPaint = new SolidColorPaint(randSKColor)
                });
            }

            XAxes = new()
            {
                new Axis
                {
                    Labeler = value => PlanetList.FirstOrDefault(x => x.distanceFromSun == value)?.name,
                    TextSize = 15,

                }
            };

        }
        public ObservableCollection<ICartesianAxis> XAxes { get; set; }


        //private IEnumerable<double> f(Planet c)
        //{
        //    PropertyInfo[] properties = typeof(c).GetProperties();
        //    foreach (PropertyInfo property in properties)
        //    {
        //        if property.g
        //    }


        //}

        Random rnd = new Random();
        public List<ISeries> Series { get; set; }
        //{
        //    new RowSeries<int>
        //    {
        //        Values = new List<int> { 8, -3, 4, -3, 3, 4, -2 },
        //        Stroke = null,
        //        DataLabelsPaint = new SolidColorPaint(new SKColor(45, 45, 45)),
        //        DataLabelsSize = 14,
        //        DataLabelsPosition = DataLabelsPosition.End
        //    },
        //    new RowSeries<int>
        //    {
        //        Values = new List<int> { 4, -6, 5, -9, 4, 8, -6 },
        //        Stroke = null,
        //        DataLabelsPaint = new SolidColorPaint(new SKColor(250, 250, 250)),
        //        DataLabelsSize = 14,
        //        DataLabelsPosition = DataLabelsPosition.Middle
        //    },
        //    new RowSeries<int>
        //    {
        //        Values = new List<int> { 6, -9, 3, -6, 8, 2, -9 },
        //        Stroke = null,
        //        DataLabelsPaint = new SolidColorPaint(new SKColor(45, 45, 45)),
        //        DataLabelsSize = 14,
        //        DataLabelsPosition = DataLabelsPosition.Start
        //    }
        //};

        public SKColor randSKColor()
        {
            return new SKColor((byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256));
        }
    }
}

