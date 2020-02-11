using Jpp.AddIn.MailAssistant.Abstracts;
using System;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Wrappers
{
    internal class TaskRequestItemWrapper : IOutlookItem
    {
        private readonly Outlook.TaskRequestItem _innerObject;

        public Type InnerObjectType => typeof(Outlook.TaskRequestItem);
        public string Id => "";
        public string Description => "";
        public string Folder => ((Outlook.Folder)_innerObject.Parent).Name;
        public int Size => _innerObject.Size;
        public string Subject => _innerObject.Subject;
        public string Sender => _innerObject.PropertyAccessor.GetProperty(Constants.PR_SENDER_EMAIL_ADDRESS) as string;

        public TaskRequestItemWrapper(Outlook.TaskRequestItem item)
        {
            _innerObject = item ?? throw new ArgumentNullException(nameof(item));
        }

        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing) { } // TODO: dispose managed objects.

            Marshal.ReleaseComObject(_innerObject);

            _disposedValue = true;
        }

        ~TaskRequestItemWrapper()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
