// Decompiled with JetBrains decompiler
// Type: Kiosk.Impresora
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.Management;
using System.Printing;
using System.Runtime.InteropServices;

namespace Kiosk
{
  public class Impresora
  {
    private static ManagementScope oManagementScope = (ManagementScope) null;
    public Impresora.Def_Impressores[] Impressores;

    public Impresora()
    {
      this.Impressores = new Impresora.Def_Impressores[0];
    }

    public bool Add_Impressora(string _nom, int _sb, int _st, bool _c)
    {
      int length = this.Impressores.Length;
      Array.Resize<Impresora.Def_Impressores>(ref this.Impressores, length + 1);
      this.Impressores[length] = new Impresora.Def_Impressores();
      this.Impressores[length].Nom = _nom;
      this.Impressores[length].Cut = _c;
      this.Impressores[length].SkeepBottom = _sb;
      this.Impressores[length].SkeepTop = _st;
      return true;
    }

    private bool Print_Reset(string _ptr_device)
    {
      if (string.IsNullOrEmpty(_ptr_device) || !this.Exist_Printer(_ptr_device))
        return false;
      PrintQueue printQueue = new LocalPrintServer().GetPrintQueue(_ptr_device);
      printQueue.Refresh();
      if (printQueue.NumberOfJobs > 0)
      {
        foreach (PrintSystemJobInfo printJobInfo in printQueue.GetPrintJobInfoCollection())
          printJobInfo.Cancel();
      }
      return true;
    }

    private int Wait_Ticket_Poll(string _ptr_device)
    {
      PrintQueue printQueue = new LocalPrintServer().GetPrintQueue(_ptr_device);
      printQueue.GetPrintJobInfoCollection();
      if (printQueue.NumberOfJobs <= 0)
        return 0;
      return printQueue.IsPrinting ? 2 : 1;
    }

    public bool Printer_Poll(string _ptr_device)
    {
      if (string.IsNullOrEmpty(_ptr_device))
        return false;
      PrintQueue printQueue = new LocalPrintServer().GetPrintQueue(_ptr_device);
      printQueue.GetPrintJobInfoCollection();
      return printQueue.NumberOfJobs > 0 && printQueue.IsPrinting;
    }

