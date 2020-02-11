using System;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant
{
    /// <summary>
    /// This class tracks the state of an Outlook Explorer window for your
    /// add-in and ensures that what happens in this window is handled correctly.
    /// </summary>
    internal class OutlookExplorer
    {
        #region Events

        public event EventHandler Close;
        public event EventHandler<InvalidateEventArgs> InvalidateControl;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of the tracking class for a particular explorer 
        /// </summary>
        /// <param name="explorer">A new explorer window to track</param>
        ///<remarks></remarks>
        public OutlookExplorer(Outlook.Explorer explorer)
        {
            Window = explorer;

            // Hookup Close event
            ((Outlook.ExplorerEvents_Event)explorer).Close += OutlookExplorerWindow_Close;

            // Hookup SelectionChange event
            Window.SelectionChange += Window_SelectionChange;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for SelectionChange event
        /// </summary>
        private void Window_SelectionChange()
        {
            RaiseInvalidateControl("MoveToShared");
            RaiseInvalidateControl("MoveFolderToSharedFolder");
            RaiseInvalidateControl("MoveToSharedContextMenuMailItem");
            RaiseInvalidateControl("MoveToSharedContextMenuMultipleItems");
        }

        /// <summary>
        /// Event Handler for Close event.
        /// </summary>
        private void OutlookExplorerWindow_Close()
        {
            // Unhook explorer-level events
            Window.SelectionChange -= Window_SelectionChange;

            ((Outlook.ExplorerEvents_Event)Window).Close -= OutlookExplorerWindow_Close;

            // Raise the OutlookExplorer close event
            Close?.Invoke(this, EventArgs.Empty);

            Window = null;
        }

        #endregion

        #region Methods

        private void RaiseInvalidateControl(string controlId)
        {
            InvalidateControl?.Invoke(this, new InvalidateEventArgs(controlId));
        }

        #endregion

        #region Properties

        /// <summary>
        /// The actual Outlook explorer window wrapped by this instance
        /// </summary>
        internal Outlook.Explorer Window { get; private set; }
        
        #endregion

        #region Helper Class

        public class InvalidateEventArgs : EventArgs
        {
            public InvalidateEventArgs(string controlId)
            {
                ControlId = controlId;
            }

            public string ControlId { get; }
        }

        #endregion
    }
}
