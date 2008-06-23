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
using System.Globalization;
using IZ.WebFileManager.Components;
using System.IO;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;

namespace IZ.WebFileManager
{
	[SupportsEventValidation]
	[DefaultProperty ("Text")]
	[ToolboxData ("<{0}:FolderTree runat=\"server\"></{0}:FolderTree>")]
	public sealed class FolderTree : FileManagerControlBase, ICallbackEventHandler
	{
		#region Constructors

		public FolderTree ()
			: base () {
			InitTreeView ();
		}

		public FolderTree (FileManagerController controller)
			: base (controller) {
			InitTreeView ();
		}

		public FolderTree (FileManagerController controller, FileView fileFiew)
			: this (controller) {
			_fileView = fileFiew;
		}

		#endregion

		#region Fields
		FileView _fileView;

		bool stylesPrepared;
		Style hoverNodeStyle;
		TreeNodeStyle nodeStyle;
		TreeNodeStyle selectedNodeStyle;
		Collection<FolderNode> _nodes;
		Style controlLinkStyle;
		Style nodeLinkStyle;
		Style selectedNodeLinkStyle;
		Style hoverNodeLinkStyle;
		int _registeredStylesCounter = -1;
		Dictionary<Style, string> _registeredStyleClassNames;
		#endregion

		#region Events
		public EventHandler SelectedFolderChanged;
		#endregion

		#region Properties

		// TODO
		string SelectedPath {
			get { return null; }
		}

		[DefaultValue ("")]
		[Description ("The url of the image to show when a node can be collapsed.")]
		[UrlProperty]
		[Category ("Appearance")]
		string CollapseImageUrl {
			get {
				return (string) (ViewState ["CollapseImageUrl"] ?? String.Empty);
			}
			set {
				ViewState ["CollapseImageUrl"] = value;
			}
		}

		[DefaultValue ("")]
		[UrlProperty]
		[Description ("The url of the image to show when a node can be expanded.")]
		[Category ("Appearance")]
		string ExpandImageUrl {
			get {
				return (string) (ViewState ["ExpandImageUrl"] ?? String.Empty);
			}
			set {
				ViewState ["ExpandImageUrl"] = value;
			}
		}

		[PersistenceMode (PersistenceMode.InnerProperty)]
		[NotifyParentProperty (true)]
		[DefaultValue (null)]
		[Category ("Styles")]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
		Style HoverNodeStyle {
			get {
				if (hoverNodeStyle == null) {
					hoverNodeStyle = new Style ();
					if (IsTrackingViewState)
						((IStateManager) hoverNodeStyle).TrackViewState ();
				}
				return hoverNodeStyle;
			}
		}

		[DefaultValue (20)]
		int NodeIndent {
			get {
				return (int) (ViewState ["NodeIndent"] ?? 20);
			}
			set {
				ViewState ["NodeIndent"] = value;
			}
		}

		Collection<FolderNode> Nodes {
			get {
				if (_nodes == null) {
					_nodes = new Collection<FolderNode> ();
				}
				return _nodes;
			}
		}

		[PersistenceMode (PersistenceMode.InnerProperty)]
		[NotifyParentProperty (true)]
		[DefaultValue (null)]
		[Category ("Styles")]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
		TreeNodeStyle NodeStyle {
			get {
				if (nodeStyle == null) {
					nodeStyle = new TreeNodeStyle ();
					if (IsTrackingViewState)
						((IStateManager) nodeStyle).TrackViewState ();
				}
				return nodeStyle;
			}
		}

		[UrlProperty]
		[DefaultValue ("")]
		[Description ("The url of the image to show for leaf nodes.")]
		[Category ("Appearance")]
		string NoExpandImageUrl {
			get {
				return (string) (ViewState ["NoExpandImageUrl"] ?? String.Empty);
			}
			set {
				ViewState ["NoExpandImageUrl"] = value;
			}
		}

		char PathSeparator {
			get {
				return '/';
			}
		}

