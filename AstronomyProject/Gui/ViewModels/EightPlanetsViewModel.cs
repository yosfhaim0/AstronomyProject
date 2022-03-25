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
    public class ColumValue
    {
        public object Value { get; set; }
        public string Name { get; set; }

    }
    public class EightPlanetsViewModel : ViewModelBase
    {
        readonly EightPlanets _eightPlanetsInfo;
        public ObservableCollection<Planet> PlanetList { get; set; } = new();



        private Planet _selectedPlanet;
        public Planet SelectedPlanet
        {
            get { return _selectedPlanet ?? PlanetList?.FirstOrDefault(); }
            set
            {
                SetProperty(ref _selectedPlanet, value);
            }
        }
        public List<string> PropNames { get; set; } = new();

        public ObservableCollection<ColumValue> PropList { get; set; } = new();

        private string _selectedProp;

        public string SelectedProp
        {
            get { return _selectedProp; }
            set
            {
                SetProperty(ref _selectedProp, value);
                setColum(SelectedProp);
            }
        }
        public EightPlanetsViewModel(EightPlanets eightPlanetsInfo)
        {
            _eightPlanetsInfo = eightPlanetsInfo;
            PlanetList.AddRange(_eightPlanetsInfo.GetEightPlanetsInfo());
            PropNames= typeof(Planet).GetProperties().Select(x=>x.Name).ToList();
            
            // The UnitWidth is only required for column or financial series
            // because the library needs to know the width of each column, by default the
            // width is 1, but when you are using a different scale, you must let the library know it.
            XAxes = new Axis[]
            {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("MMMM dd"),
                LabelsRotation = 15,

                // in this case we want our columns with a width of 1 day, we can get that number
                // using the following syntax
                UnitWidth = TimeSpan.FromDays(1).Ticks, // mark

                // The MinStep property forces the separator to be greater than 1 day.
                MinStep = TimeSpan.FromDays(1).Ticks // mark

                // if the difference between our points is in hours then we would:
                // UnitWidth = TimeSpan.FromHours(1).Ticks,

                // since all the months and years have a different number of days
                // we can use the average, it would not cause any visible error in the user interface
                // Months: TimeSpan.FromDays(30.4375).Ticks
                // Years: TimeSpan.FromDays(365.25).Ticks
            }
            };

        }

        private void setColum(string propertyName)
        {
            List<ColumValue> res = new();
            foreach (var item in PlanetList)
            {
                var pro = item.GetType().GetProperty(propertyName);
                res.Add(new ColumValue { Name = pro.Name, Value = pro.GetValue(item, null) });
            }
            PropList.Clear();
            PropList.AddRange(res);
        }

        public IEnumerable<ISeries> Series { get; set; }
        public IEnumerable<ICartesianAxis> XAxes { get; set; }

    }
}

