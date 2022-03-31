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
using System.Collections;

namespace Gui.ViewModels
{
    public class ColumValue
    {
        public Object Value { get; set; }
        public string Name { get; set; }
        public string Plant { get; set; }

    }
    public class EightPlanetsViewModel : ViewModelBase
    {
        readonly EightPlanets _eightPlanetsInfo;
        public ObservableCollection<Planet> PlanetList { get; set; } = new();

        public List<string> ExplanImageList { get; set; } = new();
        private string _explanImage;

        public string ExplanImage
        {
            get { return _explanImage; }
            set
            {
                SetProperty(ref _explanImage, value);
            }            
        }

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
                setColum();
                setExplanImage();
            }
        }

        private void setExplanImage()
        {
            ExplanImage = _eightPlanetsInfo.getExplanImageList(SelectedProp);
        }

        public EightPlanetsViewModel(EightPlanets eightPlanetsInfo)
        {
            _eightPlanetsInfo = eightPlanetsInfo;
            PlanetList.AddRange(_eightPlanetsInfo.GetEightPlanetsInfo());
            PropNames = typeof(Planet).GetProperties().Select(x => x.Name).ToList();

            PropNames.Remove("Name");
            PropNames.Remove("Url");
            PropNames.Remove("HasRingSystem");
            PropNames.Remove("HasGlobalMagneticField");
            PropNames.Remove("Id");
            //ExplanImageList.AddRange(_eightPlanetsInfo.getExplanImageList(PropNames));
            SelectedProp = PropNames.FirstOrDefault();

            XAxes = new List<Axis>
            {
                new()
                {
                    // Use the labels property to define named labels.
                    Labels = PlanetList.Select(X=>X.Name).ToArray(),
                    TextSize=22
                    

                }
            };



        }

        private void setColum()
        {


            List<ColumValue> res = new();
            foreach (var item in PlanetList)
            {
                var pro = item.GetType().GetProperty(SelectedProp);
                res.Add(new ColumValue { Name = pro.Name, Value = pro.GetValue(item, null), Plant = item.Name });
            }
            PropList.Clear();
            PropList.AddRange(res);

            Series.Clear();
            Series.Add(new ColumnSeries<double>
            {
                Name = SelectedProp,
                Values = new ObservableCollection<double>().AddRange(PropList.Select(x => (double)x.Value).ToArray()),
            });

            YAxes = new List<Axis>
            {
                new()
                {//String.Format("{0:0.##}", 123.4567); 
                    // Now the Y axis we will display labels as currency
                    // LiveCharts provides some common formatters
                    // in this case we are using the currency formatter.
                    Labeler =  (value) =>$"{string.Format("{0:0.###}", value)} {_eightPlanetsInfo.findMida(SelectedProp)}",
                    TextSize=22,
                    // you could also build your own currency formatter
                    // for example:
                    // Labeler = (value) => value.ToString("C")

                    // But the one that LiveCharts provides creates shorter labels when
                    // the amount is in millions or trillions
                }
            };

        }

       
        public ObservableCollection<ISeries> Series { get; set; } = new();

        public List<Axis> XAxes { get; set; }

        public List<Axis> YAxes { get; set; }

    }
}