		string ClientScriptReference {
			get {
				if (_fileView != null)
					return FileManagerController.ClientScriptObjectNamePrefix + _fileView.ClientID + "_FolderTree";
				return FileManagerController.ClientScriptObjectNamePrefix + ClientID;
			}
		}

		[PersistenceMode (PersistenceMode.InnerProperty)]
		[NotifyParentProperty (true)]
		[DefaultValue (null)]
		[Category ("Styles")]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
		TreeNodeStyle SelectedNodeStyle {
			get {
				if (selectedNodeStyle == null) {
					selectedNodeStyle = new TreeNodeStyle ();
					if (IsTrackingViewState)
						((IStateManager) selectedNodeStyle).TrackViewState ();
				}
				return selectedNodeStyle;
			}
		}

		Style ControlLinkStyle {
			get {
				if (controlLinkStyle == null) {
					controlLinkStyle = new Style ();
					controlLinkStyle.Font.Underline = false;
					//controlLinkStyle.AlwaysRenderTextDecoration = true;
				}
				return controlLinkStyle;
			}
		}

		Style NodeLinkStyle {
			get {
				if (nodeLinkStyle == null) {
					nodeLinkStyle = new Style ();
				}
				return nodeLinkStyle;
			}
		}

		Style SelectedNodeLinkStyle {
			get {
				if (selectedNodeLinkStyle == null) {
					selectedNodeLinkStyle = new Style ();
				}
				return selectedNodeLinkStyle;
			}
		}

		Style HoverNodeLinkStyle {
			get {
				if (hoverNodeLinkStyle == null) {
					hoverNodeLinkStyle = new Style ();
				}
				return hoverNodeLinkStyle;
			}
		}

		[DefaultValue (false)]
		bool ShowLines {
			get {
				return (bool) (ViewState ["ShowLines"] ?? false);
			}
			set {
				ViewState ["ShowLines"] = value;
			}
		}

		protected override HtmlTextWriterTag TagKey {
			get { return HtmlTextWriterTag.Div; }
		}

		#endregion

		#region  Methods

		protected override void AddAttributesToRender (HtmlTextWriter writer) {
			base.AddAttributesToRender (writer);

			writer.AddStyleAttribute (HtmlTextWriterStyle.Position, "relative");
			writer.AddStyleAttribute (HtmlTextWriterStyle.ZIndex, "20");
			writer.AddStyleAttribute (HtmlTextWriterStyle.Overflow, "auto");
		}

		void InitTreeView () {
			NodeStyle.HorizontalPadding = 4;
			NodeStyle.Height = 20;
			NodeStyle.ForeColor = Color.Black;
			NodeStyle.Font.Names = new string [] { "Tahoma", "Verdana", "Geneva", "Arial", "Helvetica", "sans-serif" };
			NodeStyle.Font.Size = FontUnit.Parse ("11px", null);
			HoverNodeStyle.Font.Underline = true;
			SelectedNodeStyle.Font.Underline = false;
			SelectedNodeStyle.ForeColor = Color.White;
			SelectedNodeStyle.BackColor = Color.FromArgb (0x316AC5);
		}

		void OnSelectedFolderChanged (object sender, EventArgs e) {
			if (SelectedFolderChanged != null)
				SelectedFolderChanged (this, EventArgs.Empty);
		}

		void OnFolderPopulate (FolderNode node) {
			Controller.EnsureDefaults ();

			DirectoryInfo directoryInfo = Controller.ResolveFileManagerItemInfo (node.ValuePath).Directory;
			if (!directoryInfo.Exists)
				return;
			
			bool checkHiddenFolders = !String.IsNullOrEmpty (HiddenFilesAndFoldersPrefix) && !ShowHiddenFilesAndFolders;

			foreach (DirectoryInfo dir in directoryInfo.GetDirectories ()) {

				if (checkHiddenFolders && dir.Name.StartsWith (HiddenFilesAndFoldersPrefix, StringComparison.InvariantCultureIgnoreCase))
					continue;
				
				FolderNode treeNode = new FolderNode (this);
				node.ChildNodes.Add (treeNode);
				treeNode.Text = dir.Name;
				treeNode.ImageUrl = Controller.GetFolderSmallImage (dir);
				if (_fileView != null)
					treeNode.NavigateUrl = "javascript:WFM_" + _fileView.ClientID + ".Navigate(\"" + treeNode.ValuePath + "\");";
			}
		}

