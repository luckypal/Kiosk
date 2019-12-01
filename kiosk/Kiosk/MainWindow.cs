using BarcodeLib;
using CefSharp;
using CefSharp.WinForms;
using CoreAudioApi;
using FluorineFx;
using FluorineFx.Messaging.Api.Service;
using FluorineFx.Net;
using GLib;
using GLib.Config;
using GLib.Devices;
using Kiosk.Properties;
using Microsoft.Win32;
using NiiPrinterCLib;
using RawInput_dll;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Media;
using System.Net;
using System.Net.NetworkInformation;
using System.Printing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Kiosk
{
	public class MainWindow : Form, IPendingServiceCallback
	{
		public class MenuHandler : IContextMenuHandler
		{
			void IContextMenuHandler.OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
			{
			}

			bool IContextMenuHandler.OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
			{
				return false;
			}

			void IContextMenuHandler.OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
			{
			}

			bool IContextMenuHandler.RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
			{
				return true;
			}
		}

		public class JsDialogHandler : IJsDialogHandler
		{
			public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, string acceptLang, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
			{
				return false;
			}

			public bool OnJSBeforeUnload(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
			{
				return false;
			}

			public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
			{
			}

			public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
			{
			}
		}

		public class LifeSpanHandler : ILifeSpanHandler
		{
			public event Action<string> PopupRequest;

			public bool DoClose(IWebBrowser browserControl, IBrowser browser)
			{
				return true;
			}

			public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
			{
			}

			public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
			{
			}

			public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IWindowInfo windowInfo, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
			{
				newBrowser = browserControl;
				return true;
			}
		}

		public class NavIRequestHandler : IRequestHandler, ILoadHandler
		{
			public readonly IWebBrowser model;

			public readonly MainWindow MainW;

			public void OnFrameLoadEnd(IWebBrowser browserControl, FrameLoadEndEventArgs frameLoadEndArgs)
			{
			}

			public void OnFrameLoadStart(IWebBrowser browserControl, FrameLoadStartEventArgs frameLoadStartArgs)
			{
			}

			public void OnLoadingStateChange(IWebBrowser browserControl, LoadingStateChangedEventArgs loadingStateChangedArgs)
			{
			}

			public NavIRequestHandler(IWebBrowser _model, MainWindow _main)
			{
				model = _model;
				model.RequestHandler = this;
				model.LoadHandler = this;
				MainW = _main;
			}

			public void OnResourceLoadComplete(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response, UrlRequestStatus status, long receivedContentLength)
			{
			}

			public void OnResourceRedirect(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, ref string newUrl)
			{
			}

			public void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
			{
			}

			public bool OnQuotaRequest(IWebBrowser browserControl, IBrowser browser, string originUrl, long newSize, IRequestCallback callback)
			{
				return false;
			}

			public bool OnProtocolExecution(IWebBrowser browserControl, IBrowser browser, string url)
			{
				return false;
			}

			public void OnRenderViewReady(IWebBrowser browserControl, IBrowser browser)
			{
			}

			public bool OnCertificateError(IWebBrowser browserControl, IBrowser browser, CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
			{
				return false;
			}

			public void OnPluginCrashed(IWebBrowser browserControl, IBrowser browser, string pluginPath)
			{
			}

			public bool OnBeforeMenu(IWebBrowser browser)
			{
				return true;
			}

			public CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
			{
				if (request.ReferrerUrl.ToLower().Contains("://menu.") && !request.Url.ToLower().Contains("://menu.") && request.TransitionType == TransitionType.CliendRedirect && request.Url.ToLower().Contains("tickquioscv2.aspx".ToLower()))
				{
					MainW.Status = Fases.GoHome;
				}
				Console.WriteLine(request.ReferrerUrl + " <> " + request.Url + " <> " + request.Method + " <> " + request.TransitionType);
				return CefReturnValue.Continue;
			}

			public void OnLoadError(IWebBrowser browserControl, LoadErrorEventArgs loadErrorArgs)
			{
				if (loadErrorArgs.ErrorCode != CefErrorCode.NameNotResolved && loadErrorArgs.ErrorCode != CefErrorCode.InternetDisconnected)
				{
				}
			}

			public bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool isRedirect)
			{
				Keys modifierKeys = Control.ModifierKeys;
				if ((modifierKeys & Keys.Shift) == Keys.Shift || (modifierKeys & Keys.Control) == Keys.Control)
				{
					return true;
				}
				if (MainW == null)
				{
					return true;
				}
				if (MainW.navegador == null)
				{
					return true;
				}
				if (MainW.Opcions.ComprarTemps > 0 && MainW.Opcions.News != 1)
				{
					return true;
				}
				MainW.Show_Navegador();
				string[] array = request.Url.Split(':');
				string[] array2 = request.Url.Split('/');
				if (MainW.Opcions.News == 1)
				{
					int num = 0;
					for (int i = 0; i < MainW.Opcions.Web_Zone.Length; i++)
					{
						if (!string.IsNullOrEmpty(MainW.Opcions.Web_Zone[i]) && request.Url.ToLower().Contains(MainW.Opcions.Web_Zone[i].ToLower()))
						{
							num = 1;
						}
					}
					if (num == 0)
					{
						if (!request.Url.ToLower().Contains("#lp-main"))
						{
							request.Url += "#lp-main";
						}
						if (request.Url.ToLower().Contains("&"))
						{
							return true;
						}
					}
				}
				if (MainW.Opcions.FullScreen == 1)
				{
					string text = request.Url.ToLower();
					if (MainW.Opcions.ModoKiosk != 0 && !text.Contains("StartGameQuiosk.aspx".ToLower()))
					{
						if (MainW.Opcions.ModoKiosk == 1)
						{
							MainW.Opcions.FullScreen = 2;
						}
						else
						{
							MainW.GoGames();
						}
					}
				}
				if (MainW.Opcions.TimeNavigate)
				{
				}
				if ((array[0].ToLower() == "http".ToLower() || array[0].ToLower() == "https".ToLower()) && array2[2].ToLower() == MainW.Opcions.Srv_Ip)
				{
					if (request.Url.ToLower().Contains("StartGameQuiosk.aspx".ToLower()))
					{
						MainW.CloseOSK();
					}
					if (MainW.Opcions.ModoTickets == 1 && request.Url.ToLower().Contains("tickquioscv2.aspx".ToLower()))
					{
						MainW.MenuGames = 0;
					}
					return false;
				}
				if (array[0].ToLower() == "http".ToLower() || array[0].ToLower() == "https".ToLower())
				{
					if (MainW.Opcions.Credits <= 0m && MainW.Opcions.Temps <= 0 && MainW.Opcions.ModoKiosk == 1 && MainW.Opcions.News != 1)
					{
						if (MainW.Opcions.News == 0)
						{
							MainW.Stop_Temps();
							if (MainW.Opcions.ModoTickets == 0)
							{
								try
								{
									DLG_Message dLG_Message = new DLG_Message(MainW.Opcions.Localize.Text("Insert credits"), ref MainW.Opcions);
									dLG_Message.ShowDialog();
								}
								catch
								{
								}
							}
							else
							{
								try
								{
									DLG_Message dLG_Message = new DLG_Message(MainW.Opcions.Localize.Text("Insert credits or ticket"), ref MainW.Opcions);
									dLG_Message.ShowDialog();
								}
								catch
								{
								}
							}
							MainW.Status = Fases.GoHome;
							return true;
						}
						if (MainW.Opcions.BrowserBarOn == 0)
						{
							return true;
						}
					}
					if (MainW.Opcions.Temps <= 0 && MainW.Opcions.ModoTickets == 0 && MainW.Opcions.ModoKiosk == 1 && MainW.Opcions.News != 1)
					{
						if (MainW.Opcions.News == 1)
						{
							MainW.Opcions.ComprarTemps = 1;
							return true;
						}
						if (MainW.Opcions.BrowserBarOn == 0)
						{
							return true;
						}
					}
					if (MainW.Opcions.News != 2)
					{
						MainW._WebLink = request.Url;
						MainW.Start_Temps();
						if (MainW.Opcions.News != 1)
						{
							MainW.Status = Fases.GoNavigate;
						}
					}
					else
					{
						MainW.Start_Temps();
					}
					return false;
				}
				return true;
			}

			public bool OnBeforeResourceLoad(IWebBrowser browser, IRequest requestResponse)
			{
				return false;
			}

			public bool OnResourceResponse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
			{
				return false;
			}

			public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength, ref IDownloadHandler handler)
			{
				return false;
			}

			public bool GetAuthCredentials(IWebBrowser browserControl, IBrowser browser, IFrame frame, bool isProxy, string host, int port, string realm, string scheme, IAuthCallback callback)
			{
				return false;
			}

			public bool OnOpenUrlFromTab(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
			{
				return false;
			}
		}

		private enum Fases
		{
			StartUp,
			WaitStartUp,
			Reset,
			WaitReset,
			WaitDevices,
			GoUpdate,
			Update,
			Splash,
			WaitSplash,
			CheckRoom,
			WaitCheckRoom,
			LockUser,
			WaitLockUser,
			TestUser,
			WaitTestUser,
			TestCom,
			WaitTestCom,
			Config,
			WaitConfig,
			Register,
			WaitRegister,
			GoLogin,
			Login,
			GoHome,
			Home,
			GoRemoteReset,
			RemoteReset,
			GoManteniment,
			Manteniment,
			GoNavigate,
			Navigate,
			NavigateScreenSaver,
			Admin,
			CheckDevice,
			WaitCheckDevice,
			Calibrar,
			WaitCalibrar,
			Extern,
			OutOfService,
			WaitOutOfService,
			Stop
		}

		public struct POINT
		{
			public int X;

			public int Y;

			public static implicit operator Point(POINT point)
			{
				return new Point(point.X, point.Y);
			}
		}

		internal struct KBDLLHOOKSTRUCT
		{
			public int vkCode;

			private int scanCode;

			public int flags;

			private int time;

			private int dwExtraInfo;
		}

		internal delegate IntPtr HookHandlerDelegate(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

		private enum DBT
		{
			DBT_DEVICEARRIVAL = 0x8000,
			DBT_DEVICEREMOVECOMPLETE = 32772,
			DBT_DEVTYP_OEM = 0,
			DBT_DEVTYP_VOLUME = 2,
			DBT_DEVTYP_PORT = 3,
			DBT_DEVTYP_DEVICEINTERFACE = 5,
			DBT_DEVTYP_HANDLE = 6
		}

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

		public class UsersRSO : RemoteSharedObject
		{
			public void msgFromSrvr(string msg)
			{
			}
		}

		private enum Srv_Command
		{
			Null,
			Status,
			Credits,
			AddCredits,
			SubCredits,
			SubCadeaux,
			AddTicket,
			SubTicket,
			Room,
			AnularTicket,
			VerificarTicket,
			KioskSetTime,
			KioskGetTime,
			KioskCommand
		}

		public class Hook_Srv_KioskCommand : IPendingServiceCallback
		{
			public int OK;

			public string Resposta;

			public int timeout;

			public Hook_Srv_KioskCommand()
			{
				OK = -2;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						OK = 1;
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_KioskSetTime : IPendingServiceCallback
		{
			public int OK;

			public int segons;

			public string Resposta;

			public int timeout;

			public Hook_Srv_KioskSetTime()
			{
				OK = -2;
				segons = 0;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						Resposta = result.ToString();
						OK = 1;
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_KioskGetTime : IPendingServiceCallback
		{
			public int OK;

			public int segons;

			public string Resposta;

			public int timeout;

			public Hook_Srv_KioskGetTime()
			{
				OK = -2;
				segons = 0;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						Resposta = result.ToString();
						segons = 0;
						try
						{
							segons = int.Parse(Resposta);
						}
						catch
						{
						}
						OK = 1;
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Verificar_Ticket : IPendingServiceCallback
		{
			public int OK;

			public int ticket;

			public string Resposta;

			public int timeout;

			public Hook_Srv_Verificar_Ticket()
			{
				OK = -2;
				ticket = 0;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						Resposta = result.ToString();
						OK = 1;
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Anular_Ticket : IPendingServiceCallback
		{
			public int OK;

			public int ticket;

			public int timeout;

			public bool estat;

			public Hook_Srv_Anular_Ticket()
			{
				OK = -2;
				ticket = 0;
				estat = false;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else if (Convert.ToInt32(result) == 0)
					{
						OK = 0;
					}
					else
					{
						OK = 1;
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Credits : IPendingServiceCallback
		{
			public int OK;

			public int credits;

			public int timeout;

			public Hook_Srv_Credits()
			{
				OK = -2;
				credits = 0;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						credits = 0;
						OK = 0;
					}
					else
					{
						int num = Convert.ToInt32(result);
						int num2 = num;
						if (num2 == -1)
						{
							credits = 0;
							OK = 0;
						}
						else if (num < 0)
						{
							credits = 0;
							OK = 0;
						}
						else
						{
							credits = num;
							OK = 1;
						}
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Sub_Credits : IPendingServiceCallback
		{
			public int OK;

			public int ticket;

			public int timeout;

			public Hook_Srv_Sub_Credits()
			{
				OK = -2;
				ticket = 0;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						int num = Convert.ToInt32(result);
						if (num == 0)
						{
							OK = 0;
						}
						else if (num == ticket)
						{
							OK = 1;
						}
						else
						{
							OK = 0;
						}
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Sub_Cadeaux : IPendingServiceCallback
		{
			public int OK;

			public int ticket;

			public int timeout;

			public Hook_Srv_Sub_Cadeaux()
			{
				OK = -2;
				ticket = 0;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						int num = Convert.ToInt32(result);
						if (num == 0)
						{
							OK = 0;
						}
						else if (num == ticket)
						{
							OK = 1;
						}
						else
						{
							OK = 0;
						}
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Add_Credits : IPendingServiceCallback
		{
			public int OK;

			public int ticket;

			public int timeout;

			public Hook_Srv_Add_Credits()
			{
				OK = -2;
				ticket = 0;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						int num = Convert.ToInt32(result);
						if (num == 0)
						{
							OK = 0;
						}
						else if (num == ticket)
						{
							OK = 1;
						}
						else
						{
							OK = 0;
						}
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Login : IPendingServiceCallback
		{
			public int login;

			public int OK;

			public int ticket;

			public int timeout;

			public string err;

			public Hook_Srv_Login()
			{
				OK = -2;
				ticket = 0;
				timeout = 0;
				login = 0;
				err = "";
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				bool flag = true;
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						int num = Convert.ToInt32(result);
						if (num <= 0)
						{
							OK = 0;
							login = 2;
							ticket = 0;
						}
						else
						{
							OK = 1;
							login = 1;
							ticket = num;
						}
					}
				}
				catch (Exception ex)
				{
					err = ex.Message;
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Sub_Ticket : IPendingServiceCallback
		{
			public int OK;

			public int timeout;

			public Hook_Srv_Sub_Ticket()
			{
				OK = -2;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else if (!Convert.ToBoolean(result))
					{
						OK = 0;
					}
					else
					{
						OK = 1;
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Add_Ticket : IPendingServiceCallback
		{
			public int OK;

			public int Ticket;

			public int timeout;

			public Hook_Srv_Add_Ticket()
			{
				Ticket = 0;
				OK = -2;
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						int num = Convert.ToInt32(result);
						if (num == 0)
						{
							OK = 0;
						}
						else
						{
							Ticket = num;
							OK = 1;
						}
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		public class Hook_Srv_Add_Pay : IPendingServiceCallback
		{
			public int OK;

			public int Pay;

			public int timeout;

			public string Resposta;

			public Hook_Srv_Add_Pay()
			{
				Pay = 0;
				OK = -2;
				Resposta = "";
				timeout = 0;
			}

			public void ResultReceived(IPendingServiceCall call)
			{
				object result = call.Result;
				try
				{
					if (result.ToString().ToLower() == "error")
					{
						OK = 0;
					}
					else
					{
						Resposta = result.ToString();
						OK = 1;
					}
				}
				catch
				{
				}
				timeout = 0;
			}
		}

		private class BitmapData
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

		private const int SM_TABLETPC = 86;

		private const int APPCOMMAND_VOLUME_MUTE = 524288;

		private const int APPCOMMAND_VOLUME_UP = 655360;

		private const int APPCOMMAND_VOLUME_DOWN = 589824;

		private const int WM_APPCOMMAND = 793;

		private const int MOUSEEVENTF_ABSOLUTE = 32768;

		private const int MOUSEEVENTF_LEFTDOWN = 2;

		private const int MOUSEEVENTF_LEFTUP = 4;

		private const int MOUSEEVENTF_MIDDLEDOWN = 32;

		private const int MOUSEEVENTF_MIDDLEUP = 64;

		private const int MOUSEEVENTF_MOVE = 1;

		private const int MOUSEEVENTF_RIGHTDOWN = 8;

		private const int MOUSEEVENTF_RIGHTUP = 16;

		private const int MOUSEEVENTF_WHEEL = 2048;

		private const int MOUSEEVENTF_XDOWN = 128;

		private const int MOUSEEVENTF_XUP = 256;

		private const int USE_ALT = 1;

		private const int USE_CTRL = 2;

		private const int USE_SHIFT = 4;

		private const int USE_WIN = 8;

		private const int WH_KEYBOARD_LL = 13;

		private const int WM_KEYUP = 257;

		private const int WM_SYSKEYUP = 261;

		private const int WM_KEYDOWN = 256;

		private const int WM_SYSKEYDOWN = 260;

		private const int WM_MOUSEMOVE = 512;

		private const int VK_SHIFT = 16;

		private const int VK_CONTROL = 17;

		private const int VK_PAUSE = 19;

		private const int VK_MENU = 18;

		private const int VK_CAPITAL = 20;

		private const int VK_SCROLL = 145;

		private const int VK_LCONTROL = 162;

		private const int VK_RCONTROL = 163;

		private const int VK_LWIN = 91;

		private const int VK_RWIN = 92;

		private const int VK_HOME = 36;

		private const int VK_ALT = 18;

		private const int VK_DELETE = 46;

		private const int VK_DELETEN = 109;

		private const int VK_INSERT = 45;

		private const int VK_F12 = 123;

		private const int VK_F1 = 112;

		private const int VK_F2 = 113;

		private const int VK_PRINT = 42;

		private const int WM_DEVICECHANGE = 537;

		private const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

		private const int WM_POWERBROADCAST = 536;

		private const int PBT_APMPOWERSTATUSCHANGE = 10;

		private const int PBT_APMRESUMEAUTOMATIC = 18;

		private const int PBT_APMRESUMESUSPEND = 7;

		private const int PBT_APMSUSPEND = 4;

		private const int PBT_POWERSETTINGCHANGE = 32787;

		public Impresora Imp;

		private int StartupDetect;

		private int doReset = 0;

		private int TimeoutPubli;

		private string Web_V2_A = "tel";

		private string Web_V2_B = "menu";

		public string Last_COM;

		public string Last_DRV;

		private Bitmap Img_Banner1;

		private Bitmap Img_Banner2;

		private bool tabletEnabled;

		private PipeServer _pipeServer;

		public Publicitat publi;

		private MMDevice device = null;

		private RawInput _rawinput;

		public string TicketToCheck;

		public int TicketOK;

		public string LastTicket;

		public DLG_Calibrar _DCalibrar;

		public string RedirectNews;

		public int Banner_On = 0;

		public int MenuGames = 0;

		private int DelayServer;

		private int ErrorEnLogin = 0;

		private int ErrorJamp = 0;

		private int controlcredits = 0;

		private int Emu_Mouse_Click = 0;

		private int Emu_Mouse_RClick = 0;

		public DLG_ValidarTicket ValidacioTicket = null;

		public DLG_TicketCredits ValidacioTicketCredits = null;

		public DLG_TimeOut ValidacioTimeOut = null;

		private bool oldStopCredits = false;

		private string ErrorGenericText;

		private RemoteSharedObject _sharedObject;

		private bool CoolStartUp = true;

		public bool ForceCheck;

		public MainWindow ControlWin;

		public Configuracion Opcions;

		public bool AdminEnabled = false;

		public ChromiumWebBrowser navegador;

		private Fases Status;

		private string _WebLink;

		private bool DisplayKeyboard;

		private int Windows_MidaY;

		public string errors;

		private DLG_Login LoginAdmin = null;

		private DLG_Config ConfigAdmin = null;

		public bool nofocus = false;

		private string errlink = "";

		public int TiempoAviso = 30;

		private SoundPlayer snd_alarma = null;

		private int cnttimerCredits = 10;

		private int Ticket_Add_Credits = 0;

		private int Ticket_Add_Cadeau = 0;

		public InsertCredits InsertCreditsDLG = null;

		private int anm_apla = 0;

		private int cnttime = 0;

		private DateTime ltime;

		private Control_CCTALK_COIN cct2 = null;

		private Control_Comestero rm5 = null;

		private Control_NV_SSP ssp = null;

		private Control_NV_SSP_P6 ssp3 = null;

		private Control_NV_SIO sio = null;

		private Control_F40_CCTalk f40 = null;

		private Control_Trilogy tri = null;

		private DLG_Registro login_dlg = null;

		private DRIVER_Ticket_ICT dispenser = null;

		private int _sem_timerPoll_Tick = 0;

		public int ErrorNet = 0;

		public int ErrorDevices = 0;

		public int LastErrorNet;

		private int DelayInt = 0;

		private int cntInt = 0;

		private int FiltreCnt = 0;

		private string tmp_ip = "";

		private string tmp_user = "";

		private string tmp_pw = "";

		private string tmp_w = "";

		private int needEnable = 0;

		private int needEnableM = 1;

		private bool enpoolint = false;

		private int waitneedEnable = 0;

		private int waitneedEnableM = 0;

		private int TimeoutCredits = 0;

		private int TimeoutHome = 0;

		private decimal old_credits_gratuits = -1m;

		public DLG_Sponsors _DLG_Sponsors;

		private short mHotKeyId = 0;

		private IntPtr hookID = IntPtr.Zero;

		private HookHandlerDelegate proc;

		private bool _khook = false;

		private int CtrlScroll = 0;

		private NetConnection _netConnection;

		public int errorcreditsserv = 0;

		private int ErrorNetServer = 0;

		public int cnterror = 0;

		public static RemoteSharedObject _sharedObjectPagos;

		private int delayreconnect = 0;

		private DLG_Message_Full dlg_playforticket = null;

		private DLG_Message_Full dlg_msg_printer = null;

		private DLG_Dispenser_Out dlg_checks = null;

		private Srv_Command _Srv_Commad;

		public int Error_Servidor = 0;

		public Hook_Srv_KioskCommand _Hook_Srv_KioskCommand = null;

		public Hook_Srv_KioskSetTime _Hook_Srv_KioskSetTime = null;

		public Hook_Srv_KioskGetTime _Hook_Srv_KioskGetTime = null;

		public Hook_Srv_Verificar_Ticket _Hook_Srv_Verificar_Ticket = null;

		public Hook_Srv_Anular_Ticket _Hook_Srv_Anular_Ticket = null;

		public Hook_Srv_Credits _Hook_Srv_Credits = null;

		public Hook_Srv_Sub_Credits _Hook_Srv_Sub_Credits = null;

		public Hook_Srv_Sub_Cadeaux _Hook_Srv_Sub_Cadeaux = null;

		public Hook_Srv_Add_Credits _Hook_Srv_Add_Credits = null;

		public Hook_Srv_Login _Hook_Srv_Login = null;

		public Hook_Srv_Sub_Ticket _Hook_Srv_Sub_Ticket = null;

		public Hook_Srv_Add_Ticket _Hook_Srv_Add_Ticket = null;

		public Hook_Srv_Add_Pay _Hook_Srv_Add_Pay = null;

		private int _Last_Id = -1;

		private int[] Cache_Tickets = new int[0];

		private byte _ESCPOS_FU = 128;

		private byte _ESCPOS_FE = 8;

		private byte _ESCPOS_FDW = 32;

		private byte _ESCPOS_FDH = 16;

		private byte _ESCPOS_AL = 0;

		private byte _ESCPOS_AC = 1;

		private byte _ESCPOS_AR = 2;

		private int _MMVol = 50;

		private bool alamaramute;

		private string dbglog = "";

		private string errtick = "";

		private static ManagementScope oManagementScope = null;

		private string web_versio;

		private string web_vnc;

		private string web_date;

		private string[] ban_users;

		private string[] ban_mac;

		private string[] ban_ip;

		private string[] ban_country;

		private IContainer components = null;

		private Panel pMenu;

		private TextBox tURL;

		private Label iSponsor;

		private Button bKeyboard;

		private Button bGo;

		private Button bForward;

		private Button bBack;

		private Button bHome;

		private System.Windows.Forms.Timer timerCredits;

		private System.Windows.Forms.Timer timerPoll;

		private System.Windows.Forms.Timer timerStartup;

		private Label lcdClock;

		private ImageList imgLed;

		private Label pScreenSaver;

		private Panel pTicket;

		private Button bTicket;

		private Panel pNavegation;

		private Panel pLogin;

		private Button bLogin;

		private Panel pTemps;

		private Panel pKeyboard;

		private Button bBar;

		private Panel pTime;

		private Button bTime;

		private Label eCredits;

		private Button bVUp;

		private Button bVDown;

		private Button bMute;

		private System.Windows.Forms.Timer timerPrinter;

		private Panel rtest;

		private PictureBox pInsertCoin;

		public Panel EntraJocs;

		private System.Windows.Forms.Timer timerMessages;

		private Label lCGRAT;

		private Label lCALL;

		private Panel pGETTON;

		private Button bGETTON;

		private Label lGETTON;

		public string WebLink
		{
			get
			{
				return _WebLink;
			}
			set
			{
				if (Cef.IsInitialized && navegador != null && navegador.IsBrowserInitialized)
				{
					string text = value;
					try
					{
						text = text.Replace('\\', '/');
						Uri uri = new Uri(text);
					}
					catch
					{
						bool flag = true;
						text = "http://".ToLower() + text;
					}
					try
					{
						Uri uri = new Uri(text);
						string scheme = uri.Scheme;
						switch (Status)
						{
						case Fases.GoHome:
						case Fases.Home:
							if (scheme.ToLower() != "http".ToLower() && scheme.ToLower() != "https".ToLower() && (Opcions.FreeGames != 0 || !(scheme.ToLower() == "about".ToLower())))
							{
								return;
							}
							break;
						default:
							if (scheme.ToLower() != "http".ToLower() && scheme.ToLower() != "https".ToLower())
							{
								return;
							}
							break;
						}
						_WebLink = text;
						navegador.Load(text);
						if (!nofocus)
						{
							navegador.Focus();
						}
						nofocus = false;
					}
					catch (Exception ex)
					{
						errlink = ex.Message;
					}
				}
			}
		}

		public MainWindow(ref Configuracion _opc)
		{
			tabletEnabled = (GetSystemMetrics(86) != 0);
			Opcions = _opc;
			Lock_Rotacio_Intel();
			Set_Dpi_100();
			Opcions.__ModoTablet = 0;
			bool flag = 0 == 0;
			Opcions.Enable_Lectors = -1;
			Opcions.Temps = 0;
			Opcions.CancelTemps = 0;
			Opcions.TempsDeTicket = Opcions.Temps;
			Opcions.Credits = 0m;
			Opcions.Add_Credits = 0m;
			Opcions.Sub_Credits = 0m;
			Opcions.Send_Add_Credits = 0m;
			Opcions.Send_Sub_Credits = 0m;
			Opcions.ForceGoConfig = false;
			Opcions.InGame = false;
			RedirectNews = "";
			cntInt = 0;
			StartupDetect = 0;
			ErrorJamp = 0;
			ErrorEnLogin = 0;
			TimeoutPubli = 0;
			controlcredits = 0;
			snd_alarma = null;
			ForceCheck = false;
			_sem_timerPoll_Tick = 0;
			AdminEnabled = false;
			Status = Fases.StartUp;
			DisplayKeyboard = false;
			ShowCursor(bShow: true);
			_DCalibrar = null;
			DelayServer = 100;
			Last_DRV = "-";
			Last_COM = "?";
			Check_Windows_Mode();
			int num = 0;
			int num2 = -1;
			for (int i = 0; i < Opcions.Web_Zone.Length; i++)
			{
				if (Opcions.Srv_Ip == Opcions.Web_Zone[i])
				{
					num = 1;
				}
				if (string.IsNullOrEmpty(Opcions.Web_Zone[i]) && num2 == -1)
				{
					num2 = i;
				}
			}
			if (num == 0 && num2 >= 0)
			{
				Opcions.Web_Zone[num2] = Opcions.Srv_Ip;
				Opcions.Save_Net();
			}
			num = 0;
			num2 = -1;
			for (int i = 0; i < Opcions.Web_Zone.Length; i++)
			{
				if (Opcions.Srv_Web_Ip == Opcions.Web_Zone[i])
				{
					num = 1;
				}
				if (string.IsNullOrEmpty(Opcions.Web_Zone[i]) && num2 == -1)
				{
					num2 = i;
				}
			}
			if (num == 0 && num2 >= 0)
			{
				Opcions.Web_Zone[num2] = Opcions.Srv_Web_Ip;
				Opcions.Save_Net();
			}
			Opcions.Web_Zone[0] = "gserver" + Opcions.Srv_port + "." + Opcions.Get_Domain(Opcions.Srv_Ip);
			try
			{
				string[] directories = Directory.GetDirectories(Path.GetTempPath(), "scoped_dir*");
				for (int i = 0; i < directories.Length; i++)
				{
					try
					{
						Directory.Delete(directories[i], recursive: true);
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			string text = "c:\\kiosk\\cache";
			if (Directory.Exists(text))
			{
				try
				{
					Directory.Delete(text, recursive: true);
				}
				catch
				{
				}
			}
			ErrorNetServer = 0;
			try
			{
				_netConnection = new NetConnection();
				_netConnection.OnConnect += _netConnection_OnConnect;
				_netConnection.OnDisconnect += _netConnection_OnDisconnect;
				_netConnection.NetStatus += _netConnection_NetStatus;
			}
			catch
			{
			}
			InitializeComponent();
			SuspendLayout();
			Keyboard_Hook();
			flag = false;
			if (!Environment.MachineName.ToLower().Contains("cinto"))
			{
				TaskManager_Off();
			}
			else
			{
				MessageBox.Show("STOP MODE DEBUG OFF");
			}
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			Install_Keyboard_Driver();
			CefSettings cefSettings = new CefSettings();
			cefSettings.LogSeverity = LogSeverity.Disable;
			cefSettings.CefCommandLineArgs.Add("enable-npapi", "1");
			cefSettings.CefCommandLineArgs["enable-system-flash"] = "1";
			cefSettings.CachePath = text;
			string text2 = "c:\\kiosk\\pepflashplayer.dll";
			if (File.Exists(text2))
			{
				cefSettings.CefCommandLineArgs.Add("ppapi-flash-path", text2);
			}
			else
			{
				text2 = Application.StartupPath + "\\pepflashplayer.dll";
				if (File.Exists(text2))
				{
					cefSettings.CefCommandLineArgs.Add("ppapi-flash-path", text2);
				}
			}
			do
			{
				Cef.Initialize(cefSettings);
			}
			while (!Cef.IsInitialized);
			BrowserSettings browserSettings = new BrowserSettings
			{
				Javascript = CefState.Enabled,
				Plugins = CefState.Enabled,
				JavascriptAccessClipboard = CefState.Disabled
			};
			Img_Banner1 = new Bitmap("data\\baner_fr.png");
			Img_Banner2 = new Bitmap("data\\baner2_fr.png");
			EntraJocs.BackgroundImage = Img_Banner1;
			navegador = new ChromiumWebBrowser("about:void");
			navegador.Dock = DockStyle.Fill;
			navegador.Location = new Point(0, 50);
			navegador.Name = "navegador";
			navegador.BackColor = Color.DeepSkyBlue;
			navegador.MenuHandler = new MenuHandler();
			navegador.TabIndex = 14;
			navegador.JsDialogHandler = new JsDialogHandler();
			navegador.Move += Move_Screesaver;
			NavIRequestHandler navIRequestHandler = new NavIRequestHandler(navegador, this);
			LifeSpanHandler lifeSpanHandler = new LifeSpanHandler();
			base.Controls.Clear();
			base.Controls.Add(lCALL);
			base.Controls.Add(pScreenSaver);
			base.Controls.Add(navegador);
			base.Controls.Add(EntraJocs);
			base.Controls.Add(pMenu);
			ResumeLayout(performLayout: false);
			Opcions.Enable_Lectors = -1;
			base.Location = Screen.PrimaryScreen.Bounds.Location;
			flag = true;
			base.Bounds = Screen.PrimaryScreen.Bounds;
			ControlWin = this;
			Windows_MidaY = base.Height;
			Modo_Kiosk_On();
			navegador.Focus();
			Configurar_Splash(0);
			cnttimerCredits = 11;
			Error_Servidor = 0;
			ErrorNet = 0;
			cntInt = 0;
			ErrorDevices = 0;
			CoolStartUp = true;
			Opcions.Credits = 0m;
			Opcions.TimeNavigate = false;
			cnttime = 0;
			Hide_Browser_Nav();
			Start_Temps();
			Status = Fases.Reset;
			doReset = 0;
			if (Opcions.News != 1)
			{
				flag = true;
				Start_Service("http://" + Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx");
			}
			else
			{
				flag = true;
				Start_Service("http://" + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page);
			}
			Render_Bar_Menu();
			device = null;
			if (Opcions.modo_XP == 0)
			{
				try
				{
					MMDeviceEnumerator mMDeviceEnumerator = new MMDeviceEnumerator();
					device = mMDeviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
				}
				catch (Exception)
				{
				}
			}
			if (Opcions.News != 1)
			{
				Find_Selector();
				Find_Printer();
			}
			else
			{
				Opcions.ModoTickets = 0;
				pMenu.Visible = false;
			}
			if (Opcions.Publi.ToLower().Contains("http:") || Opcions.Publi.ToLower().Contains("https:"))
			{
				Opcions.Publi = "";
			}
			Opcions.SEND_Mail("STARTUP", "POWER ON");
			flag = true;
			if (Opcions.__ModoTablet == 1)
			{
				Opcions.VersionPRG += "TAB";
				_pipeServer = new PipeServer();
				_pipeServer.PipeMessage += PipesMessageHandler;
			}
			flag = true;
			load_web("http://" + Opcions.Srv_Web_Ip + "/__check.html", 0);
			Opcions.SpyUser(Opcions.RemoteParam);
			Imp = new Impresora();
			Imp.Add_Impressora("*CUSTOM", 3, 3, _c: true);
			Imp.Add_Impressora("*NII", 3, 3, _c: true);
			Imp.Add_Impressora("*SANEI", 3, 3, _c: true);
			Imp.Add_Impressora("*STAR", 3, 3, _c: true);
		}

		private void Reconnect_Service()
		{
			if (_netConnection == null)
			{
				try
				{
					_netConnection = new NetConnection();
					_netConnection.OnConnect += _netConnection_OnConnect;
					_netConnection.OnDisconnect += _netConnection_OnDisconnect;
					_netConnection.NetStatus += _netConnection_NetStatus;
				}
				catch
				{
				}
			}
		}

		private void Install_Keyboard_Driver()
		{
			_rawinput = null;
			try
			{
				_rawinput = new RawInput(base.Handle);
			}
			catch (Exception ex)
			{
				MessageBox.Show("OBJ (Main): " + ex.Message);
			}
			if (_rawinput != null)
			{
				_rawinput.CaptureOnlyIfTopMostWindow = false;
				_rawinput.KeyPressed += OnKeyPressed;
				_rawinput.MouseMove += OnMouseMove;
			}
		}

		private void Block_Accessibility()
		{
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Accessibility\\StickyKeys", writable: true);
				registryKey.SetValue("Flags", "506", RegistryValueKind.String);
			}
			catch (Exception)
			{
				bool flag = true;
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Accessibility\\Keyboard Response", writable: true);
				registryKey.SetValue("Flags", "122", RegistryValueKind.String);
			}
			catch (Exception)
			{
				bool flag = true;
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Accessibility\\ToggleKeys", writable: true);
				registryKey.SetValue("Flags", "58", RegistryValueKind.String);
			}
			catch (Exception)
			{
				bool flag = true;
			}
			try
			{
				RegistryKey registryKey = Registry.Users.OpenSubKey(".DEFAULT\\Control Panel\\Accessibility\\StickyKeys", writable: true);
				registryKey.SetValue("Flags", "506", RegistryValueKind.String);
			}
			catch (Exception)
			{
				bool flag = true;
			}
			try
			{
				RegistryKey registryKey = Registry.Users.OpenSubKey(".DEFAULT\\Control Panel\\Accessibility\\Keyboard Response", writable: true);
				registryKey.SetValue("Flags", "122", RegistryValueKind.String);
			}
			catch (Exception)
			{
				bool flag = true;
			}
			try
			{
				RegistryKey registryKey = Registry.Users.OpenSubKey(".DEFAULT\\Control Panel\\Accessibility\\ToggleKeys", writable: true);
				registryKey.SetValue("Flags", "58", RegistryValueKind.String);
			}
			catch (Exception)
			{
				bool flag = true;
			}
		}

		private void Block_AltF4()
		{
			if (Opcions.__ModoTablet != 1)
			{
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.System);
				string text = "c:\\Kiosk";
				string path = folderPath2 + "\\kbfmgr.exe";
				if (File.Exists(path))
				{
					try
					{
						using (Process process = Process.Start("kbfmgr.exe", "/enable"))
						{
							Thread.Sleep(500);
							process.WaitForExit();
						}
					}
					catch
					{
					}
				}
			}
		}

		private void ReInstall_Keyboard_Driver()
		{
			if (_rawinput != null)
			{
				_rawinput.ReleaseHandle();
				_rawinput = null;
			}
			Install_Keyboard_Driver();
		}

		[DllImport("user32.dll")]
		private static extern int GetSystemMetrics(int nIndex);

		private void PipesMessageHandler(string message)
		{
			if (message.ToLower().Contains("admin"))
			{
				try
				{
					if (Opcions.Running)
					{
						Status = Fases.Reset;
						doReset = 0;
					}
					Opcions.RunConfig = true;
					AdminEnabled = true;
				}
				catch
				{
				}
			}
		}

		private void Reset_CEF()
		{
			SuspendLayout();
			if (Cef.IsInitialized)
			{
				Cef.Shutdown();
			}
			try
			{
				string[] directories = Directory.GetDirectories(Path.GetTempPath(), "scoped_dir*");
				for (int i = 0; i < directories.Length; i++)
				{
					try
					{
						Directory.Delete(directories[i], recursive: true);
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			CefSettings cefSettings = new CefSettings();
			cefSettings.LogSeverity = LogSeverity.Disable;
			do
			{
				Cef.Initialize(cefSettings);
			}
			while (!Cef.IsInitialized);
			ResumeLayout(performLayout: false);
		}

		public void Check_Windows_Mode()
		{
			bool flag = true;
			bool flag2 = false;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", writable: true);
				string text = registryKey.GetValue("Shell").ToString();
				registryKey.Close();
				if (!text.ToLower().Contains("loader.exe".ToLower()))
				{
					flag2 = true;
				}
			}
			catch
			{
			}
			int num = 0;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				num = int.Parse(registryKey.GetValue("DisableTaskMgr").ToString());
				registryKey.Close();
			}
			catch
			{
			}
			if (flag2)
			{
				Lock_Windows();
			}
			else if (num == 1)
			{
				Lock_Windows();
			}
			else if (MessageBox.Show("Enable Kiosk Mode?", "WARNNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				Lock_Windows();
				Process.Start("shutdown.exe", "/r /t 2");
				while (true)
				{
					Application.DoEvents();
					flag = true;
				}
			}
		}

		public void Lock_Rotacio_Intel()
		{
			bool flag = true;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Intel\\Display\\igfxcui\\HotKeys", writable: true);
				registryKey.SetValue("Enable", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
		}

		public void Set_Dpi_100()
		{
			bool flag = true;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", writable: true);
				registryKey.SetValue("LogPixels", 96, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Control Panel\\Accessibility\\HighContrast", writable: true);
				registryKey.SetValue("Flags", 122, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.Users.OpenSubKey(".DEFAULT\\Control Panel\\Accessibility\\HighContrast", writable: true);
				registryKey.SetValue("Flags", 122, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
		}

		public void UnLock_Windows()
		{
			bool flag = true;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout", writable: true);
				registryKey.DeleteValue("Scancode Map");
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoWinKeys", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoWinKeys", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoViewContextMenu", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoViewContextMenu", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("DisableTaskMgr", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("NoDesktop", 0, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				using (Process process = Process.Start("kbfmgr.exe", "/disable"))
				{
					process.WaitForExit();
				}
			}
			catch
			{
			}
		}

		public void Lock_Windows()
		{
			bool flag = true;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Keyboard Layout", writable: true);
				byte[] value = new byte[24]
				{
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					0,
					3,
					0,
					0,
					0,
					0,
					0,
					91,
					224,
					0,
					0,
					92,
					224,
					0,
					0,
					0,
					0
				};
				registryKey.SetValue("Scancode Map", value, RegistryValueKind.Binary);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoWinKeys", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoWinKeys", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoViewContextMenu", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer", writable: true);
				registryKey.SetValue("NoViewContextMenu", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true);
				registryKey.SetValue("NoDesktop", 1, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", writable: true);
				registryKey.SetValue("Shell", "c:\\kiosk\\loader.exe", RegistryValueKind.String);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				using (Process process = Process.Start("kbfmgr.exe", "/enable"))
				{
					process.WaitForExit();
				}
			}
			catch
			{
			}
		}

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

		public static void TaskManager_Off()
		{
		}

		[DllImport("user32.dll")]
		private static extern int ShowCursor(bool bShow);

		public int Parser_Ticket(string _user, string _add, int _remove)
		{
			TicketToCheck += _add;
			while (!string.IsNullOrEmpty(TicketToCheck) && TicketToCheck[0] != '4' && TicketToCheck[0] != '7')
			{
				TicketToCheck = TicketToCheck.Remove(0, 1);
			}
			if (TicketToCheck.Length >= 14)
			{
				int num = Gestion.Check_Mod10(_user, TicketToCheck);
				if (num > 0)
				{
					LastTicket = TicketToCheck.Substring(0, 14);
					if (_remove == 1)
					{
						TicketToCheck = TicketToCheck.Remove(0, 14);
						TicketToCheck = "";
					}
					TicketOK = 1;
					return num;
				}
				LastTicket = "";
				TicketToCheck = TicketToCheck.Remove(0, 1);
				TicketOK = 0;
				return Parser_Ticket(_user, "", 1);
			}
			return 0;
		}

		private void OnMouseMove(object sender, InputEventArg e)
		{
			Opcions.LastMouseMove = DateTime.Now;
			TimeoutHome = 0;
		}

		public void Validate_Ticket()
		{
			TicketOK = 0;
			int num = Parser_Ticket(Opcions.Srv_User, "", 0);
			if (Opcions.ModoTickets == 1 && TicketOK == 1 && !Opcions.InGame && Opcions.Running && num == 2)
			{
				TicketToCheck = TicketToCheck.Remove(0, 14);
				int _ticket = 0;
				int _id = 0;
				Gestion.Decode_Mod10(LastTicket, out _ticket, out _id);
				Opcions.TicketTemps = _ticket;
				Opcions.IdTicketTemps = _id;
				if (Opcions.IdTicketTemps > 0)
				{
					Srv_Sub_Ticket(Opcions.IdTicketTemps, 0);
				}
			}
		}

		private void OnKeyPressed(object sender, InputEventArg e)
		{
			Opcions.LastMouseMove = DateTime.Now;
			TimeoutHome = 0;
			bool flag = false;
			switch (e.KeyPressEvent.Message)
			{
			case 256u:
				if ((e.KeyPressEvent.VKey == 9 && e.KeyPressEvent.Flag == 32) || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 32) || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 0) || (e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 2) || e.KeyPressEvent.Flag == 32)
				{
					flag = true;
				}
				break;
			case 257u:
				if ((e.KeyPressEvent.VKey == 9 && e.KeyPressEvent.Flag == 32) || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 32) || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 0) || (e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 2) || e.KeyPressEvent.Flag == 32)
				{
					flag = true;
				}
				break;
			case 260u:
				if ((e.KeyPressEvent.VKey == 9 && e.KeyPressEvent.Flag == 32) || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 32) || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 0) || (e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 2) || e.KeyPressEvent.Flag == 32)
				{
					flag = true;
				}
				break;
			case 261u:
				if ((e.KeyPressEvent.VKey == 9 && e.KeyPressEvent.Flag == 32) || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 32) || (e.KeyPressEvent.VKey == 27 && e.KeyPressEvent.Flag == 0) || (e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 91 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 92 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 3) || (e.KeyPressEvent.VKey == 37 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 38 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 39 && e.KeyPressEvent.Flag == 2) || (e.KeyPressEvent.VKey == 40 && e.KeyPressEvent.Flag == 2) || e.KeyPressEvent.Flag == 32)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				e.KeyPressEvent.Remove = true;
				return;
			}
			string[] array = e.KeyPressEvent.DeviceName.Split('#');
			if (array.Length >= 2)
			{
				string[] array2 = array[1].Split('&');
				if (!string.IsNullOrEmpty(array2[0]))
				{
					Opcions.Last_Device = array2[0];
				}
			}
			Opcions.LastMouseMove = DateTime.Now;
			TimeoutHome = 0;
			if (Opcions.Test_Barcode == 1 && e.KeyPressEvent.Message == 256 && (e.KeyPressEvent.Flag & 1) == 0)
			{
				KeysConverter keysConverter = new KeysConverter();
				switch (e.KeyPressEvent.VKeyName)
				{
				case "D1":
				case "D2":
				case "D3":
				case "D4":
				case "D5":
				case "D6":
				case "D7":
				case "D8":
				case "D9":
				case "D0":
					Parser_Ticket(Opcions.Srv_User, keysConverter.ConvertToString(e.KeyPressEvent.VKey), 0);
					e.KeyPressEvent.Remove = true;
					return;
				case "ENTER":
					Parser_Ticket(Opcions.Srv_User, "", 1);
					e.KeyPressEvent.Remove = false;
					return;
				}
			}
			if (Opcions.ModoPS2 == 0 && Opcions.ForceAllKey == 0 && e.KeyPressEvent.DeviceName.Contains(Opcions.Barcode) && !string.IsNullOrEmpty(Opcions.Barcode))
			{
				if (e.KeyPressEvent.Message == 256 && (e.KeyPressEvent.Flag & 1) == 0)
				{
					KeysConverter keysConverter = new KeysConverter();
					switch (e.KeyPressEvent.VKeyName)
					{
					case "D1":
					case "D2":
					case "D3":
					case "D4":
					case "D5":
					case "D6":
					case "D7":
					case "D8":
					case "D9":
					case "D0":
						Parser_Ticket(Opcions.Srv_User, keysConverter.ConvertToString(e.KeyPressEvent.VKey), 0);
						break;
					case "ENTER":
					{
						TicketOK = 0;
						int num = Parser_Ticket(Opcions.Srv_User, "", 0);
						if (Opcions.ModoTickets != 1 || TicketOK != 1 || Opcions.InGame || !Opcions.Running)
						{
							break;
						}
						TicketToCheck = TicketToCheck.Remove(0, 14);
						if (num == 1)
						{
							int _ticket = 0;
							int _id = 0;
							Gestion.Decode_Mod10(LastTicket, out _ticket, out _id);
							Opcions.TicketTemps = _ticket;
							Opcions.IdTicketTemps = _id;
							if (Opcions.IdTicketTemps > 0)
							{
								Srv_Sub_Ticket(Opcions.IdTicketTemps, 0);
							}
						}
						if (num == 2)
						{
							bool flag2 = false;
							if (ValidacioTicket == null)
							{
								ValidacioTicket = new DLG_ValidarTicket(ref Opcions);
								ValidacioTicket.MWin = this;
							}
							if (ValidacioTicket.IsDisposed)
							{
								ValidacioTicket = new DLG_ValidarTicket(ref Opcions);
								ValidacioTicket.MWin = this;
							}
							ValidacioTicket.Ticket = LastTicket;
							Opcions.Verificar_Ticket = 0;
							try
							{
								Opcions.Verificar_Ticket = int.Parse(LastTicket.Substring(1, 11));
							}
							catch
							{
							}
							Srv_Verificar_Ticket(Opcions.Verificar_Ticket, 0);
							ValidacioTicket.Show();
						}
						break;
					}
					}
				}
				e.KeyPressEvent.Remove = true;
			}
			doReset = 0;
			switch (e.KeyPressEvent.Message)
			{
			case 256u:
			case 260u:
				switch (e.KeyPressEvent.VKeyName)
				{
				case "SCROLL":
				{
					bool flag2 = true;
					break;
				}
				case "PAUSE":
					CtrlScroll |= 1;
					if (Opcions.Running && CtrlScroll == 3)
					{
						Status = Fases.Reset;
					}
					break;
				case "F8":
					CtrlScroll |= 8;
					if (Opcions.Running && (CtrlScroll == 3 || CtrlScroll == 24 || CtrlScroll == 6))
					{
						Status = Fases.Reset;
					}
					break;
				case "F5":
					CtrlScroll |= 8;
					if (!Opcions.Running && Status != Fases.WaitCalibrar && Status != Fases.Calibrar)
					{
						Status = Fases.Calibrar;
					}
					break;
				case "F9":
					CtrlScroll |= 16;
					if (Opcions.Running && (CtrlScroll == 3 || CtrlScroll == 24 || CtrlScroll == 6))
					{
						Status = Fases.Reset;
					}
					break;
				case "F16":
					Opcions.RunConfig = true;
					AdminEnabled = true;
					Status = Fases.Reset;
					break;
				case "F3":
					CtrlScroll |= 4;
					if (Opcions.Running && (CtrlScroll == 3 || CtrlScroll == 24 || CtrlScroll == 6))
					{
						Status = Fases.Reset;
					}
					break;
				case "F1":
					CtrlScroll |= 2;
					if (Opcions.Running && (CtrlScroll == 3 || CtrlScroll == 24 || CtrlScroll == 6))
					{
						Status = Fases.Reset;
					}
					break;
				case "LWIN":
				case "RWIN":
					e.KeyPressEvent.Remove = true;
					break;
				case "LCONTROL":
				case "RCONTROL":
				case "F10":
					if (AdminEnabled)
					{
						Opcions.RunConfig = true;
						BackColor = Color.Blue;
					}
					break;
				}
				break;
			case 257u:
			case 261u:
				switch (e.KeyPressEvent.VKeyName)
				{
				case "PAUSE":
					CtrlScroll &= 1;
					break;
				case "F1":
					CtrlScroll &= 2;
					break;
				case "F3":
					CtrlScroll &= 4;
					break;
				case "F8":
					CtrlScroll &= 8;
					break;
				case "F9":
					CtrlScroll &= 16;
					break;
				case "LWIN":
				case "RWIN":
					e.KeyPressEvent.Remove = true;
					break;
				case "LCONTROL":
				case "RCONTROL":
					if (AdminEnabled && !Opcions.RunConfig)
					{
						Opcions.RunConfig = true;
						BackColor = Color.Blue;
					}
					break;
				}
				break;
			}
			if (!Opcions.Emu_Mouse)
			{
				return;
			}
			POINT lpPoint;
			switch (e.KeyPressEvent.Message)
			{
			case 258u:
			case 259u:
				break;
			case 256u:
			case 260u:
			{
				string vKeyName = e.KeyPressEvent.VKeyName;
				if (vKeyName == null)
				{
					break;
				}
				if (!(vKeyName == "F12"))
				{
					if (!(vKeyName == "F11"))
					{
						if (!(vKeyName == "LEFT"))
						{
							if (!(vKeyName == "RIGHT"))
							{
								int y;
								if (!(vKeyName == "UP"))
								{
									if (vKeyName == "DOWN")
									{
										GetCursorPos(out lpPoint);
										y = lpPoint.Y;
										lpPoint.Y += 8;
										if (y != lpPoint.Y)
										{
											SetCursorPos(lpPoint.X, lpPoint.Y);
										}
									}
									break;
								}
								GetCursorPos(out lpPoint);
								y = lpPoint.Y;
								lpPoint.Y -= 8;
								if (lpPoint.Y < 0)
								{
									lpPoint.Y = 0;
								}
								if (y != lpPoint.Y)
								{
									SetCursorPos(lpPoint.X, lpPoint.Y);
								}
							}
							else
							{
								GetCursorPos(out lpPoint);
								int y = lpPoint.X;
								lpPoint.X += 8;
								if (y != lpPoint.X)
								{
									SetCursorPos(lpPoint.X, lpPoint.Y);
								}
							}
						}
						else
						{
							GetCursorPos(out lpPoint);
							int y = lpPoint.X;
							lpPoint.X -= 8;
							if (lpPoint.X < 0)
							{
								lpPoint.X = 0;
							}
							if (y != lpPoint.X)
							{
								SetCursorPos(lpPoint.X, lpPoint.Y);
							}
						}
					}
					else if (Emu_Mouse_Click == 0 && Opcions.Emu_Mouse_RClick)
					{
						GetCursorPos(out lpPoint);
						mouse_event(32776, lpPoint.X, lpPoint.Y, 0, 0);
						Emu_Mouse_Click = 1;
					}
				}
				else if (Emu_Mouse_Click == 0)
				{
					GetCursorPos(out lpPoint);
					mouse_event(32770, lpPoint.X, lpPoint.Y, 0, 0);
					Emu_Mouse_Click = 1;
				}
				break;
			}
			case 257u:
			case 261u:
			{
				string vKeyName = e.KeyPressEvent.VKeyName;
				if (vKeyName == null)
				{
					break;
				}
				if (!(vKeyName == "F12"))
				{
					if (vKeyName == "F11" && Emu_Mouse_Click == 1 && Emu_Mouse_RClick == 1)
					{
						GetCursorPos(out lpPoint);
						mouse_event(32784, lpPoint.X, lpPoint.Y, 0, 0);
						Emu_Mouse_Click = 0;
					}
				}
				else if (Emu_Mouse_Click == 1)
				{
					GetCursorPos(out lpPoint);
					mouse_event(32774, lpPoint.X, lpPoint.Y, 0, 0);
					Emu_Mouse_Click = 0;
				}
				break;
			}
			}
		}

		public bool Es_Pot_Navegar()
		{
			if (Opcions.Temps <= 0)
			{
				return false;
			}
			if (Opcions.ForceLogin == 1 && !Opcions.Logged)
			{
				return false;
			}
			return true;
		}

		public void Render_Bar_Menu()
		{
			if (Opcions.FreeGames == 0)
			{
				bTime.Height = 30;
				eCredits.Visible = true;
			}
			else
			{
				bTime.Height = 43;
				eCredits.Visible = false;
			}
			switch (Opcions.ModoKiosk)
			{
			case 0:
				pLogin.Visible = false;
				pTicket.Visible = false;
				pNavegation.Visible = true;
				pGETTON.Visible = false;
				pTemps.Visible = true;
				oldStopCredits = Opcions.StopCredits;
				break;
			case 1:
				if (Opcions.ForceLogin == 1)
				{
					pLogin.Visible = true;
				}
				else
				{
					pLogin.Visible = false;
				}
				pTime.Visible = false;
				pTicket.Visible = false;
				pTemps.Left = 0;
				if (Opcions.ModoTickets == 1)
				{
					pTicket.Left = pTemps.Width;
					pNavegation.Left = pTemps.Width + pTicket.Width;
					pNavegation.Width = base.Width - pNavegation.Left - pKeyboard.Width;
					pGETTON.Left = pTemps.Width + pTicket.Width;
					pGETTON.Width = base.Width - pGETTON.Left - pKeyboard.Width;
					pTicket.Visible = true;
				}
				else if (Opcions.News == 2)
				{
					pNavegation.Left = pTemps.Width;
					pNavegation.Width = base.Width - pNavegation.Left - pKeyboard.Width;
					if (Opcions.ModoTickets == 0)
					{
						pTemps.Visible = false;
						pNavegation.Left = 0;
						pNavegation.Width = base.Width - pNavegation.Left - pKeyboard.Width;
					}
					pGETTON.Left = pTemps.Width;
					pGETTON.Width = base.Width - pGETTON.Left - pKeyboard.Width;
					if (Opcions.ModoTickets == 0)
					{
						pTemps.Visible = false;
						pGETTON.Left = 0;
						pGETTON.Width = base.Width - pGETTON.Left - pKeyboard.Width;
					}
				}
				else
				{
					pTime.Left = pTemps.Width;
					pNavegation.Left = pTemps.Width + pTime.Width;
					pNavegation.Width = base.Width - pNavegation.Left - pKeyboard.Width;
					pGETTON.Left = pTemps.Width + pTime.Width;
					pGETTON.Width = base.Width - pGETTON.Left - pKeyboard.Width;
					pTime.Visible = true;
				}
				pNavegation.Visible = true;
				pGETTON.Visible = false;
				if (Opcions.News == 2 && Opcions.ModoTickets == 0)
				{
					pTemps.Visible = false;
				}
				else
				{
					pTemps.Visible = true;
				}
				if (Opcions.StopCredits)
				{
					pInsertCoin.Image = Resources.insertcoin2_off;
				}
				else
				{
					pInsertCoin.Image = null;
				}
				oldStopCredits = Opcions.StopCredits;
				break;
			case 2:
				pLogin.Visible = true;
				pTicket.Visible = false;
				pNavegation.Visible = true;
				pGETTON.Visible = false;
				pTemps.Visible = true;
				oldStopCredits = Opcions.StopCredits;
				break;
			}
		}

		public bool Start_Service(string _web)
		{
			string fileName = Path.GetTempPath() + "tmp121212.tmp";
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				WebClient webClient = new WebClient();
				webClient.DownloadFile(_web, fileName);
			}
			catch (Exception ex)
			{
				ErrorGenericText = ex.Message;
				return false;
			}
			return true;
		}

		private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			MessageBox.Show("ERROR (CurrentDomain_UnhandledException) ... " + e);
			if (null != ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void Activar_Modo_Config()
		{
			Configurar_Splash(0);
			Status = Fases.Reset;
			doReset = 0;
		}

		public void GoGames()
		{
			CloseOSK();
			TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
			nofocus = true;
			if (Opcions.ModoKiosk == 0)
			{
				if (Opcions.News != 1)
				{
					bool flag = true;
					WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/MenuGame.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",MenuGame.aspx," + timeSpan.TotalMilliseconds;
				}
				else
				{
					bool flag = true;
					WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",Default.aspx," + timeSpan.TotalMilliseconds;
				}
			}
			else if (Opcions.ModoTickets == 1)
			{
				if (Opcions.News != 1)
				{
					if (Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_A) || Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_B))
					{
						bool flag = true;
						WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/StartGameQuiosk.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",tickquioscV2.aspx," + timeSpan.TotalMilliseconds;
					}
					else
					{
						bool flag = true;
						WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/StartGameQuiosk.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",tickquiosc.aspx," + timeSpan.TotalMilliseconds;
					}
				}
				else
				{
					bool flag = true;
					WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",Default.aspx," + timeSpan.TotalMilliseconds;
				}
			}
			else if (Opcions.News != 1)
			{
				bool flag = true;
				WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/StartGameQuiosk.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",DemoQuiosk.aspx," + timeSpan.TotalMilliseconds;
			}
			else
			{
				bool flag = true;
				WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",Default.aspx," + timeSpan.TotalMilliseconds;
			}
			nofocus = false;
		}

		private void Close_Devices()
		{
			Opcions.Enable_Lectors = -1;
			if (ssp3 != null)
			{
				ssp3.Close();
				ssp3 = null;
			}
			if (ssp != null)
			{
				ssp.Close();
				ssp = null;
			}
			if (sio != null)
			{
				sio.Close();
				sio = null;
			}
			if (rm5 != null)
			{
				rm5.Close();
				rm5 = null;
			}
			if (cct2 != null)
			{
				cct2.Close();
				cct2 = null;
			}
			if (tri != null)
			{
				tri.Close();
				tri = null;
			}
			if (f40 != null)
			{
				f40.Close();
				f40 = null;
			}
		}

		private void timerStartup_Tick(object sender, EventArgs e)
		{
			timerStartup.Enabled = false;
		}

		private int Control_Zone()
		{
			string webLink = _WebLink;
			if (string.IsNullOrEmpty(webLink))
			{
				return -1;
			}
			webLink = webLink.ToLower();
			string[] array = webLink.Split('/');
			if (array[0].ToLower() == "file:".ToLower() || array[0].ToLower() == "ftps:".ToLower() || array[0].ToLower() == "ftp:".ToLower() || array[0].ToLower() == "about:blank".ToLower())
			{
				return -1;
			}
			if (array[0].ToLower() == "http:".ToLower() || array[0].ToLower() == "https:".ToLower())
			{
				return 0;
			}
			return -1;
		}

		public bool Detectar_Zona_Temps()
		{
			if (Control_Zone() < 0)
			{
				return false;
			}
			if (Opcions.ForceLogin == 1 && !Opcions.Logged)
			{
				return false;
			}
			_ = Opcions.BrowserBarOn;
			bool flag = 0 == 0;
			string webLink = _WebLink;
			string[] array = webLink.Split(':');
			string[] array2 = webLink.Split('/');
			for (int i = 0; i < Opcions.Web_Zone.Length; i++)
			{
				if (!string.IsNullOrEmpty(Opcions.Web_Zone[i]) && array2[2].ToLower() == Opcions.Web_Zone[i].ToLower())
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsNetworkAvailable()
		{
			return IsNetworkAvailable(0L);
		}

		[DllImport("wininet.dll")]
		private static extern bool InternetGetConnectedState(int Description, int ReservedValue);

		public static bool IsNetworkAvailable(long minimumSpeed)
		{
			int description = 0;
			return InternetGetConnectedState(description, 0);
		}

		private void SoundCredits()
		{
			using (SoundPlayer soundPlayer = new SoundPlayer(Environment.CurrentDirectory + "\\data\\points.wav"))
			{
				soundPlayer.Play();
			}
		}

		private void SoundTest()
		{
			using (SoundPlayer soundPlayer = new SoundPlayer(Environment.CurrentDirectory + "\\data\\test.wav"))
			{
				soundPlayer.Play();
			}
		}

		private void SoundAlarm()
		{
			bool flag = true;
			if (snd_alarma == null)
			{
				SoundRestore();
				snd_alarma = new SoundPlayer(Environment.CurrentDirectory + "\\data\\alarma.wav");
			}
			SoundMax();
			snd_alarma.Stop();
			snd_alarma.PlayLooping();
		}

		private void Show_Browser_Nav()
		{
			tURL.Visible = true;
			bBack.Visible = true;
			bForward.Visible = true;
			bGo.Visible = true;
		}

		private void Hide_Browser_Nav()
		{
			tURL.Visible = false;
			bBack.Visible = false;
			bForward.Visible = false;
			bGo.Visible = false;
		}

		private void Control_Dispenser()
		{
			if (Opcions.Disp_Pay_Running == 4 && dlg_checks.IsClosed)
			{
				Opcions.Disp_Pay_Running = 0;
				dlg_checks.Dispose();
				dlg_checks = null;
				if (dispenser != null)
				{
					dispenser.Close();
					dispenser = null;
				}
			}
			if (Opcions.Disp_Pay_Running == 3 && _Srv_Commad == Srv_Command.Null)
			{
				Opcions.Disp_Pay_Ticket_Fail = 3;
				Opcions.Disp_Pay_Running = 4;
				Gestio_Pagament(Opcions.Disp_Pay_Ticket_Credits, 1);
			}
			if (Opcions.Disp_Pay_Running == 30 && _Srv_Commad == Srv_Command.Null)
			{
				Opcions.Disp_Pay_Running = 3;
			}
			if (Opcions.Disp_Pay_Running == 20 && _Srv_Commad == Srv_Command.Null)
			{
				Opcions.Disp_Pay_Ticket_Out_Flag = 1;
				dispenser.Tickets_Out = 0;
				dispenser.Payout(1);
				dispenser.Poll();
				Thread.Sleep(100);
				Opcions.Disp_Pay_Running = 2;
			}
			if (Opcions.Disp_Pay_Running == 2)
			{
				dispenser.Poll();
				if (dispenser.Tickets_Out >= 1)
				{
					Opcions.Disp_Pay_Ticket_Credits -= Opcions.Disp_Val;
					if (Opcions.Disp_Pay_Ticket_Credits < 0)
					{
						Opcions.Disp_Pay_Ticket_Credits = 0;
					}
					Opcions.Disp_Pay_Ticket--;
					if (Opcions.Disp_Pay_Ticket < 0)
					{
						Opcions.Disp_Pay_Ticket = 0;
					}
					Opcions.Disp_Pay_Ticket_Out++;
					if (Opcions.Disp_Pay_Ticket > 0)
					{
						dispenser.Tickets_Out = 0;
						Srv_Sub_Cadeaux(Opcions.Disp_Val, 0);
						Opcions.Disp_Pay_Running = 20;
					}
					else
					{
						dispenser.Tickets_Out = 0;
						Opcions.Disp_Pay_Running = 3;
					}
				}
				else if (dispenser.EnError)
				{
					Opcions.Disp_Pay_Ticket_Cnt_Fail++;
					if (Opcions.Disp_Pay_Ticket_Cnt_Fail > 3)
					{
						Opcions.Disp_Pay_Ticket_Fail = 1;
						dispenser.Close();
						dispenser = null;
						Srv_Sub_Cadeaux(-Opcions.Disp_Val, 0);
						Opcions.Disp_Pay_Running = 30;
					}
					else
					{
						dispenser.Reset();
						Opcions.Disp_Pay_Ticket_Fail = 0;
						Opcions.Disp_Pay_Ticket_Cnt_Fail = 0;
					}
				}
			}
			if (Opcions.Disp_Pay_Running != 1 || dlg_checks != null)
			{
				return;
			}
			Opcions.Disp_Pay_Ticket_Cnt_Fail = 0;
			Opcions.Disp_Pay_Ticket_Fail = 0;
			dlg_checks = new DLG_Dispenser_Out(ref Opcions);
			dlg_checks.Show();
			Application.DoEvents();
			if (dispenser == null)
			{
				dispenser = new DRIVER_Ticket_ICT();
				dispenser.port = dispenser.Find_Device();
				dispenser.Open();
			}
			if (!dispenser.OnLine)
			{
				dispenser.Open();
			}
			if (dispenser.OnLine)
			{
				Opcions.Disp_Pay_Running = 2;
				dispenser.Reset();
				Thread.Sleep(100);
				dispenser.Poll();
				if (Opcions.Disp_Pay_Ticket > 0)
				{
					dispenser.Tickets_Out = 0;
					Srv_Sub_Cadeaux(Opcions.Disp_Val, 0);
					Opcions.Disp_Pay_Running = 20;
				}
				else
				{
					dispenser.Tickets_Out = 0;
					Opcions.Disp_Pay_Running = 3;
				}
			}
			else
			{
				dispenser.Close();
				dispenser = null;
				Opcions.Disp_Pay_Running = 3;
			}
		}

		private void timerCredits_Tick(object sender, EventArgs e)
		{
			Control_Dispenser();
			if (Banner_On == 2 && Opcions.ModoKiosk == 1 && EntraJocs != null)
			{
				if (Opcions.CancelTemps < 20)
				{
					if ((Opcions.CancelTemps & 1) == 1)
					{
						if (EntraJocs.BackgroundImage != Img_Banner2)
						{
							EntraJocs.BackgroundImage = Img_Banner2;
							EntraJocs.Invalidate();
						}
					}
					else if (EntraJocs.BackgroundImage != Img_Banner1)
					{
						EntraJocs.BackgroundImage = Img_Banner1;
						EntraJocs.Invalidate();
					}
				}
				else if (EntraJocs.BackgroundImage != Img_Banner1)
				{
					EntraJocs.BackgroundImage = Img_Banner1;
					EntraJocs.Invalidate();
				}
			}
			if (Opcions.ModoTickets == 1)
			{
				if (Opcions.Temps > 0)
				{
					if (_Srv_Commad == Srv_Command.AddTicket)
					{
						try
						{
							if (bTicket.Enabled)
							{
								bTicket.Enabled = false;
							}
						}
						catch
						{
						}
					}
					else
					{
						try
						{
							if (!bTicket.Enabled)
							{
								if (!Opcions.InGame && MenuGames == 0 && !_WebLink.Contains("://menu."))
								{
									bTicket.Enabled = true;
								}
							}
							else
							{
								if (Opcions.InGame && MenuGames == 0)
								{
									bTicket.Enabled = false;
								}
								if (MenuGames == 0 && _WebLink.Contains("://menu."))
								{
									bTicket.Enabled = false;
								}
							}
						}
						catch
						{
						}
					}
				}
				else
				{
					try
					{
						if (bTicket.Enabled)
						{
							bTicket.Enabled = false;
							if (Opcions.ModoKiosk == 1)
							{
								EntraJocs.Visible = false;
								Banner_On = 0;
							}
						}
						else if (EntraJocs.Visible)
						{
							EntraJocs.Visible = false;
							Banner_On = 0;
						}
					}
					catch
					{
					}
				}
			}
			if (_netConnection != null)
			{
				if (!_netConnection.Connected)
				{
					Srv_Connect();
				}
				else
				{
					if (_Hook_Srv_Verificar_Ticket != null)
					{
						_Hook_Srv_Verificar_Ticket.timeout++;
						switch (_Hook_Srv_Verificar_Ticket.OK)
						{
						case 1:
						{
							_Hook_Srv_Verificar_Ticket.OK = -2;
							Opcions.Ticket_Verificar.Parser(_Hook_Srv_Verificar_Ticket.Resposta);
							bool flag = false;
							ValidacioTicket.Update_Info();
							_Srv_Commad = Srv_Command.Null;
							_Hook_Srv_Verificar_Ticket = null;
							break;
						}
						case 0:
							Srv_Verificar_Ticket(Opcions.Verificar_Ticket, 1);
							break;
						case -1:
							if (_Hook_Srv_Verificar_Ticket.timeout > 100)
							{
								_Hook_Srv_Verificar_Ticket.OK = 0;
							}
							break;
						}
					}
					if (_Hook_Srv_Anular_Ticket != null)
					{
						_Hook_Srv_Anular_Ticket.timeout++;
						switch (_Hook_Srv_Anular_Ticket.OK)
						{
						case 1:
							_Hook_Srv_Anular_Ticket.OK = -2;
							_Srv_Commad = Srv_Command.Null;
							Srv_Verificar_Ticket(_Hook_Srv_Anular_Ticket.ticket, 0);
							_Hook_Srv_Anular_Ticket = null;
							break;
						case 0:
							Srv_Anular_Ticket(_Hook_Srv_Anular_Ticket.ticket, _Hook_Srv_Anular_Ticket.estat, 1);
							break;
						case -1:
							if (_Hook_Srv_Anular_Ticket.timeout > 100)
							{
								_Hook_Srv_Anular_Ticket.OK = 0;
							}
							break;
						}
					}
					if (_Hook_Srv_Add_Pay != null)
					{
						_Hook_Srv_Add_Pay.timeout++;
						switch (_Hook_Srv_Add_Pay.OK)
						{
						case 1:
							_Hook_Srv_Add_Pay.OK = -2;
							if (!string.IsNullOrEmpty(_Hook_Srv_Add_Pay.Resposta))
							{
								int num = Opcions.Ticket_Pago.Parser(_Hook_Srv_Add_Pay.Resposta);
								if (num > 0)
								{
									Opcions.Pagar_Ticket_ID = Opcions.Ticket_Pago.Ticket;
									int join = Opcions.JoinTicket;
									if (Opcions.Temps <= 0)
									{
										join = 0;
									}
									if (Opcions.Disp_Enable == 1)
									{
										Ticket_Out_Check(Opcions.Impresora_Tck, Opcions.Ticket_Pago.Pago, Opcions.Ticket_Pago.Ticket, Opcions.Ticket_Model, Opcions.Ticket_Cut, Opcions.Ticket_N_FEED, Opcions.Ticket_N_HEAD, Opcions.Ticket_60mm, Opcions.Ticket_Pago.DataT, Opcions.Ticket_Pago.CRC, Opcions.Disp_Pay_Ticket_Out, join);
									}
									else if (Opcions.AutoTicketTime == 2 && Opcions.Temps > 0)
									{
										Ticket_Out_Mes_Temps(Opcions.Impresora_Tck, Opcions.Ticket_Pago.Pago, Opcions.Ticket_Pago.Ticket, Opcions.Ticket_Model, Opcions.Ticket_Cut, Opcions.Ticket_N_FEED, Opcions.Ticket_N_HEAD, Opcions.Ticket_60mm, Opcions.Ticket_Pago.DataT, Opcions.Ticket_Pago.CRC, 0, Opcions.Temps);
										Opcions.Temps = 0;
										Opcions.TempsDeTicket = Opcions.Temps;
										Opcions.CancelTemps = 0;
									}
									else
									{
										Ticket_Out(Opcions.Impresora_Tck, Opcions.Ticket_Pago.Pago, Opcions.Ticket_Pago.Ticket, Opcions.Ticket_Model, Opcions.Ticket_Cut, Opcions.Ticket_N_FEED, Opcions.Ticket_N_HEAD, Opcions.Ticket_60mm, Opcions.Ticket_Pago.DataT, Opcions.Ticket_Pago.CRC, join);
									}
								}
							}
							_Srv_Commad = Srv_Command.Null;
							_Hook_Srv_Add_Pay = null;
							break;
						case 0:
							Srv_Add_Pay(1);
							break;
						case -1:
							if (_Hook_Srv_Add_Pay.timeout > 100)
							{
								_Hook_Srv_Add_Pay.OK = 0;
								Srv_Add_Pay(1);
							}
							break;
						}
					}
					if (_Hook_Srv_Add_Ticket != null)
					{
						_Hook_Srv_Add_Ticket.timeout++;
						switch (_Hook_Srv_Add_Ticket.OK)
						{
						case 1:
							_Hook_Srv_Add_Ticket.OK = -2;
							if (Ticket(Opcions.Impresora_Tck, Opcions.ValorTemps, Opcions.Temps, _Hook_Srv_Add_Ticket.Ticket, Opcions.Ticket_Model, Opcions.Ticket_Cut, Opcions.Ticket_N_FEED, Opcions.Ticket_N_HEAD, Opcions.Ticket_60mm))
							{
								Opcions.Temps = 0;
								Opcions.CancelTemps = 0;
								Opcions.TempsDeTicket = 0;
							}
							Status = Fases.GoHome;
							_Srv_Commad = Srv_Command.Null;
							_Hook_Srv_Add_Ticket = null;
							break;
						case 0:
							Srv_Add_Ticket(Opcions.Temps, 1);
							break;
						case -1:
							if (_Hook_Srv_Add_Ticket.timeout > 100)
							{
								_Hook_Srv_Add_Ticket.OK = 0;
								Srv_Add_Ticket(Opcions.Temps, 1);
							}
							break;
						}
					}
					if (_Hook_Srv_Sub_Ticket != null)
					{
						_Hook_Srv_Sub_Ticket.timeout++;
						switch (_Hook_Srv_Sub_Ticket.OK)
						{
						case 1:
							_Hook_Srv_Sub_Ticket.OK = -2;
							Opcions.Temps += Opcions.TicketTemps;
							Opcions.TempsDeTicket = Opcions.Temps;
							Opcions.TicketTemps = 0;
							_Srv_Commad = Srv_Command.Null;
							_Hook_Srv_Sub_Ticket = null;
							break;
						case 0:
							Opcions.TicketTemps = 0;
							Opcions.IdTicketTemps = 0;
							_Srv_Commad = Srv_Command.Null;
							_Hook_Srv_Sub_Ticket = null;
							if (InsertCreditsDLG != null)
							{
								InsertCreditsDLG.Close();
								InsertCreditsDLG.Dispose();
								InsertCreditsDLG = null;
							}
							InsertCreditsDLG = new InsertCredits(ref Opcions, 1);
							InsertCreditsDLG.Show();
							break;
						case -1:
							if (_Hook_Srv_Sub_Ticket.timeout > 100)
							{
								_Hook_Srv_Sub_Ticket.OK = 0;
								Srv_Sub_Ticket(Opcions.IdTicketTemps, 1);
							}
							break;
						}
					}
					if (_Hook_Srv_Add_Credits != null)
					{
						_Hook_Srv_Add_Credits.timeout++;
						switch (_Hook_Srv_Add_Credits.OK)
						{
						case 1:
							Opcions.Send_Add_Credits = 0m;
							_Hook_Srv_Add_Credits.OK = -2;
							_Srv_Commad = Srv_Command.Null;
							SoundCredits();
							break;
						case 0:
							Srv_Add_Credits(Opcions.Send_Add_Credits, 1);
							break;
						case -1:
							if (_Hook_Srv_Add_Credits.timeout > 100)
							{
								_Hook_Srv_Add_Credits.OK = 0;
								Srv_Add_Credits(Opcions.Send_Add_Credits, 1);
							}
							break;
						}
					}
					if (_Hook_Srv_Sub_Credits != null)
					{
						_Hook_Srv_Sub_Credits.timeout++;
						switch (_Hook_Srv_Sub_Credits.OK)
						{
						case 1:
							Opcions.Send_Sub_Credits = 0m;
							_Hook_Srv_Sub_Credits.OK = -2;
							_Srv_Commad = Srv_Command.Null;
							break;
						case 0:
							Srv_Sub_Credits(Opcions.Send_Sub_Credits, 1);
							break;
						case -1:
							if (_Hook_Srv_Sub_Credits.timeout > 100)
							{
								_Hook_Srv_Sub_Credits.OK = 0;
								Srv_Sub_Credits(Opcions.Send_Sub_Credits, 1);
							}
							break;
						}
					}
					if (_Hook_Srv_Sub_Cadeaux != null)
					{
						_Hook_Srv_Sub_Cadeaux.timeout++;
						switch (_Hook_Srv_Sub_Cadeaux.OK)
						{
						case 1:
							_Hook_Srv_Sub_Cadeaux.OK = -2;
							_Srv_Commad = Srv_Command.Null;
							break;
						case 0:
							Srv_Sub_Cadeaux(Opcions.Disp_Val, 1);
							break;
						case -1:
							if (_Hook_Srv_Sub_Cadeaux.timeout > 100)
							{
								_Hook_Srv_Sub_Cadeaux.OK = 0;
								Srv_Sub_Cadeaux(Opcions.Disp_Val, 1);
							}
							break;
						}
					}
					if (_Hook_Srv_Credits != null)
					{
						_Hook_Srv_Credits.timeout++;
						switch (_Hook_Srv_Credits.OK)
						{
						case 1:
							_Hook_Srv_Credits.OK = -2;
							if (_Hook_Srv_Credits.credits >= 0)
							{
								Update_Server_Credits(_Hook_Srv_Credits.credits);
							}
							else
							{
								Update_Server_Credits(0m);
							}
							errorcreditsserv = 0;
							_Srv_Commad = Srv_Command.Null;
							break;
						case 0:
							errorcreditsserv = 1;
							Srv_Credits(1);
							break;
						case -1:
							if (_Hook_Srv_Credits.timeout > 100)
							{
								errorcreditsserv = 1;
								_Hook_Srv_Credits.OK = 0;
								Srv_Credits(1);
							}
							break;
						}
					}
					if (_Hook_Srv_KioskCommand != null)
					{
						_Hook_Srv_KioskCommand.timeout++;
						switch (_Hook_Srv_KioskCommand.OK)
						{
						case 1:
							_Hook_Srv_KioskCommand.OK = -2;
							_Srv_Commad = Srv_Command.Null;
							break;
						case 0:
							Srv_KioskCommand(1);
							break;
						case -1:
							if (_Hook_Srv_KioskCommand.timeout > 100)
							{
								_Hook_Srv_KioskCommand.OK = 0;
								Srv_KioskCommand(1);
							}
							break;
						}
					}
					if (_Hook_Srv_KioskSetTime != null)
					{
						_Hook_Srv_KioskSetTime.timeout++;
						switch (_Hook_Srv_KioskSetTime.OK)
						{
						case 1:
							_Hook_Srv_KioskSetTime.OK = -2;
							_Srv_Commad = Srv_Command.Null;
							break;
						case 0:
							Srv_KioskSetTime(Opcions.Temps, 1);
							break;
						case -1:
							if (_Hook_Srv_KioskSetTime.timeout > 100)
							{
								_Hook_Srv_KioskSetTime.OK = 0;
								Srv_KioskSetTime(Opcions.Temps, 1);
							}
							break;
						}
					}
					if (_Hook_Srv_KioskGetTime != null)
					{
						_Hook_Srv_KioskGetTime.timeout++;
						switch (_Hook_Srv_KioskGetTime.OK)
						{
						case 1:
							_Hook_Srv_KioskGetTime.OK = -2;
							Opcions.Temps = _Hook_Srv_KioskSetTime.segons;
							_Srv_Commad = Srv_Command.Null;
							break;
						case 0:
							Srv_KioskGetTime(1);
							break;
						case -1:
							if (_Hook_Srv_KioskGetTime.timeout > 100)
							{
								_Hook_Srv_KioskGetTime.OK = 0;
								Srv_KioskGetTime(1);
							}
							break;
						}
					}
					if (_Hook_Srv_Login != null)
					{
						_Hook_Srv_Login.timeout++;
						switch (_Hook_Srv_Login.OK)
						{
						case 1:
							_Hook_Srv_Login.OK = -2;
							_Srv_Commad = Srv_Command.Null;
							break;
						case 0:
						{
							bool flag = true;
							Srv_Test_Login(Opcions.Srv_User, Opcions.Srv_User_P, 1);
							break;
						}
						case -1:
							if (_Hook_Srv_Login.timeout > 50)
							{
								bool flag = true;
								_Hook_Srv_Login.OK = 0;
								Srv_Test_Login(Opcions.Srv_User, Opcions.Srv_User_P, 1);
							}
							break;
						}
					}
					if (_Srv_Commad == Srv_Command.Null)
					{
						if (string.IsNullOrEmpty(Opcions.Srv_Room) && _Srv_Commad == Srv_Command.Null)
						{
							Srv_Get_Room();
						}
						if (Opcions.Add_Credits > 0m && Opcions.Send_Add_Credits == 0m && _Srv_Commad == Srv_Command.Null)
						{
							Opcions.Send_Add_Credits = Opcions.Add_Credits;
							Opcions.Add_Credits -= Opcions.Send_Add_Credits;
							Srv_Add_Credits(Opcions.Send_Add_Credits, 0);
						}
						if (Opcions.Sub_Credits > 0m && Opcions.Send_Sub_Credits == 0m && _Srv_Commad == Srv_Command.Null)
						{
							Opcions.Send_Sub_Credits = Opcions.Sub_Credits;
							Opcions.Sub_Credits -= Opcions.Send_Sub_Credits;
							Srv_Sub_Credits(Opcions.Send_Sub_Credits, 0);
						}
						if (_Srv_Commad != 0)
						{
						}
					}
				}
			}
			cnttimerCredits++;
			if (cnttimerCredits < 4)
			{
				return;
			}
			cnttimerCredits = 0;
			if (!Opcions.Running)
			{
				return;
			}
			if (Opcions.InGame)
			{
				if (Opcions.ModoTickets != 1)
				{
					lcdClock.Visible = false;
				}
			}
			else if (Opcions.News == 2 && Opcions.ModoTickets == 0)
			{
				lcdClock.Visible = false;
			}
			else
			{
				lcdClock.Visible = true;
			}
			if (Opcions.ComprarTemps == 1 && Opcions.News != 2)
			{
				Opcions.ComprarTemps = 2;
				Stop_Temps();
				CreditManagerFull creditManagerFull = new CreditManagerFull(ref Opcions);
				creditManagerFull.Show();
			}
			if (errorcreditsserv == 1)
			{
				eCredits.Text = "-";
			}
			else
			{
				eCredits.Text = $"{Opcions.Credits}";
			}
			string text = Opcions.Localize.Text("Insert coins");
			if (Opcions.ModoTickets == 1)
			{
				text = Opcions.Localize.Text("Insert coins or ticket");
			}
			if (Opcions.Temps > 0)
			{
				TimeSpan timeSpan = new TimeSpan(0, 0, Opcions.Temps);
				text = ((timeSpan.Hours != 0) ? string.Format(Opcions.Localize.Text("Time available") + "\r\n{0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : string.Format(Opcions.Localize.Text("Time available") + "\r\n{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds));
				if (timeSpan.Hours == 0 && timeSpan.Minutes == 0)
				{
					text = string.Format(Opcions.Localize.Text("Time available") + "\r\n{0}", timeSpan.Seconds);
				}
				if (MenuGames == 1)
				{
					text = Opcions.Localize.Text("Free\r\nGames");
				}
			}
			else if (Opcions.ModoTickets == 0 && Opcions.Credits > 0m)
			{
				text = Opcions.Localize.Text("Buy time");
			}
			lcdClock.Text = text;
			if (!Es_Pot_Navegar())
			{
				Stop_Temps();
			}
			else if (Opcions.ModoTickets == 0 && Opcions.News == 2 && Opcions.Temps > 0)
			{
				Show_Browser_Nav();
			}
			if (Detectar_Zona_Temps())
			{
				if (Opcions.Temps > 0)
				{
					Show_Browser_Nav();
				}
			}
			else
			{
				Stop_Temps();
			}
			if (Opcions.BrowserBarOn == 1)
			{
				Show_Browser_Nav();
			}
			if (!Opcions.TimeNavigate || Opcions.FullScreen != 0)
			{
				return;
			}
			if (cnttime == 0)
			{
				ltime = DateTime.Now;
				cnttime = 1;
			}
			if (cnttime == 1)
			{
				DateTime now = DateTime.Now;
				TimeSpan timeSpan2 = now.Subtract(ltime);
				if ((int)timeSpan2.TotalSeconds > 0)
				{
					ltime = now;
					Opcions.CancelTemps += (int)timeSpan2.TotalSeconds;
					Opcions.Temps -= (int)timeSpan2.TotalSeconds;
					if (Opcions.TempsDeTicket > 0)
					{
						Opcions.TempsDeTicket = Opcions.Temps;
					}
				}
			}
			if (Opcions.Temps <= 0)
			{
				cnttime = 0;
				Opcions.Temps = 0;
				Opcions.CancelTemps = 0;
				Opcions.TempsDeTicket = Opcions.Temps;
				Status = Fases.GoHome;
				Stop_Temps();
				if (!Opcions.InGame && Opcions.Credits > 0m && Opcions.CancelTempsOn == 1)
				{
					bool flag = false;
					Srv_Sub_Credits(Opcions.Credits, 0);
				}
			}
		}

		public bool PingTest()
		{
			Ping ping = new Ping();
			PingReply pingReply = null;
			try
			{
				pingReply = ping.Send(IPAddress.Parse("8.8.8.8"));
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

		public bool PingServer()
		{
			Ping ping = new Ping();
			PingReply pingReply = null;
			try
			{
				pingReply = ping.Send(Opcions.Srv_Ip);
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

		private string XLat_Error(int _err, int _errserv, int _errdev)
		{
			int num = _err;
			if (_err == 0 && _errserv == 0)
			{
				num = _errdev;
			}
			if (_err == 0 && _errserv > 0)
			{
				num = _errserv;
			}
			switch (num)
			{
			case 0:
				if (ErrorEnLogin == 1)
				{
					return "LOGIN";
				}
				if (ErrorJamp == 1)
				{
					return "JAM";
				}
				return "OK";
			case 200:
				return "ERROR USER";
			case 201:
				return "ERROR SERVER";
			case 100:
				return "ERROR INTERNET";
			case 101:
				return "ERROR NET";
			case 102:
				return "ERROR ISP";
			case 900:
				return "ERROR COIN";
			case 901:
				return "ERROR RM5";
			case 902:
				return "ERROR CCT02";
			case 800:
				return "ERROR BNV";
			case 801:
				return "ERROR BNV SSP";
			case 802:
				return "ERROR BNV SIO";
			case 803:
				return "ERROR BNV TRI";
			case 804:
				return "ERROR BNV F40";
			case 805:
				return "ERROR BNV SSP V3";
			case 500:
			case 501:
			case 502:
			case 503:
			case 510:
				return "ERROR CRD [" + num + "]";
			case 504:
				return "LOGIN";
			default:
				return "ERROR " + num;
			}
		}

		public void Internal_Add_Credits(int _c)
		{
			Internal_Add_Credits((decimal)_c);
		}

		private void Internal_Add_Credits(decimal _c)
		{
			Opcions.LastMouseMove = DateTime.Now;
			Opcions.Add_Credits += _c;
			if (Opcions.ModoTickets == 1 || Opcions.News == 2)
			{
				float num = 60f / (float)Opcions.ValorTemps;
				int num2 = (int)(num * (float)_c);
				Opcions.Temps += num2;
				if (Opcions.TempsDeTicket > 0)
				{
					Opcions.TempsDeTicket = Opcions.Temps;
				}
			}
			if (Opcions.InGame)
			{
				return;
			}
			string text;
			if (Opcions.ModoTickets == 1)
			{
				if (Opcions.News != 1)
				{
					if (Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_A) || Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_B))
					{
						bool flag = true;
						text = "http://" + Opcions.Srv_Web_Ip + "/tickquioscV2.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",tickquioscV2.aspx";
					}
					else
					{
						bool flag = true;
						text = "http://" + Opcions.Srv_Web_Ip + "/tickquiosc.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",tickquiosc.aspx";
					}
				}
				else
				{
					bool flag = true;
					text = "http://" + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",Default.aspx";
				}
			}
			else if (Opcions.News != 1)
			{
				bool flag = true;
				text = "http://" + Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",DemoQuiosk.aspx";
			}
			else
			{
				bool flag = true;
				text = "http://" + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",Default.aspx";
			}
			if (!_WebLink.ToLower().Contains(text.ToLower()))
			{
			}
		}

		private void Configurar_Splash(int _modo)
		{
			if (InsertCreditsDLG != null)
			{
				InsertCreditsDLG.Close();
				InsertCreditsDLG.Dispose();
				InsertCreditsDLG = null;
			}
			string text = "";
			pScreenSaver.Top = 0;
			pScreenSaver.Left = 0;
			bool flag = true;
			pScreenSaver.Width = Screen.PrimaryScreen.Bounds.Width;
			if (_modo != 0)
			{
				pScreenSaver.Height = Screen.PrimaryScreen.Bounds.Height - 150;
				lCALL.Width = pScreenSaver.Width;
				lCALL.Height = 152;
				lCALL.Top = pScreenSaver.Height - 1;
				lCALL.Left = 0;
				lCALL.Text = Opcions.Srv_ID_Tlf;
				lCALL.Visible = true;
				lCALL.Invalidate();
			}
			else
			{
				pScreenSaver.Height = Screen.PrimaryScreen.Bounds.Height;
				lCALL.Visible = false;
				lCALL.Invalidate();
			}
			pScreenSaver.BackgroundImageLayout = ImageLayout.Stretch;
			pScreenSaver.ForeColor = Color.White;
			flag = true;
			pScreenSaver.Text = "";
			if (_modo == 0)
			{
				Label label = pScreenSaver;
				label.Text = label.Text + "Version " + Opcions.VersionPRG + "\nStarting...\n";
			}
			else
			{
				text = XLat_Error(ErrorNet, ErrorNetServer, ErrorDevices);
				pScreenSaver.Text = "(Status:" + XLat_Error(ErrorNet, ErrorNetServer, ErrorDevices) + ")\n Version " + Opcions.VersionPRG + "\n" + pScreenSaver.Text;
			}
			pScreenSaver.Text = "[" + Opcions.Srv_Ip + " - " + Opcions.Srv_port + " - " + Opcions.Srv_User + " - " + Opcions.IDMAQUINA + "]\n" + pScreenSaver.Text;
			switch (_modo)
			{
			case 0:
				pScreenSaver.BackColor = Color.Black;
				if (Opcions.ModoKiosk == 1)
				{
					if (Opcions.News > 0)
					{
						pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\news.png");
					}
					else
					{
						pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\logo.jpg");
					}
				}
				else
				{
					pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\loading.png");
				}
				break;
			case 1:
				pScreenSaver.ForeColor = Color.Black;
				pScreenSaver.BackColor = Color.FromArgb(247, 236, 20);
				lCALL.BackColor = Color.Red;
				lCALL.ForeColor = Color.White;
				if (XLat_Error(ErrorNet, ErrorNetServer, ErrorDevices) == "LOGIN")
				{
					pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\uerr.png");
				}
				else if (XLat_Error(ErrorNet, ErrorNetServer, ErrorDevices) == "JAM")
				{
					SoundAlarm();
					pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\critical.png");
				}
				else
				{
					pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\oos.png");
				}
				break;
			case 2:
				lCALL.BackColor = Color.Yellow;
				lCALL.ForeColor = Color.Black;
				pScreenSaver.ForeColor = Color.Black;
				pScreenSaver.BackColor = Color.Black;
				pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\cfg.png");
				break;
			case 3:
				pScreenSaver.ForeColor = Color.White;
				pScreenSaver.BackColor = Color.Black;
				lCALL.BackColor = Color.Yellow;
				lCALL.ForeColor = Color.Black;
				pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\scr.png");
				break;
			case 10:
				pScreenSaver.ForeColor = Color.White;
				pScreenSaver.BackColor = Color.Black;
				lCALL.BackColor = Color.Yellow;
				lCALL.ForeColor = Color.Black;
				pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\uerr.png");
				break;
			case 100:
				pScreenSaver.ForeColor = Color.White;
				pScreenSaver.BackColor = Color.Black;
				lCALL.BackColor = Color.Yellow;
				lCALL.ForeColor = Color.Black;
				pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\mant.png");
				break;
			case 101:
			case 102:
				pScreenSaver.ForeColor = Color.White;
				pScreenSaver.BackColor = Color.Black;
				lCALL.BackColor = Color.Yellow;
				lCALL.ForeColor = Color.Black;
				pScreenSaver.BackgroundImage = (Bitmap)Image.FromFile(Environment.CurrentDirectory + "\\data\\reset.png");
				break;
			}
			pScreenSaver.Visible = true;
			pScreenSaver.Invalidate();
		}

		public void OutOfService()
		{
			if (Status != Fases.Register && Status != Fases.WaitRegister && Status != Fases.OutOfService && Status != Fases.WaitOutOfService)
			{
				Status = Fases.OutOfService;
				if (Cef.IsInitialized)
				{
					GoWebInicial();
				}
			}
		}

		public void Check_Connection()
		{
			if (IsNetworkAvailable())
			{
				if (PingTest())
				{
					ErrorNet = 0;
					return;
				}
				OutOfService();
				ErrorNet = 101;
			}
			else
			{
				OutOfService();
				ErrorNet = 100;
			}
		}

		public void Check_Connection_WD()
		{
			if (!IsNetworkAvailable())
			{
				OutOfService();
			}
		}

		public void CloseOSK()
		{
			Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
			if (processesByName != null && processesByName.Length >= 1)
			{
				PipeClient pipeClient = new PipeClient();
				if (pipeClient != null)
				{
					try
					{
						pipeClient.Send("QUIT", "KVKeyboard");
					}
					catch
					{
					}
				}
			}
		}

		public void ForceKillVNC()
		{
			Opcions.ForceSpy = false;
			Opcions.Spy = 2;
			bool flag = true;
			string text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\uvnc bvba\\UltraVNC\\winvnc.exe";
			if (File.Exists(text))
			{
				if (Configuracion.VNC_Running())
				{
					Process.Start(text, "-kill");
				}
				int num = 0;
				while (Configuracion.VNC_Running())
				{
					num++;
					Thread.Sleep(500);
					if (num == 4)
					{
						Process.Start(text, "-kill");
					}
					if (num > 10)
					{
						break;
					}
				}
			}
			else
			{
				MessageBox.Show("No VNC Installed");
			}
		}

		public void ForceChangePassword()
		{
			bool flag = true;
		}

		private void Estat_Servidor_VAR()
		{
			if (string.IsNullOrEmpty(Opcions.RemoteCmd))
			{
				return;
			}
			switch (Opcions.RemoteCmd.ToUpper())
			{
			case "REMOTE":
			{
				doReset = 0;
				Opcions.RemoteCmd = "";
				Srv_KioskCommand(1);
				for (int i = 0; i < 10; i++)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				if (Opcions.ForceSpy)
				{
					for (int i = 0; i < 5; i++)
					{
						Application.DoEvents();
						Thread.Sleep(100);
					}
					ForceKillVNC();
					for (int i = 0; i < 10; i++)
					{
						Application.DoEvents();
						Thread.Sleep(100);
					}
				}
				if (!Opcions.ForceSpy && Opcions.Spy > 0)
				{
					Opcions.UserSpy = Opcions.Srv_User;
					Opcions.SpyUser(Opcions.RemoteParam);
				}
				break;
			}
			case "REMOFF":
			{
				doReset = 0;
				Opcions.RemoteCmd = "";
				Srv_KioskCommand(1);
				for (int i = 0; i < 10; i++)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				ForceKillVNC();
				break;
			}
			case "MODWIN":
			{
				doReset = 0;
				Opcions.RemoteCmd = "";
				Srv_KioskCommand(1);
				for (int i = 0; i < 10; i++)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				ForceModoWindows();
				break;
			}
			case "CHGKEY":
			{
				doReset = 0;
				Opcions.RemoteCmd = "";
				Srv_KioskCommand(1);
				for (int i = 0; i < 10; i++)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				ForceChangePassword();
				break;
			}
			case "SNDBLL":
			{
				doReset = 0;
				Opcions.RemoteCmd = "";
				Srv_KioskCommand(1);
				for (int i = 0; i < 10; i++)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				break;
			}
			case "PWROFF":
			{
				doReset = 0;
				Opcions.RemoteCmd = "";
				Srv_KioskCommand(1);
				for (int i = 0; i < 10; i++)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				Status = Fases.GoRemoteReset;
				break;
			}
			case "KSKOFF":
				if (Opcions.EnManteniment == 0 && Opcions.Running)
				{
					Opcions.EnManteniment = 1;
					for (int i = 0; i < 10; i++)
					{
						Application.DoEvents();
						Thread.Sleep(100);
					}
					Status = Fases.GoManteniment;
					doReset = 0;
				}
				break;
			case "KSKON":
				Opcions.RemoteCmd = "";
				Srv_KioskCommand(1);
				if (Opcions.EnManteniment == 1 && Opcions.Running)
				{
					Opcions.EnManteniment = 0;
					for (int i = 0; i < 10; i++)
					{
						Application.DoEvents();
						Thread.Sleep(100);
					}
					Status = Fases.Reset;
					doReset = 0;
				}
				break;
			case "GOCFG":
			{
				Opcions.RemoteCmd = "";
				Srv_KioskCommand(1);
				for (int i = 0; i < 10; i++)
				{
					Application.DoEvents();
					Thread.Sleep(100);
				}
				Opcions.ForceGoConfig = true;
				Status = Fases.Reset;
				break;
			}
			}
		}

		private void Estat_Servidor()
		{
			bool flag = true;
			load_web("http://" + Opcions.Srv_Web_Ip + "/__check.html", 1);
			if (!string.IsNullOrEmpty(web_versio))
			{
				int num = 2261;
				try
				{
					num = int.Parse(web_versio);
				}
				catch
				{
				}
				if (2261 < num)
				{
					Status = Fases.GoUpdate;
					return;
				}
			}
			if (Opcions.ForceReset && Status != Fases.RemoteReset)
			{
				Status = Fases.GoRemoteReset;
			}
			else if (Opcions.ForceManteniment && Status != Fases.Manteniment)
			{
				Status = Fases.GoManteniment;
			}
			else if (!Opcions.ForceSpy && Opcions.Spy > 0)
			{
				Opcions.SpyUser(Opcions.RemoteParam);
			}
		}

		private void GoWebInicial()
		{
			if (Opcions.FreeGames == 1)
			{
				TimeSpan timeSpan;
				if (Opcions.Credits + Opcions.Add_Credits > 0m && errorcreditsserv == 0)
				{
					timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
					if (Opcions.ModoTickets == 1)
					{
						if (Opcions.News != 1)
						{
							if (Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_A) || Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_B))
							{
								bool flag = true;
								WebLink = "http://" + Opcions.Srv_Web_Ip + "/tickquioscV2.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",tickquioscV2.aspx," + timeSpan.TotalMilliseconds;
							}
							else
							{
								bool flag = true;
								WebLink = "http://" + Opcions.Srv_Web_Ip + "/tickquiosc.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",tickquiosc.aspx," + timeSpan.TotalMilliseconds;
							}
						}
						else
						{
							bool flag = true;
							WebLink = "http://" + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + "," + Opcions.Srv_Web_Page + "," + timeSpan.TotalMilliseconds;
						}
					}
					else if (Opcions.News != 1)
					{
						bool flag = true;
						WebLink = "http://" + Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",DemoQuiosk.aspx," + timeSpan.TotalMilliseconds;
					}
					else
					{
						bool flag = true;
						WebLink = "http://" + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + "," + Opcions.Srv_Web_Page + "," + timeSpan.TotalMilliseconds;
					}
					return;
				}
				timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
				if (Opcions.ModoTickets == 1)
				{
					if (Opcions.News != 1)
					{
						if (Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_A) || Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_B))
						{
							bool flag = true;
							WebLink = "http://" + Opcions.Srv_Web_Ip + "/tickquioscV2.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",tickquioscV2.aspx," + timeSpan.TotalMilliseconds;
						}
						else
						{
							bool flag = true;
							WebLink = "http://" + Opcions.Srv_Web_Ip + "/tickquiosc.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",tickquiosc.aspx," + timeSpan.TotalMilliseconds;
						}
					}
					else
					{
						bool flag = true;
						WebLink = "http://" + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + "," + Opcions.Srv_Web_Page + "," + timeSpan.TotalMilliseconds;
					}
				}
				else if (Opcions.News != 1)
				{
					bool flag = true;
					WebLink = "http://" + Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",DemoQuiosk.aspx," + timeSpan.TotalMilliseconds;
				}
				else
				{
					bool flag = true;
					WebLink = "http://" + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + "," + Opcions.Srv_Web_Page + "," + timeSpan.TotalMilliseconds;
				}
			}
			else
			{
				WebLink = "about:blank";
			}
		}

		[DllImport("user32.dll")]
		public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

		[DllImport("user32.dll")]
		private static extern bool SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);

		private void timerPoll_Tick(object sender, EventArgs e)
		{
			if (Opcions.__ModoTablet == 1 && _pipeServer != null)
			{
				_pipeServer.Listen("Kiosk");
			}
			Random random = new Random();
			string text = "";
			if (enpoolint)
			{
				return;
			}
			enpoolint = true;
			if (_netConnection != null)
			{
				if (_netConnection.Connected)
				{
					bHome.BackColor = Color.Green;
					if (_sharedObject != null && !_sharedObject.Connected)
					{
						bHome.BackColor = Color.Orange;
					}
				}
				else
				{
					bHome.BackColor = Color.Red;
				}
			}
			if (controlcredits == 1 && Status != Fases.WaitOutOfService && Status != Fases.WaitLockUser)
			{
				timerCredits_Tick(sender, e);
			}
			if (!Opcions.Running && Status != Fases.WaitOutOfService)
			{
				bool flag = true;
				pScreenSaver.Text = "";
				text = XLat_Error(ErrorNet, ErrorNetServer, ErrorDevices);
				pScreenSaver.Text = "(Status:" + XLat_Error(ErrorNet, ErrorNetServer, ErrorDevices) + ")\n" + Status.ToString() + " [" + Opcions.Srv_User + " - " + Opcions.IDMAQUINA + "]\nVersion " + Opcions.VersionPRG + pScreenSaver.Text;
			}
			if (_sem_timerPoll_Tick == 1)
			{
				enpoolint = false;
				return;
			}
			_sem_timerPoll_Tick = 1;
			cntInt++;
			FiltreCnt++;
			TimeSpan timeSpan;
			if (FiltreCnt >= 10)
			{
				if (Opcions.InGame && ValidacioTimeOut != null)
				{
					ValidacioTimeOut.Dispose();
					ValidacioTimeOut = null;
					Opcions.LastMouseMove = DateTime.Now;
				}
				if (ValidacioTimeOut != null && ValidacioTimeOut.OK)
				{
					Opcions.Temps = 0;
					TimeoutHome = 0;
					lcdClock.Invalidate();
					pMenu.Invalidate();
					Invalidate();
					ValidacioTimeOut.Dispose();
					ValidacioTimeOut = null;
					Opcions.LastMouseMove = DateTime.Now;
				}
				FiltreCnt = 0;
				timeSpan = DateTime.Now - Opcions.LastMouseMove;
				int num = (int)timeSpan.TotalSeconds;
				if (num > Opcions.ResetTemps && Opcions.Temps > 0 && Opcions.Credits <= 0m && Opcions.ticketCleanTemps == 0)
				{
					if (ValidacioTimeOut == null)
					{
						ValidacioTimeOut = new DLG_TimeOut(ref Opcions, "Clean time");
						ValidacioTimeOut.Show();
					}
					if (ValidacioTimeOut.IsDisposed)
					{
						ValidacioTimeOut = new DLG_TimeOut(ref Opcions, "Clean time");
						ValidacioTimeOut.Show();
					}
				}
			}
			if (Opcions.FullScreen == 2)
			{
				Opcions.FullScreen = 0;
				if (Opcions.News != 1)
				{
					pMenu.Visible = true;
				}
				else
				{
					pMenu.Visible = false;
				}
				navegador.Visible = false;
				bool flag = true;
				base.FormBorderStyle = FormBorderStyle.None;
				base.Bounds = Screen.PrimaryScreen.Bounds;
				Hide_Browser_Nav();
			}
			if (Opcions.ModoKiosk == 0 && Opcions.InGame && !Opcions.U_InGame)
			{
				SetCursorPos(1, 200);
				mouse_event(2, 1, 200, 0, 0);
				mouse_event(4, 1, 200, 0, 0);
				if (Opcions.CursorOn == 0)
				{
					ShowCursor(bShow: false);
					ShowCursor(bShow: false);
					ShowCursor(bShow: false);
					ShowCursor(bShow: false);
				}
			}
			if (Opcions.InGame)
			{
				if (!Opcions.U_InGame)
				{
					if (Opcions.NoCreditsInGame == 1)
					{
						oldStopCredits = false;
						Opcions.StopCredits = true;
					}
					CloseOSK();
					Opcions.U_InGame = true;
					Opcions.TempsDeTicket = Opcions.Temps;
					bool flag = true;
					if (oldStopCredits != Opcions.StopCredits)
					{
						if (Opcions.StopCredits)
						{
							pInsertCoin.Image = Resources.insertcoin2_off;
							Opcions.Enable_Lectors = -1;
							Close_Devices();
						}
						else
						{
							pInsertCoin.Image = null;
							if (Opcions.Enable_Lectors != 2 && !Opcions.StopCredits)
							{
								Opcions.Error_Billetero = 0;
								Opcions.Enable_Lectors = 0;
							}
						}
					}
					oldStopCredits = Opcions.StopCredits;
				}
			}
			else
			{
				if (Opcions.U_InGame)
				{
					if (Opcions.NoCreditsInGame == 1)
					{
						if (Opcions.Credits > 0m)
						{
							oldStopCredits = false;
							Opcions.StopCredits = true;
						}
						else
						{
							oldStopCredits = true;
							Opcions.StopCredits = false;
						}
					}
					if (Opcions.Temps > 0 && (Opcions.ModoTickets == 1 || Opcions.News == 2) && MenuGames == 0)
					{
						if (!WebLink.ToLower().Contains("tickquioscv2"))
						{
							MenuGames = 1;
							bool flag = false;
							try
							{
								bTicket.Enabled = false;
							}
							catch
							{
							}
						}
						else
						{
							try
							{
								bTicket.Enabled = true;
							}
							catch
							{
							}
							if (Opcions.Temps <= 0)
							{
								MenuGames = 0;
								try
								{
									bTicket.Enabled = false;
								}
								catch
								{
								}
							}
							bool flag = true;
						}
					}
					if (!Opcions.InGame)
					{
						Opcions.TempsDeTicket = Opcions.Temps;
						bool flag = true;
					}
				}
				Opcions.Pagar_Ticket_Busy = 0;
				Opcions.U_InGame = false;
				if (oldStopCredits != Opcions.StopCredits)
				{
					if (Opcions.StopCredits)
					{
						pInsertCoin.Image = Resources.insertcoin2_off;
						Opcions.Enable_Lectors = -1;
						Close_Devices();
					}
					else
					{
						pInsertCoin.Image = null;
						if (Opcions.Enable_Lectors != 2)
						{
							Opcions.Error_Billetero = 0;
							Opcions.Enable_Lectors = 0;
						}
					}
				}
				oldStopCredits = Opcions.StopCredits;
			}
			switch (Opcions.Enable_Lectors)
			{
			case 0:
				switch (Opcions.Dev_Coin.ToLower())
				{
				case "rm5":
					if (rm5 == null && Opcions.Dev_Coin_P != "-" && Opcions.Dev_Coin_P != "?")
					{
						rm5 = new Control_Comestero();
						rm5.port = Opcions.Dev_Coin_P;
						int dev_Bank = Opcions.Dev_Bank;
						if (dev_Bank == 1)
						{
							rm5.Set_Brazil();
						}
						else
						{
							rm5.Set_Euro();
						}
						if (!rm5.Open())
						{
							ErrorDevices = 901;
							OutOfService();
						}
						needEnableM = 1;
						waitneedEnableM = 0;
					}
					break;
				case "cct2":
					if (cct2 == null && Opcions.Dev_Coin_P != "-" && Opcions.Dev_Coin_P != "?")
					{
						cct2 = new Control_CCTALK_COIN();
						cct2.port = Opcions.Dev_Coin_P;
						if (!cct2.Open())
						{
							ErrorDevices = 902;
							OutOfService();
						}
						needEnableM = 1;
						waitneedEnableM = 0;
					}
					break;
				}
				switch (Opcions.Dev_BNV.ToLower())
				{
				case "ssp":
					if (ssp != null)
					{
						break;
					}
					Opcions.Error_Billetero = 0;
					if (!(Opcions.Dev_BNV_P != "-") || !(Opcions.Dev_BNV_P != "?"))
					{
						break;
					}
					ssp = new Control_NV_SSP();
					ssp.port = Opcions.Dev_BNV_P;
					if (!ssp.Open())
					{
						ForceCheck = true;
						if (Opcions.ModoTickets != 0 || Opcions.News != 2)
						{
							bool flag = true;
							Status = Fases.Reset;
						}
					}
					needEnable = 1;
					waitneedEnable = 0;
					break;
				case "ssp3":
					if (ssp3 != null)
					{
						break;
					}
					Opcions.Error_Billetero = 0;
					if (!(Opcions.Dev_BNV_P != "-") || !(Opcions.Dev_BNV_P != "?"))
					{
						break;
					}
					ssp3 = new Control_NV_SSP_P6();
					ssp3.port = Opcions.Dev_BNV_P;
					if (!ssp3.Open())
					{
						ForceCheck = false;
						if (Opcions.ModoTickets != 0 || Opcions.News != 2)
						{
							bool flag = true;
							Status = Fases.Reset;
						}
					}
					needEnable = 1;
					waitneedEnable = 0;
					break;
				case "sio":
					if (sio != null)
					{
						break;
					}
					Opcions.Error_Billetero = 0;
					if (Opcions.Dev_BNV_P != "-" && Opcions.Dev_BNV_P != "?")
					{
						sio = new Control_NV_SIO();
						sio.port = Opcions.Dev_BNV_P;
						switch (Opcions.Dev_Bank)
						{
						case 2:
							sio.Set_Dominicana();
							break;
						case 1:
							sio.Set_Brazil();
							break;
						default:
							sio.Set_Euro();
							break;
						}
						if (!sio.Open())
						{
							ErrorDevices = 802;
							OutOfService();
						}
						needEnable = 1;
						waitneedEnable = 0;
					}
					break;
				case "f40":
					if (f40 != null)
					{
						break;
					}
					Opcions.Error_Billetero = 0;
					if (Opcions.Dev_BNV_P != "-" && Opcions.Dev_BNV_P != "?")
					{
						f40 = new Control_F40_CCTalk();
						f40.port = Opcions.Dev_BNV_P;
						switch (Opcions.Dev_Bank)
						{
						case 2:
							f40.Set_Dominicana();
							break;
						case 1:
							f40.Set_Brazil();
							break;
						default:
							f40.Set_Euro();
							break;
						}
						if (!f40.Open())
						{
							ErrorDevices = 804;
							OutOfService();
						}
						needEnable = 1;
						waitneedEnable = 0;
					}
					break;
				case "tri":
					if (tri != null)
					{
						break;
					}
					Opcions.Error_Billetero = 0;
					if (Opcions.Dev_BNV_P != "-" && Opcions.Dev_BNV_P != "?")
					{
						tri = new Control_Trilogy();
						if (Opcions.Dev_Bank == 1)
						{
							tri.Set_Brazil();
						}
						else
						{
							tri.Set_Euro();
						}
						tri.port = Opcions.Dev_BNV_P;
						if (!tri.Open())
						{
							ErrorDevices = 803;
							OutOfService();
						}
						needEnable = 1;
						waitneedEnable = 0;
					}
					break;
				}
				Opcions.Enable_Lectors = 2;
				break;
			case 2:
				waitneedEnable++;
				waitneedEnableM++;
				if (rm5 != null)
				{
					if (errorcreditsserv == 1)
					{
						if (needEnableM != 102)
						{
							needEnableM = 2;
						}
					}
					else if (needEnableM != 101)
					{
						needEnableM = 1;
					}
					if (waitneedEnableM > 8)
					{
						waitneedEnableM = 0;
						if (needEnableM == 1)
						{
							if (!Opcions.StopCredits)
							{
								rm5.Enable();
							}
							needEnableM += 100;
						}
						if (needEnableM == 2)
						{
							rm5.Disable();
							needEnableM += 100;
						}
					}
					if (rm5.SwapControl == 0)
					{
						rm5.Poll();
						rm5.SwapControl ^= 1;
					}
					else
					{
						rm5.Parser();
						rm5.SwapControl ^= 1;
						if (rm5.Creditos > 0)
						{
							int creditos = rm5.Creditos;
							Internal_Add_Credits(creditos);
							rm5.Creditos -= creditos;
							cnttimerCredits = 11;
						}
					}
				}
				if (cct2 != null)
				{
					if (errorcreditsserv == 1)
					{
						if (needEnableM != 102)
						{
							needEnableM = 2;
						}
					}
					else if (needEnableM != 101)
					{
						needEnableM = 1;
					}
					if (waitneedEnableM > 8)
					{
						waitneedEnableM = 0;
						if (needEnableM == 1)
						{
							if (!Opcions.StopCredits)
							{
								cct2.Enable(cct2.Coin_Acceptor_ID);
							}
							needEnableM += 100;
						}
						if (needEnableM == 2)
						{
							cct2.Disable(cct2.Coin_Acceptor_ID);
							needEnableM += 100;
						}
					}
					cct2.Poll();
					if (cct2.Creditos > 0)
					{
						int creditos = cct2.Creditos;
						Internal_Add_Credits(creditos);
						cct2.Creditos -= creditos;
						cnttimerCredits = 11;
					}
				}
				if (ssp != null)
				{
					if (errorcreditsserv == 1)
					{
						if (needEnable != 102)
						{
							needEnable = 2;
						}
					}
					else if (needEnable != 101)
					{
						needEnable = 1;
					}
					if (waitneedEnable > 8)
					{
						waitneedEnable = 0;
						if (needEnable == 1)
						{
							if (!Opcions.StopCredits)
							{
								ssp.Enable();
							}
							needEnable += 100;
						}
						if (needEnable == 2)
						{
							ssp.Disable();
							needEnable += 100;
						}
					}
					if (!ssp.Poll())
					{
						Opcions.Error_Billetero++;
						if (Opcions.Error_Billetero > 10)
						{
							Close_Devices();
							Opcions.Error_Billetero = 0;
							ErrorDevices = 800;
							OutOfService();
						}
					}
					else if (ssp.TimeOutComs > 4 && ssp.TimeOutComs != 1000)
					{
						ssp.Close();
						ssp = null;
						Opcions.Enable_Lectors = 0;
					}
					else
					{
						ssp.TimeOutComs = 0;
						Opcions.Error_Billetero = 0;
						if (ssp.Creditos > 0m)
						{
							decimal creditos2 = ssp.Creditos;
							Internal_Add_Credits(creditos2);
							ssp.Creditos -= creditos2;
							cnttimerCredits = 11;
						}
					}
				}
				if (ssp3 != null)
				{
					if (errorcreditsserv == 1)
					{
						if (needEnable != 102)
						{
							needEnable = 2;
						}
					}
					else if (needEnable != 101)
					{
						needEnable = 1;
					}
					if (waitneedEnable > 8)
					{
						waitneedEnable = 0;
						if (needEnable == 1)
						{
							if (!Opcions.StopCredits)
							{
								ssp3.Enable();
							}
							needEnable += 100;
						}
						if (needEnable == 2)
						{
							ssp3.Disable();
							needEnable += 100;
						}
					}
					if (!ssp3.Poll())
					{
						Opcions.Error_Billetero++;
						if (Opcions.Error_Billetero > 10)
						{
							Close_Devices();
							Opcions.Error_Billetero = 0;
							ErrorDevices = 800;
							OutOfService();
						}
					}
					else if (ssp3.TimeOutComs > 20)
					{
						ssp3.Close();
						ssp3 = null;
						Opcions.Enable_Lectors = 0;
					}
					else
					{
						Opcions.Error_Billetero = 0;
						if (ssp3.Creditos > 0)
						{
							decimal creditos2 = ssp3.Creditos;
							Internal_Add_Credits(creditos2);
							ssp3.Creditos -= (int)creditos2;
							cnttimerCredits = 11;
						}
					}
				}
				if (sio != null)
				{
					if (errorcreditsserv == 1)
					{
						if (needEnable != 102)
						{
							needEnable = 2;
						}
					}
					else if (needEnable != 101)
					{
						needEnable = 1;
					}
					if (waitneedEnable > 8)
					{
						waitneedEnable = 0;
						if (needEnable == 1)
						{
							if (!Opcions.StopCredits)
							{
								sio.Enable();
							}
							needEnable += 100;
						}
						if (needEnable == 2)
						{
							sio.Disable();
							needEnable += 100;
						}
					}
					sio.Poll();
					sio.Parser();
					if (sio.CriticalJamp == 0)
					{
						if (sio.Creditos > 0)
						{
							int creditos = sio.Creditos;
							Internal_Add_Credits(creditos);
							sio.Creditos -= creditos;
							cnttimerCredits = 11;
						}
					}
					else if (sio.CriticalJamp == 1)
					{
						ErrorJamp = 1;
						sio.CriticalJamp = 2;
					}
				}
				if (tri != null)
				{
					if (errorcreditsserv == 1)
					{
						if (needEnable != 102)
						{
							needEnable = 2;
						}
					}
					else if (needEnable != 101)
					{
						needEnable = 1;
					}
					if (waitneedEnable > 8)
					{
						waitneedEnable = 0;
						if (needEnable == 1)
						{
							if (!Opcions.StopCredits)
							{
								tri.Enable();
							}
							needEnable += 100;
						}
						if (needEnable == 2)
						{
							tri.Disable();
							needEnable += 100;
						}
					}
					tri.Poll();
					tri.Parser();
					if (tri.Creditos > 0)
					{
						int creditos = tri.Creditos;
						Internal_Add_Credits(creditos);
						tri.Creditos -= creditos;
						cnttimerCredits = 11;
					}
				}
				if (f40 == null)
				{
					break;
				}
				if (errorcreditsserv == 1)
				{
					if (needEnable != 102 && needEnable != 202)
					{
						needEnable = 2;
					}
				}
				else if (needEnable != 101 && needEnable != 201)
				{
					needEnable = 1;
				}
				if (waitneedEnable > 8)
				{
					waitneedEnable = 0;
					if (needEnable == 101)
					{
						if (!Opcions.StopCredits)
						{
							f40.Enable_All();
						}
						needEnable += 100;
					}
					if (needEnable == 1)
					{
						if (!Opcions.StopCredits)
						{
							f40.Enable();
						}
						needEnable += 100;
					}
					if (needEnable == 102)
					{
						f40.Disable_All();
						needEnable += 100;
					}
					if (needEnable == 2)
					{
						f40.Disable();
						needEnable += 100;
					}
				}
				f40.Poll();
				if (f40.TimeOutComs > 10)
				{
					f40.Close();
					f40 = null;
					Opcions.Enable_Lectors = 0;
					break;
				}
				Opcions.Error_Billetero = 0;
				if (f40.Creditos > 0)
				{
					int creditos = f40.Creditos;
					Internal_Add_Credits(creditos);
					f40.Creditos -= creditos;
					cnttimerCredits = 11;
				}
				break;
			}
			if (Opcions.Show_Browser)
			{
				if (Opcions.FreeGames == 0)
				{
					if (WebLink == "about:blank")
					{
						if (navegador.Visible)
						{
							navegador.Visible = false;
						}
					}
					else if (!navegador.Visible)
					{
						navegador.Visible = true;
					}
				}
				else if (!navegador.Visible)
				{
					navegador.Visible = true;
				}
			}
			if (Opcions.MissatgeCreditsGratuits >= 1)
			{
				Opcions.MissatgeCreditsGratuits = 0;
				if (dlg_playforticket != null)
				{
					dlg_playforticket.Close();
					dlg_playforticket.Dispose();
					dlg_playforticket = null;
				}
				dlg_playforticket = new DLG_Message_Full(Opcions.Localize.Text("On doit jouer tous les credits gratuits pour prendre a check cadeaux!"), ref Opcions, _warn: true);
				dlg_playforticket.Show();
				Opcions.ForceRefresh = 1;
			}
			if (Opcions.MissatgePrinter != "")
			{
				if (dlg_msg_printer != null)
				{
					dlg_msg_printer.Close();
					dlg_msg_printer.Dispose();
					dlg_msg_printer = null;
				}
				dlg_msg_printer = new DLG_Message_Full(Opcions.MissatgePrinter, ref Opcions, _warn: true);
				dlg_msg_printer.Show();
				Opcions.MissatgePrinter = "";
				Opcions.ForceRefresh = 1;
			}
			if (dlg_msg_printer != null && dlg_msg_printer.IsClosed)
			{
				dlg_msg_printer.Close();
				dlg_msg_printer.Dispose();
				dlg_msg_printer = null;
			}
			if (dlg_playforticket != null && dlg_playforticket.IsClosed)
			{
				dlg_playforticket.Close();
				dlg_playforticket.Dispose();
				dlg_playforticket = null;
			}
			switch (Status)
			{
			case Fases.Reset:
			{
				Control_Getton_Reset();
				Reconnect_Service();
				Opcions.Block_Remotes();
				timerMessages.Enabled = false;
				old_credits_gratuits = -1m;
				StartupDetect = 1;
				Opcions.Load_Net();
				if (Opcions.ModoPlayCreditsGratuits == 0)
				{
					lCGRAT.Visible = false;
				}
				else
				{
					lCGRAT.Visible = true;
					lCGRAT.Text = "Credits gratuits\r\n" + Opcions.CreditsGratuits;
					lCGRAT.Invalidate();
				}
				bool flag = true;
				Opcions.MissatgeCreditsGratuits = 0;
				if (dlg_playforticket != null)
				{
					dlg_playforticket.Close();
					dlg_playforticket.Dispose();
					dlg_playforticket = null;
				}
				Opcions.MissatgePrinter = "";
				if (dlg_msg_printer != null)
				{
					dlg_msg_printer.Close();
					dlg_msg_printer.Dispose();
					dlg_msg_printer = null;
				}
				if (dlg_checks != null)
				{
					dlg_checks.Close();
					dlg_checks.Dispose();
					dlg_checks = null;
				}
				Configuracion.Access_Log("Reset kiosk");
				MenuGames = 0;
				Banner_On = 0;
				Opcions.ticketCleanTemps = 0;
				Opcions.EnManteniment = 0;
				DelayServer = 500 + random.Next(100) * 10;
				if (Opcions.__ModoTablet == 0)
				{
					CloseOSK();
				}
				else if (Opcions.ModoKiosk != 0)
				{
					Tablet();
				}
				if (publi != null)
				{
					publi.Close();
					publi.Dispose();
					publi = null;
				}
				if (_DCalibrar != null)
				{
					_DCalibrar.Close();
					_DCalibrar.Dispose();
					_DCalibrar = null;
				}
				if (snd_alarma != null)
				{
					SoundRestore();
					snd_alarma.Stop();
				}
				EntraJocs.Visible = false;
				snd_alarma = null;
				ErrorEnLogin = 0;
				Opcions.Emu_Mouse_RClick = false;
				ShowCursor(bShow: true);
				ShowCursor(bShow: true);
				ShowCursor(bShow: true);
				ShowCursor(bShow: true);
				if (_netConnection != null)
				{
					_netConnection.Close();
				}
				Opcions.U_InGame = false;
				Opcions.Srv_Room = "";
				Opcions.ForceAllKey = 0;
				Opcions.Temps = 0;
				Opcions.CancelTemps = 0;
				Opcions.TempsDeTicket = Opcions.Temps;
				Opcions.Running = false;
				Opcions.Enable_Lectors = -1;
				Close_Devices();
				Configurar_Splash(0);
				Error_Servidor = 0;
				cntInt = 0;
				ErrorNet = 0;
				ErrorDevices = 0;
				tmp_ip = Opcions.Srv_Ip;
				tmp_user = Opcions.Srv_User;
				tmp_pw = Opcions.Srv_User_P;
				controlcredits = 0;
				timerStartup.Enabled = false;
				Opcions.RunConfig = false;
				if (Opcions.ForceGoConfig)
				{
					Opcions.RunConfig = true;
				}
				DelayInt = 0;
				if (InsertCreditsDLG != null)
				{
					InsertCreditsDLG.Close();
					InsertCreditsDLG.Dispose();
					InsertCreditsDLG = null;
				}
				Status = Fases.WaitReset;
				GoWebInicial();
				if (doReset > 10)
				{
					flag = true;
					Status = Fases.GoRemoteReset;
					doReset = 0;
				}
				break;
			}
			case Fases.WaitReset:
				AdminEnabled = true;
				DelayInt++;
				if (DelayInt < 50)
				{
					if (Opcions.RunConfig || Opcions.ForceGoConfig)
					{
						Status = Fases.Config;
					}
				}
				else
				{
					DelayInt = 0;
					Status = Fases.WaitDevices;
				}
				break;
			case Fases.WaitDevices:
				if (!Opcions.RunConfig && !Opcions.ForceGoConfig)
				{
					Find_Validator();
				}
				DelayInt = 0;
				Status = Fases.Splash;
				if (Opcions.NoCreditsInGame == 1 && Opcions.Credits > 0m)
				{
					oldStopCredits = false;
					Opcions.StopCredits = true;
				}
				break;
			case Fases.Splash:
				DelayInt++;
				AdminEnabled = true;
				controlcredits = 1;
				if (CoolStartUp)
				{
					CoolStartUp = false;
				}
				if (DelayInt <= 10)
				{
					break;
				}
				Opcions.RunConfig = false;
				if (Opcions.ForceGoConfig)
				{
					Opcions.RunConfig = true;
				}
				if (ErrorEnLogin == 1 || ErrorJamp == 1)
				{
					AdminEnabled = true;
					DelayInt = 0;
					OutOfService();
					break;
				}
				AdminEnabled = true;
				DelayInt = 0;
				Srv_Connect();
				Thread.Sleep(1000);
				Srv_Test_Login(Opcions.Srv_User, Opcions.Srv_User_P, 0);
				DelayInt = 0;
				if (Status != Fases.OutOfService)
				{
					timerStartup.Interval = 4000;
					timerStartup.Enabled = true;
					Status = Fases.WaitSplash;
				}
				break;
			case Fases.WaitSplash:
				DelayInt++;
				if (DelayInt > 20)
				{
					int dev_Bank = _Hook_Srv_Login.OK;
					if (dev_Bank == -2)
					{
						if (_Hook_Srv_Login.login == 1)
						{
							Ticket_Add_Credits = _Hook_Srv_Login.ticket;
							Status = Fases.CheckRoom;
							bool flag = true;
						}
						else if (_Hook_Srv_Login.login == 2)
						{
							Status = Fases.LockUser;
						}
						else
						{
							Status = Fases.Reset;
						}
					}
				}
				if (DelayInt > 40)
				{
					bool flag = true;
					Srv_Connect();
					Thread.Sleep(1000);
					_Srv_Commad = Srv_Command.Null;
					if (_Hook_Srv_Login != null)
					{
						_Hook_Srv_Login.OK = -2;
					}
					Srv_Test_Login(Opcions.Srv_User, Opcions.Srv_User_P, 0);
					DelayInt = 0;
				}
				if (Opcions.RunConfig)
				{
					Status = Fases.Config;
				}
				break;
			case Fases.CheckRoom:
				DelayInt = 0;
				if (_Srv_Commad == Srv_Command.Null)
				{
					Srv_Get_Room();
					Status = Fases.WaitCheckRoom;
				}
				break;
			case Fases.WaitCheckRoom:
				DelayInt++;
				if (DelayInt <= 10)
				{
					break;
				}
				if (!string.IsNullOrEmpty(Opcions.Srv_Room))
				{
					if (Opcions.News != 1)
					{
						Status = Fases.CheckDevice;
					}
					else
					{
						Status = Fases.StartUp;
					}
				}
				else
				{
					Status = Fases.Reset;
				}
				break;
			case Fases.Register:
			{
				tmp_user = "";
				tmp_pw = "";
				tmp_w = Opcions.Srv_Ip;
				Configurar_Splash(2);
				DLG_New_User dLG_New_User = new DLG_New_User(ref Opcions);
				dLG_New_User.Update_Info(tmp_user, tmp_pw, tmp_w);
				dLG_New_User.Focus();
				dLG_New_User.ShowDialog();
				tmp_user = (Opcions.Srv_User = dLG_New_User.User);
				tmp_pw = (Opcions.Srv_User_P = dLG_New_User.Password);
				tmp_w = (Opcions.Srv_Ip = dLG_New_User.Web);
				DelayInt = 0;
				Srv_Connect();
				Srv_Test_Login(tmp_user, tmp_pw, 0);
				Status = Fases.WaitRegister;
				break;
			}
			case Fases.WaitRegister:
				DelayInt++;
				if (DelayInt > 20)
				{
					int dev_Bank = _Hook_Srv_Login.OK;
					if (dev_Bank == -2)
					{
						if (_Hook_Srv_Login.login == 1)
						{
							Ticket_Add_Credits = _Hook_Srv_Login.ticket;
							Opcions.Srv_User = tmp_user;
							Opcions.Srv_User_P = tmp_pw;
							Opcions.Save_Net();
							Status = Fases.Reset;
						}
						else
						{
							Status = Fases.Register;
						}
					}
				}
				if (DelayInt > 50)
				{
					Status = Fases.Register;
				}
				break;
			case Fases.Calibrar:
				DelayInt = 0;
				Opcions.Enable_Lectors = -1;
				Close_Devices();
				if (_DCalibrar == null)
				{
					_DCalibrar = new DLG_Calibrar(ref Opcions, 1);
				}
				else if (_DCalibrar.IsDisposed)
				{
					_DCalibrar = null;
					_DCalibrar = new DLG_Calibrar(ref Opcions, 1);
				}
				_DCalibrar.Show();
				Status = Fases.WaitCalibrar;
				break;
			case Fases.WaitCalibrar:
				if (Opcions.RunConfig)
				{
					if (_DCalibrar != null)
					{
						_DCalibrar.Close();
						_DCalibrar.Dispose();
						_DCalibrar = null;
					}
					Status = Fases.Reset;
				}
				if (_DCalibrar != null)
				{
					if (_DCalibrar.IsDisposed)
					{
						_DCalibrar = null;
						Status = Fases.Reset;
					}
				}
				else
				{
					Status = Fases.Reset;
				}
				break;
			case Fases.CheckDevice:
				if (Opcions.News == 2)
				{
					ForceCheck = false;
					Status = Fases.StartUp;
				}
				else if (Opcions.Dev_BNV == "?" || Opcions.Dev_BNV_P == "?" || ForceCheck)
				{
					ssp = new Control_NV_SSP();
					ssp.respuesta = false;
					ssp.Start_Find_Device();
					DelayInt = 0;
					Status = Fases.WaitCheckDevice;
				}
				else
				{
					Status = Fases.StartUp;
				}
				break;
			case Fases.WaitCheckDevice:
				DelayInt++;
				if (ssp == null)
				{
					if (DelayInt > 10)
					{
						if (Opcions.News == 2)
						{
							ForceCheck = false;
							Status = Fases.StartUp;
						}
						else
						{
							ErrorDevices = 800;
							OutOfService();
						}
					}
					break;
				}
				ssp.Poll();
				if (ssp.respuesta)
				{
					ForceCheck = false;
					ssp.Stop_Find_Device();
					ssp.Close();
					ssp = null;
					Status = Fases.StartUp;
				}
				else
				{
					if (DelayInt <= 10)
					{
						break;
					}
					if (ssp.Last_Find_Device())
					{
						if (ssp.Poll_Find_Device())
						{
							DelayInt = 0;
						}
						break;
					}
					ssp.Stop_Find_Device();
					ssp.Close();
					ssp = null;
					if (Opcions.News == 2)
					{
						ForceCheck = false;
						Status = Fases.StartUp;
					}
					else
					{
						ErrorDevices = 800;
						OutOfService();
					}
				}
				break;
			case Fases.Config:
			{
				Opcions.Running = false;
				timerStartup.Enabled = false;
				Opcions.Enable_Lectors = -1;
				Close_Devices();
				Opcions.ForceAllKey = 1;
				ForceCheck = false;
				ReInstall_Keyboard_Driver();
				LoginAdmin = new DLG_Login(ref Opcions, 0);
				SetFocusKiosk();
				SoundAlarm();
				Application.DoEvents();
				if (!Opcions.ForceGoConfig)
				{
					LoginAdmin.Focus();
					LoginAdmin.ShowDialog();
				}
				else
				{
					LoginAdmin.Logeado = 1;
				}
				Opcions.ForceAllKey = 0;
				int dev_Bank = LoginAdmin.Logeado;
				if (dev_Bank == 1)
				{
					Configuracion.Access_Log("Andmin access");
					if (snd_alarma != null)
					{
						SoundRestore();
						snd_alarma.Stop();
					}
					snd_alarma = null;
					ErrorJamp = 0;
					Configurar_Splash(2);
					Opcions.ForceAllKey = 1;
					Opcions.Emu_Mouse_RClick = true;
					ConfigAdmin = new DLG_Config(ref Opcions);
					ConfigAdmin.MWin = this;
					ConfigAdmin.Focus();
					ConfigAdmin.ShowDialog();
					Opcions.Emu_Mouse_RClick = false;
					Opcions.ForceAllKey = 0;
					Focus();
					Render_Bar_Menu();
				}
				else
				{
					Configuracion.Access_Log("Andmin access fail");
				}
				Opcions.ForceGoConfig = false;
				Status = Fases.WaitConfig;
				break;
			}
			case Fases.WaitConfig:
				Estat_Servidor_VAR();
				Opcions.RunConfig = false;
				Status = Fases.Reset;
				break;
			case Fases.StartUp:
				AdminEnabled = false;
				navegador.Focus();
				ErrorNetServer = 0;
				_Srv_Commad = Srv_Command.Null;
				Srv_Connect();
				timerPoll.Enabled = true;
				cnttimerCredits = 11;
				DelayInt = 0;
				Opcions.Error_Billetero = 0;
				if (Opcions.News != 1)
				{
					if (Opcions.NoCreditsInGame == 1)
					{
						if (!Opcions.StopCredits)
						{
							Opcions.Enable_Lectors = 0;
						}
					}
					else
					{
						Opcions.Enable_Lectors = 0;
					}
				}
				else
				{
					Opcions.Enable_Lectors = -1;
				}
				if (Opcions.Credits > 0m)
				{
					Opcions.Temps = 120;
				}
				Status = Fases.WaitStartUp;
				break;
			case Fases.WaitStartUp:
			{
				Estat_Servidor_VAR();
				DelayInt++;
				if (DelayInt <= 10)
				{
					break;
				}
				pScreenSaver.Visible = false;
				lCALL.Visible = false;
				Opcions.Running = true;
				Status = Fases.GoHome;
				if (Opcions.ModoKiosk == 0)
				{
					Opcions.FullScreen = 1;
					Opcions.Show_Browser = false;
					navegador.Visible = false;
					Stop_Temps();
					Status = Fases.GoNavigate;
					if (Opcions.__ModoTablet == 0)
					{
						CloseOSK();
					}
					else if (Opcions.ModoKiosk != 0)
					{
						Tablet();
					}
					else
					{
						CloseOSK();
					}
					if (Opcions.CursorOn == 0)
					{
						ShowCursor(bShow: false);
						ShowCursor(bShow: false);
						ShowCursor(bShow: false);
						ShowCursor(bShow: false);
					}
					timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
					if (Opcions.ModoTickets == 1)
					{
						if (Opcions.News != 1)
						{
							if (Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_A) || Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_B))
							{
								bool flag = true;
								WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/MenuGame.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",MenuGame.aspx," + timeSpan.TotalMilliseconds;
							}
							else
							{
								bool flag = true;
								WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/MenuGame.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",MenuGame.aspx," + timeSpan.TotalMilliseconds;
							}
						}
						else
						{
							bool flag = true;
							WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",Default.aspx," + timeSpan.TotalMilliseconds;
						}
					}
					else if (Opcions.News != 1)
					{
						bool flag = true;
						WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/MenuGame.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",MenuGame.aspx," + timeSpan.TotalMilliseconds;
					}
					else
					{
						bool flag = true;
						WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page + "?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",Default.aspx," + timeSpan.TotalMilliseconds;
					}
					base.FormBorderStyle = FormBorderStyle.None;
					base.ControlBox = false;
					base.Bounds = Screen.PrimaryScreen.Bounds;
					pMenu.Visible = false;
				}
				if (Opcions.Monitors <= 1)
				{
					break;
				}
				Screen[] allScreens = Screen.AllScreens;
				if (Opcions.Monitors > 1 && allScreens.Length > 1)
				{
					if (publi != null)
					{
						publi.Close();
						publi.Dispose();
						publi = null;
					}
					publi = new Publicitat(ref Opcions);
					allScreens = Screen.AllScreens;
					Rectangle bounds = Screen.PrimaryScreen.Bounds;
					Rectangle bounds2 = allScreens[1].Bounds;
					if (bounds2.X <= 0)
					{
						bounds2 = allScreens[0].Bounds;
					}
					publi.Show();
					publi.SetBounds(bounds2.X, bounds2.Y, bounds2.Width, bounds2.Height);
					publi.Update();
					publi.Reload();
					publi.Play();
				}
				break;
			}
			case Fases.LockUser:
				Opcions.Enable_Lectors = -1;
				Close_Devices();
				Stop_Temps();
				Configurar_Splash(10);
				LastErrorNet = -1;
				Opcions.RunConfig = false;
				AdminEnabled = true;
				Status = Fases.WaitLockUser;
				break;
			case Fases.GoUpdate:
				Control_Getton_Reset();
				Opcions.Enable_Lectors = -1;
				Close_Devices();
				Stop_Temps();
				Configurar_Splash(102);
				LastErrorNet = -1;
				Opcions.RunConfig = false;
				AdminEnabled = true;
				Status = Fases.Update;
				DelayInt = 0;
				break;
			case Fases.Update:
				DelayInt++;
				if (DelayInt == 5)
				{
					bool flag = true;
					Close();
				}
				break;
			case Fases.GoRemoteReset:
				Control_Getton_Reset();
				Opcions.Enable_Lectors = -1;
				Close_Devices();
				Stop_Temps();
				Configurar_Splash(101);
				LastErrorNet = -1;
				Opcions.RunConfig = false;
				AdminEnabled = true;
				Status = Fases.RemoteReset;
				DelayInt = 0;
				break;
			case Fases.RemoteReset:
				DelayInt++;
				if (DelayInt == 10)
				{
					bool flag = true;
					Process.Start("shutdown.exe", "/r /t 2");
				}
				break;
			case Fases.GoManteniment:
				Control_Getton_Reset();
				Opcions.Enable_Lectors = -1;
				Close_Devices();
				Stop_Temps();
				Configurar_Splash(100);
				LastErrorNet = -1;
				Opcions.RunConfig = false;
				AdminEnabled = true;
				Status = Fases.Manteniment;
				DelayInt = 0;
				break;
			case Fases.Manteniment:
				Estat_Servidor_VAR();
				DelayInt++;
				if (Opcions.RunConfig)
				{
					Status = Fases.Config;
				}
				break;
			case Fases.OutOfService:
				if (Opcions.__ModoTablet == 1 && Opcions.ModoKiosk != 0)
				{
					Tablet();
				}
				Opcions.Enable_Lectors = -1;
				Close_Devices();
				Check_Connection();
				Stop_Temps();
				Configurar_Splash(1);
				LastErrorNet = -1;
				Opcions.RunConfig = false;
				AdminEnabled = true;
				DelayInt = 0;
				Status = Fases.WaitOutOfService;
				break;
			case Fases.WaitOutOfService:
				Estat_Servidor_VAR();
				if (ErrorJamp == 1)
				{
					if (Opcions.RunConfig)
					{
						Status = Fases.Config;
					}
					break;
				}
				if (ErrorNet + ErrorNetServer + ErrorDevices != LastErrorNet)
				{
					LastErrorNet = ErrorNet + ErrorNetServer + ErrorDevices;
					Configurar_Splash(1);
				}
				if (IsNetworkAvailable())
				{
					if (PingTest())
					{
						if (ErrorNetServer == 0)
						{
							if (ErrorDevices == 0 && ErrorEnLogin != 1)
							{
								Status = Fases.Reset;
							}
							ErrorNet = 0;
						}
						else if (ErrorEnLogin != 1)
						{
							Status = Fases.Reset;
						}
					}
					else
					{
						ErrorNet = 101;
					}
				}
				else
				{
					ErrorNet = 100;
				}
				if (Opcions.RunConfig)
				{
					Status = Fases.Config;
				}
				DelayInt++;
				if (Error_Servidor == 0 && ErrorNet == 0 && DelayInt > 300 && ErrorEnLogin != 1 && ErrorJamp != 1)
				{
					Status = Fases.Reset;
				}
				if (DelayInt > DelayServer)
				{
					Status = Fases.Reset;
					doReset++;
				}
				break;
			case Fases.GoLogin:
				Control_Getton_Reset();
				if (!Opcions.Logged)
				{
					if (login_dlg == null)
					{
						login_dlg = new DLG_Registro(ref Opcions);
					}
					login_dlg.Login = -1;
					Status = Fases.Login;
					login_dlg.ShowDialog();
				}
				else
				{
					Status = Fases.GoNavigate;
					login_dlg = null;
				}
				break;
			case Fases.Login:
				if (login_dlg == null)
				{
					login_dlg = new DLG_Registro(ref Opcions);
					login_dlg.Login = -1;
					login_dlg.ShowDialog();
				}
				if (login_dlg.Login >= 0)
				{
					if (login_dlg.Login == 0)
					{
						Opcions.Running = true;
						Status = Fases.GoHome;
						login_dlg = null;
					}
					else
					{
						Start_Temps();
						Status = Fases.GoNavigate;
						login_dlg = null;
						WebLink = "https://www.google.com";
					}
				}
				break;
			case Fases.GoNavigate:
				timerMessages.Enabled = true;
				DelayInt = 0;
				TimeoutPubli = 0;
				TimeoutCredits = 0;
				Status = Fases.Navigate;
				if (Opcions.ModoKiosk == 0)
				{
					MenuGames = 1;
				}
				if (!Detectar_Zona_Temps())
				{
					Hide_Browser_Nav();
				}
				else if (Opcions.ModoKiosk == 1)
				{
					if (Opcions.Temps > 0)
					{
						Show_Browser_Nav();
						Start_Temps();
						if (Banner_On == 1)
						{
							Banner_On = 2;
							Opcions.CancelTemps = 0;
							EntraJocs.Visible = true;
						}
					}
					else
					{
						if (InsertCreditsDLG != null)
						{
							InsertCreditsDLG.Close();
							InsertCreditsDLG.Dispose();
							InsertCreditsDLG = null;
						}
						InsertCreditsDLG = new InsertCredits(ref Opcions, 0);
						InsertCreditsDLG.Show();
						Status = Fases.GoHome;
					}
				}
				if (Opcions.BrowserBarOn == 1)
				{
					Show_Browser_Nav();
				}
				break;
			case Fases.Home:
			case Fases.Navigate:
			case Fases.NavigateScreenSaver:
				Control_Getton();
				if (Opcions.ForceRefresh == 1)
				{
					navegador.Reload();
					Opcions.ForceRefresh = 0;
				}
				Opcions.Block_Remotes();
				Estat_Servidor_VAR();
				Control_Credits_Gratuits();
				DelayInt++;
				doReset = 0;
				TimeoutPubli++;
				if (Opcions.News == 1 && !Opcions.Running)
				{
					Status = Fases.Reset;
				}
				if (publi != null && TimeoutPubli > 300)
				{
					TimeoutPubli = 0;
					publi.Next();
				}
				TimeoutCredits++;
				TimeoutHome++;
				if (Opcions.Credits <= 0m)
				{
					if (Opcions.InGame)
					{
						if (TimeoutCredits > Opcions.TimeoutCredits * 10 && Opcions.ModoKiosk == 1)
						{
							Status = Fases.GoHome;
							TimeoutHome = 0;
							TimeoutCredits = 0;
						}
					}
					else
					{
						TimeoutCredits = 0;
						if (TimeoutHome > 3000)
						{
							if (Opcions.ModoKiosk != 0)
							{
								Status = Fases.GoHome;
							}
							TimeoutHome = 0;
						}
					}
				}
				else
				{
					TimeoutHome = 0;
					TimeoutCredits = 0;
				}
				Check_Connection_WD();
				break;
			case Fases.GoHome:
				TimeoutHome = 0;
				timerMessages.Enabled = true;
				Opcions.ForceGoConfig = false;
				CloseOSK();
				if (Opcions.CursorOn == 0)
				{
					ShowCursor(bShow: false);
					ShowCursor(bShow: false);
					ShowCursor(bShow: false);
					ShowCursor(bShow: false);
				}
				TimeoutCredits = 0;
				GoWebInicial();
				if (WebLink == "about:blank")
				{
					if (navegador.Visible)
					{
						navegador.Visible = false;
					}
				}
				else if (!navegador.Visible)
				{
					navegador.Visible = true;
				}
				Status = Fases.Home;
				if (Opcions.Temps <= 0)
				{
					Hide_Browser_Nav();
				}
				if (Opcions.BrowserBarOn == 1)
				{
					Show_Browser_Nav();
				}
				break;
			}
			_sem_timerPoll_Tick = 0;
			enpoolint = false;
		}

		private void bHome_Click(object sender, EventArgs e)
		{
			bool flag = true;
			Srv_Connect();
			Status = Fases.GoHome;
			Hide_Browser_Nav();
			Stop_Temps();
			if (Opcions.CancelTempsOn == 1 && Opcions.Credits > 0m)
			{
				flag = false;
				int value = Opcions.CancelTemps / 12 * 5;
				if (Opcions.Credits > (decimal)value)
				{
					Srv_Sub_Credits(value, 0);
				}
				else
				{
					Srv_Sub_Credits(Opcions.Credits, 0);
				}
			}
			Banner_On = 0;
			MenuGames = 0;
			Opcions.CancelTemps = 0;
			EntraJocs.Visible = false;
			if (Opcions.ModoTickets == 0 && Opcions.News == 2)
			{
				Start_Temps();
			}
		}

		private void bGo_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(tURL.Text))
			{
				if (tURL.Text.ToLower().Contains(Opcions.Srv_Ip))
				{
					Status = Fases.GoHome;
				}
				else
				{
					Status = Fases.GoNavigate;
					WebLink = tURL.Text;
				}
			}
			if (Opcions.ModoTickets == 0 && Opcions.News == 2)
			{
				Start_Temps();
			}
		}

		private void tURL_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\r' && !string.IsNullOrEmpty(tURL.Text))
			{
				e.Handled = true;
				if (tURL.Text.ToLower().Contains("file:".ToLower()) || tURL.Text.ToLower().Contains("about:".ToLower()) || tURL.Text.ToLower().Contains("ftp:".ToLower()) || tURL.Text.ToLower().Contains("ftps:".ToLower()))
				{
					Status = Fases.GoHome;
				}
				else if (Opcions.News != 1)
				{
					Status = Fases.GoNavigate;
					WebLink = tURL.Text;
				}
			}
		}

		private void Control_Credits_Gratuits()
		{
			if (Opcions.ModoPlayCreditsGratuits != 0 && old_credits_gratuits != Opcions.CreditsGratuits)
			{
				lCGRAT.Text = "Credits gratuits\r\n" + Opcions.CreditsGratuits;
				lCGRAT.Invalidate();
				old_credits_gratuits = Opcions.CreditsGratuits;
			}
		}

		private void bKeyboard_Click(object sender, EventArgs e)
		{
			if (Opcions.InGame)
			{
				return;
			}
			Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
			if (processesByName == null)
			{
				Process.Start("KVKeyboard.exe", "es");
			}
			else if (processesByName.Length >= 1)
			{
				PipeClient pipeClient = new PipeClient();
				if (pipeClient != null)
				{
					try
					{
						pipeClient.Send("QUIT", "KVKeyboard");
					}
					catch
					{
					}
				}
			}
			else
			{
				Process.Start("KVKeyboard.exe", "es");
			}
		}

		private void Tablet()
		{
			Process[] processesByName = Process.GetProcessesByName("KVKeyboard");
			if (processesByName == null)
			{
				Process.Start("KVKeyboard.exe", "es");
			}
			else if (processesByName.Length < 1)
			{
				Process.Start("KVKeyboard.exe", "es");
			}
		}

		private void MainWindow_Paint(object sender, PaintEventArgs e)
		{
		}

		private void MainWindow_SizeChanged(object sender, EventArgs e)
		{
			if (Opcions.RunConfig && pScreenSaver != null)
			{
				pScreenSaver.Top = 0;
				pScreenSaver.Left = 0;
				pScreenSaver.Height = Screen.PrimaryScreen.Bounds.Height;
				pScreenSaver.Width = Screen.PrimaryScreen.Bounds.Width;
			}
			Windows_MidaY = base.Height;
			if (!DisplayKeyboard)
			{
			}
		}

		[DllImport("user32.dll")]
		private static extern int FindWindow(string cls, string wndwText);

		[DllImport("user32.dll")]
		private static extern int ShowWindow(int hwnd, int cmd);

		[DllImport("user32.dll")]
		private static extern long SHAppBarMessage(long dword, int cmd);

		[DllImport("user32.dll")]
		private static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int vk);

		[DllImport("user32.dll")]
		private static extern int UnregisterHotKey(IntPtr hwnd, int id);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam);

		[DllImport("user32.dll")]
		public static extern short GetAsyncKeyState(int vKey);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, HookHandlerDelegate lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("WinLockDll.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CtrlAltDel_Enable_Disable(bool bEnableDisable);

		public void ForceModoWindows()
		{
			UnLock_Windows();
			bool flag = true;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", writable: true);
				registryKey.SetValue("Shell", "explorer.exe", RegistryValueKind.String);
				registryKey.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon", writable: true);
				registryKey.SetValue("Shell", "explorer.exe", RegistryValueKind.String);
				registryKey.Close();
			}
			catch
			{
			}
			Configuracion.WinReset();
		}

		public void Keyboard_Hook()
		{
			if (!_khook)
			{
				proc = Int_Keyboard;
				using (Process process = Process.GetCurrentProcess())
				{
					using (ProcessModule processModule = process.MainModule)
					{
						hookID = SetWindowsHookEx(13, proc, GetModuleHandle(processModule.ModuleName), 0u);
					}
				}
			}
			_khook = true;
		}

		public void Keyboard_Restore()
		{
			if (_khook)
			{
				UnhookWindowsHookEx(hookID);
			}
			_khook = false;
		}

		private void System_ShutDown()
		{
			timerCredits.Enabled = false;
			timerPoll.Enabled = false;
			Environment.Exit(0);
		}

		public void Normal_Screen()
		{
			if (Opcions.News != 1)
			{
				pMenu.Visible = true;
			}
			else
			{
				pMenu.Visible = false;
			}
		}

		private IntPtr Int_Keyboard(int nCode, IntPtr wParam, ref KBDLLHOOKSTRUCT lParam)
		{
			bool flag = false;
			switch ((int)wParam)
			{
			case 256:
			case 257:
			case 260:
			case 261:
				if ((lParam.vkCode == 9 && lParam.flags == 32) || (lParam.vkCode == 27 && lParam.flags == 32) || (lParam.vkCode == 27 && lParam.flags == 0) || (lParam.vkCode == 91 && lParam.flags == 1) || (lParam.vkCode == 92 && lParam.flags == 1) || lParam.flags == 32)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				return (IntPtr)1;
			}
			return CallNextHookEx(hookID, nCode, wParam, ref lParam);
		}

		private void RegisterGlobalHotKey(Keys hotkey, int modifiers)
		{
			try
			{
				mHotKeyId++;
				if (mHotKeyId > 0 && RegisterHotKey(base.Handle, mHotKeyId, modifiers, Convert.ToInt16(hotkey)) == 0)
				{
					MessageBox.Show(mHotKeyId.ToString());
				}
			}
			catch
			{
				UnregisterGlobalHotKey();
			}
		}

		private void UnregisterGlobalHotKey()
		{
			for (int i = 0; i < mHotKeyId; i++)
			{
				UnregisterHotKey(base.Handle, i);
			}
		}

		[DllImport("user32.dll")]
		public static extern int PeekMessage(out Message lpMsg, IntPtr window, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

		[DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, int Flags);

		private bool USB_DEVICE(Message m)
		{
			return false;
		}

		private void OnDeviceChange(Message m)
		{
			if (m.WParam.ToInt32() != 32768 && m.WParam.ToInt32() != 32772)
			{
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 536)
			{
				int num = (int)m.WParam;
				if (num == 7)
				{
					if (Opcions.Credits <= 0m)
					{
						Status = Fases.GoRemoteReset;
					}
					else if (!Opcions.InGame)
					{
						Status = Fases.GoRemoteReset;
					}
				}
			}
			if (m.Msg == 537)
			{
				DBT dBT = (DBT)(int)m.WParam;
				int num2 = (!(m.LParam == IntPtr.Zero)) ? Marshal.ReadInt32(m.LParam, 4) : 0;
				DBT dBT2 = dBT;
				if (dBT2 == DBT.DBT_DEVICEARRIVAL)
				{
					switch (num2)
					{
					case 5:
					{
						DEV_BROADCAST_DEVICEINTERFACE_1 dEV_BROADCAST_DEVICEINTERFACE_ = (DEV_BROADCAST_DEVICEINTERFACE_1)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_DEVICEINTERFACE_1));
						string arg = "";
						int num3 = 0;
						while (dEV_BROADCAST_DEVICEINTERFACE_.dbcc_name[num3] != 0)
						{
							arg += dEV_BROADCAST_DEVICEINTERFACE_.dbcc_name[num3++];
						}
						break;
					}
					}
				}
			}
			base.WndProc(ref m);
			if (m.Msg != 786)
			{
			}
		}

		private void Modo_Kiosk_Off()
		{
			bool flag = false;
			ShowWindow(FindWindow("Shell_TrayWnd", null), 1);
			CtrlAltDel_Enable_Disable(bEnableDisable: true);
		}

		private void Modo_Kiosk_On()
		{
			bool flag = true;
			CtrlAltDel_Enable_Disable(bEnableDisable: false);
			Block_Accessibility();
		}

		[DllImport("user32")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32")]
		private static extern int ShowWindow(IntPtr hWnd, int swCommand);

		[DllImport("user32")]
		private static extern bool IsIconic(IntPtr hWnd);

		public static void SetFocusKiosk()
		{
			IntPtr mainWindowHandle = Process.GetCurrentProcess().MainWindowHandle;
			if (IsIconic(mainWindowHandle))
			{
				ShowWindow(mainWindowHandle, 9);
			}
			SetForegroundWindow(mainWindowHandle);
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{
			bool flag = false;
			SetFocusKiosk();
			timerPoll.Enabled = true;
			if (Opcions.ForceLogin == 1)
			{
				bGo.Visible = true;
			}
			else
			{
				bGo.Visible = false;
			}
		}

		private void lcdClock_Click(object sender, EventArgs e)
		{
		}

		public void Start_Temps()
		{
			if (!Detectar_Zona_Temps())
			{
				Stop_Temps();
				return;
			}
			Opcions.TimeNavigate = true;
			if (Opcions.ModoTickets != 0)
			{
				Banner_On = 1;
			}
			Opcions.CancelTemps = 0;
		}

		public void Stop_Temps()
		{
			Opcions.TimeNavigate = false;
			cnttime = 0;
			Banner_On = 0;
		}

		public void Config_Temps()
		{
		}

		public void Show_Navegador()
		{
			Opcions.Show_Browser = true;
		}

		private void Srv_Connect()
		{
			ErrorNetServer = 0;
			if (_netConnection == null)
			{
				if (Status != Fases.OutOfService && !Opcions.RunConfig)
				{
					ErrorNetServer = 501;
					OutOfService();
				}
			}
			else
			{
				if (_netConnection.Connected)
				{
					return;
				}
				bool flag;
				if (Opcions.News != 1)
				{
					flag = true;
					Start_Service("http://" + Opcions.Srv_Web_Ip + "/DemoQuiosk.aspx");
				}
				else
				{
					flag = true;
					Start_Service("http://" + Opcions.Srv_Web_Ip + "/" + Opcions.Srv_Web_Page);
				}
				flag = true;
				string command = "rtmp://" + Opcions.Srv_Ip + ":" + Convert.ToInt32(Opcions.Srv_port) + "/" + Opcions.Srv_Rtm;
				if (Opcions.Srv_User == "-")
				{
					ErrorEnLogin = 1;
					LastErrorNet = ErrorNet + ErrorNetServer + ErrorDevices;
					Configurar_Splash(1);
					return;
				}
				try
				{
					_netConnection.Connect(command, "QuiosK|" + Opcions.Srv_User, Opcions.Srv_User_P);
				}
				catch (Exception ex)
				{
					ErrorGenericText = ex.Message;
					if (Status != Fases.OutOfService && !Opcions.RunConfig)
					{
						ErrorNetServer = 502;
						OutOfService();
					}
					return;
				}
				DateTime t = DateTime.Now.AddSeconds(30.0);
				while (true)
				{
					Application.DoEvents();
					if (DateTime.Now > t)
					{
						break;
					}
					if (_netConnection.Connected)
					{
						return;
					}
				}
				if (Status != Fases.OutOfService && !Opcions.RunConfig)
				{
					ErrorNetServer = 503;
					if (!_netConnection.Connected)
					{
						OutOfService();
					}
				}
			}
		}

		public void ResultReceived(IPendingServiceCall call)
		{
			object result = call.Result;
			if (_Srv_Commad == Srv_Command.Credits)
			{
				try
				{
					decimal d = Convert.ToDecimal(result);
					if (d < 0m)
					{
						d = 0m;
					}
					errorcreditsserv = 0;
				}
				catch
				{
					errorcreditsserv = 1;
				}
			}
			if (_Srv_Commad == Srv_Command.SubCredits)
			{
				try
				{
					if (Convert.ToBoolean(result))
					{
						Opcions.Sub_Credits = 0m;
					}
					else
					{
						Opcions.Sub_Credits = 0m;
					}
				}
				catch
				{
				}
			}
			if (_Srv_Commad == Srv_Command.Room)
			{
				try
				{
					_Srv_Commad = Srv_Command.Null;
					int num = Convert.ToInt32(result);
					Opcions.Srv_Room = string.Concat(num);
					Connect_Room();
				}
				catch
				{
				}
			}
			_Srv_Commad = Srv_Command.Null;
		}

		private void Reconnect_ShareObject()
		{
			if (_netConnection != null && _netConnection.Connected)
			{
				if (_sharedObject == null)
				{
					delayreconnect = 0;
					_sharedObject = RemoteSharedObject.GetRemote("QuioskOn" + Opcions.Srv_User, _netConnection.Uri.ToString(), false);
					_sharedObject.OnConnect += _sharedObject_OnConnect;
					_sharedObject.OnDisconnect += _sharedObject_OnDisconnect;
					_sharedObject.NetStatus += _sharedObject_NetStatus;
					_sharedObject.Sync += _sharedObject_Sync;
					_sharedObject.Connect(_netConnection);
				}
				Connect_Room();
			}
		}

		private void _netConnection_OnConnect(object sender, EventArgs e)
		{
			if (_netConnection != null && _netConnection.Connected)
			{
				if (_sharedObject == null)
				{
					_sharedObject = RemoteSharedObject.GetRemote("QuioskOn" + Opcions.Srv_User, _netConnection.Uri.ToString(), false);
					_sharedObject.OnConnect += _sharedObject_OnConnect;
					_sharedObject.OnDisconnect += _sharedObject_OnDisconnect;
					_sharedObject.NetStatus += _sharedObject_NetStatus;
					_sharedObject.Sync += _sharedObject_Sync;
					_sharedObject.Connect(_netConnection);
				}
				Connect_Room();
			}
			errorcreditsserv = 0;
			cnterror = 0;
			ErrorNetServer = 0;
		}

		private void Connect_Room()
		{
			if (!string.IsNullOrEmpty(Opcions.Srv_Room) && _sharedObjectPagos == null)
			{
				_sharedObjectPagos = RemoteSharedObject.GetRemote(typeof(UsersRSO), "PagosSala" + Opcions.Srv_Room, _netConnection.Uri.ToString(), true);
				_sharedObjectPagos.OnConnect += _sharedObjectPagos_OnConnect;
				_sharedObjectPagos.OnDisconnect += _sharedObjectPagos_OnDisconnect;
				_sharedObjectPagos.NetStatus += _sharedObjectPagos_NetStatus;
				_sharedObjectPagos.Sync += _sharedObjectPagos_Sync;
				_sharedObjectPagos.Connect(_netConnection);
			}
		}

		private void _netConnection_OnDisconnect(object sender, EventArgs e)
		{
			errorcreditsserv = 1;
			if (_sharedObject != null)
			{
				try
				{
					_sharedObject.Close();
					_sharedObject.Dispose();
				}
				catch
				{
				}
				_sharedObject = null;
			}
			if (_sharedObjectPagos != null)
			{
				try
				{
					_sharedObjectPagos.Close();
					_sharedObjectPagos.Dispose();
				}
				catch
				{
				}
				_sharedObjectPagos = null;
			}
			cnterror++;
			if (cnterror > 5 && Status != Fases.OutOfService)
			{
				if (Status != Fases.WaitOutOfService && !Opcions.RunConfig)
				{
					ErrorNetServer = 504;
					ErrorEnLogin = 1;
					OutOfService();
				}
				cnterror = 0;
			}
		}

		private void _netConnection_NetStatus(object sender, NetStatusEventArgs e)
		{
			string a = e.Info["level"] as string;
			if (a == "error")
			{
			}
			if (a == "status")
			{
			}
			bool flag = true;
		}

		private void _sharedObject_OnConnect(object sender, EventArgs e)
		{
			errorcreditsserv = 0;
		}

		private void _sharedObject_OnDisconnect(object sender, EventArgs e)
		{
			errorcreditsserv = 1;
		}

		private void _sharedObjectPagos_OnConnect(object sender, EventArgs e)
		{
		}

		private void _sharedObjectPagos_OnDisconnect(object sender, EventArgs e)
		{
		}

		private void _sharedObjectPagos_NetStatus(object sender, NetStatusEventArgs e)
		{
			string a = e.Info["level"] as string;
			if (a == "error")
			{
			}
			if (!(a == "status"))
			{
			}
		}

		private int Gestio_Pagament(int _pag, int _tick = 0)
		{
			if (_pag <= 0 && _tick == 0)
			{
				return 100;
			}
			if (Opcions.Disp_Enable == 1 && _tick == 0)
			{
				if (!Check_Printer_Ready(Opcions.Impresora_Tck))
				{
					return 1;
				}
				int num = Opcions.Disp_Val;
				int num2 = 0;
				if (Opcions.Disp_Min * Opcions.Disp_Val > num)
				{
					num = Opcions.Disp_Min * Opcions.Disp_Val;
				}
				if (Opcions.Disp_Max * Opcions.Disp_Val > num2)
				{
					num2 = Opcions.Disp_Max * Opcions.Disp_Val;
				}
				if (Opcions.Disp_Min > 0 && _pag < num)
				{
					return 101;
				}
				int num3 = num2 / Opcions.Disp_Val;
				int num4 = _pag / Opcions.Disp_Val;
				if (Opcions.Disp_Max > 0 && num4 > num3)
				{
					num4 = num3;
				}
				Opcions.Disp_Pay_Ticket_Credits = _pag;
				Opcions.Disp_Pay_Ticket = num4;
				Opcions.Disp_Pay_Ticket_Out = 0;
				Opcions.Disp_Pay_Ticket_Cnt_Fail = 0;
				Opcions.Disp_Pay_Ticket_Fail = 0;
				Opcions.Disp_Pay_Ticket_Out_Flag = 0;
				Opcions.Disp_Pay_Running = 1;
				return 1;
			}
			if (Check_Printer_Ready(Opcions.Impresora_Tck))
			{
				if (Opcions.Pagar_Ticket_Busy != 1)
				{
					Opcions.Pagar_Ticket_Val = _pag;
					Opcions.Pagar_Ticket_Busy = 1;
					Srv_Add_Pay(0);
					return 0;
				}
				return 200;
			}
			return 201;
		}

		private void _sharedObjectPagos_Sync(object sender, SyncEventArgs e)
		{
			if (Opcions.ModoTickets == 0 || _sharedObjectPagos == null || !_sharedObjectPagos.Connected)
			{
				return;
			}
			ICollection attributeNames = _sharedObjectPagos.GetAttributeNames();
			string text = (string)_sharedObjectPagos.GetAttribute(Opcions.Srv_User);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string[] array = text.Split('|');
			if (array.Length >= 2)
			{
				if (array[0] == "true")
				{
					int num = 1;
					if (Opcions.ModoPlayCreditsGratuits == 1 && Opcions.CreditsGratuits > 0m)
					{
						Opcions.MissatgeCreditsGratuits = 1;
						num = 0;
					}
					if (num == 1)
					{
						int pag = 0;
						try
						{
							pag = int.Parse(array[1]);
						}
						catch
						{
						}
						if (Gestio_Pagament(pag) == 1)
						{
							num = 0;
						}
					}
				}
				else
				{
					Opcions.Pagar_Ticket_Busy = 0;
				}
			}
			else
			{
				Opcions.Pagar_Ticket_Busy = 0;
			}
		}

		public void Update_Server_Credits(decimal _crd, bool _dis = true)
		{
			Opcions.Credits = _crd;
			if (Opcions.ModoTickets == 1)
			{
			}
			if (Opcions.ModoTickets == 0 && Opcions.News == 2)
			{
				int num = (int)Opcions.Credits / Opcions.ValorTemps;
				Opcions.Temps = 60 * num;
			}
			if (Opcions.NoCreditsInGame == 1)
			{
				if (Opcions.Credits > 0m)
				{
					if (StartupDetect == 1 || _dis)
					{
						Opcions.Enable_Lectors = -1;
						oldStopCredits = false;
						Opcions.StopCredits = true;
					}
				}
				else
				{
					Opcions.StopCredits = false;
				}
			}
			StartupDetect = 0;
		}

		private void _sharedObject_Sync(object sender, SyncEventArgs e)
		{
			ASObject[] changeList = e.ChangeList;
			foreach (ASObject aSObject in changeList)
			{
				foreach (KeyValuePair<string, object> item in aSObject)
				{
					if (item.Key == "name")
					{
						if ((string)item.Value == "RemoteCmd")
						{
							try
							{
								Opcions.RemoteCmd = (string)_sharedObject.GetAttribute("RemoteCmd");
							}
							catch
							{
							}
						}
						if ((string)item.Value == "RemoteParam")
						{
							try
							{
								Opcions.RemoteParam = (string)_sharedObject.GetAttribute("RemoteParam");
							}
							catch
							{
							}
						}
						if ((string)item.Value == "saldo_cuenta")
						{
							try
							{
								double num = (double)_sharedObject.GetAttribute("saldo_cuenta");
								Opcions.SaldoCredits = (decimal)num;
								if (Opcions.LockCredits == 0)
								{
									if (Opcions.NoCreditsInGame == 1)
									{
										if (Opcions.InGame)
										{
											oldStopCredits = false;
											Opcions.StopCredits = true;
										}
										else if (Opcions.Credits > 0m)
										{
											oldStopCredits = false;
											Opcions.StopCredits = true;
										}
										else
										{
											Opcions.StopCredits = false;
										}
									}
									else
									{
										Opcions.StopCredits = false;
									}
								}
								else if (Opcions.NoCreditsInGame == 1)
								{
									if (Opcions.InGame)
									{
										oldStopCredits = false;
										Opcions.StopCredits = true;
									}
									else if (Opcions.SaldoCredits <= 0m)
									{
										oldStopCredits = false;
										Opcions.StopCredits = true;
									}
									else if (Opcions.Credits > 0m)
									{
										oldStopCredits = false;
										Opcions.StopCredits = true;
									}
									else
									{
										Opcions.StopCredits = false;
									}
								}
								else if (Opcions.SaldoCredits <= 0m)
								{
									Opcions.StopCredits = true;
								}
								else
								{
									Opcions.StopCredits = false;
								}
							}
							catch
							{
							}
						}
						if ((string)item.Value == "CreditBono")
						{
							try
							{
								double num2 = (double)_sharedObject.GetAttribute("CreditBono");
								Opcions.CreditsGratuits = (decimal)num2;
							}
							catch
							{
							}
							if (Opcions.NoCreditsInGame == 1 && !(Opcions.Credits > 0m))
							{
								Opcions.StopCredits = false;
							}
						}
						if ((string)item.Value == "Credit")
						{
							try
							{
								double num2 = (double)_sharedObject.GetAttribute("Credit");
								Update_Server_Credits((decimal)num2, _dis: false);
							}
							catch
							{
							}
						}
						if ((string)item.Value == "Jogant")
						{
							try
							{
								bool inGame = (bool)_sharedObject.GetAttribute("Jogant");
								Opcions.InGame = inGame;
							}
							catch
							{
							}
						}
					}
					if (item.Key == "code")
					{
						if (item.Value == "clear")
						{
							string text = "chg";
						}
						if (item.Value == "change")
						{
							string text = "chg";
						}
					}
				}
			}
		}

		private void _sharedObject_NetStatus(object sender, NetStatusEventArgs e)
		{
			string a = e.Info["level"] as string;
			if (a == "error")
			{
			}
			if (!(a == "status"))
			{
			}
		}

		private void Srv_Get_Room()
		{
			if (_netConnection != null)
			{
				if (!_netConnection.Connected)
				{
					Srv_Connect();
				}
				_Srv_Commad = Srv_Command.Room;
				try
				{
					_netConnection.Call("QuioskVerUserSala", this, Opcions.Srv_User);
				}
				catch
				{
				}
			}
		}

		public void Srv_KioskCommand(int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_KioskCommand == null)
			{
				_Hook_Srv_KioskCommand = new Hook_Srv_KioskCommand();
			}
			if (_Hook_Srv_KioskCommand.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad == Srv_Command.Null || _err != 0)
			{
				if (_Hook_Srv_KioskCommand.OK == 0)
				{
				}
				Error_Servidor = 0;
				_Hook_Srv_KioskCommand.OK = -1;
				_Hook_Srv_KioskCommand.timeout = 0;
				_Srv_Commad = Srv_Command.KioskCommand;
				try
				{
					_netConnection.Call("QuioskSendCMD", _Hook_Srv_KioskCommand, Opcions.Srv_User, "-", "-");
				}
				catch (Exception ex)
				{
					ErrorGenericText = ex.Message;
					_Hook_Srv_KioskCommand.OK = -1;
					Error_Servidor = 1100;
				}
			}
		}

		public void Srv_KioskSetTime(int _segons, int _err)
		{
			if (_segons <= 0 || Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_KioskSetTime == null)
			{
				_Hook_Srv_KioskSetTime = new Hook_Srv_KioskSetTime();
			}
			if (_Hook_Srv_KioskSetTime.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad == Srv_Command.Null || _err != 0)
			{
				if (_Hook_Srv_KioskSetTime.OK != 0)
				{
					_Hook_Srv_KioskSetTime.segons = _segons;
				}
				Error_Servidor = 0;
				_Hook_Srv_KioskSetTime.OK = -1;
				_Hook_Srv_KioskSetTime.timeout = 0;
				_Srv_Commad = Srv_Command.KioskSetTime;
				try
				{
					_netConnection.Call("QuioskSetTime", _Hook_Srv_KioskSetTime, Opcions.Srv_User, _segons);
				}
				catch (Exception ex)
				{
					ErrorGenericText = ex.Message;
					_Hook_Srv_KioskSetTime.OK = -1;
					Error_Servidor = 702;
				}
			}
		}

		public void Srv_KioskGetTime(int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_KioskGetTime == null)
			{
				_Hook_Srv_KioskGetTime = new Hook_Srv_KioskGetTime();
			}
			if (_Hook_Srv_KioskGetTime.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad == Srv_Command.Null || _err != 0)
			{
				if (_Hook_Srv_KioskGetTime.OK == 0)
				{
				}
				Error_Servidor = 0;
				_Hook_Srv_KioskGetTime.OK = -1;
				_Hook_Srv_KioskGetTime.timeout = 0;
				_Srv_Commad = Srv_Command.KioskGetTime;
				try
				{
					_netConnection.Call("QuioskGetTime", _Hook_Srv_KioskGetTime, Opcions.Srv_User);
				}
				catch (Exception ex)
				{
					ErrorGenericText = ex.Message;
					_Hook_Srv_KioskGetTime.OK = -1;
					Error_Servidor = 702;
				}
			}
		}

		public void Srv_Verificar_Ticket(int _tck, int _err)
		{
			if (_tck <= 0 || Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Verificar_Ticket == null)
			{
				_Hook_Srv_Verificar_Ticket = new Hook_Srv_Verificar_Ticket();
			}
			if (_Hook_Srv_Verificar_Ticket.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad == Srv_Command.Null || _err != 0)
			{
				if (_Hook_Srv_Verificar_Ticket.OK != 0)
				{
					_Hook_Srv_Verificar_Ticket.ticket = _tck;
				}
				Error_Servidor = 0;
				_Hook_Srv_Verificar_Ticket.OK = -1;
				_Hook_Srv_Verificar_Ticket.timeout = 0;
				_Srv_Commad = Srv_Command.VerificarTicket;
				try
				{
					_netConnection.Call("QuioskVerTickPago", _Hook_Srv_Verificar_Ticket, Opcions.Srv_User, _tck);
				}
				catch (Exception ex)
				{
					ErrorGenericText = ex.Message;
					_Hook_Srv_Verificar_Ticket.OK = -1;
					Error_Servidor = 702;
				}
			}
		}

		public void Srv_Anular_Ticket(int _tck, bool _est, int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Anular_Ticket == null)
			{
				_Hook_Srv_Anular_Ticket = new Hook_Srv_Anular_Ticket();
			}
			if (_Hook_Srv_Anular_Ticket.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad == Srv_Command.Null || _err != 0)
			{
				if (_Hook_Srv_Anular_Ticket.OK != 0)
				{
					_Hook_Srv_Anular_Ticket.ticket = _tck;
					_Hook_Srv_Anular_Ticket.estat = _est;
				}
				Error_Servidor = 0;
				_Hook_Srv_Anular_Ticket.OK = -1;
				_Hook_Srv_Anular_Ticket.timeout = 0;
				_Srv_Commad = Srv_Command.AnularTicket;
				try
				{
					_netConnection.Call("QuioskAnulaTickPago", _Hook_Srv_Anular_Ticket, Opcions.Srv_User, _tck, _est);
				}
				catch (Exception ex)
				{
					ErrorGenericText = ex.Message;
					_Hook_Srv_Anular_Ticket.OK = -1;
					Error_Servidor = 701;
				}
			}
		}

		private void Srv_Credits(int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Credits == null)
			{
				_Hook_Srv_Credits = new Hook_Srv_Credits();
			}
			if (_Hook_Srv_Credits.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad == Srv_Command.Null || _err != 0)
			{
				if (_Hook_Srv_Credits.OK <= 0)
				{
				}
				Error_Servidor = 0;
				_Hook_Srv_Credits.OK = -1;
				_Hook_Srv_Credits.timeout = 0;
				_Hook_Srv_Credits.credits = 0;
				_Srv_Commad = Srv_Command.Credits;
				try
				{
					_netConnection.Call("QuioskVerPasta", _Hook_Srv_Credits, Opcions.Srv_User);
				}
				catch (Exception ex)
				{
					ErrorGenericText = ex.Message;
					_Hook_Srv_Credits.OK = -1;
					Error_Servidor = 201;
				}
			}
		}

		private void Srv_Sub_Credits(decimal _crd, int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Sub_Credits == null)
			{
				_Hook_Srv_Sub_Credits = new Hook_Srv_Sub_Credits();
			}
			if (_Hook_Srv_Sub_Credits.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad != 0 && _err == 0)
			{
				return;
			}
			if (_Hook_Srv_Sub_Credits.OK != 0)
			{
				Ticket_Add_Credits++;
				if (Ticket_Add_Credits <= 0)
				{
					Ticket_Add_Credits++;
				}
				_Hook_Srv_Sub_Credits.ticket = Ticket_Add_Credits;
			}
			Error_Servidor = 0;
			_Hook_Srv_Sub_Credits.OK = -1;
			_Hook_Srv_Sub_Credits.timeout = 0;
			_Srv_Commad = Srv_Command.SubCredits;
			try
			{
				_netConnection.Call("QuioskCobrarParcialV2", _Hook_Srv_Sub_Credits, Opcions.Srv_User, (int)_crd, _Hook_Srv_Sub_Credits.ticket);
			}
			catch (Exception ex)
			{
				ErrorGenericText = ex.Message;
				_Hook_Srv_Sub_Credits.OK = -1;
				Error_Servidor = 301;
			}
		}

		private void Srv_Sub_Cadeaux(decimal _crd, int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Sub_Cadeaux == null)
			{
				_Hook_Srv_Sub_Cadeaux = new Hook_Srv_Sub_Cadeaux();
			}
			if (_Hook_Srv_Sub_Cadeaux.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad != 0 && _err == 0)
			{
				return;
			}
			if (_Hook_Srv_Sub_Cadeaux.OK != 0)
			{
				Ticket_Add_Credits++;
				if (Ticket_Add_Credits <= 0)
				{
					Ticket_Add_Credits++;
				}
				_Hook_Srv_Sub_Cadeaux.ticket = Ticket_Add_Credits;
			}
			Error_Servidor = 0;
			_Hook_Srv_Sub_Cadeaux.OK = -1;
			_Hook_Srv_Sub_Cadeaux.timeout = 0;
			_Srv_Commad = Srv_Command.SubCadeaux;
			try
			{
				_netConnection.Call("QuioskCobrarCadeau", _Hook_Srv_Sub_Cadeaux, Opcions.Srv_User, (int)_crd, _Hook_Srv_Sub_Cadeaux.ticket, Opcions.Disp_Pay_Ticket_Out_Flag);
			}
			catch (Exception ex)
			{
				ErrorGenericText = ex.Message;
				_Hook_Srv_Sub_Cadeaux.OK = -1;
				Error_Servidor = 701;
			}
		}

		private void Srv_Add_Credits(decimal _crd, int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Add_Credits == null)
			{
				_Hook_Srv_Add_Credits = new Hook_Srv_Add_Credits();
			}
			if (_Hook_Srv_Add_Credits.OK == -1)
			{
				return;
			}
			Srv_Connect();
			if (_Srv_Commad != 0 && _err == 0)
			{
				return;
			}
			if (_Hook_Srv_Add_Credits.OK != 0)
			{
				Ticket_Add_Credits++;
				if (Ticket_Add_Credits <= 1)
				{
					Ticket_Add_Credits++;
				}
				_Hook_Srv_Add_Credits.ticket = Ticket_Add_Credits;
			}
			Error_Servidor = 0;
			_Hook_Srv_Add_Credits.OK = -1;
			_Hook_Srv_Add_Credits.timeout = 0;
			_Srv_Commad = Srv_Command.AddCredits;
			try
			{
				_netConnection.Call("QuioskPasta_Dec", _Hook_Srv_Add_Credits, Opcions.Srv_User, (int)_crd, _Hook_Srv_Add_Credits.ticket);
			}
			catch (Exception ex)
			{
				ErrorGenericText = ex.Message;
				_Hook_Srv_Add_Credits.OK = -1;
				Error_Servidor = 401;
			}
		}

		private void Srv_Test_Login(string _u, string _p, int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Login == null)
			{
				_Hook_Srv_Login = new Hook_Srv_Login();
			}
			if (_Hook_Srv_Login.OK != -1)
			{
				Srv_Connect();
				Error_Servidor = 0;
				_Hook_Srv_Login.ticket = 0;
				_Hook_Srv_Login.timeout = 0;
				_Hook_Srv_Login.OK = -1;
				_Hook_Srv_Login.login = 0;
				if (_u == "-")
				{
					_Hook_Srv_Login.OK = -2;
					_Hook_Srv_Login.login = 2;
				}
				else if (_Srv_Commad == Srv_Command.Null || _err != 0)
				{
					try
					{
						bool flag = true;
						_netConnection.Call("Login_User", _Hook_Srv_Login, _u, _p);
					}
					catch (Exception ex)
					{
						bool flag = true;
						ErrorGenericText = ex.Message;
						_Hook_Srv_Login.OK = -1;
						Error_Servidor = 601;
					}
				}
			}
		}

		public void Srv_Sub_Ticket(int _id, int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Sub_Ticket == null)
			{
				_Hook_Srv_Sub_Ticket = new Hook_Srv_Sub_Ticket();
			}
			if (_Hook_Srv_Sub_Ticket.OK != -1)
			{
				Srv_Connect();
				if (_Srv_Commad == Srv_Command.Null || _err != 0)
				{
					Error_Servidor = 0;
					_Hook_Srv_Sub_Ticket.OK = -1;
					_Hook_Srv_Sub_Ticket.timeout = 0;
					_Srv_Commad = Srv_Command.SubTicket;
					try
					{
						_netConnection.Call("QuioskDelTick", _Hook_Srv_Sub_Ticket, _id);
					}
					catch (Exception ex)
					{
						ErrorGenericText = ex.Message;
						_Hook_Srv_Sub_Ticket.OK = -1;
						Error_Servidor = 301;
					}
				}
			}
		}

		private void Srv_Add_Ticket(int _crd, int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Add_Ticket == null)
			{
				_Hook_Srv_Add_Ticket = new Hook_Srv_Add_Ticket();
			}
			if (_Hook_Srv_Add_Ticket.OK != -1)
			{
				Srv_Connect();
				if (_Srv_Commad == Srv_Command.Null || _err != 0)
				{
					Error_Servidor = 0;
					_Hook_Srv_Add_Ticket.Ticket = 0;
					_Hook_Srv_Add_Ticket.OK = -1;
					_Hook_Srv_Add_Ticket.timeout = 0;
					_Srv_Commad = Srv_Command.AddTicket;
					try
					{
						if (Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_A) || Opcions.Srv_Web_Ip.ToLower().Contains(Web_V2_B))
						{
							bool flag = false;
							_netConnection.Call("QuioskNewTickV2", _Hook_Srv_Add_Ticket, Opcions.Srv_User, _crd);
						}
						else
						{
							_netConnection.Call("QuioskNewTick", _Hook_Srv_Add_Ticket, Opcions.Srv_User, _crd);
						}
					}
					catch (Exception ex)
					{
						ErrorGenericText = ex.Message;
						_Hook_Srv_Add_Ticket.OK = -1;
						Error_Servidor = 301;
					}
				}
			}
		}

		private void Srv_Add_Pay(int _err)
		{
			if (Opcions == null)
			{
				return;
			}
			if (_Hook_Srv_Add_Pay == null)
			{
				_Hook_Srv_Add_Pay = new Hook_Srv_Add_Pay();
			}
			if (_Hook_Srv_Add_Pay.OK != -1)
			{
				Srv_Connect();
				if (_Srv_Commad == Srv_Command.Null || _err != 0)
				{
					Error_Servidor = 0;
					_Hook_Srv_Add_Pay.Resposta = "";
					_Hook_Srv_Add_Pay.Pay = 0;
					_Hook_Srv_Add_Pay.OK = -1;
					_Hook_Srv_Add_Pay.timeout = 0;
					_Srv_Commad = Srv_Command.AddTicket;
					try
					{
						_netConnection.Call("QuioskCobrarV2", _Hook_Srv_Add_Pay, Opcions.Srv_User);
					}
					catch (Exception ex)
					{
						ErrorGenericText = ex.Message;
						_Hook_Srv_Add_Pay.OK = -1;
						Error_Servidor = 301;
					}
				}
			}
		}

		private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
		{
			Close_Devices();
			Opcions.Log_Debug("Shutdown");
			CtrlAltDel_Enable_Disable(bEnableDisable: true);
		}

		private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
			}
			else
			{
				bool flag = true;
			}
		}

		[DllImport("gdi32.dll", SetLastError = true)]
		private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		[DllImport("gdi32.dll")]
		private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

		[DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[DllImport("gdi32.dll")]
		private static extern bool DeleteDC(IntPtr hdc);

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

		private static BitmapData GetBitmapData(Bitmap bmpFileName)
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

		private char[] Convert_String_Char(string _s)
		{
			return _s.ToCharArray();
		}

		public void _Old_Ticket_ESCPOS(string _ptr_device, decimal _valor, int _tick, int _id, int _model, int _cut, int _skeep)
		{
			NIIClassLib nIIClassLib = new NIIClassLib();
			Barcode barcode = new Barcode();
			rtest.BackColor = Color.Yellow;
			rtest.ForeColor = Color.Yellow;
			timerPrinter.Enabled = true;
			string text = Gestion.Build_Mod10(Opcions.Srv_User, _tick, _id, 0);
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 500, 90);
			Bitmap bmpFileName = new Bitmap(original);
			BitmapData bitmapData = GetBitmapData(bmpFileName);
			BitArray dots = bitmapData.Dots;
			byte[] bytes = BitConverter.GetBytes(bitmapData.Width);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('@'));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('T'));
					binaryWriter.Write(Convert.ToChar(0));
					binaryWriter.Write('\n');
					binaryWriter.Write('\n');
					binaryWriter.Write('\n');
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar(33));
					binaryWriter.Write(Convert.ToChar(48));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar(97));
					binaryWriter.Write(Convert.ToChar(1));
					binaryWriter.Write("-- La Belle Net --".ToCharArray());
					binaryWriter.Write('\n');
					DateTime now = DateTime.Now;
					binaryWriter.Write($"{now.Day}/{now.Month:00}/{now.Year:0000} {now.Hour:00}:{now.Minute:00}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('a');
					binaryWriter.Write('\0');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write((byte)160);
					binaryWriter.Write('\n');
					binaryWriter.Write(Opcions.Localize.Text("Location:").ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('\0');
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin1}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin2}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin3}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin4}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"  RC: {Opcions.Srv_ID_Lin5}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write(string.Format("  " + Opcions.Localize.Text("Kiosk ID:") + " {0}", Opcions.Srv_User).ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('\0');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('a');
					binaryWriter.Write('\0');
					binaryWriter.Write("------------------------------------------".ToCharArray());
					binaryWriter.Write('\r');
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('0');
					binaryWriter.Write(0);
					binaryWriter.Write(string.Format(Opcions.Localize.Text("Ticket:") + " {0}", text).ToCharArray());
					binaryWriter.Write('\n');
					decimal value = _tick;
					TimeSpan timeSpan = new TimeSpan(0, 0, (int)value);
					string text2 = string.Format(Opcions.Localize.Text("Time: ") + " {0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
					if (_valor <= 0m)
					{
						text2 = "TEST TICKET";
					}
					binaryWriter.Write('\u001b');
					binaryWriter.Write('a');
					binaryWriter.Write('\u0001');
					binaryWriter.Write(text2.ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\r');
					binaryWriter.Write('\n');
					binaryWriter.Write(0);
					binaryWriter.Write('\u001b');
					binaryWriter.Write('3');
					binaryWriter.Write((byte)24);
					int num = 0;
					while (num < bitmapData.Height)
					{
						binaryWriter.Write('\u001b');
						binaryWriter.Write('*');
						binaryWriter.Write((byte)33);
						binaryWriter.Write(bytes[0]);
						binaryWriter.Write(bytes[1]);
						for (int i = 0; i < bitmapData.Width; i++)
						{
							for (int j = 0; j < 3; j++)
							{
								byte b = 0;
								for (int k = 0; k < 8; k++)
								{
									int num2 = (num / 8 + j) * 8 + k;
									int num3 = num2 * bitmapData.Width + i;
									bool flag = false;
									if (num3 < dots.Length)
									{
										flag = dots[num3];
									}
									b = (byte)(b | (byte)((flag ? 1 : 0) << 7 - k));
								}
								binaryWriter.Write(b);
							}
						}
						num += 24;
						binaryWriter.Write('\n');
					}
					binaryWriter.Write('\u001b');
					binaryWriter.Write('3');
					binaryWriter.Write((byte)30);
					binaryWriter.Write('\u001b');
					binaryWriter.Write('a');
					binaryWriter.Write('\u0001');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('0');
					binaryWriter.Write('\n');
					binaryWriter.Write(Opcions.Localize.Text("THANKS").ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('\0');
					binaryWriter.Write(Opcions.Srv_ID_LinBottom.ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write("------------------------------------------".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('a');
					binaryWriter.Write('\0');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('\0');
					for (int num3 = 0; num3 < _skeep; num3++)
					{
						binaryWriter.Write('\n');
					}
					if (_cut == 1)
					{
						binaryWriter.Write((byte)27);
						binaryWriter.Write((byte)74);
						binaryWriter.Write((byte)120);
						binaryWriter.Write((byte)27);
						binaryWriter.Write((byte)105);
					}
					binaryWriter.Write('\u001d');
					binaryWriter.Write('V');
					binaryWriter.Write((byte)66);
					binaryWriter.Write((byte)3);
					binaryWriter.Flush();
					Print_ESCPOS(_ptr_device, memoryStream.ToArray());
				}
			}
		}

		private void PrinterErrorShow(int _err, string _cod = "")
		{
			Opcions.MissatgePrinter = "";
			switch (_err)
			{
			case 0:
				Opcions.MissatgePrinter = Opcions.Localize.Text("ERROR: PRINTER NOT INSTALLED");
				break;
			case 1:
				Opcions.MissatgePrinter = Opcions.Localize.Text("ERROR: PRINTER OFFLINE OR NOT PRESENT");
				break;
			case 2:
				Opcions.MissatgePrinter = Opcions.Localize.Text("ERROR: PRINTER, CALL ATTENDANT (") + _cod + ")";
				break;
			}
		}

		public bool Ticket(string _ptr_device, decimal _valor, int _tick, int _id, int _model, int _cut, int _skeep, int _preskeep, int _wide)
		{
			if (Opcions.ModoTickets == 0 && _id != 0)
			{
				return false;
			}
			if (string.IsNullOrEmpty(_ptr_device))
			{
				PrinterErrorShow(0);
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				PrinterErrorShow(1);
				return false;
			}
			if (!PaperOut_Printer(_ptr_device))
			{
				PrinterErrorShow(2, errtick);
				return false;
			}
			Ticket_Reset(_ptr_device);
			Tickets tickets = new Tickets();
			Tickets.Info_Ticket info_Ticket = new Tickets.Info_Ticket();
			info_Ticket.TXT_Null = Opcions.Localize.Text("Repeated tickets are null");
			info_Ticket.TXT_Valid = Opcions.Localize.Text("Valid only for 15 minutes");
			info_Ticket.TXT_Valid2 = Opcions.Localize.Text("Change only for gift check");
			info_Ticket.TXT_Bottom = Opcions.Srv_ID_LinBottom;
			info_Ticket.TXT_Thanks = Opcions.Localize.Text("THANKS");
			info_Ticket.TXT_Lin1 = Opcions.Srv_ID_Lin1;
			info_Ticket.TXT_Lin2 = Opcions.Srv_ID_Lin2;
			info_Ticket.TXT_Lin3 = Opcions.Srv_ID_Lin3;
			info_Ticket.TXT_Lin4 = Opcions.Srv_ID_Lin4;
			info_Ticket.TXT_Lin5 = Opcions.Srv_ID_Lin5;
			info_Ticket.TXT_Location = Opcions.Localize.Text("Depositary");
			info_Ticket.TXT_BorneID = Opcions.Localize.Text("KIOSK");
			info_Ticket.TXT_Time = Opcions.Localize.Text("TIME");
			info_Ticket.TXT_Ticket = Opcions.Localize.Text("Fact");
			info_Ticket.TXT_Points = Opcions.Localize.Text("Euros");
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin1))
			{
				info_Ticket.TXT_Lin1 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin2))
			{
				info_Ticket.TXT_Lin2 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin3))
			{
				info_Ticket.TXT_Lin3 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin4))
			{
				info_Ticket.TXT_Lin4 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin5))
			{
				info_Ticket.TXT_Lin5 = " ";
			}
			tickets.Ticket_ESCPOS(_ptr_device, _valor, _tick, _id, _model, _cut, _skeep, _preskeep, _wide, 1, DateTime.Now, Opcions.Srv_User, info_Ticket);
			return true;
		}

		public bool _Old_Ticket(string _ptr_device, decimal _valor, int _tick, int _id, int _model, int _cut, int _skeep)
		{
			if (Opcions.ModoTickets == 0 && _id != 0)
			{
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				PrinterErrorShow(1);
				return false;
			}
			if (!PaperOut_Printer(_ptr_device))
			{
				PrinterErrorShow(2, errtick);
				return false;
			}
			Ticket_Reset(_ptr_device);
			if (_model == 1)
			{
				_Old_Ticket_ESCPOS(_ptr_device, _valor, _tick, _id, _model, _cut, _skeep);
				return true;
			}
			NIIClassLib nIIClassLib = new NIIClassLib();
			Barcode barcode = new Barcode();
			rtest.BackColor = Color.Yellow;
			rtest.ForeColor = Color.Yellow;
			timerPrinter.Enabled = true;
			string text = Gestion.Build_Mod10(Opcions.Srv_User, _tick, _id, 0);
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 580, 100);
			Bitmap bitmap = new Bitmap(original);
			IntPtr intPtr = CreateCompatibleDC(IntPtr.Zero);
			IntPtr hgdiobj = CreateCompatibleBitmap(intPtr, 580, 100);
			SelectObject(intPtr, hgdiobj);
			ImageRasterHelper.ConvertHBitmap(intPtr, bitmap, bIncludeSize: true);
			long o_jobid;
			int num;
			try
			{
				num = nIIClassLib.NiiStartDoc(_ptr_device, out o_jobid);
			}
			catch (Exception)
			{
				DeleteDC(intPtr);
				return false;
			}
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B6101", 6L, out o_jobid);
			string text2 = "0a\"-- La Belle Net --\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			DateTime now = DateTime.Now;
			text2 = $"\"{now.Day}/{now.Month:00}/{now.Year:0000} {now.Hour:00}:{now.Minute:00}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B21a0", 6L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B6100", 6L, out o_jobid);
			text2 = string.Format("0a\"" + Opcions.Localize.Text("Location:") + "\"0a");
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
			text2 = $"\"  {Opcions.Srv_ID_Lin1}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"  {Opcions.Srv_ID_Lin2}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"  {Opcions.Srv_ID_Lin3}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"  {Opcions.Srv_ID_Lin4}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"  RC: {Opcions.Srv_ID_Lin5}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = string.Format("\"  " + Opcions.Localize.Text("Kiosk ID:") + " {0}\"0a", Opcions.Srv_User);
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B6101", 6L, out o_jobid);
			text2 = $"\"--------------------------------------------\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			text2 = string.Format("\"" + Opcions.Localize.Text("Ticket:") + " {0}\"0a", text);
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			decimal value = _tick;
			TimeSpan timeSpan = new TimeSpan(0, 0, (int)value);
			text2 = string.Format("\"" + Opcions.Localize.Text("Time: ") + " {0}:{1:00}:{2:00}\"0a", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			if (_valor <= 0m)
			{
				text2 = "\"TEST TICKET\"0a";
			}
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			nIIClassLib.NiiImagePrintEx(_ptr_device, intPtr, 580, 100, 1, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			text2 = string.Format("0a\"" + Opcions.Localize.Text("THANKS") + "\"0a", "bar12x12");
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
			text2 = string.Format("\"" + Opcions.Srv_ID_LinBottom + "\"0a");
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"--------------------------------------------\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B4A781b69", 10L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B723160", 8L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1D\"G\"10", 7L, out o_jobid);
			num = nIIClassLib.NiiEndDoc(_ptr_device);
			DeleteDC(intPtr);
			return true;
		}

		private bool Ticket_Reset(string _ptr_device)
		{
			if (Opcions.ModoTickets == 0 && !Opcions.RunConfig)
			{
				return false;
			}
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

		private int Wait_Ticket_Poll()
		{
			LocalPrintServer localPrintServer = new LocalPrintServer();
			PrintQueue printQueue = localPrintServer.GetPrintQueue(Opcions.Impresora_Tck);
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

		private int Ticket_Poll()
		{
			LocalPrintServer localPrintServer = new LocalPrintServer();
			PrintQueue printQueue = localPrintServer.GetPrintQueue(Opcions.Impresora_Tck);
			PrintJobInfoCollection printJobInfoCollection = printQueue.GetPrintJobInfoCollection();
			if (printQueue.NumberOfJobs > 0)
			{
				if (printQueue.IsPrinting)
				{
					rtest.BackColor = Color.Blue;
					rtest.ForeColor = Color.Blue;
					return 1;
				}
				rtest.BackColor = Color.Green;
				rtest.ForeColor = Color.Green;
				return 0;
			}
			rtest.BackColor = Color.White;
			rtest.ForeColor = Color.White;
			timerPrinter.Enabled = false;
			return 0;
		}

		private bool _Old_Ticket_Out_ESCPOS(string _ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep = 0)
		{
			string arg = Gestion.Build_Mod10(Opcions.Srv_User, (int)_valor, _id, 1);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('@'));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('T'));
					binaryWriter.Write(Convert.ToChar(0));
					binaryWriter.Write('\n');
					binaryWriter.Write('\n');
					binaryWriter.Write('\n');
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar(33));
					binaryWriter.Write(Convert.ToChar(48));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar(97));
					binaryWriter.Write(Convert.ToChar(1));
					binaryWriter.Write("-- La Belle Net --".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\n');
					DateTime now = DateTime.Now;
					binaryWriter.Write($"{now.Day}/{now.Month:00}/{now.Year:0000} {now.Hour:00}:{now.Minute:00}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write((byte)48);
					binaryWriter.Write(Opcions.Localize.Text("CONGRATULATIONS").ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write((byte)16);
					binaryWriter.Write(Opcions.Localize.Text("Ticket value").ToCharArray());
					binaryWriter.Write('\n');
					int num = (int)_valor;
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write((byte)48);
					binaryWriter.Write(string.Format("{0}.{1:00} " + Opcions.Localize.Text("Euros"), num / 100, num % 100).ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write((byte)48);
					binaryWriter.Write($"*{arg}*".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write((byte)48);
					binaryWriter.Write($"ID: {_id}");
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('a');
					binaryWriter.Write('\0');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write((byte)160);
					binaryWriter.Write('\n');
					binaryWriter.Write(Opcions.Localize.Text("Location:").ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('\0');
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin1}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin2}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin3}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin4}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"  RC: {Opcions.Srv_ID_Lin5}".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write(string.Format("  " + Opcions.Localize.Text("Kiosk ID:") + " {0}", Opcions.Srv_User).ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('a');
					binaryWriter.Write('\u0001');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('0');
					binaryWriter.Write('\n');
					binaryWriter.Write(Opcions.Localize.Text("THANKS").ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('\0');
					binaryWriter.Write(Opcions.Srv_ID_LinBottom.ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write("------------------------------------------".ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('a');
					binaryWriter.Write('\0');
					binaryWriter.Write('\u001b');
					binaryWriter.Write('!');
					binaryWriter.Write('\0');
					for (int i = 0; i < _skeep; i++)
					{
						binaryWriter.Write('\n');
					}
					if (_cut == 1)
					{
						binaryWriter.Write((byte)27);
						binaryWriter.Write((byte)74);
						binaryWriter.Write((byte)120);
						binaryWriter.Write((byte)27);
						binaryWriter.Write((byte)105);
					}
					binaryWriter.Write('\u001d');
					binaryWriter.Write('V');
					binaryWriter.Write((byte)66);
					binaryWriter.Write((byte)3);
					binaryWriter.Flush();
					return Print_ESCPOS(_ptr_device, memoryStream.ToArray());
				}
			}
		}

		private bool Buscar_Ticket(int _id)
		{
			if (Cache_Tickets == null)
			{
				return false;
			}
			if (Cache_Tickets.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < Cache_Tickets.Length; i++)
			{
				if (Cache_Tickets[i] == _id)
				{
					return true;
				}
			}
			return false;
		}

		private void Add_Buscar_Ticket(int _id)
		{
			if (Cache_Tickets == null)
			{
				Cache_Tickets = new int[1];
				Cache_Tickets[0] = _id;
			}
			else if (!Buscar_Ticket(_id))
			{
				Array.Resize(ref Cache_Tickets, Cache_Tickets.Length + 1);
				Cache_Tickets[Cache_Tickets.Length - 1] = _id;
			}
		}

		public bool Ticket_Out(string _ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _wide, DateTime _temps, string _cod, int _join)
		{
			if (Opcions.ModoTickets == 0 && _id != 0)
			{
				return false;
			}
			if (string.IsNullOrEmpty(_ptr_device))
			{
				PrinterErrorShow(0);
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				PrinterErrorShow(1);
				return false;
			}
			if (!PaperOut_Printer(_ptr_device))
			{
				PrinterErrorShow(2, errtick);
				return false;
			}
			Ticket_Reset(_ptr_device);
			if (_Last_Id == _id)
			{
				return true;
			}
			if (Buscar_Ticket(_id))
			{
				return true;
			}
			Add_Buscar_Ticket(_id);
			_Last_Id = _id;
			Tickets tickets = new Tickets();
			Tickets.Info_Ticket info_Ticket = new Tickets.Info_Ticket();
			info_Ticket.TXT_Null = Opcions.Localize.Text("Repeated tickets are null");
			info_Ticket.TXT_Valid = Opcions.Localize.Text("Valid only for 15 minutes");
			info_Ticket.TXT_Valid2 = Opcions.Localize.Text("Change only for gift check");
			info_Ticket.TXT_Bottom = Opcions.Srv_ID_LinBottom;
			info_Ticket.TXT_Thanks = Opcions.Localize.Text("THANKS");
			info_Ticket.TXT_Lin1 = Opcions.Srv_ID_Lin1;
			info_Ticket.TXT_Lin2 = Opcions.Srv_ID_Lin2;
			info_Ticket.TXT_Lin3 = Opcions.Srv_ID_Lin3;
			info_Ticket.TXT_Lin4 = Opcions.Srv_ID_Lin4;
			info_Ticket.TXT_Lin5 = Opcions.Srv_ID_Lin5;
			info_Ticket.TXT_Location = Opcions.Localize.Text("Depositary");
			info_Ticket.TXT_BorneID = Opcions.Localize.Text("KIOSK");
			info_Ticket.TXT_Time = Opcions.Localize.Text("TIME");
			info_Ticket.TXT_Ticket = Opcions.Localize.Text("Fact");
			info_Ticket.TXT_Points = Opcions.Localize.Text("Euros");
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin1))
			{
				info_Ticket.TXT_Lin1 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin2))
			{
				info_Ticket.TXT_Lin2 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin3))
			{
				info_Ticket.TXT_Lin3 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin4))
			{
				info_Ticket.TXT_Lin4 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin5))
			{
				info_Ticket.TXT_Lin5 = " ";
			}
			tickets.Ticket_Out_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, Opcions.Srv_User, info_Ticket, Opcions.TicketHidePay, _join);
			Opcions.ticketCleanTemps = 1;
			return true;
		}

		public bool Ticket_Out_Mes_Temps(string _ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _wide, DateTime _temps, string _cod, int _join, int _tick_temps)
		{
			if (Opcions.ModoTickets == 0 && _id != 0)
			{
				return false;
			}
			if (string.IsNullOrEmpty(_ptr_device))
			{
				PrinterErrorShow(0);
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				PrinterErrorShow(1);
				return false;
			}
			if (!PaperOut_Printer(_ptr_device))
			{
				PrinterErrorShow(2, errtick);
				return false;
			}
			Ticket_Reset(_ptr_device);
			if (_Last_Id == _id)
			{
				return true;
			}
			if (Buscar_Ticket(_id))
			{
				return true;
			}
			Add_Buscar_Ticket(_id);
			_Last_Id = _id;
			Tickets tickets = new Tickets();
			Tickets.Info_Ticket info_Ticket = new Tickets.Info_Ticket();
			info_Ticket.TXT_Null = Opcions.Localize.Text("Repeated tickets are null");
			info_Ticket.TXT_Valid = Opcions.Localize.Text("Valid only for 15 minutes");
			info_Ticket.TXT_Valid2 = Opcions.Localize.Text("Change only for gift check");
			info_Ticket.TXT_Bottom = Opcions.Srv_ID_LinBottom;
			info_Ticket.TXT_Thanks = Opcions.Localize.Text("THANKS");
			info_Ticket.TXT_Lin1 = Opcions.Srv_ID_Lin1;
			info_Ticket.TXT_Lin2 = Opcions.Srv_ID_Lin2;
			info_Ticket.TXT_Lin3 = Opcions.Srv_ID_Lin3;
			info_Ticket.TXT_Lin4 = Opcions.Srv_ID_Lin4;
			info_Ticket.TXT_Lin5 = Opcions.Srv_ID_Lin5;
			info_Ticket.TXT_Location = Opcions.Localize.Text("Depositary");
			info_Ticket.TXT_BorneID = Opcions.Localize.Text("KIOSK");
			info_Ticket.TXT_Time = Opcions.Localize.Text("TIME");
			info_Ticket.TXT_Ticket = Opcions.Localize.Text("Fact");
			info_Ticket.TXT_Points = Opcions.Localize.Text("Euros");
			info_Ticket.TXT_GAS0 = Opcions.Localize.Text("Change for");
			info_Ticket.TXT_GAS1 = Opcions.Localize.Text("GAS");
			info_Ticket.TXT_GAS2 = Opcions.Localize.Text("Merchandise");
			info_Ticket.TXT_GAS3 = Opcions.Localize.Text("Xec Gift");
			info_Ticket.TXT_GAS4 = Opcions.Localize.Text(" to ");
			info_Ticket.TXT_GAS5 = Opcions.Localize.Text(" hours");
			info_Ticket.TXT_GAS6 = Opcions.Localize.Text("Valid until");
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin1))
			{
				info_Ticket.TXT_Lin1 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin2))
			{
				info_Ticket.TXT_Lin2 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin3))
			{
				info_Ticket.TXT_Lin3 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin4))
			{
				info_Ticket.TXT_Lin4 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin5))
			{
				info_Ticket.TXT_Lin5 = " ";
			}
			if (Opcions.Ticket_Carburante == 1)
			{
				tickets.Ticket_Out_Mes_Temps_GAS_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, Opcions.Srv_User, info_Ticket, Opcions.TicketHidePay, _join, _tick_temps);
			}
			else
			{
				tickets.Ticket_Out_Mes_Temps_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, Opcions.Srv_User, info_Ticket, Opcions.TicketHidePay, _join, _tick_temps);
			}
			Opcions.ticketCleanTemps = 1;
			return true;
		}

		public bool Ticket_Out_Check(string _ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _wide, DateTime _temps, string _cod, int _ntick, int _join)
		{
			if (Opcions.ModoTickets == 0 && _id != 0)
			{
				return false;
			}
			if (string.IsNullOrEmpty(_ptr_device))
			{
				PrinterErrorShow(0);
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				PrinterErrorShow(1);
				return false;
			}
			if (!PaperOut_Printer(_ptr_device))
			{
				PrinterErrorShow(2, errtick);
				return false;
			}
			Ticket_Reset(_ptr_device);
			if (_Last_Id == _id)
			{
				return true;
			}
			if (Buscar_Ticket(_id))
			{
				return true;
			}
			Add_Buscar_Ticket(_id);
			_Last_Id = _id;
			Tickets tickets = new Tickets();
			Tickets.Info_Ticket info_Ticket = new Tickets.Info_Ticket();
			info_Ticket.TXT_Null = Opcions.Localize.Text("Repeated tickets are null");
			info_Ticket.TXT_Valid = Opcions.Localize.Text("Valid only for 15 minutes");
			info_Ticket.TXT_Valid2 = Opcions.Localize.Text("Change only for gift check");
			info_Ticket.TXT_Bottom = Opcions.Srv_ID_LinBottom;
			info_Ticket.TXT_Thanks = Opcions.Localize.Text("THANKS");
			info_Ticket.TXT_Lin1 = Opcions.Srv_ID_Lin1;
			info_Ticket.TXT_Lin2 = Opcions.Srv_ID_Lin2;
			info_Ticket.TXT_Lin3 = Opcions.Srv_ID_Lin3;
			info_Ticket.TXT_Lin4 = Opcions.Srv_ID_Lin4;
			info_Ticket.TXT_Lin5 = Opcions.Srv_ID_Lin5;
			info_Ticket.TXT_Location = Opcions.Localize.Text("Depositary");
			info_Ticket.TXT_BorneID = Opcions.Localize.Text("KIOSK");
			info_Ticket.TXT_Time = Opcions.Localize.Text("TIME");
			info_Ticket.TXT_Ticket = Opcions.Localize.Text("Fact");
			info_Ticket.TXT_Points = Opcions.Localize.Text("Euros");
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin1))
			{
				info_Ticket.TXT_Lin1 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin2))
			{
				info_Ticket.TXT_Lin2 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin3))
			{
				info_Ticket.TXT_Lin3 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin4))
			{
				info_Ticket.TXT_Lin4 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin5))
			{
				info_Ticket.TXT_Lin5 = " ";
			}
			tickets.Ticket_Out_ESCPOS_Check(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, Opcions.Srv_User, info_Ticket, Opcions.TicketHidePay, _ntick, _join);
			Opcions.ticketCleanTemps = 1;
			return true;
		}

		public bool Ticket_Out_Conf(string _ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _wide, DateTime _temps, string _cod)
		{
			if (Opcions.ModoTickets == 0 && _id != 0)
			{
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				PrinterErrorShow(1);
				return false;
			}
			if (!PaperOut_Printer(_ptr_device))
			{
				PrinterErrorShow(2, errtick);
				return false;
			}
			Tickets tickets = new Tickets();
			Tickets.Info_Ticket info_Ticket = new Tickets.Info_Ticket();
			info_Ticket.TXT_Null = Opcions.Localize.Text("Repeated tickets are null");
			info_Ticket.TXT_Valid = Opcions.Localize.Text("Valid only for 15 minutes");
			info_Ticket.TXT_Valid2 = Opcions.Localize.Text("Change only for gift check");
			info_Ticket.TXT_Bottom = Opcions.Srv_ID_LinBottom;
			info_Ticket.TXT_Thanks = Opcions.Localize.Text("THANKS");
			info_Ticket.TXT_Lin1 = Opcions.Srv_ID_Lin1;
			info_Ticket.TXT_Lin2 = Opcions.Srv_ID_Lin2;
			info_Ticket.TXT_Lin3 = Opcions.Srv_ID_Lin3;
			info_Ticket.TXT_Lin4 = Opcions.Srv_ID_Lin4;
			info_Ticket.TXT_Lin5 = Opcions.Srv_ID_Lin5;
			info_Ticket.TXT_Location = Opcions.Localize.Text("Depositary");
			info_Ticket.TXT_BorneID = Opcions.Localize.Text("KIOSK");
			info_Ticket.TXT_Time = Opcions.Localize.Text("TIME");
			info_Ticket.TXT_Ticket = Opcions.Localize.Text("Fact");
			info_Ticket.TXT_Points = Opcions.Localize.Text("Euros");
			info_Ticket.TXT_Cancel = Opcions.Localize.Text("Validated");
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin1))
			{
				info_Ticket.TXT_Lin1 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin2))
			{
				info_Ticket.TXT_Lin2 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin3))
			{
				info_Ticket.TXT_Lin3 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin4))
			{
				info_Ticket.TXT_Lin4 = " ";
			}
			if (string.IsNullOrEmpty(info_Ticket.TXT_Lin5))
			{
				info_Ticket.TXT_Lin5 = " ";
			}
			tickets.Ticket_Out_Conf_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep, _wide, 1, _temps, _cod, Opcions.Srv_User, info_Ticket, DateTime.Now);
			return true;
		}

		public bool _Old_Ticket_Out(string _ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep = 0)
		{
			if (Opcions.ModoTickets == 0 && _id != 0)
			{
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				PrinterErrorShow(1);
				return false;
			}
			if (!PaperOut_Printer(_ptr_device))
			{
				PrinterErrorShow(2, errtick);
				return false;
			}
			Ticket_Reset(_ptr_device);
			if (_Last_Id == _id)
			{
				return true;
			}
			_Last_Id = _id;
			if (_model == 1)
			{
				_Old_Ticket_Out_ESCPOS(_ptr_device, _valor, _id, _model, _cut, _skeep, _preskeep);
				return true;
			}
			NIIClassLib nIIClassLib = new NIIClassLib();
			Barcode barcode = new Barcode();
			string text = Gestion.Build_Mod10(Opcions.Srv_User, (int)_valor, _id, 1);
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 580, 100);
			Bitmap bitmap = new Bitmap(original);
			IntPtr hdc = CreateCompatibleDC(IntPtr.Zero);
			IntPtr hgdiobj = CreateCompatibleBitmap(hdc, 580, 100);
			SelectObject(hdc, hgdiobj);
			ImageRasterHelper.ConvertHBitmap(hdc, bitmap, bIncludeSize: true);
			long o_jobid;
			int num;
			try
			{
				num = nIIClassLib.NiiStartDoc(_ptr_device, out o_jobid);
			}
			catch (Exception)
			{
				DeleteDC(hdc);
				return false;
			}
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B6101", 6L, out o_jobid);
			string text2 = "0a\"-- La Belle Net --\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			DateTime now = DateTime.Now;
			text2 = $"\"{now.Day}/{now.Month:00}/{now.Year:0000} {now.Hour:00}:{now.Minute:00}\"0a0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			text2 = string.Format("\"" + Opcions.Localize.Text("CONGRATULATIONS") + "\"0a");
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2110", 6L, out o_jobid);
			text2 = string.Format("\"" + Opcions.Localize.Text("Ticket value") + "\"0a");
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			int num2 = (int)_valor;
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			text2 = string.Format("\"{0}.{1:00} " + Opcions.Localize.Text("Euros") + "\"0a", num2 / 100, num2 % 100);
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"0a";
			bool flag = true;
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			text2 = $"\"*{text}*\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			text2 = $"\"ID: {_id}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B21a0", 6L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B6100", 6L, out o_jobid);
			text2 = string.Format("0a\"" + Opcions.Localize.Text("Location:") + "\"0a");
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
			text2 = $"\"  {Opcions.Srv_ID_Lin1}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"  {Opcions.Srv_ID_Lin2}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"  {Opcions.Srv_ID_Lin3}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"  {Opcions.Srv_ID_Lin4}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"  RC: {Opcions.Srv_ID_Lin5}\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = string.Format("\"  " + Opcions.Localize.Text("Kiosk ID:") + " {0}\"0a", Opcions.Srv_User);
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2130", 6L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B6101", 6L, out o_jobid);
			text2 = string.Format("0a\"" + Opcions.Localize.Text("THANKS") + "\"0a", "bar12x12");
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
			text2 = string.Format("\"" + Opcions.Srv_ID_LinBottom + "\"0a");
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			text2 = $"\"--------------------------------------------\"0a";
			num = nIIClassLib.NiiPrint(_ptr_device, text2, text2.Length, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B2100", 6L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B4A781b69", 10L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1B723160", 8L, out o_jobid);
			num = nIIClassLib.NiiPrint(_ptr_device, "1D\"G\"10", 7L, out o_jobid);
			num = nIIClassLib.NiiEndDoc(_ptr_device);
			DeleteDC(hdc);
			return true;
		}

		private bool Ticket_Out_ESCPOS(string _ptr_device, decimal _valor, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm)
		{
			string arg = Gestion.Build_Mod10(Opcions.Srv_User, (int)_valor, _id, 1);
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
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write("-- La Belle Net --".ToCharArray());
					binaryWriter.Write((byte)10);
					DateTime now = DateTime.Now;
					binaryWriter.Write($"{now.Day}/{now.Month:00}/{now.Year:0000} {now.Hour:00}:{now.Minute:00}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Opcions.Localize.Text("CONGRATULATIONS").ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Opcions.Localize.Text("Ticket value").ToCharArray());
					binaryWriter.Write((byte)10);
					int num = (int)_valor;
					binaryWriter.Write(string.Format("{0}.{1:00} " + Opcions.Localize.Text("Euros"), num / 100, num % 100).ToCharArray());
					binaryWriter.Write('\n');
					binaryWriter.Write($"*{arg}*".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write($"ID: {_id}");
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write(string.Format("  " + Opcions.Localize.Text("Kiosk ID:") + " {0}", Opcions.Srv_User).ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)_60mm);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AL);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FU + _ESCPOS_FDW + _60mm));
					binaryWriter.Write(Opcions.Localize.Text("Location:").ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)0);
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin1}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin2}".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin3}".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin4}".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write($"  RC: {Opcions.Srv_ID_Lin5}".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Opcions.Localize.Text("THANKS").ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write(Opcions.Srv_ID_LinBottom.ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write((byte)10);
					for (int i = 0; i < _skeep; i++)
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
					return Print_ESCPOS(_ptr_device, memoryStream.ToArray());
				}
			}
		}

		private string Convert_Text_POS(string _t)
		{
			if (_t.Length == 12)
			{
				return _t + " ";
			}
			return _t;
		}

		public bool Ticket_ESCPOS(string _ptr_device, decimal _valor, int _tick, int _id, int _model, int _cut, int _skeep, int _preskeep, int _60mm)
		{
			Barcode barcode = new Barcode();
			string text = Gestion.Build_Mod10(Opcions.Srv_User, _tick, _id, 0);
			barcode.IncludeLabel = false;
			barcode.LabelFont = new Font("Arial", 20f);
			barcode.Alignment = AlignmentPositions.CENTER;
			barcode.RotateFlipType = RotateFlipType.RotateNoneFlipNone;
			barcode.LabelPosition = LabelPositions.TOPCENTER;
			Image original = barcode.Encode(TYPE.CODE128, text, Color.Black, Color.White, 350, 60);
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
					binaryWriter.Write((byte)27);
					binaryWriter.Write('t');
					binaryWriter.Write((byte)2);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AC);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					for (int i = 0; i < _preskeep; i++)
					{
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write("-- La Belle Net --".ToCharArray());
					binaryWriter.Write((byte)10);
					DateTime now = DateTime.Now;
					binaryWriter.Write($"{now.Day}/{now.Month:00}/{now.Year:0000} {now.Hour:00}:{now.Minute:00}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('a');
					binaryWriter.Write(_ESCPOS_AL);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)(_ESCPOS_FE + _ESCPOS_FU + _ESCPOS_FDW + _60mm));
					binaryWriter.Write(Opcions.Localize.Text("Location:").ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write((byte)27);
					binaryWriter.Write('!');
					binaryWriter.Write((byte)0);
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin1}".ToCharArray());
					binaryWriter.Write((byte)10);
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin2}".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin3}".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write($"  {Opcions.Srv_ID_Lin4}".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write($"  RC: {Opcions.Srv_ID_Lin5}".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDH + _ESCPOS_FDW + _60mm));
					binaryWriter.Write(string.Format("  " + Opcions.Localize.Text("Kiosk ID:") + " {0}", Opcions.Srv_User).ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(string.Format(Opcions.Localize.Text("Ticket:") + " {0}", text).ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(10));
					decimal value = _tick;
					TimeSpan timeSpan = new TimeSpan(0, 0, (int)value);
					string text2 = string.Format(Opcions.Localize.Text("Time: ") + " {0}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
					if (_valor <= 0m)
					{
						text2 = "TEST TICKET";
					}
					binaryWriter.Write(text2.ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('a'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_AC));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(24));
					int num = 0;
					while (num < bitmapData.Height)
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
									int num2 = (num / 8 + k) * 8 + l;
									int i = num2 * bitmapData.Width + j;
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
						num += 24;
						binaryWriter.Write(Convert.ToChar(10));
					}
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('3'));
					binaryWriter.Write(Convert.ToChar(30));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_ESCPOS_FDW + _ESCPOS_FDH + _60mm));
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Opcions.Localize.Text("THANKS").ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Convert.ToChar(27));
					binaryWriter.Write(Convert.ToChar('!'));
					binaryWriter.Write(Convert.ToChar(_60mm));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write(Opcions.Srv_ID_LinBottom.ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					binaryWriter.Write("---------------------------------".ToCharArray());
					binaryWriter.Write(Convert.ToChar(10));
					for (int i = 0; i < _skeep; i++)
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
					return Print_ESCPOS(_ptr_device, memoryStream.ToArray());
				}
			}
		}

		public bool Check_Printer_Ready(string _ptr_device)
		{
			if (string.IsNullOrEmpty(_ptr_device))
			{
				PrinterErrorShow(0);
				return false;
			}
			if (!Exist_Printer(_ptr_device))
			{
				PrinterErrorShow(1);
				return false;
			}
			if (!PaperOut_Printer(_ptr_device))
			{
				PrinterErrorShow(2, errtick);
				return false;
			}
			return true;
		}

		private void bTicket_Click(object sender, EventArgs e)
		{
			if (Opcions != null && Opcions.Temps > 0)
			{
				bool flag = false;
				DLG_Abonar_Ticket dLG_Abonar_Ticket = new DLG_Abonar_Ticket(ref Opcions, Opcions.Localize.Text("ATTENTION!\r\n\r\nPour éviter de perdre vos jeux gratuits, jouez-les avant d'imprimer votre ticket\r\n\r\nVOUS ARRÊTEZ DE SURFER SUR INTERNET\r\nIMPRIMER TICKET?"));
				dLG_Abonar_Ticket.ShowDialog();
				if (dLG_Abonar_Ticket.OK && Check_Printer_Ready(Opcions.Impresora_Tck))
				{
					Srv_Add_Ticket(Opcions.Temps, 0);
				}
			}
		}

		private void bLogin_Click(object sender, EventArgs e)
		{
			if (Opcions.Logged)
			{
				Opcions.Logged = false;
				bLogin.Image = Resources.ico_userd;
				Status = Fases.GoHome;
				Hide_Browser_Nav();
				Stop_Temps();
				return;
			}
			DLG_Registro dLG_Registro = new DLG_Registro(ref Opcions);
			dLG_Registro.ShowDialog();
			if (dLG_Registro.Login == 1)
			{
				Opcions.Logged = true;
				bLogin.Image = Resources.ico_1;
			}
			else
			{
				Opcions.Logged = false;
				bLogin.Image = Resources.ico_userd;
			}
		}

		private void bBar_Click(object sender, EventArgs e)
		{
			DLG_Barcode dLG_Barcode = new DLG_Barcode(ref Opcions);
			dLG_Barcode.MWin = this;
			dLG_Barcode.ShowDialog();
		}

		private void bTime_Click(object sender, EventArgs e)
		{
			if (Opcions.ComprarTemps == 0 && !Opcions.InGame && Opcions.News != 2)
			{
				Opcions.ComprarTemps = 2;
				Stop_Temps();
				CreditManagerFull creditManagerFull = new CreditManagerFull(ref Opcions);
				creditManagerFull.Show();
				Start_Temps();
			}
		}

		private void MainWindow_MouseMove(object sender, MouseEventArgs e)
		{
			Opcions.LastMouseMove = DateTime.Now;
			TimeoutHome = 0;
		}

		private void Move_Screesaver(object sender, EventArgs e)
		{
			Opcions.LastMouseMove = DateTime.Now;
			TimeoutHome = 0;
		}

		[DllImport("winmm.dll")]
		public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

		[DllImport("winmm.dll")]
		public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

		private void bMute_Click(object sender, EventArgs e)
		{
			if (Opcions.modo_XP == 1)
			{
				SendMessageW(base.Handle, 793, base.Handle, (IntPtr)524288);
			}
			else if (device != null)
			{
				int num = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100f);
				if (num == 0)
				{
					num = _MMVol;
				}
				else
				{
					_MMVol = num;
					num = 0;
				}
				device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)num / 100f;
				SoundTest();
			}
		}

		private void SoundMax()
		{
			if (device != null)
			{
				alamaramute = device.AudioEndpointVolume.Mute;
				device.AudioEndpointVolume.Mute = false;
				_MMVol = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100f);
				device.AudioEndpointVolume.MasterVolumeLevelScalar = 1f;
			}
		}

		private void SoundRestore()
		{
			if (device != null)
			{
				device.AudioEndpointVolume.Mute = alamaramute;
				device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)_MMVol / 100f;
			}
		}

		private void bVDown_Click(object sender, EventArgs e)
		{
			if (Opcions.modo_XP == 1)
			{
				SendMessageW(base.Handle, 793, base.Handle, (IntPtr)589824);
			}
			else if (device != null)
			{
				int num = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100f);
				num = ((num > 5) ? (num - 5) : 0);
				device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)num / 100f;
				_MMVol = num;
				SoundTest();
			}
		}

		private void bVUp_Click(object sender, EventArgs e)
		{
			if (Opcions.modo_XP == 1)
			{
				SendMessageW(base.Handle, 793, base.Handle, (IntPtr)655360);
			}
			else if (device != null)
			{
				int num = (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100f);
				num = ((num < 95) ? (num + 5) : 100);
				device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)num / 100f;
				_MMVol = num;
				SoundTest();
			}
		}

		private void timerPrinter_Tick(object sender, EventArgs e)
		{
			Ticket_Poll();
		}

		private void PrintProps(ManagementObject o, string prop)
		{
			try
			{
				object obj = dbglog;
				dbglog = obj + "(" + prop + "|" + o[prop] + ")\r\n";
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
			}
		}

		private void Find_Validator(bool _force = false)
		{
			if (Last_DRV != "-")
			{
				Opcions.Dev_BNV_P = Last_COM;
				Opcions.Dev_BNV = Last_DRV;
			}
			string text = Opcions.Dev_BNV.ToLower();
			if (text == null)
			{
				return;
			}
			if (!(text == "ssp"))
			{
				if (!(text == "ssp3"))
				{
					if (text == "f40" && Find_Validator_CCT(_force: true) == 2 && Find_Validator_SSP(_force: true) == 2)
					{
						Find_Validator_SSP6(_force: true);
					}
				}
				else if (Find_Validator_SSP6(_force: true) == 2 && Find_Validator_SSP(_force: true) == 2)
				{
					Find_Validator_CCT(_force: true);
				}
			}
			else if (Find_Validator_SSP(_force: true) == 2 && Find_Validator_SSP6(_force: true) == 2)
			{
				Find_Validator_CCT(_force: true);
			}
		}

		private int Find_Validator_SSP(bool _force = false)
		{
			if (Opcions.RunConfig)
			{
				return 0;
			}
			if (Opcions.Dev_BNV.ToLower() == "ssp".ToLower() && Opcions.Dev_BNV_P.ToLower().Contains("com"))
			{
				Control_NV_SSP control_NV_SSP = new Control_NV_SSP();
				control_NV_SSP.port = Opcions.Dev_BNV_P;
				control_NV_SSP.Open();
				Thread.Sleep(100);
				Application.DoEvents();
				control_NV_SSP.Poll();
				control_NV_SSP.Close();
				if (control_NV_SSP.respuesta)
				{
					Last_COM = Opcions.Dev_BNV_P;
					Last_DRV = Opcions.Dev_BNV;
					return 1;
				}
			}
			if (Opcions.Dev_BNV.ToLower() == "ssp".ToLower() || string.IsNullOrEmpty(Opcions.Dev_BNV) || Opcions.Dev_BNV == "?" || _force)
			{
				Control_NV_SSP control_NV_SSP = new Control_NV_SSP();
				control_NV_SSP.Start_Find_Device();
				while (!control_NV_SSP.Poll_Find_Device())
				{
					Thread.Sleep(100);
					Application.DoEvents();
					if (Opcions.RunConfig)
					{
						control_NV_SSP.Close();
						return 0;
					}
				}
				control_NV_SSP.Stop_Find_Device();
				control_NV_SSP.Close();
				if (control_NV_SSP._f_resp_scom != "-")
				{
					Opcions.Dev_BNV = "ssp";
					Opcions.Dev_BNV_P = control_NV_SSP._f_resp_scom;
					Last_COM = Opcions.Dev_BNV_P;
					Last_DRV = Opcions.Dev_BNV;
					return 1;
				}
			}
			return 2;
		}

		private int Find_Validator_SSP6(bool _force = false)
		{
			if (Opcions.RunConfig)
			{
				return 0;
			}
			if (Opcions.Dev_BNV.ToLower() == "ssp3".ToLower() && Opcions.Dev_BNV_P.ToLower().Contains("com"))
			{
				Control_NV_SSP_P6 control_NV_SSP_P = new Control_NV_SSP_P6();
				control_NV_SSP_P.port = Opcions.Dev_BNV_P;
				control_NV_SSP_P.Open();
				Thread.Sleep(100);
				Application.DoEvents();
				control_NV_SSP_P.Poll();
				control_NV_SSP_P.Close();
				if (control_NV_SSP_P.m_Respuesta)
				{
					Last_COM = Opcions.Dev_BNV_P;
					Last_DRV = Opcions.Dev_BNV;
					return 1;
				}
			}
			if (Opcions.Dev_BNV.ToLower() == "ssp3".ToLower() || string.IsNullOrEmpty(Opcions.Dev_BNV) || Opcions.Dev_BNV == "?" || _force)
			{
				Control_NV_SSP_P6 control_NV_SSP_P = new Control_NV_SSP_P6();
				control_NV_SSP_P.Start_Find_Device();
				do
				{
					Thread.Sleep(100);
					Application.DoEvents();
					if (Opcions.RunConfig)
					{
						control_NV_SSP_P.Close();
						return 0;
					}
				}
				while (!control_NV_SSP_P.Poll_Find_Device());
				control_NV_SSP_P.Stop_Find_Device();
				control_NV_SSP_P.Close();
				if (control_NV_SSP_P._f_resp_scom != "-")
				{
					Opcions.Dev_BNV = "ssp3";
					Opcions.Dev_BNV_P = control_NV_SSP_P._f_resp_scom;
					Last_COM = Opcions.Dev_BNV_P;
					Last_DRV = Opcions.Dev_BNV;
					return 1;
				}
			}
			return 2;
		}

		private int Find_Validator_CCT(bool _force = false)
		{
			if (Opcions.RunConfig)
			{
				return 0;
			}
			if (Opcions.Dev_BNV.ToLower() == "f40".ToLower() && Opcions.Dev_BNV_P.ToLower().Contains("com"))
			{
				Control_F40_CCTalk control_F40_CCTalk = new Control_F40_CCTalk();
				control_F40_CCTalk.port = Opcions.Dev_BNV_P;
				control_F40_CCTalk.Open();
				Thread.Sleep(100);
				control_F40_CCTalk.Poll();
				control_F40_CCTalk.Close();
				if (control_F40_CCTalk.respuesta)
				{
					Last_COM = Opcions.Dev_BNV_P;
					Last_DRV = Opcions.Dev_BNV;
					return 1;
				}
			}
			if (Opcions.Dev_BNV.ToLower() == "f40".ToLower() || string.IsNullOrEmpty(Opcions.Dev_BNV) || Opcions.Dev_BNV == "?" || _force)
			{
				Control_F40_CCTalk control_F40_CCTalk = new Control_F40_CCTalk();
				control_F40_CCTalk.Start_Find_Device();
				do
				{
					control_F40_CCTalk.Find_Device();
					if (control_F40_CCTalk.respuesta)
					{
						Opcions.Dev_BNV = "f40";
						Opcions.Dev_BNV_P = control_F40_CCTalk._f_resp_scom;
						control_F40_CCTalk.Stop_Find_Device();
						control_F40_CCTalk.Close();
						Last_COM = Opcions.Dev_BNV_P;
						Last_DRV = Opcions.Dev_BNV;
						return 1;
					}
					control_F40_CCTalk.Poll_Find_Device();
					Application.DoEvents();
					if (Opcions.RunConfig)
					{
						control_F40_CCTalk.Close();
						return 0;
					}
				}
				while (control_F40_CCTalk._f_com != "-");
				control_F40_CCTalk.Stop_Find_Device();
				control_F40_CCTalk.Close();
			}
			return 2;
		}

		private void Find_Selector()
		{
			if (Opcions.Dev_Coin.ToLower() == "rm5".ToLower() || string.IsNullOrEmpty(Opcions.Dev_Coin) || Opcions.Dev_Coin == "?")
			{
				Control_Comestero control_Comestero = new Control_Comestero();
				if (Opcions.Dev_Coin_P.ToLower().Contains("com".ToLower()))
				{
					control_Comestero.port = Opcions.Dev_Coin_P;
					control_Comestero._f_com = Opcions.Dev_Coin_P;
					if (control_Comestero.Open())
					{
						control_Comestero.Close();
						return;
					}
					control_Comestero.Close();
				}
				control_Comestero.Start_Find_Device();
				while (!control_Comestero.Poll_Find_Device())
				{
					Thread.Sleep(50);
				}
				control_Comestero.Stop_Find_Device();
				if (control_Comestero._f_resp_scom != "-")
				{
					Opcions.Dev_Coin = "rm5";
					Opcions.Dev_Coin_P = control_Comestero._f_resp_scom;
				}
			}
			if (!(Opcions.Dev_Coin.ToLower() == "cct2".ToLower()) && !string.IsNullOrEmpty(Opcions.Dev_Coin) && !(Opcions.Dev_Coin == "?"))
			{
				return;
			}
			Control_CCTALK_COIN control_CCTALK_COIN = new Control_CCTALK_COIN();
			if (Opcions.Dev_Coin_P.ToLower().Contains("com".ToLower()))
			{
				control_CCTALK_COIN.port = Opcions.Dev_Coin_P;
				control_CCTALK_COIN._f_com = Opcions.Dev_Coin_P;
				control_CCTALK_COIN.GetInfo(2, "POLL");
				control_CCTALK_COIN.Start_Find_Device();
				Thread.Sleep(500);
				if (control_CCTALK_COIN.Poll_Find_Device())
				{
					control_CCTALK_COIN.Close();
					return;
				}
				control_CCTALK_COIN.Close();
			}
			control_CCTALK_COIN.Start_Find_Device();
			while (!control_CCTALK_COIN.Poll_Find_Device())
			{
				Thread.Sleep(50);
			}
			control_CCTALK_COIN.Stop_Find_Device();
			if (control_CCTALK_COIN._f_resp_scom != "-")
			{
				Opcions.Dev_Coin = "cct2";
				Opcions.Dev_Coin_P = control_CCTALK_COIN._f_resp_scom;
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
			if (Opcions.ModoTickets == 1 && Opcions.modo_XP == 0)
			{
				if (string.IsNullOrEmpty(_ptr_device))
				{
					return false;
				}
				errtick = "";
				LocalPrintServer localPrintServer = new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer);
				PrintQueueCollection printQueues = localPrintServer.GetPrintQueues();
				foreach (PrintQueue item in printQueues)
				{
					item.Refresh();
					if (item.Name.ToLower() == _ptr_device.ToLower())
					{
						errtick = SpotTroubleUsingQueueAttributes(item);
						if (errtick != "OK")
						{
							return false;
						}
						errtick = SpotTroubleUsingProperties(item);
						if (errtick != "OK")
						{
							return false;
						}
						return true;
					}
				}
			}
			return false;
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
					bool flag2 = false;
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
			if (Opcions.ModoTickets == 1 && Opcions.modo_XP == 0)
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
			}
			return false;
		}

		private void Find_Printer()
		{
			if (Opcions.ModoTickets != 1 || Opcions.modo_XP != 0)
			{
				return;
			}
			Clean_Printer();
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
			string text = "";
			string text2 = "";
			string text3 = "";
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				string text4 = item.Properties["Caption"].Value.ToString();
				bool flag = false;
				try
				{
					flag = bool.Parse(item.Properties["WorkOffline"].Value.ToString());
				}
				catch
				{
				}
				if (!flag && text4.ToLower().Contains("NII".ToLower()))
				{
					text = text4;
				}
				if (!flag && text4.ToLower().Contains("STAR".ToLower()))
				{
					text2 = text4;
				}
				if (!flag && text4.ToLower().Contains("CUSTOM".ToLower()))
				{
					text3 = text4;
				}
				if (!flag && text4.ToLower() == Opcions.Impresora_Tck.ToLower())
				{
					if (Opcions.Impresora_Tck.ToLower().Contains("nii".ToLower()))
					{
						Opcions.Ticket_Cut = 1;
					}
					if (Opcions.Impresora_Tck.ToLower().Contains("star".ToLower()))
					{
						Opcions.Ticket_Cut = 0;
					}
					if (Opcions.Impresora_Tck.ToLower().Contains("custom".ToLower()))
					{
						Opcions.Ticket_Cut = 1;
						if (Opcions.Impresora_Tck.ToLower().Contains("TG2460".ToLower()))
						{
							Opcions.Ticket_60mm = 1;
							Opcions.Ticket_N_FEED = 12;
							Opcions.Ticket_N_HEAD = 3;
						}
					}
					return;
				}
				if (!flag && Opcions.Impresora_Tck.ToLower().Contains("NII".ToLower()) && text4.ToLower().Contains("NII".ToLower()))
				{
					Opcions.Impresora_Tck = text4;
					Opcions.Ticket_Cut = 1;
					return;
				}
				if (!flag && Opcions.Impresora_Tck.ToLower().Contains("star".ToLower()) && text4.ToLower().Contains("Star".ToLower()))
				{
					Opcions.Impresora_Tck = text4;
					Opcions.Ticket_Cut = 0;
					return;
				}
				if (!flag && Opcions.Impresora_Tck.ToLower().Contains("custom".ToLower()) && text4.ToLower().Contains("custom".ToLower()))
				{
					Opcions.Impresora_Tck = text4;
					Opcions.Ticket_Cut = 1;
					if (Opcions.Impresora_Tck.ToLower().Contains("TG2460".ToLower()))
					{
						Opcions.Ticket_60mm = 1;
						Opcions.Ticket_N_FEED = 12;
						Opcions.Ticket_N_HEAD = 3;
					}
					return;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				Opcions.Impresora_Tck = text;
				Opcions.Ticket_N_FEED = 3;
				Opcions.Ticket_N_HEAD = 0;
				Opcions.Ticket_Cut = 1;
				Opcions.Ticket_Model = 0;
			}
			else if (!string.IsNullOrEmpty(text2))
			{
				Opcions.Impresora_Tck = text2;
				Opcions.Ticket_N_FEED = 3;
				Opcions.Ticket_N_HEAD = 0;
				Opcions.Ticket_Cut = 0;
				Opcions.Ticket_Model = 1;
			}
			else if (!string.IsNullOrEmpty(text3))
			{
				Opcions.Impresora_Tck = text3;
				Opcions.Ticket_N_FEED = 3;
				Opcions.Ticket_N_HEAD = 3;
				Opcions.Ticket_Cut = 1;
				Opcions.Ticket_Model = 1;
				if (Opcions.Impresora_Tck.ToLower().Contains("TG2460".ToLower()))
				{
					Opcions.Ticket_60mm = 1;
					Opcions.Ticket_N_FEED = 12;
					Opcions.Ticket_N_HEAD = 3;
				}
			}
		}

		private int set_ban_users(string _l)
		{
			int num = 0;
			string[] array = _l.Split('#');
			num = array.Length;
			if (num > 0 && string.IsNullOrEmpty(array[num - 1]))
			{
				num--;
			}
			ban_users = array;
			return num;
		}

		private int set_ban_macs(string _l)
		{
			int num = 0;
			string[] array = _l.Split('#');
			num = array.Length;
			if (num > 0 && string.IsNullOrEmpty(array[num - 1]))
			{
				num--;
			}
			ban_mac = array;
			return num;
		}

		private int set_ban_ip(string _l)
		{
			int num = 0;
			string[] array = _l.Split('#');
			num = array.Length;
			if (num > 0 && string.IsNullOrEmpty(array[num - 1]))
			{
				num--;
			}
			ban_ip = array;
			return num;
		}

		private int set_ban_country(string _l)
		{
			int num = 0;
			string[] array = _l.Split('#');
			num = array.Length;
			if (num > 0 && string.IsNullOrEmpty(array[num - 1]))
			{
				num--;
			}
			ban_country = array;
			return num;
		}

		private void set_reset(string _l, int _modo)
		{
			if (_modo == 0)
			{
				Opcions.ForceResetMask = _l;
			}
			if (string.IsNullOrEmpty(_l))
			{
				Opcions.ForceReset = false;
			}
			if (_l != Opcions.ForceResetMask)
			{
				Opcions.ForceResetMask = _l;
				long num = 0L;
				try
				{
					num = long.Parse(_l);
				}
				catch
				{
				}
				if (num > 0)
				{
					Opcions.ForceReset = true;
				}
			}
		}

		private void set_reset_user(string _l, int _modo)
		{
			string[] array = _l.Split(',');
			if (array == null || array.Length != 2 || array[1] != Opcions.Srv_User)
			{
				return;
			}
			_l = array[0];
			if (_modo == 0)
			{
				Opcions.ForceResetMask = _l;
			}
			if (string.IsNullOrEmpty(_l))
			{
				Opcions.ForceReset = false;
			}
			if (_l != Opcions.ForceResetMask)
			{
				Opcions.ForceResetMask = _l;
				long num = 0L;
				try
				{
					num = long.Parse(_l);
				}
				catch
				{
				}
				if (num > 0)
				{
					Opcions.ForceReset = true;
				}
			}
		}

		private void set_spy(string _l, int _modo)
		{
			Opcions.Spy = 0;
			Opcions.UserSpy = "?";
			if (!string.IsNullOrEmpty(_l))
			{
				Opcions.Spy = 1;
				Opcions.UserSpy = _l;
			}
		}

		private void set_remote(string _l, int _modo)
		{
			Opcions.Spy = 0;
			Opcions.UserSpy = "?";
			if (!string.IsNullOrEmpty(_l))
			{
				Opcions.Spy = 2;
				Opcions.UserSpy = _l;
			}
		}

		private void set_mantenimiento(string _l, int _modo)
		{
			if (_modo == 0)
			{
			}
			if (string.IsNullOrEmpty(_l))
			{
				Opcions.ForceManteniment = false;
			}
			if (_l != Opcions.ForceMantenimentMask)
			{
				Opcions.ForceMantenimentMask = _l;
				int num = 0;
				try
				{
					num = int.Parse(_l);
				}
				catch
				{
				}
				if (num > 0)
				{
					Opcions.ForceManteniment = true;
				}
				else
				{
					Opcions.ForceManteniment = false;
				}
			}
		}

		private void set_command(string _l, int _modo)
		{
			if (_modo == 0)
			{
			}
			if (!string.IsNullOrEmpty(_l))
			{
			}
		}

		public bool load_web(string _web, int _modo)
		{
			string text = Path.GetTempPath() + "__check.html";
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				WebClient webClient = new WebClient();
				webClient.DownloadFile(_web, text);
			}
			catch (Exception)
			{
				return false;
			}
			XmlTextReader xmlTextReader = null;
			try
			{
				xmlTextReader = new XmlTextReader(text);
			}
			catch (Exception)
			{
				xmlTextReader?.Close();
				return false;
			}
			try
			{
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						if (xmlTextReader.Name.ToLower() == "rst".ToLower())
						{
							set_reset(XmlConfig.Decrypt(xmlTextReader.ReadString()), _modo);
						}
						if (xmlTextReader.Name.ToLower() == "urst".ToLower())
						{
							set_reset_user(XmlConfig.Decrypt(xmlTextReader.ReadString()), _modo);
						}
						if (xmlTextReader.Name.ToLower() == "cmd".ToLower())
						{
							set_command(XmlConfig.Decrypt(xmlTextReader.ReadString()), _modo);
						}
						if (xmlTextReader.Name.ToLower() == "man".ToLower())
						{
							set_mantenimiento(XmlConfig.Decrypt(xmlTextReader.ReadString()), _modo);
						}
						if (xmlTextReader.Name.ToLower() == "spy".ToLower())
						{
							set_spy(XmlConfig.Decrypt(xmlTextReader.ReadString()), _modo);
						}
						if (xmlTextReader.Name.ToLower() == "rem".ToLower())
						{
							set_remote(XmlConfig.Decrypt(xmlTextReader.ReadString()), _modo);
						}
						if (xmlTextReader.Name.ToLower() == "bu".ToLower())
						{
							set_ban_users(XmlConfig.Decrypt(xmlTextReader.ReadString()));
						}
						if (xmlTextReader.Name.ToLower() == "bm".ToLower())
						{
							set_ban_macs(XmlConfig.Decrypt(xmlTextReader.ReadString()));
						}
						if (xmlTextReader.Name.ToLower() == "bi".ToLower())
						{
							set_ban_ip(XmlConfig.Decrypt(xmlTextReader.ReadString()));
						}
						if (xmlTextReader.Name.ToLower() == "bc".ToLower())
						{
							set_ban_country(XmlConfig.Decrypt(xmlTextReader.ReadString()));
						}
						if (xmlTextReader.Name.ToLower() == "info".ToLower() && xmlTextReader.HasAttributes)
						{
							for (int i = 0; i < xmlTextReader.AttributeCount; i++)
							{
								xmlTextReader.MoveToAttribute(i);
								if (xmlTextReader.Name.ToLower() == "a".ToLower())
								{
									try
									{
										web_versio = xmlTextReader.Value.ToString();
									}
									catch
									{
									}
								}
								if (xmlTextReader.Name.ToLower() == "b".ToLower())
								{
									try
									{
										web_vnc = XmlConfig.Decrypt(xmlTextReader.Value.ToString());
										if (!string.IsNullOrEmpty(web_vnc) && web_vnc != "-")
										{
											Opcions.Server_VNC = web_vnc;
										}
									}
									catch
									{
									}
								}
								if (xmlTextReader.Name.ToLower() == "c".ToLower())
								{
									try
									{
										web_date = xmlTextReader.Value.ToString();
									}
									catch
									{
									}
								}
							}
						}
					}
				}
				xmlTextReader.Close();
			}
			catch (Exception)
			{
				xmlTextReader?.Close();
				return false;
			}
			return true;
		}

		private void EntraJocs_Click(object sender, EventArgs e)
		{
			if (Banner_On != 2)
			{
				return;
			}
			TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
			bool flag = true;
			WebLink = "http://".ToLower() + Opcions.Srv_Web_Ip + "/MenuTel.aspx?t=" + Opcions.Srv_User + "," + Opcions.Srv_User_P + ",TickQuioscV2.aspx," + timeSpan.TotalMilliseconds;
			EntraJocs.Visible = false;
			Banner_On = 0;
			if (Opcions.CancelTempsOn == 1)
			{
				flag = false;
				if (Opcions.CancelTemps > 20)
				{
					int value = Opcions.CancelTemps / 12 * 5;
					if (Opcions.Credits > (decimal)value)
					{
						Srv_Sub_Credits(value, 0);
					}
					else
					{
						Srv_Sub_Credits(Opcions.Credits, 0);
					}
				}
			}
			Opcions.CancelTemps = 0;
			MenuGames = 1;
			EntraJocs.BackgroundImage = Img_Banner1;
			EntraJocs.Invalidate();
			try
			{
				bTicket.Enabled = false;
			}
			catch
			{
			}
		}

		private void timerMessages_Tick(object sender, EventArgs e)
		{
			Fases status = Status;
			if (status != Fases.Home || Opcions.ticketCleanTemps != 1 || Opcions.InGame)
			{
				return;
			}
			Opcions.ticketCleanTemps = 2;
			if (Opcions.Temps > 0 && Opcions.AutoTicketTime >= 1 && Opcions.AutoTicketTime != 2)
			{
				DLG_Abonar_Ticket dLG_Abonar_Ticket = new DLG_Abonar_Ticket(ref Opcions, Opcions.Localize.Text("ATTENTION!\r\n\r\nVous n'avez plus de jeux gratuits\r\n\r\nDésirez vous récupérer votre temp restant?\r\nIMPRIMER TICKET?"));
				dLG_Abonar_Ticket.ShowDialog();
				if (dLG_Abonar_Ticket.OK && Check_Printer_Ready(Opcions.Impresora_Tck))
				{
					Srv_Add_Ticket(Opcions.Temps, 0);
				}
			}
			Opcions.ticketCleanTemps = 0;
		}

		private void Control_Getton_Time()
		{
		}

		private void Control_Getton_Reset()
		{
		}

		private void Control_Getton()
		{
		}

		private void bGETTON_Click(object sender, EventArgs e)
		{
			Opcions.Add_Getton = 0;
			Internal_Add_Credits(25);
			Opcions.GettonOff = true;
			pGETTON.Visible = false;
			Control_Getton_Time();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Kiosk.MainWindow));
			pMenu = new System.Windows.Forms.Panel();
			pGETTON = new System.Windows.Forms.Panel();
			lGETTON = new System.Windows.Forms.Label();
			pTime = new System.Windows.Forms.Panel();
			eCredits = new System.Windows.Forms.Label();
			pKeyboard = new System.Windows.Forms.Panel();
			pTemps = new System.Windows.Forms.Panel();
			rtest = new System.Windows.Forms.Panel();
			lcdClock = new System.Windows.Forms.Label();
			pLogin = new System.Windows.Forms.Panel();
			pTicket = new System.Windows.Forms.Panel();
			lCGRAT = new System.Windows.Forms.Label();
			pNavegation = new System.Windows.Forms.Panel();
			tURL = new System.Windows.Forms.TextBox();
			iSponsor = new System.Windows.Forms.Label();
			timerCredits = new System.Windows.Forms.Timer(components);
			timerPoll = new System.Windows.Forms.Timer(components);
			timerStartup = new System.Windows.Forms.Timer(components);
			imgLed = new System.Windows.Forms.ImageList(components);
			pScreenSaver = new System.Windows.Forms.Label();
			timerPrinter = new System.Windows.Forms.Timer(components);
			EntraJocs = new System.Windows.Forms.Panel();
			timerMessages = new System.Windows.Forms.Timer(components);
			lCALL = new System.Windows.Forms.Label();
			bGETTON = new System.Windows.Forms.Button();
			bTime = new System.Windows.Forms.Button();
			pInsertCoin = new System.Windows.Forms.PictureBox();
			bVUp = new System.Windows.Forms.Button();
			bVDown = new System.Windows.Forms.Button();
			bMute = new System.Windows.Forms.Button();
			bKeyboard = new System.Windows.Forms.Button();
			bLogin = new System.Windows.Forms.Button();
			bBar = new System.Windows.Forms.Button();
			bTicket = new System.Windows.Forms.Button();
			bHome = new System.Windows.Forms.Button();
			bBack = new System.Windows.Forms.Button();
			bForward = new System.Windows.Forms.Button();
			bGo = new System.Windows.Forms.Button();
			pMenu.SuspendLayout();
			pGETTON.SuspendLayout();
			pTime.SuspendLayout();
			pKeyboard.SuspendLayout();
			pTemps.SuspendLayout();
			pLogin.SuspendLayout();
			pTicket.SuspendLayout();
			pNavegation.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pInsertCoin).BeginInit();
			SuspendLayout();
			pMenu.BackColor = System.Drawing.Color.Green;
			pMenu.Controls.Add(pGETTON);
			pMenu.Controls.Add(pTime);
			pMenu.Controls.Add(pKeyboard);
			pMenu.Controls.Add(pTemps);
			pMenu.Controls.Add(pLogin);
			pMenu.Controls.Add(pTicket);
			pMenu.Controls.Add(pNavegation);
			pMenu.Controls.Add(iSponsor);
			pMenu.Dock = System.Windows.Forms.DockStyle.Top;
			pMenu.ForeColor = System.Drawing.Color.Silver;
			pMenu.Location = new System.Drawing.Point(0, 0);
			pMenu.Name = "pMenu";
			pMenu.Size = new System.Drawing.Size(1024, 50);
			pMenu.TabIndex = 15;
			pGETTON.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			pGETTON.Controls.Add(lGETTON);
			pGETTON.Controls.Add(bGETTON);
			pGETTON.Location = new System.Drawing.Point(263, 0);
			pGETTON.Name = "pGETTON";
			pGETTON.Size = new System.Drawing.Size(595, 50);
			pGETTON.TabIndex = 17;
			lGETTON.Dock = System.Windows.Forms.DockStyle.Fill;
			lGETTON.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			lGETTON.Font = new System.Drawing.Font("Microsoft Sans Serif", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lGETTON.ForeColor = System.Drawing.Color.Yellow;
			lGETTON.Location = new System.Drawing.Point(0, 0);
			lGETTON.Name = "lGETTON";
			lGETTON.Size = new System.Drawing.Size(518, 50);
			lGETTON.TabIndex = 8;
			lGETTON.Text = "25 points gratuits chaque jour, apuyyez sur jetton >>>";
			lGETTON.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			pTime.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			pTime.Controls.Add(bTime);
			pTime.Controls.Add(eCredits);
			pTime.Location = new System.Drawing.Point(10, 0);
			pTime.Name = "pTime";
			pTime.Size = new System.Drawing.Size(80, 50);
			pTime.TabIndex = 20;
			eCredits.BackColor = System.Drawing.Color.Transparent;
			eCredits.Dock = System.Windows.Forms.DockStyle.Bottom;
			eCredits.ForeColor = System.Drawing.Color.Yellow;
			eCredits.Location = new System.Drawing.Point(0, 30);
			eCredits.Name = "eCredits";
			eCredits.Size = new System.Drawing.Size(80, 20);
			eCredits.TabIndex = 8;
			eCredits.Text = "-";
			eCredits.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			pKeyboard.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			pKeyboard.Controls.Add(pInsertCoin);
			pKeyboard.Controls.Add(bVUp);
			pKeyboard.Controls.Add(bVDown);
			pKeyboard.Controls.Add(bMute);
			pKeyboard.Controls.Add(bKeyboard);
			pKeyboard.Location = new System.Drawing.Point(857, 0);
			pKeyboard.Name = "pKeyboard";
			pKeyboard.Size = new System.Drawing.Size(166, 50);
			pKeyboard.TabIndex = 19;
			pTemps.Controls.Add(rtest);
			pTemps.Controls.Add(lcdClock);
			pTemps.Location = new System.Drawing.Point(0, 0);
			pTemps.Name = "pTemps";
			pTemps.Size = new System.Drawing.Size(81, 50);
			pTemps.TabIndex = 18;
			rtest.Location = new System.Drawing.Point(54, 20);
			rtest.Name = "rtest";
			rtest.Size = new System.Drawing.Size(24, 21);
			rtest.TabIndex = 9;
			rtest.Visible = false;
			lcdClock.BackColor = System.Drawing.Color.Transparent;
			lcdClock.Dock = System.Windows.Forms.DockStyle.Fill;
			lcdClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lcdClock.ForeColor = System.Drawing.Color.Yellow;
			lcdClock.Location = new System.Drawing.Point(0, 0);
			lcdClock.Name = "lcdClock";
			lcdClock.Size = new System.Drawing.Size(81, 50);
			lcdClock.TabIndex = 8;
			lcdClock.Text = "-";
			lcdClock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			pLogin.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			pLogin.Controls.Add(bLogin);
			pLogin.Location = new System.Drawing.Point(836, 0);
			pLogin.Name = "pLogin";
			pLogin.Size = new System.Drawing.Size(50, 50);
			pLogin.TabIndex = 18;
			pTicket.Controls.Add(lCGRAT);
			pTicket.Controls.Add(bBar);
			pTicket.Controls.Add(bTicket);
			pTicket.Location = new System.Drawing.Point(81, 0);
			pTicket.Name = "pTicket";
			pTicket.Size = new System.Drawing.Size(164, 50);
			pTicket.TabIndex = 18;
			lCGRAT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			lCGRAT.ForeColor = System.Drawing.Color.Yellow;
			lCGRAT.Location = new System.Drawing.Point(0, 0);
			lCGRAT.Name = "lCGRAT";
			lCGRAT.Size = new System.Drawing.Size(60, 50);
			lCGRAT.TabIndex = 8;
			lCGRAT.Text = "-";
			lCGRAT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lCGRAT.Visible = false;
			pNavegation.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			pNavegation.Controls.Add(bHome);
			pNavegation.Controls.Add(tURL);
			pNavegation.Controls.Add(bBack);
			pNavegation.Controls.Add(bForward);
			pNavegation.Controls.Add(bGo);
			pNavegation.Location = new System.Drawing.Point(263, 0);
			pNavegation.Name = "pNavegation";
			pNavegation.Size = new System.Drawing.Size(598, 50);
			pNavegation.TabIndex = 18;
			tURL.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			tURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			tURL.Location = new System.Drawing.Point(169, 9);
			tURL.Name = "tURL";
			tURL.Size = new System.Drawing.Size(343, 32);
			tURL.TabIndex = 4;
			tURL.Visible = false;
			tURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tURL_KeyPress);
			iSponsor.AutoSize = true;
			iSponsor.ForeColor = System.Drawing.Color.Green;
			iSponsor.Location = new System.Drawing.Point(267, 17);
			iSponsor.Name = "iSponsor";
			iSponsor.Size = new System.Drawing.Size(82, 13);
			iSponsor.TabIndex = 9;
			iSponsor.Text = "Sponsor area";
			timerCredits.Tick += new System.EventHandler(timerCredits_Tick);
			timerPoll.Tick += new System.EventHandler(timerPoll_Tick);
			timerStartup.Interval = 3000;
			timerStartup.Tick += new System.EventHandler(timerStartup_Tick);
			imgLed.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgLed.ImageStream");
			imgLed.TransparentColor = System.Drawing.Color.Transparent;
			imgLed.Images.SetKeyName(0, "bullet_ball_red.png");
			imgLed.Images.SetKeyName(1, "bullet_ball_green.png");
			imgLed.Images.SetKeyName(2, "bullet_ball_yellow.png");
			imgLed.Images.SetKeyName(3, "bullet_ball_blue.png");
			imgLed.Images.SetKeyName(4, "bullet_ball_blue.png");
			pScreenSaver.BackColor = System.Drawing.Color.Yellow;
			pScreenSaver.Font = new System.Drawing.Font("Microsoft Sans Serif", 24f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			pScreenSaver.ForeColor = System.Drawing.Color.Black;
			pScreenSaver.Location = new System.Drawing.Point(74, 142);
			pScreenSaver.Name = "pScreenSaver";
			pScreenSaver.Padding = new System.Windows.Forms.Padding(0, 0, 12, 40);
			pScreenSaver.Size = new System.Drawing.Size(568, 332);
			pScreenSaver.TabIndex = 24;
			pScreenSaver.Text = "Version 1.0.0 (Status: 100)";
			timerPrinter.Interval = 2000;
			timerPrinter.Tick += new System.EventHandler(timerPrinter_Tick);
			EntraJocs.BackColor = System.Drawing.Color.Transparent;
			EntraJocs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			EntraJocs.Dock = System.Windows.Forms.DockStyle.Top;
			EntraJocs.Location = new System.Drawing.Point(0, 50);
			EntraJocs.Name = "EntraJocs";
			EntraJocs.Size = new System.Drawing.Size(1024, 60);
			EntraJocs.TabIndex = 25;
			EntraJocs.Visible = false;
			EntraJocs.Click += new System.EventHandler(EntraJocs_Click);
			timerMessages.Interval = 1000;
			timerMessages.Tick += new System.EventHandler(timerMessages_Tick);
			lCALL.BackColor = System.Drawing.Color.Yellow;
			lCALL.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lCALL.ForeColor = System.Drawing.Color.Black;
			lCALL.Location = new System.Drawing.Point(0, 304);
			lCALL.Name = "lCALL";
			lCALL.Size = new System.Drawing.Size(1024, 150);
			lCALL.TabIndex = 26;
			lCALL.Text = "-";
			lCALL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			bGETTON.Dock = System.Windows.Forms.DockStyle.Right;
			bGETTON.FlatAppearance.BorderSize = 0;
			bGETTON.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bGETTON.Image = Kiosk.Properties.Resources.getton;
			bGETTON.Location = new System.Drawing.Point(518, 0);
			bGETTON.Name = "bGETTON";
			bGETTON.Size = new System.Drawing.Size(77, 50);
			bGETTON.TabIndex = 5;
			bGETTON.UseVisualStyleBackColor = true;
			bGETTON.Click += new System.EventHandler(bGETTON_Click);
			bTime.Dock = System.Windows.Forms.DockStyle.Fill;
			bTime.FlatAppearance.BorderSize = 0;
			bTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bTime.Image = Kiosk.Properties.Resources.ico_clock;
			bTime.Location = new System.Drawing.Point(0, 0);
			bTime.Name = "bTime";
			bTime.Size = new System.Drawing.Size(80, 30);
			bTime.TabIndex = 7;
			bTime.UseVisualStyleBackColor = true;
			bTime.Click += new System.EventHandler(bTime_Click);
			pInsertCoin.Location = new System.Drawing.Point(143, 13);
			pInsertCoin.Name = "pInsertCoin";
			pInsertCoin.Size = new System.Drawing.Size(22, 24);
			pInsertCoin.TabIndex = 10;
			pInsertCoin.TabStop = false;
			bVUp.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			bVUp.FlatAppearance.BorderSize = 0;
			bVUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bVUp.Image = Kiosk.Properties.Resources.ico_volmas;
			bVUp.Location = new System.Drawing.Point(74, 4);
			bVUp.Name = "bVUp";
			bVUp.Size = new System.Drawing.Size(32, 43);
			bVUp.TabIndex = 9;
			bVUp.UseVisualStyleBackColor = true;
			bVUp.Click += new System.EventHandler(bVUp_Click);
			bVDown.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			bVDown.FlatAppearance.BorderSize = 0;
			bVDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bVDown.Image = Kiosk.Properties.Resources.ico_volmen;
			bVDown.Location = new System.Drawing.Point(42, 4);
			bVDown.Name = "bVDown";
			bVDown.Size = new System.Drawing.Size(32, 43);
			bVDown.TabIndex = 8;
			bVDown.UseVisualStyleBackColor = true;
			bVDown.Click += new System.EventHandler(bVDown_Click);
			bMute.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			bMute.FlatAppearance.BorderSize = 0;
			bMute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bMute.Image = Kiosk.Properties.Resources.ico_volmute;
			bMute.Location = new System.Drawing.Point(10, 4);
			bMute.Name = "bMute";
			bMute.Size = new System.Drawing.Size(32, 43);
			bMute.TabIndex = 7;
			bMute.UseVisualStyleBackColor = true;
			bMute.Click += new System.EventHandler(bMute_Click);
			bKeyboard.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			bKeyboard.FlatAppearance.BorderSize = 0;
			bKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bKeyboard.Image = Kiosk.Properties.Resources.ico_osk1;
			bKeyboard.Location = new System.Drawing.Point(106, 4);
			bKeyboard.Name = "bKeyboard";
			bKeyboard.Size = new System.Drawing.Size(36, 43);
			bKeyboard.TabIndex = 6;
			bKeyboard.UseVisualStyleBackColor = true;
			bKeyboard.Click += new System.EventHandler(bKeyboard_Click);
			bLogin.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			bLogin.FlatAppearance.BorderSize = 0;
			bLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bLogin.Image = Kiosk.Properties.Resources.ico_userd;
			bLogin.Location = new System.Drawing.Point(1, 4);
			bLogin.Name = "bLogin";
			bLogin.Size = new System.Drawing.Size(48, 43);
			bLogin.TabIndex = 7;
			bLogin.UseVisualStyleBackColor = true;
			bLogin.Click += new System.EventHandler(bLogin_Click);
			bBar.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			bBar.FlatAppearance.BorderSize = 0;
			bBar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bBar.Image = Kiosk.Properties.Resources.ico_barcode;
			bBar.Location = new System.Drawing.Point(62, 4);
			bBar.Name = "bBar";
			bBar.Size = new System.Drawing.Size(48, 43);
			bBar.TabIndex = 7;
			bBar.UseVisualStyleBackColor = true;
			bBar.Click += new System.EventHandler(bBar_Click);
			bTicket.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			bTicket.FlatAppearance.BorderSize = 0;
			bTicket.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bTicket.Image = Kiosk.Properties.Resources.ico_ticket3;
			bTicket.Location = new System.Drawing.Point(112, 4);
			bTicket.Name = "bTicket";
			bTicket.Size = new System.Drawing.Size(48, 43);
			bTicket.TabIndex = 6;
			bTicket.UseVisualStyleBackColor = true;
			bTicket.Click += new System.EventHandler(bTicket_Click);
			bHome.FlatAppearance.BorderSize = 0;
			bHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bHome.Image = (System.Drawing.Image)resources.GetObject("bHome.Image");
			bHome.Location = new System.Drawing.Point(7, 4);
			bHome.Name = "bHome";
			bHome.Size = new System.Drawing.Size(48, 43);
			bHome.TabIndex = 0;
			bHome.UseVisualStyleBackColor = true;
			bHome.Click += new System.EventHandler(bHome_Click);
			bBack.FlatAppearance.BorderSize = 0;
			bBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bBack.Image = Kiosk.Properties.Resources.ico_left_green;
			bBack.Location = new System.Drawing.Point(67, 4);
			bBack.Name = "bBack";
			bBack.Size = new System.Drawing.Size(48, 43);
			bBack.TabIndex = 2;
			bBack.UseVisualStyleBackColor = true;
			bForward.FlatAppearance.BorderSize = 0;
			bForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bForward.Image = Kiosk.Properties.Resources.ico_right_green;
			bForward.Location = new System.Drawing.Point(115, 4);
			bForward.Name = "bForward";
			bForward.Size = new System.Drawing.Size(48, 43);
			bForward.TabIndex = 3;
			bForward.UseVisualStyleBackColor = true;
			bGo.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			bGo.FlatAppearance.BorderSize = 0;
			bGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			bGo.Image = Kiosk.Properties.Resources.ico_ok;
			bGo.Location = new System.Drawing.Point(518, 4);
			bGo.Name = "bGo";
			bGo.Size = new System.Drawing.Size(48, 43);
			bGo.TabIndex = 5;
			bGo.UseVisualStyleBackColor = true;
			bGo.Click += new System.EventHandler(bGo_Click);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.Color.Black;
			base.ClientSize = new System.Drawing.Size(1024, 600);
			base.Controls.Add(EntraJocs);
			base.Controls.Add(lCALL);
			base.Controls.Add(pScreenSaver);
			base.Controls.Add(pMenu);
			DoubleBuffered = true;
			Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "MainWindow";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			Text = "Kiosk";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MainWindow_FormClosing);
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MainWindow_FormClosed);
			base.Load += new System.EventHandler(MainWindow_Load);
			base.SizeChanged += new System.EventHandler(MainWindow_SizeChanged);
			base.Paint += new System.Windows.Forms.PaintEventHandler(MainWindow_Paint);
			pMenu.ResumeLayout(false);
			pMenu.PerformLayout();
			pGETTON.ResumeLayout(false);
			pTime.ResumeLayout(false);
			pKeyboard.ResumeLayout(false);
			pTemps.ResumeLayout(false);
			pLogin.ResumeLayout(false);
			pTicket.ResumeLayout(false);
			pNavegation.ResumeLayout(false);
			pNavegation.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pInsertCoin).EndInit();
			ResumeLayout(false);
		}
	}
}
