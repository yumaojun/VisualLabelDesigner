using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VisualLabelDesigner.ZplTextEditor
{
	public class Labelary
	{
		public static Image LoadImageFromWebService(string zpl, string dpmm, string width, string height)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(zpl);
			WebRequest webRequest = WebRequest.Create(string.Format("http://api.labelary.com/v1/printers/{0}/labels/{1}x{2}/0/l", dpmm, width, height));
			webRequest.Proxy = new WebProxy();
			webRequest.Proxy = WebRequest.GetSystemWebProxy();
			webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
			HttpWebRequest httpWebRequest = (HttpWebRequest)webRequest;
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.ContentLength = (long)bytes.Length;
			Stream requestStream = httpWebRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			Image result;
			try
			{
				Stream responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
				FileStream fileStream = File.Create("label.png");
				responseStream.CopyTo(fileStream);
				responseStream.Close();
				result = Image.FromStream(fileStream);
				fileStream.Close();
			}
			catch (WebException ex)
			{
				string text = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
				if (string.IsNullOrEmpty(text))
				{
					throw new WebException(ex.Message);
				}
				result = ErrorImage.Generate(text);
			}
			return result;
		}

		/// <summary>
		/// 本地打印服务
		/// </summary>
		/// <param name="zpl"></param>
		/// <param name="dpmm"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static Image LoadImageFromLocal(string zpl, int dpmm, double width, double height)
		{
			Image result = null;
			try
			{
				//IPrinterStorage printerStorage = new PrinterStorage();
				//var drawer = new ZplElementDrawer(printerStorage);

				//var analyzer = new ZplAnalyzer(printerStorage);
				//var analyzeInfo = analyzer.Analyze(zpl);

				//var imageData = drawer.Draw(((LabelInfo)analyzeInfo.LabelInfos.GetValue(0)).ZplElements, width, height, dpmm);
				//var memStream = new MemoryStream(imageData);
				//result = Image.FromStream(memStream);
			}
			catch (WebException ex)
			{
				result = ErrorImage.Generate(ex.Message);
			}
			return result;
		}
	}
}
