using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

//#////////////////////////////////////////#//
//# Iterative Version of xDirectory.Copy() #//
//# Coder: John D. Storer II               #//
//# Date:  Thursday, May 18th, 2006        #//
//# Tool:  Visual C# 2005 Express Edition  #//
//#////////////////////////////////////////#//

namespace System.IO
{
	/// <summary>
	/// xDirectory v2.0 - Copy a Source Directory and it's SubDirectories/Files.
	/// Coder: John Storer II
	/// Date: Thursday, May 18, 2006
	/// </summary>
	internal class xDirectory
	{
		/// <summary>
		/// Default Overwrite Value - Change to Preference.
		/// </summary>
		private static bool _DefaultOverwrite = false;

		/// <summary>
		/// Default Folder Iteration Limit - Change to Preference.
		/// </summary>
		private static int _DefaultIterationLimit = 1000000;

		///////////////////////////////////////////////////////////
		/////////////////// String Copy Methods ///////////////////
		///////////////////////////////////////////////////////////

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="sSource">The Source Directory</param>
		/// <param name="sDestination">The Destination Directory</param>
		public static void Copy (string sSource, string sDestination) {
			Copy (new DirectoryInfo (sSource), new DirectoryInfo (sDestination), null, null, _DefaultOverwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="sSource">The Source Directory</param>
		/// <param name="sDestination">The Destination Directory</param>
		/// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
		public static void Copy (string sSource, string sDestination, bool Overwrite) {
			Copy (new DirectoryInfo (sSource), new DirectoryInfo (sDestination), null, null, Overwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="sSource">The Source Directory</param>
		/// <param name="sDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		public static void Copy (string sSource, string sDestination, string FileFilter) {
			Copy (new DirectoryInfo (sSource), new DirectoryInfo (sDestination), FileFilter, null, _DefaultOverwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="sSource">The Source Directory</param>
		/// <param name="sDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
		public static void Copy (string sSource, string sDestination, string FileFilter, bool Overwrite) {
			Copy (new DirectoryInfo (sSource), new DirectoryInfo (sDestination), FileFilter, null, Overwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="sSource">The Source Directory</param>
		/// <param name="sDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		public static void Copy (string sSource, string sDestination, string FileFilter, string DirectoryFilter) {
			Copy (new DirectoryInfo (sSource), new DirectoryInfo (sDestination), FileFilter, DirectoryFilter, _DefaultOverwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="sSource">The Source Directory</param>
		/// <param name="sDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
		public static void Copy (string sSource, string sDestination, string FileFilter, string DirectoryFilter, bool Overwrite) {
			Copy (new DirectoryInfo (sSource), new DirectoryInfo (sDestination), FileFilter, DirectoryFilter, Overwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="sSource">The Source Directory</param>
		/// <param name="sDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
		/// <param name="FolderLimit">Iteration Limit - Total Number of Folders/SubFolders to Copy</param>
		public static void Copy (string sSource, string sDestination, string FileFilter, string DirectoryFilter, bool Overwrite, int FolderLimit) {
			Copy (new DirectoryInfo (sSource), new DirectoryInfo (sDestination), FileFilter, DirectoryFilter, Overwrite, FolderLimit);
		}

		//////////////////////////////////////////////////////////////////
		/////////////////// DirectoryInfo Copy Methods ///////////////////
		//////////////////////////////////////////////////////////////////

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="diSource">The Source Directory</param>
		/// <param name="diDestination">The Destination Directory</param>
		public static void Copy (DirectoryInfo diSource, DirectoryInfo diDestination) {
			Copy (diSource, diDestination, null, null, _DefaultOverwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="diSource">The Source Directory</param>
		/// <param name="diDestination">The Destination Directory</param>
		/// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
		public static void Copy (DirectoryInfo diSource, DirectoryInfo diDestination, bool Overwrite) {
			Copy (diSource, diDestination, null, null, Overwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="diSource">The Source Directory</param>
		/// <param name="diDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		public static void Copy (DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter) {
			Copy (diSource, diDestination, FileFilter, null, _DefaultOverwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="diSource">The Source Directory</param>
		/// <param name="diDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
		public static void Copy (DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter, bool Overwrite) {
			Copy (diSource, diDestination, FileFilter, null, Overwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="diSource">The Source Directory</param>
		/// <param name="diDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		public static void Copy (DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter, string DirectoryFilter) {
			Copy (diSource, diDestination, FileFilter, DirectoryFilter, _DefaultOverwrite, _DefaultIterationLimit);
		}

		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="diSource">The Source Directory</param>
		/// <param name="diDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
		public static void Copy (DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter, string DirectoryFilter, bool Overwrite) {
			Copy (diSource, diDestination, FileFilter, DirectoryFilter, Overwrite, _DefaultIterationLimit);
		}


		/////////////////////////////////////////////////////////////////////
		/////////////////// The xDirectory.Copy() Method! ///////////////////
		/////////////////////////////////////////////////////////////////////


		/// <summary>
		/// xDirectory.Copy() - Copy a Source Directory and it's SubDirectories/Files
		/// </summary>
		/// <param name="diSource">The Source Directory</param>
		/// <param name="diDestination">The Destination Directory</param>
		/// <param name="FileFilter">The File Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="DirectoryFilter">The Directory Filter (Standard Windows Filter Parameter, Wildcards: "*" and "?")</param>
		/// <param name="Overwrite">Whether or not to Overwrite a Destination File if it Exists.</param>
		/// <param name="FolderLimit">Iteration Limit - Total Number of Folders/SubFolders to Copy</param>
		public static void Copy (DirectoryInfo diSource, DirectoryInfo diDestination, string FileFilter, string DirectoryFilter, bool Overwrite, int FolderLimit) {
			int iterator = 0;
			List<DirectoryInfo> diSourceList = new List<DirectoryInfo> ();
			List<FileInfo> fiSourceList = new List<FileInfo> ();

			try {
				///// Error Checking /////
				if (diSource == null)
					throw new ArgumentException ("Source Directory: NULL");
				if (diDestination == null)
					throw new ArgumentException ("Destination Directory: NULL");
				if (!diSource.Exists)
					throw new IOException ("Source Directory: Does Not Exist");
				if (!(FolderLimit > 0))
					throw new ArgumentException ("Folder Limit: Less Than 1");
				if (DirectoryFilter == null || DirectoryFilter == string.Empty)
					DirectoryFilter = "*";
				if (FileFilter == null || FileFilter == string.Empty)
					FileFilter = "*";

				///// Add Source Directory to List /////
				diSourceList.Add (diSource);

				///// First Section: Get Folder/File Listing /////
				while (iterator < diSourceList.Count && iterator < FolderLimit) {
					foreach (DirectoryInfo di in diSourceList [iterator].GetDirectories (DirectoryFilter))
						diSourceList.Add (di);

					foreach (FileInfo fi in diSourceList [iterator].GetFiles (FileFilter))
						fiSourceList.Add (fi);

					iterator++;
				}

				///// Second Section: Create Folders from Listing /////
				foreach (DirectoryInfo di in diSourceList) {
					if (di.Exists) {
						string sFolderPath = diDestination.FullName + di.FullName.Remove (0, diSource.FullName.Length);

						///// Prevent Silly IOException /////
						if (!Directory.Exists (sFolderPath))
							Directory.CreateDirectory (sFolderPath);
					}
				}

				///// Third Section: Copy Files from Listing /////
				foreach (FileInfo fi in fiSourceList) {
					if (fi.Exists) {
						string sFilePath = diDestination.FullName + fi.FullName.Remove (0, diSource.FullName.Length);

						///// Better Overwrite Test W/O IOException from CopyTo() /////
						if (Overwrite)
							fi.CopyTo (sFilePath, true);
						else {
							///// Prevent Silly IOException /////
							if (!File.Exists (sFilePath))
								fi.CopyTo (sFilePath, true);
						}
					}
				}
			}
			catch { throw; }
		}
	}
}
