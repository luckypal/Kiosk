using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace wyUpdate
{
	public class PipeServer
	{
		private struct SECURITY_DESCRIPTOR
		{
			public byte revision;

			public byte size;

			public short control;

			public IntPtr owner;

			public IntPtr group;

			public IntPtr sacl;

			public IntPtr dacl;
		}

		public struct SECURITY_ATTRIBUTES
		{
			public int nLength;

			public IntPtr lpSecurityDescriptor;

			public int bInheritHandle;
		}

		public class Client
		{
			public SafeFileHandle handle;

			public FileStream stream;
		}

		public delegate void MessageReceivedHandler(byte[] message);

		public delegate void ClientDisconnectedHandler();

		private const uint SECURITY_DESCRIPTOR_REVISION = 1u;

		private const int BUFFER_SIZE = 4096;

		private Thread listenThread;

		private readonly List<Client> clients = new List<Client>();

		public int TotalConnectedClients
		{
			get
			{
				lock (clients)
				{
					return clients.Count;
				}
			}
		}

		public string PipeName
		{
			get;
			private set;
		}

		public bool Running
		{
			get;
			private set;
		}

		public event MessageReceivedHandler MessageReceived;

		public event ClientDisconnectedHandler ClientDisconnected;

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern SafeFileHandle CreateNamedPipe(string pipeName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, IntPtr lpSecurityAttributes);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int ConnectNamedPipe(SafeFileHandle hNamedPipe, IntPtr lpOverlapped);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool DisconnectNamedPipe(SafeFileHandle hHandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool InitializeSecurityDescriptor(ref SECURITY_DESCRIPTOR sd, uint dwRevision);

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool SetSecurityDescriptorDacl(ref SECURITY_DESCRIPTOR sd, bool daclPresent, IntPtr dacl, bool daclDefaulted);

		public void Start(string pipename)
		{
			PipeName = pipename;
			listenThread = new Thread(ListenForClients)
			{
				IsBackground = true
			};
			listenThread.Start();
			Running = true;
		}

		private void ListenForClients()
		{
			SECURITY_DESCRIPTOR sd = default(SECURITY_DESCRIPTOR);
			InitializeSecurityDescriptor(ref sd, 1u);
			SetSecurityDescriptorDacl(ref sd, daclPresent: true, IntPtr.Zero, daclDefaulted: false);
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf((object)sd));
			Marshal.StructureToPtr((object)sd, intPtr, fDeleteOld: false);
			SECURITY_ATTRIBUTES sECURITY_ATTRIBUTES = default(SECURITY_ATTRIBUTES);
			sECURITY_ATTRIBUTES.nLength = Marshal.SizeOf((object)sd);
			sECURITY_ATTRIBUTES.lpSecurityDescriptor = intPtr;
			sECURITY_ATTRIBUTES.bInheritHandle = 1;
			SECURITY_ATTRIBUTES sECURITY_ATTRIBUTES2 = sECURITY_ATTRIBUTES;
			IntPtr intPtr2 = Marshal.AllocCoTaskMem(Marshal.SizeOf((object)sECURITY_ATTRIBUTES2));
			Marshal.StructureToPtr((object)sECURITY_ATTRIBUTES2, intPtr2, fDeleteOld: false);
			while (true)
			{
				SafeFileHandle safeFileHandle = CreateNamedPipe(PipeName, 1073741827u, 0u, 255u, 4096u, 4096u, 0u, intPtr2);
				if (!safeFileHandle.IsInvalid)
				{
					if (ConnectNamedPipe(safeFileHandle, IntPtr.Zero) == 0)
					{
						safeFileHandle.Close();
						continue;
					}
					Client client = new Client();
					client.handle = safeFileHandle;
					Client client2 = client;
					lock (clients)
					{
						clients.Add(client2);
					}
					Thread thread = new Thread(Read);
					thread.IsBackground = true;
					Thread thread2 = thread;
					thread2.Start(client2);
				}
			}
		}

		private void Read(object clientObj)
		{
			Client client = (Client)clientObj;
			client.stream = new FileStream(client.handle, FileAccess.ReadWrite, 4096, isAsync: true);
			byte[] array = new byte[4096];
			while (true)
			{
				int num = 0;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					try
					{
						if (client.stream.Read(array, 0, 4) != 0)
						{
							int num2 = BitConverter.ToInt32(array, 0);
							do
							{
								int num3 = client.stream.Read(array, 0, Math.Min(num2 - num, 4096));
								memoryStream.Write(array, 0, num3);
								num += num3;
							}
							while (num < num2);
							goto IL_0087;
						}
					}
					catch
					{
					}
					goto end_IL_0032;
					IL_0087:
					if (num != 0)
					{
						if (this.MessageReceived != null)
						{
							this.MessageReceived(memoryStream.ToArray());
						}
						continue;
					}
					end_IL_0032:;
				}
				break;
			}
			lock (clients)
			{
				DisconnectNamedPipe(client.handle);
				client.stream.Close();
				client.handle.Close();
				clients.Remove(client);
			}
			if (this.ClientDisconnected != null)
			{
				this.ClientDisconnected();
			}
		}

		public void SendMessage(byte[] message)
		{
			lock (clients)
			{
				byte[] bytes = BitConverter.GetBytes(message.Length);
				foreach (Client client in clients)
				{
					client.stream.Write(bytes, 0, 4);
					client.stream.Write(message, 0, message.Length);
					client.stream.Flush();
				}
			}
		}
	}
}
