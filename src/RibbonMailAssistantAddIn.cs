using Jpp.AddIn.MailAssistant.Factories;
using Jpp.AddIn.MailAssistant.Properties;
using Jpp.AddIn.MailAssistant.Wrappers;
using Microsoft.AppCenter.Crashes;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant
{
    [ComVisible(true)]
    public class RibbonMailAssistantAddIn : Office.IRibbonExtensibility
    {
        private string projectValue = "";

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
                CopyAttachments(selection);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                //TODO: need to info user. Cannot rethrow as will be swallowed up by Outlook.
            }
        }

        public bool GetVisible_MoveToShared(Office.IRibbonControl control)
        {
            var selection = GetItemSelection(control);
            return selection != null && selection.Count >= 1;
        }

        public void OnAction_MoveToShared(Office.IRibbonControl control)
        {
            try
            {
                var selection = GetItemSelection(control);
                MoveSelection(selection);
            }
            catch (Exception e)
            {
                Crashes.TrackError(e);
                //TODO: need to info user. Cannot rethrow as will be swallowed up by Outlook.
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

        public void OnChange_Project(Office.IRibbonControl control, string text)
        {
            projectValue = text;
        }

        public void OnAction_GoToProject(Office.IRibbonControl control)
        {
            NavigateToProjectFolder(projectValue);
        }

        #endregion

        #region Helpers

        private void NavigateToProjectFolder(string project)
        {
            var explorer = Globals.ThisAddIn.Application.ActiveExplorer();
            
            using (var wrappedSharedFolder = OutlookFolderFactory.GetOrCreateSharedFolder(Globals.ThisAddIn.Application, project))
            {
                wrappedSharedFolder.NavigateToFolder(explorer);
            }
        }

        private void MoveSelection(Outlook.Selection selection)
        {
            using (var wrappedSelection = new SelectionWrapper(selection))
            using (var wrappedSharedFolder = OutlookFolderFactory.GetOrCreateSharedFolder(Globals.ThisAddIn.Application))
            {
                wrappedSharedFolder.MoveIntoFolder(wrappedSelection);
            }
        }

        private void NewSharedFolder()
        {
            using var folder = OutlookFolderFactory.GetOrCreateSharedFolder(Globals.ThisAddIn.Application);
            {
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
                _ => throw new NotImplementedException()
            };

        private Outlook.Selection GetItemSelection(Office.IRibbonControl control) => control.Context switch
            {
                Outlook.Selection context => context,
                Outlook.Explorer explorer => explorer.Selection,
                _ => throw new NotImplementedException()
            };

        #endregion
    }
}
