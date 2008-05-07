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
using System.Web.UI;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Drawing.Design;
using System.Diagnostics.CodeAnalysis;

namespace IZ.WebFileManager
{
	[PersistChildren (false)]
	[ParseChildren (true)]
	public sealed class FileType : IStateManager
	{
		readonly StateBag bag = new StateBag ();
		readonly FileManagerCommandCollection fileManagerCommandCollection = new FileManagerCommandCollection ();

		[MergableProperty (true)]
		[PersistenceMode (PersistenceMode.InnerProperty)]
		public FileManagerCommandCollection Commands {
			get { return fileManagerCommandCollection; }
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		[DefaultValue ("")]
		[UrlProperty]
		[Bindable (true)]
		public string SmallImageUrl {
			get { return bag ["SmallIconUrl"] == null ? String.Empty : (string) bag ["SmallIconUrl"]; }
			set { bag ["SmallIconUrl"] = value; }
		}

		[SuppressMessage ("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		[Editor ("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof (UITypeEditor))]
		[DefaultValue ("")]
		[UrlProperty]
		[Bindable (true)]
		public string LargeImageUrl {
			get { return bag ["LargeIconUrl"] == null ? String.Empty : (string) bag ["LargeIconUrl"]; }
			set { bag ["LargeIconUrl"] = value; }
		}

		[DefaultValue ("")]
		public string Extensions {
			get { return bag ["Extensions"] == null ? String.Empty : (string) bag ["Extensions"]; }
			set { bag ["Extensions"] = value; }
		}

		[DefaultValue ("")]
		public string Name {
			get { return bag ["Name"] == null ? String.Empty : (string) bag ["Name"]; }
			set { bag ["Name"] = value; }
		}


		#region IStateManager Members

		bool IStateManager.IsTrackingViewState {
			get { return ((IStateManager) bag).IsTrackingViewState; }
		}

		void IStateManager.LoadViewState (object state) {
			if (state == null)
				return;

			object [] states = (object []) state;

			((IStateManager) bag).LoadViewState (states [0]);
			((IStateManager) Commands).LoadViewState (states [1]);
		}

		object IStateManager.SaveViewState () {
			object [] states = new object [2];

			states [0] = ((IStateManager) bag).SaveViewState ();
			states [1] = ((IStateManager) Commands).SaveViewState ();

			for (int i = 0; i < states.Length; i++) {
				if (states [i] != null)
					return states;
			}
			return null;
		}

		void IStateManager.TrackViewState () {
			((IStateManager) bag).TrackViewState ();
			((IStateManager) Commands).TrackViewState ();
		}

		#endregion

		internal void SetDirty () {
			bag.SetDirty (true);
		}
	}
}
