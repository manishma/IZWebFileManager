<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="~/RootDirectories.aspx.cs" Inherits="RootDirectories" Title="Untitled Page" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="padding: 10px;">
        Select directory to view:
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
            <asp:ListItem Value="/">My Documents</asp:ListItem>
            <asp:ListItem Value="/My Pictures">My Pictures</asp:ListItem>
            <asp:ListItem Value="/_hidden">_hidden</asp:ListItem>
        </asp:DropDownList>
    </div>
	<div>
		<iz:filemanager id="FileManager1" runat="server" height="400" width="600">
        </iz:filemanager>
	</div>
</asp:Content>
