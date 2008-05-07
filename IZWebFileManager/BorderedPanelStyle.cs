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
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Drawing.Design;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Drawing;

namespace IZ.WebFileManager
{
	[TypeConverter (typeof (ExpandableObjectConverter))]
	public class BorderedPanelStyle : PanelStyle
	{
		[Flags]
		enum StyleKey
		{
			OuterBorderBottomImageUrl = 0x0001,
			OuterBorderTopImageUrl = 0x0002,
			OuterBorderLeftImageUrl = 0x0004,
			OuterBorderRightImageUrl = 0x0008,

			OuterBorderLeftTopCornerImageUrl = 0x0010,
			OuterBorderLeftBottomCornerImageUrl = 0x0020,
			OuterBorderRightTopCornerImageUrl = 0x0040,
			OuterBorderRightBottomCornerImageUrl = 0x0080,

			OuterBorderLeftWidth = 0x0100,
			OuterBorderBottomWidth = 0x0200,
			OuterBorderRightWidth = 0x0400,
			OuterBorderTopWidth = 0x0800,

			OuterBorderStyle = 0x1000,
			OuterBorderBackColor = 0x2000,

			PaddingTop = 0x010000,
			PaddingBottom = 0x020000,
			PaddingLeft = 0x040000,
			PaddingRight = 0x080000,

			MarginTop = 0x100000,
			MarginBottom = 0x200000,
			MarginLeft = 0x400000,
			MarginRight = 0x800000,
		}

		#region Constructors

		public BorderedPanelStyle ()
			: base (new StateBag ()) {
		}

		public BorderedPanelStyle (StateBag bag)
			: base (bag) {
		}

		#endregion

		#region Fields

		int styles;

		#endregion

		#region Properties

		[DefaultValue (ContentDirection.NotSet)]
		public override ContentDirection Direction {
			get {
				return base.Direction;
			}
			set {
				base.Direction = value;
			}
		}

		[DefaultValue (HorizontalAlign.NotSet)]
		public override HorizontalAlign HorizontalAlign {
			get {
				return base.HorizontalAlign;
			}
			set {
				base.HorizontalAlign = value;
			}
		}

		[DefaultValue (ScrollBars.None)]
		public override ScrollBars ScrollBars {
			get {
				return base.ScrollBars;
			}
			set {
				base.ScrollBars = value;
			}
		}

		[DefaultValue (true)]
		public override bool Wrap {
			get {
				return base.Wrap;
			}
			set {
				base.Wrap = value;
			}
		}

