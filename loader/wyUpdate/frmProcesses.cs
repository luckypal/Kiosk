using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace wyUpdate
{
	public class frmProcesses : Form
	{
		private const int SidePadding = 12;

		private readonly ClientLanguage clientLang;

		private readonly List<FileInfo> filenames;

		private List<Process> runningProcesses;

		private readonly BackgroundWorker bw = new BackgroundWorker();

		private Rectangle m_DescripRect;

		private IContainer components;

		private Button btnCloseProc;

		private Button btnCloseAll;

		private ListBox listProc;

		private Button btnCancel;

		private Timer chkProc;

		public frmProcesses(List<FileInfo> files, List<Process> rProcesses, ClientLanguage cLang)
		{
			runningProcesses = rProcesses;
			Font = SystemFonts.MessageBoxFont;
			InitializeComponent();
			filenames = files;
			clientLang = cLang;
			Text = clientLang.ProcessDialog.Title;
			btnCloseProc.Text = clientLang.ClosePrc;
			btnCloseAll.Text = clientLang.CloseAllPrc;
			btnCancel.Text = clientLang.CancelUpdate;
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			btnCloseAll.Left = btnCloseProc.Right + 10;
			UpdateSizes();
			UpdateList();
			bw.WorkerSupportsCancellation = true;
			bw.DoWork += bw_DoWork;
			bw.RunWorkerCompleted += bw_RunWorkerCompleted;
			bw.RunWorkerAsync();
		}

		private void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			List<Process> list = new List<Process>();
			foreach (FileInfo filename in filenames)
			{
				Process[] processesByName = Process.GetProcessesByName(filename.Name.Replace(filename.Extension, ""));
				Process[] array = processesByName;
				foreach (Process process in array)
				{
					try
					{
						if (process.MainModule != null && string.Equals(process.MainModule.FileName, filename.FullName, StringComparison.OrdinalIgnoreCase) && !InstallUpdate.ProcessIsSelf(process.MainModule.FileName))
						{
							list.Add(process);
						}
					}
					catch
					{
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].HasExited)
				{
					list.RemoveAt(j);
					j--;
				}
			}
			e.Result = ((list.Count > 0) ? list : null);
		}

		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Result == null)
			{
				base.DialogResult = DialogResult.OK;
				chkProc.Enabled = false;
				return;
			}
			List<Process> procs = (List<Process>)e.Result;
			if (!SameProcs(procs, runningProcesses))
			{
				runningProcesses = procs;
				UpdateList();
			}
		}

		private void UpdateList()
		{
			listProc.Items.Clear();
			foreach (Process runningProcess in runningProcesses)
			{
				listProc.Items.Add(runningProcess.MainWindowTitle + " (" + runningProcess.ProcessName + ".exe)");
			}
			listProc.SelectedIndex = 0;
		}

		private void UpdateSizes()
		{
			int num = base.Width - base.ClientRectangle.Width + btnCloseAll.Right + 35 + btnCancel.Width;
			MinimumSize = new Size(num, 178);
			if (base.Width < num)
			{
				base.Width = num;
			}
			m_DescripRect = new Rectangle(new Point(12, 12), TextRenderer.MeasureText(clientLang.ProcessDialog.Content, Font, new Size(base.ClientRectangle.Width - 24, 1), TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding));
			listProc.Location = new Point(12, m_DescripRect.Bottom + 5);
			listProc.Width = base.ClientRectangle.Width - 24;
			listProc.Height = base.ClientRectangle.Height - listProc.Top - (base.ClientRectangle.Height - btnCloseProc.Top) - 5;
		}

		private void closeProc_Click(object sender, EventArgs e)
		{
			try
			{
				if (runningProcesses[listProc.SelectedIndex].MainWindowHandle == IntPtr.Zero)
				{
					runningProcesses[listProc.SelectedIndex].Kill();
				}
				else
				{
					runningProcesses[listProc.SelectedIndex].CloseMainWindow();
				}
				string text = (string)listProc.Items[listProc.SelectedIndex];
				if (!text.StartsWith("[closing]"))
				{
					text = "[closing] " + text;
				}
				listProc.Items[listProc.SelectedIndex] = text;
			}
			catch
			{
			}
			if (!bw.IsBusy)
			{
				bw.RunWorkerAsync();
			}
		}

		private void closeAll_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < runningProcesses.Count; i++)
			{
				try
				{
					if (runningProcesses[i].MainWindowHandle == IntPtr.Zero)
					{
						runningProcesses[i].Kill();
					}
					else
					{
						runningProcesses[i].CloseMainWindow();
					}
					string text = (string)listProc.Items[i];
					if (!text.StartsWith("[closing]"))
					{
						text = "[closing] " + text;
					}
					listProc.Items[i] = text;
				}
				catch
				{
				}
			}
			if (!bw.IsBusy)
			{
				bw.RunWorkerAsync();
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			chkProc.Enabled = false;
			DialogResult dialogResult = MessageBox.Show(clientLang.CancelDialog.Content, clientLang.CancelDialog.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
			if (dialogResult == DialogResult.Yes)
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			base.DialogResult = DialogResult.None;
			chkProc.Enabled = true;
		}

		public static bool SameProcs(List<Process> procs1, List<Process> procs2)
		{
			if (procs1.Count != procs2.Count)
			{
				return false;
			}
			for (int i = 0; i < procs1.Count; i++)
			{
				if (procs1[i].Id != procs2[i].Id || procs1[i].MainWindowTitle != procs2[i].MainWindowTitle)
				{
					return false;
				}
			}
			return true;
		}

		private void chkProc_Tick(object sender, EventArgs e)
		{
			if (!bw.IsBusy)
			{
				bw.RunWorkerAsync();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			TextRenderer.DrawText(e.Graphics, clientLang.ProcessDialog.Content, Font, m_DescripRect, Color.White, TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);
			base.OnPaint(e);
		}

		private void frmProcesses_Resize(object sender, EventArgs e)
		{
			UpdateSizes();
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
			btnCloseProc = new System.Windows.Forms.Button();
			btnCloseAll = new System.Windows.Forms.Button();
			listProc = new System.Windows.Forms.ListBox();
			btnCancel = new System.Windows.Forms.Button();
			chkProc = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			btnCloseProc.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			btnCloseProc.AutoSize = true;
			btnCloseProc.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btnCloseProc.Location = new System.Drawing.Point(12, 214);
			btnCloseProc.Name = "btnCloseProc";
			btnCloseProc.Size = new System.Drawing.Size(88, 22);
			btnCloseProc.TabIndex = 0;
			btnCloseProc.Click += new System.EventHandler(closeProc_Click);
			btnCloseAll.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			btnCloseAll.AutoSize = true;
			btnCloseAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btnCloseAll.Location = new System.Drawing.Point(106, 214);
			btnCloseAll.Name = "btnCloseAll";
			btnCloseAll.Size = new System.Drawing.Size(113, 22);
			btnCloseAll.TabIndex = 1;
			btnCloseAll.Click += new System.EventHandler(closeAll_Click);
			listProc.FormattingEnabled = true;
			listProc.HorizontalScrollbar = true;
			listProc.IntegralHeight = false;
			listProc.Location = new System.Drawing.Point(3, 60);
			listProc.Name = "listProc";
			listProc.Size = new System.Drawing.Size(367, 117);
			listProc.TabIndex = 0;
			btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			btnCancel.AutoSize = true;
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btnCancel.Location = new System.Drawing.Point(279, 214);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(92, 22);
			btnCancel.TabIndex = 2;
			btnCancel.Click += new System.EventHandler(btnCancel_Click);
			chkProc.Enabled = true;
			chkProc.Interval = 2000;
			chkProc.Tick += new System.EventHandler(chkProc_Tick);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			base.ClientSize = new System.Drawing.Size(383, 248);
			base.ControlBox = false;
			base.Controls.Add(btnCloseProc);
			base.Controls.Add(btnCloseAll);
			base.Controls.Add(btnCancel);
			base.Controls.Add(listProc);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(349, 178);
			base.Name = "frmProcesses";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = " ";
			base.Resize += new System.EventHandler(frmProcesses_Resize);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
