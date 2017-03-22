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

		internal class NativeMethods
		{

			/// <summary>
			/// Changes the parent window of the specified child window.
			/// </summary>
			/// <param name="hWndChild">A handle to the child window.</param>
			/// <param name="hWndNewParent">
			/// A handle to the new parent window. If this parameter is NULL, the desktop window becomes the new parent window.
			/// If this parameter is HWND_MESSAGE, the child window becomes a message-only window.
			/// </param>
			/// <returns></returns>
			[DllImport("user32.dll", SetLastError = true)]
			internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

			/// <summary>
			/// Changes an attribute of the specified window. The function also sets the 32-bit (long)
			/// value at the specified offset into the extra window memory.
			/// </summary>
			/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
			/// <param name="nIndex">The zero-based offset to the value to be set.</param>
			/// <param name="dwNewLong">The replacement value.</param>
			/// <returns></returns>
			[DllImport("user32.dll", SetLastError = true)]
			internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

			/// <summary>
			/// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD)
			/// value at the specified offset into the extra window memory.
			/// </summary>
			/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
			/// <param name="nIndex">The zero-based offset to the value to be retrieved. </param>
			/// <returns></returns>
			[DllImport("user32.dll", SetLastError = true)]
			internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

			/// <summary>
			/// Retrieves the coordinates of a window's client area. 
			/// </summary>
			/// <param name="hWnd">A handle to the window whose client coordinates are to be retrieved.</param>
			/// <param name="lpRect">A pointer to a RECT structure that receives the client coordinates.</param>
			/// <returns></returns>
			[DllImport("user32.dll", SetLastError = true)]
			internal static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

		}

		Point mouseLocation;
		bool _PreviewMode;
		bool _FromProgram;
		Random rand = new Random();

		System.Timers.Timer RefreshTimer;
		System.Timers.Timer GoNextTimer;
		object TimersLock = new object();

		public ScreenSaverForm()
		{
			InitializeComponent();
		}

		public ScreenSaverForm(Rectangle Bounds, bool fromProgram)
		{
			_FromProgram = fromProgram;
			InitializeComponent();
			this.Bounds = Bounds;
		}

		/// <summary>Sets a new window style.</summary>
		const int GWL_STYLE = -16;
		/// <summary>Child window. A window with this style cannot have a menu bar.</summary>
		public const int WS_CHILD = 0x40000000;

		public ScreenSaverForm(IntPtr PreviewWndHandle)
		{
			InitializeComponent();
			// Set the preview window as the parent of this window
			NativeMethods.SetParent(Handle, PreviewWndHandle);
			// Retrieve information about this window.
			var windowLong = NativeMethods.GetWindowLong(Handle, GWL_STYLE);
			// Make this a child window so it will close when the parent dialog closes
			NativeMethods.SetWindowLong(Handle, GWL_STYLE, windowLong | WS_CHILD);
			// Place our window inside the parent
			Rectangle ParentRect;
			NativeMethods.GetClientRect(PreviewWndHandle, out ParentRect);
			Size = ParentRect.Size;
			Location = new Point(0, 0);
			_PreviewMode = true;
		}

		void HideScreenSaver()
		{
			if (!_PreviewMode)
			{
				if (_FromProgram)
				{
					Close();
				}
				else
				{
					Application.Exit();
				}
			}
		}

		/// <summary>
		/// Display the form on each of the computer's monitors.
		/// </summary>
		public static void ShowScreenSaver(bool fromProgram)
		{
			foreach (Screen screen in Screen.AllScreens)
			{
				var screensaver = new ScreenSaverForm(screen.Bounds, fromProgram);
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
			// Use the string from the Registry if it exists
			SettingsManager.WebItems.Load();
			LoadNextItem();
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
			if (!_PreviewMode)
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
