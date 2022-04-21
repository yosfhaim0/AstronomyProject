using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel.Services;
using Gui.LiveCharts;
using Gui.Views;
using Models;

namespace Gui.Dialogs
{
    public class ChartDialog
    {
        readonly IMediaService _mediaService;

        TagImageChart _chartDialog;

        public ChartDialog(IMediaService mediaService)
        {
            _mediaService = mediaService;
            _chartDialog = new();
        }

        public async Task ShowChartByMedia(MediaGroupe media)
        {
            _chartDialog = new();
            var vm = new ViewModels.TagImageChartViewModel();
            vm.IsLoading = true;
            _chartDialog.DataContext = vm;
            _chartDialog.Show();
            Chart tagsChart;
            if (media.Tags.Any())
            {
                tagsChart = SetLiveChart(media.Tags);
            }
            else
            {
                var tags = await _mediaService.GetMediaTags(media);
                tagsChart = SetLiveChart(tags);
            }
            vm.TagsChart = tagsChart;
            vm.IsLoading = false;

        }

        public void CloseDialog()
        {
            if (_chartDialog.IsVisible)
            {
                _chartDialog.Close();
            }
        }

        private Chart SetLiveChart(IEnumerable<ImaggaTag> tags)
        {
            return new ChartBuilder()
                .SetRowSeries(tags.Select(x => x.Confidence), "Confidence", 15)
                .SetXAxes(labeler: (v) => v.ToString())
                .SetYAxes(labels: tags.Select(t => t.Tag).ToList())
                .Build();
        }


    }
}
