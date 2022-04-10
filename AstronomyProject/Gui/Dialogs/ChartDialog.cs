using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Models;

namespace Gui.Dialogs
{
    public class ChartDialog
    {
        readonly IMediaService _mediaService;

        public ChartDialog(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        public async Task ShowChartByMedia(MediaGroupe media)
        {
            var chart = new Views.TagImageChart();
            var vm = new ViewModels.TagImageChartViewModel();
            vm.IsLoading = true;
            chart.DataContext = vm;
            chart.Show();
            Tuple<List<ISeries>, List<Axis>, List<Axis>> tuple;
            if (media.Tags.Any())
            {
                tuple = SetLiveChart(media.Tags);
            }
            else
            {
                var tags = await _mediaService.GetMediaTags(media);
                tuple = SetLiveChart(tags);
            }
            vm.NewMediaIn(tuple);
            vm.IsLoading = false;

        }

        private Tuple<List<ISeries>, List<Axis>, List<Axis>>  SetLiveChart(IEnumerable<ImaggaTag> tags)
        {
            List<ISeries> tagsSeries = new()
            {
                new RowSeries<double>
                {
                    Name = "Confidence",
                    Values = new List<double>(tags.Select(x => x.Confidence)),
                    MaxBarWidth = 15,
                }
            };

            List<Axis> yAxes = new()
            {
                new()
                {
                    Labels = tags.Select(t => t.Tag).ToList(),
                }
            };

            List<Axis> xAxes = new()
            {
                new()
                {
                    Labeler =(value) => value.ToString(),
                }
            };
            return new(tagsSeries, yAxes, xAxes);
        }
    }
}
