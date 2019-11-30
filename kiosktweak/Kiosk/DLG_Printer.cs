using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Management;
using System.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Printer : Form
	{
		[StructLayout(LayoutKind.Sequential)]
		internal class DEV_BROADCAST_HDR
		{
			internal int dbch_size;

			internal int dbch_devicetype;

			internal int dbch_reserved;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal class DEV_BROADCAST_DEVICEINTERFACE_1
		{
			internal int dbcc_size;

			internal int dbcc_devicetype;

			internal int dbcc_reserved;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.U1)]
			internal byte[] dbcc_classguid;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
			internal char[] dbcc_name;
		}

		internal struct SP_DEVICE_INTERFACE_DATA
		{
			internal int cbSize;

			internal Guid InterfaceClassGuid;

			internal int Flags;

			internal IntPtr Reserved;
		}

		internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
		{
			internal int cbSize;

			internal string DevicePath;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class DEV_BROADCAST_DEVICEINTERFACE
		{
			internal int dbcc_size;

			internal int dbcc_devicetype;

			internal int dbcc_reserved;

			internal Guid dbcc_classguid;

			internal short dbcc_name;
		}

		private enum DBT
		{
			DBT_DEVICEARRIVAL = 0x8000,
			DBT_DEVICEQUERYREMOVE = 32769,
			DBT_DEVICEQUERYREMOVEFAILED = 32770,
			DBT_DEVICEREMOVEPENDING = 32771,
			DBT_DEVICETYPESPECIFIC = 32773,
			DBT_DEVICEREMOVECOMPLETE = 32772,
			DBT_CUSTOMEVENT = 32774,
			DBT_DEVTYP_OEM = 0,
			DBT_DEVTYP_VOLUME = 2,
			DBT_DEVTYP_PORT = 3,
			DBT_DEVTYP_DEVICEINTERFACE = 5,
			DBT_DEVTYP_HANDLE = 6,
			DBT_DEVNODES_CHANGED = 7,
			DBT_QUERYCHANGECONFIG = 23,
			DBT_CONFIGCHANGED = 24
		}

		private const int WM_DEVICECHANGE = 537;

		private const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

		private static ManagementScope oManagementScope;

		private byte _ESCPOS_FU = 128;

		private byte _ESCPOS_FE = 8;

		private byte _ESCPOS_FDW = 32;

		private byte _ESCPOS_FDH = 16;

		private byte _ESCPOS_AL;

		private byte _ESCPOS_AC = 1;

		private byte _ESCPOS_AR = 2;

		private bool RefreshPrint;

		private IContainer components;

		private Button bOK;

		private Button eFind;

		private ListBox cP1;

		private Button bClear;

		private Button bTest;

		private Button bJobs;

		private CheckBox aCut;

		private Button bInfo;

		private Label lInfo;

		private ToolTip toolTip1;

		private Timer timerINFO;

		public DLG_Printer()
		{
			InitializeComponent();
			eFind_Click(null, null);
			RefreshPrint = true;
		}

		[DllImport("user32.dll")]
		public static extern int PeekMessage(out Message lpMsg, IntPtr window, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 537)
			{
				DBT dBT = (DBT)(int)m.WParam;
				if (!(m.LParam == IntPtr.Zero))
				{
					Marshal.ReadInt32(m.LParam, 4);
				}
				DBT dBT2 = dBT;
				if (dBT2 == DBT.DBT_DEVNODES_CHANGED || dBT2 == DBT.DBT_DEVICEARRIVAL || dBT2 == DBT.DBT_DEVICEREMOVECOMPLETE)
				{
					RefreshPrint = true;
				}
			}
			base.WndProc(ref m);
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void eFind_Click(object sender, EventArgs e)
		{
			RefreshPrint = true;
		}

		private void bClear_Click(object sender, EventArgs e)
		{
			Clean_Printer();
			eFind_Click(null, null);
		}

		[DllImport("printui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern void PrintUIEntryW(IntPtr hwnd, IntPtr hinst, string lpszCmdLine, int nCmdShow);

		public static void Clean_Printer()
		{
			string text = null;
			string text2 = null;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				string text3 = item.Properties["Caption"].Value.ToString();
				string text4 = item.Properties["Name"].Value.ToString();
				bool flag = false;
				try
				{
					flag = bool.Parse(item.Properties["WorkOffline"].Value.ToString());
				}
				catch
				{
				}
				if (flag && text3.ToLower().Contains("NII".ToLower()))
				{
					if (!text3.ToLower().Contains("(".ToLower()))
					{
						text = text3;
					}
					string lpszCmdLine = "/dl /n " + '"' + text4 + '"';
					PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
				}
				if (!flag && text3.ToLower().Contains("NII".ToLower()))
				{
					text2 = text3;
				}
			}
			if (text != null && text2 != null)
			{
				RenamePrinter(text2, text);
			}
		}

		public static void RenamePrinter(string sPrinterName, string newName)
		{
			oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
			oManagementScope.Connect();
			SelectQuery selectQuery = new SelectQuery();
			selectQuery.QueryString = "SELECT * FROM Win32_Printer WHERE Name = '" + sPrinterName.Replace("\\", "\\\\") + "'";
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(oManagementScope, selectQuery);
			ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
			if (managementObjectCollection.Count != 0)
			{
				using (ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator())
				{
					if (managementObjectEnumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)managementObjectEnumerator.Current;
						managementObject.InvokeMethod("RenamePrinter", new object[1]
						{
							newName
						});
					}
				}
			}
		}

		private bool Exist_Printer(string _ptr_device)
		{
			if (string.IsNullOrEmpty(_ptr_device))
			{
				return false;
			}
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				string text = item.Properties["Caption"].Value.ToString();
				if (text.ToLower() == _ptr_device.ToLower())
				{
					bool flag = false;
					try
					{
						flag = bool.Parse(item.Properties["WorkOffline"].Value.ToString());
					}
					catch
					{
					}
					return !flag;
				}
			}
			return false;
		}

		private bool Clear_Jobs(string _ptr_device)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			if (string.IsNullOrEmpty(_ptr_device))
			{
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				MessageBox.Show("Printer '" + _ptr_device + "' is not present");
				return false;
			}
			LocalPrintServer val = (LocalPrintServer)(object)new LocalPrintServer();
			PrintQueue printQueue = ((PrintServer)val).GetPrintQueue(_ptr_device);
			((PrintSystemObject)printQueue).Refresh();
			if (printQueue.get_NumberOfJobs() > 0)
			{
				PrintJobInfoCollection printJobInfoCollection = printQueue.GetPrintJobInfoCollection();
				foreach (PrintSystemJobInfo item in printJobInfoCollection)
				{
					item.Cancel();
				}
			}
			return true;
		}

		private int Count_Jobs(string _ptr_device)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			if (string.IsNullOrEmpty(_ptr_device))
			{
				return 0;
			}
			if (!Exist_Printer(_ptr_device))
			{
				MessageBox.Show("Printer '" + _ptr_device + "' is not present");
				return 0;
			}
			LocalPrintServer val = (LocalPrintServer)(object)new LocalPrintServer();
			PrintQueue printQueue = ((PrintServer)val).GetPrintQueue(_ptr_device);
			((PrintSystemObject)printQueue).Refresh();
			int num = 0;
			if (printQueue.get_NumberOfJobs() > 0)
			{
				PrintJobInfoCollection printJobInfoCollection = printQueue.GetPrintJobInfoCollection();
				{
					foreach (PrintSystemJobInfo item in printJobInfoCollection)
					{
						_ = item;
						num++;
					}
					return num;
				}
			}
			return num;
		}

		private void bJobs_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(cP1.Text))
			{
				Clear_Jobs(cP1.Text);
			}
			else
			{
				MessageBox.Show("Select any printer");
			}
		}

		private void bTest_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(cP1.Text))
			{
				Ticket_Test(cP1.Text, aCut.Checked ? 1 : 0);
			}
			else
			{
				MessageBox.Show("Select any printer");
			}
		}

		public bool Print_ESCPOS(string printerName, byte[] document)
		{
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

		public bool Ticket_Test(string ptr_device, int _cut)
		{
			Encoding.GetEncoding("IBM437");
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write((byte)27);
					binaryWriter.Write('@');
					binaryWriter.Write((byte)27);
					binaryWriter.Write('T');
					binaryWriter.Write((byte)0);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('t');
					binaryWriter.Write((byte)2);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write($"PRINTER TEST".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					for (int i = 0; i < 3; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar(114));
					binaryWriter.Write(Convert.ToChar(49));
					binaryWriter.Write(Convert.ToChar(96));
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

		private void bInfo_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(cP1.Text))
			{
				lInfo.Text = cP1.Text + "\r\nJobs: " + Count_Jobs(cP1.Text);
			}
			else
			{
				MessageBox.Show("Select any printer");
			}
		}

		private void cP1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(cP1.Text))
			{
				lInfo.Text = cP1.Text + "\r\nJobs: " + Count_Jobs(cP1.Text);
			}
		}

		private void timerINFO_Tick(object sender, EventArgs e)
		{
			if (RefreshPrint)
			{
				cP1.Items.Clear();
				new PrintDocument();
				foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
				{
					cP1.Items.Add(installedPrinter);
				}
				RefreshPrint = false;
			}
		}

		private void DLG_Printer_Load(object sender, EventArgs e)
		{
			RefreshPrint = true;
			timerINFO.Enabled = true;
		}

		private void DLG_Printer_FormClosing(object sender, FormClosingEventArgs e)
		{
			RefreshPrint = false;
			timerINFO.Enabled = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			cP1 = new System.Windows.Forms.ListBox();
			aCut = new System.Windows.Forms.CheckBox();
			bInfo = new System.Windows.Forms.Button();
			bJobs = new System.Windows.Forms.Button();
			bTest = new System.Windows.Forms.Button();
			bClear = new System.Windows.Forms.Button();
			eFind = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			lInfo = new System.Windows.Forms.Label();
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			timerINFO = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			cP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cP1.FormattingEnabled = true;
			cP1.ItemHeight = 24;
			cP1.Location = new System.Drawing.Point(67, 13);
			cP1.Name = "cP1";
			cP1.Size = new System.Drawing.Size(354, 244);
			cP1.TabIndex = 2;
			toolTip1.SetToolTip(cP1, "Printers list");
			cP1.SelectedIndexChanged += new System.EventHandler(cP1_SelectedIndexChanged);
			aCut.AutoSize = true;
			aCut.Checked = true;
			aCut.CheckState = System.Windows.Forms.CheckState.Checked;
			aCut.Location = new System.Drawing.Point(67, 263);
			aCut.Name = "aCut";
			aCut.Size = new System.Drawing.Size(101, 17);
			aCut.TabIndex = 6;
			aCut.Text = "Force cut paper";
			toolTip1.SetToolTip(aCut, "Force command to cut paper");
			aCut.UseVisualStyleBackColor = true;
			bInfo.Image = Kiosk.Properties.Resources.ico_info;
			bInfo.Location = new System.Drawing.Point(12, 272);
			bInfo.Name = "bInfo";
			bInfo.Size = new System.Drawing.Size(48, 48);
			bInfo.TabIndex = 7;
			toolTip1.SetToolTip(bInfo, "Resfresh info");
			bInfo.UseVisualStyleBackColor = true;
			bInfo.Click += new System.EventHandler(bInfo_Click);
			bJobs.Image = Kiosk.Properties.Resources.ico_brush;
			bJobs.Location = new System.Drawing.Point(13, 326);
			bJobs.Name = "bJobs";
			bJobs.Size = new System.Drawing.Size(48, 48);
			bJobs.TabIndex = 5;
			toolTip1.SetToolTip(bJobs, "Clear jobs");
			bJobs.UseVisualStyleBackColor = true;
			bJobs.Click += new System.EventHandler(bJobs_Click);
			bTest.Image = Kiosk.Properties.Resources.ico_ticket;
			bTest.Location = new System.Drawing.Point(12, 218);
			bTest.Name = "bTest";
			bTest.Size = new System.Drawing.Size(48, 48);
			bTest.TabIndex = 4;
			toolTip1.SetToolTip(bTest, "Test printer");
			bTest.UseVisualStyleBackColor = true;
			bTest.Click += new System.EventHandler(bTest_Click);
			bClear.Image = Kiosk.Properties.Resources.ico_del;
			bClear.Location = new System.Drawing.Point(12, 66);
			bClear.Name = "bClear";
			bClear.Size = new System.Drawing.Size(48, 48);
			bClear.TabIndex = 3;
			toolTip1.SetToolTip(bClear, "Clear disconnected printers");
			bClear.UseVisualStyleBackColor = true;
			bClear.Click += new System.EventHandler(bClear_Click);
			eFind.Image = Kiosk.Properties.Resources.ico_find;
			eFind.Location = new System.Drawing.Point(12, 12);
			eFind.Name = "eFind";
			eFind.Size = new System.Drawing.Size(48, 48);
			eFind.TabIndex = 0;
			toolTip1.SetToolTip(eFind, "Find printers");
			eFind.UseVisualStyleBackColor = true;
			eFind.Click += new System.EventHandler(eFind_Click);
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(373, 326);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(48, 48);
			bOK.TabIndex = 1;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			lInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lInfo.Location = new System.Drawing.Point(67, 287);
			lInfo.Name = "lInfo";
			lInfo.Size = new System.Drawing.Size(300, 87);
			lInfo.TabIndex = 8;
			lInfo.Text = "Select printer";
			toolTip1.SetToolTip(lInfo, "Selected printer and jobs pending");
			timerINFO.Tick += new System.EventHandler(timerINFO_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(433, 381);
			base.Controls.Add(lInfo);
			base.Controls.Add(bInfo);
			base.Controls.Add(aCut);
			base.Controls.Add(bJobs);
			base.Controls.Add(bTest);
			base.Controls.Add(bClear);
			base.Controls.Add(cP1);
			base.Controls.Add(eFind);
			base.Controls.Add(bOK);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Printer";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Tweak Printer";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DLG_Printer_FormClosing);
			base.Load += new System.EventHandler(DLG_Printer_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
