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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web.UI;
using System;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("WebFileManager")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("WebFileManager")]
[assembly: AssemblyCopyright("Copyright © 2006 Igor Zelmanovich <izwebfilemanager@gmail.com>")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
[assembly: CLSCompliant(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("0d47e5c8-af56-48cf-8f5a-b7c2a38a74ba")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("2.5.0.1")]
[assembly: TagPrefix("IZ.WebFileManager", "iz")]

[assembly: AllowPartiallyTrustedCallers]

[assembly: WebResource("IZ.WebFileManager.resources.FileManagerController.js", "application/x-javascript", PerformSubstitution=true)]
[assembly: WebResource("IZ.WebFileManager.resources.FileView.js", "application/x-javascript")]
[assembly: WebResource("IZ.WebFileManager.resources.BorderedPanel.js", "application/x-javascript")]
[assembly: WebResource ("IZ.WebFileManager.resources.FolderTree.js", "application/x-javascript")]

[assembly: WebResource("IZ.WebFileManager.resources.RootFolderSmall.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.RootFolderLarge.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.FileSmall.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.FolderSmall.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.FileLarge.gif", "image/gif")]
[assembly: WebResource ("IZ.WebFileManager.resources.FolderLarge.gif", "image/gif")]

[assembly: WebResource("IZ.WebFileManager.resources.FolderUp.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.Move.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.Copy.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.Rename.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.Delete.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.Process.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.View.gif", "image/gif")]
[assembly: WebResource ("IZ.WebFileManager.resources.NewFolder.gif", "image/gif")]
[assembly: WebResource ("IZ.WebFileManager.resources.Refresh.gif", "image/gif")]

[assembly: WebResource("IZ.WebFileManager.resources.Empty.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.PopOut.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.PopOutRtl.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.Bullet.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.CheckMark.gif", "image/gif")]

[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtndown_B.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtndown_L.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtndown_LB.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtndown_LT.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtndown_R.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtndown_RB.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtndown_RT.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtndown_T.gif", "image/gif")]

[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtnhover_B.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtnhover_L.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtnhover_LB.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtnhover_LT.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtnhover_R.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtnhover_RB.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtnhover_RT.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.toolbarbtnhover_T.gif", "image/gif")]

[assembly: WebResource("IZ.WebFileManager.resources.toolbarbg.gif", "image/gif")]

[assembly: WebResource("IZ.WebFileManager.resources.detailscolumnheader_R.gif", "image/gif")]
[assembly: WebResource("IZ.WebFileManager.resources.detailscolumnheader_RB.gif", "image/gif")]
[assembly: WebResource ("IZ.WebFileManager.resources.drag_copy.cur", "application/octet-stream")]
[assembly: WebResource ("IZ.WebFileManager.resources.drag_move.cur", "application/octet-stream")]
