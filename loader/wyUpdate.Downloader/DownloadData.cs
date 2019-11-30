using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace wyUpdate.Downloader
{
	internal class DownloadData
	{
		private WebResponse response;

		private Stream stream;

		private long size;

		private long start;

		public string Filename;

		private static readonly List<char> invalidFilenameChars = new List<char>(Path.GetInvalidFileNameChars());

		public WebResponse Response
		{
			get
			{
				return response;
			}
			set
			{
				response = value;
			}
		}

		public Stream DownloadStream
		{
			get
			{
				if (start == size)
				{
					return Stream.Null;
				}
				return stream ?? (stream = response.GetResponseStream());
			}
		}

		public int PercentDone
		{
			get
			{
				if (size > 0)
				{
					return (int)(start * 100 / size);
				}
				return 0;
			}
		}

		public long TotalDownloadSize => size;

		public long StartPoint
		{
			get
			{
				return start;
			}
			set
			{
				start = value;
			}
		}

		public bool IsProgressKnown => size > -1;

		public static DownloadData Create(string url, string destFolder)
		{
			DownloadData downloadData = new DownloadData();
			WebRequest request = GetRequest(url);
			try
			{
				if (request is FtpWebRequest)
				{
					request.Method = "SIZE";
					downloadData.response = request.GetResponse();
					downloadData.GetFileSize();
					request = GetRequest(url);
					downloadData.response = request.GetResponse();
				}
				else
				{
					downloadData.response = request.GetResponse();
					downloadData.GetFileSize();
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error downloading \"{url}\": {ex.Message}", ex);
			}
			ValidateResponse(downloadData.response, url);
			string text = downloadData.response.Headers["Content-Disposition"];
			if (text != null)
			{
				int num = text.IndexOf("filename=", StringComparison.OrdinalIgnoreCase);
				if (num != -1)
				{
					num += 9;
					if (text.Length > num)
					{
						int num2 = text.IndexOf(';', num);
						num2 = ((num2 != -1) ? (num2 - num) : (text.Length - num));
						text = text.Substring(num, num2).Trim();
					}
					else
					{
						text = null;
					}
				}
				else
				{
					text = null;
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				text = Path.GetFileName(downloadData.response.ResponseUri.LocalPath);
			}
			if (!string.IsNullOrEmpty(text) && text.IndexOfAny(invalidFilenameChars.ToArray()) != -1)
			{
				StringBuilder stringBuilder = new StringBuilder(text.Length - 1);
				for (int i = 0; i < text.Length; i++)
				{
					if (invalidFilenameChars.IndexOf(text[i]) == -1)
					{
						stringBuilder.Append(text[i]);
					}
				}
				text = stringBuilder.ToString().Trim();
			}
			if (string.IsNullOrEmpty(text))
			{
				text = Path.GetFileName(Path.GetTempFileName());
			}
			string text2 = downloadData.Filename = Path.Combine(destFolder, text);
			if (!downloadData.IsProgressKnown && File.Exists(text2))
			{
				File.Delete(text2);
			}
			if (downloadData.IsProgressKnown && File.Exists(text2))
			{
				if (!(downloadData.Response is HttpWebResponse))
				{
					File.Delete(text2);
				}
				else
				{
					downloadData.start = new FileInfo(text2).Length;
					if (downloadData.start > downloadData.size)
					{
						File.Delete(text2);
					}
					else if (downloadData.start < downloadData.size)
					{
						downloadData.response.Close();
						request = GetRequest(url);
						((HttpWebRequest)request).AddRange((int)downloadData.start);
						downloadData.response = request.GetResponse();
						if (((HttpWebResponse)downloadData.Response).StatusCode != HttpStatusCode.PartialContent)
						{
							File.Delete(text2);
							downloadData.start = 0L;
						}
					}
				}
			}
			return downloadData;
		}

		private static void ValidateResponse(WebResponse response, string url)
		{
			if (response is HttpWebResponse)
			{
				HttpWebResponse httpWebResponse = (HttpWebResponse)response;
				if (httpWebResponse.StatusCode == HttpStatusCode.NotFound || httpWebResponse.ContentType.Contains("text/html"))
				{
					throw new Exception($"Could not download \"{url}\" - a web page was returned from the web server.");
				}
			}
			else if (response is FtpWebResponse && ((FtpWebResponse)response).StatusCode == FtpStatusCode.ConnectionClosed)
			{
				throw new Exception($"Could not download \"{url}\" - FTP server closed the connection.");
			}
		}

		private void GetFileSize()
		{
			if (response != null)
			{
				try
				{
					size = response.ContentLength;
				}
				catch
				{
					size = -1L;
				}
			}
		}

		private static WebRequest GetRequest(string url)
		{
			UriBuilder uriBuilder = new UriBuilder(url);
			bool flag = !string.IsNullOrEmpty(uriBuilder.UserName) && !string.IsNullOrEmpty(uriBuilder.Password);
			if (flag && (uriBuilder.Scheme == Uri.UriSchemeHttp || uriBuilder.Scheme == Uri.UriSchemeHttps))
			{
				url = new UriBuilder(uriBuilder.Scheme, uriBuilder.Host, uriBuilder.Port, uriBuilder.Path, uriBuilder.Fragment).ToString();
			}
			WebRequest webRequest = WebRequest.Create(url);
			webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
			if (webRequest is HttpWebRequest)
			{
				webRequest.Credentials = (flag ? new NetworkCredential(uriBuilder.UserName, uriBuilder.Password) : CredentialCache.DefaultCredentials);
				((HttpWebRequest)webRequest).UserAgent = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 6.1; en-US; wyUpdate)";
			}
			return webRequest;
		}

		public void Close()
		{
			response.Close();
		}
	}
}
