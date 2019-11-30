using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using wyDay.Controls;
using wyUpdate.Common;
using wyUpdate.Downloader;
using wyUpdate.Properties;

namespace wyUpdate
{
	public class frmMain : Form
	{
		[Flags]
		public enum EXECUTION_STATE : uint
		{
			ES_AWAYMODE_REQUIRED = 0x40,
			ES_CONTINUOUS = 0x80000000,
			ES_DISPLAY_REQUIRED = 0x2,
			ES_SYSTEM_REQUIRED = 0x1
		}

		private readonly UpdateHelper updateHelper = new UpdateHelper();

		private bool isAutoUpdateMode;

		private string autoUpdateStateFile;

		private UpdateStep autoUpdateStepProcessing;

		private bool currentlyExtracting;

		public bool IsNewSelf;

		private bool beginAutoUpdateInstallation;

		private string oldAUTempFolder;

		private bool logOffBlocked;

		private frmFilesInUse inUseForm;

		private string selfUpdateFileLoc;

		private string clientSFLoc;

		private string oldSelfLocation;

		private string newSelfLocation;

		private bool selfUpdateFromRC1;

		private ServerFile SelfServerFile;

		public bool IsAdmin;

		public readonly ClientFile update = new ClientFile();

		private string PasswordUpdateCmd;

		private ServerFile ServerFile;

		private VersionChoice updateFrom;

		private UpdateDetails updtDetails;

		private FileDownloader downloader;

		private InstallUpdate installUpdate;

		private readonly ClientLanguage clientLang = new ClientLanguage();

		private Frame frameOn = Frame.Checking;

		private string EXE_RUN = "kiosk";

		private bool isCancelled;

		private string error;

		private string errorDetails;

		private string updateFilename;

		private string serverFileLoc;

		private string clientFileLoc;

		private string serverOverwrite;

		private string updatePathVar;

		private string customUrlArgs;

		private string baseDirectory;

		private string tempDirectory;

		private readonly PanelDisplay panelDisplaying;

		private UpdateStepOn startStep;

		public SelfUpdateState SelfUpdateState;

		private bool needElevation;

		private bool uninstalling;

		private bool isSilent;

		public int ReturnCode;

		private ClientFileType clientFileType;

		private bool _isApplicationRun = true;

		private bool QuickCheck;

		private bool QuickCheckNoErr;

		private bool QuickCheckJustCheck;

		private bool SkipUpdateInfo;

		private string OutputInfo;

		private string StartOnErr;

		private string StartOnErrArgs;

		private bool UpdatingFromService;

		private Logger log;

		private string forcedLanguageCulture;

		private string customProxyUrl;

		private string customProxyUser;

		private string customProxyPassword;

		private string customProxyDomain;

		private static byte[] bytes = Encoding.ASCII.GetBytes("2kAWAFxg");

		private bool OnlyInstall;

		public int ModoStartup;

		private IContainer components;

		private Button btnNext;

		private Button btnCancel;

		private System.Windows.Forms.Timer tWatchDog;

		private void SetupAutoupdateMode()
		{
			updateHelper.SenderProcessClosed += UpdateHelper_SenderProcessClosed;
			updateHelper.RequestReceived += UpdateHelper_RequestReceived;
			updateHelper.StartPipeServer(this);
		}

		private void StartQuickAndDirtyAutoUpdateMode()
		{
			updateHelper.StartPipeServer(this);
			int num = 0;
			while (updateHelper.TotalConnectedClients == 0 && num != 30000)
			{
				num += 300;
				Thread.Sleep(300);
			}
		}

		private void StartNewSelfAndClose()
		{
			bool flag = false;
			if (!updateHelper.RunningServer)
			{
				flag = true;
				updateHelper.StartPipeServer(this);
			}
			Process process = new Process();
			process.StartInfo.FileName = newSelfLocation;
			process.StartInfo.Arguments = "-cdata:\"" + clientFileLoc + "\" -basedir:\"" + baseDirectory + " \" /autoupdate /ns";
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			Process process2 = process;
			process2.Start();
			if (flag)
			{
				int num = 0;
				while (updateHelper.TotalConnectedClients == 0 && num != 30000)
				{
					num += 300;
					Thread.Sleep(300);
				}
			}
			updateHelper.SendNewWyUpdate(UpdateHelperData.PipenameFromFilename(newSelfLocation), process2.Id);
			CancelUpdate(ForceClose: true);
		}

		private void UpdateHelper_RequestReceived(object sender, UpdateAction a, UpdateStep s)
		{
			if (a == UpdateAction.Cancel)
			{
				CancelUpdate(ForceClose: true);
			}
			else
			{
				if (FilterBadRequest(s))
				{
					return;
				}
				autoUpdateStepProcessing = ((s != UpdateStep.ForceRecheckForUpdate) ? s : UpdateStep.CheckForUpdate);
				switch (s)
				{
				case UpdateStep.ForceRecheckForUpdate:
					panelDisplaying.ClearText();
					ShowFrame(Frame.Checking);
					CheckForUpdate();
					break;
				case UpdateStep.CheckForUpdate:
					CheckForUpdate();
					break;
				case UpdateStep.DownloadUpdate:
					ShowFrame(Frame.InstallUpdates);
					DownloadUpdate();
					break;
				case UpdateStep.BeginExtraction:
					update.CurrentlyUpdating = UpdateOn.Extracting;
					InstallUpdates(update.CurrentlyUpdating);
					break;
				case UpdateStep.RestartInfo:
					updateHelper.SendSuccess(autoUpdateStepProcessing, (int)base.Handle);
					break;
				case UpdateStep.Install:
					if (!updateHelper.IsAService)
					{
						base.Visible = true;
						base.TopMost = true;
						base.TopMost = false;
					}
					if (needElevation)
					{
						SaveAutoUpdateData(UpdateStepOn.UpdateReadyToInstall);
						StartSelfElevated();
					}
					else if (SelfUpdateState == SelfUpdateState.Extracted)
					{
						update.CurrentlyUpdating = UpdateOn.InstallSelfUpdate;
						InstallUpdates(update.CurrentlyUpdating);
					}
					else
					{
						update.CurrentlyUpdating = UpdateOn.ClosingProcesses;
						InstallUpdates(update.CurrentlyUpdating);
					}
					break;
				}
			}
		}

		private void UpdateHelper_SenderProcessClosed(object sender, EventArgs e)
		{
			if (isAutoUpdateMode && !updateHelper.Installing)
			{
				if (updateHelper.RestartInfoSent)
				{
					updateHelper.Installing = true;
					UpdateHelper_RequestReceived(null, UpdateAction.UpdateStep, UpdateStep.Install);
				}
				else
				{
					CancelUpdate(ForceClose: true);
				}
			}
		}

		private bool FilterBadRequest(UpdateStep s)
		{
			if (SelfUpdateState == SelfUpdateState.Downloaded && update.CurrentlyUpdating != UpdateOn.ExtractSelfUpdate && s != 0)
			{
				updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
				update.CurrentlyUpdating = UpdateOn.ExtractSelfUpdate;
				InstallUpdates(update.CurrentlyUpdating);
				return true;
			}
			switch (s)
			{
			case UpdateStep.ForceRecheckForUpdate:
				if (frameOn == Frame.Checking && downloader != null)
				{
					updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
					return true;
				}
				if (frameOn != Frame.Checking && frameOn != Frame.UpdateInfo)
				{
					return true;
				}
				break;
			case UpdateStep.CheckForUpdate:
				if (frameOn == Frame.Checking && downloader != null)
				{
					updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
					return true;
				}
				if (frameOn != Frame.Checking)
				{
					updateHelper.SendSuccess(ServerFile.NewVersion, panelDisplaying.GetChanges(rtf: true), ed2IsRtf: true);
					return true;
				}
				break;
			case UpdateStep.DownloadUpdate:
				if (frameOn == Frame.Checking)
				{
					if (downloader == null)
					{
						autoUpdateStepProcessing = UpdateStep.CheckForUpdate;
						updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
						CheckForUpdate();
					}
					else
					{
						updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
					}
					return true;
				}
				if (frameOn == Frame.InstallUpdates)
				{
					if (IsInDownloadState())
					{
						updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
					}
					else
					{
						updateHelper.SendSuccess(UpdateStep.DownloadUpdate);
					}
					return true;
				}
				break;
			case UpdateStep.BeginExtraction:
				if (frameOn == Frame.Checking)
				{
					if (downloader == null)
					{
						autoUpdateStepProcessing = UpdateStep.CheckForUpdate;
						updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
						CheckForUpdate();
					}
					else
					{
						updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
					}
					return true;
				}
				if (frameOn == Frame.UpdateInfo)
				{
					ShowFrame(Frame.InstallUpdates);
					autoUpdateStepProcessing = UpdateStep.DownloadUpdate;
					updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
					DownloadUpdate();
					return true;
				}
				if (frameOn == Frame.InstallUpdates)
				{
					if (IsInDownloadState())
					{
						updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
						return true;
					}
					if (updtDetails != null)
					{
						updateHelper.SendSuccess(UpdateStep.BeginExtraction);
						return true;
					}
					if (currentlyExtracting)
					{
						updateHelper.SendProgress(0, UpdateStep.BeginExtraction);
						return true;
					}
				}
				break;
			case UpdateStep.RestartInfo:
			case UpdateStep.Install:
				if (frameOn == Frame.Checking)
				{
					if (downloader == null)
					{
						autoUpdateStepProcessing = UpdateStep.CheckForUpdate;
						updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
						CheckForUpdate();
					}
					else
					{
						updateHelper.SendProgress(0, UpdateStep.CheckForUpdate);
					}
					return true;
				}
				if (frameOn == Frame.UpdateInfo)
				{
					ShowFrame(Frame.InstallUpdates);
					autoUpdateStepProcessing = UpdateStep.DownloadUpdate;
					updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
					DownloadUpdate();
					return true;
				}
				if (frameOn == Frame.InstallUpdates)
				{
					if (IsInDownloadState())
					{
						updateHelper.SendProgress(0, UpdateStep.DownloadUpdate);
						return true;
					}
					if (currentlyExtracting)
					{
						updateHelper.SendProgress(0, UpdateStep.BeginExtraction);
						return true;
					}
				}
				break;
			}
			return false;
		}

		private bool IsInDownloadState()
		{
			if (update.CurrentlyUpdating != UpdateOn.DownloadingUpdate && update.CurrentlyUpdating != 0)
			{
				return update.CurrentlyUpdating == UpdateOn.ExtractSelfUpdate;
			}
			return true;
		}

		private string CreateAutoUpdateTempFolder()
		{
			oldAUTempFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "wyUpdate AU\\cache\\" + update.GUID);
			if (!Directory.Exists(oldAUTempFolder))
			{
				return GetCacheFolder(update.GUID);
			}
			return oldAUTempFolder;
		}

