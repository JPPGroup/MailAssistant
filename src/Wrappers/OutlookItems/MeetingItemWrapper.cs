﻿using Jpp.AddIn.MailAssistant.Abstracts;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Wrappers
{
    internal class MeetingItemWrapper : IMoveable
    {
        private readonly Outlook.MeetingItem _innerObject;

        public Type InnerObjectType => typeof(Outlook.MeetingItem);
        public string Id => (string)_innerObject.PropertyAccessor.GetProperty(Constants.PR_INTERNET_MESSAGE_ID);
        public string RestrictCriteria
        {
            get
            {
                var dateFrom = _innerObject.SentOn.AddMinutes(-Constants.SEARCH_WINDOW_MINUTES).ToString(Constants.SEARCH_DATE_FORMAT);
                var dateTo = _innerObject.SentOn.AddMinutes(Constants.SEARCH_WINDOW_MINUTES).ToString(Constants.SEARCH_DATE_FORMAT);

                return $"[SentOn] >= '{dateFrom}' And [SentOn] <= '{dateTo}'";
            }
        }
        public string Description => $"{_innerObject.SentOn} | {_innerObject.Subject}";
        public string Folder => ((Outlook.Folder)_innerObject.Parent).Name;
        public int Size => _innerObject.Size;
        public string Subject => _innerObject.Subject;
        public string Sender => _innerObject.PropertyAccessor.GetProperty(Constants.PR_SENDER_EMAIL_ADDRESS) as string;

        public MeetingItemWrapper(Outlook.MeetingItem item)
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

            Outlook.MeetingItem moved = null;
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

        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing) { } // TODO: dispose managed objects.

            Marshal.ReleaseComObject(_innerObject);

            _disposedValue = true;
        }

        ~MeetingItemWrapper()
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
