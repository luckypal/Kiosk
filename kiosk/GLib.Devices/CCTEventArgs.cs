namespace GLib.Devices
{
	public class CCTEventArgs
	{
		public byte[] buf;

		public byte CCTEvType;

		public int idxEnd;

		public int idxStart;

		public ushort msgChk;

		public int nData;

		public CCTEventArgs()
		{
			nData = (idxStart = (idxEnd = (msgChk = (CCTEvType = 0))));
			buf = new byte[1024];
		}

		public CCTEventArgs(int n)
			: this()
		{
			nData = n;
		}

		public CCTEventArgs(int start, int end)
			: this()
		{
			idxStart = start;
			idxEnd = end;
			nData = end - start;
		}
	}
}
