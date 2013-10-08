<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
	CodeFile="Programming.aspx.cs" Inherits="Programming" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <div style="margin-top: 8px; margin-bottom: 8px;">
        In this example IZWebFileManager instance is created dynamically in Page_Load.
    </div>
	<div>
		<asp:PlaceHolder runat="server" ID="PlaceHolder1"></asp:PlaceHolder>
	</div>
</asp:Content>