		void BuildTree () {

			for (int i = 0; i < RootDirectories.Count; i++) {
				RootDirectory rootDir = RootDirectories [i];
				FolderNode treeNode = new FolderNode (this);
				Nodes.Add (treeNode);
				treeNode.ImageUrl = Controller.GetRootFolderSmallImage (rootDir);
				SetText (rootDir, treeNode, i);
				treeNode.Value = PathSeparator + rootDir.TextInternal;
				if (_fileView != null)
					treeNode.NavigateUrl = "javascript:WFM_" + _fileView.ClientID + ".Navigate(\"" + treeNode.ValuePath + "\");";
			}

			for (int i = 0; i < RootDirectories.Count; i++)
				ExpandRecursive (Nodes [i], RootDirectories [i].ExpandDepth);

		}

		void SetText (RootDirectory rootDir, FolderNode treeNode, int index) {
			if (rootDir.ShowRootIndex)
				treeNode.Text = String.Format (CultureInfo.InvariantCulture, "{0} ([{1}])", rootDir.TextInternal, index);
			else
				treeNode.Text = rootDir.TextInternal;
		}

		void ExpandRecursive (FolderNode treeNode, int dept) {
			if (dept == 0)
				return;

			treeNode.Expand ();
			foreach (FolderNode child in treeNode.ChildNodes)
				ExpandRecursive (child, dept - 1);
		}

		#endregion

		protected override void TrackViewState () {

			base.TrackViewState ();
			if (hoverNodeStyle != null) {
				((IStateManager) hoverNodeStyle).TrackViewState ();
			}
			if (nodeStyle != null) {
				((IStateManager) nodeStyle).TrackViewState ();
			}
			if (selectedNodeStyle != null) {
				((IStateManager) selectedNodeStyle).TrackViewState ();
			}
		}

		protected override object SaveViewState () {
			object [] states = new object [4];
			states [0] = base.SaveViewState ();
			states [1] = (hoverNodeStyle == null ? null : ((IStateManager) hoverNodeStyle).SaveViewState ());
			states [2] = (nodeStyle == null ? null : ((IStateManager) nodeStyle).SaveViewState ());
			states [3] = (selectedNodeStyle == null ? null : ((IStateManager) selectedNodeStyle).SaveViewState ());

			for (int i = states.Length - 1; i >= 0; i--) {
				if (states [i] != null)
					return states;
			}

			return null;
		}

		protected override void LoadViewState (object savedState) {
			if (savedState == null)
				return;

			object [] states = (object []) savedState;
			base.LoadViewState (states [0]);

			if (states [1] != null)
				((IStateManager) HoverNodeStyle).LoadViewState (states [1]);
			if (states [2] != null)
				((IStateManager) NodeStyle).LoadViewState (states [2]);
			if (states [3] != null)
				((IStateManager) SelectedNodeStyle).LoadViewState (states [3]);
		}

		string callbackResult;

		void ICallbackEventHandler.RaiseCallbackEvent (string eventArgs) {
			int index = eventArgs.IndexOf ('|');
			FolderNode node = new FolderNode (this);
			node.ValuePath = eventArgs.Substring (index + 1);

			node.Expand ();
			int num = node.ChildNodes.Count;
			if (num == 0) {
				callbackResult = "*";
				return;
			}

			ArrayList levelLines = new ArrayList ();
			for (int i = 0; i < index; i++)
				levelLines.Add (eventArgs [i] == '1' ? this : null);

			StringBuilder sb = new StringBuilder ();
			HtmlTextWriter writer = new HtmlTextWriter (new StringWriter (sb));
			EnsureStylesPrepared ();

			for (int n = 0; n < num; n++)
				RenderNode (writer, node.ChildNodes [n], index, levelLines, true, n < num - 1);

			callbackResult = sb.ToString ();
		}

