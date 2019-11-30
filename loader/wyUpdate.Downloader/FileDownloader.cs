using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using wyUpdate.Common;

namespace wyUpdate.Downloader
{
	public class FileDownloader
	{
		private const int BufferSize = 4096;

		private readonly Stopwatch sw = new Stopwatch();

		private long sentSinceLastCalc;

		private string downloadSpeed;

		private string url;

		private List<string> urlList = new List<string>();

		private readonly string destFolder;

		private bool waitingForResponse;

		public long Adler32;

		private readonly Adler32 downloadedAdler32 = new Adler32();

		public byte[] SignedSHA1Hash;

		public string PublicSignKey;

		public bool UseRelativeProgress;

		private readonly BackgroundWorker bw = new BackgroundWorker();

		public static WebProxy CustomProxy;

		private static readonly string[] units = new string[9]
		{
			"bytes",
			"KB",
			"MB",
			"GB",
			"TB",
			"PB",
			"EB",
			"ZB",
			"YB"
		};

		public string DownloadingTo
		{
			get;
			private set;
		}

		public event ProgressChangedHandler ProgressChanged;

		public FileDownloader(List<string> urls, string downloadfolder)
		{
			urlList = urls;
			destFolder = downloadfolder;
			bw.WorkerReportsProgress = true;
			bw.WorkerSupportsCancellation = true;
			bw.DoWork += bw_DoWork;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerCompleted += bw_RunWorkerCompleted;
		}

		private void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			if (urlList == null || urlList.Count == 0)
			{
				if (string.IsNullOrEmpty(url))
				{
					if (!bw.CancellationPending)
					{
						bw.ReportProgress(0, new object[5]
						{
							-1,
							-1,
							string.Empty,
							ProgressStatus.Failure,
							new Exception("No download urls are specified.")
						});
					}
					return;
				}
				urlList = new List<string>
				{
					url
				};
			}
			if (CustomProxy != null)
			{
				WebRequest.DefaultWebProxy = CustomProxy;
			}
			else
			{
				IWebProxy systemWebProxy = WebRequest.GetSystemWebProxy();
				if (systemWebProxy.Credentials == null)
				{
					systemWebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
				}
				WebRequest.DefaultWebProxy = systemWebProxy;
			}
			bool flag = true;
			Exception ex = null;
			foreach (string url2 in urlList)
			{
				ex = null;
				try
				{
					url = url2;
					BeginDownload();
					ValidateDownload();
				}
				catch (Exception ex2)
				{
					ex = ex2;
					if (!waitingForResponse)
					{
						flag = false;
					}
				}
				if (ex == null || bw.CancellationPending)
				{
					flag = false;
					break;
				}
			}
			if (flag && WebRequest.DefaultWebProxy != null)
			{
				WebRequest.DefaultWebProxy = null;
				foreach (string url3 in urlList)
				{
					ex = null;
					try
					{
						url = url3;
						BeginDownload();
						ValidateDownload();
					}
					catch (Exception ex3)
					{
						ex = ex3;
					}
					if (ex == null || bw.CancellationPending)
					{
						break;
					}
				}
			}
			if (bw.CancellationPending || ex != null)
			{
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
			object[] array = (object[])e.UserState;
			if (this.ProgressChanged != null)
			{
				this.ProgressChanged((int)array[0], (int)array[1], (string)array[2], (ProgressStatus)array[3], array[4]);
			}
		}

		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			bw.DoWork -= bw_DoWork;
			bw.ProgressChanged -= bw_ProgressChanged;
			bw.RunWorkerCompleted -= bw_RunWorkerCompleted;
		}

