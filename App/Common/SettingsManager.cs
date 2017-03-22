using JocysCom.ClassLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JocysCom.WebScreenSaver
{
	public class SettingsManager
	{

		/// <summary>User Settings.</summary>
		public static SettingsData<WebItem> WebItems = new SettingsData<WebItem>("WebItems.xml", false, "Web Item Settings.");

	}
}