		string ICallbackEventHandler.GetCallbackResult () {
			return callbackResult;
		}

		protected override ControlCollection CreateControlCollection () {
			return new EmptyControlCollection (this);
		}

		protected override void OnPreRender (EventArgs e) {
			base.OnPreRender (e);

			if (Page.Header == null)
				throw new InvalidOperationException ("Using FolderTree requires Page.Header to be non-null (e.g. <head runat=\"server\" />).");

			Page.ClientScript.RegisterClientScriptResource (typeof (FolderTree), "IZ.WebFileManager.resources.FolderTree.js");
			string callbackScript = string.Format ("FolderTree.prototype.PopulateNode = function(argument, clientCallback, context, clientErrorCallback) {{ {0}; }};\n", Page.ClientScript.GetCallbackEventReference ("this._uniqueId", "argument", "clientCallback", "context", "clientErrorCallback", true));
			Page.ClientScript.RegisterClientScriptBlock (typeof (FolderTree), "FolderTree.prototype.PopulateNode", callbackScript, true);

			string folderTree = ClientScriptReference;
			StringBuilder script = new StringBuilder ();
			script.AppendLine ("var " + folderTree + "= new FolderTree ('" + Controller.ClientID + "','" + ClientID + "','" + UniqueID + "','" + GetNodeImageUrl ("Expand") + "','" + GetNodeImageUrl ("Collapse") + "','" + GetNodeImageUrl ("NoExpand") + "');");

			EnsureStylesPrepared ();

			if (selectedNodeStyle != null) {
				script.AppendFormat ("{0}._selectedClass = '{1}';\n", folderTree, _registeredStyleClassNames [SelectedNodeStyle]);
				script.AppendFormat ("{0}._selectedLinkClass = '{1}';\n", folderTree, _registeredStyleClassNames [SelectedNodeLinkStyle]);
			}

			if (hoverNodeStyle != null) {
				script.AppendFormat ("{0}._hoverClass = '{1}';\n", folderTree, _registeredStyleClassNames [HoverNodeStyle]);
				script.AppendFormat ("{0}._hoverLinkClass = '{1}';\n", folderTree, _registeredStyleClassNames [HoverNodeLinkStyle]);
			}
			if (_fileView != null)
				script.AppendLine (folderTree + ".Navigate (['" + String.Join ("','", Controller.GetPathHashCodes (_fileView.CurrentDirectory.FileManagerPath)) + "'],0);");

			Page.ClientScript.RegisterStartupScript (typeof (FolderTree), this.ClientID, script.ToString (), true);

			BuildTree ();
		}

		void EnsureStylesPrepared () {
			if (stylesPrepared)
				return;
			stylesPrepared = true;
			PrepareStyles ();
		}

		private void PrepareStyles () {
			_registeredStyleClassNames = new Dictionary<Style, string> ();

			ControlLinkStyle.Font.CopyFrom (ControlStyle.Font);
			ControlLinkStyle.ForeColor = ControlStyle.ForeColor;
			RegisterStyle (ControlLinkStyle);

			if (nodeStyle != null)
				RegisterStyle (NodeStyle, NodeLinkStyle);

			if (selectedNodeStyle != null)
				RegisterStyle (SelectedNodeStyle, SelectedNodeLinkStyle);

			if (hoverNodeStyle != null)
				RegisterStyle (HoverNodeStyle, HoverNodeLinkStyle);
		}

		void RegisterStyle (Style baseStyle, Style linkStyle) {
			linkStyle.Font.CopyFrom (baseStyle.Font);
			linkStyle.ForeColor = baseStyle.ForeColor;
			linkStyle.BorderStyle = BorderStyle.None;
			baseStyle.Font.ClearDefaults ();
			RegisterStyle (linkStyle);
			RegisterStyle (baseStyle);
		}

		string IncrementStyleClassName () {
			_registeredStylesCounter++;
			return ClientID + "_" + _registeredStylesCounter;
		}