		private static string GetCacheFolder(string guid)
		{
			string text = SystemFolders.GetUserProfile();
			if (string.IsNullOrEmpty(text))
			{
				text = SystemFolders.GetCurrentUserAppData();
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new Exception("Failed to retrieve the user profile folder.");
			}
			string text2 = Path.Combine(text, "wc");
			if (!Directory.Exists(text2))
			{
				Directory.CreateDirectory(text2);
				File.SetAttributes(text2, FileAttributes.Hidden | FileAttributes.System);
			}
			string text3 = Path.Combine(text2, guid);
			if (Directory.Exists(text3))
			{
				string path = Path.Combine(text3, guid);
				if (!File.Exists(path))
				{
					string[] directories = Directory.GetDirectories(text3);
					string[] array = directories;
					foreach (string path2 in array)
					{
						Directory.Delete(path2, recursive: true);
					}
					directories = Directory.GetFiles(text3);
					string[] array2 = directories;
					foreach (string path3 in array2)
					{
						File.Delete(path3);
					}
					using (File.Create(path))
					{
						return text3;
					}
				}
				return text3;
			}
			string text4 = null;
			string[] directories2 = Directory.GetDirectories(text2);
			for (int k = 0; k < directories2.Length; k++)
			{
				string fileName = Path.GetFileName(directories2[k]);
				if (!string.IsNullOrEmpty(fileName) && guid.IndexOf(fileName) == 0)
				{
					if (File.Exists(Path.Combine(directories2[k], guid)))
					{
						return directories2[k];
					}
					text4 = fileName;
				}
			}
			string text5 = Path.Combine(text2, guid.Substring(0, (text4 == null) ? 1 : (text4.Length + 1)));
			Directory.CreateDirectory(text5);
			using (File.Create(Path.Combine(text5, guid)))
			{
				return text5;
			}
		}

		private void PrepareStepOn(UpdateStepOn step)
		{
			switch (step)
			{
			case UpdateStepOn.Checking:
				ShowFrame(Frame.Checking);
				break;
			case UpdateStepOn.UpdateAvailable:
				ShowFrame(Frame.UpdateInfo);
				break;
			case UpdateStepOn.UpdateDownloaded:
				update.CurrentlyUpdating = UpdateOn.Extracting;
				needElevation = NeedElevationToUpdate();
				ShowFrame(Frame.InstallUpdates);
				panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;
				break;
			case UpdateStepOn.UpdateReadyToInstall:
			{
				string text = Path.Combine(tempDirectory, "updtdetails.udt");
				if (File.Exists(text))
				{
					updtDetails = UpdateDetails.Load(text);
					update.CurrentlyUpdating = UpdateOn.ClosingProcesses;
					needElevation = NeedElevationToUpdate();
					ShowFrame(Frame.InstallUpdates);
					panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;
					SetStepStatus(1, clientLang.Extract);
					break;
				}
				throw new Exception("Update details file does not exist.");
			}
			default:
				throw new Exception("Can't restore from this automatic update state: " + step);
			}
		}

		private void SaveAutoUpdateData(UpdateStepOn updateStepOn)
		{
			using (FileStream fileStream = new FileStream(autoUpdateStateFile, FileMode.Create, FileAccess.Write))
			{
				WriteFiles.WriteHeader(fileStream, "IUAUFV1");
				if (SelfUpdateState == SelfUpdateState.Extracted)
				{
					string text = Path.Combine(tempDirectory, "selfUpdate.sup");
					SaveSelfUpdateData(text);
					WriteFiles.WriteString(fileStream, 13, text);
				}
				WriteFiles.WriteInt(fileStream, 1, (int)updateStepOn);
				if (updateHelper.FileOrServiceToExecuteAfterUpdate != null)
				{
					WriteFiles.WriteString(fileStream, 2, updateHelper.FileOrServiceToExecuteAfterUpdate);
				}
				if (updateHelper.AutoUpdateID != null)
				{
					WriteFiles.WriteString(fileStream, 3, updateHelper.AutoUpdateID);
				}
				if (updateHelper.ExecutionArguments != null)
				{
					WriteFiles.WriteString(fileStream, 12, updateHelper.ExecutionArguments);
				}
				if (updateHelper.IsAService)
				{
					fileStream.WriteByte(128);
				}
				if (!string.IsNullOrEmpty(serverFileLoc))
				{
					WriteFiles.WriteString(fileStream, 4, serverFileLoc);
				}
				if (!string.IsNullOrEmpty(clientSFLoc))
				{
					WriteFiles.WriteString(fileStream, 5, clientSFLoc);
				}
				if (!string.IsNullOrEmpty(oldAUTempFolder))
				{
					WriteFiles.WriteString(fileStream, 6, oldAUTempFolder);
				}
				if (!string.IsNullOrEmpty(tempDirectory))
				{
					WriteFiles.WriteString(fileStream, 11, tempDirectory);
				}
				if (!string.IsNullOrEmpty(updateFilename))
				{
					WriteFiles.WriteString(fileStream, 7, updateFilename);
				}
				if (SelfUpdateState != 0)
				{
					WriteFiles.WriteInt(fileStream, 8, (int)SelfUpdateState);
					if (SelfUpdateState == SelfUpdateState.Downloaded)
					{
						WriteFiles.WriteString(fileStream, 9, updateFilename);
					}
					else if (SelfUpdateState == SelfUpdateState.Extracted)
					{
						WriteFiles.WriteString(fileStream, 9, newSelfLocation);
						WriteFiles.WriteString(fileStream, 10, oldSelfLocation);
					}
				}
				fileStream.WriteByte(byte.MaxValue);
			}
		}

		private void LoadAutoUpdateData()
		{
			autoUpdateStateFile = Path.Combine(tempDirectory, "autoupdate");
			using (FileStream fileStream = new FileStream(autoUpdateStateFile, FileMode.Open, FileAccess.Read))
			{
				if (!ReadFiles.IsHeaderValid(fileStream, "IUAUFV1"))
				{
					throw new Exception("Auto update state file ID is wrong.");
				}
				byte b = (byte)fileStream.ReadByte();
				while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
				{
					switch (b)
					{
					case 1:
						startStep = (UpdateStepOn)ReadFiles.ReadInt(fileStream);
						break;
					case 2:
						updateHelper.FileOrServiceToExecuteAfterUpdate = ReadFiles.ReadString(fileStream);
						break;
					case 3:
						updateHelper.AutoUpdateID = ReadFiles.ReadString(fileStream);
						break;
					case 12:
						updateHelper.ExecutionArguments = ReadFiles.ReadString(fileStream);
						break;
					case 128:
						updateHelper.IsAService = true;
						break;
					case 4:
						serverFileLoc = ReadFiles.ReadString(fileStream);
						if (!File.Exists(serverFileLoc))
						{
							serverFileLoc = null;
						}
						break;
					case 5:
						clientSFLoc = ReadFiles.ReadString(fileStream);
						if (!File.Exists(clientSFLoc))
						{
							clientSFLoc = null;
						}
						break;
					case 6:
						oldAUTempFolder = ReadFiles.ReadString(fileStream);
						break;
					case 11:
						tempDirectory = ReadFiles.ReadString(fileStream);
						break;
					case 7:
						updateFilename = ReadFiles.ReadString(fileStream);
						break;
					case 8:
						SelfUpdateState = (SelfUpdateState)ReadFiles.ReadInt(fileStream);
						break;
					case 9:
						if (SelfUpdateState == SelfUpdateState.Downloaded)
						{
							updateFilename = ReadFiles.ReadString(fileStream);
						}
						else
						{
							newSelfLocation = ReadFiles.ReadString(fileStream);
						}
						break;
					case 10:
						oldSelfLocation = ReadFiles.ReadString(fileStream);
						break;
					case 13:
						LoadSelfUpdateData(ReadFiles.ReadString(fileStream));
						break;
					default:
						ReadFiles.SkipField(fileStream, b);
						break;
					}
					b = (byte)fileStream.ReadByte();
				}
			}
			if (serverFileLoc == null)
			{
				startStep = UpdateStepOn.Checking;
				return;
			}
			LoadServerFile(setChangesText: true);
			if (frameOn != Frame.Checking)
			{
				RemoveTempDirectory();
			}
			else if (SelfUpdateState == SelfUpdateState.Extracted && !IsNewSelf)
			{
				StartNewSelfAndClose();
			}
			else if (SelfUpdateState == SelfUpdateState.WillUpdate || SelfUpdateState == SelfUpdateState.Downloaded)
			{
				LoadClientServerFile();
			}
		}

		public static bool RunProcess(string name)
		{
			Process process = new Process();
			try
			{
				process.StartInfo.WorkingDirectory = "c:\\Kiosk\\";
				process.StartInfo.FileName = name;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
				process.Start();
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool FindAndKillProcess(string name)
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

		public static bool FindProcess(string name)
		{
			name = name.ToLower();
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().StartsWith(name))
				{
					return true;
				}
			}
			return false;
		}

