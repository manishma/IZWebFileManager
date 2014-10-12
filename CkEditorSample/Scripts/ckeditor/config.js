/// <reference path="../../FileBrowser/FileBrowser.aspx" />
/// <reference path="../../FileBrowser/FileBrowser.aspx" />
/**
 * @license Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.html or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
    config.filebrowserBrowseUrl = 'http:/FileBrowser/FileBrowser.aspx?type=files&lang=it-IT';
    config.filebrowserImageBrowseUrl = 'http:/FileBrowser/FileBrowser.aspx?type=images&lang=it-IT';
    config.filebrowserFlashBrowseUrl = '/FileBrowser/FileBrowser.aspx?type=flash&lang=it-IT';
	config.plugins = 'dialogui,dialog,about,a11yhelp,dialogadvtab,basicstyles,bidi,blockquote,clipboard,button,panelbutton,panel,floatpanel,colorbutton,colordialog,templates,menu,contextmenu,div,resize,toolbar,elementspath,enterkey,entities,popup,filebrowser,find,fakeobjects,flash,floatingspace,listblock,richcombo,font,forms,format,horizontalrule,htmlwriter,iframe,wysiwygarea,image,indent,smiley,justify,link,indentlist,list,liststyle,magicline,maximize,newpage,pagebreak,pastetext,pastefromword,preview,print,removeformat,save,selectall,showblocks,showborders,sourcearea,specialchar,menubutton,scayt,stylescombo,tab,table,tabletools,undo,wsc';
	config.skin = 'bootstrapck';
};
