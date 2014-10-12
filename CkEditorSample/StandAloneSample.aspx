<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="StandAloneSample.aspx.cs" Inherits="StandAloneSample" %>

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
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <div class="jumbotron">
        <div class="container">
            <h2>File Browser stand alone</h2>
            <p>Use File Browse to fill your own field.</p>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">
    <div class="row">
        <div class="col-lg-6">
            <div class="page-header">
                <h1>Open File Browser in a new window</h1>
            </div>
            <p>
                This sample show how to use File Browser opening it in a new browser window (with
                <em>window.open</em> method)
            </p>
            <div class="form-group">
                <div class="input-group">
                    <input type="text" class="form-control input-lg" id="newWinField">
                    <span class="input-group-btn">
                        <button class="btn btn-primary btn-lg" type="button" id="newWinBtn">Choose a file</button>
                    </span>
                </div>
            </div>
            <!-- /input-group -->
            <p>
                FileBrowser.aspx is open in a new window with three parameters: fn (the name of the callback function), 
                caller (the window to which the function belong - opener for the opener window) and (optional) langCode.  
            </p>
            <pre class="prettyprint">
&lt;script&gt>
    /* Note.: jQuery SHOULD BE LOADED for the examples on this page are working properly */

    //CallBack function
    function newWinFn(fileurl) {
        $('#newWinField').val(fileurl);
    }
    $(function () {
        // Button click event
        $('#newWinBtn').on('click', function (e) {
            e.preventDefault();
            var top = window.screenTop + 50;
            var left = window.screenLeft + 50;
            window.open('/FileBrowser/FileBrowser.aspx?caller=opener&fn=newWinFn&langCode=en', 'fileBrowser', 'top=' + top + ',left=' + left + ',menubar=0,scrollbars=0,toolbar=0,height=550,width=700');
        })
    });
&lt;/script&gt;
            </pre>
        </div>
        <!-- /.col-lg-6 -->
        <div class="col-lg-6">
            <div class="page-header">
                <h1>Open File Browser in a modal dialog</h1>
            </div>
            <p>
                This sample show how to use File Browser opening it in a bootstrap modal 
                dialog using iframe.
            </p>
            <div class="form-group">
                <div class="input-group">
                    <input type="text" class="form-control input-lg" id="modalField">
                    <span class="input-group-btn">
                        <a class="btn btn-primary btn-lg" href="#FileBrowserModal" data-toggle="modal">Choose a file</a>
                    </span>
                </div>
            </div>
            <!-- /input-group -->
            <p>
                FileBrowser.aspx is hosted in a iframe element with three parameters: fn (the name of the callback function), 
                caller (the window to which the function belong - parent for parent of iframe) and (optional) langCode.  
            </p>
            <pre class="prettyprint">
&lt;div class="modal fade" id="FileBrowserModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
    aria-hidden="true"&gt;
    &lt;div class="modal-dialog modal-lg"&gt;
        &lt;div class="modal-content"&gt;
            &lt;div class="modal-header"&gt;
                &lt;button type="button" class="close" data-dismiss="modal"&gt;
                    &lt;span aria-hidden="true"&gt;&times;&lt;/span&gt;&lt;span
                        class="sr-only"&gt;Close&lt;/span&gt;&lt;/button&gt;
                &lt;h4 class="modal-title" id="H1"&gt;File browser&lt;/h4&gt;
            &lt;/div&gt;
            &lt;div class="modal-body"&gt;
                &lt;iframe id="FB_frame"&gt;&lt;/iframe&gt;
            &lt;/div&gt;
        &lt;/div&gt;
    &lt;/div&gt;
&lt;/div&gt;
&lt;script&gt>
    /* Note.: jQuery SHOULD BE LOADED for the examples on this page are working properly */

        function fileBrowserCallBack(fileurl) {
            $('#modalField').val(fileurl);
            $('#FileBrowserModal').modal('hide');
        }

        // Loading of iframe content is postposed to not slow down page loading
        $(function () {
            $(window).on('load', function () {
                $('#FB_frame').attr('src', '/FileBrowser/FileBrowser.aspx?caller=parent&fn=fileBrowserCallBack&langCode=en');
            });
        })
&lt;/script&gt;
            </pre>
        </div>
        <!-- /.col-lg-6 -->
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
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="Server">
    <script>
        /* Note.: jQuery SHOULD BE LOADED for the examples on this page are working properly */

        //CallBack function
        function newWinFn(fileurl) {
            $('#newWinField').val(fileurl);
        }
        $(function () {
            // Button click event
            $('#newWinBtn').on('click', function (e) {
                e.preventDefault();
                var top = window.screenTop + 50;
                var left = window.screenLeft + 50;
                window.open('/FileBrowser/FileBrowser.aspx?caller=opener&fn=newWinFn&langCode=en', 'fileBrowser', 'top=' + top + ',left=' + left + ',menubar=0,scrollbars=0,toolbar=0,height=550,width=700');
            })
        });
    </script>
    <script>
        function fileBrowserCallBack(fileurl) {
            $('#modalField').val(fileurl);
            $('#FileBrowserModal').modal('hide');
        }

        // Loading of iframe content is postposed to not slow down page loading
        $(function () {
            $(window).on('load', function () {
                $('#FB_frame').attr('src', '/FileBrowser/FileBrowser.aspx?caller=parent&fn=fileBrowserCallBack&langCode=en');
            });
        })
    </script>
</asp:Content>

