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
	public sealed class FileTypeCollection : StateManagedCollection
	{
		private static readonly Type [] _knownTypes = new Type [] { typeof (FileType) };

		public FileType this [int i] {
			get { return (FileType) ((IList) this) [i]; }
			set { ((IList) this) [i] = value; }
		}

		public int Add (FileType fileType) {
			return ((IList) this).Add (fileType);
		}

		public bool Contains (FileType fileType) {
			return ((IList) this).Contains (fileType);
		}

		public void CopyTo (FileType [] fileTypeArray, int index) {
			base.CopyTo (fileTypeArray, index);
		}

		public int IndexOf (FileType fileType) {
			return ((IList) this).IndexOf (fileType);
		}

		public void Insert (int index, FileType fileType) {
			((IList) this).Insert (index, fileType);
		}

		public void Remove (FileType fileType) {
			((IList) this).Remove (fileType);
		}

		public void RemoveAt (int index) {
			((IList) this).RemoveAt (index);
		}

		protected override object CreateKnownType (int index) {
			if (index != 0)
				throw new ArgumentOutOfRangeException ("Unknown Type");

			return new FileType ();
		}

		protected override Type [] GetKnownTypes () {
			return _knownTypes;
		}

		protected override void SetDirtyObject (object o) {
			((FileType) o).SetDirty ();
		}
	}
}
