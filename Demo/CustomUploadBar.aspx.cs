using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomUploadBar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            String dir = FileManager1.CurrentDirectory.PhysicalPath;
            String fileName = Path.GetFileName(FileUpload1.FileName);

            String filePath = Path.Combine(dir, fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);

            FileUpload1.PostedFile.SaveAs(filePath);
        }
    }
}