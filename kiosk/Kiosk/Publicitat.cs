// Decompiled with JetBrains decompiler
// Type: Kiosk.Publicitat
// Assembly: Kiosk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C3E32FFD-072D-4F9D-AAE4-A7F2B29E989A
// Assembly location: E:\kiosk\Kiosk.exe

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
    public int pos = 0;
    private IContainer components = (IContainer) null;
    public Publicitat.Item_Publi[] Items;
    public bool OK;
    public bool Error;
    public Configuracion opciones;
    private Label banner;

    public Publicitat(ref Configuracion _opc)
    {
      this.Error = false;
      this.OK = false;
      this.opciones = _opc;
      this.Items = new Publicitat.Item_Publi[0];
      this.pos = -1;
      this.InitializeComponent();
      this.banner.AutoSize = false;
      this.Localize();
    }

    public void Reload()
    {
      this.pos = -1;
      this.load_web("http://" + this.opciones.Srv_Web_Ip + "/__publi.html");
    }

    public void Play()
    {
      this.pos = -1;
      this.Next();
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
      string str = Path.GetTempPath() + _file;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      if (System.IO.File.Exists(str))
      {
        try
        {
          System.IO.File.Delete(str);
        }
        catch
        {
        }
      }
      try
      {
        new WebClient().DownloadFile("http://" + this.opciones.Srv_Web_Ip + "/" + _file, str);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public bool load_web(string _web)
    {
      string str = Path.GetTempPath() + "__publi.html";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      Exception exception;
      try
      {
        new WebClient().DownloadFile(_web, str);
      }
      catch (Exception ex)
      {
        exception = ex;
        return false;
      }
      XmlTextReader xmlTextReader1 = (XmlTextReader) null;
      XmlTextReader xmlTextReader2;
      try
      {
        xmlTextReader2 = new XmlTextReader(str);
      }
      catch (Exception ex)
      {
        exception = ex;
        xmlTextReader1?.Close();
        return false;
      }
      this.Items = new Publicitat.Item_Publi[0];
      try
      {
        while (xmlTextReader2.Read())
        {
          if (xmlTextReader2.NodeType == XmlNodeType.Element)
          {
            if (xmlTextReader2.Name.ToLower() == "local".ToLower())
            {
              Array.Resize<Publicitat.Item_Publi>(ref this.Items, this.Items.Length + 1);
              this.Items[this.Items.Length - 1] = new Publicitat.Item_Publi();
              this.Items[this.Items.Length - 1].Text = "@";
              if (xmlTextReader2.HasAttributes)
              {
                for (int i = 0; i < xmlTextReader2.AttributeCount; ++i)
                {
                  xmlTextReader2.MoveToAttribute(i);
                  if (xmlTextReader2.Name.ToLower() == "backcolor".ToLower())
                    this.Items[this.Items.Length - 1].Fons = Convert.ToInt32(xmlTextReader2.Value.ToString(), 16);
                  if (xmlTextReader2.Name.ToLower() == "color".ToLower())
                    this.Items[this.Items.Length - 1].Color = Convert.ToInt32(xmlTextReader2.Value.ToString(), 16);
                }
              }
            }
            if (xmlTextReader2.Name.ToLower() == "element".ToLower())
            {
              Array.Resize<Publicitat.Item_Publi>(ref this.Items, this.Items.Length + 1);
              this.Items[this.Items.Length - 1] = new Publicitat.Item_Publi();
              if (xmlTextReader2.HasAttributes)
              {
                for (int i = 0; i < xmlTextReader2.AttributeCount; ++i)
                {
                  xmlTextReader2.MoveToAttribute(i);
                  if (xmlTextReader2.Name.ToLower() == "img".ToLower())
                    this.Items[this.Items.Length - 1].Imatge = xmlTextReader2.Value.ToString();
                  if (xmlTextReader2.Name.ToLower() == "text".ToLower())
                    this.Items[this.Items.Length - 1].Text = xmlTextReader2.Value.ToString();
                  if (xmlTextReader2.Name.ToLower() == "backcolor".ToLower())
                    this.Items[this.Items.Length - 1].Fons = Convert.ToInt32(xmlTextReader2.Value.ToString(), 16);
                  if (xmlTextReader2.Name.ToLower() == "color".ToLower())
                    this.Items[this.Items.Length - 1].Color = Convert.ToInt32(xmlTextReader2.Value.ToString(), 16);
                }
              }
            }
          }
        }
        xmlTextReader2.Close();
      }
      catch (Exception ex)
      {
        exception = ex;
        xmlTextReader2?.Close();
        return false;
      }
      return true;
    }

    private void Localize()
    {
      this.SuspendLayout();
      this.ResumeLayout();
    }

    private void Publicitat_Load(object sender, EventArgs e)
    {
    }

    public void Next()
    {
      int num = 0;
      ++this.pos;
      if (this.opciones.ModoKiosk != 0)
      {
        if (this.pos > this.Items.Length)
          this.pos = 0;
      }
      else if (this.pos >= this.Items.Length)
        this.pos = 0;
      int pos = this.pos;
      string tempPath = Path.GetTempPath();
      for (; pos < this.Items.Length; ++pos)
      {
        if (this.Items[pos].Text == "@" && !string.IsNullOrEmpty(this.opciones.Publi))
        {
          num = 1;
          this.BackgroundImage = (Image) null;
          this.BackColor = Color.FromArgb(this.Items[pos].Fons & (int) byte.MaxValue, this.Items[pos].Fons >> 16 & (int) byte.MaxValue, this.Items[pos].Fons & (int) byte.MaxValue);
          this.banner.Visible = true;
          this.banner.Text = this.opciones.Publi;
          this.banner.ForeColor = Color.FromArgb(this.Items[pos].Color & (int) byte.MaxValue, this.Items[pos].Color >> 16 & (int) byte.MaxValue, this.Items[pos].Color & (int) byte.MaxValue);
          break;
        }
        if (!string.IsNullOrEmpty(this.Items[pos].Imatge))
        {
          if (this.Items[pos].Load == 0)
          {
            this.load_file(this.Items[pos].Imatge);
            this.Items[pos].Load = 1;
          }
          if (System.IO.File.Exists(tempPath + this.Items[pos].Imatge))
          {
            num = 1;
            this.BackgroundImage = (Image) new Bitmap(tempPath + this.Items[pos].Imatge);
            this.BackColor = Color.FromArgb(this.Items[pos].Fons & (int) byte.MaxValue, this.Items[pos].Fons >> 16 & (int) byte.MaxValue, this.Items[pos].Fons & (int) byte.MaxValue);
            this.banner.Visible = false;
            this.banner.Text = "";
            break;
          }
        }
        if (!string.IsNullOrEmpty(this.Items[pos].Text))
        {
          num = 1;
          this.BackgroundImage = (Image) null;
          this.BackColor = Color.FromArgb(this.Items[pos].Fons & (int) byte.MaxValue, this.Items[pos].Fons << 16 & (int) byte.MaxValue, this.Items[pos].Fons & (int) byte.MaxValue);
          this.banner.Visible = true;
          this.banner.ForeColor = Color.FromArgb(this.Items[pos].Color & (int) byte.MaxValue, this.Items[pos].Color >> 16 & (int) byte.MaxValue, this.Items[pos].Color & (int) byte.MaxValue);
          this.banner.Text = this.Items[pos].Text;
          break;
        }
      }
      if (num != 0)
        return;
      this.BackgroundImage = (Image) null;
      this.BackColor = Color.Black;
      this.banner.Visible = true;
      if (this.opciones.ModoKiosk != 0)
        this.banner.Text = "Borne Internet";
      else
        this.banner.Text = "";
      this.banner.ForeColor = Color.Yellow;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.banner = new Label();
      this.SuspendLayout();
      this.banner.AutoSize = true;
      this.banner.BackColor = Color.Transparent;
      this.banner.Dock = DockStyle.Fill;
      this.banner.Font = new Font("Microsoft Sans Serif", 100f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.banner.ForeColor = Color.Yellow;
      this.banner.Location = new Point(0, 0);
      this.banner.Name = "banner";
      this.banner.Size = new Size(0, 153);
      this.banner.TabIndex = 0;
      this.banner.TextAlign = ContentAlignment.MiddleCenter;
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.Black;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.ClientSize = new Size(277, 93);
      this.Controls.Add((Control) this.banner);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (Publicitat);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "PubliWeb";
      this.Load += new EventHandler(this.Publicitat_Load);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public class Item_Publi
    {
      public string Imatge;
      public string Text;
      public int Color;
      public int Fons;
      public int Load;

      public Item_Publi()
      {
        this.Imatge = "";
        this.Load = 0;
        this.Text = "";
        this.Color = (int) ushort.MaxValue;
        this.Fons = 0;
      }
    }
  }
}
