using Kiosk.Properties;
using SimpleWifi;
using SimpleWifi.Win32.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace Kiosk
{
	public class DLG_Wifi : Form
	{
		private bool RefreshNet;

		public bool OK;

		public bool IsClosed;

		private Wifi wifi;

		private int lastindex = -1;

		private IContainer components;

		private Panel pBOTTOM;

		private Button bOk;

		private Button bFind;

		public ListBox listWifi;

		private Button bDisconnect;

		private Button bConnect;

		private Timer timerNet;

		private ToolTip toolTip1;

		public DLG_Wifi()
		{
			IsClosed = false;
			OK = false;
			InitializeComponent();
			Localize();
			wifi = new Wifi();
			wifi.ConnectionStatusChanged += wifi_ConnectionStatusChanged;
			NetworkChange.NetworkAddressChanged += AddressChangedCallback;
			RefreshNet = true;
		}

		public void AddressChangedCallback(object sender, EventArgs e)
		{
			RefreshNet = true;
		}

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		private void wifi_ConnectionStatusChanged(object sender, WifiStatusEventArgs e)
		{
			RefreshNet = true;
		}

		private void Disconnect()
		{
			wifi.Disconnect();
		}

		private bool Status()
		{
			if (wifi.ConnectionStatus == WifiStatus.Connected)
			{
				return true;
			}
			return false;
		}

		private bool Connect_Forced(string _info)
		{
			return false;
		}

		private bool Connect(string _info, bool _auto = true)
		{
			string[] array = _info.Split(',');
			return Connect(array[0], _over: false, null, null, null, null, null, _auto);
		}

		private bool Connect(string _info, string _sec = null, bool _auto = true)
		{
			string[] array = _info.Split(',');
			return Connect(array[0], _over: false, null, null, null, null, _sec, _auto);
		}

		private bool Connect(string _info, bool _over, string _sec = null, bool _auto = true)
		{
			string[] array = _info.Split(',');
			return Connect(array[0], _over, null, null, null, null, _sec, _auto);
		}

		private bool Connect(string _info, string _password, string _sec = null, bool _auto = true)
		{
			string[] array = _info.Split(',');
			return Connect(array[0], _over: true, null, _password, null, null, _sec, _auto);
		}

		private bool Connect(string _info, string _password, string _rename, string _sec, bool _auto = true)
		{
			string[] array = _info.Split(',');
			return Connect(array[0], _over: true, null, _password, null, _rename, _sec, _auto);
		}

		private bool Connect(string _name, bool _over, string _username, string _password, string _domain, string _rename, string _sec = null, bool _auto = true)
		{
			IEnumerable<AccessPoint> source = List();
			AccessPoint accessPoint = null;
			int num = 1;
			for (int i = 0; i < source.ToArray().Length; i++)
			{
				string text = "";
				if (!string.IsNullOrEmpty(source.ToList()[i].Name))
				{
					text = source.ToList()[i].Name;
				}
				else if (source.ToList()[i]._network.dot11BssType == Dot11BssType.Infrastructure && source.ToList()[i]._network.networkConnectable)
				{
					switch (source.ToList()[i]._network.dot11DefaultAuthAlgorithm)
					{
					case Dot11AuthAlgorithm.IEEE80211_SharedKey:
						text = "<HIDEN WEP> " + num;
						num++;
						break;
					case Dot11AuthAlgorithm.IEEE80211_Open:
						text = "<HIDEN OPEN> " + num;
						num++;
						break;
					case Dot11AuthAlgorithm.RSNA_PSK:
						text = "<HIDEN RSNA PSK> " + num;
						num++;
						break;
					case Dot11AuthAlgorithm.RSNA:
						text = "<HIDEN RSNA> " + num;
						num++;
						break;
					case Dot11AuthAlgorithm.WPA:
						text = "<HIDEN WPA> " + num;
						num++;
						break;
					case Dot11AuthAlgorithm.WPA_PSK:
						text = "<HIDEN WPA PSK> " + num;
						num++;
						break;
					}
				}
				if (text.ToLower() == _name.ToLower())
				{
					accessPoint = source.ToList()[i];
					break;
				}
			}
			if (accessPoint == null)
			{
				return false;
			}
			if (!string.IsNullOrEmpty(_rename))
			{
				accessPoint._network.dot11Ssid.SSIDLength = (uint)_rename.Length;
				byte[] array = new byte[32];
				for (int j = 0; j < 32; j++)
				{
					array[j] = 0;
				}
				byte[] bytes = Encoding.ASCII.GetBytes(_rename);
				for (int k = 0; k < bytes.Length; k++)
				{
					array[k] = bytes[k];
				}
				accessPoint._network.dot11Ssid.SSID = array;
				accessPoint._network.profileName = _rename;
			}
			switch (_sec)
			{
			case "IEEE80211_Open":
				accessPoint._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.IEEE80211_Open;
				break;
			case "IEEE80211_SharedKey":
				accessPoint._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.IEEE80211_SharedKey;
				break;
			case "WPA":
				accessPoint._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.WPA;
				break;
			case "WPA_PSK":
				accessPoint._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.WPA_PSK;
				break;
			case "RSNA":
				accessPoint._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.RSNA;
				break;
			case "RSNA_PSK":
				accessPoint._network.dot11DefaultAuthAlgorithm = Dot11AuthAlgorithm.RSNA_PSK;
				break;
			}
			AuthRequest authRequest = new AuthRequest(accessPoint);
			bool flag = _over;
			if (authRequest.IsPasswordRequired && flag)
			{
				if (authRequest.IsUsernameRequired && !string.IsNullOrEmpty(_username))
				{
					authRequest.Username = _username;
				}
				authRequest.Password = _password;
				if (authRequest.IsDomainSupported && !string.IsNullOrEmpty(_domain))
				{
					authRequest.Domain = _domain;
				}
			}
			authRequest.AutoConnect = _auto;
			accessPoint.ConnectAsync(authRequest, flag, OnConnectedComplete);
			return true;
		}

		private void OnConnectedComplete(bool success)
		{
			RefreshNet = true;
		}

		public IEnumerable<AccessPoint> List()
		{
			IEnumerable<AccessPoint> enumerable = from ap in wifi.GetAccessPoints()
				orderby ap.SignalStrength descending
				select ap;
			int num = 1;
			foreach (AccessPoint item in enumerable)
			{
				if (!string.IsNullOrEmpty(item.Name))
				{
					listWifi.Items.Add(item.Name + "," + item.SignalStrength + "," + item.IsConnected + "," + item._network.dot11DefaultAuthAlgorithm.ToString());
				}
				else if (item._network.dot11BssType == Dot11BssType.Infrastructure && item._network.networkConnectable)
				{
					switch (item._network.dot11DefaultAuthAlgorithm)
					{
					case Dot11AuthAlgorithm.IEEE80211_Open:
						listWifi.Items.Add("<HIDEN OPEN> " + num + "," + item.SignalStrength + ",False," + item._network.dot11DefaultAuthAlgorithm.ToString());
						num++;
						break;
					case Dot11AuthAlgorithm.IEEE80211_SharedKey:
						listWifi.Items.Add("<HIDEN WEP> " + num + "," + item.SignalStrength + ",False," + item._network.dot11DefaultAuthAlgorithm.ToString());
						num++;
						break;
					case Dot11AuthAlgorithm.RSNA_PSK:
						listWifi.Items.Add("<HIDEN RSNA PSK> " + num + "," + item.SignalStrength + ",False," + item._network.dot11DefaultAuthAlgorithm.ToString());
						num++;
						break;
					case Dot11AuthAlgorithm.RSNA:
						listWifi.Items.Add("<HIDEN RSNA> " + num + "," + item.SignalStrength + ",False," + item._network.dot11DefaultAuthAlgorithm.ToString());
						num++;
						break;
					case Dot11AuthAlgorithm.WPA:
						listWifi.Items.Add("<HIDEN WPA> " + num + "," + item.SignalStrength + ",False," + item._network.dot11DefaultAuthAlgorithm.ToString());
						num++;
						break;
					case Dot11AuthAlgorithm.WPA_PSK:
						listWifi.Items.Add("<HIDEN WPA PSK> " + num + "," + item.SignalStrength + ",False," + item._network.dot11DefaultAuthAlgorithm.ToString());
						num++;
						break;
					}
				}
			}
			listWifi.Invalidate();
			return enumerable;
		}

		private bool DeleteProfile(string _name)
		{
			IEnumerable<AccessPoint> source = List();
			for (int i = 0; i < source.ToArray().Length; i++)
			{
				if (source.ToList()[i].Name.ToLower() == _name.ToLower())
				{
					AccessPoint accessPoint = source.ToList()[i];
					accessPoint.DeleteProfile();
					return true;
				}
			}
			return false;
		}

		private AccessPoint Info(string _name)
		{
			IEnumerable<AccessPoint> source = List();
			for (int i = 0; i < source.ToArray().Length; i++)
			{
				if (source.ToList()[i].Name.ToLower() == _name.ToLower())
				{
					return source.ToList()[i];
				}
			}
			return null;
		}

		private void bFind_Click(object sender, EventArgs e)
		{
			RefreshNet = true;
		}

		private void bOk_Click(object sender, EventArgs e)
		{
			OK = true;
			Close();
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			OK = false;
			Close();
		}

		private void DLG_Wifi_FormClosed(object sender, FormClosedEventArgs e)
		{
			IsClosed = true;
		}

		private void listWifi_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.Graphics.FillRectangle(Brushes.Black, e.Bounds);
			Rectangle bounds = e.Bounds;
			bounds.X++;
			bounds.Y++;
			bounds.Width -= 2;
			bounds.Height -= 2;
			Brush brush = Brushes.White;
			string[] array = (e.Index < 0) ? new string[3]
			{
				"NO WIFI",
				"0",
				"false"
			} : listWifi.Items[e.Index].ToString().Split(',');
			if (array[2].ToLower() == "true".ToLower())
			{
				brush = Brushes.Green;
			}
			e.Graphics.FillRectangle(brush, bounds);
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e.Graphics.FillRectangle(Brushes.Blue, bounds);
			}
			Rectangle rect = bounds;
			rect.Y = bounds.Y + bounds.Height - 24;
			rect.Height = 24;
			e.Graphics.FillRectangle(Brushes.Red, rect);
			int num = 0;
			try
			{
				num = int.Parse(array[1]);
			}
			catch
			{
			}
			rect = bounds;
			rect.Y = bounds.Y + bounds.Height - 24;
			rect.Height = 24;
			int num3 = rect.Width = rect.Width * num / 100;
			Brush brush2 = Brushes.OrangeRed;
			if (num > 50)
			{
				brush2 = Brushes.Yellow;
			}
			if (num > 75)
			{
				brush2 = Brushes.YellowGreen;
			}
			e.Graphics.FillRectangle(brush2, rect);
			e.Graphics.MeasureString(array[1] + "%", e.Font);
			rect.Width = bounds.Width;
			rect.X += 32;
			e.Graphics.DrawString(array[1] + "%", e.Font, Brushes.Black, rect, StringFormat.GenericDefault);
			rect = bounds;
			rect.Y += 8;
			rect.X += 32;
			string text = array[0];
			if (array[2].ToLower() == "true".ToLower())
			{
				text += " -> CONNECT";
			}
			e.Graphics.DrawString(text, e.Font, Brushes.Black, rect, StringFormat.GenericDefault);
		}

		private void DLG_Wifi_Load(object sender, EventArgs e)
		{
			timerNet.Enabled = true;
			RefreshNet = true;
		}

		private void bConnect_Click(object sender, EventArgs e)
		{
			if (listWifi.SelectedIndex == -1)
			{
				MSG_Ok mSG_Ok = new MSG_Ok("Select any Wifi");
				mSG_Ok.ShowDialog();
				mSG_Ok.Dispose();
				RefreshNet = true;
				return;
			}
			string[] array = listWifi.Items[listWifi.SelectedIndex].ToString().Split(',');
			if (array.Length < 1)
			{
				RefreshNet = true;
				return;
			}
			DLG_Wifi_Connect dLG_Wifi_Connect = new DLG_Wifi_Connect(array[0], "?", array[3]);
			dLG_Wifi_Connect.ShowDialog();
			_ = dLG_Wifi_Connect.NewProfile;
			bool alwaysConnect = dLG_Wifi_Connect.AlwaysConnect;
			if (!dLG_Wifi_Connect.OK)
			{
				RefreshNet = true;
				return;
			}
			Disconnect();
			if (string.IsNullOrEmpty(dLG_Wifi_Connect.Password) || dLG_Wifi_Connect.Password == "?")
			{
				Connect(dLG_Wifi_Connect.SID, alwaysConnect);
			}
			else if (dLG_Wifi_Connect.SID != array[0])
			{
				Connect(array[0] + ",100,False", dLG_Wifi_Connect.Password, dLG_Wifi_Connect.SID, dLG_Wifi_Connect.SEC, alwaysConnect);
			}
			else
			{
				Connect(array[0] + ",100,False", dLG_Wifi_Connect.Password, alwaysConnect);
			}
			RefreshNet = true;
		}

		private void listWifi_DoubleClick(object sender, EventArgs e)
		{
			Disconnect();
			Connect(listWifi.Items[listWifi.SelectedIndex].ToString(), _auto: true);
			RefreshNet = true;
		}

		private void bDisconnect_Click(object sender, EventArgs e)
		{
			Disconnect();
			RefreshNet = true;
		}

		private void timerNet_Tick(object sender, EventArgs e)
		{
			if (RefreshNet)
			{
				listWifi.Items.Clear();
				List();
				RefreshNet = false;
			}
		}

		private void DLG_Wifi_FormClosing(object sender, FormClosingEventArgs e)
		{
			RefreshNet = false;
			timerNet.Enabled = false;
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
			pBOTTOM = new System.Windows.Forms.Panel();
			bDisconnect = new System.Windows.Forms.Button();
			bConnect = new System.Windows.Forms.Button();
			bFind = new System.Windows.Forms.Button();
			bOk = new System.Windows.Forms.Button();
			listWifi = new System.Windows.Forms.ListBox();
			timerNet = new System.Windows.Forms.Timer(components);
			toolTip1 = new System.Windows.Forms.ToolTip(components);
			pBOTTOM.SuspendLayout();
			SuspendLayout();
			pBOTTOM.Controls.Add(bDisconnect);
			pBOTTOM.Controls.Add(bConnect);
			pBOTTOM.Controls.Add(bFind);
			pBOTTOM.Controls.Add(bOk);
			pBOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
			pBOTTOM.Location = new System.Drawing.Point(0, 379);
			pBOTTOM.Name = "pBOTTOM";
			pBOTTOM.Size = new System.Drawing.Size(807, 48);
			pBOTTOM.TabIndex = 5;
			bDisconnect.Dock = System.Windows.Forms.DockStyle.Left;
			bDisconnect.Image = Kiosk.Properties.Resources.ico_wifi_disconnect;
			bDisconnect.Location = new System.Drawing.Point(96, 0);
			bDisconnect.Name = "bDisconnect";
			bDisconnect.Size = new System.Drawing.Size(48, 48);
			bDisconnect.TabIndex = 4;
			toolTip1.SetToolTip(bDisconnect, "Disconnect current Wi-Fi connection");
			bDisconnect.UseVisualStyleBackColor = true;
			bDisconnect.Click += new System.EventHandler(bDisconnect_Click);
			bConnect.Dock = System.Windows.Forms.DockStyle.Left;
			bConnect.Image = Kiosk.Properties.Resources.ico_wifi_connect;
			bConnect.Location = new System.Drawing.Point(48, 0);
			bConnect.Name = "bConnect";
			bConnect.Size = new System.Drawing.Size(48, 48);
			bConnect.TabIndex = 3;
			toolTip1.SetToolTip(bConnect, "Connect to selected Wi-Fi");
			bConnect.UseVisualStyleBackColor = true;
			bConnect.Click += new System.EventHandler(bConnect_Click);
			bFind.Dock = System.Windows.Forms.DockStyle.Left;
			bFind.Image = Kiosk.Properties.Resources.ico_wifi_find;
			bFind.Location = new System.Drawing.Point(0, 0);
			bFind.Name = "bFind";
			bFind.Size = new System.Drawing.Size(48, 48);
			bFind.TabIndex = 2;
			toolTip1.SetToolTip(bFind, "Find new Wi-Fi connections");
			bFind.UseVisualStyleBackColor = true;
			bFind.Click += new System.EventHandler(bFind_Click);
			bOk.Dock = System.Windows.Forms.DockStyle.Right;
			bOk.Image = Kiosk.Properties.Resources.ico_ok;
			bOk.Location = new System.Drawing.Point(759, 0);
			bOk.Name = "bOk";
			bOk.Size = new System.Drawing.Size(48, 48);
			bOk.TabIndex = 0;
			bOk.UseVisualStyleBackColor = true;
			bOk.Click += new System.EventHandler(bOk_Click);
			listWifi.Dock = System.Windows.Forms.DockStyle.Fill;
			listWifi.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			listWifi.Font = new System.Drawing.Font("Microsoft Sans Serif", 16f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			listWifi.FormattingEnabled = true;
			listWifi.ItemHeight = 64;
			listWifi.Location = new System.Drawing.Point(0, 0);
			listWifi.Name = "listWifi";
			listWifi.Size = new System.Drawing.Size(807, 379);
			listWifi.TabIndex = 6;
			listWifi.DrawItem += new System.Windows.Forms.DrawItemEventHandler(listWifi_DrawItem);
			listWifi.DoubleClick += new System.EventHandler(listWifi_DoubleClick);
			timerNet.Tick += new System.EventHandler(timerNet_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(807, 427);
			base.ControlBox = false;
			base.Controls.Add(listWifi);
			base.Controls.Add(pBOTTOM);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "DLG_Wifi";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = " ";
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DLG_Wifi_FormClosing);
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(DLG_Wifi_FormClosed);
			base.Load += new System.EventHandler(DLG_Wifi_Load);
			pBOTTOM.ResumeLayout(false);
			ResumeLayout(false);
		}
	}
}
