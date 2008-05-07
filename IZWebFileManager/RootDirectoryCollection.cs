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

namespace IZ.WebFileManager
{
	public sealed class RootDirectoryCollection : StateManagedCollection
	{
		private static readonly Type [] _knownTypes = new Type [] { typeof (RootDirectory) };

		public RootDirectory this [int i] {
			get { return (RootDirectory) ((IList) this) [i]; }
			set { ((IList) this) [i] = value; }
		}

		public RootDirectory this [string name] {
			get {
				for (int i = 0; i < Count; i++) {
					RootDirectory dir = this [i];
					if (String.Compare (dir.TextInternal, name, true) == 0)
						return dir;
				}
				return null;
			}
		}

		public int Add (RootDirectory rootDirectory) {
			return ((IList) this).Add (rootDirectory);
		}

		public bool Contains (RootDirectory rootDirectory) {
			return ((IList) this).Contains (rootDirectory);
		}

		public void CopyTo (RootDirectory [] rootDirectoryArray, int index) {
			base.CopyTo (rootDirectoryArray, index);
		}

		public int IndexOf (RootDirectory rootDirectory) {
			return ((IList) this).IndexOf (rootDirectory);
		}

		public void Insert (int index, RootDirectory rootDirectory) {
			((IList) this).Insert (index, rootDirectory);
		}

		public void Remove (RootDirectory rootDirectory) {
			((IList) this).Remove (rootDirectory);
		}

		public void RemoveAt (int index) {
			((IList) this).RemoveAt (index);
		}

		protected override object CreateKnownType (int index) {
			if (index != 0)
				throw new ArgumentOutOfRangeException ("Unknown Type");

			return new RootDirectory ();
		}

		protected override Type [] GetKnownTypes () {
			return _knownTypes;
		}

		protected override void SetDirtyObject (object o) {
			((RootDirectory) o).SetDirty ();
		}
	}
}
