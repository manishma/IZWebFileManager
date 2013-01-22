using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IZ.WebFileManager;

public partial class Programming : System.Web.UI.Page
{
	protected void Page_Load (object sender, EventArgs e) {

		// create instance
		FileManager fileManager = new FileManager ();

		// set layouts
		fileManager.Height = 400;
		fileManager.Width = 600;

		// add root directory
		RootDirectory rootDirectory = new RootDirectory ();
		rootDirectory.DirectoryPath = DirectoryManager.GetRootDirectoryPath (Context);
		rootDirectory.Text = "My Documents";
		fileManager.RootDirectories.Add (rootDirectory);

        fileManager.ExecuteCommand += FileManagerOnExecuteCommand;

        FileManagerCommand wyswygCmd = new FileManagerCommand();
        wyswygCmd.CommandName = "WYSWYG";
        wyswygCmd.Name = "Edit with WYSWYG editor";

        FileManagerCommand editCmd = new FileManagerCommand();
        editCmd.CommandName = "EditText";
        editCmd.Name = "Edit";

	    FileType htmlFileType = new FileType();
        htmlFileType.Extensions = "htm, html";
        htmlFileType.Name = "HTML Document";
        htmlFileType.LargeImageUrl = "images/32x32/html.gif";
        htmlFileType.SmallImageUrl = "images/16x16/html.gif";
        htmlFileType.Commands.Add(wyswygCmd);
        htmlFileType.Commands.Add(editCmd);
        fileManager.FileTypes.Add(htmlFileType);

        FileType txtFileType = new FileType();
        txtFileType.Extensions = "txt, js, css";
        txtFileType.Name = "HTML Document";
        txtFileType.Commands.Add(editCmd);
        fileManager.FileTypes.Add(txtFileType);

		PlaceHolder1.Controls.Add (fileManager);
	}

    private void FileManagerOnExecuteCommand(object sender, ExecuteCommandEventArgs e)
    {
        e.ClientScript = "alert('Use ExecuteCommand event to handle your custom command.\\nCommandName=" + e.CommandName +
                         "\\nItem=" + e.Items[0].VirtualPath.Replace("'", "\\'") + "')";
    }
}
