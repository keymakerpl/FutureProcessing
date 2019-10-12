using Infrastructure.Dialogs;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Infrastructure.Base
{
    public abstract class DetailViewModelBase : BindableBase, IDetailViewModelBase, IConfirmNavigationRequest, IRegionMemberLifetime
    {
        protected readonly IEventAggregator _eventAggregator;

        protected readonly IMessageDialogService _dialogService;
        private bool _hasChanges;

        private bool _isReadOnly;
        private string _title = "";

        public DetailViewModelBase(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _dialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            CloseCommand = new DelegateCommand(OnCloseDetailViewExecute); 
            CancelCommand = new DelegateCommand(OnCancelEditExecute);
        }

        public bool AllowLoadAsync { get; set; } = true; 
        public ICommand CancelCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    RaisePropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public Guid ID { get; protected set; }
        public bool IsReadOnly { get => _isReadOnly; set { SetProperty(ref _isReadOnly, value); } }
        public ICommand SaveCommand { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public virtual void Load()
        {
            throw new NotImplementedException();
        }

        public virtual void Load(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task LoadAsync()
        {
            throw new NotImplementedException("Ups!");
        }

        public virtual Task LoadAsync(Guid id)
        {
            throw new NotImplementedException("Ups!");
        }

        #region Events and Events Handlers
        protected virtual void OnCancelEditExecute()
        {
            Console.WriteLine("Not implemented");
        }

        protected virtual void OnCloseDetailViewExecute()
        {
            _eventAggregator.GetEvent<AfterDetailClosedEvent>().Publish(new AfterDetailClosedEventArgs()
            {
                Id = this.ID,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual bool OnSaveCanExecute()
        {
            return false;
        }

        protected virtual void OnSaveExecute()
        {
            throw new NotImplementedException();
        }

        protected virtual void RaiseDetailSavedEvent(Guid modelId, string displayMember)
        {
            _eventAggregator.GetEvent<AfterDetailSavedEvent>()
                .Publish(new AfterDetailSavedEventArgs()
                {
                    Id = modelId,
                    DisplayMember = displayMember,
                    ViewModelName = this.GetType().Name
                });
        }

        protected virtual void RaiseDetailDeletedEvent(Guid modelId, string displayMember)
        {
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Publish(new AfterDetailDeletedEventArgs()
                {
                    Id = modelId,
                    ViewModelName = this.GetType().Name
                });
        }
        #endregion

        /// <summary>
        /// Optimistic save
        /// </summary>
        /// <param name="saveFunc"></param>
        /// <param name="afterSaveAction"></param>
        /// <returns></returns>
        protected async Task SaveWithOptimisticConcurrencyAsync(Func<Task> saveFunc, Action afterSaveAction)
        {
            try
            {
                await saveFunc();
            }
            catch (DbUpdateConcurrencyException e) 
            {
                var databaseValues = e.Entries.Single().GetDatabaseValues();
                if (databaseValues == null) 
                {
                    await _dialogService
                        .ShowInformationMessageAsync(this, "Deleted...", "Deleted element.");
                    RaiseDetailSavedEvent(ID, Title);
                    return;
                }

                var dialogResult =
                    await _dialogService
                    .ShowConfirmationMessageAsync(this, "Changed by someone else...", "Override?");

                if (dialogResult == DialogResult.OK)
                {
                    var entry = e.Entries.Single(); 
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues()); 
                    await saveFunc(); 
                }
                else
                {
                    await e.Entries.Single().ReloadAsync(); 
                    await LoadAsync(ID); 
                }
            }

            afterSaveAction();
        }

        #region Navigation
        public virtual async void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            var dialogResult = true;
            if (HasChanges)
            {
                dialogResult = await _dialogService.ShowConfirmationMessageAsync(this, "Not saved...", "Continue?")
                    == DialogResult.OK;
            }

            continuationCallback(dialogResult);
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public virtual bool KeepAlive => false;
        #endregion

    }
}
