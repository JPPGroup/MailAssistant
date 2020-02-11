using Jpp.AddIn.MailAssistant.Abstracts;
using System;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Wrappers
{
    internal class AddressEntryWrapper : IWrappedObject
    {
        private readonly Outlook.AddressEntry _innerObject;

        public string Location => GetLocation();
        public string Company => GetCompany();
        public string Address => GetAddress();

        public Type InnerObjectType => typeof(Outlook.AddressEntry);

        public AddressEntryWrapper(Outlook.AddressEntry item)
        {
            _innerObject = item ?? throw new ArgumentNullException(nameof(item));
        }

        private string GetLocation()
        {
            if (_innerObject == null) return "";
            if (_innerObject.Type != "EX") return "";

            var currentUser = _innerObject.GetExchangeUser();
            return currentUser != null ? currentUser.OfficeLocation : "";
        }

        private string GetCompany()
        {
            if (_innerObject == null) return "";
            if (_innerObject.Type != "EX") return "";

            var currentUser = _innerObject.GetExchangeUser();
            return currentUser != null ? currentUser.CompanyName : "";
        }

        private string GetAddress()
        {
            if (_innerObject.Type != "EX") return _innerObject.Address;

            if (_innerObject.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry || _innerObject.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
            {
                var exchangeUserUser = _innerObject.GetExchangeUser();
                return exchangeUserUser?.PrimarySmtpAddress;
            }

            if (_innerObject.AddressEntryUserType != Outlook.OlAddressEntryUserType.olSmtpAddressEntry) return "";

            return _innerObject.PropertyAccessor.GetProperty(Constants.PR_SMTP_ADDRESS) as string;
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

        ~AddressEntryWrapper()
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