		void RegisterStyle (Style baseStyle) {
			string className = IncrementStyleClassName ();
			Page.Header.StyleSheet.CreateStyleRule (baseStyle, this, "." + className);
			_registeredStyleClassNames [baseStyle] = className;
		}

		string GetBindingKey (string dataMember, int depth) {
			return dataMember + " " + depth;
		}

		protected override void RenderContents (HtmlTextWriter writer) {
			int num = Nodes.Count;
			for (int n = 0; n < num; n++)
				RenderNode (writer, Nodes [n], 0, new ArrayList (), n > 0, n < num - 1);
		}

		void RenderNode (HtmlTextWriter writer, FolderNode node, int level, ArrayList levelLines, bool hasPrevious, bool hasNext) {
			string nodeImage;

			bool hasChildNodes = node.Expanded ? node.ChildNodes.Count > 0 : true;
			if (hasNext)
				levelLines.Add (this);
			else
				levelLines.Add (null);

			writer.AddAttribute ("cellpadding", "0", false);
			writer.AddAttribute ("cellspacing", "0", false);
			writer.AddStyleAttribute ("border-width", "0");
			writer.RenderBeginTag (HtmlTextWriterTag.Table);

			Unit nodeSpacing = GetNodeSpacing (node);

			if (nodeSpacing != Unit.Empty && (node.Depth > 0 || node.Index > 0))
				RenderMenuItemSpacing (writer, nodeSpacing);

			writer.RenderBeginTag (HtmlTextWriterTag.Tr);

			// Vertical lines from previous levels

			nodeImage = GetNodeImageUrl ("I");
			for (int n = 0; n < level; n++) {
				writer.RenderBeginTag (HtmlTextWriterTag.Td);
				writer.AddStyleAttribute ("width", NodeIndent + "px");
				//writer.AddStyleAttribute ("height", "1px");
				writer.RenderBeginTag (HtmlTextWriterTag.Div);
				if (ShowLines && levelLines [n] != null) {
					writer.AddAttribute ("src", nodeImage);
					writer.AddAttribute (HtmlTextWriterAttribute.Alt, "", false);
					writer.RenderBeginTag (HtmlTextWriterTag.Img);
					writer.RenderEndTag ();
				}
				writer.RenderEndTag ();
				writer.RenderEndTag ();	// TD
			}

			// Node image + line

			string shape = String.Empty;

			if (ShowLines) {
				if (hasPrevious && hasNext)
					shape = "T";
				else if (hasPrevious && !hasNext)
					shape = "L";
				else if (!hasPrevious && hasNext)
					shape = "R";
				else
					shape = "Dash";
			}

			if (hasChildNodes) {
				if (node.Expanded)
					shape += "Collapse";
				else
					shape += "Expand";
			}
			else if (!ShowLines)
				shape = "NoExpand";

			if (!String.IsNullOrEmpty (shape)) {
				nodeImage = GetNodeImageUrl (shape);
				writer.RenderBeginTag (HtmlTextWriterTag.Td);	// TD

				writer.AddAttribute (HtmlTextWriterAttribute.Href, GetClientExpandEvent (node));
				writer.RenderBeginTag (HtmlTextWriterTag.A);	// Anchor

				writer.AddAttribute ("alt", String.Empty);

				writer.AddAttribute (HtmlTextWriterAttribute.Id, GetNodeClientId (node, "img"));
				writer.AddAttribute ("src", nodeImage);
				writer.AddStyleAttribute (HtmlTextWriterStyle.BorderWidth, "0");
				writer.RenderBeginTag (HtmlTextWriterTag.Img);
				writer.RenderEndTag ();

				writer.RenderEndTag ();		// Anchor

				writer.RenderEndTag ();		// TD
			}

			writer.RenderBeginTag (HtmlTextWriterTag.Td);
			writer.AddAttribute ("onmouseout", ClientScriptReference + ".UnhoverNode(this)", false);
			writer.AddAttribute ("onmouseover", ClientScriptReference + ".HoverNode(this, event)", false);
			writer.AddAttribute (HtmlTextWriterAttribute.Id, GetNodeClientId (node, null));
			writer.RenderBeginTag (HtmlTextWriterTag.Div);
			writer.AddAttribute ("cellpadding", "0", false);
			writer.AddAttribute ("cellspacing", "0", false);
			writer.AddStyleAttribute ("border-width", "0");
			writer.RenderBeginTag (HtmlTextWriterTag.Table);
			writer.RenderBeginTag (HtmlTextWriterTag.Tr);

			// Node icon

			string imageUrl = node.ImageUrl.Length > 0 ? ResolveClientUrl (node.ImageUrl) : null;

			if (!String.IsNullOrEmpty (imageUrl)) {
				writer.RenderBeginTag (HtmlTextWriterTag.Td);	// TD
				BeginNodeTag (writer, node);
				writer.AddAttribute ("src", imageUrl);
				writer.AddStyleAttribute (HtmlTextWriterStyle.BorderWidth, "0");
				writer.AddAttribute ("alt", String.Empty);
				writer.RenderBeginTag (HtmlTextWriterTag.Img);
				writer.RenderEndTag ();	// IMG
				writer.RenderEndTag ();	// node tag
				writer.RenderEndTag ();	// TD
			}

			writer.AddStyleAttribute ("white-space", "nowrap");
			AddNodeStyle (writer, node, level);

			writer.AddAttribute ("nodepath", GetNodePath (node, levelLines));
			writer.AddAttribute (HtmlTextWriterAttribute.Id, GetNodeClientId (node, "node"));

			writer.RenderBeginTag (HtmlTextWriterTag.Td);	// TD

			AddNodeLinkStyle (writer, node, level);
			BeginNodeTag (writer, node);
			writer.Write (node.Text);
			writer.RenderEndTag ();	// node tag

			writer.RenderEndTag ();	// TD

			writer.RenderEndTag (); // TR
			writer.RenderEndTag (); // TABLE
			writer.RenderEndTag (); // DIV
			writer.RenderEndTag (); // TD

			writer.RenderEndTag ();	// TR

			if (nodeSpacing != Unit.Empty)
				RenderMenuItemSpacing (writer, nodeSpacing);

			writer.RenderEndTag ();	// TABLE

			// Children

			if (!node.Expanded || node.ChildNodes.Count == 0)
				writer.AddStyleAttribute ("display", "none");
			else
				writer.AddStyleAttribute ("display", "block");
			writer.AddAttribute (HtmlTextWriterAttribute.Id, GetNodeClientId (node, "span"));
			writer.RenderBeginTag (HtmlTextWriterTag.Span);

			if (node.Expanded) {
				//AddChildrenPadding (writer, node);
				int num = node.ChildNodes.Count;
				if (num == 0)
					writer.Write ("*");
				else
					for (int n = 0; n < num; n++)
						RenderNode (writer, node.ChildNodes [n], level + 1, levelLines, true, n < num - 1);
				//if (hasNext)
				//    AddChildrenPadding (writer, node);
			}
			writer.RenderEndTag ();	// SPAN

			levelLines.RemoveAt (level);
		}

