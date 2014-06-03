using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace Celeriq.Utilities
{
	/// <summary />
	public static class ArchiveDomain
	{
		/// <summary />
		public static bool ExtractArchive(string destinationFolder, string archiveFile)
		{
			var zip = ZipFile.Read(archiveFile);
			foreach (var item in zip)
			{
				item.Extract(destinationFolder, ExtractExistingFileAction.OverwriteSilently);
			}
			return true;
		}

		/// <summary />
		public static bool CreateArchive(string sourceFolder, string filter, string archiveFile)
		{
			using (var zip = new ZipFile())
			{
				var files = Directory.GetFiles(sourceFolder, filter, SearchOption.AllDirectories);
				foreach (var file in files)
				{
					zip.AddFile(file, string.Empty);
				}
				zip.Save(archiveFile);
			}
			return true;
		}
	}
}