		public static void EnableLazySSL()
		{
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(OnCheckSSLCert));
		}

		private static bool OnCheckSSLCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		public void Cancel()
		{
			bw.CancelAsync();
		}

		public void Download()
		{
			if (PublicSignKey != null && SignedSHA1Hash == null)
			{
				bw_RunWorkerCompleted(null, null);
				this.ProgressChanged(-1, -1, string.Empty, ProgressStatus.Failure, new Exception("The update is not signed. All updates must be signed in order to be installed."));
			}
			else
			{
				bw.RunWorkerAsync();
			}
		}

		private void BeginDownload()
		{
			DownloadData downloadData = null;
			FileStream fileStream = null;
			try
			{
				sw.Start();
				waitingForResponse = true;
				downloadData = DownloadData.Create(url, destFolder);
				waitingForResponse = false;
				downloadedAdler32.Reset();
				DownloadingTo = downloadData.Filename;
				if (!File.Exists(DownloadingTo))
				{
					fileStream = File.Open(DownloadingTo, FileMode.Create, FileAccess.Write);
				}
				else
				{
					if (Adler32 != 0)
					{
						GetAdler32(DownloadingTo);
					}
					fileStream = File.Open(DownloadingTo, FileMode.Append, FileAccess.Write);
				}
				byte[] buffer = new byte[4096];
				sentSinceLastCalc = downloadData.StartPoint;
				do
				{
					int num;
					if ((num = downloadData.DownloadStream.Read(buffer, 0, 4096)) <= 0)
					{
						return;
					}
					if (bw.CancellationPending)
					{
						downloadData.Close();
						fileStream.Close();
						return;
					}
					downloadData.StartPoint += num;
					if (Adler32 != 0)
					{
						downloadedAdler32.Update(buffer, 0, num);
					}
					fileStream.Write(buffer, 0, num);
					calculateBps(downloadData.StartPoint, downloadData.TotalDownloadSize);
					if (!bw.CancellationPending)
					{
						bw.ReportProgress(0, new object[5]
						{
							UseRelativeProgress ? InstallUpdate.GetRelativeProgess(0, downloadData.PercentDone) : downloadData.PercentDone,
							downloadData.PercentDone,
							downloadSpeed,
							ProgressStatus.None,
							null
						});
					}
				}
				while (!bw.CancellationPending);
				downloadData.Close();
				fileStream.Close();
			}
			catch (UriFormatException innerException)
			{
				throw new Exception($"Could not parse the URL \"{url}\" - it's either malformed or is an unknown protocol.", innerException);
			}
			catch (Exception ex)
			{
				if (string.IsNullOrEmpty(DownloadingTo))
				{
					throw new Exception($"Error trying to save file: {ex.Message}", ex);
				}
				throw new Exception($"Error trying to save file \"{DownloadingTo}\": {ex.Message}", ex);
			}
			finally
			{
				downloadData?.Close();
				fileStream?.Close();
			}
		}

		private void calculateBps(long BytesReceived, long TotalBytes)
		{
			if (!(sw.Elapsed < TimeSpan.FromSeconds(2.0)))
			{
				sw.Stop();
				long num = BytesReceived - sentSinceLastCalc;
				double bytes = (double)num * 1000.0 / sw.Elapsed.TotalMilliseconds;
				downloadSpeed = BytesToString(BytesReceived, time: false) + " / " + ((TotalBytes == 0) ? "unknown" : BytesToString(TotalBytes, time: false)) + "   (" + BytesToString(bytes, time: true) + "/sec)";
				sentSinceLastCalc = BytesReceived;
				sw.Reset();
				sw.Start();
			}
		}

		private static string BytesToString(double bytes, bool time)
		{
			int num = 0;
			while (bytes >= 921.6)
			{
				bytes /= 1024.0;
				num++;
			}
			if (!time && num <= 0)
			{
				return $"{bytes} {units[num]}";
			}
			return $"{bytes:0.00} {units[num]}";
		}

		private void ValidateDownload()
		{
			if (bw.CancellationPending)
			{
				return;
			}
			if (Adler32 != 0 && Adler32 != downloadedAdler32.Value)
			{
				throw new Exception("The downloaded file \"" + Path.GetFileName(DownloadingTo) + "\" failed the Adler32 validation.");
			}
			if (PublicSignKey != null)
			{
				if (SignedSHA1Hash == null)
				{
					throw new Exception("The downloaded file \"" + Path.GetFileName(DownloadingTo) + "\" is not signed.");
				}
				byte[] array = null;
				try
				{
					using (FileStream inputStream = new FileStream(DownloadingTo, FileMode.Open, FileAccess.Read))
					{
						using (SHA1CryptoServiceProvider sHA1CryptoServiceProvider = new SHA1CryptoServiceProvider())
						{
							array = sHA1CryptoServiceProvider.ComputeHash(inputStream);
						}
					}
					RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
					rSACryptoServiceProvider.FromXmlString(PublicSignKey);
					RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
					rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
					if (!rSAPKCS1SignatureDeformatter.VerifySignature(array, SignedSHA1Hash))
					{
						throw new Exception("Verification failed.");
					}
				}
				catch (Exception ex)
				{
					string str = "The downloaded file \"" + Path.GetFileName(DownloadingTo) + "\" failed the signature validation: " + ex.Message;
					long length = new FileInfo(DownloadingTo).Length;
					str = str + "\r\n\r\nThis error is likely caused by a download that ended prematurely. Total size of the downloaded file: " + BytesToString(length, time: false);
					if ((double)length >= 921.6)
					{
						object obj = str;
						str = obj + " (" + length + " bytes).";
					}
					if (array != null)
					{
						str = str + "\r\n\r\nComputed SHA1 hash of the downloaded file: " + BitConverter.ToString(array);
					}
					throw new Exception(str);
				}
			}
		}

		private void GetAdler32(string fileName)
		{
			byte[] array = new byte[4096];
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				int num;
				do
				{
					num = fileStream.Read(array, 0, array.Length);
					downloadedAdler32.Update(array, 0, num);
				}
				while (!bw.CancellationPending && num > 0);
			}
		}
	}
}
