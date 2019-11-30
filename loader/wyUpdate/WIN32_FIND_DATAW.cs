using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace wyUpdate
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct WIN32_FIND_DATAW
	{
		private const int MAX_PATH = 260;

		public int dwFileAttributes;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;

		public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;

		public int nFileSizeHigh;

		public int nFileSizeLow;

		public int dwReserved0;

		public int dwReserved1;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string cFileName;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
		public string cAlternateFileName;
	}
}
