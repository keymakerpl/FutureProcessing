using Infrastructure.Base;
using Infrastructure.Dialogs;
using Infrastructure.Interfaces;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using WebDataAccess;

namespace FutureProcessing.ViewModels
{
    public class ChartViewModel : DetailViewModelBase
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IWebContext _webContext;
        public ChartViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IVoteRepository voteRepository, IWebContext webContext)
            : base(eventAggregator, messageDialogService)
        {
            this._voteRepository = voteRepository;
            this._webContext = webContext;

            Title = "Charts";

            CandidatesLegend = new ObservableCollection<CandidateLookupItem>();

            SeriesCollection = new SeriesCollection();
        }

        public ObservableCollection<CandidateLookupItem> CandidatesLegend { get; }

        public Func<int, string> Formatter { get; set; }

        public override bool KeepAlive => true;

        public string[] Labels { get; set; }

        public SeriesCollection SeriesCollection { get; set; }

        public override async void Load()
        {
            try
            {
                
                SeriesCollection.Add
                    (
                        new ColumnSeries
                        {
                            Title = "Votes",
                        }
                    );

                var votes = await _voteRepository.GetAllAsync();

                var candidates = _webContext.GetCandidates().ToArray();
                var goodVotes = votes.Where(v => v.VotedCandidates.Count == 1).Select(v => v.VotedCandidates.FirstOrDefault()).ToList();

                SeriesCollection[0].Values = new ChartValues<int>();

                Labels = new string[candidates.Length];
                for (int i = 0; i < Labels.Length; i++)
                {
                    var count = goodVotes.Where(c => c.Name == candidates[i].name && c.Party == candidates[i].party).Count();
                    SeriesCollection[0].Values.Add(count);
                    Labels[i] = i.ToString(); //$"{candidates[i].name} ({candidates[i].party})";
                    CandidatesLegend.Add(new CandidateLookupItem()
                    {
                        Index = i,
                        Candidate = new Business.DTOs.Candidate() { name = candidates[i].name, party = candidates[i].party }
                    });
                }

                Formatter = value => value.ToString("N");
            }
            catch (Exception ex)
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
    }
}