using Jpp.AddIn.MailAssistant.Forms;
using Jpp.Common.Backend;
using Jpp.Common.Backend.Auth;

namespace Jpp.AddIn.MailAssistant.Backend
{
    internal class OfficeAddInOAuth : BaseOAuthAuthentication
    {
        public OfficeAddInOAuth(IMessageProvider messenger) : base(messenger) { }

        public override void AuthenticationPrompt()
        {
            using var frm = new LoginForm(this, GetAuthenticationURL());
            frm.ShowDialog();
        }

        public override void ExpirePrompt()
        {
            using var frm = new LoginForm(this, GetExpireURL());
            frm.ShowDialog();
        }
    }
}