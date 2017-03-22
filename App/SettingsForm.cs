using JocysCom.ClassLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JocysCom.WebScreenSaver
{
	public partial class SettingsForm : Form
	{
		public SettingsForm()
		{
			InitializeComponent();
			var info = new AssemblyInfo();
			Text = info.GetTitle();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{

		}

		private void DeleteButton_Click(object sender, EventArgs e)
		{

		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{

		}

		private void OkButton_Click(object sender, EventArgs e)
		{
			SettingsManager.WebItems.Version = 1;
			SettingsManager.WebItems.Save();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void SettingsControl_Load(object sender, EventArgs e)
		{
			SettingsManager.WebItems.Load();
			var items = SettingsManager.WebItems.Items;
			if (items.Count == 0)
			{
				items.Add(new WebScreenSaver.WebItem() { Url = "about:blank" });
			}
			SettingsControl.Data = SettingsManager.WebItems;
			ApplyFilter();
		}

		void ApplyFilter()
		{
			var text = SettingsControl.FilterTextBox.Text;
			var items = SettingsManager.WebItems.Items;
			var newList = items.Where(x => x.Url.Contains(text)).ToArray();
			var filteredItems = new ClassLibrary.ComponentModel.SortableBindingList<WebItem>(newList);
			var changed = newList.Count() != items.Count();
			SettingsControl.UpdateOnly = changed;
			SettingsControl.DataGridView.DataSource = changed ? filteredItems : items;
		}

		private void PreviewButton_Click(object sender, EventArgs e)
		{
			ScreenSaverForm.ShowScreenSaver(true);
		}
	}
}
