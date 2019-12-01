using System;
using System.Drawing;
using System.Runtime.InteropServices;

public class ImageRasterHelper
{
	private static bool RGBEgual(Color c1, Color c2)
	{
		return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
	}

	private static bool RGBGreatEgual(Color c1, int R, int G, int B)
	{
		return c1.R >= R && c1.G >= G && c1.B >= B;
	}

	public static byte[] ConvertShortBitmap(Bitmap bitmap, bool bIncludeSize, bool bDoubleDensity)
	{
		int num = (bitmap.Height <= 8) ? 1 : 3;
		int num2 = bIncludeSize ? 3 : 0;
		int num3 = num * bitmap.Width;
		byte[] array = new byte[num3 + num2];
		int num4 = 0;
		while (num4 < array.Length)
		{
			array[num4++] = 0;
		}
		if (bIncludeSize)
		{
			array[0] = (byte)((num != 1) ? ((!bDoubleDensity) ? 32 : 33) : (bDoubleDensity ? 1 : 0));
			array[1] = (byte)(bitmap.Width & 0xFF);
			array[2] = (byte)((bitmap.Width & 0xFF00) >> 8);
		}
		for (int i = 0; i < bitmap.Width; i++)
		{
			for (int j = 0; j < bitmap.Height && j != 24; j++)
			{
				Color pixel = bitmap.GetPixel(i, j);
				if (!RGBGreatEgual(pixel, 255, 255, 128))
				{
					int num5 = num * i + j / 8;
					byte b = (byte)(128 >> j % 8);
					array[num5 + num2] |= b;
				}
			}
		}
		return array;
	}

	public static byte[] ConvertBitmap(Bitmap bitmap, bool bIncludeSize)
	{
		int num = bIncludeSize ? 2 : 0;
		int num2 = bitmap.Width / 8;
		if (num2 * 8 != bitmap.Width)
		{
			num2++;
		}
		int num3 = bitmap.Height / 8;
		if (num3 * 8 != bitmap.Height)
		{
			num3++;
		}
		if (num2 < 1 || num2 > 255 || num3 < 1 || num3 > 48 || num2 * num3 > 1536)
		{
			throw new Exception("Incorrect size");
		}
		byte[] array = new byte[num2 * num3 * 8 + (bIncludeSize ? 2 : 0)];
		int num4 = 0;
		while (num4 < array.Length)
		{
			array[num4++] = 0;
		}
		if (bIncludeSize)
		{
			array[0] = (byte)(num2 & 0xFF);
			array[1] = (byte)(num3 & 0xFF);
		}
		for (int i = 0; i < bitmap.Width; i++)
		{
			for (int j = 0; j < bitmap.Height; j++)
			{
				Color pixel = bitmap.GetPixel(i, j);
				if (!RGBGreatEgual(pixel, 255, 255, 128))
				{
					int num5 = num3 * i + j / 8;
					byte b = (byte)(128 >> j % 8);
					array[num5 + num] |= b;
				}
			}
		}
		return array;
	}

	[DllImport("gdi32.dll")]
	private static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);

	public static void ConvertHBitmap(IntPtr hdc, Bitmap bitmap, bool bIncludeSize)
	{
		for (int i = 0; i < bitmap.Width; i++)
		{
			for (int j = 0; j < bitmap.Height; j++)
			{
				if (bitmap.GetPixel(i, j).R == 0)
				{
					SetPixel(hdc, i, j, 0u);
				}
				else
				{
					SetPixel(hdc, i, j, 16777215u);
				}
			}
		}
	}
}
