using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace Kiosk
{
	public class Publicitat : Form
	{
		public class Item_Publi
		{
			public string Imatge;

			public string Text;

			public int Color;

			public int Fons;

			public int Load;

			public Item_Publi()
			{
				Imatge = "";
				Load = 0;
				Text = "";
				Color = 65535;
				Fons = 0;
			}
		}

		public Item_Publi[] Items;

		public int pos = 0;

		public bool OK;

		public bool Error;

		public Configuracion opciones;

		private IContainer components = null;

		private Label banner;

		public Publicitat(ref Configuracion _opc)
		{
			Error = false;
			OK = false;
			opciones = _opc;
			Items = new Item_Publi[0];
			pos = -1;
			InitializeComponent();
			banner.AutoSize = false;
			Localize();
		}

		public void Reload()
		{
			pos = -1;
			load_web("http://" + opciones.Srv_Web_Ip + "/__publi.html");
		}

		public void Play()
		{
			pos = -1;
			Next();
		}

		public void Stop()
		{
		}

		public void Pause()
		{
		}

		public void Resume()
		{
		}

		public bool load_file(string _file)
		{
			string text = Path.GetTempPath() + _file;
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (File.Exists(text))
			{
				try
				{
					File.Delete(text);
				}
				catch
				{
				}
			}
			try
			{
				WebClient webClient = new WebClient();
				webClient.DownloadFile("http://" + opciones.Srv_Web_Ip + "/" + _file, text);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public bool load_web(string _web)
		{
			string text = Path.GetTempPath() + "__publi.html";
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
			Items = new Item_Publi[0];
			try
			{
				while (xmlTextReader.Read())
				{
					if (xmlTextReader.NodeType == XmlNodeType.Element)
					{
						if (xmlTextReader.Name.ToLower() == "local".ToLower())
						{
							Array.Resize(ref Items, Items.Length + 1);
							Items[Items.Length - 1] = new Item_Publi();
							Items[Items.Length - 1].Text = "@";
							if (xmlTextReader.HasAttributes)
							{
								for (int i = 0; i < xmlTextReader.AttributeCount; i++)
								{
									xmlTextReader.MoveToAttribute(i);
									if (xmlTextReader.Name.ToLower() == "backcolor".ToLower())
									{
										Items[Items.Length - 1].Fons = Convert.ToInt32(xmlTextReader.Value.ToString(), 16);
									}
									if (xmlTextReader.Name.ToLower() == "color".ToLower())
									{
										Items[Items.Length - 1].Color = Convert.ToInt32(xmlTextReader.Value.ToString(), 16);
									}
								}
							}
						}
						if (xmlTextReader.Name.ToLower() == "element".ToLower())
						{
							Array.Resize(ref Items, Items.Length + 1);
							Items[Items.Length - 1] = new Item_Publi();
							if (xmlTextReader.HasAttributes)
							{
								for (int i = 0; i < xmlTextReader.AttributeCount; i++)
								{
									xmlTextReader.MoveToAttribute(i);
									if (xmlTextReader.Name.ToLower() == "img".ToLower())
									{
										Items[Items.Length - 1].Imatge = xmlTextReader.Value.ToString();
									}
									if (xmlTextReader.Name.ToLower() == "text".ToLower())
									{
										Items[Items.Length - 1].Text = xmlTextReader.Value.ToString();
									}
									if (xmlTextReader.Name.ToLower() == "backcolor".ToLower())
									{
										Items[Items.Length - 1].Fons = Convert.ToInt32(xmlTextReader.Value.ToString(), 16);
									}
									if (xmlTextReader.Name.ToLower() == "color".ToLower())
									{
										Items[Items.Length - 1].Color = Convert.ToInt32(xmlTextReader.Value.ToString(), 16);
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

		private void Localize()
		{
			SuspendLayout();
			ResumeLayout();
		}

		private void Publicitat_Load(object sender, EventArgs e)
		{
		}

		public void Next()
		{
			int num = 0;
			pos++;
			if (opciones.ModoKiosk != 0)
			{
				if (pos > Items.Length)
				{
					pos = 0;
				}
			}
			else if (pos >= Items.Length)
			{
				pos = 0;
			}
			int i = pos;
			string tempPath = Path.GetTempPath();
			for (; i < Items.Length; i++)
			{
				if (Items[i].Text == "@" && !string.IsNullOrEmpty(opciones.Publi))
				{
					num = 1;
					BackgroundImage = null;
					BackColor = Color.FromArgb(Items[i].Fons & 0xFF, (Items[i].Fons >> 16) & 0xFF, Items[i].Fons & 0xFF);
					banner.Visible = true;
					banner.Text = opciones.Publi;
					banner.ForeColor = Color.FromArgb(Items[i].Color & 0xFF, (Items[i].Color >> 16) & 0xFF, Items[i].Color & 0xFF);
					break;
				}
				if (!string.IsNullOrEmpty(Items[i].Imatge))
				{
					if (Items[i].Load == 0)
					{
						load_file(Items[i].Imatge);
						Items[i].Load = 1;
					}
					if (File.Exists(tempPath + Items[i].Imatge))
					{
						num = 1;
						BackgroundImage = new Bitmap(tempPath + Items[i].Imatge);
						BackColor = Color.FromArgb(Items[i].Fons & 0xFF, (Items[i].Fons >> 16) & 0xFF, Items[i].Fons & 0xFF);
						banner.Visible = false;
						banner.Text = "";
						break;
					}
				}
				if (!string.IsNullOrEmpty(Items[i].Text))
				{
					num = 1;
					BackgroundImage = null;
					BackColor = Color.FromArgb(Items[i].Fons & 0xFF, (Items[i].Fons << 16) & 0xFF, Items[i].Fons & 0xFF);
					banner.Visible = true;
					banner.ForeColor = Color.FromArgb(Items[i].Color & 0xFF, (Items[i].Color >> 16) & 0xFF, Items[i].Color & 0xFF);
					banner.Text = Items[i].Text;
					break;
				}
			}
			if (num == 0)
			{
				BackgroundImage = null;
				BackColor = Color.Black;
				banner.Visible = true;
				if (opciones.ModoKiosk != 0)
				{
					banner.Text = "Borne Internet";
				}
				else
				{
					banner.Text = "";
				}
				banner.ForeColor = Color.Yellow;
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
			banner = new System.Windows.Forms.Label();
			SuspendLayout();
			banner.AutoSize = true;
			banner.BackColor = System.Drawing.Color.Transparent;
			banner.Dock = System.Windows.Forms.DockStyle.Fill;
			banner.Font = new System.Drawing.Font("Microsoft Sans Serif", 100f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			banner.ForeColor = System.Drawing.Color.Yellow;
			banner.Location = new System.Drawing.Point(0, 0);
			banner.Name = "banner";
			banner.Size = new System.Drawing.Size(0, 153);
			banner.TabIndex = 0;
			banner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.Color.Black;
			BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			base.ClientSize = new System.Drawing.Size(277, 93);
			base.Controls.Add(banner);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "Publicitat";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "PubliWeb";
			base.Load += new System.EventHandler(Publicitat_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
