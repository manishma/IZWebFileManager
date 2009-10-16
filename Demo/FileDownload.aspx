<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="FileDownload.aspx.cs" Inherits="FileDownload" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
	<asp:CheckBox ID="ProhibitDownload" runat="server" AutoPostBack="True" 
		Checked="True" Text="Prohibit download" />
&nbsp; (uncheck to allow)
</div>
<div>
<asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
</div>
		<iz:FileManager ID="FileManager1" runat="server" Height="400" Width="600" 
			onfiledownload="FileManager1_FileDownload">
			<RootDirectories>
				<iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
			</RootDirectories>
		</iz:FileManager>
</asp:Content>

