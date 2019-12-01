using System;
using System.ComponentModel;
using System.Management;
using System.Printing;
using System.Runtime.InteropServices;

namespace Kiosk
{
	public class Impresora
	{
		public class Def_Impressores
		{
			public string Nom;

			public bool Cut;

			public int SkeepTop;

			public int SkeepBottom;

			public Def_Impressores()
			{
				Nom = "";
				Cut = true;
				SkeepBottom = 3;
				SkeepTop = 3;
			}
		}

		public Def_Impressores[] Impressores;

		private static ManagementScope oManagementScope = null;

		public Impresora()
		{
			Impressores = new Def_Impressores[0];
		}

		public bool Add_Impressora(string _nom, int _sb, int _st, bool _c)
		{
			int num = Impressores.Length;
			Array.Resize(ref Impressores, num + 1);
			Impressores[num] = new Def_Impressores();
			Impressores[num].Nom = _nom;
			Impressores[num].Cut = _c;
			Impressores[num].SkeepBottom = _sb;
			Impressores[num].SkeepTop = _st;
			return true;
		}

		private bool Print_Reset(string _ptr_device)
		{
			if (string.IsNullOrEmpty(_ptr_device))
			{
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				return false;
			}
			LocalPrintServer localPrintServer = new LocalPrintServer();
			PrintQueue printQueue = localPrintServer.GetPrintQueue(_ptr_device);
			printQueue.Refresh();
			if (printQueue.NumberOfJobs > 0)
			{
				PrintJobInfoCollection printJobInfoCollection = printQueue.GetPrintJobInfoCollection();
				foreach (PrintSystemJobInfo item in printJobInfoCollection)
				{
					item.Cancel();
				}
			}
			return true;
		}

		private int Wait_Ticket_Poll(string _ptr_device)
		{
			LocalPrintServer localPrintServer = new LocalPrintServer();
			PrintQueue printQueue = localPrintServer.GetPrintQueue(_ptr_device);
			PrintJobInfoCollection printJobInfoCollection = printQueue.GetPrintJobInfoCollection();
			if (printQueue.NumberOfJobs > 0)
			{
				if (printQueue.IsPrinting)
				{
					return 2;
				}
				return 1;
			}
			return 0;
		}

		public bool Printer_Poll(string _ptr_device)
		{
			if (string.IsNullOrEmpty(_ptr_device))
			{
				return false;
			}
			LocalPrintServer localPrintServer = new LocalPrintServer();
			PrintQueue printQueue = localPrintServer.GetPrintQueue(_ptr_device);
			PrintJobInfoCollection printJobInfoCollection = printQueue.GetPrintJobInfoCollection();
			if (printQueue.NumberOfJobs > 0)
			{
				if (printQueue.IsPrinting)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		[DllImport("printui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern void PrintUIEntryW(IntPtr hwnd, IntPtr hinst, string lpszCmdLine, int nCmdShow);

		public static void Clean_Nipon_Printer()
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

		public static void Clean_Custom_Printer()
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
				if (flag && text3.ToLower().Contains("CUSTOM".ToLower()))
				{
					if (!text3.ToLower().Contains("(".ToLower()))
					{
						text = text3;
					}
					string lpszCmdLine = "/dl /n " + '"' + text4 + '"';
					PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
				}
				if (!flag && text3.ToLower().Contains("CUSTOM".ToLower()))
				{
					text2 = text3;
				}
			}
			if (text != null && text2 != null)
			{
				RenamePrinter(text2, text);
			}
		}

		public static void Clean_Star_Printer()
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
				if (flag && text3.ToLower().Contains("START".ToLower()))
				{
					if (!text3.ToLower().Contains("(".ToLower()))
					{
						text = text3;
					}
					string lpszCmdLine = "/dl /n " + '"' + text4 + '"';
					PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
				}
				if (!flag && text3.ToLower().Contains("STAR".ToLower()))
				{
					text2 = text3;
				}
			}
			if (text != null && text2 != null)
			{
				RenamePrinter(text2, text);
			}
		}

