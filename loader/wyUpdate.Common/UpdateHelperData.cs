using System;
using System.Collections.Generic;
using System.IO;

namespace wyUpdate.Common
{
	internal class UpdateHelperData
	{
		public UpdateAction Action;

		public UpdateStep UpdateStep;

		public List<string> ExtraData = new List<string>();

		public List<bool> ExtraDataIsRTF = new List<bool>();

		public Response ResponseType;

		public int Progress = -1;

		public int ProcessID;

		public UpdateHelperData()
		{
		}

		public UpdateHelperData(UpdateAction action)
		{
			Action = action;
		}

		public UpdateHelperData(UpdateStep step)
		{
			Action = UpdateAction.UpdateStep;
			UpdateStep = step;
		}

		public UpdateHelperData(Response responseType, UpdateStep step)
			: this(step)
		{
			ResponseType = responseType;
		}

		public UpdateHelperData(Response responseType, UpdateStep step, int progress)
			: this(responseType, step)
		{
			Progress = progress;
		}

		public UpdateHelperData(Response responseType, UpdateStep step, string messageTitle, string messageBody)
			: this(responseType, step)
		{
			ExtraData.Add(messageTitle);
			ExtraData.Add(messageBody);
			ExtraDataIsRTF.Add(item: false);
			ExtraDataIsRTF.Add(item: false);
		}

		public byte[] GetByteArray()
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				WriteFiles.WriteInt(memoryStream, 1, (int)Action);
				WriteFiles.WriteInt(memoryStream, 2, (int)UpdateStep);
				for (int i = 0; i < ExtraData.Count; i++)
				{
					if (!string.IsNullOrEmpty(ExtraData[i]))
					{
						if (ExtraDataIsRTF[i])
						{
							memoryStream.WriteByte(128);
						}
						WriteFiles.WriteString(memoryStream, 3, ExtraData[i]);
					}
				}
				if (ProcessID != 0)
				{
					WriteFiles.WriteInt(memoryStream, 4, ProcessID);
				}
				if (Progress > -1 && Progress <= 100)
				{
					WriteFiles.WriteInt(memoryStream, 5, Progress);
				}
				if (ResponseType != 0)
				{
					WriteFiles.WriteInt(memoryStream, 6, (int)ResponseType);
				}
				memoryStream.WriteByte(byte.MaxValue);
				return memoryStream.ToArray();
			}
		}

		public static UpdateHelperData FromByteArray(byte[] data)
		{
			UpdateHelperData updateHelperData = new UpdateHelperData();
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				byte b = (byte)memoryStream.ReadByte();
				while (!ReadFiles.ReachedEndByte(memoryStream, b, byte.MaxValue))
				{
					switch (b)
					{
					case 1:
						updateHelperData.Action = (UpdateAction)ReadFiles.ReadInt(memoryStream);
						break;
					case 2:
						updateHelperData.UpdateStep = (UpdateStep)ReadFiles.ReadInt(memoryStream);
						break;
					case 128:
						updateHelperData.ExtraDataIsRTF.Add(item: true);
						break;
					case 3:
						updateHelperData.ExtraData.Add(ReadFiles.ReadString(memoryStream));
						if (updateHelperData.ExtraDataIsRTF.Count != updateHelperData.ExtraData.Count)
						{
							updateHelperData.ExtraDataIsRTF.Add(item: false);
						}
						break;
					case 4:
						updateHelperData.ProcessID = ReadFiles.ReadInt(memoryStream);
						break;
					case 5:
						updateHelperData.Progress = ReadFiles.ReadInt(memoryStream);
						break;
					case 6:
						updateHelperData.ResponseType = (Response)ReadFiles.ReadInt(memoryStream);
						break;
					default:
						ReadFiles.SkipField(memoryStream, b);
						break;
					}
					b = (byte)memoryStream.ReadByte();
				}
				return updateHelperData;
			}
		}

		public static string PipenameFromFilename(string filename)
		{
			string text = filename.Replace("\\", "").ToLower();
			int length = text.Length;
			return "\\\\.\\pipe\\" + text.Substring(Math.Max(0, length - 246), Math.Min(246, length));
		}
	}
}
