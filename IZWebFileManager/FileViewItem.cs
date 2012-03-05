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
using System.IO;
using System.Web;
using System.Globalization;

namespace IZ.WebFileManager
{
	sealed class FileViewItem
	{
		string _thumbnailImage;
		string _smallImage;
		string _largeImage;
		string _size;
		string _type;
		string _modified;
		string _cliendID;
	    readonly FileSystemInfo _fsi;
		readonly FileManagerControlBase _fileView;
		bool? _hidden;

	    public FileSystemInfo FileSystemInfo {
			get { return _fsi; }
		}

		public bool IsDirectory {
			get { return _fsi is DirectoryInfo; }
		}

		public string ClientID {
			get { return _cliendID; }
			set { _cliendID = value; }
		}

		public bool CanBeRenamed {
			get { return true; }
		}

		public string SmallImage {
			get {
				if (_smallImage == null)
					_smallImage = _fileView.Controller.GetItemSmallImage (_fsi);
				return _smallImage;
			}
		}

		public string LargeImage {
			get {
				if (_largeImage == null)
					_largeImage = _fileView.Controller.GetItemLargeImage (_fsi);
				return _largeImage;
			}
		}

		public string ThumbnailImage {
			get {
				if (_thumbnailImage == null)
					_thumbnailImage = _fileView.Controller.GetItemThumbnailImage (this, _fileView.CurrentDirectory);
				return _thumbnailImage;
			}
		}

		public string Info {
			get { return String.Empty; }
		}

		public string Size {
			get {
				if (_size == null)
					_size = _fsi is DirectoryInfo ? "&nbsp;" : FileSizeToString (((FileInfo) _fsi).Length);
				return _size;
			}
		}

		public string Type {
			get {
				if (_type == null)
					_type = GetItemType (_fsi);
				return _type;
			}
		}

		public string Modified {
			get {
				if (_modified == null)
					_modified = _fsi.LastWriteTime.ToString ("g", null);
				return _modified;
			}
		}

		public string Name {
			get { return _fsi.Name; }
		}

	    internal string RelativePath { get; private set; }

		public bool Hidden {
			get {
				if (!_hidden.HasValue) {
					_hidden = (!String.IsNullOrEmpty (_fileView.HiddenFilesAndFoldersPrefix) && _fsi.Name.StartsWith (_fileView.HiddenFilesAndFoldersPrefix, StringComparison.InvariantCultureIgnoreCase));
					if (!_hidden.Value && _fsi is FileInfo) {
						string ext = _fsi.Extension.ToLower (CultureInfo.InvariantCulture).TrimStart ('.');
						_hidden = _fileView.Controller.HiddenFilesArray.Contains (ext);
					}
				}
				return _hidden.Value;
			}
		}

		internal FileViewItem (DirectoryInfo parentDirectory, FileSystemInfo fsi, FileManagerControlBase fileView) {
		    this._fsi = fsi;
			this._fileView = fileView;
		    RelativePath = fsi.FullName.Substring(parentDirectory.FullName.TrimEnd(Path.DirectorySeparatorChar).Length + 1).Replace(Path.DirectorySeparatorChar, '/');
		}

		string GetItemType (FileSystemInfo fsi) {
			if (fsi is DirectoryInfo)
				return _fileView.Controller.GetResourceString ("File_Folder", "File Folder");
			else {
				FileInfo file = (FileInfo) fsi;
				FileType ft = _fileView.Controller.GetFileType (file);
				if (ft != null && ft.Name.Length > 0)
					return ft.Name;
				else {
					return file.Extension.ToUpper (CultureInfo.InvariantCulture).TrimStart ('.') + " File";
				}
			}

		}

		static string FileSizeToString (long size) {
			if (size < 1024)
				return size.ToString (null, null) + " B";
			else if (size < 1048576)
				return (size / 1024).ToString (null, null) + " KB";
			else
				return (size / 1048576).ToString (null, null) + " MB";

		}
	}
}
