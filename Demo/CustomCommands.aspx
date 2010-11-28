<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Title="Custom Commands" ValidateRequest=false %>

<%@ Import Namespace="System.Reflection" %>

<%@ Import Namespace="System.IO" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<script runat="server">

    protected void FileManager1_ExecuteCommand(object sender, ExecuteCommandEventArgs e)
    {
        if (Page.IsCallback)
        {
            e.ClientScript = "alert('Use ExecuteCommand event to handle your custom command.\\nCommandName=" + e.CommandName +
                             "\\nItem=" + e.Items[0].VirtualPath + "')";
        }
        else
        {
            if (e.CommandName == "EditText")
            {
                if(e.Items.Count == 0)
                    return;

                FileManagerItemInfo item = e.Items[0];
                ViewState["EditDocumentPath"] = item.PhysicalPath;
                string text = File.ReadAllText(item.PhysicalPath);

                TextBox1.Text = text;

                FileManager1.Visible = false;
                TextEditor.Visible = true;
            }
        }
    }
    
    private void Button1_Click(object sender, EventArgs e)
    {
        string filePath = (string) ViewState["EditDocumentPath"];
        File.WriteAllText(filePath, TextBox1.Text);
        
        FileManager1.Visible = true;
        TextEditor.Visible = false;
    }
    
    private void Button2_Click(object sender, EventArgs e)
    {
        FileManager1.Visible = true;
        TextEditor.Visible = false;
    }    
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div style="margin-top: 8px; margin-bottom: 8px;">
    Set up custom file commands (right click) and handle executing on PostBack or Callback by you chose.
	</div>
	<div>
		<iz:FileManager ID="FileManager1" runat="server" Height="400" Width="600" 
            onexecutecommand="FileManager1_ExecuteCommand">
			<RootDirectories>
				<iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
			</RootDirectories>
            <FileTypes>
                <iz:FileType Extensions="txt,htm,html,css,js,ini,config" Name="Text Document" SmallImageUrl="images/16x16/notepad.png"  LargeImageUrl="images/32x32/notepad.png">
                    <Commands>
                        <iz:FileManagerCommand Name="Edit (PostBack)" Method="PostBack" CommandName="EditText" SmallImageUrl="images/16x16/notepad.png" />
                        <iz:FileManagerCommand Name="Edit (Callback)" CommandName="EditText" SmallImageUrl="images/16x16/notepad.png" />
                    </Commands>
                </iz:FileType>
            </FileTypes>
		</iz:FileManager>
        <asp:Panel runat="server" ID="TextEditor" Visible="false">
            <div>
                <asp:TextBox runat="server" ID="TextBox1" TextMode="MultiLine" Height="400" Width="600"/>
            </div>
            <div>
                <asp:Button runat="server" ID="Button1" Text="Save" OnClick="Button1_Click" /><asp:Button runat="server" ID="Button2" Text="Cancel" OnClick="Button2_Click" />
            </div>
        </asp:Panel>
	</div>
</asp:Content>
