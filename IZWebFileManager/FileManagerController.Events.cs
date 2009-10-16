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

namespace IZ.WebFileManager
{
	partial class FileManagerController
	{
		#region Events

		[Category ("Action")]
		public event EventHandler<RenameCancelEventArgs> ItemRenaming;

		[Category ("Action")]
		public event EventHandler<NewDocumentCancelEventArgs> NewDocumentCreating;

		[Category ("Action")]
		public event EventHandler<NewDocumentEventArgs> NewDocumentCreated;

		[Category ("Action")]
		public event EventHandler<ExecuteCommandEventArgs> ExecuteCommand;

		[Category ("Action")]
		public event EventHandler<RenameEventArgs> ItemRenamed;

		[Category ("Action")]
		public event EventHandler<NewFolderCancelEventArgs> NewFolderCreating;

		[Category ("Action")]
		public event EventHandler<NewFolderEventArgs> NewFolderCreated;

		[Category ("Action")]
		public event EventHandler<UploadFileCancelEventArgs> FileUploading;

		[Category ("Action")]
		public event EventHandler<UploadFileEventArgs> FileUploaded;

		[Category ("Action")]
		public event EventHandler<SelectedItemsActionCancelEventArgs> SelectedItemsAction;

		[Category ("Action")]
		public event EventHandler<SelectedItemsActionEventArgs> SelectedItemsActionComplete;

		[Category ("Action")]
		public event EventHandler<DownloadFileCancelEventArgs> FileDownload;

		#endregion

		#region OnEvent Methods

		private void OnItemRenaming (RenameCancelEventArgs e) {
			if (ItemRenaming != null)
				ItemRenaming (this, e);
		}

		private void OnItemRenamed (RenameEventArgs e) {
			if (ItemRenamed != null)
				ItemRenamed (this, e);
		}

		private void OnNewFolderCreating (NewFolderCancelEventArgs e) {
			if (NewFolderCreating != null)
				NewFolderCreating (this, e);
		}

		private void OnNewFolderCreated (NewFolderEventArgs e) {
			if (NewFolderCreated != null)
				NewFolderCreated (this, e);
		}

		private void OnFileUploading (UploadFileCancelEventArgs e) {
			if (FileUploading != null)
				FileUploading (this, e);
		}

		private void OnFileUploaded (UploadFileEventArgs e) {
			if (FileUploaded != null)
				FileUploaded (this, e);
		}

		private void OnSelectedItemsAction (SelectedItemsActionCancelEventArgs e) {
			if (SelectedItemsAction != null)
				SelectedItemsAction (this, e);
		}

		private void OnSelectedItemsActionComplete (SelectedItemsActionEventArgs e) {
			if (SelectedItemsActionComplete != null)
				SelectedItemsActionComplete (this, e);
		}

		private void OnNewDocumentCreating (NewDocumentCancelEventArgs e) {
			if (NewDocumentCreating != null)
				NewDocumentCreating (this, e);
		}

		private void OnNewDocumentCreated (NewDocumentEventArgs e) {
			if (NewDocumentCreated != null)
				NewDocumentCreated (this, e);
		}

		private void OnExecuteCommand (ExecuteCommandEventArgs e) {
			if (ExecuteCommand != null)
				ExecuteCommand (this, e);
		}

		#endregion
	}
}
