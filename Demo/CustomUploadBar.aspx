<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomUploadBar.aspx.cs" Inherits="CustomUploadBar" MasterPageFile="~/MasterPage.master" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div style="margin-top: 8px; margin-bottom: 8px;">
        Use your own upload file control
    </div>
    <div>
        <iz:FileManager ID="FileManager1" runat="server" Height="400px" Width="600" ProhibitedFiles="bat, exe" ShowUploadBar="false">
            <RootDirectories>
                <iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" />
            </RootDirectories>
        </iz:FileManager>
    </div>
    <div style="width:600px; border: 1px solid #666666;">
        Select file to upload:<br />
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <asp:Button runat="server" ID="Button1" Text="<%$ Resources: IZWebFileManagerResource, Upload_File  %>" OnClick="Button1_Click" />
    </div>
</asp:Content>
