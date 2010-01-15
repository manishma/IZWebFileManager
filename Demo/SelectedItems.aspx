<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="SelectedItems.aspx.cs" Inherits="SelectedItems" %>

<%@ Register TagPrefix="iz" Namespace="IZ.WebFileManager" Assembly="IZ.WebFileManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
	<div>
		<iz:FileManager AllowUpload="false" ID="FileManager1" runat="server" Height="400" Width="600">
			<RootDirectories>
				<iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
			</RootDirectories>
		</iz:FileManager>
	</div>
	<div style="padding:5px;">
		<asp:Label runat="server" ID="SelectedItemsLog" />
	</div>
	<div style="padding:5px;">
		<asp:Button runat="server" Text="Log current directory" OnClick="LogCurrentDirectory" />
		<asp:Button runat="server" Text="Log selected items" OnClick="LogSelected" />
		<asp:FileUpload runat="server" ID="FileUpload1" /><asp:Button runat="server" Text="Upload file" OnClick="Upload"/>
	</div>
</asp:Content>
