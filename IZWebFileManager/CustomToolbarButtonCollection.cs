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
using System.Collections;

namespace IZ.WebFileManager
{
	public sealed class CustomToolbarButtonCollection : StateManagedCollection
	{
		private static readonly Type [] _knownTypes = new Type [] { typeof (CustomToolbarButton) };

		public CustomToolbarButton this [int i] {
			get { return (CustomToolbarButton) ((IList) this) [i]; }
			set { ((IList) this) [i] = value; }
		}

		public int Add (CustomToolbarButton button) {
			return ((IList) this).Add (button);
		}

		public bool Contains (CustomToolbarButton button) {
			return ((IList) this).Contains (button);
		}

		public void CopyTo (CustomToolbarButton [] buttonArray, int index) {
			base.CopyTo (buttonArray, index);
		}

		public int IndexOf (CustomToolbarButton button) {
			return ((IList) this).IndexOf (button);
		}

		public void Insert (int index, CustomToolbarButton button) {
			((IList) this).Insert (index, button);
		}

		public void Remove (CustomToolbarButton button) {
			((IList) this).Remove (button);
		}

		public void RemoveAt (int index) {
			((IList) this).RemoveAt (index);
		}

		protected override object CreateKnownType (int index) {
			if (index != 0)
				throw new ArgumentOutOfRangeException ("Unknown Type");

			return new CustomToolbarButton ();
		}

		protected override Type [] GetKnownTypes () {
			return _knownTypes;
		}

		protected override void SetDirtyObject (object o) {
			((CustomToolbarButton) o).SetDirty ();
		}
	}
}
