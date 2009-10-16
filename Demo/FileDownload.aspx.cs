using System;
using System.Web;
using IZ.WebFileManager;

public partial class FileDownload : System.Web.UI.Page
{
	protected void Page_Load (object sender, EventArgs e) {
		Label1.Text = "";
	}
	protected void FileManager1_FileDownload (object sender, DownloadFileCancelEventArgs e) {
		if(ProhibitDownload.Checked)
		{
			e.Cancel = true;
			Label1.Text = "Downloading file " + HttpUtility.HtmlEncode (e.DownloadFile.PhysicalPath) + " is prohibited.";
		}
	}
}
