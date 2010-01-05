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
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Design;
using System.Web;

namespace IZ.WebFileManager
{
	public sealed class RootDirectory : IStateManager
	{
		readonly StateBag bag = new StateBag ();

		[DefaultValue ("~/")]
		public string DirectoryPath {
			get { return bag ["DirectoryPath"] == null ? "~/" : (string) bag ["DirectoryPath"]; }
			set { bag ["DirectoryPath"] = value; }
		}

		/// <summary>
		/// Gets or sets the number of levels that are expanded in the folder tree when a FileManager control is displayed for the first time. 
		/// </summary>
		/// <value>
		/// The depth to display in the folder tree when the FileManager is initially displayed. The default is -1, which displays all the nodes.
		/// </value>
		[DefaultValue (1)]
		public int ExpandDepth {
			get { return bag ["ExpandDepth"] == null ? 1 : (int) bag ["ExpandDepth"]; }
			set { bag ["ExpandDepth"] = value; }
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

		/// <summary>
		/// Gets or sets the text displayed for the node in the folder tree of FileManager control.
		/// </summary>
		[DefaultValue ("")]
		public string Text {
			get { return (string) bag ["Text"] ?? String.Empty; }
			set {
				// validate value must be valid file/folder name 
				if (!FileManagerController.Validate (value))
					throw new ArgumentException (String.Format ("'{0}' is not a valid value for RootDirectory.Text, It cannot contain any of the following characters: \\/:*?\"<>|", value));
				bag ["Text"] = value;
			}
		}

		internal string TextInternal {
			get {
				if (String.IsNullOrEmpty (Text))
					return VirtualPathUtility.GetFileName (DirectoryPath);
				return Text;
			}
		}

		[DefaultValue (true)]
		public bool ShowRootIndex {
			get { return bag ["ShowRootIndex"] == null ? true : (bool) bag ["ShowRootIndex"]; }
			set {
				bag ["ShowRootIndex"] = value;
			}
		}

		/// <summary>
		/// Overridden
		/// </summary>
		/// <returns></returns>
		public override string ToString () {
			return DirectoryPath;
		}

		#region IStateManager Members

		bool IStateManager.IsTrackingViewState {
			get { return ((IStateManager) bag).IsTrackingViewState; }
		}

		void IStateManager.LoadViewState (object state) {
			((IStateManager) bag).LoadViewState (state);
		}

		object IStateManager.SaveViewState () {
			return ((IStateManager) bag).SaveViewState ();
		}

		void IStateManager.TrackViewState () {
			((IStateManager) bag).TrackViewState ();
		}

		#endregion

		internal void SetDirty () {
			bag.SetDirty (true);
		}
	}
}