		[Category ("Appearance")]
		[DefaultValue (typeof (Color), "")]
		[TypeConverter (typeof (WebColorConverter))]
		[NotifyParentProperty (true)]
		public Color OuterBorderBackColor {
			get {
				if (CheckStyle (StyleKey.OuterBorderBackColor))
					return (Color) this.ViewState ["OuterBorderBackColor"];
				return Color.Empty;
			}
			set {
				this.ViewState ["OuterBorderBackColor"] = value;
				SetStyle (StyleKey.OuterBorderBackColor);
			}
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[UrlProperty]
		[DefaultValue ("")]
		[Category ("Appearance")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string OuterBorderBottomImageUrl {
			get {
				if (CheckStyle (StyleKey.OuterBorderBottomImageUrl))
					return (string) ViewState ["OuterBorderBottomImageUrl"];
				return String.Empty;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				ViewState ["OuterBorderBottomImageUrl"] = value;
				SetStyle (StyleKey.OuterBorderBottomImageUrl);
			}
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[UrlProperty]
		[DefaultValue ("")]
		[Category ("Appearance")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string OuterBorderTopImageUrl {
			get {
				if (CheckStyle (StyleKey.OuterBorderTopImageUrl))
					return (string) ViewState ["OuterBorderTopImageUrl"];
				return String.Empty;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				ViewState ["OuterBorderTopImageUrl"] = value;
				SetStyle (StyleKey.OuterBorderTopImageUrl);
			}
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[UrlProperty]
		[DefaultValue ("")]
		[Category ("Appearance")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string OuterBorderLeftImageUrl {
			get {
				if (CheckStyle (StyleKey.OuterBorderLeftImageUrl))
					return (string) ViewState ["OuterBorderLeftImageUrl"];
				return String.Empty;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				ViewState ["OuterBorderLeftImageUrl"] = value;
				SetStyle (StyleKey.OuterBorderLeftImageUrl);
			}
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[UrlProperty]
		[DefaultValue ("")]
		[Category ("Appearance")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string OuterBorderRightImageUrl {
			get {
				if (CheckStyle (StyleKey.OuterBorderRightImageUrl))
					return (string) ViewState ["OuterBorderRightImageUrl"];
				return String.Empty;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				ViewState ["OuterBorderRightImageUrl"] = value;
				SetStyle (StyleKey.OuterBorderRightImageUrl);
			}
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[UrlProperty]
		[DefaultValue ("")]
		[Category ("Appearance")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string OuterBorderRightBottomCornerImageUrl {
			get {
				if (CheckStyle (StyleKey.OuterBorderRightBottomCornerImageUrl))
					return (string) ViewState ["OuterBorderRightBottomCornerImageUrl"];
				return String.Empty;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				ViewState ["OuterBorderRightBottomCornerImageUrl"] = value;
				SetStyle (StyleKey.OuterBorderRightBottomCornerImageUrl);
			}
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[UrlProperty]
		[DefaultValue ("")]
		[Category ("Appearance")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string OuterBorderRightTopCornerImageUrl {
			get {
				if (CheckStyle (StyleKey.OuterBorderRightTopCornerImageUrl))
					return (string) ViewState ["OuterBorderRightTopCornerImageUrl"];
				return String.Empty;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				ViewState ["OuterBorderRightTopCornerImageUrl"] = value;
				SetStyle (StyleKey.OuterBorderRightTopCornerImageUrl);
			}
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[UrlProperty]
		[DefaultValue ("")]
		[Category ("Appearance")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string OuterBorderLeftBottomCornerImageUrl {
			get {
				if (CheckStyle (StyleKey.OuterBorderLeftBottomCornerImageUrl))
					return (string) ViewState ["OuterBorderLeftBottomCornerImageUrl"];
				return String.Empty;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				ViewState ["OuterBorderLeftBottomCornerImageUrl"] = value;
				SetStyle (StyleKey.OuterBorderLeftBottomCornerImageUrl);
			}
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[UrlProperty]
		[DefaultValue ("")]
		[Category ("Appearance")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		public string OuterBorderLeftTopCornerImageUrl {
			get {
				if (CheckStyle (StyleKey.OuterBorderLeftTopCornerImageUrl))
					return (string) ViewState ["OuterBorderLeftTopCornerImageUrl"];
				return String.Empty;
			}
			set {
				if (value == null)
					throw new ArgumentNullException ("value");
				ViewState ["OuterBorderLeftTopCornerImageUrl"] = value;
				SetStyle (StyleKey.OuterBorderLeftTopCornerImageUrl);
			}
		}

		[DefaultValue (OuterBorderStyle.None)]
		[Category ("Layout")]
		public OuterBorderStyle OuterBorderStyle {
			get {
				if (CheckStyle (StyleKey.OuterBorderStyle))
					return (OuterBorderStyle) ViewState ["OuterBorderStyle"];
				return OuterBorderStyle.None;
			}
			set {
				if (value < OuterBorderStyle.None || value > OuterBorderStyle.AllSides)
					throw new ArgumentOutOfRangeException ("value");
				ViewState ["OuterBorderStyle"] = value;
				SetStyle (StyleKey.OuterBorderStyle);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[Category ("Layout")]
		public Unit OuterBorderTopWidth {
			get {
				if (CheckStyle (StyleKey.OuterBorderTopWidth))
					return (Unit) ViewState ["OuterBorderTopWidth"];
				return Unit.Empty;
			}
			set {
				if (value.Value < 0)
					throw new ArgumentOutOfRangeException ("value");
				ViewState ["OuterBorderTopWidth"] = value;
				SetStyle (StyleKey.OuterBorderTopWidth);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[Category ("Layout")]
		public Unit OuterBorderRightWidth {
			get {
				if (CheckStyle (StyleKey.OuterBorderRightWidth))
					return (Unit) ViewState ["OuterBorderRightWidth"];
				return Unit.Empty;
			}
			set {
				if (value.Value < 0)
					throw new ArgumentOutOfRangeException ("value");
				ViewState ["OuterBorderRightWidth"] = value;
				SetStyle (StyleKey.OuterBorderRightWidth);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[Category ("Layout")]
		public Unit OuterBorderBottomWidth {
			get {
				if (CheckStyle (StyleKey.OuterBorderBottomWidth))
					return (Unit) ViewState ["OuterBorderBottomWidth"];
				return Unit.Empty;
			}
			set {
				if (value.Value < 0)
					throw new ArgumentOutOfRangeException ("value");
				ViewState ["OuterBorderBottomWidth"] = value;
				SetStyle (StyleKey.OuterBorderBottomWidth);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[Category ("Layout")]
		public Unit OuterBorderLeftWidth {
			get {
				if (CheckStyle (StyleKey.OuterBorderLeftWidth))
					return (Unit) ViewState ["OuterBorderLeftWidth"];
				return Unit.Empty;
			}
			set {
				if (value.Value < 0)
					throw new ArgumentOutOfRangeException ("value");
				ViewState ["OuterBorderLeftWidth"] = value;
				SetStyle (StyleKey.OuterBorderLeftWidth);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[NotifyParentProperty (true)]
		[RefreshProperties (RefreshProperties.Repaint)]
		[Category ("Layout")]
		public Unit PaddingTop {
			get {
				if (CheckStyle (StyleKey.PaddingTop))
					return (Unit) ViewState ["PaddingTop"];
				return Unit.Empty;
			}
			set {
				ViewState ["PaddingTop"] = value;
				SetStyle (StyleKey.PaddingTop);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[NotifyParentProperty (true)]
		[RefreshProperties (RefreshProperties.Repaint)]
		[Category ("Layout")]
		public Unit PaddingBottom {
			get {
				if (CheckStyle (StyleKey.PaddingBottom))
					return (Unit) ViewState ["PaddingBottom"];
				return Unit.Empty;
			}
			set {
				ViewState ["PaddingBottom"] = value;
				SetStyle (StyleKey.PaddingBottom);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[NotifyParentProperty (true)]
		[RefreshProperties (RefreshProperties.Repaint)]
		[Category ("Layout")]
		public Unit PaddingLeft {
			get {
				if (CheckStyle (StyleKey.PaddingLeft))
					return (Unit) ViewState ["PaddingLeft"];
				return Unit.Empty;
			}
			set {
				ViewState ["PaddingLeft"] = value;
				SetStyle (StyleKey.PaddingLeft);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[NotifyParentProperty (true)]
		[RefreshProperties (RefreshProperties.Repaint)]
		[Category ("Layout")]
		public Unit PaddingRight {
			get {
				if (CheckStyle (StyleKey.PaddingRight))
					return (Unit) ViewState ["PaddingRight"];
				return Unit.Empty;
			}
			set {
				ViewState ["PaddingRight"] = value;
				SetStyle (StyleKey.PaddingRight);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[NotifyParentProperty (true)]
		[RefreshProperties (RefreshProperties.Repaint)]
		[Category ("Layout")]
		public Unit MarginTop {
			get {
				if (CheckStyle (StyleKey.MarginTop))
					return (Unit) ViewState ["MarginTop"];
				return Unit.Empty;
			}
			set {
				ViewState ["MarginTop"] = value;
				SetStyle (StyleKey.MarginTop);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[NotifyParentProperty (true)]
		[RefreshProperties (RefreshProperties.Repaint)]
		[Category ("Layout")]
		public Unit MarginBottom {
			get {
				if (CheckStyle (StyleKey.MarginBottom))
					return (Unit) ViewState ["MarginBottom"];
				return Unit.Empty;
			}
			set {
				ViewState ["MarginBottom"] = value;
				SetStyle (StyleKey.MarginBottom);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[NotifyParentProperty (true)]
		[RefreshProperties (RefreshProperties.Repaint)]
		[Category ("Layout")]
		public Unit MarginLeft {
			get {
				if (CheckStyle (StyleKey.MarginLeft))
					return (Unit) ViewState ["MarginLeft"];
				return Unit.Empty;
			}
			set {
				ViewState ["MarginLeft"] = value;
				SetStyle (StyleKey.MarginLeft);
			}
		}

		[DefaultValue (typeof (Unit), "")]
		[NotifyParentProperty (true)]
		[RefreshProperties (RefreshProperties.Repaint)]
		[Category ("Layout")]
		public Unit MarginRight {
			get {
				if (CheckStyle (StyleKey.MarginRight))
					return (Unit) ViewState ["MarginRight"];
				return Unit.Empty;
			}
			set {
				ViewState ["MarginRight"] = value;
				SetStyle (StyleKey.MarginRight);
			}
		}

		public override bool IsEmpty {
			get { return base.IsEmpty && styles == 0; }
		}

		#endregion

		void SetStyle (StyleKey styleKey) {
			styles |= (int) styleKey;
		}

		bool CheckStyle (StyleKey styleKey) {
			return ((styles & (int) styleKey) != 0);
		}

		protected override void FillStyleAttributes (CssStyleCollection attributes, IUrlResolutionService urlResolver) {
			base.FillStyleAttributes (attributes, urlResolver);

			if (PaddingTop != Unit.Empty)
				attributes.Add (HtmlTextWriterStyle.PaddingTop, PaddingTop.ToString ());
			if (PaddingLeft != Unit.Empty)
				attributes.Add (HtmlTextWriterStyle.PaddingLeft, PaddingLeft.ToString ());
			if (PaddingRight != Unit.Empty)
				attributes.Add (HtmlTextWriterStyle.PaddingRight, PaddingRight.ToString ());
			if (PaddingBottom != Unit.Empty)
				attributes.Add (HtmlTextWriterStyle.PaddingBottom, PaddingBottom.ToString ());

			if (MarginTop != Unit.Empty)
				attributes.Add (HtmlTextWriterStyle.MarginTop, MarginTop.ToString ());
			if (MarginLeft != Unit.Empty)
				attributes.Add (HtmlTextWriterStyle.MarginLeft, MarginLeft.ToString ());
			if (MarginRight != Unit.Empty)
				attributes.Add (HtmlTextWriterStyle.MarginRight, MarginRight.ToString ());
			if (MarginBottom != Unit.Empty)
				attributes.Add (HtmlTextWriterStyle.MarginBottom, MarginBottom.ToString ());
		}

		public override void CopyFrom (Style s) {
			base.CopyFrom (s);

			BorderedPanelStyle ps = s as BorderedPanelStyle;
			if (ps == null)
				return;

			if (ps.CheckStyle (StyleKey.OuterBorderBackColor))
				OuterBorderBackColor = ps.OuterBorderBackColor;

			if (ps.CheckStyle (StyleKey.PaddingBottom))
				PaddingBottom = ps.PaddingBottom;

			if (ps.CheckStyle (StyleKey.PaddingLeft))
				PaddingLeft = ps.PaddingLeft;

			if (ps.CheckStyle (StyleKey.PaddingRight))
				PaddingRight = ps.PaddingRight;

			if (ps.CheckStyle (StyleKey.PaddingTop))
				PaddingTop = ps.PaddingTop;

			if (ps.CheckStyle (StyleKey.MarginBottom))
				MarginBottom = ps.MarginBottom;

			if (ps.CheckStyle (StyleKey.MarginLeft))
				MarginLeft = ps.MarginLeft;

			if (ps.CheckStyle (StyleKey.MarginRight))
				MarginRight = ps.MarginRight;

			if (ps.CheckStyle (StyleKey.MarginTop))
				MarginTop = ps.MarginTop;

			if (ps.CheckStyle (StyleKey.OuterBorderBottomImageUrl))
				OuterBorderBottomImageUrl = ps.OuterBorderBottomImageUrl;

			if (ps.CheckStyle (StyleKey.OuterBorderBottomWidth))
				OuterBorderBottomWidth = ps.OuterBorderBottomWidth;

			if (ps.CheckStyle (StyleKey.OuterBorderLeftBottomCornerImageUrl))
				OuterBorderLeftBottomCornerImageUrl = ps.OuterBorderLeftBottomCornerImageUrl;

			if (ps.CheckStyle (StyleKey.OuterBorderLeftImageUrl))
				OuterBorderLeftImageUrl = ps.OuterBorderLeftImageUrl;

			if (ps.CheckStyle (StyleKey.OuterBorderLeftTopCornerImageUrl))
				OuterBorderLeftTopCornerImageUrl = ps.OuterBorderLeftTopCornerImageUrl;

			if (ps.CheckStyle (StyleKey.OuterBorderLeftWidth))
				OuterBorderLeftWidth = ps.OuterBorderLeftWidth;

			if (ps.CheckStyle (StyleKey.OuterBorderRightBottomCornerImageUrl))
				OuterBorderRightBottomCornerImageUrl = ps.OuterBorderRightBottomCornerImageUrl;

			if (ps.CheckStyle (StyleKey.OuterBorderRightImageUrl))
				OuterBorderRightImageUrl = ps.OuterBorderRightImageUrl;

			if (ps.CheckStyle (StyleKey.OuterBorderRightTopCornerImageUrl))
				OuterBorderRightTopCornerImageUrl = ps.OuterBorderRightTopCornerImageUrl;

			if (ps.CheckStyle (StyleKey.OuterBorderRightWidth))
				OuterBorderRightWidth = ps.OuterBorderRightWidth;

			if (ps.CheckStyle (StyleKey.OuterBorderStyle))
				OuterBorderStyle = ps.OuterBorderStyle;

			if (ps.CheckStyle (StyleKey.OuterBorderTopImageUrl))
				OuterBorderTopImageUrl = ps.OuterBorderTopImageUrl;

			if (ps.CheckStyle (StyleKey.OuterBorderTopWidth))
				OuterBorderTopWidth = ps.OuterBorderTopWidth;
		}

		public override void MergeWith (Style s) {
			base.MergeWith (s);

			BorderedPanelStyle ps = s as BorderedPanelStyle;
			if (ps == null)
				return;

			if (!CheckStyle (StyleKey.OuterBorderBackColor) && ps.CheckStyle (StyleKey.OuterBorderBackColor))
				OuterBorderBackColor = ps.OuterBorderBackColor;

			if (!CheckStyle (StyleKey.PaddingBottom) && ps.CheckStyle (StyleKey.PaddingBottom))
				PaddingBottom = ps.PaddingBottom;

			if (!CheckStyle (StyleKey.PaddingLeft) && ps.CheckStyle (StyleKey.PaddingLeft))
				PaddingLeft = ps.PaddingLeft;

			if (!CheckStyle (StyleKey.PaddingRight) && ps.CheckStyle (StyleKey.PaddingRight))
				PaddingRight = ps.PaddingRight;

			if (!CheckStyle (StyleKey.PaddingTop) && ps.CheckStyle (StyleKey.PaddingTop))
				PaddingTop = ps.PaddingTop;

			if (!CheckStyle (StyleKey.MarginBottom) && ps.CheckStyle (StyleKey.MarginBottom))
				MarginBottom = ps.MarginBottom;

			if (!CheckStyle (StyleKey.MarginLeft) && ps.CheckStyle (StyleKey.MarginLeft))
				MarginLeft = ps.MarginLeft;

			if (!CheckStyle (StyleKey.MarginRight) && ps.CheckStyle (StyleKey.MarginRight))
				MarginRight = ps.MarginRight;

			if (!CheckStyle (StyleKey.MarginTop) && ps.CheckStyle (StyleKey.MarginTop))
				MarginTop = ps.MarginTop;

			if (!CheckStyle (StyleKey.OuterBorderBottomImageUrl) && ps.CheckStyle (StyleKey.OuterBorderBottomImageUrl))
				OuterBorderBottomImageUrl = ps.OuterBorderBottomImageUrl;

			if (!CheckStyle (StyleKey.OuterBorderBottomWidth) && ps.CheckStyle (StyleKey.OuterBorderBottomWidth))
				OuterBorderBottomWidth = ps.OuterBorderBottomWidth;

			if (!CheckStyle (StyleKey.OuterBorderLeftBottomCornerImageUrl) && ps.CheckStyle (StyleKey.OuterBorderLeftBottomCornerImageUrl))
				OuterBorderLeftBottomCornerImageUrl = ps.OuterBorderLeftBottomCornerImageUrl;

			if (!CheckStyle (StyleKey.OuterBorderLeftImageUrl) && ps.CheckStyle (StyleKey.OuterBorderLeftImageUrl))
				OuterBorderLeftImageUrl = ps.OuterBorderLeftImageUrl;

			if (!CheckStyle (StyleKey.OuterBorderLeftTopCornerImageUrl) && ps.CheckStyle (StyleKey.OuterBorderLeftTopCornerImageUrl))
				OuterBorderLeftTopCornerImageUrl = ps.OuterBorderLeftTopCornerImageUrl;

			if (!CheckStyle (StyleKey.OuterBorderLeftWidth) && ps.CheckStyle (StyleKey.OuterBorderLeftWidth))
				OuterBorderLeftWidth = ps.OuterBorderLeftWidth;

			if (!CheckStyle (StyleKey.OuterBorderRightBottomCornerImageUrl) && ps.CheckStyle (StyleKey.OuterBorderRightBottomCornerImageUrl))
				OuterBorderRightBottomCornerImageUrl = ps.OuterBorderRightBottomCornerImageUrl;

			if (!CheckStyle (StyleKey.OuterBorderRightImageUrl) && ps.CheckStyle (StyleKey.OuterBorderRightImageUrl))
				OuterBorderRightImageUrl = ps.OuterBorderRightImageUrl;

			if (!CheckStyle (StyleKey.OuterBorderRightTopCornerImageUrl) && ps.CheckStyle (StyleKey.OuterBorderRightTopCornerImageUrl))
				OuterBorderRightTopCornerImageUrl = ps.OuterBorderRightTopCornerImageUrl;

			if (!CheckStyle (StyleKey.OuterBorderRightWidth) && ps.CheckStyle (StyleKey.OuterBorderRightWidth))
				OuterBorderRightWidth = ps.OuterBorderRightWidth;

			if (!CheckStyle (StyleKey.OuterBorderStyle) && ps.CheckStyle (StyleKey.OuterBorderStyle))
				OuterBorderStyle = ps.OuterBorderStyle;

			if (!CheckStyle (StyleKey.OuterBorderTopImageUrl) && ps.CheckStyle (StyleKey.OuterBorderTopImageUrl))
				OuterBorderTopImageUrl = ps.OuterBorderTopImageUrl;

			if (!CheckStyle (StyleKey.OuterBorderTopWidth) && ps.CheckStyle (StyleKey.OuterBorderTopWidth))
				OuterBorderTopWidth = ps.OuterBorderTopWidth;
		}

		public override void Reset () {
			base.Reset ();

			if (CheckStyle (StyleKey.OuterBorderBackColor))
				ViewState.Remove ("OuterBorderBackColor");

			if (CheckStyle (StyleKey.PaddingBottom))
				ViewState.Remove ("PaddingBottom");

			if (CheckStyle (StyleKey.PaddingLeft))
				ViewState.Remove ("PaddingLeft");

			if (CheckStyle (StyleKey.PaddingRight))
				ViewState.Remove ("PaddingRight");

			if (CheckStyle (StyleKey.PaddingTop))
				ViewState.Remove ("PaddingTop");

			if (CheckStyle (StyleKey.MarginBottom))
				ViewState.Remove ("MarginBottom");

			if (CheckStyle (StyleKey.MarginLeft))
				ViewState.Remove ("MarginLeft");

			if (CheckStyle (StyleKey.MarginRight))
				ViewState.Remove ("MarginRight");

			if (CheckStyle (StyleKey.MarginTop))
				ViewState.Remove ("MarginTop");

			if (CheckStyle (StyleKey.OuterBorderBottomImageUrl))
				ViewState.Remove ("OuterBorderBottomImageUrl");

			if (CheckStyle (StyleKey.OuterBorderBottomWidth))
				ViewState.Remove ("OuterBorderBackColor");

			if (CheckStyle (StyleKey.OuterBorderLeftBottomCornerImageUrl))
				ViewState.Remove ("OuterBorderLeftBottomCornerImageUrl");

			if (CheckStyle (StyleKey.OuterBorderLeftImageUrl))
				ViewState.Remove ("OuterBorderLeftImageUrl");

			if (CheckStyle (StyleKey.OuterBorderLeftTopCornerImageUrl))
				ViewState.Remove ("OuterBorderLeftTopCornerImageUrl");

			if (CheckStyle (StyleKey.OuterBorderLeftWidth))
				ViewState.Remove ("OuterBorderLeftWidth");

			if (CheckStyle (StyleKey.OuterBorderRightBottomCornerImageUrl))
				ViewState.Remove ("OuterBorderRightBottomCornerImageUrl");

			if (CheckStyle (StyleKey.OuterBorderRightImageUrl))
				ViewState.Remove ("OuterBorderRightImageUrl");

			if (CheckStyle (StyleKey.OuterBorderRightTopCornerImageUrl))
				ViewState.Remove ("OuterBorderRightTopCornerImageUrl");

			if (CheckStyle (StyleKey.OuterBorderRightWidth))
				ViewState.Remove ("OuterBorderRightWidth");

			if (CheckStyle (StyleKey.OuterBorderStyle))
				ViewState.Remove ("OuterBorderStyle");

			if (CheckStyle (StyleKey.OuterBorderTopImageUrl))
				ViewState.Remove ("OuterBorderTopImageUrl");

			if (CheckStyle (StyleKey.OuterBorderTopWidth))
				ViewState.Remove ("OuterBorderTopWidth");

			styles = 0;
		}

	}
}
