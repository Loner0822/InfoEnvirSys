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
using System.Windows.Forms;

namespace PublishSys
{
	public class MapForm : Form
	{
		private IniOperator inip = null;

		private string WorkPath = AppDomain.CurrentDomain.BaseDirectory;

		private AccessHelper ahp1 = null;

		private AccessHelper ahp2 = null;

		private AccessHelper ahp3 = null;

		private AccessHelper ahp4 = null;

		private AccessHelper ahp5 = null;

		private AccessHelper ahp6 = null;

		public string unitid = "";

		private string[] folds = null;

		private string map_type = "g_map";

		private List<string> Prop_GUID;

		private Dictionary<string, string> Show_Name;

		private Dictionary<string, string> Show_FDName;

		private Dictionary<string, string> inherit_GUID;

		private Dictionary<string, string> Show_Value;

		private Dictionary<string, string> GUID_ICON = new Dictionary<string, string>();

		private Dictionary<string, object> borderDic = null;

		private List<double[]> borList = null;

		private Dictionary<string, string> GL_NAME;

		private Dictionary<string, string> GL_JDCODE;

		private Dictionary<string, string> GL_UPGUID;

		private Dictionary<string, string> GL_MAP;

		private string slat;

		private string slng;

		private IContainer components = null;

		private Panel panel1;

		private GroupBox groupBox1;

		private Splitter splitter1;

		private Panel panel2;

		private GroupBox groupBox2;

		private TreeView treeView1;

		private FolderBrowserDialog folderBrowserDialog1;

		private ContextMenuStrip contextMenuStrip1;

		private ToolStripMenuItem toolStripMenuItem2;

		private Timer timer1;

		private OpenFileDialog openFileDialog1;

		private TabControl tabControl1;

		private TabPage tabPage1;

		private GroupBox groupBox5;

		private Panel panel5;

		private MapHelper.MapHelper mapHelper1;

		private Splitter splitter3;

		private GroupBox groupBox6;

		private CheckedListBox checkedListBox1;

		private Panel panel8;

		private Button button3;

		private Label label4;

		private Label label3;

		private Label label2;

		private Label label1;

		private TextBox textBox3;

		private TextBox textBox2;

		private Button button1;

		private TextBox textBox1;

		private TabPage tabPage2;

		private GroupBox groupBox3;

		private GroupBox groupBox4;

		private TabControl tabControl2;

		private Splitter splitter2;

		private GroupBox groupBox7;

		private TabControl tabControl3;

		private TabPage tabPage3;

		private DataGridView dataGridView2;

		private TabPage tabPage4;

		private DataGridView dataGridView3;

		private FlowLayoutPanel flowLayoutPanel1;

		private ContextMenuStrip contextMenuStrip2;

		private ToolStripMenuItem 全选ToolStripMenuItem;

		private ContextMenuStrip contextMenuStrip3;

		private ToolStripMenuItem 清空ToolStripMenuItem;

		public MapForm()
		{
			InitializeComponent();
		}

		private void MapForm_Load(object sender, EventArgs e)
		{
			tabControl2.Controls.Clear();
			ahp2 = new AccessHelper(WorkPath + "Publish\\data\\ZSK_H0001Z000K00.mdb");
			string sql = "select UPGUID, PROPVALUE from ZSK_PROP_H0001Z000K00 where ISDELETE = 0 and PROPNAME = '图符库' order by PROPVALUE, SHOWINDEX";
			DataTable dataTable = ahp2.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["PROPVALUE"].ToString();
				string pguid = dataTable.Rows[i]["UPGUID"].ToString();
				int num = text.IndexOf("图符库");
				if (num < 0)
				{
					continue;
				}
				text = text.Substring(0, num);
				if (text == "备用")
				{
					continue;
				}
				bool flag = false;
				for (int j = 0; j < tabControl2.TabPages.Count; j++)
				{
					if (tabControl2.TabPages[j].Name == text)
					{
						flag = true;
						FlowLayoutPanel flp = (FlowLayoutPanel)tabControl2.TabPages[j].Controls[0];
						Add_Icon(flp, pguid);
					}
				}
				if (!flag)
				{
					tabControl2.TabPages.Add(text);
					num = tabControl2.TabPages.Count - 1;
					FlowLayoutPanel flp = new FlowLayoutPanel();
					flp.Dock = DockStyle.Fill;
					flp.FlowDirection = FlowDirection.LeftToRight;
					flp.WrapContents = true;
					flp.AutoScroll = true;
					flp.MouseDown += dataGridView_MouseDown;
					Add_Icon(flp, pguid);
					tabControl2.TabPages[num].Name = text;
					tabControl2.TabPages[num].BackColor = SystemColors.Control;
					tabControl2.TabPages[num].Controls.Add(flp);
				}
			}
			ahp2.CloseConn();
		}

