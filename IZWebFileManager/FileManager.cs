// Copyright (C) 2006 Igor Zelmanovich <izwebfilemanager@gmail.com>
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;
using IZ.WebFileManager.Components;
using System.Web;
using System.Collections.Specialized;
using System.Drawing.Design;
using Legend.Web;

namespace IZ.WebFileManager
{
    [ToolboxData("<{0}:FileManager runat=\"server\" width=\"400\" height=\"300\"></{0}:FileManager>")]
    public sealed class FileManager : FileManagerControlBase, IPostBackEventHandler
    {
        #region Fields

        FileView _fileView;
        FolderTree _folderTree;
        private FolderTree _selectFolderTree;
        ToolbarMenu _toolBar;
        BorderedPanelStyle _addressBarStyle;
        BorderedPanelStyle _addressTextBoxStyle;
        BorderedPanelStyle _toolbarStyle;
        BorderedPanelStyle _toolbarButtonStyle;
        BorderedPanelStyle _toolbarButtonHoverStyle;
        BorderedPanelStyle _toolbarButtonPressedStyle;
        ToolbarOptions _toolbarOptions;
        CustomToolbarButtonCollection _cusomToolbarButtonCollection;

        BorderedPanelStyle _defaultToolbarStyle;
        BorderedPanelStyle _defaultToolbarButtonStyle;
        BorderedPanelStyle _defaultToolbarButtonHoverStyle;
        BorderedPanelStyle _defaultToolbarButtonPressedStyle;

        AccessMode _defaultAccessMode = AccessMode.ReadOnly;

        private BorderedPanel enabledToolbarButton;
        private BorderedPanel disabledToolbarButton;

        #endregion

        #region Events

        [Category("Action")]
        public event CommandEventHandler ToolbarCommand;

        #endregion

        #region Properties

        [Themeable(false)]
        [Localizable(false)]
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowToolBar
        {
            get { return ViewState["ShowToolBar"] == null ? true : (bool)ViewState["ShowToolBar"]; }
            set { ViewState["ShowToolBar"] = value; }
        }

        [Themeable(false)]
        [Localizable(false)]
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowAddressBar
        {
            get { return ViewState["ShowAddressBar"] == null ? true : (bool)ViewState["ShowAddressBar"]; }
            set { ViewState["ShowAddressBar"] = value; }
        }

        [Themeable(false)]
        [Localizable(false)]
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowSearchBox
        {
            get { return ViewState["ShowSearchBox"] == null ? true : (bool)ViewState["ShowSearchBox"]; }
            set { ViewState["ShowSearchBox"] = value; }
        }

        [Themeable(false)]
        [Localizable(false)]
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowUploadBar
        {
            get { return ViewState["ShowUploadBar"] == null ? true : (bool)ViewState["ShowUploadBar"]; }
            set { ViewState["ShowUploadBar"] = value; }
        }

        [Category("Appearance")]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Localizable(false)]
        [Themeable(true)]
        public ToolbarOptions ToolbarOptions
        {
            get
            {
                if (_toolbarOptions == null)
                {
                    _toolbarOptions = new ToolbarOptions(ViewState);
                }
                return _toolbarOptions;
            }
        }

        [MergableProperty(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Behavior")]
        [Localizable(false)]
        [Themeable(false)]
        public CustomToolbarButtonCollection CustomToolbarButtons
        {
            get
            {
                if (_cusomToolbarButtonCollection == null)
                {
                    _cusomToolbarButtonCollection = new CustomToolbarButtonCollection();
                    if (IsTrackingViewState)
                        ((IStateManager)_cusomToolbarButtonCollection).TrackViewState();
                }
                return _cusomToolbarButtonCollection;
            }
        }

        [NotifyParentProperty(true)]
        [TypeConverter(typeof(WebColorConverter))]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "#316AC5")]
        public Color SelectedItemBackColor
        {
            get { return FileView.SelectedItemStyle.BackColor; }
            set
            {
                FileView.SelectedItemStyle.BackColor = value;
                FolderTree.SelectedNodeStyle.BackColor = value;
                SelectFolderTree.SelectedNodeStyle.BackColor = value;
                Controller.DynamicHoverStyle.BackColor = value;
            }
        }

