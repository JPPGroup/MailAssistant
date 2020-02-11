using Jpp.AddIn.MailAssistant.OutputReports;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Jpp.AddIn.MailAssistant.Forms
{
    internal partial class MoveReportForm : Form
    {
        private readonly MoveReport _report;

        public MoveReportForm(MoveReport report)
        {
            _report = report;
            InitializeComponent();

            PopulateGrid();
            SetStatusPanel();
            SetLabelDetails();
        }

        private void SetLabelDetails()
        {
            txtTarget.Text = _report.Target.ToString();

            txtMoved.Text = _report.Moved.ToString();

            txtSkipped.Text = _report.Skipped.ToString();
            if (_report.Skipped > 0) txtSkipped.BackColor = Color.Orange;

            txtFailed.Text = _report.Failed.ToString();
            if (_report.Failed > 0) txtFailed.BackColor = Color.Red;

            txtDuplicate.Text = _report.Duplicate.ToString();
            if (_report.Duplicate > 0) txtDuplicate.BackColor = Color.Orange;

            txtError.Text = _report.Error.ToString();
            if (_report.Error > 0) txtError.BackColor = Color.Red;
        }

        private void SetStatusPanel()
        {
            switch (_report.OverallStatus)
            {
                case RagStatus.Red:
                    panelStatus.BackColor = Color.Red;
                    break;
                case RagStatus.Amber:
                    panelStatus.BackColor = Color.Orange;
                    break;
                case RagStatus.Green:
                    panelStatus.BackColor = Color.Green;
                    break;
            }
        }

        private void PopulateGrid()
        {
            gridItems.DataSource = _report.Items.ToList();
            gridItems.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);

            SetColumns();
        }

        private void SetColumns()
        {
            using (var column = gridItems.Columns[nameof(ItemProperties.Description)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 0;
                    column.Width = 450;
                }
            }

            using (var column = gridItems.Columns[nameof(ItemProperties.Source)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 1;
                    column.Width = 150;
                }
            }

            using (var column = gridItems.Columns[nameof(ItemProperties.Destination)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 2;
                    column.Width = 150;
                }
            }

            using (var column = gridItems.Columns[nameof(ItemProperties.Status)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 3;
                    column.Width = 150;
                }
            }
        }

        private void BtnClose_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
