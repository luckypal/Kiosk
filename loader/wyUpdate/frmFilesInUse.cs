using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Forms;

namespace wyUpdate
{
	public class frmFilesInUse : Form
	{
		private struct RM_UNIQUE_PROCESS
		{
			public int dwProcessId;

			public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
		}

		private enum RM_APP_TYPE
		{
			RmUnknownApp = 0,
			RmMainWindow = 1,
			RmOtherWindow = 2,
			RmService = 3,
			RmExplorer = 4,
			RmConsole = 5,
			RmCritical = 1000
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct RM_PROCESS_INFO
		{
			public RM_UNIQUE_PROCESS Process;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
			public string strAppName;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
			public string strServiceShortName;

			public RM_APP_TYPE ApplicationType;

			public uint AppStatus;

			public uint TSSessionId;

			[MarshalAs(UnmanagedType.Bool)]
			public bool bRestartable;
		}

		private const int RmRebootReasonNone = 0;

		private const int CCH_RM_MAX_APP_NAME = 255;

		private const int CCH_RM_MAX_SVC_NAME = 63;

		private const int SidePadding = 12;

		private readonly ClientLanguage clientLang;

		public bool CancelUpdate;

		private readonly bool showingProcesses;

		private readonly BackgroundWorker bw;

		private Timer chkProc;

		private List<Process> runningProcesses;

		public string FilenameInUse;

		private Rectangle m_DescripRect;

		private IContainer components;

		private Button btnCancel;

		private TextBox txtFile;

		private ListBox listProc;

		private Label lblProc;

		private Button btnCloseProc;

		private Button btnCloseAll;

		[DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
		private static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

		[DllImport("rstrtmgr.dll")]
		private static extern int RmEndSession(uint pSessionHandle);

		[DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
		private static extern int RmRegisterResources(uint pSessionHandle, uint nFiles, string[] rgsFilenames, uint nApplications, [In] RM_UNIQUE_PROCESS[] rgApplications, uint nServices, string[] rgsServiceNames);

		[DllImport("rstrtmgr.dll")]
		private static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, [In] [Out] RM_PROCESS_INFO[] rgAffectedApps, ref uint lpdwRebootReasons);

		public frmFilesInUse(ClientLanguage cLang, string filename)
		{
			InitializeComponent();
			clientLang = cLang;
			Text = clientLang.FilesInUseDialog.Title;
			btnCancel.Text = clientLang.CancelUpdate;
			FilenameInUse = filename;
			txtFile.Text = filename;
			if (VistaTools.AtLeastVista())
			{
				try
				{
					runningProcesses = GetProcessesUsingFiles(new string[1]
					{
						filename
					});
				}
				catch
				{
				}
				if (runningProcesses != null && runningProcesses.Count > 0)
				{
					UpdateList();
					lblProc.Text = clientLang.FilesInUseDialog.SubTitle;
					btnCloseProc.Text = clientLang.ClosePrc;
					btnCloseAll.Text = clientLang.CloseAllPrc;
					lblProc.Visible = true;
					listProc.Visible = true;
					btnCloseAll.Visible = true;
					btnCloseProc.Visible = true;
					showingProcesses = true;
					chkProc = new Timer
					{
						Enabled = true,
						Interval = 2000
					};
					chkProc.Tick += chkProc_Tick;
					bw = new BackgroundWorker
					{
						WorkerSupportsCancellation = true
					};
					bw.DoWork += bw_DoWork;
					bw.RunWorkerCompleted += bw_RunWorkerCompleted;
					bw.RunWorkerAsync();
				}
			}
			SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
			UpdateSizes();
		}

		public static List<Process> GetProcessesUsingFiles(IList<string> filePaths)
		{
			List<Process> result = null;
			if (RmStartSession(out uint pSessionHandle, 0, Guid.NewGuid().ToString("N")) != 0)
			{
				throw new Win32Exception();
			}
			try
			{
				string[] array = new string[filePaths.Count];
				filePaths.CopyTo(array, 0);
				if (RmRegisterResources(pSessionHandle, (uint)array.Length, array, 0u, null, 0u, null) != 0)
				{
					throw new Win32Exception();
				}
				uint pnProcInfoNeeded = 0u;
				uint pnProcInfo = 0u;
				uint lpdwRebootReasons = 0u;
				switch (RmGetList(pSessionHandle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons))
				{
				case 234:
				{
					RM_PROCESS_INFO[] array2 = new RM_PROCESS_INFO[pnProcInfoNeeded];
					pnProcInfo = pnProcInfoNeeded;
					if (RmGetList(pSessionHandle, out pnProcInfoNeeded, ref pnProcInfo, array2, ref lpdwRebootReasons) != 0)
					{
						throw new Win32Exception();
					}
					result = new List<Process>((int)pnProcInfo);
					for (int i = 0; i < pnProcInfo; i++)
					{
						try
						{
							result.Add(Process.GetProcessById(array2[i].Process.dwProcessId));
						}
						catch (ArgumentException)
						{
						}
					}
					return result;
				}
				default:
					throw new Win32Exception();
				case 0:
					return result;
				}
			}
			finally
			{
				RmEndSession(pSessionHandle);
			}
		}

		private void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			List<Process> processesUsingFiles = GetProcessesUsingFiles(new string[1]
			{
				FilenameInUse
			});
			e.Result = ((processesUsingFiles != null && processesUsingFiles.Count > 0) ? processesUsingFiles : null);
		}

		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Result != null)
			{
				List<Process> procs = (List<Process>)e.Result;
				if (!frmProcesses.SameProcs(procs, runningProcesses))
				{
					runningProcesses = procs;
					UpdateList();
				}
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			chkProc.Enabled = false;
			CancelUpdate = (DialogResult.Yes == MessageBox.Show(clientLang.CancelDialog.Content, clientLang.CancelDialog.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2));
			if (!CancelUpdate)
			{
				base.DialogResult = DialogResult.None;
				chkProc.Enabled = true;
			}
		}

