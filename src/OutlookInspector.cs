using System;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant
{

    /// <summary>
    /// This class tracks the state of an Outlook Inspector window for your
    /// add-in and ensures that what happens in this window is handled correctly.
    /// </summary>
    internal class OutlookInspector
    {
        #region Events

        public event EventHandler Close;
        public event EventHandler<InvalidateEventArgs> InvalidateControl;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of the tracking class for a particular 
        /// inspector and custom task pane.
        /// </summary>
        /// <param name="inspector">A new inspector window to track</param>
        ///<remarks></remarks>
        public OutlookInspector(Outlook.Inspector inspector)
        {
            Window = inspector;

            // Hookup the close event
            ((Outlook.InspectorEvents_Event)inspector).Close += OutlookInspectorWindow_Close;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event Handler for the inspector close event.
        /// </summary>
        private void OutlookInspectorWindow_Close()
        {
            // Unhook events from the window
            ((Outlook.InspectorEvents_Event)Window).Close -= OutlookInspectorWindow_Close;

            // Raise the OutlookInspector close event
            Close?.Invoke(this, EventArgs.Empty);

            // Unhook any item-level instance variables
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
        /// The actual Outlook inspector window wrapped by this instance
        /// </summary>
        internal Outlook.Inspector Window { get; private set; }

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
