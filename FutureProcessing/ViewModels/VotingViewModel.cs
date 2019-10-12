using Business.Entities;
using FutureProcessing.Wrapper;
using Infrastructure.Base;
using Infrastructure.Constants;
using Infrastructure.Dialogs;
using Infrastructure.Interfaces;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebDataAccess;

namespace FutureProcessing.ViewModels
{
    public class CandidateLookupItem
    {
        public Business.DTOs.Candidate Candidate { get; set; }

        public int Index { get; set; }
        public bool IsChecked { get; set; }
    }

    public class VotingViewModel : DetailViewModelBase
    {
        private readonly IPersonManager _personManager;
        private readonly IRegionManager _regionManager;
        private readonly IVoteRepository _voteRepository;
        private readonly IWebContext _webContext;

        public VotingViewModel(IEventAggregator eventAggregator, IMessageDialogService dialogService, IRegionManager regionManager, IWebContext webContext,
            IPersonManager personManager, IVoteRepository voteRepository)
            : base(eventAggregator, dialogService)
        {
            this._webContext = webContext;
            this._personManager = personManager;
            this._regionManager = regionManager;

            Candidates = new ObservableCollection<CandidateLookupItem>();
            VoteCommand = new DelegateCommand(OnVoteExecute);
            this._voteRepository = voteRepository;
        }

        public ObservableCollection<CandidateLookupItem> Candidates { get; private set; }

        public IPersonWrapper Person { get; private set; }

        public DelegateCommand VoteCommand { get; private set; }

        public override async Task LoadAsync()
        {
            await InitializeCandidates();
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var loginInfo = navigationContext.Parameters.GetValue<Collection<KeyValuePair<string, string>>>("LoginInfo");

            if (loginInfo != null)
            {
                var firstName = loginInfo.SingleOrDefault(k => k.Key == "FirstName").Value;
                var lastName = loginInfo.SingleOrDefault(k => k.Key == "LastName").Value;
                var pesel = loginInfo.SingleOrDefault(k => k.Key == "PESEL").Value;

                var person = await _personManager.GetPersonAsync(pesel, firstName, lastName);
                Person = new PersonWrapper(person);

                PrepareReadOnly(Person);
            }

            await LoadAsync();
        }

        private bool CandidateHasVote(Business.DTOs.Candidate candidate, IPersonWrapper person)
        {
            if (candidate == null)
                throw new ArgumentNullException(nameof(candidate));

            if (person == null)
                throw new ArgumentNullException(nameof(person));

            var vote = GetPersonVote(person);
            if (vote == null) return false;

            var result = vote.VotedCandidates.Any(c => c.Name == candidate.name && c.Party == candidate.party);
            return result;
        }

        private async void CreateVote()
        {
            try
            {
                var vote = new Vote();

                var selectedCandidates = Candidates.Where(c => c.IsChecked);
                foreach (var candidate in selectedCandidates)
                {
                    vote.VotedCandidates.Add(new Business.Entities.Candidate()
                    {
                        Name = candidate.Candidate.name,
                        Party = candidate.Candidate.party
                    });
                }

                vote.IsConfirmed = true;
                vote.PersonId = ((PersonWrapper)Person).Model.Id;

                _voteRepository.Add(vote);

                await SaveWithOptimisticConcurrencyAsync(_voteRepository.SaveAsync, () =>
                {
                    _regionManager.Regions[RegionNames.ContentRegion].RemoveAll();
                    _regionManager.RequestNavigate(RegionNames.ContentRegion, ViewNames.StatisticsView);
                });
            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                //TODO: Logger
            }
        }

        private IEnumerable<Business.DTOs.Candidate> GetCandidates()
        {
            return _webContext.GetCandidates();
        }

        private Vote GetPersonVote(IPersonWrapper person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            var votes = _voteRepository.FindByInclude(v => v.PersonId == ((PersonWrapper)person).Model.Id, c => c.VotedCandidates);
            var vote = votes.FirstOrDefault();
            return vote;
        }

        private async Task InitializeCandidates()
        {
            try
            {
                Candidates.Clear();
                var candidates = await Task.FromResult(GetCandidates());
                foreach (var candidate in candidates)
                {
                    Candidates.Add(new CandidateLookupItem() { Candidate = candidate, IsChecked = CandidateHasVote(candidate, Person) });
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                //TODO: Logger
            }
        }

        private async void OnVoteExecute()
        {
            var dialogResult = await _dialogService.ShowConfirmationMessageAsync(this, "Accepting your vote...", "Are you sure to accept your choose?");
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            CreateVote();
        }

        private void PrepareReadOnly(IPersonWrapper person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            var vote = GetPersonVote(person);

            if (vote != null && vote.IsConfirmed)
                IsReadOnly = true;
        }
    }
}