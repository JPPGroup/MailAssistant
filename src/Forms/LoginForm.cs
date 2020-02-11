using Jpp.Common.Backend.Auth;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System;
using System.Windows.Forms;

namespace Jpp.AddIn.MailAssistant.Forms
{
    public partial class LoginForm : Form
    {
        private readonly BaseOAuthAuthentication _authentication;

        public LoginForm(BaseOAuthAuthentication authentication, string initialUrl)
        {
            _authentication = authentication;

            InitializeComponent();

            webView.Source = new Uri(initialUrl);
            webView.NavigationStarting += WebView_NavigationStarting;
        }

        private async void WebView_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            if (await _authentication.EvaluateURL(e.Uri)) Close();
        }
    }
}
