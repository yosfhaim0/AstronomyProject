using Prism.Mvvm;
using Prism.Regions;

namespace Gui.ViewModels
{
    public class ViewModelBase : BindableBase, INavigationAware
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private bool _isActive = false;

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }


        public void OnNavigatedTo(NavigationContext navigationContext)
        {    
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {       
        }
    }
}