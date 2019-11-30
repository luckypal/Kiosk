using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace wyUpdate.Common
{
	public class RegChange : ICloneable
	{
		private static ConstructorInfo SafeRegistryHandleConstructor;

		private static ConstructorInfo RegistryKeyConstructor;

		public RegistryValueKind RegValueKind;

		public string ValueName;

		public object ValueData;

		public string SubKey;

		internal RegBasekeys RegBasekey;

		public RegOperations RegOperation;

		public bool Is32BitKey;

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegOpenKeyEx(IntPtr hKey, string subKey, uint options, int sam, out IntPtr phkResult);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int RegCreateKeyEx(IntPtr hKey, string lpSubKey, int Reserved, string lpClass, int dwOptions, int samDesired, IntPtr lpSecurityAttributes, out IntPtr phkResult, out int lpdwDisposition);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int RegDeleteKeyEx(IntPtr hKey, string lpSubKey, int samDesired, int Reserved);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegQueryInfoKey(IntPtr hKey, string lpClass, int[] lpcbClass, IntPtr lpReserved_MustBeZero, ref int lpcSubKeys, int[] lpcbMaxSubKeyLen, int[] lpcbMaxClassLen, ref int lpcValues, int[] lpcbMaxValueNameLen, int[] lpcbMaxValueLen, int[] lpcbSecurityDescriptor, int[] lpftLastWriteTime);

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		private static extern int RegEnumKeyEx(IntPtr hKey, int dwIndex, StringBuilder lpName, out int lpcbName, int[] lpReserved, StringBuilder lpClass, int[] lpcbClass, long[] lpftLastWriteTime);

		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern int RegCloseKey(IntPtr hKey);

		private static RegistryKey CreateSubKey32(RegistryKey pParentKey, string pSubKeyName)
		{
			if (pParentKey != null)
			{
				IntPtr registryKeyHandle;
				IntPtr intPtr = registryKeyHandle = GetRegistryKeyHandle(pParentKey);
				if (!intPtr.Equals(IntPtr.Zero))
				{
					IntPtr phkResult;
					int lpdwDisposition;
					int num = RegCreateKeyEx(registryKeyHandle, pSubKeyName, 0, null, 0, 131615, IntPtr.Zero, out phkResult, out lpdwDisposition);
					switch (num)
					{
					case 5:
					case 1346:
						throw new SecurityException("WOW64 registry subkey creation failed. Security_RegistryPermission - you don't have permission to create the subkey \"" + pSubKeyName + "\"");
					default:
						throw new Exception("Creating WOW64 registry subkey \"" + pSubKeyName + "\" failed. Return code: " + num);
					case 0:
					{
						RegistryKey registryKey = PointerToRegistryKey(phkResult, pWritable: true);
						if (registryKey == null)
						{
							throw new Exception("Creating WOW64 registry subkey \"" + pSubKeyName + "\" failed. PointerToRegistryKey return null.");
						}
						return registryKey;
					}
					}
				}
			}
			throw new Exception("CreateSubKey32: Parent key is not open");
		}

		private static void DeleteSubKeyTree32(RegistryKey pParentKey, string pSubKeyName)
		{
			if (pParentKey != null)
			{
				IntPtr registryKeyHandle;
				IntPtr intPtr = registryKeyHandle = GetRegistryKeyHandle(pParentKey);
				if (!intPtr.Equals(IntPtr.Zero))
				{
					IntPtr intPtr2 = OpenSubKey32Ptr(registryKeyHandle, pSubKeyName);
					if (intPtr2 != IntPtr.Zero)
					{
						try
						{
							int num = InternalSubKeyCount(intPtr2);
							if (num > 0)
							{
								string[] array = InternalGetSubKeyNames(num, intPtr2);
								for (int i = 0; i < array.Length; i++)
								{
									DeleteSubKeyTreeInternal(intPtr2, array[i]);
								}
							}
						}
						finally
						{
							RegCloseKey(intPtr2);
						}
						int num2 = RegDeleteKeyEx(registryKeyHandle, pSubKeyName, 512, 0);
						if (num2 != 0)
						{
							Win32Error(num2, null);
						}
						return;
					}
					throw new ArgumentException("Arg_RegSubKeyAbsent");
				}
			}
			throw new Exception("DeleteSubKeyTree32: Parent key is not open");
		}

		private static int InternalSubKeyCount(IntPtr hkey)
		{
			int lpcSubKeys = 0;
			int lpcValues = 0;
			int num = RegQueryInfoKey(hkey, null, null, IntPtr.Zero, ref lpcSubKeys, null, null, ref lpcValues, null, null, null, null);
			if (num != 0)
			{
				Win32Error(num, null);
			}
			return lpcSubKeys;
		}

		private static string[] InternalGetSubKeyNames(int numSubKeys, IntPtr hkey)
		{
			string[] array = new string[numSubKeys];
			if (numSubKeys > 0)
			{
				StringBuilder stringBuilder = new StringBuilder(256);
				for (int i = 0; i < numSubKeys; i++)
				{
					int lpcbName = stringBuilder.Capacity;
					int num = RegEnumKeyEx(hkey, i, stringBuilder, out lpcbName, null, null, null, null);
					if (num != 0)
					{
						Win32Error(num, null);
					}
					array[i] = stringBuilder.ToString();
				}
			}
			return array;
		}

		private static void DeleteSubKeyTreeInternal(IntPtr hParentKey, string subkey)
		{
			IntPtr intPtr = OpenSubKey32Ptr(hParentKey, subkey);
			if (intPtr != IntPtr.Zero)
			{
				try
				{
					int num = InternalSubKeyCount(intPtr);
					if (num > 0)
					{
						string[] array = InternalGetSubKeyNames(num, intPtr);
						for (int i = 0; i < array.Length; i++)
						{
							DeleteSubKeyTreeInternal(intPtr, array[i]);
						}
					}
				}
				finally
				{
					RegCloseKey(intPtr);
				}
				int num2 = RegDeleteKeyEx(hParentKey, subkey, 512, 0);
				if (num2 != 0)
				{
					Win32Error(num2, null);
				}
				return;
			}
			throw new ArgumentException("Arg_RegSubKeyAbsent");
		}

		private static void Win32Error(int errorCode, string str)
		{
			switch (errorCode)
			{
			case 2:
				throw new IOException("Arg_RegKeyNotFound", errorCode);
			case 5:
				if (str != null)
				{
					throw new UnauthorizedAccessException("UnauthorizedAccess_RegistryKeyGeneric_Key: " + str);
				}
				throw new UnauthorizedAccessException();
			default:
				throw new IOException("Registry access failed - error code: " + errorCode, errorCode);
			}
		}

		private static IntPtr OpenSubKey32Ptr(IntPtr parentKeyHandle, string pSubKeyName)
		{
			IntPtr phkResult;
			switch (RegOpenKeyEx(parentKeyHandle, pSubKeyName, 0u, 131609, out phkResult))
			{
			case 5:
			case 1346:
				throw new SecurityException("Security_RegistryPermission - you don't have permission to open the subkey.");
			default:
				return IntPtr.Zero;
			case 0:
				return phkResult;
			}
		}

		private static RegistryKey OpenSubKey32(RegistryKey pParentKey, string pSubKeyName)
		{
			if (pParentKey != null)
			{
				IntPtr registryKeyHandle;
				IntPtr intPtr = registryKeyHandle = GetRegistryKeyHandle(pParentKey);
				if (!intPtr.Equals(IntPtr.Zero))
				{
					IntPtr intPtr2 = OpenSubKey32Ptr(registryKeyHandle, pSubKeyName);
					if (intPtr2 == IntPtr.Zero)
					{
						return null;
					}
					return PointerToRegistryKey(intPtr2, pWritable: false);
				}
			}
			throw new Exception("OpenSubKey: Parent key is not open");
		}

		private static IntPtr GetRegistryKeyHandle(RegistryKey pRegisteryKey)
		{
			Type type = Type.GetType("Microsoft.Win32.RegistryKey");
			FieldInfo field = type.GetField("hkey", BindingFlags.Instance | BindingFlags.NonPublic);
			SafeHandle safeHandle = (SafeHandle)field.GetValue(pRegisteryKey);
			return safeHandle.DangerousGetHandle();
		}

		private static RegistryKey PointerToRegistryKey(IntPtr hKey, bool pWritable)
		{
			if ((object)SafeRegistryHandleConstructor == null)
			{
				Type type = typeof(SafeHandleZeroOrMinusOneIsInvalid).Assembly.GetType("Microsoft.Win32.SafeHandles.SafeRegistryHandle");
				Type[] types = new Type[2]
				{
					typeof(IntPtr),
					typeof(bool)
				};
				SafeRegistryHandleConstructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
				Type[] types2 = new Type[2]
				{
					type,
					typeof(bool)
				};
				RegistryKeyConstructor = typeof(RegistryKey).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, types2, null);
			}
			if ((object)SafeRegistryHandleConstructor == null || (object)RegistryKeyConstructor == null)
			{
				throw new Exception("Failed to get the 'SafeHandle' or 'RegistryKey' constructor. Make sure wyUpdate.exe has full trust Code Access Security (CAS) and make sure you're not using wyUpdate with the \"supportedRuntime\" configuration.");
			}
			object obj = SafeRegistryHandleConstructor.Invoke(new object[2]
			{
				hKey,
				false
			});
			return (RegistryKey)RegistryKeyConstructor.Invoke(new object[2]
			{
				obj,
				pWritable
			});
		}

		public RegChange()
		{
		}

		public RegChange(RegOperations regOp, RegBasekeys regBase, string subKey, bool is32BitKey)
		{
			RegOperation = regOp;
			RegBasekey = regBase;
			SubKey = subKey;
			RegValueKind = RegistryValueKind.String;
			Is32BitKey = is32BitKey;
		}

		public RegChange(RegOperations regOp, RegBasekeys regBase, string subKey, bool is32BitKey, string valueName)
		{
			RegOperation = regOp;
			RegBasekey = regBase;
			SubKey = subKey;
			ValueName = valueName;
			RegValueKind = RegistryValueKind.String;
			Is32BitKey = is32BitKey;
		}

		public RegChange(RegOperations regOp, RegBasekeys regBase, string subKey, bool is32BitKey, string valueName, object valueData)
		{
			RegOperation = regOp;
			RegBasekey = regBase;
			SubKey = subKey;
			ValueData = valueData;
			ValueName = valueName;
			RegValueKind = RegistryValueKind.String;
			Is32BitKey = is32BitKey;
		}

		public RegChange(RegOperations regOp, RegBasekeys regBase, string subKey, bool is32BitKey, string valueName, object valueData, RegistryValueKind valueType)
		{
			RegOperation = regOp;
			RegBasekey = regBase;
			SubKey = subKey;
			ValueData = valueData;
			ValueName = valueName;
			RegValueKind = valueType;
			Is32BitKey = is32BitKey;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (RegOperation)
			{
			case RegOperations.CreateValue:
				stringBuilder.Append("Creating value ");
				break;
			case RegOperations.RemoveValue:
				stringBuilder.Append("Removing value ");
				break;
			case RegOperations.CreateKey:
				stringBuilder.Append("Creating subkey ");
				break;
			case RegOperations.RemoveKey:
				stringBuilder.Append("Removing subkey ");
				break;
			}
			if (RegOperation == RegOperations.CreateValue || RegOperation == RegOperations.RemoveValue)
			{
				if (!string.IsNullOrEmpty(ValueName))
				{
					stringBuilder.Append("\"" + ValueName + "\" in ");
				}
				else
				{
					stringBuilder.Append("\"(Default)\" in ");
				}
			}
			stringBuilder.Append(RegBasekey + "\\" + SubKey);
			return stringBuilder.ToString();
		}

		public object Clone()
		{
			return new RegChange(RegOperation, RegBasekey, SubKey, Is32BitKey, ValueName, ValueData, RegValueKind);
		}

		public void WriteToStream(Stream fs, bool embedBinaryData)
		{
			fs.WriteByte(142);
			WriteFiles.WriteInt(fs, 1, (int)RegOperation);
			WriteFiles.WriteInt(fs, 2, (int)RegBasekey);
			WriteFiles.WriteInt(fs, 3, (int)RegValueKind);
			WriteFiles.WriteDeprecatedString(fs, 4, SubKey);
			if (!string.IsNullOrEmpty(ValueName))
			{
				WriteFiles.WriteDeprecatedString(fs, 5, ValueName);
			}
			bool flag = !embedBinaryData && RegValueKind == RegistryValueKind.Binary && ValueData is string;
			if (flag)
			{
				fs.WriteByte(128);
			}
			if (RegOperation == RegOperations.CreateValue)
			{
				switch (RegValueKind)
				{
				case RegistryValueKind.Binary:
					if (flag)
					{
						WriteFiles.WriteDeprecatedString(fs, 7, (string)ValueData);
					}
					else if (embedBinaryData && RegValueKind == RegistryValueKind.Binary && ValueData is string)
					{
						WriteOutFile(fs, 7, (string)ValueData);
					}
					else
					{
						WriteFiles.WriteByteArray(fs, 7, (byte[])ValueData);
					}
					break;
				case RegistryValueKind.DWord:
					WriteFiles.WriteInt(fs, 7, (int)ValueData);
					break;
				case RegistryValueKind.QWord:
					WriteFiles.WriteLong(fs, 7, (long)ValueData);
					break;
				case RegistryValueKind.MultiString:
					WriteFiles.WriteDeprecatedString(fs, 7, MultiStringToString(ValueData));
					break;
				case RegistryValueKind.String:
				case RegistryValueKind.ExpandString:
					WriteFiles.WriteDeprecatedString(fs, 7, (string)ValueData);
					break;
				}
			}
			if (Is32BitKey)
			{
				fs.WriteByte(129);
			}
			fs.WriteByte(158);
		}

		private static void WriteOutFile(Stream fs, byte flag, string filename)
		{
			int value = (int)new FileInfo(filename).Length;
			byte[] array = new byte[4096];
			fs.WriteByte(flag);
			fs.Write(BitConverter.GetBytes(value), 0, 4);
			try
			{
				using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					int num;
					do
					{
						num = fileStream.Read(array, 0, array.Length);
						fs.Write(array, 0, num);
					}
					while (num > 0);
				}
			}
			catch (Exception innerException)
			{
				throw new Exception("The binary data failed to load from file " + filename, innerException);
			}
		}

		private static string MultiStringToString(object strs)
		{
			string[] array = strs as string[];
			if (array == null)
			{
				return (string)strs;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i]);
				if (i != stringBuilder.Length - 1)
				{
					stringBuilder.Append("\r\n");
				}
			}
			return stringBuilder.ToString();
		}

		public static RegChange ReadFromStream(Stream fs)
		{
			RegChange regChange = new RegChange();
			bool flag = false;
			byte b = (byte)fs.ReadByte();
			while (!ReadFiles.ReachedEndByte(fs, b, 158))
			{
				switch (b)
				{
				case 1:
					regChange.RegOperation = (RegOperations)ReadFiles.ReadInt(fs);
					break;
				case 2:
					regChange.RegBasekey = (RegBasekeys)ReadFiles.ReadInt(fs);
					break;
				case 3:
					regChange.RegValueKind = (RegistryValueKind)ReadFiles.ReadInt(fs);
					break;
				case 4:
					regChange.SubKey = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 5:
					regChange.ValueName = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 6:
					if (regChange.RegValueKind != RegistryValueKind.ExpandString && regChange.RegValueKind != RegistryValueKind.String)
					{
						regChange.RegValueKind = RegistryValueKind.String;
					}
					regChange.ValueData = ReadFiles.ReadDeprecatedString(fs);
					break;
				case 128:
					flag = true;
					break;
				case 129:
					regChange.Is32BitKey = true;
					break;
				case 7:
					switch (regChange.RegValueKind)
					{
					case RegistryValueKind.Binary:
						if (flag)
						{
							regChange.ValueData = ReadFiles.ReadDeprecatedString(fs);
						}
						else
						{
							regChange.ValueData = ReadFiles.ReadByteArray(fs);
						}
						break;
					case RegistryValueKind.DWord:
						regChange.ValueData = ReadFiles.ReadInt(fs);
						break;
					case RegistryValueKind.QWord:
						regChange.ValueData = ReadFiles.ReadLong(fs);
						break;
					case RegistryValueKind.String:
					case RegistryValueKind.ExpandString:
					case RegistryValueKind.MultiString:
						regChange.ValueData = ReadFiles.ReadDeprecatedString(fs);
						break;
					}
					break;
				default:
					ReadFiles.SkipField(fs, b);
					break;
				}
				b = (byte)fs.ReadByte();
			}
			return regChange;
		}

		public void ExecuteOperation()
		{
			ExecuteOperation(null);
		}

		public void ExecuteOperation(List<RegChange> rollbackRegistry)
		{
			switch (RegOperation)
			{
			case RegOperations.CreateValue:
				CreateValue(rollbackRegistry);
				break;
			case RegOperations.RemoveValue:
				DeleteRegistryValue(rollbackRegistry);
				break;
			case RegOperations.CreateKey:
				CreateKey(rollbackRegistry);
				break;
			case RegOperations.RemoveKey:
				DeleteRegistryKey(rollbackRegistry);
				break;
			}
		}

		private static object StringToMultiString(object str)
		{
			if (str is string[])
			{
				return str;
			}
			return ((string)str).Split(new string[1]
			{
				"\r\n"
			}, StringSplitOptions.RemoveEmptyEntries);
		}

		private void CreateValue(List<RegChange> rollbackRegistry)
		{
			if (rollbackRegistry != null)
			{
				RegChange regChange = new RegChange(RegOperations.CreateValue, RegBasekey, SubKey, Is32BitKey, ValueName);
				try
				{
					using (RegistryKey registryKey = ReturnOpenKey(SubKey))
					{
						regChange.RegValueKind = registryKey.GetValueKind(ValueName);
						regChange.ValueData = registryKey.GetValue(ValueName, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
					}
				}
				catch (Exception)
				{
					regChange.RegOperation = RegOperations.RemoveValue;
				}
				rollbackRegistry.Add(regChange);
			}
			if (rollbackRegistry != null)
			{
				BackupCreateKeyTree(rollbackRegistry);
			}
			using (RegistryKey registryKey2 = ReturnCreateKey())
			{
				registryKey2.SetValue(ValueName, (RegValueKind == RegistryValueKind.MultiString) ? StringToMultiString(ValueData) : ValueData, RegValueKind);
			}
		}

		private void DeleteRegistryValue(List<RegChange> rollbackRegistry)
		{
			RegistryKey registryKey = ReturnOpenKey(SubKey);
			object obj = null;
			try
			{
				obj = registryKey.GetValue(ValueName, null);
			}
			catch
			{
			}
			if (obj != null)
			{
				registryKey.Close();
				registryKey = ReturnCreateKey();
				registryKey.DeleteValue(ValueName);
				rollbackRegistry?.Add(new RegChange(RegOperations.CreateValue, RegBasekey, SubKey, Is32BitKey, ValueName, obj));
			}
			registryKey.Close();
		}

		private RegistryKey ReturnCreateKey()
		{
			bool flag = Is32BitKey && IntPtr.Size == 8;
			switch (RegBasekey)
			{
			case RegBasekeys.HKEY_CLASSES_ROOT:
				if (flag)
				{
					return CreateSubKey32(Registry.ClassesRoot, SubKey);
				}
				return Registry.ClassesRoot.CreateSubKey(SubKey);
			case RegBasekeys.HKEY_CURRENT_CONFIG:
				if (flag)
				{
					return CreateSubKey32(Registry.CurrentConfig, SubKey);
				}
				return Registry.CurrentConfig.CreateSubKey(SubKey);
			case RegBasekeys.HKEY_LOCAL_MACHINE:
				if (flag)
				{
					return CreateSubKey32(Registry.LocalMachine, SubKey);
				}
				return Registry.LocalMachine.CreateSubKey(SubKey);
			case RegBasekeys.HKEY_USERS:
				if (flag)
				{
					return CreateSubKey32(Registry.Users, SubKey);
				}
				return Registry.Users.CreateSubKey(SubKey);
			default:
				if (flag)
				{
					return CreateSubKey32(Registry.CurrentUser, SubKey);
				}
				return Registry.CurrentUser.CreateSubKey(SubKey);
			}
		}

		private RegistryKey ReturnOpenKey(string skey)
		{
			bool flag = Is32BitKey && IntPtr.Size == 8;
			switch (RegBasekey)
			{
			case RegBasekeys.HKEY_CLASSES_ROOT:
				if (flag)
				{
					return OpenSubKey32(Registry.ClassesRoot, skey);
				}
				return Registry.ClassesRoot.OpenSubKey(skey);
			case RegBasekeys.HKEY_CURRENT_CONFIG:
				if (flag)
				{
					return OpenSubKey32(Registry.CurrentConfig, skey);
				}
				return Registry.CurrentConfig.OpenSubKey(skey);
			case RegBasekeys.HKEY_LOCAL_MACHINE:
				if (flag)
				{
					return OpenSubKey32(Registry.LocalMachine, skey);
				}
				return Registry.LocalMachine.OpenSubKey(skey);
			case RegBasekeys.HKEY_USERS:
				if (flag)
				{
					return OpenSubKey32(Registry.Users, skey);
				}
				return Registry.Users.OpenSubKey(skey);
			default:
				if (flag)
				{
					return OpenSubKey32(Registry.CurrentUser, skey);
				}
				return Registry.CurrentUser.OpenSubKey(skey);
			}
		}

		private void CreateKey(List<RegChange> rollbackRegistry)
		{
			if (!SubkeyExists(SubKey))
			{
				if (rollbackRegistry != null)
				{
					BackupCreateKeyTree(rollbackRegistry);
				}
				RegistryKey registryKey = ReturnCreateKey();
				registryKey.Close();
			}
		}

		private void BackupCreateKeyTree(List<RegChange> rollbackRegistry)
		{
			char[] separator = new char[2]
			{
				'/',
				'\\'
			};
			string[] array = SubKey.Split(separator, StringSplitOptions.RemoveEmptyEntries);
			string text = "";
			string[] array2 = array;
			int num = 0;
			while (true)
			{
				if (num < array2.Length)
				{
					string str = array2[num];
					text = text + str + "\\";
					if (!SubkeyExists(text))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			rollbackRegistry.Add(new RegChange(RegOperations.RemoveKey, RegBasekey, text, Is32BitKey));
		}

		private bool SubkeyExists(string subkey)
		{
			using (RegistryKey registryKey = ReturnOpenKey(subkey))
			{
				return registryKey != null;
			}
		}

		private void DeleteRegistryKey(List<RegChange> rollbackRegistry)
		{
			bool flag = Is32BitKey && IntPtr.Size == 8;
			bool flag2 = true;
			if (rollbackRegistry != null)
			{
				flag2 = BackupDeleteKeyTree(SubKey, rollbackRegistry);
			}
			if (flag2)
			{
				try
				{
					switch (RegBasekey)
					{
					case RegBasekeys.HKEY_CLASSES_ROOT:
						if (flag)
						{
							DeleteSubKeyTree32(Registry.ClassesRoot, SubKey);
						}
						else
						{
							Registry.ClassesRoot.DeleteSubKeyTree(SubKey);
						}
						break;
					case RegBasekeys.HKEY_CURRENT_CONFIG:
						if (flag)
						{
							DeleteSubKeyTree32(Registry.CurrentConfig, SubKey);
						}
						else
						{
							Registry.CurrentConfig.DeleteSubKeyTree(SubKey);
						}
						break;
					case RegBasekeys.HKEY_CURRENT_USER:
						if (flag)
						{
							DeleteSubKeyTree32(Registry.CurrentUser, SubKey);
						}
						else
						{
							Registry.CurrentUser.DeleteSubKeyTree(SubKey);
						}
						break;
					case RegBasekeys.HKEY_LOCAL_MACHINE:
						if (flag)
						{
							DeleteSubKeyTree32(Registry.LocalMachine, SubKey);
						}
						else
						{
							Registry.LocalMachine.DeleteSubKeyTree(SubKey);
						}
						break;
					case RegBasekeys.HKEY_USERS:
						if (flag)
						{
							DeleteSubKeyTree32(Registry.Users, SubKey);
						}
						else
						{
							Registry.Users.DeleteSubKeyTree(SubKey);
						}
						break;
					}
				}
				catch
				{
				}
			}
		}

		private bool BackupDeleteKeyTree(string subkey, List<RegChange> rollbackRegistry)
		{
			using (RegistryKey registryKey = ReturnOpenKey(subkey))
			{
				if (registryKey == null)
				{
					return false;
				}
				rollbackRegistry.Add(new RegChange(RegOperations.CreateKey, RegBasekey, subkey, Is32BitKey));
				string[] valueNames = registryKey.GetValueNames();
				string[] array = valueNames;
				foreach (string text in array)
				{
					rollbackRegistry.Add(new RegChange(RegOperations.CreateValue, RegBasekey, subkey, Is32BitKey, text, registryKey.GetValue(text, null, RegistryValueOptions.DoNotExpandEnvironmentNames), registryKey.GetValueKind(text)));
				}
				string[] subKeyNames = registryKey.GetSubKeyNames();
				string[] array2 = subKeyNames;
				foreach (string str in array2)
				{
					BackupDeleteKeyTree(subkey + "\\" + str, rollbackRegistry);
				}
			}
			return true;
		}
	}
}
