using JocysCom.ClassLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace JocysCom.WebScreenSaver
{
    public class WebItem : ISettingsItem
    {
        public WebItem()
        {
            Enabled = true;
            ScriptErrorsSuppressed = true;
        }

        [DefaultValue(""), Category("(General)")]
        public string Url { get; set; }

        /// <summary>Auto-refresh current page after.</summary>
        [DefaultValue(0), Category("(General)")]
        public int RefreshSeconds { get; set; }

        /// <summary>Go to next URL.</summary>
        [DefaultValue(0), Category("(General)")]
        public int GoNextSeconds { get; set; }

        [DefaultValue(true), Category("(General)")]
        public bool Enabled { get; set; }

        [Browsable(false)]
        public bool IsEmpty { get { return string.IsNullOrEmpty(Url); } }

        [DefaultValue(false)]
        public bool HideCursor { get; set; }

        #region Web Browser Settitngs

        /// <summary>
        /// Gets or sets a value indicating whether the control can navigate to another page
        /// after its initial page has been loaded.
        /// </summary>
        [DefaultValue(false), Category("Browser")]
        public bool AllowNavigation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the System.Windows.Forms.WebBrowser control
        /// navigates to documents that are dropped onto it.
        /// </summary>
        [DefaultValue(false), Category("Browser")]
        public bool AllowWebBrowserDrop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the System.Windows.Forms.WebBrowser displays
        /// dialog boxes such as script error messages.
        /// </summary>
        [DefaultValue(true), Category("Browser")]
        public bool ScriptErrorsSuppressed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether scroll bars are displayed in the System.Windows.Forms.WebBrowser
        /// control.
        /// </summary>
        [DefaultValue(false), Category("Browser")]
        public bool ScrollBarsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether keyboard shortcuts are enabled within
        /// the System.Windows.Forms.WebBrowser control.
        /// </summary>
        [DefaultValue(false), Category("Browser")]
        public bool WebBrowserShortcutsEnabled { get; set; }

        #endregion

    }
}
