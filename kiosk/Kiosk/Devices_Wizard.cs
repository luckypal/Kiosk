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
			Resumen
		}

		public bool OK;

		public Configuracion opciones;

		private bool run = false;

		private bool enint = false;

		private int Canales = 16;

		public string SSP_Com = "?";

		public int[] SSP_Value;

		public int[] SSP_Inhibit;

		public int[] SSP_Enabled;

		public string SSP3_Com = "?";

		public int[] SSP3_Value;

		public int[] SSP3_Inhibit;

		public int[] SSP3_Enabled;

		public string SIO_Com = "?";

		public int[] SIO_Value;

		public int[] SIO_Inhibit;

		public int[] SIO_Enabled;

		public string Tri_Com = "?";

		public int[] Tri_Value;

		public int[] Tri_Inhibit;

		public int[] Tri_Enabled;

		public string F40_Com = "?";

		public int[] F40_Value;

		public int[] F40_Inhibit;

		public int[] F40_Enabled;

		public string RM5_Com = "?";

		public int[] RM5_Value;

		public int[] RM5_Inhibit;

		public int[] RM5_Enabled;

		public string CCT2_Com = "?";

		public int[] CCT2_Value;

		public int[] CCT2_Inhibit;

		public int[] CCT2_Enabled;

		private Control_Comestero rm5 = null;

		private Control_CCTALK_COIN cct2 = null;

		private Control_NV_SSP ssp = null;

		private Control_NV_SSP_P6 ssp3 = null;

		private Control_NV_SIO sio = null;

		private Control_F40_CCTalk f40 = null;

		private Control_Trilogy tri = null;

		private Wizard Fase = Wizard.Nulo;

		public bool CoinDetected = false;

		public string CoinModel = "?";

		public string CoinModel_P = "?";

		public bool BillDetected = false;

		public string BillModel = "?";

		public string BillModel_P = "?";

		private string error;

		private IContainer components = null;

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
			OK = false;
			run = false;
			enint = false;
			Fase = Wizard.Nulo;
			InitializeComponent();
			Fase = Wizard.StartUp;
			Init_Vars();
			Load_Cfg("devices.cfg");
			Localize();
			opciones = _opc;
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		public Devices_Wizard(string _cfg)
		{
			Init_Vars();
			Load_Cfg(_cfg);
		}

		private void Init_Vars()
		{
			SSP_Com = "?";
			SSP_Value = new int[Canales];
			SSP_Inhibit = new int[Canales];
			SSP_Enabled = new int[Canales];
			SSP3_Com = "?";
			SSP3_Value = new int[Canales];
			SSP3_Inhibit = new int[Canales];
			SSP3_Enabled = new int[Canales];
			SIO_Com = "?";
			SIO_Value = new int[Canales];
			SIO_Inhibit = new int[Canales];
			SIO_Enabled = new int[Canales];
			Tri_Com = "?";
			Tri_Value = new int[Canales];
			Tri_Inhibit = new int[Canales];
			Tri_Enabled = new int[Canales];
			F40_Com = "?";
			F40_Value = new int[Canales];
			F40_Inhibit = new int[Canales];
			F40_Enabled = new int[Canales];
			RM5_Com = "?";
			RM5_Value = new int[Canales];
			RM5_Inhibit = new int[Canales];
			RM5_Enabled = new int[Canales];
			CCT2_Com = "?";
			CCT2_Value = new int[Canales];
			CCT2_Inhibit = new int[Canales];
			CCT2_Enabled = new int[Canales];
			CoinModel = "?";
			CoinModel_P = "?";
			BillModel = "?";
			BillModel_P = "?";
		}

		private void Refresh_SSP()
		{
			pTitle.Title = "Configure NV SSP";
			string[] portNames = SerialPort.GetPortNames();
			lCom.Items.Clear();
			lCom.Items.Add("?");
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					lCom.Items.Add(portNames[i]);
				}
			}
			lCom.Text = SSP_Com;
			if (SSP_Com != "?")
			{
				for (int i = 0; i < 16; i++)
				{
					Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
					array[0].Visible = true;
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = CheckState.Unchecked;
					array[0].Text = "0.0";
				}
				return;
			}
			if (ssp == null)
			{
				ssp = new Control_NV_SSP();
				ssp.port = SSP_Com;
				ssp.Open();
			}
			for (int i = 0; i < 16; i++)
			{
				Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
				if (i >= ssp.Canales)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				if (!ssp.GetChannelEnabled(i + 1))
				{
					((CheckBox)array[0]).ThreeState = true;
					((CheckBox)array[0]).CheckState = CheckState.Indeterminate;
				}
				else
				{
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = ((!ssp.GetChannelInhibit(i + 1)) ? CheckState.Checked : CheckState.Unchecked);
				}
				if (ssp.GetChannelValue(i + 1) < ssp.Multiplicador)
				{
					array[0].Text = "0." + ssp.GetChannelValue(i + 1) + " " + ssp.GetChannelCurrency(i + 1);
				}
				else
				{
					array[0].Text = ssp.GetChannelValue(i + 1) / ssp.Multiplicador + " " + ssp.GetChannelCurrency(i + 1);
				}
				array[0].Visible = true;
				array[0].Invalidate();
			}
		}

		private void Refresh_SSP3()
		{
			pTitle.Title = "Configure NV SSP v3";
			string[] portNames = SerialPort.GetPortNames();
			lCom.Items.Clear();
			lCom.Items.Add("?");
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					lCom.Items.Add(portNames[i]);
				}
			}
			lCom.Text = SSP3_Com;
			if (SSP3_Com != "?")
			{
				for (int i = 0; i < 16; i++)
				{
					Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
					array[0].Visible = true;
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = CheckState.Unchecked;
					array[0].Text = "0.0";
				}
				return;
			}
			if (ssp3 == null)
			{
				ssp3 = new Control_NV_SSP_P6();
				ssp3.port = SSP_Com;
				ssp3.Open();
			}
			for (int i = 0; i < 16; i++)
			{
				Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
				if (i >= ssp3.Canales)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				if (!ssp3.GetChannelEnabled(i + 1))
				{
					((CheckBox)array[0]).ThreeState = true;
					((CheckBox)array[0]).CheckState = CheckState.Indeterminate;
				}
				else
				{
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = ((!ssp3.GetChannelInhibit(i + 1)) ? CheckState.Checked : CheckState.Unchecked);
				}
				if (ssp3.GetChannelValue(i + 1) < (decimal)ssp3.Multiplier)
				{
					array[0].Text = "0." + ssp3.GetChannelValue(i + 1) + " " + ssp3.GetChannelCurrency(i + 1);
				}
				else
				{
					array[0].Text = ssp3.GetChannelValue(i + 1) / (decimal)ssp3.Multiplier + " " + ssp3.GetChannelCurrency(i + 1);
				}
				array[0].Visible = true;
				array[0].Invalidate();
			}
		}

		private void Refresh_F40()
		{
			pTitle.Title = "Configure PayPrint F40 CCTalk";
			string[] portNames = SerialPort.GetPortNames();
			lCom.Items.Clear();
			lCom.Items.Add("?");
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					lCom.Items.Add(portNames[i]);
				}
			}
			lCom.Text = F40_Com;
			if (F40_Com != "?")
			{
				for (int i = 0; i < 16; i++)
				{
					Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
					array[0].Visible = true;
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = CheckState.Unchecked;
					array[0].Text = "0.0";
				}
				return;
			}
			if (f40 == null)
			{
				f40 = new Control_F40_CCTalk();
				f40.port = F40_Com;
				f40.Open();
			}
			for (int i = 0; i < 16; i++)
			{
				Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
				if (i >= f40.Canales)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				if (f40.Canal[i] == 0)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				((CheckBox)array[0]).ThreeState = false;
				((CheckBox)array[0]).CheckState = ((f40.eCanal[i] == 1) ? CheckState.Checked : CheckState.Unchecked);
				if ((decimal)f40.Canal[i] < f40.Base)
				{
					array[0].Text = "0." + f40.Canal[i];
				}
				else
				{
					array[0].Text = string.Concat(f40.Canal[i]);
				}
				array[0].Visible = true;
				array[0].Invalidate();
			}
		}

		private void Refresh_Trilogy()
		{
			pTitle.Title = "Configure Pyramid Trilogy";
			string[] portNames = SerialPort.GetPortNames();
			lCom.Items.Clear();
			lCom.Items.Add("?");
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					lCom.Items.Add(portNames[i]);
				}
			}
			lCom.Text = Tri_Com;
			if (Tri_Com != "?")
			{
				for (int i = 0; i < 16; i++)
				{
					Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
					array[0].Visible = true;
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = CheckState.Unchecked;
					array[0].Text = "0.0";
				}
				return;
			}
			if (tri == null)
			{
				tri = new Control_Trilogy();
				tri.port = Tri_Com;
			}
			for (int i = 0; i < 16; i++)
			{
				Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
				if (i >= tri.Canales)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				if (tri.Canal[i] == 0)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				((CheckBox)array[0]).ThreeState = false;
				((CheckBox)array[0]).CheckState = ((tri.eCanal[i] == 1) ? CheckState.Checked : CheckState.Unchecked);
				if ((decimal)tri.Canal[i] < tri.Base)
				{
					array[0].Text = "0." + tri.Canal[i];
				}
				else
				{
					array[0].Text = string.Concat(tri.Canal[i]);
				}
				array[0].Visible = true;
				array[0].Invalidate();
			}
		}

		private void Refresh_SIO()
		{
			pTitle.Title = "Configure NV SIO";
			string[] portNames = SerialPort.GetPortNames();
			lCom.Items.Clear();
			lCom.Items.Add("?");
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					lCom.Items.Add(portNames[i]);
				}
			}
			lCom.Text = SIO_Com;
			if (SIO_Com != "?")
			{
				for (int i = 0; i < 16; i++)
				{
					Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
					array[0].Visible = true;
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = CheckState.Unchecked;
					array[0].Text = "0.0";
				}
				return;
			}
			if (sio == null)
			{
				sio = new Control_NV_SIO();
				sio.port = SIO_Com;
				sio.Open();
			}
			for (int i = 0; i < 16; i++)
			{
				Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
				if (i >= sio.Canales)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				if (sio.Canal[i] == 0)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				((CheckBox)array[0]).ThreeState = false;
				((CheckBox)array[0]).CheckState = ((sio.eCanal[i] == 1) ? CheckState.Checked : CheckState.Unchecked);
				if ((decimal)sio.Canal[i] < sio.Base)
				{
					array[0].Text = "0." + sio.Canal[i];
				}
				else
				{
					array[0].Text = string.Concat(sio.Canal[i]);
				}
				array[0].Visible = true;
				array[0].Invalidate();
			}
		}

		private void Refresh_RM5()
		{
			pTitle.Title = "Configure Comestero RM5";
			string[] portNames = SerialPort.GetPortNames();
			lCom.Items.Clear();
			lCom.Items.Add("?");
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					lCom.Items.Add(portNames[i]);
				}
			}
			lCom.Text = RM5_Com;
			if (RM5_Com != "?")
			{
				for (int i = 0; i < 16; i++)
				{
					Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
					array[0].Visible = true;
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = CheckState.Unchecked;
					array[0].Text = "0.0";
				}
				return;
			}
			if (rm5 == null)
			{
				rm5 = new Control_Comestero();
				rm5.port = RM5_Com;
				rm5.Open();
			}
			for (int i = 0; i < 16; i++)
			{
				Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
				if (i >= rm5.Canales)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				if (rm5.Canal[i] == 0)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				((CheckBox)array[0]).ThreeState = false;
				((CheckBox)array[0]).CheckState = ((rm5.eCanal[i] == 1) ? CheckState.Checked : CheckState.Unchecked);
				if ((decimal)rm5.Canal[i] < rm5.Base)
				{
					array[0].Text = "0." + rm5.Canal[i];
				}
				else
				{
					array[0].Text = string.Concat(rm5.Canal[i]);
				}
				array[0].Visible = true;
				array[0].Invalidate();
			}
		}

		private void Refresh_CCT2()
		{
			pTitle.Title = "Configure CCTalk COIN ACCEPTOR (ID 2)";
			string[] portNames = SerialPort.GetPortNames();
			lCom.Items.Clear();
			lCom.Items.Add("?");
			if (portNames != null)
			{
				for (int i = 0; i < portNames.Length; i++)
				{
					lCom.Items.Add(portNames[i]);
				}
			}
			lCom.Text = CCT2_Com;
			if (CCT2_Com != "?")
			{
				for (int i = 0; i < 16; i++)
				{
					Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
					array[0].Visible = true;
					((CheckBox)array[0]).ThreeState = false;
					((CheckBox)array[0]).CheckState = CheckState.Unchecked;
					array[0].Text = "0.0";
				}
				return;
			}
			if (cct2 == null)
			{
				cct2 = new Control_CCTALK_COIN();
				cct2.port = CCT2_Com;
				cct2.Open();
			}
			for (int i = 0; i < 16; i++)
			{
				Control[] array = tBill_Config.Controls.Find("eCANAL_" + (i + 1), searchAllChildren: false);
				if (i >= cct2.Canales)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				if (cct2.Canal[i] == 0)
				{
					array[0].Visible = false;
					array[0].Invalidate();
					continue;
				}
				((CheckBox)array[0]).ThreeState = false;
				((CheckBox)array[0]).CheckState = ((cct2.eCanal[i] == 1) ? CheckState.Checked : CheckState.Unchecked);
				if (cct2.Canal[i] < cct2.Base)
				{
					array[0].Text = "0." + cct2.Canal[i];
				}
				else
				{
					array[0].Text = string.Concat(cct2.Canal[i]);
				}
				array[0].Visible = true;
				array[0].Invalidate();
			}
		}

		private void SetFase(Wizard _f)
		{
			switch (_f)
			{
			case Wizard.CoinMode:
			case Wizard.CoinDetected:
			case Wizard.BillMode:
			case Wizard.BillDetected:
			case Wizard.RM5Config:
			case Wizard.CCT2Config:
			case Wizard.SIOConfig:
			case Wizard.SSPConfig:
			case Wizard.SSP3Config:
			case Wizard.F40Config:
			case Wizard.TriConfig:
				bAnt.Enabled = true;
				bSig.Enabled = true;
				bOK.Enabled = false;
				break;
			case Wizard.Resumen:
				bAnt.Enabled = true;
				bSig.Enabled = false;
				bOK.Enabled = true;
				break;
			case Wizard.Nulo:
			case Wizard.StartUp:
			case Wizard.CoinDetecting:
			case Wizard.BillDetecting:
				bAnt.Enabled = false;
				bSig.Enabled = false;
				bOK.Enabled = false;
				break;
			case Wizard.Info:
				bAnt.Enabled = false;
				bSig.Enabled = true;
				bOK.Enabled = false;
				break;
			}
			switch (_f)
			{
			case Wizard.CoinMode:
				tabs.SelectTab("tCoin_Mode");
				break;
			case Wizard.BillMode:
				tabs.SelectTab("tBill_Mode");
				break;
			case Wizard.CoinDetecting:
				rCoin_M1.Checked = false;
				rCoin_M2.Checked = false;
				rCoin_M3.Checked = false;
				tabs.SelectTab("tCoin_Detect");
				break;
			case Wizard.CoinDetected:
				tabs.SelectTab("tCoin_Detect");
				break;
			case Wizard.BillDetecting:
				rBill_M1.Checked = false;
				rBill_M2.Checked = false;
				rBill_M3.Checked = false;
				tabs.SelectTab("tBill_Detect");
				break;
			case Wizard.BillDetected:
				pBill.Visible = false;
				tabs.SelectTab("tBill_Detect");
				break;
			case Wizard.RM5Config:
				Refresh_RM5();
				tabs.SelectTab("tBill_Config");
				break;
			case Wizard.CCT2Config:
				Refresh_CCT2();
				tabs.SelectTab("tBill_Config");
				break;
			case Wizard.SSPConfig:
				Refresh_SSP();
				tabs.SelectTab("tBill_Config");
				break;
			case Wizard.SSP3Config:
				Refresh_SSP3();
				tabs.SelectTab("tBill_Config");
				break;
			case Wizard.SIOConfig:
				Refresh_SIO();
				tabs.SelectTab("tBill_Config");
				break;
			case Wizard.F40Config:
				Refresh_F40();
				tabs.SelectTab("tBill_Config");
				break;
			case Wizard.TriConfig:
				Refresh_Trilogy();
				tabs.SelectTab("tBill_Config");
				break;
			case Wizard.Resumen:
				tabs.SelectTab("tResum");
				break;
			case Wizard.Info:
				tabs.SelectTab("tInfo");
				break;
			}
			Fase = _f;
		}

		private void bSig_Click(object sender, EventArgs e)
		{
			switch (Fase)
			{
			case Wizard.Info:
				SetFase(Wizard.CoinMode);
				break;
			case Wizard.CoinMode:
				if (rCoin_M1.Checked)
				{
					SetFase(Wizard.CoinDetecting);
				}
				if (rCoin_M2.Checked)
				{
					CoinModel = "-";
					SetFase(Wizard.BillMode);
				}
				if (rCoin_M3.Checked)
				{
					SetFase(Wizard.CCT2Config);
				}
				break;
			case Wizard.CoinDetected:
				if (CoinDetected)
				{
					switch (CoinModel.ToLower())
					{
					case "rm5":
						SetFase(Wizard.RM5Config);
						break;
					case "cct2":
						SetFase(Wizard.CCT2Config);
						break;
					default:
						SetFase(Wizard.BillMode);
						break;
					}
				}
				else
				{
					SetFase(Wizard.BillMode);
				}
				break;
			case Wizard.BillMode:
				if (rBill_M1.Checked)
				{
					SetFase(Wizard.BillDetecting);
				}
				if (rBill_M2.Checked)
				{
					BillModel = "-";
					SetFase(Wizard.Resumen);
				}
				if (rBill_M3.Checked)
				{
					SetFase(Wizard.SSPConfig);
				}
				break;
			case Wizard.BillDetected:
				pBill.Visible = false;
				if (BillDetected)
				{
					switch (BillModel)
					{
					case "ssp":
						if (ssp == null)
						{
							ssp = new Control_NV_SSP();
							ssp.Start_Find_Device();
							Thread.Sleep(100);
							ssp.Poll();
							Thread.Sleep(100);
							ssp.Poll();
							Thread.Sleep(100);
						}
						SetFase(Wizard.SSPConfig);
						break;
					case "ssp3":
						if (ssp3 == null)
						{
							ssp3 = new Control_NV_SSP_P6();
							ssp3.Start_Find_Device();
							Thread.Sleep(100);
							ssp3.Poll();
							Thread.Sleep(100);
							ssp3.Poll();
							Thread.Sleep(100);
						}
						SetFase(Wizard.SSP3Config);
						break;
					case "f40":
						SetFase(Wizard.F40Config);
						break;
					case "sio":
						SetFase(Wizard.SIOConfig);
						break;
					case "tri":
						SetFase(Wizard.TriConfig);
						break;
					default:
						SetFase(Wizard.Resumen);
						break;
					}
				}
				else
				{
					SetFase(Wizard.Resumen);
				}
				break;
			case Wizard.RM5Config:
				if (RM5_Com != "-" && RM5_Com != "?")
				{
					for (int i = 0; i < Canales; i++)
					{
						if (i < rm5.Canales)
						{
							RM5_Enabled[i] = (rm5.GetChannelEnabled(i + 1) ? 1 : 0);
							RM5_Inhibit[i] = ((!rm5.GetChannelInhibit(i + 1)) ? 1 : 0);
							RM5_Value[i] = rm5.GetChannelValue(i + 1);
						}
						else
						{
							RM5_Enabled[i] = (RM5_Inhibit[i] = (RM5_Value[i] = 0));
						}
					}
					CoinModel = "rm5";
					CoinModel_P = RM5_Com;
				}
				SetFase(Wizard.BillMode);
				break;
			case Wizard.CCT2Config:
				if (CCT2_Com != "-" && CCT2_Com != "?")
				{
					for (int i = 0; i < Canales; i++)
					{
						if (i < cct2.Canales)
						{
							CCT2_Enabled[i] = (cct2.GetChannelEnabled(i + 1) ? 1 : 0);
							CCT2_Inhibit[i] = ((!cct2.GetChannelInhibit(i + 1)) ? 1 : 0);
							CCT2_Value[i] = (int)cct2.GetChannelValue(i + 1);
						}
						else
						{
							CCT2_Enabled[i] = (CCT2_Inhibit[i] = (CCT2_Value[i] = 0));
						}
					}
					CoinModel = "cct2";
					CoinModel_P = CCT2_Com;
					SetFase(Wizard.BillMode);
				}
				else
				{
					SetFase(Wizard.RM5Config);
				}
				break;
			case Wizard.SSPConfig:
				if (SSP_Com != "-" && SSP_Com != "?")
				{
					for (int i = 0; i < Canales; i++)
					{
						if (i < ssp.Canales)
						{
							SSP_Enabled[i] = (ssp.GetChannelEnabled(i + 1) ? 1 : 0);
							SSP_Inhibit[i] = ((!ssp.GetChannelInhibit(i + 1)) ? 1 : 0);
							SSP_Value[i] = ssp.GetChannelValue(i + 1);
						}
						else
						{
							SSP_Enabled[i] = (SSP_Inhibit[i] = (SSP_Value[i] = 0));
						}
					}
					BillModel = "ssp";
					BillModel_P = SSP_Com;
					SetFase(Wizard.Resumen);
				}
				else
				{
					SetFase(Wizard.SIOConfig);
				}
				break;
			case Wizard.SSP3Config:
				if (SSP3_Com != "-" && SSP3_Com != "?")
				{
					for (int i = 0; i < Canales; i++)
					{
						if (i < ssp3.Canales)
						{
							SSP3_Enabled[i] = (ssp3.GetChannelEnabled(i + 1) ? 1 : 0);
							SSP3_Inhibit[i] = ((!ssp3.GetChannelInhibit(i + 1)) ? 1 : 0);
							SSP3_Value[i] = (int)ssp3.GetChannelValue(i + 1);
						}
						else
						{
							SSP3_Enabled[i] = (SSP3_Inhibit[i] = (SSP3_Value[i] = 0));
						}
					}
					BillModel = "ssp3";
					BillModel_P = SSP3_Com;
					SetFase(Wizard.Resumen);
				}
				else
				{
					SetFase(Wizard.SIOConfig);
				}
				break;
			case Wizard.SIOConfig:
				if (SIO_Com != "-" && SIO_Com != "?")
				{
					for (int i = 0; i < Canales; i++)
					{
						if (i < sio.Canales)
						{
							SIO_Enabled[i] = (sio.GetChannelEnabled(i + 1) ? 1 : 0);
							SIO_Inhibit[i] = ((!sio.GetChannelInhibit(i + 1)) ? 1 : 0);
							SIO_Value[i] = sio.GetChannelValue(i + 1);
						}
						else
						{
							SIO_Enabled[i] = (SIO_Inhibit[i] = (SIO_Value[i] = 0));
						}
					}
					BillModel = "sio";
					BillModel_P = SIO_Com;
					SetFase(Wizard.Resumen);
				}
				else
				{
					SetFase(Wizard.TriConfig);
				}
				break;
			case Wizard.TriConfig:
				if (Tri_Com != "-" && Tri_Com != "?")
				{
					for (int i = 0; i < Canales; i++)
					{
						if (i < tri.Canales)
						{
							Tri_Enabled[i] = (tri.GetChannelEnabled(i + 1) ? 1 : 0);
							Tri_Inhibit[i] = ((!tri.GetChannelInhibit(i + 1)) ? 1 : 0);
							Tri_Value[i] = tri.GetChannelValue(i + 1);
						}
						else
						{
							Tri_Enabled[i] = (Tri_Inhibit[i] = (Tri_Value[i] = 0));
						}
					}
					BillModel = "tri";
					BillModel_P = Tri_Com;
					SetFase(Wizard.Resumen);
				}
				else
				{
					SetFase(Wizard.F40Config);
				}
				break;
			case Wizard.F40Config:
				if (F40_Com != "-" && F40_Com != "?")
				{
					for (int i = 0; i < Canales; i++)
					{
						if (i < f40.Canales)
						{
							F40_Enabled[i] = (f40.GetChannelEnabled(i + 1) ? 1 : 0);
							F40_Inhibit[i] = ((!f40.GetChannelInhibit(i + 1)) ? 1 : 0);
							F40_Value[i] = f40.GetChannelValue(i + 1);
						}
						else
						{
							F40_Enabled[i] = (F40_Inhibit[i] = (F40_Value[i] = 0));
						}
					}
					BillModel = "f40";
					BillModel_P = F40_Com;
				}
				SetFase(Wizard.Resumen);
				break;
			}
		}

		private void Devices_Wizard_Load(object sender, EventArgs e)
		{
			run = true;
			bgControl.RunWorkerAsync();
			SetFase(Wizard.Info);
		}

		private void Devices_Wizard_FormClosing(object sender, FormClosingEventArgs e)
		{
			run = false;
			bgControl.CancelAsync();
			bgControl.Dispose();
			if (sio != null)
			{
				sio.Close();
			}
			if (ssp != null)
			{
				ssp.Close();
			}
			if (ssp3 != null)
			{
				ssp3.Close();
			}
			if (rm5 != null)
			{
				rm5.Close();
			}
			if (cct2 != null)
			{
				cct2.Close();
			}
			if (f40 != null)
			{
				f40.Close();
			}
			if (tri != null)
			{
				tri.Close();
			}
		}

		private void bgControl_DoWork(object sender, DoWorkEventArgs e)
		{
			while (run)
			{
				bgControl.ReportProgress(0);
				Thread.Sleep(100);
			}
		}

		private void bgControl_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (enint)
			{
				return;
			}
			enint = true;
			switch (Fase)
			{
			case Wizard.CoinDetecting:
				SetFase(Wizard.CoinDetectingWait_CCT2);
				CoinModel = "?";
				CoinModel_P = "?";
				pCoin.Visible = true;
				pCoin.Invalidate();
				break;
			case Wizard.CoinDetectingWait_RM5:
				if (rm5 == null)
				{
					rm5 = new Control_Comestero();
					rm5.Start_Find_Device();
				}
				Thread.Sleep(100);
				if (rm5.Poll_Find_Device())
				{
					rm5.Stop_Find_Device();
					if (rm5._f_resp_scom != "-")
					{
						CoinDetected = true;
						CoinModel = "rm5";
						RM5_Com = rm5._f_resp_scom;
						dRM5.CheckState = CheckState.Checked;
						dRM5.Checked = true;
					}
					else
					{
						RM5_Com = "-";
						dRM5.CheckState = CheckState.Indeterminate;
					}
					pCoin.Visible = false;
					SetFase(Wizard.CoinDetected);
				}
				pCoin.Invalidate();
				break;
			case Wizard.CoinDetectingWait_CCT2:
				if (cct2 == null)
				{
					cct2 = new Control_CCTALK_COIN();
					cct2.Start_Find_Device();
				}
				Thread.Sleep(100);
				if (cct2.Poll_Find_Device())
				{
					cct2.Stop_Find_Device();
					if (cct2._f_resp_scom != "-")
					{
						CoinDetected = true;
						CoinModel = "cct2";
						CCT2_Com = cct2._f_resp_scom;
						dCCT2.CheckState = CheckState.Checked;
						dCCT2.Checked = true;
						pCoin.Visible = false;
						SetFase(Wizard.CoinDetected);
					}
					else
					{
						CCT2_Com = "-";
						dCCT2.CheckState = CheckState.Indeterminate;
						pCoin.Visible = false;
						SetFase(Wizard.CoinDetectingWait_RM5);
					}
				}
				pCoin.Invalidate();
				break;
			case Wizard.BillDetecting:
				BillModel = "?";
				BillModel_P = "?";
				SetFase(Wizard.BillDetectingWait);
				pBill.Visible = true;
				pBill.Invalidate();
				break;
			case Wizard.BillDetectingWait:
				SetFase(Wizard.BillDetectingWait_SSP);
				break;
			case Wizard.BillDetectingWait_SSP:
				pBill.Invalidate();
				if (ssp == null)
				{
					ssp = new Control_NV_SSP();
					ssp.Start_Find_Device();
				}
				Thread.Sleep(100);
				if (ssp.Poll_Find_Device())
				{
					ssp.Stop_Find_Device();
					if (ssp._f_resp_scom != "-")
					{
						BillDetected = true;
						BillModel = "ssp";
						SSP_Com = ssp._f_resp_scom;
						dNV9_SSP.CheckState = CheckState.Checked;
						dNV9_SSP.Checked = true;
						dNV9_SSP.Text = "NV SSP (" + SSP_Com + ")";
						SetFase(Wizard.BillDetected);
					}
					else
					{
						SSP_Com = "-";
						dNV9_SSP.CheckState = CheckState.Indeterminate;
						SetFase(Wizard.BillDetectingWait_SSP3);
					}
					pBill.Visible = false;
					ssp.Close();
					ssp = null;
				}
				Thread.Sleep(100);
				break;
			case Wizard.BillDetectingWait_SSP3:
				pBill.Invalidate();
				if (ssp3 == null)
				{
					ssp3 = new Control_NV_SSP_P6();
					ssp3.Start_Find_Device();
				}
				Thread.Sleep(100);
				if (ssp3.Poll_Find_Device())
				{
					ssp3.Stop_Find_Device();
					if (ssp3._f_resp_scom != "-")
					{
						BillDetected = true;
						BillModel = "ssp3";
						SSP3_Com = ssp3._f_resp_scom;
						dNV9_SSP3.CheckState = CheckState.Checked;
						dNV9_SSP3.Checked = true;
						dNV9_SSP3.Text = "NV SSP v3 (" + SSP3_Com + ")";
						SetFase(Wizard.BillDetected);
					}
					else
					{
						SSP3_Com = "-";
						dNV9_SSP3.CheckState = CheckState.Indeterminate;
						SetFase(Wizard.BillDetectingWait_SIO);
					}
					pBill.Visible = false;
					ssp3.Close();
					ssp3 = null;
				}
				Thread.Sleep(100);
				break;
			case Wizard.BillDetectingWait_F40:
				pBill.Invalidate();
				if (f40 == null)
				{
					f40 = new Control_F40_CCTalk();
					f40.Start_Find_Device();
				}
				Thread.Sleep(100);
				F40_Com = f40.Find_Device();
				if (f40._f_resp_scom != "-" && CoinModel_P != F40_Com)
				{
					BillDetected = true;
					BillModel = "f40";
					dF40.CheckState = CheckState.Checked;
					dF40.Checked = true;
					dF40.Text = "ccTalk (ID 40) (" + F40_Com + ")";
					SetFase(Wizard.BillDetected);
				}
				else
				{
					F40_Com = "-";
					dF40.CheckState = CheckState.Indeterminate;
					SetFase(Wizard.BillDetectingWait_Tri);
				}
				f40.Close();
				f40 = null;
				Thread.Sleep(100);
				break;
			case Wizard.BillDetectingWait_Tri:
				pBill.Invalidate();
				if (tri == null)
				{
					tri = new Control_Trilogy();
					tri.Start_Find_Device();
				}
				Thread.Sleep(100);
				Tri_Com = tri.Find_Device();
				if (tri._f_resp_scom != "-")
				{
					BillDetected = true;
					BillModel = "tri";
					dTrilogy.CheckState = CheckState.Checked;
					dTrilogy.Checked = true;
					dTrilogy.Text = "Trilogy (" + Tri_Com + ")";
					SetFase(Wizard.BillDetected);
				}
				else
				{
					Tri_Com = "-";
					dTrilogy.CheckState = CheckState.Indeterminate;
					SetFase(Wizard.BillDetected);
				}
				tri.Close();
				tri = null;
				Thread.Sleep(100);
				break;
			case Wizard.BillDetectingWait_SIO:
				pBill.Invalidate();
				if (sio == null)
				{
					sio = new Control_NV_SIO();
				}
				Thread.Sleep(100);
				SIO_Com = sio.Find_Device();
				if (sio._f_resp_scom != "-")
				{
					BillDetected = true;
					BillModel = "sio";
					dNV_SIO.CheckState = CheckState.Checked;
					dNV_SIO.Checked = true;
					dNV_SIO.Text = "NV SIO (300bps) (" + SIO_Com + ")";
					SetFase(Wizard.BillDetected);
				}
				else
				{
					SIO_Com = "-";
					dNV_SIO.CheckState = CheckState.Indeterminate;
					SetFase(Wizard.BillDetectingWait_F40);
				}
				sio.Close();
				sio = null;
				Thread.Sleep(100);
				break;
			case Wizard.SSPConfig:
				if (ssp != null)
				{
					ssp.Poll();
				}
				else if (SSP_Com != "-" && SSP_Com != "?")
				{
					ssp = new Control_NV_SSP();
					ssp.port = SSP_Com;
					ssp.Open();
				}
				break;
			case Wizard.SSP3Config:
				if (ssp3 != null)
				{
					ssp3.Poll();
				}
				else if (SSP3_Com != "-" && SSP3_Com != "?")
				{
					ssp3 = new Control_NV_SSP_P6();
					ssp3.port = SSP3_Com;
					ssp3.Open();
				}
				break;
			case Wizard.F40Config:
				if (f40 != null)
				{
					f40.Poll();
				}
				else if (F40_Com != "-" && F40_Com != "?")
				{
					f40 = new Control_F40_CCTalk();
					f40.port = F40_Com;
					f40.Open();
				}
				break;
			case Wizard.TriConfig:
				if (tri != null)
				{
					tri.Poll();
				}
				else if (Tri_Com != "-" && Tri_Com != "?")
				{
					tri = new Control_Trilogy();
					tri.port = Tri_Com;
					tri.Open();
				}
				break;
			case Wizard.SIOConfig:
				if (sio != null)
				{
					sio.Poll();
					sio.Parser();
				}
				else if (SIO_Com != "-" && SIO_Com != "?")
				{
					sio = new Control_NV_SIO();
					sio.port = SIO_Com;
					sio.Open();
				}
				break;
			case Wizard.BillMode:
				if (f40 != null)
				{
					f40.Poll();
				}
				if (tri != null)
				{
					tri.Poll();
				}
				if (ssp != null)
				{
					ssp.Poll();
				}
				if (ssp3 != null)
				{
					ssp3.Poll();
				}
				if (sio != null)
				{
					sio.Poll();
				}
				break;
			}
			enint = false;
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		public void Save(string _cfg)
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg;
			string text2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + _cfg + ".tmp";
			XmlTextWriter xmlTextWriter = new XmlTextWriter(text2, Encoding.ASCII);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			try
			{
				xmlTextWriter.WriteStartElement("config".ToLower());
				xmlTextWriter.WriteStartElement("select".ToLower());
				xmlTextWriter.WriteAttributeString("coin".ToLower(), CoinModel);
				xmlTextWriter.WriteAttributeString("coin_p".ToLower(), CoinModel_P);
				xmlTextWriter.WriteAttributeString("bnv".ToLower(), BillModel);
				xmlTextWriter.WriteAttributeString("bnv_p".ToLower(), BillModel_P);
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("devices".ToLower());
				xmlTextWriter.WriteStartElement("ssp");
				xmlTextWriter.WriteAttributeString("com", SSP_Com);
				for (int i = 0; i < Canales; i++)
				{
					xmlTextWriter.WriteStartElement("channel_" + (i + 1));
					if (SSP_Enabled[i] == 1)
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), SSP_Value[i].ToString());
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), SSP_Enabled[i].ToString());
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), SSP_Inhibit[i].ToString());
					}
					else
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), "1");
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("ssp3");
				xmlTextWriter.WriteAttributeString("com", SSP3_Com);
				for (int i = 0; i < Canales; i++)
				{
					xmlTextWriter.WriteStartElement("channel_" + (i + 1));
					if (SSP3_Enabled[i] == 1)
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), SSP3_Value[i].ToString());
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), SSP3_Enabled[i].ToString());
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), SSP3_Inhibit[i].ToString());
					}
					else
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), "1");
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("sio");
				xmlTextWriter.WriteAttributeString("com", SIO_Com);
				for (int i = 0; i < Canales; i++)
				{
					xmlTextWriter.WriteStartElement("channel_" + (i + 1));
					if (SIO_Enabled[i] == 1)
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), SIO_Value[i].ToString());
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), SIO_Enabled[i].ToString());
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), SIO_Inhibit[i].ToString());
					}
					else
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), "1");
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("rm5");
				xmlTextWriter.WriteAttributeString("com", RM5_Com);
				for (int i = 0; i < Canales; i++)
				{
					xmlTextWriter.WriteStartElement("channel_" + (i + 1));
					if (RM5_Enabled[i] == 1)
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), RM5_Value[i].ToString());
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), RM5_Enabled[i].ToString());
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), RM5_Inhibit[i].ToString());
					}
					else
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), "1");
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("f40");
				xmlTextWriter.WriteAttributeString("com", F40_Com);
				for (int i = 0; i < Canales; i++)
				{
					xmlTextWriter.WriteStartElement("channel_" + (i + 1));
					if (F40_Enabled[i] == 1)
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), F40_Value[i].ToString());
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), F40_Enabled[i].ToString());
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), F40_Inhibit[i].ToString());
					}
					else
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), "1");
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("tri");
				xmlTextWriter.WriteAttributeString("com", Tri_Com);
				for (int i = 0; i < Canales; i++)
				{
					xmlTextWriter.WriteStartElement("channel_" + (i + 1));
					if (Tri_Enabled[i] == 1)
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), Tri_Value[i].ToString());
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), Tri_Enabled[i].ToString());
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), Tri_Inhibit[i].ToString());
					}
					else
					{
						xmlTextWriter.WriteAttributeString("value" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("enabled" + (i + 1), "0");
						xmlTextWriter.WriteAttributeString("inhibit" + (i + 1), "1");
					}
					xmlTextWriter.WriteEndElement();
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
			}
			catch (Exception ex)
			{
				error = ex.Message;
				xmlTextWriter.Flush();
				xmlTextWriter.Close();
				File.Delete(text2);
				return;
			}
			xmlTextWriter.Flush();
			xmlTextWriter.Close();
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			File.Copy(text2, text);
			File.Delete(text2);
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
			catch (Exception)
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
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "coin".ToLower())
								{
									CoinModel = xmlTextReader.Value;
								}
								else if (xmlTextReader.Name.ToLower() == "coin_p".ToLower())
								{
									CoinModel_P = xmlTextReader.Value;
								}
								else if (xmlTextReader.Name.ToLower() == "bnv".ToLower())
								{
									BillModel = xmlTextReader.Value;
								}
								else if (xmlTextReader.Name.ToLower() == "bnv_p".ToLower())
								{
									BillModel_P = xmlTextReader.Value;
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "ssp".ToLower() && xmlTextReader.HasAttributes)
						{
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "com".ToLower())
								{
									SSP_Com = xmlTextReader.Value;
								}
								else
								{
									for (int j = 0; j < Canales; j++)
									{
										string text = "channel" + (j + 1);
										if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											SSP_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
										else if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											SSP_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
									}
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "ssp3".ToLower() && xmlTextReader.HasAttributes)
						{
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "com".ToLower())
								{
									SSP3_Com = xmlTextReader.Value;
								}
								else
								{
									for (int j = 0; j < Canales; j++)
									{
										string text = "channel" + (j + 1);
										if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											SSP3_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
										else if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											SSP3_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
									}
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "sio".ToLower() && xmlTextReader.HasAttributes)
						{
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "com".ToLower())
								{
									SIO_Com = xmlTextReader.Value;
								}
								else
								{
									for (int j = 0; j < Canales; j++)
									{
										string text = "channel" + (j + 1);
										if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											SIO_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
										else if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											SIO_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
									}
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "rm5".ToLower() && xmlTextReader.HasAttributes)
						{
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "com".ToLower())
								{
									RM5_Com = xmlTextReader.Value;
								}
								else
								{
									for (int j = 0; j < Canales; j++)
									{
										string text = "channel" + (j + 1);
										if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											RM5_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
										else if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											RM5_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
									}
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "tri".ToLower() && xmlTextReader.HasAttributes)
						{
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "com".ToLower())
								{
									Tri_Com = xmlTextReader.Value;
								}
								else
								{
									for (int j = 0; j < Canales; j++)
									{
										string text = "channel" + (j + 1);
										if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											Tri_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
										else if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											Tri_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
									}
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "f40".ToLower() && xmlTextReader.HasAttributes)
						{
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "com".ToLower())
								{
									F40_Com = xmlTextReader.Value;
								}
								else
								{
									for (int j = 0; j < Canales; j++)
									{
										string text = "channel" + (j + 1);
										if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											F40_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
										else if (xmlTextReader.Name.ToLower() == text.ToLower())
										{
											F40_Value[j] = Convert.ToInt32(xmlTextReader.Value);
										}
									}
								}
							}
						}
					}
				}
				xmlTextReader.Close();
			}
			catch (Exception ex2)
			{
				error = ex2.Message;
				return false;
			}
			return true;
		}

		private void bOK_Click(object sender, EventArgs e)
		{
			Save("devices.cfg");
			if (opciones != null)
			{
				opciones.Dev_BNV = BillModel;
				opciones.Dev_BNV_P = BillModel_P;
				opciones.Dev_Coin = CoinModel;
				opciones.Dev_Coin_P = CoinModel_P;
			}
			OK = true;
			Close();
		}

		private void lCom_SelectionChangeCommitted(object sender, EventArgs e)
		{
			switch (Fase)
			{
			case Wizard.SSPConfig:
				SSP_Com = lCom.Text;
				break;
			case Wizard.SSP3Config:
				SSP3_Com = lCom.Text;
				break;
			case Wizard.SIOConfig:
				SIO_Com = lCom.Text;
				break;
			case Wizard.TriConfig:
				Tri_Com = lCom.Text;
				break;
			case Wizard.RM5Config:
				RM5_Com = lCom.Text;
				break;
			case Wizard.F40Config:
				F40_Com = lCom.Text;
				break;
			}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kiosk.Devices_Wizard));
			pBotons = new System.Windows.Forms.Panel();
			bCancel = new System.Windows.Forms.Button();
			bSig = new System.Windows.Forms.Button();
			bAnt = new System.Windows.Forms.Button();
			bOK = new System.Windows.Forms.Button();
			tabs = new System.Windows.Forms.TabControl();
			tInfo = new System.Windows.Forms.TabPage();
			hInfo = new GLib.Forms.GradientPanel();
			infoWizard = new System.Windows.Forms.TextBox();
			tCoin_Mode = new System.Windows.Forms.TabPage();
			rCoin_M3 = new System.Windows.Forms.RadioButton();
			rCoin_M1 = new System.Windows.Forms.RadioButton();
			rCoin_M2 = new System.Windows.Forms.RadioButton();
			hCoin_Mode = new GLib.Forms.GradientPanel();
			tCoin_Detect = new System.Windows.Forms.TabPage();
			dCCT2 = new System.Windows.Forms.CheckBox();
			pCoin = new System.Windows.Forms.ProgressBar();
			dRM5 = new System.Windows.Forms.CheckBox();
			hCoin_Detect = new GLib.Forms.GradientPanel();
			tBill_Mode = new System.Windows.Forms.TabPage();
			rBill_M3 = new System.Windows.Forms.RadioButton();
			rBill_M1 = new System.Windows.Forms.RadioButton();
			rBill_M2 = new System.Windows.Forms.RadioButton();
			pBillMode = new GLib.Forms.GradientPanel();
			tBill_Detect = new System.Windows.Forms.TabPage();
			dNV_SIO = new System.Windows.Forms.CheckBox();
			pBill = new System.Windows.Forms.ProgressBar();
			dTrilogy = new System.Windows.Forms.CheckBox();
			dF40 = new System.Windows.Forms.CheckBox();
			dNV9_SSP = new System.Windows.Forms.CheckBox();
			dNV9_SSP3 = new System.Windows.Forms.CheckBox();
			pBillDetect = new GLib.Forms.GradientPanel();
			tBill_Config = new System.Windows.Forms.TabPage();
			eCANAL_16 = new System.Windows.Forms.CheckBox();
			eCANAL_15 = new System.Windows.Forms.CheckBox();
			eCANAL_14 = new System.Windows.Forms.CheckBox();
			eCANAL_13 = new System.Windows.Forms.CheckBox();
			eCANAL_12 = new System.Windows.Forms.CheckBox();
			eCANAL_11 = new System.Windows.Forms.CheckBox();
			eCANAL_10 = new System.Windows.Forms.CheckBox();
			eCANAL_9 = new System.Windows.Forms.CheckBox();
			eCANAL_8 = new System.Windows.Forms.CheckBox();
			eCANAL_7 = new System.Windows.Forms.CheckBox();
			eCANAL_6 = new System.Windows.Forms.CheckBox();
			eCANAL_5 = new System.Windows.Forms.CheckBox();
			eCANAL_4 = new System.Windows.Forms.CheckBox();
			eCANAL_3 = new System.Windows.Forms.CheckBox();
			eCANAL_2 = new System.Windows.Forms.CheckBox();
			eCANAL_1 = new System.Windows.Forms.CheckBox();
			iCom = new System.Windows.Forms.Label();
			lCom = new System.Windows.Forms.ComboBox();
			pTitle = new GLib.Forms.GradientPanel();
			tResum = new System.Windows.Forms.TabPage();
			bgControl = new System.ComponentModel.BackgroundWorker();
			pBotons.SuspendLayout();
			tabs.SuspendLayout();
			tInfo.SuspendLayout();
			tCoin_Mode.SuspendLayout();
			tCoin_Detect.SuspendLayout();
			tBill_Mode.SuspendLayout();
			tBill_Detect.SuspendLayout();
			tBill_Config.SuspendLayout();
			SuspendLayout();
			pBotons.Controls.Add(bCancel);
			pBotons.Controls.Add(bSig);
			pBotons.Controls.Add(bAnt);
			pBotons.Controls.Add(bOK);
			pBotons.Dock = System.Windows.Forms.DockStyle.Bottom;
			pBotons.Location = new System.Drawing.Point(0, 415);
			pBotons.Name = "pBotons";
			pBotons.Size = new System.Drawing.Size(657, 54);
			pBotons.TabIndex = 0;
			bCancel.Image = Kiosk.Properties.Resources.ico_del;
			bCancel.Location = new System.Drawing.Point(12, 7);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(75, 41);
			bCancel.TabIndex = 0;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bSig.Image = Kiosk.Properties.Resources.ico_right_green;
			bSig.Location = new System.Drawing.Point(489, 7);
			bSig.Name = "bSig";
			bSig.Size = new System.Drawing.Size(75, 41);
			bSig.TabIndex = 2;
			bSig.UseVisualStyleBackColor = true;
			bSig.Click += new System.EventHandler(bSig_Click);
			bAnt.Image = Kiosk.Properties.Resources.ico_left_blue;
			bAnt.Location = new System.Drawing.Point(408, 7);
			bAnt.Name = "bAnt";
			bAnt.Size = new System.Drawing.Size(75, 41);
			bAnt.TabIndex = 1;
			bAnt.UseVisualStyleBackColor = true;
			bOK.Image = Kiosk.Properties.Resources.ico_ok;
			bOK.Location = new System.Drawing.Point(570, 7);
			bOK.Name = "bOK";
			bOK.Size = new System.Drawing.Size(75, 41);
			bOK.TabIndex = 3;
			bOK.UseVisualStyleBackColor = true;
			bOK.Click += new System.EventHandler(bOK_Click);
			tabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
			tabs.Controls.Add(tInfo);
			tabs.Controls.Add(tCoin_Mode);
			tabs.Controls.Add(tCoin_Detect);
			tabs.Controls.Add(tBill_Mode);
			tabs.Controls.Add(tBill_Detect);
			tabs.Controls.Add(tBill_Config);
			tabs.Controls.Add(tResum);
			tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			tabs.ItemSize = new System.Drawing.Size(0, 1);
			tabs.Location = new System.Drawing.Point(0, 0);
			tabs.Multiline = true;
			tabs.Name = "tabs";
			tabs.SelectedIndex = 0;
			tabs.Size = new System.Drawing.Size(657, 415);
			tabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			tabs.TabIndex = 1;
			tInfo.Controls.Add(hInfo);
			tInfo.Controls.Add(infoWizard);
			tInfo.Location = new System.Drawing.Point(4, 5);
			tInfo.Name = "tInfo";
			tInfo.Padding = new System.Windows.Forms.Padding(3);
			tInfo.Size = new System.Drawing.Size(649, 406);
			tInfo.TabIndex = 0;
			hInfo.BackgroundImage = (System.Drawing.Image)resources.GetObject("hInfo.BackgroundImage");
			hInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			hInfo.Dock = System.Windows.Forms.DockStyle.Top;
			hInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			hInfo.Location = new System.Drawing.Point(3, 3);
			hInfo.Name = "hInfo";
			hInfo.PageEndColor = System.Drawing.Color.Empty;
			hInfo.PageStartColor = System.Drawing.Color.LightSteelBlue;
			hInfo.Size = new System.Drawing.Size(643, 60);
			hInfo.TabIndex = 3;
			hInfo.Title = "Configuration devices";
			hInfo.Title_Alignement = GLib.Forms.GradientPanel.Alignement.Left;
			infoWizard.Location = new System.Drawing.Point(8, 68);
			infoWizard.Margin = new System.Windows.Forms.Padding(0);
			infoWizard.Multiline = true;
			infoWizard.Name = "infoWizard";
			infoWizard.ReadOnly = true;
			infoWizard.Size = new System.Drawing.Size(633, 333);
			infoWizard.TabIndex = 2;
			infoWizard.Text = "1) Install drivers.\r\n\r\n2) Connect device.\r\n\r\n3) COnfigure device.\r\n";
			tCoin_Mode.Controls.Add(rCoin_M3);
			tCoin_Mode.Controls.Add(rCoin_M1);
			tCoin_Mode.Controls.Add(rCoin_M2);
			tCoin_Mode.Controls.Add(hCoin_Mode);
			tCoin_Mode.Location = new System.Drawing.Point(4, 5);
			tCoin_Mode.Name = "tCoin_Mode";
			tCoin_Mode.Padding = new System.Windows.Forms.Padding(3);
			tCoin_Mode.Size = new System.Drawing.Size(649, 406);
			tCoin_Mode.TabIndex = 1;
			tCoin_Mode.UseVisualStyleBackColor = true;
			rCoin_M3.AutoSize = true;
			rCoin_M3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rCoin_M3.Location = new System.Drawing.Point(30, 133);
			rCoin_M3.Name = "rCoin_M3";
			rCoin_M3.Size = new System.Drawing.Size(120, 35);
			rCoin_M3.TabIndex = 3;
			rCoin_M3.Text = "Manual";
			rCoin_M3.UseVisualStyleBackColor = true;
			rCoin_M1.AutoSize = true;
			rCoin_M1.Checked = true;
			rCoin_M1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rCoin_M1.Location = new System.Drawing.Point(30, 90);
			rCoin_M1.Name = "rCoin_M1";
			rCoin_M1.Size = new System.Drawing.Size(112, 35);
			rCoin_M1.TabIndex = 2;
			rCoin_M1.TabStop = true;
			rCoin_M1.Text = "Detect";
			rCoin_M1.UseVisualStyleBackColor = true;
			rCoin_M2.AutoSize = true;
			rCoin_M2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rCoin_M2.Location = new System.Drawing.Point(30, 176);
			rCoin_M2.Name = "rCoin_M2";
			rCoin_M2.Size = new System.Drawing.Size(97, 35);
			rCoin_M2.TabIndex = 1;
			rCoin_M2.Text = "None";
			rCoin_M2.UseVisualStyleBackColor = true;
			hCoin_Mode.BackgroundImage = (System.Drawing.Image)resources.GetObject("hCoin_Mode.BackgroundImage");
			hCoin_Mode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			hCoin_Mode.Dock = System.Windows.Forms.DockStyle.Top;
			hCoin_Mode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			hCoin_Mode.Location = new System.Drawing.Point(3, 3);
			hCoin_Mode.Name = "hCoin_Mode";
			hCoin_Mode.PageEndColor = System.Drawing.Color.Empty;
			hCoin_Mode.PageStartColor = System.Drawing.Color.LightSteelBlue;
			hCoin_Mode.Size = new System.Drawing.Size(643, 60);
			hCoin_Mode.TabIndex = 0;
			hCoin_Mode.Title = "Coin acceptor";
			hCoin_Mode.Title_Alignement = GLib.Forms.GradientPanel.Alignement.Left;
			tCoin_Detect.Controls.Add(dCCT2);
			tCoin_Detect.Controls.Add(pCoin);
			tCoin_Detect.Controls.Add(dRM5);
			tCoin_Detect.Controls.Add(hCoin_Detect);
			tCoin_Detect.Location = new System.Drawing.Point(4, 5);
			tCoin_Detect.Name = "tCoin_Detect";
			tCoin_Detect.Size = new System.Drawing.Size(649, 406);
			tCoin_Detect.TabIndex = 3;
			tCoin_Detect.UseVisualStyleBackColor = true;
			dCCT2.AutoSize = true;
			dCCT2.Enabled = false;
			dCCT2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dCCT2.Location = new System.Drawing.Point(27, 85);
			dCCT2.Name = "dCCT2";
			dCCT2.Size = new System.Drawing.Size(435, 35);
			dCCT2.TabIndex = 4;
			dCCT2.Text = "CCTalk COIN ACCEPTOR (ID 2)";
			dCCT2.ThreeState = true;
			dCCT2.UseVisualStyleBackColor = true;
			pCoin.Dock = System.Windows.Forms.DockStyle.Bottom;
			pCoin.Location = new System.Drawing.Point(0, 392);
			pCoin.Name = "pCoin";
			pCoin.Size = new System.Drawing.Size(649, 14);
			pCoin.Step = 100;
			pCoin.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			pCoin.TabIndex = 3;
			pCoin.Value = 100;
			pCoin.Visible = false;
			dRM5.AutoSize = true;
			dRM5.Enabled = false;
			dRM5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dRM5.Location = new System.Drawing.Point(27, 126);
			dRM5.Name = "dRM5";
			dRM5.Size = new System.Drawing.Size(230, 35);
			dRM5.TabIndex = 2;
			dRM5.Text = "Comestero RM5";
			dRM5.ThreeState = true;
			dRM5.UseVisualStyleBackColor = true;
			hCoin_Detect.BackgroundImage = (System.Drawing.Image)resources.GetObject("hCoin_Detect.BackgroundImage");
			hCoin_Detect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			hCoin_Detect.Dock = System.Windows.Forms.DockStyle.Top;
			hCoin_Detect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			hCoin_Detect.Location = new System.Drawing.Point(0, 0);
			hCoin_Detect.Name = "hCoin_Detect";
			hCoin_Detect.PageEndColor = System.Drawing.Color.Empty;
			hCoin_Detect.PageStartColor = System.Drawing.Color.LightSteelBlue;
			hCoin_Detect.Size = new System.Drawing.Size(649, 60);
			hCoin_Detect.TabIndex = 1;
			hCoin_Detect.Title = "Find coin acceptor";
			hCoin_Detect.Title_Alignement = GLib.Forms.GradientPanel.Alignement.Left;
			tBill_Mode.Controls.Add(rBill_M3);
			tBill_Mode.Controls.Add(rBill_M1);
			tBill_Mode.Controls.Add(rBill_M2);
			tBill_Mode.Controls.Add(pBillMode);
			tBill_Mode.Location = new System.Drawing.Point(4, 5);
			tBill_Mode.Name = "tBill_Mode";
			tBill_Mode.Size = new System.Drawing.Size(649, 406);
			tBill_Mode.TabIndex = 2;
			tBill_Mode.UseVisualStyleBackColor = true;
			rBill_M3.AutoSize = true;
			rBill_M3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rBill_M3.Location = new System.Drawing.Point(30, 133);
			rBill_M3.Name = "rBill_M3";
			rBill_M3.Size = new System.Drawing.Size(120, 35);
			rBill_M3.TabIndex = 7;
			rBill_M3.Text = "Manual";
			rBill_M3.UseVisualStyleBackColor = true;
			rBill_M1.AutoSize = true;
			rBill_M1.Checked = true;
			rBill_M1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rBill_M1.Location = new System.Drawing.Point(30, 90);
			rBill_M1.Name = "rBill_M1";
			rBill_M1.Size = new System.Drawing.Size(112, 35);
			rBill_M1.TabIndex = 6;
			rBill_M1.TabStop = true;
			rBill_M1.Text = "Detect";
			rBill_M1.UseVisualStyleBackColor = true;
			rBill_M2.AutoSize = true;
			rBill_M2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			rBill_M2.Location = new System.Drawing.Point(30, 174);
			rBill_M2.Name = "rBill_M2";
			rBill_M2.Size = new System.Drawing.Size(97, 35);
			rBill_M2.TabIndex = 5;
			rBill_M2.Text = "None";
			rBill_M2.UseVisualStyleBackColor = true;
			pBillMode.BackgroundImage = (System.Drawing.Image)resources.GetObject("pBillMode.BackgroundImage");
			pBillMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			pBillMode.Dock = System.Windows.Forms.DockStyle.Top;
			pBillMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			pBillMode.Location = new System.Drawing.Point(0, 0);
			pBillMode.Name = "pBillMode";
			pBillMode.PageEndColor = System.Drawing.Color.Empty;
			pBillMode.PageStartColor = System.Drawing.Color.LightSteelBlue;
			pBillMode.Size = new System.Drawing.Size(649, 60);
			pBillMode.TabIndex = 4;
			pBillMode.Title = "Validator";
			pBillMode.Title_Alignement = GLib.Forms.GradientPanel.Alignement.Left;
			tBill_Detect.Controls.Add(dNV_SIO);
			tBill_Detect.Controls.Add(pBill);
			tBill_Detect.Controls.Add(dTrilogy);
			tBill_Detect.Controls.Add(dF40);
			tBill_Detect.Controls.Add(dNV9_SSP);
			tBill_Detect.Controls.Add(dNV9_SSP3);
			tBill_Detect.Controls.Add(pBillDetect);
			tBill_Detect.Location = new System.Drawing.Point(4, 5);
			tBill_Detect.Name = "tBill_Detect";
			tBill_Detect.Size = new System.Drawing.Size(649, 406);
			tBill_Detect.TabIndex = 6;
			tBill_Detect.UseVisualStyleBackColor = true;
			dNV_SIO.AutoSize = true;
			dNV_SIO.Enabled = false;
			dNV_SIO.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dNV_SIO.Location = new System.Drawing.Point(30, 166);
			dNV_SIO.Name = "dNV_SIO";
			dNV_SIO.Size = new System.Drawing.Size(239, 35);
			dNV_SIO.TabIndex = 9;
			dNV_SIO.Text = "NV SIO (300bps)";
			dNV_SIO.UseVisualStyleBackColor = true;
			pBill.Dock = System.Windows.Forms.DockStyle.Bottom;
			pBill.Location = new System.Drawing.Point(0, 392);
			pBill.Name = "pBill";
			pBill.Size = new System.Drawing.Size(649, 14);
			pBill.Step = 100;
			pBill.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			pBill.TabIndex = 8;
			pBill.Value = 100;
			pBill.Visible = false;
			dTrilogy.AutoSize = true;
			dTrilogy.Enabled = false;
			dTrilogy.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dTrilogy.Location = new System.Drawing.Point(30, 252);
			dTrilogy.Name = "dTrilogy";
			dTrilogy.Size = new System.Drawing.Size(115, 35);
			dTrilogy.TabIndex = 7;
			dTrilogy.Text = "Trilogy";
			dTrilogy.UseVisualStyleBackColor = true;
			dF40.AutoSize = true;
			dF40.Enabled = false;
			dF40.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dF40.Location = new System.Drawing.Point(30, 209);
			dF40.Name = "dF40";
			dF40.Size = new System.Drawing.Size(203, 35);
			dF40.TabIndex = 6;
			dF40.Text = "ccTalk (ID 40)";
			dF40.UseVisualStyleBackColor = true;
			dNV9_SSP.AutoSize = true;
			dNV9_SSP.Enabled = false;
			dNV9_SSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dNV9_SSP.Location = new System.Drawing.Point(30, 80);
			dNV9_SSP.Name = "dNV9_SSP";
			dNV9_SSP.Size = new System.Drawing.Size(132, 35);
			dNV9_SSP.TabIndex = 4;
			dNV9_SSP.Text = "NV SSP";
			dNV9_SSP.UseVisualStyleBackColor = true;
			dNV9_SSP3.AutoSize = true;
			dNV9_SSP3.Enabled = false;
			dNV9_SSP3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dNV9_SSP3.Location = new System.Drawing.Point(30, 123);
			dNV9_SSP3.Name = "dNV9_SSP3";
			dNV9_SSP3.Size = new System.Drawing.Size(168, 35);
			dNV9_SSP3.TabIndex = 4;
			dNV9_SSP3.Text = "NV SSP v3";
			dNV9_SSP3.UseVisualStyleBackColor = true;
			pBillDetect.BackgroundImage = (System.Drawing.Image)resources.GetObject("pBillDetect.BackgroundImage");
			pBillDetect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			pBillDetect.Dock = System.Windows.Forms.DockStyle.Top;
			pBillDetect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			pBillDetect.Location = new System.Drawing.Point(0, 0);
			pBillDetect.Name = "pBillDetect";
			pBillDetect.PageEndColor = System.Drawing.Color.Empty;
			pBillDetect.PageStartColor = System.Drawing.Color.LightSteelBlue;
			pBillDetect.Size = new System.Drawing.Size(649, 60);
			pBillDetect.TabIndex = 3;
			pBillDetect.Title = "Find bill validator";
			pBillDetect.Title_Alignement = GLib.Forms.GradientPanel.Alignement.Left;
			tBill_Config.Controls.Add(eCANAL_16);
			tBill_Config.Controls.Add(eCANAL_15);
			tBill_Config.Controls.Add(eCANAL_14);
			tBill_Config.Controls.Add(eCANAL_13);
			tBill_Config.Controls.Add(eCANAL_12);
			tBill_Config.Controls.Add(eCANAL_11);
			tBill_Config.Controls.Add(eCANAL_10);
			tBill_Config.Controls.Add(eCANAL_9);
			tBill_Config.Controls.Add(eCANAL_8);
			tBill_Config.Controls.Add(eCANAL_7);
			tBill_Config.Controls.Add(eCANAL_6);
			tBill_Config.Controls.Add(eCANAL_5);
			tBill_Config.Controls.Add(eCANAL_4);
			tBill_Config.Controls.Add(eCANAL_3);
			tBill_Config.Controls.Add(eCANAL_2);
			tBill_Config.Controls.Add(eCANAL_1);
			tBill_Config.Controls.Add(iCom);
			tBill_Config.Controls.Add(lCom);
			tBill_Config.Controls.Add(pTitle);
			tBill_Config.Location = new System.Drawing.Point(4, 5);
			tBill_Config.Name = "tBill_Config";
			tBill_Config.Size = new System.Drawing.Size(649, 406);
			tBill_Config.TabIndex = 9;
			tBill_Config.UseVisualStyleBackColor = true;
			eCANAL_16.AutoSize = true;
			eCANAL_16.Checked = true;
			eCANAL_16.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_16.Location = new System.Drawing.Point(319, 369);
			eCANAL_16.Name = "eCANAL_16";
			eCANAL_16.Size = new System.Drawing.Size(33, 24);
			eCANAL_16.TabIndex = 38;
			eCANAL_16.Text = "-";
			eCANAL_16.UseVisualStyleBackColor = true;
			eCANAL_15.AutoSize = true;
			eCANAL_15.Checked = true;
			eCANAL_15.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_15.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_15.Location = new System.Drawing.Point(319, 337);
			eCANAL_15.Name = "eCANAL_15";
			eCANAL_15.Size = new System.Drawing.Size(33, 24);
			eCANAL_15.TabIndex = 37;
			eCANAL_15.Text = "-";
			eCANAL_15.UseVisualStyleBackColor = true;
			eCANAL_14.AutoSize = true;
			eCANAL_14.Checked = true;
			eCANAL_14.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_14.Location = new System.Drawing.Point(319, 305);
			eCANAL_14.Name = "eCANAL_14";
			eCANAL_14.Size = new System.Drawing.Size(33, 24);
			eCANAL_14.TabIndex = 36;
			eCANAL_14.Text = "-";
			eCANAL_14.UseVisualStyleBackColor = true;
			eCANAL_13.AutoSize = true;
			eCANAL_13.Checked = true;
			eCANAL_13.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_13.Location = new System.Drawing.Point(319, 273);
			eCANAL_13.Name = "eCANAL_13";
			eCANAL_13.Size = new System.Drawing.Size(33, 24);
			eCANAL_13.TabIndex = 35;
			eCANAL_13.Text = "-";
			eCANAL_13.UseVisualStyleBackColor = true;
			eCANAL_12.AutoSize = true;
			eCANAL_12.Checked = true;
			eCANAL_12.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_12.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_12.Location = new System.Drawing.Point(319, 241);
			eCANAL_12.Name = "eCANAL_12";
			eCANAL_12.Size = new System.Drawing.Size(33, 24);
			eCANAL_12.TabIndex = 34;
			eCANAL_12.Text = "-";
			eCANAL_12.UseVisualStyleBackColor = true;
			eCANAL_11.AutoSize = true;
			eCANAL_11.Checked = true;
			eCANAL_11.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_11.Location = new System.Drawing.Point(319, 209);
			eCANAL_11.Name = "eCANAL_11";
			eCANAL_11.Size = new System.Drawing.Size(33, 24);
			eCANAL_11.TabIndex = 33;
			eCANAL_11.Text = "-";
			eCANAL_11.UseVisualStyleBackColor = true;
			eCANAL_10.AutoSize = true;
			eCANAL_10.Checked = true;
			eCANAL_10.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_10.Location = new System.Drawing.Point(319, 177);
			eCANAL_10.Name = "eCANAL_10";
			eCANAL_10.Size = new System.Drawing.Size(33, 24);
			eCANAL_10.TabIndex = 32;
			eCANAL_10.Text = "-";
			eCANAL_10.UseVisualStyleBackColor = true;
			eCANAL_9.AutoSize = true;
			eCANAL_9.Checked = true;
			eCANAL_9.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_9.Location = new System.Drawing.Point(319, 145);
			eCANAL_9.Name = "eCANAL_9";
			eCANAL_9.Size = new System.Drawing.Size(33, 24);
			eCANAL_9.TabIndex = 31;
			eCANAL_9.Text = "-";
			eCANAL_9.UseVisualStyleBackColor = true;
			eCANAL_8.AutoSize = true;
			eCANAL_8.Checked = true;
			eCANAL_8.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_8.Location = new System.Drawing.Point(22, 369);
			eCANAL_8.Name = "eCANAL_8";
			eCANAL_8.Size = new System.Drawing.Size(33, 24);
			eCANAL_8.TabIndex = 30;
			eCANAL_8.Text = "-";
			eCANAL_8.UseVisualStyleBackColor = true;
			eCANAL_7.AutoSize = true;
			eCANAL_7.Checked = true;
			eCANAL_7.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_7.Location = new System.Drawing.Point(22, 337);
			eCANAL_7.Name = "eCANAL_7";
			eCANAL_7.Size = new System.Drawing.Size(33, 24);
			eCANAL_7.TabIndex = 29;
			eCANAL_7.Text = "-";
			eCANAL_7.UseVisualStyleBackColor = true;
			eCANAL_6.AutoSize = true;
			eCANAL_6.Checked = true;
			eCANAL_6.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_6.Location = new System.Drawing.Point(22, 305);
			eCANAL_6.Name = "eCANAL_6";
			eCANAL_6.Size = new System.Drawing.Size(33, 24);
			eCANAL_6.TabIndex = 28;
			eCANAL_6.Text = "-";
			eCANAL_6.UseVisualStyleBackColor = true;
			eCANAL_5.AutoSize = true;
			eCANAL_5.Checked = true;
			eCANAL_5.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_5.Location = new System.Drawing.Point(22, 273);
			eCANAL_5.Name = "eCANAL_5";
			eCANAL_5.Size = new System.Drawing.Size(33, 24);
			eCANAL_5.TabIndex = 27;
			eCANAL_5.Text = "-";
			eCANAL_5.UseVisualStyleBackColor = true;
			eCANAL_4.AutoSize = true;
			eCANAL_4.Checked = true;
			eCANAL_4.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_4.Location = new System.Drawing.Point(22, 241);
			eCANAL_4.Name = "eCANAL_4";
			eCANAL_4.Size = new System.Drawing.Size(33, 24);
			eCANAL_4.TabIndex = 26;
			eCANAL_4.Text = "-";
			eCANAL_4.UseVisualStyleBackColor = true;
			eCANAL_3.AutoSize = true;
			eCANAL_3.Checked = true;
			eCANAL_3.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_3.Location = new System.Drawing.Point(22, 209);
			eCANAL_3.Name = "eCANAL_3";
			eCANAL_3.Size = new System.Drawing.Size(33, 24);
			eCANAL_3.TabIndex = 25;
			eCANAL_3.Text = "-";
			eCANAL_3.UseVisualStyleBackColor = true;
			eCANAL_2.AutoSize = true;
			eCANAL_2.Checked = true;
			eCANAL_2.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_2.Location = new System.Drawing.Point(22, 177);
			eCANAL_2.Name = "eCANAL_2";
			eCANAL_2.Size = new System.Drawing.Size(33, 24);
			eCANAL_2.TabIndex = 24;
			eCANAL_2.Text = "-";
			eCANAL_2.UseVisualStyleBackColor = true;
			eCANAL_1.AutoSize = true;
			eCANAL_1.Checked = true;
			eCANAL_1.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			eCANAL_1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			eCANAL_1.Location = new System.Drawing.Point(22, 145);
			eCANAL_1.Name = "eCANAL_1";
			eCANAL_1.Size = new System.Drawing.Size(33, 24);
			eCANAL_1.TabIndex = 23;
			eCANAL_1.Text = "-";
			eCANAL_1.UseVisualStyleBackColor = true;
			iCom.AutoSize = true;
			iCom.Location = new System.Drawing.Point(19, 95);
			iCom.Name = "iCom";
			iCom.Size = new System.Drawing.Size(53, 13);
			iCom.TabIndex = 8;
			iCom.Text = "COM Port";
			lCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			lCom.Font = new System.Drawing.Font("Microsoft Sans Serif", 20f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lCom.FormattingEnabled = true;
			lCom.Location = new System.Drawing.Point(78, 81);
			lCom.Name = "lCom";
			lCom.Size = new System.Drawing.Size(167, 39);
			lCom.TabIndex = 7;
			lCom.SelectionChangeCommitted += new System.EventHandler(lCom_SelectionChangeCommitted);
			pTitle.BackgroundImage = (System.Drawing.Image)resources.GetObject("pTitle.BackgroundImage");
			pTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			pTitle.Dock = System.Windows.Forms.DockStyle.Top;
			pTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			pTitle.Location = new System.Drawing.Point(0, 0);
			pTitle.Name = "pTitle";
			pTitle.PageEndColor = System.Drawing.Color.Empty;
			pTitle.PageStartColor = System.Drawing.Color.LightSteelBlue;
			pTitle.Size = new System.Drawing.Size(649, 60);
			pTitle.TabIndex = 6;
			pTitle.Title = "F40 Configuration";
			pTitle.Title_Alignement = GLib.Forms.GradientPanel.Alignement.Left;
			tResum.Location = new System.Drawing.Point(4, 5);
			tResum.Name = "tResum";
			tResum.Size = new System.Drawing.Size(649, 406);
			tResum.TabIndex = 10;
			tResum.UseVisualStyleBackColor = true;
			bgControl.WorkerReportsProgress = true;
			bgControl.WorkerSupportsCancellation = true;
			bgControl.DoWork += new System.ComponentModel.DoWorkEventHandler(bgControl_DoWork);
			bgControl.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgControl_ProgressChanged);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(657, 469);
			base.ControlBox = false;
			base.Controls.Add(tabs);
			base.Controls.Add(pBotons);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "Devices_Wizard";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "Devices";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Devices_Wizard_FormClosing);
			base.Load += new System.EventHandler(Devices_Wizard_Load);
			pBotons.ResumeLayout(false);
			tabs.ResumeLayout(false);
			tInfo.ResumeLayout(false);
			tInfo.PerformLayout();
			tCoin_Mode.ResumeLayout(false);
			tCoin_Mode.PerformLayout();
			tCoin_Detect.ResumeLayout(false);
			tCoin_Detect.PerformLayout();
			tBill_Mode.ResumeLayout(false);
			tBill_Mode.PerformLayout();
			tBill_Detect.ResumeLayout(false);
			tBill_Detect.PerformLayout();
			tBill_Config.ResumeLayout(false);
			tBill_Config.PerformLayout();
			ResumeLayout(false);
		}
	}
}
