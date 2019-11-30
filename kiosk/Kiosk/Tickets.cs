// Decompiled with JetBrains decompiler
// Type: Kiosk.Tickets
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using BarcodeLib;
using GLib;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Kiosk
{
  public class Tickets
  {
    private byte _ESCPOS_FU = 128;
    private byte _ESCPOS_FE = 8;
    private byte _ESCPOS_FDW = 32;
    private byte _ESCPOS_FDH = 16;
    private byte _ESCPOS_AL = 0;
    private byte _ESCPOS_AC = 1;
    private byte _ESCPOS_AR = 2;
    private string[] ascii7x5 = new string[140]
    {
      "  ######  ",
      "##      ##",
      "##    ####",
      "##  ##  ##",
      "####    ##",
      "##      ##",
      "  ######  ",
      "    ##    ",
      "  ####    ",
      "    ##    ",
      "    ##    ",
      "    ##    ",
      "    ##    ",
      "##########",
      "  ######  ",
      "##      ##",
      "        ##",
      "      ##  ",
      "    ##    ",
      "  ##      ",
      "##########",
      "  ######  ",
      "##      ##",
      "        ##",
      "      ### ",
      "        ##",
      "##      ##",
      "  ######  ",
      "##        ",
      "##        ",
      "##      ##",
      "##########",
      "        ##",
      "        ##",
      "        ##",
      "##########",
      "##        ",
      "##        ",
      "######### ",
      "        ##",
      "        ##",
      "######### ",
      "    ######",
      "  ##      ",
      "##        ",
      "######### ",
      "##      ##",
      "##      ##",
      " ######## ",
      "##########",
      "        ##",
      "        ##",
      "      ##  ",
      "      ##  ",
      "    ##    ",
      "    ##    ",
      " ######## ",
      "##      ##",
      "##      ##",
      " ######## ",
      "##      ##",
      "##      ##",
      " ######## ",
      " ######## ",
      "##      ##",
      "##      ##",
      " #########",
      "        ##",
      "        ##",
      "        ##",
      "  ######  ",
      "##      ##",
      "##      ##",
      "##########",
      "##      ##",
      "##      ##",
      "##      ##",
      "########  ",
      "##      ##",
      "##      ##",
      "########  ",
      "##      ##",
      "##      ##",
      "########  ",
      "  ########",
      "##        ",
      "##        ",
      "##        ",
      "##        ",
      "##        ",
      "  ########",
      "########  ",
      "##      ##",
      "##      ##",
      "##      ##",
      "##      ##",
      "##      ##",
      "########  ",
      "##########",
      "##        ",
      "##        ",
      "##########",
      "##        ",
      "##        ",
      "##########",
      "##########",
      "##        ",
      "##        ",
      "######    ",
      "##        ",
      "##        ",
      "##        ",
      "  ########",
      "##        ",
      "##        ",
      "##    ####",
      "##      ##",
      "##      ##",
      "  ########",
      "##      ##",
      "##      ##",
      "##      ##",
      "##########",
      "##      ##",
      "##      ##",
      "##      ##",
      " ######## ",
      "    ##    ",
      "    ##    ",
      "    ##    ",
      "    ##    ",
      "    ##    ",
      " ######## ",
      "        ##",
      "        ##",
      "        ##",
      "        ##",
      " ##     ##",
      " ##     ##",
      "   ###### "
    };

    public bool Print_ESCPOS(string printerName, byte[] document)
    {
      NativeMethods.DOC_INFO_1 di = new NativeMethods.DOC_INFO_1();
      di.pDataType = "RAW";
      di.pDocName = "Bit Image Test";
      IntPtr hPrinter = new IntPtr(0);
      if (!NativeMethods.OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
        throw new Win32Exception();
      if (!NativeMethods.StartDocPrinter(hPrinter, 1, di))
        throw new Win32Exception();
      byte[] source = document;
      IntPtr num = Marshal.AllocCoTaskMem(source.Length);
      Marshal.Copy(source, 0, num, source.Length);
      if (!NativeMethods.StartPagePrinter(hPrinter))
        throw new Win32Exception();
      int dwWritten;
      NativeMethods.WritePrinter(hPrinter, num, source.Length, out dwWritten);
      NativeMethods.EndPagePrinter(hPrinter);
      Marshal.FreeCoTaskMem(num);
      NativeMethods.EndDocPrinter(hPrinter);
      NativeMethods.ClosePrinter(hPrinter);
      return true;
    }

    public bool Ticket_Out_Mes_Temps_ESCPOS(
      string ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm,
      int _header,
      DateTime _timestamp,
      string _control,
      string _user,
      Tickets.Info_Ticket _text,
      int _hide,
      int _join,
      int _tick_temps)
    {
      string StringToEncode1 = Gestion.Build_Mod10(_user, (int) _valor, _id, 1);
      int num1 = 0;
      if (ptr_device.ToLower().Contains("star"))
        num1 = 1;
      Barcode barcode1 = new Barcode();
      Encoding encoding = Encoding.GetEncoding("IBM437");
      barcode1.IncludeLabel = false;
      barcode1.LabelFont = new Font("Arial", 20f);
      barcode1.Alignment = AlignmentPositions.CENTER;
      barcode1.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode1.LabelPosition = LabelPositions.TOPCENTER;
      Bitmap bmpFileName1 = new Bitmap(barcode1.Encode(TYPE.CODE128, StringToEncode1, Color.Black, Color.White, 350, 90));
      Bitmap bmpFileName2 = new Bitmap(350, 300);
      using (Graphics graphics = Graphics.FromImage((Image) bmpFileName2))
      {
        using (Font font1 = new Font("Calibri", 120f, FontStyle.Bold))
        {
          Font font2 = new Font("Calibri", 34f);
          Font font3 = new Font("Calibri", 30f);
          SizeF sizeF1 = graphics.MeasureString(_control, font1);
          graphics.FillRectangle(Brushes.White, 0, 0, 350, 300);
          graphics.FillRectangle((Brush) new HatchBrush(HatchStyle.DashedUpwardDiagonal, Color.Black, Color.White), 2, 2, 346, 296);
          graphics.DrawString(_control, font1, Brushes.Black, (float) (175.0 - (double) sizeF1.Width / 2.0), 0.0f);
          int num2 = (int) _valor;
          string str1 = _hide != 1 ? (!(_text.TXT_Points.ToLower() == "euros") ? string.Format("{0}# {1}", (object) num2, (object) _text.TXT_Points).PadLeft(14, '#') : string.Format("{0}.{1:00}# €", (object) (num2 / 100), (object) (num2 % 100)).PadLeft(12, '#')) : "".PadLeft(12, '*');
          SizeF sizeF2 = graphics.MeasureString(str1, font2);
          graphics.DrawString(str1, font2, Brushes.Black, (float) (175.0 - (double) sizeF2.Width / 2.0), (float) (300.0 - (double) sizeF2.Height * 2.0));
          string str2 = string.Format("{3:00}:{4:00} {0:00}/{1:00}/{2:0000}", (object) _timestamp.Day, (object) _timestamp.Month, (object) _timestamp.Year, (object) _timestamp.Hour, (object) _timestamp.Minute);
          sizeF2 = graphics.MeasureString(str2, font3);
          graphics.DrawString(str2, font3, Brushes.Black, (float) (175.0 - (double) sizeF2.Width / 2.0), (float) (300.0 - (double) sizeF2.Height - 5.0));
        }
      }
      Tickets.BitmapData bitmapData1 = Tickets.GetBitmapData(bmpFileName1);
      BitArray dots1 = bitmapData1.Dots;
      byte[] bytes1 = BitConverter.GetBytes(bitmapData1.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          if (num1 == 0)
          {
            if (ptr_device.ToLower().Contains("custom"))
            {
              binaryWriter.Write((byte) 27);
              binaryWriter.Write('t');
              binaryWriter.Write((byte) 19);
            }
            else
            {
              binaryWriter.Write((byte) 27);
              binaryWriter.Write('t');
              binaryWriter.Write((byte) 2);
            }
          }
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("{0}: {1}", (object) _text.TXT_BorneID, (object) _user).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 1);
          Tickets.BitmapData bitmapData2 = Tickets.GetBitmapData(bmpFileName2);
          BitArray dots2 = bitmapData2.Dots;
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num2 = 0;
          while (num2 < bitmapData2.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes1[0]));
            binaryWriter.Write(Convert.ToChar(bytes1[1]));
            for (int index1 = 0; index1 < bitmapData2.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num2 / 8 + index2) * 8 + index3) * bitmapData2.Width + index1;
                  bool flag = false;
                  if (index4 < dots2.Length)
                    flag = dots2[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num2 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToByte(10));
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((uint) this._ESCPOS_FDW + (uint) this._ESCPOS_FDH));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num4 = 0;
          while (num4 < bitmapData1.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes1[0]));
            binaryWriter.Write(Convert.ToChar(bytes1[1]));
            for (int index1 = 0; index1 < bitmapData1.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num4 / 8 + index2) * 8 + index3) * bitmapData1.Width + index1;
                  bool flag = false;
                  if (index4 < dots1.Length)
                    flag = dots1[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num4 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(string.Format("{0}", (object) StringToEncode1).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FDW + _60mm));
          string str1 = string.Concat((object) (int) _valor);
          string str2 = string.Format("{0:X}/{1:X2}{2:X}{3:X2}/{4:X2}{5:X2}/{6}", (object) (int) _valor, (object) _timestamp.Day, (object) _timestamp.Month, (object) (_timestamp.Year - 2000), (object) _timestamp.Hour, (object) _timestamp.Minute, (object) str1.Length);
          binaryWriter.Write(str2.ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FU + (int) this._ESCPOS_FDW + _60mm));
          if (_text.TXT_Location != null)
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin1)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin2)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin3)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin4)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format("RC: {0}", (object) _text.TXT_Lin5)));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write("---------------------------------".ToCharArray());
          Barcode barcode2 = new Barcode();
          string StringToEncode2 = Gestion.Build_Mod10(_user, _tick_temps, 0, 0);
          barcode2.IncludeLabel = false;
          barcode2.LabelFont = new Font("Arial", 20f);
          barcode2.Alignment = AlignmentPositions.CENTER;
          barcode2.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
          barcode2.LabelPosition = LabelPositions.TOPCENTER;
          Tickets.BitmapData bitmapData3 = Tickets.GetBitmapData(new Bitmap(barcode2.Encode(TYPE.CODE128, StringToEncode2, Color.Black, Color.White, 350, 50)));
          BitArray dots3 = bitmapData3.Dots;
          byte[] bytes2 = BitConverter.GetBytes(bitmapData3.Width);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Decimal) _tick_temps);
          string str3 = string.Format("{0}: {1}:{2:00}:{3:00}", (object) _text.TXT_Time, (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
          binaryWriter.Write(str3.ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          int num5 = 0;
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToByte(10));
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((uint) this._ESCPOS_FDW + (uint) this._ESCPOS_FDH));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          while (num5 < bitmapData3.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes2[0]));
            binaryWriter.Write(Convert.ToChar(bytes2[1]));
            for (int index1 = 0; index1 < bitmapData3.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num5 / 8 + index2) * 8 + index3) * bitmapData3.Width + index1;
                  bool flag = false;
                  if (index4 < dots3.Length)
                    flag = dots3[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num5 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDH + (int) this._ESCPOS_FDW + _60mm));
          binaryWriter.Write(string.Format("{1}", (object) _text.TXT_Ticket, (object) StringToEncode2).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          if (!string.IsNullOrEmpty(_text.TXT_Bottom))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid2))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Null))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Flush();
          if (ptr_device.ToLower().Contains("nii"))
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar(114));
            binaryWriter.Write(Convert.ToChar(49));
            binaryWriter.Write(Convert.ToChar(96));
          }
          if (_join == 0)
          {
            if (_cut == 1)
            {
              if (ptr_device.ToLower().Contains("2460"))
              {
                binaryWriter.Write(Convert.ToChar(27));
                binaryWriter.Write('d');
                binaryWriter.Write(15);
              }
              else
              {
                binaryWriter.Write(Convert.ToChar(27));
                binaryWriter.Write('d');
                binaryWriter.Write(0);
              }
              binaryWriter.Flush();
              binaryWriter.Write(Convert.ToChar(29));
              binaryWriter.Write(Convert.ToChar('V'));
              binaryWriter.Write(Convert.ToChar(0));
              binaryWriter.Write(Convert.ToChar(27));
              binaryWriter.Write('i');
            }
            else
            {
              binaryWriter.Write(Convert.ToChar(27));
              binaryWriter.Write('d');
              binaryWriter.Write(5);
            }
          }
          else
          {
            binaryWriter.Write("8<---------------------------------".ToCharArray());
            binaryWriter.Write((byte) 10);
            binaryWriter.Write((byte) 10);
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(ptr_device, memoryStream.ToArray());
        }
      }
    }

    public bool Ticket_Out_Mes_Temps_GAS_ESCPOS(
      string ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm,
      int _header,
      DateTime _timestamp,
      string _control,
      string _user,
      Tickets.Info_Ticket _text,
      int _hide,
      int _join,
      int _tick_temps)
    {
      string StringToEncode1 = Gestion.Build_Mod10(_user, (int) _valor, _id, 1);
      int num1 = 0;
      if (ptr_device.ToLower().Contains("star"))
        num1 = 1;
      Barcode barcode1 = new Barcode();
      Encoding encoding = Encoding.GetEncoding("IBM437");
      barcode1.IncludeLabel = false;
      barcode1.LabelFont = new Font("Arial", 20f);
      barcode1.Alignment = AlignmentPositions.CENTER;
      barcode1.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode1.LabelPosition = LabelPositions.TOPCENTER;
      Bitmap bmpFileName1 = new Bitmap(barcode1.Encode(TYPE.CODE128, StringToEncode1, Color.Black, Color.White, 350, 90));
      Bitmap bmpFileName2 = new Bitmap(350, 300);
      using (Graphics graphics = Graphics.FromImage((Image) bmpFileName2))
      {
        using (Font font1 = new Font("Calibri", 120f, FontStyle.Bold))
        {
          Font font2 = new Font("Calibri", 34f);
          Font font3 = new Font("Calibri", 30f);
          SizeF sizeF1 = graphics.MeasureString(_control, font1);
          graphics.FillRectangle(Brushes.White, 0, 0, 350, 300);
          graphics.FillRectangle((Brush) new HatchBrush(HatchStyle.DashedUpwardDiagonal, Color.Black, Color.White), 2, 2, 346, 296);
          graphics.DrawString(_control, font1, Brushes.Black, (float) (175.0 - (double) sizeF1.Width / 2.0), 0.0f);
          int num2 = (int) _valor;
          string str1 = _hide != 1 ? (!(_text.TXT_Points.ToLower() == "euros") ? string.Format("{0}# {1}", (object) num2, (object) _text.TXT_Points).PadLeft(14, '#') : string.Format("{0}.{1:00}# €", (object) (num2 / 100), (object) (num2 % 100)).PadLeft(12, '#')) : "".PadLeft(12, '*');
          SizeF sizeF2 = graphics.MeasureString(str1, font2);
          graphics.DrawString(str1, font2, Brushes.Black, (float) (175.0 - (double) sizeF2.Width / 2.0), (float) (300.0 - (double) sizeF2.Height * 2.0));
          string str2 = string.Format("{3:00}:{4:00} {0:00}/{1:00}/{2:0000}", (object) _timestamp.Day, (object) _timestamp.Month, (object) _timestamp.Year, (object) _timestamp.Hour, (object) _timestamp.Minute);
          sizeF2 = graphics.MeasureString(str2, font3);
          graphics.DrawString(str2, font3, Brushes.Black, (float) (175.0 - (double) sizeF2.Width / 2.0), (float) (300.0 - (double) sizeF2.Height - 5.0));
        }
      }
      Tickets.BitmapData bitmapData1 = Tickets.GetBitmapData(bmpFileName1);
      BitArray dots1 = bitmapData1.Dots;
      byte[] bytes1 = BitConverter.GetBytes(bitmapData1.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          if (num1 == 0)
          {
            if (ptr_device.ToLower().Contains("custom"))
            {
              binaryWriter.Write((byte) 27);
              binaryWriter.Write('t');
              binaryWriter.Write((byte) 19);
            }
            else
            {
              binaryWriter.Write((byte) 27);
              binaryWriter.Write('t');
              binaryWriter.Write((byte) 2);
            }
          }
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("{0}: {1}", (object) _text.TXT_BorneID, (object) _user).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 1);
          Tickets.BitmapData bitmapData2 = Tickets.GetBitmapData(bmpFileName2);
          BitArray dots2 = bitmapData2.Dots;
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num2 = 0;
          while (num2 < bitmapData2.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes1[0]));
            binaryWriter.Write(Convert.ToChar(bytes1[1]));
            for (int index1 = 0; index1 < bitmapData2.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num2 / 8 + index2) * 8 + index3) * bitmapData2.Width + index1;
                  bool flag = false;
                  if (index4 < dots2.Length)
                    flag = dots2[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num2 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToByte(10));
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((uint) this._ESCPOS_FDW + (uint) this._ESCPOS_FDH));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num4 = 0;
          while (num4 < bitmapData1.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes1[0]));
            binaryWriter.Write(Convert.ToChar(bytes1[1]));
            for (int index1 = 0; index1 < bitmapData1.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num4 / 8 + index2) * 8 + index3) * bitmapData1.Width + index1;
                  bool flag = false;
                  if (index4 < dots1.Length)
                    flag = dots1[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num4 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(string.Format("{0}", (object) StringToEncode1).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FDW + _60mm));
          string str1 = string.Concat((object) (int) _valor);
          string str2 = string.Format("{0:X}/{1:X2}{2:X}{3:X2}/{4:X2}{5:X2}/{6}", (object) (int) _valor, (object) _timestamp.Day, (object) _timestamp.Month, (object) (_timestamp.Year - 2000), (object) _timestamp.Hour, (object) _timestamp.Minute, (object) str1.Length);
          binaryWriter.Write(str2.ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          if (_text.TXT_GAS0 != null)
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS0));
            binaryWriter.Write((byte) 10);
          }
          if (_text.TXT_GAS1 != null)
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS1));
            binaryWriter.Write((byte) 10);
          }
          if (_text.TXT_GAS2 != null)
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS2));
            binaryWriter.Write((byte) 10);
          }
          if (_text.TXT_GAS3 != null)
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS3));
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS6));
          binaryWriter.Write((byte) 10);
          _timestamp = _timestamp.AddDays(2.0);
          string s = string.Format("{0:00}/{1:00}/{2:0000} {5} {3:00}:{4:00} {6}", (object) _timestamp.Day, (object) _timestamp.Month, (object) _timestamp.Year, (object) _timestamp.Hour, (object) _timestamp.Minute, (object) _text.TXT_GAS4, (object) _text.TXT_GAS5);
          binaryWriter.Write(encoding.GetBytes(s));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FDW + _60mm));
          binaryWriter.Write((byte) 10);
          if (_text.TXT_Location != null)
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin1)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin2)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin3)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin4)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format("RC: {0}", (object) _text.TXT_Lin5)));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write("---------------------------------".ToCharArray());
          Barcode barcode2 = new Barcode();
          string StringToEncode2 = Gestion.Build_Mod10(_user, _tick_temps, 0, 0);
          barcode2.IncludeLabel = false;
          barcode2.LabelFont = new Font("Arial", 20f);
          barcode2.Alignment = AlignmentPositions.CENTER;
          barcode2.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
          barcode2.LabelPosition = LabelPositions.TOPCENTER;
          Tickets.BitmapData bitmapData3 = Tickets.GetBitmapData(new Bitmap(barcode2.Encode(TYPE.CODE128, StringToEncode2, Color.Black, Color.White, 350, 50)));
          BitArray dots3 = bitmapData3.Dots;
          byte[] bytes2 = BitConverter.GetBytes(bitmapData3.Width);
          binaryWriter.Write((byte) 10);
          TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Decimal) _tick_temps);
          string str3 = string.Format("{0}: {1}:{2:00}:{3:00}", (object) _text.TXT_Time, (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
          binaryWriter.Write(str3.ToCharArray());
          binaryWriter.Write((byte) 10);
          int num5 = 0;
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToByte(10));
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((uint) this._ESCPOS_FDW + (uint) this._ESCPOS_FDH));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          while (num5 < bitmapData3.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes2[0]));
            binaryWriter.Write(Convert.ToChar(bytes2[1]));
            for (int index1 = 0; index1 < bitmapData3.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num5 / 8 + index2) * 8 + index3) * bitmapData3.Width + index1;
                  bool flag = false;
                  if (index4 < dots3.Length)
                    flag = dots3[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num5 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDH + (int) this._ESCPOS_FDW + _60mm));
          binaryWriter.Write(string.Format("{1}", (object) _text.TXT_Ticket, (object) StringToEncode2).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Flush();
          if (ptr_device.ToLower().Contains("nii"))
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar(114));
            binaryWriter.Write(Convert.ToChar(49));
            binaryWriter.Write(Convert.ToChar(96));
          }
          if (_join == 0)
          {
            if (_cut == 1)
            {
              if (ptr_device.ToLower().Contains("2460"))
              {
                binaryWriter.Write(Convert.ToChar(27));
                binaryWriter.Write('d');
                binaryWriter.Write(15);
              }
              else
              {
                binaryWriter.Write(Convert.ToChar(27));
                binaryWriter.Write('d');
                binaryWriter.Write(0);
              }
              binaryWriter.Flush();
              binaryWriter.Write(Convert.ToChar(29));
              binaryWriter.Write(Convert.ToChar('V'));
              binaryWriter.Write(Convert.ToChar(0));
              binaryWriter.Write(Convert.ToChar(27));
              binaryWriter.Write('i');
            }
            else
            {
              binaryWriter.Write(Convert.ToChar(27));
              binaryWriter.Write('d');
              binaryWriter.Write(5);
            }
          }
          else
          {
            binaryWriter.Write("8<---------------------------------".ToCharArray());
            binaryWriter.Write((byte) 10);
            binaryWriter.Write((byte) 10);
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(ptr_device, memoryStream.ToArray());
        }
      }
    }

    public bool Ticket_Out_ESCPOS(
      string ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm,
      int _header,
      DateTime _timestamp,
      string _control,
      string _user,
      Tickets.Info_Ticket _text,
      int _hide,
      int _join)
    {
      string StringToEncode = Gestion.Build_Mod10(_user, (int) _valor, _id, 1);
      int num1 = 0;
      if (ptr_device.ToLower().Contains("star"))
        num1 = 1;
      Barcode barcode = new Barcode();
      Encoding encoding = Encoding.GetEncoding("IBM437");
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      Bitmap bmpFileName1 = new Bitmap(barcode.Encode(TYPE.CODE128, StringToEncode, Color.Black, Color.White, 350, 90));
      Bitmap bmpFileName2 = new Bitmap(350, 300);
      using (Graphics graphics = Graphics.FromImage((Image) bmpFileName2))
      {
        using (Font font1 = new Font("Calibri", 120f, FontStyle.Bold))
        {
          Font font2 = new Font("Calibri", 34f);
          Font font3 = new Font("Calibri", 30f);
          SizeF sizeF1 = graphics.MeasureString(_control, font1);
          graphics.FillRectangle(Brushes.White, 0, 0, 350, 300);
          graphics.FillRectangle((Brush) new HatchBrush(HatchStyle.DashedUpwardDiagonal, Color.Black, Color.White), 2, 2, 346, 296);
          graphics.DrawString(_control, font1, Brushes.Black, (float) (175.0 - (double) sizeF1.Width / 2.0), 0.0f);
          int num2 = (int) _valor;
          string str1 = _hide != 1 ? (!(_text.TXT_Points.ToLower() == "euros") ? string.Format("{0}# {1}", (object) num2, (object) _text.TXT_Points).PadLeft(14, '#') : string.Format("{0}.{1:00}# €", (object) (num2 / 100), (object) (num2 % 100)).PadLeft(12, '#')) : "".PadLeft(12, '*');
          SizeF sizeF2 = graphics.MeasureString(str1, font2);
          graphics.DrawString(str1, font2, Brushes.Black, (float) (175.0 - (double) sizeF2.Width / 2.0), (float) (300.0 - (double) sizeF2.Height * 2.0));
          string str2 = string.Format("{3:00}:{4:00} {0:00}/{1:00}/{2:0000}", (object) _timestamp.Day, (object) _timestamp.Month, (object) _timestamp.Year, (object) _timestamp.Hour, (object) _timestamp.Minute);
          sizeF2 = graphics.MeasureString(str2, font3);
          graphics.DrawString(str2, font3, Brushes.Black, (float) (175.0 - (double) sizeF2.Width / 2.0), (float) (300.0 - (double) sizeF2.Height - 5.0));
        }
      }
      Tickets.BitmapData bitmapData1 = Tickets.GetBitmapData(bmpFileName1);
      BitArray dots1 = bitmapData1.Dots;
      byte[] bytes = BitConverter.GetBytes(bitmapData1.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          if (num1 == 0)
          {
            if (ptr_device.ToLower().Contains("custom"))
            {
              binaryWriter.Write((byte) 27);
              binaryWriter.Write('t');
              binaryWriter.Write((byte) 19);
            }
            else
            {
              binaryWriter.Write((byte) 27);
              binaryWriter.Write('t');
              binaryWriter.Write((byte) 2);
            }
          }
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("{0}: {1}", (object) _text.TXT_BorneID, (object) _user).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 1);
          Tickets.BitmapData bitmapData2 = Tickets.GetBitmapData(bmpFileName2);
          BitArray dots2 = bitmapData2.Dots;
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num2 = 0;
          while (num2 < bitmapData2.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes[0]));
            binaryWriter.Write(Convert.ToChar(bytes[1]));
            for (int index1 = 0; index1 < bitmapData2.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num2 / 8 + index2) * 8 + index3) * bitmapData2.Width + index1;
                  bool flag = false;
                  if (index4 < dots2.Length)
                    flag = dots2[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num2 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToByte(10));
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((uint) this._ESCPOS_FDW + (uint) this._ESCPOS_FDH));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num4 = 0;
          while (num4 < bitmapData1.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes[0]));
            binaryWriter.Write(Convert.ToChar(bytes[1]));
            for (int index1 = 0; index1 < bitmapData1.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num4 / 8 + index2) * 8 + index3) * bitmapData1.Width + index1;
                  bool flag = false;
                  if (index4 < dots1.Length)
                    flag = dots1[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num4 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(string.Format("{0}", (object) StringToEncode).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FDW + _60mm));
          string str1 = string.Concat((object) (int) _valor);
          string str2 = string.Format("{0:X}/{1:X2}{2:X}{3:X2}/{4:X2}{5:X2}/{6}", (object) (int) _valor, (object) _timestamp.Day, (object) _timestamp.Month, (object) (_timestamp.Year - 2000), (object) _timestamp.Hour, (object) _timestamp.Minute, (object) str1.Length);
          binaryWriter.Write(str2.ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FU + (int) this._ESCPOS_FDW + _60mm));
          if (_text.TXT_Location != null)
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin1)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin2)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin3)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin4)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format("RC: {0}", (object) _text.TXT_Lin5)));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          if (!string.IsNullOrEmpty(_text.TXT_Bottom))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid2))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Null))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Flush();
          if (ptr_device.ToLower().Contains("nii"))
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar(114));
            binaryWriter.Write(Convert.ToChar(49));
            binaryWriter.Write(Convert.ToChar(96));
          }
          if (_join == 0)
          {
            if (_cut == 1)
            {
              if (ptr_device.ToLower().Contains("2460"))
              {
                binaryWriter.Write(Convert.ToChar(27));
                binaryWriter.Write('d');
                binaryWriter.Write(15);
              }
              else
              {
                binaryWriter.Write(Convert.ToChar(27));
                binaryWriter.Write('d');
                binaryWriter.Write(0);
              }
              binaryWriter.Flush();
              binaryWriter.Write(Convert.ToChar(29));
              binaryWriter.Write(Convert.ToChar('V'));
              binaryWriter.Write(Convert.ToChar(0));
              binaryWriter.Write(Convert.ToChar(27));
              binaryWriter.Write('i');
            }
            else
            {
              binaryWriter.Write(Convert.ToChar(27));
              binaryWriter.Write('d');
              binaryWriter.Write(5);
            }
          }
          else
          {
            binaryWriter.Write("8<---------------------------------".ToCharArray());
            binaryWriter.Write((byte) 10);
            binaryWriter.Write((byte) 10);
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(ptr_device, memoryStream.ToArray());
        }
      }
    }

    public bool Ticket_Out_ESCPOS_Check(
      string ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm,
      int _header,
      DateTime _timestamp,
      string _control,
      string _user,
      Tickets.Info_Ticket _text,
      int _hide,
      int _ntick,
      int _join)
    {
      string StringToEncode = Gestion.Build_Mod10(_user, (int) _valor, _id, 1);
      int num1 = 0;
      if (ptr_device.ToLower().Contains("star"))
        num1 = 1;
      Barcode barcode = new Barcode();
      Encoding encoding = Encoding.GetEncoding("IBM437");
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      Bitmap bmpFileName1 = new Bitmap(barcode.Encode(TYPE.CODE128, StringToEncode, Color.Black, Color.White, 350, 90));
      Bitmap bmpFileName2 = new Bitmap(350, 400);
      using (Graphics graphics = Graphics.FromImage((Image) bmpFileName2))
      {
        using (Font font1 = new Font("Calibri", 120f, FontStyle.Bold))
        {
          Font font2 = new Font("Calibri", 34f);
          Font font3 = new Font("Calibri", 30f);
          SizeF sizeF1 = graphics.MeasureString(_control, font1);
          graphics.FillRectangle(Brushes.White, 0, 0, 350, 400);
          graphics.FillRectangle((Brush) new HatchBrush(HatchStyle.DashedUpwardDiagonal, Color.Black, Color.White), 2, 2, 346, 396);
          graphics.DrawString(_control, font1, Brushes.Black, (float) (175.0 - (double) sizeF1.Width / 2.0), 0.0f);
          int num2 = (int) _valor;
          string str1 = string.Format("Cheques");
          SizeF sizeF2 = graphics.MeasureString(str1, font2);
          graphics.DrawString(str1, font2, Brushes.Black, (float) (175.0 - (double) sizeF2.Width / 2.0), 170f);
          string str2 = _ntick > 0 ? string.Format("#{0}#", (object) _ntick) : "---";
          SizeF sizeF3 = graphics.MeasureString(str2, font2);
          graphics.DrawString(str2, font2, Brushes.Black, (float) (175.0 - (double) sizeF3.Width / 2.0), 210f);
          SizeF sizeF4;
          if (num2 > 0)
          {
            string str3 = string.Format("Reste");
            sizeF4 = graphics.MeasureString(str3, font2);
            graphics.DrawString(str3, font2, Brushes.Black, (float) (175.0 - (double) sizeF4.Width / 2.0), 250f);
            string str4 = _hide != 1 ? (!(_text.TXT_Points.ToLower() == "euros") ? string.Format("{0}# {1}", (object) num2, (object) _text.TXT_Points).PadLeft(14, '#') : string.Format("{0}.{1:00}# €", (object) (num2 / 100), (object) (num2 % 100)).PadLeft(12, '#')) : "".PadLeft(12, '*');
            sizeF4 = graphics.MeasureString(str4, font2);
            graphics.DrawString(str4, font2, Brushes.Black, (float) (175.0 - (double) sizeF4.Width / 2.0), 290f);
          }
          string str5 = string.Format("{3:00}:{4:00} {0:00}/{1:00}/{2:0000}", (object) _timestamp.Day, (object) _timestamp.Month, (object) _timestamp.Year, (object) _timestamp.Hour, (object) _timestamp.Minute);
          sizeF4 = graphics.MeasureString(str5, font3);
          graphics.DrawString(str5, font3, Brushes.Black, (float) (175.0 - (double) sizeF4.Width / 2.0), (float) (400.0 - (double) sizeF4.Height - 5.0));
        }
      }
      Tickets.BitmapData bitmapData1 = Tickets.GetBitmapData(bmpFileName1);
      BitArray dots1 = bitmapData1.Dots;
      byte[] bytes = BitConverter.GetBytes(bitmapData1.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          if (num1 == 0)
          {
            if (ptr_device.ToLower().Contains("custom"))
            {
              binaryWriter.Write((byte) 27);
              binaryWriter.Write('t');
              binaryWriter.Write((byte) 19);
            }
            else
            {
              binaryWriter.Write((byte) 27);
              binaryWriter.Write('t');
              binaryWriter.Write((byte) 2);
            }
          }
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("{0}: {1}", (object) _text.TXT_BorneID, (object) _user).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 1);
          Tickets.BitmapData bitmapData2 = Tickets.GetBitmapData(bmpFileName2);
          BitArray dots2 = bitmapData2.Dots;
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num2 = 0;
          while (num2 < bitmapData2.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes[0]));
            binaryWriter.Write(Convert.ToChar(bytes[1]));
            for (int index1 = 0; index1 < bitmapData2.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num2 / 8 + index2) * 8 + index3) * bitmapData2.Width + index1;
                  bool flag = false;
                  if (index4 < dots2.Length)
                    flag = dots2[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num2 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToByte(10));
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((uint) this._ESCPOS_FDW + (uint) this._ESCPOS_FDH));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num4 = 0;
          while (num4 < bitmapData1.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes[0]));
            binaryWriter.Write(Convert.ToChar(bytes[1]));
            for (int index1 = 0; index1 < bitmapData1.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num4 / 8 + index2) * 8 + index3) * bitmapData1.Width + index1;
                  bool flag = false;
                  if (index4 < dots1.Length)
                    flag = dots1[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num4 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(string.Format("{0}", (object) StringToEncode).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FDW + _60mm));
          string str1 = string.Concat((object) (int) _valor);
          string str2 = string.Format("{0:X}/{1:X2}{2:X}{3:X2}/{4:X2}{5:X2}/{6}", (object) (int) _valor, (object) _timestamp.Day, (object) _timestamp.Month, (object) (_timestamp.Year - 2000), (object) _timestamp.Hour, (object) _timestamp.Minute, (object) str1.Length);
          binaryWriter.Write(str2.ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FU + (int) this._ESCPOS_FDW + _60mm));
          if (_text.TXT_Location != null)
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin1)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin2)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin3)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin4)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format("RC: {0}", (object) _text.TXT_Lin5)));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          if (!string.IsNullOrEmpty(_text.TXT_Bottom))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid2))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Null))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Flush();
          if (ptr_device.ToLower().Contains("nii"))
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar(114));
            binaryWriter.Write(Convert.ToChar(49));
            binaryWriter.Write(Convert.ToChar(96));
          }
          if (_join == 0)
          {
            if (_cut == 1)
            {
              if (ptr_device.ToLower().Contains("2460"))
              {
                binaryWriter.Write(Convert.ToChar(27));
                binaryWriter.Write('d');
                binaryWriter.Write(15);
              }
              else
              {
                binaryWriter.Write(Convert.ToChar(27));
                binaryWriter.Write('d');
                binaryWriter.Write(0);
              }
              binaryWriter.Flush();
              binaryWriter.Write(Convert.ToChar(29));
              binaryWriter.Write(Convert.ToChar('V'));
              binaryWriter.Write(Convert.ToChar(0));
              binaryWriter.Write(Convert.ToChar(27));
              binaryWriter.Write('i');
            }
            else
            {
              binaryWriter.Write(Convert.ToChar(27));
              binaryWriter.Write('d');
              binaryWriter.Write(5);
            }
          }
          else
          {
            binaryWriter.Write("8<---------------------------------".ToCharArray());
            binaryWriter.Write((byte) 10);
            binaryWriter.Write((byte) 10);
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(ptr_device, memoryStream.ToArray());
        }
      }
    }

    public bool Ticket_Image(
      string ptr_device,
      Decimal _valor,
      int _tick,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm,
      int _header,
      DateTime _timestamp,
      string _user,
      Tickets.Info_Ticket _text,
      string _bmp)
    {
      int num1 = 0;
      if (ptr_device.ToLower().Contains("star"))
        num1 = 1;
      Barcode barcode = new Barcode();
      Encoding encoding = Encoding.GetEncoding("IBM437");
      string str1 = Gestion.Build_Mod10(_user, _tick, _id, 0);
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      Tickets.BitmapData bitmapData = Tickets.GetBitmapData(new Bitmap((Image) new Bitmap(_bmp)));
      BitArray dots = bitmapData.Dots;
      byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          if (num1 == 0)
          {
            binaryWriter.Write((byte) 27);
            binaryWriter.Write('t');
            binaryWriter.Write((byte) 2);
          }
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("{0}: {1}", (object) _text.TXT_BorneID, (object) _user).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Decimal) _tick);
          string str2 = string.Format("{0}: {1}:{2:00}:{3:00}", (object) _text.TXT_Time, (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
          if (_valor <= new Decimal(0))
            str2 = "TEST TICKET";
          binaryWriter.Write(str2.ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(string.Format("{2:0}:{3:00} {0}/{1:00}", (object) _timestamp.Day, (object) _timestamp.Month, (object) _timestamp.Hour, (object) _timestamp.Minute).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FU + (int) this._ESCPOS_FDW + _60mm));
          if (_text.TXT_Location != null)
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin1)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin2)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin3)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin4)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format("RC: {0}", (object) _text.TXT_Lin5)));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num2 = 0;
          while (num2 < bitmapData.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes[0]));
            binaryWriter.Write(Convert.ToChar(bytes[1]));
            for (int index1 = 0; index1 < bitmapData.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num2 / 8 + index2) * 8 + index3) * bitmapData.Width + index1;
                  bool flag = false;
                  if (index4 < dots.Length)
                    flag = dots[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num2 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDH + (int) this._ESCPOS_FDW + _60mm));
          binaryWriter.Write(string.Format("{1}", (object) _text.TXT_Ticket, (object) str1).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(_text.TXT_Thanks.ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          if (!string.IsNullOrEmpty(_text.TXT_Bottom))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid2))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Null))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          if (ptr_device.ToLower().Contains("nii"))
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar(114));
            binaryWriter.Write(Convert.ToChar(49));
            binaryWriter.Write(Convert.ToChar(96));
          }
          if (_cut == 1)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('m');
          }
          else
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('J');
            binaryWriter.Write(Convert.ToChar(120));
            binaryWriter.Write(Convert.ToChar(29));
            binaryWriter.Write(Convert.ToChar('V'));
            binaryWriter.Write(Convert.ToChar(0));
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(ptr_device, memoryStream.ToArray());
        }
      }
    }

    public bool Ticket_ESCPOS(
      string ptr_device,
      Decimal _valor,
      int _tick,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm,
      int _header,
      DateTime _timestamp,
      string _user,
      Tickets.Info_Ticket _text)
    {
      int num1 = 0;
      if (ptr_device.ToLower().Contains("star"))
        num1 = 1;
      Barcode barcode = new Barcode();
      Encoding encoding = Encoding.GetEncoding("IBM437");
      string StringToEncode = Gestion.Build_Mod10(_user, _tick, _id, 0);
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      Tickets.BitmapData bitmapData = Tickets.GetBitmapData(new Bitmap(barcode.Encode(TYPE.CODE128, StringToEncode, Color.Black, Color.White, 350, 90)));
      BitArray dots = bitmapData.Dots;
      byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          if (num1 == 0)
          {
            binaryWriter.Write((byte) 27);
            binaryWriter.Write('t');
            binaryWriter.Write((byte) 2);
          }
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("{0}: {1}", (object) _text.TXT_BorneID, (object) _user).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          TimeSpan timeSpan = new TimeSpan(0, 0, (int) (Decimal) _tick);
          string str = string.Format("{0}: {1}:{2:00}:{3:00}", (object) _text.TXT_Time, (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
          if (_valor <= new Decimal(0))
            str = "TEST TICKET";
          binaryWriter.Write(str.ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(string.Format("{2:0}:{3:00} {0}/{1:00}", (object) _timestamp.Day, (object) _timestamp.Month, (object) _timestamp.Hour, (object) _timestamp.Minute).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FE + (int) this._ESCPOS_FU + (int) this._ESCPOS_FDW + _60mm));
          if (_text.TXT_Location != null)
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) 0);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin1)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin2)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin3)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format(" {0} ", (object) _text.TXT_Lin4)));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(encoding.GetBytes(string.Format("RC: {0}", (object) _text.TXT_Lin5)));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(24));
          int num2 = 0;
          while (num2 < bitmapData.Height)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar('*'));
            binaryWriter.Write(Convert.ToChar(33));
            binaryWriter.Write(Convert.ToChar(bytes[0]));
            binaryWriter.Write(Convert.ToChar(bytes[1]));
            for (int index1 = 0; index1 < bitmapData.Width; ++index1)
            {
              for (int index2 = 0; index2 < 3; ++index2)
              {
                byte num3 = 0;
                for (int index3 = 0; index3 < 8; ++index3)
                {
                  int index4 = ((num2 / 8 + index2) * 8 + index3) * bitmapData.Width + index1;
                  bool flag = false;
                  if (index4 < dots.Length)
                    flag = dots[index4];
                  num3 |= (byte) ((flag ? 1 : 0) << 7 - index3);
                }
                binaryWriter.Write(num3);
              }
            }
            num2 += 24;
            binaryWriter.Write(Convert.ToChar(10));
          }
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDH + (int) this._ESCPOS_FDW + _60mm));
          binaryWriter.Write(string.Format("{1}", (object) _text.TXT_Ticket, (object) StringToEncode).ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(_text.TXT_Thanks.ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          if (!string.IsNullOrEmpty(_text.TXT_Bottom))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Valid2))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
            binaryWriter.Write((byte) 10);
          }
          if (!string.IsNullOrEmpty(_text.TXT_Null))
          {
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
            binaryWriter.Write((byte) 10);
          }
          binaryWriter.Write("---------------------------------".ToCharArray());
          binaryWriter.Write(Convert.ToChar(10));
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          if (ptr_device.ToLower().Contains("nii"))
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar(114));
            binaryWriter.Write(Convert.ToChar(49));
            binaryWriter.Write(Convert.ToChar(96));
          }
          if (_cut == 1)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('m');
          }
          else
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('J');
            binaryWriter.Write(Convert.ToChar(120));
            binaryWriter.Write(Convert.ToChar(29));
            binaryWriter.Write(Convert.ToChar('V'));
            binaryWriter.Write(Convert.ToChar(0));
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(ptr_device, memoryStream.ToArray());
        }
      }
    }

    public static Tickets.BitmapData GetBitmapData(Bitmap bmpFileName)
    {
      using (Bitmap bitmap = bmpFileName)
      {
        int maxValue = (int) sbyte.MaxValue;
        int index = 0;
        BitArray bitArray = new BitArray(bitmap.Width * bitmap.Height);
        for (int y = 0; y < bitmap.Height; ++y)
        {
          for (int x = 0; x < bitmap.Width; ++x)
          {
            Color pixel = bitmap.GetPixel(x, y);
            int num = (int) ((double) pixel.R * 0.3 + (double) pixel.G * 0.59 + (double) pixel.B * 0.11);
            bitArray[index] = num < maxValue;
            ++index;
          }
        }
        return new Tickets.BitmapData()
        {
          Dots = bitArray,
          Height = bitmap.Height,
          Width = bitmap.Width
        };
      }
    }

    public byte XLAT_Dig(char _c)
    {
      switch (_c)
      {
        case '1':
          return 1;
        case '2':
          return 2;
        case '3':
          return 3;
        case '4':
          return 4;
        case '5':
          return 5;
        case '6':
          return 6;
        case '7':
          return 7;
        case '8':
          return 8;
        case '9':
          return 9;
        case 'A':
          return 10;
        case 'B':
          return 11;
        case 'C':
          return 12;
        case 'D':
          return 13;
        case 'E':
          return 14;
        case 'F':
          return 15;
        case 'G':
          return 16;
        case 'H':
          return 17;
        default:
          return 0;
      }
    }

    public bool Ticket_Out_Conf_ESCPOS(
      string ptr_device,
      Decimal _valor,
      int _id,
      int _model,
      int _cut,
      int _skeep,
      int _preskeep,
      int _60mm,
      int _header,
      DateTime _timestamp,
      string _control,
      string _user,
      Tickets.Info_Ticket _text,
      DateTime _timestampC)
    {
      string StringToEncode = Gestion.Build_Mod10(_user, (int) _valor, _id, 1);
      int num1 = 0;
      if (ptr_device.ToLower().Contains("star"))
        num1 = 1;
      Barcode barcode = new Barcode();
      Encoding encoding = Encoding.GetEncoding("IBM437");
      barcode.IncludeLabel = false;
      barcode.LabelFont = new Font("Arial", 20f);
      barcode.Alignment = AlignmentPositions.CENTER;
      barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
      barcode.LabelPosition = LabelPositions.TOPCENTER;
      Tickets.BitmapData bitmapData = Tickets.GetBitmapData(new Bitmap(barcode.Encode(TYPE.CODE128, StringToEncode, Color.Black, Color.White, 350, 90)));
      BitArray dots = bitmapData.Dots;
      BitConverter.GetBytes(bitmapData.Width);
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('@');
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('T');
          binaryWriter.Write((byte) 0);
          if (num1 == 0)
          {
            binaryWriter.Write((byte) 27);
            binaryWriter.Write('t');
            binaryWriter.Write((byte) 2);
          }
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          for (int index = 0; index < _preskeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          binaryWriter.Write(string.Format("{0}: {1}", (object) _text.TXT_BorneID, (object) _user).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((uint) this._ESCPOS_FDW + (uint) this._ESCPOS_FDH));
          binaryWriter.Write(string.Format("{0}", (object) _control).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(Convert.ToByte(10));
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((uint) this._ESCPOS_FDW + (uint) this._ESCPOS_FDH));
          int num2 = (int) _valor;
          if (num1 == 0)
          {
            binaryWriter.Write(string.Format("{0}.{1:00} ", (object) (num2 / 100), (object) (num2 % 100)).ToCharArray());
            binaryWriter.Write(213);
          }
          else
            binaryWriter.Write(string.Format("{0}.{1:00} Euros", (object) (num2 / 100), (object) (num2 % 100)).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(string.Format("{2:00}:{3:00} {0}/{1:00}", (object) _timestamp.Day, (object) _timestamp.Month, (object) _timestamp.Hour, (object) _timestamp.Minute).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write('\n');
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('3'));
          binaryWriter.Write(Convert.ToChar(30));
          binaryWriter.Write(string.Format("{0}", (object) StringToEncode).ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('a');
          binaryWriter.Write(this._ESCPOS_AC);
          binaryWriter.Write((byte) 27);
          binaryWriter.Write('!');
          binaryWriter.Write((byte) ((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          if (_text.TXT_Cancel != null)
            binaryWriter.Write(encoding.GetBytes(_text.TXT_Cancel));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(string.Format("{2:00}:{3:00} {0}/{1:00}", (object) _timestampC.Day, (object) _timestampC.Month, (object) _timestampC.Hour, (object) _timestampC.Minute).ToCharArray());
          binaryWriter.Write('\n');
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('a'));
          binaryWriter.Write(Convert.ToChar(this._ESCPOS_AC));
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar((int) this._ESCPOS_FDW + (int) this._ESCPOS_FDH + _60mm));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write(Convert.ToChar(27));
          binaryWriter.Write(Convert.ToChar('!'));
          binaryWriter.Write(Convert.ToChar(_60mm));
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write("-1------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write((byte) 10);
          binaryWriter.Write("-2------------------------------".ToCharArray());
          binaryWriter.Write((byte) 10);
          for (int index = 0; index < _skeep; ++index)
            binaryWriter.Write(Convert.ToChar(10));
          if (ptr_device.ToLower().Contains("nii"))
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write(Convert.ToChar(114));
            binaryWriter.Write(Convert.ToChar(49));
            binaryWriter.Write(Convert.ToChar(96));
          }
          if (_cut == 1)
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('m');
          }
          else
          {
            binaryWriter.Write(Convert.ToChar(27));
            binaryWriter.Write('J');
            binaryWriter.Write(Convert.ToChar(120));
            binaryWriter.Write(Convert.ToChar(29));
            binaryWriter.Write(Convert.ToChar('V'));
            binaryWriter.Write(Convert.ToChar(0));
          }
          binaryWriter.Flush();
          return this.Print_ESCPOS(ptr_device, memoryStream.ToArray());
        }
      }
    }

    public class Info_Ticket
    {
      public string TXT_Cancel;
      public string TXT_BorneID;
      public string TXT_Location;
      public string TXT_Thanks;
      public string TXT_Valid;
      public string TXT_Valid2;
      public string TXT_Null;
      public string TXT_Bottom;
      public string TXT_Lin1;
      public string TXT_Lin2;
      public string TXT_Lin3;
      public string TXT_Lin4;
      public string TXT_Lin5;
      public string TXT_Ticket;
      public string TXT_Time;
      public string TXT_Points;
      public string TXT_GAS0;
      public string TXT_GAS1;
      public string TXT_GAS2;
      public string TXT_GAS3;
      public string TXT_GAS4;
      public string TXT_GAS5;
      public string TXT_GAS6;
    }

    public class BitmapData
    {
      public BitArray Dots { get; set; }

      public int Height { get; set; }

      public int Width { get; set; }
    }
  }
}
