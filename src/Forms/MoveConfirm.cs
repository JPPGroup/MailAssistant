using System;
using System.Windows.Forms;

namespace Jpp.AddIn.MailAssistant.Forms
{
    public partial class MoveConfirm : Form
    {
        public MoveConfirm()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (Remember.Checked)
            {
                UserSettings.SnoozeDialogUntil(DateTime.Now + new TimeSpan(30, 0, 0, 0), true);
            }

            this.Close();
        }

        private void IgnoreButton_Click(object sender, EventArgs e)
        {
            if (Remember.Checked)
            {
                UserSettings.SnoozeDialogUntil(DateTime.Now + new TimeSpan(30, 0, 0, 0), false);
            }
            this.Close();
        }
    }
}