    [DllImport("printui.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern void PrintUIEntryW(
      IntPtr hwnd,
      IntPtr hinst,
      string lpszCmdLine,
      int nCmdShow);

    public static void Clean_Nipon_Printer()
    {
      string newName = (string) null;
      string _ptr_device = (string) null;
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Printer").Get())
      {
        string str1 = managementObject.Properties["Caption"].Value.ToString();
        string str2 = managementObject.Properties["Name"].Value.ToString();
        bool flag = false;
        try
        {
          flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
        }
        catch
        {
        }
        if (flag && str1.ToLower().Contains("NII".ToLower()))
        {
          if (!str1.ToLower().Contains("(".ToLower()))
            newName = str1;
          string lpszCmdLine = "/dl /n " + (object) '"' + str2 + (object) '"';
          Impresora.PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
        }
        if (!flag && str1.ToLower().Contains("NII".ToLower()))
          _ptr_device = str1;
      }
      if (newName == null || _ptr_device == null)
        return;
      Impresora.RenamePrinter(_ptr_device, newName);
    }

    public static void Clean_Custom_Printer()
    {
      string newName = (string) null;
      string _ptr_device = (string) null;
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Printer").Get())
      {
        string str1 = managementObject.Properties["Caption"].Value.ToString();
        string str2 = managementObject.Properties["Name"].Value.ToString();
        bool flag = false;
        try
        {
          flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
        }
        catch
        {
        }
        if (flag && str1.ToLower().Contains("CUSTOM".ToLower()))
        {
          if (!str1.ToLower().Contains("(".ToLower()))
            newName = str1;
          string lpszCmdLine = "/dl /n " + (object) '"' + str2 + (object) '"';
          Impresora.PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
        }
        if (!flag && str1.ToLower().Contains("CUSTOM".ToLower()))
          _ptr_device = str1;
      }
      if (newName == null || _ptr_device == null)
        return;
      Impresora.RenamePrinter(_ptr_device, newName);
    }

    public static void Clean_Star_Printer()
    {
      string newName = (string) null;
      string _ptr_device = (string) null;
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Printer").Get())
      {
        string str1 = managementObject.Properties["Caption"].Value.ToString();
        string str2 = managementObject.Properties["Name"].Value.ToString();
        bool flag = false;
        try
        {
          flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
        }
        catch
        {
        }
        if (flag && str1.ToLower().Contains("START".ToLower()))
        {
          if (!str1.ToLower().Contains("(".ToLower()))
            newName = str1;
          string lpszCmdLine = "/dl /n " + (object) '"' + str2 + (object) '"';
          Impresora.PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
        }
        if (!flag && str1.ToLower().Contains("STAR".ToLower()))
          _ptr_device = str1;
      }
      if (newName == null || _ptr_device == null)
        return;
      Impresora.RenamePrinter(_ptr_device, newName);
    }

    public static void Clean_SANEI_Printer()
    {
      string newName = (string) null;
      string _ptr_device = (string) null;
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Printer").Get())
      {
        string str1 = managementObject.Properties["Caption"].Value.ToString();
        string str2 = managementObject.Properties["Name"].Value.ToString();
        bool flag = false;
        try
        {
          flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
        }
        catch
        {
        }
        if (flag && str1.ToLower().Contains("SANEI".ToLower()))
        {
          if (!str1.ToLower().Contains("(".ToLower()))
            newName = str1;
          string lpszCmdLine = "/dl /n " + (object) '"' + str2 + (object) '"';
          Impresora.PrintUIEntryW(IntPtr.Zero, IntPtr.Zero, lpszCmdLine, 0);
        }
        if (!flag && str1.ToLower().Contains("SANEI".ToLower()))
          _ptr_device = str1;
      }
      if (newName == null || _ptr_device == null)
        return;
      Impresora.RenamePrinter(_ptr_device, newName);
    }

    public static void RenamePrinter(string _ptr_device, string newName)
    {
      Impresora.oManagementScope = new ManagementScope(ManagementPath.DefaultPath);
      Impresora.oManagementScope.Connect();
      SelectQuery selectQuery = new SelectQuery();
      selectQuery.QueryString = "SELECT * FROM Win32_Printer WHERE Name = '" + _ptr_device.Replace("\\", "\\\\") + "'";
      ManagementObjectCollection objectCollection = new ManagementObjectSearcher(Impresora.oManagementScope, (ObjectQuery) selectQuery).Get();
      if (objectCollection.Count == 0)
        return;
      using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = objectCollection.GetEnumerator())
      {
        if (enumerator.MoveNext())
          ((ManagementObject) enumerator.Current).InvokeMethod(nameof (RenamePrinter), new object[1]
          {
            (object) newName
          });
      }
    }

    private string SpotTroubleUsingQueueAttributes(PrintQueue pq)
    {
      int queueStatus = (int) pq.QueueStatus;
      if (true)
        return "OK";
      if ((pq.QueueStatus & PrintQueueStatus.PaperProblem) == PrintQueueStatus.PaperProblem && pq.IsOutOfPaper)
        return "Has a paper problem";
      if ((pq.QueueStatus & PrintQueueStatus.NoToner) == PrintQueueStatus.NoToner)
        return "Is out of toner";
      if ((pq.QueueStatus & PrintQueueStatus.DoorOpen) == PrintQueueStatus.DoorOpen)
        return "Has an open door";
      if ((pq.QueueStatus & PrintQueueStatus.Error) == PrintQueueStatus.Error)
        return "Is in an error state";
      if ((pq.QueueStatus & PrintQueueStatus.NotAvailable) == PrintQueueStatus.NotAvailable)
        return "Is not available";
      if ((pq.QueueStatus & PrintQueueStatus.Offline) == PrintQueueStatus.Offline)
        return "Is off line";
      if ((pq.QueueStatus & PrintQueueStatus.OutOfMemory) == PrintQueueStatus.OutOfMemory)
        return "Is out of memory";
      if ((pq.QueueStatus & PrintQueueStatus.PaperOut) == PrintQueueStatus.PaperOut)
        return "Is out of paper";
      if ((pq.QueueStatus & PrintQueueStatus.OutputBinFull) == PrintQueueStatus.OutputBinFull)
        return "Has a full output bin";
      if ((pq.QueueStatus & PrintQueueStatus.PaperJam) == PrintQueueStatus.PaperJam)
        return "Has a paper jam";
      if ((pq.QueueStatus & PrintQueueStatus.Paused) == PrintQueueStatus.Paused)
        return "Is paused";
      if ((pq.QueueStatus & PrintQueueStatus.TonerLow) == PrintQueueStatus.TonerLow)
        return "Is low on toner";
      return (pq.QueueStatus & PrintQueueStatus.UserIntervention) == PrintQueueStatus.UserIntervention ? "Needs user intervention" : "OK";
    }

