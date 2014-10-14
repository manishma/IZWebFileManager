<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TinyMCE.aspx.cs" Inherits="TiniMCE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Scripts/tinymce/tinymce.min.js"></script>
    <script>
        tinymce.init({
            selector: "textarea#content",
            theme: "modern",
            height: 300,
            plugins: [
                 "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                 "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                 "save table contextmenu directionality emoticons template paste textcolor"
            ],
            content_css: "css/metro-bootstrap.min.css",
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons",
            file_browser_callback: function (field, url, type, win) {
                tinyMCE.activeEditor.windowManager.open({
                    url: '/FileBrowser/FileBrowser.aspx?caller=tinymce4&langCode=en&type=' + type,
                    title: 'File Browser',
                    width: 700,
                    height: 500,
                    inline: true,
                    close_previous: false
                }, {
                    window: win,
                    field: field
                });
                return false;
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContent" runat="Server">
    <div class="jumbotron">
        <div class="container">
            <h2 class="samples">Tiny MCE 4
            </h2>
            <p>Adding file browsing capability to Tiny MCE</p>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">
    <div>
        <p>This example displays all plugins that comes with the TinyMCE package.</p>
        <p>To test File Browser click on "Image", "Link" or "Media" toolbar icons and then click on "Browse" icon right after the URL field.
From the File Browser popup window select a file with double click or "select" right click option.</p>
        <textarea name="content" id="content" style="width: 100%">
        &lt;h1&gt;&lt;img style="float: right;" title="TinyMCE Logo" src="/userfiles/images/n2_tn.jpg" alt="TinyMCE Logo" height="80" width="92" /&gt;Welcome to the TinyMCE editor demo!&lt;/h1&gt;
&lt;p&gt;Feel free to try out the different features that are provided, please note that the &lt;b&gt;MoxieManager&lt;/b&gt; specific functionality is part of our commercial offering. The demo is to show the integration.&lt;/p&gt;
&lt;h2&gt;Got questions or need help?&lt;/h2&gt;
&lt;p&gt;If you have questions or need help, feel free to visit our &lt;a href="../forum/index.php"&gt;community forum&lt;/a&gt;! We also offer Enterprise &lt;a href="../enterprise/support.php"&gt;support&lt;/a&gt; solutions. Also do not miss out on the &lt;a href="../wiki.php"&gt;documentation&lt;/a&gt;, its a great resource wiki for understanding how TinyMCE works and integrates.&lt;/p&gt;
&lt;h2&gt;Found a bug?&lt;/h2&gt;
&lt;p&gt;If you think you have found a bug, you can use the &lt;a href="../develop/bugtracker.php"&gt;Bug Tracker&lt;/a&gt; to report bugs to the developers.&lt;/p&gt;
&lt;p&gt;And here is a simple table for you to play with.&lt;/p&gt;
&lt;table border="0"&gt;&lt;tbody&gt;&lt;tr&gt;
&lt;td&gt;&lt;strong&gt;Product&lt;/strong&gt;&lt;/td&gt;
&lt;td&gt;&lt;strong&gt;Cost&lt;/strong&gt;&lt;/td&gt;
&lt;td&gt;&lt;strong&gt;Really?&lt;/strong&gt;&lt;/td&gt;
&lt;/tr&gt;&lt;tr&gt;
&lt;td&gt;TinyMCE&lt;/td&gt;
&lt;td&gt;Free&lt;/td&gt;
&lt;td&gt;YES!&lt;/td&gt;
&lt;/tr&gt;&lt;tr&gt;
&lt;td&gt;Plupload&lt;/td&gt;
&lt;td&gt;Free&lt;/td&gt;
&lt;td&gt;YES!&lt;/td&gt;
&lt;/tr&gt;&lt;/tbody&gt;&lt;/table&gt;
&lt;p&gt;Enjoy our software and create great content!&lt;/p&gt;
&lt;p&gt;Oh, and by the way, don't forget to check out our other product called &lt;a href="http://www.plupload.com" target="_blank"&gt;Plupload&lt;/a&gt;, your ultimate upload solution with HTML5 upload support!&lt;/p&gt;

        </textarea>
    </div>
    <div>
        <p>To use File Browser with Tiny MCE you must correctly "init" the editor configuring file_browser_callback like in the example below.</p>
    </div>
    <pre class="prettyprint">
&lt;head&gt;
    .......
    &lt;script src="Scripts/tinymce/tinymce.min.js"&gt;&lt;/script&gt;
    &lt;script&gt;
        tinymce.init({
            selector: "textarea#content",
            theme: "modern",
            height: 300,
            plugins: [
                    "advlist autolink link image lists charmap print preview hr anchor pagebreak spellchecker",
                    "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
                    "save table contextmenu directionality emoticons template paste textcolor"
            ],
            content_css: "css/metro-bootstrap.min.css",
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image | print preview media fullpage | forecolor backcolor emoticons",
            file_browser_callback: function (field, url, type, win) {
                tinyMCE.activeEditor.windowManager.open({
                    url: '/FileBrowser/FileBrowser.aspx?caller=tinymce4&langCode=en&type=' + type,
                    title: 'File Browser',
                    width: 700,
                    height: 500,
                    inline: true,
                    close_previous: false
                }, {
                    window: win,
                    field: field
                });
                return false;
            }

        });
    &lt;/script&gt;
    .....
&lt;/head&gt;
    </pre>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Scripts" runat="Server">
</asp:Content>

