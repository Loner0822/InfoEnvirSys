using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraWaitForm;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace EnvirInfoSys
{
	public class InfoWaitForm : XtraForm
	{
		private string WorkPath = AppDomain.CurrentDomain.BaseDirectory;

		private Thread fThread = null;

		private PictureHelper pichelper = null;

		private InfoForm tmpiffm = null;

		private IContainer components = null;

		private ProgressBarControl progressBarControl1;

		private ProgressPanel progressPanel1;

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 74)
			{
				ImportFromDLL.COPYDATASTRUCT cOPYDATASTRUCT = (ImportFromDLL.COPYDATASTRUCT)m.GetLParam(typeof(ImportFromDLL.COPYDATASTRUCT));
				string lpData = cOPYDATASTRUCT.lpData;
				switch (cOPYDATASTRUCT.dwData)
				{
				case 1111:
					Initialize(int.Parse(lpData));
					break;
				case 2222:
					UpdateValue(int.Parse(lpData));
					break;
				case 3333:
					SetDescription(lpData);
					break;
				case 7777:
					Close();
					break;
				}
			}
			base.WndProc(ref m);
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

		public InfoWaitForm()
		{
			InitializeComponent();
		}

		private void SetCaption(string caption)
		{
			progressPanel1.Caption = caption;
		}

		private void SetDescription(string description)
		{
			progressPanel1.Description = description;
		}

		private void Initialize(int maxvalue)
		{
			progressBarControl1.Visible = true;
			progressBarControl1.Properties.Minimum = 0;
			progressBarControl1.Properties.Maximum = maxvalue;
			progressBarControl1.Properties.ProgressViewStyle = ProgressViewStyle.Solid;
			progressBarControl1.Position = 0;
			progressBarControl1.Properties.ShowTitle = true;
			progressBarControl1.Properties.PercentView = true;
		}

		private void UpdateValue(int value)
		{
			progressBarControl1.Position = value;
		}

		private void InfoWaitForm_Shown(object sender, EventArgs e)
		{
			pichelper = new PictureHelper();
			fThread = new Thread(GetPic);
			fThread.Start();
		}

		private bool TestServerConnection(string host, int port, int millisecondsTimeout)
		{
			using (TcpClient tcpClient = new TcpClient())
			{
				try
				{
					IAsyncResult asyncResult = tcpClient.BeginConnect(host, port, null, null);
					asyncResult.AsyncWaitHandle.WaitOne(millisecondsTimeout);
					return tcpClient.Connected;
				}
				catch (Exception)
				{
					return false;
				}
				finally
				{
					tcpClient.Close();
				}
			}
		}

		private void GetPic()
		{
			SendMessage("正在连接服务器...", 3333, Text);
			tmpiffm = (InfoForm)base.Owner;
			SendMessage("", 4444, tmpiffm.Text);
			string url = "http://" + NetConnect.ip + ":" + NetConnect.port + "/updataService.asmx";
			string str = "http://" + NetConnect.ip + ":" + NetConnect.port + "/downfile/H0001Z000E00/";
			if (NetConnect.ip == null)
			{
				XtraMessageBox.Show("请查看左下角网络连接状况");
				SendMessage("", 7777, Text);
			}
			if (TestServerConnection(NetConnect.ip, int.Parse(NetConnect.port), 500))
			{
				pichelper.ClearDir(url, new string[1]
				{
					WorkPath + "picture\\" + tmpiffm.Node_GUID
				});
				SendMessage("正在从服务器下载照片...", 3333, Text);
				DataTable dataTable = pichelper.QueryData(url, new string[1]
				{
					"select PFNAME from M_ICONPHOTO_H0001Z000E00 where ISDELETE = 0 and UPGUID = '" + tmpiffm.Node_GUID + "' and TDATATYPE = '照片'"
				});
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					string str2 = "(" + (i + 1).ToString() + "/" + dataTable.Rows.Count.ToString() + ")";
					SendMessage("正在从服务器下载照片..." + str2, 3333, Text);
					string fileNameWithoutExtension = dataTable.Rows[i]["PFNAME"].ToString();
					string[] parameter = new string[2]
					{
						WorkPath + "picture\\" + tmpiffm.Node_GUID + "\\",
                        dataTable.Rows[i]["PFNAME"].ToString()
                    };
					pichelper.DownloadPic(str + dataTable.Rows[i]["PFNAME"].ToString(), parameter, Text);
				}
			}
			else
			{
				SendMessage("连接服务器失败,即将从本地导入照片", 3333, Text);
				Thread.Sleep(1000);
			}
			SendMessage("正在导入照片...", 3333, Text);
			if (Directory.Exists(WorkPath + "picture\\" + tmpiffm.Node_GUID))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(WorkPath + "picture\\" + tmpiffm.Node_GUID);
				FileInfo[] files = directoryInfo.GetFiles();
				FileInfo[] array = files;
				foreach (FileInfo fileInfo in array)
				{
					SendMessage(fileInfo.FullName, 5555, tmpiffm.Text);
				}
			}
			SendMessage("", 6666, tmpiffm.Text);
			SendMessage("", 7777, Text);
		}

		private void InfoWaitForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			fThread.Abort();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
			progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
			((System.ComponentModel.ISupportInitialize)progressBarControl1.Properties).BeginInit();
			SuspendLayout();
			progressBarControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
			progressBarControl1.Location = new System.Drawing.Point(12, 84);
			progressBarControl1.Name = "progressBarControl1";
			progressBarControl1.Size = new System.Drawing.Size(381, 27);
			progressBarControl1.TabIndex = 0;
			progressPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
			progressPanel1.Appearance.BackColor = System.Drawing.Color.Transparent;
			progressPanel1.Appearance.Options.UseBackColor = true;
			progressPanel1.BarAnimationElementThickness = 2;
			progressPanel1.Caption = "请稍后";
			progressPanel1.Description = "加载中...";
			progressPanel1.Location = new System.Drawing.Point(12, 12);
			progressPanel1.Name = "progressPanel1";
			progressPanel1.Size = new System.Drawing.Size(381, 66);
			progressPanel1.TabIndex = 1;
			progressPanel1.Text = "progressPanel1";
			base.AutoScaleDimensions = new System.Drawing.SizeF(10f, 22f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			base.ClientSize = new System.Drawing.Size(405, 126);
			base.ControlBox = false;
			base.Controls.Add(progressPanel1);
			base.Controls.Add(progressBarControl1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.Name = "InfoWaitForm";
			Text = "下载照片";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(InfoWaitForm_FormClosed);
			base.Shown += new System.EventHandler(InfoWaitForm_Shown);
			((System.ComponentModel.ISupportInitialize)progressBarControl1.Properties).EndInit();
			ResumeLayout(performLayout: false);
		}
	}
}
