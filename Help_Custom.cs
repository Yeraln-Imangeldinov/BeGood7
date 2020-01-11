using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualStudio.Shell.Interop;

namespace BeGood3
{
    #region Request_Data
    public class Request_Data
    {
        private static string value = "bool";
        private static string path_value = "";

        public Request_Data() 
        {
        }
        public string GetValue()
        {
            return value;
        }
        public void SetValue(string arg)
        {
            value = arg;
        }
        public void SetValuePath(string arg)
        {
            path_value = arg;
        }
        public string GetValuePath()
        {
            return path_value;
        }
    }

    #endregion
    internal sealed class Help_Custom
    {


        private Request_Data data;
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("14fddd46-fa1b-4caf-9a3b-ee95074fec21");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        //Form help methods
        Form1 form2;
        /// <summary>
        /// Initializes a new instance of the <see cref="Help_Custom"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private Help_Custom(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);

            // retrieve and set  file path from options
            //Create form with path from options
            BeGood3Package myToolsOptionsPackage = this.package as BeGood3Package;
            form2 = new Form1(myToolsOptionsPackage.OptionInteger);

            data = new Request_Data();
            //Set Path to static var 
            data.SetValuePath(myToolsOptionsPackage.OptionInteger);

            // Set the opacity to 75%.
            form2.Opacity = 0;
            // Size the form to be 300 pixels in height and width.
            form2.Size = new Size(1, 1);
            // Display the form in the center of the screen.
            form2.StartPosition = FormStartPosition.CenterScreen;

        }
        
        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static Help_Custom Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in Help_Custom's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new Help_Custom(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!File.Exists(data.GetValuePath()))
            {
                
                string title = "Error! “File not founded";
                string mess = "To add see: TOOLS->options->Custom Help Category. file://c:\\File.chm";
                VsShellUtilities.ShowMessageBox(
                               this.package,
                               mess,
                               title,
                               OLEMSGICON.OLEMSGICON_INFO,
                               OLEMSGBUTTON.OLEMSGBUTTON_OK,
                               OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }
            else 
            {
                form2.HelpMethod();
               // form2.Close();
            }
        }
    };

    public class Form1 : System.Windows.Forms.Form
    {
       
        Request_Data data;

        public Form1(string setstr)
        {
            
            data = new Request_Data();
            data.SetValuePath(setstr);
        }
        public void HelpMethod()
        {
            HelpNavigator navigator = HelpNavigator.KeywordIndex;
            Help.ShowHelp(this, data.GetValuePath(), navigator, data.GetValue());

        }
    }
}

