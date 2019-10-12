using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace Infrastructure.Dialogs
{
    public interface IMessageDialogService
    {
        Task<DialogResult> ShowConfirmationMessageAsync(object context, string title, string message);
        Task<string> ShowInputMessageAsync(object context, string title, string message);
        Task ShowInformationMessageAsync(object context, string title, string message);
        Task ShowAccessDeniedMessageAsync(object context, string title = null, string message = null);
        Task<ProgressDialogController> GetProgressBarDialog(object context, string title, string message);
    }
}