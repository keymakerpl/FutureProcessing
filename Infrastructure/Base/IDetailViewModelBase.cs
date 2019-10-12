using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Infrastructure.Base
{
    public interface IDetailViewModelBase
    {
        ICommand CancelCommand { get; set; }
        ICommand CloseCommand { get; set; }
        bool HasChanges { get; set; }
        Guid ID { get; }
        bool IsReadOnly { get; set; }
        ICommand SaveCommand { get; }
        string Title { get; }

        void Load();
        void Load(Guid id);
        Task LoadAsync();
        Task LoadAsync(Guid id);
    }
}