		//private void AddChildrenPadding (HtmlTextWriter writer, FolderNode node) {
		//    int level = node.Depth;
		//    Unit cnp = Unit.Empty;

		//    if (cnp.IsEmpty && nodeStyle != null)
		//        cnp = nodeStyle.ChildNodesPadding;

		//    double value;
		//    if (cnp.IsEmpty || (value = cnp.Value) == 0 || cnp.Type != UnitType.Pixel)
		//        return;

		//    writer.RenderBeginTag (HtmlTextWriterTag.Table);
		//    writer.AddAttribute ("height", ((int) value).ToString (), false);
		//    writer.RenderBeginTag (HtmlTextWriterTag.Tr);
		//    writer.RenderBeginTag (HtmlTextWriterTag.Td);
		//    writer.RenderEndTag (); // td
		//    writer.RenderEndTag (); // tr
		//    writer.RenderEndTag (); // table
		//}

		private void RenderMenuItemSpacing (HtmlTextWriter writer, Unit itemSpacing) {
			writer.AddStyleAttribute ("height", itemSpacing.ToString ());
			writer.RenderBeginTag (HtmlTextWriterTag.Tr);
			writer.RenderBeginTag (HtmlTextWriterTag.Td);
			writer.RenderEndTag ();
			writer.RenderEndTag ();
		}

