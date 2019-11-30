using GLib;
using SingerOSK;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace KVKeyboard
{
	public class VKeyboard_Form : Form
	{
		public delegate void NewMessageDelegate(string NewMessage);

		private string Idioma;

		private string err;

		private PipeServer _pipeServer;

		private IContainer components;

		private ElementHost VTeclat;

		private Timer tServer;

		public VKeyboard_Form(string _idioma)
		{
			Idioma = _idioma;
			InitializeComponent();
			VTeclat.set_Child((UIElement)(object)new SingerOnScreenKeyboard(base.Handle));
			base.Width = Screen.PrimaryScreen.WorkingArea.Width;
			base.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Bottom - base.Height);
			_pipeServer = new PipeServer();
			_pipeServer.PipeMessage += PipesMessageHandler;
			tServer.Enabled = true;
		}

		private void PipesMessageHandler(string message)
		{
			if (message.ToLower().Contains("quit"))
			{
				try
				{
					Environment.Exit(0);
				}
				catch
				{
				}
			}
		}

		private void VKeyboard_Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			_pipeServer.PipeMessage -= PipesMessageHandler;
			_pipeServer = null;
		}

		private void tServer_Tick(object sender, EventArgs e)
		{
			if (_pipeServer != null)
			{
				_pipeServer.Listen("KVKeyboard");
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
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			components = new System.ComponentModel.Container();
			VTeclat = (ElementHost)(object)new ElementHost();
			tServer = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			((System.Windows.Forms.Control)(object)VTeclat).Dock = System.Windows.Forms.DockStyle.Fill;
			((System.Windows.Forms.Control)(object)VTeclat).Location = new System.Drawing.Point(0, 0);
			((System.Windows.Forms.Control)(object)VTeclat).Name = "VTeclat";
			((System.Windows.Forms.Control)(object)VTeclat).Size = new System.Drawing.Size(718, 171);
			((System.Windows.Forms.Control)(object)VTeclat).TabIndex = 0;
			((System.Windows.Forms.Control)(object)VTeclat).Text = "elementHost1";
			VTeclat.set_Child((UIElement)null);
			tServer.Interval = 500;
			tServer.Tick += new System.EventHandler(tServer_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(718, 171);
			base.ControlBox = false;
			base.Controls.Add((System.Windows.Forms.Control)(object)VTeclat);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "VKeyboard_Form";
			base.TopMost = true;
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(VKeyboard_Form_FormClosing);
			ResumeLayout(false);
		}
	}
}
