namespace JocysCom.WebScreenSaver
{
	partial class ScreenSaverForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenSaverForm));
			this.MainWebBrowser = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// MainWebBrowser
			// 
			this.MainWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.MainWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.MainWebBrowser.Name = "MainWebBrowser";
			this.MainWebBrowser.ScriptErrorsSuppressed = true;
			this.MainWebBrowser.Size = new System.Drawing.Size(640, 480);
			this.MainWebBrowser.TabIndex = 2;
			// 
			// ScreenSaverForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(640, 480);
			this.Controls.Add(this.MainWebBrowser);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ScreenSaverForm";
			this.Text = "ScreenSaverForm";
			this.Load += new System.EventHandler(this.ScreenSaverForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser MainWebBrowser;
	}
}