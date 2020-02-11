namespace Jpp.AddIn.MailAssistant
{
    public static class Constants
    {
        public const string PR_INTERNET_MESSAGE_ID = "http://schemas.microsoft.com/mapi/proptag/0x1035001E";
        public const string PR_SMTP_ADDRESS = "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
        public const string PR_SENDER_EMAIL_ADDRESS = "http://schemas.microsoft.com/mapi/proptag/0x0C1F001E";

        public const string BASE_SHARED_FOLDER_NAME = "JPP_Shared";
        public const string NAMESPACE_TYPE = "MAPI";
        public const string SEARCH_DATE_FORMAT = "dd MMMM yyyy h:mm tt";
        public const int SEARCH_WINDOW_MINUTES = 2;
    }

    public enum RagStatus {  Red, Amber, Green }
    public enum ItemStatus { Duplicate, Moved, Failed, Skipped }
}
