using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace wyUpdate
{
	public class ClientLanguage
	{
		private string m_NextButton = "Next";

		private string m_UpdateButton = "Update";

		private string m_FinishButton = "Finish";

		private string m_CancelButton = "Cancel";

		private string m_ShowDetails = "Show details";

		private ScreenDialog m_ProcessDialog = new ScreenDialog("Close processes...", null, "The following processes need to be closed before updating can continue. Select a process and click Close Process.");

		private ScreenDialog m_FilesInUseDialog = new ScreenDialog("Files in use...", "These files are used by the following processes:", "The following files are in use. These files must be closed before the update can continue.");

		private ScreenDialog m_CancelDialog = new ScreenDialog("Cancel update?", null, "Are you sure you want to exit before the update is complete?");

		private string m_ClosePrc = "Close Process";

		private string m_CloseAllPrc = "Close All Processes";

		private string m_CancelUpdate = "Cancel Update";

		private string m_ServerError = "Unable to check for updates, the server file failed to load.";

		private string m_AdminError = "wyUpdate needs administrative privileges to update %product%. You can do this one of two ways:\r\n\r\n1. When prompted, enter an administrator's username and password.\r\n\r\n2. In Windows Explorer right click wyUpdate.exe and click \"Run as Administrator\"";

		private string m_DownloadError = "The update failed to download.";

		private string m_GeneralUpdateError = "The update failed to install.";

		private string m_SelfUpdateInstallError = "The updated version of wyUpdate required to update %product% failed to install.";

		private string m_LogOffError = "Updating %product%. You must cancel wyUpdate before you can log off.";

		private ScreenDialog m_Checking = new ScreenDialog("Searching for updates", "wyUpdate is searching for updates.", "wyUpdate is searching for updates to %product%. This process could take a few minutes.");

		private ScreenDialog m_UpdateInfo = new ScreenDialog("Update Information", "Changes in the latest version of %product%.", "The version of %product% installed on this computer is %old_version%. The latest version is %new_version%. Listed below are the changes and improvements:");

		private ScreenDialog m_DownInstall = new ScreenDialog("Downloading & Installing updates", "Updating %product% to the latest version.", "wyUpdate is downloading and installing updates for %product%. This process could take a few minutes.");

		private ScreenDialog m_Uninstall = new ScreenDialog("Uninstalling files, folders, and registry", "Uninstalling files and registry for %product%.", "wyUpdate is uninstalling files and registry created when updates were applied to %product%.");

		private ScreenDialog m_SuccessUpdate = new ScreenDialog("Update successful!", null, "%product% has been successfully updated to version %new_version%");

		private ScreenDialog m_AlreadyLatest = new ScreenDialog("Latest version already installed", null, "%product% is currently up-to-date. Remember to check for new updates frequently.");

		private ScreenDialog m_NoUpdateToLatest = new ScreenDialog("No update to the latest version", null, "There is a newer version of %product% (version %new_version%), but no update available from the version you currently have installed (version %old_version%).");

		private ScreenDialog m_UpdateError = new ScreenDialog("An error occurred", null, null);

		private string m_UpdateBottom = "Click Update to begin.";

		private string m_FinishBottom = "Click Finish to exit.";

		private string m_Download = "Downloading update";

		private string m_DownloadingSelfUpdate = "Downloading new wyUpdate";

		private string m_SelfUpdate = "Updating wyUpdate";

		private string m_Extract = "Extracting files";

		private string m_Processes = "Closing processes";

		private string m_PreExec = "Executing files";

		private string m_Files = "Backing up and updating files";

		private string m_Registry = "Backing up and updating registry";

		private string m_Optimize = "Optimizing and executing files";

		private string m_TempFiles = "Removing temporary files";

		private string m_UninstallRegistry = "Uninstalling registry";

		private string m_UninstallFiles = "Uninstalling files & folders";

		private string m_RollingBackFiles = "Rolling back files";

		private string m_RollingBackRegistry = "Rolling back registry";

		private string m_ProductName;

		private string m_OldVersion;

		private string m_NewVersion;

		public string NextButton
		{
			get
			{
				return ParseText(m_NextButton);
			}
			set
			{
				m_NextButton = value;
			}
		}

		public string UpdateButton
		{
			get
			{
				return ParseText(m_UpdateButton);
			}
			set
			{
				m_UpdateButton = value;
			}
		}

		public string FinishButton
		{
			get
			{
				return ParseText(m_FinishButton);
			}
			set
			{
				m_FinishButton = value;
			}
		}

		public string CancelButton
		{
			get
			{
				return ParseText(m_CancelButton);
			}
			set
			{
				m_CancelButton = value;
			}
		}

		public string ShowDetails
		{
			get
			{
				return ParseText(m_ShowDetails);
			}
			set
			{
				m_ShowDetails = value;
			}
		}

		public ScreenDialog ProcessDialog
		{
			get
			{
				return ParseScreenDialog(m_ProcessDialog);
			}
			set
			{
				m_ProcessDialog = value;
			}
		}

		public ScreenDialog FilesInUseDialog
		{
			get
			{
				return ParseScreenDialog(m_FilesInUseDialog);
			}
			set
			{
				m_FilesInUseDialog = value;
			}
		}

		public ScreenDialog CancelDialog
		{
			get
			{
				return ParseScreenDialog(m_CancelDialog);
			}
			set
			{
				m_CancelDialog = value;
			}
		}

		public string ClosePrc
		{
			get
			{
				return ParseText(m_ClosePrc);
			}
			set
			{
				m_ClosePrc = value;
			}
		}

		public string CloseAllPrc
		{
			get
			{
				return ParseText(m_CloseAllPrc);
			}
			set
			{
				m_CloseAllPrc = value;
			}
		}

		public string CancelUpdate
		{
			get
			{
				return ParseText(m_CancelUpdate);
			}
			set
			{
				m_CancelUpdate = value;
			}
		}

		public string ServerError
		{
			get
			{
				return ParseText(m_ServerError);
			}
			set
			{
				m_ServerError = value;
			}
		}

		public string AdminError
		{
			get
			{
				return ParseText(m_AdminError);
			}
			set
			{
				m_AdminError = value;
			}
		}

		public string DownloadError
		{
			get
			{
				return ParseText(m_DownloadError);
			}
			set
			{
				m_DownloadError = value;
			}
		}

		public string GeneralUpdateError
		{
			get
			{
				return ParseText(m_GeneralUpdateError);
			}
			set
			{
				m_GeneralUpdateError = value;
			}
		}

		public string SelfUpdateInstallError
		{
			get
			{
				return ParseText(m_SelfUpdateInstallError);
			}
			set
			{
				m_SelfUpdateInstallError = value;
			}
		}

		public string LogOffError
		{
			get
			{
				return ParseText(m_LogOffError);
			}
			set
			{
				m_LogOffError = value;
			}
		}

		public ScreenDialog Checking
		{
			get
			{
				return ParseScreenDialog(m_Checking);
			}
			set
			{
				m_Checking = value;
			}
		}

		public ScreenDialog UpdateInfo
		{
			get
			{
				return ParseScreenDialog(m_UpdateInfo);
			}
			set
			{
				m_UpdateInfo = value;
			}
		}

		public ScreenDialog DownInstall
		{
			get
			{
				return ParseScreenDialog(m_DownInstall);
			}
			set
			{
				m_DownInstall = value;
			}
		}

		public ScreenDialog Uninstall
		{
			get
			{
				return ParseScreenDialog(m_Uninstall);
			}
			set
			{
				m_Uninstall = value;
			}
		}

		public ScreenDialog SuccessUpdate
		{
			get
			{
				return ParseScreenDialog(m_SuccessUpdate);
			}
			set
			{
				m_SuccessUpdate = value;
			}
		}

		public ScreenDialog AlreadyLatest
		{
			get
			{
				return ParseScreenDialog(m_AlreadyLatest);
			}
			set
			{
				m_AlreadyLatest = value;
			}
		}

		public ScreenDialog NoUpdateToLatest
		{
			get
			{
				return ParseScreenDialog(m_NoUpdateToLatest);
			}
			set
			{
				m_NoUpdateToLatest = value;
			}
		}

		public ScreenDialog UpdateError
		{
			get
			{
				return ParseScreenDialog(m_UpdateError);
			}
			set
			{
				m_UpdateError = value;
			}
		}

		public string UpdateBottom
		{
			get
			{
				return ParseText(m_UpdateBottom);
			}
			set
			{
				m_UpdateBottom = value;
			}
		}

		public string FinishBottom
		{
			get
			{
				return ParseText(m_FinishBottom);
			}
			set
			{
				m_FinishBottom = value;
			}
		}

		public string Download
		{
			get
			{
				return ParseText(m_Download);
			}
			set
			{
				m_Download = value;
			}
		}

		public string DownloadingSelfUpdate
		{
			get
			{
				return ParseText(m_DownloadingSelfUpdate);
			}
			set
			{
				m_DownloadingSelfUpdate = value;
			}
		}

		public string SelfUpdate
		{
			get
			{
				return ParseText(m_SelfUpdate);
			}
			set
			{
				m_SelfUpdate = value;
			}
		}

		public string Extract
		{
			get
			{
				return ParseText(m_Extract);
			}
			set
			{
				m_Extract = value;
			}
		}

		public string Processes
		{
			get
			{
				return ParseText(m_Processes);
			}
			set
			{
				m_Processes = value;
			}
		}

		public string PreExec
		{
			get
			{
				return ParseText(m_PreExec);
			}
			set
			{
				m_PreExec = value;
			}
		}

		public string Files
		{
			get
			{
				return ParseText(m_Files);
			}
			set
			{
				m_Files = value;
			}
		}

		public string Registry
		{
			get
			{
				return ParseText(m_Registry);
			}
			set
			{
				m_Registry = value;
			}
		}

		public string Optimize
		{
			get
			{
				return ParseText(m_Optimize);
			}
			set
			{
				m_Optimize = value;
			}
		}

		public string TempFiles
		{
			get
			{
				return ParseText(m_TempFiles);
			}
			set
			{
				m_TempFiles = value;
			}
		}

		public string UninstallRegistry
		{
			get
			{
				return ParseText(m_UninstallRegistry);
			}
			set
			{
				m_UninstallRegistry = value;
			}
		}

		public string UninstallFiles
		{
			get
			{
				return ParseText(m_UninstallFiles);
			}
			set
			{
				m_UninstallFiles = value;
			}
		}

		public string RollingBackFiles
		{
			get
			{
				return ParseText(m_RollingBackFiles);
			}
			set
			{
				m_RollingBackFiles = value;
			}
		}

		public string RollingBackRegistry
		{
			get
			{
				return ParseText(m_RollingBackRegistry);
			}
			set
			{
				m_RollingBackRegistry = value;
			}
		}

		public string NewVersion
		{
			set
			{
				m_NewVersion = value;
			}
		}

		public void SetVariables(string product, string oldversion)
		{
			m_ProductName = product;
			m_OldVersion = oldversion;
		}

		private string ParseText(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return text;
			}
			List<string> excludeVariables = new List<string>();
			return ParseVariableText(text, ref excludeVariables);
		}

		private string ParseVariableText(string text, ref List<string> excludeVariables)
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
				string text2 = VariableToPretty(text.Substring(num + 1, num2 - num - 1), ref excludeVariables);
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

		private string VariableToPretty(string variable, ref List<string> excludeVariables)
		{
			variable = variable.ToLower();
			if (excludeVariables.Contains(variable))
			{
				return null;
			}
			excludeVariables.Add(variable);
			string result;
			switch (variable)
			{
			case "product":
				result = ParseVariableText(m_ProductName, ref excludeVariables);
				break;
			case "old_version":
				result = ParseVariableText(m_OldVersion, ref excludeVariables);
				break;
			case "new_version":
				result = ParseVariableText(m_NewVersion, ref excludeVariables);
				break;
			default:
				excludeVariables.RemoveAt(excludeVariables.Count - 1);
				return null;
			}
			excludeVariables.Remove(variable);
			return result;
		}

		private ScreenDialog ParseScreenDialog(ScreenDialog dialog)
		{
			return new ScreenDialog(ParseText(dialog.Title), ParseText(dialog.SubTitle), ParseText(dialog.Content));
		}

		public void Open(MemoryStream ms)
		{
			ms.Position = 0L;
			try
			{
				using (XmlTextReader reader = new XmlTextReader(ms))
				{
					ReadLanguageFile(reader);
				}
			}
			catch
			{
			}
		}

		private void ReadLanguageFile(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
				{
					if (reader.LocalName.Equals("Buttons"))
					{
						ReadButtons(reader);
					}
					else if (reader.LocalName.Equals("Screens"))
					{
						ReadScreens(reader);
					}
					else if (reader.LocalName.Equals("Dialogs"))
					{
						ReadDialogs(reader);
					}
					else if (reader.LocalName.Equals("Status"))
					{
						ReadStatus(reader);
					}
					else if (reader.LocalName.Equals("Errors"))
					{
						ReadErrors(reader);
					}
					else if (reader.LocalName.Equals("Bottoms"))
					{
						ReadBottoms(reader);
					}
				}
			}
			reader.Close();
		}

		private void ReadButtons(XmlReader reader)
		{
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !reader.LocalName.Equals("Buttons")))
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName.Equals("Next"))
					{
						NextButton = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Update"))
					{
						UpdateButton = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Finish"))
					{
						FinishButton = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Cancel"))
					{
						CancelButton = reader.ReadString();
					}
					else if (reader.LocalName.Equals("ShowDetails"))
					{
						ShowDetails = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Close"))
					{
						ClosePrc = reader.ReadString();
					}
					else if (reader.LocalName.Equals("CloseAll"))
					{
						CloseAllPrc = reader.ReadString();
					}
					else if (reader.LocalName.Equals("CancelUpdate"))
					{
						CancelUpdate = reader.ReadString();
					}
				}
			}
		}

		private void ReadScreens(XmlReader reader)
		{
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !reader.LocalName.Equals("Screens")))
			{
				if (reader.NodeType == XmlNodeType.Element && !reader.IsEmptyElement)
				{
					if (reader.LocalName.Equals("Checking"))
					{
						ReadScreenDialog(reader, m_Checking);
					}
					else if (reader.LocalName.Equals("UpdateInfo"))
					{
						ReadScreenDialog(reader, m_UpdateInfo);
					}
					else if (reader.LocalName.Equals("DownInstall"))
					{
						ReadScreenDialog(reader, m_DownInstall);
					}
					else if (reader.LocalName.Equals("Uninstall"))
					{
						ReadScreenDialog(reader, m_Uninstall);
					}
					else if (reader.LocalName.Equals("SuccessUpdate"))
					{
						ReadScreenDialog(reader, m_SuccessUpdate);
					}
					else if (reader.LocalName.Equals("AlreadyLatest"))
					{
						ReadScreenDialog(reader, m_AlreadyLatest);
					}
					else if (reader.LocalName.Equals("NoUpdateToLatest"))
					{
						ReadScreenDialog(reader, m_NoUpdateToLatest);
					}
					else if (reader.LocalName.Equals("UpdateError"))
					{
						ReadScreenDialog(reader, m_UpdateError);
					}
				}
			}
		}

		private void ReadDialogs(XmlReader reader)
		{
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !reader.LocalName.Equals("Dialogs")))
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName.Equals("Cancel"))
					{
						ReadScreenDialog(reader, m_CancelDialog);
					}
					else if (reader.LocalName.Equals("Processes"))
					{
						ReadScreenDialog(reader, m_ProcessDialog);
					}
					else if (reader.LocalName.Equals("FilesInUse"))
					{
						ReadScreenDialog(reader, m_FilesInUseDialog);
					}
				}
			}
		}

		private static void ReadScreenDialog(XmlReader reader, ScreenDialog sd)
		{
			string localName = reader.LocalName;
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !reader.LocalName.Equals(localName)))
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName.Equals("Title"))
					{
						sd.Title = reader.ReadString();
					}
					else if (reader.LocalName.Equals("SubTitle"))
					{
						sd.SubTitle = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Content"))
					{
						sd.Content = reader.ReadString();
					}
				}
			}
		}

		private void ReadStatus(XmlReader reader)
		{
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !reader.LocalName.Equals("Status")))
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName.Equals("Download"))
					{
						Download = reader.ReadString();
					}
					else if (reader.LocalName.Equals("DownloadSelfUpdate"))
					{
						DownloadingSelfUpdate = reader.ReadString();
					}
					else if (reader.LocalName.Equals("SelfUpdate"))
					{
						SelfUpdate = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Extract"))
					{
						Extract = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Processes"))
					{
						Processes = reader.ReadString();
					}
					else if (reader.LocalName.Equals("PreExec"))
					{
						PreExec = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Files"))
					{
						Files = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Registry"))
					{
						Registry = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Optimize"))
					{
						Optimize = reader.ReadString();
					}
					else if (reader.LocalName.Equals("TempFiles"))
					{
						TempFiles = reader.ReadString();
					}
					else if (reader.LocalName.Equals("UninstallFiles"))
					{
						UninstallFiles = reader.ReadString();
					}
					else if (reader.LocalName.Equals("UninstallReg"))
					{
						UninstallRegistry = reader.ReadString();
					}
					else if (reader.LocalName.Equals("RollingBackFiles"))
					{
						RollingBackFiles = reader.ReadString();
					}
					else if (reader.LocalName.Equals("RollingBackRegistry"))
					{
						RollingBackRegistry = reader.ReadString();
					}
				}
			}
		}

		private void ReadErrors(XmlReader reader)
		{
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !reader.LocalName.Equals("Errors")))
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName.Equals("ServFile"))
					{
						ServerError = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Admin"))
					{
						AdminError = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Update"))
					{
						GeneralUpdateError = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Download"))
					{
						DownloadError = reader.ReadString();
					}
					else if (reader.LocalName.Equals("SelfUpdate"))
					{
						SelfUpdateInstallError = reader.ReadString();
					}
					else if (reader.LocalName.Equals("LogOff"))
					{
						LogOffError = reader.ReadString();
					}
				}
			}
		}

		private void ReadBottoms(XmlReader reader)
		{
			while (reader.Read() && (reader.NodeType != XmlNodeType.EndElement || !reader.LocalName.Equals("Bottoms")))
			{
				if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName.Equals("Update"))
					{
						UpdateBottom = reader.ReadString();
					}
					else if (reader.LocalName.Equals("Finish"))
					{
						FinishBottom = reader.ReadString();
					}
				}
			}
		}
	}
}
