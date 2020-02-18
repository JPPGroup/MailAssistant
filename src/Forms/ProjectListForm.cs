using Jpp.AddIn.MailAssistant.Projects;
using Jpp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Jpp.AddIn.MailAssistant.Forms
{
    public partial class ProjectListForm : Form
    {
        private readonly ProjectService _projectService;
        private IEnumerable<Project> _projectList;
        private string _searchText;

        public string SelectedPath
        {
            get
            {
                if (gridProjects.SelectedRows.Count != 1) return null;
                var item = gridProjects.SelectedRows[0];

                return item.Cells[nameof(Project.SharedMailPath)].Value.ToString();
            }
        }

        public string SelectedFolder
        {
            get
            {
                if (gridProjects.SelectedRows.Count != 1) return null;
                var item = gridProjects.SelectedRows[0];

                return item.Cells[nameof(Project.Folder)].Value.ToString();
            }
        }

        public ProjectListForm(ProjectService service)
        {
            InitializeComponent();
            _projectService = service;
        }

        private void _projectService_ProjectListChanged(object sender, EventArgs e)
        {
            LoadProjects();
        }

        private void ProjectListForm_Load(object sender, EventArgs e)
        {
            _projectService.ProjectListChanged += _projectService_ProjectListChanged;
            LoadProjects();
        }

        private void ProjectListForm_Closed(object sender, EventArgs e)
        {
            _projectService.ProjectListChanged -= _projectService_ProjectListChanged;
        }

        private void LoadProjects()
        {
            var result = _projectService.GetProjects();
            _projectList = result.OrderByDescending(p => p.Code, new ProjectCodeComparer());
            PopulateGrid(txtSearchBox.Text);
            ActiveControl = txtSearchBox;
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
            using (var column = gridProjects.Columns[nameof(Project.Code)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 0;
                    column.Width = 100;
                }
            }

            using (var column = gridProjects.Columns[nameof(Project.Name)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 1;
                    column.Width = 350;
                }
            }

            using (var column = gridProjects.Columns[nameof(Project.Discipline)])
            {
                if (column != null)
                {
                    column.Visible = true;
                    column.DisplayIndex = 2;
                    column.Width = 150;
                }
            }

            using (var column = gridProjects.Columns[nameof(Project.Grouping)])
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

        private void gridProjects_SelectionChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = gridProjects.SelectedRows.Count == 1;
        }
    }
}
