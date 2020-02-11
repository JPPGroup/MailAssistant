using Jpp.AddIn.MailAssistant.Abstracts;
using System;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Wrappers
{
    internal class SelectionWrapper : IWrappedObject
    {
        private readonly Outlook.Selection _innerObject;

        public Type InnerObjectType => typeof(Outlook.Selection);
        public int Count => _innerObject.Count;
        public dynamic this[int i] => _innerObject[i];

        public SelectionWrapper(Outlook.Selection selection)
        {
            _innerObject = selection ?? throw new ArgumentNullException(nameof(selection));
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

        ~SelectionWrapper()
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
