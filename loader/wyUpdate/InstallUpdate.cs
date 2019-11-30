using Ionic.Zip;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using wyUpdate.Common;
using wyUpdate.Compression.Vcdiff;

namespace wyUpdate
{
	internal class InstallUpdate
	{
		public const int TotalUpdateSteps = 7;

		public string ExtractPassword;

		private static string[] frameworkV2_0Dirs;

		private static string[] frameworkV4_0Dirs;

		public string SkipStartService;

		public string NewSelfLoc;

		public string OldSelfLoc;

		private readonly BackgroundWorker bw = new BackgroundWorker();

		public string Filename;

		public string OutputDirectory;

		public string TempDirectory;

		public string ProgramDirectory;

		public bool IsAdmin;

		public IntPtr MainWindowHandle;

		public UpdateDetails UpdtDetails;

		public ClientFileType ClientFileType;

		public ClientFile ClientFile;

		public bool SkipProgressReporting;

		public bool SkipUIReporting;

		private volatile bool paused;

		public event ProgressChangedHandler ProgressChanged;

		public event ChangeRollbackDelegate Rollback;

		public void RunUpdateClientDataFile()
		{
			bw.DoWork += bw_DoWorkClientData;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedClientData;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkClientData(object sender, DoWorkEventArgs e)
		{
			Exception ex = null;
			try
			{
				OutputDirectory = Path.Combine(TempDirectory, "ClientData");
				Directory.CreateDirectory(OutputDirectory);
				string text = null;
				if (ClientFileType != ClientFileType.Final && File.Exists(Path.Combine(Path.GetDirectoryName(Filename), "client.wyc")))
				{
					text = Filename;
					Filename = Path.Combine(Path.GetDirectoryName(Filename), "client.wyc");
					ClientFileType = ClientFileType.Final;
				}
				if (ClientFileType == ClientFileType.PreRC2)
				{
					if (ClientFile.TopImage != null)
					{
						ClientFile.TopImageFilename = "t.png";
						string filename = Path.Combine(OutputDirectory, "t.png");
						ClientFile.TopImage.Save(filename, ImageFormat.Png);
					}
					if (ClientFile.SideImage != null)
					{
						ClientFile.SideImageFilename = "s.png";
						string filename = Path.Combine(OutputDirectory, "s.png");
						ClientFile.SideImage.Save(filename, ImageFormat.Png);
					}
				}
				else
				{
					ExtractUpdateFile();
					if (File.Exists(Path.Combine(OutputDirectory, "iuclient.iuc")))
					{
						ClientFile clientFile = new ClientFile();
						clientFile.LoadClientData(Path.Combine(OutputDirectory, "iuclient.iuc"));
						clientFile.InstalledVersion = ClientFile.InstalledVersion;
						ClientFile = clientFile;
						File.Delete(Path.Combine(OutputDirectory, "iuclient.iuc"));
					}
				}
				List<UpdateFile> updateFiles = UpdtDetails.UpdateFiles;
				FixUpdateFilesPaths(updateFiles);
				RollbackUpdate.WriteUninstallFile(TempDirectory, Path.Combine(OutputDirectory, "uninstall.dat"), updateFiles);
				List<UpdateFile> files = new List<UpdateFile>();
				AddFiles(OutputDirectory.Length + 1, OutputDirectory, files);
				string text2 = Path.Combine(TempDirectory, "client.file");
				ClientFile.SaveClientFile(files, text2);
				FileAttributes fileAttributes = FileAttributes.Normal;
				if (File.Exists(Filename))
				{
					fileAttributes = File.GetAttributes(Filename);
				}
				bool flag = (fileAttributes & FileAttributes.Hidden) != 0 || (fileAttributes & FileAttributes.ReadOnly) != 0 || (fileAttributes & FileAttributes.System) != 0;
				if (flag)
				{
					File.SetAttributes(Filename, FileAttributes.Normal);
				}
				File.Copy(text2, Filename, overwrite: true);
				if (flag)
				{
					File.SetAttributes(Filename, fileAttributes);
				}
				if (text != null)
				{
					File.Delete(text);
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (ex != null)
			{
				bw.ReportProgress(1, true);
				RollbackUpdate.RollbackStartedServices(TempDirectory);
				RollbackUpdate.RollbackRegedCOM(TempDirectory);
				RollbackUpdate.RollbackRegistry(TempDirectory);
				bw.ReportProgress(1, false);
				RollbackUpdate.RollbackFiles(TempDirectory, ProgramDirectory);
				RollbackUpdate.RollbackUnregedCOM(TempDirectory);
				RollbackUpdate.RollbackStoppedServices(TempDirectory);
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_RunWorkerCompletedClientData(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkClientData;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedClientData;
		}

		private static void AddFiles(int charsToTrim, string dir, List<UpdateFile> files)
		{
			string[] files2 = Directory.GetFiles(dir);
			string[] directories = Directory.GetDirectories(dir);
			string[] array = files2;
			foreach (string text in array)
			{
				files.Add(new UpdateFile
				{
					Filename = text,
					RelativePath = text.Substring(charsToTrim)
				});
			}
			string[] array2 = directories;
			foreach (string dir2 in array2)
			{
				AddFiles(charsToTrim, dir2, files);
			}
		}

		public void RunUnzipProcess()
		{
			bw.DoWork += bw_DoWorkUnzip;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedUnzip;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkUnzip(object sender, DoWorkEventArgs e)
		{
			Exception ex = null;
			string text = Path.Combine(TempDirectory, "updtdetails.udt");
			try
			{
				ExtractUpdateFile();
				try
				{
					File.Delete(Filename);
				}
				catch
				{
				}
				if (!File.Exists(text))
				{
					throw new Exception("The update details file \"updtdetails.udt\" is missing.");
				}
				UpdtDetails = UpdateDetails.Load(text);
				if (Directory.Exists(Path.Combine(TempDirectory, "patches")))
				{
					foreach (UpdateFile updateFile in UpdtDetails.UpdateFiles)
					{
						if (updateFile.DeltaPatchRelativePath != null)
						{
							if (IsCancelled())
							{
								break;
							}
							string path = Path.Combine(TempDirectory, updateFile.RelativePath);
							if (!Directory.Exists(Path.GetDirectoryName(path)))
							{
								Directory.CreateDirectory(Path.GetDirectoryName(path));
							}
							while (true)
							{
								try
								{
									using (FileStream original = File.Open(FixUpdateDetailsPaths(updateFile.RelativePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
									{
										using (FileStream delta = File.Open(Path.Combine(TempDirectory, updateFile.DeltaPatchRelativePath), FileMode.Open, FileAccess.Read, FileShare.Read))
										{
											using (FileStream output = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
											{
												VcdiffDecoder.Decode(original, delta, output, updateFile.NewFileAdler32);
											}
										}
									}
								}
								catch (IOException ex2)
								{
									int hRForException = Marshal.GetHRForException(ex2);
									if ((hRForException & 0xFFFF) != 32)
									{
										throw new PatchApplicationException("Patch failed to apply to this file: " + FixUpdateDetailsPaths(updateFile.RelativePath) + "\r\n\r\nBecause that file failed to patch, and there's no \"catch-all\" update to download, the update failed to apply. The failure to patch usually happens because the file was modified from the original version. Reinstall the original version of this app.\r\n\r\n\r\nInternal error: " + ex2.Message);
									}
									bw.ReportProgress(0, new object[5]
									{
										-1,
										-1,
										string.Empty,
										ProgressStatus.SharingViolation,
										FixUpdateDetailsPaths(updateFile.RelativePath)
									});
									Thread.Sleep(1000);
									if (!IsCancelled())
									{
										continue;
									}
									goto IL_022b;
								}
								catch (Exception ex3)
								{
									throw new PatchApplicationException("Patch failed to apply to this file: " + FixUpdateDetailsPaths(updateFile.RelativePath) + "\r\n\r\nBecause that file failed to patch, and there's no \"catch-all\" update to download, the update failed to apply. The failure to patch usually happens because the file was modified from the original version. Reinstall the original version of this app.\r\n\r\n\r\nInternal error: " + ex3.Message);
								}
								File.SetLastWriteTime(path, File.GetLastWriteTime(Path.Combine(TempDirectory, updateFile.DeltaPatchRelativePath)));
								break;
							}
						}
						IL_022b:;
					}
					try
					{
						Directory.Delete(Path.Combine(TempDirectory, "patches"), recursive: true);
					}
					catch
					{
					}
				}
			}
			catch (BadPasswordException)
			{
				ex = new BadPasswordException("Could not install the encrypted update because the password did not match.");
			}
			catch (Exception ex5)
			{
				ex = ex5;
			}
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					"Cancelling update...",
					ProgressStatus.None,
					null
				});
				if (ex != null && (object)ex.GetType() != typeof(PatchApplicationException))
				{
					try
					{
						Directory.Delete(OutputDirectory, recursive: true);
					}
					catch
					{
					}
				}
				else
				{
					string[] directories = Directory.GetDirectories(TempDirectory);
					string[] array = directories;
					foreach (string path2 in array)
					{
						if (!(Path.GetFileName(path2) == "selfupdate"))
						{
							try
							{
								Directory.Delete(path2, recursive: true);
							}
							catch
							{
							}
						}
					}
					if (File.Exists(text))
					{
						File.Delete(text);
					}
				}
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_RunWorkerCompletedUnzip(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkUnzip;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedUnzip;
		}

		private void ExtractUpdateFile()
		{
			using (ZipFile zipFile = ZipFile.Read(Filename))
			{
				int count = zipFile.Entries.Count;
				int num = 0;
				foreach (ZipEntry item in zipFile)
				{
					if (IsCancelled())
					{
						break;
					}
					if (!SkipProgressReporting)
					{
						int num2 = (count > 0) ? (num * 100 / count) : 0;
						bw.ReportProgress(0, new object[5]
						{
							GetRelativeProgess(1, num2),
							num2,
							"Extracting " + Path.GetFileName(item.FileName),
							ProgressStatus.None,
							null
						});
						num++;
					}
					if (!string.IsNullOrEmpty(ExtractPassword))
					{
						item.ExtractWithPassword(OutputDirectory, ExtractExistingFileAction.OverwriteSilently, ExtractPassword);
					}
					else
					{
						item.Extract(OutputDirectory, ExtractExistingFileAction.OverwriteSilently);
					}
				}
			}
		}

		public void RunOptimizeExecute()
		{
			bw.DoWork += bw_DoWorkOptimizeExecute;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedOptimizeExecute;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkOptimizeExecute(object sender, DoWorkEventArgs e)
		{
			bw.ReportProgress(0, new object[5]
			{
				GetRelativeProgess(6, 0),
				0,
				string.Empty,
				ProgressStatus.None,
				null
			});
			List<UninstallFileInfo> list = new List<UninstallFileInfo>();
			List<string> list2 = new List<string>();
			Exception ex = null;
			for (int i = 0; i < UpdtDetails.UpdateFiles.Count; i++)
			{
				if (!UpdtDetails.UpdateFiles[i].IsNETAssembly && (UpdtDetails.UpdateFiles[i].RegisterCOMDll & COMRegistration.Register) != COMRegistration.Register)
				{
					continue;
				}
				if (IsCancelled())
				{
					break;
				}
				if (UpdtDetails.UpdateFiles[i].RelativePath.Length < 4 || !(UpdtDetails.UpdateFiles[i].RelativePath.Substring(0, 4) != "temp"))
				{
					continue;
				}
				string text = FixUpdateDetailsPaths(UpdtDetails.UpdateFiles[i].RelativePath);
				if (UpdtDetails.UpdateFiles[i].IsNETAssembly)
				{
					if (!string.IsNullOrEmpty(text))
					{
						NGenInstall(text, UpdtDetails.UpdateFiles[i]);
					}
				}
				else
				{
					try
					{
						RegisterDllServer(text, Uninstall: false);
						list.Add(new UninstallFileInfo
						{
							Path = text,
							RegisterCOMDll = COMRegistration.UnRegister
						});
					}
					catch (Exception ex2)
					{
						ex = ex2;
						break;
					}
				}
			}
			RollbackUpdate.WriteRollbackCOM(Path.Combine(TempDirectory, "backup\\reggedComList.bak"), list);
			bw.ReportProgress(0, new object[5]
			{
				GetRelativeProgess(6, 50),
				50,
				string.Empty,
				ProgressStatus.None,
				null
			});
			if (!IsCancelled() && ex == null)
			{
				for (int j = 0; j < UpdtDetails.UpdateFiles.Count; j++)
				{
					if (UpdtDetails.UpdateFiles[j].Execute && !UpdtDetails.UpdateFiles[j].ExBeforeUpdate)
					{
						string text2 = FixUpdateDetailsPaths(UpdtDetails.UpdateFiles[j].RelativePath);
						if (!string.IsNullOrEmpty(text2))
						{
							try
							{
								if (UpdtDetails.UpdateFiles[j].ElevationType == ElevationType.NotElevated && IsAdmin && VistaTools.AtLeastVista())
								{
									int num = (int)LimitedProcess.Start(text2, string.IsNullOrEmpty(UpdtDetails.UpdateFiles[j].CommandLineArgs) ? null : ParseText(UpdtDetails.UpdateFiles[j].CommandLineArgs), fallback: false, UpdtDetails.UpdateFiles[j].WaitForExecution, UpdtDetails.UpdateFiles[j].ProcessWindowStyle);
									if (UpdtDetails.UpdateFiles[j].RollbackOnNonZeroRet && num != 0 && (UpdtDetails.UpdateFiles[j].RetExceptions == null || !UpdtDetails.UpdateFiles[j].RetExceptions.Contains(num)))
									{
										ex = new Exception("\"" + text2 + "\" returned " + num + ".");
										goto IL_058e;
									}
								}
								else
								{
									ProcessStartInfo processStartInfo = new ProcessStartInfo();
									processStartInfo.FileName = text2;
									processStartInfo.WindowStyle = UpdtDetails.UpdateFiles[j].ProcessWindowStyle;
									ProcessStartInfo processStartInfo2 = processStartInfo;
									if (!string.IsNullOrEmpty(UpdtDetails.UpdateFiles[j].CommandLineArgs))
									{
										processStartInfo2.Arguments = ParseText(UpdtDetails.UpdateFiles[j].CommandLineArgs);
									}
									if (!IsAdmin && UpdtDetails.UpdateFiles[j].ElevationType == ElevationType.Elevated)
									{
										processStartInfo2.Verb = "runas";
										processStartInfo2.ErrorDialog = true;
										processStartInfo2.ErrorDialogParentHandle = MainWindowHandle;
									}
									Process process = Process.Start(processStartInfo2);
									if (UpdtDetails.UpdateFiles[j].WaitForExecution && process != null)
									{
										process.WaitForExit();
										if (UpdtDetails.UpdateFiles[j].RollbackOnNonZeroRet && process.ExitCode != 0 && (UpdtDetails.UpdateFiles[j].RetExceptions == null || !UpdtDetails.UpdateFiles[j].RetExceptions.Contains(process.ExitCode)))
										{
											ex = new Exception("\"" + processStartInfo2.FileName + "\" returned " + process.ExitCode + ".");
											goto IL_058e;
										}
									}
								}
							}
							catch (Exception ex3)
							{
								ex = new Exception("Failed to execute the file \"" + text2 + "\": " + ex3.Message, ex3);
								goto IL_058e;
							}
						}
					}
				}
			}
			goto IL_058e;
			IL_058e:
			if (!IsCancelled() && ex == null)
			{
				try
				{
					foreach (StartService item in UpdtDetails.ServicesToStart)
					{
						if (SkipStartService == null || string.Compare(SkipStartService, item.Name, StringComparison.OrdinalIgnoreCase) != 0)
						{
							using (ServiceController serviceController = new ServiceController(item.Name))
							{
								ServiceControllerStatus status = serviceController.Status;
								if (status != ServiceControllerStatus.Running)
								{
									if (item.Arguments != null)
									{
										for (int k = 0; k < item.Arguments.Length; k++)
										{
											item.Arguments[k] = ParseText(item.Arguments[k]);
										}
										serviceController.Start(item.Arguments);
									}
									else
									{
										serviceController.Start();
									}
									bw.ReportProgress(0, new object[5]
									{
										GetRelativeProgess(6, 50),
										50,
										"Waiting for service to start: " + serviceController.DisplayName,
										ProgressStatus.None,
										null
									});
									serviceController.WaitForStatus(ServiceControllerStatus.Running);
									list2.Add(item.Name);
								}
							}
						}
					}
				}
				catch (Exception ex4)
				{
					ex = ex4;
				}
				RollbackUpdate.WriteRollbackServices(Path.Combine(TempDirectory, "backup\\startedServices.bak"), list2);
			}
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(1, true);
				RollbackUpdate.RollbackStartedServices(TempDirectory);
				RollbackUpdate.RollbackRegedCOM(TempDirectory);
				RollbackUpdate.RollbackRegistry(TempDirectory);
				bw.ReportProgress(1, false);
				RollbackUpdate.RollbackFiles(TempDirectory, ProgramDirectory);
				RollbackUpdate.RollbackUnregedCOM(TempDirectory);
				RollbackUpdate.RollbackStoppedServices(TempDirectory);
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_RunWorkerCompletedOptimizeExecute(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkOptimizeExecute;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedOptimizeExecute;
		}

		private static void GetFrameworkV2_0Directories()
		{
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework"))
			{
				if (registryKey != null)
				{
					string text = (string)registryKey.GetValue("InstallRoot", null);
					if (text != null)
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(text);
						directoryInfo = directoryInfo.Parent;
						if (directoryInfo != null)
						{
							List<string> list = new List<string>(2);
							string text2 = Path.Combine(directoryInfo.FullName, "Framework\\v2.0.50727");
							if (Directory.Exists(text2))
							{
								list.Add(text2);
							}
							text2 = Path.Combine(directoryInfo.FullName, "Framework64\\v2.0.50727");
							if (Directory.Exists(text2))
							{
								list.Add(text2);
							}
							if (list.Count != 0)
							{
								frameworkV2_0Dirs = list.ToArray();
							}
						}
					}
				}
			}
		}

		private static void GetFrameworkV4_0Directories()
		{
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NETFramework"))
			{
				if (registryKey != null)
				{
					string text = (string)registryKey.GetValue("InstallRoot", null);
					if (text != null)
					{
						DirectoryInfo directoryInfo = new DirectoryInfo(text);
						directoryInfo = directoryInfo.Parent;
						if (directoryInfo != null)
						{
							List<string> list = new List<string>(2);
							string text2 = Path.Combine(directoryInfo.FullName, "Framework\\v4.0.30319");
							if (Directory.Exists(text2))
							{
								list.Add(text2);
							}
							text2 = Path.Combine(directoryInfo.FullName, "Framework64\\v4.0.30319");
							if (Directory.Exists(text2))
							{
								list.Add(text2);
							}
							if (list.Count != 0)
							{
								frameworkV4_0Dirs = list.ToArray();
							}
						}
					}
				}
			}
		}

		private static void NGenInstall(string filename, UpdateFile updateFile)
		{
			string[] array;
			switch (updateFile.FrameworkVersion)
			{
			default:
				return;
			case FrameworkVersion.Net2_0:
				if (frameworkV2_0Dirs == null)
				{
					GetFrameworkV2_0Directories();
				}
				array = frameworkV2_0Dirs;
				break;
			case FrameworkVersion.Net4_0:
				if (frameworkV4_0Dirs == null)
				{
					GetFrameworkV4_0Directories();
				}
				array = frameworkV4_0Dirs;
				break;
			}
			if (array != null)
			{
				Process process = new Process();
				process.StartInfo.FileName = Path.Combine(array[(updateFile.CPUVersion != CPUVersion.x86) ? (array.Length - 1) : 0], "ngen.exe");
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				process.StartInfo.Arguments = " install \"" + filename + "\" /nologo";
				Process process2 = process;
				process2.Start();
				process2.WaitForExit();
			}
		}

		private static void NGenUninstall(string filename, UninstallFileInfo uninstallFile)
		{
			string[] array;
			switch (uninstallFile.FrameworkVersion)
			{
			default:
				return;
			case FrameworkVersion.Net2_0:
				if (frameworkV2_0Dirs == null)
				{
					GetFrameworkV2_0Directories();
				}
				array = frameworkV2_0Dirs;
				break;
			case FrameworkVersion.Net4_0:
				if (frameworkV4_0Dirs == null)
				{
					GetFrameworkV4_0Directories();
				}
				array = frameworkV4_0Dirs;
				break;
			}
			if (array != null)
			{
				Process process = new Process();
				process.StartInfo.FileName = Path.Combine(array[(uninstallFile.CPUVersion != CPUVersion.x86) ? (array.Length - 1) : 0], "ngen.exe");
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				process.StartInfo.Arguments = " uninstall \"" + filename + "\" /nologo";
				Process process2 = process;
				process2.Start();
				process2.WaitForExit();
			}
		}

		public static void RegisterDllServer(string DllPath, bool Uninstall)
		{
			Process process = new Process();
			process.StartInfo.FileName = "regsvr32.exe";
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.Arguments = (Uninstall ? "/s /u" : "/s") + " \"" + DllPath + "\"";
			using (Process process2 = process)
			{
				process2.Start();
				process2.WaitForExit();
				switch (process2.ExitCode)
				{
				case 0:
					break;
				case 1:
					throw new Exception("RegSvr32 failed - bad arguments. File: " + DllPath);
				case 2:
					throw new Exception("RegSvr32 failed - OLE initilization failed for " + DllPath);
				case 3:
					throw new Exception("RegSvr32 failed - Failed to load the module, you may need to check for problems with dependencies. File: " + DllPath);
				case 4:
					throw new Exception("RegSvr32 failed - Can't find " + (Uninstall ? "DllUnregisterServer" : "DllRegisterServer") + " entry point in the file, maybe it's not a .DLL or .OCX? File: " + DllPath);
				case 5:
					throw new Exception("RegSvr32 failed - The assembly was loaded, but the call to " + (Uninstall ? "DllUnregisterServer" : "DllRegisterServer") + " failed. File: " + DllPath);
				default:
					throw new Exception("Failed to " + (Uninstall ? "unregister" : "register") + " dll with RegSvr32. Return code: " + process2.ExitCode + ". File: " + DllPath);
				}
			}
		}

		public void RunProcessesCheck()
		{
			bw.DoWork += bw_DoWorkProcessesCheck;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedProcessesCheck;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkProcessesCheck(object sender, DoWorkEventArgs e)
		{
			List<FileInfo> list = null;
			List<Process> list2 = null;
			List<string> list3 = new List<string>();
			Exception ex = null;
			Directory.CreateDirectory(Path.Combine(TempDirectory, "backup"));
			try
			{
				if (SkipStartService != null)
				{
					using (ServiceController serviceController = new ServiceController(SkipStartService))
					{
						if (serviceController.Status != ServiceControllerStatus.Stopped)
						{
							serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
						}
					}
				}
				foreach (string item in UpdtDetails.ServicesToStop)
				{
					using (ServiceController serviceController2 = new ServiceController(item))
					{
						ServiceControllerStatus serviceControllerStatus = ServiceControllerStatus.Stopped;
						try
						{
							serviceControllerStatus = serviceController2.Status;
						}
						catch
						{
						}
						if (serviceControllerStatus == ServiceControllerStatus.Running)
						{
							try
							{
								serviceController2.Stop();
							}
							catch (Exception)
							{
								serviceController2.Refresh();
								if (serviceController2.Status != ServiceControllerStatus.Stopped)
								{
									throw;
								}
							}
							bw.ReportProgress(0, new object[5]
							{
								-1,
								-1,
								"Waiting for service to stop: " + serviceController2.DisplayName,
								ProgressStatus.None,
								null
							});
							serviceController2.WaitForStatus(ServiceControllerStatus.Stopped);
							list3.Add(item);
						}
					}
				}
				list = new List<FileInfo>(new DirectoryInfo(ProgramDirectory).GetFiles("*.exe", SearchOption.AllDirectories));
				RemoveSelfFromProcesses(list);
				DeleteClientInPath(ProgramDirectory, Path.Combine(TempDirectory, "base"));
				list2 = ProcessesNeedClosing(list);
				if (list2.Count == 0)
				{
					list = null;
					list2 = null;
				}
				else if (SkipUIReporting)
				{
					for (int i = 0; i < 20; i++)
					{
						Thread.Sleep(1000);
						list2 = ProcessesNeedClosing(list);
						if (list2.Count == 0)
						{
							break;
						}
					}
					if (list2.Count != 0)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendLine(list2.Count + " processes are running:\r\n");
						foreach (Process item2 in list2)
						{
							stringBuilder.AppendLine(item2.MainWindowTitle + " (" + item2.ProcessName + ".exe)");
						}
						throw new Exception(stringBuilder.ToString());
					}
				}
			}
			catch (Exception ex3)
			{
				ex = ex3;
			}
			RollbackUpdate.WriteRollbackServices(Path.Combine(TempDirectory, "backup\\stoppedServices.bak"), list3);
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(1, false);
				RollbackUpdate.RollbackStoppedServices(TempDirectory);
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Success,
					new object[2]
					{
						list,
						list2
					}
				});
			}
		}

		private void bw_RunWorkerCompletedProcessesCheck(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkProcessesCheck;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedProcessesCheck;
		}

		private static void RemoveSelfFromProcesses(List<FileInfo> files)
		{
			int num = 0;
			while (true)
			{
				if (num < files.Count)
				{
					if (ProcessIsSelf(files[num].FullName))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			files.RemoveAt(num);
		}

		public static bool ProcessIsSelf(string processPath)
		{
			string selfLocation = VersionTools.SelfLocation;
			return string.Equals(processPath, selfLocation, StringComparison.OrdinalIgnoreCase);
		}

		private static List<Process> ProcessesNeedClosing(List<FileInfo> baseFiles)
		{
			List<Process> list = new List<Process>();
			foreach (FileInfo baseFile in baseFiles)
			{
				Process[] processesByName = Process.GetProcessesByName(baseFile.Name.Replace(baseFile.Extension, ""));
				Process[] array = processesByName;
				foreach (Process process in array)
				{
					try
					{
						if (process.MainModule != null && string.Equals(process.MainModule.FileName, baseFile.FullName, StringComparison.OrdinalIgnoreCase))
						{
							list.Add(process);
						}
					}
					catch
					{
					}
				}
			}
			return list;
		}

		public void RunUpdateRegistry()
		{
			bw.DoWork += bw_DoWorkRegistry;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedRegistry;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkRegistry(object sender, DoWorkEventArgs e)
		{
			List<RegChange> rollbackRegistry = new List<RegChange>();
			for (int i = 0; i < UpdtDetails.RegistryModifications.Count; i++)
			{
				UpdtDetails.RegistryModifications[i] = ParseRegChange(UpdtDetails.RegistryModifications[i]);
			}
			Exception ex = null;
			try
			{
				UpdateRegistry(rollbackRegistry);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			RollbackUpdate.WriteRollbackRegistry(Path.Combine(TempDirectory, "backup\\regList.bak"), rollbackRegistry);
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(1, true);
				RollbackUpdate.RollbackRegistry(TempDirectory);
				bw.ReportProgress(1, false);
				RollbackUpdate.RollbackFiles(TempDirectory, ProgramDirectory);
				RollbackUpdate.RollbackUnregedCOM(TempDirectory);
				RollbackUpdate.RollbackStoppedServices(TempDirectory);
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_RunWorkerCompletedRegistry(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkRegistry;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedRegistry;
		}

		private void UpdateRegistry(List<RegChange> rollbackRegistry)
		{
			int num = 0;
			bw.ReportProgress(0, new object[5]
			{
				GetRelativeProgess(5, 0),
				0,
				string.Empty,
				ProgressStatus.None,
				null
			});
			foreach (RegChange registryModification in UpdtDetails.RegistryModifications)
			{
				if (IsCancelled())
				{
					break;
				}
				num++;
				int num2 = num * 100 / UpdtDetails.RegistryModifications.Count;
				bw.ReportProgress(0, new object[5]
				{
					GetRelativeProgess(5, num2),
					num2,
					registryModification.ToString(),
					ProgressStatus.None,
					null
				});
				registryModification.ExecuteOperation(rollbackRegistry);
			}
		}

		public void RunSelfUpdate()
		{
			bw.DoWork += bw_DoWorkSelfUpdate;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedSelfUpdate;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkSelfUpdate(object sender, DoWorkEventArgs e)
		{
			Exception ex = null;
			try
			{
				ExtractUpdateFile();
				try
				{
					File.Delete(Filename);
				}
				catch
				{
				}
				KillProcess(OldSelfLoc);
				string text = Path.Combine(OutputDirectory, "updtdetails.udt");
				if (File.Exists(text))
				{
					UpdtDetails = UpdateDetails.Load(text);
					File.Delete(text);
				}
				CreatewyUpdateFromPatch();
				UpdateFile updateFile = FindNewClient();
				int num = 0;
				while (true)
				{
					try
					{
						FileAttributes attributes = File.GetAttributes(OldSelfLoc);
						bool flag = (attributes & FileAttributes.Hidden) != 0 || (attributes & FileAttributes.ReadOnly) != 0 || (attributes & FileAttributes.System) != 0;
						if (flag)
						{
							File.SetAttributes(OldSelfLoc, FileAttributes.Normal);
						}
						File.Copy(NewSelfLoc, OldSelfLoc, overwrite: true);
						if (flag)
						{
							File.SetAttributes(OldSelfLoc, attributes);
						}
					}
					catch (IOException e2)
					{
						int hRForException = Marshal.GetHRForException(e2);
						if ((hRForException & 0xFFFF) != 32)
						{
							throw;
						}
						Thread.Sleep(1000);
						if (!IsCancelled())
						{
							if (num == 30)
							{
								throw;
							}
							num++;
							continue;
						}
					}
					break;
				}
				if (updateFile != null)
				{
					NGenInstall(OldSelfLoc, updateFile);
				}
				File.Delete(NewSelfLoc);
				Directory.Delete(Path.Combine(OutputDirectory, "base"));
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					"Cancelling update...",
					ProgressStatus.None,
					null
				});
				if (ex != null && (object)ex.GetType() != typeof(PatchApplicationException))
				{
					try
					{
						Directory.Delete(OutputDirectory, recursive: true);
					}
					catch
					{
					}
				}
				else
				{
					string[] directories = Directory.GetDirectories(TempDirectory);
					string[] array = directories;
					foreach (string path in array)
					{
						try
						{
							Directory.Delete(path, recursive: true);
						}
						catch
						{
						}
					}
				}
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					"Self update complete",
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_RunWorkerCompletedSelfUpdate(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkSelfUpdate;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedSelfUpdate;
		}

		public void JustExtractSelfUpdate()
		{
			bw.DoWork += bw_DoWorkExtractSelfUpdate;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedExtractSelfUpdate;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkExtractSelfUpdate(object sender, DoWorkEventArgs e)
		{
			Exception ex = null;
			try
			{
				if (!Directory.Exists(OutputDirectory))
				{
					Directory.CreateDirectory(OutputDirectory);
				}
				ExtractUpdateFile();
				try
				{
					File.Delete(Filename);
				}
				catch
				{
				}
				string text = Path.Combine(OutputDirectory, "updtdetails.udt");
				if (File.Exists(text))
				{
					UpdtDetails = UpdateDetails.Load(text);
				}
				CreatewyUpdateFromPatch();
				FindNewClient();
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					"Cancelling update...",
					ProgressStatus.None,
					null
				});
				if (ex != null)
				{
					try
					{
						Directory.Delete(OutputDirectory, recursive: true);
					}
					catch
					{
					}
				}
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					"Self update extraction complete",
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_RunWorkerCompletedExtractSelfUpdate(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkExtractSelfUpdate;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedExtractSelfUpdate;
		}

		public void JustInstallSelfUpdate()
		{
			bw.DoWork += bw_DoWorkInstallSelfUpdate;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedInstallSelfUpdate;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkInstallSelfUpdate(object sender, DoWorkEventArgs e)
		{
			Exception ex = null;
			try
			{
				string text = Path.Combine(OutputDirectory, "updtdetails.udt");
				if (File.Exists(text))
				{
					UpdtDetails = UpdateDetails.Load(text);
				}
				UpdateFile updateFile = FindNewClient();
				KillProcess(OldSelfLoc);
				int num = 0;
				while (true)
				{
					try
					{
						FileAttributes attributes = File.GetAttributes(OldSelfLoc);
						bool flag = (attributes & FileAttributes.Hidden) != 0 || (attributes & FileAttributes.ReadOnly) != 0 || (attributes & FileAttributes.System) != 0;
						if (flag)
						{
							File.SetAttributes(OldSelfLoc, FileAttributes.Normal);
						}
						File.Copy(NewSelfLoc, OldSelfLoc, overwrite: true);
						if (flag)
						{
							File.SetAttributes(OldSelfLoc, attributes);
						}
					}
					catch (IOException e2)
					{
						int hRForException = Marshal.GetHRForException(e2);
						if ((hRForException & 0xFFFF) != 32)
						{
							throw;
						}
						Thread.Sleep(1000);
						if (!IsCancelled())
						{
							if (num == 30)
							{
								throw;
							}
							num++;
							continue;
						}
					}
					break;
				}
				if (updateFile != null)
				{
					NGenInstall(OldSelfLoc, updateFile);
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					"Cancelling update...",
					ProgressStatus.None,
					null
				});
				if (ex != null)
				{
					try
					{
						Directory.Delete(OutputDirectory, recursive: true);
					}
					catch
					{
					}
				}
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					"Self update complete",
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_RunWorkerCompletedInstallSelfUpdate(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkInstallSelfUpdate;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedInstallSelfUpdate;
		}

		private void CreatewyUpdateFromPatch()
		{
			if (!Directory.Exists(Path.Combine(OutputDirectory, "patches")))
			{
				return;
			}
			ProgramDirectory = Path.GetDirectoryName(OldSelfLoc);
			TempDirectory = OutputDirectory;
			if (UpdtDetails.UpdateFiles[0].DeltaPatchRelativePath != null)
			{
				string path = Path.Combine(TempDirectory, UpdtDetails.UpdateFiles[0].RelativePath);
				if (!Directory.Exists(Path.GetDirectoryName(path)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(path));
				}
				try
				{
					using (FileStream original = File.Open(OldSelfLoc, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using (FileStream delta = File.Open(Path.Combine(TempDirectory, UpdtDetails.UpdateFiles[0].DeltaPatchRelativePath), FileMode.Open, FileAccess.Read, FileShare.Read))
						{
							using (FileStream output = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
							{
								VcdiffDecoder.Decode(original, delta, output, UpdtDetails.UpdateFiles[0].NewFileAdler32);
							}
						}
					}
				}
				catch (Exception ex)
				{
					throw new PatchApplicationException("Patch failed to apply to this file: " + FixUpdateDetailsPaths(UpdtDetails.UpdateFiles[0].RelativePath) + "\r\n\r\nBecause that file failed to patch, and there's no \"catch-all\" update to download, the update failed to apply. The failure to patch usually happens because the file was modified from the original version. Reinstall the original version of this app.\r\n\r\n\r\nInternal error: " + ex.Message);
				}
				File.SetLastWriteTime(path, File.GetLastWriteTime(Path.Combine(TempDirectory, UpdtDetails.UpdateFiles[0].DeltaPatchRelativePath)));
			}
			try
			{
				Directory.Delete(Path.Combine(TempDirectory, "patches"), recursive: true);
			}
			catch
			{
			}
		}

		private UpdateFile FindNewClient()
		{
			for (int i = 0; i < UpdtDetails.UpdateFiles.Count; i++)
			{
				if (UpdtDetails.UpdateFiles[i].IsNETAssembly)
				{
					NewSelfLoc = Path.Combine(OutputDirectory, UpdtDetails.UpdateFiles[i].RelativePath);
					return UpdtDetails.UpdateFiles[i];
				}
			}
			string[] files = Directory.GetFiles(Path.Combine(OutputDirectory, "base"), "*.exe", SearchOption.AllDirectories);
			if (files.Length > 0)
			{
				NewSelfLoc = files[0];
				return null;
			}
			throw new Exception("New wyUpdate couldn't be found.");
		}

		private static void KillProcess(string filename)
		{
			Process[] processes = Process.GetProcesses();
			Process[] array = processes;
			foreach (Process process in array)
			{
				try
				{
					if (string.Equals(process.MainModule.FileName, filename, StringComparison.OrdinalIgnoreCase))
					{
						process.Kill();
					}
				}
				catch
				{
				}
			}
		}

		public InstallUpdate()
		{
			bw.WorkerReportsProgress = true;
			bw.WorkerSupportsCancellation = true;
		}

		private void bw_DoWorkUpdateFiles(object sender, DoWorkEventArgs e)
		{
			string text = Path.Combine(TempDirectory, "backup");
			string[] array = new string[13];
			string[] array2 = new string[13]
			{
				"base",
				"system",
				"64system",
				"root",
				"appdata",
				"lappdata",
				"comappdata",
				"comdesktop",
				"comstartmenu",
				"cp86",
				"cp64",
				"curdesk",
				"curstart"
			};
			string[] array3 = new string[13]
			{
				ProgramDirectory,
				SystemFolders.GetSystem32x86(),
				SystemFolders.GetSystem32x64(),
				SystemFolders.GetRootDrive(),
				SystemFolders.GetCurrentUserAppData(),
				SystemFolders.GetCurrentUserLocalAppData(),
				SystemFolders.GetCommonAppData(),
				SystemFolders.GetCommonDesktop(),
				SystemFolders.GetCommonProgramsStartMenu(),
				SystemFolders.GetCommonProgramFilesx86(),
				SystemFolders.GetCommonProgramFilesx64(),
				SystemFolders.GetCurrentUserDesktop(),
				SystemFolders.GetCurrentUserProgramsStartMenu()
			};
			List<FileFolder> rollbackList = new List<FileFolder>();
			int totalDone = 0;
			Exception ex = null;
			try
			{
				int totalFiles = 0;
				for (int i = 0; i < array2.Length; i++)
				{
					if (Directory.Exists(Path.Combine(TempDirectory, array2[i])))
					{
						array[i] = Path.Combine(text, array2[i]);
						array2[i] = Path.Combine(TempDirectory, array2[i]);
						Directory.CreateDirectory(array[i]);
						DeleteClientInPath(array3[i], array2[i]);
						totalFiles += new DirectoryInfo(array2[i]).GetFiles("*", SearchOption.AllDirectories).Length;
					}
				}
				for (int j = 0; j < array2.Length; j++)
				{
					if (IsCancelled())
					{
						break;
					}
					if (array[j] != null)
					{
						UpdateFiles(array2[j], array3[j], array[j], rollbackList, ref totalDone, ref totalFiles);
					}
				}
				DeleteFiles(text, rollbackList);
				InstallShortcuts(array3, text, rollbackList);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			RollbackUpdate.WriteRollbackFiles(Path.Combine(text, "fileList.bak"), rollbackList);
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(1, false);
				RollbackUpdate.RollbackFiles(TempDirectory, ProgramDirectory);
				RollbackUpdate.RollbackUnregedCOM(TempDirectory);
				RollbackUpdate.RollbackStoppedServices(TempDirectory);
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.ProgressPercentage == 0)
			{
				object[] array = (object[])e.UserState;
				if (this.ProgressChanged != null)
				{
					this.ProgressChanged((int)array[0], (int)array[1], (string)array[2], (ProgressStatus)array[3], array[4]);
				}
			}
			else if (e.ProgressPercentage == 1 && this.Rollback != null)
			{
				this.Rollback((bool)e.UserState);
			}
		}

		private void bw_RunWorkerCompletedUpdateFiles(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkUpdateFiles;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedUpdateFiles;
		}

		public static int GetRelativeProgess(int stepOn, int stepProgress)
		{
			return stepOn * 100 / 7 + stepProgress / 7;
		}

		private void UpdateFiles(string tempDir, string progDir, string backupFolder, List<FileFolder> rollbackList, ref int totalDone, ref int totalFiles)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(tempDir);
			FileInfo[] files = directoryInfo.GetFiles("*");
			for (int i = 0; i < files.Length; i++)
			{
				if (IsCancelled())
				{
					break;
				}
				int num = totalDone * 100 / totalFiles;
				bw.ReportProgress(0, new object[5]
				{
					GetRelativeProgess(4, num),
					num,
					"Updating " + files[i].Name,
					ProgressStatus.None,
					null
				});
				if (File.Exists(Path.Combine(progDir, files[i].Name)))
				{
					int num2 = 0;
					while (true)
					{
						try
						{
							string text = Path.Combine(progDir, files[i].Name);
							File.Copy(text, Path.Combine(backupFolder, files[i].Name), overwrite: true);
							FileAttributes attributes = File.GetAttributes(text);
							bool flag = (attributes & FileAttributes.Hidden) != 0 || (attributes & FileAttributes.ReadOnly) != 0 || (attributes & FileAttributes.System) != 0;
							if (flag)
							{
								File.SetAttributes(text, FileAttributes.Normal);
							}
							File.Copy(files[i].FullName, text, overwrite: true);
							if (flag)
							{
								File.SetAttributes(text, attributes);
							}
						}
						catch (IOException e)
						{
							int hRForException = Marshal.GetHRForException(e);
							int num3 = hRForException & 0xFFFF;
							if (num3 != 32 && num3 != 1224)
							{
								throw;
							}
							if (!SkipUIReporting)
							{
								bw.ReportProgress(0, new object[5]
								{
									-1,
									-1,
									string.Empty,
									ProgressStatus.SharingViolation,
									Path.Combine(progDir, files[i].Name)
								});
							}
							Thread.Sleep(1000);
							if (!IsCancelled())
							{
								if (SkipUIReporting && num2 == 20)
								{
									throw;
								}
								num2++;
								continue;
							}
						}
						break;
					}
				}
				else
				{
					File.Move(files[i].FullName, Path.Combine(progDir, files[i].Name));
					rollbackList.Add(new FileFolder(Path.Combine(progDir, files[i].Name)));
				}
				totalDone++;
			}
			if (IsCancelled())
			{
				return;
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories("*");
			for (int j = 0; j < directories.Length; j++)
			{
				if (IsCancelled())
				{
					break;
				}
				string text2 = Path.Combine(progDir, directories[j].Name);
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
					rollbackList.Add(new FileFolder(text2, deleteFolder: true));
				}
				else
				{
					Directory.CreateDirectory(Path.Combine(backupFolder, directories[j].Name));
				}
				UpdateFiles(directories[j].FullName, text2, Path.Combine(backupFolder, directories[j].Name), rollbackList, ref totalDone, ref totalFiles);
			}
		}

		private static void SetACLOnFolders(string basis, string extracted, string backup)
		{
			try
			{
				AuthorizationRuleCollection accessRules = new DirectoryInfo(basis).GetAccessControl(AccessControlSections.All).GetAccessRules(includeExplicit: true, includeInherited: true, typeof(NTAccount));
				AuthorizationRuleCollection auditRules = new DirectoryInfo(basis).GetAccessControl(AccessControlSections.All).GetAuditRules(includeExplicit: true, includeInherited: true, typeof(NTAccount));
				DirectoryInfo directoryInfo = new DirectoryInfo(extracted);
				DirectorySecurity accessControl = directoryInfo.GetAccessControl(AccessControlSections.All);
				AuthorizationRuleCollection accessRules2 = accessControl.GetAccessRules(includeExplicit: true, includeInherited: true, typeof(NTAccount));
				AuthorizationRuleCollection auditRules2 = accessControl.GetAuditRules(includeExplicit: true, includeInherited: true, typeof(NTAccount));
				DirectorySecurity directorySecurity = new DirectorySecurity();
				foreach (FileSystemAccessRule item in accessRules2)
				{
					directorySecurity.AddAccessRule(item);
				}
				foreach (FileSystemAuditRule item2 in auditRules2)
				{
					directorySecurity.AddAuditRule(item2);
				}
				DirectoryInfo directoryInfo2 = new DirectoryInfo(backup);
				DirectorySecurity accessControl2 = directoryInfo2.GetAccessControl(AccessControlSections.All);
				accessRules2 = accessControl2.GetAccessRules(includeExplicit: true, includeInherited: true, typeof(NTAccount));
				auditRules2 = accessControl2.GetAuditRules(includeExplicit: true, includeInherited: true, typeof(NTAccount));
				DirectorySecurity directorySecurity2 = new DirectorySecurity();
				foreach (FileSystemAccessRule item3 in accessRules2)
				{
					directorySecurity2.AddAccessRule(item3);
				}
				foreach (FileSystemAuditRule item4 in auditRules2)
				{
					directorySecurity2.AddAuditRule(item4);
				}
				foreach (FileSystemAccessRule item5 in accessRules)
				{
					directorySecurity.AddAccessRule(item5);
					directorySecurity2.AddAccessRule(item5);
				}
				foreach (FileSystemAuditRule item6 in auditRules)
				{
					directorySecurity.AddAuditRule(item6);
					directorySecurity2.AddAuditRule(item6);
				}
				directoryInfo.SetAccessControl(directorySecurity);
				directoryInfo2.SetAccessControl(directorySecurity2);
			}
			catch (Exception ex)
			{
				throw new Exception("Failed to set the ACL (access control list) on the files and folders. Make sure this user has the ability to read and write ACL properties of files and folders. Full error: " + ex.Message);
			}
		}

		public void RunUpdateFiles()
		{
			bw.DoWork += bw_DoWorkUpdateFiles;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedUpdateFiles;
			bw.RunWorkerAsync();
		}

		private void DeleteFiles(string backupFolder, List<FileFolder> rollbackList)
		{
			foreach (UpdateFile updateFile in UpdtDetails.UpdateFiles)
			{
				if (updateFile.DeleteFile)
				{
					string text = Path.Combine(backupFolder, updateFile.RelativePath.Substring(0, updateFile.RelativePath.LastIndexOf('\\')));
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					string text2 = FixUpdateDetailsPaths(updateFile.RelativePath);
					if (!string.Equals(VersionTools.SelfLocation, text2, StringComparison.OrdinalIgnoreCase) && File.Exists(text2))
					{
						File.Copy(text2, Path.Combine(text, Path.GetFileName(text2)));
						File.SetAttributes(text2, FileAttributes.Normal);
						File.Delete(text2);
					}
				}
			}
			for (int num = UpdtDetails.FoldersToDelete.Count - 1; num >= 0; num--)
			{
				string text = FixUpdateDetailsPaths(UpdtDetails.FoldersToDelete[num]);
				try
				{
					Directory.Delete(text, UpdtDetails.FoldersToDelete[num].StartsWith("coms"));
					rollbackList.Add(new FileFolder(text, deleteFolder: false));
				}
				catch
				{
				}
			}
		}

		private void InstallShortcuts(string[] destFolders, string backupFolder, List<FileFolder> rollbackList)
		{
			bool flag = true;
			bool flag2 = true;
			bool flag3 = true;
			bool flag4 = true;
			foreach (string previousCommonDesktopShortcut in UpdtDetails.PreviousCommonDesktopShortcuts)
			{
				if (File.Exists(Path.Combine(destFolders[7], previousCommonDesktopShortcut.Substring(11))))
				{
					flag = true;
					break;
				}
				flag = false;
			}
			foreach (string previousCommonSMenuShortcut in UpdtDetails.PreviousCommonSMenuShortcuts)
			{
				if (File.Exists(Path.Combine(destFolders[8], previousCommonSMenuShortcut.Substring(13))))
				{
					flag2 = true;
					break;
				}
				flag2 = false;
			}
			foreach (string previousCUserDesktopShortcut in UpdtDetails.PreviousCUserDesktopShortcuts)
			{
				if (File.Exists(Path.Combine(destFolders[11], previousCUserDesktopShortcut.Substring(8))))
				{
					flag3 = true;
					break;
				}
				flag3 = false;
			}
			foreach (string previousCUserSMenuShortcut in UpdtDetails.PreviousCUserSMenuShortcuts)
			{
				if (File.Exists(Path.Combine(destFolders[12], previousCUserSMenuShortcut.Substring(9))))
				{
					flag4 = true;
					break;
				}
				flag4 = false;
			}
			for (int i = 0; i < UpdtDetails.ShortcutInfos.Count; i++)
			{
				string a = UpdtDetails.ShortcutInfos[i].RelativeOuputPath.Substring(0, 4);
				if ((a == "comd" && !flag) || (a == "coms" && !flag2) || (a == "curd" && !flag3) || (a == "curs" && !flag4))
				{
					continue;
				}
				a = FixUpdateDetailsPaths(UpdtDetails.ShortcutInfos[i].RelativeOuputPath);
				string text;
				if (File.Exists(a))
				{
					text = Path.Combine(backupFolder, UpdtDetails.ShortcutInfos[i].RelativeOuputPath.Substring(0, UpdtDetails.ShortcutInfos[i].RelativeOuputPath.LastIndexOf('\\')));
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
					File.Copy(a, Path.Combine(text, Path.GetFileName(a)), overwrite: true);
					File.Delete(a);
				}
				else
				{
					rollbackList.Add(new FileFolder(a));
				}
				text = Path.GetDirectoryName(a);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
					rollbackList.Add(new FileFolder(text, deleteFolder: true));
				}
				ShellShortcut shellShortcut = new ShellShortcut(a);
				shellShortcut.Path = ParseText(UpdtDetails.ShortcutInfos[i].Path);
				shellShortcut.WorkingDirectory = ParseText(UpdtDetails.ShortcutInfos[i].WorkingDirectory);
				shellShortcut.WindowStyle = UpdtDetails.ShortcutInfos[i].WindowStyle;
				shellShortcut.Description = UpdtDetails.ShortcutInfos[i].Description;
				ShellShortcut shellShortcut2 = shellShortcut;
				shellShortcut2.Save();
			}
		}

		public void RunDeleteTemporary()
		{
			bw.DoWork += bw_DoWorkDeleteTemporary;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedDeleteTemporary;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkDeleteTemporary(object sender, DoWorkEventArgs e)
		{
			try
			{
				Directory.Delete(TempDirectory, recursive: true);
			}
			catch
			{
			}
			bw.ReportProgress(0, new object[5]
			{
				-1,
				-1,
				string.Empty,
				ProgressStatus.Success,
				null
			});
		}

		private void bw_RunWorkerCompletedDeleteTemporary(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkDeleteTemporary;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedDeleteTemporary;
		}

		public void RunUninstall()
		{
			bw.DoWork += bw_DoWorkUninstall;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedUninstall;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkUninstall(object sender, DoWorkEventArgs e)
		{
			List<UninstallFileInfo> list = new List<UninstallFileInfo>();
			List<string> list2 = new List<string>();
			List<RegChange> list3 = new List<RegChange>();
			List<UninstallFileInfo> list4 = new List<UninstallFileInfo>();
			List<string> list5 = new List<string>();
			RollbackUpdate.ReadUninstallData(Filename, list, list2, list3, list4, list5);
			foreach (string item in list5)
			{
				try
				{
					using (ServiceController serviceController = new ServiceController(item))
					{
						serviceController.Stop();
						serviceController.WaitForStatus(ServiceControllerStatus.Stopped);
					}
				}
				catch
				{
				}
			}
			foreach (UninstallFileInfo item2 in list4)
			{
				try
				{
					RegisterDllServer(item2.Path, Uninstall: true);
				}
				catch
				{
				}
			}
			foreach (UninstallFileInfo item3 in list)
			{
				try
				{
					if (item3.UnNGENFile)
					{
						NGenUninstall(item3.Path, item3);
					}
					if (item3.DeleteFile)
					{
						File.Delete(item3.Path);
					}
				}
				catch
				{
				}
			}
			for (int num = list2.Count - 1; num >= 0; num--)
			{
				try
				{
					Directory.Delete(list2[num]);
				}
				catch
				{
				}
			}
			bw.ReportProgress(0, new object[5]
			{
				-1,
				-1,
				string.Empty,
				ProgressStatus.None,
				null
			});
			foreach (RegChange item4 in list3)
			{
				try
				{
					item4.ExecuteOperation();
				}
				catch
				{
				}
			}
			bw.ReportProgress(0, new object[5]
			{
				-1,
				-1,
				string.Empty,
				ProgressStatus.Success,
				null
			});
		}

		private void bw_RunWorkerCompletedUninstall(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkUninstall;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedUninstall;
		}

		public void RunPreExecute()
		{
			bw.DoWork += bw_DoWorkPreExecute;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompletedPreExecute;
			bw.RunWorkerAsync();
		}

		private void bw_DoWorkPreExecute(object sender, DoWorkEventArgs e)
		{
			bw.ReportProgress(0, new object[5]
			{
				GetRelativeProgess(3, 0),
				0,
				string.Empty,
				ProgressStatus.None,
				null
			});
			List<UninstallFileInfo> list = new List<UninstallFileInfo>();
			Exception ex = null;
			for (int i = 0; i < UpdtDetails.UpdateFiles.Count; i++)
			{
				bool flag = (UpdtDetails.UpdateFiles[i].RegisterCOMDll & (COMRegistration.UnRegister | COMRegistration.PreviouslyRegistered)) != 0;
				if ((!UpdtDetails.UpdateFiles[i].Execute || !UpdtDetails.UpdateFiles[i].ExBeforeUpdate) && !flag)
				{
					continue;
				}
				string text = FixUpdateDetailsPaths(UpdtDetails.UpdateFiles[i].RelativePath);
				if (!string.IsNullOrEmpty(text))
				{
					if (!flag)
					{
						try
						{
							if (UpdtDetails.UpdateFiles[i].ElevationType == ElevationType.NotElevated && IsAdmin && VistaTools.AtLeastVista())
							{
								int num = (int)LimitedProcess.Start(text, string.IsNullOrEmpty(UpdtDetails.UpdateFiles[i].CommandLineArgs) ? null : ParseText(UpdtDetails.UpdateFiles[i].CommandLineArgs), fallback: false, UpdtDetails.UpdateFiles[i].WaitForExecution, UpdtDetails.UpdateFiles[i].ProcessWindowStyle);
								if (UpdtDetails.UpdateFiles[i].RollbackOnNonZeroRet && num != 0 && (UpdtDetails.UpdateFiles[i].RetExceptions == null || !UpdtDetails.UpdateFiles[i].RetExceptions.Contains(num)))
								{
									ex = new Exception("\"" + text + "\" returned " + num + ".");
									break;
								}
							}
							else
							{
								ProcessStartInfo processStartInfo = new ProcessStartInfo();
								processStartInfo.FileName = text;
								processStartInfo.WindowStyle = UpdtDetails.UpdateFiles[i].ProcessWindowStyle;
								ProcessStartInfo processStartInfo2 = processStartInfo;
								if (!string.IsNullOrEmpty(UpdtDetails.UpdateFiles[i].CommandLineArgs))
								{
									processStartInfo2.Arguments = ParseText(UpdtDetails.UpdateFiles[i].CommandLineArgs);
								}
								if (!IsAdmin && UpdtDetails.UpdateFiles[i].ElevationType == ElevationType.Elevated)
								{
									processStartInfo2.Verb = "runas";
									processStartInfo2.ErrorDialog = true;
									processStartInfo2.ErrorDialogParentHandle = MainWindowHandle;
								}
								Process process = Process.Start(processStartInfo2);
								if (UpdtDetails.UpdateFiles[i].WaitForExecution && process != null)
								{
									process.WaitForExit();
									if (UpdtDetails.UpdateFiles[i].RollbackOnNonZeroRet && process.ExitCode != 0 && (UpdtDetails.UpdateFiles[i].RetExceptions == null || !UpdtDetails.UpdateFiles[i].RetExceptions.Contains(process.ExitCode)))
									{
										ex = new Exception("\"" + processStartInfo2.FileName + "\" returned " + process.ExitCode + ".");
										break;
									}
								}
							}
						}
						catch (Exception ex2)
						{
							ex = new Exception("Failed to execute the file \"" + text + "\": " + ex2.Message, ex2);
							break;
						}
					}
					else
					{
						try
						{
							RegisterDllServer(text, Uninstall: true);
							list.Add(new UninstallFileInfo
							{
								Path = text,
								RegisterCOMDll = COMRegistration.Register
							});
						}
						catch (Exception ex3)
						{
							ex = ex3;
							break;
						}
					}
				}
			}
			RollbackUpdate.WriteRollbackCOM(Path.Combine(TempDirectory, "backup\\unreggedComList.bak"), list);
			if (IsCancelled() || ex != null)
			{
				bw.ReportProgress(1, false);
				RollbackUpdate.RollbackUnregedCOM(TempDirectory);
				RollbackUpdate.RollbackStoppedServices(TempDirectory);
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Failure,
					ex
				});
			}
			else
			{
				bw.ReportProgress(0, new object[5]
				{
					-1,
					-1,
					string.Empty,
					ProgressStatus.Success,
					null
				});
			}
		}

		private void bw_RunWorkerCompletedPreExecute(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWorkPreExecute;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompletedPreExecute;
		}

		private void FixUpdateFilesPaths(List<UpdateFile> updateFiles)
		{
			for (int i = 0; i < updateFiles.Count; i++)
			{
				if (updateFiles[i].IsNETAssembly)
				{
					UpdateFile updateFile = updateFiles[i];
					updateFile.Filename = FixUpdateDetailsPaths(updateFile.RelativePath);
					updateFiles[i] = updateFile;
				}
			}
		}

		private string FixUpdateDetailsPaths(string relPath)
		{
			if (relPath.Length < 4)
			{
				return null;
			}
			switch (relPath.Substring(0, 4))
			{
			case "base":
				return Path.Combine(ProgramDirectory, relPath.Substring(5));
			case "syst":
				return Path.Combine(SystemFolders.GetSystem32x86(), relPath.Substring(7));
			case "64sy":
				return Path.Combine(SystemFolders.GetSystem32x64(), relPath.Substring(9));
			case "temp":
				return Path.Combine(TempDirectory, relPath);
			case "appd":
				return Path.Combine(SystemFolders.GetCurrentUserAppData(), relPath.Substring(8));
			case "lapp":
				return Path.Combine(SystemFolders.GetCurrentUserLocalAppData(), relPath.Substring(9));
			case "coma":
				return Path.Combine(SystemFolders.GetCommonAppData(), relPath.Substring(11));
			case "comd":
				return Path.Combine(SystemFolders.GetCommonDesktop(), relPath.Substring(11));
			case "coms":
				return Path.Combine(SystemFolders.GetCommonProgramsStartMenu(), relPath.Substring(13));
			case "root":
				return Path.Combine(SystemFolders.GetRootDrive(), relPath.Substring(5));
			case "cp86":
				return Path.Combine(SystemFolders.GetCommonProgramFilesx86(), relPath.Substring(5));
			case "cp64":
				return Path.Combine(SystemFolders.GetCommonProgramFilesx64(), relPath.Substring(5));
			case "curd":
				return Path.Combine(SystemFolders.GetCurrentUserDesktop(), relPath.Substring(8));
			case "curs":
				return Path.Combine(SystemFolders.GetCurrentUserProgramsStartMenu(), relPath.Substring(9));
			default:
				return null;
			}
		}

		public void Cancel()
		{
			bw.CancelAsync();
		}

		public void Pause(bool pause)
		{
			paused = pause;
		}

		private bool IsCancelled()
		{
			while (paused)
			{
				if (bw.CancellationPending)
				{
					return true;
				}
				Thread.Sleep(1000);
			}
			return bw.CancellationPending;
		}

		private static void DeleteClientInPath(string destPath, string origPath)
		{
			string text = ClientInTempBase(destPath, origPath);
			if (text != null)
			{
				File.Delete(text);
			}
		}

		private static string ClientInTempBase(string actualBase, string tempBase)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			string selfLocation = VersionTools.SelfLocation;
			if (SystemFolders.PathRelativePathTo(stringBuilder, actualBase, 16u, selfLocation, 0u) && stringBuilder.Length >= 2)
			{
				selfLocation = stringBuilder.ToString().Substring(0, 2);
				if (selfLocation == ".\\")
				{
					selfLocation = Path.Combine(tempBase, stringBuilder.ToString());
					if (File.Exists(selfLocation))
					{
						return selfLocation;
					}
				}
			}
			return null;
		}

		private RegChange ParseRegChange(RegChange reg)
		{
			if (reg.RegValueKind == RegistryValueKind.MultiString || reg.RegValueKind == RegistryValueKind.String)
			{
				reg.ValueData = ParseText((string)reg.ValueData);
			}
			return reg;
		}

		private string ParseText(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			List<string> excludeVariables = new List<string>();
			return ParseVariableText(text, excludeVariables);
		}

		private string ParseVariableText(string text, List<string> excludeVariables)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = text.IndexOf('%', 0);
			if (num == -1)
			{
				return text;
			}
			stringBuilder.Append(text.Substring(0, num));
			while (num != -1)
			{
				int num2 = text.IndexOf('%', num + 1);
				if (num2 == -1)
				{
					stringBuilder.Append(text.Substring(num, text.Length - num));
					return stringBuilder.ToString();
				}
				string text2 = VariableToPretty(text.Substring(num + 1, num2 - num - 1), excludeVariables);
				if (text2 == null)
				{
					stringBuilder.Append(text.Substring(num, num2 - num));
				}
				else
				{
					stringBuilder.Append(text2);
					num2++;
					if (num2 == text.Length)
					{
						return stringBuilder.ToString();
					}
				}
				num = num2;
			}
			return stringBuilder.ToString();
		}

		private string VariableToPretty(string variable, List<string> excludeVariables)
		{
			variable = variable.ToLower();
			if (excludeVariables.Contains(variable))
			{
				return null;
			}
			excludeVariables.Add(variable);
			string text;
			switch (variable)
			{
			case "basedir":
				text = ProgramDirectory;
				if (text[text.Length - 1] != '\\')
				{
					text += '\\';
				}
				break;
			case "wu-temp":
				text = Path.Combine(TempDirectory, "temp") + "\\";
				break;
			default:
				excludeVariables.RemoveAt(excludeVariables.Count - 1);
				return null;
			}
			excludeVariables.Remove(variable);
			return text;
		}
	}
}
