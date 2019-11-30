using System;
using System.IO;

namespace wyUpdate.Compression.Vcdiff
{
	internal sealed class AddressCache
	{
		private const byte SelfMode = 0;

		private const byte HereMode = 1;

		private int nearSize;

		private int sameSize;

		private int[] near;

		private int nextNearSlot;

		private int[] same;

		private Stream addressStream;

		internal AddressCache(int nearSize, int sameSize)
		{
			this.nearSize = nearSize;
			this.sameSize = sameSize;
			near = new int[nearSize];
			same = new int[sameSize * 256];
		}

		internal void Reset(byte[] addresses)
		{
			nextNearSlot = 0;
			Array.Clear(near, 0, near.Length);
			Array.Clear(same, 0, same.Length);
			addressStream = new MemoryStream(addresses, writable: false);
		}

		internal int DecodeAddress(int here, byte mode)
		{
			int num;
			switch (mode)
			{
			case 0:
				num = IOHelper.ReadBigEndian7BitEncodedInt(addressStream);
				break;
			case 1:
				num = here - IOHelper.ReadBigEndian7BitEncodedInt(addressStream);
				break;
			default:
			{
				if (mode - 2 < nearSize)
				{
					num = near[mode - 2] + IOHelper.ReadBigEndian7BitEncodedInt(addressStream);
					break;
				}
				int num2 = mode - (2 + nearSize);
				num = same[num2 * 256 + IOHelper.CheckedReadByte(addressStream)];
				break;
			}
			}
			Update(num);
			return num;
		}

		private void Update(int address)
		{
			if (nearSize > 0)
			{
				near[nextNearSlot] = address;
				nextNearSlot = (nextNearSlot + 1) % nearSize;
			}
			if (sameSize > 0)
			{
				same[address % (sameSize * 256)] = address;
			}
		}
	}
}
