<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" Title="Untitled Page" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="margin-top: 8px; margin-bottom: 8px;">
    Use built-in "XP" style or customize it using Style Properties
    </div>
    <div>
        <iz:FileManager ID="FileManager1" runat="server" Height="400" Width="600">
        <RootDirectories>
        <iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
        </RootDirectories>
        </iz:FileManager>
    
    </div>
</asp:Content>

