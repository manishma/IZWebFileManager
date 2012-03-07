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
using System.Linq;
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
	    readonly DirectoryInfo directory;
	    readonly SortMode sort;
	    readonly SortDirection sortDirection;
	    private readonly string searchTerm;

		internal DirectoryProvider (DirectoryInfo directory, SortMode sort, SortDirection sortDirection, string searchTerm) {

			this.directory = directory;
			this.sort = sort;
			this.sortDirection = sortDirection;

            if (searchTerm != null)
            {
                searchTerm = searchTerm.Trim();
                if (!String.IsNullOrEmpty(searchTerm) && !searchTerm.Contains("*"))
                    searchTerm = "*" + searchTerm + "*";
            }

		    this.searchTerm = searchTerm;
		}

		public FileSystemInfo [] GetFileSystemInfos () {
			return GetFileSystemInfos (FileSystemInfosFilter.All);
		}

		public FileSystemInfo [] GetFileSystemInfos (FileSystemInfosFilter filter) {
			if (!directory.Exists)
				return new FileSystemInfo [0];

            FileSystemInfo[] dirs;
            switch (filter)
            {
                case FileSystemInfosFilter.Directories:
                    dirs = String.IsNullOrEmpty(searchTerm) 
                        ? directory.GetDirectories()
                        : directory.GetDirectories(searchTerm, SearchOption.AllDirectories);
                    break;
                case FileSystemInfosFilter.Files:
                    dirs = String.IsNullOrEmpty(searchTerm)
                        ? directory.GetFiles()
                        : directory.GetFiles(searchTerm, SearchOption.AllDirectories);
                    break;
                default:
                    dirs = String.IsNullOrEmpty(searchTerm)
                        ? directory.GetFileSystemInfos()
                        : SearchFilesAndDirectories();;
                    break;
            }
            Array.Sort<FileSystemInfo>(dirs, new Comparison<FileSystemInfo>(CompareFileSystemInfos));
            return dirs;
		}

        private FileSystemInfo[] SearchFilesAndDirectories()
        {
            var list = new List<FileSystemInfo>();
            list.AddRange(directory.GetDirectories(searchTerm, SearchOption.AllDirectories));
            list.AddRange(directory.GetFiles(searchTerm, SearchOption.AllDirectories));
            return list.ToArray();
        }


	    int CompareFileSystemInfos(FileSystemInfo file1, FileSystemInfo file2)
        {
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

		public enum FileSystemInfosFilter
		{
			All,
			Directories,
			Files
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
