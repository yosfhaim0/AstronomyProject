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
            PropNames = typeof(Planet).GetProperties().Select(x => x.Name).ToList();
            PropNames.Remove("Name");
            PropNames.Remove("Url");
            PropNames.Remove("HasRingSystem");
            PropNames.Remove("HasGlobalMagneticField");
            PropNames.Remove("Id");

            SelectedProp = "Mass";
            //Series = new ObservableCollection<ISeries>() { new ColumnSeries<double>() };
            setColum(SelectedProp);




            XAxes = new List<Axis>
            {
                new()
                {
                    // Use the labels property to define named labels.
                    Labels = PlanetList.Select(X=>X.Name).ToArray()
                }
            };



        }

        private void setColum(string propertyName)
        {


            List<ColumValue> res = new();
            foreach (var item in PlanetList)
            {
                var pro = item.GetType().GetProperty(propertyName);
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
                {
                    // Now the Y axis we will display labels as currency
                    // LiveCharts provides some common formatters
                    // in this case we are using the currency formatter.
                    Labeler =  (value) =>value+findMida(SelectedProp),

                    // you could also build your own currency formatter
                    // for example:
                    // Labeler = (value) => value.ToString("C")

                    // But the one that LiveCharts provides creates shorter labels when
                    // the amount is in millions or trillions
                }
            };

        }

        private static string findMida(string value)
        {
            switch (value)
            {
                case "Mass":
                    return "(1024kg)";
                case "Diameter":
                    return "(km)";
                case "Density":
                    return "(kg / m3)";
                case "Gravity":
                    return "(m / s2)";
                case "EscapeVelocity":
                    return "(km/ s)";
                case "RotationPeriod":
                    return "(hours)";
                case "LengthofDay":
                    return "(hours)";
                case "DistanceFromSun":
                    return "(106 km)";
                case "Perihelion":
                    return "(106 km)";
                case "Aphelion":
                    return "(106 km)";
                case "OrbitalPeriod":
                    return "(days)";
                case "OrbitalVelocity":
                    return "(km/ s)";
                case "OrbitalInclination":
                    return "(degrees)";
   
                case "ObliquityToOrbit":
                    return "(degrees)";
                case "MeanTemperature":
                    return "(C)";
                case "SurfacePressure":
                    return "(bars)";
                
                default:
                    return "";
            }
        }

        public ObservableCollection<ISeries> Series { get; set; } = new();

        public List<Axis> XAxes { get; set; }

        public List<Axis> YAxes { get; set; }

    }
}

