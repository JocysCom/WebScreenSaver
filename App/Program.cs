using JocysCom.ClassLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JocysCom.WebScreenSaver
{

	static class Program
	{

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			// Requires System.Configuration.Installl reference.
			var ic = new Arguments(args);
			if (ic.Count == 0)
			{
				Application.Run(new SettingsForm());
			}
			// c – Show the configuration settings dialog box.
			else if (ic.ContainsKey("c", true))
			{
				var settingsForm = new SettingsForm();
				var handle = ic.GetValue<long>("c", true);
				if (handle > 0)
				{
					var configWndHandle = new IntPtr(handle);
					//-----------------------------------------
					// Open inside parent window.
					//-----------------------------------------
					// Set the preview window as the parent of this window
					//NativeMethods.SetParent(settingsForm.Handle, configWndHandle);
					// Retrieve information about this window.
					//var windowLong = NativeMethods.GetWindowLong(settingsForm.Handle, GWL_STYLE);
					// Make this a child window so it will close when the parent dialog closes
					//NativeMethods.SetWindowLong(settingsForm.Handle, GWL_STYLE, windowLong | WS_CHILD);
					// Get parent client rectangle.
					//System.Drawing.Rectangle parentCliRect;
					//NativeMethods.GetClientRect(configWndHandle, out parentCliRect);
					//settingsForm.Top = 0;
					//settingsForm.Left = 0;
					//settingsForm.Width = parentCliRect.Width;
					//settingsForm.Height = parentCliRect.Height;
					//-----------------------------------------
					// Open in new window.
					//-----------------------------------------
					// Get parent window rectangle.
					System.Drawing.Rectangle parentWinRect;
					NativeMethods.GetWindowRect(configWndHandle, out parentWinRect);
					settingsForm.Top = parentWinRect.Top;
					settingsForm.Left = parentWinRect.Left;
				}
				Application.Run(settingsForm);
			}
			// p #### – Display a preview of the screensaver using the specified window handle.
			else if (ic.ContainsKey("p", true))
			{
				var handle = ic.GetValue<long>("p", true);
				if (handle > 0)
				{
					var previewWndHandle = new IntPtr(handle);
					// Get size of the parent window.
					System.Drawing.Rectangle ParentRect;
					NativeMethods.GetClientRect(previewWndHandle, out ParentRect);
					// Apply size. Place window inside the parent.
					//mainForm.Top = 0;
					//mainForm.Left = 0;
					//mainForm.Width = ParentRect.Width;
					//mainForm.Height = ParentRect.Height;
					var mainForm = new ScreenSaverForm(ParentRect, false, true);
					// Set the preview window as the parent of this window
					NativeMethods.SetParent(mainForm.Handle, previewWndHandle);
					//// Retrieve information about this window.
					var windowLong = NativeMethods.GetWindowLong(mainForm.Handle, GWL_STYLE);
					//// Make this a child window so it will close when the parent dialog closes
					NativeMethods.SetWindowLong(mainForm.Handle, GWL_STYLE, windowLong | WS_CHILD);
					mainForm.Size = ParentRect.Size;
					mainForm.Location = new System.Drawing.Point(0, 0);
					mainForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
					mainForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
					Application.Run(mainForm);
				}
				else
				{
					var error = string.Format("Error! Wrong window handle format: '{0}'", string.Join(" ", args));
					MessageBox.Show(error, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
			// s – Start the screensaver in full-screen mode.
			else if (ic.ContainsKey("s"))
			{
				ScreenSaverForm.ShowScreenSaver(false);
				Application.Run();
			}
			else if (ic.ContainsKey("d"))
			{
				// d – Start the screensaver in debug mode.
			}
			else
			{
				var error = string.Format("Command line argument \"{0}\" is not valid.", ic.Keys.Cast<string>().First());
				MessageBox.Show(error, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		/// <summary>Sets a new window style.</summary>
		const int GWL_STYLE = -16;
		/// <summary>Child window. A window with this style cannot have a menu bar.</summary>
		public const int WS_CHILD = 0x40000000;
		public const int WS_VISIBLE = 0x10000000;
		public const int WS_CLIPCHILDREN = 0x02000000;

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
			internal static extern bool GetClientRect(IntPtr hWnd, out System.Drawing.Rectangle lpRect);

			/// <summary>
			/// Retrieves the dimensions of the bounding rectangle of the specified window. 
			/// </summary>
			/// <param name="hWnd">A handle to the window whose client coordinates are to be retrieved.</param>
			/// <param name="lpRect">A pointer to a RECT structure that receives the client coordinates.</param>
			/// <returns></returns>
			[DllImport("user32.dll", SetLastError = true)]
			internal static extern bool GetWindowRect(IntPtr hWnd, out System.Drawing.Rectangle lpRect);


		}

	}
}
