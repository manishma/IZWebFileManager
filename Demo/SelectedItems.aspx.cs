using System;
using System.IO;
using IZ.WebFileManager;

public partial class SelectedItems : System.Web.UI.Page
{
	protected void Page_Load (object sender, EventArgs e) {

	}

	protected void LogSelected (object sender, EventArgs e) {
		SelectedItemsLog.Text = "";

		if (FileManager1.SelectedItems.Length == 0)
			SelectedItemsLog.Text = "Please, select any item first.<br />";
		else
			foreach (FileManagerItemInfo item in FileManager1.SelectedItems) {
				SelectedItemsLog.Text += item.VirtualPath + "<br />";

			}
	}

	protected void LogCurrentDirectory (object sender, EventArgs e) {
		SelectedItemsLog.Text = "";
		SelectedItemsLog.Text = FileManager1.CurrentDirectory.VirtualPath + "<br />";
	}

	protected void Upload(object sender, EventArgs e)
	{
		SelectedItemsLog.Text = "";
		if(FileUpload1.HasFile)
		{
			string name = Path.GetFileName(FileUpload1.FileName);
			string vdir = FileManager1.CurrentDirectory.VirtualPath;
			string dir = FileManager1.CurrentDirectory.PhysicalPath;

			FileUpload1.SaveAs(Path.Combine(dir, name));

			SelectedItemsLog.Text = String.Format("File '{0}' was uploaded to '{1}' successfuly.<br />", name, vdir);
		}
		else
			SelectedItemsLog.Text = "Please, select file to upload.<br />";
	}
}