        [NotifyParentProperty(true)]
        [TypeConverter(typeof(WebColorConverter))]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "#FFFFFF")]
        public Color SelectedItemForeColor
        {
            get { return FileView.SelectedItemStyle.ForeColor; }
            set
            {
                FileView.SelectedItemStyle.ForeColor = value;
                FolderTree.SelectedNodeStyle.ForeColor = value;
                SelectFolderTree.SelectedNodeStyle.ForeColor = value;
                Controller.DynamicHoverStyle.ForeColor = value;
            }
        }

        [NotifyParentProperty(true)]
        [TypeConverter(typeof(WebColorConverter))]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "")]
        public Color SelectedItemBorderColor
        {
            get { return FileView.SelectedItemStyle.BorderColor; }
            set
            {
                FileView.SelectedItemStyle.BorderColor = value;
                FolderTree.SelectedNodeStyle.BorderColor = value;
                SelectFolderTree.SelectedNodeStyle.BorderColor = value;
                Controller.DynamicHoverStyle.BorderColor = value;
            }
        }

        FileView FileView
        {
            get
            {
                EnsureChildControls();
                return _fileView;
            }
        }

        FolderTree FolderTree
        {
            get
            {
                EnsureChildControls();
                return _folderTree;
            }
        }

        FolderTree SelectFolderTree
        {
            get
            {
                EnsureChildControls();
                return _selectFolderTree;
            }
        }

        public override FileManagerItemInfo[] SelectedItems
        {
            get
            {
                return FileView.SelectedItems;
            }
        }

        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DefaultValue("")]
        [UrlProperty]
        [Bindable(true)]
        [Category("Appearance")]
        public string SplitterImageUrl
        {
            get { return ViewState["SplitterImageUrl"] == null ? String.Empty : (string)ViewState["SplitterImageUrl"]; }
            set { ViewState["SplitterImageUrl"] = value; }
        }

        // ContainerDirectory
        [DefaultValue("~/userfiles")]
        [Category("Data")]
        [Bindable(true)]
        [Localizable(false)]
        [Themeable(false)]
        public string MainDirectory
        {
            get { return ViewState["MainDirectory"] == null ? String.Empty : (string)ViewState["MainDirectory"]; }
            set { ViewState["MainDirectory"] = value; }
        }



        // TODO
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        internal BorderedPanelStyle AddressBarStyle
        {
            get
            {
                if (_addressBarStyle == null)
                {
                    _addressBarStyle = new BorderedPanelStyle();
                    _addressBarStyle.PaddingLeft = Unit.Pixel(3);
                    _addressBarStyle.PaddingTop = Unit.Pixel(2);
                    _addressBarStyle.PaddingBottom = Unit.Pixel(2);
                    _addressBarStyle.PaddingRight = Unit.Pixel(2);
                    if (IsTrackingViewState)
                        ((IStateManager)_addressBarStyle).TrackViewState();
                }
                return _addressBarStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style TextBoxStyle
        {
            get { return AddressTextBoxStyle; }
        }

        private BorderedPanelStyle AddressTextBoxStyle
        {
            get
            {
                if (_addressTextBoxStyle == null)
                {
                    _addressTextBoxStyle = new BorderedPanelStyle();
                    _addressTextBoxStyle.Font.Names = new string[] { "Tahoma", "Verdana", "Geneva", "Arial", "Helvetica", "sans-serif" };
                    _addressTextBoxStyle.Font.Size = FontUnit.Parse("11px", null);
                    _addressTextBoxStyle.BorderStyle = BorderStyle.Solid;
                    _addressTextBoxStyle.BorderWidth = Unit.Pixel(1);
                    _addressTextBoxStyle.BorderColor = Color.FromArgb(0xACA899);
                    _addressTextBoxStyle.PaddingLeft = Unit.Pixel(2);
                    _addressTextBoxStyle.PaddingTop = Unit.Pixel(2);
                    _addressTextBoxStyle.PaddingBottom = Unit.Pixel(2);
                    _addressTextBoxStyle.BackColor = Color.White;
                    if (IsTrackingViewState)
                        ((IStateManager)_addressTextBoxStyle).TrackViewState();
                }
                return _addressTextBoxStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public BorderedPanelStyle ToolbarStyle
        {
            get
            {
                if (_toolbarStyle == null)
                {
                    _toolbarStyle = new BorderedPanelStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_toolbarStyle).TrackViewState();
                }
                return _toolbarStyle;
            }
        }

        bool ToolbarStyleCreated
        {
            get { return _toolbarStyle != null; }
        }

        BorderedPanelStyle DefaultToolbarStyle
        {
            get
            {
                if (_defaultToolbarStyle == null)
                {
                    _defaultToolbarStyle = new BorderedPanelStyle();
                    _defaultToolbarStyle.ForeColor = Color.Black;
                    _defaultToolbarStyle.Font.Names = new string[] { "Tahoma", "Verdana", "Geneva", "Arial", "Helvetica", "sans-serif" };
                    _defaultToolbarStyle.Font.Size = FontUnit.Parse("11px", null);
                    _defaultToolbarStyle.Height = Unit.Pixel(24);
                    _defaultToolbarStyle.BackImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbg.gif");
                    _defaultToolbarStyle.BackColor = Color.FromArgb(0xEBEADB);
                    _defaultToolbarStyle.PaddingLeft = Unit.Pixel(3);
                    _defaultToolbarStyle.PaddingRight = Unit.Pixel(3);
                    _defaultToolbarStyle.PaddingTop = Unit.Pixel(2);
                }
                return _defaultToolbarStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public BorderedPanelStyle ToolbarButtonStyle
        {
            get
            {
                if (_toolbarButtonStyle == null)
                {
                    _toolbarButtonStyle = new BorderedPanelStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_toolbarButtonStyle).TrackViewState();
                }
                return _toolbarButtonStyle;
            }
        }

        bool ToolbarButtonStyleCreated
        {
            get { return _toolbarButtonStyle != null; }
        }

        BorderedPanelStyle DefaultToolbarButtonStyle
        {
            get
            {
                if (_defaultToolbarButtonStyle == null)
                {
                    _defaultToolbarButtonStyle = new BorderedPanelStyle();
                    _defaultToolbarButtonStyle.OuterBorderStyle = OuterBorderStyle.AllSides;
                    _defaultToolbarButtonStyle.OuterBorderTopWidth = Unit.Pixel(3);
                    _defaultToolbarButtonStyle.OuterBorderLeftWidth = Unit.Pixel(3);
                    _defaultToolbarButtonStyle.OuterBorderRightWidth = Unit.Pixel(3);
                    _defaultToolbarButtonStyle.OuterBorderBottomWidth = Unit.Pixel(3);
                    _defaultToolbarButtonStyle.ForeColor = Color.Black;
                }
                return _defaultToolbarButtonStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public BorderedPanelStyle ToolbarButtonHoverStyle
        {
            get
            {
                if (_toolbarButtonHoverStyle == null)
                {
                    _toolbarButtonHoverStyle = new BorderedPanelStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_toolbarButtonHoverStyle).TrackViewState();
                }
                return _toolbarButtonHoverStyle;
            }
        }

        bool ToolbarButtonHoverStyleCreated
        {
            get { return _toolbarButtonHoverStyle != null; }
        }

        BorderedPanelStyle DefaultToolbarButtonHoverStyle
        {
            get
            {
                if (_defaultToolbarButtonHoverStyle == null)
                {
                    _defaultToolbarButtonHoverStyle = new BorderedPanelStyle();
                    _defaultToolbarButtonHoverStyle.ForeColor = Color.Black;
                    _defaultToolbarButtonHoverStyle.BackColor = Color.FromArgb(0xf5f5ef);
                    _defaultToolbarButtonHoverStyle.OuterBorderLeftBottomCornerImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtnhover_LB.gif");
                    _defaultToolbarButtonHoverStyle.OuterBorderRightBottomCornerImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtnhover_RB.gif");
                    _defaultToolbarButtonHoverStyle.OuterBorderLeftTopCornerImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtnhover_LT.gif");
                    _defaultToolbarButtonHoverStyle.OuterBorderRightTopCornerImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtnhover_RT.gif");
                    _defaultToolbarButtonHoverStyle.OuterBorderTopImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtnhover_T.gif");
                    _defaultToolbarButtonHoverStyle.OuterBorderBottomImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtnhover_B.gif");
                    _defaultToolbarButtonHoverStyle.OuterBorderLeftImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtnhover_L.gif");
                    _defaultToolbarButtonHoverStyle.OuterBorderRightImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtnhover_R.gif");
                }
                return _defaultToolbarButtonHoverStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public BorderedPanelStyle ToolbarButtonPressedStyle
        {
            get
            {
                if (_toolbarButtonPressedStyle == null)
                {
                    _toolbarButtonPressedStyle = new BorderedPanelStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_toolbarButtonPressedStyle).TrackViewState();
                }
                return _toolbarButtonPressedStyle;
            }
        }

        bool ToolbarButtonPressedStyleCreated
        {
            get { return _toolbarButtonPressedStyle != null; }
        }

        BorderedPanelStyle DefaultToolbarButtonPressedStyle
        {
            get
            {
                if (_defaultToolbarButtonPressedStyle == null)
                {
                    _defaultToolbarButtonPressedStyle = new BorderedPanelStyle();
                    _defaultToolbarButtonPressedStyle.ForeColor = Color.Black;
                    _defaultToolbarButtonPressedStyle.BackColor = Color.FromArgb(0xe3e3db);
                    _defaultToolbarButtonPressedStyle.OuterBorderLeftBottomCornerImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtndown_LB.gif");
                    _defaultToolbarButtonPressedStyle.OuterBorderRightBottomCornerImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtndown_RB.gif");
                    _defaultToolbarButtonPressedStyle.OuterBorderLeftTopCornerImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtndown_LT.gif");
                    _defaultToolbarButtonPressedStyle.OuterBorderRightTopCornerImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtndown_RT.gif");
                    _defaultToolbarButtonPressedStyle.OuterBorderTopImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtndown_T.gif");
                    _defaultToolbarButtonPressedStyle.OuterBorderBottomImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtndown_B.gif");
                    _defaultToolbarButtonPressedStyle.OuterBorderLeftImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtndown_L.gif");
                    _defaultToolbarButtonPressedStyle.OuterBorderRightImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.toolbarbtndown_R.gif");
                }
                return _defaultToolbarButtonPressedStyle;
            }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(false)]
        [Localizable(false)]
        public bool UseLinkToOpenItem
        {
            get { return FileView.UseLinkToOpenItem; }
            set { FileView.UseLinkToOpenItem = value; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [Localizable(false)]
        public bool EnableContextMenu
        {
            get { return FileView.EnableContextMenu; }
            set { FileView.EnableContextMenu = value; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("_blank")]
        [Localizable(false)]
        public string LinkToOpenItemTarget
        {
            get { return FileView.LinkToOpenItemTarget; }
            set { FileView.LinkToOpenItemTarget = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(FileViewRenderMode.Icons)]
        [Localizable(false)]
        public FileViewRenderMode FileViewMode
        {
            get { return FileView.View; }
            set { FileView.View = value; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(AccessMode.ReadOnly)]
        [Localizable(false)]
        public AccessMode DefaultAccessMode
        {
            get { return _defaultAccessMode; }
            set
            {
                if (value == AccessMode.Default)
                    value = AccessMode.ReadOnly;
                _defaultAccessMode = value;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style FileViewStyle
        {
            get { return FileView.ControlStyle; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public BorderedPanelStyle DetailsColumnHeaderStyle
        {
            get { return FileView.DetailsColumnHeaderStyle; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style DetailsSortedColumnStyle
        {
            get { return FileView.DetailsSortedColumnStyle; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style FolderTreeStyle
        {
            get { return FolderTree.ControlStyle; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public SubMenuStyle DynamicMenuStyle
        {
            get { return Controller.DynamicMenuStyle; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style SelectFolderTreeStyle
        {
            get { return SelectFolderTree.ControlStyle; }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("[0]/")]
        [Localizable(false)]
        [Themeable(false)]
        public string Directory
        {
            get { return FileView.Directory; }
            set { FileView.Directory = value; }
        }

        public override FileManagerItemInfo CurrentDirectory
        {
            get
            {
                return FileView.CurrentDirectory;
            }
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (ShowToolBar)
                CreateToolbar();
            RegisterSplitterClientScript();
            RegisterLayoutSetupScript();

            Page.Form.Enctype = "multipart/form-data";

            Page.ClientScript.RegisterExpandoAttribute(_fileView.ClientID, "rootControl", ClientID);

            if (ShowFileUploadBar)
                Page.ClientScript.RegisterClientScriptBlock(typeof(FileManager), "file-upload-bar",
@"

function FileManager_UploadFile(e, name) {
    var lc = e.parentNode.parentNode;
    var td = lc.parentNode;
    var div = document.createElement('DIV');
    td.insertBefore(div, lc);

    var remove = " + FileManagerController.JavaScriptSerializer.Serialize(HttpUtility.HtmlEncode(Controller.GetResourceString("Upload_File_Remove", "Remove"))) + @"
    div.innerHTML='<input type=""file"" name=""' + name + '"" /> <a href=""javascript:void(0)"" onclick=""FileManager_RemoveUploasFile(this)"">' + remove + '</a>';

    FileManager_UpdateFileUploadCount(td, 1);
};

function FileManager_RemoveUploasFile(e) {
    var div = e.parentNode;
    var td = div.parentNode;
    td.removeChild(div);

    FileManager_UpdateFileUploadCount(td, -1);
};

function FileManager_UpdateFileUploadCount(td, i) {
    if(!td.file_upload_counter)
        td.file_upload_counter = { count: 0 };
    td.file_upload_counter.count = td.file_upload_counter.count + i;
    
    var links = FileManager_GetChildByTagName(td, 'DIV', td.file_upload_counter.count);
    var link1 = FileManager_GetChildByTagName(links, 'DIV', 0);
    var link2 = FileManager_GetChildByTagName(links, 'DIV', 1);
    var submit = FileManager_GetChildByTagName(td.parentNode, 'DIV', 1);
    if(td.file_upload_counter.count>0) {
        submit.style.visibility = 'visible';
        link1.style.display = 'none';
        link2.style.display = 'block';
    } else {
        submit.style.visibility = 'hidden';
        link1.style.display = 'block';
        link2.style.display = 'none';
    }
}

function FileManager_GetChildByTagName(element, tagName, index) {
    var childs = element.childNodes;
    var upperTagName = tagName.toUpperCase();
    var currIndex = 0;
    for(i in childs) {
        var child = childs[i];
        if(child.tagName && child.tagName.toUpperCase() == upperTagName) {
            if(currIndex == index)
                return child;
            currIndex = currIndex + 1;
        }
    }
} 

", true);

            Page.ClientScript.RegisterStartupScript(typeof(FileManager), "select-folder", @"
window['" + _fileView.ClientScriptReference + @"_SelectedFolderPath'] = ['" + String.Join("','", Controller.GetPathHashCodes(GetCurrentDirectory().FileManagerPath)) + @"'];
window['" + Controller.ClientScriptReference + @"PromptDirectory'] =  function(selectedFolder, callback) {

    var selectFolderPanel = document.getElementById('" + ClientID + @"_SelectFolderPanel');

    window['" + _selectFolderTree.ClientScriptReference + @"_OnSelect'] = function(arg) {
        selectedFolder = arg;
    };

    document.getElementById('" + ClientID + @"_SelectFolderOK').onclick = function() {
        callback(selectedFolder);
        selectFolderPanel.style.display = 'none';
    };
    document.getElementById('" + ClientID + @"_SelectFolderCancel').onclick = function() {
        callback(null);
        selectFolderPanel.style.display = 'none';
    };

    var selectFolderTree = window['" + _selectFolderTree.ClientScriptReference + @"']
    var roots = ['" + String.Join("','", RootDirectories.Cast<RootDirectory>().Select(x => Controller.GetPathHashCode("/" + x.TextInternal)).ToArray()) + @"'];
    var selected = window['" + _fileView.ClientScriptReference + @"_SelectedFolderPath'];

    var pos = WebForm_GetElementPosition(document.getElementById('" + ClientID + @"'));
    selectFolderPanel.style.display = 'block';
    WebForm_SetElementX(selectFolderPanel, pos.x);
    WebForm_SetElementY(selectFolderPanel, pos.y);
    WebForm_SetElementHeight(selectFolderPanel, pos.height);
    WebForm_SetElementWidth(selectFolderPanel, pos.width);

    for(var i=0; i<roots.length; i++){
        if(roots[i] != selected[0])
            selectFolderTree.Refresh(roots[i]);
    }
    selectFolderTree.RequireRefresh (selected);
    selectFolderTree.Navigate (selected, 0);
}
", true);
        }

        private void RegisterLayoutSetupScript()
        {
            StringBuilder sb = new StringBuilder();
            if (!ShowAddressBar)
                sb.AppendLine("var addressBarHeight = 0;");
            else
                sb.AppendLine("var addressBarHeight = WebForm_GetElementPosition(WebForm_GetElementById('" + ClientID + "_AddressBar')).height;");
            if (!ShowToolBar)
                sb.AppendLine("var toolBarHeight = 0;");
            else
                sb.AppendLine("var toolBarHeight = WebForm_GetElementPosition(WebForm_GetElementById('" + ClientID + "_ToolBar')).height;");
            if (ReadOnly || !AllowUpload || !ShowUploadBar)
                sb.AppendLine("var uploadBarHeight = 0;");
            else
                sb.AppendLine("var uploadBarHeight = WebForm_GetElementPosition(WebForm_GetElementById('" + ClientID + "_UploadBar')).height;");
            sb.AppendLine("var fileManagerHeight = WebForm_GetElementPosition(WebForm_GetElementById('" + ClientID + "')).height;");
            sb.AppendLine("var requestedHeight = fileManagerHeight - addressBarHeight - toolBarHeight - uploadBarHeight;");
            sb.AppendLine("WebForm_SetElementHeight(WebForm_GetElementById('" + _fileView.ClientID + "'), requestedHeight);");
            sb.AppendLine("WebForm_SetElementHeight(WebForm_GetElementById('" + _folderTree.ClientID + "'), requestedHeight);");
            Page.ClientScript.RegisterStartupScript(typeof(FileManager), ClientID + "LayoutSetup", sb.ToString(), true);

        }

        private void RegisterSplitterClientScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("var __fileView;");
            sb.AppendLine("var __fileViewWidth;");
            sb.AppendLine("var __folderTree;");
            sb.AppendLine("var __folderTreeWidth;");
            sb.AppendLine("var __clientX;");
            sb.AppendLine("var __document_onmousemove;");
            sb.AppendLine("var __document_onmouseup;");
            sb.AppendLine("function " + ClientID + "SplitterDragStart(e) {");
            sb.AppendLine("if(e == null) var e = event;");
            sb.AppendLine("__fileView = WebForm_GetElementById('" + _fileView.ClientID + "');");
            sb.AppendLine("__fileViewWidth = WebForm_GetElementPosition(__fileView).width;");
            sb.AppendLine("__folderTree = WebForm_GetElementById('" + _folderTree.ClientID + "');");
            sb.AppendLine("__folderTreeWidth = WebForm_GetElementPosition(__folderTree).width;");
            sb.AppendLine("__clientX = e.clientX;");
            sb.AppendLine("__document_onmousemove = document.onmousemove;");
            sb.AppendLine("__document_onmouseup = document.onmouseup;");
            sb.AppendLine("document.onmousemove = " + ClientID + "SplitterDrag;");
            sb.AppendLine("document.onmouseup = " + ClientID + "SplitterDragEnd;");
            sb.AppendLine("return false;");
            sb.AppendLine("}");
            sb.AppendLine("function " + ClientID + "SplitterDragEnd(e) {");
            sb.AppendLine("document.onmousemove = __document_onmousemove;");
            sb.AppendLine("document.onmouseup = __document_onmouseup;");
            sb.AppendLine("return false;");
            sb.AppendLine("}");
            sb.AppendLine("function " + ClientID + "SplitterDrag(e) {");
            sb.AppendLine("if(e == null) var e = event;");
            if (Controller.IsRightToLeft)
                sb.AppendLine("var __delta = __clientX - e.clientX;");
            else
                sb.AppendLine("var __delta = e.clientX - __clientX;");
            sb.AppendLine("WebForm_SetElementWidth(__fileView, __fileViewWidth - __delta);");
            sb.AppendLine("WebForm_SetElementWidth(__folderTree, __folderTreeWidth + __delta);");
            sb.AppendLine("return false;");
            sb.AppendLine("}");
            Page.ClientScript.RegisterClientScriptBlock(typeof(FileManager), ClientID + "SplitterDrag", sb.ToString(), true);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateFileView();
            CreateFolderTree();
            CreateSelectFolderDialog();
        }

        private void CreateSelectFolderDialog()
        {
            _selectFolderTree = new FolderTree(Controller);
            _selectFolderTree.ID = "SelectFolderTree";
            _selectFolderTree.Width = 250;
            _selectFolderTree.Height = 300;
            Controls.Add(_selectFolderTree);
        }

        private void CreateFolderTree()
        {
            _folderTree = new FolderTree(Controller, _fileView);
            _folderTree.ID = "FolderTree";
            Controls.Add(_folderTree);
        }

        private void CreateFileView()
        {
            _fileView = new FileView(Controller);
            _fileView.ID = "FileView";
            Controls.Add(_fileView);
        }

        private void CreateToolbar()
        {
            _toolBar = new ToolbarMenu(
                ClientID + "tb",
                Controller.CurrentUICulture.TextInfo.IsRightToLeft,
                RenderToolbarItem,
                RenderToolbarPopupItem,
                Controller.DynamicMenuStyle,
                Controller.DynamicMenuItemStyle,
                Controller.DynamicHoverStyle);

            Controls.Add(enabledToolbarButton = CreateToolbarButton(true));
            Controls.Add(disabledToolbarButton = CreateToolbarButton(false));

            string clientClickFunction = "javascript:" + FileManagerController.ClientScriptObjectNamePrefix + Controller.ClientID + ".On{0}(" + FileManagerController.ClientScriptObjectNamePrefix + _fileView.ClientID + ", '{1}');return false;";

            // Copy to
            if (ToolbarOptions.ShowCopyButton)
            {
                MenuItem itemCopyTo = new MenuItem();
                itemCopyTo.Text = Controller.GetResourceString("Copy", "Copy");
                itemCopyTo.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Copy);
                itemCopyTo.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.SelectedItemsCopyTo, "");
                itemCopyTo.Enabled = !ReadOnly;
                _toolBar.Items.Add(itemCopyTo);
            }

            // Move to
            if (ToolbarOptions.ShowMoveButton)
            {
                MenuItem itemMoveTo = new MenuItem();
                itemMoveTo.Text = Controller.GetResourceString("Move", "Move");
                itemMoveTo.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Move);
                itemMoveTo.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.SelectedItemsMoveTo, "");
                itemMoveTo.Enabled = !ReadOnly && AllowDelete;
                _toolBar.Items.Add(itemMoveTo);
            }

            // Delete
            if (ToolbarOptions.ShowDeleteButton)
            {
                MenuItem itemDelete = new MenuItem();
                itemDelete.Text = Controller.GetResourceString("Delete", "Delete");
                itemDelete.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Delete);
                itemDelete.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.SelectedItemsDelete, "");
                itemDelete.Enabled = !ReadOnly && AllowDelete;
                _toolBar.Items.Add(itemDelete);
            }

            // Rename
            if (ToolbarOptions.ShowRenameButton)
            {
                MenuItem itemRename = new MenuItem();
                itemRename.Text = Controller.GetResourceString("Rename", "Rename");
                itemRename.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Rename);
                itemRename.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.Rename, "");
                itemRename.Enabled = !ReadOnly && AllowDelete;
                _toolBar.Items.Add(itemRename);
            }

            // NewFolder
            if (ToolbarOptions.ShowNewFolderButton)
            {
                MenuItem itemNewFolder = new MenuItem();
                itemNewFolder.Text = Controller.GetResourceString("Create_New_Folder", "New Folder");
                itemNewFolder.ImageUrl = Controller.GetToolbarImage(ToolbarImages.NewFolder);
                itemNewFolder.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.NewFolder, "");
                itemNewFolder.Enabled = !ReadOnly;
                _toolBar.Items.Add(itemNewFolder);
            }

            // FolderUp
            if (ToolbarOptions.ShowFolderUpButton)
            {
                MenuItem itemFolderUp = new MenuItem();
                itemFolderUp.Text = Controller.GetResourceString("Up", "Up");
                itemFolderUp.ImageUrl = Controller.GetToolbarImage(ToolbarImages.FolderUp);
                itemFolderUp.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewNavigate, "..");
                _toolBar.Items.Add(itemFolderUp);
            }

            // View
            if (ToolbarOptions.ShowViewButton)
            {
                MenuItem itemView = new MenuItem();
                itemView.Text = Controller.GetResourceString("View", "View");
                itemView.ImageUrl = Controller.GetToolbarImage(ToolbarImages.View);
                itemView.NavigateUrl = "javascript: return;";
                _toolBar.Items.Add(itemView);

                // Icons
                MenuItem itemViewIcons = new MenuItem();
                itemViewIcons.Text = Controller.GetResourceString("Icons", "Icons");
                itemViewIcons.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewChangeView, FileViewRenderMode.Icons);
                itemView.ChildItems.Add(itemViewIcons);

                // Details
                MenuItem itemViewDetails = new MenuItem();
                itemViewDetails.Text = Controller.GetResourceString("Details", "Details");
                itemViewDetails.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewChangeView, FileViewRenderMode.Details);
                itemView.ChildItems.Add(itemViewDetails);

                // Thumbnails
                MenuItem itemViewThumbnails = new MenuItem();
                itemViewThumbnails.Text = Controller.GetResourceString("Thumbnails", "Thumbnails");
                itemViewThumbnails.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewChangeView, FileViewRenderMode.Thumbnails);
                itemView.ChildItems.Add(itemViewThumbnails);
            }

            // Refresh
            if (ToolbarOptions.ShowRefreshButton)
            {
                MenuItem itemRefresh = new MenuItem();
                itemRefresh.Text = Controller.GetResourceString("Refresh", "Refresh");
                itemRefresh.Value = "Refresh";
                itemRefresh.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Refresh);
                itemRefresh.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.Refresh, "");
                _toolBar.Items.Add(itemRefresh);
            }

            // Custom Buttons
            if (_cusomToolbarButtonCollection != null)
            {
                for (int i = 0; i < _cusomToolbarButtonCollection.Count; i++)
                {
                    CustomToolbarButton button = _cusomToolbarButtonCollection[i];
                    string postBackStatement = null;
                    if (button.PerformPostBack)
                    {
                        postBackStatement = Page.ClientScript.GetPostBackEventReference(this, "Toolbar:" + i) + ";theForm.__EVENTTARGET.value = '';theForm.__EVENTARGUMENT.value = '';";
                    }
                    MenuItem item = new MenuItem()
                    {
                        Text = button.Text,
                        ImageUrl = button.ImageUrl,
                        NavigateUrl = "javascript:" + button.OnClientClick + ";" + postBackStatement + ";return false;"
                    };
                    _toolBar.Items.Add(item);
                }
            }
        }

        void RenderToolbarPopupItem(HtmlTextWriter writer, MenuItem menuItem, int index)
        {
            writer
                .Table(e => e.Cellpadding(0).Cellspacing(0).Border(0).Cursor("default").Onclick(menuItem.NavigateUrl))
                    .Tr()
                        .Td()
                            .Img(e => e.Width(16).Height(16).Src(Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.Empty.gif"))).EndTag()
                        .EndTag()
                        .Td(e => e.PaddingLeft(2).PaddingRight(2).Width("100%"))
                            .Text("&nbsp;" + menuItem.Text)
                        .EndTag()
                        .Td()
                            .Img(e => e.Width(16).Height(16).Src(Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.Empty.gif"))).EndTag()
                        .EndTag()
                    .EndTag()
                .EndTag();
        }

        class ToolbarButtonPanel : BorderedPanel
        {
            private readonly Control owner;

            public ToolbarButtonPanel(Control owner)
            {
                this.owner = owner;
            }

            public override string OuterBorderHoverImagesArrayName
            {
                get { return owner.ClientID + "tbHover"; }

            }

            public override string OuterBorderPressedImagesArrayName
            {
                get { return owner.ClientID + "tbPressed"; }
            }
        }

        private BorderedPanel CreateToolbarButton(bool enabled)
        {
            var panel = new ToolbarButtonPanel(this);
            panel.ControlStyle.CopyFrom(DefaultToolbarButtonStyle);
            if (ToolbarButtonStyleCreated)
                panel.ControlStyle.CopyFrom(ToolbarButtonStyle);
            panel.Style[HtmlTextWriterStyle.Cursor] = "default";
            if (enabled)
            {
                panel.HoverSyle.CopyFrom(DefaultToolbarButtonHoverStyle);
                if (ToolbarButtonHoverStyleCreated)
                    panel.HoverSyle.CopyFrom(ToolbarButtonHoverStyle);
                panel.PressedSyle.CopyFrom(DefaultToolbarButtonPressedStyle);
                if (ToolbarButtonPressedStyleCreated)
                    panel.PressedSyle.CopyFrom(ToolbarButtonPressedStyle);
            }
            else
                panel.Style["color"] = "gray";

            return panel;
        }

        private void RenderToolbarItem(HtmlTextWriter writer, MenuItem menuItem, int index)
        {
            BorderedPanel panel;

            if (menuItem.Enabled)
            {
                panel = enabledToolbarButton;
                panel.Attributes["onclick"] = menuItem.NavigateUrl;
            }
            else
            {
                panel = disabledToolbarButton;
            }

            panel.ID = "tb" + index;
            panel.RenderBeginTag(writer);

            writer
                .Table(e => e.Cellpadding(0).Cellspacing(0).Border(0))
                    .Tr()
                        .Td()
                            .Img(e => e.Src(ResolveClientUrl(menuItem.ImageUrl))).EndTag()
                        .EndTag()
                        .Td(e => e.PaddingLeft(2).PaddingRight(2).WhiteSpace("nowrap"))
                            .Text("&nbsp;" + menuItem.Text)
                        .EndTag()
                    .EndTag()
                .EndTag();

            panel.RenderEndTag(writer);
        }

        protected override Style CreateControlStyle()
        {
            Style style = base.CreateControlStyle();
            style.BackColor = Color.FromArgb(0xEDEBE0);
            return style;
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "default");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                writer.Write(String.Format(System.Globalization.CultureInfo.InvariantCulture,
                "<div><table width=\"{0}\" height=\"{1}\" bgcolor=\"#f5f5f5\" bordercolor=\"#c7c7c7\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\"><tr><td valign=\"middle\" align=\"center\">IZWebFileManager - <b>{2}</b></td></tr></table></div>",
                    Width,
                    Height,
                    ID));
                return;
            }

            AddAttributesToRender(writer);
            RenderBeginOuterTable(writer);
            if (ShowAddressBar)
                RenderAddressBar(writer);
            if (ShowToolBar)
                RenderToolBar(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            RenderFolderTree(writer);

            writer.RenderEndTag();

            // splitter
            writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "col-resize");
            writer.AddAttribute("onmousedown", ClientID + "SplitterDragStart(event)");
            writer.AddAttribute("onselectstart", "return false");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            if (SplitterImageUrl.Length > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Src, ResolveClientUrl(SplitterImageUrl));
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Width, "3px");
                writer.AddAttribute(HtmlTextWriterAttribute.Height, "3px");
                writer.AddAttribute(HtmlTextWriterAttribute.Src, Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.Empty.gif"));
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            RenderFileView(writer);

            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            if (ShowFileUploadBar)
                RenderFileUploadBar(writer);
            RenderEndOuterTable(writer);

            var style = new Style();
            style.CopyFrom(ControlStyle);
            style.Width = Unit.Empty;
            style.Height = Unit.Empty;

            writer.Div(attr => attr.Id(ClientID + "_SelectFolderPanel").Position("fixed").Display("none").ZIndex(100).BackgroundColor("rgba(0,0,0, 0.2)").Style("filter", "progid:DXImageTransform.Microsoft.gradient(startColorstr=#33000000,endColorstr=#33000000)"))
                .Div(attr => attr.Style(style).Display("inline-block").Position("relative").Left("30%").Top("10%").Padding(2).Direction(Controller.IsRightToLeft))
                    .Div(attr => attr.Padding(5))
                        .Text(GetResourceString("SelectDestination", "Select destination directory"), true)
                    .EndTag()
                    .RenderControl(_selectFolderTree)
                    .Div(attr => attr.Align(Controller.IsRightToLeft ? "left" : "right"))
                        .Button(attr => attr.Id(ClientID + "_SelectFolderOK").Attr("type", "button"))
                            .Text(GetResourceString("Select", "Select"), true)
                        .EndTag()
                        .Button(attr => attr.Id(ClientID + "_SelectFolderCancel").Attr("type", "button"))
                            .Text(GetResourceString("Cancel", "Cancel"), true)
                        .EndTag()
                    .EndTag()
                .EndTag()
            .EndTag();
        }

        private bool ShowFileUploadBar
        {
            get { return !ReadOnly && AllowUpload && ShowUploadBar; }
        }

        private void RenderToolBar(HtmlTextWriter writer)
        {
            var p = new BorderedPanel();
            p.Page = Page;
            p.ControlStyle.CopyFrom(DefaultToolbarStyle);
            if (ToolbarStyleCreated)
                p.ControlStyle.CopyFrom(ToolbarStyle);

            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "relative");
            writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, "100");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_ToolBar");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            p.RenderBeginTag(writer);
            _toolBar.Render(writer);
            p.RenderEndTag(writer);
            writer.RenderEndTag();
        }

        private void RenderFolderTree(HtmlTextWriter writer)
        {
            _folderTree.Width = new Unit(Width.Value * 0.25, Width.Type);
            _folderTree.Height = 100;
            _folderTree.RenderControl(writer);
        }

        private void RenderFileView(HtmlTextWriter writer)
        {
            _fileView.Width = new Unit(Width.Value * 0.75, Width.Type);
            _fileView.Height = 100;
            _fileView.RenderControl(writer);
        }

        private void RenderAddressBar(HtmlTextWriter writer)
        {
            AddressBarStyle.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_AddressBar");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            RenderTextBox(writer, "_Address", CurrentDirectory.FileManagerPath, null,
                Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.go.png"));

            if (ShowSearchBox)
            {
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "130px");
                RenderTextBox(writer, "_Search", FileView.SearchTerm, Controller.GetResourceString("SearchWatermark", "Search"),
                    Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.search.png"),
                    Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.clear.png")
                    );
            }

            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "16px");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, Controller.GetToolbarImage(ToolbarImages.Process));
            writer.AddAttribute(HtmlTextWriterAttribute.Id, _fileView.ClientID + "_ProgressImg");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Visibility, "hidden");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.RenderEndTag();
        }

        private void RenderTextBox(HtmlTextWriter writer, string name, string value, string watermark, params string[] icons)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, "2px");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);

            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "relative");
            writer.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, (16 * icons.Length) + "px");
            AddressTextBoxStyle.AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            var textBoxStyle = new BorderedPanelStyle();
            textBoxStyle.Font.MergeWith(AddressTextBoxStyle.Font);
            textBoxStyle.ForeColor = AddressTextBoxStyle.ForeColor;
            writer.AddAttribute(HtmlTextWriterAttribute.Id, _fileView.ClientID + name);
            writer.AddAttribute(HtmlTextWriterAttribute.Value, value, true);
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Margin, "0");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
            textBoxStyle.AddAttributesToRender(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "relative");
            writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, "2");
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "transparent");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            for (var i = 0; i < icons.Length; i++)
            {
                var icon = icons[i];
                var i1 = i;
                writer
                    .A(x => x.Id(_fileView.ClientID + name + i1).Width(16).Height(16).Href("javascript:;").Display("none").Position("absolute").Right(16 * i1).Bottom(AddressTextBoxStyle.PaddingBottom))
                        .Img(x => x.Border(0).Src(icon))
                        .EndTag()
                    .EndTag();
            }

            if (!String.IsNullOrEmpty(watermark))
            {
                writer
                    .Span(x => x.Id(_fileView.ClientID + name + "Watermark").Position("absolute").ZIndex(1).Top(AddressTextBoxStyle.PaddingTop).Left(AddressTextBoxStyle.PaddingLeft).Color(Color.FromArgb(0xACA899)).Italic().Cursor("text"))
                        .Text(watermark)
                    .EndTag();

            }

            writer.RenderEndTag(); // div

            writer.RenderEndTag(); // td
        }

        private void RenderFileUploadBar(HtmlTextWriter writer)
        {
            var onclick = "FileManager_UploadFile(this, '" + ClientID + "_Upload" + "');";
            var left = Controller.IsRightToLeft ? "right" : "left";
            var right = Controller.IsRightToLeft ? "left" : "right";

            writer
                .Div(e => e.Style(AddressBarStyle).Attr(HtmlTextWriterAttribute.Id, ClientID + "_UploadBar"))
                    .Div(e => e.Float(left))
                        .Div()
                            .Div()
                                .A(e => e.Href("javascript:void(0);").Onclick(onclick))
                                    .Text(HttpUtility.HtmlEncode(Controller.GetResourceString("Upload_File", "Upload File")))
                                .EndTag()
                            .EndTag()
                            .Div(e => e.Display("none"))
                                .A(e => e.Href("javascript:void(0);").Onclick(onclick))
                                    .Text(HttpUtility.HtmlEncode(Controller.GetResourceString("Upload_Another_File", "Upload Another File")))
                                .EndTag()
                            .EndTag()
                        .EndTag()
                    .EndTag()
                    .Div(e => e.Float(right).Visibility("hidden"))
                        .Input(e => e
                            .Attr(HtmlTextWriterAttribute.Type, "button")
                            .Attr(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, "Upload"))
                            .Attr(HtmlTextWriterAttribute.Value, Controller.GetResourceString("Submit", "Submit")))
                        .EndTag()
                    .EndTag()
                    .Div(e => e.Clear("both")).EndTag()
                .EndTag();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void RenderBeginOuterTable(HtmlTextWriter writer)
        {
            Controller.AddDirectionAttribute(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);
            writer.AddAttribute(HtmlTextWriterAttribute.Valign, "top");
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void RenderEndOuterTable(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        protected override object SaveViewState()
        {
            object[] states = new object[6];

            states[0] = base.SaveViewState();
            if (_toolbarStyle != null)
                states[1] = ((IStateManager)_toolbarStyle).SaveViewState();
            if (_toolbarButtonStyle != null)
                states[2] = ((IStateManager)_toolbarButtonStyle).SaveViewState();
            if (_toolbarButtonHoverStyle != null)
                states[3] = ((IStateManager)_toolbarButtonHoverStyle).SaveViewState();
            if (_toolbarButtonPressedStyle != null)
                states[4] = ((IStateManager)_toolbarButtonPressedStyle).SaveViewState();
            if (_cusomToolbarButtonCollection != null)
                states[5] = ((IStateManager)_cusomToolbarButtonCollection).SaveViewState();

            for (int i = 0; i < states.Length; i++)
            {
                if (states[i] != null)
                    return states;
            }
            return null;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
                return;

            object[] states = (object[])savedState;

            base.LoadViewState(states[0]);
            if (states[1] != null)
                ((IStateManager)ToolbarStyle).LoadViewState(states[1]);
            if (states[2] != null)
                ((IStateManager)ToolbarButtonStyle).LoadViewState(states[2]);
            if (states[3] != null)
                ((IStateManager)ToolbarButtonHoverStyle).LoadViewState(states[3]);
            if (states[4] != null)
                ((IStateManager)ToolbarButtonPressedStyle).LoadViewState(states[4]);
            if (states[5] != null)
                ((IStateManager)CustomToolbarButtons).LoadViewState(states[5]);
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();

            if (_toolbarStyle != null)
                ((IStateManager)_toolbarStyle).TrackViewState();
            if (_toolbarButtonStyle != null)
                ((IStateManager)_toolbarButtonStyle).TrackViewState();
            if (_toolbarButtonHoverStyle != null)
                ((IStateManager)_toolbarButtonHoverStyle).TrackViewState();
            if (_toolbarButtonPressedStyle != null)
                ((IStateManager)_toolbarButtonPressedStyle).TrackViewState();
            if (_cusomToolbarButtonCollection != null)
                ((IStateManager)_cusomToolbarButtonCollection).TrackViewState();
        }

        void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == "Upload")
            {
                var dir = GetCurrentDirectory();
                for (var i = 0; i < Page.Request.Files.Count; i++)
                {
                    if (Page.Request.Files.GetKey(i) == ClientID + "_Upload")
                    {
                        var postedFile = Page.Request.Files[i];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {
                            Controller.ProcessFileUpload(dir, postedFile);
                        }
                    }
                }
            }
            else if (eventArgument.StartsWith("Toolbar:", StringComparison.Ordinal))
            {
                int i = int.Parse(eventArgument.Substring(8));
                CustomToolbarButton button = CustomToolbarButtons[i];
                OnToolbarCommand(new CommandEventArgs(button.CommandName, button.CommandArgument));
            }
        }

        void OnToolbarCommand(CommandEventArgs e)
        {
            if (ToolbarCommand != null)
                ToolbarCommand(this, e);
        }

        internal override FileManagerItemInfo GetCurrentDirectory()
        {
            return Controller.ResolveFileManagerItemInfo(_fileView.Directory);
        }

        #region IPostBackEventHandler Members

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            RaisePostBackEvent(eventArgument);
        }

        #endregion
    }
}
