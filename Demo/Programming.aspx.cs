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

		PlaceHolder1.Controls.Add (fileManager);
	}
}
