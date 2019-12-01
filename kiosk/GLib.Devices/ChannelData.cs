namespace GLib.Devices
{
	public class ChannelData
	{
		public int Value;

		public byte Channel;

		public char[] Currency;

		public int Level;

		public bool Recycling;

		public bool Inhibit;

		public bool Enabled;

		public ChannelData()
		{
			Value = 0;
			Channel = 0;
			Currency = new char[3];
			Level = 0;
			Inhibit = false;
			Enabled = false;
			Recycling = false;
		}
	}
}
