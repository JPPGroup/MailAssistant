using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Threading;
using Jpp.AddIn.MailAssistant.Projects;
using Jpp.AddIn.MailAssistant.Wrappers;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace Jpp.AddIn.MailAssistant
{
    public partial class ThisAddIn
    {
        #region Instance Variables

        private static ProjectService _projectService;
        private static readonly object _padlock = new object();

        private Outlook.Explorers _explorers;
        private Outlook.Inspectors _inspectors;

        internal static List<OutlookExplorer> Windows;  // List of tracked explorer windows  
        internal static List<OutlookInspector> InspectorWindows; // List of tracked inspector windows         
        internal static Office.IRibbonUI Ribbon; // Ribbon UI reference

        internal static ProjectService ProjectService
        {
            get
            {
                if (_projectService == null)
                {
                    lock (_padlock)
                    {
                        if (_projectService == null)
                        {
                            _projectService = new ProjectService();
                        }
                    }
                }
                return _projectService;
            }
        }

        #endregion

        #region VSTO Startup and Shutdown methods

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            var setupThread = new Thread(Setup);
            setupThread.SetApartmentState(ApartmentState.STA);
            setupThread.Start();

            // Initialize variables
            _explorers = Application.Explorers;
            _inspectors = Application.Inspectors;

            Windows = new List<OutlookExplorer>();
            InspectorWindows = new List<OutlookInspector>();

            // Wire up event handlers to handle multiple Explorer & Inspector windows
            _explorers.NewExplorer += OutlookEvent_Explorers_NewExplorer;
            _inspectors.NewInspector += OutlookEvent__Inspectors_NewInspector;

            // Add the ActiveExplorer to Windows
            var explorer = Application.ActiveExplorer();
            var window = new OutlookExplorer(explorer);
            Windows.Add(window);

            // Hook up event handlers for window
            window.Close += WrappedWindow_Close;
            window.InvalidateControl += WrappedWindow_InvalidateControl;
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            // Unhook event handlers
            _explorers.NewExplorer -= OutlookEvent_Explorers_NewExplorer;
            _inspectors.NewInspector -= OutlookEvent__Inspectors_NewInspector;

            // Dereference objects
            _explorers = null;
            _inspectors = null;
            _projectService = null;
            Windows.Clear();
            Windows = null;
            InspectorWindows.Clear();
            InspectorWindows = null;
            Ribbon = null;
        }

        protected override Office.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new RibbonMailAssistantAddIn();
        }

        private void Setup()
        {
            AppCenter.Start("85ffea91-fbef-4cdf-9e69-ac7c15e3a683", typeof(Analytics), typeof(Crashes));
            //TODO: check if these are to be set before or after start.
            Analytics.SetEnabledAsync(true);
            Crashes.SetEnabledAsync(true);

            using var user = new AddressEntryWrapper(Application.Session.CurrentUser.AddressEntry);
            AppCenter.SetUserId(user.Address);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Looks up the window wrapper for a given window object
        /// </summary>
        /// <param name="window">An outlook explorer window</param>
        /// <returns></returns>
        internal static OutlookExplorer FindOutlookExplorer(object window)
        {
            foreach (var explorer in Windows)
            {
                if (explorer.Window == window) return explorer;
            }

            return null;
        }

        /// <summary>
        /// Looks up the window wrapper for a given window object
        /// </summary>
        /// <param name="window">An outlook inspector window</param>
        /// <returns></returns>
        internal static OutlookInspector FindOutlookInspector(object window)
        {
            foreach (var inspector in InspectorWindows)
            {
                if (inspector.Window == window) return inspector;
            }

            return null;
        }
        
        #endregion

        #region Event Handlers

        private static void OutlookEvent_Explorers_NewExplorer(Outlook.Explorer explorer)
        {
            // Check to see if this is a new window we don't already track
            var existingWindow = FindOutlookExplorer(explorer);

            // If the collection has a window for this Explorer then return, otherwise we should add it
            if (existingWindow != null) return;

            var window = new OutlookExplorer(explorer);
            window.Close += WrappedWindow_Close;
            window.InvalidateControl += WrappedWindow_InvalidateControl;
            Windows.Add(window);
        }

        private static void OutlookEvent__Inspectors_NewInspector(Outlook.Inspector inspector)
        {
            Ribbon.Invalidate();

            // Check to see if this is a new window we don't already track
            var existingInspector = FindOutlookInspector(inspector);
            
            // If the collection has a window for this Inspector then return, otherwise we should add it
            if (existingInspector != null) return;

            var window = new OutlookInspector(inspector);
            window.Close += WrappedInspectorWindow_Close;
            window.InvalidateControl += WrappedInspectorWindow_InvalidateControl;
            InspectorWindows.Add(window);
        }

        private static void WrappedInspectorWindow_InvalidateControl(object sender, OutlookInspector.InvalidateEventArgs e)
        {
            Ribbon?.InvalidateControl(e.ControlId);
        }

        private static void WrappedInspectorWindow_Close(object sender, EventArgs e)
        {
            var window = (OutlookInspector)sender;
            window.Close -= WrappedInspectorWindow_Close;
            InspectorWindows.Remove(window);
        }

        private static void WrappedWindow_InvalidateControl(object sender, OutlookExplorer.InvalidateEventArgs e)
        {
            Ribbon?.InvalidateControl(e.ControlId);
        }

        private static void WrappedWindow_Close(object sender, EventArgs e)
        {
            var window = (OutlookExplorer)sender;
            window.Close -= WrappedWindow_Close;
            Windows.Remove(window);
        }

        #endregion

        #region VSTO generated code

        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            Shutdown += ThisAddIn_Shutdown;
        }

        #endregion
    }
}
