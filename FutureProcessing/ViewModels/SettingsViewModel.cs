using FutureProcessing.Wrapper;
using Infrastructure.Base;
using Infrastructure.Constants;
using Infrastructure.Dialogs;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;

namespace FutureProcessing.ViewModels
{
    public class SettingsViewModel : DetailViewModelBase
    {
        private readonly IConfig _config;
        private readonly IRegionManager _regionManager;
        private IConfigWrapper _configWrapper;

        public SettingsViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IRegionManager regionManager, IConfig config)
            : base(eventAggregator, messageDialogService)
        {
            this._regionManager = regionManager;
            this._config = config;
        }

        public IConfigWrapper ConfigWrapper { get => _configWrapper; set { SetProperty(ref _configWrapper, value); } }

        public override void Load()
        {
            try
            {
                ConfigWrapper = new ConfigWrapper(_config as Config);
            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                //TODO: Logger
            }

            ConfigWrapper.PropertyChanged += (s, a) => ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
        }

        protected override bool OnSaveCanExecute()
        {
            return ConfigWrapper != null && _config.IsValid();
        }

        protected override void OnSaveExecute()
        {
            try
            {
                _config.SaveConfig();
            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                //TODO: Logger
            }

            _regionManager.Regions[RegionNames.ContentRegion].RemoveAll();
            _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.LoginView);
        }
    }
}