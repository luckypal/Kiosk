using GLib;
using GLib.Config;
using Kiosk.SerQuiosc2;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Kiosk
{
	public class Configuracion : XmlConfig
	{
		public class Ticket_Resposta
		{
			public int Ticket;

			public int Pago;

			public int Verificado;

			public int Sala;

			public int UserID;

			public DateTime DataT;

			public string CRC;

			public int Parser(string _v)
			{
				if (string.IsNullOrEmpty(_v))
				{
					return 0;
				}
				string[] array = _v.Split('|');
				if (array.Length < 6)
				{
					return 0;
				}
				Pago = 0;
				try
				{
					decimal d = decimal.Parse(array[0], new CultureInfo("es-ES"));
					Pago = (int)(d * 100m);
				}
				catch
				{
					Pago = 0;
					Verificado = 3;
					return 0;
				}
				if (Pago < 0)
				{
					Verificado = 3;
					return 0;
				}
				Verificado = 4;
				int num = 0;
				try
				{
					num = int.Parse(array[5]);
				}
				catch
				{
					return 0;
				}
				CRC = $"{(char)(65 + num / 100)}{num % 100:00}";
				Ticket = 0;
				try
				{
					Ticket = int.Parse(array[6]);
				}
				catch
				{
					return 0;
				}
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				string[] array2 = array[4].Split(' ');
				string[] array3 = array2[0].Split('/');
				if (array3.Length < 3)
				{
					return 0;
				}
				try
				{
					num5 = int.Parse(array3[0]);
				}
				catch
				{
					return 0;
				}
				try
				{
					num3 = int.Parse(array3[1]);
				}
				catch
				{
					return 0;
				}
				try
				{
					num2 = int.Parse(array3[2]);
				}
				catch
				{
					return 0;
				}
				string[] array4 = array2[1].Split(':');
				if (array4.Length < 3)
				{
					return 0;
				}
				try
				{
					num4 = int.Parse(array4[0]);
				}
				catch
				{
					return 0;
				}
				try
				{
					num6 = int.Parse(array4[1]);
				}
				catch
				{
					return 0;
				}
				try
				{
					num7 = int.Parse(array4[2]);
				}
				catch
				{
					return 0;
				}
				DataT = DateTime.Now;
				try
				{
					DataT = new DateTime(num2, num3, num5, num4, num6, num7);
				}
				catch
				{
					return 0;
				}
				Verificado = 0;
				if (array[1].ToLower() == "t")
				{
					Verificado = 1;
				}
				return 1;
			}
		}

		public string VersionPRG;

		public string debugtxt;

		public int Verificar_Ticket;

		public bool ForceGoConfig;

		public Ticket_Resposta Ticket_Pago;

		public Ticket_Resposta Ticket_Verificar;

		public int News;

		public int Add_Getton;

		public bool GettonOff;

		public DateTime GettonOffLast;

		public bool GettonWait = false;

		public int ForceRefresh;

		public int Ticket_Carburante;

		public string IDMAQUINA;

		public ulong dIDMAQUINA;

		public string Publi;

		public int Monitors;

		public int modo_XP = 0;

		public int parche = 0;

		public string CPUID;

		public bool Emu_Mouse;

		public bool Emu_Mouse_RClick;

		public int JoinTicket;

		public int CfgVer;

		public string LastVersion;

		public int MissatgeCreditsGratuits;

		public string MissatgePrinter;

		public string Srv_Web_Ip;

		public string Srv_Web_Page;

		public string Server_VNC;

		public bool ForceReset;

		public bool ForceManteniment;

		public string ForceResetMask;

		public string ForceMantenimentMask;

		public int NoCreditsInGame = 0;

		public int TicketHidePay;

		public int BillDisableServer;

		public int AutoTicketTime;

		public int Error_Billetero;

		public int Spy;

		public string UserSpy;

		public int Dev_Bank;

		public int ForceAllKey;

		public int ticketCleanTemps;

		public int EnManteniment;

		public int FreeGames;

		public int ModoKiosk;

		public int CursorOn;

		public string PasswordADM;

		public string Dev_Coin;

		public string Dev_Coin_P;

		public string Dev_BNV;

		public string Dev_BNV_P;

		public string[] Web_Zone;

		public bool TimeNavigate;

		public int ForceLogin;

		public int ResetTemps;

		public int ValorTemps;

		public string Home;

		public int EmuKeyboard;

		public string Srv_Rtm = "Service";

		public string Srv_Ip;

		public string Srv_port = "1938";

		public string Srv_User = "-";

		public string Srv_User_P = "-";

		public string Srv_ID_Lin1;

		public string Srv_ID_Lin2;

		public string Srv_ID_Lin3;

		public string Srv_ID_Lin4;

		public string Srv_ID_Lin5;

		public string Srv_ID_LinBottom;

		public string Srv_ID_Tlf;

		public string Impresora_Tck;

		public string Barcode;

		public int ModoTickets;

		public int ModoPS2;

		public bool StopCredits;

		public int TimeoutCredits;

		public int BrowserBarOn;

		public string RemoteCmd;

		public string RemoteParam;

		public decimal CreditsGratuits;

		public int ModoPlayCreditsGratuits;

		public int Pagar_Ticket_Val;

		public int Pagar_Ticket_Busy;

		public int Pagar_Ticket_ID;

		public int __ModoTablet = 0;

		public int CancelTempsOn;

		private int Canales = 16;

		public int Ticket_N_FEED;

		public int Ticket_N_HEAD;

		public int Ticket_Cut;

		public int Ticket_Model;

		public int Ticket_60mm;

		public long dMAC;

		public bool PrintTicket;

		public bool RunConfig;

		public DateTime LastMouseMove;

		public int ComprarTemps;

		public bool ModeAchat;

		public bool Running;

		public int FullScreen;

		public decimal SaldoCredits;

		public decimal Credits;

		public decimal Sub_Credits;

		public decimal Add_Credits;

		public decimal Send_Add_Credits;

		public decimal Send_Sub_Credits;

		public bool Logged;

		public bool InGame;

		public bool U_InGame;

		public int Temps;

		public int CancelTemps;

		public int TempsDeTicket;

		public int uCredits;

		public int uTemps;

		public bool Show_Browser;

		public int Enable_Lectors;

		public int LockCredits;

		public int Reset = 0;

		public string Last_Device;

		public int Test_Barcode;

		public int TicketTemps;

		public int IdTicketTemps;

		public string MAC;

		public string MACLAN;

		public string MACWIFI;

		public string Srv_Room;

		public LogFile _logLogin;

		public LogFile _logCredits;

		public LogFile _logServer;

		public int BotoTicket;

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

		public int Disp_Enable;

		public int Disp_Val;

		public int Disp_Min;

		public int Disp_Max;

		public int Disp_Out;

		public int Disp_Ticket;

		public string Disp_Port;

		public string Disp_Model;

		public int Disp_Recovery;

		public string Disp_Recovery_Pass;

		public int Disp_Pay_Ticket;

		public int Disp_Pay_Ticket_Credits;

		public int Disp_Pay_Ticket_Out;

		public int Disp_Pay_Ticket_Fail;

		public int Disp_Pay_Ticket_Cnt_Fail;

		public int Disp_Pay_Ticket_Out_Flag;

		public int Disp_Pay_Running;

		public new string error;

		public bool ForceSpy = false;

		public Configuracion()
		{
			__ModoTablet = 0;
			Add_Getton = 0;
			CfgFile = AppName + ".options";
			Ticket_Pago = new Ticket_Resposta();
			Ticket_Verificar = new Ticket_Resposta();
			VersionPRG = "2.261";
			News = 0;
			MAC = GetMACAddress();
			string s = "0" + GetMACAddress();
			dMAC = 0L;
			bool flag;
			try
			{
				long num = dMAC = long.Parse(s, NumberStyles.AllowHexSpecifier);
			}
			catch (Exception ex)
			{
				flag = true;
				s = ex.Message;
			}
			MACLAN = $"0{GetMACAddress_LAN()}";
			MACWIFI = $"0{GetMACAddress_WIFI()}";
			if (IDMAQUINA == "00")
			{
				IDMAQUINA = "0";
			}
			if (MAC == "00")
			{
				MAC = "0";
			}
			if (MACLAN == "00")
			{
				MACLAN = "0";
			}
			if (string.IsNullOrEmpty(MACLAN))
			{
				MACLAN = "0";
			}
			if (MACWIFI == "00")
			{
				MACWIFI = "0";
			}
			if (string.IsNullOrEmpty(MACWIFI))
			{
				MACWIFI = "0";
			}
			OperatingSystem oSVersion = Environment.OSVersion;
			modo_XP = 0;
			if (oSVersion.Version.Major > 5)
			{
				modo_XP = 0;
				CPUID = GetMACAddress();
			}
			else
			{
				modo_XP = 1;
				CPUID = "0";
			}
			IDMAQUINA = CPUID;
			if (MACLAN != "0")
			{
				IDMAQUINA = MACLAN;
			}
			s = "0" + IDMAQUINA;
			dIDMAQUINA = 0uL;
			try
			{
				ulong num2 = dIDMAQUINA = ulong.Parse(s, NumberStyles.AllowHexSpecifier);
			}
			catch (Exception)
			{
			}
			MissatgePrinter = "";
			Spy = 0;
			GettonOff = true;
			UserSpy = "?";
			Emu_Mouse = false;
			Emu_Mouse_RClick = false;
			Dev_Bank = 0;
			debugtxt = "";
			FreeGames = 1;
			EnManteniment = 0;
			ModoKiosk = 1;
			loc = "frLU";
			unique = true;
			ModoPS2 = 0;
			ticketCleanTemps = 0;
			SaldoCredits = 0m;
			RunConfig = false;
			Running = false;
			FullScreen = 0;
			ModoPlayCreditsGratuits = 0;
			CursorOn = 1;
			ForceAllKey = 0;
			ValorTemps = 25;
			BotoTicket = 1;
			MissatgeCreditsGratuits = 0;
			ForceRefresh = 0;
			Disp_Enable = 0;
			Disp_Min = -1;
			Disp_Max = -10;
			Disp_Out = 0;
			Disp_Val = 500;
			Disp_Ticket = 1;
			Disp_Port = "?";
			Disp_Model = "ICT";
			Disp_Recovery = 1;
			Disp_Recovery_Pass = "5432";
			ForceGoConfig = false;
			CfgVer = 2;
			flag = true;
			AutoTicketTime = 0;
			ComprarTemps = 0;
			BrowserBarOn = 0;
			Verificar_Ticket = 0;
			TicketHidePay = 0;
			BillDisableServer = 0;
			Impresora_Tck = "";
			Server_VNC = "control.game-host.org";
			Web_Zone = new string[256];
			flag = false;
			Web_Zone[0] = "a.b.c";
			Web_Zone[1] = "tel.tbtlp.com";
			Web_Zone[2] = "www.jogarvip.com";
			Web_Zone[3] = "www.nalvip.com";
			Web_Zone[4] = "hw.html5slot.com";
			Web_Zone[5] = "tel.html5slot.com";
			Web_Zone[6] = "menu.html5slot.com";
			Web_Zone[7] = "hw.netkgame.com";
			Web_Zone[8] = "tel.netkgame.com";
			Web_Zone[9] = "menu.netkgame.com";
			Disp_Enable = 0;
			Disp_Val = 500;
			Disp_Min = -1;
			Disp_Max = -10;
			Disp_Out = 0;
			Disp_Ticket = 1;
			Disp_Port = "?";
			Disp_Model = "ICT";
			Disp_Recovery = 1;
			Disp_Recovery_Pass = "5432";
			Publi = "";
			ForceReset = false;
			ForceManteniment = false;
			ForceResetMask = "";
			ForceMantenimentMask = "";
			Monitors = 1;
			LastVersion = "0";
			Srv_Rtm = "Service";
			flag = false;
			VersionPRG += "t";
			Srv_Ip = "tel.tbtlp.com";
			Srv_Web_Ip = "tel.tbtlp.com";
			flag = true;
			flag = true;
			flag = true;
			Srv_Web_Page = "tickquiosc.aspx";
			flag = true;
			ModoTickets = 1;
			flag = true;
			LockCredits = 0;
			Srv_ID_Lin1 = " ";
			Srv_ID_Lin2 = " ";
			Srv_ID_Lin3 = " ";
			Srv_ID_Lin4 = " ";
			Srv_ID_Lin5 = " ";
			Srv_ID_LinBottom = "Changer pour un cadeau";
			Srv_ID_Tlf = " ";
			Home = "http://www.google.com";
			Srv_User = "-";
			Srv_User_P = "-";
			Srv_Room = "";
			PasswordADM = XmlConfig.X2Y("admin1234");
			Show_Browser = false;
			Enable_Lectors = 0;
			Error_Billetero = 0;
			Add_Credits = 0m;
			Sub_Credits = 0m;
			Send_Add_Credits = 0m;
			CreditsGratuits = 0m;
			ForceLogin = 0;
			Test_Barcode = 0;
			RemoteCmd = "";
			RemoteParam = "";
			Barcode = "NO_DEVICE";
			Logged = false;
			TimeoutCredits = 120;
			ResetTemps = 600;
			TicketTemps = 0;
			IdTicketTemps = 0;
			U_InGame = false;
			InGame = false;
			LastMouseMove = DateTime.Now;
			Ticket_Carburante = 0;
			Pagar_Ticket_Val = 0;
			Pagar_Ticket_Busy = 0;
			Pagar_Ticket_ID = 0;
			CancelTempsOn = 0;
			JoinTicket = 1;
			NoCreditsInGame = 0;
			Ticket_N_FEED = 3;
			Ticket_N_HEAD = 0;
			Ticket_Cut = 1;
			Ticket_Model = 0;
			Dev_Coin = "?";
			Dev_BNV = "?";
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
			DataPath = "data\\";
			AppName = "kiosk";
			CfgFile = AppName + ".options";
			CfgFileFull = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + CfgFile;
			Ticket_60mm = 0;
			StopCredits = false;
			if (modo_XP == 0)
			{
				try
				{
					PrintDocument printDocument = new PrintDocument();
					foreach (string installedPrinter in PrinterSettings.InstalledPrinters)
					{
						if (installedPrinter.ToLower() == "NII ExD NP-3511".ToLower())
						{
							Impresora_Tck = installedPrinter;
							Ticket_N_FEED = 3;
							Ticket_N_HEAD = 0;
							Ticket_Cut = 1;
							Ticket_Model = 0;
							break;
						}
						if (installedPrinter.ToLower() == "NII ExD NP-3411".ToLower())
						{
							Impresora_Tck = installedPrinter;
							Ticket_N_FEED = 3;
							Ticket_N_HEAD = 0;
							Ticket_Cut = 1;
							Ticket_Model = 0;
							break;
						}
						if (installedPrinter.ToLower() == "NII ExD NP-211".ToLower())
						{
							Impresora_Tck = installedPrinter;
							Ticket_N_FEED = 3;
							Ticket_N_HEAD = 0;
							Ticket_Cut = 1;
							Ticket_Model = 0;
							break;
						}
						if (installedPrinter.ToLower() == "CUSTOM TG2480-H".ToLower())
						{
							Impresora_Tck = installedPrinter;
							Ticket_N_FEED = 3;
							Ticket_N_HEAD = 3;
							Ticket_Cut = 1;
							Ticket_Model = 1;
							break;
						}
						if (installedPrinter.ToLower() == "CUSTOM Engineering TG2460".ToLower())
						{
							Impresora_Tck = installedPrinter;
							Ticket_Cut = 1;
							Ticket_Model = 1;
							Ticket_60mm = 1;
							Ticket_N_FEED = 12;
							Ticket_N_HEAD = 3;
							break;
						}
						if (installedPrinter.ToLower() == "Star TSP100 Cutter (TSP143)".ToLower())
						{
							Impresora_Tck = installedPrinter;
							Ticket_N_FEED = 3;
							Ticket_N_HEAD = 0;
							Ticket_Cut = 0;
							Ticket_Model = 1;
							break;
						}
					}
				}
				catch (Exception)
				{
				}
			}
			PrintTicket = false;
			ModeAchat = true;
			Log_Debug("Startup");
		}

		public bool PingTest(string _http)
		{
			Ping ping = new Ping();
			PingReply pingReply = null;
			try
			{
				pingReply = ping.Send(IPAddress.Parse(_http));
			}
			catch
			{
				return false;
			}
			if (pingReply.Status == IPStatus.Success)
			{
				return true;
			}
			return false;
		}

		public bool Start_Service(string _web)
		{
			string fileName = System.IO.Path.GetTempPath() + "tmp121212.tmp";
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				WebClient webClient = new WebClient();
				webClient.DownloadFile(_web, fileName);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public string Get_Domain(string _http)
		{
			string[] array = _http.Split('.');
			int num = array.Length;
			if (num > 1)
			{
				return array[num - 2] + "." + array[num - 1];
			}
			return array[0] + ".com";
		}

		public void Save_Net()
		{
			bool flag = true;
			Save_Net($"{MACLAN}", "cfg");
		}

		public void Save_Net(string _mac, string _pref)
		{
			if (Srv_User == "-")
			{
				return;
			}
			Save();
			bool flag = true;
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			string text = "http://wsquioscv2." + Get_Domain(Srv_Ip) + "/ServiceQuiosc.asmx";
			flag = false;
			text = "http://wsquioscv3." + Get_Domain(Srv_Ip) + "/ServiceQuiosc.asmx";
			ulong num = 0uL;
			try
			{
				ulong num2 = ulong.Parse(_mac, NumberStyles.AllowHexSpecifier);
				num = num2;
			}
			catch
			{
			}
			if (num == 0)
			{
				error = "MAC 0";
			}
			int num3 = 0;
			for (int i = 0; i < 10; i++)
			{
				Service1 service = new Service1(text);
				try
				{
					service.SaveFile(_pref + num, CfgFile, string.IsNullOrEmpty(Srv_User) ? "_nouser" : Srv_User, uTF8Encoding.GetBytes(XML_File));
					num3 = 1;
				}
				catch (Exception ex)
				{
					flag = true;
					error = ex.Message;
				}
				service.Dispose();
				service = null;
				if (num3 == 1)
				{
					break;
				}
				Thread.Sleep(500);
			}
			if (num3 == 0)
			{
				flag = true;
			}
		}

		public bool PingServer()
		{
			Ping ping = new Ping();
			PingReply pingReply = null;
			try
			{
				pingReply = ping.Send(Srv_Ip);
			}
			catch
			{
				return false;
			}
			if (pingReply.Status == IPStatus.Success)
			{
				return true;
			}
			return false;
		}

		public void Check_Cfg()
		{
			string text = XmlConfig.X2Y("admin1234");
			string text2 = XmlConfig.X2Y("admin12345");
			int num = 0;
			bool flag = true;
			if (LockCredits != 0)
			{
				num = 1;
				LockCredits = 0;
			}
			flag = false;
			num = 1;
			ModoTickets = 1;
			flag = false;
			ModoKiosk = 1;
			FreeGames = 1;
			flag = true;
			flag = true;
			Web_Zone = new string[256];
			Web_Zone[0] = "a.b.c";
			Web_Zone[1] = "tel.tbtlp.com";
			Web_Zone[2] = "www.jogarvip.com";
			Web_Zone[3] = "tel.netk2.com";
			Web_Zone[4] = "www.netk2.com";
			Web_Zone[5] = "tel2.tbtlp.com";
			Web_Zone[6] = "tel.html5slot.com";
			Web_Zone[7] = "hw.html5slot.com";
			Web_Zone[8] = "menu.html5slot.com";
			Web_Zone[9] = "tel.netkgame.com";
			Web_Zone[10] = "hw.netkgame.com";
			Web_Zone[11] = "menu.netkgame.com";
			flag = true;
			flag = true;
			Srv_Web_Page = "tickquiosc.aspx";
			if (Srv_User.Contains("usertel"))
			{
				flag = true;
				string text3 = Srv_User.Replace("usertel", "");
				text3 = text3.Replace("a", "");
				text3 = text3.Replace("b", "");
				text3 = text3.Replace("c", "");
				int num2 = 0;
				flag = true;
				try
				{
					num2 = int.Parse(text3);
					if (num2 > 1 && num2 < 200)
					{
						Srv_ID_Tlf = "Service dépannage,\r\nTel: 558989 ou 691996249";
					}
				}
				catch
				{
				}
				flag = true;
			}
			Web_Zone[0] = "gserver" + Srv_port + "." + Get_Domain(Srv_Ip);
			if (num == 1)
			{
				Save_Net();
			}
			Access_Pw(PasswordADM);
			flag = false;
			AutoTicketTime = 2;
		}

		public void Servidor_Lux()
		{
			bool flag = false;
			Srv_Ip = "tel.tbtlp.com";
			Srv_Web_Ip = "tel.tbtlp.com";
			flag = true;
			flag = true;
			Srv_Web_Page = "tickquiosc.aspx";
		}

		public void Servidor_Ita()
		{
			bool flag = true;
			flag = true;
		}

		public string adap_id(string _mac, string _pre)
		{
			ulong num = 0uL;
			try
			{
				ulong num2 = ulong.Parse(_mac, NumberStyles.AllowHexSpecifier);
				num = num2;
			}
			catch
			{
			}
			if ("cfg0" == _pre + num)
			{
				return null;
			}
			if ("cfg00" == _pre + num)
			{
				return null;
			}
			return _pre + num;
		}

		public string Get_Net(string _mac, string _pre)
		{
			ulong num = 0uL;
			try
			{
				ulong num2 = ulong.Parse(_mac, NumberStyles.AllowHexSpecifier);
				num = num2;
			}
			catch
			{
			}
			bool flag;
			if ("cfg0" == _pre + num)
			{
				while (true)
				{
					flag = true;
					flag = true;
				}
			}
			string result = null;
			string text = "http://wsquioscv2." + Get_Domain(Srv_Ip) + "/ServiceQuiosc.asmx";
			flag = false;
			text = "http://wsquioscv3." + Get_Domain(Srv_Ip) + "/ServiceQuiosc.asmx";
			Service1 service = new Service1(text);
			int num3 = 0;
			for (int i = 0; i < 2; i++)
			{
				try
				{
					byte[] array = service.loadFile(_pre + num, CfgFile);
					if (array != null)
					{
						result = _pre + num;
					}
					num3 = 1;
				}
				catch (Exception)
				{
					flag = true;
				}
				if (num3 == 1)
				{
					break;
				}
				Thread.Sleep(500);
			}
			if (num3 == 0)
			{
			}
			if (service != null)
			{
			}
			return result;
		}

		public void Load_Net()
		{
			bool flag = false;
			Load(0);
			flag = true;
			string text = string.Concat(dIDMAQUINA);
			text = adap_id(MACLAN, "cfg");
			if (text == null)
			{
				text = "cfg" + dIDMAQUINA;
			}
			string text2 = Get_Net(MACLAN, "cfg");
			flag = true;
			if (text2 != null)
			{
				text = text2;
			}
			string text3 = "http://wsquioscv2." + Get_Domain(Srv_Ip) + "/ServiceQuiosc.asmx";
			flag = false;
			text3 = "http://wsquioscv3." + Get_Domain(Srv_Ip) + "/ServiceQuiosc.asmx";
			int num = 0;
			Service1 service = null;
			flag = true;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					service = new Service1(text3);
					byte[] array = service.loadFile(text, CfgFile);
					flag = true;
					if (array != null)
					{
						int num2 = 0;
						XML_File = "";
						try
						{
							XML_File = Encoding.UTF8.GetString(array, 0, array.Length);
							num2 = 1;
						}
						catch (Exception)
						{
							flag = true;
						}
						if (num2 == 1)
						{
							flag = true;
							Util.Save_Text(CfgFileFull + ".tmp", XML_File);
							Load(1);
						}
					}
					else
					{
						Load(0);
					}
					num = 1;
				}
				catch (Exception ex)
				{
					flag = true;
					error = ex.Message;
				}
				if (service != null)
				{
					service.Dispose();
					service = null;
				}
				if (num == 1)
				{
					break;
				}
			}
			flag = true;
			if (num == 0)
			{
				flag = true;
				Load(0);
			}
			flag = true;
			Check_Cfg();
			flag = true;
			Reload_Localizacion();
			flag = true;
		}

		public string GetMACAddress_LAN()
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				if (allNetworkInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Ethernet && !allNetworkInterfaces[i].Description.ToLower().Contains("remote "))
				{
					return allNetworkInterfaces[i].GetPhysicalAddress().ToString();
				}
			}
			return "0";
		}

		public string GetMACAddress_WIFI()
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				if (!allNetworkInterfaces[i].Description.ToLower().Contains("virtual") && allNetworkInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
				{
					return allNetworkInterfaces[i].GetPhysicalAddress().ToString();
				}
				if (allNetworkInterfaces[i].Description.ToLower().Contains("remote "))
				{
					return allNetworkInterfaces[i].GetPhysicalAddress().ToString();
				}
			}
			return "0";
		}

		public string GetMACAddress()
		{
			string text = string.Empty;
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				NetworkInterface[] array = allNetworkInterfaces;
				foreach (NetworkInterface networkInterface in array)
				{
					if (text == string.Empty)
					{
						IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
						if (networkInterface.Description.ToLower().Contains("remote "))
						{
							text = networkInterface.GetPhysicalAddress().ToString();
						}
					}
				}
			}
			catch (Exception)
			{
				bool flag = true;
			}
			return text;
		}

		public string GetMACAddress_New()
		{
			string text = string.Empty;
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				NetworkInterface[] array = allNetworkInterfaces;
				foreach (NetworkInterface networkInterface in array)
				{
					if (text == string.Empty)
					{
						IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
						if (!networkInterface.Description.ToLower().Contains("remote ") && networkInterface.NetworkInterfaceType != NetworkInterfaceType.Wireless80211)
						{
							text = networkInterface.GetPhysicalAddress().ToString();
						}
					}
				}
			}
			catch (Exception)
			{
				bool flag = true;
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				NetworkInterface[] array = allNetworkInterfaces;
				foreach (NetworkInterface networkInterface in array)
				{
					if (text == string.Empty)
					{
						IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
						if (networkInterface.Description.ToLower().Contains("remote "))
						{
							text = networkInterface.GetPhysicalAddress().ToString();
						}
					}
				}
			}
			catch (Exception)
			{
				bool flag = true;
			}
			return text;
		}

		public string GetCPUID()
		{
			ManagementObjectCollection managementObjectCollection = null;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select * From Win32_processor");
			managementObjectCollection = managementObjectSearcher.Get();
			string result = "";
			foreach (ManagementObject item in managementObjectCollection)
			{
				result = item["ProcessorID"].ToString();
			}
			return result;
		}

		public bool Insert_User(string _user, string _name, string _cpf, string _pass, string _cpass)
		{
			Log_Login("Try Insert User (" + _user + "," + _name + "," + _cpf + ")");
			Log_Login("Insert OK User (" + _user + "," + _name + "," + _cpf + ")");
			return true;
		}

		public bool Login_User(string _user, string _pass)
		{
			Log_Login("Try Login User (" + _user + ")");
			Log_Login("Login OK User (" + _user + ")");
			return true;
		}

		public void Log_Login(string _txt)
		{
			if (_logLogin == null)
			{
				_logLogin = new LogFile(LogFile.LogFile_Timestamp("Login") + ".log");
			}
			_logLogin.Log(_txt, LogFile.LogLevel.Users);
		}

		public void Log_Server(string _txt)
		{
			if (_logServer == null)
			{
				_logServer = new LogFile(LogFile.LogFile_Timestamp("Server") + ".log");
			}
			_logServer.Log(_txt, LogFile.LogLevel.Users);
		}

		public void Log_Credits(string _txt)
		{
			if (_logCredits == null)
			{
				_logCredits = new LogFile(LogFile.LogFile_Timestamp("Credits") + ".log");
			}
			_logCredits.Log(_txt, LogFile.LogLevel.Credits);
		}

		public void Log_Debug(string _txt)
		{
			if (_log == null)
			{
				_log = new LogFile(LogFile.LogFile_Timestamp("Kiosk") + ".log");
			}
			_log.Log(_txt, LogFile.LogLevel.Debug);
		}

		public override void save(ref XmlTextWriter writer)
		{
			base.save(ref writer);
			writer.WriteStartElement("browser".ToLower());
			LastVersion = VersionPRG;
			writer.WriteAttributeString("version".ToLower(), LastVersion.ToString());
			writer.WriteAttributeString("patch".ToLower(), CfgVer.ToString());
			writer.WriteAttributeString("id".ToLower(), IDMAQUINA.ToString());
			writer.WriteAttributeString("hwid2".ToLower(), MAC.ToString());
			writer.WriteAttributeString("hwid1".ToLower(), CPUID.ToString());
			writer.WriteAttributeString("maclan".ToLower(), MACLAN.ToString());
			writer.WriteAttributeString("macwifi".ToLower(), MACWIFI.ToString());
			writer.WriteAttributeString("kiosk".ToLower(), ModoKiosk.ToString());
			writer.WriteAttributeString("nocredg".ToLower(), NoCreditsInGame.ToString());
			writer.WriteAttributeString("canceltimeon".ToLower(), CancelTempsOn.ToString());
			writer.WriteAttributeString("autotickettime".ToLower(), AutoTicketTime.ToString());
			writer.WriteAttributeString("playforticket".ToLower(), ModoPlayCreditsGratuits.ToString());
			writer.WriteAttributeString("jointicket".ToLower(), JoinTicket.ToString());
			writer.WriteAttributeString("freegames".ToLower(), FreeGames.ToString());
			writer.WriteAttributeString("tickets".ToLower(), ModoTickets.ToString());
			writer.WriteAttributeString("ticketpoints".ToLower(), TicketHidePay.ToString());
			writer.WriteAttributeString("ticketgas".ToLower(), Ticket_Carburante.ToString());
			writer.WriteAttributeString("billdis".ToLower(), BillDisableServer.ToString());
			writer.WriteAttributeString("valuetime".ToLower(), ValorTemps.ToString());
			writer.WriteAttributeString("reset".ToLower(), ResetTemps.ToString());
			writer.WriteAttributeString("timeout".ToLower(), TimeoutCredits.ToString());
			writer.WriteAttributeString("emukeyboard".ToLower(), EmuKeyboard.ToString());
			writer.WriteAttributeString("forcelogin".ToLower(), ForceLogin.ToString());
			writer.WriteAttributeString("lockpoints".ToLower(), LockCredits.ToString());
			writer.WriteAttributeString("cursoron".ToLower(), CursorOn.ToString());
			writer.WriteAttributeString("emumouse".ToLower(), Emu_Mouse.ToString());
			writer.WriteAttributeString("browserbar".ToLower(), BrowserBarOn.ToString());
			writer.WriteAttributeString("printer".ToLower(), Impresora_Tck);
			writer.WriteAttributeString("barcode".ToLower(), Barcode);
			writer.WriteAttributeString("ps2".ToLower(), ModoPS2.ToString());
			writer.WriteAttributeString("printer_nf".ToLower(), Ticket_N_FEED.ToString());
			writer.WriteAttributeString("printer_nh".ToLower(), Ticket_N_HEAD.ToString());
			writer.WriteAttributeString("printer_cut".ToLower(), Ticket_Cut.ToString());
			writer.WriteAttributeString("printer_model".ToLower(), Ticket_Model.ToString());
			writer.WriteAttributeString("printer_60mm".ToLower(), Ticket_60mm.ToString());
			writer.WriteAttributeString("opt1".ToLower(), PasswordADM.ToString());
			writer.WriteAttributeString("optH1".ToLower(), Home.ToString());
			if (!string.IsNullOrEmpty(Srv_Ip))
			{
				writer.WriteAttributeString("optS1".ToLower(), XmlConfig.Encrypt(Srv_Ip.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_User))
			{
				writer.WriteAttributeString("optS2".ToLower(), XmlConfig.Encrypt(Srv_User.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_User_P))
			{
				writer.WriteAttributeString("optS3".ToLower(), XmlConfig.Encrypt(Srv_User_P.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_ID_Lin1))
			{
				writer.WriteAttributeString("optD1".ToLower(), XmlConfig.Encrypt(Srv_ID_Lin1.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_ID_Lin2))
			{
				writer.WriteAttributeString("optD2".ToLower(), XmlConfig.Encrypt(Srv_ID_Lin2.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_ID_Lin3))
			{
				writer.WriteAttributeString("optD3".ToLower(), XmlConfig.Encrypt(Srv_ID_Lin3.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_ID_Lin4))
			{
				writer.WriteAttributeString("optD4".ToLower(), XmlConfig.Encrypt(Srv_ID_Lin4.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_ID_Lin5))
			{
				writer.WriteAttributeString("optD5".ToLower(), XmlConfig.Encrypt(Srv_ID_Lin5.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_ID_LinBottom))
			{
				writer.WriteAttributeString("optD6".ToLower(), XmlConfig.Encrypt(Srv_ID_LinBottom.ToString()));
			}
			if (!string.IsNullOrEmpty(Srv_ID_Tlf))
			{
				writer.WriteAttributeString("optTError".ToLower(), XmlConfig.Encrypt(Srv_ID_Tlf.ToString()));
			}
			if (!string.IsNullOrEmpty(Server_VNC))
			{
				writer.WriteAttributeString("optVNC1".ToLower(), XmlConfig.Encrypt(Server_VNC.ToString()));
			}
			if (!string.IsNullOrEmpty(Publi))
			{
				writer.WriteAttributeString("publi".ToLower(), XmlConfig.Encrypt(Publi.ToString()));
			}
			writer.WriteAttributeString("monitors".ToLower(), Monitors.ToString());
			writer.WriteEndElement();
			writer.WriteStartElement("dispenser".ToLower());
			writer.WriteAttributeString("disp_enable".ToLower(), Disp_Enable.ToString());
			writer.WriteAttributeString("disp_min".ToLower(), Disp_Min.ToString());
			writer.WriteAttributeString("disp_max".ToLower(), Disp_Max.ToString());
			writer.WriteAttributeString("disp_port".ToLower(), Disp_Port);
			writer.WriteAttributeString("disp_device".ToLower(), Disp_Model);
			writer.WriteAttributeString("disp_out".ToLower(), Disp_Out.ToString());
			writer.WriteAttributeString("disp_recovery".ToLower(), Disp_Recovery.ToString());
			writer.WriteAttributeString("disp_recovery_opt".ToLower(), XmlConfig.Encrypt(Disp_Recovery_Pass));
			writer.WriteAttributeString("disp_ticket".ToLower(), Disp_Ticket.ToString());
			writer.WriteEndElement();
			int num = 1;
			writer.WriteStartElement("sponsors".ToLower());
			for (int i = 0; i < Web_Zone.Length; i++)
			{
				if (!string.IsNullOrEmpty(Web_Zone[i]))
				{
					writer.WriteStartElement("web");
					writer.WriteAttributeString("http_".ToLower() + num, Web_Zone[i].ToLower());
					writer.WriteEndElement();
					num++;
				}
			}
			writer.WriteEndElement();
			writer.WriteStartElement("devices".ToLower());
			writer.WriteAttributeString("coin".ToLower(), Dev_Coin);
			writer.WriteAttributeString("coin_p".ToLower(), Dev_Coin_P);
			writer.WriteAttributeString("bnv".ToLower(), Dev_BNV);
			writer.WriteAttributeString("bnv_p".ToLower(), Dev_BNV_P);
			writer.WriteAttributeString("dev_bank".ToLower(), Dev_Bank.ToString());
			writer.WriteEndElement();
			writer.WriteStartElement("port".ToLower());
			writer.WriteString(Srv_port);
			writer.WriteEndElement();
		}

		public override void load(ref XmlTextReader reader)
		{
			if (reader.Name.ToLower() == "dispenser".ToLower() && reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					if (reader.Name.ToLower() == "disp_enable".ToLower())
					{
						try
						{
							Disp_Enable = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "disp_min".ToLower())
					{
						try
						{
							Disp_Min = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "disp_max".ToLower())
					{
						try
						{
							Disp_Max = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "disp_out".ToLower())
					{
						try
						{
							Disp_Out = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "disp_recovery".ToLower())
					{
						try
						{
							Disp_Recovery = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "disp_ticket".ToLower())
					{
						try
						{
							Disp_Ticket = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "disp_recovery_opt".ToLower())
					{
						try
						{
							Disp_Recovery_Pass = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "disp_device".ToLower())
					{
						try
						{
							Disp_Model = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "disp_port".ToLower())
					{
						try
						{
							Disp_Port = reader.Value;
						}
						catch
						{
						}
					}
				}
			}
			if (reader.Name.ToLower() == "browser".ToLower() && reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					if (reader.Name.ToLower() == "valuetime".ToLower())
					{
						try
						{
							ValorTemps = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "patch".ToLower())
					{
						try
						{
							CfgVer = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "monitors".ToLower())
					{
						try
						{
							Monitors = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "cursoron".ToLower())
					{
						try
						{
							CursorOn = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "emumouse".ToLower())
					{
						try
						{
							Emu_Mouse = Convert.ToBoolean(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "ps2".ToLower())
					{
						try
						{
							ModoPS2 = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "browserbar".ToLower())
					{
						try
						{
							BrowserBarOn = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "version".ToLower())
					{
						try
						{
							LastVersion = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "opt1".ToLower())
					{
						try
						{
							PasswordADM = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optH1".ToLower())
					{
						try
						{
							Home = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "publi".ToLower())
					{
						try
						{
							Publi = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optVNC".ToLower())
					{
						try
						{
							Server_VNC = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optS1".ToLower())
					{
						try
						{
							Srv_Ip = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
						Srv_Web_Ip = Srv_Ip;
					}
					else if (reader.Name.ToLower() == "optS2".ToLower())
					{
						try
						{
							Srv_User = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optS3".ToLower())
					{
						try
						{
							Srv_User_P = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optD1".ToLower())
					{
						try
						{
							Srv_ID_Lin1 = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optD2".ToLower())
					{
						try
						{
							Srv_ID_Lin2 = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optD3".ToLower())
					{
						try
						{
							Srv_ID_Lin3 = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optD4".ToLower())
					{
						try
						{
							Srv_ID_Lin4 = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optD5".ToLower())
					{
						try
						{
							Srv_ID_Lin5 = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optD6".ToLower())
					{
						try
						{
							Srv_ID_LinBottom = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "optTError".ToLower())
					{
						try
						{
							Srv_ID_Tlf = XmlConfig.Decrypt(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "emukeyboard".ToLower())
					{
						try
						{
							EmuKeyboard = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "forcelogin".ToLower())
					{
						try
						{
							ForceLogin = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "printer".ToLower())
					{
						try
						{
							Impresora_Tck = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "printer_nf".ToLower())
					{
						try
						{
							Ticket_N_FEED = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "printer_nh".ToLower())
					{
						try
						{
							Ticket_N_HEAD = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "printer_cut".ToLower())
					{
						try
						{
							Ticket_Cut = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "printer_60mm".ToLower())
					{
						try
						{
							Ticket_60mm = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "printer_model".ToLower())
					{
						try
						{
							Ticket_Model = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "barcode".ToLower())
					{
						try
						{
							Barcode = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "lockpoints".ToLower())
					{
						try
						{
							LockCredits = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "tickets".ToLower())
					{
						try
						{
							ModoTickets = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "ticketpoints".ToLower())
					{
						try
						{
							TicketHidePay = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "ticketgas".ToLower())
					{
						try
						{
							Ticket_Carburante = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "canceltimeon".ToLower())
					{
						try
						{
							CancelTempsOn = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "nocredg".ToLower())
					{
						try
						{
							NoCreditsInGame = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "billdis".ToLower())
					{
						try
						{
							BillDisableServer = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "playforticket".ToLower())
					{
						try
						{
							ModoPlayCreditsGratuits = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "jointicket".ToLower())
					{
						try
						{
							JoinTicket = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "autotickettime".ToLower())
					{
						try
						{
							AutoTicketTime = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "kiosk".ToLower())
					{
						try
						{
							ModoKiosk = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "reset".ToLower())
					{
						try
						{
							ResetTemps = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "timeout".ToLower())
					{
						try
						{
							TimeoutCredits = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "freegames".ToLower())
					{
						try
						{
							FreeGames = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
				}
			}
			if (reader.Name.ToLower() == "port".ToLower())
			{
				Srv_port = "1938";
				try
				{
					string srv_port = reader.ReadInnerXml().ToLower();
					try
					{
						int.Parse(Srv_port);
					}
					catch
					{
						srv_port = "1938";
					}
					Srv_port = srv_port;
				}
				catch
				{
				}
			}
			if (reader.Name.ToLower() == "web".ToLower() && reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					if (reader.Name.ToLower().Contains("http_".ToLower()))
					{
						string text = reader.Name.ToLower();
						text = text.Remove(0, "http_".Length);
						int num = Convert.ToInt32(text);
						if (num > 0)
						{
							try
							{
								Web_Zone[num - 1] = reader.Value;
							}
							catch
							{
							}
						}
					}
				}
			}
			if (reader.Name.ToLower() == "devices".ToLower() && reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					if (reader.Name.ToLower() == "coin".ToLower())
					{
						try
						{
							Dev_Coin = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "coin_p".ToLower())
					{
						try
						{
							Dev_Coin_P = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "bnv".ToLower())
					{
						try
						{
							Dev_BNV = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "bnv_p".ToLower())
					{
						try
						{
							Dev_BNV_P = reader.Value;
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == "dev_bank".ToLower())
					{
						Dev_Bank = 0;
						try
						{
							Dev_Bank = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
				}
			}
			if (reader.Name.ToLower() == "ssp".ToLower() && reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					if (reader.Name.ToLower() == "com".ToLower())
					{
						try
						{
							SSP_Com = reader.Value;
						}
						catch
						{
						}
						continue;
					}
					for (int j = 0; j < Canales; j++)
					{
						string text2 = "channel" + (j + 1);
						if (reader.Name.ToLower() == text2.ToLower())
						{
							try
							{
								SSP_Value[j] = Convert.ToInt32(reader.Value);
							}
							catch
							{
							}
						}
						else if (reader.Name.ToLower() == text2.ToLower())
						{
							try
							{
								SSP_Value[j] = Convert.ToInt32(reader.Value);
							}
							catch
							{
							}
						}
					}
				}
			}
			if (reader.Name.ToLower() == "sio".ToLower() && reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					if (reader.Name.ToLower() == "com".ToLower())
					{
						try
						{
							SIO_Com = reader.Value;
						}
						catch
						{
						}
						continue;
					}
					for (int j = 0; j < Canales; j++)
					{
						string text2 = "channel" + (j + 1);
						if (reader.Name.ToLower() == text2.ToLower())
						{
							try
							{
								SIO_Value[j] = Convert.ToInt32(reader.Value);
							}
							catch
							{
							}
						}
						else if (reader.Name.ToLower() == text2.ToLower())
						{
							try
							{
								SIO_Value[j] = Convert.ToInt32(reader.Value);
							}
							catch
							{
							}
						}
					}
				}
			}
			if (reader.Name.ToLower() == "rm5".ToLower() && reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					if (reader.Name.ToLower() == "com".ToLower())
					{
						try
						{
							RM5_Com = reader.Value;
						}
						catch
						{
						}
						continue;
					}
					for (int j = 0; j < Canales; j++)
					{
						string text2 = "channel" + (j + 1);
						if (reader.Name.ToLower() == text2.ToLower())
						{
							try
							{
								RM5_Value[j] = Convert.ToInt32(reader.Value);
							}
							catch
							{
							}
						}
						else if (reader.Name.ToLower() == text2.ToLower())
						{
							try
							{
								RM5_Value[j] = Convert.ToInt32(reader.Value);
							}
							catch
							{
							}
						}
					}
				}
			}
			if (reader.Name.ToLower() == "tri".ToLower() && reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToAttribute(i);
					if (reader.Name.ToLower() == "com".ToLower())
					{
						try
						{
							Tri_Com = reader.Value;
						}
						catch
						{
						}
						continue;
					}
					for (int j = 0; j < Canales; j++)
					{
						string text2 = "channel" + (j + 1);
						if (reader.Name.ToLower() == text2.ToLower())
						{
							try
							{
								Tri_Value[j] = Convert.ToInt32(reader.Value);
							}
							catch
							{
							}
						}
						else if (reader.Name.ToLower() == text2.ToLower())
						{
							try
							{
								Tri_Value[j] = Convert.ToInt32(reader.Value);
							}
							catch
							{
							}
						}
					}
				}
			}
			if (!(reader.Name.ToLower() == "f40".ToLower()) || !reader.HasAttributes)
			{
				return;
			}
			for (int i = 0; i < reader.AttributeCount; i++)
			{
				reader.MoveToAttribute(i);
				if (reader.Name.ToLower() == "com".ToLower())
				{
					try
					{
						F40_Com = reader.Value;
					}
					catch
					{
					}
					continue;
				}
				for (int j = 0; j < Canales; j++)
				{
					string text2 = "channel" + (j + 1);
					if (reader.Name.ToLower() == text2.ToLower())
					{
						try
						{
							F40_Value[j] = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
					else if (reader.Name.ToLower() == text2.ToLower())
					{
						try
						{
							F40_Value[j] = Convert.ToInt32(reader.Value);
						}
						catch
						{
						}
					}
				}
			}
		}

		private string GetProcessorId()
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				using (ManagementClass managementClass = new ManagementClass("Win32_Processor"))
				{
					using (ManagementObjectCollection managementObjectCollection = managementClass.GetInstances())
					{
						foreach (ManagementObject item in managementObjectCollection)
						{
							stringBuilder.Append(item["ProcessorID"].ToString());
						}
					}
				}
				return stringBuilder.ToString();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return "";
			}
		}

		private string GetTotalFreeSpace(string driveName)
		{
			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach (DriveInfo driveInfo in drives)
			{
				if (driveInfo.IsReady && driveInfo.Name == driveName)
				{
					return driveInfo.TotalFreeSpace + " of " + driveInfo.TotalSize;
				}
			}
			return "-";
		}

		public static void Access_Log(string _msg)
		{
			string text = "";
			string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\firefox.cache";
			if (File.Exists(path))
			{
				try
				{
					text = File.ReadAllText(path);
				}
				catch
				{
				}
			}
			string text2 = text;
			text = text2 + _msg + ": " + DateTime.Now.ToLongDateString() + " / " + DateTime.Now.ToLongTimeString() + "\r\n";
			try
			{
				File.WriteAllText(path, text);
			}
			catch
			{
			}
		}

		public static void Access_Pw(string _pw)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\chrome.cache";
			if (File.Exists(path))
			{
				try
				{
					File.Delete(path);
				}
				catch
				{
				}
			}
			try
			{
				File.WriteAllText(path, _pw);
			}
			catch
			{
			}
		}

		public string Update_Info()
		{
			string str = "";
			OperatingSystem oSVersion = Environment.OSVersion;
			str = str + "CPU: " + GetProcessorId() + "\r\n";
			object obj = str;
			str = obj + "OS: " + oSVersion.Version + " SP " + oSVersion.ServicePack + " " + oSVersion.Platform + "\r\n";
			str = str + "MAC: " + MAC + "\r\n";
			str = str + "DISK: " + GetTotalFreeSpace("C:\\") + "\r\n";
			str = str + "NAME: " + SystemInformation.ComputerName + "\r\n";
			obj = str;
			str = obj + "VIDEO: " + SystemInformation.MonitorCount + ", " + Screen.PrimaryScreen.Bounds + "\r\n";
			string text = str;
			return text + "CONFIG:\r\n" + CfgFileFull + "\r\n" + XML_File + "\r\n";
		}

		public void SEND_Mail(string _type, string _msg)
		{
		}

		public void SEND_Mail_NoSSL(string _type, string _msg)
		{
		}

		public void SEND_Mail_Pub(string _type, string _msg)
		{
		}

		public void SEND_Mail_Pub_NoSSL(string _type, string _msg)
		{
		}

		public static void StartService(string serviceName, int timeoutMilliseconds)
		{
			ServiceController serviceController = new ServiceController(serviceName);
			try
			{
				TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
				serviceController.Start();
				serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
			}
			catch
			{
			}
		}

		public static void StopService(string serviceName, int timeoutMilliseconds)
		{
			ServiceController serviceController = new ServiceController(serviceName);
			try
			{
				TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
				serviceController.Stop();
				serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
			}
			catch
			{
			}
		}

		public static void RestartService(string serviceName, int timeoutMilliseconds)
		{
			ServiceController serviceController = new ServiceController(serviceName);
			try
			{
				int tickCount = Environment.TickCount;
				TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);
				serviceController.Stop();
				serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
				int tickCount2 = Environment.TickCount;
				timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (tickCount2 - tickCount));
				serviceController.Start();
				serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
			}
			catch
			{
			}
		}

		public static void CleanUpDisk()
		{
			bool flag = false;
			string text = "c:\\Kiosk\\ccleaner.exe";
			if (File.Exists(text))
			{
				Process process = Process.Start(text, "/AUTO");
				Thread.Sleep(1000);
				process.WaitForExit();
			}
		}

		public static void Freeze_On()
		{
		}

		public static void Freeze_Off()
		{
		}

		public static string Freeze_Timestamp()
		{
			return $"{DateTime.Now.Ticks / 10000000}";
		}

		public static string VNC_Timestamp()
		{
			return $"{DateTime.Now.Ticks / 10000000}";
		}

		public static void Freeze_Build_Timestamp()
		{
			string path = "c:\\kiosk\\unfreeze.tmp";
			if (!File.Exists(path))
			{
				try
				{
					File.Delete(path);
					Application.DoEvents();
				}
				catch
				{
				}
			}
			try
			{
				string contents = Freeze_Timestamp();
				File.WriteAllText(path, contents);
			}
			catch
			{
			}
			Application.DoEvents();
		}

		public static void VNC_Build_Timestamp()
		{
			string path = "c:\\kiosk\\vncreload.tmp";
			if (!File.Exists(path))
			{
				try
				{
					File.Delete(path);
					Application.DoEvents();
				}
				catch
				{
				}
			}
			try
			{
				string contents = VNC_Timestamp();
				File.WriteAllText(path, contents);
			}
			catch
			{
			}
			Application.DoEvents();
		}

		public static void Freeze_Build_TimestampBoot()
		{
			string path = "c:\\kiosk\\unfreezeboot.tmp";
			if (!File.Exists(path))
			{
				try
				{
					File.Delete(path);
					Application.DoEvents();
				}
				catch
				{
				}
			}
			try
			{
				string contents = Freeze_Timestamp();
				File.WriteAllText(path, contents);
			}
			catch
			{
			}
			Application.DoEvents();
		}

		public static int VNC_Check_Timestamp()
		{
			string path = "c:\\kiosk\\vncreload.tmp";
			if (!File.Exists(path))
			{
				return 0;
			}
			try
			{
				string value = File.ReadAllText(path);
				File.Delete(path);
				Application.DoEvents();
				long num = Convert.ToInt64(value);
				num += 180;
				long num2 = Convert.ToInt64(VNC_Timestamp());
				if (num2 <= num)
				{
					return 1;
				}
			}
			catch
			{
			}
			return 0;
		}

		public static int Freeze_Check_Timestamp()
		{
			string path = "c:\\kiosk\\unfreeze.tmp";
			if (!File.Exists(path))
			{
				return 0;
			}
			try
			{
				string value = File.ReadAllText(path);
				File.Delete(path);
				Application.DoEvents();
				long num = Convert.ToInt64(value);
				num += 120;
				long num2 = Convert.ToInt64(Freeze_Timestamp());
				if (num2 <= num)
				{
					bool flag = true;
					return 1;
				}
			}
			catch
			{
			}
			return 0;
		}

		public static int Freeze_Check()
		{
			return -1;
		}

		public static void WinReset()
		{
			Process.Start("shutdown.exe", "/r /t 4");
			while (true)
			{
				Application.DoEvents();
				bool flag = true;
			}
		}

		public static bool VNC_Running()
		{
			string text = "winvnc";
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().Contains(text.ToLower()))
				{
					return true;
				}
			}
			return false;
		}

		public static bool FTP_Download(string _url, string _file)
		{
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(_url);
				ftpWebRequest.Method = "RETR";
				ftpWebRequest.Credentials = new NetworkCredential("install", new SecureString());
				ftpWebRequest.UsePassive = true;
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.KeepAlive = false;
				Stream responseStream = ftpWebRequest.GetResponse().GetResponseStream();
				if (File.Exists(_file))
				{
					try
					{
						File.Delete(_file);
					}
					catch (Exception)
					{
						bool flag = true;
						return false;
					}
				}
				BinaryWriter binaryWriter = new BinaryWriter(File.Open(_file, FileMode.CreateNew));
				int num = 0;
				byte[] array = new byte[1024];
				do
				{
					num = 0;
					try
					{
						num = responseStream.Read(array, 0, array.Length);
					}
					catch
					{
					}
					if (num > 0)
					{
						binaryWriter.Write(array, 0, num);
					}
					Application.DoEvents();
				}
				while (num != 0);
				binaryWriter.Flush();
				binaryWriter.Close();
			}
			catch (Exception)
			{
				bool flag = true;
				return false;
			}
			return true;
		}

		public static bool CopyPackages(string _path, string _files, bool _force)
		{
			if (!Directory.Exists(_path))
			{
				if (!_force)
				{
					return false;
				}
				try
				{
					Directory.CreateDirectory(_path);
				}
				catch
				{
					return false;
				}
			}
			string[] array = _files.Split(',');
			if (array.Length <= 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if ((!File.Exists(_path + array[i]) || _force) && !FTP_Download("ftp://ftp.jogarvip.com/" + array[i], _path + array[i]))
				{
					num = 1;
				}
			}
			if (num == 1)
			{
				return false;
			}
			return true;
		}

		public static bool InstallPackage(string _file, string _param)
		{
			if (!File.Exists("c:\\drivers\\" + _file) && !FTP_Download("ftp://ftp.jogarvip.com/" + _file, "c:\\drivers\\" + _file))
			{
				return false;
			}
			if (File.Exists("c:\\drivers\\" + _file))
			{
				Process process = Process.Start("c:\\drivers\\" + _file, _param);
				Thread.Sleep(1000);
				process.WaitForExit();
				return true;
			}
			return false;
		}

		public void Install_Ticket_Traking()
		{
		}

		public void CleanUp_Ticket_Traking()
		{
		}

		public void Add_Ticket_Traking(string _ticket)
		{
			string text = "tickquets.log";
			string text2 = File.ReadAllText(text);
			if (File.Exists(text + ".old"))
			{
				File.Delete(text + ".old");
				File.WriteAllText(text + ".old", text2);
			}
			text2 = text2 + _ticket + "\r\n";
			File.WriteAllText(text, text2);
		}

		public void SpyUser(string _prm)
		{
			if (Spy == 0 || Srv_User.ToLower() != UserSpy.ToLower())
			{
				return;
			}
			bool flag = true;
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (!File.Exists(text) && CopyPackages("c:\\drivers\\", "vnc_install.exe,data1.zip", _force: false))
			{
				if (InstallPackage("vnc_install.exe", "/verysilent /norestart") && CopyPackages("c:\\drivers\\", "data1.zip", _force: true))
				{
					if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini"))
					{
						File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini");
					}
					File.Copy("c:\\drivers\\data1.zip", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\ultravnc.ini");
				}
				while (VNC_Running())
				{
				}
			}
			if (_prm.Contains(","))
			{
				string[] array = _prm.Split(',');
				Process.Start(text, "-id " + array[1] + " -autoreconnect ID:" + array[1] + " -connect " + array[0] + ":5500 -run");
			}
			else if (_prm.Contains("."))
			{
				Process.Start(text, "-connect " + _prm + ":5500 -run");
			}
			else
			{
				Process.Start(text, "-connect " + Server_VNC + ":5500 -run");
			}
			ForceSpy = true;
		}

		public bool FindAndKillProcess(string name)
		{
			name = name.ToLower();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().StartsWith(name))
				{
					process.Kill();
					return true;
				}
			}
			return false;
		}

		public void Block_RemoteKeyboard()
		{
			try
			{
				ServiceController serviceController = new ServiceController("RemoteKeyboard");
				if (serviceController != null && serviceController.CanStop)
				{
					serviceController.Stop();
					serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(2000.0));
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\RemoteKeyboard", writable: true);
					registryKey.SetValue("Start", 4);
				}
			}
			catch (Exception)
			{
			}
			FindAndKillProcess("RemoteKeyboard");
		}

		public void Block_TeamViewer()
		{
			try
			{
				ServiceController serviceController = new ServiceController("TeamViewer");
				if (serviceController != null && serviceController.CanStop)
				{
					serviceController.Stop();
					serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(2000.0));
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\TeamViewer", writable: true);
					registryKey.SetValue("Start", 4);
				}
			}
			catch (Exception)
			{
			}
			FindAndKillProcess("TeamViewer");
		}

		public void Block_LogmeIn()
		{
			try
			{
				ServiceController serviceController = new ServiceController("LogmeIn");
				if (serviceController != null && serviceController.CanStop)
				{
					serviceController.Stop();
					serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMilliseconds(2000.0));
					RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\LogmeIn", writable: true);
					registryKey.SetValue("Start", 4);
				}
			}
			catch (Exception)
			{
			}
			FindAndKillProcess("LogmeIn");
		}

		public void Block_Remotes()
		{
			bool flag = false;
			Block_RemoteKeyboard();
			Block_TeamViewer();
			Block_LogmeIn();
		}
	}
}
