using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraWaitForm;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
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
                //tcpClient.Close();
                try
				{
                    IAsyncResult asyncResult = tcpClient.BeginConnect(host, port, null, null);
					asyncResult.AsyncWaitHandle.WaitOne(millisecondsTimeout);
					return tcpClient.Connected;
                    /*Ping ping = new Ping();
                    PingOptions poptions = new PingOptions
                    {
                        DontFragment = true
                    };
                    string data = string.Empty;
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    //int timeout = 500;

                    PingReply reply = ping.Send(IPAddress.Parse(host), millisecondsTimeout, buffer, poptions);
                    if (reply.Status == IPStatus.Success)
                        return true;
                    else
                        return false;*/

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
            FileReader.once_ahp = new AccessHelper(WorkPath + "data\\mapdata.accdb", "zbxh2012base518");
            string rand_GUID = tmpiffm.Node_GUID;
            if (NetConnect.state == "云服务")
            {
                string sql = "select PRANDTAB from RAND_ZSKOBJECT where ISDELETE = 0 and PGUID = '" + tmpiffm.Icon_GUID + "'";
                DataTable dt = FileReader.once_ahp.ExecuteDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    sql = "select PRAND from " + dt.Rows[0]["PRANDTAB"].ToString() + " where ISDELETE = 0 and UNITID = '" + tmpiffm.unitid + "' and PGUID = '" + tmpiffm.Node_GUID + "'";
                    dt = FileReader.once_ahp.ExecuteDataTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        rand_GUID = dt.Rows[0]["PRAND"].ToString();
                    }
                }
                FileReader.once_ahp.CloseConn();
            }

            if (NetConnect.state != "未连接")
			{
				pichelper.ClearDir(url, new string[1]
				{
					WorkPath + "picture\\" + tmpiffm.Node_GUID
				});
				SendMessage("正在从服务器下载照片...", 3333, Text);
                
				DataTable dataTable = pichelper.QueryData(url, new string[1]
				{
					"select PFNAME from M_ICONPHOTO_H0001Z000E00 where ISDELETE = 0 and UPGUID = '" + rand_GUID + "' and TDATATYPE = '照片'"
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
            while (fThread.ThreadState != System.Threading.ThreadState.Aborted)
            {
                Thread.Sleep(100);
            }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoWaitForm));
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();
            this.progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBarControl1
            // 
            this.progressBarControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressBarControl1.Location = new System.Drawing.Point(12, 84);
            this.progressBarControl1.Name = "progressBarControl1";
            this.progressBarControl1.Size = new System.Drawing.Size(381, 27);
            this.progressBarControl1.TabIndex = 0;
            // 
            // progressPanel1
            // 
            this.progressPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressPanel1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressPanel1.Appearance.Options.UseBackColor = true;
            this.progressPanel1.BarAnimationElementThickness = 2;
            this.progressPanel1.Caption = "请稍后";
            this.progressPanel1.Description = "加载中...";
            this.progressPanel1.Location = new System.Drawing.Point(12, 12);
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(381, 66);
            this.progressPanel1.TabIndex = 1;
            this.progressPanel1.Text = "progressPanel1";
            // 
            // InfoWaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(405, 126);
            this.ControlBox = false;
            this.Controls.Add(this.progressPanel1);
            this.Controls.Add(this.progressBarControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InfoWaitForm";
            this.Text = "下载照片";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InfoWaitForm_FormClosed);
            this.Shown += new System.EventHandler(this.InfoWaitForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();
            this.ResumeLayout(false);

		}
	}
}
