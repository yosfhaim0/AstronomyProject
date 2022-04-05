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
using Prism.Commands;

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

        private ObservableCollection<string> _selectedProps = new();

        private string _selectedProp;
        public string SelectedProp
        {
            get
            {
                return _selectedProp;
            }
            set
            {
                SetProperty(ref _selectedProp, value);
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
            PropNames = typeof(Planet)
                .GetProperties()
                .Select(x => x.Name)
                .ToList();

            PropNames.Remove("Name");
            PropNames.Remove("Url");
            PropNames.Remove("HasRingSystem");
            PropNames.Remove("HasGlobalMagneticField");
            PropNames.Remove("Id");

            //SelectedProp = PropNames.FirstOrDefault();
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

        private DelegateCommand<object> _selectMenyFeildsCommand;

        public DelegateCommand<object> SelectMenyFeildsCommand => _selectMenyFeildsCommand
            ??= new(
            (selectedPropNames) =>
            {
                var propNames = (selectedPropNames as IList<object>)
                .Select(i => i.ToString())
                .ToList();
                if(propNames.Count <= 0)
                {
                    Series.Clear();
                    return;
                }
                foreach (var p1 in propNames)
                {
                    var p_p1 = p1.Replace(" ", string.Empty);
                    foreach (var p2 in propNames)
                    {
                        var p_p2 = p2.Replace(" ", string.Empty);
                        if (_eightPlanetsService.FindMida(p_p1) != _eightPlanetsService.FindMida(p_p2))
                        {
                            return;
                        }
                    }
                }
                var mida = _eightPlanetsService.FindMida(propNames.FirstOrDefault().Replace(" ", string.Empty));
                SetColumn(propNames, mida);

            });

        private void SetPropName()
        {

            List<string> r = new();
            foreach (var v in PropNames)
            {
                string[] split = Regex.Split(v, @"(?<!^)(?=[A-Z])");
                var res = split.Length > 1 ? string.Join(" ", split) : split[0];
                r.Add(res);
            }
            PropNames.Clear();
            PropNames.AddRange(r);
        }

        private void SetColumn(List<string> vs, string mida)
        {
            List<ColumValue> res = new();
            foreach (var item in PlanetList)
            {
                foreach (var v in vs)
                {
                    var prop = item
                            .GetType()
                            .GetProperty(v.Replace(" ", string.Empty));
                    res.Add(new ColumValue
                    {
                        Name = prop.Name,
                        Value = prop.GetValue(item, null),
                        Plant = item.Name
                    });
                }

            }
            PropList.Clear();
            PropList.AddRange(res);

            var seri = new List<ISeries>();
            foreach (var v in vs)
            {
                var ll = PropList
                    .Where(p => p.Name == v.Replace(" ", string.Empty))
                    .Select(x => (double)x.Value);
                seri.Add(new ColumnSeries<double>
                {
                    Name = v,
                    Values = new ObservableCollection<double>(ll),
                    MaxBarWidth = 10,
                });
            }
            Series.Clear();
            Series.AddRange(seri);

            YAxes = new List<Axis>
            {
                new()
                {//String.Format("{0:0.##}", 123.4567); 
                    // Now the Y axis we will display labels as currency
                    // LiveCharts provides some common formatters
                    // in this case we are using the currency formatter.
                    
                    Labeler =  (value) =>$"{FormatNumber(value)}{mida}",
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

