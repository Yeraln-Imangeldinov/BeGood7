using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel;

using System.Windows.Forms;
using System.IO;

namespace BeGood3
{
    public class OptionPageGrid : DialogPage
    {

        private string optionString = "";

        [Category("Custom Help Category")]
        [DisplayName("Custom Help")]
        [Description("Path to file")]
        public string OptionFilePath
        {
            get {return optionString; 
            }
            set { optionString = value;
                    Request_Data data = new Request_Data();
                    //Set Path to static var 
                    data.SetValuePath(value);
                ThreadHelper.ThrowIfNotOnUIThread();
                if (!File.Exists(data.GetValuePath()))
                {
                    // Initializes the variables to pass to the MessageBox.Show method.
                    string message = "Error! File not founded";
                    string caption = "Error Detected in Input";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;

                    // Displays the MessageBox.
                    result = MessageBox.Show(message, caption, buttons);
                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        // Closes the parent form.
                        return;
                    }

                   
                }
            }

        }
    }
    
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(BeGood3Package.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(OptionPageGrid),
    "Custom Help Category", "Custom Help Grid Page", 0, 0, true)]
    public sealed class BeGood3Package : AsyncPackage
    {
        public string OptionInteger
        {
            get
            {
                OptionPageGrid page = (OptionPageGrid)GetDialogPage(typeof(OptionPageGrid));
                return page.OptionFilePath;
            }
        }

        /// <summary>
        /// BeGood3Package GUID string.
        /// </summary>
        public const string PackageGuidString = "cd906a2d-8d7f-4318-8c5c-361c78b23d50";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            await Help_Custom.InitializeAsync(this);

        }

        #endregion
    }
}
