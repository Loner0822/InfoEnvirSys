using MapHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Xml;

namespace PublishSys
{
	public class PubForm : Form
	{
		private AccessHelper ahp = null;

		private IniOperator inip = null;

		private string WorkPath = AppDomain.CurrentDomain.BaseDirectory;

		private string AccessPath = AppDomain.CurrentDomain.BaseDirectory + "Publish\\data\\PersonMange.mdb";

		private Dictionary<string, string> UnitID_Level;

		private string Last_Level = null;

		private IContainer components = null;

		private MenuStrip menuStrip1;

		private ToolStripMenuItem 导入单位ToolStripMenuItem;

		private OpenFileDialog openFileDialog1;

		private ToolStripMenuItem 数据管理ToolStripMenuItem;

		private ToolStripMenuItem 数据备份ToolStripMenuItem;

		private ToolStripMenuItem 数据恢复ToolStripMenuItem;

		private ToolStripMenuItem 系统设置ToolStripMenuItem;

		private ToolStripMenuItem iP设置ToolStripMenuItem;

		private ToolStripMenuItem 退出ToolStripMenuItem;

		private Panel panel2;

		private Splitter splitter1;

		private GroupBox groupBox2;

		private TreeView treeView1;

		private Panel panel1;

		private TextBox textBox3;

		private TextBox textBox2;

		private Label label3;

		private Label label2;

		private TextBox textBox1;

		private Button button2;

		private Label label1;

		private Button button1;

		private MapHelper.MapHelper mapHelper1;

		private ContextMenuStrip contextMenuStrip1;

		private ToolStripMenuItem 删除ToolStripMenuItem;

		private ToolStripMenuItem 数据同步ToolStripMenuItem;

		private GroupBox groupBox1;

		private DataGridView dataGridView1;

		private ToolStripMenuItem 数据上传ToolStripMenuItem;

		private ToolStripMenuItem 下载地图ToolStripMenuItem;

		private ToolStripMenuItem 下载图符ToolStripMenuItem;

		private ToolStripMenuItem toolStripMenuItem1;

		public PubForm()
		{
			InitializeComponent();
		}

