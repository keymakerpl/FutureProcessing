using Business.Entities;
using Infrastructure.Base;
using Infrastructure.Dialogs;
using Infrastructure.Interfaces;
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
    public class StatsViewModel : DetailViewModelBase
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IWebContext _webContext;
        private int _incorrectVotes;

        public StatsViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IVoteRepository voteRepository, IWebContext webContext)
            : base(eventAggregator, messageDialogService)
        {
            this._voteRepository = voteRepository;
            this._webContext = webContext;
            Title = "Stats";

            CandidatesVotes = new ObservableCollection<object>();
        }

        public ObservableCollection<object> CandidatesVotes { get; }

        public int IncorrectVotes
        {
            get { return _incorrectVotes; }
            set { SetProperty(ref _incorrectVotes, value); }
        }

        public override bool KeepAlive => true;

        public override async void Load()
        {
            try
            {
                var votes = await GetVotes();
                var badVotes = votes.Where(v => v.VotedCandidates.Count > 1 || v.VotedCandidates.Count == 0);
                var goodVotes = votes.Where(v => v.VotedCandidates.Count == 1).Select(v => v.VotedCandidates.FirstOrDefault()).ToList();

                var candidates = _webContext.GetCandidates();
                foreach (var candidate in candidates)
                {
                    var count = goodVotes.Where(c => c.Name == candidate.name && c.Party == candidate.party).Count();
                    CandidatesVotes.Add(new { candidate.name, candidate.party, count });
                }

                IncorrectVotes = badVotes.Count();
            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                //TODO: Logger
            }
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Load();
        }

        private async Task<IEnumerable<Vote>> GetVotes()
        {
            return await _voteRepository.GetAllAsync();
        }
    }
}