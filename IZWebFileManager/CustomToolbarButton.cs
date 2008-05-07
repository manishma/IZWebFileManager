// Copyright (C) 2008 Igor Zelmanovich <izwebfilemanager@gmail.com>
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
using System.Web.UI;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Drawing.Design;

namespace IZ.WebFileManager
{
	public sealed class CustomToolbarButton : IStateManager
	{
		readonly StateBag _bag = new StateBag ();

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		[DefaultValue ("")]
		[UrlProperty]
		public string ImageUrl {
			get { return (string) (_bag ["ImageUrl"] ?? String.Empty); }
			set { _bag ["ImageUrl"] = value; }
		}

		[DefaultValue ("")]
		public string Text {
			get { return (string) (_bag ["Text"] ?? String.Empty); }
			set { _bag ["Text"] = value; }
		}

		[DefaultValue ("")]
		public string CommandName {
			get { return (string) (_bag ["CommandName"] ?? String.Empty); }
			set { _bag ["CommandName"] = value; }
		}

		[DefaultValue ("")]
		public string CommandArgument {
			get { return (string) (_bag ["CommandArgument"] ?? String.Empty); }
			set { _bag ["CommandArgument"] = value; }
		}

		[DefaultValue ("")]
		public string OnClientClick {
			get { return (string) (_bag ["OnClientClick"] ?? String.Empty); }
			set { _bag ["OnClientClick"] = value; }
		}

		[DefaultValue (true)]
		public bool PerformPostBack {
			get { return (bool) (_bag ["PerformPostBack"] ?? true); }
			set { _bag ["PerformPostBack"] = value; }
		}

		#region IStateManager Members

		bool IStateManager.IsTrackingViewState {
			get { return ((IStateManager) _bag).IsTrackingViewState; }
		}

		void IStateManager.LoadViewState (object state) {
			((IStateManager) _bag).LoadViewState (state);
		}

		object IStateManager.SaveViewState () {
			return ((IStateManager) _bag).SaveViewState ();
		}

		void IStateManager.TrackViewState () {
			((IStateManager) _bag).TrackViewState ();
		}

		#endregion

		internal void SetDirty () {
			_bag.SetDirty (true);
		}

	}
}
