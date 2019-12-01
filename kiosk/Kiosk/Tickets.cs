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
			public BitArray Dots
			{
				get;
				set;
			}

			public int Height
			{
				get;
				set;
			}

			public int Width
			{
				get;
				set;
			}
		}

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
			bool flag = true;
			NativeMethods.DOC_INFO_1 dOC_INFO_ = new NativeMethods.DOC_INFO_1();
			dOC_INFO_.pDataType = "RAW";
			dOC_INFO_.pDocName = "Bit Image Test";
			IntPtr hPrinter = new IntPtr(0);
			if (NativeMethods.OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
			{
				if (NativeMethods.StartDocPrinter(hPrinter, 1, dOC_INFO_))
				{
					IntPtr intPtr = Marshal.AllocCoTaskMem(document.Length);
					Marshal.Copy(document, 0, intPtr, document.Length);
					if (NativeMethods.StartPagePrinter(hPrinter))
					{
						NativeMethods.WritePrinter(hPrinter, intPtr, document.Length, out int _);
						NativeMethods.EndPagePrinter(hPrinter);
						Marshal.FreeCoTaskMem(intPtr);
						NativeMethods.EndDocPrinter(hPrinter);
						NativeMethods.ClosePrinter(hPrinter);
						return true;
					}
					throw new Win32Exception();
				}
				throw new Win32Exception();
			}
			throw new Win32Exception();
		}

		public bool Ticket_Out_Mes_Temps_ESCPOS(string ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm, int _header, DateTime _timestamp, string _control, string _user, Info_Ticket _text, int _hide, int _join, int _tick_temps)
		{
			string text = Gestion.Build_Mod10(_user, (int)_valor, _id, 1);
			int num = 0;
			if (ptr_device.ToLower().Contains("star"))
			{
				num = 1;
			}
			Barcode barcode = new Barcode();
			Encoding encoding = Encoding.GetEncoding("IBM437");
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 90);
			Bitmap bmpFileName = new Bitmap(original);
			Bitmap bitmap = new Bitmap(350, 300);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				using (Font font3 = new Font("Calibri", 120f, FontStyle.Bold))
				{
					Font font = new Font("Calibri", 34f);
					Font font2 = new Font("Calibri", 30f);
					SizeF sizeF = graphics.MeasureString(_control, font3);
					graphics.FillRectangle(Brushes.White, 0, 0, 350, 300);
					graphics.FillRectangle(new HatchBrush(HatchStyle.DashedUpwardDiagonal, Color.Black, Color.White), 2, 2, 346, 296);
					graphics.DrawString(_control, font3, Brushes.Black, 175f - sizeF.Width / 2f, 0f);
					int num2 = (int)_valor;
					string text2;
					if (_hide == 1)
					{
						text2 = "";
						text2 = text2.PadLeft(12, '*');
					}
					else if (_text.TXT_Points.ToLower() == "euros")
					{
						text2 = $"{num2 / 100}.{num2 % 100:00}# €";
						text2 = text2.PadLeft(12, '#');
					}
					else
					{
						text2 = $"{num2}# {_text.TXT_Points}";
						text2 = text2.PadLeft(14, '#');
					}
					sizeF = graphics.MeasureString(text2, font);
					graphics.DrawString(text2, font, Brushes.Black, 175f - sizeF.Width / 2f, 300f - sizeF.Height * 2f);
					text2 = string.Format("{3:00}:{4:00} {0:00}/{1:00}/{2:0000}", _timestamp.Day, _timestamp.Month, _timestamp.Year, _timestamp.Hour, _timestamp.Minute);
					sizeF = graphics.MeasureString(text2, font2);
					graphics.DrawString(text2, font2, Brushes.Black, 175f - sizeF.Width / 2f, 300f - sizeF.Height - 5f);
				}
			}
			BitmapData bitmapData = GetBitmapData(bmpFileName);
			BitArray dots = bitmapData.Dots;
			byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((byte)27);
					binaryWriter.Write('@');
					binaryWriter.Write((byte)27);
					binaryWriter.Write('T');
					binaryWriter.Write((byte)0);
					if (num == 0)
					{
						if (ptr_device.ToLower().Contains("custom"))
						{
							binaryWriter.Write((byte)27);
							binaryWriter.Write('t');
							binaryWriter.Write((byte)19);
						}
						else
						{
							binaryWriter.Write((byte)27);
							binaryWriter.Write('t');
							binaryWriter.Write((byte)2);
						}
					}
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write($"{_text.TXT_BorneID}: {_user}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)1);
					BitmapData bitmapData2 = GetBitmapData(bitmap);
					BitArray dots2 = bitmapData2.Dots;
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num3 = 0;
					while (num3 < bitmapData2.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData2.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num3 / 8 + k) * 8 + l;
									int i = num4 * bitmapData2.Width + j;
									bool flag = false;
									if (i < dots2.Length)
									{
										flag = dots2[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
							}
						}
						num3 += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToByte(10));
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num5 = 0;
					while (num5 < bitmapData.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num5 / 8 + k) * 8 + l;
									int i = num4 * bitmapData.Width + j;
									bool flag = false;
									if (i < dots.Length)
									{
										flag = dots[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
							}
						}
						num5 += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write($"{text}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FDW + _60mm));
					string text3 = string.Concat((int)_valor);
					string text4 = $"{(int)_valor:X}/{_timestamp.Day:X2}{_timestamp.Month:X}{_timestamp.Year - 2000:X2}/{_timestamp.Hour:X2}{_timestamp.Minute:X2}/{text3.Length}";
					binaryWriter.Write(text4.ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FU + _ESCPOS_FDW + _60mm));
					if (_text.TXT_Location != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
					}
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)0);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin1} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin2} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin3} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin4} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($"RC: {_text.TXT_Lin5}"));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write("---------------------------------".ToCharArray());
					barcode = new Barcode();
					text = Gestion.Build_Mod10(_user, _tick_temps, 0, 0);
					barcode.IncludeLabel = false;
					barcode.LabelFont = new Font("Arial", 20f);
					barcode.Alignment = AlignmentPositions.CENTER;
					barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
					barcode.LabelPosition = LabelPositions.TOPCENTER;
					original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 50);
					bmpFileName = new Bitmap(original);
					bitmapData = GetBitmapData(bmpFileName);
					dots = bitmapData.Dots;
					bytes = BitConverter.GetBytes(bitmapData.Width);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					decimal value = _tick_temps;
					TimeSpan timeSpan = new TimeSpan(0, 0, (int)value);
					string text5 = $"{_text.TXT_Time}: {timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
					binaryWriter.Write(text5.ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					num5 = 0;
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToByte(10));
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					while (num5 < bitmapData.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num5 / 8 + k) * 8 + l;
									int i = num4 * bitmapData.Width + j;
									bool flag = false;
									if (i < dots.Length)
									{
										flag = dots[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
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
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDH + _ESCPOS_FDW + _60mm));
					binaryWriter.Write(string.Format("{1}", _text.TXT_Ticket, text).ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					if (!string.IsNullOrEmpty(_text.TXT_Bottom))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid2))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Null))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					for (int i = 0; i < _skeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
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
						binaryWriter.Write((byte)10);
						binaryWriter.Write((byte)10);
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Flush();
					return Print_ESCPOS(ptr_device, memoryStream.ToArray());
				}
			}
		}

		public bool Ticket_Out_Mes_Temps_GAS_ESCPOS(string ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm, int _header, DateTime _timestamp, string _control, string _user, Info_Ticket _text, int _hide, int _join, int _tick_temps)
		{
			string text = Gestion.Build_Mod10(_user, (int)_valor, _id, 1);
			int num = 0;
			if (ptr_device.ToLower().Contains("star"))
			{
				num = 1;
			}
			Barcode barcode = new Barcode();
			Encoding encoding = Encoding.GetEncoding("IBM437");
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 90);
			Bitmap bmpFileName = new Bitmap(original);
			Bitmap bitmap = new Bitmap(350, 300);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				using (Font font3 = new Font("Calibri", 120f, FontStyle.Bold))
				{
					Font font = new Font("Calibri", 34f);
					Font font2 = new Font("Calibri", 30f);
					SizeF sizeF = graphics.MeasureString(_control, font3);
					graphics.FillRectangle(Brushes.White, 0, 0, 350, 300);
					graphics.FillRectangle(new HatchBrush(HatchStyle.DashedUpwardDiagonal, Color.Black, Color.White), 2, 2, 346, 296);
					graphics.DrawString(_control, font3, Brushes.Black, 175f - sizeF.Width / 2f, 0f);
					int num2 = (int)_valor;
					string text2;
					if (_hide == 1)
					{
						text2 = "";
						text2 = text2.PadLeft(12, '*');
					}
					else if (_text.TXT_Points.ToLower() == "euros")
					{
						text2 = $"{num2 / 100}.{num2 % 100:00}# €";
						text2 = text2.PadLeft(12, '#');
					}
					else
					{
						text2 = $"{num2}# {_text.TXT_Points}";
						text2 = text2.PadLeft(14, '#');
					}
					sizeF = graphics.MeasureString(text2, font);
					graphics.DrawString(text2, font, Brushes.Black, 175f - sizeF.Width / 2f, 300f - sizeF.Height * 2f);
					text2 = string.Format("{3:00}:{4:00} {0:00}/{1:00}/{2:0000}", _timestamp.Day, _timestamp.Month, _timestamp.Year, _timestamp.Hour, _timestamp.Minute);
					sizeF = graphics.MeasureString(text2, font2);
					graphics.DrawString(text2, font2, Brushes.Black, 175f - sizeF.Width / 2f, 300f - sizeF.Height - 5f);
				}
			}
			BitmapData bitmapData = GetBitmapData(bmpFileName);
			BitArray dots = bitmapData.Dots;
			byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((byte)27);
					binaryWriter.Write('@');
					binaryWriter.Write((byte)27);
					binaryWriter.Write('T');
					binaryWriter.Write((byte)0);
					if (num == 0)
					{
						if (ptr_device.ToLower().Contains("custom"))
						{
							binaryWriter.Write((byte)27);
							binaryWriter.Write('t');
							binaryWriter.Write((byte)19);
						}
						else
						{
							binaryWriter.Write((byte)27);
							binaryWriter.Write('t');
							binaryWriter.Write((byte)2);
						}
					}
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write($"{_text.TXT_BorneID}: {_user}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)1);
					BitmapData bitmapData2 = GetBitmapData(bitmap);
					BitArray dots2 = bitmapData2.Dots;
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num3 = 0;
					while (num3 < bitmapData2.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData2.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num3 / 8 + k) * 8 + l;
									int i = num4 * bitmapData2.Width + j;
									bool flag = false;
									if (i < dots2.Length)
									{
										flag = dots2[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
							}
						}
						num3 += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToByte(10));
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num5 = 0;
					while (num5 < bitmapData.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num5 / 8 + k) * 8 + l;
									int i = num4 * bitmapData.Width + j;
									bool flag = false;
									if (i < dots.Length)
									{
										flag = dots[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
							}
						}
						num5 += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write($"{text}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FDW + _60mm));
					string text3 = string.Concat((int)_valor);
					string text4 = $"{(int)_valor:X}/{_timestamp.Day:X2}{_timestamp.Month:X}{_timestamp.Year - 2000:X2}/{_timestamp.Hour:X2}{_timestamp.Minute:X2}/{text3.Length}";
					binaryWriter.Write(text4.ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					if (_text.TXT_GAS0 != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS0));
						binaryWriter.Write((byte)10);
					}
					if (_text.TXT_GAS1 != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS1));
						binaryWriter.Write((byte)10);
					}
					if (_text.TXT_GAS2 != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS2));
						binaryWriter.Write((byte)10);
					}
					if (_text.TXT_GAS3 != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS3));
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write(encoding.GetBytes(_text.TXT_GAS6));
					binaryWriter.Write((byte)10);
					_timestamp = _timestamp.AddDays(2.0);
					string s = string.Format("{0:00}/{1:00}/{2:0000} {5} {3:00}:{4:00} {6}", _timestamp.Day, _timestamp.Month, _timestamp.Year, _timestamp.Hour, _timestamp.Minute, _text.TXT_GAS4, _text.TXT_GAS5);
					binaryWriter.Write(encoding.GetBytes(s));
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FDW + _60mm));
					binaryWriter.Write((byte)10);
					if (_text.TXT_Location != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
					}
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)0);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin1} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin2} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin3} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin4} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($"RC: {_text.TXT_Lin5}"));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write("---------------------------------".ToCharArray());
					barcode = new Barcode();
					text = Gestion.Build_Mod10(_user, _tick_temps, 0, 0);
					barcode.IncludeLabel = false;
					barcode.LabelFont = new Font("Arial", 20f);
					barcode.Alignment = AlignmentPositions.CENTER;
					barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
					barcode.LabelPosition = LabelPositions.TOPCENTER;
					original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 50);
					bmpFileName = new Bitmap(original);
					bitmapData = GetBitmapData(bmpFileName);
					dots = bitmapData.Dots;
					bytes = BitConverter.GetBytes(bitmapData.Width);
					binaryWriter.Write((byte)10);
					decimal value = _tick_temps;
					TimeSpan timeSpan = new TimeSpan(0, 0, (int)value);
					string text5 = $"{_text.TXT_Time}: {timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
					binaryWriter.Write(text5.ToCharArray());
					binaryWriter.Write((byte)10);
					num5 = 0;
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToByte(10));
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					while (num5 < bitmapData.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num5 / 8 + k) * 8 + l;
									int i = num4 * bitmapData.Width + j;
									bool flag = false;
									if (i < dots.Length)
									{
										flag = dots[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
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
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDH + _ESCPOS_FDW + _60mm));
					binaryWriter.Write(string.Format("{1}", _text.TXT_Ticket, text).ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					for (int i = 0; i < _skeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
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
						binaryWriter.Write((byte)10);
						binaryWriter.Write((byte)10);
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Flush();
					return Print_ESCPOS(ptr_device, memoryStream.ToArray());
				}
			}
		}

		public bool Ticket_Out_ESCPOS(string ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm, int _header, DateTime _timestamp, string _control, string _user, Info_Ticket _text, int _hide, int _join)
		{
			string text = Gestion.Build_Mod10(_user, (int)_valor, _id, 1);
			int num = 0;
			if (ptr_device.ToLower().Contains("star"))
			{
				num = 1;
			}
			Barcode barcode = new Barcode();
			Encoding encoding = Encoding.GetEncoding("IBM437");
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 90);
			Bitmap bmpFileName = new Bitmap(original);
			Bitmap bitmap = new Bitmap(350, 300);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				using (Font font3 = new Font("Calibri", 120f, FontStyle.Bold))
				{
					Font font = new Font("Calibri", 34f);
					Font font2 = new Font("Calibri", 30f);
					SizeF sizeF = graphics.MeasureString(_control, font3);
					graphics.FillRectangle(Brushes.White, 0, 0, 350, 300);
					graphics.FillRectangle(new HatchBrush(HatchStyle.DashedUpwardDiagonal, Color.Black, Color.White), 2, 2, 346, 296);
					graphics.DrawString(_control, font3, Brushes.Black, 175f - sizeF.Width / 2f, 0f);
					int num2 = (int)_valor;
					string text2;
					if (_hide == 1)
					{
						text2 = "";
						text2 = text2.PadLeft(12, '*');
					}
					else if (_text.TXT_Points.ToLower() == "euros")
					{
						text2 = $"{num2 / 100}.{num2 % 100:00}# €";
						text2 = text2.PadLeft(12, '#');
					}
					else
					{
						text2 = $"{num2}# {_text.TXT_Points}";
						text2 = text2.PadLeft(14, '#');
					}
					sizeF = graphics.MeasureString(text2, font);
					graphics.DrawString(text2, font, Brushes.Black, 175f - sizeF.Width / 2f, 300f - sizeF.Height * 2f);
					text2 = string.Format("{3:00}:{4:00} {0:00}/{1:00}/{2:0000}", _timestamp.Day, _timestamp.Month, _timestamp.Year, _timestamp.Hour, _timestamp.Minute);
					sizeF = graphics.MeasureString(text2, font2);
					graphics.DrawString(text2, font2, Brushes.Black, 175f - sizeF.Width / 2f, 300f - sizeF.Height - 5f);
				}
			}
			BitmapData bitmapData = GetBitmapData(bmpFileName);
			BitArray dots = bitmapData.Dots;
			byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((byte)27);
					binaryWriter.Write('@');
					binaryWriter.Write((byte)27);
					binaryWriter.Write('T');
					binaryWriter.Write((byte)0);
					if (num == 0)
					{
						if (ptr_device.ToLower().Contains("custom"))
						{
							binaryWriter.Write((byte)27);
							binaryWriter.Write('t');
							binaryWriter.Write((byte)19);
						}
						else
						{
							binaryWriter.Write((byte)27);
							binaryWriter.Write('t');
							binaryWriter.Write((byte)2);
						}
					}
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write($"{_text.TXT_BorneID}: {_user}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)1);
					BitmapData bitmapData2 = GetBitmapData(bitmap);
					BitArray dots2 = bitmapData2.Dots;
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num3 = 0;
					while (num3 < bitmapData2.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData2.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num3 / 8 + k) * 8 + l;
									int i = num4 * bitmapData2.Width + j;
									bool flag = false;
									if (i < dots2.Length)
									{
										flag = dots2[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
							}
						}
						num3 += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToByte(10));
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num5 = 0;
					while (num5 < bitmapData.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num5 / 8 + k) * 8 + l;
									int i = num4 * bitmapData.Width + j;
									bool flag = false;
									if (i < dots.Length)
									{
										flag = dots[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
							}
						}
						num5 += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write($"{text}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FDW + _60mm));
					string text3 = string.Concat((int)_valor);
					string text4 = $"{(int)_valor:X}/{_timestamp.Day:X2}{_timestamp.Month:X}{_timestamp.Year - 2000:X2}/{_timestamp.Hour:X2}{_timestamp.Minute:X2}/{text3.Length}";
					binaryWriter.Write(text4.ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FU + _ESCPOS_FDW + _60mm));
					if (_text.TXT_Location != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
					}
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)0);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin1} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin2} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin3} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin4} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($"RC: {_text.TXT_Lin5}"));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					if (!string.IsNullOrEmpty(_text.TXT_Bottom))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid2))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Null))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					for (int i = 0; i < _skeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
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
						binaryWriter.Write((byte)10);
						binaryWriter.Write((byte)10);
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Flush();
					return Print_ESCPOS(ptr_device, memoryStream.ToArray());
				}
			}
		}

		public bool Ticket_Out_ESCPOS_Check(string ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm, int _header, DateTime _timestamp, string _control, string _user, Info_Ticket _text, int _hide, int _ntick, int _join)
		{
			string text = Gestion.Build_Mod10(_user, (int)_valor, _id, 1);
			int num = 0;
			if (ptr_device.ToLower().Contains("star"))
			{
				num = 1;
			}
			Barcode barcode = new Barcode();
			Encoding encoding = Encoding.GetEncoding("IBM437");
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 90);
			Bitmap bmpFileName = new Bitmap(original);
			Bitmap bitmap = new Bitmap(350, 400);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				using (Font font3 = new Font("Calibri", 120f, FontStyle.Bold))
				{
					Font font = new Font("Calibri", 34f);
					Font font2 = new Font("Calibri", 30f);
					SizeF sizeF = graphics.MeasureString(_control, font3);
					graphics.FillRectangle(Brushes.White, 0, 0, 350, 400);
					graphics.FillRectangle(new HatchBrush(HatchStyle.DashedUpwardDiagonal, Color.Black, Color.White), 2, 2, 346, 396);
					graphics.DrawString(_control, font3, Brushes.Black, 175f - sizeF.Width / 2f, 0f);
					int num2 = (int)_valor;
					string text2 = $"Cheques";
					graphics.DrawString(x: 175f - graphics.MeasureString(text2, font).Width / 2f, s: text2, font: font, brush: Brushes.Black, y: 170f);
					text2 = ((_ntick > 0) ? $"#{_ntick}#" : "---");
					graphics.DrawString(x: 175f - graphics.MeasureString(text2, font).Width / 2f, s: text2, font: font, brush: Brushes.Black, y: 210f);
					if (num2 > 0)
					{
						text2 = $"Reste";
						graphics.DrawString(x: 175f - graphics.MeasureString(text2, font).Width / 2f, s: text2, font: font, brush: Brushes.Black, y: 250f);
						if (_hide == 1)
						{
							text2 = "";
							text2 = text2.PadLeft(12, '*');
						}
						else if (_text.TXT_Points.ToLower() == "euros")
						{
							text2 = $"{num2 / 100}.{num2 % 100:00}# €";
							text2 = text2.PadLeft(12, '#');
						}
						else
						{
							text2 = $"{num2}# {_text.TXT_Points}";
							text2 = text2.PadLeft(14, '#');
						}
						graphics.DrawString(x: 175f - graphics.MeasureString(text2, font).Width / 2f, s: text2, font: font, brush: Brushes.Black, y: 290f);
					}
					text2 = string.Format("{3:00}:{4:00} {0:00}/{1:00}/{2:0000}", _timestamp.Day, _timestamp.Month, _timestamp.Year, _timestamp.Hour, _timestamp.Minute);
					sizeF = graphics.MeasureString(text2, font2);
					graphics.DrawString(text2, font2, Brushes.Black, 175f - sizeF.Width / 2f, 400f - sizeF.Height - 5f);
				}
			}
			BitmapData bitmapData = GetBitmapData(bmpFileName);
			BitArray dots = bitmapData.Dots;
			byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((byte)27);
					binaryWriter.Write('@');
					binaryWriter.Write((byte)27);
					binaryWriter.Write('T');
					binaryWriter.Write((byte)0);
					if (num == 0)
					{
						if (ptr_device.ToLower().Contains("custom"))
						{
							binaryWriter.Write((byte)27);
							binaryWriter.Write('t');
							binaryWriter.Write((byte)19);
						}
						else
						{
							binaryWriter.Write((byte)27);
							binaryWriter.Write('t');
							binaryWriter.Write((byte)2);
						}
					}
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write($"{_text.TXT_BorneID}: {_user}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)1);
					BitmapData bitmapData2 = GetBitmapData(bitmap);
					BitArray dots2 = bitmapData2.Dots;
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num3 = 0;
					while (num3 < bitmapData2.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData2.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num3 / 8 + k) * 8 + l;
									int i = num4 * bitmapData2.Width + j;
									bool flag = false;
									if (i < dots2.Length)
									{
										flag = dots2[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
							}
						}
						num3 += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToByte(10));
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num5 = 0;
					while (num5 < bitmapData.Height)
					{
						binaryWriter.Write(Convert.ToChar(27));
						binaryWriter.Write(Convert.ToChar('*'));
						binaryWriter.Write(Convert.ToChar(33));
						binaryWriter.Write(Convert.ToChar(bytes[0]));
						binaryWriter.Write(Convert.ToChar(bytes[1]));
						for (int j = 0; j < bitmapData.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num4 = (num5 / 8 + k) * 8 + l;
									int i = num4 * bitmapData.Width + j;
									bool flag = false;
									if (i < dots.Length)
									{
										flag = dots[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
							}
						}
						num5 += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write($"{text}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FDW + _60mm));
					string text3 = string.Concat((int)_valor);
					string text4 = $"{(int)_valor:X}/{_timestamp.Day:X2}{_timestamp.Month:X}{_timestamp.Year - 2000:X2}/{_timestamp.Hour:X2}{_timestamp.Minute:X2}/{text3.Length}";
					binaryWriter.Write(text4.ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FU + _ESCPOS_FDW + _60mm));
					if (_text.TXT_Location != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
					}
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)0);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin1} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin2} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin3} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin4} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($"RC: {_text.TXT_Lin5}"));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					if (!string.IsNullOrEmpty(_text.TXT_Bottom))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid2))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Null))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					for (int i = 0; i < _skeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
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
						binaryWriter.Write((byte)10);
						binaryWriter.Write((byte)10);
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Flush();
					return Print_ESCPOS(ptr_device, memoryStream.ToArray());
				}
			}
		}

		public bool Ticket_Image(string ptr_device, decimal _valor, int _tick, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm, int _header, DateTime _timestamp, string _user, Info_Ticket _text, string _bmp)
		{
			int num = 0;
			if (ptr_device.ToLower().Contains("star"))
			{
				num = 1;
			}
			Barcode barcode = new Barcode();
			Encoding encoding = Encoding.GetEncoding("IBM437");
			string arg = Gestion.Build_Mod10(_user, _tick, _id, 0);
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = new Bitmap(_bmp);
			Bitmap bmpFileName = new Bitmap(original);
			BitmapData bitmapData = GetBitmapData(bmpFileName);
			BitArray dots = bitmapData.Dots;
			byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((byte)27);
					binaryWriter.Write('@');
					binaryWriter.Write((byte)27);
					binaryWriter.Write('T');
					binaryWriter.Write((byte)0);
					if (num == 0)
					{
						binaryWriter.Write((byte)27);
						binaryWriter.Write('t');
						binaryWriter.Write((byte)2);
					}
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write($"{_text.TXT_BorneID}: {_user}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					decimal value = _tick;
					TimeSpan timeSpan = new TimeSpan(0, 0, (int)value);
					string text = $"{_text.TXT_Time}: {timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
					if (_valor <= 0m)
					{
						text = "TEST TICKET";
					}
					binaryWriter.Write(text.ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write(string.Format("{2:0}:{3:00} {0}/{1:00}", _timestamp.Day, _timestamp.Month, _timestamp.Hour, _timestamp.Minute).ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FU + _ESCPOS_FDW + _60mm));
					if (_text.TXT_Location != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
					}
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)0);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin1} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin2} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin3} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin4} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($"RC: {_text.TXT_Lin5}"));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
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
						for (int j = 0; j < bitmapData.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num3 = (num2 / 8 + k) * 8 + l;
									int i = num3 * bitmapData.Width + j;
									bool flag = false;
									if (i < dots.Length)
									{
										flag = dots[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
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
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDH + _ESCPOS_FDW + _60mm));
					binaryWriter.Write(string.Format("{1}", _text.TXT_Ticket, arg).ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(_text.TXT_Thanks.ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					if (!string.IsNullOrEmpty(_text.TXT_Bottom))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid2))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Null))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					for (int i = 0; i < _skeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
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
					return Print_ESCPOS(ptr_device, memoryStream.ToArray());
				}
			}
		}

		public bool Ticket_ESCPOS(string ptr_device, decimal _valor, int _tick, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm, int _header, DateTime _timestamp, string _user, Info_Ticket _text)
		{
			int num = 0;
			if (ptr_device.ToLower().Contains("star"))
			{
				num = 1;
			}
			Barcode barcode = new Barcode();
			Encoding encoding = Encoding.GetEncoding("IBM437");
			string text = Gestion.Build_Mod10(_user, _tick, _id, 0);
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 90);
			Bitmap bmpFileName = new Bitmap(original);
			BitmapData bitmapData = GetBitmapData(bmpFileName);
			BitArray dots = bitmapData.Dots;
			byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((byte)27);
					binaryWriter.Write('@');
					binaryWriter.Write((byte)27);
					binaryWriter.Write('T');
					binaryWriter.Write((byte)0);
					if (num == 0)
					{
						binaryWriter.Write((byte)27);
						binaryWriter.Write('t');
						binaryWriter.Write((byte)2);
					}
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write($"{_text.TXT_BorneID}: {_user}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					decimal value = _tick;
					TimeSpan timeSpan = new TimeSpan(0, 0, (int)value);
					string text2 = $"{_text.TXT_Time}: {timeSpan.Hours}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
					if (_valor <= 0m)
					{
						text2 = "TEST TICKET";
					}
					binaryWriter.Write(text2.ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write(string.Format("{2:0}:{3:00} {0}/{1:00}", _timestamp.Day, _timestamp.Month, _timestamp.Hour, _timestamp.Minute).ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FU + _ESCPOS_FDW + _60mm));
					if (_text.TXT_Location != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Location));
					}
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)0);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin1} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin2} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin3} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($" {_text.TXT_Lin4} "));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(encoding.GetBytes($"RC: {_text.TXT_Lin5}"));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
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
						for (int j = 0; j < bitmapData.Width; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								byte b = 0;
								for (int l = 0; l < 8; l++)
								{
									int num3 = (num2 / 8 + k) * 8 + l;
									int i = num3 * bitmapData.Width + j;
									bool flag = false;
									if (i < dots.Length)
									{
										flag = dots[i];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - l));
								}
								binaryWriter.Write(b);
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
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDH + _ESCPOS_FDW + _60mm));
					binaryWriter.Write(string.Format("{1}", _text.TXT_Ticket, text).ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(_text.TXT_Thanks.ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					if (!string.IsNullOrEmpty(_text.TXT_Bottom))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Bottom));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Valid2))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Valid2));
						binaryWriter.Write((byte)10);
					}
					if (!string.IsNullOrEmpty(_text.TXT_Null))
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Null));
						binaryWriter.Write((byte)10);
					}
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					for (int i = 0; i < _skeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
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
					return Print_ESCPOS(ptr_device, memoryStream.ToArray());
				}
			}
		}

		public static BitmapData GetBitmapData(Bitmap bmpFileName)
		{
			using (Bitmap bitmap = bmpFileName)
			{
				int num = 127;
				int num2 = 0;
				int length = bitmap.Width * bitmap.Height;
				BitArray bitArray = new BitArray(length);
				for (int i = 0; i < bitmap.Height; i++)
				{
					for (int j = 0; j < bitmap.Width; j++)
					{
						Color pixel = bitmap.GetPixel(j, i);
						int num3 = (int)((double)(int)pixel.R * 0.3 + (double)(int)pixel.G * 0.59 + (double)(int)pixel.B * 0.11);
						bitArray[num2] = (num3 < num);
						num2++;
					}
				}
				BitmapData bitmapData = new BitmapData();
				bitmapData.Dots = bitArray;
				bitmapData.Height = bitmap.Height;
				bitmapData.Width = bitmap.Width;
				return bitmapData;
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

		public bool Ticket_Out_Conf_ESCPOS(string ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm, int _header, DateTime _timestamp, string _control, string _user, Info_Ticket _text, DateTime _timestampC)
		{
			string text = Gestion.Build_Mod10(_user, (int)_valor, _id, 1);
			int num = 0;
			if (ptr_device.ToLower().Contains("star"))
			{
				num = 1;
			}
			Barcode barcode = new Barcode();
			Encoding encoding = Encoding.GetEncoding("IBM437");
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 90);
			Bitmap bmpFileName = new Bitmap(original);
			BitmapData bitmapData = GetBitmapData(bmpFileName);
			BitArray dots = bitmapData.Dots;
			byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((byte)27);
					binaryWriter.Write('@');
					binaryWriter.Write((byte)27);
					binaryWriter.Write('T');
					binaryWriter.Write((byte)0);
					if (num == 0)
					{
						binaryWriter.Write((byte)27);
						binaryWriter.Write('t');
						binaryWriter.Write((byte)2);
					}
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write($"{_text.TXT_BorneID}: {_user}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					binaryWriter.Write($"{_control}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write(Convert.ToByte(10));
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					int num2 = (int)_valor;
					if (num == 0)
					{
						binaryWriter.Write($"{num2 / 100}.{num2 % 100:00} ".ToCharArray());
						binaryWriter.Write(213);
					}
					else
					{
						binaryWriter.Write($"{num2 / 100}.{num2 % 100:00} Euros".ToCharArray());
					}
					binaryWriter.Write('\n');
					binaryWriter.Write(string.Format("{2:00}:{3:00} {0}/{1:00}", _timestamp.Day, _timestamp.Month, _timestamp.Hour, _timestamp.Minute).ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\n');
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write($"{text}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					if (_text.TXT_Cancel != null)
					{
						binaryWriter.Write(encoding.GetBytes(_text.TXT_Cancel));
					}
					binaryWriter.Write((byte)10);
					binaryWriter.Write(string.Format("{2:00}:{3:00} {0}/{1:00}", _timestampC.Day, _timestampC.Month, _timestampC.Hour, _timestampC.Minute).ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write("-1------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write("-2------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					for (int i = 0; i < _skeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
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
					return Print_ESCPOS(ptr_device, memoryStream.ToArray());
				}
			}
		}
	}
}
