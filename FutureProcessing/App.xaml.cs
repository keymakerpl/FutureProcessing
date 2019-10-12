using FutureProcessing.Data.Repository;
using FutureProcessing.Views;
using Infrastructure.Constants;
using Infrastructure.Dialogs;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using MSSQLDataAccess;
using PersonManager;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using WebDataAccess;
using System.Linq;

namespace FutureProcessing
{
    public partial class App
    {
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {            
            moduleCatalog.AddModule(typeof(MSSQLDataAccessModule));
            moduleCatalog.AddModule(typeof(WebDataAccessModule));
            moduleCatalog.AddModule<PersonManagerModule>("PersonManagerModule");

            base.ConfigureModuleCatalog(moduleCatalog);
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IConfig, Config>();
            containerRegistry.Register<IWebContext, WebContext>();
            containerRegistry.Register<IPeselValidator, PeselValidator>();
            containerRegistry.Register<IDialogCoordinator, DialogCoordinator>();
            containerRegistry.Register<IMessageDialogService, MessageDialogService>();
            containerRegistry.Register<IStringHasher, StringHasher>();
            containerRegistry.Register<ICandidateRepository, CandidateRepository>();
            containerRegistry.Register<IPersonRepository, PersonRepository>();
            containerRegistry.Register<IVoteRepository, VoteRepository>();

            containerRegistry.RegisterForNavigation<LoginView>(ViewNames.LoginView);
            containerRegistry.RegisterForNavigation<VotingView>(ViewNames.VotingView);
            containerRegistry.RegisterForNavigation<SettingsView>(ViewNames.SettingsView);
            containerRegistry.RegisterForNavigation<StatisticsView>(ViewNames.StatisticsView);
            containerRegistry.RegisterForNavigation<ChartView>(ViewNames.ChartView);
            containerRegistry.RegisterForNavigation<StatsView>(ViewNames.StatsView);
        }
    }
}
