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
using System.Drawing.Design;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;

namespace IZ.WebFileManager
{
    [ToolboxData("<{0}:IZPanel runat=\"server\" width=\"125px\" height=\"50px\"></{0}:IZPanel>")]
    [PersistChildren(true)]
    [ParseChildren(false)]
    public class BorderedPanel : Panel
    {
        #region Fields

        BorderedPanelStyle _hoverSyle;
        BorderedPanelStyle _pressedSyle;
        string _hoverCssClass;
        string _pressedCssClass;

        #endregion

        #region Properties

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BorderedPanelStyle HoverSyle
        {
            get
            {
                if (_hoverSyle == null)
                {
                    _hoverSyle = new BorderedPanelStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_hoverSyle).TrackViewState();
                }
                return _hoverSyle;
            }

        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public BorderedPanelStyle PressedSyle
        {
            get
            {
                if (_pressedSyle == null)
                {
                    _pressedSyle = new BorderedPanelStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)_pressedSyle).TrackViewState();
                }
                return _pressedSyle;
            }

        }

        [DefaultValue(typeof(Color), "")]
        [Category("Appearance")]
        public Color OuterBorderBackColor
        {
            get
            {
                if (ControlStyleCreated)
                    return ((BorderedPanelStyle)ControlStyle).OuterBorderBackColor;
                return Color.Empty;
            }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderBackColor = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [UrlProperty]
        [DefaultValue("")]
        [Category("Appearance")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string OuterBorderBottomImageUrl
        {
            get { return !ControlStyleCreated ? String.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderBottomImageUrl; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderBottomImageUrl = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [UrlProperty]
        [DefaultValue("")]
        [Category("Appearance")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string OuterBorderLeftImageUrl
        {
            get { return !ControlStyleCreated ? String.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderLeftImageUrl; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderLeftImageUrl = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [UrlProperty]
        [DefaultValue("")]
        [Category("Appearance")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string OuterBorderLeftBottomCornerImageUrl
        {
            get { return !ControlStyleCreated ? String.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderLeftBottomCornerImageUrl; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderLeftBottomCornerImageUrl = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [UrlProperty]
        [DefaultValue("")]
        [Category("Appearance")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string OuterBorderLeftTopCornerImageUrl
        {
            get { return !ControlStyleCreated ? String.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderLeftTopCornerImageUrl; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderLeftTopCornerImageUrl = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [UrlProperty]
        [DefaultValue("")]
        [Category("Appearance")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string OuterBorderRightImageUrl
        {
            get { return !ControlStyleCreated ? String.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderRightImageUrl; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderRightImageUrl = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [UrlProperty]
        [DefaultValue("")]
        [Category("Appearance")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string OuterBorderRightBottomCornerImageUrl
        {
            get { return !ControlStyleCreated ? String.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderRightBottomCornerImageUrl; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderRightBottomCornerImageUrl = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [UrlProperty]
        [DefaultValue("")]
        [Category("Appearance")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string OuterBorderRightTopCornerImageUrl
        {
            get { return !ControlStyleCreated ? String.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderRightTopCornerImageUrl; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderRightTopCornerImageUrl = value; }
        }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
        [UrlProperty]
        [DefaultValue("")]
        [Category("Appearance")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string OuterBorderTopImageUrl
        {
            get { return !ControlStyleCreated ? String.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderTopImageUrl; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderTopImageUrl = value; }
        }

        [DefaultValue(OuterBorderStyle.None)]
        [Category("Layout")]
        public OuterBorderStyle OuterBorderStyle
        {
            get { return !ControlStyleCreated ? OuterBorderStyle.None : ((BorderedPanelStyle)ControlStyle).OuterBorderStyle; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderStyle = value; }
        }

        [DefaultValue(typeof(Unit), "")]
        [Category("Layout")]
        public Unit OuterBorderBottomWidth
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderBottomWidth; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderBottomWidth = value; }
        }

        [DefaultValue(typeof(Unit), "")]
        [Category("Layout")]
        public Unit OuterBorderTopWidth
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderTopWidth; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderTopWidth = value; }
        }

        [DefaultValue(typeof(Unit), "")]
        [Category("Layout")]
        public Unit OuterBorderLeftWidth
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderLeftWidth; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderLeftWidth = value; }
        }

        [DefaultValue(typeof(Unit), "")]
        [Category("Layout")]
        public Unit OuterBorderRightWidth
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).OuterBorderRightWidth; }
            set { ((BorderedPanelStyle)ControlStyle).OuterBorderRightWidth = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Layout")]
        [NotifyParentProperty(true)]
        public Unit PaddingTop
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).PaddingTop; }
            set { ((BorderedPanelStyle)ControlStyle).PaddingTop = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Layout")]
        [NotifyParentProperty(true)]
        public Unit PaddingLeft
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).PaddingLeft; }
            set { ((BorderedPanelStyle)ControlStyle).PaddingLeft = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Layout")]
        [NotifyParentProperty(true)]
        public Unit PaddingBottom
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).PaddingBottom; }
            set { ((BorderedPanelStyle)ControlStyle).PaddingBottom = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Layout")]
        [NotifyParentProperty(true)]
        public Unit PaddingRight
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).PaddingRight; }
            set { ((BorderedPanelStyle)ControlStyle).PaddingRight = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        public Unit MarginBottom
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).MarginBottom; }
            set { ((BorderedPanelStyle)ControlStyle).MarginBottom = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        public Unit MarginLeft
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).MarginLeft; }
            set { ((BorderedPanelStyle)ControlStyle).MarginLeft = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        public Unit MarginRight
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).MarginRight; }
            set { ((BorderedPanelStyle)ControlStyle).MarginRight = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Appearance")]
        [NotifyParentProperty(true)]
        public Unit MarginTop
        {
            get { return !ControlStyleCreated ? Unit.Empty : ((BorderedPanelStyle)ControlStyle).MarginTop; }
            set { ((BorderedPanelStyle)ControlStyle).MarginTop = value; }
        }

        public virtual string OuterBorderHoverImagesArrayName
        {
            get { return ClientID + "OuterBorderHover"; }
        }

        public virtual string OuterBorderPressedImagesArrayName
        {
            get { return ClientID + "OuterBorderPressed"; }
        }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.ClientScript.RegisterClientScriptResource(typeof(BorderedPanel), "IZ.WebFileManager.resources.BorderedPanel.js");

            bool isHeader = Page.Header != null;

            if (_hoverSyle != null)
            {
                if (!isHeader)
                    throw new InvalidOperationException("Using BorderedPanel.HoverStyle requires Page.Header to be non-null (e.g. <head runat=\"server\" />).");
                BorderedPanelStyle s = new BorderedPanelStyle();
                s.CopyFrom(_hoverSyle);
                if (ControlStyleCreated)
                    s.MergeWith(ControlStyle);
                Page.Header.StyleSheet.RegisterStyle(s, this);
                _hoverCssClass = s.RegisteredCssClass;
                RegisterBorderImagesArray(s, OuterBorderHoverImagesArrayName);
            }

            if (_pressedSyle != null)
            {
                if (!isHeader)
                    throw new InvalidOperationException("Using BorderedPanel.PressedSyle requires Page.Header to be non-null (e.g. <head runat=\"server\" />).");
                BorderedPanelStyle s = new BorderedPanelStyle();
                s.CopyFrom(_pressedSyle);
                if (_hoverSyle != null)
                    s.MergeWith(_hoverSyle);
                if (ControlStyleCreated)
                    s.MergeWith(ControlStyle);
                Page.Header.StyleSheet.RegisterStyle(s, this);
                _pressedCssClass = s.RegisteredCssClass;
                RegisterBorderImagesArray(s, OuterBorderPressedImagesArrayName);
            }

            if (ControlStyleCreated)
            {
                if (isHeader)
                    Page.Header.StyleSheet.RegisterStyle(ControlStyle, this);
            }
        }

        void RegisterBorderImagesArray(BorderedPanelStyle style, String arrayName)
        {
            Page.ClientScript.RegisterArrayDeclaration(arrayName,
                "'" + String.Join("','", new string[]{
                    ResolveUrl(style.OuterBorderTopImageUrl),
                    ResolveUrl(style.OuterBorderLeftImageUrl),
                    ResolveUrl(style.OuterBorderRightImageUrl),
                    ResolveUrl(style.OuterBorderBottomImageUrl),

                    ResolveUrl(style.OuterBorderLeftTopCornerImageUrl),
                    ResolveUrl(style.OuterBorderLeftBottomCornerImageUrl),
                    ResolveUrl(style.OuterBorderRightTopCornerImageUrl),
                    ResolveUrl(style.OuterBorderRightBottomCornerImageUrl)

                }) + "'");
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            if (OuterBorderStyle == OuterBorderStyle.None)
                AddClientScriptAttributes(writer);
        }

        void AddClientScriptAttributes(HtmlTextWriter writer)
        {
            if (_hoverSyle != null)
            {
                writer.AddAttribute("onmouseover", "BorderedPanel_SetStyle('" + ClientID + "'," + OuterBorderHoverImagesArrayName + ",'" + _hoverCssClass + "')");
                writer.AddAttribute("onmouseout", "BorderedPanel_RestoreStyle('" + ClientID + "')");
            }

            if (_pressedSyle != null)
            {
                writer.AddAttribute("onmousedown", "BorderedPanel_SetStyle('" + ClientID + "'," + OuterBorderPressedImagesArrayName + ",'" + _pressedCssClass + "')");
                if (_hoverSyle != null)
                    writer.AddAttribute("onmouseup", "BorderedPanel_SetStyle('" + ClientID + "'," + OuterBorderHoverImagesArrayName + ",'" + _hoverCssClass + "')");
                else
                    writer.AddAttribute("onmouseup", "BorderedPanel_RestoreStyle('" + ClientID + "')");
            }
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_hoverSyle != null)
                ((IStateManager)_hoverSyle).TrackViewState();
            if (_pressedSyle != null)
                ((IStateManager)_pressedSyle).TrackViewState();
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
                return;

            object[] states = (object[])savedState;

            base.LoadViewState(states[0]);
            if (states[1] != null)
                ((IStateManager)HoverSyle).LoadViewState(states[1]);
            if (states[2] != null)
                ((IStateManager)PressedSyle).LoadViewState(states[2]);
        }

        protected override object SaveControlState()
        {
            object[] states = new object[3];

            states[0] = base.SaveViewState();
            if (_hoverSyle != null)
                states[1] = ((IStateManager)_hoverSyle).SaveViewState();
            if (_pressedSyle != null)
                states[2] = ((IStateManager)_pressedSyle).SaveViewState();

            for (int i = 0; i < states.Length; i++)
            {
                if (states[i] != null)
                    return states;
            }
            return null;
        }

        protected override Style CreateControlStyle()
        {
            return new BorderedPanelStyle(ViewState);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            RenderBeginOuterBorder(writer);

            if (Page.Header != null) // base class Panel add inline attribut background-image, even if style is registered with header
                ((BorderedPanelStyle) ControlStyle).BackImageUrl = String.Empty;
            base.RenderBeginTag(writer);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            RenderEndOuterBorder(writer);
        }

        void RenderBeginOuterBorder(System.Web.UI.HtmlTextWriter writer)
        {
            if (OuterBorderStyle == OuterBorderStyle.None)
                return;

            AddClientScriptAttributes(writer);

            if (writer == null)
                throw new ArgumentNullException("writer");

            if (OuterBorderBackColor != Color.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundColor, ColorTranslator.ToHtml(OuterBorderBackColor));

            writer.AddStyleAttribute(HtmlTextWriterStyle.Direction, "ltr");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0");
            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            if ((OuterBorderStyle & OuterBorderStyle.Top) > 0)
                RenderOuterBorderTopRow(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            if ((OuterBorderStyle & OuterBorderStyle.Left) > 0)
                RenderOuterBorderLeft(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Td);

        }

        void RenderOuterBorderTopRow(System.Web.UI.HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            if ((OuterBorderStyle & OuterBorderStyle.Left) > 0)
                RenderOuterBorderLeftTopCorner(writer);
            RenderOuterBorderTop(writer);
            if ((OuterBorderStyle & OuterBorderStyle.Right) > 0)
                RenderOuterBorderRightTopCorner(writer);

            writer.RenderEndTag();
        }

        void RenderOuterBorderLeft(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("background-repeat", "repeat-y");
            writer.AddStyleAttribute("background-position", "center left");
            if (OuterBorderLeftWidth != Unit.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, OuterBorderLeftWidth.ToString());
            if (OuterBorderLeftImageUrl.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + ResolveClientUrl(OuterBorderLeftImageUrl) + ")");
            if (!String.IsNullOrEmpty(ClientID))
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_BorderL");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
        }

        void RenderOuterBorderRightTopCorner(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("background-repeat", "no-repeat");
            writer.AddStyleAttribute("background-position", "top right");
            if (OuterBorderRightTopCornerImageUrl.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + ResolveClientUrl(OuterBorderRightTopCornerImageUrl) + ")");
            if (!String.IsNullOrEmpty(ClientID))
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_BorderRT");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //if (OuterBorderRightTopCornerImageUrl.Length > 0)
            //{
            //    writer.AddAttribute(HtmlTextWriterAttribute.Src, _urlResolver.ResolveClientUrl(OuterBorderRightTopCornerImageUrl));
            //    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");
            //    writer.RenderBeginTag(HtmlTextWriterTag.Img);
            //    writer.RenderEndTag();
            //}
            writer.RenderEndTag();
        }

        void RenderOuterBorderTop(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("background-repeat", "repeat-x");
            writer.AddStyleAttribute("background-position", "top center");
            if (OuterBorderTopWidth != Unit.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, OuterBorderTopWidth.ToString());
            if (OuterBorderTopImageUrl.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + ResolveClientUrl(OuterBorderTopImageUrl) + ")");
            if (!String.IsNullOrEmpty(ClientID))
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_BorderT");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
        }

        void RenderOuterBorderLeftTopCorner(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("background-repeat", "no-repeat");
            writer.AddStyleAttribute("background-position", "top left");
            if (OuterBorderLeftTopCornerImageUrl.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + ResolveClientUrl(OuterBorderLeftTopCornerImageUrl) + ")");
            if (!String.IsNullOrEmpty(ClientID))
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_BorderLT");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //if (OuterBorderLeftTopCornerImageUrl.Length > 0)
            //{
            //    writer.AddAttribute(HtmlTextWriterAttribute.Src, _urlResolver.ResolveClientUrl(OuterBorderLeftTopCornerImageUrl));
            //    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");
            //    writer.RenderBeginTag(HtmlTextWriterTag.Img);
            //    writer.RenderEndTag();
            //}
            writer.RenderEndTag();
        }

        void RenderEndOuterBorder(System.Web.UI.HtmlTextWriter writer)
        {
            if (OuterBorderStyle == OuterBorderStyle.None)
                return;

            if (writer == null)
                throw new ArgumentNullException("writer");

            writer.RenderEndTag();

            if ((OuterBorderStyle & OuterBorderStyle.Right) > 0)
                RenderOuterBorderRight(writer);

            writer.RenderEndTag();

            if ((OuterBorderStyle & OuterBorderStyle.Bottom) > 0)
                RenderOuterBorderBottomRow(writer);

            writer.RenderEndTag();
        }

        void RenderOuterBorderBottomRow(System.Web.UI.HtmlTextWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);

            if ((OuterBorderStyle & OuterBorderStyle.Left) > 0)
                RenderOuterBorderLeftBottomCorner(writer);
            RenderOuterBorderBottom(writer);
            if ((OuterBorderStyle & OuterBorderStyle.Right) > 0)
                RenderOuterBorderRightBottomCorner(writer);

            writer.RenderEndTag();
        }

        void RenderOuterBorderRightBottomCorner(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("background-repeat", "no-repeat");
            writer.AddStyleAttribute("background-position", "bottom right");
            if (OuterBorderRightBottomCornerImageUrl.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + ResolveClientUrl(OuterBorderRightBottomCornerImageUrl) + ")");
            if (!String.IsNullOrEmpty(ClientID))
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_BorderRB");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //if (OuterBorderRightBottomCornerImageUrl.Length > 0)
            //{
            //    writer.AddAttribute(HtmlTextWriterAttribute.Src, _urlResolver.ResolveClientUrl(OuterBorderRightBottomCornerImageUrl));
            //    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");
            //    writer.RenderBeginTag(HtmlTextWriterTag.Img);
            //    writer.RenderEndTag();
            //}
            writer.RenderEndTag();
        }

        void RenderOuterBorderBottom(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("background-repeat", "repeat-x");
            writer.AddStyleAttribute("background-position", "bottom center");
            if (OuterBorderBottomWidth != Unit.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, OuterBorderBottomWidth.ToString());
            if (OuterBorderBottomImageUrl.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + ResolveClientUrl(OuterBorderBottomImageUrl) + ")");
            if (!String.IsNullOrEmpty(ClientID))
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_BorderB");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
        }

        void RenderOuterBorderLeftBottomCorner(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("background-repeat", "no-repeat");
            writer.AddStyleAttribute("background-position", "bottom left");
            if (OuterBorderLeftBottomCornerImageUrl.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + ResolveClientUrl(OuterBorderLeftBottomCornerImageUrl) + ")");
            if (!String.IsNullOrEmpty(ClientID))
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_BorderLB");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            //if (OuterBorderLeftBottomCornerImageUrl.Length > 0)
            //{
            //    writer.AddAttribute(HtmlTextWriterAttribute.Src, _urlResolver.ResolveClientUrl(OuterBorderLeftBottomCornerImageUrl));
            //    writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");
            //    writer.RenderBeginTag(HtmlTextWriterTag.Img);
            //    writer.RenderEndTag();
            //}
            writer.RenderEndTag();
        }

        void RenderOuterBorderRight(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddStyleAttribute("background-repeat", "repeat-y");
            writer.AddStyleAttribute("background-position", "center right");
            if (OuterBorderRightWidth != Unit.Empty)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, OuterBorderRightWidth.ToString());
            if (OuterBorderRightImageUrl.Length > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.BackgroundImage, "url(" + ResolveClientUrl(OuterBorderRightImageUrl) + ")");
            if (!String.IsNullOrEmpty(ClientID))
                writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_BorderR");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.RenderEndTag();
        }

    }
}
