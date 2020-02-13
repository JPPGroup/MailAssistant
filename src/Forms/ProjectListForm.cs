using Jpp.Common;
using Jpp.Common.Backend;
using Jpp.Common.Backend.Auth;
using Jpp.Common.Backend.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpp.AddIn.MailAssistant.Forms
{
    public partial class ProjectListForm : Form
    {
        private readonly BaseOAuthAuthentication _authentication;
        private readonly Projects _projectService;
        private IEnumerable<ProjectModel> _projectList;
        private string _searchText;

        public string SelectedFolder
        {
            get
            {
                if (gridProjects.SelectedRows.Count != 1) return null;
                var item = gridProjects.SelectedRows[0];

                var group = item.Cells[nameof(ProjectModel.Grouping)].Value;
                var code = item.Cells[nameof(ProjectModel.Code)].Value;
                var name = item.Cells[nameof(ProjectModel.Name)].Value;

                return $"Testing\\{group}\\{code}-{name}";
            }
        }

        public ProjectListForm(BaseOAuthAuthentication authentication, IStorageProvider storage)
        {
            InitializeComponent();

            _authentication = authentication;
            _projectService = new Projects(_authentication, storage);

            ThisAddIn.MessageProvider.ErrorOccurred += MessageProvider_OnErrorOccurred;
        }

        private void MessageProvider_OnErrorOccurred(object sender, EventArgs e)
        {
            Close();
        }

        private async void ProjectListForm_Load(object sender, EventArgs e)
        {
            if (!_authentication.Authenticated)
            {
                await _authentication.Authenticate();
            }

            if (_authentication.Authenticated)
            {
                await LoadProjectList();
                ActiveControl = txtSearchBox;
            }
            else
            {
                MessageBox.Show(@"Not authenticated, please login.", @"Mail Assistant", MessageBoxButtons.OK,MessageBoxIcon.Error);
                Close();
            }
        }

        private async Task LoadProjectList()
        {
            var result = await _projectService.GetAllProjects();
            _projectList = result.OrderByDescending(p => p.Code, new ProjectCodeComparer());
            PopulateGrid();
        }

        private void PopulateGrid(string searchText = "")
        {
            if (_searchText == searchText) return;

            var projects = !string.IsNullOrEmpty(searchText)
                ? _projectList.Where(project => project.Code.ToLower().Contains(searchText.ToLower()) || project.Name.ToLower().Contains(searchText.ToLower()))
                : _projectList;

            gridProjects.DataSource = projects.ToList();
            gridProjects.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);

            SetColumns();

            _searchText = searchText;
        }

        private void SetColumns()
        {
            using (var column = gridProjects.Columns[nameof(ProjectModel.Code)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 0;
                    column.Width = 100;
                }
            }

            using (var column = gridProjects.Columns[nameof(ProjectModel.Name)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 1;
                    column.Width = 350;
                }
            }

            using (var column = gridProjects.Columns[nameof(ProjectModel.Discipline)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 2;
                    column.Width = 150;
                }
            }

            using (var column = gridProjects.Columns[nameof(ProjectModel.Grouping)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 3;
                    column.Width = 100;
                }
            }
        }

        private void TxtSearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender is TextBox textBox) PopulateGrid(textBox.Text);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = gridProjects.SelectedRows.Count == 1 ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        ~ProjectListForm()
        {
            ThisAddIn.MessageProvider.ErrorOccurred -= MessageProvider_OnErrorOccurred;
        }

        private void gridProjects_SelectionChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = gridProjects.SelectedRows.Count == 1;
        }
    }
}
