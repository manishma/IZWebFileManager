<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<script runat="server">

    protected void FileManager1_SelectedItemsAction(object sender, SelectedItemsActionCancelEventArgs e)
    {
        if(e.Action == SelectedItemsAction.Open)
        {
            FileManagerItemInfo item = e.SelectedItems[0];
            
            if(VirtualPathUtility.GetFileName(item.VirtualPath).StartsWith("_"))
            {
                e.Cancel = true;
                e.ClientMessage = "Access Deny!";
            }
        }
    }
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div style="margin-top: 8px; margin-bottom: 8px;">
       Use SelectedItemsAction event to cancel navigation to specific folder or opening file.<br />
       Try to navigate to "_hidden" folder or open "_settings.txt" file.
    </div>
    <div>
        <iz:FileManager ID="FileManager1" runat="server" Height="400px" Width="600" ShowHiddenFilesAndFolders="True" HiddenFilesAndFoldersPrefix="_" OnSelectedItemsAction="FileManager1_SelectedItemsAction">
            <RootDirectories>
                <iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
            </RootDirectories>
        </iz:FileManager>
    
    </div>
</asp:Content>