		[DllImport("user32")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		private void btnNext_Click(object sender, EventArgs e)
		{
			if (base.WindowState == FormWindowState.Minimized)
			{
				string text = "Kiosk";
				Process[] processes = Process.GetProcesses();
				foreach (Process process in processes)
				{
					if (process.ProcessName.ToLower().Contains(text.ToLower()))
					{
						IntPtr mainWindowHandle = process.MainWindowHandle;
						SetForegroundWindow(mainWindowHandle);
						return;
					}
				}
			}
			tWatchDog.Enabled = false;
			if (frameOn == Frame.UpdatedSuccessfully || frameOn == Frame.AlreadyUpToDate || frameOn == Frame.NoUpdatePathAvailable || frameOn == Frame.Error)
			{
				FindAndKillProcess(EXE_RUN);
				Application.Restart();
			}
			else if (FrameIs.ErrorFinish(frameOn))
			{
				Close();
			}
			else if (needElevation || SelfUpdateState == SelfUpdateState.WillUpdate)
			{
				StartSelfElevated();
			}
			else
			{
				ShowFrame(frameOn + 1);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			CancelUpdate();
		}

		private void CancelUpdate(bool ForceClose = false, bool skipConfirmDialog = false)
		{
			if ((frameOn == Frame.Checking || frameOn == Frame.InstallUpdates) && !ForceClose)
			{
				if (frameOn == Frame.InstallUpdates && !IsDownloading())
				{
					installUpdate.Pause(pause: true);
					panelDisplaying.PauseProgressBar();
				}
				DialogResult dialogResult = skipConfirmDialog ? DialogResult.Yes : MessageBox.Show(clientLang.CancelDialog.Content, clientLang.CancelDialog.Title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
				panelDisplaying.UnPauseProgressBar();
				if (dialogResult == DialogResult.Yes)
				{
					if (frameOn != Frame.Checking && frameOn != Frame.InstallUpdates)
					{
						isCancelled = true;
						return;
					}
					isCancelled = true;
					if (IsDownloading())
					{
						if (downloader != null)
						{
							downloader.Cancel();
						}
						return;
					}
					if (frameOn == Frame.InstallUpdates && !IsDownloading())
					{
						installUpdate.Cancel();
					}
					DisableCancel();
				}
				else if (frameOn == Frame.InstallUpdates && !IsDownloading())
				{
					installUpdate.Pause(pause: false);
				}
			}
			else
			{
				isCancelled = true;
			}
		}

		private bool IsDownloading()
		{
			if (frameOn != Frame.Checking)
			{
				if (frameOn == Frame.InstallUpdates && downloader != null)
				{
					if (update.CurrentlyUpdating != UpdateOn.DownloadingUpdate)
					{
						return update.CurrentlyUpdating == UpdateOn.DownloadingSelfUpdate;
					}
					return true;
				}
				return false;
			}
			return true;
		}

		private void DisableCancel()
		{
			if (btnCancel.Enabled)
			{
				SystemMenu.DisableCloseButton(this);
			}
			btnCancel.Enabled = false;
			btnCancel.Visible = false;
		}

		private void EnableCancel()
		{
			if (!btnCancel.Enabled)
			{
				SystemMenu.EnableCloseButton(this);
			}
			btnCancel.Enabled = true;
			btnCancel.Visible = true;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			if (!btnCancel.Enabled)
			{
				SystemMenu.DisableCloseButton(this);
			}
			base.OnSizeChanged(e);
		}

		private void SetButtonText()
		{
			btnNext.Text = clientLang.NextButton;
			btnCancel.Text = clientLang.CancelButton;
		}

		private void btnCancel_SizeChanged(object sender, EventArgs e)
		{
		}

		private void CheckForUpdate()
		{
			if (!string.IsNullOrEmpty(serverOverwrite))
			{
				BeginDownload(new List<string>
				{
					serverOverwrite
				}, 0L, null, relativeProgress: false, checkSigning: false);
			}
			else
			{
				BeginDownload(update.ServerFileSites, 0L, null, relativeProgress: false, checkSigning: false);
			}
		}

		private void DownloadUpdate()
		{
			if (SelfUpdateState == SelfUpdateState.FullUpdate || (isAutoUpdateMode && SelfUpdateState == SelfUpdateState.WillUpdate))
			{
				update.CurrentlyUpdating = UpdateOn.DownloadingSelfUpdate;
				BeginSelfUpdateDownload(updateFrom.FileSites, updateFrom.Adler32);
			}
			else
			{
				update.CurrentlyUpdating = UpdateOn.DownloadingUpdate;
				BeginDownload(updateFrom.FileSites, updateFrom.Adler32, updateFrom.SignedSHA1Hash, relativeProgress: true, checkSigning: true);
			}
		}

		private void BeginDownload(List<string> sites, long adler32, byte[] signedSHA1Hash, bool relativeProgress, bool checkSigning)
		{
			if (downloader != null)
			{
				downloader.ProgressChanged -= ShowProgress;
				downloader.ProgressChanged -= SelfUpdateProgress;
			}
			downloader = new FileDownloader(sites, tempDirectory)
			{
				Adler32 = adler32,
				UseRelativeProgress = relativeProgress,
				SignedSHA1Hash = signedSHA1Hash,
				PublicSignKey = (checkSigning ? update.PublicSignKey : null)
			};
			downloader.ProgressChanged += ShowProgress;
			downloader.Download();
		}

		private void BeginSelfUpdateDownload(List<string> sites, long adler32)
		{
			if (downloader != null)
			{
				downloader.ProgressChanged -= ShowProgress;
				downloader.ProgressChanged -= SelfUpdateProgress;
			}
			string text = Path.Combine(tempDirectory, "selfupdate");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			downloader = new FileDownloader(sites, text)
			{
				Adler32 = adler32
			};
			downloader.ProgressChanged += SelfUpdateProgress;
			downloader.Download();
		}

		private void DownloadClientSFSuccess()
		{
			ServerFile serverFile = ServerFile.Load(clientSFLoc, updatePathVar, customUrlArgs);
			if (VersionTools.Compare(VersionTools.FromExecutingAssembly(), serverFile.NewVersion) < 0)
			{
				SelfUpdateState = SelfUpdateState.WillUpdate;
				if (isAutoUpdateMode)
				{
					SelfServerFile = serverFile;
					LoadClientServerFile();
				}
			}
			if (SkipUpdateInfo)
			{
				needElevation = NeedElevationToUpdate();
				if (needElevation || SelfUpdateState == SelfUpdateState.WillUpdate)
				{
					StartSelfElevated();
				}
				else
				{
					ShowFrame(Frame.InstallUpdates);
				}
			}
			else
			{
				ShowFrame(Frame.UpdateInfo);
			}
		}

		private void LoadClientServerFile()
		{
			if (SelfServerFile == null)
			{
				SelfServerFile = ServerFile.Load(clientSFLoc, updatePathVar, customUrlArgs);
			}
			updateFrom = SelfServerFile.GetVersionChoice(VersionTools.FromExecutingAssembly());
		}

		private void ServerDownloadedSuccessfully()
		{
			LoadServerFile(setChangesText: true);
			if (frameOn == Frame.Checking)
			{
				if (isAutoUpdateMode)
				{
					autoUpdateStateFile = Path.Combine(tempDirectory, "autoupdate");
				}
				BeginSelfUpdateDownload(update.ClientServerSites, 0L);
			}
		}

		private void LoadServerFile(bool setChangesText)
		{
			ServerFile = ServerFile.Load(serverFileLoc, updatePathVar, customUrlArgs);
			clientLang.NewVersion = ServerFile.NewVersion;
			if (VersionTools.Compare(update.InstalledVersion, ServerFile.NewVersion) >= 0)
			{
				if (isAutoUpdateMode)
				{
					updateHelper.SendSuccess(null, null, ed2IsRtf: true);
					isCancelled = true;
					isAutoUpdateMode = false;
					frameOn = Frame.AlreadyUpToDate;
					Close();
				}
				else
				{
					ShowFrame(Frame.AlreadyUpToDate);
				}
				return;
			}
			updateFrom = ServerFile.GetVersionChoice(update.InstalledVersion);
			if ((updateFrom.InstallingTo & InstallingTo.SysDirx64) == InstallingTo.SysDirx64 && !SystemFolders.Is64Bit())
			{
				error = "Update available, but can't install 64-bit files on a 32-bit machine.";
				errorDetails = "There's an update available (version " + ServerFile.NewVersion + "). However, this update will install files to the x64 (64-bit) system32 folder. And because this machine is an x86 (32-bit), there isn't an x64 system32 folder.";
				ShowFrame(Frame.Error);
			}
			else if ((updateFrom.InstallingTo & InstallingTo.CommonFilesx64) == InstallingTo.CommonFilesx64 && !SystemFolders.Is64Bit())
			{
				error = "Update available, but can't install 64-bit files on a 32-bit machine.";
				errorDetails = "There's an update available (version " + ServerFile.NewVersion + "). However, this update will install files to the x64 (64-bit) \"Program File\\Common Files\" folder. And because this machine is an x86 (32-bit), there isn't an x64 \"Program File\\Common Files\" folder.";
				ShowFrame(Frame.Error);
			}
			else
			{
				if (!setChangesText && !isAutoUpdateMode)
				{
					return;
				}
				int num = ServerFile.VersionChoices.IndexOf(updateFrom);
				for (int num2 = ServerFile.VersionChoices.Count - 1; num2 >= num; num2--)
				{
					if (num2 != ServerFile.VersionChoices.Count - 1 && (!ServerFile.CatchAllUpdateExists || (ServerFile.CatchAllUpdateExists && num2 != ServerFile.VersionChoices.Count - 2)))
					{
						panelDisplaying.AppendAndBoldText("\r\n\r\n" + ServerFile.VersionChoices[num2 + 1].Version + ":\r\n\r\n");
					}
					if (!ServerFile.CatchAllUpdateExists || (ServerFile.CatchAllUpdateExists && num2 != ServerFile.VersionChoices.Count - 2))
					{
						if (ServerFile.VersionChoices[num2].RTFChanges)
						{
							panelDisplaying.AppendRichText(ServerFile.VersionChoices[num2].Changes);
						}
						else
						{
							panelDisplaying.AppendText(ServerFile.VersionChoices[num2].Changes);
						}
					}
				}
			}
		}

		[DllImport("user32.dll")]
		private static extern bool ShutdownBlockReasonCreate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string pwszReason);

		[DllImport("user32.dll")]
		private static extern bool ShutdownBlockReasonDestroy(IntPtr hWnd);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

		private void BlockLogOff(bool block)
		{
			logOffBlocked = block;
			if (block)
			{
				if (VistaTools.AtLeastVista())
				{
					ShutdownBlockReasonCreate(base.Handle, clientLang.LogOffError);
				}
				SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
			}
			else
			{
				if (VistaTools.AtLeastVista())
				{
					ShutdownBlockReasonDestroy(base.Handle);
				}
				SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
			}
		}

		protected override void WndProc(ref Message aMessage)
		{
			if (!logOffBlocked || (aMessage.Msg != 17 && aMessage.Msg != 22))
			{
				base.WndProc(ref aMessage);
			}
		}

		private void ShowProgress(int percentDone, int unweightedPercent, string extraStatus, ProgressStatus status, object payload)
		{
			if (base.IsDisposed)
			{
				return;
			}
			if (percentDone > -1 && percentDone < 101)
			{
				panelDisplaying.Progress = percentDone;
				if (isAutoUpdateMode && autoUpdateStepProcessing != UpdateStep.Install)
				{
					updateHelper.SendProgress(unweightedPercent, autoUpdateStepProcessing);
				}
			}
			if (!string.IsNullOrEmpty(extraStatus) && extraStatus != panelDisplaying.ProgressStatus)
			{
				panelDisplaying.ProgressStatus = extraStatus;
			}
			if (status == ProgressStatus.SharingViolation)
			{
				if (inUseForm == null)
				{
					inUseForm = new frmFilesInUse(clientLang, (string)payload);
					inUseForm.ShowDialog(this);
					if (inUseForm != null && inUseForm.CancelUpdate)
					{
						CancelUpdate(ForceClose: false, skipConfirmDialog: true);
					}
				}
				return;
			}
			if (inUseForm != null)
			{
				inUseForm.Close();
				inUseForm.Dispose();
				inUseForm = null;
			}
			if (installUpdate != null && (status == ProgressStatus.Success || status == ProgressStatus.Failure))
			{
				installUpdate.Rollback -= ChangeRollback;
				installUpdate.ProgressChanged -= ShowProgress;
			}
			if (status == ProgressStatus.Success)
			{
				if (frameOn == Frame.Checking)
				{
					serverFileLoc = downloader.DownloadingTo;
					try
					{
						ServerDownloadedSuccessfully();
					}
					catch (NoUpdatePathToNewestException)
					{
						ShowFrame(Frame.NoUpdatePathAvailable);
						return;
					}
					catch (Exception ex2)
					{
						status = ProgressStatus.Failure;
						payload = ex2;
					}
				}
				else
				{
					if (update.CurrentlyUpdating == UpdateOn.DownloadingUpdate)
					{
						updateFilename = downloader.DownloadingTo;
					}
					StepCompleted();
				}
			}
			if (status != ProgressStatus.Failure)
			{
				return;
			}
			if (isCancelled)
			{
				Close();
				return;
			}
			if (frameOn == Frame.Checking)
			{
				error = clientLang.ServerError;
				errorDetails = ((Exception)payload).Message;
			}
			else if (update.CurrentlyUpdating == UpdateOn.DownloadingUpdate)
			{
				error = clientLang.DownloadError;
				errorDetails = ((Exception)payload).Message;
			}
			else
			{
				if (payload is PatchApplicationException && updateFrom != ServerFile.VersionChoices[ServerFile.VersionChoices.Count - 1] && ServerFile.VersionChoices[ServerFile.VersionChoices.Count - 1].Version == ServerFile.NewVersion)
				{
					updateFrom = ServerFile.VersionChoices[ServerFile.VersionChoices.Count - 1];
					error = null;
					panelDisplaying.UpdateItems[1].Status = UpdateItemStatus.Nothing;
					if (isAutoUpdateMode)
					{
						autoUpdateStepProcessing = UpdateStep.DownloadUpdate;
					}
					DownloadUpdate();
					currentlyExtracting = false;
					return;
				}
				error = clientLang.GeneralUpdateError;
				errorDetails = ((Exception)payload).Message;
			}
			ShowFrame(Frame.Error);
		}

		private void SelfUpdateProgress(int percentDone, int unweightedPercent, string extraStatus, ProgressStatus status, object payload)
		{
			if (base.IsDisposed)
			{
				return;
			}
			if (percentDone > -1 && percentDone < 101)
			{
				panelDisplaying.Progress = percentDone;
			}
			if (!string.IsNullOrEmpty(extraStatus) && extraStatus != panelDisplaying.ProgressStatus)
			{
				panelDisplaying.ProgressStatus = extraStatus;
			}
			if (installUpdate != null && (status == ProgressStatus.Success || status == ProgressStatus.Failure))
			{
				installUpdate.ProgressChanged -= SelfUpdateProgress;
			}
			if (status == ProgressStatus.Success)
			{
				if (frameOn == Frame.Checking)
				{
					clientSFLoc = downloader.DownloadingTo;
					try
					{
						DownloadClientSFSuccess();
					}
					catch (Exception ex)
					{
						status = ProgressStatus.Failure;
						payload = ex;
					}
				}
				else
				{
					switch (update.CurrentlyUpdating)
					{
					case UpdateOn.DownloadingSelfUpdate:
						updateFilename = downloader.DownloadingTo;
						if (isAutoUpdateMode)
						{
							SelfUpdateState = SelfUpdateState.Downloaded;
							SaveAutoUpdateData(UpdateStepOn.UpdateAvailable);
							update.CurrentlyUpdating = UpdateOn.ExtractSelfUpdate;
							InstallUpdates(update.CurrentlyUpdating);
						}
						else
						{
							panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;
							update.CurrentlyUpdating = UpdateOn.FullSelfUpdate;
							InstallUpdates(update.CurrentlyUpdating);
						}
						break;
					case UpdateOn.FullSelfUpdate:
						panelDisplaying.UpdateItems[1].Status = UpdateItemStatus.Success;
						StartSelfElevated();
						break;
					case UpdateOn.ExtractSelfUpdate:
						SelfUpdateState = SelfUpdateState.Extracted;
						newSelfLocation = installUpdate.NewSelfLoc;
						SaveAutoUpdateData(UpdateStepOn.UpdateAvailable);
						StartNewSelfAndClose();
						return;
					case UpdateOn.InstallSelfUpdate:
						SelfUpdateState = SelfUpdateState.None;
						SaveAutoUpdateData(UpdateStepOn.UpdateReadyToInstall);
						IsNewSelf = false;
						StartSelfElevated();
						return;
					}
				}
			}
			if (status != ProgressStatus.Failure)
			{
				return;
			}
			bool flag = VersionTools.Compare(VersionTools.FromExecutingAssembly(), ServerFile.MinClientVersion) < 0;
			bool flag2 = frameOn != Frame.Checking && (object)payload.GetType() == typeof(PatchApplicationException) && updateFrom != SelfServerFile.VersionChoices[SelfServerFile.VersionChoices.Count - 1] && SelfServerFile.VersionChoices[SelfServerFile.VersionChoices.Count - 1].Version == SelfServerFile.NewVersion;
			if (flag && !flag2)
			{
				error = clientLang.SelfUpdateInstallError;
				errorDetails = ((Exception)payload).Message;
				ShowFrame(Frame.Error);
			}
			else if (frameOn == Frame.Checking)
			{
				SelfUpdateState = SelfUpdateState.None;
				if (SkipUpdateInfo)
				{
					needElevation = NeedElevationToUpdate();
					if (needElevation)
					{
						StartSelfElevated();
					}
					else
					{
						ShowFrame(Frame.InstallUpdates);
					}
				}
				else
				{
					ShowFrame(Frame.UpdateInfo);
				}
			}
			else if (flag2)
			{
				updateFrom = SelfServerFile.VersionChoices[SelfServerFile.VersionChoices.Count - 1];
				error = null;
				errorDetails = null;
				panelDisplaying.UpdateItems[1].Status = UpdateItemStatus.Nothing;
				if (isAutoUpdateMode)
				{
					SelfUpdateState = SelfUpdateState.WillUpdate;
					SaveAutoUpdateData(UpdateStepOn.UpdateAvailable);
				}
				DownloadUpdate();
			}
			else if (isAutoUpdateMode)
			{
				SelfUpdateState = SelfUpdateState.None;
				if (update.CurrentlyUpdating == UpdateOn.InstallSelfUpdate)
				{
					SaveAutoUpdateData(UpdateStepOn.UpdateReadyToInstall);
					UpdateHelper_RequestReceived(this, UpdateAction.UpdateStep, UpdateStep.Install);
				}
				else
				{
					SaveAutoUpdateData(UpdateStepOn.UpdateAvailable);
					UpdateHelper_RequestReceived(this, UpdateAction.UpdateStep, UpdateStep.DownloadUpdate);
				}
			}
			else
			{
				StartSelfElevated();
			}
		}

		private void UninstallProgress(int percentDone, int unweightedPercent, string extraStatus, ProgressStatus status, object payload)
		{
			if (base.IsDisposed)
			{
				return;
			}
			if (percentDone > -1 && percentDone < 101)
			{
				panelDisplaying.Progress = percentDone;
			}
			if (!string.IsNullOrEmpty(extraStatus) && extraStatus != panelDisplaying.ProgressStatus)
			{
				panelDisplaying.ProgressStatus = extraStatus;
			}
			if (status == ProgressStatus.Success || status == ProgressStatus.Failure)
			{
				installUpdate.ProgressChanged -= UninstallProgress;
			}
			switch (status)
			{
			case ProgressStatus.None:
				panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;
				SetStepStatus(1, clientLang.UninstallRegistry);
				break;
			case ProgressStatus.Success:
				Close();
				break;
			case ProgressStatus.Failure:
				if (isSilent)
				{
					Close();
					break;
				}
				error = clientLang.GeneralUpdateError;
				errorDetails = ((Exception)payload).Message;
				ShowFrame(Frame.Error);
				break;
			}
		}

		private void CheckProcess(int percentDone, int unweightedPercent, string extraStatus, ProgressStatus status, object payload)
		{
			if (base.IsDisposed)
			{
				return;
			}
			if (!string.IsNullOrEmpty(extraStatus) && extraStatus != panelDisplaying.ProgressStatus)
			{
				panelDisplaying.ProgressStatus = extraStatus;
			}
			if (status == ProgressStatus.Success || status == ProgressStatus.Failure)
			{
				installUpdate.Rollback -= ChangeRollback;
				installUpdate.ProgressChanged -= CheckProcess;
			}
			if (status == ProgressStatus.Success)
			{
				List<FileInfo> files = null;
				List<Process> list = null;
				object[] array = payload as object[];
				if (array != null)
				{
					files = (List<FileInfo>)array[0];
					list = (List<Process>)array[1];
				}
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						try
						{
							if (list[i].HasExited)
							{
								list.RemoveAt(i);
							}
						}
						catch
						{
						}
					}
					if (list.Count > 0)
					{
						Show();
						base.TopMost = true;
						base.TopMost = false;
						using (Form form = new frmProcesses(files, list, clientLang))
						{
							if (form.ShowDialog() == DialogResult.Cancel)
							{
								CancelUpdate(ForceClose: true);
								return;
							}
						}
					}
				}
				update.CurrentlyUpdating++;
				InstallUpdates(update.CurrentlyUpdating);
			}
			if (status == ProgressStatus.Failure)
			{
				if (isCancelled)
				{
					Close();
					return;
				}
				error = clientLang.GeneralUpdateError;
				errorDetails = ((Exception)payload).Message;
				ShowFrame(Frame.Error);
			}
		}

