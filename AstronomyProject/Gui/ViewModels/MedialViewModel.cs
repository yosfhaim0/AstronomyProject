using Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui.ViewModels
{
    public class MedialViewModel : ViewModelBase
    {
        private bool _isMuted = false;
        public bool IsMuted
        {
            get { return _isMuted; }
            set { SetProperty(ref _isMuted, value); }
        }

        public event EventHandler PlayRequested;

        public event EventHandler PauseRequested;

        public event EventHandler StopRequested;

        private DelegateCommand _play;
        public DelegateCommand PlayCommand => _play ??= new(
            () =>
            {
                PlayRequested?.Invoke(this, EventArgs.Empty);
            });

        private DelegateCommand _pause;
        public DelegateCommand PauseCommand => _pause ??= new(
            () =>
            {
                PauseRequested?.Invoke(this, EventArgs.Empty);
            });

        private DelegateCommand _stop;
        public DelegateCommand StopCommand => _stop ??= new(
            () =>
            {
                StopRequested?.Invoke(this, EventArgs.Empty);
            });

        private DelegateCommand _mute;

        public DelegateCommand MuteCommand => _mute ??= new(
            () =>
            {
                IsMuted = !IsMuted;
            });

        MediaGroupe _selectedMedia;
        public MediaGroupe SelectedMedia
        {
            get { return _selectedMedia; }
            set 
            { 
                SetProperty(ref _selectedMedia, value); 
            }
        }

    }
}
