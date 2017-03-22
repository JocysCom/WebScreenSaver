using JocysCom.ClassLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var ic = new InstallContext(null, args);
            // c – Show the configuration settings dialog box.
            if (ic.Parameters.Count == 0 || ic.Parameters.ContainsKey("c"))
            {
                Application.Run(new SettingsForm());
            }
            // p #### – Display a preview of the screensaver using the specified window handle.
            else if (ic.Parameters.ContainsKey("p"))
            {
                var handleValue = ic.Parameters["p"];
                // If parameter was separated by space instead of '=' then...
                if (string.IsNullOrEmpty(handleValue))
                {
                    var index = args.Select(x => x.ToLower()).ToList().IndexOf("p");
                    if (index + 1 < args.Length) handleValue = args[index + 1];
                }
                long handle;
                if (long.TryParse(handleValue, out handle))
                {
                    var previewWndHandle = new IntPtr(handle);
                    Application.Run(new ScreenSaverForm(previewWndHandle));
                }
                else
                {
                    var error = string.Format("Error! Wrong window handle format: '{0}'", string.Join(" ", args));
                    MessageBox.Show(error, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            // s – Start the screensaver in full-screen mode.
            else if (ic.Parameters.ContainsKey("s"))
            {
                ScreenSaverForm.ShowScreenSaver(false);
                Application.Run();
            }
            else if (ic.Parameters.ContainsKey("d"))
            {
                // d – Start the screensaver in debug mode.
            }
            else
            {
                var error = string.Format("Command line argument \"{0}\" is not valid.", ic.Parameters.Keys.Cast<string>().First());
                MessageBox.Show(error, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

    }
}
