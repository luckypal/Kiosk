// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Info
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Info : Form
  {
    private IContainer components = (IContainer) null;
    public bool OK;
    public bool IsClosed;
    public Configuracion opciones;
    private Panel pBOTTOM;
    private Button bOk;
    private TextBox tNetBIOS;
    private Label lNetBIOS;
    private TextBox tComputer;
    private Label lComputer;
    private GroupBox gDNS;
    private TextBox tDNS2;
    private TextBox tDNS1;
    private Label lDNS2;
    private Label lDNS1;
    private RadioButton rStaticDNS;
    private RadioButton rAutoDNS;
    private GroupBox gTCP;
    private TextBox tGateway;
    private TextBox tMask;
    private TextBox tIP;
    private Label lGateway;
    private Label lMask;
    private Label lIP;
    private RadioButton rStaticDHCP;
    private RadioButton rAutoDHCP;
    private Label lServer;
    private TextBox iServer;
    private TextBox iUser;
    private Label lUser;
    private TextBox iSelector;
    private Label lSelector;
    private TextBox iValidator;
    private Label lValidator;
    private TextBox iPrinter;
    private Label lPrinter;
    private TextBox iEthernet;
    private Label lEthernet;
    private TextBox iCPU;
    private Label lCPU;
    private TextBox iBarcode;
    private Label lBarcode;
    private Label lSystem;
    private TextBox iSystem;
    private TextBox iWifi;
    private Label lWifi;
    private TextBox iVNC;
    private Label lVNC;

    public DLG_Info(ref Configuracion _opc)
    {
      this.OK = false;
      this.IsClosed = false;
      this.opciones = _opc;
      this.InitializeComponent();
      this.Localize();
      bool dhcp = false;
      this.tComputer.Text = Environment.MachineName;
      this.tNetBIOS.Text = DLG_Info.GetWorkGroup();
      string[] enableDHCP;
      string[] ipAdresses;
      string[] subnets;
      string[] gateways;
      string[] dnses;
      DLG_Info.GetIP(out enableDHCP, out ipAdresses, out subnets, out gateways, out dnses, out dhcp);
      if (dhcp && enableDHCP != null)
      {
        this.rAutoDHCP.Checked = true;
        this.rStaticDHCP.Checked = false;
      }
      else
      {
        this.rAutoDHCP.Checked = false;
        this.rStaticDHCP.Checked = true;
        if (ipAdresses.Length > 0)
          this.tIP.Text = ipAdresses[0];
        if (subnets.Length > 0)
          this.tMask.Text = subnets[0];
        if (gateways.Length > 0)
          this.tGateway.Text = gateways[0];
      }
      if (dhcp && enableDHCP != null)
      {
        this.rAutoDNS.Checked = true;
        this.rStaticDNS.Checked = false;
      }
      else
      {
        this.rAutoDNS.Checked = false;
        this.rStaticDNS.Checked = true;
        if (dnses.Length > 0)
          this.tDNS1.Text = dnses[0];
        if (dnses.Length > 1)
          this.tDNS2.Text = dnses[1];
      }
      this.iServer.Text = this.opciones.Srv_Ip + "," + this.opciones.Srv_port;
      this.iUser.Text = this.opciones.Srv_User;
      if (this.opciones.Dev_Coin == "-")
        this.iSelector.Text = "DISABLED";
      else
        this.iSelector.Text = this.opciones.Dev_Coin + " Port: " + this.opciones.Dev_Coin_P;
      if (this.opciones.Dev_BNV == "-")
        this.iValidator.Text = "DISABLED";
      else
        this.iValidator.Text = this.opciones.Dev_BNV + " Port: " + this.opciones.Dev_BNV_P;
      if (string.IsNullOrEmpty(this.opciones.Impresora_Tck))
        this.iPrinter.Text = "DISABLED";
      else
        this.iPrinter.Text = this.opciones.Impresora_Tck;
      if (string.IsNullOrEmpty(this.opciones.Barcode))
        this.iBarcode.Text = "DISABLED";
      else
        this.iBarcode.Text = this.opciones.Barcode;
      this.iVNC.Text = this.opciones.Server_VNC;
      this.iCPU.Text = this.opciones.CPUID;
      this.iEthernet.Text = "";
      this.iWifi.Text = "";
      NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
      for (int index = 0; index < networkInterfaces.Length; ++index)
      {
        if (networkInterfaces[index].NetworkInterfaceType == NetworkInterfaceType.Ethernet)
        {
          if (networkInterfaces[index].OperationalStatus == OperationalStatus.Up)
            this.iEthernet.Text = networkInterfaces[index].Description + "\r\nCONNECTED MAC " + (object) networkInterfaces[index].GetPhysicalAddress();
          else
            this.iEthernet.Text = networkInterfaces[index].Description + "\r\nDISCONNECTED MAC " + (object) networkInterfaces[index].GetPhysicalAddress();
        }
        if (!networkInterfaces[index].Description.ToLower().Contains("virtual") && networkInterfaces[index].NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
        {
          if (networkInterfaces[index].OperationalStatus == OperationalStatus.Up)
            this.iWifi.Text = networkInterfaces[index].Description + "\r\nCONNECTED MAC " + (object) networkInterfaces[index].GetPhysicalAddress();
          else
            this.iWifi.Text = networkInterfaces[index].Description + "\r\nDISCONNECTED MAC " + (object) networkInterfaces[index].GetPhysicalAddress();
        }
      }
      this.iSystem.Text = "Cpu: " + DLG_Info.Get_CPU() + "\r\nRAM: " + DLG_Info.Get_RAM() + "\r\nVideo: " + DLG_Info.Get_Video() + "\r\nSerials: " + DLG_Info.Get_Serials() + "\r\nFlash: " + DLG_Info.Check_Flash() + " / " + DLG_Info.Check_Flash_ActiveX() + " / " + DLG_Info.Check_Flash_Plugin() + "\r\nJava: " + DLG_Info.Check_Java() + "\r\nVNC: " + DLG_Info.Check_VNC() + "\r\nCleaner: " + DLG_Info.Check_Cleaner() + "\r\n";
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    public static string GetWorkGroup()
    {
      return new ManagementObject(string.Format("Win32_ComputerSystem.Name='{0}'", (object) Environment.MachineName))["Workgroup"].ToString();
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

    public static void SetIP(
      string IpAddresses,
      string SubnetMask,
      string Gateway,
      string DnsSearchOrder)
    {
      foreach (ManagementObject instance in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
      {
        if ((bool) instance["IPEnabled"])
        {
          ManagementBaseObject methodParameters1 = instance.GetMethodParameters("EnableStatic");
          ManagementBaseObject methodParameters2 = instance.GetMethodParameters("SetGateways");
          ManagementBaseObject methodParameters3 = instance.GetMethodParameters("SetDNSServerSearchOrder");
          methodParameters2["DefaultIPGateway"] = (object) new string[1]
          {
            Gateway
          };
          methodParameters2["GatewayCostMetric"] = (object) new int[1]
          {
            1
          };
          methodParameters1["IPAddress"] = (object) IpAddresses.Split(',');
          methodParameters1[nameof (SubnetMask)] = (object) new string[1]
          {
            SubnetMask
          };
          methodParameters3["DNSServerSearchOrder"] = (object) DnsSearchOrder.Split(',');
          instance.InvokeMethod("EnableStatic", methodParameters1, (InvokeMethodOptions) null);
          instance.InvokeMethod("SetGateways", methodParameters2, (InvokeMethodOptions) null);
          instance.InvokeMethod("SetDNSServerSearchOrder", methodParameters3, (InvokeMethodOptions) null);
          break;
        }
      }
    }

    public static void SetDHCP()
    {
      foreach (ManagementObject instance in new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
      {
        if ((bool) instance["IPEnabled"])
        {
          ManagementBaseObject methodParameters = instance.GetMethodParameters("SetDNSServerSearchOrder");
          methodParameters["DNSServerSearchOrder"] = (object) null;
          instance.InvokeMethod("EnableDHCP", (ManagementBaseObject) null, (InvokeMethodOptions) null);
          instance.InvokeMethod("SetDNSServerSearchOrder", methodParameters, (InvokeMethodOptions) null);
        }
      }
    }

    private void NewShell(string exe)
    {
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon", true);
        registryKey.SetValue("Shell", (object) exe, RegistryValueKind.String);
        registryKey.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error al modificar el programa de arranque : " + ex.Message);
      }
    }

    private void DisableDesktop(int o)
    {
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoDesktop", (object) o, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error al configurar el Desktop : " + ex.Message);
      }
    }

    public static void DisableTaskManager(int o)
    {
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        registryKey.SetValue("DisableTaskMgr", (object) o, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error al configurar el TaskManager : " + ex.Message);
      }
    }

    private void DisableRegedit(int o)
    {
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        registryKey.SetValue("DisableRegistryTools", (object) o, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch (Exception ex)
      {
        try
        {
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true))
          {
            registryKey.SetValue("DisableRegistryTools", (object) o);
            registryKey.Close();
          }
        }
        catch
        {
          int num = (int) MessageBox.Show("Error al configurar el Regedit 1/4 : " + ex.Message);
        }
      }
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true);
        registryKey.SetValue("DisableRegistryTools", (object) o, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch (Exception ex)
      {
        try
        {
          using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies", true))
          {
            registryKey.CreateSubKey("System");
            registryKey.Close();
          }
        }
        catch
        {
          int num = (int) MessageBox.Show("Error al configurar el Regedit 2/4 : " + ex.Message);
        }
        try
        {
          using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", true))
          {
            registryKey.SetValue("DisableRegistryTools", (object) o);
            registryKey.Close();
          }
        }
        catch
        {
          int num = (int) MessageBox.Show("Error al configurar el Regedit 3/4 : " + ex.Message);
        }
      }
    }

    private void DisableControlPanel(int o)
    {
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", true);
        registryKey.SetValue("NoControlPanel ", (object) o, RegistryValueKind.DWord);
        registryKey.Close();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error al configurar el Control Panel : " + ex.Message);
      }
    }

    private void bOk_Click(object sender, EventArgs e)
    {
      this.OK = true;
      this.Close();
    }

    private void FRM_Info_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.IsClosed = true;
    }

    public static string Check_Flash()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Macromedia\\FlashPlayer\\");
      if (registryKey == null)
        return (string) null;
      string str = (string) null;
      try
      {
        str = registryKey.GetValue("CurrentVersion", (object) string.Empty).ToString();
      }
      catch
      {
      }
      return "FlashPlayer v" + str;
    }

    public static string Check_Flash_ActiveX()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Macromedia\\FlashPlayerActiveX\\");
      if (registryKey == null)
        return (string) null;
      string str = (string) null;
      try
      {
        str = registryKey.GetValue("Version", (object) string.Empty).ToString();
      }
      catch
      {
      }
      return "FlashPlayerActiveX v" + str;
    }

    public static string Get_RAM()
    {
      ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
      ulong num1 = 0;
      foreach (ManagementObject managementObject in managementObjectSearcher.Get())
        num1 += Convert.ToUInt64(managementObject.Properties["Capacity"].Value);
      ulong num2 = num1 / 1073741824UL;
      if (num2 > 0UL)
        return string.Format("{0} Giga Bytes", (object) num2);
      ulong num3 = num1 / 1048576UL;
      if (num3 > 0UL)
        return string.Format("{0} Mega Bytes", (object) num3);
      return string.Format("{0} Bytes", (object) num1);
    }

    public static string Get_Video()
    {
      ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Description FROM Win32_VideoController");
      string str = "";
      foreach (ManagementObject managementObject in managementObjectSearcher.Get())
      {
        if (str != "")
          str += ", ";
        str += Convert.ToString(managementObject.Properties["Description"].Value);
      }
      return str;
    }

    public static string Get_Video_Bad_Driver()
    {
      ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Description FROM Win32_VideoController");
      string str = "";
      foreach (ManagementObject managementObject in managementObjectSearcher.Get())
      {
        if (Convert.ToString(managementObject.Properties["InfFilename"].Value).ToLower() == "display.inf".ToLower() && Convert.ToString(managementObject.Properties["InfSection"].Value).ToLower() == "vga".ToLower())
          str = Convert.ToString(managementObject.Properties["Description"].Value);
      }
      return str;
    }

    public static string Get_Serials()
    {
      string[] portNames = SerialPort.GetPortNames();
      string str = "";
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
        {
          if (str != "")
            str += ", ";
          str += portNames[index];
        }
      }
      return str;
    }

    public static string Check_Flash_Plugin()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Macromedia\\FlashPlayerPlugin\\");
      if (registryKey == null)
        return (string) null;
      string str = (string) null;
      try
      {
        str = registryKey.GetValue("Version", (object) string.Empty).ToString();
      }
      catch
      {
      }
      return "FlashPlayerPlugin v" + str;
    }

    public static string Check_Java()
    {
      RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\JavaSoft\\Java Runtime Environment");
      if (registryKey == null)
        return (string) null;
      string str = (string) null;
      try
      {
        str = registryKey.GetValue("CurrentVersion", (object) string.Empty).ToString();
      }
      catch
      {
      }
      return "Java Runtime Environment v" + str;
    }

    public static string Check_Cleaner()
    {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Piriform\\CCleaner");
      if (registryKey == null)
        return (string) null;
      string str = (string) null;
      try
      {
        str = registryKey.GetValue("NewVersion", (object) string.Empty).ToString();
      }
      catch
      {
      }
      return "Version " + str;
    }

    public static string Check_VNC()
    {
      if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe") && File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini"))
        return "OK";
      return (string) null;
    }

    public static string Get_CPU()
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("SELECT Name FROM Win32_processor").Get().GetEnumerator())
      {
        if (enumerator.MoveNext())
          empty1 = enumerator.Current["Name"].ToString();
      }
      using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_processor").Get().GetEnumerator())
      {
        if (enumerator.MoveNext())
          empty2 = enumerator.Current["ProcessorId"].ToString();
      }
      return string.Format("{0}", (object) empty1);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pBOTTOM = new Panel();
      this.bOk = new Button();
      this.tNetBIOS = new TextBox();
      this.lNetBIOS = new Label();
      this.tComputer = new TextBox();
      this.lComputer = new Label();
      this.gDNS = new GroupBox();
      this.tDNS2 = new TextBox();
      this.tDNS1 = new TextBox();
      this.lDNS2 = new Label();
      this.lDNS1 = new Label();
      this.rStaticDNS = new RadioButton();
      this.rAutoDNS = new RadioButton();
      this.gTCP = new GroupBox();
      this.tGateway = new TextBox();
      this.tMask = new TextBox();
      this.tIP = new TextBox();
      this.lGateway = new Label();
      this.lMask = new Label();
      this.lIP = new Label();
      this.rStaticDHCP = new RadioButton();
      this.rAutoDHCP = new RadioButton();
      this.lServer = new Label();
      this.iServer = new TextBox();
      this.iUser = new TextBox();
      this.lUser = new Label();
      this.iSelector = new TextBox();
      this.lSelector = new Label();
      this.iValidator = new TextBox();
      this.lValidator = new Label();
      this.iPrinter = new TextBox();
      this.lPrinter = new Label();
      this.iEthernet = new TextBox();
      this.lEthernet = new Label();
      this.iCPU = new TextBox();
      this.lCPU = new Label();
      this.iBarcode = new TextBox();
      this.lBarcode = new Label();
      this.lSystem = new Label();
      this.iSystem = new TextBox();
      this.iWifi = new TextBox();
      this.lWifi = new Label();
      this.iVNC = new TextBox();
      this.lVNC = new Label();
      this.pBOTTOM.SuspendLayout();
      this.gDNS.SuspendLayout();
      this.gTCP.SuspendLayout();
      this.SuspendLayout();
      this.pBOTTOM.Controls.Add((Control) this.bOk);
      this.pBOTTOM.Dock = DockStyle.Bottom;
      this.pBOTTOM.Location = new Point(0, 434);
      this.pBOTTOM.Name = "pBOTTOM";
      this.pBOTTOM.Size = new Size(784, 48);
      this.pBOTTOM.TabIndex = 5;
      this.bOk.Dock = DockStyle.Right;
      this.bOk.Image = (Image) Resources.ico_ok;
      this.bOk.Location = new Point(736, 0);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(48, 48);
      this.bOk.TabIndex = 0;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.tNetBIOS.Enabled = false;
      this.tNetBIOS.Location = new Point(442, 34);
      this.tNetBIOS.Name = "tNetBIOS";
      this.tNetBIOS.Size = new Size(330, 20);
      this.tNetBIOS.TabIndex = 14;
      this.lNetBIOS.AutoSize = true;
      this.lNetBIOS.Location = new Point(376, 37);
      this.lNetBIOS.Name = "lNetBIOS";
      this.lNetBIOS.Size = new Size(60, 13);
      this.lNetBIOS.TabIndex = 18;
      this.lNetBIOS.Text = "Workgroup";
      this.tComputer.Enabled = false;
      this.tComputer.Location = new Point(442, 6);
      this.tComputer.Name = "tComputer";
      this.tComputer.Size = new Size(330, 20);
      this.tComputer.TabIndex = 13;
      this.lComputer.AutoSize = true;
      this.lComputer.Location = new Point(376, 9);
      this.lComputer.Name = "lComputer";
      this.lComputer.Size = new Size(52, 13);
      this.lComputer.TabIndex = 17;
      this.lComputer.Text = "Computer";
      this.gDNS.Controls.Add((Control) this.tDNS2);
      this.gDNS.Controls.Add((Control) this.tDNS1);
      this.gDNS.Controls.Add((Control) this.lDNS2);
      this.gDNS.Controls.Add((Control) this.lDNS1);
      this.gDNS.Controls.Add((Control) this.rStaticDNS);
      this.gDNS.Controls.Add((Control) this.rAutoDNS);
      this.gDNS.Location = new Point(379, 317);
      this.gDNS.Name = "gDNS";
      this.gDNS.Size = new Size(393, 111);
      this.gDNS.TabIndex = 16;
      this.gDNS.TabStop = false;
      this.gDNS.Text = "DNS";
      this.tDNS2.Enabled = false;
      this.tDNS2.Location = new Point(133, 71);
      this.tDNS2.Name = "tDNS2";
      this.tDNS2.Size = new Size(133, 20);
      this.tDNS2.TabIndex = 1;
      this.tDNS1.Enabled = false;
      this.tDNS1.Location = new Point(133, 44);
      this.tDNS1.Name = "tDNS1";
      this.tDNS1.Size = new Size(133, 20);
      this.tDNS1.TabIndex = 0;
      this.lDNS2.AutoSize = true;
      this.lDNS2.Location = new Point(73, 74);
      this.lDNS2.Name = "lDNS2";
      this.lDNS2.Size = new Size(58, 13);
      this.lDNS2.TabIndex = 3;
      this.lDNS2.Text = "Secondary";
      this.lDNS1.AutoSize = true;
      this.lDNS1.Location = new Point(73, 47);
      this.lDNS1.Name = "lDNS1";
      this.lDNS1.Size = new Size(41, 13);
      this.lDNS1.TabIndex = 2;
      this.lDNS1.Text = "Primary";
      this.rStaticDNS.AutoSize = true;
      this.rStaticDNS.Enabled = false;
      this.rStaticDNS.Location = new Point(7, 44);
      this.rStaticDNS.Name = "rStaticDNS";
      this.rStaticDNS.Size = new Size(48, 17);
      this.rStaticDNS.TabIndex = 1;
      this.rStaticDNS.TabStop = true;
      this.rStaticDNS.Text = "DNS";
      this.rStaticDNS.UseVisualStyleBackColor = true;
      this.rAutoDNS.AutoSize = true;
      this.rAutoDNS.Enabled = false;
      this.rAutoDNS.Location = new Point(7, 20);
      this.rAutoDNS.Name = "rAutoDNS";
      this.rAutoDNS.Size = new Size(98, 17);
      this.rAutoDNS.TabIndex = 0;
      this.rAutoDNS.TabStop = true;
      this.rAutoDNS.Text = "DNS Automatic";
      this.rAutoDNS.UseVisualStyleBackColor = true;
      this.gTCP.Controls.Add((Control) this.tGateway);
      this.gTCP.Controls.Add((Control) this.tMask);
      this.gTCP.Controls.Add((Control) this.tIP);
      this.gTCP.Controls.Add((Control) this.lGateway);
      this.gTCP.Controls.Add((Control) this.lMask);
      this.gTCP.Controls.Add((Control) this.lIP);
      this.gTCP.Controls.Add((Control) this.rStaticDHCP);
      this.gTCP.Controls.Add((Control) this.rAutoDHCP);
      this.gTCP.Location = new Point(379, 164);
      this.gTCP.Name = "gTCP";
      this.gTCP.Size = new Size(393, 150);
      this.gTCP.TabIndex = 15;
      this.gTCP.TabStop = false;
      this.gTCP.Text = "TCP";
      this.tGateway.Enabled = false;
      this.tGateway.Location = new Point(87, 122);
      this.tGateway.Name = "tGateway";
      this.tGateway.Size = new Size(133, 20);
      this.tGateway.TabIndex = 2;
      this.tMask.Enabled = false;
      this.tMask.Location = new Point(87, 95);
      this.tMask.Name = "tMask";
      this.tMask.Size = new Size(133, 20);
      this.tMask.TabIndex = 1;
      this.tIP.Enabled = false;
      this.tIP.Location = new Point(87, 68);
      this.tIP.Name = "tIP";
      this.tIP.Size = new Size(133, 20);
      this.tIP.TabIndex = 0;
      this.lGateway.AutoSize = true;
      this.lGateway.Location = new Point(27, 125);
      this.lGateway.Name = "lGateway";
      this.lGateway.Size = new Size(49, 13);
      this.lGateway.TabIndex = 4;
      this.lGateway.Text = "Gateway";
      this.lMask.AutoSize = true;
      this.lMask.Location = new Point(27, 98);
      this.lMask.Name = "lMask";
      this.lMask.Size = new Size(33, 13);
      this.lMask.TabIndex = 3;
      this.lMask.Text = "Mask";
      this.lIP.AutoSize = true;
      this.lIP.Location = new Point(27, 71);
      this.lIP.Name = "lIP";
      this.lIP.Size = new Size(17, 13);
      this.lIP.TabIndex = 2;
      this.lIP.Text = "IP";
      this.rStaticDHCP.AutoSize = true;
      this.rStaticDHCP.Enabled = false;
      this.rStaticDHCP.Location = new Point(7, 44);
      this.rStaticDHCP.Name = "rStaticDHCP";
      this.rStaticDHCP.Size = new Size(65, 17);
      this.rStaticDHCP.TabIndex = 1;
      this.rStaticDHCP.TabStop = true;
      this.rStaticDHCP.Text = "IP Static";
      this.rStaticDHCP.UseVisualStyleBackColor = true;
      this.rAutoDHCP.AutoSize = true;
      this.rAutoDHCP.Enabled = false;
      this.rAutoDHCP.Location = new Point(7, 20);
      this.rAutoDHCP.Name = "rAutoDHCP";
      this.rAutoDHCP.Size = new Size(76, 17);
      this.rAutoDHCP.TabIndex = 0;
      this.rAutoDHCP.TabStop = true;
      this.rAutoDHCP.Text = "IP Dinamic";
      this.rAutoDHCP.UseVisualStyleBackColor = true;
      this.lServer.AutoSize = true;
      this.lServer.Location = new Point(12, 16);
      this.lServer.Name = "lServer";
      this.lServer.Size = new Size(38, 13);
      this.lServer.TabIndex = 19;
      this.lServer.Text = "Server";
      this.iServer.Enabled = false;
      this.iServer.Location = new Point(70, 13);
      this.iServer.Name = "iServer";
      this.iServer.Size = new Size(288, 20);
      this.iServer.TabIndex = 20;
      this.iUser.Enabled = false;
      this.iUser.Location = new Point(70, 41);
      this.iUser.Name = "iUser";
      this.iUser.Size = new Size(288, 20);
      this.iUser.TabIndex = 22;
      this.lUser.AutoSize = true;
      this.lUser.Location = new Point(12, 44);
      this.lUser.Name = "lUser";
      this.lUser.Size = new Size(29, 13);
      this.lUser.TabIndex = 21;
      this.lUser.Text = "User";
      this.iSelector.Enabled = false;
      this.iSelector.Location = new Point(70, 69);
      this.iSelector.Name = "iSelector";
      this.iSelector.Size = new Size(288, 20);
      this.iSelector.TabIndex = 24;
      this.lSelector.AutoSize = true;
      this.lSelector.Location = new Point(12, 72);
      this.lSelector.Name = "lSelector";
      this.lSelector.Size = new Size(46, 13);
      this.lSelector.TabIndex = 23;
      this.lSelector.Text = "Selector";
      this.iValidator.Enabled = false;
      this.iValidator.Location = new Point(70, 97);
      this.iValidator.Name = "iValidator";
      this.iValidator.Size = new Size(288, 20);
      this.iValidator.TabIndex = 26;
      this.lValidator.AutoSize = true;
      this.lValidator.Location = new Point(12, 100);
      this.lValidator.Name = "lValidator";
      this.lValidator.Size = new Size(48, 13);
      this.lValidator.TabIndex = 25;
      this.lValidator.Text = "Validator";
      this.iPrinter.Enabled = false;
      this.iPrinter.Location = new Point(70, 125);
      this.iPrinter.Name = "iPrinter";
      this.iPrinter.Size = new Size(288, 20);
      this.iPrinter.TabIndex = 28;
      this.lPrinter.AutoSize = true;
      this.lPrinter.Location = new Point(12, 128);
      this.lPrinter.Name = "lPrinter";
      this.lPrinter.Size = new Size(37, 13);
      this.lPrinter.TabIndex = 27;
      this.lPrinter.Text = "Printer";
      this.iEthernet.Enabled = false;
      this.iEthernet.Location = new Point(442, 90);
      this.iEthernet.Multiline = true;
      this.iEthernet.Name = "iEthernet";
      this.iEthernet.Size = new Size(330, 36);
      this.iEthernet.TabIndex = 31;
      this.lEthernet.AutoSize = true;
      this.lEthernet.Location = new Point(376, 93);
      this.lEthernet.Name = "lEthernet";
      this.lEthernet.Size = new Size(47, 13);
      this.lEthernet.TabIndex = 32;
      this.lEthernet.Text = "Ethernet";
      this.iCPU.Enabled = false;
      this.iCPU.Location = new Point(442, 62);
      this.iCPU.Name = "iCPU";
      this.iCPU.Size = new Size(330, 20);
      this.iCPU.TabIndex = 33;
      this.lCPU.AutoSize = true;
      this.lCPU.Location = new Point(376, 65);
      this.lCPU.Name = "lCPU";
      this.lCPU.Size = new Size(18, 13);
      this.lCPU.TabIndex = 34;
      this.lCPU.Text = "ID";
      this.iBarcode.Enabled = false;
      this.iBarcode.Location = new Point(70, 153);
      this.iBarcode.Name = "iBarcode";
      this.iBarcode.Size = new Size(288, 20);
      this.iBarcode.TabIndex = 36;
      this.lBarcode.AutoSize = true;
      this.lBarcode.Location = new Point(12, 156);
      this.lBarcode.Name = "lBarcode";
      this.lBarcode.Size = new Size(47, 13);
      this.lBarcode.TabIndex = 35;
      this.lBarcode.Text = "Barcode";
      this.lSystem.AutoSize = true;
      this.lSystem.Location = new Point(12, 210);
      this.lSystem.Name = "lSystem";
      this.lSystem.Size = new Size(41, 13);
      this.lSystem.TabIndex = 37;
      this.lSystem.Text = "System";
      this.iSystem.Enabled = false;
      this.iSystem.Location = new Point(15, 230);
      this.iSystem.Multiline = true;
      this.iSystem.Name = "iSystem";
      this.iSystem.Size = new Size(343, 198);
      this.iSystem.TabIndex = 38;
      this.iWifi.Enabled = false;
      this.iWifi.Location = new Point(442, 130);
      this.iWifi.Multiline = true;
      this.iWifi.Name = "iWifi";
      this.iWifi.Size = new Size(330, 36);
      this.iWifi.TabIndex = 39;
      this.lWifi.AutoSize = true;
      this.lWifi.Location = new Point(376, 133);
      this.lWifi.Name = "lWifi";
      this.lWifi.Size = new Size(25, 13);
      this.lWifi.TabIndex = 40;
      this.lWifi.Text = "Wifi";
      this.iVNC.Enabled = false;
      this.iVNC.Location = new Point(70, 181);
      this.iVNC.Name = "iVNC";
      this.iVNC.Size = new Size(288, 20);
      this.iVNC.TabIndex = 42;
      this.lVNC.AutoSize = true;
      this.lVNC.Location = new Point(12, 184);
      this.lVNC.Name = "lVNC";
      this.lVNC.Size = new Size(29, 13);
      this.lVNC.TabIndex = 41;
      this.lVNC.Text = "VNC";
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(784, 482);
      this.ControlBox = false;
      this.Controls.Add((Control) this.iVNC);
      this.Controls.Add((Control) this.lVNC);
      this.Controls.Add((Control) this.iWifi);
      this.Controls.Add((Control) this.lWifi);
      this.Controls.Add((Control) this.iSystem);
      this.Controls.Add((Control) this.lSystem);
      this.Controls.Add((Control) this.iBarcode);
      this.Controls.Add((Control) this.lBarcode);
      this.Controls.Add((Control) this.iCPU);
      this.Controls.Add((Control) this.lCPU);
      this.Controls.Add((Control) this.iEthernet);
      this.Controls.Add((Control) this.lEthernet);
      this.Controls.Add((Control) this.iPrinter);
      this.Controls.Add((Control) this.lPrinter);
      this.Controls.Add((Control) this.iValidator);
      this.Controls.Add((Control) this.lValidator);
      this.Controls.Add((Control) this.iSelector);
      this.Controls.Add((Control) this.lSelector);
      this.Controls.Add((Control) this.iUser);
      this.Controls.Add((Control) this.lUser);
      this.Controls.Add((Control) this.iServer);
      this.Controls.Add((Control) this.lServer);
      this.Controls.Add((Control) this.tNetBIOS);
      this.Controls.Add((Control) this.lNetBIOS);
      this.Controls.Add((Control) this.tComputer);
      this.Controls.Add((Control) this.lComputer);
      this.Controls.Add((Control) this.gDNS);
      this.Controls.Add((Control) this.gTCP);
      this.Controls.Add((Control) this.pBOTTOM);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Info);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = " ";
      this.FormClosed += new FormClosedEventHandler(this.FRM_Info_FormClosed);
      this.pBOTTOM.ResumeLayout(false);
      this.gDNS.ResumeLayout(false);
      this.gDNS.PerformLayout();
      this.gTCP.ResumeLayout(false);
      this.gTCP.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
