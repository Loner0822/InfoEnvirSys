using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Description;

namespace EnvirInfoSys
{
	public class Webservice
	{
		public static string InvokeWebservice(string url, string methodname, object[] args)
		{
			try
			{
				string text = "zbxh";
				WebClient webClient = new WebClient();
				Stream stream = webClient.OpenRead(url + "?WSDL");
				ServiceDescription serviceDescription = ServiceDescription.Read(stream);
				string name = serviceDescription.Services[0].Name;
				ServiceDescriptionImporter serviceDescriptionImporter = new ServiceDescriptionImporter();
				serviceDescriptionImporter.AddServiceDescription(serviceDescription, "", "");
				CodeNamespace codeNamespace = new CodeNamespace(text);
				CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
				codeCompileUnit.Namespaces.Add(codeNamespace);
				serviceDescriptionImporter.Import(codeNamespace, codeCompileUnit);
				CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
				ICodeCompiler codeCompiler = cSharpCodeProvider.CreateCompiler();
				CompilerParameters compilerParameters = new CompilerParameters();
				compilerParameters.GenerateExecutable = false;
				compilerParameters.GenerateInMemory = true;
				compilerParameters.ReferencedAssemblies.Add("System.dll");
				compilerParameters.ReferencedAssemblies.Add("System.XML.dll");
				compilerParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
				compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
				CompilerResults compilerResults = codeCompiler.CompileAssemblyFromDom(compilerParameters, codeCompileUnit);
				if (compilerResults.Errors.HasErrors)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (CompilerError error in compilerResults.Errors)
					{
						stringBuilder.Append(error.ToString());
						stringBuilder.Append(Environment.NewLine);
					}
					throw new Exception(stringBuilder.ToString());
				}
				Assembly compiledAssembly = compilerResults.CompiledAssembly;
				Type type = compiledAssembly.GetType(text + "." + name, throwOnError: true, ignoreCase: true);
				object obj = Activator.CreateInstance(type);
				MethodInfo method = type.GetMethod(methodname);
				return (string)method.Invoke(obj, args);
			}
			catch
			{
				return "error";
			}
		}

		public static bool Download(string url, string localfile, string FormName)
		{
			bool flag = false;
			long num = 0L;
			long httpLength = GetHttpLength(url);
			SendMessage(httpLength.ToString(), 1111, FormName);
			if (httpLength == 0)
			{
				return false;
			}
			FileStream fileStream;
			if (File.Exists(localfile))
			{
				try
				{
					fileStream = File.OpenWrite(localfile);
				}
				catch (Exception)
				{
					return false;
				}
				num = fileStream.Length;
				if (num >= httpLength)
				{
					return true;
				}
				fileStream.Seek(num, SeekOrigin.Current);
			}
			else
			{
				fileStream = new FileStream(localfile, FileMode.Create);
				num = 0L;
			}
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				if (num > 0)
				{
					httpWebRequest.AddRange((int)num);
				}
				Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
				byte[] array = new byte[512];
				int num2 = responseStream.Read(array, 0, array.Length);
				long num3 = num;
				while (num2 > 0)
				{
					num3 += num2;
					fileStream.Write(array, 0, num2);
					SendMessage(num3.ToString(), 2222, FormName);
					num2 = responseStream.Read(array, 0, array.Length);
				}
				fileStream.Close();
				responseStream.Close();
				flag = true;
			}
			catch (Exception)
			{
				fileStream.Close();
				flag = false;
			}
			return flag;
		}

		private static void SendMessage(string strText, int data, string FormName)
		{
			IntPtr hWnd = ImportFromDLL.FindWindow(null, FormName);
			IntPtr handle = Process.GetCurrentProcess().Handle;
			ImportFromDLL.COPYDATASTRUCT pcd = default(ImportFromDLL.COPYDATASTRUCT);
			pcd.cbData = 1000;
			pcd.lpData = strText;
			pcd.dwData = data;
			ImportFromDLL.SendMessage(hWnd, 74, handle, ref pcd);
		}

		private static long GetHttpLength(string url)
		{
			long result = 0L;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				if (httpWebResponse.StatusCode == HttpStatusCode.OK)
				{
					result = httpWebResponse.ContentLength;
				}
				httpWebResponse.Close();
				return result;
			}
			catch
			{
				return result;
			}
		}
	}
}
