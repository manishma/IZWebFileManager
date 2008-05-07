<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div style="margin-top: 8px; margin-bottom: 8px;">
    Use ImagesFolder, ...ImageUrl and SpecialFolders properties to customize your icon theme
    </div>
    <div>
        <iz:FileManager ID="FileManager1" runat="server" Height="400px" Width="600" ImagesFolder="~/Nuvola/icons">
        <RootDirectories>
        <iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" SmallImageUrl="~/Nuvola/16x16/folder_home.gif" />
        </RootDirectories>
        <SpecialFolders>
        <iz:SpecialFolder DirectoryPath="~/Files/My Documents" SmallImageUrl="~/Nuvola/16x16/folder_home.gif" LargeImageUrl="~/Nuvola/32x32/folder_home.gif" />
        <iz:SpecialFolder DirectoryPath="~/Files/My Documents/Favorites" SmallImageUrl="~/Nuvola/16x16/folder_favorite.gif" LargeImageUrl="~/Nuvola/32x32/folder_favorite.gif" />
        <iz:SpecialFolder DirectoryPath="~/Files/My Documents/My Pictures" SmallImageUrl="~/Nuvola/16x16/folder_photo.gif" LargeImageUrl="~/Nuvola/32x32/folder_photo.gif" />
        </SpecialFolders>
        </iz:FileManager>
    
    </div>
</asp:Content>
