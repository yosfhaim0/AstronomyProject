﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DomainModel.Services;

using Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Text.RegularExpressions;

namespace Gui.ViewModels
{
    public class EightPlanetsViewModel : ViewModelBase
    {
        readonly EightPlanetsService _eightPlanetsService;
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
            get { return _selectedProp.Replace(" ", string.Empty); }
            set
            {
                SetProperty(ref _selectedProp, value);
                SetColum();
                SetExplanImage();
            }
        }

        private void SetExplanImage()
        {
            ExplanImage = _eightPlanetsService.GetExplanImageList(SelectedProp);
        }

        public EightPlanetsViewModel(EightPlanetsService eightPlanetsService)
        {
            _eightPlanetsService = eightPlanetsService;
            PlanetList.AddRange(_eightPlanetsService.GetEightPlanetsInfo());
            PropNames = typeof(Planet).GetProperties().Select(x => x.Name).ToList();

            PropNames.Remove("Name");
            PropNames.Remove("Url");
            PropNames.Remove("HasRingSystem");
            PropNames.Remove("HasGlobalMagneticField");
            PropNames.Remove("Id");
            SelectedProp = PropNames.FirstOrDefault();
            SetPropName();
            //ExplanImageList.AddRange(_eightPlanetsInfo.getExplanImageList(PropNames));


            XAxes = new List<Axis>
            {
                new()
                {
                    // Use the labels property to define named labels.
                    Labels = PlanetList.Select(x => x.Name).ToArray(),
                    TextSize=22,
                    NameTextSize=22,



                }
            };



        }

        private void SetPropName()
        {
            List<string> r = new();
            string str = "";
            foreach (var v in PropNames)
            {
                string[] split = Regex.Split(v, @"(?<!^)(?=[A-Z])");
                foreach (var item in split)
                {
                    str += item + " ";
                }
                r.Add(str);
                str = "";
            }
            PropNames.Clear();
            PropNames.AddRange(r);
        }

        private void SetColum()
        {


            List<ColumValue> res = new();
            var str = "";
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
                MaxBarWidth = 10,
            });

            YAxes = new List<Axis>
            {
                new()
                {//String.Format("{0:0.##}", 123.4567); 
                    // Now the Y axis we will display labels as currency
                    // LiveCharts provides some common formatters
                    // in this case we are using the currency formatter.
                    
                    Labeler =  (value) =>$"{FormatNumber(value)}{_eightPlanetsService.FindMida(SelectedProp)}",
                    TextSize=22,
                    // you could also build your own currency formatter
                    // for example:
                    // Labeler = (value) => value.ToString("C")

                    // But the one that LiveCharts provides creates shorter labels when
                    // the amount is in millions or trillions
                }
            };

        }
        private static string FormatNumber(double num)
        {
            if (num <= 1 && num >= 0)
                return string.Format("{0:0.###}", (num));
            // Ensure number has max 3 significant digits (no rounding up can happen)
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            if (i == 0)
                return num.ToString("0.##");
            num = num / i * i;

            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "B";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "M";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "K";

            return num.ToString("#,0");
        }

        public ObservableCollection<ISeries> Series { get; set; } = new();

        public List<Axis> XAxes { get; set; }

        public List<Axis> YAxes { get; set; }

    }

    public class ColumValue
    {
        public object Value { get; set; }
        public string Name { get; set; }
        public string Plant { get; set; }

    }
}

