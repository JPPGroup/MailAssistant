using Jpp.AddIn.MailAssistant.Abstracts;
using Jpp.AddIn.MailAssistant.Exceptions;
using Jpp.AddIn.MailAssistant.Factories;
using Jpp.AddIn.MailAssistant.Forms;
using Jpp.AddIn.MailAssistant.OutputReports;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant.Wrappers
{
    internal class FolderWrapper : IWrappedObject
    {
        private readonly Outlook.Folder _innerObject;

        public Type InnerObjectType => typeof(Outlook.Folder);
        public string Name => _innerObject.Name;
        public Outlook.Items Items => _innerObject.Items;

        public FolderWrapper(Outlook.Folder folder)
        {
            _innerObject = folder ?? throw new ArgumentNullException(nameof(folder));
        }

        public void Rename(string name)
        {
            _innerObject.Name = name;
        }

        public FolderWrapper GetOrCreateSubFolder(string folderName)
        {
            var subFolder = GetSubFolder(folderName) ?? CreateSubFolder(folderName);
            return new FolderWrapper(subFolder);
        }

        public void NavigateToFolder(Outlook.Explorer explorer)
        {
            explorer.CurrentFolder = _innerObject;
        }

        public void MoveIntoFolder(SelectionWrapper selection)
        {
            var frm = new ProgressForm { progressBar = { Maximum = selection.Count }, Text = $@"Moving '{Name}'..." };

            MoveReport outcome = null;

            frm.Shown += async delegate
            {
                try
                {
                    var progress = new Progress<int>(i =>
                    {
                        frm.progressBar.Value = i;
                        frm.lblProgress.Text = $@"{frm.progressBar.Value} of {frm.progressBar.Maximum}";
                    });

                    outcome = await MoveSelectionIntoFolderAsync(selection, progress);
                }
                catch (Exception e)
                {
                    Crashes.TrackError(e);
                    throw;
                }
                finally
                {
                    frm.Close();
                }
            };

            frm.ShowDialog();

            if (outcome == null) throw new ArgumentNullException(nameof(outcome));
            outcome.LogAndShowResults();
        }

        private static bool CheckFolderForCode(string folderName, string matchName)
        {
            const string find = "-";

            var charFolderLoc = folderName.IndexOf(find, StringComparison.Ordinal);
            var charMatchLoc = matchName.IndexOf(find, StringComparison.Ordinal);

            if (charFolderLoc <= 0 || charMatchLoc <= 0) return false;
            if (charFolderLoc != charMatchLoc) return false;

            return folderName.Substring(0, charFolderLoc) == matchName.Substring(0, charMatchLoc);
        }

        private Outlook.Folder GetSubFolder(string folderName)
        {
            return GetSubFolderViaName(folderName) ?? GetSubFolderViaCode(folderName);
        }

        private Outlook.Folder GetSubFolderViaCode(string folderName)
        {
            return _innerObject.Folders.Cast<Outlook.Folder>().FirstOrDefault(folder => CheckFolderForCode(folder.Name, folderName));
        }

        // Folder name index is fastest method for getting folder. But no graceful way to handle if folder isn't folder.
        private Outlook.Folder GetSubFolderViaName(string folderName)
        {
            try
            {
                var folder = _innerObject.Folders[folderName];
                return (Outlook.Folder)folder;
            }
            catch (COMException ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    { "Parent", Name },
                    { "Folder", folderName }
                });

                return null;
            }
        }

        private Outlook.Folder CreateSubFolder(string folderName)
        {
            var folder = (Outlook.Folder) _innerObject.Folders.Add(folderName, Outlook.OlDefaultFolders.olFolderInbox);

            Analytics.TrackEvent("New folder created", new Dictionary<string, string> {
                { "Parent", Name },
                { "Folder", folderName }
            });

            return folder;
        }

        private Task<MoveReport> MoveSelectionIntoFolderAsync(SelectionWrapper selection, IProgress<int> progress)
        {
            return Task.Run(() =>
            {
                var outcome = new MoveReport(this, selection);
                bool? autoDelete =  null;
                if (UserSettings.IsDialogSnoozed())
                {
                    autoDelete = UserSettings.IsAutoDelete();
                }
                bool? oneDelete = null;

                for (var i = 1; i <= selection.Count; i++) // Fine to move forward through selection, as collection doesn't change on move of item.
                {
                    progress.Report(i);

                    try
                    {
                        using (IOutlookItem outlookItem = OutlookItemFactory.Create(selection[i]))
                        {
                            var itemProps = new ItemProperties(outlookItem.Description, outlookItem.Folder, Name);

                            if (outlookItem is IMoveable moveableItem)
                            {
                                if (IsItemPresent(moveableItem))
                                {
                                    itemProps.Status = ItemStatus.Duplicate;

                                    if (oneDelete == null && !autoDelete.HasValue)
                                    {
                                        MoveConfirm moveConfirm = new MoveConfirm();
                                        if (moveConfirm.ShowDialog() == DialogResult.Yes)
                                        {
                                            oneDelete = true;
                                        }
                                        else
                                        {
                                            oneDelete = false;
                                        }
                                    }
                                    
                                    if ((autoDelete.HasValue && autoDelete.Value) || (oneDelete.HasValue && oneDelete.Value))
                                    {
                                        moveableItem.Delete();
                                    }

                                    /*if (UserSettings.IsDialogSnoozed())
                                    {
                                        moveableItem.Delete();
                                        itemProps.Status = ItemStatus.DuplicateDeleted;
                                    }*/
                                }
                                
                                else itemProps.Status = moveableItem.Move(_innerObject) ? ItemStatus.Moved : ItemStatus.Failed;
                            }
                            else
                            {
                                itemProps.Status = ItemStatus.Skipped;
                            }

                            outcome.AddAndTrackItem(itemProps);
                        }
                    }
                    catch (OutlookItemFactoryException e) //Log factory exception and move to next item.
                    {
                        outcome.Error++;
                        Crashes.TrackError(e);
                    }
                    catch (COMException comEx) //Something bad happened, lets stop.
                    {
                        outcome.Error++;
                        Crashes.TrackError(comEx);
                        break;
                    }
                }

                outcome.CompletedMove = true;
                return outcome;
            });
        }

        private bool IsItemPresent(IMoveable moveableItem)
        {
            Outlook.Items restrictedItems = null;

            try
            {
                restrictedItems = Items.Restrict(moveableItem.RestrictCriteria);

                foreach (var item in restrictedItems)
                {
                    try
                    {
                        using (IOutlookItem resultItem = OutlookItemFactory.Create(item))
                        {
                            if (!(resultItem is IMoveable resultMoveable)) continue;

                            if (resultMoveable.Equals(moveableItem)) return true;
                        }
                    }
                    catch (OutlookItemFactoryException e) //Log factory exception and move to next item
                    {
                        Crashes.TrackError(e);
                    }
                }

                return false;
            }
            finally
            {
                if (restrictedItems != null) Marshal.ReleaseComObject(restrictedItems);
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

        ~FolderWrapper()
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
