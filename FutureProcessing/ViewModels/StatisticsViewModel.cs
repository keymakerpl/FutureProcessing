using Infrastructure.Base;
using Infrastructure.Constants;
using Infrastructure.Dialogs;
using Prism.Events;
using Prism.Regions;

namespace FutureProcessing.ViewModels
{
    public class StatisticsViewModel : DetailViewModelBase
    {
        private readonly IRegionManager _regionManager;

        public StatisticsViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IRegionManager regionManager) : base(eventAggregator, messageDialogService)
        {
            this._regionManager = regionManager;
        }

        public override bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            _regionManager.RequestNavigate(RegionNames.StatsTabControlRegion, ViewNames.ChartView);
            _regionManager.RequestNavigate(RegionNames.StatsTabControlRegion, ViewNames.StatsView);
        }
    }
}