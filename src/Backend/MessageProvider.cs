using Jpp.Common.Backend;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpp.AddIn.MailAssistant.Backend
{
    internal class MessageProvider : IMessageProvider
    {
        public event EventHandler ErrorOccurred;

        public Task ShowStorageAccessPermissionWarning()
        {
            throw new NotImplementedException();
        }

        public async Task ShowCriticalError(string message)
        {
            OnErrorPrompt();
            await ShowError(message);
        }

        public async Task ShowError(string message)
        {
            OnErrorPrompt();
            await Task.Run(() => MessageBox.Show(message));
        }

        protected virtual void OnErrorPrompt()
        {
            ErrorOccurred?.Invoke(this, EventArgs.Empty);
        }
    }
}
