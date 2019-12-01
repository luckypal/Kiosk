using System.IO;
using System.Security.Cryptography;

namespace GLib
{
	public class Util
	{
		public static bool HashFile(string _file, bool _create)
		{
			if (!File.Exists(_file))
			{
				return false;
			}
			string path = _file + ".ver";
			HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider();
			byte[] array = null;
			using (FileStream inputStream = File.Open(_file, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				array = hashAlgorithm.ComputeHash(inputStream);
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] ^= 123;
			}
			if (_create)
			{
				FileStream fileStream = File.OpenWrite(path);
				fileStream.Write(array, 0, array.Length);
				fileStream.Close();
				return true;
			}
			byte[] array2 = new byte[array.Length];
			using (FileStream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				inputStream.Read(array2, 0, array.Length);
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array2[i] != array[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool Load_Text(string _file, out string _text)
		{
			_text = "";
			if (!File.Exists(_file))
			{
				return false;
			}
			try
			{
				_text = File.ReadAllText(_file);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool Save_Text(string _file, string _text)
		{
			if (File.Exists(_file))
			{
				try
				{
					File.Delete(_file);
				}
				catch
				{
					return false;
				}
			}
			try
			{
				File.WriteAllText(_file, _text);
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
