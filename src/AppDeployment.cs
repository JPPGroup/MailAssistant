using System;
using System.Deployment.Application;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace Jpp.AddIn.MailAssistant
{
    public class AppDeploymentCheck : IDisposable
    {
        private const int CHECK_INTERVAL_MINUTES = 60;
        private Timer _timer;

        public AppDeploymentCheck()
        {
            _timer = new Timer(TimeSpan.FromMinutes(CHECK_INTERVAL_MINUTES).TotalMilliseconds);
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            UpdateAvailable();
        }

        private static void UpdateAvailable()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return;

            var deployment = ApplicationDeployment.CurrentDeployment;
            var deploymentFullName = deployment.UpdatedApplicationFullName;
            var appId = new ApplicationIdentity(deploymentFullName);
            var everything = new PermissionSet(PermissionState.Unrestricted);

            var trust = new ApplicationTrust(appId)
            {
                DefaultGrantSet = new PolicyStatement(everything),
                IsApplicationTrustedToRun = true,
                Persist = true
            };

            ApplicationSecurityManager.UserApplicationTrusts.Add(trust);

            if (!deployment.CheckForUpdate()) return;

            var updateInfo = deployment.CheckForDetailedUpdate();
            MessageBox.Show($@"Update available: {updateInfo.AvailableVersion}. Please restart Outlook.",@"Mail Assistant",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        }

        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                _timer.Dispose();
                _timer = null;
            }

            // TODO: dispose unmanaged objects.

            _disposedValue = true;
        }

        ~AppDeploymentCheck()
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
