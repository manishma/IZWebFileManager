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

namespace IZ.WebFileManager
{
	public sealed class FileManagerItemInfo
	{
		private string _fileManagerPath;
		private string _virtualPath;
		private string _physicalPath;
		private FileInfo _file;
		private DirectoryInfo _directory;

		public string FileManagerPath {
			get { return _fileManagerPath; }
		}

		[Obsolete ("this property is obsolete, use VirtualPath instead")]
		public string AbsolutePath {
			get { return VirtualPath; }
		}

		public string VirtualPath {
			get { return _virtualPath; }
		}

		internal FileInfo File {
			get {
				if (_file == null)
					_file = new FileInfo (PhysicalPath);
				return _file;
			}
		}

		internal DirectoryInfo Directory {
			get {
				if (_directory == null)
					_directory = new DirectoryInfo (PhysicalPath);
				return _directory;
			}
		}

		public string PhysicalPath {
			get { return _physicalPath; }
		}

		internal FileManagerItemInfo (string fileManagerPath, string virtualPath, string phisicalPath) {
			_fileManagerPath = fileManagerPath;
			_virtualPath = virtualPath;
			_physicalPath = phisicalPath;
		}

		internal void EnsureDirectoryExists () {
			if (!Directory.Exists)
				Directory.Create ();
		}
	}
}
