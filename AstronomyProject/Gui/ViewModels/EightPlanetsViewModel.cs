using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using DomainModel.Services;

using Models;
using System.Text.RegularExpressions;
using Prism.Commands;
using Gui.LiveCharts;

namespace Gui.ViewModels
{
    public class EightPlanetsViewModel : ViewModelBase
    {
        readonly EightPlanetsService _eightPlanetsService;

        public EightPlanetsViewModel(EightPlanetsService eightPlanetsService)
        {
            _eightPlanetsService = eightPlanetsService;
            PlanetList.AddRange(_eightPlanetsService.GetEightPlanetsInfo());

            PropNames = new(_eightPlanetsService.GetPropertyToolTipPairs());
            SetPropName();

            SelectedProp = PropNames.First();
        }

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

        public ObservableCollection<PropertyToolTipPair> PropNames { get; set; }

        public ObservableCollection<ColumValue> PlanetsProperties { get; set; } = new();

        private PropertyToolTipPair _selectedProp;
        public PropertyToolTipPair SelectedProp
        {
            get
            {
                return _selectedProp;
            }
            set
            {
                SetProperty(ref _selectedProp, value);
                SetPropertiesChart();
                ExplanImage = _eightPlanetsService.GetExplanImageList(SelectedProp.Property);
            }
        }

        private void SetPropName()
        {
            foreach (var v in PropNames)
            {
                string[] split = Regex.Split(v.PropertyName, @"(?<!^)(?=[A-Z])");
                v.PropertyName = split.Length > 1 ? string.Join(" ", split) : split[0];
            }
        }

        private Chart _propertiesChart;
        public Chart PropertiesChart 
        { 
            get => _propertiesChart; 
            set => SetProperty(ref _propertiesChart, value);
        }

        private void SetPropertiesChart()
        {
            List<ColumValue> temp = new();
            foreach (var planet in PlanetList)
            {
                var prop = planet.GetType()
                    .GetProperty(SelectedProp.Property);
                temp.Add(new ColumValue
                {
                    Name = prop.Name,
                    Value = prop.GetValue(planet, null),
                    Plant = planet.Name
                });
            }
            PlanetsProperties.Clear();
            PlanetsProperties.AddRange(temp);

            PropertiesChart = new ChartBuilder()
                .SetColumnSeries(values: PlanetsProperties.Select(x => (double)x.Value), 
                    name: SelectedProp.PropertyName)
                .SetXAxes(labels: PlanetList.Select(x => x.Name).ToList(), textSize:22, nameTextSize: 22)
                .SetYAxes(labeler: (value) => $"{FormatNumber(value)}{_eightPlanetsService.FindMida(SelectedProp.Property)}",
                    textSize: 22)
                .Build();
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
    }

    public class ColumValue
    {
        public object Value { get; set; }
        public string Name { get; set; }
        public string Plant { get; set; }

    }
}