		private Unit GetNodeSpacing (FolderNode node) {
			if (node.Selected && selectedNodeStyle != null && selectedNodeStyle.NodeSpacing != Unit.Empty)
				return selectedNodeStyle.NodeSpacing;
			if (nodeStyle != null)
				return nodeStyle.NodeSpacing;
			else
				return Unit.Empty;
		}

		void AddNodeStyle (HtmlTextWriter writer, FolderNode node, int level) {
			StringBuilder sb = new StringBuilder ();
			if (nodeStyle != null) {
				sb.Append (" ");
				sb.Append (nodeStyle.CssClass);
				sb.Append (" ");
				sb.Append (_registeredStyleClassNames [nodeStyle]);
			}
			if (node.ValuePath == SelectedPath && selectedNodeStyle != null) {
				sb.Append (" ");
				sb.Append (selectedNodeStyle.CssClass);
				sb.Append (" ");
				sb.Append (_registeredStyleClassNames [selectedNodeStyle]);
			}
			writer.AddAttribute (HtmlTextWriterAttribute.Class, sb.ToString ());
		}

		void AddNodeLinkStyle (HtmlTextWriter writer, FolderNode node, int level) {
			StringBuilder sb = new StringBuilder ();
			sb.Append (" ");
			sb.Append (_registeredStyleClassNames [ControlLinkStyle]);

			if (nodeStyle != null) {
				sb.Append (" ");
				sb.Append (nodeLinkStyle.CssClass);
				sb.Append (" ");
				sb.Append (_registeredStyleClassNames [nodeLinkStyle]);
			}
			if (node.ValuePath == SelectedPath && selectedNodeStyle != null) {
				sb.Append (" ");
				sb.Append (selectedNodeLinkStyle.CssClass);
				sb.Append (" ");
				sb.Append (_registeredStyleClassNames [selectedNodeLinkStyle]);
			}
			writer.AddAttribute (HtmlTextWriterAttribute.Class, sb.ToString ());
		}

		void BeginNodeTag (HtmlTextWriter writer, FolderNode node) {
			if (node.ToolTip.Length > 0)
				writer.AddAttribute ("title", node.ToolTip);

			if (!String.IsNullOrEmpty (node.NavigateUrl)) {
				string navUrl = ResolveClientUrl (node.NavigateUrl);
				writer.AddAttribute ("href", navUrl);
				writer.RenderBeginTag (HtmlTextWriterTag.A);
			}
			else
				writer.RenderBeginTag (HtmlTextWriterTag.Span);
		}

		string GetNodeClientId (FolderNode node, string sufix) {
			return ClientID + "_" + node.ClientID + (sufix != null ? "_" + sufix : "");
		}

		string GetNodeImageUrl (string shape) {
			if (!ShowLines) {
				if (shape == "Expand") {
					if (!String.IsNullOrEmpty (ExpandImageUrl))
						return ResolveClientUrl (ExpandImageUrl);
				}
				else if (shape == "Collapse") {
					if (!String.IsNullOrEmpty (CollapseImageUrl))
						return ResolveClientUrl (CollapseImageUrl);
				}
				else if (shape == "NoExpand") {
					if (!String.IsNullOrEmpty (NoExpandImageUrl))
						return ResolveClientUrl (NoExpandImageUrl);
				}
			}
			return Page.ClientScript.GetWebResourceUrl (typeof (Page), "TreeView_Default_" + shape + ".gif");
		}

		string GetNodeIconUrl (string icon) {
			return Page.ClientScript.GetWebResourceUrl (typeof (TreeView), icon + ".gif");
		}

