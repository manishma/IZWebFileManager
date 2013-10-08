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
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;

namespace IZ.WebFileManager
{
	//public delegate void RenameCancelEventHandler(object sender, RenameCancelEventArgs e);
	//public class RenameCancelEventArgs : FileManagerCancelEventArgs
	//{
	//    internal RenameCancelEventArgs() { }
	//}

	//public delegate void RenameEventHandler(object sender, RenameEventArgs e);
	//public class RenameEventArgs : EventArgs
	//{
	//    internal RenameEventArgs() { }
	//}

	//public delegate void ExecuteCommandEventHandler(object sender, ExecuteCommandEventArgs e);
	public class ExecuteCommandEventArgs : CommandEventArgs
	{
		internal ExecuteCommandEventArgs (string commandName, object argument) : base (commandName, argument) { }

		private string clientScript = "";

		public string ClientScript {
			get { return clientScript == null ? String.Empty : clientScript; }
			set { clientScript = value; }
		}

		private Collection<FileManagerItemInfo> items = new Collection<FileManagerItemInfo> ();

		public Collection<FileManagerItemInfo> Items {
			get { return items; }
		}

	}

	//public delegate void RenameCancelEventHandler(object sender, RenameCancelEventArgs e);
	public class RenameCancelEventArgs : FileManagerCancelEventArgs
	{
		internal RenameCancelEventArgs () { }

		private FileManagerItemInfo fileManagerItem;

		public FileManagerItemInfo FileManagerItem {
			get { return fileManagerItem; }
			internal set { fileManagerItem = value; }
		}

		private string newName;

		public string NewName {
			get { return newName; }
			set { newName = value; }
		}

	}

	//public delegate void RenameEventHandler(object sender, RenameEventArgs e);
	public class RenameEventArgs : EventArgs
	{
		internal RenameEventArgs () { }

		private FileManagerItemInfo fileManagerItem;

		public FileManagerItemInfo FileManagerItem {
			get { return fileManagerItem; }
			internal set { fileManagerItem = value; }
		}

	}


	//public delegate void NewFolderCancelEventHandler(object sender, NewFolderCancelEventArgs e);
	public class NewFolderCancelEventArgs : FileManagerCancelEventArgs
	{
		internal NewFolderCancelEventArgs () { }

		private FileManagerItemInfo destDir;

		public FileManagerItemInfo DestinationDirectory {
			get { return destDir; }
			internal set { destDir = value; }
		}
	}

	//public delegate void NewFolderEventHandler(object sender, NewFolderEventArgs e);
	public class NewFolderEventArgs : EventArgs
	{
		internal NewFolderEventArgs () { }

		private FileManagerItemInfo newFolder;

		public FileManagerItemInfo NewFolder {
			get { return newFolder; }
			internal set { newFolder = value; }
		}


	}

	//public delegate void NewDocumentEventHandler(object sender, NewDocumentEventArgs e);
	public class NewDocumentEventArgs : EventArgs
	{
		internal NewDocumentEventArgs () { }
		private NewDocumentTemplate template;

		public NewDocumentTemplate Template {
			get { return template; }
			internal set { template = value; }
		}

		private FileManagerItemInfo newDocument;

		public FileManagerItemInfo NewDocument {
			get { return newDocument; }
			internal set { newDocument = value; }
		}


	}

	//public delegate void NewDocumentCancelEventHandler(object sender, NewDocumentCancelEventArgs e);
	public class NewDocumentCancelEventArgs : FileManagerCancelEventArgs
	{

		internal NewDocumentCancelEventArgs () { }
		private NewDocumentTemplate template;

		public NewDocumentTemplate Template {
			get { return template; }
			internal set { template = value; }
		}

		private FileManagerItemInfo destDir;

		public FileManagerItemInfo DestinationDirectory {
			get { return destDir; }
			internal set { destDir = value; }
		}
	}

	public enum SelectedItemsAction { Move, Copy, Delete, Open }

	public class SelectedItemsActionCancelEventArgs : FileManagerCancelEventArgs
	{
		private SelectedItemsAction _action;
		public SelectedItemsAction Action {
			get { return _action; }
		}

		internal SelectedItemsActionCancelEventArgs (SelectedItemsAction action) {
			_action = action;
		}

		private Collection<FileManagerItemInfo> items = new Collection<FileManagerItemInfo> ();

		public Collection<FileManagerItemInfo> SelectedItems {
			get { return items; }
			//internal set { items = value; }
		}
		private FileManagerItemInfo destDir;

		public FileManagerItemInfo DestinationDirectory {
			get { return destDir; }
			internal set { destDir = value; }
		}

	}
	public class SelectedItemsActionEventArgs : EventArgs
	{
		internal SelectedItemsActionEventArgs () { }
	}
	//public delegate void UploadFileCancelEventHandler(object sender, UploadFileCancelEventArgs e);
	public class UploadFileCancelEventArgs : FileManagerCancelEventArgs
	{

		internal UploadFileCancelEventArgs () { }

		private FileManagerItemInfo destDir;

		public FileManagerItemInfo DestinationDirectory {
			get { return destDir; }
			internal set { destDir = value; }
		}

		private HttpPostedFile postedFile;

		public HttpPostedFile PostedFile {
			get { return postedFile; }
			internal set { postedFile = value; }
		}

		private string saveName;

		public string SaveName {
			get { return (saveName == null || saveName.Length == 0) ? Path.GetFileName (PostedFile.FileName) : saveName; }
			set { saveName = value; }
		}

	}

	//public delegate void UploadFileEventHandler(object sender, UploadFileEventArgs e);
	public class UploadFileEventArgs : EventArgs
	{

		internal UploadFileEventArgs () { }

		private FileManagerItemInfo uploadedFile;

		public FileManagerItemInfo UploadedFile {
			get { return uploadedFile; }
			internal set { uploadedFile = value; }
		}
	}

	public class DownloadFileCancelEventArgs : CancelEventArgs
	{
		public FileManagerItemInfo DownloadFile { get; internal set; }
	}

	public class FileManagerCancelEventArgs : CancelEventArgs
	{
		private string clientMessage;

		public string ClientMessage {
			get { return clientMessage; }
			set { clientMessage = value; }
		}

	}

}
