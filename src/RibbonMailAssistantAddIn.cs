using Jpp.AddIn.MailAssistant.Factories;
using Jpp.AddIn.MailAssistant.Wrappers;
using Microsoft.AppCenter.Crashes;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Jpp.AddIn.MailAssistant.Forms;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant
{
    [ComVisible(true)]
    public class RibbonMailAssistantAddIn : Office.IRibbonExtensibility
    {
        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonId)
        {
            string customUi;

            //Return the appropriate Ribbon XML for ribbonId
            switch (ribbonId)
            {
                case "Microsoft.Outlook.Explorer":
                    customUi = GetResourceText("Jpp.AddIn.MailAssistant.Ribbons.Explorer.xml");
                    return customUi;
                default:
                    return string.Empty;
            }
        }

        #endregion

        #region Ribbon Callbacks

        public void OnLoad_Ribbon(Office.IRibbonUI ribbonUi)
        {
            ThisAddIn.Ribbon = ribbonUi;
        }

        
        public void OnAction_SendToHub(Office.IRibbonControl control)
        {
            try
            {
                var selection = GetAttachmentSelection(control);
                if (selection != null && selection.Count >= 1)
                {
                    CopyAttachments(selection);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                //TODO: need to info user. Cannot rethrow as will be swallowed up by Outlook.
            }
        }

        public void OnAction_MoveToShared(Office.IRibbonControl control)
        {
            try
            {
                var selection = GetItemSelection(control);
                if (selection != null && selection.Count >= 1)
                {
                    MoveSelection(selection);
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e); //TODO: need to info user. Cannot rethrow as will be swallowed up by Outlook.
            }
        }

        public void OnAction_NewFolder(Office.IRibbonControl control) //IRibbonControl need for callback signature but unused
        {
            try
            {
                NewSharedFolder();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                //TODO: need to info user. Cannot rethrow as will be swallowed up by Outlook.
            }
        }


        public void OnAction_GoToFolder(Office.IRibbonControl control)
        {

            try
            {
                NavigateToSharedFolder();
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                //TODO: need to info user. Cannot rethrow as will be swallowed up by Outlook.
            }
        }

        public void OnAction_RenameSharedFolder(Office.IRibbonControl control)
        {

            try
            {
                var folder = control.Context as Outlook.Folder;

                using var frm = new ProjectListForm(ThisAddIn.ProjectService);
                var result = frm.ShowDialog();
                if (result == DialogResult.OK && folder != null)
                {
                    folder.Name = frm.SelectedFolder;
                }
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                //TODO: need to info user. Cannot rethrow as will be swallowed up by Outlook.
            }
        }

        #endregion

        #region Helpers

        private void NavigateToSharedFolder()
        {
            var explorer = Globals.ThisAddIn.Application.ActiveExplorer();

            using var wrappedSharedFolder = OutlookFolderFactory.GetOrCreateSharedFolder(Globals.ThisAddIn.Application);
            wrappedSharedFolder?.NavigateToFolder(explorer);
        }

        private void MoveSelection(Outlook.Selection selection)
        {
            if (selection.Count >= 50)
            {
                MessageBox.Show($@"Exceeded maximum number of moveable items.{Environment.NewLine}Please select less than 50 items.", 
                    @"Mail Assistant", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return;
            }

            using var wrappedSelection = new SelectionWrapper(selection);
            using var wrappedSharedFolder = OutlookFolderFactory.GetOrCreateSharedFolder(Globals.ThisAddIn.Application);
            wrappedSharedFolder?.MoveIntoFolder(wrappedSelection);
        }

        private void NewSharedFolder()
        {
            using var folder = OutlookFolderFactory.GetOrCreateSharedFolder(Globals.ThisAddIn.Application);
            {
                if (folder == null)
                {
                    return;
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.Append($"Successfully created folder: \n{folder.Name}.");

                MessageBox.Show(stringBuilder.ToString(), @"Mail Assistant", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static void CopyAttachments(Outlook.AttachmentSelection selection)
        {
            MessageBox.Show(@"Not implemented", @"Mail Assistant", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string GetResourceText(string resourceName)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceNames = asm.GetManifestResourceNames();
            foreach (var name in resourceNames)
            {
                if (string.Compare(resourceName, name, StringComparison.OrdinalIgnoreCase) != 0) continue;

                var stream = asm.GetManifestResourceStream(name);
                if (stream == null) continue;

                using var resourceReader = new StreamReader(stream);
                return resourceReader.ReadToEnd();
            }
            return null;
        }

        private Outlook.AttachmentSelection GetAttachmentSelection(Office.IRibbonControl control) => control.Context switch
            {
                Outlook.AttachmentSelection context => context,
                Outlook.Explorer explorer => explorer.AttachmentSelection,
                _ => null
            };

        private Outlook.Selection GetItemSelection(Office.IRibbonControl control) => control.Context switch
            {
                Outlook.Selection context => context,
                Outlook.Explorer explorer => explorer.Selection,
                _ => null
            };

        #endregion
    }
}
