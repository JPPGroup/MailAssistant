using System;
using System.Diagnostics;
using System.Reflection;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant
{
    /// <summary>
    /// Helper class to access common properties of Outlook Items.
    /// </summary>
    internal class OutlookItem
    {
        private readonly Type _type;
        private Type _typeOlObjectClass;

        #region OutlookItem Constants

        private const string OL_ACTIONS = "Actions";
        private const string OL_APPLICATION = "Application";
        private const string OL_ATTACHMENTS = "Attachments";
        private const string OL_BILLING_INFORMATION = "BillingInformation";
        private const string OL_BODY = "Body";
        private const string OL_CATEGORIES = "Categories";
        private const string OL_CLASS = "Class";
        private const string OL_CLOSE = "Close";
        private const string OL_COMPANIES = "Companies";
        private const string OL_CONVERSATION_INDEX = "ConversationIndex";
        private const string OL_CONVERSATION_TOPIC = "ConversationTopic";
        private const string OL_COPY = "Copy";
        private const string OL_CREATION_TIME = "CreationTime";
        private const string OL_DISPLAY = "Display";
        private const string OL_DOWNLOAD_STATE = "DownloadState";
        private const string OL_ENTRY_ID = "EntryID";
        private const string OL_FORM_DESCRIPTION = "FormDescription";
        private const string OL_GET_INSPECTOR = "GetInspector";
        private const string OL_IMPORTANCE = "Importance";
        private const string OL_IS_CONFLICT = "IsConflict";
        private const string OL_ITEM_PROPERTIES = "ItemProperties";
        private const string OL_LAST_MODIFICATION_TIME = "LastModificationTime";
        private const string OL_LINKS = "Links";
        private const string OL_MARK_FOR_DOWNLOAD = "MarkForDownload";
        private const string OL_MESSAGE_CLASS = "MessageClass";
        private const string OL_MILEAGE = "Mileage";
        private const string OL_MOVE = "Move";
        private const string OL_NO_AGING = "NoAging";
        private const string OL_OUTLOOK_INTERNAL_VERSION = "OutlookInternalVersion";
        private const string OL_OUTLOOK_VERSION = "OutlookVersion";
        private const string OL_PARENT = "Parent";
        private const string OL_PRINT_OUT = "PrintOut";
        private const string OL_PROPERTY_ACCESSOR = "PropertyAccessor";
        private const string OL_SAVE = "Save";
        private const string OL_SAVE_AS = "SaveAs";
        private const string OL_SAVED = "Saved";
        private const string OL_SENSITIVITY = "Sensitivity";
        private const string OL_SESSION = "Session";
        private const string OL_SHOW_CATEGORIES_DIALOG = "ShowCategoriesDialog";
        private const string OL_SIZE = "Size";
        private const string OL_SUBJECT = "Subject";
        private const string OL_UN_READ = "UnRead";
        private const string OL_USER_PROPERTIES = "UserProperties";
        
        #endregion

        #region Constructor

        public OutlookItem(object item)
        {
            InnerObject = item;
            _type = InnerObject.GetType();
        }

        #endregion

        #region Properties

        public Outlook.Actions Actions => GetPropertyValue(OL_ACTIONS) as Outlook.Actions;
        public Outlook.Application Application => GetPropertyValue(OL_APPLICATION) as Outlook.Application;
        public Outlook.Attachments Attachments => GetPropertyValue(OL_ATTACHMENTS) as Outlook.Attachments;
        public string BillingInformation
        {
            get => GetPropertyValue(OL_BILLING_INFORMATION).ToString();
            set => SetPropertyValue(OL_BILLING_INFORMATION, value);
        }
        public string Body
        {
            get => GetPropertyValue(OL_BODY).ToString();
            set => SetPropertyValue(OL_BODY, value);
        }
        public string Categories
        {
            get => GetPropertyValue(OL_CATEGORIES).ToString();
            set => SetPropertyValue(OL_CATEGORIES, value);
        }
        public string Companies
        {
            get => GetPropertyValue(OL_COMPANIES).ToString();
            set => SetPropertyValue(OL_COMPANIES, value);
        }
        public Outlook.OlObjectClass Class
        {
            get
            {
                if (_typeOlObjectClass != null) return (Outlook.OlObjectClass)Enum.ToObject(_typeOlObjectClass, GetPropertyValue(OL_CLASS));

                const Outlook.OlObjectClass objClass = Outlook.OlObjectClass.olAction;
                _typeOlObjectClass = objClass.GetType();
                return (Outlook.OlObjectClass)Enum.ToObject(_typeOlObjectClass, GetPropertyValue(OL_CLASS));
            }
        }
        public string ConversationIndex => GetPropertyValue(OL_CONVERSATION_INDEX).ToString();
        public string ConversationTopic => GetPropertyValue(OL_CONVERSATION_TOPIC).ToString();
        public DateTime CreationTime => (DateTime)GetPropertyValue(OL_CREATION_TIME);
        public Outlook.OlDownloadState DownloadState => (Outlook.OlDownloadState)GetPropertyValue(OL_DOWNLOAD_STATE);
        public string EntryId => GetPropertyValue(OL_ENTRY_ID).ToString();
        public Outlook.FormDescription FormDescription => (Outlook.FormDescription)GetPropertyValue(OL_FORM_DESCRIPTION);
        public object InnerObject { get; }
        public Outlook.Inspector GetInspector => GetPropertyValue(OL_GET_INSPECTOR) as Outlook.Inspector;
        public Outlook.OlImportance Importance
        {
            get => (Outlook.OlImportance)GetPropertyValue(OL_IMPORTANCE);
            set => SetPropertyValue(OL_IMPORTANCE, value);
        }
        public bool IsConflict => (bool)GetPropertyValue(OL_IS_CONFLICT);
        public Outlook.ItemProperties ItemProperties => (Outlook.ItemProperties)GetPropertyValue(OL_ITEM_PROPERTIES);
        public DateTime LastModificationTime => (DateTime)GetPropertyValue(OL_LAST_MODIFICATION_TIME);
        public Outlook.Links Links => GetPropertyValue(OL_LINKS) as Outlook.Links;
        public Outlook.OlRemoteStatus MarkForDownload
        {
            get => (Outlook.OlRemoteStatus)GetPropertyValue(OL_MARK_FOR_DOWNLOAD);
            set => SetPropertyValue(OL_MARK_FOR_DOWNLOAD, value);
        }
        public string MessageClass
        {
            get => GetPropertyValue(OL_MESSAGE_CLASS).ToString();
            set => SetPropertyValue(OL_MESSAGE_CLASS, value);
        }
        public string Mileage
        {
            get => GetPropertyValue(OL_MILEAGE).ToString();
            set => SetPropertyValue(OL_MILEAGE, value);
        }
        public bool NoAging
        {
            get => (bool) GetPropertyValue(OL_NO_AGING);
            set => SetPropertyValue(OL_NO_AGING, value);
        }
        public long OutlookInternalVersion => (long)GetPropertyValue(OL_OUTLOOK_INTERNAL_VERSION);
        public string OutlookVersion => GetPropertyValue(OL_OUTLOOK_VERSION).ToString();
        public Outlook.Folder Parent => GetPropertyValue(OL_PARENT) as Outlook.Folder;
        public Outlook.PropertyAccessor PropertyAccessor => GetPropertyValue(OL_PROPERTY_ACCESSOR) as Outlook.PropertyAccessor;
        public bool Saved => (bool)GetPropertyValue(OL_SAVED);
        public Outlook.OlSensitivity Sensitivity
        {
            get => (Outlook.OlSensitivity) GetPropertyValue(OL_SENSITIVITY);
            set => SetPropertyValue(OL_SENSITIVITY, value);
        }
        public Outlook.NameSpace Session => GetPropertyValue(OL_SESSION) as Outlook.NameSpace;
        public long Size => (long)GetPropertyValue(OL_SIZE);
        public string Subject
        {
            get => GetPropertyValue(OL_SUBJECT).ToString();
            set => SetPropertyValue(OL_SUBJECT, value);
        }
        public bool UnRead
        {
            get => (bool)GetPropertyValue(OL_UN_READ);
            set => SetPropertyValue(OL_UN_READ, value);
        }
        public Outlook.UserProperties UserProperties => GetPropertyValue(OL_USER_PROPERTIES) as Outlook.UserProperties;

        #endregion

        #region Methods

        public void Close(Outlook.OlInspectorClose saveMode)
        {
            object[] args = { saveMode };
            CallMethod(OL_CLOSE, args);
        }

        public object Copy()
        {
            return CallMethod(OL_COPY);
        }

        public void Display()
        {
            CallMethod(OL_DISPLAY);
        }

        public object Move(Outlook.Folder destinationFolder)
        {
            object[] args = { destinationFolder };
            return CallMethod(OL_MOVE, args);
        }

        public void PrintOut()
        {
            CallMethod(OL_PRINT_OUT);
        }

        public void Save()
        {
            CallMethod(OL_SAVE);
        }

        public void SaveAs(string path, Outlook.OlSaveAsType type)
        {
            object[] args = { path, type };
            CallMethod(OL_SAVE_AS, args);
        }

        public void ShowCategoriesDialog()
        {
            CallMethod(OL_SHOW_CATEGORIES_DIALOG);
        }

        #endregion

        #region Helper Functions

        private object GetPropertyValue(string propertyName)
        {
            try
            {
                return _type.InvokeMember(propertyName,BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty,null, InnerObject, Array.Empty<object>());
            }
            catch (SystemException ex)
            {
                Debug.WriteLine($"OutlookItem: GetPropertyValue for {propertyName} Exception: {ex.Message} ");
                throw;
            }
        }

        private void SetPropertyValue(string propertyName, object propertyValue)
        {
            try
            {
                _type.InvokeMember(propertyName,BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty,null, InnerObject,new[] { propertyValue });
            }
            catch (SystemException ex)
            {
                Debug.WriteLine($"OutlookItem: SetPropertyValue for {propertyName} Exception: {ex.Message} ");
                throw;
            }
        }

        private object CallMethod(string methodName)
        {
            try
            {
                return _type.InvokeMember(methodName,BindingFlags.Public | BindingFlags.InvokeMethod, null, InnerObject, Array.Empty<object>());
            }
            catch (SystemException ex)
            {
                Debug.WriteLine($"OutlookItem: CallMethod for {methodName} Exception: {ex.Message} ");
                throw;
            }
        }

        private object CallMethod(string methodName, object[] args)
        {
            try
            {
                return _type.InvokeMember(methodName,BindingFlags.Public | BindingFlags.InvokeMethod,null, InnerObject, args);
            }
            catch (SystemException ex)
            {
                Debug.WriteLine( $"OutlookItem: CallMethod for {methodName} Exception: {ex.Message} ");
                throw;
            }
        }

        #endregion
    }
}