<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .modal-body
        {
            padding: 0 2px 3px;
        }

        #FB_frame
        {
            display: block;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            width: 100%;
            height: 500px;
            border: 0;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="HeaderContent" runat="server" ID="Header">
    <div class="jumbotron">
        <div class="container">
            <h1>IZ Web File Browser <small>Revisited</small></h1>
            <p>
                Using this open source server control to add file browsing capability to custom
                form and to CkEditor and TinyMCE WYSIWYG html editors.
            </p>
            <p>
                <small id="callBackMsg">Instant demo: open File Browser and doubleclick on file icons
                    or rigth-click and select <em>select</em> option from pop-up menu   
                to choose a file.</small>
            </p>
            <p>
                <a class="btn btn-primary btn-lg" href="#FileBrowserModal" data-toggle="modal">Open
                    File Browser</a>
            </p>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <div class="row">
        <div class="col-sm-4 text-center">
            <h1>Stand alone </h1>
            <p><big>Use IZ Web File Browser in your ASP.net 3.5 applications.</big></p>
            <div class="row">
                <div class="col-xs-8 col-xs-offset-2">
                    <p>
                        <img src="/img/NET.png" alt="" class="img-responsive center-block" />
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <a href="/StandAloneSample.aspx" class="btn btn-primary">Read more...</a>
                </div>
            </div>
        </div>
        <div class="col-sm-4 text-center">
            <h1>CKEditor </h1>
            <p><big>Add File Browsing capability to CKEditor for free.</big></p>
             <div class="row">
                <div class="col-xs-10 col-xs-offset-1">
                    <p>
                        <img src="/img/logo-ckeditor-h100.png" alt="" class="img-responsive center-block" />
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <a href="/CkEditorSample.aspx" class="btn btn-primary">Read more...</a>
                </div>
            </div>
        </div>
        <div class="col-sm-4 text-center">
            <h1>Tiny MCE</h1>
            <p><big>Add File Browsing capability to Tiny MCE for free</big></p>
             <div class="row">
                <div class="col-xs-6 col-xs-offset-3">
                    <p>
                        <img src="/img/tinymce_logo.png" alt="" class="img-responsive center-block" />
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <a href="#" class="btn btn-primary">Read more...</a>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="FileBrowserModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span
                            class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">File browser</h4>
                </div>
                <div class="modal-body">
                    <iframe id="FB_frame"></iframe>
                </div>
                <%--                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
    <button type="button" class="btn btn-primary">Save changes</button>
                </div>--%>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="Server">
    <script>
        function fileBrowserCallBack(filename) {
            $('#callBackMsg').html('Instant demo: you choose <a href ="' + filename + '">' + filename + '</a>');
            $('#FileBrowserModal').modal('hide');
        }

        $(function () {
            $(window).on('load', function () {
                $('#FB_frame').attr('src', '/FileBrowser/FileBrowser.aspx?caller=parent&fn=fileBrowserCallBack&langCode=en');
            });
        })
    </script>
</asp:Content>