		private void UpdateSizes()
		{
			if (txtFile != null && clientLang != null)
			{
				m_DescripRect = new Rectangle(new Point(12, 12), TextRenderer.MeasureText(clientLang.FilesInUseDialog.Content, Font, new Size(base.ClientRectangle.Width - 24, 1), TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding));
				txtFile.Location = new Point(12, m_DescripRect.Bottom + 5);
				txtFile.Width = base.ClientRectangle.Width - 24;
				int num = base.ClientRectangle.Height - txtFile.Top - (base.ClientRectangle.Height - btnCancel.Top) - 5;
				if (showingProcesses)
				{
					txtFile.Height = num / 2 - 5 - lblProc.Height;
					lblProc.Location = new Point(12, txtFile.Bottom + 12);
					listProc.Height = txtFile.Height;
					listProc.Location = new Point(12, lblProc.Bottom + 5);
					listProc.Width = base.ClientRectangle.Width - 24;
				}
				else
				{
					txtFile.Height = num;
				}
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			UpdateSizes();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			TextRenderer.DrawText(e.Graphics, clientLang.FilesInUseDialog.Content, Font, m_DescripRect, Color.White, TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding);
			base.OnPaint(e);
		}

		private void btnCloseProc_Click(object sender, EventArgs e)
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

		private void btnCloseAll_Click(object sender, EventArgs e)
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

		private void chkProc_Tick(object sender, EventArgs e)
		{
			if (!bw.IsBusy)
			{
				bw.RunWorkerAsync();
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
			btnCancel = new System.Windows.Forms.Button();
			txtFile = new System.Windows.Forms.TextBox();
			listProc = new System.Windows.Forms.ListBox();
			lblProc = new System.Windows.Forms.Label();
			btnCloseProc = new System.Windows.Forms.Button();
			btnCloseAll = new System.Windows.Forms.Button();
			SuspendLayout();
			btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			btnCancel.AutoSize = true;
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btnCancel.Location = new System.Drawing.Point(279, 302);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(92, 22);
			btnCancel.TabIndex = 3;
			btnCancel.Click += new System.EventHandler(btnCancel_Click);
			txtFile.Location = new System.Drawing.Point(12, 62);
			txtFile.Multiline = true;
			txtFile.Name = "txtFile";
			txtFile.ReadOnly = true;
			txtFile.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			txtFile.Size = new System.Drawing.Size(346, 100);
			txtFile.TabIndex = 4;
			listProc.FormattingEnabled = true;
			listProc.HorizontalScrollbar = true;
			listProc.IntegralHeight = false;
			listProc.Location = new System.Drawing.Point(9, 203);
			listProc.Name = "listProc";
			listProc.Size = new System.Drawing.Size(367, 93);
			listProc.TabIndex = 5;
			listProc.Visible = false;
			lblProc.AutoSize = true;
			lblProc.Location = new System.Drawing.Point(12, 187);
			lblProc.Name = "lblProc";
			lblProc.Size = new System.Drawing.Size(0, 13);
			lblProc.TabIndex = 6;
			lblProc.Visible = false;
			btnCloseProc.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			btnCloseProc.AutoSize = true;
			btnCloseProc.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btnCloseProc.Location = new System.Drawing.Point(12, 302);
			btnCloseProc.Name = "btnCloseProc";
			btnCloseProc.Size = new System.Drawing.Size(88, 22);
			btnCloseProc.TabIndex = 7;
			btnCloseProc.Visible = false;
			btnCloseProc.Click += new System.EventHandler(btnCloseProc_Click);
			btnCloseAll.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			btnCloseAll.AutoSize = true;
			btnCloseAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btnCloseAll.Location = new System.Drawing.Point(106, 302);
			btnCloseAll.Name = "btnCloseAll";
			btnCloseAll.Size = new System.Drawing.Size(113, 22);
			btnCloseAll.TabIndex = 8;
			btnCloseAll.Visible = false;
			btnCloseAll.Click += new System.EventHandler(btnCloseAll_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(383, 336);
			base.ControlBox = false;
			base.Controls.Add(btnCloseProc);
			base.Controls.Add(btnCloseAll);
			base.Controls.Add(lblProc);
			base.Controls.Add(listProc);
			base.Controls.Add(txtFile);
			base.Controls.Add(btnCancel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			MinimumSize = new System.Drawing.Size(350, 350);
			base.Name = "frmFilesInUse";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = " ";
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
