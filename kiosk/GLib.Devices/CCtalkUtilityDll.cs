using System.Runtime.InteropServices;

namespace GLib.Devices
{
	public class CCtalkUtilityDll
	{
		[DllImport("CRCDLL.dll")]
		public unsafe static extern ushort crcCalculation(int nLen, byte* pC, ushort seed);
	}
}
