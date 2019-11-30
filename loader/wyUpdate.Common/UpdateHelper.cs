using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace wyUpdate.Common
{
	internal class UpdateHelper
	{
		private PipeServer pipeServer;

		public bool RestartInfoSent;

		public bool Installing;

		public string FileOrServiceToExecuteAfterUpdate;

		public bool IsAService;

		public string ExecutionArguments;

		public string AutoUpdateID;

		private Control owner;

		public bool RunningServer
		{
			get
			{
				if (pipeServer != null)
				{
					return pipeServer.Running;
				}
				return false;
			}
		}

		public int TotalConnectedClients
		{
			get
			{
				if (pipeServer != null)
				{
					return pipeServer.TotalConnectedClients;
				}
				return 0;
			}
		}

		public event EventHandler SenderProcessClosed;

		public event RequestHandler RequestReceived;

		public void StartPipeServer(Control OwnerHandle)
		{
			owner = OwnerHandle;
			pipeServer = new PipeServer();
			pipeServer.MessageReceived += pipeServer_MessageReceived;
			pipeServer.ClientDisconnected += pipeServer_ClientDisconnected;
			pipeServer.Start(UpdateHelperData.PipenameFromFilename(VersionTools.SelfLocation));
		}

		private void pipeServer_ClientDisconnected()
		{
			try
			{
				if (!owner.IsDisposed)
				{
					owner.Invoke(new PipeServer.ClientDisconnectedHandler(ClientDisconnected));
				}
			}
			catch
			{
			}
		}

		private void ClientDisconnected()
		{
			if (this.SenderProcessClosed != null && pipeServer.TotalConnectedClients == 0)
			{
				this.SenderProcessClosed(this, EventArgs.Empty);
			}
		}

		private void pipeServer_MessageReceived(byte[] message)
		{
			try
			{
				if (!owner.IsDisposed)
				{
					owner.Invoke(new PipeServer.MessageReceivedHandler(ServerReceivedData), message);
				}
			}
			catch
			{
			}
		}

		private void ServerReceivedData(byte[] message)
		{
			UpdateHelperData updateHelperData = UpdateHelperData.FromByteArray(message);
			if (updateHelperData.Action == UpdateAction.GetwyUpdateProcessID)
			{
				pipeServer.SendMessage(new UpdateHelperData(UpdateAction.GetwyUpdateProcessID)
				{
					ProcessID = Process.GetCurrentProcess().Id
				}.GetByteArray());
				return;
			}
			UpdateStep updateStep = updateHelperData.UpdateStep;
			switch (updateStep)
			{
			case UpdateStep.RestartInfo:
				RestartInfoSent = true;
				if (updateHelperData.ExtraData.Count > 0)
				{
					FileOrServiceToExecuteAfterUpdate = updateHelperData.ExtraData[0];
					IsAService = updateHelperData.ExtraDataIsRTF[0];
				}
				if (updateHelperData.ExtraData.Count > 1)
				{
					AutoUpdateID = updateHelperData.ExtraData[1];
				}
				if (updateHelperData.ExtraData.Count > 2)
				{
					ExecutionArguments = updateHelperData.ExtraData[2];
				}
				break;
			case UpdateStep.Install:
				if (Installing)
				{
					return;
				}
				Installing = true;
				break;
			}
			if (this.RequestReceived != null)
			{
				this.RequestReceived(this, updateHelperData.Action, updateStep);
			}
		}

		public void SendProgress(int progress, UpdateStep step)
		{
			pipeServer.SendMessage(new UpdateHelperData(Response.Progress, step, progress).GetByteArray());
		}

		public void SendSuccess(string extraData1, string extraData2, bool ed2IsRtf)
		{
			UpdateHelperData updateHelperData = new UpdateHelperData(Response.Succeeded, UpdateStep.CheckForUpdate, extraData1, extraData2);
			updateHelperData.ExtraDataIsRTF[1] = ed2IsRtf;
			pipeServer.SendMessage(updateHelperData.GetByteArray());
		}

		public void SendSuccess(UpdateStep step)
		{
			pipeServer.SendMessage(new UpdateHelperData(Response.Succeeded, step).GetByteArray());
		}

		public void SendSuccess(UpdateStep step, int windowHandle)
		{
			pipeServer.SendMessage(new UpdateHelperData(Response.Succeeded, step)
			{
				ProcessID = windowHandle
			}.GetByteArray());
		}

		public void SendFailed(string messageTitle, string messageBody, UpdateStep step)
		{
			pipeServer.SendMessage(new UpdateHelperData(Response.Failed, step, messageTitle, messageBody).GetByteArray());
		}

		public void SendNewWyUpdate(string pipeName, int processID)
		{
			UpdateHelperData updateHelperData = new UpdateHelperData(UpdateAction.NewWyUpdateProcess);
			updateHelperData.ProcessID = processID;
			UpdateHelperData updateHelperData2 = updateHelperData;
			updateHelperData2.ExtraData.Add(pipeName);
			updateHelperData2.ExtraDataIsRTF.Add(item: false);
			pipeServer.SendMessage(updateHelperData2.GetByteArray());
		}
	}
}
