using System.Collections.Generic;

namespace wyUpdate.Common
{
	public class VersionChoice
	{
		public string Version;

		public string Changes;

		public bool RTFChanges;

		public List<string> FileSites = new List<string>();

		public long FileSize;

		public long Adler32;

		public byte[] SignedSHA1Hash;

		public InstallingTo InstallingTo;
	}
}
