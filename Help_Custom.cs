using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

using EnvDTE;
namespace BeGood3
{
    
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class Help_Custom
    {
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

            string text = "Print";
            Form1 form2 = new Form1();

            // Set the opacity to 75%.
            form2.Opacity = 0;
            // Size the form to be 300 pixels in height and width.
            form2.Size = new Size(1, 1);
            // Display the form in the center of the screen.
            form2.StartPosition = FormStartPosition.CenterScreen;
            form2.HelpMethod(text);
            
            //Application.Run(form2);
            ThreadHelper.ThrowIfNotOnUIThread();
            string message = string.Format(CultureInfo.CurrentCulture, "Inside {0}.MenuItemCallback()", this.GetType().FullName);
            string title = "Help_Custom";
            // Show a message box to prove we were here
            //VsShellUtilities.ShowMessageBox(
            //    this.package,
            //    message,
            //    title,
            //    OLEMSGICON.OLEMSGICON_INFO,
            //    OLEMSGBUTTON.OLEMSGBUTTON_OK,
            //    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            form2.Close();
        }
    };
}

public class Form1 : System.Windows.Forms.Form
{
    private const string helpfile = "file://c:\\mql5.chm";


    public Form1()
    {
       
    }
    public void HelpMethod(string arg)
    {
        HelpNavigator navigator = HelpNavigator.KeywordIndex;
        Help.ShowHelp(this, helpfile, navigator, arg);
      
    }
};



//Sub GoogleSearchMSDN()
//    Dim url As String
//    Dim searchFor As TextSelection = DTE.ActiveDocument.Selection()
//    If searchFor.Text<> "" Then
//        url = "www.google.com/search?q=MSDN+" + searchFor.Text
//    Else
//        url = "www.google.com/search?q=MSDN"
//    End If
//    DTE.ExecuteCommand("View.URL", url)
//End Sub