		public static void Clean_SANEI_Printer()
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
				if (flag && text3.ToLower().Contains("SANEI".ToLower()))
				{
					if (!text3.ToLower().Contains("(".ToLower()))
					{
						text = text3;
					}
					string lpszCmdLine = "/dl /n " + '"' + text4 + '"';
					PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
				}
				if (!flag && text3.ToLower().Contains("SANEI".ToLower()))
				{
					text2 = text3;
				}
			}
			if (text != null && text2 != null)
			{
				RenamePrinter(text2, text);
			}
		}

		public static void RenamePrinter(string _ptr_device, string newName)
		{
			oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
			oManagementScope.Connect();
			SelectQuery selectQuery = new SelectQuery();
			selectQuery.QueryString = "SELECT * FROM Win32_Printer WHERE Name = '" + _ptr_device.Replace("\\", "\\\\") + "'";
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

		private string SpotTroubleUsingQueueAttributes(PrintQueue pq)
		{
			_ = pq.QueueStatus;
			if (0 == 0)
			{
				return "OK";
			}
			if ((pq.QueueStatus & PrintQueueStatus.PaperProblem) == PrintQueueStatus.PaperProblem && pq.IsOutOfPaper)
			{
				return "Has a paper problem";
			}
			if ((pq.QueueStatus & PrintQueueStatus.NoToner) == PrintQueueStatus.NoToner)
			{
				return "Is out of toner";
			}
			if ((pq.QueueStatus & PrintQueueStatus.DoorOpen) == PrintQueueStatus.DoorOpen)
			{
				return "Has an open door";
			}
			if ((pq.QueueStatus & PrintQueueStatus.Error) == PrintQueueStatus.Error)
			{
				return "Is in an error state";
			}
			if ((pq.QueueStatus & PrintQueueStatus.NotAvailable) == PrintQueueStatus.NotAvailable)
			{
				return "Is not available";
			}
			if ((pq.QueueStatus & PrintQueueStatus.Offline) == PrintQueueStatus.Offline)
			{
				return "Is off line";
			}
			if ((pq.QueueStatus & PrintQueueStatus.OutOfMemory) == PrintQueueStatus.OutOfMemory)
			{
				return "Is out of memory";
			}
			if ((pq.QueueStatus & PrintQueueStatus.PaperOut) == PrintQueueStatus.PaperOut)
			{
				return "Is out of paper";
			}
			if ((pq.QueueStatus & PrintQueueStatus.OutputBinFull) == PrintQueueStatus.OutputBinFull)
			{
				return "Has a full output bin";
			}
			if ((pq.QueueStatus & PrintQueueStatus.PaperJam) == PrintQueueStatus.PaperJam)
			{
				return "Has a paper jam";
			}
			if ((pq.QueueStatus & PrintQueueStatus.Paused) == PrintQueueStatus.Paused)
			{
				return "Is paused";
			}
			if ((pq.QueueStatus & PrintQueueStatus.TonerLow) == PrintQueueStatus.TonerLow)
			{
				return "Is low on toner";
			}
			if ((pq.QueueStatus & PrintQueueStatus.UserIntervention) == PrintQueueStatus.UserIntervention)
			{
				return "Needs user intervention";
			}
			return "OK";
		}

		private string SpotTroubleUsingProperties(PrintQueue pq)
		{
			if (pq.IsWaiting)
			{
				return "OK";
			}
			if (pq.HasPaperProblem && pq.IsOutOfPaper)
			{
				return "Has a paper problem";
			}
			if (!pq.HasToner)
			{
				return "Is out of toner";
			}
			if (pq.IsDoorOpened)
			{
				return "Has an open door";
			}
			if (pq.IsInError)
			{
				return "Is in an error state";
			}
			if (pq.IsNotAvailable)
			{
				return "Is not available";
			}
			if (pq.IsOffline)
			{
				return "Is off line";
			}
			if (pq.IsOutOfMemory)
			{
				return "Is out of memory";
			}
			if (pq.IsOutOfPaper)
			{
				return "Is out of paper";
			}
			if (pq.IsOutputBinFull)
			{
				return "Has a full output bin";
			}
			if (pq.IsPaperJammed)
			{
				return "Has a paper jam";
			}
			if (pq.IsPaused)
			{
				return "Is paused";
			}
			if (pq.IsTonerLow)
			{
				return "Is low on toner";
			}
			if (pq.NeedUserIntervention)
			{
				return "Needs user intervention";
			}
			return "OK";
		}

		private bool PaperOut_Printer(string _ptr_device)
		{
			if (string.IsNullOrEmpty(_ptr_device))
			{
				return false;
			}
			LocalPrintServer localPrintServer = new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer);
			PrintQueueCollection printQueues = localPrintServer.GetPrintQueues();
			foreach (PrintQueue item in printQueues)
			{
				item.Refresh();
				if (item.Name.ToLower() == _ptr_device.ToLower())
				{
					string a = SpotTroubleUsingQueueAttributes(item);
					if (a != "OK")
					{
						return false;
					}
					a = SpotTroubleUsingProperties(item);
					if (a != "OK")
					{
						return false;
					}
					return true;
				}
			}
			return false;
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

		public string Impresora_Like(string _imp)
		{
			if (_imp.Contains("*"))
			{
				return _imp.Substring(1).ToLower();
			}
			return _imp.ToLower();
		}

		public string Find_Printer(bool _clean = false)
		{
			if (_clean)
			{
				Clean_Custom_Printer();
				Clean_Nipon_Printer();
				Clean_Star_Printer();
				Clean_SANEI_Printer();
			}
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				string text = item.Properties["Caption"].Value.ToString();
				bool flag = false;
				try
				{
					flag = bool.Parse(item.Properties["WorkOffline"].Value.ToString());
				}
				catch
				{
				}
				if (!flag)
				{
					for (int i = 0; i < Impressores.Length; i++)
					{
						string value = Impresora_Like(Impressores[i].Nom);
						if (text.ToLower().Contains(value))
						{
							return text;
						}
					}
				}
			}
			return null;
		}

		public string Print_Ticket(byte[] document)
		{
			string text = Find_Printer();
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			NativeMethods.DOC_INFO_1 dOC_INFO_ = new NativeMethods.DOC_INFO_1();
			dOC_INFO_.pDataType = "RAW";
			dOC_INFO_.pDocName = "Bit Image Test";
			IntPtr hPrinter = new IntPtr(0);
			if (NativeMethods.OpenPrinter(text.Normalize(), out hPrinter, IntPtr.Zero))
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
						return text;
					}
					throw new Win32Exception();
				}
				throw new Win32Exception();
			}
			throw new Win32Exception();
		}
	}
}
