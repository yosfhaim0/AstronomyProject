using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.ViewModels
{
    public interface IMediaPlayer
    {
        Action Play { get; set; }

        Action Stop { get; set; }

        Action Pause { get; set; }

        Action Mute { get; set; }
    }
}
