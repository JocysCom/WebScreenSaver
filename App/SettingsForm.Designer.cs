namespace JocysCom.WebScreenSaver
{
	partial class SettingsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.WebPagesTabControl = new System.Windows.Forms.TabControl();
            this.WebPagesTabPage = new System.Windows.Forms.TabPage();
            this.SettingsControl = new JocysCom.ClassLibrary.Configuration.SettingsUserControl();
            this.CloseButton = new System.Windows.Forms.Button();
            this.OkButton = new System.Windows.Forms.Button();
            this.PreviewButton = new System.Windows.Forms.Button();
            this.UrlColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecondsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoNext = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WebPagesTabControl.SuspendLayout();
            this.WebPagesTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // WebPagesTabControl
            // 
            this.WebPagesTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WebPagesTabControl.Controls.Add(this.WebPagesTabPage);
            this.WebPagesTabControl.Location = new System.Drawing.Point(12, 12);
            this.WebPagesTabControl.Name = "WebPagesTabControl";
            this.WebPagesTabControl.SelectedIndex = 0;
            this.WebPagesTabControl.Size = new System.Drawing.Size(659, 355);
            this.WebPagesTabControl.TabIndex = 0;
            // 
            // WebPagesTabPage
            // 
            this.WebPagesTabPage.Controls.Add(this.SettingsControl);
            this.WebPagesTabPage.Location = new System.Drawing.Point(4, 22);
            this.WebPagesTabPage.Name = "WebPagesTabPage";
            this.WebPagesTabPage.Size = new System.Drawing.Size(651, 329);
            this.WebPagesTabPage.TabIndex = 0;
            this.WebPagesTabPage.Text = "Web Pages";
            this.WebPagesTabPage.UseVisualStyleBackColor = true;
            // 
            // SettingsControl
            // 
            this.SettingsControl.Data = null;
            this.SettingsControl.DataGridViewColumns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UrlColumn,
            this.SecondsColumn,
            this.GoNext});
            this.SettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsControl.Location = new System.Drawing.Point(0, 0);
            this.SettingsControl.Name = "SettingsControl";
            this.SettingsControl.Size = new System.Drawing.Size(651, 329);
            this.SettingsControl.TabIndex = 0;
            this.SettingsControl.Load += new System.EventHandler(this.SettingsControl_Load);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.CausesValidation = false;
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(596, 373);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 7;
            this.CloseButton.Text = "Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(515, 373);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 6;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // PreviewButton
            // 
            this.PreviewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PreviewButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.PreviewButton.Location = new System.Drawing.Point(12, 373);
            this.PreviewButton.Name = "PreviewButton";
            this.PreviewButton.Size = new System.Drawing.Size(75, 23);
            this.PreviewButton.TabIndex = 6;
            this.PreviewButton.Text = "Preview";
            this.PreviewButton.UseVisualStyleBackColor = true;
            this.PreviewButton.Click += new System.EventHandler(this.PreviewButton_Click);
            // 
            // UrlColumn
            // 
            this.UrlColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.UrlColumn.DataPropertyName = "Url";
            this.UrlColumn.HeaderText = "URL";
            this.UrlColumn.Name = "UrlColumn";
            // 
            // SecondsColumn
            // 
            this.SecondsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SecondsColumn.DataPropertyName = "RefreshSeconds";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.SecondsColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.SecondsColumn.HeaderText = "Refresh";
            this.SecondsColumn.Name = "SecondsColumn";
            this.SecondsColumn.Width = 71;
            // 
            // GoNext
            // 
            this.GoNext.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.GoNext.DataPropertyName = "GoNextSeconds";
            this.GoNext.HeaderText = "Go Next";
            this.GoNext.Name = "GoNext";
            this.GoNext.Width = 73;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 408);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.PreviewButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.WebPagesTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.WebPagesTabControl.ResumeLayout(false);
            this.WebPagesTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl WebPagesTabControl;
		private System.Windows.Forms.TabPage WebPagesTabPage;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Button OkButton;
		private ClassLibrary.Configuration.SettingsUserControl SettingsControl;
		private System.Windows.Forms.Button PreviewButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn UrlColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecondsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoNext;
    }
}

