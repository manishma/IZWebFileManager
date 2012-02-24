<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Title="Untitled Page" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<script runat="server">
</script>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div style="margin-top: 8px; margin-bottom: 8px;">
    Viewing this page requires authentication.
    </div>
    <div>
        <iz:FileManager ID="FileManager1" runat="server" Height="400" Width="600">
            <RootDirectories>
                <iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
            </RootDirectories>
        </iz:FileManager>
    </div>
</asp:Content>
