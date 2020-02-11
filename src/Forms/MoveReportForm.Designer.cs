namespace Jpp.AddIn.MailAssistant.Forms
{
    partial class MoveReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.panMain = new System.Windows.Forms.Panel();
            this.txtDuplicate = new System.Windows.Forms.TextBox();
            this.lblDuplicate = new System.Windows.Forms.Label();
            this.txtFailed = new System.Windows.Forms.TextBox();
            this.lblFailed = new System.Windows.Forms.Label();
            this.txtSkipped = new System.Windows.Forms.TextBox();
            this.lblSkipped = new System.Windows.Forms.Label();
            this.txtMoved = new System.Windows.Forms.TextBox();
            this.lblMoved = new System.Windows.Forms.Label();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.lblTarget = new System.Windows.Forms.Label();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.gridItems = new System.Windows.Forms.DataGridView();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtError = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridItems)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(868, 392);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panMain
            // 
            this.panMain.BackColor = System.Drawing.Color.White;
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.txtError);
            this.panMain.Controls.Add(this.lblError);
            this.panMain.Controls.Add(this.txtDuplicate);
            this.panMain.Controls.Add(this.lblDuplicate);
            this.panMain.Controls.Add(this.txtFailed);
            this.panMain.Controls.Add(this.lblFailed);
            this.panMain.Controls.Add(this.txtSkipped);
            this.panMain.Controls.Add(this.lblSkipped);
            this.panMain.Controls.Add(this.txtMoved);
            this.panMain.Controls.Add(this.lblMoved);
            this.panMain.Controls.Add(this.txtTarget);
            this.panMain.Controls.Add(this.lblTarget);
            this.panMain.Controls.Add(this.panelStatus);
            this.panMain.Controls.Add(this.gridItems);
            this.panMain.Location = new System.Drawing.Point(12, 12);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(931, 374);
            this.panMain.TabIndex = 7;
            // 
            // txtDuplicate
            // 
            this.txtDuplicate.Enabled = false;
            this.txtDuplicate.Location = new System.Drawing.Point(569, 49);
            this.txtDuplicate.Name = "txtDuplicate";
            this.txtDuplicate.Size = new System.Drawing.Size(42, 20);
            this.txtDuplicate.TabIndex = 18;
            // 
            // lblDuplicate
            // 
            this.lblDuplicate.AutoSize = true;
            this.lblDuplicate.Location = new System.Drawing.Point(487, 52);
            this.lblDuplicate.Name = "lblDuplicate";
            this.lblDuplicate.Size = new System.Drawing.Size(58, 13);
            this.lblDuplicate.TabIndex = 17;
            this.lblDuplicate.Text = "Duplicate :";
            // 
            // txtFailed
            // 
            this.txtFailed.Enabled = false;
            this.txtFailed.Location = new System.Drawing.Point(398, 49);
            this.txtFailed.Name = "txtFailed";
            this.txtFailed.Size = new System.Drawing.Size(42, 20);
            this.txtFailed.TabIndex = 16;
            // 
            // lblFailed
            // 
            this.lblFailed.AutoSize = true;
            this.lblFailed.Location = new System.Drawing.Point(316, 52);
            this.lblFailed.Name = "lblFailed";
            this.lblFailed.Size = new System.Drawing.Size(41, 13);
            this.lblFailed.TabIndex = 15;
            this.lblFailed.Text = "Failed :";
            // 
            // txtSkipped
            // 
            this.txtSkipped.Enabled = false;
            this.txtSkipped.Location = new System.Drawing.Point(242, 49);
            this.txtSkipped.Name = "txtSkipped";
            this.txtSkipped.Size = new System.Drawing.Size(42, 20);
            this.txtSkipped.TabIndex = 14;
            // 
            // lblSkipped
            // 
            this.lblSkipped.AutoSize = true;
            this.lblSkipped.Location = new System.Drawing.Point(161, 52);
            this.lblSkipped.Name = "lblSkipped";
            this.lblSkipped.Size = new System.Drawing.Size(52, 13);
            this.lblSkipped.TabIndex = 13;
            this.lblSkipped.Text = "Skipped :";
            // 
            // txtMoved
            // 
            this.txtMoved.Enabled = false;
            this.txtMoved.Location = new System.Drawing.Point(93, 49);
            this.txtMoved.Name = "txtMoved";
            this.txtMoved.Size = new System.Drawing.Size(42, 20);
            this.txtMoved.TabIndex = 12;
            // 
            // lblMoved
            // 
            this.lblMoved.AutoSize = true;
            this.lblMoved.Location = new System.Drawing.Point(11, 52);
            this.lblMoved.Name = "lblMoved";
            this.lblMoved.Size = new System.Drawing.Size(46, 13);
            this.lblMoved.TabIndex = 11;
            this.lblMoved.Text = "Moved :";
            // 
            // txtTarget
            // 
            this.txtTarget.Enabled = false;
            this.txtTarget.Location = new System.Drawing.Point(93, 11);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.Size = new System.Drawing.Size(100, 20);
            this.txtTarget.TabIndex = 10;
            // 
            // lblTarget
            // 
            this.lblTarget.AutoSize = true;
            this.lblTarget.Location = new System.Drawing.Point(11, 14);
            this.lblTarget.Name = "lblTarget";
            this.lblTarget.Size = new System.Drawing.Size(44, 13);
            this.lblTarget.TabIndex = 9;
            this.lblTarget.Text = "Target :";
            // 
            // panelStatus
            // 
            this.panelStatus.Location = new System.Drawing.Point(813, 11);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(100, 58);
            this.panelStatus.TabIndex = 8;
            // 
            // gridItems
            // 
            this.gridItems.AllowUserToAddRows = false;
            this.gridItems.AllowUserToDeleteRows = false;
            this.gridItems.AllowUserToResizeRows = false;
            this.gridItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridItems.Location = new System.Drawing.Point(14, 75);
            this.gridItems.MultiSelect = false;
            this.gridItems.Name = "gridItems";
            this.gridItems.ReadOnly = true;
            this.gridItems.RowHeadersVisible = false;
            this.gridItems.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridItems.Size = new System.Drawing.Size(899, 285);
            this.gridItems.TabIndex = 7;
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Enabled = false;
            this.btnExport.Location = new System.Drawing.Point(787, 392);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // txtError
            // 
            this.txtError.Enabled = false;
            this.txtError.Location = new System.Drawing.Point(738, 49);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(42, 20);
            this.txtError.TabIndex = 20;
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(656, 52);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(35, 13);
            this.lblError.TabIndex = 19;
            this.lblError.Text = "Error :";
            // 
            // SelectionReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(955, 427);
            this.ControlBox = false;
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectionReportForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Result Summary";
            this.panMain.ResumeLayout(false);
            this.panMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.DataGridView gridItems;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.TextBox txtDuplicate;
        private System.Windows.Forms.Label lblDuplicate;
        private System.Windows.Forms.TextBox txtFailed;
        private System.Windows.Forms.Label lblFailed;
        private System.Windows.Forms.TextBox txtSkipped;
        private System.Windows.Forms.Label lblSkipped;
        private System.Windows.Forms.TextBox txtMoved;
        private System.Windows.Forms.Label lblMoved;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.Label lblTarget;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.Label lblError;
    }
}