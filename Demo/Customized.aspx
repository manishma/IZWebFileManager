<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<script runat="server">

	protected void FileManager1_ExecuteCommand (object sender, ExecuteCommandEventArgs e) {
		e.ClientScript = "alert('Use ExecuteCommand event to handle your custom command.\\nCommandName=" + e.CommandName + "\\nItem=" + e.Items [0].AbsolutePath + "')";
	}

	protected void CheckBox1_CheckedChanged (object sender, EventArgs e) {
		FileManager1.ReadOnly = ((CheckBox) sender).Checked;
	}

	protected void CheckBox2_CheckedChanged (object sender, EventArgs e) {
		FileManager1.AllowUpload = ((CheckBox) sender).Checked;
	}

	protected void CheckBox3_CheckedChanged (object sender, EventArgs e) {
		FileManager1.AllowDelete = ((CheckBox) sender).Checked;
	}

	protected void CheckBox4_CheckedChanged (object sender, EventArgs e) {
		FileManager1.AllowOverwrite = ((CheckBox) sender).Checked;
	}

	protected void CheckBox5_CheckedChanged (object sender, EventArgs e) {
		FileManager1.UseLinkToOpenItem = ((CheckBox) sender).Checked;
	}

	protected void FileManager1_ToolbarCommand (object sender, CommandEventArgs e) {
		if (e.CommandName == "CreateZip") {

			string zipFile = System.IO.Path.Combine (FileManager1.CurrentDirectory.PhysicalPath, "ZIP_" + DateTime.Now.ToString ("yyyyMMddHHmmss") + ".zip");

			//Create an empty zip file
			byte [] emptyzip = new byte [] { 80, 75, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

			using (System.IO.FileStream fs = System.IO.File.Create (zipFile)) {
				fs.Write (emptyzip, 0, emptyzip.Length);
				fs.Flush ();
				fs.Close ();
			}

			string [] selectedItems = new string [FileManager1.SelectedItems.Length];
			for (int i = 0; i < FileManager1.SelectedItems.Length; i++) {
				selectedItems [i] = System.IO.Path.GetFileName (FileManager1.SelectedItems [i].PhysicalPath);
			}
			//Copy a folder and its contents into the newly created zip file
			Shell32.ShellClass sc = new Shell32.ShellClass ();
			Shell32.Folder DestFlder = sc.NameSpace (zipFile);
			Shell32.Folder SrcFlder = sc.NameSpace (FileManager1.CurrentDirectory.PhysicalPath);
			Shell32.FolderItems items = SrcFlder.Items ();
			foreach (Shell32.FolderItem item in items) {
				if (Array.LastIndexOf<string> (selectedItems, item.Name) >= 0)
					DestFlder.CopyHere (item, 20);
			}

			//Ziping a file using the Windows Shell API creates another thread where the zipping is executed.
			//This means that it is possible that this console app would end before the zipping thread 
			//starts to execute which would cause the zip to never occur and you will end up with just
			//an empty zip file. So wait a second and give the zipping thread time to get started
			System.Threading.Thread.Sleep (1000);
		}
		else {
			throw new InvalidOperationException ();
		}
	}
</script>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

	<script type="text/javascript">
    function OpenItem(path) {
        alert('Use ClientOpenItemFunction property to handle Open command\npath='+path);
    }
	</script>

	<div style="margin-top: 8px; margin-bottom: 8px;">
		Use your custom File Types, Commands & New Document Templates<br />
		<asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged"
			Text="ReadOnly" />&nbsp;&nbsp;<asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True"
				Checked="True" OnCheckedChanged="CheckBox2_CheckedChanged" Text="AllowUpload" />&nbsp;<asp:CheckBox
					ID="CheckBox3" runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged="CheckBox3_CheckedChanged"
					Text="AllowDelete" />
		<asp:CheckBox ID="CheckBox4" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox4_CheckedChanged"
			Text="AllowOverwrite" />
		<asp:CheckBox ID="CheckBox5" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox5_CheckedChanged"
			Text="UseLinkToOpenItem" /></div>
	<div>
		<iz:FileManager ID="FileManager1" runat="server" Height="400px" Width="600" OnExecuteCommand="FileManager1_ExecuteCommand"
			ClientOpenItemFunction="OpenItem" OnToolbarCommand="FileManager1_ToolbarCommand">
			<RootDirectories>
				<iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
			</RootDirectories>
			<CustomToolbarButtons>
				<iz:CustomToolbarButton Text="Create Zip" CommandName="CreateZip" ImageUrl="images/16x16/zip.gif" />
				<iz:CustomToolbarButton Text="Say Hello!" PerformPostBack="false" OnClientClick="alert('Hello!')"
					ImageUrl="images/16x16/smile.gif" />
			</CustomToolbarButtons>
			<ToolbarOptions ShowRefreshButton="false" />
			<Templates>
				<iz:NewDocumentTemplate Name="HTML Page" NewFileName="New HTML File" MasterFileUrl="Templates/HTMLPage.htm" />
				<iz:NewDocumentTemplate Name="JScript File" NewFileName="JScript" MasterFileUrl="Templates/JScript.js" />
				<iz:NewDocumentTemplate Name="Style Sheet" NewFileName="StyleSheet" MasterFileUrl="Templates/StyleSheet.css" />
			</Templates>
			<FileTypes>
				<iz:FileType Extensions="zip, rar, iso" Name="Archive" SmallImageUrl="images/16x16/compressed.gif"
					LargeImageUrl="images/32x32/compressed.gif">
				</iz:FileType>
				<iz:FileType Extensions="doc, rtf" Name="Microsoft Word Document" SmallImageUrl="images/16x16/word.gif"
					LargeImageUrl="images/32x32/word.gif">
				</iz:FileType>
				<iz:FileType Extensions="xls, csv" Name="Microsoft Excel Worksheet" SmallImageUrl="images/16x16/excel.gif"
					LargeImageUrl="images/32x32/excel.gif">
				</iz:FileType>
				<iz:FileType Extensions="ppt, pps" Name="Microsoft PowerPoint Presentation" SmallImageUrl="images/16x16/PowerPoint.gif"
					LargeImageUrl="images/32x32/PowerPoint.gif">
				</iz:FileType>
				<iz:FileType Extensions="gif, jpg, png" Name="Image" SmallImageUrl="images/16x16/image.gif"
					LargeImageUrl="images/32x32/image.gif">
				</iz:FileType>
				<iz:FileType SmallImageUrl="images/16x16/media.gif" Name="Windows Media File" Extensions="mp3,wma,vmv,avi,divx"
					LargeImageUrl="images/32x32/media.gif">
				</iz:FileType>
				<iz:FileType Extensions="txt" Name="Text Document">
					<Commands>
						<iz:FileManagerCommand Name="Edit" CommandName="EditText" SmallImageUrl="images/16x16/edit.gif" />
					</Commands>
				</iz:FileType>
				<iz:FileType Extensions="xml, xsl, xsd" Name="XML Document" LargeImageUrl="images/32x32/xml.gif"
					SmallImageUrl="images/16x16/xml.gif">
					<Commands>
						<iz:FileManagerCommand Name="Edit" CommandName="EditText" SmallImageUrl="images/16x16/edit.gif" />
					</Commands>
				</iz:FileType>
				<iz:FileType Extensions="css" Name="Cascading Style Sheet" LargeImageUrl="images/32x32/styleSheet.gif"
					SmallImageUrl="images/16x16/styleSheet.gif">
					<Commands>
						<iz:FileManagerCommand Name="Edit" CommandName="EditText" SmallImageUrl="images/16x16/edit.gif" />
					</Commands>
				</iz:FileType>
				<iz:FileType Extensions="js, vbs" Name="Script File" LargeImageUrl="images/32x32/script.gif"
					SmallImageUrl="images/16x16/script.gif">
					<Commands>
						<iz:FileManagerCommand Name="Edit" CommandName="EditText" SmallImageUrl="images/16x16/edit.gif" />
					</Commands>
				</iz:FileType>
				<iz:FileType Extensions="htm, html" Name="HTML Document" LargeImageUrl="images/32x32/html.gif"
					SmallImageUrl="images/16x16/html.gif">
					<Commands>
						<iz:FileManagerCommand Name="Edit with WYSWYG editor" CommandName="WYSWYG" />
						<iz:FileManagerCommand Name="Edit" CommandName="EditText" SmallImageUrl="images/16x16/edit.gif" />
					</Commands>
				</iz:FileType>
			</FileTypes>
		</iz:FileManager>
	</div>
</asp:Content>
