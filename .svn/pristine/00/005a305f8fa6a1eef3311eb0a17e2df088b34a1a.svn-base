using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using MapHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EnvirInfoSys
{
	public class MapSetForm : XtraForm
	{
		private string WorkPath = AppDomain.CurrentDomain.BaseDirectory;

		private string AccessPath = AppDomain.CurrentDomain.BaseDirectory + "data\\ENVIR_H0001Z000E00.mdb";

		private string IniFilePath = AppDomain.CurrentDomain.BaseDirectory + "parameter.ini";

		private string[] folds = null;

		public string unitid = "";

		private string[] GL_PGUID;

		private List<GL_Node> GL_List;

		private Dictionary<string, string> GL_NAME;

		private Dictionary<string, string> GL_JDCODE;

		private Dictionary<string, string> GL_UPGUID;

		private Dictionary<string, string> GL_MAP;

		private Dictionary<string, string> GL_NAME_PGUID;

		private Dictionary<string, Polygon> GL_POLY;

		private Dictionary<string, object> borderDic = null;

		private List<double[]> borList = new List<double[]>();

		private LineData borData = null;

		private bool Before_ShowMap = false;

		private int cur_level = 0;

		private int now_level = 0;

		private string levelguid = "";

		private string map_type = "";

		private int MapX = 0;

		private int MapY = 0;

		private IContainer components = null;

		private BarManager barManager1;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private GroupControl groupControl2;

		private PanelControl panelControl1;

		private FlowLayoutPanel flowLayoutPanel1;

		private PanelControl panelControl2;

		private Label label1;

		private Splitter splitter1;

		private GroupControl groupControl1;

		private TreeList treeList1;

		private BarButtonItem barButtonItem1;

		private PopupMenu popupMenu1;

		private BarButtonItem barButtonItem2;

		private XtraOpenFileDialog xtraOpenFileDialog1;

		private MapHelper.MapHelper mapHelper1;

		public MapSetForm()
		{
			InitializeComponent();
		}

		private void MapSetForm_Load(object sender, EventArgs e)
		{
			folds = Get_Map_List();
			Load_Unit_Level();
			FileReader.inip = new IniOperator(IniFilePath);
			string s = FileReader.inip.ReadString("mapproperties", "centerlat", "");
			string s2 = FileReader.inip.ReadString("mapproperties", "centerlng", "");
			mapHelper1.centerlat = double.Parse(s);
			mapHelper1.centerlng = double.Parse(s2);
			string sql = "select LAT, LNG from ORGCENTERDATA where ISDELETE = 0 and UNITEID = '" + unitid + "'";
			DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				mapHelper1.centerlat = double.Parse(dataTable.Rows[0]["LAT"].ToString());
				mapHelper1.centerlng = double.Parse(dataTable.Rows[0]["LNG"].ToString());
			}
			mapHelper1.webpath = WorkPath + "googlemap";
			mapHelper1.roadmappath = WorkPath + "googlemap\\map";
			mapHelper1.satellitemappath = WorkPath + "googlemap\\satellite";
			mapHelper1.iconspath = WorkPath + "PNGICONFOLDER";
			mapHelper1.maparr = folds;
			Load_Border(unitid);
			PictureBox pictureBox = new PictureBox();
			ToolTip toolTip = new ToolTip();
			toolTip.SetToolTip(pictureBox, "指针");
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.BorderStyle = BorderStyle.Fixed3D;
			pictureBox.Width = 32;
			pictureBox.Height = 32;
			pictureBox.Click += Vector_Click;
			pictureBox.Name = "指针";
			FileStream fileStream = new FileStream(WorkPath + "icon\\指针.png", FileMode.Open, FileAccess.Read);
			pictureBox.Image = Image.FromStream(fileStream);
			flowLayoutPanel1.Controls.Add(pictureBox);
			fileStream.Close();
			fileStream.Dispose();
			pictureBox = new PictureBox();
			toolTip = new ToolTip();
			toolTip.SetToolTip(pictureBox, "画线");
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.Width = 32;
			pictureBox.Height = 32;
			pictureBox.Click += Line_Click;
			pictureBox.Name = "画线";
			fileStream = new FileStream(WorkPath + "icon\\画线.png", FileMode.Open, FileAccess.Read);
			pictureBox.Image = Image.FromStream(fileStream);
			flowLayoutPanel1.Controls.Add(pictureBox);
			fileStream.Close();
			fileStream.Dispose();
			pictureBox = new PictureBox();
			toolTip = new ToolTip();
			toolTip.SetToolTip(pictureBox, "画多边形");
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.Width = 32;
			pictureBox.Height = 32;
			pictureBox.Click += Polygon_Click;
			pictureBox.Name = "画多边形";
			fileStream = new FileStream(WorkPath + "icon\\多边形.png", FileMode.Open, FileAccess.Read);
			pictureBox.Image = Image.FromStream(fileStream);
			flowLayoutPanel1.Controls.Add(pictureBox);
			fileStream.Close();
			fileStream.Dispose();
			pictureBox = new PictureBox();
			toolTip = new ToolTip();
			toolTip.SetToolTip(pictureBox, "中心点");
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.Width = 32;
			pictureBox.Height = 32;
			pictureBox.MouseDown += Center_MouseDown;
			pictureBox.Click += Center_Click;
			pictureBox.Name = "中心点";
			fileStream = new FileStream(WorkPath + "icon\\中心点.png", FileMode.Open, FileAccess.Read);
			pictureBox.Image = Image.FromStream(fileStream);
			flowLayoutPanel1.Controls.Add(pictureBox);
			fileStream.Close();
			fileStream.Dispose();
			mapHelper1.ShowMap(cur_level, cur_level.ToString(), canEdit: false, map_type, null, null, null, 1.0, 400);
		}

		private void Polygon_Click(object sender, EventArgs e)
		{
		}

		private void Line_Click(object sender, EventArgs e)
		{
		}

		private void Vector_Click(object sender, EventArgs e)
		{
		}

		private void Center_MouseDown(object sender, MouseEventArgs e)
		{
			PictureBox pictureBox = (PictureBox)sender;
			pictureBox.BorderStyle = BorderStyle.Fixed3D;
		}

		private void Center_Click(object sender, EventArgs e)
		{
			double[] mapCenter = mapHelper1.GetMapCenter();
			TreeListNode focusedNode = treeList1.FocusedNode;
			string text = focusedNode.GetValue("pguid").ToString();
			string sql = "select PGUID from ORGCENTERDATA where ISDELETE = 0 and PGUID = '" + text + "' and UNITEID = '" + unitid + "'";
			DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				sql = "update ORGCENTERDATA set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', LAT = '" + mapCenter[0].ToString() + "', LNG = '" + mapCenter[1].ToString() + "' where ISDELETE = 0 and PGUID = '" + text + "' and UNITEID = '" + unitid + "'";
				FileReader.line_ahp.ExecuteSql(sql, (OleDbParameter[])null);
			}
			else
			{
				sql = "insert into ORGCENTERDATA (PGUID, S_UDTIME, UNITEID, LAT, LNG) values('" + text + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + text + "', '" + mapCenter[0].ToString() + "', '" + mapCenter[1].ToString() + "')";
				FileReader.line_ahp.ExecuteSql(sql, (OleDbParameter[])null);
			}
			PictureBox pictureBox = (PictureBox)sender;
			pictureBox.BorderStyle = BorderStyle.None;
			XtraMessageBox.Show("中心点设置成功!");
		}

		private string[] Get_Map_List()
		{
			string[] array = null;
			string path = WorkPath + "googlemap\\map";
			if (!Directory.Exists(path))
			{
				XtraMessageBox.Show("未导入地图文件!请重新发布软件");
				FileReader.often_ahp.CloseConn();
				FileReader.line_ahp.CloseConn();
				FileReader.log_ahp.CloseConn();
				Environment.Exit(0);
			}
			array = Directory.GetDirectories(path);
			for (int i = 0; i < array.Length; i++)
			{
				int num = array[i].LastIndexOf("\\");
				array[i] = array[i].Substring(num + 1);
			}
			return array;
		}

		private void Load_Unit_Level()
		{
			GL_NAME = new Dictionary<string, string>();
			GL_JDCODE = new Dictionary<string, string>();
			GL_UPGUID = new Dictionary<string, string>();
			GL_MAP = new Dictionary<string, string>();
			GL_NAME_PGUID = new Dictionary<string, string>();
			GL_POLY = new Dictionary<string, Polygon>();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ZSK_H0001Z000K01.mdb");
			string sql = "select PGUID, JDNAME, JDCODE, UPGUID from ZSK_OBJECT_H0001Z000K01 where ISDELETE = 0 order by LEVELNUM, SHOWINDEX";
			DataTable dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			GL_PGUID = new string[dataTable.Rows.Count];
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["PGUID"].ToString();
				GL_PGUID[i] = text;
				GL_NAME[text] = dataTable.Rows[i]["JDNAME"].ToString();
				GL_JDCODE[text] = dataTable.Rows[i]["JDCODE"].ToString();
				GL_NAME_PGUID[dataTable.Rows[i]["JDNAME"].ToString()] = text;
			}
			FileReader.once_ahp.CloseConn();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ENVIRDYDATA_H0001Z000E00.mdb");
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["PGUID"].ToString();
				sql = "select MAPLEVEL from MAPDUIYING_H0001Z000E00 where ISDELETE = 0 and LEVELGUID = '" + text + "' and UNITID = '" + unitid + "'";
				DataTable dataTable2 = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable2.Rows.Count > 0)
				{
					GL_MAP.Add(text, dataTable2.Rows[0]["MAPLEVEL"].ToString());
				}
				else
				{
					GL_MAP.Add(text, string.Empty);
				}
			}
			FileReader.once_ahp.CloseConn();
			GL_List = new List<GL_Node>();
			treeList1.Nodes.Clear();
			treeList1.Appearance.FocusedCell.BackColor = Color.SteelBlue;
			treeList1.KeyFieldName = "pguid";
			treeList1.ParentFieldName = "upguid";
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\PersonMange.mdb");
			sql = "select PGUID, UPPGUID, ORGNAME, ULEVEL from RG_单位注册 where ISDELETE = 0 and PGUID = '" + unitid + "'";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				GL_Node gL_Node = new GL_Node();
				gL_Node.pguid = dataTable.Rows[i]["PGUID"].ToString();
				gL_Node.upguid = dataTable.Rows[i]["UPPGUID"].ToString();
				GL_UPGUID[gL_Node.pguid] = dataTable.Rows[i]["UPPGUID"].ToString();
				gL_Node.Name = dataTable.Rows[i]["ORGNAME"].ToString();
				gL_Node.level = dataTable.Rows[i]["ULEVEL"].ToString();
				GL_List.Add(gL_Node);
				Add_Unit_Node(gL_Node);
			}
			treeList1.DataSource = GL_List;
			treeList1.HorzScrollVisibility = ScrollVisibility.Auto;
			treeList1.Columns[1].Visible = false;
			treeList1.Columns[2].Visible = false;
			treeList1.Columns[3].Visible = false;
			treeList1.Columns[4].Visible = false;
			treeList1.Columns[5].Visible = false;
			treeList1.ExpandAll();
			FileReader.once_ahp.CloseConn();
		}

		private void Add_Unit_Node(GL_Node pa)
		{
			string sql = "select PGUID, UPPGUID, ORGNAME, ULEVEL from RG_单位注册 where ISDELETE = 0 and UPPGUID = '" + pa.pguid + "'";
			DataTable dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				GL_Node gL_Node = new GL_Node();
				gL_Node.pguid = dataTable.Rows[i]["PGUID"].ToString();
				gL_Node.upguid = dataTable.Rows[i]["UPPGUID"].ToString();
				GL_UPGUID[gL_Node.pguid] = dataTable.Rows[i]["UPPGUID"].ToString();
				gL_Node.Name = dataTable.Rows[i]["ORGNAME"].ToString();
				gL_Node.level = dataTable.Rows[i]["ULEVEL"].ToString();
				GL_List.Add(gL_Node);
				Add_Unit_Node(gL_Node);
			}
		}

		private void Load_Border(string u_guid)
		{
			borList = new List<double[]>();
			borderDic = new Dictionary<string, object>();
			LineData lineData = new LineData();
			lineData.Load_Line("边界线");
			if (lineData.Type != null)
			{
				borData = lineData;
			}
			borderDic.Add("type", borData.Type);
			borderDic.Add("width", borData.Width);
			borderDic.Add("color", borData.Color);
			borderDic.Add("opacity", borData.Opacity);
			string sql = "select LAT, LNG from BORDERDATA where ISDELETE = 0 and UNITID = '" + u_guid + "' order by SHOWINDEX";
			DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				borList.Add(new double[2]
				{
					double.Parse(dataTable.Rows[i]["LAT"].ToString()),
					double.Parse(dataTable.Rows[i]["LNG"].ToString())
				});
			}
			if (dataTable.Rows.Count <= 0)
			{
				Load_Border(GL_UPGUID[u_guid]);
			}
			else
			{
				borderDic.Add("path", borList);
			}
		}

		private List<double[]> Get_Border_Line(string pguid)
		{
			string text = pguid;
			List<double[]> list = new List<double[]>();
			string sql = "select LAT, LNG from BORDERDATA where ISDELETE = 0 and UNITID = '" + pguid + "' order by SHOWINDEX";
			DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				list.Add(new double[2]
				{
					double.Parse(dataTable.Rows[i]["LAT"].ToString()),
					double.Parse(dataTable.Rows[i]["LNG"].ToString())
				});
			}
			while (list.Count < 3)
			{
				text = GL_UPGUID[text];
				list = Get_Border_Line(text);
			}
			GL_POLY[pguid] = new Polygon(list);
			return list;
		}

		private void DrawBorder()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string dlabel = "fb66d40b-50fa-4d88-8156-c590328004cb";
			dictionary["color"] = borData.Color;
			dictionary["weight"] = 0;
			dictionary["fillColor"] = "#C0C0C0";
			dictionary["fillOpacity"] = 0.5;
			dictionary["points"] = borList;
			if (borList.Count > 0)
			{
				mapHelper1.DarkOuter(dlabel, dictionary);
			}
			TreeListNode focusedNode = treeList1.FocusedNode;
			string text = focusedNode.GetValue("pguid").ToString();
			List<double[]> value = Get_Border_Line(text);
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2["type"] = borData.Type;
			dictionary2["width"] = borData.Width;
			dictionary2["color"] = borData.Color;
			dictionary2["opacity"] = borData.Opacity;
			dictionary2["path"] = value;
			mapHelper1.DrawBorder(text, dictionary2);
			foreach (TreeListNode node in focusedNode.Nodes)
			{
				text = node.GetValue("pguid").ToString();
				value = Get_Border_Line(text);
				dictionary2 = new Dictionary<string, object>();
				dictionary2["type"] = "实线";
				dictionary2["width"] = 1;
				dictionary2["color"] = "#8B0000";
				dictionary2["opacity"] = 0.75;
				dictionary2["path"] = value;
				mapHelper1.DrawBorder(text, dictionary2);
			}
		}

		private void EraseBorder()
		{
			TreeListNode focusedNode = treeList1.FocusedNode;
			mapHelper1.deleteMarker(focusedNode["pguid"].ToString());
			foreach (TreeListNode node in focusedNode.Nodes)
			{
				mapHelper1.deleteMarker(node["pguid"].ToString());
			}
		}

		private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
		{
			TreeListNode focusedNode = treeList1.FocusedNode;
			if (e.Node != null)
			{
				levelguid = GL_NAME_PGUID[e.Node.GetValue("level").ToString()];
				string[] array = GL_MAP[levelguid].Split(',');
				string sql = "select MAPLEVEL from ENVIRMAPDY_H0001Z000E00 where ISDELETE = 0 and UNITID = '" + unitid + "' and PGUID = '" + focusedNode["pguid"].ToString() + "'";
				DataTable dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					array = dataTable.Rows[0]["MAPLEVEL"].ToString().Split(',');
				}
				if (array[0] != string.Empty)
				{
					cur_level = int.Parse(array[0]);
				}
				else
				{
					cur_level = 0;
				}
				label1.Text = "当前级别：" + GL_NAME[levelguid];
				Load_Border(e.Node.GetValue("pguid").ToString());
				GL_POLY[e.Node.GetValue("pguid").ToString()] = new Polygon(borList);
				bool flag = false;
				sql = "select MARKELAT, MARKELNG from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and MAKRENAME like '%" + e.Node.GetValue("Name").ToString() + "%'";
				dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					mapHelper1.centerlat = double.Parse(dataTable.Rows[0]["MARKELAT"].ToString());
					mapHelper1.centerlng = double.Parse(dataTable.Rows[0]["MARKELNG"].ToString());
					flag = true;
				}
				sql = "select LAT, LNG from ORGCENTERDATA where ISDELETE = 0 and UNITEID = '" + e.Node.GetValue("pguid").ToString() + "'";
				dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					mapHelper1.centerlat = double.Parse(dataTable.Rows[0]["LAT"].ToString());
					mapHelper1.centerlng = double.Parse(dataTable.Rows[0]["LNG"].ToString());
					flag = true;
				}
				if (!flag && Before_ShowMap)
				{
					double[] mapCenter = mapHelper1.GetMapCenter();
					mapHelper1.centerlat = mapCenter[0];
					mapHelper1.centerlng = mapCenter[1];
				}
				Before_ShowMap = true;
				mapHelper1.ShowMap(cur_level, cur_level.ToString(), canEdit: false, map_type, null, null, null, 1.0, 400);
			}
		}

		private void treeList1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				TreeListNode nodeAt = treeList1.GetNodeAt(e.X, e.Y);
				if (nodeAt != null)
				{
					popupMenu1.ShowPopup(barManager1, Control.MousePosition);
					MapX = Control.MousePosition.X;
					MapY = Control.MousePosition.Y;
					treeList1.FocusedNode = nodeAt;
				}
			}
		}

		private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
		{
			MapLevelForm mapLevelForm = new MapLevelForm();
			TreeListNode focusedNode = treeList1.FocusedNode;
			mapLevelForm.StartPosition = FormStartPosition.Manual;
			mapLevelForm.Left = MapX;
			mapLevelForm.Top = MapY;
			if (mapLevelForm.Top + mapLevelForm.Height > base.Height)
			{
				mapLevelForm.Top -= mapLevelForm.Height;
			}
			mapLevelForm.nodeid = focusedNode["pguid"].ToString();
			mapLevelForm.unitid = unitid;
			mapLevelForm.ShowDialog();
			if (focusedNode.ParentNode != null)
			{
				treeList1.FocusedNode = focusedNode.ParentNode;
			}
			else if (focusedNode.Nodes[0] != null)
			{
				treeList1.FocusedNode = focusedNode.Nodes[0];
			}
			treeList1.FocusedNode = focusedNode;
		}

		private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
		{
			borList = new List<double[]>();
			TreeListNode focusedNode = treeList1.FocusedNode;
			if (xtraOpenFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string fileName = xtraOpenFileDialog1.FileName;
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
				string sql = "select PGUID from BORDERDATA where ISDELETE = 0 and UNITID = '" + focusedNode["pguid"].ToString() + "'";
				DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				string remark = "添加" + focusedNode["Name"].ToString() + "的边界线";
				if (dataTable.Rows.Count > 0)
				{
					sql = "update BORDERDATA set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', ISDELETE = 1 where ISDELETE = 0 and UNITID = '" + focusedNode["pguid"].ToString() + "'";
					FileReader.line_ahp.ExecuteSql(sql, (OleDbParameter[])null);
					remark = "修改" + focusedNode["Name"].ToString() + "的边界线";
				}
				for (int j = 0; j < borList.Count; j++)
				{
					sql = "insert into BORDERDATA (PGUID, S_UDTIME, UNITID, LAT, LNG, SHOWINDEX) values('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + focusedNode["pguid"].ToString() + "', '" + borList[j][0] + "', '" + borList[j][1] + "', '" + j.ToString() + "')";
					FileReader.line_ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
				ComputerInfo.WriteLog("导入边界线", remark);
				mapHelper1.ShowMap(cur_level, cur_level.ToString(), canEdit: false, map_type, null, null, null, 1.0, 400);
			}
		}

		private void mapHelper1_MapTypeChanged(string mapType)
		{
			map_type = mapType;
		}

		private void mapHelper1_LevelChanged(int lastLevel, int currLevel, string showLevel)
		{
			now_level = currLevel;
			DrawBorder();
		}

		private void mapHelper1_MapDblClick(string button, bool canedit, double lat, double lng, int x, int y, string markerguid)
		{
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapSetForm));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.mapHelper1 = new MapHelper.MapHelper();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1,
            this.barButtonItem2});
            this.barManager1.MaxItemId = 2;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1297, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 696);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1297, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 696);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1297, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 696);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "设置地图对应级别";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "导入单位边界线";
            this.barButtonItem2.Id = 1;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // treeList1
            // 
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(2, 31);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.Size = new System.Drawing.Size(196, 663);
            this.treeList1.TabIndex = 0;
            this.treeList1.ViewStyle = DevExpress.XtraTreeList.TreeListViewStyle.TreeView;
            this.treeList1.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList1_FocusedNodeChanged);
            this.treeList1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeList1_MouseDown);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.treeList1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(200, 696);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "管辖范围";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 696);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.flowLayoutPanel1);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(2, 31);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1083, 63);
            this.panelControl1.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(885, 59);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.label1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl2.Location = new System.Drawing.Point(887, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(194, 59);
            this.panelControl2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(2, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 55);
            this.label1.TabIndex = 2;
            this.label1.Text = "当前级别：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.mapHelper1);
            this.groupControl2.Controls.Add(this.panelControl1);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(210, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1087, 696);
            this.groupControl2.TabIndex = 3;
            this.groupControl2.Text = "地图显示";
            // 
            // mapHelper1
            // 
            this.mapHelper1.BackColor = System.Drawing.Color.Black;
            this.mapHelper1.centerlat = 0D;
            this.mapHelper1.centerlng = 0D;
            this.mapHelper1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapHelper1.iconspath = null;
            this.mapHelper1.Location = new System.Drawing.Point(2, 94);
            this.mapHelper1.maparr = null;
            this.mapHelper1.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.mapHelper1.Name = "mapHelper1";
            this.mapHelper1.roadmappath = null;
            this.mapHelper1.satellitemappath = null;
            this.mapHelper1.Size = new System.Drawing.Size(1083, 600);
            this.mapHelper1.TabIndex = 3;
            this.mapHelper1.webpath = null;
            this.mapHelper1.MapDblClick += new MapHelper.MapHelper.DlMapDblClick(this.mapHelper1_MapDblClick);
            this.mapHelper1.LevelChanged += new MapHelper.MapHelper.DlLevelChanged(this.mapHelper1_LevelChanged);
            this.mapHelper1.MapTypeChanged += new MapHelper.MapHelper.DlMapTypeChanged(this.mapHelper1_MapTypeChanged);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // xtraOpenFileDialog1
            // 
            this.xtraOpenFileDialog1.FileName = null;
            this.xtraOpenFileDialog1.Filter = "文本文件|*.txt";
            // 
            // MapSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 696);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MapSetForm";
            this.Text = "地图设置";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MapSetForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
