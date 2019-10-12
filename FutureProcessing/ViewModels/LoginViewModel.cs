using Infrastructure.Base;
using Infrastructure.Constants;
using Infrastructure.Dialogs;
using Infrastructure.Events;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

namespace FutureProcessing.ViewModels
{
    public class LoginViewModel : DetailViewModelBase
    {
        private readonly IConfig _config;
        private readonly IPersonManager _personManager;
        private readonly IRegionManager _regionManager;

        private DateTime? _dateOfBirth;
        private string _firstName;
        private string _lastName;
        private string _pesel;

        public LoginViewModel(IEventAggregator eventAggregator, IMessageDialogService dialogService, IRegionManager regionManager,
            IConfig config, IPersonManager personManager)
            : base(eventAggregator, dialogService)
        {
            this._personManager = personManager;
            this._regionManager = regionManager;
            this._config = config;

            LoginCommand = new DelegateCommand(OnLoginExecute, OnLoginCanExecute);

            Initialize();
        }

        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set { SetProperty(ref _dateOfBirth, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        public DelegateCommand LoginCommand { get; private set; }

        public string Pesel
        {
            get { return _pesel; }
            set { SetProperty(ref _pesel, value); }
        }

        private bool ConnectionIsValid()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private void Initialize()
        {
            this.PropertyChanged += (s, a) => LoginCommand.RaiseCanExecuteChanged();
        }

        private bool IsMaturePerson()
        {
            var matureTime = DateTime.Now.AddYears(-18);
            return matureTime > DateOfBirth;
        }

        private bool OnLoginCanExecute()
        {
            if (String.IsNullOrEmpty(FirstName)) return false;
            if (String.IsNullOrEmpty(LastName)) return false;
            if (String.IsNullOrEmpty(Pesel)) return false;
            if (!DateOfBirth.HasValue) return false;

            return true;
        }

        private async void OnLoginExecute()
        {
            var service = await ServiceReady();
            if (!service) return;

            var peselVerification = _personManager.VerifyPesel(Pesel);
            if (peselVerification != PeselStatuses.Valid)
            {
                await _dialogService.ShowInformationMessageAsync(this, "PESEL is not valid...", $"PESEL number is not valid: {peselVerification}");
                return;
            }

            if (_personManager.DateOfBirthFromPesel != DateOfBirth.Value)
            {
                await _dialogService.ShowInformationMessageAsync(this, "Date of birth is not valid...", "Date of birth from PESEL is not valid");
                return;
            }

            if (!_personManager.PersonIsAllowedToVote(Pesel))
            {
                await _dialogService.ShowInformationMessageAsync(this, "Blacklisted PESEL...", "You are not allowed to vote");
                return;
            }

            if (!IsMaturePerson())
            {
                await _dialogService.ShowInformationMessageAsync(this, "Not allowed...", "Only mature persons can vote");
                return;
            }

            var loginInfo = new Collection<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("PESEL", Pesel),
                new KeyValuePair<string, string>("FirstName", FirstName),
                new KeyValuePair<string, string>("LastName", LastName)
            };

            _eventAggregator.GetEvent<AfterUserLoggedinEvent>().Publish(new UserAuthorizationEventArgs { Pesel = Pesel, Name = FirstName, LastName = LastName } );

            var parameters = new NavigationParameters();
            parameters.Add("LoginInfo", loginInfo);

            _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.VotingView, parameters);
        }

        private async Task<bool> ServiceReady()
        {
            if (!_config.IsValid())
            {
                await _dialogService.ShowInformationMessageAsync(this, "Not configured...", "Please go to File > Settings and provide settings");
                return false;
            }

            if (!ConnectionIsValid())
            {
                await _dialogService.ShowInformationMessageAsync(this, "Not connected...", "No connection to internet");
                return false;
            }            

            var progress = await _dialogService.GetProgressBarDialog(this, "Checking database...", "Please wait");
            progress.SetIndeterminate();

            bool serviceReady = _personManager.IsServiceReady();
            await progress.CloseAsync();

            if (!serviceReady)
            {
                await _dialogService.ShowInformationMessageAsync(this, "Not connected...", "No connection to SQL");
                return false;
            }

            return true;
        }
    }
}