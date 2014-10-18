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
            <h1>IZ Web File Manager <small>Revisited</small></h1>
            <p>
                Add file browsing capability to custom
                form and to CkEditor and TinyMCE html editors.
            </p>
            <p>
                <small id="callBackMsg">Instant demo: open File Browser and doubleclick 
                    or rigth-click <em>select</em> on a file  
                    to choose it.</small>
            </p>
            <p>
                <a class="btn btn-primary btn-lg" href="#FileBrowserModal" data-toggle="modal">Open
                    File Browser</a>
                <a class="btn btn-warning btn-lg" href="https://github.com/magicbruno/IZWebFileManager/tree/master/CkEditorSample"
                    data-toggle="modal">Download</a>
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
    <hr />
    <div class="row">
        <div class="col-xs-10 col-xs-offset-1 text-center">
            <div class="page-header">
                <h1>Just a File Browser for ASP.NET 3.5</h1>
            </div>
            <article>

                <p>
                    This project is based on  <a href="https://github.com/manishma/IZWebFileManager">IZWebFileManager by
                        Manishima</a>. It proposes some small changes to the original project
                    and demostates how use IZWebFileManager with <a href="https://github.com/ckeditor">CkEditor</a>
                    and <a href="http://www.tinymce.com/">Tiny MCE</a>.
                </p>

                <h2>License</h2>

                <p>
                    My code is delivered under the free software/open source GNU General Public License
                    (commonly known as the "GPL"). Form more informaztion about original IZWebFileManager
                    license model refer to <a href="http://www.izwebfilemanager.com/">project page</a>.
                </p>

                <h2>Changes log</h2>

                <div class="task-list">
                    <p>
                        <strong>Default image in png format</strong>. In IZWebFileManager you may define
                        a folder containing icons used for buttons, generic folders and generic file. In
                        version 2.8.1 only gif images was recognized. I have added png image support.
                    </p>
                    <p>
                        <strong>Custom Thumnbnail Manager</strong>. I have added <em>CustomThumbnailHandler</em>
                        property to the control which allow to define a custom handler for images thumbnails.</p>
                </div>

                <h3>Added features:</h3>

                <div class="task-list">
                    <p>
                        <strong>Custom Upload Manager</strong>. In CkEditor Sample a Custom File Manager
                        is used (based upon popular FineUploader) that implement drag and drop functionality.
                    </p>
                    <p>
                        <strong>HTML Editors interface</strong>. File Browser is ready to use. You need
                        only to configure your preferred editor for an custom file browser (specific instructions
                        and samples are provided). Go to site for more informations.</p>
                    <p>
                        <strong>Custom toolbar button</strong>. I added to File Manager toolbar a custom
                        button the show images in a bootstrap modal.</p>
                </div>

                <h2>Notes</h2>

                <p>The project use various external resources:</p>

                <div class="task-list">
                    <p>
                        <strong>jQuery</strong> (version 1.11.1 is included)<br>
                    </p>
                    <p>
                        <strong>Bootstrap Framework</strong> . A subset of the  css framework version 3.2.0.1
                        was generated (and compressed) from the less files (provided). You could easy customize
                        the subset changing and recompiling /CkEditorSample/Content/FileBrowser/FileBrowser.less.
                    </p>
                    <p>
                        <strong>Javascript plugin</strong>. All plugins are bundled and minimized in /FileManager/js/FileManager.min.js.
                        See /FileManager/js/FileManager.js.bundle file for details. Bundling and compressing
                        was handled using <strong>Web Essentials</strong> a popular free Visual Studio Extension.
                    </p>
                </div>
            </article>
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

