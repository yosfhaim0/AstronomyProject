using Prism.Mvvm;

namespace Gui.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
    }
}