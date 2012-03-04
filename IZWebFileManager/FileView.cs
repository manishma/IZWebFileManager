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
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IZ.WebFileManager.Components;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Collections.Specialized;
using Legend.Web;

namespace IZ.WebFileManager
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:FileView runat=\"server\" width=\"400\" height=\"300\"></{0}:FileView>")]
    public sealed class FileView : FileManagerControlBase
    {
        #region Constructors

        public FileView() : base() { InitializeDefaults(); }
        public FileView(FileManagerController controller) : base(controller) { InitializeDefaults(); }

        #endregion

        #region Fields

        BorderedPanelStyle _detailsColumnHeaderStyle;
        Style _editTextBoxStyle;
        Style _itemStyle;
        Style _selectedItemStyle;
        Style _detailsSortedColumnStyle;
        StringBuilder _initScript = new StringBuilder();
        ContextMenu _contextMenu;
        ContextMenu _selectedItemsContextMenu;

        // control state fields
        FileViewRenderMode _view = FileViewRenderMode.Icons;
        SortMode _sort = SortMode.Name;
        SortDirection _sortDirection;
        bool _showInGroups;
        string _directory;
        FileManagerItemInfo _currentDirectory;

        #endregion

        #region Properties

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public BorderedPanelStyle DetailsColumnHeaderStyle
        {
            get
            {
                if (_detailsColumnHeaderStyle == null)
                {
                    _detailsColumnHeaderStyle = new BorderedPanelStyle();
                    _detailsColumnHeaderStyle.Height = Unit.Pixel(17);
                    _detailsColumnHeaderStyle.BackColor = Color.FromArgb(0xebeadb);
                    _detailsColumnHeaderStyle.BackImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.detailscolumnheader_RB.gif");
                    _detailsColumnHeaderStyle.OuterBorderLeftImageUrl = Page.ClientScript.GetWebResourceUrl(GetType(), "IZ.WebFileManager.resources.detailscolumnheader_R.gif");
                    _detailsColumnHeaderStyle.Font.Bold = false;
                    _detailsColumnHeaderStyle.PaddingLeft = Unit.Pixel(4);
                    _detailsColumnHeaderStyle.PaddingRight = Unit.Pixel(4);
                    _detailsColumnHeaderStyle.PaddingTop = Unit.Pixel(3);
                    _detailsColumnHeaderStyle.OuterBorderStyle = OuterBorderStyle.Left;
                    _detailsColumnHeaderStyle.OuterBorderLeftWidth = Unit.Pixel(2);

                    if (IsTrackingViewState)
                        ((IStateManager)_detailsColumnHeaderStyle).TrackViewState();
                }
                return _detailsColumnHeaderStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style DetailsSortedColumnStyle
        {
            get
            {
                if (_detailsSortedColumnStyle == null)
                {
                    _detailsSortedColumnStyle = new Style();
                    _detailsSortedColumnStyle.BackColor = Color.FromArgb(0xF7F7F7);

                    if (IsTrackingViewState)
                        ((IStateManager)_detailsSortedColumnStyle).TrackViewState();
                }
                return _detailsSortedColumnStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style SelectedItemStyle
        {
            get
            {
                if (_selectedItemStyle == null)
                {
                    _selectedItemStyle = new Style();
                    _selectedItemStyle.ForeColor = Color.White;
                    _selectedItemStyle.BackColor = Color.FromArgb(0x316AC5);

                    if (IsTrackingViewState)
                        ((IStateManager)_selectedItemStyle).TrackViewState();
                }
                return _selectedItemStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style ItemStyle
        {
            get
            {
                if (_itemStyle == null)
                {
                    _itemStyle = new Style();

                    if (IsTrackingViewState)
                        ((IStateManager)_itemStyle).TrackViewState();
                }
                return _itemStyle;
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Category("Appearance")]
        public Style EditTextBoxStyle
        {
            get
            {
                if (_editTextBoxStyle == null)
                {
                    _editTextBoxStyle = new Style();
                    _editTextBoxStyle.BorderStyle = BorderStyle.Solid;
                    _editTextBoxStyle.BorderWidth = Unit.Pixel(1);
                    _editTextBoxStyle.BorderColor = Color.Black;
                    _editTextBoxStyle.Font.Names = new string[] { "Tahoma", "Verdana", "Geneva", "Arial", "Helvetica", "sans-serif" };
                    _editTextBoxStyle.Font.Size = FontUnit.Parse("11px", null);

                    if (IsTrackingViewState)
                        ((IStateManager)_editTextBoxStyle).TrackViewState();
                }
                return _editTextBoxStyle;
            }
        }

        [Bindable(true)]
        [Category("Data")]
        [DefaultValue("[0]/")]
        [Localizable(false)]
        [Themeable(false)]
        public string Directory
        {
            get { return _directory == null ? "[0]/" : _directory; }
            internal set
            {
                _directory = value;
                _currentDirectory = null;
            }
        }

        internal string SearchTerm { get; set; }

        string[] selectedItemsStr;
        FileManagerItemInfo[] selectedItems;
        public override FileManagerItemInfo[] SelectedItems
        {
            get
            {
                if (selectedItems == null)
                {
                    if (selectedItemsStr == null || selectedItemsStr.Length == 0)
                        selectedItems = new FileManagerItemInfo[0];
                    else
                    {
                        ArrayList selectedItemsArray = new ArrayList();
                        FileManagerItemInfo dir = GetCurrentDirectory();
                        foreach (string name in selectedItemsStr)
                        {
                            FileManagerItemInfo itemInfo = ResolveFileManagerItemInfo(name);
                            selectedItemsArray.Add(itemInfo);
                        }
                        selectedItems = (FileManagerItemInfo[])selectedItemsArray.ToArray(typeof(FileManagerItemInfo));
                    }
                }
                return selectedItems;
            }
        }

        public override FileManagerItemInfo CurrentDirectory
        {
            get
            {
                if (_currentDirectory == null)
                    _currentDirectory = Controller.ResolveFileManagerItemInfo(Directory);
                return _currentDirectory;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Localizable(false)]
        private bool ShowInGroups
        {
            get { return _showInGroups; }
            set { _showInGroups = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(FileViewRenderMode.Icons)]
        [Localizable(false)]
        public FileViewRenderMode View
        {
            get { return _view; }
            set { _view = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(SortMode.Name)]
        [Localizable(false)]
        public SortMode Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(SortDirection.Ascending)]
        [Localizable(false)]
        public SortDirection SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(false)]
        [Localizable(false)]
        public bool UseLinkToOpenItem
        {
            get { return (bool)(ViewState["UseLinkToOpenItem"] ?? false); }
            set { ViewState["UseLinkToOpenItem"] = value; }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        [Localizable(false)]
        public bool EnableContextMenu
        {
            get { return ViewState.GetValue("EnableContextMenu", true); }
            set { ViewState.SetValue("EnableContextMenu", value); }
        }

        [Bindable(true)]
        [Category("Behavior")]
        [DefaultValue("_blank")]
        [Localizable(false)]
        public string LinkToOpenItemTarget
        {
            get { return (string)(ViewState["LinkToOpenItemTarget"] ?? "_blank"); }
            set { ViewState["LinkToOpenItemTarget"] = value; }
        }

        internal string LinkToOpenItemClass
        {
            get { return "linkToOpenItem" + ClientID; }
        }

        #endregion

        #region Methods

        private void InitializeDefaults()
        {
            LinkToOpenItemTarget = "_blank";
        }

        protected internal override string RenderContents()
        {
            StringBuilder sb = new StringBuilder();
            HtmlTextWriter writer = new HtmlTextWriter(new StringWriter(sb, null));
            RenderContents(writer);
            return sb.ToString();
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (DesignMode)
                return;

            var directoryInfo = GetCurrentDirectory().Directory;

            var render = FileViewRender.GetRender(this);
            var provider = new DirectoryProvider(directoryInfo, Sort, SortDirection, SearchTerm);

            render.RenderBeginList(writer);

            foreach (FileSystemInfo fsi in provider.GetFileSystemInfos())
            {
                var item = new FileViewItem(directoryInfo, fsi, this);

                if (!ShowHiddenFilesAndFolders && item.Hidden)
                    continue;

                render.RenderItem(writer, item);
            }

            render.RenderEndList(writer);
            RenderInitScript(writer);
        }

        private void RenderInitScript(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_InitScript");
            output.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            output.AddStyleAttribute(HtmlTextWriterStyle.Visibility, "hidden");
            output.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.Write(_initScript.ToString());
            output.Write(FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".Items = new Array(" + String.Join(",", (string[])itemIds.ToArray(typeof(string))) + ");\r\n");
            output.Write(FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".ClearSelectedItems();\r\n");
            output.RenderEndTag();
        }

        internal override FileManagerItemInfo GetCurrentDirectory()
        {
            return Controller.ResolveFileManagerItemInfo(Directory);
        }

        int _itemIndex;
        ArrayList itemIds = new ArrayList();
        internal void RenderItemBeginTag(HtmlTextWriter output, FileViewItem item)
        {
            string id = ClientID + "_Item_" + _itemIndex;
            item.ClientID = id;

            int fileType = -2; //is Directory
            if (item.FileSystemInfo is FileInfo)
            {
                FileInfo file = (FileInfo)item.FileSystemInfo;
                FileType ft = Controller.GetFileType(file);
                if (ft != null)
                    fileType = Controller.FileTypes.IndexOf(ft);
                else
                    fileType = -1;
            }

            itemIds.Add(id);

            output.AddAttribute(HtmlTextWriterAttribute.Id, id);
            output.RenderBeginTag(HtmlTextWriterTag.Div);

            // trace init script 
            _initScript.AppendLine("var " + id + " = document.getElementById('" + id + "');");
            _initScript.AppendLine(FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".InitItem(" + id + ",'" + FileManagerController.EncodeURIComponent(item.RelativePath) + "'," + (item.IsDirectory ? "true" : "false") + "," + (item.CanBeRenamed ? "true" : "false") + "," + "false" + "," + fileType + ");");

            _itemIndex++;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        internal void RenderItemEndTag(HtmlTextWriter output)
        {
            output.RenderEndTag();
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            if (EnableContextMenu)
            {
                _contextMenu.Render(writer);
                _selectedItemsContextMenu.Render(writer);
            }

            RenderFocusTextBox(writer);
            RenderEditTextBox(writer);

            base.RenderBeginTag(writer);
        }

        void RenderEditTextBox(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
            writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, "100");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Visibility, "hidden");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, Controller.ClientID + "_TextBox");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        private void RenderFocusTextBox(HtmlTextWriter writer)
        {
            writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "1px");
            writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, "transparent");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "absolute");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_Focus");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }


        internal override FileManagerItemInfo ResolveFileManagerItemInfo(string path)
        {
            string fileManagerPath;
            if (String.IsNullOrEmpty(path))
                fileManagerPath = Directory;
            else if (path[0] == '[' || path[0] == '/')
                fileManagerPath = path;
            else
                fileManagerPath = VirtualPathUtility.AppendTrailingSlash(Directory) + path;

            return Controller.ResolveFileManagerItemInfo(fileManagerPath);
        }

        protected override object SaveViewState()
        {
            object[] states = new object[6];

            states[0] = base.SaveViewState();
            if (_detailsColumnHeaderStyle != null)
                states[1] = ((IStateManager)_detailsColumnHeaderStyle).SaveViewState();
            if (_detailsSortedColumnStyle != null)
                states[2] = ((IStateManager)_detailsSortedColumnStyle).SaveViewState();
            if (_editTextBoxStyle != null)
                states[3] = ((IStateManager)_editTextBoxStyle).SaveViewState();
            if (_itemStyle != null)
                states[4] = ((IStateManager)_itemStyle).SaveViewState();
            if (_selectedItemStyle != null)
                states[5] = ((IStateManager)_selectedItemStyle).SaveViewState();

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
                ((IStateManager)DetailsColumnHeaderStyle).LoadViewState(states[1]);
            if (states[2] != null)
                ((IStateManager)DetailsSortedColumnStyle).LoadViewState(states[2]);
            if (states[3] != null)
                ((IStateManager)EditTextBoxStyle).LoadViewState(states[3]);
            if (states[4] != null)
                ((IStateManager)ItemStyle).LoadViewState(states[4]);
            if (states[5] != null)
                ((IStateManager)SelectedItemStyle).LoadViewState(states[5]);
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();

            if (_detailsColumnHeaderStyle != null)
                ((IStateManager)_detailsColumnHeaderStyle).TrackViewState();
            if (_detailsSortedColumnStyle != null)
                ((IStateManager)_detailsSortedColumnStyle).TrackViewState();
            if (_editTextBoxStyle != null)
                ((IStateManager)_editTextBoxStyle).TrackViewState();
            if (_itemStyle != null)
                ((IStateManager)_itemStyle).TrackViewState();
            if (_selectedItemStyle != null)
                ((IStateManager)_selectedItemStyle).TrackViewState();
        }

        protected override object SaveControlState()
        {
            RegisterHiddenField("View", View.ToString());
            RegisterHiddenField("Sort", Sort.ToString());
            RegisterHiddenField("SortDirection", SortDirection.ToString());
            RegisterHiddenField("ShowInGroups", ShowInGroups ? "true" : "false");
            RegisterHiddenField("Directory", FileManagerController.EncodeURIComponent(CurrentDirectory.FileManagerPath));
            RegisterHiddenField("SelectedItems", "");
            RegisterHiddenField("SearchTerm", SearchTerm);

            return new object[] { base.SaveControlState() };
        }

        protected override void LoadControlState(object savedState)
        {
            base.LoadControlState(((object[])savedState)[0]);

            View = (FileViewRenderMode)Enum.Parse(typeof(FileViewRenderMode), GetValueFromHiddenField("View"));
            Sort = (SortMode)Enum.Parse(typeof(SortMode), GetValueFromHiddenField("Sort"));
            SortDirection = (SortDirection)Enum.Parse(typeof(SortDirection), GetValueFromHiddenField("SortDirection"));
            ShowInGroups = bool.Parse(GetValueFromHiddenField("ShowInGroups"));
            Directory = HttpUtility.UrlDecode(GetValueFromHiddenField("Directory"));
            SearchTerm = HttpUtility.UrlDecode(GetValueFromHiddenField("SearchTerm"));

            string[] selectedItemsEncoded = GetValueFromHiddenField("SelectedItems").Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            ArrayList selectedItemsArray = new ArrayList();
            foreach (string pathEncoded in selectedItemsEncoded)
            {
                string path = HttpUtility.UrlDecode(pathEncoded);
                selectedItemsArray.Add(path);
            }

            selectedItemsStr = (string[])selectedItemsArray.ToArray(typeof(string));
        }

        string GetInitInstanceScript()
        {

            StringBuilder sb = new StringBuilder();
            string fileView = FileManagerController.ClientScriptObjectNamePrefix + ClientID;
            sb.AppendLine("var " + fileView + "=new FileView('" + ClientID + "','" + Controller.ClientID + "','" + ItemStyle.RegisteredCssClass + "','" + SelectedItemStyle.RegisteredCssClass + "','" + EditTextBoxStyle.RegisteredCssClass + "');");
            sb.AppendLine(fileView + ".Initialize();");

            return sb.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            Page.Header.StyleSheet.RegisterStyle(ItemStyle, this);
            Page.Header.StyleSheet.RegisterStyle(SelectedItemStyle, this);
            Page.Header.StyleSheet.RegisterStyle(EditTextBoxStyle, this);

            if (UseLinkToOpenItem)
            {
                Style linkStyle = new Style();
                linkStyle.Font.Underline = false;
                Page.Header.StyleSheet.CreateStyleRule(linkStyle, this, "a." + LinkToOpenItemClass);

                Style hoverLinkStyle = new Style();
                hoverLinkStyle.Font.Underline = true;
                Page.Header.StyleSheet.CreateStyleRule(hoverLinkStyle, this, "a." + LinkToOpenItemClass + ":hover");

                Style itemLinkStyle = new Style();
                itemLinkStyle.ForeColor = ItemStyle.ForeColor;
                if (itemLinkStyle.ForeColor == Color.Empty)
                    itemLinkStyle.ForeColor = ForeColor;
                Page.Header.StyleSheet.CreateStyleRule(itemLinkStyle, this, "." + ItemStyle.RegisteredCssClass + " a." + LinkToOpenItemClass);

                Style selectedItemLinkStyle = new Style();
                selectedItemLinkStyle.ForeColor = SelectedItemStyle.ForeColor;
                if (selectedItemLinkStyle.ForeColor == Color.Empty)
                    selectedItemLinkStyle.ForeColor = ForeColor;
                Page.Header.StyleSheet.CreateStyleRule(selectedItemLinkStyle, this, "." + SelectedItemStyle.RegisteredCssClass + " a." + LinkToOpenItemClass);
            }

            Page.ClientScript.RegisterClientScriptResource(typeof(FileView), "IZ.WebFileManager.resources.FileView.js");
            Page.ClientScript.RegisterStartupScript(typeof(FileView), ClientID, GetInitInstanceScript(), true);

            if (EnableContextMenu)
            {
                CreateContextMenu();
                CreateSelectedItemsContextMenu();
            }
        }

        internal string GetSortEventReference(SortMode sort)
        {
            return Controller.GetCommandEventReference(this, FileManagerCommands.FileViewSort.ToString(), "'" + sort.ToString() + "'");
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".SetFocus()");
            Controller.AddDirectionAttribute(writer);
            writer.AddStyleAttribute(HtmlTextWriterStyle.Position, "relative");
            writer.AddStyleAttribute(HtmlTextWriterStyle.ZIndex, "20");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Overflow, "auto");
        }

        void CreateContextMenu()
        {
            _contextMenu = new ContextMenu(
                ClientID + "cm",
                Controller.CurrentUICulture.TextInfo.IsRightToLeft, 
                RenderContextMenuPopupItem,
                Controller.DynamicMenuStyle,
                Controller.DynamicMenuItemStyle,
                Controller.DynamicHoverStyle);

            // Root
            MenuItem root = new MenuItem();
            root.Text = "_contextMenu";
            root.NavigateUrl = "javascript: return;";
            _contextMenu.Items.Add(root);

            string clientClickFunction = "javascript:" + FileManagerController.ClientScriptObjectNamePrefix + Controller.ClientID + ".On{0}(" + FileManagerController.ClientScriptObjectNamePrefix + ClientID + ", '{1}');return false;";

            // View
            MenuItem itemView = new MenuItem();
            itemView.Text = GetResourceString("View", "View");
            itemView.Value = "View";
            itemView.NavigateUrl = "javascript: return;";
            root.ChildItems.Add(itemView);

            // Icons
            MenuItem itemViewIcons = new MenuItem();
            itemViewIcons.Text = GetResourceString("Icons", "Icons");
            itemViewIcons.Value = "Icons";
            itemViewIcons.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewChangeView, FileViewRenderMode.Icons);
            itemView.ChildItems.Add(itemViewIcons);

            // Details
            MenuItem itemViewDetails = new MenuItem();
            itemViewDetails.Text = GetResourceString("Details", "Details");
            itemViewDetails.Value = "Details";
            itemViewDetails.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewChangeView, FileViewRenderMode.Details);
            itemView.ChildItems.Add(itemViewDetails);

            // Thumbnails
            MenuItem itemViewThumbnails = new MenuItem();
            itemViewThumbnails.Text = GetResourceString("Thumbnails", "Thumbnails");
            itemViewThumbnails.Value = "Thumbnails";
            itemViewThumbnails.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewChangeView, FileViewRenderMode.Thumbnails);
            itemView.ChildItems.Add(itemViewThumbnails);

            root.ChildItems.Add(new MenuItem("__separator__", "__separator__", null, "javascript: return;"));

            // Arrange Icons By
            MenuItem itemArrangeIconsBy = new MenuItem();
            itemArrangeIconsBy.Text = GetResourceString("Arrange_Icons_By", "Arrange Icons By");
            itemArrangeIconsBy.Value = "Arrange_Icons_By";
            itemArrangeIconsBy.NavigateUrl = "javascript: return;";
            root.ChildItems.Add(itemArrangeIconsBy);

            // Name
            MenuItem itemArrangeIconsByName = new MenuItem();
            itemArrangeIconsByName.Text = GetResourceString("Name", "Name");
            itemArrangeIconsByName.Value = "Name";
            itemArrangeIconsByName.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewSort, SortMode.Name);
            itemArrangeIconsBy.ChildItems.Add(itemArrangeIconsByName);

            // Size
            MenuItem itemArrangeIconsBySize = new MenuItem();
            itemArrangeIconsBySize.Text = GetResourceString("Size", "Size");
            itemArrangeIconsBySize.Value = "Size";
            itemArrangeIconsBySize.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewSort, SortMode.Size);
            itemArrangeIconsBy.ChildItems.Add(itemArrangeIconsBySize);

            // Type
            MenuItem itemArrangeIconsByType = new MenuItem();
            itemArrangeIconsByType.Text = GetResourceString("Type", "Type");
            itemArrangeIconsByType.Value = "Type";
            itemArrangeIconsByType.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewSort, SortMode.Type);
            itemArrangeIconsBy.ChildItems.Add(itemArrangeIconsByType);

            // Modified
            MenuItem itemArrangeIconsByModified = new MenuItem();
            itemArrangeIconsByModified.Text = GetResourceString("Date_Modified", "Date Modified");
            itemArrangeIconsByModified.Value = "Modified";
            itemArrangeIconsByModified.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.FileViewSort, SortMode.Modified);
            itemArrangeIconsBy.ChildItems.Add(itemArrangeIconsByModified);

            // Refresh
            MenuItem itemRefresh = new MenuItem();
            itemRefresh.Text = Controller.GetResourceString("Refresh", "Refresh");
            itemRefresh.Value = "Refresh";
            itemRefresh.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Refresh);
            itemRefresh.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.Refresh, "");
            root.ChildItems.Add(itemRefresh);

            root.ChildItems.Add(new MenuItem("__separator__", "__separator__", null, "javascript: return;"));

            // New
            MenuItem itemNew = new MenuItem();
            itemNew.Text = GetResourceString("Create", "New");
            itemNew.Value = "Create";
            itemNew.NavigateUrl = "javascript: return;";
            itemNew.Enabled = !ReadOnly;
            root.ChildItems.Add(itemNew);

            if (!ReadOnly)
            {
                // New Folder
                MenuItem itemFolder = new MenuItem();
                itemFolder.Text = GetResourceString("New_Folder", "Folder");
                itemFolder.Value = "New_Folder";
                itemFolder.ImageUrl = Controller.GetFolderSmallImage();
                itemFolder.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.NewFolder, "");
                itemNew.ChildItems.Add(itemFolder);

                itemNew.ChildItems.Add(new MenuItem("__separator__", "__separator__", null, "javascript: return;"));

                for (int i = 0; i < Templates.Count; i++)
                {
                    NewDocumentTemplate template = Templates[i];
                    MenuItem item = new MenuItem();
                    item.Text = template.Name;
                    item.Value = "Template" + i;
                    if (template.SmallImageUrl.Length > 0)
                        item.ImageUrl = ResolveUrl(template.SmallImageUrl);
                    else if (template.MasterFileUrl.Length > 0)
                        item.ImageUrl = Controller.GetFileSmallImage(new FileInfo(Page.MapPath(template.MasterFileUrl)));
                    else
                        item.ImageUrl = Controller.GetFileSmallImage();
                    item.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.NewDocument, Templates.IndexOf(template));
                    itemNew.ChildItems.Add(item);
                }
            }

            // client script
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function " + ClientID + "_ShowContextMenu(x,y) {");

            sb.AppendLine("var bulletImgSrc = '" + Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.Bullet.gif") + "';");
            sb.AppendLine("var emptyImgSrc = '" + Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.Empty.gif") + "';");

            sb.AppendLine("var nameImg = WebForm_GetElementById('" + ClientID + "CMIMGName');");
            sb.AppendLine("var sizeImg = WebForm_GetElementById('" + ClientID + "CMIMGSize');");
            sb.AppendLine("var typeImg = WebForm_GetElementById('" + ClientID + "CMIMGType');");
            sb.AppendLine("var modifiedImg = WebForm_GetElementById('" + ClientID + "CMIMGModified');");
            sb.AppendLine("var iconsImg = WebForm_GetElementById('" + ClientID + "CMIMGIcons');");
            sb.AppendLine("var detailsImg = WebForm_GetElementById('" + ClientID + "CMIMGDetails');");
            sb.AppendLine("var thumbnailsImg = WebForm_GetElementById('" + ClientID + "CMIMGThumbnails');");

            sb.AppendLine("nameImg.src = emptyImgSrc;");
            sb.AppendLine("sizeImg.src = emptyImgSrc;");
            sb.AppendLine("typeImg.src = emptyImgSrc;");
            sb.AppendLine("modifiedImg.src = emptyImgSrc;");
            sb.AppendLine("iconsImg.src = emptyImgSrc;");
            sb.AppendLine("detailsImg.src = emptyImgSrc;");
            sb.AppendLine("thumbnailsImg.src = emptyImgSrc;");

            sb.AppendLine("var sort = " + FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".GetSort();");
            sb.AppendLine("switch(sort) {");
            sb.AppendLine("case '" + SortMode.Name + "':");
            sb.AppendLine("nameImg.src = bulletImgSrc;");
            sb.AppendLine("break;");
            sb.AppendLine("case '" + SortMode.Type + "':");
            sb.AppendLine("typeImg.src = bulletImgSrc;");
            sb.AppendLine("break;");
            sb.AppendLine("case '" + SortMode.Size + "':");
            sb.AppendLine("sizeImg.src = bulletImgSrc;");
            sb.AppendLine("break;");
            sb.AppendLine("case '" + SortMode.Modified + "':");
            sb.AppendLine("modifiedImg.src = bulletImgSrc;");
            sb.AppendLine("break;");
            sb.AppendLine("}");

            sb.AppendLine("var view = " + FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".GetView();");
            sb.AppendLine("switch(view) {");
            sb.AppendLine("case '" + FileViewRenderMode.Icons + "':");
            sb.AppendLine("iconsImg.src = bulletImgSrc;");
            sb.AppendLine("break;");
            sb.AppendLine("case '" + FileViewRenderMode.Details + "':");
            sb.AppendLine("detailsImg.src = bulletImgSrc;");
            sb.AppendLine("break;");
            sb.AppendLine("case '" + FileViewRenderMode.Thumbnails + "':");
            sb.AppendLine("thumbnailsImg.src = bulletImgSrc;");
            sb.AppendLine("break;");
            sb.AppendLine("}");


            sb.AppendLine("var node = WebForm_GetElementById('" + _contextMenu.ClientID + "')");
            sb.AppendLine("WebForm_SetElementX(node, x)");
            sb.AppendLine("WebForm_SetElementY(node, y)");
            sb.AppendLine("IZWebFileManager_ShowElement('" + _contextMenu.ClientID + "_0');");
            sb.AppendLine("}");
            Page.ClientScript.RegisterClientScriptBlock(typeof(FileView), ClientID + "_ShowContextMenu", sb.ToString(), true);
        }

        void CreateSelectedItemsContextMenu()
        {
            _selectedItemsContextMenu = new ContextMenu(
                ClientID + "scm",
                Controller.CurrentUICulture.TextInfo.IsRightToLeft, 
                RenderContextMenuPopupItem,
                Controller.DynamicMenuStyle,
                Controller.DynamicMenuItemStyle,
                Controller.DynamicHoverStyle);

            // Root
            MenuItem root = new MenuItem();
            root.Text = "_contextMenu";
            root.NavigateUrl = "javascript: return;";
            _selectedItemsContextMenu.Items.Add(root);

            string clientClickFunction = "javascript:" + FileManagerController.ClientScriptObjectNamePrefix + Controller.ClientID + ".On{0}(" + FileManagerController.ClientScriptObjectNamePrefix + ClientID + ", '{1}');return false;";

            StringBuilder sbCommands = new StringBuilder();

            for (int i = 0; i < FileTypes.Count; i++)
            {
                FileType ft = FileTypes[i];
                for (int j = 0; j < ft.Commands.Count; j++)
                {
                    FileManagerCommand command = ft.Commands[j];
                    MenuItem itemCommand = new MenuItem();
                    itemCommand.Text = command.Name;
                    itemCommand.Value = i + "_" + j;
                    itemCommand.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.ExecuteCommand, "0:" + j);
                    itemCommand.ImageUrl = command.SmallImageUrl;
                    root.ChildItems.Add(itemCommand);

                    sbCommands.AppendLine(ClientID + "_SetCommandVisible('" + ClientID + "CMD" + i + "_" + j + "', (" + FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".SelectedItems.length==1) && (" + FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".SelectedItems[0].FileType==" + i + "));");
                }
            }

            MenuItem itemOpen = new MenuItem();
            itemOpen.Text = GetResourceString("Open", "Open");
            itemOpen.Value = "Open";
            itemOpen.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.ExecuteCommand, "0:-1");
            root.ChildItems.Add(itemOpen);

            MenuItem itemDownload = new MenuItem();
            itemDownload.Text = GetResourceString("Download", "Download");
            itemDownload.Value = "Download";
            itemDownload.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.ExecuteCommand, "0:-2");
            root.ChildItems.Add(itemDownload);
            sbCommands.AppendLine(ClientID + "_SetCommandVisible('" + ClientID + "CMDDownload', (" + FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".SelectedItems.length==1) && (" + FileManagerController.ClientScriptObjectNamePrefix + ClientID + ".SelectedItems[0].FileType!=-2));");

            root.ChildItems.Add(new MenuItem("__separator__", "__separator__", null, "javascript: return;"));

            // Copy to
            MenuItem itemCopyTo = new MenuItem();
            itemCopyTo.Text = GetResourceString("Copy", "Copy");
            itemCopyTo.Value = "Copy";
            itemCopyTo.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Copy);
            itemCopyTo.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.SelectedItemsCopyTo, "");
            itemCopyTo.Enabled = !ReadOnly;
            root.ChildItems.Add(itemCopyTo);

            // Move to
            MenuItem itemMoveTo = new MenuItem();
            itemMoveTo.Text = GetResourceString("Move", "Move");
            itemMoveTo.Value = "Move";
            itemMoveTo.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Move);
            itemMoveTo.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.SelectedItemsMoveTo, "");
            itemMoveTo.Enabled = !ReadOnly && AllowDelete;
            root.ChildItems.Add(itemMoveTo);

            root.ChildItems.Add(new MenuItem("__separator__", "__separator__", null, "javascript: return;"));

            // Delete
            MenuItem itemDelete = new MenuItem();
            itemDelete.Text = GetResourceString("Delete", "Delete");
            itemDelete.Value = "Delete";
            itemDelete.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Delete);
            itemDelete.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.SelectedItemsDelete, "");
            itemDelete.Enabled = !ReadOnly && AllowDelete;
            root.ChildItems.Add(itemDelete);

            // Rename
            MenuItem itemRename = new MenuItem();
            itemRename.Text = GetResourceString("Rename", "Rename");
            itemRename.Value = "Rename";
            itemRename.ImageUrl = Controller.GetToolbarImage(ToolbarImages.Rename);
            itemRename.NavigateUrl = String.Format(clientClickFunction, FileManagerCommands.Rename, "");
            itemRename.Enabled = !ReadOnly && AllowDelete;
            root.ChildItems.Add(itemRename);

            // client script
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("function " + ClientID + "_ShowSelectedItemsContextMenu(x,y) {");
            sb.Append(sbCommands.ToString());
            sb.AppendLine("var node = WebForm_GetElementById('" + _selectedItemsContextMenu.ClientID + "')");
            sb.AppendLine("WebForm_SetElementX(node, x)");
            sb.AppendLine("WebForm_SetElementY(node, y)");
            sb.AppendLine("IZWebFileManager_ShowElement('" + _selectedItemsContextMenu.ClientID + "_0');");
            sb.AppendLine("}");
            sb.AppendLine("function " + ClientID + "_SetCommandVisible(command, value) {");
            sb.AppendLine("var node = WebForm_GetElementById(command);");
            sb.AppendLine("var row = node.parentNode;");
            sb.AppendLine("if (value) {");
            sb.AppendLine("row.style.visibility = \"visible\";");
            sb.AppendLine("row.style.display = \"block\";");
            sb.AppendLine("row.style.position = \"static\";");
            //sb.AppendLine ("row.parentNode.parentNode.style.height = \"auto\";");
            sb.AppendLine("} else {");
            sb.AppendLine("row.style.visibility = \"hidden\";");
            sb.AppendLine("row.style.display = \"none\";");
            sb.AppendLine("row.style.position = \"absolute\";");
            //sb.AppendLine ("row.parentNode.parentNode.style.height = \"0px\";");
            sb.AppendLine("}");
            sb.AppendLine("}");
            Page.ClientScript.RegisterClientScriptBlock(typeof(FileView), ClientID + "_ShowSelectedItemsContextMenu", sb.ToString(), true);
        }

        void RenderContextMenuPopupItem(HtmlTextWriter writer, MenuItem menuItem, int index)
        {
            if (menuItem.Text == "__separator__")
            {
                writer
                    .Table(e => e.Cellpadding(0).Cellspacing(0).Border(0).BackgroundColor("#ACA899").Height(1).Width("100%").Cursor("default"))
                        .Tr()
                            .Td()
                                .Img(e => e.Width(1).Height(1).Src(Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.Empty.gif"))).EndTag()
                            .EndTag()
                        .EndTag()
                    .EndTag();
            }
            else
            {
                writer
                    .Table(e =>
                               {
                                   e.Cellpadding(0).Cellspacing(0).Border(0).Width("100%").Cursor("default").Id(ClientID + "CMD" + menuItem.Value);
                                   if (menuItem.Enabled)
                                       e.Onclick(menuItem.NavigateUrl);
                                   else
                                       e.Color("gray");
                                   return e;
                               })
                        .Tr()
                            .Td()
                                .Img(e => e.Id(ClientID + "CMIMG" + menuItem.Value).Width(16).Height(16)
                                    .Src(String.IsNullOrEmpty(menuItem.ImageUrl) ? Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.Empty.gif"): ResolveClientUrl(menuItem.ImageUrl)))
                                .EndTag()
                            .EndTag()
                            .Td(e => e.WhiteSpace("nowrap").PaddingLeft(2).PaddingRight(2).Width("100%"))
                                .Text("&nbsp;" + menuItem.Text)
                            .EndTag()
                            .Td()
                                .Img(e => e.Height(16).Width(16)
                                    .Src(menuItem.ChildItems.Count == 0 ? 
                                        Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.Empty.gif") : 
                                        (Controller.IsRightToLeft ? 
                                            Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.PopOutRtl.gif"):
                                            Page.ClientScript.GetWebResourceUrl(typeof(FileManagerController), "IZ.WebFileManager.resources.PopOut.gif"))))
                                .EndTag()
                            .EndTag()
                        .EndTag()
                    .EndTag();
            }
        }

        #endregion
    }
}