		private void MapForm_Shown(object sender, EventArgs e)
		{
			ahp1 = new AccessHelper(WorkPath + "Publish\\data\\ENVIR_H0001Z000E00.mdb");
			ahp2 = new AccessHelper(WorkPath + "Publish\\data\\ZSK_H0001Z000K00.mdb");
			ahp3 = new AccessHelper(WorkPath + "Publish\\data\\ZSK_H0001Z000K01.mdb");
			ahp4 = new AccessHelper(WorkPath + "Publish\\data\\ZSK_H0001Z000E00.mdb");
			ahp5 = new AccessHelper(WorkPath + "Publish\\data\\经纬度注册.mdb");
			ahp6 = new AccessHelper(WorkPath + "Publish\\data\\ENVIRDYDATA_H0001Z000E00.mdb");
			checkedListBox1.Items.Clear();
			Load_Unit_Level();
			if (treeView1.Nodes.Count <= 0)
			{
				MessageBox.Show("未导入组织结构数据，即将关闭窗口");
				Close();
				return;
			}
			button3.Enabled = false;
			inip = new IniOperator(WorkPath + "Publish\\parameter.ini");
			textBox1.Text = inip.ReadString("mapproperties", unitid, "");
			textBox2.Text = inip.ReadString("mapproperties", "centerlng", "0");
			textBox3.Text = inip.ReadString("mapproperties", "centerlat", "0");
			if (textBox2.Text == "0" || textBox3.Text == "0")
			{
				MessageBox.Show("获取不到当前经纬度");
			}
			checkedListBox1.Items.Clear();
			if (textBox1.Text != string.Empty)
			{
				Show_Map_List(textBox1.Text);
			}
			borList = new List<double[]>();
			borderDic = new Dictionary<string, object>();
			borderDic.Add("type", "实线");
			borderDic.Add("width", 1);
			borderDic.Add("color", "#000000");
			borderDic.Add("opacity", 1);
			string sql = "select LAT, LNG from BORDERDATA where ISDELETE = 0 and UNITID = '" + unitid + "' order by SHOWINDEX";
			DataTable dataTable = ahp5.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				borList.Add(new double[2]
				{
					double.Parse(dataTable.Rows[i]["LAT"].ToString()),
					double.Parse(dataTable.Rows[i]["LNG"].ToString())
				});
			}
			if (dataTable.Rows.Count > 0)
			{
				borderDic.Add("path", borList);
			}
			else
			{
				borderDic = null;
			}
			if (treeView1.Nodes.Count > 0)
			{
				treeView1.SelectedNode = null;
				treeView1.SelectedNode = treeView1.Nodes[0];
			}
			tabControl3.TabPages[0].Parent = null;
			timer1.Enabled = true;
		}

		private void Add_Icon(FlowLayoutPanel flp, string pguid)
		{
			string str = WorkPath + "Publish\\ICONDER\\b_PNGICON\\";
			ucPictureBox ucPictureBox = new ucPictureBox();
			string sql = "select JDNAME from ZSK_OBJECT_H0001Z000K00 where ISDELETE = 0 and PGUID = '" + pguid + "'";
			DataTable dataTable = ahp2.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				ucPictureBox.Parent = flp;
				ucPictureBox.Name = pguid;
				ucPictureBox.IconName = dataTable.Rows[0]["JDNAME"].ToString();
				ucPictureBox.IconPguid = pguid;
				ucPictureBox.IconPath = str + pguid + ".png";
				ucPictureBox.Single_Click += Icon_SingleClick;
				ucPictureBox.Double_Click += Icon_DoubleClick;
				ucPictureBox.IconCheck = false;
			}
		}

		private void Show_Icon_List(string levelguid)
		{
			string str = WorkPath + "Publish\\ICONDER\\b_PNGICON\\";
			string sql = "select PGUID, JDNAME from ZSK_OBJECT_H0001Z000K00 where ISDELETE = 0 order by LEVELNUM, SHOWINDEX";
			DataTable dataTable = ahp2.ExecuteDataTable(sql, (OleDbParameter[])null);
			string a = "";
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["PGUID"].ToString();
				string iconName = dataTable.Rows[i]["JDNAME"].ToString();
				if (!File.Exists(str + text + ".png"))
				{
					continue;
				}
				sql = "select PGUID from ICONDUIYING_H0001Z000E00 where ISDELETE = 0 and ICONGUID = '" + text + "' and LEVELGUID = '" + levelguid + "' and UNITEID = '" + unitid + "'";
				DataTable dataTable2 = ahp6.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable2.Rows.Count > 0)
				{
					if (a == "")
					{
						a = text;
					}
					ucPictureBox ucPictureBox = new ucPictureBox();
					ucPictureBox.Parent = flowLayoutPanel1;
					ucPictureBox.Name = text;
					ucPictureBox.IconName = iconName;
					ucPictureBox.IconPguid = text;
					ucPictureBox.IconPath = str + text + ".png";
					ucPictureBox.Single_Click += Icon_SingleClick;
					ucPictureBox.Double_Click += Icon_DoubleClick;
					ucPictureBox.IconCheck = false;
				}
			}
		}

		private void Icon_SingleClick(object sender, EventArgs e, string iconguid)
		{
			ucPictureBox ucPictureBox = (ucPictureBox)sender;
			if (!ucPictureBox.IconCheck)
			{
				if (ucPictureBox.Parent == flowLayoutPanel1)
				{
					foreach (ucPictureBox control in flowLayoutPanel1.Controls)
					{
						control.IconCheck = false;
					}
					ucPictureBox.IconCheck = true;
					Show_Icon_Property(iconguid);
				}
				else
				{
					FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)ucPictureBox.Parent;
					foreach (ucPictureBox control2 in flowLayoutPanel.Controls)
					{
						control2.IconCheck = false;
					}
					ucPictureBox.IconCheck = true;
				}
			}
		}

		private void Icon_DoubleClick(object sender, EventArgs e, string iconguid)
		{
			TreeNode selectedNode = treeView1.SelectedNode;
			if (selectedNode == null)
			{
				return;
			}
			string text = selectedNode.Tag.ToString();
			ucPictureBox ucPictureBox = (ucPictureBox)sender;
			if (ucPictureBox.Parent == flowLayoutPanel1)
			{
				Control value = ucPictureBox;
				flowLayoutPanel1.Controls.Remove(value);
				string sql = "update ICONDUIYING_H0001Z000E00 set ISDELETE = 1, S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ICONGUID = '" + iconguid + "' and LEVELGUID = '" + text + "' and UNITID = '" + unitid + "'";
				ahp6.ExecuteSql(sql, (OleDbParameter[])null);
			}
			else
			{
				foreach (ucPictureBox control in flowLayoutPanel1.Controls)
				{
					if (control.IconPguid == ucPictureBox.IconPguid)
					{
						MessageBox.Show("已添加该图符!");
						return;
					}
				}
				ucPictureBox ucPictureBox3 = new ucPictureBox();
				ucPictureBox3.IconName = ucPictureBox.IconName;
				ucPictureBox3.IconPguid = ucPictureBox.IconPguid;
				ucPictureBox3.IconPath = ucPictureBox.IconPath;
				ucPictureBox3.IconCheck = false;
				ucPictureBox3.Single_Click += Icon_SingleClick;
				ucPictureBox3.Double_Click += Icon_DoubleClick;
				flowLayoutPanel1.Controls.Add(ucPictureBox3);
				string sql = "select PGUID from ICONDUIYING_H0001Z000E00 where ICONGUID = '" + iconguid + "' and LEVELGUID = '" + text + "' and UNITID = '" + unitid + "'";
				DataTable dataTable = ahp6.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					sql = "update ICONDUIYING_H0001Z000E00 set ISDELETE = 0, S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ICONGUID = '" + iconguid + "' and LEVELGUID = '" + text + "' and UNITID = '" + unitid + "'";
					ahp6.ExecuteSql(sql, (OleDbParameter[])null);
				}
				else
				{
					sql = "insert into ICONDUIYING_H0001Z000E00 (PGUID, S_UDTIME, LEVELGUID, ICONGUID, UNITEID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + text + "', '" + iconguid + "', '" + unitid + "')";
					ahp6.ExecuteSql(sql, (OleDbParameter[])null);
				}
			}
			if (flowLayoutPanel1.Controls.Count > 0)
			{
				ucPictureBox ucPictureBox4 = (ucPictureBox)flowLayoutPanel1.Controls[0];
				string iconPguid = ucPictureBox4.IconPguid;
				Icon_SingleClick(flowLayoutPanel1.Controls[0], new EventArgs(), iconPguid);
			}
		}

		private void Show_Icon_Property(string iconguid)
		{
			string text = "";
			text = "{26E232C8-595F-44E5-8E0F-8E0FC1BD7D24}";
			Get_Property_Data(dataGridView2, iconguid, text);
			text = "{B55806E6-9D63-4666-B6EB-AAB80814648E}";
			Get_Property_Data(dataGridView3, iconguid, text);
		}

		private void Get_Property_Data(DataGridView dgv, string iconguid, string typeguid)
		{
			Prop_GUID = new List<string>();
			Show_Name = new Dictionary<string, string>();
			Show_FDName = new Dictionary<string, string>();
			inherit_GUID = new Dictionary<string, string>();
			Show_Value = new Dictionary<string, string>();
			dgv.Columns.Clear();
			string sql = "select PGUID, PROPNAME, FDNAME, SOURCEGUID, PROPVALUE from ZSK_PROP_H0001Z000K00 where ISDELETE = 0 and UPGUID = '" + iconguid + "' and PROTYPEGUID = '" + typeguid + "' order by SHOWINDEX";
			DataTable proptable = ahp2.ExecuteDataTable(sql, (OleDbParameter[])null);
			Add_Prop(proptable);
			sql = "select PGUID, PROPNAME, FDNAME, SOURCEGUID, PROPVALUE from ZSK_PROP_H0001Z000K01 where ISDELETE = 0 and UPGUID = '" + iconguid + "' and PROTYPEGUID = '" + typeguid + "' order by SHOWINDEX";
			proptable = ahp3.ExecuteDataTable(sql, (OleDbParameter[])null);
			Add_Prop(proptable);
			List<string> list = new List<string>();
			bool flag = false;
			for (int i = 0; i < Prop_GUID.Count; i++)
			{
				string text = Prop_GUID[i];
				flag = true;
				DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
				dataGridViewTextBoxColumn.Name = text;
				dataGridViewTextBoxColumn.HeaderText = Show_Name[text];
				dgv.Columns.Add(dataGridViewTextBoxColumn);
				list.Add(Show_Value[text]);
			}
			if (flag)
			{
				dgv.Rows.Add(list.ToArray());
			}
		}

		private void Add_Prop(DataTable proptable)
		{
			for (int i = 0; i < proptable.Rows.Count; i++)
			{
				Prop_GUID.Add(proptable.Rows[i]["PGUID"].ToString());
				Show_Name[proptable.Rows[i]["PGUID"].ToString()] = proptable.Rows[i]["PROPNAME"].ToString();
				Show_FDName[proptable.Rows[i]["PGUID"].ToString()] = proptable.Rows[i]["FDNAME"].ToString();
				inherit_GUID[proptable.Rows[i]["PGUID"].ToString()] = proptable.Rows[i]["SOURCEGUID"].ToString();
				Show_Value[proptable.Rows[i]["PGUID"].ToString()] = proptable.Rows[i]["PROPVALUE"].ToString();
			}
		}

		private void flowLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				contextMenuStrip3.Show(Control.MousePosition.X, Control.MousePosition.Y);
			}
		}

		private void dataGridView_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				contextMenuStrip2.Show(Control.MousePosition.X, Control.MousePosition.Y);
			}
		}

		private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = treeView1.SelectedNode;
			if (selectedNode != null)
			{
				string text = selectedNode.Tag.ToString();
				int selectedIndex = tabControl2.SelectedIndex;
				FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)tabControl2.TabPages[selectedIndex].Controls[0];
				foreach (ucPictureBox control in flowLayoutPanel.Controls)
				{
					bool flag = false;
					foreach (ucPictureBox control2 in flowLayoutPanel1.Controls)
					{
						if (control.IconPguid == control2.IconPguid)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						ucPictureBox ucPictureBox3 = new ucPictureBox();
						ucPictureBox3.IconName = control.IconName;
						ucPictureBox3.IconPguid = control.IconPguid;
						ucPictureBox3.IconPath = control.IconPath;
						ucPictureBox3.IconCheck = false;
						ucPictureBox3.Single_Click += Icon_SingleClick;
						ucPictureBox3.Double_Click += Icon_DoubleClick;
						flowLayoutPanel1.Controls.Add(ucPictureBox3);
						string sql = "select PGUID from ICONDUIYING_H0001Z000E00 where ICONGUID = '" + control.IconPguid + "' and LEVELGUID = '" + text + "' and UNITID = '" + unitid + "'";
						DataTable dataTable = ahp6.ExecuteDataTable(sql, (OleDbParameter[])null);
						if (dataTable.Rows.Count > 0)
						{
							sql = "update ICONDUIYING_H0001Z000E00 set ISDELETE = 0, S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ICONGUID = '" + control.IconPguid + "' and LEVELGUID = '" + text + "' and UNITID = '" + unitid + "'";
							ahp6.ExecuteSql(sql, (OleDbParameter[])null);
						}
						else
						{
							sql = "insert into ICONDUIYING_H0001Z000E00 (PGUID, S_UDTIME, LEVELGUID, ICONGUID, UNITEID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + text + "', '" + control.IconPguid + "', '" + unitid + "')";
							ahp6.ExecuteSql(sql, (OleDbParameter[])null);
						}
					}
				}
				if (flowLayoutPanel1.Controls.Count > 0)
				{
					ucPictureBox ucPictureBox4 = (ucPictureBox)flowLayoutPanel1.Controls[0];
					string iconPguid = ucPictureBox4.IconPguid;
					Icon_SingleClick(flowLayoutPanel1.Controls[0], new EventArgs(), iconPguid);
				}
			}
		}

		private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TreeNode selectedNode = treeView1.SelectedNode;
			if (selectedNode != null)
			{
				string text = selectedNode.Tag.ToString();
				foreach (ucPictureBox control in flowLayoutPanel1.Controls)
				{
					string sql = "update ICONDUIYING_H0001Z000E00 set ISDELETE = 1, S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ICONGUID = '" + control.IconPguid + "' and LEVELGUID = '" + text + "' and UNITID = '" + unitid + "'";
					ahp6.ExecuteSql(sql, (OleDbParameter[])null);
				}
				flowLayoutPanel1.Controls.Clear();
			}
		}

		private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabPage selectedTab = tabControl2.SelectedTab;
			FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)selectedTab.Controls[0];
			foreach (ucPictureBox control in flowLayoutPanel.Controls)
			{
				control.IconCheck = false;
			}
		}

		private void Delete_Path()
		{
			string text = WorkPath + "Publish\\googlemap\\map";
			string text2 = WorkPath + "Publish\\googlemap\\satellite";
			string text3 = "";
			if (Directory.Exists(text))
			{
				text3 = text3 + text + ",";
			}
			if (Directory.Exists(text2))
			{
				text3 += text2;
			}
			Process process = Process.Start(WorkPath + "DeleteDir.exe", text3);
			process.WaitForExit();
		}

		private void Load_Unit_Level()
		{
			GL_NAME = new Dictionary<string, string>();
			GL_JDCODE = new Dictionary<string, string>();
			GL_UPGUID = new Dictionary<string, string>();
			GL_MAP = new Dictionary<string, string>();
			string sql = "select PGUID, JDNAME, JDCODE, UPGUID from ZSK_OBJECT_H0001Z000K01 where ISDELETE = 0";
			DataTable dataTable = ahp3.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				GL_NAME[dataTable.Rows[i]["PGUID"].ToString()] = dataTable.Rows[i]["JDNAME"].ToString();
				GL_JDCODE[dataTable.Rows[i]["PGUID"].ToString()] = dataTable.Rows[i]["JDCODE"].ToString();
				GL_UPGUID[dataTable.Rows[i]["PGUID"].ToString()] = dataTable.Rows[i]["UPGUID"].ToString();
			}
			treeView1.Nodes.Clear();
			treeView1.HideSelection = false;
			treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
			treeView1.DrawNode += treeView1_DrawNode;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				if (dataTable.Rows[i]["UPGUID"].ToString() == string.Empty)
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Text = GL_NAME[dataTable.Rows[i]["PGUID"].ToString()];
					treeNode.Tag = dataTable.Rows[i]["PGUID"].ToString();
					treeView1.Nodes.Add(treeNode);
					Add_Unit_Node(treeNode);
				}
			}
			treeView1.ExpandAll();
		}

		private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
		{
			e.DrawDefault = true;
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode selectedNode = treeView1.SelectedNode;
			if (selectedNode == null)
			{
				return;
			}
			string text = selectedNode.Tag.ToString();
			flowLayoutPanel1.Controls.Clear();
			Show_Icon_List(text);
			for (int i = 0; i < checkedListBox1.Items.Count; i++)
			{
				if (checkedListBox1.GetItemChecked(i))
				{
					checkedListBox1.SetItemChecked(i, value: false);
				}
			}
			string sql = "select MAPLEVEL from MAPDUIYING_H0001Z000E00 where ISDELETE = 0 and LEVELGUID = '" + text + "' and UNITEID = '" + unitid + "'";
			DataTable dataTable = ahp6.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				string text2 = dataTable.Rows[0]["MAPLEVEL"].ToString();
				string[] array = text2.Split(',');
				for (int i = 0; i < array.Length; i++)
				{
					for (int j = 0; j < checkedListBox1.Items.Count; j++)
					{
						if (checkedListBox1.Items[j].ToString() == array[i])
						{
							checkedListBox1.SetItemChecked(j, value: true);
							break;
						}
					}
				}
			}
			int selectedIndex = checkedListBox1.SelectedIndex;
			if (selectedIndex >= 0)
			{
				mapHelper1.ShowMap(int.Parse(checkedListBox1.Items[selectedIndex].ToString()), checkedListBox1.Items[selectedIndex].ToString(), canEdit: false, map_type, null, null, null, 1.0, -1);
			}
		}

		private void Add_Unit_Node(TreeNode pa)
		{
			foreach (KeyValuePair<string, string> item in GL_UPGUID)
			{
				if (item.Value == pa.Tag.ToString())
				{
					TreeNode treeNode = new TreeNode();
					treeNode.Text = GL_NAME[item.Key];
					treeNode.Tag = item.Key;
					pa.Nodes.Add(treeNode);
					Add_Unit_Node(treeNode);
				}
			}
		}

		private void Show_Map_List(string tmp)
		{
			int num = tmp.LastIndexOf("roadmap");
			if (num < 0)
			{
				num = tmp.LastIndexOf("satellite");
			}
			if (num < 0)
			{
				num = tmp.LastIndexOf("satellite_en");
			}
			if (num > 0)
			{
				tmp = tmp.Substring(0, num);
			}
			textBox1.Text = tmp;
			checkedListBox1.Items.Clear();
			string path = textBox1.Text + "\\satellite_en";
			if (!Directory.Exists(path))
			{
				MessageBox.Show("请下载或导入混合图(无偏移)");
				textBox1.Text = "";
				return;
			}
			path = textBox1.Text + "\\roadmap";
			if (!Directory.Exists(path))
			{
				MessageBox.Show("请下载或导入街道图");
				textBox1.Text = "";
				return;
			}
			if (!Check_Map_LngLat())
			{
				MessageBox.Show("当前下载地图与经纬度不对应");
				textBox1.Text = "";
				return;
			}
			inip.WriteString("mapproperties", unitid, textBox1.Text);
			folds = Directory.GetDirectories(path);
			for (int i = 0; i < folds.Length; i++)
			{
				num = folds[i].LastIndexOf("\\");
				folds[i] = folds[i].Substring(num + 1);
			}
			Array.Sort(folds, new CompStr());
			for (int i = 0; i < folds.Length; i++)
			{
				checkedListBox1.Items.Add(folds[i]);
			}
			mapHelper1.centerlat = double.Parse(textBox3.Text);
			mapHelper1.centerlng = double.Parse(textBox2.Text);
			mapHelper1.webpath = WorkPath + "Publish\\googlemap";
			mapHelper1.roadmappath = tmp + "\\roadmap";
			mapHelper1.satellitemappath = tmp + "\\satellite_en";
			mapHelper1.iconspath = WorkPath + "Publish\\PNGICONFOLDER";
			mapHelper1.maparr = folds;
			if (checkedListBox1.Items.Count > 0)
			{
				checkedListBox1.SelectedIndex = 0;
			}
		}

		private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			button3.Enabled = true;
			int selectedIndex = checkedListBox1.SelectedIndex;
			mapHelper1.ShowMap(int.Parse(checkedListBox1.Items[selectedIndex].ToString()), "", canEdit: false, map_type, null, borderDic, null, 1.0, -1);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (textBox2.Text.Trim().Equals(""))
			{
				MessageBox.Show("请输入经度");
				textBox2.Focus();
				return;
			}
			if (textBox3.Text.Trim().Equals(""))
			{
				MessageBox.Show("请输入纬度");
				textBox3.Focus();
				return;
			}
			inip = new IniOperator(WorkPath + "Publish\\parameter.ini");
			folderBrowserDialog1.SelectedPath = inip.ReadString("mapproperties", unitid, "");
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				string selectedPath = folderBrowserDialog1.SelectedPath;
				Show_Map_List(selectedPath);
			}
		}

		private bool Check_Map_LngLat()
		{
			string text = textBox1.Text + "\\satellite_en";
			folds = Directory.GetDirectories(text);
			for (int i = 0; i < folds.Length; i++)
			{
				int num = folds[i].LastIndexOf("\\");
				folds[i] = folds[i].Substring(num + 1);
			}
			Array.Sort(folds, new CompStr());
			double num2 = double.Parse(textBox2.Text);
			double num3 = double.Parse(textBox3.Text);
			num2 = Math.Pow(2.0, int.Parse(folds[folds.Length - 1]) - 1) * (1.0 + num2 / 180.0);
			num3 = Math.Pow(2.0, int.Parse(folds[folds.Length - 1]) - 1) * (1.0 - Math.Log(Math.Tan(Math.PI * num3 / 180.0) + 1.0 / Math.Cos(Math.PI * num3 / 180.0)) / Math.PI);
			string str = text + "\\" + folds[folds.Length - 1];
			int num4 = (int)Math.Floor(num2);
			int num5 = (int)Math.Ceiling(num3);
			str = str + "\\" + num4.ToString();
			if (Directory.Exists(str))
			{
				str = str + "\\" + num5.ToString() + ".jpg";
				if (File.Exists(str))
				{
					return true;
				}
			}
			return false;
		}

		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			textBox2.Text = slng;
			textBox3.Text = slat;
			inip = new IniOperator(WorkPath + "Publish\\parameter.ini");
			inip.WriteString("mapproperties", "centerlat", slat);
			inip.WriteString("mapproperties", "centerlng", slng);
			string sql = "update ORGCENTERDATA set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', LNG = '" + slng + "', LAT = '" + slat + "' where ISDELETE = 0 and UNITEID = '" + unitid + "'";
			ahp5.ExecuteSql(sql, (OleDbParameter[])null);
			mapHelper1.centerlat = double.Parse(textBox3.Text);
			mapHelper1.centerlng = double.Parse(textBox2.Text);
			int selectedIndex = checkedListBox1.SelectedIndex;
			mapHelper1.ShowMap(int.Parse(checkedListBox1.Items[selectedIndex].ToString()), "", canEdit: false, map_type, null, borderDic, null, 1.0, -1);
		}

		private void mapHelper1_MapRightClick(bool canedit, double lat, double lng, int x, int y)
		{
			contextMenuStrip1.Show(Control.MousePosition.X, Control.MousePosition.Y);
			slat = string.Concat(lat);
			slng = string.Concat(lng);
		}

		private void checkedListBox1_Leave(object sender, EventArgs e)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < checkedListBox1.Items.Count; i++)
			{
				if (checkedListBox1.GetItemChecked(i))
				{
					list.Add(checkedListBox1.Items[i].ToString());
				}
			}
			TreeNode selectedNode = treeView1.SelectedNode;
			if (selectedNode != null)
			{
				string text = selectedNode.Tag.ToString();
				string sql = "select MAPLEVEL from MAPDUIYING_H0001Z000E00 where ISDELETE = 0 and LEVELGUID = '" + text + "' and UNITEID = '" + unitid + "'";
				DataTable dataTable = ahp6.ExecuteDataTable(sql, (OleDbParameter[])null);
				string text2 = string.Join(",", list);
				GL_MAP[text] = text2;
				if (dataTable.Rows.Count > 0)
				{
					sql = "update MAPDUIYING_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', MAPLEVEL = '" + text2 + "' where ISDELETE = 0 and LEVELGUID = '" + text + "' and UNITEID = '" + unitid + "'";
					ahp6.ExecuteSql(sql, (OleDbParameter[])null);
				}
				else
				{
					sql = "insert into MAPDUIYING_H0001Z000E00 (PGUID, S_UDTIME, LEVELGUID, MAPLEVEL, UNITEID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + text + "', '" + text2 + "', '" + unitid + "')";
					ahp6.ExecuteSql(sql, (OleDbParameter[])null);
				}
			}
		}

		private void MapForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			groupBox1.Focus();
			TreeNode selectedNode = treeView1.SelectedNode;
			if (selectedNode == null)
			{
				return;
			}
			string text = selectedNode.Tag.ToString();
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string path = WorkPath + "Publish\\googlemap\\map";
			if (Directory.Exists(path))
			{
				list = Directory.GetDirectories(path).ToList();
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = Path.GetFileNameWithoutExtension(list[i]);
			}
			string sql = "select MAPLEVEL from MAPDUIYING_H0001Z000E00 where ISDELETE = 0 and UNITEID = '" + unitid + "'";
			DataTable dataTable = ahp6.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text2 = dataTable.Rows[i]["MAPLEVEL"].ToString();
				if (text2 != string.Empty)
				{
					list2.AddRange(text2.Split(','));
				}
			}
			list2 = list2.Distinct().ToList();
			for (int i = list2.Count - 1; i >= 0; i--)
			{
				if (list2[i] == string.Empty)
				{
					list2.Remove(list2[i]);
				}
			}
			List<string> list3 = list.Except(list2).ToList();
			List<string> list4 = list2.Except(list).ToList();
			if (list3.Count + list4.Count == 0 || MessageBox.Show("是否更新对应地图文件?", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				return;
			}
			string value = textBox3.Text.Trim();
			string value2 = textBox2.Text.Trim();
			inip = new IniOperator(WorkPath + "Publish\\parameter.ini");
			inip.WriteString("mapproperties", "centerlat", value);
			inip.WriteString("mapproperties", "centerlng", value2);
			string str = textBox1.Text.Trim();
			string text3 = WorkPath + "Publish\\googlemap\\map";
			string text4 = WorkPath + "Publish\\googlemap\\satellite";
			string text5 = string.Empty;
			string text6 = string.Empty;
			if (!Directory.Exists(text3))
			{
				Directory.CreateDirectory(text3);
			}
			if (!Directory.Exists(text4))
			{
				Directory.CreateDirectory(text4);
			}
			string str2 = str + "\\roadmap";
			for (int i = 0; i < list4.Count; i++)
			{
				string text7 = text3 + "\\" + list4[i];
				if (!Directory.Exists(text7))
				{
					Directory.CreateDirectory(text7);
				}
				string text8 = str2 + "\\" + list4[i];
				string text9 = text5;
				text5 = text9 + text8 + "：" + text7 + ",";
			}
			str2 = str + "\\satellite_en";
			for (int i = 0; i < list4.Count; i++)
			{
				string text7 = text4 + "\\" + list4[i];
				if (!Directory.Exists(text7))
				{
					Directory.CreateDirectory(text7);
				}
				string text8 = str2 + "\\" + list4[i];
				string text9 = text5;
				text5 = text9 + text8 + "：" + text7 + ",";
			}
			Process process;
			if (text5 != string.Empty)
			{
				text5 = text5.Substring(0, text5.Length - 1);
				process = Process.Start(WorkPath + "CopyDir.exe", text5 + " 1");
				process.WaitForExit();
			}
			for (int i = 0; i < list3.Count; i++)
			{
				string text7 = text3 + "\\" + list3[i];
				while (Directory.Exists(text7))
				{
					text6 = text6 + text7 + ",";
				}
			}
			for (int i = 0; i < list3.Count; i++)
			{
				string text7 = text4 + "\\" + list3[i];
				while (Directory.Exists(text7))
				{
					text6 = text6 + text7 + ",";
				}
			}
			process = Process.Start(WorkPath + "DeleteDir.exe", text6);
			inip = new IniOperator(WorkPath + "Publish\\RegInfo.ini");
			inip.WriteString("Public", "UnitID", unitid);
			MessageBox.Show("地图更新成功!");
			ahp1.CloseConn();
			ahp2.CloseConn();
			ahp3.CloseConn();
			ahp4.CloseConn();
			ahp5.CloseConn();
			ahp6.CloseConn();
		}

		private void MapForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			ahp1.CloseConn();
			ahp2.CloseConn();
			ahp3.CloseConn();
			ahp4.CloseConn();
			ahp5.CloseConn();
			ahp6.CloseConn();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Enabled = false;
			inip = new IniOperator(WorkPath + "Publish\\RegInfo.ini");
			string a = inip.ReadString("Public", "UnitID", "");
			if (a != unitid)
			{
				Delete_Path();
			}
		}

		private void mapHelper1_MapTypeChanged(string mapType)
		{
			map_type = mapType;
		}

		private void button3_Click(object sender, EventArgs e)
		{
			int selectedIndex = checkedListBox1.SelectedIndex;
			borList = new List<double[]>();
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string fileName = openFileDialog1.FileName;
				string[] array = File.ReadAllLines(fileName);
				string[] array2 = array;
				foreach (string text in array2)
				{
					string[] array3 = text.Split(' ', ',', ':', '\t', '\r', '\n', ';');
					borList.Add(new double[2]
					{
						double.Parse(array3[1]),
						double.Parse(array3[0])
					});
				}
				borderDic["path"] = borList;
				string sql = "select PGUID from BORDERDATA where ISDELETE = 0 and UNITID = '" + unitid + "'";
				DataTable dataTable = ahp5.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					sql = "update BORDERDATA set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', ISDELETE = 1 where ISDELETE = 0 and UNITID = '" + unitid + "'";
					ahp5.ExecuteSql(sql, (OleDbParameter[])null);
				}
				for (int j = 0; j < borList.Count; j++)
				{
					sql = "insert into BORDERDATA (PGUID, S_UDTIME, UNITID, LAT, LNG, SHOWINDEX) values('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + unitid + "', '" + borList[j][0] + "', '" + borList[j][1] + "', '" + j.ToString() + "')";
					ahp5.ExecuteSql(sql, (OleDbParameter[])null);
				}
				mapHelper1.ShowMap(int.Parse(checkedListBox1.Items[selectedIndex].ToString()), checkedListBox1.Items[selectedIndex].ToString(), canEdit: false, map_type, null, null, null, 1.0, -1);
			}
		}

		private void mapHelper1_LevelChanged(int lastLevel, int currLevel, string showLevel)
		{
			mapHelper1.DrawBorder(unitid, borderDic);
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
			System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(PublishSys.MapForm));
			panel1 = new System.Windows.Forms.Panel();
			groupBox1 = new System.Windows.Forms.GroupBox();
			treeView1 = new System.Windows.Forms.TreeView();
			splitter1 = new System.Windows.Forms.Splitter();
			panel2 = new System.Windows.Forms.Panel();
			groupBox2 = new System.Windows.Forms.GroupBox();
			tabControl1 = new System.Windows.Forms.TabControl();
			tabPage1 = new System.Windows.Forms.TabPage();
			groupBox5 = new System.Windows.Forms.GroupBox();
			panel5 = new System.Windows.Forms.Panel();
			mapHelper1 = new MapHelper.MapHelper();
			splitter3 = new System.Windows.Forms.Splitter();
			groupBox6 = new System.Windows.Forms.GroupBox();
			checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			panel8 = new System.Windows.Forms.Panel();
			button3 = new System.Windows.Forms.Button();
			label4 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			textBox3 = new System.Windows.Forms.TextBox();
			textBox2 = new System.Windows.Forms.TextBox();
			button1 = new System.Windows.Forms.Button();
			textBox1 = new System.Windows.Forms.TextBox();
			tabPage2 = new System.Windows.Forms.TabPage();
			groupBox7 = new System.Windows.Forms.GroupBox();
			tabControl3 = new System.Windows.Forms.TabControl();
			tabPage3 = new System.Windows.Forms.TabPage();
			dataGridView2 = new System.Windows.Forms.DataGridView();
			tabPage4 = new System.Windows.Forms.TabPage();
			dataGridView3 = new System.Windows.Forms.DataGridView();
			splitter2 = new System.Windows.Forms.Splitter();
			groupBox4 = new System.Windows.Forms.GroupBox();
			tabControl2 = new System.Windows.Forms.TabControl();
			groupBox3 = new System.Windows.Forms.GroupBox();
			flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
			toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			timer1 = new System.Windows.Forms.Timer(components);
			openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(components);
			全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(components);
			清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			panel1.SuspendLayout();
			groupBox1.SuspendLayout();
			panel2.SuspendLayout();
			groupBox2.SuspendLayout();
			tabControl1.SuspendLayout();
			tabPage1.SuspendLayout();
			groupBox5.SuspendLayout();
			panel5.SuspendLayout();
			groupBox6.SuspendLayout();
			panel8.SuspendLayout();
			tabPage2.SuspendLayout();
			groupBox7.SuspendLayout();
			tabControl3.SuspendLayout();
			tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
			tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridView3).BeginInit();
			groupBox4.SuspendLayout();
			groupBox3.SuspendLayout();
			contextMenuStrip1.SuspendLayout();
			contextMenuStrip2.SuspendLayout();
			contextMenuStrip3.SuspendLayout();
			SuspendLayout();
			panel1.Controls.Add(groupBox1);
			panel1.Dock = System.Windows.Forms.DockStyle.Left;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(222, 758);
			panel1.TabIndex = 0;
			groupBox1.Controls.Add(treeView1);
			groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			groupBox1.Location = new System.Drawing.Point(0, 0);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(222, 758);
			groupBox1.TabIndex = 0;
			groupBox1.TabStop = false;
			groupBox1.Text = "组织结构";
			groupBox1.UseCompatibleTextRendering = true;
			treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			treeView1.Location = new System.Drawing.Point(3, 24);
			treeView1.Name = "treeView1";
			treeView1.Size = new System.Drawing.Size(216, 731);
			treeView1.TabIndex = 0;
			treeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(treeView1_DrawNode);
			treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(treeView1_AfterSelect);
			splitter1.Location = new System.Drawing.Point(222, 0);
			splitter1.Name = "splitter1";
			splitter1.Size = new System.Drawing.Size(10, 758);
			splitter1.TabIndex = 1;
			splitter1.TabStop = false;
			panel2.BackColor = System.Drawing.SystemColors.Control;
			panel2.Controls.Add(groupBox2);
			panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			panel2.Location = new System.Drawing.Point(232, 0);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(1540, 758);
			panel2.TabIndex = 2;
			groupBox2.Controls.Add(tabControl1);
			groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			groupBox2.Location = new System.Drawing.Point(0, 0);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(1540, 758);
			groupBox2.TabIndex = 0;
			groupBox2.TabStop = false;
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(tabPage2);
			tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			tabControl1.Location = new System.Drawing.Point(3, 24);
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new System.Drawing.Size(1534, 731);
			tabControl1.TabIndex = 3;
			tabPage1.BackColor = System.Drawing.SystemColors.Control;
			tabPage1.Controls.Add(groupBox5);
			tabPage1.Controls.Add(splitter3);
			tabPage1.Controls.Add(groupBox6);
			tabPage1.Controls.Add(panel8);
			tabPage1.Location = new System.Drawing.Point(4, 28);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new System.Windows.Forms.Padding(3);
			tabPage1.Size = new System.Drawing.Size(1526, 699);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "地图对应";
			groupBox5.Controls.Add(panel5);
			groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
			groupBox5.Location = new System.Drawing.Point(234, 66);
			groupBox5.Name = "groupBox5";
			groupBox5.Size = new System.Drawing.Size(1289, 630);
			groupBox5.TabIndex = 7;
			groupBox5.TabStop = false;
			groupBox5.Text = "地图显示";
			panel5.Controls.Add(mapHelper1);
			panel5.Dock = System.Windows.Forms.DockStyle.Fill;
			panel5.Location = new System.Drawing.Point(3, 24);
			panel5.Name = "panel5";
			panel5.Size = new System.Drawing.Size(1283, 603);
			panel5.TabIndex = 6;
			mapHelper1.BackColor = System.Drawing.Color.Black;
			mapHelper1.centerlat = 0.0;
			mapHelper1.centerlng = 0.0;
			mapHelper1.Dock = System.Windows.Forms.DockStyle.Fill;
			mapHelper1.iconspath = null;
			mapHelper1.Location = new System.Drawing.Point(0, 0);
			mapHelper1.maparr = null;
			mapHelper1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			mapHelper1.Name = "mapHelper1";
			mapHelper1.roadmappath = null;
			mapHelper1.satellitemappath = null;
			mapHelper1.Size = new System.Drawing.Size(1283, 603);
			mapHelper1.TabIndex = 0;
			mapHelper1.webpath = null;
			mapHelper1.MapRightClick += new MapHelper.MapHelper.DlMapRightClick(mapHelper1_MapRightClick);
			mapHelper1.LevelChanged += new MapHelper.MapHelper.DlLevelChanged(mapHelper1_LevelChanged);
			mapHelper1.MapTypeChanged += new MapHelper.MapHelper.DlMapTypeChanged(mapHelper1_MapTypeChanged);
			splitter3.Location = new System.Drawing.Point(224, 66);
			splitter3.Name = "splitter3";
			splitter3.Size = new System.Drawing.Size(10, 630);
			splitter3.TabIndex = 6;
			splitter3.TabStop = false;
			groupBox6.Controls.Add(checkedListBox1);
			groupBox6.Dock = System.Windows.Forms.DockStyle.Left;
			groupBox6.Location = new System.Drawing.Point(3, 66);
			groupBox6.Name = "groupBox6";
			groupBox6.Size = new System.Drawing.Size(221, 630);
			groupBox6.TabIndex = 5;
			groupBox6.TabStop = false;
			groupBox6.Text = "已下载地图列表";
			checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			checkedListBox1.FormattingEnabled = true;
			checkedListBox1.Items.AddRange(new object[3]
			{
				"13",
				"14",
				"15"
			});
			checkedListBox1.Location = new System.Drawing.Point(3, 24);
			checkedListBox1.Name = "checkedListBox1";
			checkedListBox1.Size = new System.Drawing.Size(215, 603);
			checkedListBox1.TabIndex = 2;
			checkedListBox1.SelectedIndexChanged += new System.EventHandler(checkedListBox1_SelectedIndexChanged);
			checkedListBox1.Leave += new System.EventHandler(checkedListBox1_Leave);
			panel8.BackColor = System.Drawing.SystemColors.Control;
			panel8.Controls.Add(button3);
			panel8.Controls.Add(label4);
			panel8.Controls.Add(label3);
			panel8.Controls.Add(label2);
			panel8.Controls.Add(label1);
			panel8.Controls.Add(textBox3);
			panel8.Controls.Add(textBox2);
			panel8.Controls.Add(button1);
			panel8.Controls.Add(textBox1);
			panel8.Dock = System.Windows.Forms.DockStyle.Top;
			panel8.Location = new System.Drawing.Point(3, 3);
			panel8.Name = "panel8";
			panel8.Size = new System.Drawing.Size(1520, 63);
			panel8.TabIndex = 4;
			button3.Enabled = false;
			button3.Location = new System.Drawing.Point(1203, 17);
			button3.Name = "button3";
			button3.Size = new System.Drawing.Size(120, 30);
			button3.TabIndex = 8;
			button3.Text = "导入边界线";
			button3.UseVisualStyleBackColor = true;
			button3.Click += new System.EventHandler(button3_Click);
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(534, 23);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(98, 18);
			label4.TabIndex = 7;
			label4.Text = "地图文件夹";
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(308, 23);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(62, 18);
			label3.TabIndex = 6;
			label3.Text = "纬度：";
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(134, 23);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(62, 18);
			label2.TabIndex = 5;
			label2.Text = "经度：";
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(30, 23);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(98, 18);
			label1.TabIndex = 4;
			label1.Text = "地图中心点";
			textBox3.Enabled = false;
			textBox3.Location = new System.Drawing.Point(376, 18);
			textBox3.Name = "textBox3";
			textBox3.Size = new System.Drawing.Size(100, 28);
			textBox3.TabIndex = 3;
			textBox2.Enabled = false;
			textBox2.Location = new System.Drawing.Point(202, 18);
			textBox2.Name = "textBox2";
			textBox2.Size = new System.Drawing.Size(100, 28);
			textBox2.TabIndex = 2;
			button1.Location = new System.Drawing.Point(1038, 17);
			button1.Name = "button1";
			button1.Size = new System.Drawing.Size(120, 30);
			button1.TabIndex = 1;
			button1.Text = "浏览";
			button1.UseVisualStyleBackColor = true;
			button1.Click += new System.EventHandler(button1_Click);
			textBox1.Enabled = false;
			textBox1.Location = new System.Drawing.Point(638, 18);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(394, 28);
			textBox1.TabIndex = 0;
			tabPage2.BackColor = System.Drawing.SystemColors.Control;
			tabPage2.Controls.Add(groupBox7);
			tabPage2.Controls.Add(splitter2);
			tabPage2.Controls.Add(groupBox4);
			tabPage2.Controls.Add(groupBox3);
			tabPage2.Location = new System.Drawing.Point(4, 28);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new System.Windows.Forms.Padding(3);
			tabPage2.Size = new System.Drawing.Size(1526, 699);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "图符对应";
			groupBox7.Controls.Add(tabControl3);
			groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
			groupBox7.Location = new System.Drawing.Point(161, 3);
			groupBox7.Name = "groupBox7";
			groupBox7.Size = new System.Drawing.Size(634, 693);
			groupBox7.TabIndex = 11;
			groupBox7.TabStop = false;
			groupBox7.Text = "图符属性";
			tabControl3.Controls.Add(tabPage3);
			tabControl3.Controls.Add(tabPage4);
			tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
			tabControl3.Location = new System.Drawing.Point(3, 24);
			tabControl3.Name = "tabControl3";
			tabControl3.SelectedIndex = 0;
			tabControl3.Size = new System.Drawing.Size(628, 666);
			tabControl3.TabIndex = 0;
			tabPage3.Controls.Add(dataGridView2);
			tabPage3.Location = new System.Drawing.Point(4, 28);
			tabPage3.Name = "tabPage3";
			tabPage3.Padding = new System.Windows.Forms.Padding(3);
			tabPage3.Size = new System.Drawing.Size(620, 634);
			tabPage3.TabIndex = 0;
			tabPage3.Text = "固定属性";
			tabPage3.UseVisualStyleBackColor = true;
			dataGridView2.AllowUserToAddRows = false;
			dataGridView2.AllowUserToDeleteRows = false;
			dataGridView2.BackgroundColor = System.Drawing.SystemColors.Control;
			dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
			dataGridView2.GridColor = System.Drawing.SystemColors.Control;
			dataGridView2.Location = new System.Drawing.Point(3, 3);
			dataGridView2.Name = "dataGridView2";
			dataGridView2.ReadOnly = true;
			dataGridView2.RowHeadersVisible = false;
			dataGridView2.RowTemplate.Height = 30;
			dataGridView2.Size = new System.Drawing.Size(614, 628);
			dataGridView2.TabIndex = 0;
			tabPage4.Controls.Add(dataGridView3);
			tabPage4.Location = new System.Drawing.Point(4, 28);
			tabPage4.Name = "tabPage4";
			tabPage4.Padding = new System.Windows.Forms.Padding(3);
			tabPage4.Size = new System.Drawing.Size(620, 634);
			tabPage4.TabIndex = 1;
			tabPage4.Text = "基础属性";
			tabPage4.UseVisualStyleBackColor = true;
			dataGridView3.AllowUserToAddRows = false;
			dataGridView3.AllowUserToDeleteRows = false;
			dataGridView3.BackgroundColor = System.Drawing.SystemColors.Control;
			dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView3.Dock = System.Windows.Forms.DockStyle.Fill;
			dataGridView3.Location = new System.Drawing.Point(3, 3);
			dataGridView3.Name = "dataGridView3";
			dataGridView3.ReadOnly = true;
			dataGridView3.RowHeadersVisible = false;
			dataGridView3.RowTemplate.Height = 30;
			dataGridView3.Size = new System.Drawing.Size(614, 628);
			dataGridView3.TabIndex = 1;
			splitter2.Dock = System.Windows.Forms.DockStyle.Right;
			splitter2.Location = new System.Drawing.Point(795, 3);
			splitter2.Name = "splitter2";
			splitter2.Size = new System.Drawing.Size(10, 693);
			splitter2.TabIndex = 10;
			splitter2.TabStop = false;
			groupBox4.Controls.Add(tabControl2);
			groupBox4.Dock = System.Windows.Forms.DockStyle.Right;
			groupBox4.Location = new System.Drawing.Point(805, 3);
			groupBox4.Name = "groupBox4";
			groupBox4.Size = new System.Drawing.Size(718, 693);
			groupBox4.TabIndex = 7;
			groupBox4.TabStop = false;
			groupBox4.Text = "图符库";
			tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			tabControl2.Location = new System.Drawing.Point(3, 24);
			tabControl2.Name = "tabControl2";
			tabControl2.SelectedIndex = 0;
			tabControl2.Size = new System.Drawing.Size(712, 666);
			tabControl2.TabIndex = 0;
			tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
			groupBox3.Controls.Add(flowLayoutPanel1);
			groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
			groupBox3.Location = new System.Drawing.Point(3, 3);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(158, 693);
			groupBox3.TabIndex = 0;
			groupBox3.TabStop = false;
			groupBox3.Text = "已对应图符";
			flowLayoutPanel1.AutoScroll = true;
			flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			flowLayoutPanel1.Location = new System.Drawing.Point(3, 24);
			flowLayoutPanel1.Name = "flowLayoutPanel1";
			flowLayoutPanel1.Size = new System.Drawing.Size(152, 666);
			flowLayoutPanel1.TabIndex = 4;
			flowLayoutPanel1.WrapContents = false;
			flowLayoutPanel1.MouseDown += new System.Windows.Forms.MouseEventHandler(flowLayoutPanel1_MouseDown);
			contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1]
			{
				toolStripMenuItem2
			});
			contextMenuStrip1.Name = "contextMenuStrip1";
			contextMenuStrip1.Size = new System.Drawing.Size(333, 32);
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new System.Drawing.Size(332, 28);
			toolStripMenuItem2.Text = "更新当前点经纬度到地图中心点";
			toolStripMenuItem2.Click += new System.EventHandler(toolStripMenuItem2_Click);
			timer1.Tick += new System.EventHandler(timer1_Tick);
			openFileDialog1.Filter = "(*.txt)|*.txt";
			contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1]
			{
				全选ToolStripMenuItem
			});
			contextMenuStrip2.Name = "contextMenuStrip2";
			contextMenuStrip2.Size = new System.Drawing.Size(117, 32);
			全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
			全选ToolStripMenuItem.Size = new System.Drawing.Size(116, 28);
			全选ToolStripMenuItem.Text = "全选";
			全选ToolStripMenuItem.Click += new System.EventHandler(全选ToolStripMenuItem_Click);
			contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1]
			{
				清空ToolStripMenuItem
			});
			contextMenuStrip3.Name = "contextMenuStrip3";
			contextMenuStrip3.Size = new System.Drawing.Size(117, 32);
			清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
			清空ToolStripMenuItem.Size = new System.Drawing.Size(116, 28);
			清空ToolStripMenuItem.Text = "清空";
			清空ToolStripMenuItem.Click += new System.EventHandler(清空ToolStripMenuItem_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(9f, 18f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1772, 758);
			base.Controls.Add(panel2);
			base.Controls.Add(splitter1);
			base.Controls.Add(panel1);
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "MapForm";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			Text = "MapForm";
			base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(MapForm_FormClosing);
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(MapForm_FormClosed);
			base.Load += new System.EventHandler(MapForm_Load);
			base.Shown += new System.EventHandler(MapForm_Shown);
			panel1.ResumeLayout(performLayout: false);
			groupBox1.ResumeLayout(performLayout: false);
			panel2.ResumeLayout(performLayout: false);
			groupBox2.ResumeLayout(performLayout: false);
			tabControl1.ResumeLayout(performLayout: false);
			tabPage1.ResumeLayout(performLayout: false);
			groupBox5.ResumeLayout(performLayout: false);
			panel5.ResumeLayout(performLayout: false);
			groupBox6.ResumeLayout(performLayout: false);
			panel8.ResumeLayout(performLayout: false);
			panel8.PerformLayout();
			tabPage2.ResumeLayout(performLayout: false);
			groupBox7.ResumeLayout(performLayout: false);
			tabControl3.ResumeLayout(performLayout: false);
			tabPage3.ResumeLayout(performLayout: false);
			((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
			tabPage4.ResumeLayout(performLayout: false);
			((System.ComponentModel.ISupportInitialize)dataGridView3).EndInit();
			groupBox4.ResumeLayout(performLayout: false);
			groupBox3.ResumeLayout(performLayout: false);
			contextMenuStrip1.ResumeLayout(performLayout: false);
			contextMenuStrip2.ResumeLayout(performLayout: false);
			contextMenuStrip3.ResumeLayout(performLayout: false);
			ResumeLayout(performLayout: false);
		}
	}
}