		string GetClientExpandEvent (FolderNode node) {
			return "javascript:" + ClientScriptReference + ".ToggleExpand (\"" + node.ClientID + "\")";
		}

		string GetNodePath (FolderNode node, ArrayList levelLines) {
			StringBuilder arg = new StringBuilder ();
			for (int i = 0; i < levelLines.Count; i++)
				if (levelLines [i] == null)
					arg.Append ('0');
				else
					arg.Append ('1');
			arg.Append ('|');
			arg.Append (node.ValuePath);
			return arg.ToString ();
		}

		sealed class FolderNode
		{
			Collection<FolderNode> _nodes;
			readonly FolderTree _owner;
			FolderNode _parent;
			int _index = -1;
			int _depth = -1;
			string _clientId;
			string _valuePath;
			string _imageUrl;
			string _navigateUrl;
			string _value;
			string _text;
			string _tooltip;
			bool _expanded;
			bool _selected;

			public FolderNode (FolderTree owner) {
				_owner = owner;
			}

			[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
			[Browsable (false)]
			public int Depth {
				get {
					if (_depth != -1)
						return _depth;
					_depth = 0;
					FolderNode node = _parent;
					while (node != null) {
						_depth++;
						node = node.Parent;
					}
					return _depth;
				}
			}

			public Collection<FolderNode> ChildNodes {
				get {
					if (_nodes == null) {
						_nodes = new FolderNodeCollection (this);
					}
					return _nodes;
				}
			}

			public bool Expanded {
				get { return _expanded; }
			}

			public string ImageUrl {
				get { return _imageUrl ?? String.Empty; }
				set { _imageUrl = value; }
			}

			public string NavigateUrl {
				get { return _navigateUrl ?? String.Empty; }
				set { _navigateUrl = value; }
			}

			public string Text {
				get { return _text ?? (_value ?? String.Empty); }
				set { _text = value; }
			}

			public string ToolTip {
				get { return _tooltip ?? String.Empty; }
				set { _tooltip = value; }
			}

			public string Value {
				get { return _value ?? (_text ?? String.Empty); }
				set { _value = value; }
			}

			[DefaultValue (false)]
			public bool Selected {
				get { return _selected; }
				set { _selected = value; }
			}

			[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
			[Browsable (false)]
			public FolderNode Parent {
				get { return _parent; }
			}

			[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
			[Browsable (false)]
			public string ValuePath {
				get {
					if (_owner == null)
						return Value;
					if (_valuePath == null) {
						StringBuilder sb = new StringBuilder (Value);
						FolderNode node = Parent;
						if (node != null) {
							sb.Insert (0, _owner.PathSeparator);
							sb.Insert (0, node.ValuePath);
							node = node.Parent;
						}
						_valuePath = sb.ToString ();
					}
					return _valuePath;
				}
				set {
					_valuePath = value;
				}
			}

			internal int Index {
				get {
					if (_index < 0) {
						_index = 0;
						if (Parent != null)
							_index = Parent.ChildNodes.IndexOf (this);
					}
					return _index;
				}
			}

			internal void SetParent (FolderNode node) {
				_parent = node;
				_index = -1;
				_depth = -1;
				_clientId = null;
				_valuePath = null;
			}

			internal string ClientID {
				get {
					if (_clientId == null) {
						_clientId = _owner.Controller.GetPathHashCode (ValuePath);
					}
					return _clientId;
				}
			}

			public void Expand () {
				if (Expanded)
					return;
				_expanded = true;
				_owner.OnFolderPopulate (this);
			}
		}

		class FolderNodeCollection : Collection<FolderNode>
		{
			readonly FolderNode _owner;

			public FolderNodeCollection (FolderNode owner) {
				_owner = owner;
			}

			protected override void InsertItem (int index, FolderNode item) {
				base.InsertItem (index, item);
				item.SetParent (_owner);
			}

			protected override void SetItem (int index, FolderNode item) {
				base.SetItem (index, item);
				item.SetParent (_owner);
			}
		}

	}
}
