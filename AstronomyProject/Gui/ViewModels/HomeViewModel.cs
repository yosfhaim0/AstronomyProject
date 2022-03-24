using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Gui.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {


        public HomeViewModel()
        {

        }

        private DelegateCommand _goToLinkCommand;
        public DelegateCommand GoToLinkCommand => _goToLinkCommand ??= new DelegateCommand(
            () =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var url = @"https://github.com/yosfhaim0/AstronomyProject"
                                    .Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
            });
    }
}