		private void PubForm_Load(object sender, EventArgs e)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (File.Exists(WorkPath + "updateinfo.xml"))
			{
				xmlDocument.Load(WorkPath + "updateinfo.xml");
				XmlNode xmlNode = xmlDocument.SelectSingleNode("ZSKUPDATE").SelectSingleNode("CURVERSION");
				XmlNode xmlNode2 = xmlDocument.SelectSingleNode("ZSKUPDATE").SelectSingleNode("LASTVERSION");
				if (xmlNode.InnerText.CompareTo(xmlNode2.InnerText) < 0)
				{
					Process process = Process.Start(WorkPath + "zskUpdateOpr.exe");
					process.WaitForExit();
				}
			}
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn.Name = "ID";
			dataGridViewTextBoxColumn.DataPropertyName = "ID";
			dataGridViewTextBoxColumn.HeaderText = "序号";
			dataGridView1.Columns.Add(dataGridViewTextBoxColumn);
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn2.Name = "PubUnit";
			dataGridViewTextBoxColumn2.DataPropertyName = "PubUnit";
			dataGridViewTextBoxColumn2.HeaderText = "发布单位";
			dataGridView1.Columns.Add(dataGridViewTextBoxColumn2);
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn3.Name = "PubTime";
			dataGridViewTextBoxColumn3.DataPropertyName = "PubTime";
			dataGridViewTextBoxColumn3.HeaderText = "发布时间";
			dataGridView1.Columns.Add(dataGridViewTextBoxColumn3);
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn4.Name = "PubSys";
			dataGridViewTextBoxColumn4.DataPropertyName = "PubSys";
			dataGridViewTextBoxColumn4.HeaderText = "发布系统";
			dataGridView1.Columns.Add(dataGridViewTextBoxColumn4);
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn5.Name = "PubVer";
			dataGridViewTextBoxColumn5.DataPropertyName = "PubVer";
			dataGridViewTextBoxColumn5.HeaderText = "系统版本";
			dataGridView1.Columns.Add(dataGridViewTextBoxColumn5);
			DataGridViewTextBoxColumn dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn6.Name = "PubPGUID";
			dataGridViewTextBoxColumn6.DataPropertyName = "PubGUID";
			dataGridViewTextBoxColumn6.HeaderText = "GUID";
			dataGridViewTextBoxColumn6.Visible = false;
			dataGridView1.Columns.Add(dataGridViewTextBoxColumn6);
			dataGridView1.Columns[0].Width = 40;
			dataGridView1.Columns[1].Width = 100;
			dataGridView1.Columns[2].Width = 100;
			dataGridView1.Columns[3].Width = 160;
			dataGridView1.Columns[4].Width = 80;
			dataGridView1.Columns[5].Width = 0;
			ahp = new AccessHelper(WorkPath + "data\\PublishData.mdb");
			string sql = "select * from ENVIR_PUBLISH_H0001Z000E00 where ISDELETE = 0 order by S_UDTIME";
			DataTable dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				DataGridViewRow dataGridViewRow = new DataGridViewRow();
				dataGridViewRow.CreateCells(dataGridView1);
				dataGridViewRow.Cells[0].Value = i + 1;
				dataGridViewRow.Cells[1].Value = dataTable.Rows[i]["UNITNAME"].ToString();
				dataGridViewRow.Cells[2].Value = dataTable.Rows[i]["S_UDTIME"].ToString();
				dataGridViewRow.Cells[3].Value = dataTable.Rows[i]["UNITNAME"].ToString() + "区域经济大数据平台";
				dataGridViewRow.Cells[4].Value = dataTable.Rows[i]["VERSION"].ToString();
				dataGridViewRow.Cells[5].Value = dataTable.Rows[i]["PGUID"].ToString();
				dataGridView1.Rows.Add(dataGridViewRow);
			}
			ahp.CloseConn();
			button1.Enabled = false;
			button2.Enabled = false;
			textBox2.Enabled = false;
			textBox3.Enabled = false;
			treeView1.HideSelection = false;
			treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
			treeView1.DrawNode += treeView1_DrawNode;
			inip = new IniOperator(WorkPath + "PackUp.ini");
			Last_Level = inip.ReadString("unitlevel", "lastlevel", "");
			Last_Level = Last_Level.Replace("\0", "");
			Get_TreeView();
		}

		private void Get_TreeView()
		{
			if (File.Exists(AccessPath))
			{
				UnitID_Level = new Dictionary<string, string>();
				ahp = new AccessHelper(AccessPath);
				string sql = "select * from RG_单位注册 where ISDELETE = 0";
				DataTable dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				List<District> list = new List<District>();
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					string text = dataTable.Rows[i]["PGUID"].ToString();
					string pid = dataTable.Rows[i]["UPPGUID"].ToString();
					string name = dataTable.Rows[i]["ORGNAME"].ToString();
					string text2 = dataTable.Rows[i]["ULEVEL"].ToString();
					UnitID_Level.Add(text, text2);
					list.Add(new District(text, pid, text2, name));
				}
				ahp.CloseConn();
				Add_Tree_Node(list);
			}
		}

		private void Add_Tree_Node(List<District> d_list)
		{
			for (int i = 0; i < d_list.Count; i++)
			{
				if (d_list[i].pid == "0")
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Tag = d_list[i].id;
					treeNode.Text = d_list[i].name;
					treeView1.Nodes.Add(treeNode);
					Add_Child_Node(d_list, treeNode);
					if (d_list[i].level == "国" || d_list[i].level == "省" || d_list[i].level == "市" || d_list[i].level == "县")
					{
						treeNode.Expand();
					}
				}
			}
			if (treeView1.Nodes.Count > 0)
			{
				treeView1.SelectedNode = treeView1.Nodes[0];
			}
		}

		private void Add_Child_Node(List<District> d_list, TreeNode pNode)
		{
			for (int i = 0; i < d_list.Count; i++)
			{
				if (!(d_list[i].pid == pNode.Tag.ToString()))
				{
					continue;
				}
				TreeNode treeNode = new TreeNode();
				treeNode.Tag = d_list[i].id;
				treeNode.Text = d_list[i].name;
				pNode.Nodes.Add(treeNode);
				if (!(d_list[i].level == Last_Level))
				{
					Add_Child_Node(d_list, treeNode);
					if (d_list[i].level == "国" || d_list[i].level == "省" || d_list[i].level == "市" || d_list[i].level == "县")
					{
						pNode.Expand();
					}
				}
			}
		}

		private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			button2.Enabled = true;
		}

		private void 下载地图ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "mapdownload\\imaps.exe");
			process.WaitForExit();
		}

		private void 下载图符ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "Publish\\IconDataDown.exe", "-1 1 2");
			process.WaitForExit();
		}

		private bool Check_File(string file)
		{
			if (file == "")
			{
				return false;
			}
			if (file.IndexOf("PersonMange") < 0)
			{
				return false;
			}
			return true;
		}

		private void 导入单位ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "Publish\\DataUp.exe", "-1 1 2");
			process.WaitForExit();
			treeView1.Nodes.Clear();
			Get_TreeView();
		}

		private void 数据备份ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "DataBF.exe");
			process.WaitForExit();
		}

		private void 数据恢复ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "DataHF.exe");
			process.WaitForExit();
		}

		private void iP设置ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "SetIP.exe");
			process.WaitForExit();
		}

		private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ahp.CloseConn();
			Environment.Exit(0);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = treeView1.SelectedNode;
			if (textBox2.Text == "")
			{
				MessageBox.Show("请填写当前单位经度!");
				textBox2.Focus();
				return;
			}
			if (textBox3.Text == "")
			{
				MessageBox.Show("请填写当前单位纬度!");
				textBox3.Focus();
				return;
			}
			inip = new IniOperator(WorkPath + "Publish\\parameter.ini");
			inip.WriteString("mapproperties", "centerlng", textBox2.Text);
			inip.WriteString("mapproperties", "centerlat", textBox3.Text);
			MapForm mapForm = new MapForm();
			mapForm.unitid = selectedNode.Tag.ToString();
			mapForm.Text = "地图对应";
			mapForm.ShowDialog();
		}

		private void button2_Click(object sender, EventArgs e)
		{
            TreeNode selectedNode = treeView1.SelectedNode;
            if (selectedNode == null)
                return;
			inip = new IniOperator(WorkPath + "Publish\\RegInfo.ini");
			if (MessageBox.Show("即将发布《" + selectedNode.Text + "区域经济大数据平台" + textBox1.Text + "》", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}

            string levellist = Get_Level_List(selectedNode.Tag.ToString());
            Process process = Process.Start(WorkPath + "Publish\\DownOrgMapByBorder.exe", selectedNode.Tag.ToString() + " " + levellist + " 1");// 单位guid，级别逗号隔开，1 删除下载，0 不删就下
            process.WaitForExit();
            if (process.ExitCode == 1)
            {
                MessageBox.Show("地图获取失败!");
                return;
            }

            inip.WriteString("Public", "UnitName", selectedNode.Text);
			inip.WriteString("Public", "UnitLevel", UnitID_Level[selectedNode.Tag.ToString()]);
			inip.WriteString("Public", "UnitID", selectedNode.Tag.ToString());
			string text = selectedNode.Tag.ToString();
			ahp = new AccessHelper(WorkPath + "Publish\\data\\PersonMange.mdb");
			string sql = "select UPPGUID, ULEVEL from RG_单位注册 where ISDELETE = 0 and PGUID = '" + selectedNode.Tag.ToString() + "'";
			DataTable dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				while (dataTable.Rows[0]["ULEVEL"].ToString() != "县")
				{
					text = dataTable.Rows[0]["UPPGUID"].ToString();
					sql = "select UPPGUID, ULEVEL from RG_单位注册 where ISDELETE = 0 and PGUID = '" + text + "'";
					dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
					if (dataTable.Rows.Count <= 0)
					{
						MessageBox.Show("读取单位列表时出错!");
						return;
					}
				}
				ahp.CloseConn();
				inip.WriteString("Public", "UnitCounty", text);
				inip.WriteString("Public", "AppName", "区域经济大数据平台");
				inip.WriteString("版本号", "VerNum", textBox1.Text);
				inip = new IniOperator(WorkPath + "PackUp.ini");
				inip.WriteString("packup", "my_app_name", "区域经济大数据平台");
				inip.WriteString("packup", "my_app_version", textBox1.Text);
				inip.WriteString("packup", "my_app_publisher", selectedNode.Text);
				inip.WriteString("packup", "my_app_exe_name", "LoginForm.exe");
				inip.WriteString("packup", "my_app_id", "{" + Guid.NewGuid().ToString("B"));
				inip.WriteString("packup", "source_exe_path", WorkPath + "Publish\\EnvirInfoSys.exe");
				inip.WriteString("packup", "source_path", WorkPath + "Publish");
				inip.WriteString("packup", "registry_subkey", "区域经济大数据平台");
				ahp = new AccessHelper(WorkPath + "Publish\\data\\PASSWORD_H0001Z000E00.mdb");
				sql = "select PGUID from PASSWORD_H0001Z000E00 where ISDELETE = 0 and PWNAME = '管理员密码' and UNITID = '" + selectedNode.Tag.ToString() + "'";
				dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					sql = "update PASSWORD_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', PWMD5 = '95d565ef66e7dff9' where PWNAME = '管理员密码' and UNITID = '" + selectedNode.Tag.ToString() + "'";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				else
				{
					sql = "insert into PASSWORD_H0001Z000E00 (PGUID, S_UDTIME, PWNAME, PWMD5, UNITID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '管理员密码', '95d565ef66e7dff9', '" + selectedNode.Tag.ToString() + "')";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				sql = "select PGUID from PASSWORD_H0001Z000E00 where ISDELETE = 0 and PWNAME = '编辑模式' and UNITID = '" + selectedNode.Tag.ToString() + "'";
				dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					sql = "update PASSWORD_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', PWMD5 = 'a0b923820dcc509a' where PWNAME = '编辑模式' and UNITID = '" + selectedNode.Tag.ToString() + "'";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				else
				{
					sql = "insert into PASSWORD_H0001Z000E00 (PGUID, S_UDTIME, PWNAME, PWMD5, UNITID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '编辑模式', 'a0b923820dcc509a', '" + selectedNode.Tag.ToString() + "')";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				sql = "select PGUID from PASSWORD_H0001Z000E00 where ISDELETE = 0 and PWNAME = '查看模式' and UNITID = '" + selectedNode.Tag.ToString() + "'";
				dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					sql = "update PASSWORD_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', PWMD5 = '9d4c2f636f067f89' where PWNAME = '查看模式' and UNITID = '" + selectedNode.Tag.ToString() + "'";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				else
				{
					sql = "insert into PASSWORD_H0001Z000E00 (PGUID, S_UDTIME, PWNAME, PWMD5, UNITID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '查看模式', '9d4c2f636f067f89', '" + selectedNode.Tag.ToString() + "')";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				ahp.CloseConn();
				ahp = new AccessHelper(WorkPath + "Publish\\data\\ENVIR_H0001Z000E00.mdb");
				sql = "select PGUID from ENVIRGXFL_H0001Z000E00 where ISDELETE = 0 and UPGUID = '-1' and UNITID = '" + selectedNode.Tag.ToString() + "'";
				dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					sql = "update ENVIRGXFL_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', FLNAME = '管辖', SHOWINDEX = 0 where ISDELETE = 0 and UPGUID = '-1' and UNITID = '" + selectedNode.Tag.ToString() + "'";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				else
				{
					sql = "insert into ENVIRGXFL_H0001Z000E00 (PGUID, S_UDTIME, FLNAME, UNITID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '管辖', '" + selectedNode.Tag.ToString() + "')";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				ahp.CloseConn();
				ahp = new AccessHelper(WorkPath + "Publish\\data\\ZSK_AppInfo.mdb");
				sql = "update APPINFO set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', UNITID = '" + selectedNode.Tag.ToString() + "' where ISDELETE = 0 and PGUID = '{8C3B99C5-26D3-48B2-A676-250189FCEA2F}'";
				ahp.ExecuteSql(sql, (OleDbParameter[])null);
				ahp.CloseConn();
				ahp = new AccessHelper(WorkPath + "Publish\\data\\ENVIRLIST_H0001Z000E00.mdb");
				sql = "select PGUID from ENVIRLIST_H0001Z000E00 where ISDELETE = 0 and UNITID = '" + selectedNode.Tag.ToString() + "' and MARKERID = 'all'";
				dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count <= 0)
				{
					sql = "insert into ENVIRLIST_H0001Z000E00 (PGUID, S_UDTIME, FUNCNAME, FUNCTION, SHOWINDEX, UNITID, MARKERID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '基本信息', 'info', -2, '" + selectedNode.Tag.ToString() + "', 'all')";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
					sql = "insert into ENVIRLIST_H0001Z000E00 (PGUID, S_UDTIME, FUNCNAME, FUNCTION, SHOWINDEX, UNITID, MARKERID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '照片', 'pic', -1, '" + selectedNode.Tag.ToString() + "', 'all')";
					ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				ahp.CloseConn();
				process = Process.Start(WorkPath + "PackUp.exe");
				process.WaitForExit();
				if (process.ExitCode == -1)
				{
					MessageBox.Show("发布失败!");
					return;
				}
				string text2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				string text3 = Guid.NewGuid().ToString("B");
				ahp = new AccessHelper(WorkPath + "data\\PublishData.mdb");
				sql = "insert into ENVIR_PUBLISH_H0001Z000E00 (PGUID, S_UDTIME, UNITID, UNITNAME, VERSION, SYSTEMNAME) values ('" + text3 + "', '" + text2 + "', '" + selectedNode.Tag.ToString() + "', '" + selectedNode.Text + "', '" + textBox1.Text + "', '" + selectedNode.Text + "区域经济大数据平台')";
				ahp.ExecuteSql(sql, (OleDbParameter[])null);
				ahp.CloseConn();
				MessageBox.Show("发布成功!");
				int count = dataGridView1.Rows.Count;
				DataGridViewRow dataGridViewRow = new DataGridViewRow();
				dataGridViewRow.CreateCells(dataGridView1);
				dataGridViewRow.Cells[0].Value = count + 1;
				dataGridViewRow.Cells[1].Value = selectedNode.Text;
				dataGridViewRow.Cells[2].Value = text2;
				dataGridViewRow.Cells[3].Value = selectedNode.Text + "区域经济大数据平台";
				dataGridViewRow.Cells[4].Value = textBox1.Text;
				dataGridViewRow.Cells[5].Value = text3;
				dataGridView1.Rows.Add(dataGridViewRow);
			}
			else
			{
				MessageBox.Show("读取单位列表时出错!");
			}
		}

        private string Get_Level_List(string unitid)
        {
            string lvlist = "";
            ahp = new AccessHelper(WorkPath + "Publish\\data\\ENVIRDYDATA_H0001Z000E00.mdb");
            string sql = "select MAPLEVEL from MAPDUIYING_H0001Z000E00 where ISDELETE = 0 and UNITEID = '" + unitid + "'";
            DataTable dt = ahp.ExecuteDataTable(sql);
            ahp.CloseConn();
            for (int i = 0; i < dt.Rows.Count; ++i)
                lvlist += dt.Rows[i]["MAPLEVEL"].ToString() + ",";
            List<string>tmp = new List<string>(lvlist.Split(','));
            List<int> tmp_num = new List<int>();
            for (int i = 0; i < tmp.Count; ++i)
                if (tmp[i] != "")
                    tmp_num.Add(int.Parse(tmp[i]));
            tmp_num.Sort();
            tmp_num = tmp_num.Distinct().ToList();

            lvlist = "";
            for (int i = 0; i < tmp_num.Count; ++i)
            {
                if (tmp_num[i] != 0)
                    lvlist += tmp_num[i].ToString() + ",";
            }
            return lvlist.Substring(0, lvlist.Length - 1);
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

		private void textBox2_Leave(object sender, EventArgs e)
		{
			TreeNode selectedNode = treeView1.SelectedNode;
			ahp = new AccessHelper(WorkPath + "Publish\\data\\经纬度注册.mdb");
			string sql = "select PGUID from ORGCENTERDATA where ISDELETE = 0 and UNITEID = '" + selectedNode.Tag.ToString() + "'";
			DataTable dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				sql = "update ORGCENTERDATA set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', LNG = '" + textBox2.Text + "' where ISDELETE = 0 and UNITEID = '" + selectedNode.Tag.ToString() + "'";
				ahp.ExecuteSql(sql, (OleDbParameter[])null);
			}
			else
			{
				sql = "insert into ORGCENTERDATA (PGUID, S_UDTIME, UNITEID, LNG) values('" + selectedNode.Tag.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + selectedNode.Tag.ToString() + "', '" + textBox2.Text + "')";
				ahp.ExecuteSql(sql, (OleDbParameter[])null);
			}
			ahp.CloseConn();
		}

		private void textBox3_Leave(object sender, EventArgs e)
		{
			TreeNode selectedNode = treeView1.SelectedNode;
			ahp = new AccessHelper(WorkPath + "Publish\\data\\经纬度注册.mdb");
			string sql = "select PGUID from ORGCENTERDATA where ISDELETE = 0 and UNITEID = '" + selectedNode.Tag.ToString() + "'";
			DataTable dataTable = ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				sql = "update ORGCENTERDATA set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', LAT = '" + textBox3.Text + "' where ISDELETE = 0 and UNITEID = '" + selectedNode.Tag.ToString() + "'";
				ahp.ExecuteSql(sql, (OleDbParameter[])null);
			}
			else
			{
				sql = "insert into ORGCENTERDATA (PGUID, S_UDTIME, UNITEID, LAT) values('" + selectedNode.Tag.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + selectedNode.Tag.ToString() + "', '" + textBox3.Text + "')";
				ahp.ExecuteSql(sql, (OleDbParameter[])null);
			}
			ahp.CloseConn();
		}

		private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ahp = new AccessHelper(WorkPath + "data\\PublishData.mdb");
			string empty = string.Empty;
			string empty2 = string.Empty;
			for (int num = dataGridView1.SelectedRows.Count - 1; num >= 0; num--)
			{
				int index = dataGridView1.SelectedRows[num].Index;
				empty = dataGridView1.Rows[index].Cells[5].Value.ToString();
				empty2 = "update ENVIR_PUBLISH_H0001Z000E00 set ISDELETE = 1, S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ISDELETE = 0 and PGUID = '" + empty + "'";
				ahp.ExecuteSql(empty2);
				dataGridView1.Rows.Remove(dataGridView1.SelectedRows[num]);
			}
			ahp.CloseConn();
			for (int num = 0; num < dataGridView1.Rows.Count; num++)
			{
				dataGridView1.Rows[num].Cells[0].Value = num + 1;
			}
		}

		private void 数据同步ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "DataUP.exe", "PublishSys.exe 0 2");
			process.WaitForExit();
		}

		private void 数据上传ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "DataUP.exe", "PublishSys.exe 1 1");
			process.WaitForExit();
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Process process = Process.Start(WorkPath + "Publish\\YUNDataUp.exe", "0 1 1");
			process.WaitForExit();
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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(PublishSys.PubForm));
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			下载地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			下载图符ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			导入单位ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			数据管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			数据备份ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			数据恢复ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			数据同步ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			数据上传ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			系统设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			iP设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			panel2 = new System.Windows.Forms.Panel();
			groupBox2 = new System.Windows.Forms.GroupBox();
			treeView1 = new System.Windows.Forms.TreeView();
			panel1 = new System.Windows.Forms.Panel();
			textBox3 = new System.Windows.Forms.TextBox();
			textBox2 = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			textBox1 = new System.Windows.Forms.TextBox();
			button2 = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			button1 = new System.Windows.Forms.Button();
			mapHelper1 = new MapHelper.MapHelper();
			splitter1 = new System.Windows.Forms.Splitter();
			groupBox1 = new System.Windows.Forms.GroupBox();
			dataGridView1 = new System.Windows.Forms.DataGridView();
			contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
			删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			menuStrip1.SuspendLayout();
			panel2.SuspendLayout();
			groupBox2.SuspendLayout();
			panel1.SuspendLayout();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			contextMenuStrip1.SuspendLayout();
			SuspendLayout();
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7]
			{
				下载地图ToolStripMenuItem,
				下载图符ToolStripMenuItem,
				导入单位ToolStripMenuItem,
				toolStripMenuItem1,
				数据管理ToolStripMenuItem,
				系统设置ToolStripMenuItem,
				退出ToolStripMenuItem
			});
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new System.Drawing.Size(1924, 32);
			menuStrip1.TabIndex = 0;
			menuStrip1.Text = "menuStrip1";
			下载地图ToolStripMenuItem.Name = "下载地图ToolStripMenuItem";
			下载地图ToolStripMenuItem.Size = new System.Drawing.Size(94, 28);
			下载地图ToolStripMenuItem.Text = "下载地图";
			下载地图ToolStripMenuItem.Visible = false;
			下载地图ToolStripMenuItem.Click += new System.EventHandler(下载地图ToolStripMenuItem_Click);
			下载图符ToolStripMenuItem.Name = "下载图符ToolStripMenuItem";
			下载图符ToolStripMenuItem.Size = new System.Drawing.Size(94, 28);
			下载图符ToolStripMenuItem.Text = "下载图符";
			下载图符ToolStripMenuItem.Visible = false;
			下载图符ToolStripMenuItem.Click += new System.EventHandler(下载图符ToolStripMenuItem_Click);
			导入单位ToolStripMenuItem.Name = "导入单位ToolStripMenuItem";
			导入单位ToolStripMenuItem.Size = new System.Drawing.Size(130, 28);
			导入单位ToolStripMenuItem.Text = "下载基础数据";
			导入单位ToolStripMenuItem.Click += new System.EventHandler(导入单位ToolStripMenuItem_Click);
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(256, 28);
			toolStripMenuItem1.Text = "上传基础数据到企业网服务器";
			toolStripMenuItem1.Click += new System.EventHandler(toolStripMenuItem1_Click);
			数据管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4]
			{
				数据备份ToolStripMenuItem,
				数据恢复ToolStripMenuItem,
				数据同步ToolStripMenuItem,
				数据上传ToolStripMenuItem
			});
			数据管理ToolStripMenuItem.Name = "数据管理ToolStripMenuItem";
			数据管理ToolStripMenuItem.Size = new System.Drawing.Size(94, 28);
			数据管理ToolStripMenuItem.Text = "数据管理";
			数据备份ToolStripMenuItem.Name = "数据备份ToolStripMenuItem";
			数据备份ToolStripMenuItem.Size = new System.Drawing.Size(152, 28);
			数据备份ToolStripMenuItem.Text = "数据备份";
			数据备份ToolStripMenuItem.Click += new System.EventHandler(数据备份ToolStripMenuItem_Click);
			数据恢复ToolStripMenuItem.Name = "数据恢复ToolStripMenuItem";
			数据恢复ToolStripMenuItem.Size = new System.Drawing.Size(152, 28);
			数据恢复ToolStripMenuItem.Text = "数据恢复";
			数据恢复ToolStripMenuItem.Click += new System.EventHandler(数据恢复ToolStripMenuItem_Click);
			数据同步ToolStripMenuItem.Name = "数据同步ToolStripMenuItem";
			数据同步ToolStripMenuItem.Size = new System.Drawing.Size(152, 28);
			数据同步ToolStripMenuItem.Text = "数据同步";
			数据同步ToolStripMenuItem.Click += new System.EventHandler(数据同步ToolStripMenuItem_Click);
			数据上传ToolStripMenuItem.Name = "数据上传ToolStripMenuItem";
			数据上传ToolStripMenuItem.Size = new System.Drawing.Size(152, 28);
			数据上传ToolStripMenuItem.Text = "数据上传";
			数据上传ToolStripMenuItem.Click += new System.EventHandler(数据上传ToolStripMenuItem_Click);
			系统设置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1]
			{
				iP设置ToolStripMenuItem
			});
			系统设置ToolStripMenuItem.Name = "系统设置ToolStripMenuItem";
			系统设置ToolStripMenuItem.Size = new System.Drawing.Size(94, 28);
			系统设置ToolStripMenuItem.Text = "系统设置";
			iP设置ToolStripMenuItem.Name = "iP设置ToolStripMenuItem";
			iP设置ToolStripMenuItem.Size = new System.Drawing.Size(132, 28);
			iP设置ToolStripMenuItem.Text = "IP设置";
			iP设置ToolStripMenuItem.Click += new System.EventHandler(iP设置ToolStripMenuItem_Click);
			退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
			退出ToolStripMenuItem.Size = new System.Drawing.Size(58, 28);
			退出ToolStripMenuItem.Text = "退出";
			退出ToolStripMenuItem.Click += new System.EventHandler(退出ToolStripMenuItem_Click);
			openFileDialog1.Filter = "(*.mdb)|*.mdb";
			panel2.Controls.Add(groupBox2);
			panel2.Controls.Add(splitter1);
			panel2.Controls.Add(groupBox1);
			panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			panel2.Location = new System.Drawing.Point(0, 32);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(1924, 645);
			panel2.TabIndex = 7;
			groupBox2.Controls.Add(treeView1);
			groupBox2.Controls.Add(panel1);
			groupBox2.Controls.Add(mapHelper1);
			groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			groupBox2.Location = new System.Drawing.Point(0, 0);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(788, 645);
			groupBox2.TabIndex = 10;
			groupBox2.TabStop = false;
			groupBox2.Text = "单位列表";
			treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			treeView1.Location = new System.Drawing.Point(3, 24);
			treeView1.Name = "treeView1";
			treeView1.Size = new System.Drawing.Size(782, 473);
			treeView1.TabIndex = 3;
			treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(treeView1_AfterSelect);
			panel1.Controls.Add(textBox3);
			panel1.Controls.Add(textBox2);
			panel1.Controls.Add(label3);
			panel1.Controls.Add(label2);
			panel1.Controls.Add(textBox1);
			panel1.Controls.Add(button2);
			panel1.Controls.Add(label1);
			panel1.Controls.Add(button1);
			panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			panel1.Location = new System.Drawing.Point(3, 497);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(782, 145);
			panel1.TabIndex = 2;
			textBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
			textBox3.Location = new System.Drawing.Point(432, 27);
			textBox3.Name = "textBox3";
			textBox3.Size = new System.Drawing.Size(150, 28);
			textBox3.TabIndex = 8;
			textBox3.Visible = false;
			textBox3.Leave += new System.EventHandler(textBox3_Leave);
			textBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
			textBox2.Location = new System.Drawing.Point(210, 27);
			textBox2.Name = "textBox2";
			textBox2.Size = new System.Drawing.Size(150, 28);
			textBox2.TabIndex = 7;
			textBox2.Visible = false;
			textBox2.Leave += new System.EventHandler(textBox2_Leave);
			label3.Anchor = System.Windows.Forms.AnchorStyles.None;
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(364, 30);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(62, 18);
			label3.TabIndex = 6;
			label3.Text = "纬度：";
			label3.Visible = false;
			label2.Anchor = System.Windows.Forms.AnchorStyles.None;
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(78, 30);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(134, 18);
			label2.TabIndex = 5;
			label2.Text = "当前单位经度：";
			label2.Visible = false;
			textBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
			textBox1.Location = new System.Drawing.Point(306, 61);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(141, 28);
			textBox1.TabIndex = 4;
			textBox1.Text = "1.00.00";
			button2.Anchor = System.Windows.Forms.AnchorStyles.None;
			button2.Enabled = false;
			button2.Location = new System.Drawing.Point(453, 61);
			button2.Name = "button2";
			button2.Size = new System.Drawing.Size(100, 30);
			button2.TabIndex = 3;
			button2.Text = "发布系统";
			button2.UseVisualStyleBackColor = true;
			button2.Click += new System.EventHandler(button2_Click);
			label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(220, 66);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(80, 18);
			label1.TabIndex = 0;
			label1.Text = "版本号：";
			button1.Anchor = System.Windows.Forms.AnchorStyles.None;
			button1.Location = new System.Drawing.Point(599, 24);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(100, 30);
			button1.TabIndex = 2;
			button1.Text = "地图对应";
			button1.UseVisualStyleBackColor = true;
			button1.Visible = false;
			button1.Click += new System.EventHandler(button1_Click);
			mapHelper1.BackColor = System.Drawing.Color.Black;
			mapHelper1.centerlat = 0.0;
			mapHelper1.centerlng = 0.0;
			mapHelper1.iconspath = null;
			mapHelper1.Location = new System.Drawing.Point(892, 184);
			mapHelper1.maparr = null;
			mapHelper1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			mapHelper1.Name = "mapHelper1";
			mapHelper1.roadmappath = null;
			mapHelper1.satellitemappath = null;
			mapHelper1.Size = new System.Drawing.Size(221, 143);
			mapHelper1.TabIndex = 1;
			mapHelper1.Visible = false;
			mapHelper1.webpath = null;
			splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			splitter1.Location = new System.Drawing.Point(788, 0);
			splitter1.Name = "splitter1";
			splitter1.Size = new System.Drawing.Size(10, 645);
			splitter1.TabIndex = 1;
			splitter1.TabStop = false;
			groupBox1.Controls.Add(dataGridView1);
			groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
			groupBox1.Location = new System.Drawing.Point(798, 0);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(1126, 645);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "发布记录";
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.AllowUserToResizeRows = false;
			dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.ContextMenuStrip = contextMenuStrip1;
			dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			dataGridView1.Location = new System.Drawing.Point(3, 24);
			dataGridView1.Name = "dataGridView1";
			dataGridView1.ReadOnly = true;
			dataGridView1.RowHeadersVisible = false;
			dataGridView1.RowTemplate.Height = 30;
			dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			dataGridView1.Size = new System.Drawing.Size(1120, 618);
			dataGridView1.TabIndex = 0;
			contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1]
			{
				删除ToolStripMenuItem
			});
			contextMenuStrip1.Name = "contextMenuStrip1";
			contextMenuStrip1.Size = new System.Drawing.Size(117, 32);
			删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
			删除ToolStripMenuItem.Size = new System.Drawing.Size(116, 28);
			删除ToolStripMenuItem.Text = "删除";
			删除ToolStripMenuItem.Click += new System.EventHandler(删除ToolStripMenuItem_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 18f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoScroll = true;
			AutoSize = true;
			base.ClientSize = new System.Drawing.Size(1924, 677);
			base.Controls.Add(panel2);
			base.Controls.Add(menuStrip1);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MainMenuStrip = menuStrip1;
			base.Name = "PubForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "区域经济大数据平台发布";
			base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			base.Load += new System.EventHandler(PubForm_Load);
			menuStrip1.ResumeLayout(performLayout: false);
			menuStrip1.PerformLayout();
			panel2.ResumeLayout(performLayout: false);
			groupBox2.ResumeLayout(performLayout: false);
			panel1.ResumeLayout(performLayout: false);
			panel1.PerformLayout();
			groupBox1.ResumeLayout(performLayout: false);
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			contextMenuStrip1.ResumeLayout(performLayout: false);
			ResumeLayout(performLayout: false);
			PerformLayout();
		}
	}
}
