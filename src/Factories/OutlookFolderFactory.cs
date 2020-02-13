using Jpp.AddIn.MailAssistant.Forms;
using Jpp.AddIn.MailAssistant.Wrappers;
using System;
using System.Linq;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Factories
{
    internal static class OutlookFolderFactory
    {
        public static FolderWrapper GetOrCreateSharedFolder(Outlook.Application outlookApplication)
        {
            if(outlookApplication == null) throw new ArgumentNullException(nameof(outlookApplication));

            using var frm = new ProjectListForm(ThisAddIn.Authentication, ThisAddIn.StorageProvider);
            var result = frm.ShowDialog();

            return result != DialogResult.OK ? null : GetSharedFolder(outlookApplication, frm.SelectedFolder);
        }

        private static FolderWrapper GetSharedFolder(Outlook.Application outlookApplication, string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) throw new ArgumentNullException(nameof(folderPath), @"Folder name not set.");

            var namespaceFolders = outlookApplication.GetNamespace(Constants.NAMESPACE_TYPE).Folders;

            var sharedFolder = namespaceFolders.Cast<Outlook.Folder>().FirstOrDefault(f => f.Name == Constants.BASE_SHARED_FOLDER_NAME);
            if (sharedFolder == null) throw new ArgumentNullException(nameof(sharedFolder), @"Base shared folder not set.");

            var arrFolders = folderPath.Split('\\');
            var folder = new FolderWrapper(sharedFolder);

            for (var i = 0; i <= arrFolders.GetUpperBound(0); i++)
            {
                if (string.IsNullOrWhiteSpace(arrFolders[i]) || folder.Name == arrFolders[i]) continue;
                folder = folder.GetOrCreateSubFolder(arrFolders[i]);
            }

            if (folder.Name != arrFolders.Last()) folder.Rename(arrFolders.Last());

            return folder;
        }
    }
}
