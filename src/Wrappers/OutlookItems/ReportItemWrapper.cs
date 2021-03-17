using Jpp.AddIn.MailAssistant.Abstracts;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Wrappers
{
    internal class ReportItemWrapper : IMoveable
    {
        private readonly Outlook.ReportItem _innerObject;

        public Type InnerObjectType => typeof(Outlook.ReportItem);
        public string Id => (string)_innerObject.PropertyAccessor.GetProperty(Constants.PR_INTERNET_MESSAGE_ID);
        public string RestrictCriteria
        {
            get
            {
                var subject = _innerObject.Subject;

                return $"[Subject] = '{subject}'";
            }
        }


        public string Description => $"{_innerObject.CreationTime} | {_innerObject.Subject}";
        public string Folder => ((Outlook.Folder)_innerObject.Parent).Name;
        public int Size => _innerObject.Size;
        public string Subject => _innerObject.Subject;
        public string Sender => _innerObject.PropertyAccessor.GetProperty(Constants.PR_SENDER_EMAIL_ADDRESS) as string;

        public ReportItemWrapper(Outlook.ReportItem item)
        {
            _innerObject = item ?? throw new ArgumentNullException(nameof(item));
        }

        public bool Equals(IMoveable other)
        {
            if (other == null) return false;
            if (other.InnerObjectType != InnerObjectType) return false;

            return other.Id == Id || other.Description == Description;
        }

        bool IMoveable.Move(Outlook.Folder folder)
        {
            //TODO : Refactor

            Outlook.ReportItem moved = null;
            Outlook.Folder parent = null;

            try
            {
                moved = _innerObject.Move(folder);
                parent = moved.Parent;

                if (parent.FullFolderPath != folder.FullFolderPath)
                {
                    var ex = new Exception("Item not moved to expected folder");
                    var props = new Dictionary<string, string>
                    {
                        {"Item", nameof(Outlook.MailItem)},
                        {"Description", Description},
                        {"Target", folder.FullFolderPath},
                        {"Actual", parent.FullFolderPath}
                    };

                    Crashes.TrackError(ex, props);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                return false;
            }
            finally
            {
                if (moved != null) Marshal.ReleaseComObject(moved);
                if (parent != null) Marshal.ReleaseComObject(parent);
            }
        }

        void IMoveable.Delete()
        {
            _innerObject.Delete();
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

        ~ReportItemWrapper()
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
