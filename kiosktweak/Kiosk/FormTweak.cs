using Kiosk.Properties;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Kiosk
{
	public class FormTweak : Form
	{
		private string[] list_drv = new string[44]
		{
			"CUSTOM DRIVERS",
			"driver_custom.exe",
			"NIPON DRIVERS",
			"driver_nipon.exe",
			"SANEI DRIVERS",
			"driver_sanei.exe",
			"COMESTERO DRIVERS",
			"driver_comestero.exe",
			"COMESTERO TOOLS 1/3",
			"ClonePro1.23.exe",
			"COMESTERO TOOLS 2/3",
			"Secicctalk1.5_setup.exe",
			"COMESTERO TOOLS 3/3",
			"Multiconfig.zip",
			"AUTOMATED DRIVERS",
			"driver_nv.exe",
			"AUTOMATED TOOLS 1/2",
			"ValidatorManager 3.3.13.msi",
			"AUTOMATED TOOLS 2/2",
			"ValidatorManager_4_3_3_206.zip",
			"ELO DRIVERS",
			"driver_elo.exe",
			"ETWO DRIVERS",
			"driver_etwotouch.msi",
			"GENERAL TOUCH DRIVERS",
			"driver_generaltouch.exe",
			"TALENT TOUCH DRIVERS",
			"driver_talenttouch.exe",
			"MICROTOUCH DRIVERS",
			"driver_microtouch.exe",
			"EGALAXY DRIVERS",
			"driver_egalaxtouch.exe",
			".NET FRAMEWORK",
			"net61.exe",
			"VC RUNTIME",
			"vc2013.exe",
			"FLASH RUNTIME",
			"flash_activex.exe",
			"FLASH RUNTIME",
			"flash_plugin.exe",
			"JAVA RUNTIME",
			"java8_runtime.exe",
			"AMD/ATI DRIVER",
			"AMD_ATI.exe"
		};

		private IContainer components;

		private Button bVNC;

		private Button bCalib;

		private Button bPrinter;

		private Button bCancel;

		private Button bPorts;

		private Button bInfo;

		private Button bWifi;

		private Button bClean;

		private Button bNTP;

		private Button bUPDATE;

		private Button bDELETEDATA;

		private Button bLOCK;

		private Button bUNLOCK;

		public FormTweak()
		{
			InitializeComponent();
			Text = "Kiosk Tweak v1.50";
		}

		private void bCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void bVNC_Click(object sender, EventArgs e)
		{
			DLG_VNC dLG_VNC = new DLG_VNC();
			dLG_VNC.ShowDialog();
		}

		private void bCalib_Click(object sender, EventArgs e)
		{
			DLG_Calibrar dLG_Calibrar = new DLG_Calibrar();
			dLG_Calibrar.ShowDialog();
		}

		private void bPrinter_Click(object sender, EventArgs e)
		{
			DLG_Printer dLG_Printer = new DLG_Printer();
			dLG_Printer.ShowDialog();
		}

		private void bPorts_Click(object sender, EventArgs e)
		{
			DLG_Ports dLG_Ports = new DLG_Ports();
			dLG_Ports.ShowDialog();
		}

		private void bInfo_Click(object sender, EventArgs e)
		{
			DLG_Info dLG_Info = new DLG_Info();
			dLG_Info.ShowDialog();
		}

		private void bWifi_Click(object sender, EventArgs e)
		{
			DLG_Wifi dLG_Wifi = new DLG_Wifi();
			dLG_Wifi.ShowDialog();
		}

		public void Missatge(string Message, string Caption, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Hand, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
		{
			MessageBox.Show(Message, Caption, buttons, icon, defaultButton, (MessageBoxOptions)8192);
		}

		private void bClean_Click(object sender, EventArgs e)
		{
			int num = 0;
			Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string str = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).Replace(" (x86)", "");
			Environment.GetFolderPath(Environment.SpecialFolder.System);
			string str2 = "c:\\Kiosk";
			string text = str + "\\CCleaner\\ccleaner.exe";
			Splash splash = new Splash("Cleanning System...");
			if (File.Exists(text))
			{
				splash.Show();
				splash.Invalidate();
				Application.DoEvents();
				Process process = Process.Start(text, "/AUTO");
				Thread.Sleep(1000);
				process.WaitForExit();
				num = 1;
				splash.Hide();
				MessageBox.Show("Clean Finish");
			}
			if (num == 0)
			{
				text = str2 + "\\ccleaner.exe";
				if (File.Exists(text))
				{
					splash.Show();
					splash.Invalidate();
					Application.DoEvents();
					Process process2 = Process.Start(text, "/AUTO");
					Thread.Sleep(1000);
					process2.WaitForExit();
					num = 1;
					splash.Hide();
					MessageBox.Show("Clean Finish");
				}
			}
			splash.Dispose();
			if (num == 0)
			{
				MessageBox.Show("Missing CCleaner");
			}
		}

		private void bNTP_Click(object sender, EventArgs e)
		{
			string str = "";
			Process process = new Process();
			string[] array = new string[12]
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
			for (int i = 0; i < array.Length; i += 2)
			{
				try
				{
					process.StartInfo.FileName = array[i];
					process.StartInfo.Arguments = array[i + 1];
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} '{process.StartInfo.FileName}','{process.StartInfo.Arguments}'\r\n";
					process.Start();
					Thread.Sleep(1000);
					process.WaitForExit();
					str += $"CONFIG {i / 2 + 1}/{array.Length / 2} OK\r\n";
				}
				catch (Exception ex)
				{
					str += $"ERROR CFG: '{process.StartInfo.FileName}','{process.StartInfo.Arguments}','{ex.Message}'\r\n";
					MessageBox.Show("Can not update Date/Time");
					return;
				}
			}
			MessageBox.Show("Date/Time updated");
		}

		public static void Lock_USB()
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\USBSTOR", writable: true);
				registryKey.SetValue("Start", 4, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
		}

		public static void UnLock_USB()
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\USBSTOR", writable: true);
				registryKey.SetValue("Start", 3, RegistryValueKind.DWord);
				registryKey.Close();
			}
			catch
			{
			}
		}

		public static bool CheckPaths(string _path)
		{
			if (!Directory.Exists(_path))
			{
				try
				{
					Directory.CreateDirectory(_path);
				}
				catch
				{
					return false;
				}
			}
			return true;
		}

		public static bool FTP_Download(string _url, string _file, ref MessageWait w)
		{
			CheckPaths("c:\\drivers\\");
			try
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)WebRequest.Create(_url);
				ftpWebRequest.Method = "RETR";
				ftpWebRequest.Credentials = new NetworkCredential("install", (string)null);
				ftpWebRequest.UsePassive = true;
				ftpWebRequest.UseBinary = true;
				ftpWebRequest.KeepAlive = false;
				Stream responseStream = ftpWebRequest.GetResponse().GetResponseStream();
				if (File.Exists("c:\\drivers\\" + _file))
				{
					try
					{
						File.Delete("c:\\drivers\\" + _file);
					}
					catch (Exception ex)
					{
						MessageBox.Show("FTP DELETE ERROR: " + ex.Message + " [" + _file + "]");
						return false;
					}
				}
				BinaryWriter binaryWriter = new BinaryWriter(File.Open("c:\\drivers\\" + _file, FileMode.CreateNew));
				int num = 0;
				int num2 = 0;
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
						num2 += num;
						binaryWriter.Write(array, 0, num);
						w.UpdateInfo(num2);
					}
					Application.DoEvents();
				}
				while (num != 0);
				binaryWriter.Flush();
				binaryWriter.Close();
			}
			catch (Exception ex2)
			{
				MessageBox.Show("FTP ERROR: " + ex2.Message + " [" + _file + "]");
				return false;
			}
			return true;
		}

		private void bUPDATE_Click(object sender, EventArgs e)
		{
			MessageWait w = new MessageWait("Download drivers");
			w.UpdateInfo(-2);
			w.Show();
			for (int i = 0; i < 10; i++)
			{
				Application.DoEvents();
				Thread.Sleep(200);
			}
			for (int j = 0; j < list_drv.Length; j += 2)
			{
				w.UpdateMSG("Download " + list_drv[j]);
				FTP_Download("ftp://ftp.jogarvip.com/" + list_drv[j + 1], list_drv[j + 1], ref w);
			}
			w.Close();
			MessageBox.Show("Drivers downloades. Stored in 'c:\\drivers'");
		}

		private void bDELETEDATA_Click(object sender, EventArgs e)
		{
			string str = "kiosk.options";
			string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\devices.cfg";
			string path2 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + str;
			string path3 = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + str + ".tmp";
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
			try
			{
				File.Delete(path2);
			}
			catch
			{
			}
			try
			{
				File.Delete(path3);
			}
			catch
			{
			}
			MessageBox.Show("Old kiosk info deleted");
		}

		private void bLOCK_Click(object sender, EventArgs e)
		{
			Lock_USB();
			MessageBox.Show("Lock access to USB disk devices");
		}

		private void bUNLOCK_Click(object sender, EventArgs e)
		{
			UnLock_USB();
			MessageBox.Show("UnLock access to USB disk devices");
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
			bUNLOCK = new System.Windows.Forms.Button();
			bLOCK = new System.Windows.Forms.Button();
			bDELETEDATA = new System.Windows.Forms.Button();
			bUPDATE = new System.Windows.Forms.Button();
			bNTP = new System.Windows.Forms.Button();
			bClean = new System.Windows.Forms.Button();
			bWifi = new System.Windows.Forms.Button();
			bInfo = new System.Windows.Forms.Button();
			bPorts = new System.Windows.Forms.Button();
			bCancel = new System.Windows.Forms.Button();
			bPrinter = new System.Windows.Forms.Button();
			bCalib = new System.Windows.Forms.Button();
			bVNC = new System.Windows.Forms.Button();
			SuspendLayout();
			bUNLOCK.Image = Kiosk.Properties.Resources.lock_open;
			bUNLOCK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bUNLOCK.Location = new System.Drawing.Point(261, 252);
			bUNLOCK.Name = "bUNLOCK";
			bUNLOCK.Size = new System.Drawing.Size(243, 48);
			bUNLOCK.TabIndex = 12;
			bUNLOCK.Text = "UnLock access to USB disk";
			bUNLOCK.UseVisualStyleBackColor = true;
			bUNLOCK.Click += new System.EventHandler(bUNLOCK_Click);
			bLOCK.Image = Kiosk.Properties.Resources._lock;
			bLOCK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bLOCK.Location = new System.Drawing.Point(12, 252);
			bLOCK.Name = "bLOCK";
			bLOCK.Size = new System.Drawing.Size(243, 48);
			bLOCK.TabIndex = 11;
			bLOCK.Text = "Lock access to USB disk";
			bLOCK.UseVisualStyleBackColor = true;
			bLOCK.Click += new System.EventHandler(bLOCK_Click);
			bDELETEDATA.Image = Kiosk.Properties.Resources.ico_brush;
			bDELETEDATA.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bDELETEDATA.Location = new System.Drawing.Point(261, 204);
			bDELETEDATA.Name = "bDELETEDATA";
			bDELETEDATA.Size = new System.Drawing.Size(243, 48);
			bDELETEDATA.TabIndex = 10;
			bDELETEDATA.Text = "Clean old kiosk data";
			bDELETEDATA.UseVisualStyleBackColor = true;
			bDELETEDATA.Click += new System.EventHandler(bDELETEDATA_Click);
			bUPDATE.Image = Kiosk.Properties.Resources.box_tall;
			bUPDATE.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bUPDATE.Location = new System.Drawing.Point(12, 204);
			bUPDATE.Name = "bUPDATE";
			bUPDATE.Size = new System.Drawing.Size(243, 48);
			bUPDATE.TabIndex = 9;
			bUPDATE.Text = "Download last drivers";
			bUPDATE.UseVisualStyleBackColor = true;
			bUPDATE.Click += new System.EventHandler(bUPDATE_Click);
			bNTP.Image = Kiosk.Properties.Resources.recycle;
			bNTP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bNTP.Location = new System.Drawing.Point(261, 156);
			bNTP.Name = "bNTP";
			bNTP.Size = new System.Drawing.Size(243, 48);
			bNTP.TabIndex = 7;
			bNTP.Text = "Clock Server Sync";
			bNTP.UseVisualStyleBackColor = true;
			bNTP.Click += new System.EventHandler(bNTP_Click);
			bClean.Image = Kiosk.Properties.Resources.ico_ccleaner;
			bClean.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bClean.Location = new System.Drawing.Point(261, 108);
			bClean.Name = "bClean";
			bClean.Size = new System.Drawing.Size(243, 48);
			bClean.TabIndex = 6;
			bClean.Text = "Clean Hard Disk";
			bClean.UseVisualStyleBackColor = true;
			bClean.Click += new System.EventHandler(bClean_Click);
			bWifi.Image = Kiosk.Properties.Resources.ico_wifi_connect;
			bWifi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bWifi.Location = new System.Drawing.Point(261, 60);
			bWifi.Name = "bWifi";
			bWifi.Size = new System.Drawing.Size(243, 48);
			bWifi.TabIndex = 5;
			bWifi.Text = "Wi-Fi";
			bWifi.UseVisualStyleBackColor = true;
			bWifi.Click += new System.EventHandler(bWifi_Click);
			bInfo.Image = Kiosk.Properties.Resources.ico_view;
			bInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bInfo.Location = new System.Drawing.Point(12, 60);
			bInfo.Name = "bInfo";
			bInfo.Size = new System.Drawing.Size(243, 48);
			bInfo.TabIndex = 1;
			bInfo.Text = "System Information";
			bInfo.UseVisualStyleBackColor = true;
			bInfo.Click += new System.EventHandler(bInfo_Click);
			bPorts.Image = Kiosk.Properties.Resources.ico_cpanel;
			bPorts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bPorts.Location = new System.Drawing.Point(12, 156);
			bPorts.Name = "bPorts";
			bPorts.Size = new System.Drawing.Size(243, 48);
			bPorts.TabIndex = 3;
			bPorts.Text = "Ports";
			bPorts.UseVisualStyleBackColor = true;
			bPorts.Click += new System.EventHandler(bPorts_Click);
			bCancel.Image = Kiosk.Properties.Resources.ico_ok;
			bCancel.Location = new System.Drawing.Point(455, 306);
			bCancel.Name = "bCancel";
			bCancel.Size = new System.Drawing.Size(48, 48);
			bCancel.TabIndex = 8;
			bCancel.UseVisualStyleBackColor = true;
			bCancel.Click += new System.EventHandler(bCancel_Click);
			bPrinter.Image = Kiosk.Properties.Resources.ico_ticket;
			bPrinter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bPrinter.Location = new System.Drawing.Point(12, 108);
			bPrinter.Name = "bPrinter";
			bPrinter.Size = new System.Drawing.Size(243, 48);
			bPrinter.TabIndex = 2;
			bPrinter.Text = "Ticket Printers";
			bPrinter.UseVisualStyleBackColor = true;
			bPrinter.Click += new System.EventHandler(bPrinter_Click);
			bCalib.Image = Kiosk.Properties.Resources.ico_touch;
			bCalib.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bCalib.Location = new System.Drawing.Point(12, 12);
			bCalib.Name = "bCalib";
			bCalib.Size = new System.Drawing.Size(243, 48);
			bCalib.TabIndex = 0;
			bCalib.Text = "Touch Screen Calibration";
			bCalib.UseVisualStyleBackColor = true;
			bCalib.Click += new System.EventHandler(bCalib_Click);
			bVNC.Image = Kiosk.Properties.Resources.ico_monitors;
			bVNC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			bVNC.Location = new System.Drawing.Point(261, 12);
			bVNC.Name = "bVNC";
			bVNC.Size = new System.Drawing.Size(243, 48);
			bVNC.TabIndex = 4;
			bVNC.Text = "Remote Access";
			bVNC.UseVisualStyleBackColor = true;
			bVNC.Click += new System.EventHandler(bVNC_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(515, 369);
			base.Controls.Add(bUNLOCK);
			base.Controls.Add(bLOCK);
			base.Controls.Add(bDELETEDATA);
			base.Controls.Add(bUPDATE);
			base.Controls.Add(bNTP);
			base.Controls.Add(bClean);
			base.Controls.Add(bWifi);
			base.Controls.Add(bInfo);
			base.Controls.Add(bPorts);
			base.Controls.Add(bCancel);
			base.Controls.Add(bPrinter);
			base.Controls.Add(bCalib);
			base.Controls.Add(bVNC);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Name = "FormTweak";
			base.ShowIcon = false;
			Text = "Kiosk Tweak v1.00";
			ResumeLayout(false);
		}
	}
}
