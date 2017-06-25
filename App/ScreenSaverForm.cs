using JocysCom.ClassLibrary.Configuration;
using JocysCom.ClassLibrary.Processes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace JocysCom.WebScreenSaver
{
	public partial class ScreenSaverForm : Form
	{

		Point mouseLocation;
		/// <summary>
		/// If _PreviewMode (small preview inside screensaver settings) then do not track mouse/keyboard movements.
		/// </summary>
		bool _PreviewMode;
		/// <summary>
		/// If FromSettings then only this form will be closed, instead of exiting application.
		/// </summary>
		bool _FromSettings;
		Random rand = new Random();

		System.Timers.Timer RefreshTimer;
		System.Timers.Timer GoNextTimer;
		object TimersLock = new object();

		/// <summary>
		/// Should be used for design this form on Visual Studio only.
		/// </summary>
		public ScreenSaverForm()
		{
			InitializeComponent();
		}

		public ScreenSaverForm(Rectangle Bounds, bool fromSettings, bool previewMode)
		{
			_FromSettings = fromSettings;
			_PreviewMode = previewMode;
			InitializeComponent();
			this.Bounds = Bounds;
		}

		void HideScreenSaver()
		{
			if (_FromSettings)
			{
				Close();
			}
			else
			{
				Application.Exit();
			}
		}

		/// <summary>
		/// Display the form on each of the computer's monitors.
		/// </summary>
		public static void ShowScreenSaver(bool fromSettings)
		{
			foreach (Screen screen in Screen.AllScreens)
			{
				var screensaver = new ScreenSaverForm(screen.Bounds, fromSettings, false);
				screensaver.Show();
			}
		}

		private void ScreenSaverForm_Load(object sender, EventArgs e)
		{
			LoadSettings();
			TopMost = true;
		}

		WebItem _CurrentItem;

		private void LoadSettings()
		{
			var info = new AssemblyInfo();
			Text = info.GetTitle();
			// Use the string from the Registry if it exists
			SettingsManager.WebItems.Load();
			LoadNextItem();
			// If this is not preview then...
			if (!_PreviewMode)
			{
				StartMouseAndKeyboardMonitor();
			}
		}

		#region Timers

		void LoadItem(WebItem item)
		{
			MainWebBrowser.AllowNavigation = item.AllowNavigation;
			MainWebBrowser.AllowWebBrowserDrop = item.AllowWebBrowserDrop;
			MainWebBrowser.ScriptErrorsSuppressed = item.ScriptErrorsSuppressed;
			MainWebBrowser.ScrollBarsEnabled = item.ScrollBarsEnabled;
			MainWebBrowser.WebBrowserShortcutsEnabled = item.WebBrowserShortcutsEnabled;
			if (item.HideCursor)
			{
				Cursor.Hide();
			}
			else
			{
				Cursor.Show();
			}
			MainWebBrowser.Url = (item == null)
					? new Uri("about:blank")
					: new Uri(item.Url);
		}

		void InitTimers(WebItem item)
		{
			lock (TimersLock)
			{
				if (_Disposing) return;
				if (item.RefreshSeconds > 0)
				{
					RefreshTimer = new System.Timers.Timer();
					RefreshTimer.Interval = item.RefreshSeconds * 1000;
					RefreshTimer.Elapsed += RefreshTimer_Elapsed;
				}
				else
				{
					if (RefreshTimer != null)
					{
						RefreshTimer.Dispose();
						RefreshTimer = null;
					}
				}
				if (item.GoNextSeconds > 0)
				{
					GoNextTimer = new System.Timers.Timer();
					GoNextTimer.Interval = item.GoNextSeconds * 1000;
					GoNextTimer.Elapsed += GoNextTimer_Elapsed;
				}
				else
				{
					if (GoNextTimer != null)
					{
						GoNextTimer.Dispose();
						GoNextTimer = null;
					}
				}
			}
		}

		T GetNextPrev<T>(IList<T> list, T item, int positions)
		{
			if (list.Count == 0) return default(T);
			var index = list.IndexOf(item);
			index = (index + positions) % list.Count;
			if (index < 0) index = list.Count - 1;
			return list[index];
		}

		void LoadNextItem()
		{
			var items = SettingsManager.WebItems.Items;
			if (items.Count > 0)
			{
				var newItem = GetNextPrev(items, _CurrentItem, 1);
				_CurrentItem = newItem;
				LoadItem(newItem);
				InitTimers(newItem);
			}
		}

		private void GoNextTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			LoadNextItem();
		}

		private void RefreshTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			MainWebBrowser.Refresh();
		}

		void DisposeTimers()
		{
			lock (TimersLock)
			{
				if (RefreshTimer != null)
				{
					RefreshTimer.Dispose();
					RefreshTimer = null;
				}
				if (GoNextTimer != null)
				{
					GoNextTimer.Dispose();
					GoNextTimer = null;
				}
			}

		}
		#endregion

		#region Keyborad and Mouse Hook

		void StartMouseAndKeyboardMonitor()
		{
			// Track mouse and keyboard for auto close.
			lock (hookLocks)
			{
				mouseHook = new MouseHook();
				mouseHook.OnMouseMove += MouseHook_OnMouseMove;
				mouseHook.OnMouseDown += MouseHook_OnMouseDown;
				mouseHook.Start();
				keyboardHook = new KeyboardHook();
				keyboardHook.KeyDown += KeyboardHook_KeyDown;
				keyboardHook.Start();
			}
		}

		MouseHook mouseHook;
		KeyboardHook keyboardHook;
		object hookLocks = new object();

		private void KeyboardHook_KeyDown(object sender, KeyEventArgs e)
		{
			HideScreenSaver();
		}

		private void MouseHook_OnMouseDown(object sender, MouseEventArgs e)
		{
			HideScreenSaver();
		}

		private void MouseHook_OnMouseMove(object sender, MouseEventArgs e)
		{
			if (!mouseLocation.IsEmpty)
			{
				// Terminate if mouse is moved a significant distance
				if (Math.Abs(mouseLocation.X - e.X) > 5 || Math.Abs(mouseLocation.Y - e.Y) > 5)
				{
					HideScreenSaver();
				}
			}
			// Update current mouse location
			mouseLocation = e.Location;
		}

		#endregion

		#region IDisposable

		bool _Disposing;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_Disposing = disposing;
				DisposeTimers();
				lock (hookLocks)
				{
					if (mouseHook != null)
					{
						mouseHook.Dispose();
						mouseHook = null;
					}
					if (keyboardHook != null)
					{
						keyboardHook.Dispose();
						keyboardHook = null;
					}
				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion
	}
}