		private void ChangeRollback(bool rbRegistry)
		{
			if (!base.IsDisposed)
			{
				DisableCancel();
				if (rbRegistry)
				{
					panelDisplaying.UpdateItems[2].Status = UpdateItemStatus.Error;
					SetStepStatus(3, clientLang.RollingBackRegistry);
				}
				else if (panelDisplaying.UpdateItems[2].Status != UpdateItemStatus.Error)
				{
					panelDisplaying.UpdateItems[1].Status = UpdateItemStatus.Error;
					SetStepStatus(2, clientLang.RollingBackFiles);
				}
				else
				{
					SetStepStatus(3, clientLang.RollingBackFiles);
				}
			}
		}

		private void SaveSelfUpdateData(string fileName)
		{
			using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(Encoding.UTF8.GetBytes("IUSUFV2"), 0, 7);
				WriteFiles.WriteDeprecatedString(fileStream, 1, clientFileLoc);
				WriteFiles.WriteDeprecatedString(fileStream, 2, serverFileLoc);
				WriteFiles.WriteDeprecatedString(fileStream, 3, clientSFLoc);
				WriteFiles.WriteDeprecatedString(fileStream, 4, baseDirectory);
				WriteFiles.WriteDeprecatedString(fileStream, 5, tempDirectory);
				WriteFiles.WriteDeprecatedString(fileStream, 6, VersionTools.SelfLocation);
				WriteFiles.WriteBool(fileStream, 7, SelfUpdateState == SelfUpdateState.WillUpdate);
				WriteFiles.WriteBool(fileStream, 8, needElevation);
				if (!string.IsNullOrEmpty(serverOverwrite))
				{
					WriteFiles.WriteDeprecatedString(fileStream, 9, serverOverwrite);
				}
				if (!string.IsNullOrEmpty(updatePathVar))
				{
					WriteFiles.WriteString(fileStream, 12, updatePathVar);
				}
				if (!string.IsNullOrEmpty(customUrlArgs))
				{
					WriteFiles.WriteString(fileStream, 13, customUrlArgs);
				}
				if (!string.IsNullOrEmpty(forcedLanguageCulture))
				{
					WriteFiles.WriteString(fileStream, 14, forcedLanguageCulture);
				}
				if (isAutoUpdateMode)
				{
					fileStream.WriteByte(128);
				}
				if (UpdatingFromService)
				{
					fileStream.WriteByte(130);
				}
				if (!string.IsNullOrEmpty(customProxyUrl))
				{
					WriteFiles.WriteString(fileStream, 15, customProxyUrl);
				}
				if (!string.IsNullOrEmpty(customProxyUser))
				{
					WriteFiles.WriteString(fileStream, 16, customProxyUser);
				}
				if (!string.IsNullOrEmpty(customProxyPassword))
				{
					WriteFiles.WriteString(fileStream, 17, customProxyPassword);
				}
				if (!string.IsNullOrEmpty(customProxyDomain))
				{
					WriteFiles.WriteString(fileStream, 18, customProxyDomain);
				}
				if (!string.IsNullOrEmpty(StartOnErr))
				{
					WriteFiles.WriteString(fileStream, 19, StartOnErr);
				}
				if (!string.IsNullOrEmpty(StartOnErrArgs))
				{
					WriteFiles.WriteString(fileStream, 20, StartOnErrArgs);
				}
				if (!string.IsNullOrEmpty(PasswordUpdateCmd))
				{
					WriteFiles.WriteString(fileStream, 21, PasswordUpdateCmd);
				}
				fileStream.WriteByte(byte.MaxValue);
			}
		}

		private void LoadSelfUpdateData(string fileName)
		{
			byte[] buffer = new byte[7];
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				fileStream.Read(buffer, 0, 7);
				string @string = Encoding.UTF8.GetString(buffer);
				if (@string != "IUSUFV2")
				{
					if (!(@string == "IUSUFV1"))
					{
						fileStream.Close();
						throw new Exception("Self update fileID is wrong: " + @string);
					}
					LoadSelfUpdateRC1Data(fileStream);
				}
				else
				{
					byte b = (byte)fileStream.ReadByte();
					while (!ReadFiles.ReachedEndByte(fileStream, b, byte.MaxValue))
					{
						switch (b)
						{
						case 1:
							clientFileLoc = ReadFiles.ReadDeprecatedString(fileStream);
							if (clientFileLoc.EndsWith("iuc", StringComparison.OrdinalIgnoreCase))
							{
								clientFileType = ClientFileType.PreRC2;
							}
							else if (clientFileLoc.EndsWith("iucz", StringComparison.OrdinalIgnoreCase))
							{
								clientFileType = ClientFileType.RC2;
							}
							else
							{
								clientFileType = ClientFileType.Final;
							}
							break;
						case 2:
							serverFileLoc = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 3:
							clientSFLoc = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 4:
							baseDirectory = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 5:
							tempDirectory = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 6:
							oldSelfLocation = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 7:
							SelfUpdateState = (ReadFiles.ReadBool(fileStream) ? SelfUpdateState.FullUpdate : SelfUpdateState.ContinuingRegularUpdate);
							break;
						case 8:
							needElevation = ReadFiles.ReadBool(fileStream);
							break;
						case 9:
							serverOverwrite = ReadFiles.ReadDeprecatedString(fileStream);
							break;
						case 12:
							updatePathVar = ReadFiles.ReadString(fileStream);
							break;
						case 13:
							customUrlArgs = ReadFiles.ReadString(fileStream);
							break;
						case 14:
							forcedLanguageCulture = ReadFiles.ReadString(fileStream);
							break;
						case 128:
							beginAutoUpdateInstallation = true;
							isAutoUpdateMode = true;
							break;
						case 130:
							UpdatingFromService = true;
							break;
						case 15:
							customProxyUrl = ReadFiles.ReadString(fileStream);
							break;
						case 16:
							customProxyUser = ReadFiles.ReadString(fileStream);
							break;
						case 17:
							customProxyPassword = ReadFiles.ReadString(fileStream);
							break;
						case 18:
							customProxyDomain = ReadFiles.ReadString(fileStream);
							break;
						case 19:
							StartOnErr = ReadFiles.ReadString(fileStream);
							break;
						case 20:
							StartOnErrArgs = ReadFiles.ReadString(fileStream);
							break;
						case 21:
							PasswordUpdateCmd = ReadFiles.ReadString(fileStream);
							break;
						default:
							ReadFiles.SkipField(fileStream, b);
							break;
						}
						b = (byte)fileStream.ReadByte();
					}
				}
			}
		}

		private void LoadSelfUpdateRC1Data(Stream fs)
		{
			selfUpdateFromRC1 = true;
			clientFileType = ClientFileType.PreRC2;
			byte b = (byte)fs.ReadByte();
			while (!ReadFiles.ReachedEndByte(fs, b, byte.MaxValue))
			{
				switch (b)
				{
				case 1:
					clientFileLoc = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 2:
					serverFileLoc = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 3:
					baseDirectory = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 4:
					tempDirectory = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 5:
					oldSelfLocation = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 6:
					newSelfLocation = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 7:
					if (ReadFiles.ReadBool(fs))
					{
						SelfUpdateState = SelfUpdateState.FullUpdate;
					}
					break;
				case 8:
					needElevation = ReadFiles.ReadBool(fs);
					break;
				default:
					ReadFiles.SkipField(fs, b);
					break;
				}
				b = (byte)fs.ReadByte();
			}
			fs.Close();
		}

		private void ShowFrame(Frame frameNum)
		{
			frameOn = frameNum;
			switch (frameNum)
			{
			case Frame.Checking:
				panelDisplaying.ChangePanel(FrameType.Update, clientLang.Checking.Title, clientLang.Checking.SubTitle, clientLang.Checking.Content, string.Empty);
				btnNext.Enabled = false;
				btnNext.Visible = false;
				if (!isAutoUpdateMode)
				{
					CheckForUpdate();
				}
				break;
			case Frame.UpdateInfo:
				panelDisplaying.ChangePanel(FrameType.TextInfo, clientLang.UpdateInfo.Title, clientLang.UpdateInfo.SubTitle, clientLang.UpdateInfo.Content, clientLang.UpdateBottom);
				needElevation = NeedElevationToUpdate();
				btnNext.Enabled = true;
				btnNext.Visible = true;
				btnNext.Text = clientLang.UpdateButton;
				if (QuickCheck)
				{
					if (QuickCheckJustCheck)
					{
						if (OutputInfo == string.Empty)
						{
							Console.WriteLine(ServerFile.NewVersion);
							Console.WriteLine(panelDisplaying.GetChanges(rtf: false));
						}
						else if (OutputInfo != null)
						{
							try
							{
								using (StreamWriter streamWriter = new StreamWriter(OutputInfo))
								{
									streamWriter.WriteLine(ServerFile.NewVersion);
									streamWriter.WriteLine(panelDisplaying.GetChanges(rtf: false));
								}
							}
							catch
							{
							}
						}
						ReturnCode = 2;
						Close();
						return;
					}
					base.ShowInTaskbar = true;
					base.WindowState = FormWindowState.Normal;
					base.TopMost = true;
					base.TopMost = false;
					QuickCheck = false;
				}
				else if (isAutoUpdateMode)
				{
					SaveAutoUpdateData(UpdateStepOn.UpdateAvailable);
					updateHelper.SendSuccess(ServerFile.NewVersion, panelDisplaying.GetChanges(rtf: true), ed2IsRtf: true);
				}
				panelDisplaying.ShowChecklist = true;
				panelDisplaying.ChangePanel(FrameType.Update, clientLang.DownInstall.Title, clientLang.DownInstall.SubTitle, clientLang.DownInstall.Content, string.Empty);
				if (SelfUpdateState == SelfUpdateState.FullUpdate)
				{
					SetStepStatus(0, clientLang.DownloadingSelfUpdate);
				}
				else
				{
					SetStepStatus(0, clientLang.Download);
				}
				if (!isAutoUpdateMode)
				{
					DownloadUpdate();
				}
				btnNext.Enabled = false;
				btnNext.Visible = false;
				break;
			case Frame.UpdatedSuccessfully:
				panelDisplaying.ChangePanel(FrameType.WelcomeFinish, clientLang.SuccessUpdate.Title, clientLang.SuccessUpdate.Content, string.Empty, string.Empty);
				btnNext.Enabled = true;
				btnNext.Visible = true;
				btnCancel.Enabled = false;
				btnCancel.Visible = false;
				btnNext.Text = clientLang.UpdateButton;
				if (OnlyInstall)
				{
					Application.Exit();
				}
				else
				{
					RunKiosk();
				}
				break;
			case Frame.AlreadyUpToDate:
				panelDisplaying.ChangePanel(FrameType.WelcomeFinish, clientLang.AlreadyLatest.Title, clientLang.AlreadyLatest.Content, string.Empty, string.Empty);
				btnNext.Enabled = true;
				btnNext.Visible = true;
				btnCancel.Enabled = false;
				btnCancel.Visible = false;
				btnNext.Text = clientLang.UpdateButton;
				if (OnlyInstall)
				{
					Application.Exit();
				}
				else
				{
					RunKiosk();
				}
				break;
			case Frame.NoUpdatePathAvailable:
				if (!string.IsNullOrEmpty(ServerFile.NoUpdateToLatestLinkText))
				{
					panelDisplaying.SetNoUpdateAvailableLink(ServerFile.NoUpdateToLatestLinkText, ServerFile.NoUpdateToLatestLinkURL);
				}
				panelDisplaying.ChangePanel(FrameType.WelcomeFinish, clientLang.NoUpdateToLatest.Title, clientLang.NoUpdateToLatest.Content, string.Empty, string.Empty);
				btnNext.Enabled = true;
				btnNext.Visible = true;
				btnCancel.Enabled = false;
				btnCancel.Visible = false;
				btnNext.Text = clientLang.UpdateButton;
				if (OnlyInstall)
				{
					Application.Exit();
				}
				else
				{
					RunKiosk();
				}
				break;
			case Frame.Uninstall:
				panelDisplaying.ShowChecklist = true;
				panelDisplaying.ChangePanel(FrameType.Update, clientLang.Uninstall.Title, clientLang.Uninstall.SubTitle, clientLang.Uninstall.Content, string.Empty);
				SetStepStatus(0, clientLang.UninstallFiles);
				btnNext.Enabled = false;
				btnNext.Visible = false;
				InstallUpdates(UpdateOn.Uninstalling);
				break;
			case Frame.Error:
				ReturnCode = 1;
				panelDisplaying.ErrorDetails = errorDetails;
				panelDisplaying.SetUpErrorDetails(clientLang.ShowDetails);
				panelDisplaying.ChangePanel(FrameType.WelcomeFinish, clientLang.UpdateError.Title, error, string.Empty, string.Empty);
				btnNext.Enabled = true;
				btnNext.Visible = true;
				btnCancel.Enabled = false;
				btnCancel.Visible = false;
				btnNext.Text = clientLang.UpdateButton;
				if (QuickCheck && !QuickCheckNoErr)
				{
					base.ShowInTaskbar = true;
					base.WindowState = FormWindowState.Normal;
					base.TopMost = true;
					base.TopMost = false;
					QuickCheck = false;
				}
				if (OnlyInstall)
				{
					Application.Exit();
				}
				else
				{
					RunKiosk();
				}
				break;
			}
			if (FrameIs.ErrorFinish(frameNum))
			{
				BlockLogOff(block: false);
				if (frameNum != Frame.UpdatedSuccessfully && frameNum != Frame.AlreadyUpToDate)
				{
					EnableCancel();
				}
				base.CancelButton = btnNext;
				ReturnCode = ((frameNum == Frame.Error) ? 1 : 0);
				if (QuickCheck)
				{
					if (frameNum != Frame.Error || QuickCheckNoErr)
					{
						if (frameNum == Frame.Error)
						{
							if (OutputInfo == string.Empty)
							{
								Console.WriteLine(error + "\r\n");
								Console.WriteLine(errorDetails);
							}
							else if (OutputInfo != null)
							{
								try
								{
									using (StreamWriter streamWriter2 = new StreamWriter(OutputInfo))
									{
										streamWriter2.WriteLine(error);
										streamWriter2.WriteLine(errorDetails);
									}
								}
								catch
								{
								}
							}
							if (StartOnErr != null)
							{
								try
								{
									LimitedProcess.Start(StartOnErr, StartOnErrArgs);
								}
								catch
								{
								}
							}
						}
						base.WindowState = FormWindowState.Minimized;
						base.ShowInTaskbar = false;
						base.Visible = true;
						Close();
						return;
					}
					base.Visible = true;
					base.TopMost = true;
					base.TopMost = false;
				}
				else
				{
					if (isAutoUpdateMode)
					{
						if (update.CurrentlyUpdating < UpdateOn.ClosingProcesses)
						{
							if (!updateHelper.RunningServer)
							{
								StartQuickAndDirtyAutoUpdateMode();
							}
							updateHelper.SendFailed(error, errorDetails, autoUpdateStepProcessing);
						}
						if (frameNum == Frame.UpdatedSuccessfully || frameNum == Frame.Error)
						{
							AutoUpdaterInfo autoUpdaterInfo2;
							if (frameNum == Frame.Error)
							{
								AutoUpdaterInfo autoUpdaterInfo = new AutoUpdaterInfo(updateHelper.AutoUpdateID, oldAUTempFolder);
								autoUpdaterInfo.AutoUpdaterStatus = AutoUpdaterStatus.UpdateFailed;
								autoUpdaterInfo.ErrorTitle = error;
								autoUpdaterInfo.ErrorMessage = errorDetails;
								autoUpdaterInfo2 = autoUpdaterInfo;
							}
							else
							{
								AutoUpdaterInfo autoUpdaterInfo3 = new AutoUpdaterInfo(updateHelper.AutoUpdateID, oldAUTempFolder);
								autoUpdaterInfo3.AutoUpdaterStatus = AutoUpdaterStatus.UpdateSucceeded;
								autoUpdaterInfo3.UpdateVersion = ServerFile.NewVersion;
								autoUpdaterInfo3.ChangesInLatestVersion = panelDisplaying.GetChanges(rtf: true);
								autoUpdaterInfo3.ChangesIsRTF = true;
								autoUpdaterInfo2 = autoUpdaterInfo3;
							}
							autoUpdaterInfo2.Save();
							try
							{
								if (updateHelper.IsAService)
								{
									using (ServiceController serviceController = new ServiceController(updateHelper.FileOrServiceToExecuteAfterUpdate))
									{
										if (updateHelper.ExecutionArguments != null)
										{
											string[] args = CmdLineToArgvW.SplitArgs(updateHelper.ExecutionArguments);
											serviceController.Start(args);
										}
										else
										{
											serviceController.Start();
										}
									}
								}
								else
								{
									LimitedProcess.Start(updateHelper.FileOrServiceToExecuteAfterUpdate, updateHelper.ExecutionArguments);
								}
							}
							catch
							{
							}
						}
						isAutoUpdateMode = false;
						Close();
						return;
					}
					if (UpdatingFromService || (update.CloseOnSuccess && frameNum == Frame.UpdatedSuccessfully) || (StartOnErr != null && frameNum == Frame.Error))
					{
						if (log != null)
						{
							if (frameNum == Frame.UpdatedSuccessfully)
							{
								log.Write("Updated successfully.");
							}
							else
							{
								log.Write(error + " - " + errorDetails);
							}
						}
						if (StartOnErr != null && frameNum == Frame.Error)
						{
							try
							{
								LimitedProcess.Start(StartOnErr, StartOnErrArgs);
							}
							catch
							{
								return;
							}
						}
						Close();
						return;
					}
				}
			}
			try
			{
				btnNext.Focus();
			}
			catch
			{
			}
			if (isSilent && FrameIs.Interaction(frameOn))
			{
				btnNext_Click(null, EventArgs.Empty);
			}
		}

		private void ShutDown()
		{
			Process process = new Process();
			try
			{
				process.StartInfo.WorkingDirectory = "C:\\Windows\\System32\\";
				process.StartInfo.FileName = "shutdown.exe";
				process.StartInfo.Arguments = "/r /t 1";
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
				process.Start();
			}
			catch
			{
			}
		}

		private void RunKiosk()
		{
			if (!OnlyInstall)
			{
				base.WindowState = FormWindowState.Minimized;
				Application.DoEvents();
				Process process = new Process();
				try
				{
					process.StartInfo.WorkingDirectory = "c:\\Kiosk\\";
					process.StartInfo.FileName = EXE_RUN + ".exe";
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
					process.Start();
				}
				catch
				{
					return;
				}
				tWatchDog.Enabled = true;
			}
		}

		private void InstallUpdates(UpdateOn CurrentlyUpdating)
		{
			switch (CurrentlyUpdating)
			{
			case UpdateOn.DownloadingUpdate:
				break;
			case UpdateOn.FullSelfUpdate:
				SetStepStatus(1, clientLang.SelfUpdate);
				installUpdate = new InstallUpdate
				{
					OldSelfLoc = oldSelfLocation,
					Filename = Path.Combine(tempDirectory, updateFilename),
					OutputDirectory = tempDirectory
				};
				installUpdate.ProgressChanged += SelfUpdateProgress;
				installUpdate.RunSelfUpdate();
				break;
			case UpdateOn.ExtractSelfUpdate:
				oldSelfLocation = VersionTools.SelfLocation;
				installUpdate = new InstallUpdate
				{
					OldSelfLoc = oldSelfLocation,
					Filename = Path.Combine(tempDirectory, updateFilename),
					OutputDirectory = Path.Combine(tempDirectory, "selfupdate")
				};
				installUpdate.ProgressChanged += SelfUpdateProgress;
				installUpdate.JustExtractSelfUpdate();
				break;
			case UpdateOn.InstallSelfUpdate:
				installUpdate = new InstallUpdate
				{
					OldSelfLoc = oldSelfLocation,
					NewSelfLoc = newSelfLocation,
					Filename = Path.Combine(tempDirectory, updateFilename),
					OutputDirectory = Path.Combine(tempDirectory, "selfupdate")
				};
				installUpdate.ProgressChanged += SelfUpdateProgress;
				installUpdate.JustInstallSelfUpdate();
				break;
			case UpdateOn.Extracting:
				currentlyExtracting = true;
				SetStepStatus(1, clientLang.Extract);
				installUpdate = new InstallUpdate
				{
					Filename = Path.Combine(tempDirectory, updateFilename),
					OutputDirectory = tempDirectory,
					TempDirectory = tempDirectory,
					ProgramDirectory = baseDirectory,
					ExtractPassword = (PasswordUpdateCmd ?? update.UpdatePassword)
				};
				installUpdate.ProgressChanged += ShowProgress;
				installUpdate.RunUnzipProcess();
				break;
			case UpdateOn.ClosingProcesses:
				SetStepStatus(1, clientLang.Processes);
				installUpdate = new InstallUpdate
				{
					UpdtDetails = updtDetails,
					TempDirectory = tempDirectory,
					ProgramDirectory = baseDirectory,
					SkipUIReporting = (UpdatingFromService || updateHelper.IsAService),
					SkipStartService = (updateHelper.IsAService ? updateHelper.FileOrServiceToExecuteAfterUpdate : null)
				};
				installUpdate.Rollback += ChangeRollback;
				installUpdate.ProgressChanged += CheckProcess;
				installUpdate.RunProcessesCheck();
				break;
			case UpdateOn.PreExecute:
				BlockLogOff(block: true);
				SetStepStatus(1, clientLang.PreExec);
				installUpdate = new InstallUpdate
				{
					UpdtDetails = updtDetails,
					TempDirectory = tempDirectory,
					ProgramDirectory = baseDirectory,
					IsAdmin = IsAdmin,
					MainWindowHandle = base.Handle
				};
				installUpdate.Rollback += ChangeRollback;
				installUpdate.ProgressChanged += ShowProgress;
				installUpdate.RunPreExecute();
				break;
			case UpdateOn.BackUpInstalling:
				SetStepStatus(1, clientLang.Files);
				installUpdate = new InstallUpdate
				{
					UpdtDetails = updtDetails,
					TempDirectory = tempDirectory,
					ProgramDirectory = baseDirectory,
					IsAdmin = IsAdmin,
					SkipUIReporting = (UpdatingFromService || updateHelper.IsAService)
				};
				installUpdate.Rollback += ChangeRollback;
				installUpdate.ProgressChanged += ShowProgress;
				installUpdate.RunUpdateFiles();
				break;
			case UpdateOn.ModifyReg:
				SetStepStatus(2, clientLang.Registry);
				installUpdate = new InstallUpdate
				{
					UpdtDetails = updtDetails,
					TempDirectory = tempDirectory,
					ProgramDirectory = baseDirectory
				};
				installUpdate.Rollback += ChangeRollback;
				installUpdate.ProgressChanged += ShowProgress;
				installUpdate.RunUpdateRegistry();
				break;
			case UpdateOn.OptimizeExecute:
				SetStepStatus(3, clientLang.Optimize);
				installUpdate = new InstallUpdate
				{
					UpdtDetails = updtDetails,
					TempDirectory = tempDirectory,
					ProgramDirectory = baseDirectory,
					SkipStartService = (updateHelper.IsAService ? updateHelper.FileOrServiceToExecuteAfterUpdate : null),
					IsAdmin = IsAdmin,
					MainWindowHandle = base.Handle
				};
				installUpdate.Rollback += ChangeRollback;
				installUpdate.ProgressChanged += ShowProgress;
				installUpdate.RunOptimizeExecute();
				break;
			case UpdateOn.WriteClientFile:
				update.InstalledVersion = ServerFile.NewVersion;
				installUpdate = new InstallUpdate
				{
					Filename = clientFileLoc,
					UpdtDetails = updtDetails,
					ClientFileType = clientFileType,
					ClientFile = update,
					SkipProgressReporting = true,
					TempDirectory = tempDirectory,
					ProgramDirectory = baseDirectory
				};
				installUpdate.Rollback += ChangeRollback;
				installUpdate.ProgressChanged += ShowProgress;
				installUpdate.RunUpdateClientDataFile();
				break;
			case UpdateOn.DeletingTemp:
				SetStepStatus(3, clientLang.TempFiles);
				installUpdate = new InstallUpdate
				{
					TempDirectory = tempDirectory
				};
				installUpdate.ProgressChanged += ShowProgress;
				installUpdate.RunDeleteTemporary();
				break;
			case UpdateOn.Uninstalling:
				installUpdate = new InstallUpdate
				{
					Filename = clientFileLoc
				};
				installUpdate.ProgressChanged += UninstallProgress;
				installUpdate.RunUninstall();
				break;
			}
		}

		private void SetStepStatus(int stepNum, string stepText)
		{
			panelDisplaying.ProgressStatus = string.Empty;
			panelDisplaying.UpdateItems[stepNum].Status = UpdateItemStatus.Working;
			panelDisplaying.UpdateItems[stepNum].Text = stepText;
		}

		private void StepCompleted()
		{
			if (update.CurrentlyUpdating == UpdateOn.DeletingTemp)
			{
				panelDisplaying.UpdateItems[3].Status = UpdateItemStatus.Success;
				ShowFrame(Frame.UpdatedSuccessfully);
				btnNext.Enabled = true;
				btnNext.Visible = true;
				return;
			}
			switch (update.CurrentlyUpdating)
			{
			case UpdateOn.DownloadingUpdate:
				panelDisplaying.UpdateItems[0].Status = UpdateItemStatus.Success;
				break;
			case UpdateOn.Extracting:
				updtDetails = installUpdate.UpdtDetails;
				break;
			case UpdateOn.BackUpInstalling:
				panelDisplaying.UpdateItems[1].Status = UpdateItemStatus.Success;
				break;
			case UpdateOn.ModifyReg:
				panelDisplaying.UpdateItems[2].Status = UpdateItemStatus.Success;
				break;
			}
			if (isAutoUpdateMode && (update.CurrentlyUpdating == UpdateOn.DownloadingUpdate || update.CurrentlyUpdating == UpdateOn.Extracting))
			{
				SaveAutoUpdateData((update.CurrentlyUpdating == UpdateOn.DownloadingUpdate) ? UpdateStepOn.UpdateDownloaded : UpdateStepOn.UpdateReadyToInstall);
				updateHelper.SendSuccess(autoUpdateStepProcessing);
				update.CurrentlyUpdating++;
				currentlyExtracting = false;
				return;
			}
			update.CurrentlyUpdating++;
			if (updtDetails == null && (update.CurrentlyUpdating == UpdateOn.PreExecute || update.CurrentlyUpdating == UpdateOn.OptimizeExecute || update.CurrentlyUpdating == UpdateOn.ModifyReg))
			{
				update.CurrentlyUpdating++;
				if (update.CurrentlyUpdating == UpdateOn.ModifyReg)
				{
					update.CurrentlyUpdating++;
				}
			}
			InstallUpdates(update.CurrentlyUpdating);
		}

		private void StartSelfElevated()
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.ErrorDialog = true;
			processStartInfo.ErrorDialogParentHandle = base.Handle;
			ProcessStartInfo processStartInfo2 = processStartInfo;
			if (SelfUpdateState == SelfUpdateState.WillUpdate)
			{
				processStartInfo2.FileName = Path.Combine(tempDirectory, Path.GetFileName(VersionTools.SelfLocation));
				File.Copy(VersionTools.SelfLocation, processStartInfo2.FileName, overwrite: true);
			}
			else if (SelfUpdateState == SelfUpdateState.FullUpdate)
			{
				processStartInfo2.FileName = oldSelfLocation;
			}
			else if (isAutoUpdateMode)
			{
				processStartInfo2.FileName = (IsNewSelf ? newSelfLocation : oldSelfLocation);
				if (string.IsNullOrEmpty(processStartInfo2.FileName))
				{
					processStartInfo2.FileName = VersionTools.SelfLocation;
				}
			}
			else
			{
				processStartInfo2.FileName = VersionTools.SelfLocation;
			}
			if (needElevation)
			{
				processStartInfo2.Verb = "runas";
				bool flag = false;
				try
				{
					if (VistaTools.AtLeastVista())
					{
						using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System"))
						{
							if (registryKey != null)
							{
								flag = ((int)registryKey.GetValue("EnableLUA", null) != 0);
							}
						}
					}
					else
					{
						flag = true;
					}
				}
				catch
				{
				}
				if (!flag)
				{
					error = clientLang.AdminError;
					ShowFrame(Frame.Error);
					return;
				}
			}
			try
			{
				string text = Path.Combine(tempDirectory, "selfUpdate.sup");
				SaveSelfUpdateData(text);
				processStartInfo2.Arguments = "-supdf:\"" + text + "\"";
				if (IsNewSelf)
				{
					processStartInfo2.Arguments += " /ns";
				}
				Process.Start(processStartInfo2);
				Close();
			}
			catch (Exception ex)
			{
				error = clientLang.AdminError;
				errorDetails = ex.Message;
				ShowFrame(Frame.Error);
			}
		}

		private bool NeedElevationToUpdate()
		{
			if (IsAdmin || OnlyUpdatingLocalUser())
			{
				return false;
			}
			if (VistaTools.AtLeastVista())
			{
				VistaTools.SetButtonShield(btnNext, showShield: true);
			}
			return true;
		}

		private static bool HaveFolderPermissions(string folder)
		{
			try
			{
				FileSystemSecurity accessControl = Directory.GetAccessControl(folder);
				AuthorizationRuleCollection accessRules = accessControl.GetAccessRules(includeExplicit: true, includeInherited: true, typeof(NTAccount));
				WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
				FileSystemRights fileSystemRights = (FileSystemRights)0;
				FileSystemRights fileSystemRights2 = (FileSystemRights)0;
				foreach (FileSystemAccessRule item in accessRules)
				{
					if (item.IdentityReference.Value.StartsWith("S-1-"))
					{
						SecurityIdentifier sid = new SecurityIdentifier(item.IdentityReference.Value);
						if (!windowsPrincipal.IsInRole(sid))
						{
							continue;
						}
					}
					else if (!windowsPrincipal.IsInRole(item.IdentityReference.Value))
					{
						continue;
					}
					if (item.AccessControlType == AccessControlType.Deny)
					{
						fileSystemRights2 |= item.FileSystemRights;
					}
					else
					{
						fileSystemRights |= item.FileSystemRights;
					}
				}
				fileSystemRights &= ~fileSystemRights2;
				return ((fileSystemRights ^ (FileSystemRights.ReadData | FileSystemRights.WriteData | FileSystemRights.AppendData | FileSystemRights.ReadExtendedAttributes | FileSystemRights.WriteExtendedAttributes | FileSystemRights.ExecuteFile | FileSystemRights.DeleteSubdirectoriesAndFiles | FileSystemRights.ReadAttributes | FileSystemRights.WriteAttributes | FileSystemRights.Delete | FileSystemRights.ReadPermissions)) & (FileSystemRights.ReadData | FileSystemRights.WriteData | FileSystemRights.AppendData | FileSystemRights.ReadExtendedAttributes | FileSystemRights.WriteExtendedAttributes | FileSystemRights.ExecuteFile | FileSystemRights.DeleteSubdirectoriesAndFiles | FileSystemRights.ReadAttributes | FileSystemRights.WriteAttributes | FileSystemRights.Delete | FileSystemRights.ReadPermissions)) == 0;
			}
			catch
			{
				return false;
			}
		}

		private bool OnlyUpdatingLocalUser()
		{
			if (((updateFrom.InstallingTo | InstallingTo.BaseDir) ^ InstallingTo.BaseDir) != 0)
			{
				return false;
			}
			string userProfile = SystemFolders.GetUserProfile();
			if ((updateFrom.InstallingTo & InstallingTo.BaseDir) != 0 && !SystemFolders.IsDirInDir(userProfile, baseDirectory) && !HaveFolderPermissions(baseDirectory))
			{
				return false;
			}
			if (!SystemFolders.IsFileInDirectory(userProfile, clientFileLoc) && !HaveFolderPermissions(Path.GetDirectoryName(clientFileLoc)))
			{
				return false;
			}
			if ((SelfUpdateState == SelfUpdateState.WillUpdate || SelfUpdateState == SelfUpdateState.FullUpdate || SelfUpdateState == SelfUpdateState.Extracted) && !SystemFolders.IsFileInDirectory(userProfile, VersionTools.SelfLocation) && !HaveFolderPermissions(Path.GetDirectoryName(VersionTools.SelfLocation)))
			{
				return false;
			}
			return true;
		}

		private static int Check_Loader()
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\WinLogon");
				if (registryKey != null)
				{
					object value = registryKey.GetValue("Shell");
					if (value != null)
					{
						string text = Convert.ToString(value);
						if (text.ToLower().Contains("loader.exe"))
						{
							return 1;
						}
					}
				}
			}
			catch
			{
			}
			return 0;
		}

		private static int Check_Freeze()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			string path = folderPath + "\\Toolwiz Time Freeze 2014\\ToolwizTimeFreeze.exe";
			File.Exists(path);
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Toolwiz\\TimefreezeNew");
				if (registryKey != null)
				{
					object value = registryKey.GetValue("CURRENT_PROTECT_MODE");
					if (value != null)
					{
						int num = Convert.ToInt32(value);
						if (num == 1)
						{
							return 1;
						}
					}
				}
			}
			catch
			{
			}
			return 0;
		}

		public static string Decrypt(string cryptedString)
		{
			if (string.IsNullOrEmpty(cryptedString))
			{
				throw new ArgumentNullException("Error Decrypt.");
			}
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream stream = new MemoryStream(Convert.FromBase64String(cryptedString));
			CryptoStream stream2 = new CryptoStream(stream, dESCryptoServiceProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
			StreamReader streamReader = new StreamReader(stream2);
			return streamReader.ReadToEnd();
		}

		public static string X2Y(string dades)
		{
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] buffer = uTF8Encoding.GetBytes(dades);
			SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider();
			byte[] array = sHA1CryptoServiceProvider.ComputeHash(buffer);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] < 16)
				{
					stringBuilder.Append("0");
				}
				stringBuilder.Append(array[i].ToString("x"));
			}
			return stringBuilder.ToString().ToUpper();
		}

		private string Get_XY()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\chrome.cache";
			if (File.Exists(path))
			{
				try
				{
					return File.ReadAllText(path);
				}
				catch
				{
				}
			}
			return Get_XY_Def();
		}

		private string Get_XY_Def()
		{
			return X2Y("batman");
		}

		private void Access_Log()
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
			text = text2 + "Loader access: " + DateTime.Now.ToLongDateString() + " / " + DateTime.Now.ToLongTimeString() + "\r\n";
			try
			{
				File.WriteAllText(path, text);
			}
			catch
			{
			}
		}

		public void Check_Admin(int _opt)
		{
			if ((Control.ModifierKeys & Keys.Control) != Keys.Control && _opt != 1)
			{
				return;
			}
			DLS_Login dLS_Login = new DLS_Login();
			dLS_Login.ShowDialog();
			if (dLS_Login != null)
			{
				string a = X2Y(dLS_Login.PassWord);
				if (a == Get_XY() || a == Get_XY_Def())
				{
					dLS_Login.Dispose();
					dLS_Login = null;
					DLG_Setup dLG_Setup = new DLG_Setup();
					dLG_Setup.ShowDialog();
				}
			}
		}

		public static string Get_WifiMAC()
		{
			string result = "-";
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			for (int i = 0; i < allNetworkInterfaces.Length; i++)
			{
				if (allNetworkInterfaces[i].OperationalStatus == OperationalStatus.Up && allNetworkInterfaces[i].NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
				{
					result = allNetworkInterfaces[i].Description + " MAC " + allNetworkInterfaces[i].GetPhysicalAddress();
					break;
				}
			}
			return result;
		}

		public frmMain(string[] args, bool _onlyinstall)
		{
			OnlyInstall = _onlyinstall;
			Font = SystemFonts.MessageBoxFont;
			IsAdmin = VistaTools.IsUserAnAdmin();
			Splash splash = new Splash();
			splash.Show();
			int num = 90;
			if (Get_WifiMAC() != "-")
			{
				splash.wifi = "WIFI";
				num = 120;
			}
			ModoStartup = 1;
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (splash.GoConfig == 1)
				{
					Check_Admin(splash.GoConfig);
					splash.GoConfig = 0;
				}
				else
				{
					Check_Admin(0);
				}
				if (i > 5 && PingTest())
				{
					num2++;
					if (num2 > 1)
					{
						break;
					}
					i--;
				}
				for (int j = 0; j < 100; j++)
				{
					Application.DoEvents();
					if (splash.GoConfig == 1)
					{
						Check_Admin(splash.GoConfig);
						splash.GoConfig = 0;
					}
					else
					{
						Check_Admin(0);
					}
					Thread.Sleep(10);
				}
				if (Get_WifiMAC() != "-")
				{
					splash.wifi = "WIFI";
				}
			}
			ModoStartup = 0;
			splash.Hide();
			splash.Dispose();
			splash = null;
			if (!PingTest())
			{
				MSGBOX_Timer mSGBOX_Timer = new MSGBOX_Timer("No INTERNET Connection. Check Network cable, network parameters or WIFI", "Ok", 5);
				mSGBOX_Timer.ShowDialog();
			}
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.WindowState = FormWindowState.Normal;
			if (Check_Loader() == 1 && !OnlyInstall)
			{
				base.WindowState = FormWindowState.Maximized;
				base.FormBorderStyle = FormBorderStyle.None;
			}
			if (!OnlyInstall)
			{
				base.WindowState = FormWindowState.Maximized;
				base.FormBorderStyle = FormBorderStyle.None;
			}
			InitializeComponent();
			if (base.ClientRectangle.Width < 800)
			{
				base.Width = base.Width - base.ClientRectangle.Width + 800;
			}
			if (base.ClientRectangle.Height < 600)
			{
				base.Height = base.Height - base.ClientRectangle.Height + 600;
			}
			panelDisplaying = new PanelDisplay(base.Width, base.Height);
			FileDownloader.EnableLazySSL();
			panelDisplaying.TabIndex = 0;
			base.Controls.Add(panelDisplaying);
			try
			{
				Arguments commands = new Arguments(args);
				ProcessArguments(commands);
				if (!string.IsNullOrEmpty(selfUpdateFileLoc))
				{
					LoadSelfUpdateData(selfUpdateFileLoc);
					ConfigureProxySettings();
					if (selfUpdateFromRC1)
					{
						if (needElevation && NeedElevationToUpdate())
						{
							error = clientLang.AdminError;
							selfUpdateFromRC1 = false;
							ShowFrame(Frame.Error);
						}
						else
						{
							needElevation = false;
							FileAttributes attributes = File.GetAttributes(oldSelfLocation);
							bool flag = (attributes & FileAttributes.Hidden) != 0 || (attributes & FileAttributes.ReadOnly) != 0 || (attributes & FileAttributes.System) != 0;
							if (flag)
							{
								File.SetAttributes(oldSelfLocation, FileAttributes.Normal);
							}
							File.Copy(newSelfLocation, oldSelfLocation, overwrite: true);
							if (flag)
							{
								File.SetAttributes(oldSelfLocation, attributes);
							}
						}
						return;
					}
				}
				else
				{
					ConfigureProxySettings();
				}
				if (clientFileType == ClientFileType.PreRC2)
				{
					update.OpenObsoleteClientFile(clientFileLoc);
				}
				else
				{
					update.OpenClientFile(clientFileLoc, clientLang, forcedLanguageCulture, updatePathVar, customUrlArgs);
				}
				clientLang.SetVariables(update.ProductName, update.InstalledVersion);
			}
			catch (Exception ex)
			{
				clientLang.SetVariables(update.ProductName, update.InstalledVersion);
				error = "Client file failed to load. The client.wyc file might be corrupt.";
				errorDetails = ex.Message;
				ShowFrame(Frame.Error);
				return;
			}
			SetButtonText();
			panelDisplaying.HeaderImageAlign = update.HeaderImageAlign;
			if (update.HeaderTextIndent >= 0)
			{
				panelDisplaying.HeaderIndent = update.HeaderTextIndent;
			}
			panelDisplaying.HideHeaderDivider = update.HideHeaderDivider;
			if (update.CustomWyUpdateTitle != null)
			{
				Text = update.CustomWyUpdateTitle;
			}
			try
			{
				if (!string.IsNullOrEmpty(update.HeaderTextColorName))
				{
					panelDisplaying.HeaderTextColor = Color.FromName(update.HeaderTextColorName);
				}
			}
			catch
			{
			}
			panelDisplaying.TopImage = Resources.kiosk2;
			panelDisplaying.SideImage = Resources.kiosk2;
			if (isAutoUpdateMode)
			{
				try
				{
					if (tempDirectory == null)
					{
						tempDirectory = CreateAutoUpdateTempFolder();
					}
				}
				catch (Exception ex2)
				{
					error = clientLang.GeneralUpdateError;
					errorDetails = "Failed to create the automatic updater temp folder: " + ex2.Message;
					ShowFrame(Frame.Error);
					return;
				}
				try
				{
					LoadAutoUpdateData();
					ConfigureProxySettings();
				}
				catch
				{
					startStep = UpdateStepOn.Checking;
				}
			}
			else if (SelfUpdateState == SelfUpdateState.FullUpdate)
			{
				try
				{
					ServerFile = ServerFile.Load(serverFileLoc, updatePathVar, customUrlArgs);
					LoadClientServerFile();
					clientLang.NewVersion = SelfServerFile.NewVersion;
				}
				catch (Exception ex3)
				{
					error = clientLang.ServerError;
					errorDetails = ex3.Message;
					ShowFrame(Frame.Error);
					return;
				}
				if (needElevation && NeedElevationToUpdate())
				{
					error = clientLang.AdminError;
					ShowFrame(Frame.Error);
				}
				else
				{
					needElevation = false;
					ShowFrame(Frame.InstallUpdates);
				}
			}
			else if (SelfUpdateState == SelfUpdateState.ContinuingRegularUpdate)
			{
				try
				{
					LoadServerFile(setChangesText: false);
				}
				catch (Exception ex4)
				{
					error = clientLang.ServerError;
					errorDetails = ex4.Message;
					ShowFrame(Frame.Error);
					return;
				}
				if (needElevation && NeedElevationToUpdate())
				{
					error = clientLang.AdminError;
					ShowFrame(Frame.Error);
				}
				else
				{
					needElevation = false;
					ShowFrame(Frame.InstallUpdates);
				}
			}
			else if (!uninstalling)
			{
				startStep = UpdateStepOn.Checking;
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

		protected override void SetVisibleCore(bool value)
		{
			base.SetVisibleCore(value);
			if (_isApplicationRun)
			{
				_isApplicationRun = false;
				if (isAutoUpdateMode)
				{
					SetupAutoupdateMode();
				}
				if (uninstalling)
				{
					ShowFrame(Frame.Uninstall);
				}
				else if (selfUpdateFromRC1)
				{
					StartSelfElevated();
				}
				else if (startStep != 0)
				{
					try
					{
						PrepareStepOn(startStep);
						if (beginAutoUpdateInstallation && !IsNewSelf)
						{
							UpdateHelper_RequestReceived(this, UpdateAction.UpdateStep, UpdateStep.Install);
						}
					}
					catch (Exception ex2)
					{
						if (startStep != UpdateStepOn.Checking)
						{
							startStep = UpdateStepOn.Checking;
							try
							{
								PrepareStepOn(startStep);
							}
							catch (Exception ex)
							{
								error = "Automatic update state failed to load.";
								errorDetails = ex.Message;
								ShowFrame(Frame.Error);
							}
						}
						else
						{
							error = "Automatic update state failed to load.";
							errorDetails = ex2.Message;
							ShowFrame(Frame.Error);
						}
					}
				}
			}
		}

		private void ProcessArguments(Arguments commands)
		{
			if (commands["supdf"] != null)
			{
				selfUpdateFileLoc = commands["supdf"];
				if (commands["ns"] != null)
				{
					IsNewSelf = true;
				}
				return;
			}
			PasswordUpdateCmd = commands["password"];
			forcedLanguageCulture = commands["forcelang"];
			if (commands["autoupdate"] != null)
			{
				isAutoUpdateMode = true;
				if (commands["ns"] != null)
				{
					IsNewSelf = true;
				}
			}
			else if (commands["uninstall"] != null)
			{
				uninstalling = true;
			}
			else
			{
				if (commands["quickcheck"] != null)
				{
					if (!OnlyInstall)
					{
						base.WindowState = FormWindowState.Minimized;
						base.ShowInTaskbar = false;
					}
					QuickCheck = true;
					if (commands["noerr"] != null)
					{
						QuickCheckNoErr = true;
					}
					if (commands["justcheck"] != null)
					{
						QuickCheckJustCheck = true;
					}
					if (QuickCheckNoErr || QuickCheckJustCheck)
					{
						OutputInfo = commands["outputinfo"];
					}
				}
				else if (commands["fromservice"] != null)
				{
					SkipUpdateInfo = true;
					UpdatingFromService = true;
					if (!string.IsNullOrEmpty(commands["logfile"]))
					{
						log = new Logger(commands["logfile"]);
					}
				}
				if (commands["skipinfo"] != null)
				{
					SkipUpdateInfo = true;
				}
				StartOnErr = commands["startonerr"];
				StartOnErrArgs = commands["startonerra"];
			}
			if (commands["cdata"] != null)
			{
				clientFileLoc = commands["cdata"];
				if (clientFileLoc.EndsWith("iuc", StringComparison.OrdinalIgnoreCase))
				{
					clientFileType = ClientFileType.PreRC2;
				}
				else if (clientFileLoc.EndsWith("iucz", StringComparison.OrdinalIgnoreCase))
				{
					clientFileType = ClientFileType.RC2;
				}
				else
				{
					clientFileType = ClientFileType.Final;
				}
			}
			else
			{
				clientFileLoc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "client.wyc");
				clientFileType = ClientFileType.Final;
				if (!File.Exists(clientFileLoc))
				{
					clientFileLoc = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "iuclient.iucz");
					clientFileType = ClientFileType.RC2;
				}
				if (!File.Exists(clientFileLoc))
				{
					clientFileLoc = clientFileLoc.Substring(0, clientFileLoc.Length - 1);
					clientFileType = ClientFileType.PreRC2;
				}
			}
			baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
			if (commands["basedir"] != null && Directory.Exists(commands["basedir"]))
			{
				baseDirectory = commands["basedir"].TrimEnd();
			}
			if (!isAutoUpdateMode)
			{
				tempDirectory = Path.Combine(Path.GetTempPath(), "w" + DateTime.Now.ToString("sff"));
				Directory.CreateDirectory(tempDirectory);
			}
			serverOverwrite = commands["server"];
			updatePathVar = commands["updatepath"];
			customUrlArgs = commands["urlargs"];
			customProxyUrl = commands["proxy"];
			customProxyUser = commands["proxyu"];
			customProxyPassword = commands["proxyp"];
			customProxyDomain = commands["proxyd"];
			if (uninstalling && commands["s"] != null)
			{
				isSilent = true;
				if (!OnlyInstall)
				{
					base.WindowState = FormWindowState.Minimized;
					base.ShowInTaskbar = false;
				}
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (needElevation || SelfUpdateState == SelfUpdateState.WillUpdate || SelfUpdateState == SelfUpdateState.FullUpdate || isSilent || isAutoUpdateMode || isCancelled || panelDisplaying.TypeofFrame == FrameType.WelcomeFinish || panelDisplaying.TypeofFrame == FrameType.TextInfo)
			{
				e.Cancel = false;
			}
			else
			{
				e.Cancel = true;
				CancelUpdate();
			}
			base.OnClosing(e);
		}

		protected override void OnClosed(EventArgs e)
		{
			if (!needElevation && SelfUpdateState != SelfUpdateState.WillUpdate && SelfUpdateState != SelfUpdateState.FullUpdate && !isAutoUpdateMode)
			{
				RemoveTempDirectory();
			}
			if (isCancelled)
			{
				ReturnCode = 3;
			}
			base.OnClosed(e);
		}

		private void RemoveTempDirectory()
		{
			if (Directory.Exists(tempDirectory))
			{
				try
				{
					Directory.Delete(tempDirectory, recursive: true);
				}
				catch
				{
				}
			}
		}

		private void ConfigureProxySettings()
		{
			if (!string.IsNullOrEmpty(customProxyUrl))
			{
				FileDownloader.CustomProxy = new WebProxy(customProxyUrl);
				if (!string.IsNullOrEmpty(customProxyUser) && !string.IsNullOrEmpty(customProxyPassword))
				{
					FileDownloader.CustomProxy.Credentials = new NetworkCredential(customProxyUser, customProxyPassword, customProxyDomain ?? string.Empty);
				}
				else
				{
					FileDownloader.CustomProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
				}
			}
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			panelDisplaying.Top = (base.Height - 150) / 2 - 225;
			panelDisplaying.Left = base.Width / 2 - 400;
		}

		private void tWatchDog_Tick(object sender, EventArgs e)
		{
			tWatchDog.Enabled = false;
			string text = "Kiosk";
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.ProcessName.ToLower().Contains(text.ToLower()))
				{
					tWatchDog.Enabled = true;
					return;
				}
			}
			btnNext_Click(null, EventArgs.Empty);
		}

		private void frmMain_KeyDown(object sender, KeyEventArgs e)
		{
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(wyUpdate.frmMain));
			btnNext = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			tWatchDog = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			btnNext.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			btnNext.AutoSize = true;
			btnNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btnNext.Location = new System.Drawing.Point(692, 524);
			btnNext.Name = "btnNext";
			btnNext.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
			btnNext.Size = new System.Drawing.Size(96, 64);
			btnNext.TabIndex = 1;
			btnNext.Visible = false;
			btnNext.Click += new System.EventHandler(btnNext_Click);
			btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			btnCancel.AutoSize = true;
			btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			btnCancel.Location = new System.Drawing.Point(590, 524);
			btnCancel.Name = "btnCancel";
			btnCancel.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
			btnCancel.Size = new System.Drawing.Size(96, 64);
			btnCancel.TabIndex = 0;
			btnCancel.Visible = false;
			btnCancel.SizeChanged += new System.EventHandler(btnCancel_SizeChanged);
			btnCancel.Click += new System.EventHandler(btnCancel_Click);
			tWatchDog.Interval = 30000;
			tWatchDog.Tick += new System.EventHandler(tWatchDog_Tick);
			base.AcceptButton = btnNext;
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			BackColor = System.Drawing.Color.Black;
			base.CancelButton = btnCancel;
			base.ClientSize = new System.Drawing.Size(800, 600);
			base.Controls.Add(btnCancel);
			base.Controls.Add(btnNext);
			DoubleBuffered = true;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.Name = "frmMain";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Loading ...";
			base.Resize += new System.EventHandler(frmMain_Resize);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
