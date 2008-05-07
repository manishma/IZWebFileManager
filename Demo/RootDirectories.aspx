<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="~/RootDirectories.aspx.cs" Inherits="RootDirectories" Title="Untitled Page" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div>
		<iz:filemanager id="FileManager1" runat="server" height="400" width="600">
        </iz:filemanager>
	</div>
</asp:Content>
