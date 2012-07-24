<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" %>

<%@ Register Assembly="IZ.WebFileManager" Namespace="IZ.WebFileManager" TagPrefix="iz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
<script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.21/jquery-ui.min.js"></script>

<link rel="stylesheet" href="http://code.jquery.com/ui/1.8.21/themes/base/jquery-ui.css" type="text/css" media="all" />

<script type="text/javascript">
    $(function () {
        $("#pnuploadimage_main").dialog({
            title: 'Select image to preview',
            autoOpen: false,
            resizable: false,
            show: "blind",
            hide: "explode",
            width: "auto",
            height: "auto",
            open: function () {
                $('#<%= DialogIsOpen.ClientID %>').val('1');
            },
            close: function () {
                $('#<%= DialogIsOpen.ClientID %>').val('0');
            }
        });
        $("#opener").click(function () {
            $("#pnuploadimage_main").dialog("open");
            return false;
        });

        var uploadBtn = $('#<%= FileManager1.ClientID %>_UploadBar input:button')[0];
        var f = uploadBtn.onclick;
        uploadBtn.onclick = function () {
            $("#pnuploadimage_main").dialog('destroy').appendTo($(theForm));
            f();
        };

        if($('#<%= DialogIsOpen.ClientID %>').val() == '1')
            $("#pnuploadimage_main").dialog("open");
            
    });

     function OpenItem(path) {
         $('#preview').attr('src', path);
         $("#pnuploadimage_main").dialog("close");
     };

</script>

    <div style="margin-top: 8px; margin-bottom: 8px;">
        <a href="javascript:;" id="opener">Select image to preview</a>
    </div>
    <div>
        <img id="preview" />
    </div>

    <asp:HiddenField runat="server" ID="DialogIsOpen" Value="0" />
    <div id="pnuploadimage_main" class="ModalWindow">
        <iz:FileManager ID="FileManager1" runat="server" Height="400" Width="600" FileViewMode="Thumbnails" ClientOpenItemFunction="OpenItem">
            <RootDirectories>
                <iz:RootDirectory DirectoryPath="~/Files/My Documents/My Pictures" Text="My Pictures" />
            </RootDirectories>
        </iz:FileManager>
    </div>
</asp:Content>
