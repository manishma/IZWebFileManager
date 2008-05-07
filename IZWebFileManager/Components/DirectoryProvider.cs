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
using System.IO;
using System.Web;
using System.Collections;
using System.Globalization;

namespace IZ.WebFileManager.Components
{
	internal sealed class DirectoryProvider
	{
		DirectoryInfo directory;
		SortMode sort;
		SortDirection sortDirection;
		Hashtable groups;

		internal DirectoryProvider (DirectoryInfo directory, SortMode sort, SortDirection sortDirection) {

			this.directory = directory;
			this.sort = sort;
			this.sortDirection = sortDirection;
		}

		public GroupInfo [] GetGroups () {
			groups = new Hashtable ();
			FileSystemInfo [] fsis = GetFileSystemInfos ();

			ArrayList groupInfos = new ArrayList ();

			switch (sort) {
			case SortMode.Name:
				Hashtable letters = new Hashtable ();
				foreach (FileSystemInfo fsi in fsis) {
					string l = fsi.Name.Substring (0, 1).ToUpper (CultureInfo.InvariantCulture);
					GroupInfo gi = (GroupInfo) letters [l];
					if (gi == null) {
						gi = new GroupInfo (l);
						letters [l] = gi;
					}
					ArrayList gfsis = (ArrayList) groups [gi];
					if (gfsis == null) {
						gfsis = new ArrayList ();
						groups [gi] = gfsis;
						groupInfos.Add (gi);
					}
					gfsis.Add (fsi);
				}
				GroupInfo [] gis = (GroupInfo []) groupInfos.ToArray (typeof (GroupInfo));
				Array.Sort<GroupInfo> (gis, new Comparison<GroupInfo> (CompareGroupInfos));
				return gis;
			case SortMode.Modified:
				break;
			case SortMode.Type:
				break;
			case SortMode.Size:
				break;
			}

			return (GroupInfo []) groupInfos.ToArray (typeof (GroupInfo));
		}

		public FileSystemInfo [] GetFileSystemInfos () {
			if (!directory.Exists)
				return new FileSystemInfo [0];

			FileSystemInfo [] dirs = directory.GetFileSystemInfos ();
			Array.Sort<FileSystemInfo> (dirs, new Comparison<FileSystemInfo> (CompareFileSystemInfos));
			return dirs;
		}


		public FileSystemInfo [] GetFileSystemInfos (GroupInfo group) {
			return (FileSystemInfo []) ((ArrayList) groups [group]).ToArray (typeof (FileSystemInfo));
		}

		int CompareGroupInfos (GroupInfo gi1, GroupInfo gi2) {
			int res = 0;
			switch (sort) {
			case SortMode.Name:
				res = String.Compare (gi1.Name, gi2.Name);
				break;
			}

			if (sortDirection == SortDirection.Descending)
				res = -res;

			return res;
		}

		int CompareFileSystemInfos (FileSystemInfo file1, FileSystemInfo file2) {
			int res = 0;
			FileInfo f1 = file1 as FileInfo;
			FileInfo f2 = file2 as FileInfo;
			switch (sort) {
			case SortMode.Name:
				res = String.Compare ((f1 == null ? "1" : "2") + file1.Name, (f2 == null ? "1" : "2") + file2.Name);
				break;
			case SortMode.Modified:
				res = String.Compare ((f1 == null ? "1" : "2") + file1.LastWriteTime.ToString ("s", null), (f2 == null ? "1" : "2") + file2.LastWriteTime.ToString ("s", null));
				break;
			case SortMode.Type:
				res = String.Compare (f1 != null ? f1.Extension.ToLower (CultureInfo.InvariantCulture) + file1.Name : String.Empty, f2 != null ? f2.Extension.ToLower (CultureInfo.InvariantCulture) + file2.Name : String.Empty);
				break;
			case SortMode.Size:
				long length1 = f1 != null ? f1.Length : -1;
				long length2 = f2 != null ? f2.Length : -1;
				res = Math.Sign (length1 - length2);
				break;
			}

			if (sortDirection == SortDirection.Descending)
				res = -res;

			return res;
		}

	}
	internal sealed class GroupInfo
	{
		private string name;

		public string Name {
			get { return name; }
			set { name = value; }
		}

		internal GroupInfo (string name) {
			this.name = name;
		}
	}

}
