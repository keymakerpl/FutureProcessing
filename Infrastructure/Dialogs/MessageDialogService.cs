using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;

namespace Infrastructure.Dialogs
{
    public class MessageDialogService : IMessageDialogService
    {
        private IDialogCoordinator _dialogCoordinator;
        private MetroDialogSettings _confirmDialogSettings;

        public MessageDialogService(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            _confirmDialogSettings = new MetroDialogSettings { AffirmativeButtonText = "Confirm", NegativeButtonText = "Cancel" };
        }

        public async Task<DialogResult> ShowConfirmationMessageAsync(object context, string title, string message)
        {
            return await _dialogCoordinator
                .ShowMessageAsync(context, title, message, MessageDialogStyle.AffirmativeAndNegative, _confirmDialogSettings) == MessageDialogResult.Affirmative ?
                DialogResult.OK : DialogResult.Cancel;
        }

        public async Task<string> ShowInputMessageAsync(object context, string title, string message)
        {
            return await _dialogCoordinator.ShowInputAsync(context, title, message, _confirmDialogSettings);
        }

        public async Task ShowInformationMessageAsync(object context, string title, string message)
        {
            await _dialogCoordinator.ShowMessageAsync(context, title, message);
        }

        public async Task ShowAccessDeniedMessageAsync(object context, string title = null, string message = null)
        {
            var dialogTitle = title != null ? title : "No access...";
            var dialogMessage = message != null ? message : "You cannot do this.";

            await _dialogCoordinator.ShowMessageAsync(context, dialogTitle, dialogMessage);
        }

        public async Task<ProgressDialogController> GetProgressBarDialog(object context, string title, string message)
        {
            var controller = await _dialogCoordinator.ShowProgressAsync(context, title, message);

            return controller;
        }
    }

    public enum DialogResult
    {
        OK,
        Cancel
    }
}
