// Decompiled with JetBrains decompiler
// Type: Kiosk.Program
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      bool flag = true;
      flag = false;
      Program.Check_DNS();
      string str1 = "c:\\kiosk\\Loader.exe";
      string str2 = "c:\\kiosk\\_Loader.exe";
      string path1 = "c:\\kiosk\\_Loader.exe.ver";
      string path2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\kiosk.options.tmp";
      if (System.IO.File.Exists(path2))
      {
        try
        {
          System.IO.File.Delete(path2);
        }
        catch
        {
        }
      }
      flag = true;
      if (System.IO.File.Exists(str1) && System.IO.File.Exists(str2) && System.IO.File.Exists(path1) && !Program.FileEquals(str1, str2) && (GLib.Util.HashFile(str2, false) && Program.FindAndKillProcess("Loader")))
      {
        Thread.Sleep(2000);
        if (!Program.FindAndKillProcess("Loader"))
        {
          try
          {
            System.IO.File.Delete(str1);
          }
          catch
          {
          }
          Thread.Sleep(1000);
          try
          {
            System.IO.File.Copy(str2, str1);
          }
          catch
          {
          }
          Thread.Sleep(1000);
          Program.Reboot();
          Thread.Sleep(10000);
          Application.Exit();
        }
      }
      flag = true;
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Configuracion _opc = new Configuracion();
      _opc.Set_AppName("kiosk".ToLower());
      flag = true;
      if (System.IO.File.Exists(_opc.CfgFileFull + ".tmp"))
      {
        try
        {
          System.IO.File.Delete(_opc.CfgFileFull + ".tmp");
        }
        catch
        {
        }
      }
      flag = true;
      _opc.Load_Net();
      flag = true;
      if (_opc.Reset == 1)
      {
        flag = true;
        _opc.Save_Net();
      }
      flag = true;
      if (_opc.MACLAN != "0")
        _opc.Save_Net(string.Format("{0}", (object) _opc.MACLAN), "cfg");
      flag = true;
      flag = false;
      Program.Update_DateTime();
      flag = true;
      if (_opc.unique && Process.GetProcessesByName(Assembly.GetExecutingAssembly().GetName().Name).Length > 1)
        return;
      Configuracion.CleanUpDisk();
      string str3 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
      if (System.IO.File.Exists(str3) && Configuracion.VNC_Check_Timestamp() == 1)
        Process.Start(str3, "-connect " + _opc.Server_VNC + ":5500 -run");
      flag = true;
      if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe") && Configuracion.Freeze_Check() == 0 && Configuracion.Freeze_Check_Timestamp() == 0)
        Configuracion.Freeze_On();
      flag = true;
      if (_opc.VersionPRG == "update")
      {
        Process.Start("shutdown.exe", "/r");
      }
      else
      {
        for (int index1 = 0; index1 < 5 && !Program.PingTest(); ++index1)
        {
          for (int index2 = 0; index2 < 100; ++index2)
          {
            Application.DoEvents();
            Thread.Sleep(10);
          }
        }
        flag = true;
        if (!Program.PingTest())
        {
          int num = (int) new MSGBOX_Timer("No INTERNET Connection. Check Network cable, network parameters or WIFI", "Ok", 5, false).ShowDialog();
        }
        Configuracion.Access_Log("Load Kiosk");
        flag = true;
        if (Program.FindWinPCapProcess() || Program.FindVMWareProcess() || Program.FindWiresharkProcess() || Program.RedPill() == 1)
          _opc.ForceSpy = true;
        flag = true;
        Application.Run((Form) new MainWindow(ref _opc));
        flag = false;
        if (Program.FindAndKillProcess("Loader"))
        {
          Thread.Sleep(2000);
          if (!Program.FindAndKillProcess("Loader"))
          {
            Program.RunProcess(str1);
            Application.Exit();
          }
        }
      }
    }

    private static bool Run_Cmd(string _param)
    {
      new Process()
      {
        StartInfo = {
          FileName = "cmd.exe",
          Arguments = ("/C \"" + _param + "\""),
          CreateNoWindow = true,
          WindowStyle = ProcessWindowStyle.Hidden
        }
      }.Start();
      return true;
    }

    public static void Update_DateTime()
    {
      int num1 = 0;
      Process process = new Process();
      string[] strArray = new string[12]
      {
        "net",
        "stop \"w32time\"",
        "w32tm",
        "/config /manualpeerlist:\"0.europe.pool.ntp.org 1.europe.pool.ntp.org 2.europe.pool.ntp.org 3.europe.pool.ntp.org\"",
        "w32tm",
        "/config /syncfromflags:MANUAL",
        "w32tm",
        "/config /reliable:YES",
        "net",
        "start \"w32time\"",
        "w32tm",
        "/resync"
      };
      DateTime now1 = DateTime.Now;
      int num2 = 0;
      DateTime now2;
      do
      {
        for (int index1 = 0; index1 < strArray.Length; index1 += 2)
        {
          try
          {
            process.StartInfo.FileName = strArray[index1];
            process.StartInfo.Arguments = strArray[index1 + 1];
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            Thread.Sleep(1000);
            process.WaitForExit();
          }
          catch (Exception ex)
          {
            ++num1;
            for (int index2 = 0; index2 < 10; ++index2)
            {
              Application.DoEvents();
              Thread.Sleep(200);
            }
          }
        }
        now2 = DateTime.Now;
        ++num2;
      }
      while (now2.Year < 2017 && num2 < 4);
      if (now2.Year >= 2017)
        return;
      Program.Run_Cmd("date 1-2-2017");
      now2 = DateTime.Now;
      if (now2.Year < 2017)
      {
        Program.Run_Cmd("date 1-2-2017");
        now2 = DateTime.Now;
        if (now2.Year < 2017)
        {
          Program.Run_Cmd("date 2/1/2017");
          now2 = DateTime.Now;
          if (now2.Year < 2017)
          {
            Program.Run_Cmd("date 2/1/2017");
            now2 = DateTime.Now;
            if (now2.Year < 2017)
            {
              int num3 = num1 + 1;
            }
          }
        }
      }
    }

    private static bool FileEquals(string path1, string path2)
    {
      byte[] numArray1 = System.IO.File.ReadAllBytes(path1);
      byte[] numArray2 = System.IO.File.ReadAllBytes(path2);
      if (numArray1.Length != numArray2.Length)
        return false;
      for (int index = 0; index < numArray1.Length; ++index)
      {
        if ((int) numArray1[index] != (int) numArray2[index])
          return false;
      }
      return true;
    }

    public static bool FindAndKillProcess(string name)
    {
      name = name.ToLower();
      foreach (Process process in Process.GetProcesses())
      {
        if (process.ProcessName.ToLower().StartsWith(name))
        {
          process.Kill();
          return true;
        }
      }
      return false;
    }

    private static void Reboot()
    {
      if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe") && Configuracion.VNC_Running())
        Configuracion.VNC_Build_Timestamp();
      Process.Start("shutdown.exe", "/r /t 2");
    }

    public static bool FindWiresharkProcess()
    {
      string lower = "wireshark".ToLower();
      foreach (Process process in Process.GetProcesses())
      {
        if (process.ProcessName.ToLower().StartsWith(lower))
        {
          process.Kill();
          return true;
        }
      }
      return false;
    }

    private static unsafe int RedPill()
    {
      byte[] source = new byte[8]
      {
        (byte) 15,
        (byte) 1,
        (byte) 13,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 195
      };
      fixed (byte* numPtr1 = new byte[6])
        fixed (byte* numPtr2 = source)
        {
          *(int*) (numPtr2 + 3) = (int) (uint) numPtr1;
          using (Program.VirtualMemoryPtr virtualMemoryPtr = new Program.VirtualMemoryPtr(source.Length))
          {
            Marshal.Copy(source, 0, (IntPtr) virtualMemoryPtr, source.Length);
            ((Program.MethodInvoker) Marshal.GetDelegateForFunctionPointer((IntPtr) virtualMemoryPtr, typeof (Program.MethodInvoker)))();
          }
          return numPtr1[5] > (byte) 208 ? 1 : 0;
        }
    }

    public static bool FindVMWareProcess()
    {
      string lower = "vmware".ToLower();
      foreach (Process process in Process.GetProcesses())
      {
        if (process.ProcessName.ToLower().StartsWith(lower))
        {
          process.Kill();
          return true;
        }
      }
      return false;
    }

    public static bool FindWinPCapProcess()
    {
      string lower = "packet".ToLower();
      foreach (Process process in Process.GetProcesses())
      {
        if (process.ProcessName.ToLower().StartsWith(lower))
        {
          process.Kill();
          return true;
        }
      }
      return false;
    }

    public static bool PingTest()
    {
      Ping ping = new Ping();
      PingReply pingReply;
      try
      {
        pingReply = ping.Send(IPAddress.Parse("8.8.8.8"));
      }
      catch
      {
        return false;
      }
      return pingReply.Status == IPStatus.Success;
    }

    public static bool RunProcess(string name)
    {
      Process process = new Process();
      try
      {
        process.StartInfo.WorkingDirectory = "c:\\Kiosk\\";
        process.StartInfo.FileName = name;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
        process.Start();
      }
      catch
      {
        return false;
      }
      return true;
    }

    public static void GetIP(
      out string[] enableDHCP,
      out string[] ipAdresses,
      out string[] subnets,
      out string[] gateways,
      out string[] dnses,
      out bool dhcp)
    {
      ipAdresses = (string[]) null;
      subnets = (string[]) null;
      gateways = (string[]) null;
      dnses = (string[]) null;
      enableDHCP = (string[]) null;
      dhcp = false;
      try
      {
        ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
        try
        {
          ManagementObjectCollection instances = managementClass.GetInstances();
          try
          {
            foreach (ManagementObject managementObject in instances)
            {
              if ((bool) managementObject["DHCPEnabled"])
                dhcp = true;
              if ((bool) managementObject["ipEnabled"])
              {
                ipAdresses = (string[]) managementObject["IPAddress"];
                subnets = (string[]) managementObject["IPSubnet"];
                gateways = (string[]) managementObject["DefaultIPGateway"];
                dnses = (string[]) managementObject["DNSServerSearchOrder"];
                break;
              }
            }
          }
          catch
          {
          }
        }
        catch
        {
        }
      }
      catch
      {
      }
    }

    public static void SetDNS(string[] _dns)
    {
      foreach (ManagementObject instance in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
      {
        if ((bool) instance["IPEnabled"])
        {
          ManagementBaseObject methodParameters = instance.GetMethodParameters("SetDNSServerSearchOrder");
          methodParameters["DNSServerSearchOrder"] = (object) _dns;
          instance.InvokeMethod("SetDNSServerSearchOrder", methodParameters, (InvokeMethodOptions) null);
        }
      }
    }

    private static void Check_DNS()
    {
      bool dhcp = false;
      string[] enableDHCP;
      string[] ipAdresses;
      string[] subnets;
      string[] gateways;
      string[] dnses;
      Program.GetIP(out enableDHCP, out ipAdresses, out subnets, out gateways, out dnses, out dhcp);
      int num1 = 0;
      if (dnses == null)
      {
        num1 = 1;
      }
      else
      {
        int num2 = 0;
        for (int index = 0; index < dnses.Length; ++index)
        {
          if (dnses[index] == "8.8.8.8")
            ++num2;
          if (dnses[index] == "8.8.4.4")
            ++num2;
        }
        if (num2 < 2)
          num1 = 1;
      }
      if (num1 != 1)
        return;
      Program.SetDNS(new string[2]
      {
        "8.8.8.8",
        "8.8.4.4"
      });
    }

    private delegate void MethodInvoker();

    public class VirtualMemoryPtr : SafeHandle
    {
      public readonly IntPtr AllocatedPointer;
      private readonly IntPtr ProcessHandle;
      private readonly IntPtr MemorySize;
      private bool Disposed;

      public VirtualMemoryPtr(int memorySize)
        : base(IntPtr.Zero, true)
      {
        this.ProcessHandle = Program.VirtualMemoryManager.GetCurrentProcessHandle();
        this.MemorySize = (IntPtr) memorySize;
        this.AllocatedPointer = Program.VirtualMemoryManager.AllocExecutionBlock(memorySize, this.ProcessHandle);
        this.Disposed = false;
      }

      public static implicit operator IntPtr(Program.VirtualMemoryPtr virtualMemoryPointer)
      {
        return virtualMemoryPointer.AllocatedPointer;
      }

      public override bool IsInvalid
      {
        get
        {
          return this.Disposed;
        }
      }

      protected override bool ReleaseHandle()
      {
        if (!this.Disposed)
        {
          this.Disposed = true;
          GC.SuppressFinalize((object) this);
          Program.VirtualMemoryManager.VirtualFreeEx(this.ProcessHandle, this.AllocatedPointer, this.MemorySize);
        }
        return true;
      }
    }

    private class VirtualMemoryManager
    {
      [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess")]
      internal static extern IntPtr GetCurrentProcessHandle();

      [DllImport("kernel32.dll")]
      internal static extern IntPtr GetCurrentProcess();

      [DllImport("kernel32.dll", SetLastError = true)]
      private static extern IntPtr VirtualAllocEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        IntPtr dwSize,
        Program.VirtualMemoryManager.AllocationType flAllocationType,
        uint flProtect);

      [DllImport("kernel32.dll", SetLastError = true)]
      private static extern bool VirtualProtectEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        IntPtr dwSize,
        uint flNewProtect,
        ref uint lpflOldProtect);

      public static IntPtr AllocExecutionBlock(int size, IntPtr hProcess)
      {
        IntPtr lpAddress = Program.VirtualMemoryManager.VirtualAllocEx(hProcess, IntPtr.Zero, (IntPtr) size, Program.VirtualMemoryManager.AllocationType.Commit | Program.VirtualMemoryManager.AllocationType.Reserve, 64U);
        if (lpAddress == IntPtr.Zero)
          throw new Win32Exception();
        uint lpflOldProtect = 0;
        if (!Program.VirtualMemoryManager.VirtualProtectEx(hProcess, lpAddress, (IntPtr) size, 64U, ref lpflOldProtect))
          throw new Win32Exception();
        return lpAddress;
      }

      public static IntPtr AllocExecutionBlock(int size)
      {
        return Program.VirtualMemoryManager.AllocExecutionBlock(size, Program.VirtualMemoryManager.GetCurrentProcessHandle());
      }

      [DllImport("kernel32.dll", SetLastError = true)]
      private static extern bool VirtualFreeEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        IntPtr dwSize,
        IntPtr dwFreeType);

      public static bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, IntPtr dwSize)
      {
        bool flag = Program.VirtualMemoryManager.VirtualFreeEx(hProcess, lpAddress, dwSize, (IntPtr) 16384L);
        if (!flag)
          throw new Win32Exception();
        return flag;
      }

      public static bool VirtualFreeEx(IntPtr lpAddress, IntPtr dwSize)
      {
        return Program.VirtualMemoryManager.VirtualFreeEx(Program.VirtualMemoryManager.GetCurrentProcessHandle(), lpAddress, dwSize);
      }

      [System.Flags]
      private enum AllocationType : uint
      {
        Commit = 4096, // 0x00001000
        Reserve = 8192, // 0x00002000
        Reset = 524288, // 0x00080000
        Physical = 4194304, // 0x00400000
        TopDown = 1048576, // 0x00100000
      }

      [System.Flags]
      private enum ProtectionOptions : uint
      {
        Execute = 16, // 0x00000010
        PageExecuteRead = 32, // 0x00000020
        PageExecuteReadWrite = 64, // 0x00000040
      }

      [System.Flags]
      private enum MemoryFreeType : uint
      {
        Decommit = 16384, // 0x00004000
        Release = 32768, // 0x00008000
      }
    }
  }
}
