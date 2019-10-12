using FutureProcessing.Views;
using Infrastructure.Base;
using Infrastructure.Constants;
using Infrastructure.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;

namespace FutureProcessing.ViewModels
{
    public class MainWindowViewModel : DetailViewModelBase
    {
        private readonly IModuleManager _moduleManager;
        private readonly IRegionManager _regionManager;

        public MainWindowViewModel(IEventAggregator eventAggregator, IMessageDialogService dialogService, IRegionManager regionManager, IModuleManager moduleManager) : base(eventAggregator, dialogService)
        {
            this._regionManager = regionManager;
            this._moduleManager = moduleManager;

            _moduleManager.LoadModule("PersonManagerModule");

            SettingsCommand = new DelegateCommand(OnSettingsExecute);
            LogoutCommand = new DelegateCommand(OnLogoutExecute);
            StatsCommand = new DelegateCommand(OnStatsExecute);
            ExitCommand = new DelegateCommand(OnExitExecute);

            Title = "Future Processing 1.0";

            ShowLoginWindow();
        }

        public DelegateCommand ExitCommand { get; }
        public DelegateCommand LogoutCommand { get; }
        public DelegateCommand SettingsCommand { get; }
        public DelegateCommand StatsCommand { get; }

        private void OnExitExecute()
        {
            System.Windows.Application.Current.Shutdown();
        }
        
        private void OnLogoutExecute()
        {
            _regionManager.Regions[RegionNames.ContentRegion].RemoveAll();
            _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.LoginView);
        }

        private void OnSettingsExecute()
        {
            _regionManager.Regions[RegionNames.ContentRegion].RemoveAll();
            _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.SettingsView);
        }

        private void OnStatsExecute()
        {
            _regionManager.Regions[RegionNames.ContentRegion].RemoveAll();
            _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.StatisticsView);
        }

        private void ShowLoginWindow()
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(LoginView));
        }
    }
}