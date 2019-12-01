namespace GLib.Devices
{
	public struct structCommand
	{
		public string descr;

		public byte[] cmdSeq;

		private byte nData;

		public string GetDesc => descr;

		public byte nDATA => nData;

		public byte[] CMDSEQ => cmdSeq;

		public structCommand(string des, byte[] bySeq)
		{
			descr = des;
			cmdSeq = bySeq;
			nData = bySeq[1];
		}
	}
}
