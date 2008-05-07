<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" %>
<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div style="margin-top: 8px; margin-bottom: 8px;">
    Use ImagesFolder, ...ImageUrl and SpecialFolders properties to customize your icon theme
    </div>
    <div>
        <iz:FileManager ID="FileManager1" runat="server" Height="400px" Width="600" FileSmallImageUrl="~/Tango/16x16/text-x-generic.png" FolderSmallImageUrl="~/Tango/16x16/folder.png" RootFolderSmallImageUrl="~/Tango/16x16/folder-remote.png" FileLargeImageUrl="~/Tango/32x32/text-x-generic.png" FolderLargeImageUrl="~/Tango/32x32/folder.png">
        <RootDirectories>
        <iz:RootDirectory DirectoryPath="~/Files/My Documents" Text="My Documents" SmallImageUrl="~/Tango/16x16/user-home.png" />
        </RootDirectories>
        <SpecialFolders>
        <iz:SpecialFolder DirectoryPath="~/Files/My Documents" SmallImageUrl="~/Tango/16x16/user-home.png" LargeImageUrl="~/Tango/32x32/user-home.png" />
        <iz:SpecialFolder DirectoryPath="~/Files/My Documents/Favorites" SmallImageUrl="~/Tango/16x16/emblem-favorite.png" LargeImageUrl="~/Tango/32x32/emblem-favorite.png" />
        <iz:SpecialFolder DirectoryPath="~/Files/My Documents/My Pictures" SmallImageUrl="~/Tango/16x16/emblem-photos.png" LargeImageUrl="~/Tango/32x32/emblem-photos.png" />
        </SpecialFolders>
        </iz:FileManager>
    
    </div>
</asp:Content>
