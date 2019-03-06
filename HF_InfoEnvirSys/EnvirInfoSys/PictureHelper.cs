using System;
using System.Data;
using System.IO;
using System.Xml;

namespace EnvirInfoSys
{
	public class PictureHelper
	{
		private string WorkPath = AppDomain.CurrentDomain.BaseDirectory;

		public DataTable QueryData(string url, string[] parameter)
		{
			string xmlString = Webservice.InvokeWebservice(url, "QueryData", parameter);
			return XmlToDataTable(xmlString);
		}

		public void DownloadPic(string url, string[] parameter, string FormName)
		{
			Webservice.Download(url, parameter[0] + parameter[1], FormName);
		}

		public void ClearDir(string url, string[] parameter)
		{
			string path = parameter[0];
			if (Directory.Exists(path))
			{
				string[] fileSystemEntries = Directory.GetFileSystemEntries(path);
				foreach (string path2 in fileSystemEntries)
				{
					if (File.Exists(path2))
					{
						File.Delete(path2);
					}
				}
			}
			else
			{
				Directory.CreateDirectory(path);
			}
		}

		public static DataTable XmlToDataTable(string xmlString)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (xmlString == "0")
			{
				return new DataTable();
			}
			xmlDocument.LoadXml(xmlString);
			StringReader stringReader = null;
			XmlTextReader xmlTextReader = null;
			try
			{
				DataSet dataSet = new DataSet();
				stringReader = new StringReader(xmlDocument.InnerXml);
				xmlTextReader = new XmlTextReader(stringReader);
				dataSet.ReadXml(xmlTextReader);
				xmlTextReader.Close();
				foreach (DataTable table in dataSet.Tables)
				{
					if (table != null)
					{
						return table;
					}
				}
			}
			catch (Exception ex)
			{
				xmlTextReader.Close();
				throw ex;
			}
			return null;
		}
	}
}
