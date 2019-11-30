// Decompiled with JetBrains decompiler
// Type: Kiosk.Devices_Wizard
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

using GLib.Devices;
using GLib.Forms;
using Kiosk.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Kiosk
{
  public class Devices_Wizard : Form
  {
    private bool run = false;
    private bool enint = false;
    private int Canales = 16;
    public string SSP_Com = "?";
    public string SSP3_Com = "?";
    public string SIO_Com = "?";
    public string Tri_Com = "?";
    public string F40_Com = "?";
    public string RM5_Com = "?";
    public string CCT2_Com = "?";
    private Control_Comestero rm5 = (Control_Comestero) null;
    private Control_CCTALK_COIN cct2 = (Control_CCTALK_COIN) null;
    private Control_NV_SSP ssp = (Control_NV_SSP) null;
    private Control_NV_SSP_P6 ssp3 = (Control_NV_SSP_P6) null;
    private Control_NV_SIO sio = (Control_NV_SIO) null;
    private Control_F40_CCTalk f40 = (Control_F40_CCTalk) null;
    private Control_Trilogy tri = (Control_Trilogy) null;
    private Devices_Wizard.Wizard Fase = Devices_Wizard.Wizard.Nulo;
    public bool CoinDetected = false;
    public string CoinModel = "?";
    public string CoinModel_P = "?";
    public bool BillDetected = false;
    public string BillModel = "?";
    public string BillModel_P = "?";
    private IContainer components = (IContainer) null;
    public bool OK;
    public Configuracion opciones;
    public int[] SSP_Value;
    public int[] SSP_Inhibit;
    public int[] SSP_Enabled;
    public int[] SSP3_Value;
    public int[] SSP3_Inhibit;
    public int[] SSP3_Enabled;
    public int[] SIO_Value;
    public int[] SIO_Inhibit;
    public int[] SIO_Enabled;
    public int[] Tri_Value;
    public int[] Tri_Inhibit;
    public int[] Tri_Enabled;
    public int[] F40_Value;
    public int[] F40_Inhibit;
    public int[] F40_Enabled;
    public int[] RM5_Value;
    public int[] RM5_Inhibit;
    public int[] RM5_Enabled;
    public int[] CCT2_Value;
    public int[] CCT2_Inhibit;
    public int[] CCT2_Enabled;
    private string error;
    private Panel pBotons;
    private Button bCancel;
    private Button bSig;
    private Button bAnt;
    private Button bOK;
    private TabControl tabs;
    private TabPage tInfo;
    private TabPage tCoin_Mode;
    private TabPage tBill_Mode;
    private TabPage tCoin_Detect;
    private TextBox infoWizard;
    private GradientPanel hCoin_Mode;
    private RadioButton rCoin_M3;
    private RadioButton rCoin_M1;
    private RadioButton rCoin_M2;
    private GradientPanel hInfo;
    private CheckBox dRM5;
    private GradientPanel hCoin_Detect;
    private RadioButton rBill_M3;
    private RadioButton rBill_M1;
    private RadioButton rBill_M2;
    private GradientPanel pBillMode;
    private BackgroundWorker bgControl;
    private TabPage tBill_Detect;
    private CheckBox dTrilogy;
    private CheckBox dF40;
    private CheckBox dNV9_SSP;
    private CheckBox dNV9_SSP3;
    private GradientPanel pBillDetect;
    private TabPage tBill_Config;
    private TabPage tResum;
    private Label iCom;
    private ComboBox lCom;
    private GradientPanel pTitle;
    private ProgressBar pCoin;
    private ProgressBar pBill;
    private CheckBox dNV_SIO;
    private CheckBox eCANAL_16;
    private CheckBox eCANAL_15;
    private CheckBox eCANAL_14;
    private CheckBox eCANAL_13;
    private CheckBox eCANAL_12;
    private CheckBox eCANAL_11;
    private CheckBox eCANAL_10;
    private CheckBox eCANAL_9;
    private CheckBox eCANAL_8;
    private CheckBox eCANAL_7;
    private CheckBox eCANAL_6;
    private CheckBox eCANAL_5;
    private CheckBox eCANAL_4;
    private CheckBox eCANAL_3;
    private CheckBox eCANAL_2;
    private CheckBox eCANAL_1;
    private CheckBox dCCT2;

    public Devices_Wizard(ref Configuracion _opc)
    {
      this.OK = false;
      this.run = false;
      this.enint = false;
      this.Fase = Devices_Wizard.Wizard.Nulo;
      this.InitializeComponent();
      this.Fase = Devices_Wizard.Wizard.StartUp;
      this.Init_Vars();
      this.Load_Cfg("devices.cfg");
      this.Localize();
      this.opciones = _opc;
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    public Devices_Wizard(string _cfg)
    {
      this.Init_Vars();
      this.Load_Cfg(_cfg);
    }

    private void Init_Vars()
    {
      this.SSP_Com = "?";
      this.SSP_Value = new int[this.Canales];
      this.SSP_Inhibit = new int[this.Canales];
      this.SSP_Enabled = new int[this.Canales];
      this.SSP3_Com = "?";
      this.SSP3_Value = new int[this.Canales];
      this.SSP3_Inhibit = new int[this.Canales];
      this.SSP3_Enabled = new int[this.Canales];
      this.SIO_Com = "?";
      this.SIO_Value = new int[this.Canales];
      this.SIO_Inhibit = new int[this.Canales];
      this.SIO_Enabled = new int[this.Canales];
      this.Tri_Com = "?";
      this.Tri_Value = new int[this.Canales];
      this.Tri_Inhibit = new int[this.Canales];
      this.Tri_Enabled = new int[this.Canales];
      this.F40_Com = "?";
      this.F40_Value = new int[this.Canales];
      this.F40_Inhibit = new int[this.Canales];
      this.F40_Enabled = new int[this.Canales];
      this.RM5_Com = "?";
      this.RM5_Value = new int[this.Canales];
      this.RM5_Inhibit = new int[this.Canales];
      this.RM5_Enabled = new int[this.Canales];
      this.CCT2_Com = "?";
      this.CCT2_Value = new int[this.Canales];
      this.CCT2_Inhibit = new int[this.Canales];
      this.CCT2_Enabled = new int[this.Canales];
      this.CoinModel = "?";
      this.CoinModel_P = "?";
      this.BillModel = "?";
      this.BillModel_P = "?";
    }

    private void Refresh_SSP()
    {
      this.pTitle.Title = "Configure NV SSP";
      string[] portNames = SerialPort.GetPortNames();
      this.lCom.Items.Clear();
      this.lCom.Items.Add((object) "?");
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
          this.lCom.Items.Add((object) portNames[index]);
      }
      this.lCom.Text = this.SSP_Com;
      if (this.SSP_Com != "?")
      {
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          controlArray[0].Visible = true;
          ((CheckBox) controlArray[0]).ThreeState = false;
          ((CheckBox) controlArray[0]).CheckState = CheckState.Unchecked;
          controlArray[0].Text = "0.0";
        }
      }
      else
      {
        if (this.ssp == null)
        {
          this.ssp = new Control_NV_SSP();
          this.ssp.port = this.SSP_Com;
          this.ssp.Open();
        }
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          if (index >= this.ssp.Canales)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else
          {
            if (!this.ssp.GetChannelEnabled(index + 1))
            {
              ((CheckBox) controlArray[0]).ThreeState = true;
              ((CheckBox) controlArray[0]).CheckState = CheckState.Indeterminate;
            }
            else
            {
              ((CheckBox) controlArray[0]).ThreeState = false;
              ((CheckBox) controlArray[0]).CheckState = !this.ssp.GetChannelInhibit(index + 1) ? CheckState.Checked : CheckState.Unchecked;
            }
            if (this.ssp.GetChannelValue(index + 1) < this.ssp.Multiplicador)
              controlArray[0].Text = "0." + (object) this.ssp.GetChannelValue(index + 1) + " " + this.ssp.GetChannelCurrency(index + 1);
            else
              controlArray[0].Text = (this.ssp.GetChannelValue(index + 1) / this.ssp.Multiplicador).ToString() + " " + this.ssp.GetChannelCurrency(index + 1);
            controlArray[0].Visible = true;
            controlArray[0].Invalidate();
          }
        }
      }
    }

    private void Refresh_SSP3()
    {
      this.pTitle.Title = "Configure NV SSP v3";
      string[] portNames = SerialPort.GetPortNames();
      this.lCom.Items.Clear();
      this.lCom.Items.Add((object) "?");
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
          this.lCom.Items.Add((object) portNames[index]);
      }
      this.lCom.Text = this.SSP3_Com;
      if (this.SSP3_Com != "?")
      {
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          controlArray[0].Visible = true;
          ((CheckBox) controlArray[0]).ThreeState = false;
          ((CheckBox) controlArray[0]).CheckState = CheckState.Unchecked;
          controlArray[0].Text = "0.0";
        }
      }
      else
      {
        if (this.ssp3 == null)
        {
          this.ssp3 = new Control_NV_SSP_P6();
          this.ssp3.port = this.SSP_Com;
          this.ssp3.Open();
        }
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          if (index >= this.ssp3.Canales)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else
          {
            if (!this.ssp3.GetChannelEnabled(index + 1))
            {
              ((CheckBox) controlArray[0]).ThreeState = true;
              ((CheckBox) controlArray[0]).CheckState = CheckState.Indeterminate;
            }
            else
            {
              ((CheckBox) controlArray[0]).ThreeState = false;
              ((CheckBox) controlArray[0]).CheckState = !this.ssp3.GetChannelInhibit(index + 1) ? CheckState.Checked : CheckState.Unchecked;
            }
            if (this.ssp3.GetChannelValue(index + 1) < (Decimal) this.ssp3.Multiplier)
              controlArray[0].Text = "0." + (object) this.ssp3.GetChannelValue(index + 1) + " " + this.ssp3.GetChannelCurrency(index + 1);
            else
              controlArray[0].Text = (this.ssp3.GetChannelValue(index + 1) / (Decimal) this.ssp3.Multiplier).ToString() + " " + this.ssp3.GetChannelCurrency(index + 1);
            controlArray[0].Visible = true;
            controlArray[0].Invalidate();
          }
        }
      }
    }

    private void Refresh_F40()
    {
      this.pTitle.Title = "Configure PayPrint F40 CCTalk";
      string[] portNames = SerialPort.GetPortNames();
      this.lCom.Items.Clear();
      this.lCom.Items.Add((object) "?");
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
          this.lCom.Items.Add((object) portNames[index]);
      }
      this.lCom.Text = this.F40_Com;
      if (this.F40_Com != "?")
      {
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          controlArray[0].Visible = true;
          ((CheckBox) controlArray[0]).ThreeState = false;
          ((CheckBox) controlArray[0]).CheckState = CheckState.Unchecked;
          controlArray[0].Text = "0.0";
        }
      }
      else
      {
        if (this.f40 == null)
        {
          this.f40 = new Control_F40_CCTalk();
          this.f40.port = this.F40_Com;
          this.f40.Open();
        }
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          if (index >= this.f40.Canales)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else if (this.f40.Canal[index] == 0)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else
          {
            ((CheckBox) controlArray[0]).ThreeState = false;
            ((CheckBox) controlArray[0]).CheckState = this.f40.eCanal[index] == 1 ? CheckState.Checked : CheckState.Unchecked;
            controlArray[0].Text = !((Decimal) this.f40.Canal[index] < this.f40.Base) ? string.Concat((object) this.f40.Canal[index]) : "0." + (object) this.f40.Canal[index];
            controlArray[0].Visible = true;
            controlArray[0].Invalidate();
          }
        }
      }
    }

    private void Refresh_Trilogy()
    {
      this.pTitle.Title = "Configure Pyramid Trilogy";
      string[] portNames = SerialPort.GetPortNames();
      this.lCom.Items.Clear();
      this.lCom.Items.Add((object) "?");
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
          this.lCom.Items.Add((object) portNames[index]);
      }
      this.lCom.Text = this.Tri_Com;
      if (this.Tri_Com != "?")
      {
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          controlArray[0].Visible = true;
          ((CheckBox) controlArray[0]).ThreeState = false;
          ((CheckBox) controlArray[0]).CheckState = CheckState.Unchecked;
          controlArray[0].Text = "0.0";
        }
      }
      else
      {
        if (this.tri == null)
        {
          this.tri = new Control_Trilogy();
          this.tri.port = this.Tri_Com;
        }
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          if (index >= this.tri.Canales)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else if (this.tri.Canal[index] == 0)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else
          {
            ((CheckBox) controlArray[0]).ThreeState = false;
            ((CheckBox) controlArray[0]).CheckState = this.tri.eCanal[index] == 1 ? CheckState.Checked : CheckState.Unchecked;
            controlArray[0].Text = !((Decimal) this.tri.Canal[index] < this.tri.Base) ? string.Concat((object) this.tri.Canal[index]) : "0." + (object) this.tri.Canal[index];
            controlArray[0].Visible = true;
            controlArray[0].Invalidate();
          }
        }
      }
    }

    private void Refresh_SIO()
    {
      this.pTitle.Title = "Configure NV SIO";
      string[] portNames = SerialPort.GetPortNames();
      this.lCom.Items.Clear();
      this.lCom.Items.Add((object) "?");
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
          this.lCom.Items.Add((object) portNames[index]);
      }
      this.lCom.Text = this.SIO_Com;
      if (this.SIO_Com != "?")
      {
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          controlArray[0].Visible = true;
          ((CheckBox) controlArray[0]).ThreeState = false;
          ((CheckBox) controlArray[0]).CheckState = CheckState.Unchecked;
          controlArray[0].Text = "0.0";
        }
      }
      else
      {
        if (this.sio == null)
        {
          this.sio = new Control_NV_SIO();
          this.sio.port = this.SIO_Com;
          this.sio.Open();
        }
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          if (index >= this.sio.Canales)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else if (this.sio.Canal[index] == 0)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else
          {
            ((CheckBox) controlArray[0]).ThreeState = false;
            ((CheckBox) controlArray[0]).CheckState = this.sio.eCanal[index] == 1 ? CheckState.Checked : CheckState.Unchecked;
            controlArray[0].Text = !((Decimal) this.sio.Canal[index] < this.sio.Base) ? string.Concat((object) this.sio.Canal[index]) : "0." + (object) this.sio.Canal[index];
            controlArray[0].Visible = true;
            controlArray[0].Invalidate();
          }
        }
      }
    }

    private void Refresh_RM5()
    {
      this.pTitle.Title = "Configure Comestero RM5";
      string[] portNames = SerialPort.GetPortNames();
      this.lCom.Items.Clear();
      this.lCom.Items.Add((object) "?");
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
          this.lCom.Items.Add((object) portNames[index]);
      }
      this.lCom.Text = this.RM5_Com;
      if (this.RM5_Com != "?")
      {
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          controlArray[0].Visible = true;
          ((CheckBox) controlArray[0]).ThreeState = false;
          ((CheckBox) controlArray[0]).CheckState = CheckState.Unchecked;
          controlArray[0].Text = "0.0";
        }
      }
      else
      {
        if (this.rm5 == null)
        {
          this.rm5 = new Control_Comestero();
          this.rm5.port = this.RM5_Com;
          this.rm5.Open();
        }
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          if (index >= this.rm5.Canales)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else if (this.rm5.Canal[index] == 0)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else
          {
            ((CheckBox) controlArray[0]).ThreeState = false;
            ((CheckBox) controlArray[0]).CheckState = this.rm5.eCanal[index] == 1 ? CheckState.Checked : CheckState.Unchecked;
            controlArray[0].Text = !((Decimal) this.rm5.Canal[index] < this.rm5.Base) ? string.Concat((object) this.rm5.Canal[index]) : "0." + (object) this.rm5.Canal[index];
            controlArray[0].Visible = true;
            controlArray[0].Invalidate();
          }
        }
      }
    }

    private void Refresh_CCT2()
    {
      this.pTitle.Title = "Configure CCTalk COIN ACCEPTOR (ID 2)";
      string[] portNames = SerialPort.GetPortNames();
      this.lCom.Items.Clear();
      this.lCom.Items.Add((object) "?");
      if (portNames != null)
      {
        for (int index = 0; index < portNames.Length; ++index)
          this.lCom.Items.Add((object) portNames[index]);
      }
      this.lCom.Text = this.CCT2_Com;
      if (this.CCT2_Com != "?")
      {
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          controlArray[0].Visible = true;
          ((CheckBox) controlArray[0]).ThreeState = false;
          ((CheckBox) controlArray[0]).CheckState = CheckState.Unchecked;
          controlArray[0].Text = "0.0";
        }
      }
      else
      {
        if (this.cct2 == null)
        {
          this.cct2 = new Control_CCTALK_COIN();
          this.cct2.port = this.CCT2_Com;
          this.cct2.Open();
        }
        for (int index = 0; index < 16; ++index)
        {
          Control[] controlArray = this.tBill_Config.Controls.Find("eCANAL_" + (object) (index + 1), false);
          if (index >= this.cct2.Canales)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else if (this.cct2.Canal[index] == 0)
          {
            controlArray[0].Visible = false;
            controlArray[0].Invalidate();
          }
          else
          {
            ((CheckBox) controlArray[0]).ThreeState = false;
            ((CheckBox) controlArray[0]).CheckState = this.cct2.eCanal[index] == 1 ? CheckState.Checked : CheckState.Unchecked;
            controlArray[0].Text = this.cct2.Canal[index] >= this.cct2.Base ? string.Concat((object) this.cct2.Canal[index]) : "0." + (object) this.cct2.Canal[index];
            controlArray[0].Visible = true;
            controlArray[0].Invalidate();
          }
        }
      }
    }

    private void SetFase(Devices_Wizard.Wizard _f)
    {
      switch (_f)
      {
        case Devices_Wizard.Wizard.Nulo:
        case Devices_Wizard.Wizard.StartUp:
        case Devices_Wizard.Wizard.CoinDetecting:
        case Devices_Wizard.Wizard.BillDetecting:
          this.bAnt.Enabled = false;
          this.bSig.Enabled = false;
          this.bOK.Enabled = false;
          break;
        case Devices_Wizard.Wizard.Info:
          this.bAnt.Enabled = false;
          this.bSig.Enabled = true;
          this.bOK.Enabled = false;
          break;
        case Devices_Wizard.Wizard.CoinMode:
        case Devices_Wizard.Wizard.CoinDetected:
        case Devices_Wizard.Wizard.BillMode:
        case Devices_Wizard.Wizard.BillDetected:
        case Devices_Wizard.Wizard.RM5Config:
        case Devices_Wizard.Wizard.CCT2Config:
        case Devices_Wizard.Wizard.SIOConfig:
        case Devices_Wizard.Wizard.SSPConfig:
        case Devices_Wizard.Wizard.SSP3Config:
        case Devices_Wizard.Wizard.F40Config:
        case Devices_Wizard.Wizard.TriConfig:
          this.bAnt.Enabled = true;
          this.bSig.Enabled = true;
          this.bOK.Enabled = false;
          break;
        case Devices_Wizard.Wizard.Resumen:
          this.bAnt.Enabled = true;
          this.bSig.Enabled = false;
          this.bOK.Enabled = true;
          break;
      }
      switch (_f)
      {
        case Devices_Wizard.Wizard.Info:
          this.tabs.SelectTab("tInfo");
          break;
        case Devices_Wizard.Wizard.CoinMode:
          this.tabs.SelectTab("tCoin_Mode");
          break;
        case Devices_Wizard.Wizard.CoinDetecting:
          this.rCoin_M1.Checked = false;
          this.rCoin_M2.Checked = false;
          this.rCoin_M3.Checked = false;
          this.tabs.SelectTab("tCoin_Detect");
          break;
        case Devices_Wizard.Wizard.CoinDetected:
          this.tabs.SelectTab("tCoin_Detect");
          break;
        case Devices_Wizard.Wizard.BillMode:
          this.tabs.SelectTab("tBill_Mode");
          break;
        case Devices_Wizard.Wizard.BillDetecting:
          this.rBill_M1.Checked = false;
          this.rBill_M2.Checked = false;
          this.rBill_M3.Checked = false;
          this.tabs.SelectTab("tBill_Detect");
          break;
        case Devices_Wizard.Wizard.BillDetected:
          this.pBill.Visible = false;
          this.tabs.SelectTab("tBill_Detect");
          break;
        case Devices_Wizard.Wizard.RM5Config:
          this.Refresh_RM5();
          this.tabs.SelectTab("tBill_Config");
          break;
        case Devices_Wizard.Wizard.CCT2Config:
          this.Refresh_CCT2();
          this.tabs.SelectTab("tBill_Config");
          break;
        case Devices_Wizard.Wizard.SIOConfig:
          this.Refresh_SIO();
          this.tabs.SelectTab("tBill_Config");
          break;
        case Devices_Wizard.Wizard.SSPConfig:
          this.Refresh_SSP();
          this.tabs.SelectTab("tBill_Config");
          break;
        case Devices_Wizard.Wizard.SSP3Config:
          this.Refresh_SSP3();
          this.tabs.SelectTab("tBill_Config");
          break;
        case Devices_Wizard.Wizard.F40Config:
          this.Refresh_F40();
          this.tabs.SelectTab("tBill_Config");
          break;
        case Devices_Wizard.Wizard.TriConfig:
          this.Refresh_Trilogy();
          this.tabs.SelectTab("tBill_Config");
          break;
        case Devices_Wizard.Wizard.Resumen:
          this.tabs.SelectTab("tResum");
          break;
      }
      this.Fase = _f;
    }

    private void bSig_Click(object sender, EventArgs e)
    {
      switch (this.Fase)
      {
        case Devices_Wizard.Wizard.Info:
          this.SetFase(Devices_Wizard.Wizard.CoinMode);
          break;
        case Devices_Wizard.Wizard.CoinMode:
          if (this.rCoin_M1.Checked)
            this.SetFase(Devices_Wizard.Wizard.CoinDetecting);
          if (this.rCoin_M2.Checked)
          {
            this.CoinModel = "-";
            this.SetFase(Devices_Wizard.Wizard.BillMode);
          }
          if (!this.rCoin_M3.Checked)
            break;
          this.SetFase(Devices_Wizard.Wizard.CCT2Config);
          break;
        case Devices_Wizard.Wizard.CoinDetected:
          if (this.CoinDetected)
          {
            switch (this.CoinModel.ToLower())
            {
              case "rm5":
                this.SetFase(Devices_Wizard.Wizard.RM5Config);
                return;
              case "cct2":
                this.SetFase(Devices_Wizard.Wizard.CCT2Config);
                return;
              default:
                this.SetFase(Devices_Wizard.Wizard.BillMode);
                return;
            }
          }
          else
          {
            this.SetFase(Devices_Wizard.Wizard.BillMode);
            break;
          }
        case Devices_Wizard.Wizard.BillMode:
          if (this.rBill_M1.Checked)
            this.SetFase(Devices_Wizard.Wizard.BillDetecting);
          if (this.rBill_M2.Checked)
          {
            this.BillModel = "-";
            this.SetFase(Devices_Wizard.Wizard.Resumen);
          }
          if (!this.rBill_M3.Checked)
            break;
          this.SetFase(Devices_Wizard.Wizard.SSPConfig);
          break;
        case Devices_Wizard.Wizard.BillDetected:
          this.pBill.Visible = false;
          if (this.BillDetected)
          {
            switch (this.BillModel)
            {
              case "ssp":
                if (this.ssp == null)
                {
                  this.ssp = new Control_NV_SSP();
                  this.ssp.Start_Find_Device();
                  Thread.Sleep(100);
                  this.ssp.Poll();
                  Thread.Sleep(100);
                  this.ssp.Poll();
                  Thread.Sleep(100);
                }
                this.SetFase(Devices_Wizard.Wizard.SSPConfig);
                return;
              case "ssp3":
                if (this.ssp3 == null)
                {
                  this.ssp3 = new Control_NV_SSP_P6();
                  this.ssp3.Start_Find_Device();
                  Thread.Sleep(100);
                  this.ssp3.Poll();
                  Thread.Sleep(100);
                  this.ssp3.Poll();
                  Thread.Sleep(100);
                }
                this.SetFase(Devices_Wizard.Wizard.SSP3Config);
                return;
              case "f40":
                this.SetFase(Devices_Wizard.Wizard.F40Config);
                return;
              case "sio":
                this.SetFase(Devices_Wizard.Wizard.SIOConfig);
                return;
              case "tri":
                this.SetFase(Devices_Wizard.Wizard.TriConfig);
                return;
              default:
                this.SetFase(Devices_Wizard.Wizard.Resumen);
                return;
            }
          }
          else
          {
            this.SetFase(Devices_Wizard.Wizard.Resumen);
            break;
          }
        case Devices_Wizard.Wizard.RM5Config:
          if (this.RM5_Com != "-" && this.RM5_Com != "?")
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              if (index < this.rm5.Canales)
              {
                this.RM5_Enabled[index] = this.rm5.GetChannelEnabled(index + 1) ? 1 : 0;
                this.RM5_Inhibit[index] = this.rm5.GetChannelInhibit(index + 1) ? 0 : 1;
                this.RM5_Value[index] = this.rm5.GetChannelValue(index + 1);
              }
              else
                this.RM5_Enabled[index] = this.RM5_Inhibit[index] = this.RM5_Value[index] = 0;
            }
            this.CoinModel = "rm5";
            this.CoinModel_P = this.RM5_Com;
          }
          this.SetFase(Devices_Wizard.Wizard.BillMode);
          break;
        case Devices_Wizard.Wizard.CCT2Config:
          if (this.CCT2_Com != "-" && this.CCT2_Com != "?")
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              if (index < this.cct2.Canales)
              {
                this.CCT2_Enabled[index] = this.cct2.GetChannelEnabled(index + 1) ? 1 : 0;
                this.CCT2_Inhibit[index] = this.cct2.GetChannelInhibit(index + 1) ? 0 : 1;
                this.CCT2_Value[index] = (int) this.cct2.GetChannelValue(index + 1);
              }
              else
                this.CCT2_Enabled[index] = this.CCT2_Inhibit[index] = this.CCT2_Value[index] = 0;
            }
            this.CoinModel = "cct2";
            this.CoinModel_P = this.CCT2_Com;
            this.SetFase(Devices_Wizard.Wizard.BillMode);
            break;
          }
          this.SetFase(Devices_Wizard.Wizard.RM5Config);
          break;
        case Devices_Wizard.Wizard.SIOConfig:
          if (this.SIO_Com != "-" && this.SIO_Com != "?")
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              if (index < this.sio.Canales)
              {
                this.SIO_Enabled[index] = this.sio.GetChannelEnabled(index + 1) ? 1 : 0;
                this.SIO_Inhibit[index] = this.sio.GetChannelInhibit(index + 1) ? 0 : 1;
                this.SIO_Value[index] = this.sio.GetChannelValue(index + 1);
              }
              else
                this.SIO_Enabled[index] = this.SIO_Inhibit[index] = this.SIO_Value[index] = 0;
            }
            this.BillModel = "sio";
            this.BillModel_P = this.SIO_Com;
            this.SetFase(Devices_Wizard.Wizard.Resumen);
            break;
          }
          this.SetFase(Devices_Wizard.Wizard.TriConfig);
          break;
        case Devices_Wizard.Wizard.SSPConfig:
          if (this.SSP_Com != "-" && this.SSP_Com != "?")
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              if (index < this.ssp.Canales)
              {
                this.SSP_Enabled[index] = this.ssp.GetChannelEnabled(index + 1) ? 1 : 0;
                this.SSP_Inhibit[index] = this.ssp.GetChannelInhibit(index + 1) ? 0 : 1;
                this.SSP_Value[index] = this.ssp.GetChannelValue(index + 1);
              }
              else
                this.SSP_Enabled[index] = this.SSP_Inhibit[index] = this.SSP_Value[index] = 0;
            }
            this.BillModel = "ssp";
            this.BillModel_P = this.SSP_Com;
            this.SetFase(Devices_Wizard.Wizard.Resumen);
            break;
          }
          this.SetFase(Devices_Wizard.Wizard.SIOConfig);
          break;
        case Devices_Wizard.Wizard.SSP3Config:
          if (this.SSP3_Com != "-" && this.SSP3_Com != "?")
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              if (index < this.ssp3.Canales)
              {
                this.SSP3_Enabled[index] = this.ssp3.GetChannelEnabled(index + 1) ? 1 : 0;
                this.SSP3_Inhibit[index] = this.ssp3.GetChannelInhibit(index + 1) ? 0 : 1;
                this.SSP3_Value[index] = (int) this.ssp3.GetChannelValue(index + 1);
              }
              else
                this.SSP3_Enabled[index] = this.SSP3_Inhibit[index] = this.SSP3_Value[index] = 0;
            }
            this.BillModel = "ssp3";
            this.BillModel_P = this.SSP3_Com;
            this.SetFase(Devices_Wizard.Wizard.Resumen);
            break;
          }
          this.SetFase(Devices_Wizard.Wizard.SIOConfig);
          break;
        case Devices_Wizard.Wizard.F40Config:
          if (this.F40_Com != "-" && this.F40_Com != "?")
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              if (index < this.f40.Canales)
              {
                this.F40_Enabled[index] = this.f40.GetChannelEnabled(index + 1) ? 1 : 0;
                this.F40_Inhibit[index] = this.f40.GetChannelInhibit(index + 1) ? 0 : 1;
                this.F40_Value[index] = this.f40.GetChannelValue(index + 1);
              }
              else
                this.F40_Enabled[index] = this.F40_Inhibit[index] = this.F40_Value[index] = 0;
            }
            this.BillModel = "f40";
            this.BillModel_P = this.F40_Com;
          }
          this.SetFase(Devices_Wizard.Wizard.Resumen);
          break;
        case Devices_Wizard.Wizard.TriConfig:
          if (this.Tri_Com != "-" && this.Tri_Com != "?")
          {
            for (int index = 0; index < this.Canales; ++index)
            {
              if (index < this.tri.Canales)
              {
                this.Tri_Enabled[index] = this.tri.GetChannelEnabled(index + 1) ? 1 : 0;
                this.Tri_Inhibit[index] = this.tri.GetChannelInhibit(index + 1) ? 0 : 1;
                this.Tri_Value[index] = this.tri.GetChannelValue(index + 1);
              }
              else
                this.Tri_Enabled[index] = this.Tri_Inhibit[index] = this.Tri_Value[index] = 0;
            }
            this.BillModel = "tri";
            this.BillModel_P = this.Tri_Com;
            this.SetFase(Devices_Wizard.Wizard.Resumen);
            break;
          }
          this.SetFase(Devices_Wizard.Wizard.F40Config);
          break;
      }
    }

    private void Devices_Wizard_Load(object sender, EventArgs e)
    {
      this.run = true;
      this.bgControl.RunWorkerAsync();
      this.SetFase(Devices_Wizard.Wizard.Info);
    }

    private void Devices_Wizard_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.run = false;
      this.bgControl.CancelAsync();
      this.bgControl.Dispose();
      if (this.sio != null)
        this.sio.Close();
      if (this.ssp != null)
        this.ssp.Close();
      if (this.ssp3 != null)
        this.ssp3.Close();
      if (this.rm5 != null)
        this.rm5.Close();
      if (this.cct2 != null)
        this.cct2.Close();
      if (this.f40 != null)
        this.f40.Close();
      if (this.tri == null)
        return;
      this.tri.Close();
    }

    private void bgControl_DoWork(object sender, DoWorkEventArgs e)
    {
      while (this.run)
      {
        this.bgControl.ReportProgress(0);
        Thread.Sleep(100);
      }
    }

    private void bgControl_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (this.enint)
        return;
      this.enint = true;
      switch (this.Fase)
      {
        case Devices_Wizard.Wizard.CoinDetecting:
          this.SetFase(Devices_Wizard.Wizard.CoinDetectingWait_CCT2);
          this.CoinModel = "?";
          this.CoinModel_P = "?";
          this.pCoin.Visible = true;
          this.pCoin.Invalidate();
          break;
        case Devices_Wizard.Wizard.CoinDetectingWait_RM5:
          if (this.rm5 == null)
          {
            this.rm5 = new Control_Comestero();
            this.rm5.Start_Find_Device();
          }
          Thread.Sleep(100);
          if (this.rm5.Poll_Find_Device())
          {
            this.rm5.Stop_Find_Device();
            if (this.rm5._f_resp_scom != "-")
            {
              this.CoinDetected = true;
              this.CoinModel = "rm5";
              this.RM5_Com = this.rm5._f_resp_scom;
              this.dRM5.CheckState = CheckState.Checked;
              this.dRM5.Checked = true;
            }
            else
            {
              this.RM5_Com = "-";
              this.dRM5.CheckState = CheckState.Indeterminate;
            }
            this.pCoin.Visible = false;
            this.SetFase(Devices_Wizard.Wizard.CoinDetected);
          }
          this.pCoin.Invalidate();
          break;
        case Devices_Wizard.Wizard.CoinDetectingWait_CCT2:
          if (this.cct2 == null)
          {
            this.cct2 = new Control_CCTALK_COIN();
            this.cct2.Start_Find_Device();
          }
          Thread.Sleep(100);
          if (this.cct2.Poll_Find_Device())
          {
            this.cct2.Stop_Find_Device();
            if (this.cct2._f_resp_scom != "-")
            {
              this.CoinDetected = true;
              this.CoinModel = "cct2";
              this.CCT2_Com = this.cct2._f_resp_scom;
              this.dCCT2.CheckState = CheckState.Checked;
              this.dCCT2.Checked = true;
              this.pCoin.Visible = false;
              this.SetFase(Devices_Wizard.Wizard.CoinDetected);
            }
            else
            {
              this.CCT2_Com = "-";
              this.dCCT2.CheckState = CheckState.Indeterminate;
              this.pCoin.Visible = false;
              this.SetFase(Devices_Wizard.Wizard.CoinDetectingWait_RM5);
            }
          }
          this.pCoin.Invalidate();
          break;
        case Devices_Wizard.Wizard.BillMode:
          if (this.f40 != null)
            this.f40.Poll();
          if (this.tri != null)
            this.tri.Poll();
          if (this.ssp != null)
            this.ssp.Poll();
          if (this.ssp3 != null)
            this.ssp3.Poll();
          if (this.sio != null)
          {
            this.sio.Poll();
            break;
          }
          break;
        case Devices_Wizard.Wizard.BillDetecting:
          this.BillModel = "?";
          this.BillModel_P = "?";
          this.SetFase(Devices_Wizard.Wizard.BillDetectingWait);
          this.pBill.Visible = true;
          this.pBill.Invalidate();
          break;
        case Devices_Wizard.Wizard.BillDetectingWait:
          this.SetFase(Devices_Wizard.Wizard.BillDetectingWait_SSP);
          break;
        case Devices_Wizard.Wizard.BillDetectingWait_SSP:
          this.pBill.Invalidate();
          if (this.ssp == null)
          {
            this.ssp = new Control_NV_SSP();
            this.ssp.Start_Find_Device();
          }
          Thread.Sleep(100);
          if (this.ssp.Poll_Find_Device())
          {
            this.ssp.Stop_Find_Device();
            if (this.ssp._f_resp_scom != "-")
            {
              this.BillDetected = true;
              this.BillModel = "ssp";
              this.SSP_Com = this.ssp._f_resp_scom;
              this.dNV9_SSP.CheckState = CheckState.Checked;
              this.dNV9_SSP.Checked = true;
              this.dNV9_SSP.Text = "NV SSP (" + this.SSP_Com + ")";
              this.SetFase(Devices_Wizard.Wizard.BillDetected);
            }
            else
            {
              this.SSP_Com = "-";
              this.dNV9_SSP.CheckState = CheckState.Indeterminate;
              this.SetFase(Devices_Wizard.Wizard.BillDetectingWait_SSP3);
            }
            this.pBill.Visible = false;
            this.ssp.Close();
            this.ssp = (Control_NV_SSP) null;
          }
          Thread.Sleep(100);
          break;
        case Devices_Wizard.Wizard.BillDetectingWait_SSP3:
          this.pBill.Invalidate();
          if (this.ssp3 == null)
          {
            this.ssp3 = new Control_NV_SSP_P6();
            this.ssp3.Start_Find_Device();
          }
          Thread.Sleep(100);
          if (this.ssp3.Poll_Find_Device())
          {
            this.ssp3.Stop_Find_Device();
            if (this.ssp3._f_resp_scom != "-")
            {
              this.BillDetected = true;
              this.BillModel = "ssp3";
              this.SSP3_Com = this.ssp3._f_resp_scom;
              this.dNV9_SSP3.CheckState = CheckState.Checked;
              this.dNV9_SSP3.Checked = true;
              this.dNV9_SSP3.Text = "NV SSP v3 (" + this.SSP3_Com + ")";
              this.SetFase(Devices_Wizard.Wizard.BillDetected);
            }
            else
            {
              this.SSP3_Com = "-";
              this.dNV9_SSP3.CheckState = CheckState.Indeterminate;
              this.SetFase(Devices_Wizard.Wizard.BillDetectingWait_SIO);
            }
            this.pBill.Visible = false;
            this.ssp3.Close();
            this.ssp3 = (Control_NV_SSP_P6) null;
          }
          Thread.Sleep(100);
          break;
        case Devices_Wizard.Wizard.BillDetectingWait_SIO:
          this.pBill.Invalidate();
          if (this.sio == null)
            this.sio = new Control_NV_SIO();
          Thread.Sleep(100);
          this.SIO_Com = this.sio.Find_Device();
          if (this.sio._f_resp_scom != "-")
          {
            this.BillDetected = true;
            this.BillModel = "sio";
            this.dNV_SIO.CheckState = CheckState.Checked;
            this.dNV_SIO.Checked = true;
            this.dNV_SIO.Text = "NV SIO (300bps) (" + this.SIO_Com + ")";
            this.SetFase(Devices_Wizard.Wizard.BillDetected);
          }
          else
          {
            this.SIO_Com = "-";
            this.dNV_SIO.CheckState = CheckState.Indeterminate;
            this.SetFase(Devices_Wizard.Wizard.BillDetectingWait_F40);
          }
          this.sio.Close();
          this.sio = (Control_NV_SIO) null;
          Thread.Sleep(100);
          break;
        case Devices_Wizard.Wizard.BillDetectingWait_F40:
          this.pBill.Invalidate();
          if (this.f40 == null)
          {
            this.f40 = new Control_F40_CCTalk();
            this.f40.Start_Find_Device();
          }
          Thread.Sleep(100);
          this.F40_Com = this.f40.Find_Device();
          if (this.f40._f_resp_scom != "-" && this.CoinModel_P != this.F40_Com)
          {
            this.BillDetected = true;
            this.BillModel = "f40";
            this.dF40.CheckState = CheckState.Checked;
            this.dF40.Checked = true;
            this.dF40.Text = "ccTalk (ID 40) (" + this.F40_Com + ")";
            this.SetFase(Devices_Wizard.Wizard.BillDetected);
          }
          else
          {
            this.F40_Com = "-";
            this.dF40.CheckState = CheckState.Indeterminate;
            this.SetFase(Devices_Wizard.Wizard.BillDetectingWait_Tri);
          }
          this.f40.Close();
          this.f40 = (Control_F40_CCTalk) null;
          Thread.Sleep(100);
          break;
        case Devices_Wizard.Wizard.BillDetectingWait_Tri:
          this.pBill.Invalidate();
          if (this.tri == null)
          {
            this.tri = new Control_Trilogy();
            this.tri.Start_Find_Device();
          }
          Thread.Sleep(100);
          this.Tri_Com = this.tri.Find_Device();
          if (this.tri._f_resp_scom != "-")
          {
            this.BillDetected = true;
            this.BillModel = "tri";
            this.dTrilogy.CheckState = CheckState.Checked;
            this.dTrilogy.Checked = true;
            this.dTrilogy.Text = "Trilogy (" + this.Tri_Com + ")";
            this.SetFase(Devices_Wizard.Wizard.BillDetected);
          }
          else
          {
            this.Tri_Com = "-";
            this.dTrilogy.CheckState = CheckState.Indeterminate;
            this.SetFase(Devices_Wizard.Wizard.BillDetected);
          }
          this.tri.Close();
          this.tri = (Control_Trilogy) null;
          Thread.Sleep(100);
          break;
        case Devices_Wizard.Wizard.SIOConfig:
          if (this.sio != null)
          {
            this.sio.Poll();
            this.sio.Parser();
            break;
          }
          if (this.SIO_Com != "-" && this.SIO_Com != "?")
          {
            this.sio = new Control_NV_SIO();
            this.sio.port = this.SIO_Com;
            this.sio.Open();
          }
          break;
        case Devices_Wizard.Wizard.SSPConfig:
          if (this.ssp != null)
          {
            this.ssp.Poll();
            break;
          }
          if (this.SSP_Com != "-" && this.SSP_Com != "?")
          {
            this.ssp = new Control_NV_SSP();
            this.ssp.port = this.SSP_Com;
            this.ssp.Open();
          }
          break;
        case Devices_Wizard.Wizard.SSP3Config:
          if (this.ssp3 != null)
          {
            this.ssp3.Poll();
            break;
          }
          if (this.SSP3_Com != "-" && this.SSP3_Com != "?")
          {
            this.ssp3 = new Control_NV_SSP_P6();
            this.ssp3.port = this.SSP3_Com;
            this.ssp3.Open();
          }
          break;
        case Devices_Wizard.Wizard.F40Config:
          if (this.f40 != null)
          {
            this.f40.Poll();
            break;
          }
          if (this.F40_Com != "-" && this.F40_Com != "?")
          {
            this.f40 = new Control_F40_CCTalk();
            this.f40.port = this.F40_Com;
            this.f40.Open();
          }
          break;
        case Devices_Wizard.Wizard.TriConfig:
          if (this.tri != null)
          {
            this.tri.Poll();
            break;
          }
          if (this.Tri_Com != "-" && this.Tri_Com != "?")
          {
            this.tri = new Control_Trilogy();
            this.tri.port = this.Tri_Com;
            this.tri.Open();
          }
          break;
      }
      this.enint = false;
    }

    private void bCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    public void Save(string _cfg)
    {
      string str1 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
      string str2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg + ".tmp";
      XmlTextWriter xmlTextWriter = new XmlTextWriter(str2, Encoding.ASCII);
      xmlTextWriter.Formatting = Formatting.Indented;
      xmlTextWriter.WriteStartDocument();
      try
      {
        xmlTextWriter.WriteStartElement("config".ToLower());
        xmlTextWriter.WriteStartElement("select".ToLower());
        xmlTextWriter.WriteAttributeString("coin".ToLower(), this.CoinModel);
        xmlTextWriter.WriteAttributeString("coin_p".ToLower(), this.CoinModel_P);
        xmlTextWriter.WriteAttributeString("bnv".ToLower(), this.BillModel);
        xmlTextWriter.WriteAttributeString("bnv_p".ToLower(), this.BillModel_P);
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("devices".ToLower());
        xmlTextWriter.WriteStartElement("ssp");
        xmlTextWriter.WriteAttributeString("com", this.SSP_Com);
        for (int index = 0; index < this.Canales; ++index)
        {
          xmlTextWriter.WriteStartElement("channel_" + (object) (index + 1));
          if (this.SSP_Enabled[index] == 1)
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), this.SSP_Value[index].ToString());
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), this.SSP_Enabled[index].ToString());
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), this.SSP_Inhibit[index].ToString());
          }
          else
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), "1");
          }
          xmlTextWriter.WriteEndElement();
        }
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("ssp3");
        xmlTextWriter.WriteAttributeString("com", this.SSP3_Com);
        for (int index = 0; index < this.Canales; ++index)
        {
          xmlTextWriter.WriteStartElement("channel_" + (object) (index + 1));
          if (this.SSP3_Enabled[index] == 1)
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), this.SSP3_Value[index].ToString());
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), this.SSP3_Enabled[index].ToString());
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), this.SSP3_Inhibit[index].ToString());
          }
          else
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), "1");
          }
          xmlTextWriter.WriteEndElement();
        }
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("sio");
        xmlTextWriter.WriteAttributeString("com", this.SIO_Com);
        for (int index = 0; index < this.Canales; ++index)
        {
          xmlTextWriter.WriteStartElement("channel_" + (object) (index + 1));
          if (this.SIO_Enabled[index] == 1)
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), this.SIO_Value[index].ToString());
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), this.SIO_Enabled[index].ToString());
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), this.SIO_Inhibit[index].ToString());
          }
          else
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), "1");
          }
          xmlTextWriter.WriteEndElement();
        }
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("rm5");
        xmlTextWriter.WriteAttributeString("com", this.RM5_Com);
        for (int index = 0; index < this.Canales; ++index)
        {
          xmlTextWriter.WriteStartElement("channel_" + (object) (index + 1));
          if (this.RM5_Enabled[index] == 1)
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), this.RM5_Value[index].ToString());
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), this.RM5_Enabled[index].ToString());
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), this.RM5_Inhibit[index].ToString());
          }
          else
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), "1");
          }
          xmlTextWriter.WriteEndElement();
        }
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("f40");
        xmlTextWriter.WriteAttributeString("com", this.F40_Com);
        for (int index = 0; index < this.Canales; ++index)
        {
          xmlTextWriter.WriteStartElement("channel_" + (object) (index + 1));
          if (this.F40_Enabled[index] == 1)
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), this.F40_Value[index].ToString());
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), this.F40_Enabled[index].ToString());
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), this.F40_Inhibit[index].ToString());
          }
          else
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), "1");
          }
          xmlTextWriter.WriteEndElement();
        }
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteStartElement("tri");
        xmlTextWriter.WriteAttributeString("com", this.Tri_Com);
        for (int index = 0; index < this.Canales; ++index)
        {
          xmlTextWriter.WriteStartElement("channel_" + (object) (index + 1));
          if (this.Tri_Enabled[index] == 1)
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), this.Tri_Value[index].ToString());
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), this.Tri_Enabled[index].ToString());
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), this.Tri_Inhibit[index].ToString());
          }
          else
          {
            xmlTextWriter.WriteAttributeString("value" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("enabled" + (object) (index + 1), "0");
            xmlTextWriter.WriteAttributeString("inhibit" + (object) (index + 1), "1");
          }
          xmlTextWriter.WriteEndElement();
        }
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.WriteEndElement();
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        xmlTextWriter.Flush();
        xmlTextWriter.Close();
        File.Delete(str2);
        return;
      }
      xmlTextWriter.Flush();
      xmlTextWriter.Close();
      if (File.Exists(str1))
        File.Delete(str1);
      File.Copy(str2, str1);
      File.Delete(str2);
    }

    public bool Load_Cfg(string _cfg)
    {
      string url = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(url);
        xmlTextReader.Read();
        xmlTextReader.Close();
      }
      catch (Exception ex)
      {
        return false;
      }
      try
      {
        XmlTextReader xmlTextReader = new XmlTextReader(url);
        while (xmlTextReader.Read())
        {
          if (xmlTextReader.NodeType == XmlNodeType.Element)
          {
            if (xmlTextReader.Name.ToLower() == "select".ToLower() && xmlTextReader.HasAttributes)
            {
              for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
              {
                xmlTextReader.MoveToAttribute(i);
                if (xmlTextReader.Name.ToLower() == "coin".ToLower())
                  this.CoinModel = xmlTextReader.Value;
                else if (xmlTextReader.Name.ToLower() == "coin_p".ToLower())
                  this.CoinModel_P = xmlTextReader.Value;
                else if (xmlTextReader.Name.ToLower() == "bnv".ToLower())
                  this.BillModel = xmlTextReader.Value;
                else if (xmlTextReader.Name.ToLower() == "bnv_p".ToLower())
                  this.BillModel_P = xmlTextReader.Value;
              }
            }
            if (xmlTextReader.Name.ToLower() == "ssp".ToLower() && xmlTextReader.HasAttributes)
            {
              for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
              {
                xmlTextReader.MoveToAttribute(i);
                if (xmlTextReader.Name.ToLower() == "com".ToLower())
                {
                  this.SSP_Com = xmlTextReader.Value;
                }
                else
                {
                  for (int index = 0; index < this.Canales; ++index)
                  {
                    string str = "channel" + (object) (index + 1);
                    if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.SSP_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                    else if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.SSP_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                  }
                }
              }
            }
            if (xmlTextReader.Name.ToLower() == "ssp3".ToLower() && xmlTextReader.HasAttributes)
            {
              for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
              {
                xmlTextReader.MoveToAttribute(i);
                if (xmlTextReader.Name.ToLower() == "com".ToLower())
                {
                  this.SSP3_Com = xmlTextReader.Value;
                }
                else
                {
                  for (int index = 0; index < this.Canales; ++index)
                  {
                    string str = "channel" + (object) (index + 1);
                    if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.SSP3_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                    else if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.SSP3_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                  }
                }
              }
            }
            if (xmlTextReader.Name.ToLower() == "sio".ToLower() && xmlTextReader.HasAttributes)
            {
              for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
              {
                xmlTextReader.MoveToAttribute(i);
                if (xmlTextReader.Name.ToLower() == "com".ToLower())
                {
                  this.SIO_Com = xmlTextReader.Value;
                }
                else
                {
                  for (int index = 0; index < this.Canales; ++index)
                  {
                    string str = "channel" + (object) (index + 1);
                    if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.SIO_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                    else if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.SIO_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                  }
                }
              }
            }
            if (xmlTextReader.Name.ToLower() == "rm5".ToLower() && xmlTextReader.HasAttributes)
            {
              for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
              {
                xmlTextReader.MoveToAttribute(i);
                if (xmlTextReader.Name.ToLower() == "com".ToLower())
                {
                  this.RM5_Com = xmlTextReader.Value;
                }
                else
                {
                  for (int index = 0; index < this.Canales; ++index)
                  {
                    string str = "channel" + (object) (index + 1);
                    if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.RM5_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                    else if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.RM5_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                  }
                }
              }
            }
            if (xmlTextReader.Name.ToLower() == "tri".ToLower() && xmlTextReader.HasAttributes)
            {
              for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
              {
                xmlTextReader.MoveToAttribute(i);
                if (xmlTextReader.Name.ToLower() == "com".ToLower())
                {
                  this.Tri_Com = xmlTextReader.Value;
                }
                else
                {
                  for (int index = 0; index < this.Canales; ++index)
                  {
                    string str = "channel" + (object) (index + 1);
                    if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.Tri_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                    else if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.Tri_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                  }
                }
              }
            }
            if (xmlTextReader.Name.ToLower() == "f40".ToLower() && xmlTextReader.HasAttributes)
            {
              for (int i = 0; i < xmlTextReader.AttributeCount; ++i)
              {
                xmlTextReader.MoveToAttribute(i);
                if (xmlTextReader.Name.ToLower() == "com".ToLower())
                {
                  this.F40_Com = xmlTextReader.Value;
                }
                else
                {
                  for (int index = 0; index < this.Canales; ++index)
                  {
                    string str = "channel" + (object) (index + 1);
                    if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.F40_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                    else if (xmlTextReader.Name.ToLower() == str.ToLower())
                      this.F40_Value[index] = Convert.ToInt32(xmlTextReader.Value);
                  }
                }
              }
            }
          }
        }
        xmlTextReader.Close();
      }
      catch (Exception ex)
      {
        this.error = ex.Message;
        return false;
      }
      return true;
    }

    private void bOK_Click(object sender, EventArgs e)
    {
      this.Save("devices.cfg");
      if (this.opciones != null)
      {
        this.opciones.Dev_BNV = this.BillModel;
        this.opciones.Dev_BNV_P = this.BillModel_P;
        this.opciones.Dev_Coin = this.CoinModel;
        this.opciones.Dev_Coin_P = this.CoinModel_P;
      }
      this.OK = true;
      this.Close();
    }

    private void lCom_SelectionChangeCommitted(object sender, EventArgs e)
    {
      switch (this.Fase)
      {
        case Devices_Wizard.Wizard.RM5Config:
          this.RM5_Com = this.lCom.Text;
          break;
        case Devices_Wizard.Wizard.SIOConfig:
          this.SIO_Com = this.lCom.Text;
          break;
        case Devices_Wizard.Wizard.SSPConfig:
          this.SSP_Com = this.lCom.Text;
          break;
        case Devices_Wizard.Wizard.SSP3Config:
          this.SSP3_Com = this.lCom.Text;
          break;
        case Devices_Wizard.Wizard.F40Config:
          this.F40_Com = this.lCom.Text;
          break;
        case Devices_Wizard.Wizard.TriConfig:
          this.Tri_Com = this.lCom.Text;
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Devices_Wizard));
      this.pBotons = new Panel();
      this.bCancel = new Button();
      this.bSig = new Button();
      this.bAnt = new Button();
      this.bOK = new Button();
      this.tabs = new TabControl();
      this.tInfo = new TabPage();
      this.hInfo = new GradientPanel();
      this.infoWizard = new TextBox();
      this.tCoin_Mode = new TabPage();
      this.rCoin_M3 = new RadioButton();
      this.rCoin_M1 = new RadioButton();
      this.rCoin_M2 = new RadioButton();
      this.hCoin_Mode = new GradientPanel();
      this.tCoin_Detect = new TabPage();
      this.dCCT2 = new CheckBox();
      this.pCoin = new ProgressBar();
      this.dRM5 = new CheckBox();
      this.hCoin_Detect = new GradientPanel();
      this.tBill_Mode = new TabPage();
      this.rBill_M3 = new RadioButton();
      this.rBill_M1 = new RadioButton();
      this.rBill_M2 = new RadioButton();
      this.pBillMode = new GradientPanel();
      this.tBill_Detect = new TabPage();
      this.dNV_SIO = new CheckBox();
      this.pBill = new ProgressBar();
      this.dTrilogy = new CheckBox();
      this.dF40 = new CheckBox();
      this.dNV9_SSP = new CheckBox();
      this.dNV9_SSP3 = new CheckBox();
      this.pBillDetect = new GradientPanel();
      this.tBill_Config = new TabPage();
      this.eCANAL_16 = new CheckBox();
      this.eCANAL_15 = new CheckBox();
      this.eCANAL_14 = new CheckBox();
      this.eCANAL_13 = new CheckBox();
      this.eCANAL_12 = new CheckBox();
      this.eCANAL_11 = new CheckBox();
      this.eCANAL_10 = new CheckBox();
      this.eCANAL_9 = new CheckBox();
      this.eCANAL_8 = new CheckBox();
      this.eCANAL_7 = new CheckBox();
      this.eCANAL_6 = new CheckBox();
      this.eCANAL_5 = new CheckBox();
      this.eCANAL_4 = new CheckBox();
      this.eCANAL_3 = new CheckBox();
      this.eCANAL_2 = new CheckBox();
      this.eCANAL_1 = new CheckBox();
      this.iCom = new Label();
      this.lCom = new ComboBox();
      this.pTitle = new GradientPanel();
      this.tResum = new TabPage();
      this.bgControl = new BackgroundWorker();
      this.pBotons.SuspendLayout();
      this.tabs.SuspendLayout();
      this.tInfo.SuspendLayout();
      this.tCoin_Mode.SuspendLayout();
      this.tCoin_Detect.SuspendLayout();
      this.tBill_Mode.SuspendLayout();
      this.tBill_Detect.SuspendLayout();
      this.tBill_Config.SuspendLayout();
      this.SuspendLayout();
      this.pBotons.Controls.Add((Control) this.bCancel);
      this.pBotons.Controls.Add((Control) this.bSig);
      this.pBotons.Controls.Add((Control) this.bAnt);
      this.pBotons.Controls.Add((Control) this.bOK);
      this.pBotons.Dock = DockStyle.Bottom;
      this.pBotons.Location = new Point(0, 415);
      this.pBotons.Name = "pBotons";
      this.pBotons.Size = new Size(657, 54);
      this.pBotons.TabIndex = 0;
      this.bCancel.Image = (Image) Resources.ico_del;
      this.bCancel.Location = new Point(12, 7);
      this.bCancel.Name = "bCancel";
      this.bCancel.Size = new Size(75, 41);
      this.bCancel.TabIndex = 0;
      this.bCancel.UseVisualStyleBackColor = true;
      this.bCancel.Click += new EventHandler(this.bCancel_Click);
      this.bSig.Image = (Image) Resources.ico_right_green;
      this.bSig.Location = new Point(489, 7);
      this.bSig.Name = "bSig";
      this.bSig.Size = new Size(75, 41);
      this.bSig.TabIndex = 2;
      this.bSig.UseVisualStyleBackColor = true;
      this.bSig.Click += new EventHandler(this.bSig_Click);
      this.bAnt.Image = (Image) Resources.ico_left_blue;
      this.bAnt.Location = new Point(408, 7);
      this.bAnt.Name = "bAnt";
      this.bAnt.Size = new Size(75, 41);
      this.bAnt.TabIndex = 1;
      this.bAnt.UseVisualStyleBackColor = true;
      this.bOK.Image = (Image) Resources.ico_ok;
      this.bOK.Location = new Point(570, 7);
      this.bOK.Name = "bOK";
      this.bOK.Size = new Size(75, 41);
      this.bOK.TabIndex = 3;
      this.bOK.UseVisualStyleBackColor = true;
      this.bOK.Click += new EventHandler(this.bOK_Click);
      this.tabs.Appearance = TabAppearance.FlatButtons;
      this.tabs.Controls.Add((Control) this.tInfo);
      this.tabs.Controls.Add((Control) this.tCoin_Mode);
      this.tabs.Controls.Add((Control) this.tCoin_Detect);
      this.tabs.Controls.Add((Control) this.tBill_Mode);
      this.tabs.Controls.Add((Control) this.tBill_Detect);
      this.tabs.Controls.Add((Control) this.tBill_Config);
      this.tabs.Controls.Add((Control) this.tResum);
      this.tabs.Dock = DockStyle.Fill;
      this.tabs.ItemSize = new Size(0, 1);
      this.tabs.Location = new Point(0, 0);
      this.tabs.Multiline = true;
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new Size(657, 415);
      this.tabs.SizeMode = TabSizeMode.Fixed;
      this.tabs.TabIndex = 1;
      this.tInfo.Controls.Add((Control) this.hInfo);
      this.tInfo.Controls.Add((Control) this.infoWizard);
      this.tInfo.Location = new Point(4, 5);
      this.tInfo.Name = "tInfo";
      this.tInfo.Padding = new Padding(3);
      this.tInfo.Size = new Size(649, 406);
      this.tInfo.TabIndex = 0;
      this.hInfo.BackgroundImage = (Image) componentResourceManager.GetObject("hInfo.BackgroundImage");
      this.hInfo.BackgroundImageLayout = ImageLayout.Stretch;
      this.hInfo.Dock = DockStyle.Top;
      this.hInfo.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.hInfo.Location = new Point(3, 3);
      this.hInfo.Name = "hInfo";
      this.hInfo.PageEndColor = Color.Empty;
      this.hInfo.PageStartColor = Color.LightSteelBlue;
      this.hInfo.Size = new Size(643, 60);
      this.hInfo.TabIndex = 3;
      this.hInfo.Title = "Configuration devices";
      this.hInfo.Title_Alignement = GradientPanel.Alignement.Left;
      this.infoWizard.Location = new Point(8, 68);
      this.infoWizard.Margin = new Padding(0);
      this.infoWizard.Multiline = true;
      this.infoWizard.Name = "infoWizard";
      this.infoWizard.ReadOnly = true;
      this.infoWizard.Size = new Size(633, 333);
      this.infoWizard.TabIndex = 2;
      this.infoWizard.Text = "1) Install drivers.\r\n\r\n2) Connect device.\r\n\r\n3) COnfigure device.\r\n";
      this.tCoin_Mode.Controls.Add((Control) this.rCoin_M3);
      this.tCoin_Mode.Controls.Add((Control) this.rCoin_M1);
      this.tCoin_Mode.Controls.Add((Control) this.rCoin_M2);
      this.tCoin_Mode.Controls.Add((Control) this.hCoin_Mode);
      this.tCoin_Mode.Location = new Point(4, 5);
      this.tCoin_Mode.Name = "tCoin_Mode";
      this.tCoin_Mode.Padding = new Padding(3);
      this.tCoin_Mode.Size = new Size(649, 406);
      this.tCoin_Mode.TabIndex = 1;
      this.tCoin_Mode.UseVisualStyleBackColor = true;
      this.rCoin_M3.AutoSize = true;
      this.rCoin_M3.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rCoin_M3.Location = new Point(30, 133);
      this.rCoin_M3.Name = "rCoin_M3";
      this.rCoin_M3.Size = new Size(120, 35);
      this.rCoin_M3.TabIndex = 3;
      this.rCoin_M3.Text = "Manual";
      this.rCoin_M3.UseVisualStyleBackColor = true;
      this.rCoin_M1.AutoSize = true;
      this.rCoin_M1.Checked = true;
      this.rCoin_M1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rCoin_M1.Location = new Point(30, 90);
      this.rCoin_M1.Name = "rCoin_M1";
      this.rCoin_M1.Size = new Size(112, 35);
      this.rCoin_M1.TabIndex = 2;
      this.rCoin_M1.TabStop = true;
      this.rCoin_M1.Text = "Detect";
      this.rCoin_M1.UseVisualStyleBackColor = true;
      this.rCoin_M2.AutoSize = true;
      this.rCoin_M2.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rCoin_M2.Location = new Point(30, 176);
      this.rCoin_M2.Name = "rCoin_M2";
      this.rCoin_M2.Size = new Size(97, 35);
      this.rCoin_M2.TabIndex = 1;
      this.rCoin_M2.Text = "None";
      this.rCoin_M2.UseVisualStyleBackColor = true;
      this.hCoin_Mode.BackgroundImage = (Image) componentResourceManager.GetObject("hCoin_Mode.BackgroundImage");
      this.hCoin_Mode.BackgroundImageLayout = ImageLayout.Stretch;
      this.hCoin_Mode.Dock = DockStyle.Top;
      this.hCoin_Mode.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.hCoin_Mode.Location = new Point(3, 3);
      this.hCoin_Mode.Name = "hCoin_Mode";
      this.hCoin_Mode.PageEndColor = Color.Empty;
      this.hCoin_Mode.PageStartColor = Color.LightSteelBlue;
      this.hCoin_Mode.Size = new Size(643, 60);
      this.hCoin_Mode.TabIndex = 0;
      this.hCoin_Mode.Title = "Coin acceptor";
      this.hCoin_Mode.Title_Alignement = GradientPanel.Alignement.Left;
      this.tCoin_Detect.Controls.Add((Control) this.dCCT2);
      this.tCoin_Detect.Controls.Add((Control) this.pCoin);
      this.tCoin_Detect.Controls.Add((Control) this.dRM5);
      this.tCoin_Detect.Controls.Add((Control) this.hCoin_Detect);
      this.tCoin_Detect.Location = new Point(4, 5);
      this.tCoin_Detect.Name = "tCoin_Detect";
      this.tCoin_Detect.Size = new Size(649, 406);
      this.tCoin_Detect.TabIndex = 3;
      this.tCoin_Detect.UseVisualStyleBackColor = true;
      this.dCCT2.AutoSize = true;
      this.dCCT2.Enabled = false;
      this.dCCT2.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dCCT2.Location = new Point(27, 85);
      this.dCCT2.Name = "dCCT2";
      this.dCCT2.Size = new Size(435, 35);
      this.dCCT2.TabIndex = 4;
      this.dCCT2.Text = "CCTalk COIN ACCEPTOR (ID 2)";
      this.dCCT2.ThreeState = true;
      this.dCCT2.UseVisualStyleBackColor = true;
      this.pCoin.Dock = DockStyle.Bottom;
      this.pCoin.Location = new Point(0, 392);
      this.pCoin.Name = "pCoin";
      this.pCoin.Size = new Size(649, 14);
      this.pCoin.Step = 100;
      this.pCoin.Style = ProgressBarStyle.Marquee;
      this.pCoin.TabIndex = 3;
      this.pCoin.Value = 100;
      this.pCoin.Visible = false;
      this.dRM5.AutoSize = true;
      this.dRM5.Enabled = false;
      this.dRM5.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dRM5.Location = new Point(27, 126);
      this.dRM5.Name = "dRM5";
      this.dRM5.Size = new Size(230, 35);
      this.dRM5.TabIndex = 2;
      this.dRM5.Text = "Comestero RM5";
      this.dRM5.ThreeState = true;
      this.dRM5.UseVisualStyleBackColor = true;
      this.hCoin_Detect.BackgroundImage = (Image) componentResourceManager.GetObject("hCoin_Detect.BackgroundImage");
      this.hCoin_Detect.BackgroundImageLayout = ImageLayout.Stretch;
      this.hCoin_Detect.Dock = DockStyle.Top;
      this.hCoin_Detect.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.hCoin_Detect.Location = new Point(0, 0);
      this.hCoin_Detect.Name = "hCoin_Detect";
      this.hCoin_Detect.PageEndColor = Color.Empty;
      this.hCoin_Detect.PageStartColor = Color.LightSteelBlue;
      this.hCoin_Detect.Size = new Size(649, 60);
      this.hCoin_Detect.TabIndex = 1;
      this.hCoin_Detect.Title = "Find coin acceptor";
      this.hCoin_Detect.Title_Alignement = GradientPanel.Alignement.Left;
      this.tBill_Mode.Controls.Add((Control) this.rBill_M3);
      this.tBill_Mode.Controls.Add((Control) this.rBill_M1);
      this.tBill_Mode.Controls.Add((Control) this.rBill_M2);
      this.tBill_Mode.Controls.Add((Control) this.pBillMode);
      this.tBill_Mode.Location = new Point(4, 5);
      this.tBill_Mode.Name = "tBill_Mode";
      this.tBill_Mode.Size = new Size(649, 406);
      this.tBill_Mode.TabIndex = 2;
      this.tBill_Mode.UseVisualStyleBackColor = true;
      this.rBill_M3.AutoSize = true;
      this.rBill_M3.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rBill_M3.Location = new Point(30, 133);
      this.rBill_M3.Name = "rBill_M3";
      this.rBill_M3.Size = new Size(120, 35);
      this.rBill_M3.TabIndex = 7;
      this.rBill_M3.Text = "Manual";
      this.rBill_M3.UseVisualStyleBackColor = true;
      this.rBill_M1.AutoSize = true;
      this.rBill_M1.Checked = true;
      this.rBill_M1.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rBill_M1.Location = new Point(30, 90);
      this.rBill_M1.Name = "rBill_M1";
      this.rBill_M1.Size = new Size(112, 35);
      this.rBill_M1.TabIndex = 6;
      this.rBill_M1.TabStop = true;
      this.rBill_M1.Text = "Detect";
      this.rBill_M1.UseVisualStyleBackColor = true;
      this.rBill_M2.AutoSize = true;
      this.rBill_M2.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rBill_M2.Location = new Point(30, 174);
      this.rBill_M2.Name = "rBill_M2";
      this.rBill_M2.Size = new Size(97, 35);
      this.rBill_M2.TabIndex = 5;
      this.rBill_M2.Text = "None";
      this.rBill_M2.UseVisualStyleBackColor = true;
      this.pBillMode.BackgroundImage = (Image) componentResourceManager.GetObject("pBillMode.BackgroundImage");
      this.pBillMode.BackgroundImageLayout = ImageLayout.Stretch;
      this.pBillMode.Dock = DockStyle.Top;
      this.pBillMode.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pBillMode.Location = new Point(0, 0);
      this.pBillMode.Name = "pBillMode";
      this.pBillMode.PageEndColor = Color.Empty;
      this.pBillMode.PageStartColor = Color.LightSteelBlue;
      this.pBillMode.Size = new Size(649, 60);
      this.pBillMode.TabIndex = 4;
      this.pBillMode.Title = "Validator";
      this.pBillMode.Title_Alignement = GradientPanel.Alignement.Left;
      this.tBill_Detect.Controls.Add((Control) this.dNV_SIO);
      this.tBill_Detect.Controls.Add((Control) this.pBill);
      this.tBill_Detect.Controls.Add((Control) this.dTrilogy);
      this.tBill_Detect.Controls.Add((Control) this.dF40);
      this.tBill_Detect.Controls.Add((Control) this.dNV9_SSP);
      this.tBill_Detect.Controls.Add((Control) this.dNV9_SSP3);
      this.tBill_Detect.Controls.Add((Control) this.pBillDetect);
      this.tBill_Detect.Location = new Point(4, 5);
      this.tBill_Detect.Name = "tBill_Detect";
      this.tBill_Detect.Size = new Size(649, 406);
      this.tBill_Detect.TabIndex = 6;
      this.tBill_Detect.UseVisualStyleBackColor = true;
      this.dNV_SIO.AutoSize = true;
      this.dNV_SIO.Enabled = false;
      this.dNV_SIO.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dNV_SIO.Location = new Point(30, 166);
      this.dNV_SIO.Name = "dNV_SIO";
      this.dNV_SIO.Size = new Size(239, 35);
      this.dNV_SIO.TabIndex = 9;
      this.dNV_SIO.Text = "NV SIO (300bps)";
      this.dNV_SIO.UseVisualStyleBackColor = true;
      this.pBill.Dock = DockStyle.Bottom;
      this.pBill.Location = new Point(0, 392);
      this.pBill.Name = "pBill";
      this.pBill.Size = new Size(649, 14);
      this.pBill.Step = 100;
      this.pBill.Style = ProgressBarStyle.Marquee;
      this.pBill.TabIndex = 8;
      this.pBill.Value = 100;
      this.pBill.Visible = false;
      this.dTrilogy.AutoSize = true;
      this.dTrilogy.Enabled = false;
      this.dTrilogy.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dTrilogy.Location = new Point(30, 252);
      this.dTrilogy.Name = "dTrilogy";
      this.dTrilogy.Size = new Size(115, 35);
      this.dTrilogy.TabIndex = 7;
      this.dTrilogy.Text = "Trilogy";
      this.dTrilogy.UseVisualStyleBackColor = true;
      this.dF40.AutoSize = true;
      this.dF40.Enabled = false;
      this.dF40.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dF40.Location = new Point(30, 209);
      this.dF40.Name = "dF40";
      this.dF40.Size = new Size(203, 35);
      this.dF40.TabIndex = 6;
      this.dF40.Text = "ccTalk (ID 40)";
      this.dF40.UseVisualStyleBackColor = true;
      this.dNV9_SSP.AutoSize = true;
      this.dNV9_SSP.Enabled = false;
      this.dNV9_SSP.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dNV9_SSP.Location = new Point(30, 80);
      this.dNV9_SSP.Name = "dNV9_SSP";
      this.dNV9_SSP.Size = new Size(132, 35);
      this.dNV9_SSP.TabIndex = 4;
      this.dNV9_SSP.Text = "NV SSP";
      this.dNV9_SSP.UseVisualStyleBackColor = true;
      this.dNV9_SSP3.AutoSize = true;
      this.dNV9_SSP3.Enabled = false;
      this.dNV9_SSP3.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.dNV9_SSP3.Location = new Point(30, 123);
      this.dNV9_SSP3.Name = "dNV9_SSP3";
      this.dNV9_SSP3.Size = new Size(168, 35);
      this.dNV9_SSP3.TabIndex = 4;
      this.dNV9_SSP3.Text = "NV SSP v3";
      this.dNV9_SSP3.UseVisualStyleBackColor = true;
      this.pBillDetect.BackgroundImage = (Image) componentResourceManager.GetObject("pBillDetect.BackgroundImage");
      this.pBillDetect.BackgroundImageLayout = ImageLayout.Stretch;
      this.pBillDetect.Dock = DockStyle.Top;
      this.pBillDetect.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pBillDetect.Location = new Point(0, 0);
      this.pBillDetect.Name = "pBillDetect";
      this.pBillDetect.PageEndColor = Color.Empty;
      this.pBillDetect.PageStartColor = Color.LightSteelBlue;
      this.pBillDetect.Size = new Size(649, 60);
      this.pBillDetect.TabIndex = 3;
      this.pBillDetect.Title = "Find bill validator";
      this.pBillDetect.Title_Alignement = GradientPanel.Alignement.Left;
      this.tBill_Config.Controls.Add((Control) this.eCANAL_16);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_15);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_14);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_13);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_12);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_11);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_10);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_9);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_8);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_7);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_6);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_5);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_4);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_3);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_2);
      this.tBill_Config.Controls.Add((Control) this.eCANAL_1);
      this.tBill_Config.Controls.Add((Control) this.iCom);
      this.tBill_Config.Controls.Add((Control) this.lCom);
      this.tBill_Config.Controls.Add((Control) this.pTitle);
      this.tBill_Config.Location = new Point(4, 5);
      this.tBill_Config.Name = "tBill_Config";
      this.tBill_Config.Size = new Size(649, 406);
      this.tBill_Config.TabIndex = 9;
      this.tBill_Config.UseVisualStyleBackColor = true;
      this.eCANAL_16.AutoSize = true;
      this.eCANAL_16.Checked = true;
      this.eCANAL_16.CheckState = CheckState.Indeterminate;
      this.eCANAL_16.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_16.Location = new Point(319, 369);
      this.eCANAL_16.Name = "eCANAL_16";
      this.eCANAL_16.Size = new Size(33, 24);
      this.eCANAL_16.TabIndex = 38;
      this.eCANAL_16.Text = "-";
      this.eCANAL_16.UseVisualStyleBackColor = true;
      this.eCANAL_15.AutoSize = true;
      this.eCANAL_15.Checked = true;
      this.eCANAL_15.CheckState = CheckState.Indeterminate;
      this.eCANAL_15.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_15.Location = new Point(319, 337);
      this.eCANAL_15.Name = "eCANAL_15";
      this.eCANAL_15.Size = new Size(33, 24);
      this.eCANAL_15.TabIndex = 37;
      this.eCANAL_15.Text = "-";
      this.eCANAL_15.UseVisualStyleBackColor = true;
      this.eCANAL_14.AutoSize = true;
      this.eCANAL_14.Checked = true;
      this.eCANAL_14.CheckState = CheckState.Indeterminate;
      this.eCANAL_14.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_14.Location = new Point(319, 305);
      this.eCANAL_14.Name = "eCANAL_14";
      this.eCANAL_14.Size = new Size(33, 24);
      this.eCANAL_14.TabIndex = 36;
      this.eCANAL_14.Text = "-";
      this.eCANAL_14.UseVisualStyleBackColor = true;
      this.eCANAL_13.AutoSize = true;
      this.eCANAL_13.Checked = true;
      this.eCANAL_13.CheckState = CheckState.Indeterminate;
      this.eCANAL_13.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_13.Location = new Point(319, 273);
      this.eCANAL_13.Name = "eCANAL_13";
      this.eCANAL_13.Size = new Size(33, 24);
      this.eCANAL_13.TabIndex = 35;
      this.eCANAL_13.Text = "-";
      this.eCANAL_13.UseVisualStyleBackColor = true;
      this.eCANAL_12.AutoSize = true;
      this.eCANAL_12.Checked = true;
      this.eCANAL_12.CheckState = CheckState.Indeterminate;
      this.eCANAL_12.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_12.Location = new Point(319, 241);
      this.eCANAL_12.Name = "eCANAL_12";
      this.eCANAL_12.Size = new Size(33, 24);
      this.eCANAL_12.TabIndex = 34;
      this.eCANAL_12.Text = "-";
      this.eCANAL_12.UseVisualStyleBackColor = true;
      this.eCANAL_11.AutoSize = true;
      this.eCANAL_11.Checked = true;
      this.eCANAL_11.CheckState = CheckState.Indeterminate;
      this.eCANAL_11.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_11.Location = new Point(319, 209);
      this.eCANAL_11.Name = "eCANAL_11";
      this.eCANAL_11.Size = new Size(33, 24);
      this.eCANAL_11.TabIndex = 33;
      this.eCANAL_11.Text = "-";
      this.eCANAL_11.UseVisualStyleBackColor = true;
      this.eCANAL_10.AutoSize = true;
      this.eCANAL_10.Checked = true;
      this.eCANAL_10.CheckState = CheckState.Indeterminate;
      this.eCANAL_10.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_10.Location = new Point(319, 177);
      this.eCANAL_10.Name = "eCANAL_10";
      this.eCANAL_10.Size = new Size(33, 24);
      this.eCANAL_10.TabIndex = 32;
      this.eCANAL_10.Text = "-";
      this.eCANAL_10.UseVisualStyleBackColor = true;
      this.eCANAL_9.AutoSize = true;
      this.eCANAL_9.Checked = true;
      this.eCANAL_9.CheckState = CheckState.Indeterminate;
      this.eCANAL_9.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_9.Location = new Point(319, 145);
      this.eCANAL_9.Name = "eCANAL_9";
      this.eCANAL_9.Size = new Size(33, 24);
      this.eCANAL_9.TabIndex = 31;
      this.eCANAL_9.Text = "-";
      this.eCANAL_9.UseVisualStyleBackColor = true;
      this.eCANAL_8.AutoSize = true;
      this.eCANAL_8.Checked = true;
      this.eCANAL_8.CheckState = CheckState.Indeterminate;
      this.eCANAL_8.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_8.Location = new Point(22, 369);
      this.eCANAL_8.Name = "eCANAL_8";
      this.eCANAL_8.Size = new Size(33, 24);
      this.eCANAL_8.TabIndex = 30;
      this.eCANAL_8.Text = "-";
      this.eCANAL_8.UseVisualStyleBackColor = true;
      this.eCANAL_7.AutoSize = true;
      this.eCANAL_7.Checked = true;
      this.eCANAL_7.CheckState = CheckState.Indeterminate;
      this.eCANAL_7.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_7.Location = new Point(22, 337);
      this.eCANAL_7.Name = "eCANAL_7";
      this.eCANAL_7.Size = new Size(33, 24);
      this.eCANAL_7.TabIndex = 29;
      this.eCANAL_7.Text = "-";
      this.eCANAL_7.UseVisualStyleBackColor = true;
      this.eCANAL_6.AutoSize = true;
      this.eCANAL_6.Checked = true;
      this.eCANAL_6.CheckState = CheckState.Indeterminate;
      this.eCANAL_6.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_6.Location = new Point(22, 305);
      this.eCANAL_6.Name = "eCANAL_6";
      this.eCANAL_6.Size = new Size(33, 24);
      this.eCANAL_6.TabIndex = 28;
      this.eCANAL_6.Text = "-";
      this.eCANAL_6.UseVisualStyleBackColor = true;
      this.eCANAL_5.AutoSize = true;
      this.eCANAL_5.Checked = true;
      this.eCANAL_5.CheckState = CheckState.Indeterminate;
      this.eCANAL_5.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_5.Location = new Point(22, 273);
      this.eCANAL_5.Name = "eCANAL_5";
      this.eCANAL_5.Size = new Size(33, 24);
      this.eCANAL_5.TabIndex = 27;
      this.eCANAL_5.Text = "-";
      this.eCANAL_5.UseVisualStyleBackColor = true;
      this.eCANAL_4.AutoSize = true;
      this.eCANAL_4.Checked = true;
      this.eCANAL_4.CheckState = CheckState.Indeterminate;
      this.eCANAL_4.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_4.Location = new Point(22, 241);
      this.eCANAL_4.Name = "eCANAL_4";
      this.eCANAL_4.Size = new Size(33, 24);
      this.eCANAL_4.TabIndex = 26;
      this.eCANAL_4.Text = "-";
      this.eCANAL_4.UseVisualStyleBackColor = true;
      this.eCANAL_3.AutoSize = true;
      this.eCANAL_3.Checked = true;
      this.eCANAL_3.CheckState = CheckState.Indeterminate;
      this.eCANAL_3.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_3.Location = new Point(22, 209);
      this.eCANAL_3.Name = "eCANAL_3";
      this.eCANAL_3.Size = new Size(33, 24);
      this.eCANAL_3.TabIndex = 25;
      this.eCANAL_3.Text = "-";
      this.eCANAL_3.UseVisualStyleBackColor = true;
      this.eCANAL_2.AutoSize = true;
      this.eCANAL_2.Checked = true;
      this.eCANAL_2.CheckState = CheckState.Indeterminate;
      this.eCANAL_2.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_2.Location = new Point(22, 177);
      this.eCANAL_2.Name = "eCANAL_2";
      this.eCANAL_2.Size = new Size(33, 24);
      this.eCANAL_2.TabIndex = 24;
      this.eCANAL_2.Text = "-";
      this.eCANAL_2.UseVisualStyleBackColor = true;
      this.eCANAL_1.AutoSize = true;
      this.eCANAL_1.Checked = true;
      this.eCANAL_1.CheckState = CheckState.Indeterminate;
      this.eCANAL_1.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.eCANAL_1.Location = new Point(22, 145);
      this.eCANAL_1.Name = "eCANAL_1";
      this.eCANAL_1.Size = new Size(33, 24);
      this.eCANAL_1.TabIndex = 23;
      this.eCANAL_1.Text = "-";
      this.eCANAL_1.UseVisualStyleBackColor = true;
      this.iCom.AutoSize = true;
      this.iCom.Location = new Point(19, 95);
      this.iCom.Name = "iCom";
      this.iCom.Size = new Size(53, 13);
      this.iCom.TabIndex = 8;
      this.iCom.Text = "COM Port";
      this.lCom.DropDownStyle = ComboBoxStyle.DropDownList;
      this.lCom.Font = new Font("Microsoft Sans Serif", 20f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lCom.FormattingEnabled = true;
      this.lCom.Location = new Point(78, 81);
      this.lCom.Name = "lCom";
      this.lCom.Size = new Size(167, 39);
      this.lCom.TabIndex = 7;
      this.lCom.SelectionChangeCommitted += new EventHandler(this.lCom_SelectionChangeCommitted);
      this.pTitle.BackgroundImage = (Image) componentResourceManager.GetObject("pTitle.BackgroundImage");
      this.pTitle.BackgroundImageLayout = ImageLayout.Stretch;
      this.pTitle.Dock = DockStyle.Top;
      this.pTitle.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pTitle.Location = new Point(0, 0);
      this.pTitle.Name = "pTitle";
      this.pTitle.PageEndColor = Color.Empty;
      this.pTitle.PageStartColor = Color.LightSteelBlue;
      this.pTitle.Size = new Size(649, 60);
      this.pTitle.TabIndex = 6;
      this.pTitle.Title = "F40 Configuration";
      this.pTitle.Title_Alignement = GradientPanel.Alignement.Left;
      this.tResum.Location = new Point(4, 5);
      this.tResum.Name = "tResum";
      this.tResum.Size = new Size(649, 406);
      this.tResum.TabIndex = 10;
      this.tResum.UseVisualStyleBackColor = true;
      this.bgControl.WorkerReportsProgress = true;
      this.bgControl.WorkerSupportsCancellation = true;
      this.bgControl.DoWork += new DoWorkEventHandler(this.bgControl_DoWork);
      this.bgControl.ProgressChanged += new ProgressChangedEventHandler(this.bgControl_ProgressChanged);
      this.AutoScaleMode = AutoScaleMode.None;
      this.ClientSize = new Size(657, 469);
      this.ControlBox = false;
      this.Controls.Add((Control) this.tabs);
      this.Controls.Add((Control) this.pBotons);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Name = nameof (Devices_Wizard);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Devices";
      this.FormClosing += new FormClosingEventHandler(this.Devices_Wizard_FormClosing);
      this.Load += new EventHandler(this.Devices_Wizard_Load);
      this.pBotons.ResumeLayout(false);
      this.tabs.ResumeLayout(false);
      this.tInfo.ResumeLayout(false);
      this.tInfo.PerformLayout();
      this.tCoin_Mode.ResumeLayout(false);
      this.tCoin_Mode.PerformLayout();
      this.tCoin_Detect.ResumeLayout(false);
      this.tCoin_Detect.PerformLayout();
      this.tBill_Mode.ResumeLayout(false);
      this.tBill_Mode.PerformLayout();
      this.tBill_Detect.ResumeLayout(false);
      this.tBill_Detect.PerformLayout();
      this.tBill_Config.ResumeLayout(false);
      this.tBill_Config.PerformLayout();
      this.ResumeLayout(false);
    }

    private enum Wizard
    {
      Nulo,
      StartUp,
      Info,
      CoinMode,
      CoinDetecting,
      CoinDetectingWait_RM5,
      CoinDetectingWait_CCT2,
      CoinDetected,
      BillMode,
      BillDetecting,
      BillDetectingWait,
      BillDetectingWait_SSP,
      BillDetectingWait_SSP3,
      BillDetectingWait_SIO,
      BillDetectingWait_F40,
      BillDetectingWait_Tri,
      BillDetected,
      RM5Config,
      RM5Test,
      CCT2Config,
      CCT2Test,
      SIOConfig,
      SIOTest,
      SSPConfig,
      SSPTest,
      SSP3Config,
      SSP3Test,
      F40Config,
      F40Test,
      TriConfig,
      TriTest,
      Resumen,
    }
  }
}