    private string SpotTroubleUsingProperties(PrintQueue pq)
    {
      if (pq.IsWaiting)
        return "OK";
      if (pq.HasPaperProblem && pq.IsOutOfPaper)
        return "Has a paper problem";
      if (!pq.HasToner)
        return "Is out of toner";
      if (pq.IsDoorOpened)
        return "Has an open door";
      if (pq.IsInError)
        return "Is in an error state";
      if (pq.IsNotAvailable)
        return "Is not available";
      if (pq.IsOffline)
        return "Is off line";
      if (pq.IsOutOfMemory)
        return "Is out of memory";
      if (pq.IsOutOfPaper)
        return "Is out of paper";
      if (pq.IsOutputBinFull)
        return "Has a full output bin";
      if (pq.IsPaperJammed)
        return "Has a paper jam";
      if (pq.IsPaused)
        return "Is paused";
      if (pq.IsTonerLow)
        return "Is low on toner";
      return pq.NeedUserIntervention ? "Needs user intervention" : "OK";
    }

    private bool PaperOut_Printer(string _ptr_device)
    {
      if (string.IsNullOrEmpty(_ptr_device))
        return false;
      foreach (PrintQueue printQueue in new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer).GetPrintQueues())
      {
        printQueue.Refresh();
        if (printQueue.Name.ToLower() == _ptr_device.ToLower())
          return !(this.SpotTroubleUsingQueueAttributes(printQueue) != "OK") && !(this.SpotTroubleUsingProperties(printQueue) != "OK");
      }
      return false;
    }

    private bool Exist_Printer(string _ptr_device)
    {
      if (string.IsNullOrEmpty(_ptr_device))
        return false;
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Printer").Get())
      {
        if (managementObject.Properties["Caption"].Value.ToString().ToLower() == _ptr_device.ToLower())
        {
          bool flag = false;
          try
          {
            flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
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
        return _imp.Substring(1).ToLower();
      return _imp.ToLower();
    }

    public string Find_Printer(bool _clean = false)
    {
      if (_clean)
      {
        Impresora.Clean_Custom_Printer();
        Impresora.Clean_Nipon_Printer();
        Impresora.Clean_Star_Printer();
        Impresora.Clean_SANEI_Printer();
      }
      foreach (ManagementObject managementObject in new ManagementObjectSearcher("SELECT * FROM Win32_Printer").Get())
      {
        string str1 = managementObject.Properties["Caption"].Value.ToString();
        bool flag = false;
        try
        {
          flag = bool.Parse(managementObject.Properties["WorkOffline"].Value.ToString());
        }
        catch
        {
        }
        if (!flag)
        {
          for (int index = 0; index < this.Impressores.Length; ++index)
          {
            string str2 = this.Impresora_Like(this.Impressores[index].Nom);
            if (str1.ToLower().Contains(str2))
              return str1;
          }
        }
      }
      return (string) null;
    }

    public string Print_Ticket(byte[] document)
    {
      string printer = this.Find_Printer(false);
      if (string.IsNullOrEmpty(printer))
        return (string) null;
      NativeMethods.DOC_INFO_1 di = new NativeMethods.DOC_INFO_1();
      di.pDataType = "RAW";
      di.pDocName = "Bit Image Test";
      IntPtr hPrinter = new IntPtr(0);
      if (!NativeMethods.OpenPrinter(printer.Normalize(), out hPrinter, IntPtr.Zero))
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
      return printer;
    }

    public class Def_Impressores
    {
      public string Nom;
      public bool Cut;
      public int SkeepTop;
      public int SkeepBottom;

      public Def_Impressores()
      {
        this.Nom = "";
        this.Cut = true;
        this.SkeepBottom = 3;
        this.SkeepTop = 3;
      }
    }
  }
}
