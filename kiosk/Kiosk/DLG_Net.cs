// Decompiled with JetBrains decompiler
// Type: Kiosk.DLG_Net
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using Kiosk.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Management;
using System.Windows.Forms;

namespace Kiosk
{
  public class DLG_Net : Form
  {
    private IContainer components = (IContainer) null;
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
    private Button bOk;
    private TextBox tComputer;
    private Label lComputer;
    private TextBox tNetBIOS;
    private Label lNetBIOS;

    public DLG_Net()
    {
      this.InitializeComponent();
      bool dhcp = false;
      this.tComputer.Text = Environment.MachineName;
      this.tNetBIOS.Text = DLG_Net.GetWorkGroup();
      string[] enableDHCP;
      string[] ipAdresses;
      string[] subnets;
      string[] gateways;
      string[] dnses;
      DLG_Net.GetIP(out enableDHCP, out ipAdresses, out subnets, out gateways, out dnses, out dhcp);
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
      if (this.rAutoDNS.Checked)
        ;
      if (!this.rAutoDHCP.Checked)
        ;
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
      this.Close();
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
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
      this.bOk = new Button();
      this.tComputer = new TextBox();
      this.lComputer = new Label();
      this.tNetBIOS = new TextBox();
      this.lNetBIOS = new Label();
      this.gDNS.SuspendLayout();
      this.gTCP.SuspendLayout();
      this.SuspendLayout();
      this.gDNS.Controls.Add((Control) this.tDNS2);
      this.gDNS.Controls.Add((Control) this.tDNS1);
      this.gDNS.Controls.Add((Control) this.lDNS2);
      this.gDNS.Controls.Add((Control) this.lDNS1);
      this.gDNS.Controls.Add((Control) this.rStaticDNS);
      this.gDNS.Controls.Add((Control) this.rAutoDNS);
      this.gDNS.Location = new Point(12, 213);
      this.gDNS.Name = "gDNS";
      this.gDNS.Size = new Size(393, 129);
      this.gDNS.TabIndex = 6;
      this.gDNS.TabStop = false;
      this.gDNS.Text = "DNS";
      this.tDNS2.Enabled = false;
      this.tDNS2.Location = new Point(87, 95);
      this.tDNS2.Name = "tDNS2";
      this.tDNS2.Size = new Size(133, 20);
      this.tDNS2.TabIndex = 1;
      this.tDNS1.Enabled = false;
      this.tDNS1.Location = new Point(87, 68);
      this.tDNS1.Name = "tDNS1";
      this.tDNS1.Size = new Size(133, 20);
      this.tDNS1.TabIndex = 0;
      this.lDNS2.AutoSize = true;
      this.lDNS2.Location = new Point(27, 98);
      this.lDNS2.Name = "lDNS2";
      this.lDNS2.Size = new Size(58, 13);
      this.lDNS2.TabIndex = 3;
      this.lDNS2.Text = "Secondary";
      this.lDNS1.AutoSize = true;
      this.lDNS1.Location = new Point(27, 71);
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
      this.gTCP.Location = new Point(12, 52);
      this.gTCP.Name = "gTCP";
      this.gTCP.Size = new Size(393, 155);
      this.gTCP.TabIndex = 5;
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
      this.bOk.BackgroundImage = (Image) Resources.ico_ok;
      this.bOk.BackgroundImageLayout = ImageLayout.Center;
      this.bOk.Location = new Point(341, 348);
      this.bOk.Name = "bOk";
      this.bOk.Size = new Size(64, 48);
      this.bOk.TabIndex = 2;
      this.bOk.UseVisualStyleBackColor = true;
      this.bOk.Click += new EventHandler(this.bOk_Click);
      this.tComputer.Enabled = false;
      this.tComputer.Location = new Point(75, 16);
      this.tComputer.Name = "tComputer";
      this.tComputer.Size = new Size(130, 20);
      this.tComputer.TabIndex = 0;
      this.lComputer.AutoSize = true;
      this.lComputer.Location = new Point(15, 19);
      this.lComputer.Name = "lComputer";
      this.lComputer.Size = new Size(52, 13);
      this.lComputer.TabIndex = 10;
      this.lComputer.Text = "Computer";
      this.tNetBIOS.Enabled = false;
      this.tNetBIOS.Location = new Point(279, 16);
      this.tNetBIOS.Name = "tNetBIOS";
      this.tNetBIOS.Size = new Size(126, 20);
      this.tNetBIOS.TabIndex = 1;
      this.lNetBIOS.AutoSize = true;
      this.lNetBIOS.Location = new Point(213, 19);
      this.lNetBIOS.Name = "lNetBIOS";
      this.lNetBIOS.Size = new Size(60, 13);
      this.lNetBIOS.TabIndex = 12;
      this.lNetBIOS.Text = "Workgroup";
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(417, 405);
      this.ControlBox = false;
      this.Controls.Add((Control) this.tNetBIOS);
      this.Controls.Add((Control) this.lNetBIOS);
      this.Controls.Add((Control) this.tComputer);
      this.Controls.Add((Control) this.lComputer);
      this.Controls.Add((Control) this.bOk);
      this.Controls.Add((Control) this.gDNS);
      this.Controls.Add((Control) this.gTCP);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (DLG_Net);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "System info";
      this.gDNS.ResumeLayout(false);
      this.gDNS.PerformLayout();
      this.gTCP.ResumeLayout(false);
      this.gTCP.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
