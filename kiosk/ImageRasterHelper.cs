// Decompiled with JetBrains decompiler
// Type: ImageRasterHelper
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.Drawing;
using System.Runtime.InteropServices;

public class ImageRasterHelper
{
  private static bool RGBEgual(Color c1, Color c2)
  {
    return (int) c1.R == (int) c2.R && (int) c1.G == (int) c2.G && (int) c1.B == (int) c2.B;
  }

  private static bool RGBGreatEgual(Color c1, int R, int G, int B)
  {
    return (int) c1.R >= R && (int) c1.G >= G && (int) c1.B >= B;
  }

  public static byte[] ConvertShortBitmap(Bitmap bitmap, bool bIncludeSize, bool bDoubleDensity)
  {
    int num1 = bitmap.Height <= 8 ? 1 : 3;
    int num2 = bIncludeSize ? 3 : 0;
    byte[] numArray = new byte[num1 * bitmap.Width + num2];
    int num3 = 0;
    while (num3 < numArray.Length)
      numArray[num3++] = (byte) 0;
    if (bIncludeSize)
    {
      numArray[0] = num1 == 1 ? (!bDoubleDensity ? (byte) 0 : (byte) 1) : (!bDoubleDensity ? (byte) 32 : (byte) 33);
      numArray[1] = (byte) (bitmap.Width & (int) byte.MaxValue);
      numArray[2] = (byte) ((bitmap.Width & 65280) >> 8);
    }
    for (int x = 0; x < bitmap.Width; ++x)
    {
      for (int y = 0; y < bitmap.Height && y != 24; ++y)
      {
        if (!ImageRasterHelper.RGBGreatEgual(bitmap.GetPixel(x, y), (int) byte.MaxValue, (int) byte.MaxValue, 128))
        {
          int num4 = num1 * x + y / 8;
          byte num5 = (byte) (128 >> y % 8);
          numArray[num4 + num2] |= num5;
        }
      }
    }
    return numArray;
  }

  public static byte[] ConvertBitmap(Bitmap bitmap, bool bIncludeSize)
  {
    int num1 = bIncludeSize ? 2 : 0;
    int num2 = bitmap.Width / 8;
    if (num2 * 8 != bitmap.Width)
      ++num2;
    int num3 = bitmap.Height / 8;
    if (num3 * 8 != bitmap.Height)
      ++num3;
    if (num2 < 1 || num2 > (int) byte.MaxValue || (num3 < 1 || num3 > 48) || num2 * num3 > 1536)
      throw new Exception("Incorrect size");
    byte[] numArray = new byte[num2 * num3 * 8 + (bIncludeSize ? 2 : 0)];
    int num4 = 0;
    while (num4 < numArray.Length)
      numArray[num4++] = (byte) 0;
    if (bIncludeSize)
    {
      numArray[0] = (byte) (num2 & (int) byte.MaxValue);
      numArray[1] = (byte) (num3 & (int) byte.MaxValue);
    }
    for (int x = 0; x < bitmap.Width; ++x)
    {
      for (int y = 0; y < bitmap.Height; ++y)
      {
        if (!ImageRasterHelper.RGBGreatEgual(bitmap.GetPixel(x, y), (int) byte.MaxValue, (int) byte.MaxValue, 128))
        {
          int num5 = num3 * x + y / 8;
          byte num6 = (byte) (128 >> y % 8);
          numArray[num5 + num1] |= num6;
        }
      }
    }
    return numArray;
  }

  [DllImport("gdi32.dll")]
  private static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);

  public static void ConvertHBitmap(IntPtr hdc, Bitmap bitmap, bool bIncludeSize)
  {
    for (int index1 = 0; index1 < bitmap.Width; ++index1)
    {
      for (int index2 = 0; index2 < bitmap.Height; ++index2)
      {
        if (bitmap.GetPixel(index1, index2).R == (byte) 0)
        {
          int num1 = (int) ImageRasterHelper.SetPixel(hdc, index1, index2, 0U);
        }
        else
        {
          int num2 = (int) ImageRasterHelper.SetPixel(hdc, index1, index2, 16777215U);
        }
      }
    }
  }
}
