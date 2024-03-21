using DesktopSecurityApp.UserInterface.Core;

namespace DesktopSecurityApp.UserInterface.ViewModels
{
    internal class MainViewModel : ObservableObject
    {

        public RelayCommand DashboardViewCommand { get; set; }
        public RelayCommand GalleryViewCommand { get; set; }
        public RelayCommand UpdateViewCommand { get; set; }

        public DashboardViewModel DashboardVM { get; set; }
        public GalleryViewModel GalleryVM { get; set; }
        public UpdateViewModel UpdateVM { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChange();
            }
        }

        public MainViewModel()
        {
            DashboardVM = new DashboardViewModel();
            GalleryVM = new GalleryViewModel();
            UpdateVM = new UpdateViewModel();

            CurrentView = DashboardVM;

            DashboardViewCommand = new RelayCommand(o =>
            {
                CurrentView = DashboardVM;
            });

            GalleryViewCommand = new RelayCommand(o =>
            {
                CurrentView = GalleryVM;
            });

            UpdateViewCommand = new RelayCommand(o =>
            {
                CurrentView = UpdateVM;
            });
        }
    }
}
