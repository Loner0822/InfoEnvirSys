using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
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
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Timers;
using System.Windows.Forms;
using System.Xml;

namespace EnvirInfoSys
{
	public class MainForm : XtraForm
	{
		private SplashScreenManager _loadForm;

		private string UnitName = "";

		private string VerNum = "";

		private string AppName = "";

		private string ProgName = "";

		private string UnitID = "-1";

		private string UnitReal = "-1";

		private readonly string WorkPath = AppDomain.CurrentDomain.BaseDirectory;

		private readonly string AccessPath = AppDomain.CurrentDomain.BaseDirectory + "data\\ENVIR_H0001Z000E00.mdb";

		private readonly string IniFilePath = AppDomain.CurrentDomain.BaseDirectory + "parameter.ini";

		private string Icon_GUID = "";

		private bool select_vector = false;

		private string Operator_GUID = "";

		private int handle;

		private double i_lat = 0.0;

		private double i_lng = 0.0;

		private Dictionary<string, string> GUID_Icon;

		private Dictionary<string, string> GUID_Name;

		private Dictionary<string, string> FDName_Value;

		private Dictionary<string, string> Icon_JDCode;

		private Dictionary<string, string> Icon_Name;

		private string[] GL_PGUID;

		private List<GL_Node> GL_List;

		private Dictionary<string, string> GL_NAME;

		private Dictionary<string, string> GL_JDCODE;

		private Dictionary<string, string> GL_UPGUID;

		private Dictionary<string, string> GL_MAP;

		private Dictionary<string, string> GL_NAME_PGUID;

		private Dictionary<string, Dictionary<string, Polygon>> GL_POLY;

		private string MapPath = "";

		private int cur_Level;

		private int now_Level;

		private string levelguid = "";

		private string map_type = "g_map";

		private bool Permission = false;

		private bool Before_ShowMap = false;

		private string[] folds = null;

		private List<Dictionary<string, object>> cur_lst;

		private string last_marker = "";

		private Dictionary<string, object> borderDic = null;

		private Dictionary<string, List<double[]>> borList = new Dictionary<string, List<double[]>>();

		private LineData borData = null;

		private LineData lineData = null;

		private string GXguid = "";

		private string FLguid = "";

		private List<string> Reg_Guid = null;

		private Dictionary<string, string> Reg_Name = null;

		private Dictionary<string, string> Reg_Down = null;

		private Dictionary<string, List<string>> Level_Icon;

		private Dictionary<string, List<string>> GX_Icon;

		//private Dictionary<string, string> Person_Marker;

		private Dictionary<string, Map_Person> Person_GUID;

		private InfoForm ifm = new InfoForm();

		private RegForm regfm = new RegForm();

		private Process TransMessage = null;

		private System.Timers.Timer NetTimer = null;

		private int borderlines = 0;

		private PictureBox currCtl = null;

		private Point startPoint = new Point(-100, -100);

		private int MapX = 0;

		private int MapY = 0;

		private double center_lat = 0.0;

		private double center_lng = 0.0;

		//private string now_person_id = "";

		private static int CodeN = 15;

		private static int[] s1 = new int[15]
		{
			12,
			14,
			7,
			9,
			8,
			10,
			11,
			5,
			3,
			15,
			13,
			4,
			1,
			2,
			6
		};

		private static int[] s2 = new int[15]
		{
			14,
			7,
			11,
			15,
			8,
			10,
			12,
			9,
			3,
			2,
			5,
			13,
			4,
			1,
			6
		};

		private static int[] s3 = new int[15]
		{
			4,
			8,
			12,
			10,
			3,
			15,
			11,
			5,
			9,
			14,
			1,
			7,
			13,
			2,
			6
		};

		private IContainer components = null;

		private DockManager dockManager1;

		private DockPanel dockPanel1;

		private ControlContainer dockPanel1_Container;

		private GroupControl groupControl1;

		private Panel panel2;

		private BarDockControl barDockControlLeft;

		private BarManager barManager1;

		private Bar bar1;

		private Bar bar2;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlRight;

		private TreeList treeList1;

		private BarSubItem barSubItem1;

		private BarButtonItem barButtonItem4;

		private BarButtonItem barButtonItem5;

		private BarButtonItem barButtonItem6;

		private BarSubItem barSubItem2;

		private BarButtonItem barButtonItem7;

		private BarButtonItem barButtonItem8;

		private BarButtonItem barButtonItem9;

		private BarButtonItem barButtonItem10;

		private BarButtonItem barButtonItem11;

		private BarButtonItem barButtonItem12;

		private BarButtonItem barButtonItem1;

		private BarButtonItem barButtonItem2;

		private PopupMenu popupMenu1;

		private SkinBarSubItem skinBarSubItem1;

		private BarButtonItem barButtonItem3;

		private BarButtonItem barButtonItem13;

		private BarButtonItem barButtonItem14;

		private BarButtonItem barButtonItem15;

		private BarButtonItem barButtonItem16;

		private BarButtonItem barButtonItem17;

		private BarButtonItem barButtonItem18;

		private PopupMenu popupMenu2;

		private BarButtonItem barButtonItem19;

		private XtraOpenFileDialog xtraOpenFileDialog1;

		private BarButtonItem barButtonItem20;

		private SplashScreenManager splashScreenManager1;

		private BarButtonItem barButtonItem21;

		private MapHelper.MapHelper mapHelper1;

		private BarButtonItem barButtonItem22;

		private BarButtonItem barButtonItem23;

		private BarButtonItem barButtonItem24;

		private BarButtonItem barButtonItem25;

		private BarButtonItem barButtonItem26;

		private XtraFolderBrowserDialog xtraFolderBrowserDialog1;

		private BarButtonItem barButtonItem27;

		private Bar bar3;

		private BarStaticItem barStaticItem1;
        private DockPanel dockPanel2;
        private ControlContainer dockPanel2_Container;
        private PanelControl panelControl1;
        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox pbMove;
        private BarStaticItem barStaticItem2;

		protected SplashScreenManager LoadForm
		{
			get
			{
				if (_loadForm == null)
				{
                    _loadForm = new SplashScreenManager(this, typeof(WaitForm1), useFadeIn: true, useFadeOut: true)
                    {
                        ClosingDelay = 0
                    };
                }
				return _loadForm;
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 74)
			{
				ImportFromDLL.COPYDATASTRUCT cOPYDATASTRUCT = (ImportFromDLL.COPYDATASTRUCT)m.GetLParam(typeof(ImportFromDLL.COPYDATASTRUCT));
				if (cOPYDATASTRUCT.dwData == 1111)
				{
					GetPersonMessage(cOPYDATASTRUCT.lpData);
				}
			}
			base.WndProc(ref m);
		}

		public void ShowMessage()
		{
			if (!LoadForm.IsSplashFormVisible)
			{
				LoadForm.ShowWaitForm();
			}
		}

		public void HideMessage()
		{
			if (LoadForm.IsSplashFormVisible)
			{
				LoadForm.CloseWaitForm();
			}
		}

		public MainForm()
		{
			InitializeComponent();
		}

		private void NetTimer_Tick(object sender, ElapsedEventArgs e)
		{
			IniOperator iniOperator = new IniOperator(WorkPath + "SyncInfo.ini");
			string text = iniOperator.ReadString("YUNLogin", "ip", "");
			string text2 = iniOperator.ReadString("YUNLogin", "port", "");
			if (TestServerConnection(text, int.Parse(text2), 500))
			{
				NetConnect.ip = text;
				NetConnect.port = text2;
				NetConnect.host = iniOperator.ReadString("YUNLogin", "host", "");
				string text3 = "http://" + NetConnect.ip + ":" + NetConnect.port + "/downfile/";
				NetConnect.state = "云服务";
				barStaticItem1.Caption = "已连接到云服务";
                SendMessage("", 2222, ProgName);
                return;
			}
			text = iniOperator.ReadString("JULogin", "ip", "");
			text2 = iniOperator.ReadString("JULogin", "port", "");
			if (TestServerConnection(text, int.Parse(text2), 500))
			{
				string url = "http://" + text + ":" + text2 + "/updataService.asmx";
				string methodname = "GetOpenFireInfo";
				NetConnect.ip = text;
				NetConnect.port = text2;
				NetConnect.host = iniOperator.ReadString("JULogin", "host", "");
				string text3 = "http://" + NetConnect.ip + ":" + NetConnect.port + "/downfile/";
				if (NetConnect.host == "")
				{
					NetConnect.host = Webservice.InvokeWebservice(url, methodname, new string[2]
					{
						UnitName,
						""
					});
					NetConnect.host = NetConnect.host.Split(',')[0];
					NetConnect.host = NetConnect.host.Substring(NetConnect.host.IndexOf('=') + 1);
					iniOperator.WriteString("JULogin", NetConnect.host, "");
				}
				NetConnect.state = "企业网";
				barStaticItem1.Caption = "已连接到企业网";
                SendMessage("", 3333, ProgName);
            }
			else
			{
				NetConnect.state = "未连接";
				barStaticItem1.Caption = "未连接到网络";
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			string text = WorkPath + "ICONDER\\b_PNGICON_tmp\\";
			string str = WorkPath + "ICONDER\\b_PNGICON\\";
			if (Directory.Exists(text))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				FileInfo[] files = directoryInfo.GetFiles();
				FileInfo[] array = files;
				foreach (FileInfo fileInfo in array)
				{
					File.Copy(text + fileInfo.Name, str + fileInfo.Name, overwrite: true);
					File.Delete(text + fileInfo.Name);
				}
				Directory.Delete(text);
			}
			text = WorkPath + "ICONDER\\s_PNGICON_tmp\\";
			str = WorkPath + "ICONDER\\s_PNGICON\\";
			if (Directory.Exists(text))
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				FileInfo[] files = directoryInfo.GetFiles();
				FileInfo[] array = files;
				foreach (FileInfo fileInfo in array)
				{
					File.Copy(text + fileInfo.Name, str + fileInfo.Name, overwrite: true);
					File.Delete(text + fileInfo.Name);
				}
				Directory.Delete(text);
			}

            int width2 = TextRenderer.MeasureText("-", new Font("宋体", 6f)).Width;
            int width3 = TextRenderer.MeasureText("管辖范围", new Font("宋体", 6f)).Width;
            int num = (dockPanel1.Height - width3) / width2;
            str = "";
            for (int j = 0; j < num; j++)
            {
                str += "-";
            }
            str += "管辖范围";
            for (int j = 0; j < num; j++)
            {
                str += "-";
            }
            dockPanel1.TabText = str;
            str = "";
            for (int j = 0; j < num; j++)
            {
                str += "-";
            }
            str += "图例";
            for (int j = 0; j < num; j++)
            {
                str += "-";
            }
            dockPanel2.TabText = str;
        }

		private void MainForm_Shown(object sender, EventArgs e)
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
			barStaticItem1.Border = BorderStyles.NoBorder;
			barStaticItem2.Border = BorderStyles.NoBorder;
			barStaticItem1.Caption = "正在连接到网络...";
			NetTimer = new System.Timers.Timer(1000.0);
			NetTimer.Elapsed += NetTimer_Tick;
			NetTimer.AutoReset = true;
			NetTimer.Enabled = true;
			mapHelper1.wb1.ScriptErrorsSuppressed = true;
			borData = new LineData();
			lineData = new LineData();
			borData.Get_NewLine();
			lineData.Get_NewLine();
			currCtl = pbMove;
			FileReader.inip = new IniOperator(WorkPath + "RegInfo.ini");
			UnitName = FileReader.inip.ReadString("Public", "UnitName", "");
			UnitName = UnitName.Replace("\0", "");
			AppName = FileReader.inip.ReadString("Public", "AppName", "");
			AppName = AppName.Replace("\0", "");
			VerNum = FileReader.inip.ReadString("版本号", "VerNum", "");
			VerNum = VerNum.Substring(0, 4);
			int width = FileReader.inip.ReadInteger("Individuation", "listwidth", 200);
			dockPanel1.Width = width;
			MapPath = FileReader.inip.ReadString("Individuation", "mappath", "");
			MapPath = MapPath.Replace("\0", "");
            if (MapPath == "")
                MapPath = WorkPath + "googlemap";
			FileReader.inip = new IniOperator(WorkPath + "SyncInfo.ini");
			string text = FileReader.inip.ReadString("YUNLogin", "host", "");
			/*FileReader.inip = new IniOperator(WorkPath + "Loginconfig.ini");
			string text2 = FileReader.inip.ReadString("login", "perid", "");
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\mapdata.accdb", "zbxh2012base518");
			string sql = "select PERSONRAND from RAND_PERSONINFO where PERSONID = " + text2;
			DataTable dataTable = FileReader.once_ahp.ExecuteDataTable(sql);
			if (dataTable.Rows.Count > 0)
			{
				text2 = dataTable.Rows[0]["PERSONRAND"].ToString();
			}
			FileReader.inip.WriteString("login", "randid", text2);
			FileReader.once_ahp.CloseConn();*/
			ProgName = UnitName + AppName + "trans";
			Process[] processesByName = Process.GetProcessesByName("TransMessage");
			Process[] array = processesByName;
			foreach (Process process2 in array)
			{
				process2.Kill();
			}
            processesByName = Process.GetProcessesByName("VideoConn");
            array = processesByName;
            foreach (Process process2 in array)
            {
                process2.Kill();
            }
            TransMessage = Process.Start(WorkPath + "TransMessage.exe");
			
			Text = UnitName + AppName + VerNum;
			FileReader.often_ahp = new AccessHelper(AccessPath);
			FileReader.line_ahp = new AccessHelper(WorkPath + "data\\经纬度注册.mdb");
			FileReader.log_ahp = new AccessHelper(WorkPath + "data\\ENVIRLOG_H0001Z000E00.mdb");
			FileReader.list_ahp = new AccessHelper(WorkPath + "data\\ENVIRLIST_H0001Z000E00.mdb");
			FileReader.inip = new IniOperator(WorkPath + "RegInfo.ini");
			UnitID = FileReader.inip.ReadString("Public", "UnitCounty", "-1");
			UnitReal = FileReader.inip.ReadString("Public", "UnitID", "-1");
			AccessHelper accessHelper = new AccessHelper(WorkPath + "data\\PASSWORD_H0001Z000E00.mdb");
			string sql = "select AUTHORITY from PASSWORD_H0001Z000E00 where ISDELETE = 0 and PWNAME = '管理员密码' and UNITID = '" + UnitID + "'";
			DataTable dataTable = accessHelper.ExecuteDataTable(sql, (OleDbParameter[])null);
			string text3 = "";
			if (dataTable.Rows.Count > 0)
			{
				text3 = dataTable.Rows[0]["AUTHORITY"].ToString();
			}
			FileReader.Authority = text3.Split(';');
			accessHelper.CloseConn();
			Permission = false;
			barButtonItem14.Visibility = BarItemVisibility.Never;
			barButtonItem15.Visibility = BarItemVisibility.Never;
			barButtonItem16.Visibility = BarItemVisibility.Never;
			barButtonItem17.Visibility = BarItemVisibility.Never;
			barButtonItem18.Visibility = BarItemVisibility.Never;
			barButtonItem8.Visibility = BarItemVisibility.Never;
			barButtonItem9.Visibility = BarItemVisibility.Never;
			barButtonItem10.Visibility = BarItemVisibility.Never;
			barButtonItem11.Visibility = BarItemVisibility.Never;
			barButtonItem12.Visibility = BarItemVisibility.Never;
			ShowMessage();
			Get_Computer_Info();
			folds = Get_Map_List();
			if (folds == null)
			{
				HideMessage();
				return;
			}
			Load_Unit_Level();
			Reg_Guid = new List<string>();
			Reg_Name = new Dictionary<string, string>();
			Reg_Down = new Dictionary<string, string>();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ZSK_H0001Z000K01.mdb");
			sql = "select PGUID, JDNAME, UPGUID from ZSK_OBJECT_H0001Z000K01 where ISDELETE = 0 order by LEVELNUM";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			string b = GL_NAME_PGUID[GL_List[0].level];
			bool flag = false;
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				string text4 = dataTable.Rows[j]["PGUID"].ToString();
				if (text4 == b)
				{
					flag = true;
				}
				if (flag)
				{
					Reg_Guid.Add(text4);
					Reg_Name[text4] = dataTable.Rows[j]["JDNAME"].ToString();
					Reg_Down[dataTable.Rows[j]["UPGUID"].ToString()] = text4;
				}
			}
			regfm.Reg_Guid = Reg_Guid;
			regfm.Reg_Name = Reg_Name;
			regfm.Draw_Form();
			Icon_JDCode = new Dictionary<string, string>();
			Icon_Name = new Dictionary<string, string>();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ZSK_H0001Z000K00.mdb");
			sql = "select PGUID, JDNAME, JDCODE from ZSK_OBJECT_H0001Z000K00 where ISDELETE = 0 order by LEVELNUM, SHOWINDEX";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				string text4 = dataTable.Rows[j]["PGUID"].ToString();
				Icon_Name.Add(text4 + ".png", dataTable.Rows[j]["JDNAME"].ToString());
				Icon_JDCode.Add(text4, dataTable.Rows[j]["JDCODE"].ToString());
			}
			FileReader.once_ahp.CloseConn();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ZSK_H0001Z000E00.mdb");
			sql = "select PGUID, JDNAME, JDCODE from ZSK_OBJECT_H0001Z000E00 where ISDELETE = 0 order by LEVELNUM, SHOWINDEX";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
				string text4 = dataTable.Rows[j]["PGUID"].ToString();
				Icon_Name.Add(text4 + ".png", dataTable.Rows[j]["JDNAME"].ToString());
				Icon_JDCode.Add(text4, dataTable.Rows[j]["JDCODE"].ToString());
			}
			FileReader.once_ahp.CloseConn();
			FileReader.inip = new IniOperator(IniFilePath);
			string text5 = FileReader.inip.ReadString("mapproperties", "centerlat", "");
			string s = FileReader.inip.ReadString("mapproperties", "centerlng", "");
			if (text5 != "")
			{
				mapHelper1.centerlat = double.Parse(text5);
			}
			if (text5 != "")
			{
				mapHelper1.centerlng = double.Parse(s);
			}
			sql = "select LAT, LNG from ORGCENTERDATA where ISDELETE = 0 and UNITEID = '" + UnitReal + "'";
			dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				mapHelper1.centerlat = double.Parse(dataTable.Rows[0]["LAT"].ToString());
				mapHelper1.centerlng = double.Parse(dataTable.Rows[0]["LNG"].ToString());
			}
			mapHelper1.webpath = WorkPath + "googlemap";
			mapHelper1.roadmappath = MapPath + "\\roadmap";
			mapHelper1.satellitemappath = MapPath + "\\satellite_en";
			mapHelper1.iconspath = WorkPath + "ICONDER";
			mapHelper1.maparr = folds;
			Load_Border(UnitReal);
			//Person_Marker = new Dictionary<string, string>();
			Person_GUID = new Dictionary<string, Map_Person>();
			sql = "select PGUID, FLNAME from ENVIRGXFL_H0001Z000E00 where ISDELETE = 0 and UPGUID = '-1' and UNITID = '" + UnitID + "' order by SHOWINDEX";
			dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int j = 0; j < dataTable.Rows.Count; j++)
			{
                BarButtonItem barButtonItem = new BarButtonItem
                {
                    Caption = dataTable.Rows[j]["FLNAME"].ToString(),
                    Tag = dataTable.Rows[j]["PGUID"].ToString()
                };
                barButtonItem.ItemClick += MenuStripItem_Click;
				bar2.InsertItem(bar2.ItemLinks[0], barButtonItem);
			}
			//radioButton1.Checked = true;
			Get_All_Icon();
			Get_All_Marker();
			if (levelguid == string.Empty)
			{
				mapHelper1.ShowMap(cur_Level, cur_Level.ToString(), canEdit: false, map_type, null, null, null, 1.0, 400);
				HideMessage();
				return;
			}
			if (bar2.ItemLinks[0].Item.Tag != null)
			{
				BarButtonItem item = (BarButtonItem)bar2.ItemLinks[0].Item;
				if (bar1.ItemLinks.Count == 0)
				{
					FLguid = "";
					Get_Marker_From_Access();
				}
				MenuStripItem_Click(barManager1, new ItemClickEventArgs(item, bar2.ItemLinks[0]));
			}
			else
			{
				FLguid = "";
				Get_Marker_From_Access();
			}
			HideMessage();
		}

		private void Get_All_Marker()
		{
			GUID_Icon = new Dictionary<string, string>();
			GUID_Name = new Dictionary<string, string>();
			List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
			string sql = "select * from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and UNITEID = '" + UnitID + "'";
			DataTable dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				GUID_Icon[dataTable.Rows[i]["PGUID"].ToString()] = dataTable.Rows[i]["ICONGUID"].ToString();
				GUID_Name[dataTable.Rows[i]["PGUID"].ToString()] = dataTable.Rows[i]["MAKRENAME"].ToString();
                string text = WorkPath + "ICONDER\\b_PNGICON\\" + dataTable.Rows[i]["ICONGUID"].ToString() + ".png";
                text = text.Replace('\\', '/');
                Dictionary<string, object> dictionary = new Dictionary<string, object>
                {
                    { "guid", dataTable.Rows[i]["PGUID"].ToString() },
                    { "name", dataTable.Rows[i]["MAKRENAME"].ToString() },
                    { "level", "-1" },
                    { "canedit", dataTable.Rows[i]["UNITEID"].ToString() == UnitID.ToString() },
                    { "type", dataTable.Rows[i]["MARKETYPE"].ToString() },
                    { "lat", dataTable.Rows[i]["MARKELAT"].ToString() },
                    { "lng", dataTable.Rows[i]["MARKELNG"].ToString() },
                    { "iconpath", text },
                    { "message", null },
                    { "topoint", null }
                };
                list.Add(dictionary);
			}
			cur_lst = list;
		}

		private void Get_All_Icon()
		{
			flowLayoutPanel1.Controls.Clear();
			string str = WorkPath + "ICONDER\\s_PNGICON\\";
			foreach (KeyValuePair<string, string> item in Icon_Name)
			{
				string key = item.Key;
				if (File.Exists(str + key))
				{
					PictureBox pictureBox = new PictureBox();
                    ToolTip toolTip = new ToolTip
                    {
                        InitialDelay = 10,
                        ReshowDelay = 10,
                        AutoPopDelay = 10000
                    };
                    toolTip.SetToolTip(pictureBox, Icon_Name[key]);
					pictureBox.Width = 32;
					pictureBox.Height = 32;
					pictureBox.Click += Icon_Click;
					pictureBox.BorderStyle = BorderStyle.Fixed3D;
					if (Permission)
					{
						pictureBox.MouseDown += Icon_MouseDown;
						pictureBox.MouseMove += Icon_MouseMove;
						pictureBox.MouseUp += Icon_MouseUp;
					}
					pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
					pictureBox.Name = key;
					FileStream fileStream = new FileStream(str + key, FileMode.Open, FileAccess.Read);
					pictureBox.Image = Image.FromStream(fileStream);
					flowLayoutPanel1.Controls.Add(pictureBox);
					fileStream.Close();
					fileStream.Dispose();
				}
			}
			PictureBox pictureBox2 = new PictureBox();
            ToolTip toolTip2 = new ToolTip
            {
                InitialDelay = 10,
                ReshowDelay = 10,
                AutoPopDelay = 10000
            };
            toolTip2.SetToolTip(pictureBox2, "全选");
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox2.BorderStyle = BorderStyle.Fixed3D;
			pictureBox2.Width = 32;
			pictureBox2.Height = 32;
			pictureBox2.Click += SelectAll_Click;
			pictureBox2.Name = "全选";
			FileStream fileStream2 = new FileStream(WorkPath + "icon\\全选.png", FileMode.Open, FileAccess.Read);
			pictureBox2.Image = Image.FromStream(fileStream2);
			flowLayoutPanel1.Controls.Add(pictureBox2);
			fileStream2.Close();
			fileStream2.Dispose();
			pictureBox2 = new PictureBox();
            toolTip2 = new ToolTip
            {
                InitialDelay = 10,
                ReshowDelay = 10,
                AutoPopDelay = 10000
            };
            toolTip2.SetToolTip(pictureBox2, "全不选");
			pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox2.Width = 32;
			pictureBox2.Height = 32;
			pictureBox2.Click += CancelAll_Click;
			pictureBox2.Name = "全不选";
			fileStream2 = new FileStream(WorkPath + "icon\\全不选.png", FileMode.Open, FileAccess.Read);
			pictureBox2.Image = Image.FromStream(fileStream2);
			flowLayoutPanel1.Controls.Add(pictureBox2);
			fileStream2.Close();
			fileStream2.Dispose();
		}

		private void Get_Computer_Info()
		{
			ComputerInfo.UserName = "";
			ComputerInfo.OSName = Environment.UserName;
			ComputerInfo.UnitID = UnitID;
			Get_Address();
		}

		private void Get_Address()
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			NetworkInterface[] array = allNetworkInterfaces;
			foreach (NetworkInterface networkInterface in array)
			{
				IPInterfaceProperties iPProperties = networkInterface.GetIPProperties();
				UnicastIPAddressInformationCollection unicastAddresses = iPProperties.UnicastAddresses;
				if (unicastAddresses.Count <= 0 || networkInterface.OperationalStatus != OperationalStatus.Up)
				{
					continue;
				}
				text = networkInterface.GetPhysicalAddress().ToString();
				foreach (UnicastIPAddressInformation item in unicastAddresses)
				{
					if (item.Address.AddressFamily == AddressFamily.InterNetwork)
					{
						text2 = item.Address.ToString();
					}
					if (item.Address.AddressFamily == AddressFamily.InterNetworkV6)
					{
						text3 = item.Address.ToString();
					}
				}
				if (string.IsNullOrWhiteSpace(text) || (string.IsNullOrWhiteSpace(text2) && string.IsNullOrWhiteSpace(text3)))
				{
					text = "";
					text2 = "";
					text3 = "";
					continue;
				}
				if (text.Length == 12)
				{
					text = $"{text.Substring(0, 2)}-{text.Substring(2, 2)}-{text.Substring(4, 2)}-{text.Substring(6, 2)}-{text.Substring(8, 2)}-{text.Substring(10, 2)}";
				}
				break;
			}
			ComputerInfo.PhyAddr = text;
			ComputerInfo.IPv4 = text2;
			ComputerInfo.IPv6 = text3;
		}

		private string[] Get_Map_List()
		{
			string[] array = null;
			if (!Directory.Exists(MapPath + "//roadmap"))
			{
				XtraMessageBox.Show("未导入地图文件!请在数据管理菜单导入地图");
				return null;
			}
			array = Directory.GetDirectories(MapPath + "//roadmap");
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
			GL_POLY = new Dictionary<string, Dictionary<string, Polygon>>();
			Level_Icon = new Dictionary<string, List<string>>();
			GX_Icon = new Dictionary<string, List<string>>();
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
				sql = "select MAPLEVEL from MAPDUIYING_H0001Z000E00 where ISDELETE = 0 and LEVELGUID = '" + text + "' and UNITEID = '" + UnitID + "'";
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
			sql = "select LEVELGUID, ICONGUID from ICONDUIYING_H0001Z000E00 where ISDELETE = 0 and UNITEID = '" + UnitID + "'";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				if (!Level_Icon.ContainsKey(dataTable.Rows[i]["LEVELGUID"].ToString()))
				{
					Level_Icon[dataTable.Rows[i]["LEVELGUID"].ToString()] = new List<string>();
				}
				Level_Icon[dataTable.Rows[i]["LEVELGUID"].ToString()].Add(dataTable.Rows[i]["ICONGUID"].ToString());
			}
			FileReader.once_ahp.CloseConn();
			sql = "select ICONGUID, FLGUID from ENVIRGXDY_H0001Z000E00 where ISDELETE = 0 and UNITID = '" + UnitID + "'";
			dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				if (!GX_Icon.ContainsKey(dataTable.Rows[i]["FLGUID"].ToString()))
				{
					GX_Icon[dataTable.Rows[i]["FLGUID"].ToString()] = new List<string>();
				}
				GX_Icon[dataTable.Rows[i]["FLGUID"].ToString()].Add(dataTable.Rows[i]["ICONGUID"].ToString());
			}
			GL_List = new List<GL_Node>();
			treeList1.Nodes.Clear();
			treeList1.Appearance.FocusedCell.BackColor = Color.SteelBlue;
			treeList1.KeyFieldName = "pguid";
			treeList1.ParentFieldName = "upguid";
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\PersonMange.mdb");
			sql = "select PGUID, UPPGUID, ORGNAME, ULEVEL from RG_单位注册 where ISDELETE = 0 and PGUID = '" + UnitReal + "'";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				GL_Node gL_Node = new GL_Node();
				gL_Node.pguid = dataTable.Rows[i]["PGUID"].ToString();
				gL_Node.upguid = dataTable.Rows[i]["UPPGUID"].ToString();
				GL_UPGUID[gL_Node.pguid] = dataTable.Rows[i]["UPPGUID"].ToString();
				gL_Node.Name = dataTable.Rows[i]["ORGNAME"].ToString();
				gL_Node.level = dataTable.Rows[i]["ULEVEL"].ToString();
				gL_Node.lat = "-1";
				gL_Node.lng = "-1";
				gL_Node.reg = false;
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
			foreach (TreeListNode node in treeList1.Nodes)
			{
				string text = node["pguid"].ToString();
				sql = "select LAT, LNG from ORGCENTERDATA where ISDELETE = 0 and PGUID = '" + text + "'";
				dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["lat"] = double.Parse(dataTable.Rows[0]["LAT"].ToString());
					node["lng"] = double.Parse(dataTable.Rows[0]["LNG"].ToString());
				}
				sql = "select MAPLEVEL from ENVIRMAPDY_H0001Z000E00 where ISDELETE = 0 and PGUID = '" + text + "' and UNITID = '" + UnitID + "'";
				dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["maps"] = dataTable.Rows[0]["MAPLEVEL"].ToString();
				}
				sql = "select PGUID from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and MAKRENAME = '" + node["Name"].ToString() + "' and UNITEID = '" + UnitID + "'";
				dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["reg"] = true;
				}
				Refresh_Nodes(node);
			}
		}

		private void treeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
		{
		}

		private void Add_Unit_Node(GL_Node pa)
		{
			string sql = "select PGUID, UPPGUID, ORGNAME, ULEVEL from RG_单位注册 where ISDELETE = 0 and UPPGUID = '" + pa.pguid + "'";
			DataTable dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
                GL_Node gL_Node = new GL_Node
                {
                    pguid = dataTable.Rows[i]["PGUID"].ToString(),
                    upguid = dataTable.Rows[i]["UPPGUID"].ToString()
                };
                GL_UPGUID[gL_Node.pguid] = dataTable.Rows[i]["UPPGUID"].ToString();
				gL_Node.Name = dataTable.Rows[i]["ORGNAME"].ToString();
				gL_Node.level = dataTable.Rows[i]["ULEVEL"].ToString();
				gL_Node.lat = "-1";
				gL_Node.lng = "-1";
				gL_Node.maps = "";
				gL_Node.reg = false;
				GL_List.Add(gL_Node);
				Add_Unit_Node(gL_Node);
			}
		}

		private void Load_Border(string u_guid)
		{
			borList = new Dictionary<string, List<double[]>>();
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
			string sql = "select LAT, LNG, BORDERGUID from BORDERDATA where ISDELETE = 0 and UNITID = '" + u_guid + "' order by SHOWINDEX";
			DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string key = dataTable.Rows[i]["BORDERGUID"].ToString();
				if (borList.ContainsKey(key))
				{
					borList[key].Add(new double[2]
					{
						double.Parse(dataTable.Rows[i]["LAT"].ToString()),
						double.Parse(dataTable.Rows[i]["LNG"].ToString())
					});
					continue;
				}
                borList[key] = new List<double[]>
                {
                    new double[2]
                {
                    double.Parse(dataTable.Rows[i]["LAT"].ToString()),
                    double.Parse(dataTable.Rows[i]["LNG"].ToString())
                }
                };
            }
			if (GL_UPGUID.ContainsKey(u_guid))
			{
				if (dataTable.Rows.Count <= 0)
				{
					Load_Border(GL_UPGUID[u_guid]);
				}
				else
				{
					borderDic.Add("path", borList);
				}
			}
		}

		private void MenuStripItem_Click(object sender, ItemClickEventArgs e)
		{
			Operator_GUID = "";
			select_vector = false;
			FLguid = "";
			BarManager barManager = (BarManager)sender;
			foreach (BarItemLink itemLink in barManager.Bars[1].ItemLinks)
			{
				try
				{
					BarButtonItem barButtonItem = (BarButtonItem)itemLink.Item;
					if (barButtonItem.Border == BorderStyles.Style3D)
					{
						barButtonItem.Border = BorderStyles.NoBorder;
						string text = barButtonItem.Caption;
						int num = text.IndexOf(' ');
						if (num > 0)
						{
							text = text.Substring(0, num);
						}
						barButtonItem.Caption = text;
					}
				}
				catch
				{
					goto IL_00de;
				}
			}
			goto IL_00de;
			IL_00de:
			e.Item.Border = BorderStyles.Style3D;
			GXguid = e.Item.Tag.ToString();
			Load_Guan_Xia();
		}

		private void ToolStripItem_Click(object sender, ItemClickEventArgs e)
		{
			Operator_GUID = "";
			select_vector = false;
			BarManager barManager = (BarManager)sender;
			BarButtonItem barButtonItem = new BarButtonItem();
			foreach (BarItemLink itemLink in barManager.Bars[1].ItemLinks)
			{
				try
				{
					BarButtonItem barButtonItem2 = (BarButtonItem)itemLink.Item;
					if (barButtonItem2.Tag != null && barButtonItem2.Tag.ToString() == GXguid)
					{
						barButtonItem = barButtonItem2;
						goto IL_00bc;
					}
				}
				catch
				{
					goto IL_00bc;
				}
			}
			goto IL_00bc;
			IL_00bc:
			foreach (BarItemLink itemLink2 in barManager.Bars[0].ItemLinks)
			{
				BarButtonItem barButtonItem2 = (BarButtonItem)itemLink2.Item;
				if (barButtonItem2.Border == BorderStyles.Style3D)
				{
					barButtonItem2.Border = BorderStyles.NoBorder;
				}
			}
			e.Item.Border = BorderStyles.Style3D;
			FLguid = e.Item.Tag.ToString();
			string text = barButtonItem.Caption;
			int num = text.IndexOf(' ');
			if (num > 0)
			{
				text = text.Substring(0, num);
			}
			barButtonItem.Caption = text;
			if (levelguid == string.Empty)
			{
				mapHelper1.ShowMap(cur_Level, cur_Level.ToString(), canEdit: false, map_type, null, null, null, 1.0, 400);
				return;
			}
			FLguid = e.Item.Tag.ToString();
			Get_Icon_List();
			string str = WorkPath + "ICONDER\\b_PNGICON\\";
			foreach (PictureBox control in flowLayoutPanel1.Controls)
			{
				if (control.Visible)
				{
					if (control.BorderStyle == BorderStyle.Fixed3D)
					{
						string text2 = str + control.Name;
						text2 = text2.Replace('\\', '/');
						mapHelper1.SetMarkerVisibleByIconPath(text2, visible: true);
					}
					else if (control.BorderStyle == BorderStyle.None)
					{
						string text2 = str + control.Name;
						text2 = text2.Replace('\\', '/');
						mapHelper1.SetMarkerVisibleByIconPath(text2, visible: false);
					}
				}
				else
				{
					string text2 = str + control.Name;
					text2 = text2.Replace('\\', '/');
					mapHelper1.SetMarkerVisibleByIconPath(text2, visible: false);
				}
			}
		}

		private void Load_Guan_Xia()
		{
			bar1.BeginUpdate();
			bar1.ClearLinks();
			bar1.Offset = 0;
			bar1.ApplyDockRowCol();
			string sql = "select PGUID, FLNAME from ENVIRGXFL_H0001Z000E00 where ISDELETE = 0 and UPGUID = '" + GXguid + "' and UNITID = '" + UnitID + "' order by SHOWINDEX";
			DataTable dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				BarButtonItem barButtonItem = new BarButtonItem();
				barButtonItem.Caption = dataTable.Rows[i]["FLNAME"].ToString();
				barButtonItem.Tag = dataTable.Rows[i]["PGUID"].ToString();
				barButtonItem.ItemClick += ToolStripItem_Click;
				bar1.AddItem(barButtonItem);
			}
			bar1.EndUpdate();
			if (bar1.ItemLinks.Count > 0)
			{
				ToolStripItem_Click(barManager1, new ItemClickEventArgs(bar1.ItemLinks[0].Item, bar1.ItemLinks[0]));
			}
		}

		private bool Icon_Reg(string pguid)
		{
			regfm.levelid = GL_NAME_PGUID[GL_List[0].level];
			regfm.unitid = UnitID;
			regfm.nodeid = pguid;
			return regfm.ShowDialog() == DialogResult.OK;
		}

		private int DrawBorder()
		{
            
            int num = 0;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string dlabel = "fb66d40b-50fa-4d88-8156-c590328004cb";
            mapHelper1.deleteMarker(dlabel);
            //string text = "e62844cb-2839-4b49-853a-250e11ec1901";
            dictionary["color"] = borData.Color;
			dictionary["weight"] = 0;
			dictionary["fillColor"] = "#C0C0C0";
			dictionary["fillOpacity"] = 0.5;
			if (borList.Count > 1)
			{
				dictionary["fillColor"] = "#CCFF33";
				foreach (KeyValuePair<string, List<double[]>> bor in borList)
				{
					dictionary["points"] = bor.Value;
				}
			}
			else
			{
				foreach (KeyValuePair<string, List<double[]>> bor2 in borList)
				{
					dictionary["points"] = bor2.Value;
					mapHelper1.DarkOuter(dlabel, dictionary);
				}
			}
			TreeListNode focusedNode = treeList1.FocusedNode;
			string pguid = focusedNode.GetValue("pguid").ToString();
			Dictionary<string, List<double[]>> dictionary2 = Get_Border_Line(pguid);
			foreach (KeyValuePair<string, List<double[]>> item in dictionary2)
			{
				Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
				dictionary3["type"] = borData.Type;
				dictionary3["width"] = borData.Width;
				dictionary3["color"] = borData.Color;
				dictionary3["opacity"] = borData.Opacity;
				dictionary3["path"] = item.Value;
				mapHelper1.DrawBorder("边界线", dictionary3);
				num++;
			}
			foreach (TreeListNode node in focusedNode.Nodes)
			{
				string pguid2 = node.GetValue("pguid").ToString();
				dictionary2 = Get_Border_Line(pguid2);
				foreach (KeyValuePair<string, List<double[]>> item2 in dictionary2)
				{
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3["type"] = "实线";
					dictionary3["width"] = 1;
					dictionary3["color"] = "#8B0000";
					dictionary3["opacity"] = borData.Opacity;
					dictionary3["path"] = item2.Value;
					mapHelper1.DrawBorder("边界线", dictionary3);
					num++;
				}
			}
			return num;
		}

		private void EraseBorder()
		{
			for (int i = 0; i < borderlines; i++)
			{
				mapHelper1.deleteMarker("边界线");
			}
		}

		private Dictionary<string, List<double[]>> Get_Border_Line(string pguid)
		{
			string text = pguid;
			Dictionary<string, List<double[]>> dictionary = new Dictionary<string, List<double[]>>();
			string sql = "select LAT, LNG, BORDERGUID from BORDERDATA where ISDELETE = 0 and UNITID = '" + pguid + "' order by SHOWINDEX";
			DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string key = dataTable.Rows[i]["BORDERGUID"].ToString();
				if (dictionary.ContainsKey(key))
				{
					dictionary[key].Add(new double[2]
					{
						double.Parse(dataTable.Rows[i]["LAT"].ToString()),
						double.Parse(dataTable.Rows[i]["LNG"].ToString())
					});
					continue;
				}
				dictionary[key] = new List<double[]>();
				dictionary[key].Add(new double[2]
				{
					double.Parse(dataTable.Rows[i]["LAT"].ToString()),
					double.Parse(dataTable.Rows[i]["LNG"].ToString())
				});
			}
			while (dictionary.Count <= 0)
			{
				if (!GL_UPGUID.ContainsKey(text))
				{
					return dictionary;
				}
				text = GL_UPGUID[text];
				dictionary = Get_Border_Line(text);
			}
			if (!GL_POLY.ContainsKey(pguid))
			{
				GL_POLY[pguid] = new Dictionary<string, Polygon>();
			}
			foreach (KeyValuePair<string, List<double[]>> item in dictionary)
			{
				GL_POLY[pguid][item.Key] = new Polygon(item.Value);
			}
			return dictionary;
		}

        private void mapHelper1_AddMarkerFinished(string markerguid, double lat, double lng, string name, bool canEdit, string iconpath, string message)
        {
            if (iconpath == WorkPath + "icon\\人.png")
            {
                return;
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>
            {
                { "guid", markerguid },
                { "name", name },
                { "level", cur_Level.ToString() },
                { "canedit", canEdit },
                { "type", "标注" },
                { "lat", lat.ToString() },
                { "lng", lng.ToString() },
                { "iconpath", iconpath },
                { "message", null },
                { "topoint", null }
            };
            cur_lst.Add(dictionary);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(iconpath);
			string sql = "insert into ENVIRICONDATA_H0001Z000E00 (PGUID, S_UDTIME, ICONGUID, LEVELGUID, MAPLEVEL, MARKELAT, MARKELNG, MAKRENAME, UNITEID, REGINFO, REGGUID) values('" + markerguid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + fileNameWithoutExtension + "', '" + levelguid + "', '" + cur_Level.ToString() + "', '" + lat.ToString() + "', '" + lng.ToString() + "', '" + name + "', '" + UnitID.ToString() + "', '" + regfm.regaddr + "', '" + regfm.regguid + "')";
			FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
			GUID_Icon[markerguid] = fileNameWithoutExtension;
			GUID_Name[markerguid] = name;
			string str = Icon_JDCode[fileNameWithoutExtension];
			sql = "insert into " + str + " (PGUID, S_UDTIME";
			foreach (string key in FDName_Value.Keys)
			{
				sql = sql + ", " + key;
			}
			string text = sql;
			sql = text + ") values('" + markerguid + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			foreach (string value in FDName_Value.Values)
			{
				sql = sql + "', '" + value;
			}
			sql += "')";
			FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
			string remark = "添加" + Icon_Name[fileNameWithoutExtension + ".png"] + "标注" + name + "到(" + lng.ToString() + ", " + lat.ToString() + ")";
			ComputerInfo.WriteLog("添加标注", remark);
		}

		private void mapHelper1_ModifyMarkerFinished(string markerguid, double lat, double lng, string name, bool canEdit, string iconpath, string message)
		{
			string sql = "update ENVIRICONDATA_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', MAKRENAME = '" + name + "', MARKELAT = '" + lat.ToString() + "', MARKELNG = '" + lng.ToString() + "' where ISDELETE = 0 and PGUID = '" + markerguid + "' and UNITEID = '" + UnitID + "'";
			FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
			string text = GUID_Icon[markerguid];
			string text2 = Icon_JDCode[text];
			if (FDName_Value != null)
			{
				sql = "update " + text2 + " set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
				foreach (KeyValuePair<string, string> item in FDName_Value)
				{
					string text3 = sql;
					sql = text3 + ", " + item.Key + " = '" + item.Value + "'";
				}
				sql = sql + " where ISDELETE = 0 and PGUID = '" + markerguid + "'";
			}
			FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
			string remark = "编辑" + Icon_Name[text + ".png"] + "标注" + name + "的属性";
			ComputerInfo.WriteLog("编辑标注属性", remark);
		}

		private void mapHelper1_RemoveMarkerFinished(string markerguid, bool ok)
		{
		}

		private void UpdateDelete(string markerguid)
		{
			if (markerguid.IndexOf("_arrow") <= 0)
			{
				if (markerguid.IndexOf("_line") > 0)
				{
					string text = markerguid.Substring(0, 32);
					for (int i = 0; i < cur_lst.Count; i++)
					{
						if (cur_lst[i]["guid"].ToString() == text)
						{
							cur_lst[i]["topoint"] = null;
							break;
						}
					}
					string sql = "update ENVIRICONDATA_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', POINTLNG = '', POINTLAT = '', POINTLINE = 0, POINTARROW = 0 where ISDELETE = 0 and PGUID = '" + text + "' and UNITEID = '" + UnitID + "'";
					FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
					sql = "update ENVIRLINE_H0001Z000E00 set ISDELETE = 1, S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ISDELETE = 0 and PGUID = '" + text + "'";
					FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
					if (handle != 2)
					{
						string str = GUID_Icon[text];
						string text2 = GUID_Name[text];
						string remark = "删除" + Icon_Name[str + ".png"] + "标注" + text2 + "的指向位置";
						ComputerInfo.WriteLog("删除标注指向位置", remark);
					}
				}
				else
				{
					for (int i = 0; i < cur_lst.Count; i++)
					{
						if (cur_lst[i]["guid"].ToString() == markerguid)
						{
							cur_lst.RemoveAt(i);
							break;
						}
					}
					string sql = "update ENVIRICONDATA_H0001Z000E00 set ISDELETE = 1, S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ISDELETE = 0 and PGUID = '" + markerguid + "' and UNITEID = '" + UnitID + "'";
					FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
					string str = GUID_Icon[markerguid];
					string text2 = GUID_Name[markerguid];
					string text3 = Icon_JDCode[str];
					sql = "update " + text3 + " set ISDELETE = 1, S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ISDELEtE = 0 and PGUID = '" + markerguid + "'";
					FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
					string remark = "删除" + Icon_Name[str + ".png"] + "标注" + text2;
					ComputerInfo.WriteLog("删除标注", remark);
				}
			}
			treeList1.Focus();
			mapHelper1.Focus();
			last_marker = "";
		}

		private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
		{
			TreeListNode focusedNode = treeList1.FocusedNode;
			Operator_GUID = "";
			select_vector = false;
			if (e.Node == null)
			{
				return;
			}
			levelguid = GL_NAME_PGUID[focusedNode["level"].ToString()];
			string[] array = null;
			array = ((focusedNode["maps"] != null && !(focusedNode["maps"].ToString() == "")) ? focusedNode["maps"].ToString().Split(',') : GL_MAP[levelguid].Split(','));
			if (array[0] != string.Empty)
			{
				cur_Level = int.Parse(array[0]);
			}
			else
			{
				cur_Level = 0;
			}
			//label1.Text = "当前级别：" + GL_NAME[levelguid];
			Load_Border(e.Node.GetValue("pguid").ToString());
			if (!GL_POLY.ContainsKey(e.Node.GetValue("pguid").ToString()))
			{
				GL_POLY[e.Node.GetValue("pguid").ToString()] = new Dictionary<string, Polygon>();
			}
			foreach (KeyValuePair<string, List<double[]>> bor in borList)
			{
				GL_POLY[e.Node.GetValue("pguid").ToString()][bor.Key] = new Polygon(bor.Value);
			}
			bool flag = false;
			double num = double.Parse(focusedNode["lat"].ToString());
			double num2 = double.Parse(focusedNode["lng"].ToString());
			if (num > 0.0 && num2 > 0.0)
			{
				if (Before_ShowMap)
				{
					mapHelper1.centerlat = num;
					mapHelper1.centerlng = num2;
				}
			}
			else
			{
				string sql = "select MARKELAT, MARKELNG from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and MAKRENAME like '%" + e.Node.GetValue("Name").ToString() + "%' and UNITEID = '" + UnitID + "'";
				DataTable dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
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
			}
			if (Icon_Name != null)
			{
				Get_Marker_From_Access();
			}
		}

		private void Get_Marker_From_Access()
		{
			string str = WorkPath + "ICONDER\\b_PNGICON\\";
			Get_Icon_List();
			if (folds.Contains(cur_Level.ToString()))
			{
				if (cur_Level != now_Level || !Before_ShowMap)
				{
					if (!Before_ShowMap)
					{
						mapHelper1.ShowMap(cur_Level, GL_NAME[levelguid], Permission, map_type, Icon_Name, null, cur_lst, 1.0, 400);
						Before_ShowMap = true;
					}
					else
					{
						now_Level = cur_Level;
						if (ifm != null)
						{
							ifm.Close();
						}
						mapHelper1.setMapLevel(cur_Level, "");
						mapHelper1.SetMapCenter(mapHelper1.centerlat, mapHelper1.centerlng);
					}
				}
				else
				{
					mapHelper1.SetMapCenter(mapHelper1.centerlat, mapHelper1.centerlng);
				}
				EraseBorder();
				borderlines = DrawBorder();
				foreach (PictureBox control in flowLayoutPanel1.Controls)
				{
					if (!(control.Name == "全选") && !(control.Name == "全不选"))
					{
						string text = str + control.Name;
						text = text.Replace('\\', '/');
						if (control.BorderStyle == BorderStyle.Fixed3D && control.Tag.ToString() == "1")
						{
							mapHelper1.SetMarkerVisibleByIconPath(text, visible: true);
						}
						else
						{
							mapHelper1.SetMarkerVisibleByIconPath(text, visible: false);
						}
					}
				}
			}
			else
			{
				mapHelper1.ShowMap(cur_Level, cur_Level.ToString(), canEdit: false, map_type, null, null, null, 1.0, 400);
			}
		}

		private void Get_Icon_List()
		{
			string text = WorkPath + "ICONDER\\b_PNGICON\\";
            System.Collections.IList list = flowLayoutPanel1.Controls;
            for (int i = 0; i < list.Count; i++)
			{
                PictureBox control = (PictureBox)list[i];
                control.Visible = control.Name == "全选" || control.Name == "全不选" || Check_Icon(control.Name) ? true : false;
                control.Tag = control.Name == "全选" || control.Name == "全不选" || Check_Icon(control.Name) ? "1" : "0";
            }
		}

		private bool Check_Icon(string iconguid)
		{
			if (!Level_Icon.ContainsKey(levelguid))
			{
				return false;
			}
			List<string> list = Level_Icon[levelguid];
			if (FLguid != "")
			{
				if (!GX_Icon.ContainsKey(FLguid))
				{
					return false;
				}
				list = list.Intersect(GX_Icon[FLguid]).ToList();
			}
			if (list.Contains(iconguid.Substring(0, 38)))
			{
				return true;
			}
			return false;
		}

		private void SelectAll_Click(object sender, EventArgs e)
		{
			string str = WorkPath + "ICONDER\\b_PNGICON\\";
			foreach (PictureBox control in flowLayoutPanel1.Controls)
			{
				if (control.Visible)
				{
					if (control.Name == "全不选")
					{
						control.BorderStyle = BorderStyle.None;
					}
					else
					{
						control.BorderStyle = BorderStyle.Fixed3D;
						if (control.Name != "全选")
						{
							string text = str + control.Name;
							text = text.Replace('\\', '/');
							mapHelper1.SetMarkerVisibleByIconPath(text, visible: true);
						}
					}
				}
			}
		}

		private void CancelAll_Click(object sender, EventArgs e)
		{
			string str = WorkPath + "ICONDER\\b_PNGICON\\";
			foreach (PictureBox control in flowLayoutPanel1.Controls)
			{
				if (control.Name == "全不选")
				{
					control.BorderStyle = BorderStyle.Fixed3D;
				}
				else
				{
					control.BorderStyle = BorderStyle.None;
					if (control.Name != "全选")
					{
						string text = str + control.Name;
						text = text.Replace('\\', '/');
						mapHelper1.SetMarkerVisibleByIconPath(text, visible: false);
					}
				}
			}
		}

		private void Icon_MouseDown(object sender, MouseEventArgs e)
		{
			PictureBox pictureBox = sender as PictureBox;
			Point mousePosition = Control.MousePosition;
			startPoint = pictureBox.Parent.PointToClient(mousePosition);
			currCtl.Parent = dockPanel1;
			currCtl.Left = pictureBox.Left;
			currCtl.Top = pictureBox.Top;
			currCtl.Image = pictureBox.Image;
			currCtl.ImageLocation = pictureBox.ImageLocation;
			currCtl.Visible = true;
		}

		private void Icon_MouseMove(object sender, MouseEventArgs e)
		{
			if (startPoint.X >= 0 && e.Button.ToString().Equals("Left"))
			{
				Point mousePosition = Control.MousePosition;
				Point point = currCtl.Parent.PointToClient(mousePosition);
				int num = point.X - startPoint.X;
				int num2 = point.Y - startPoint.Y;
				currCtl.Left += num;
				currCtl.Top += num2;
				startPoint = point;
				if (currCtl.Top > mapHelper1.Top)
				{
					currCtl.Parent = mapHelper1.wb1;
				}
				else
				{
                    currCtl.Parent = panelControl1;
				}
			}
		}

		private void Icon_MouseUp(object sender, MouseEventArgs e)
		{
			if (currCtl.Top >= panelControl1.Bottom)
			{
				PictureBox pictureBox = (PictureBox)sender;
				string str = WorkPath + "ICONDER\\b_PNGICON\\";
				startPoint.X = -100;
				startPoint.Y = -100;
				currCtl.Parent = panelControl1;
				currCtl.Visible = false;
				Icon_GUID = str + pictureBox.Name;
				Icon_GUID = Icon_GUID.Replace('\\', '/');
				mapHelper1.SetBigIconPath(str + pictureBox.Name);
			}
		}

		private void Icon_Click(object sender, EventArgs e)
		{
			string str = WorkPath + "ICONDER\\b_PNGICON\\";
			currCtl.Visible = false;
			PictureBox pictureBox = (PictureBox)sender;
			if (pictureBox.BorderStyle == BorderStyle.Fixed3D)
			{
				pictureBox.BorderStyle = BorderStyle.None;
				string text = str + pictureBox.Name;
				text = text.Replace('\\', '/');
				mapHelper1.SetMarkerVisibleByIconPath(text, visible: false);
			}
			else if (pictureBox.BorderStyle == BorderStyle.None)
			{
				pictureBox.BorderStyle = BorderStyle.Fixed3D;
				string text = str + pictureBox.Name;
				text = text.Replace('\\', '/');
				mapHelper1.SetMarkerVisibleByIconPath(text, visible: true);
			}
			bool flag = true;
			bool flag2 = true;
			foreach (PictureBox control in flowLayoutPanel1.Controls)
			{
				if (control.Name == "全选")
				{
					if (flag)
					{
						control.BorderStyle = BorderStyle.Fixed3D;
					}
					else
					{
						control.BorderStyle = BorderStyle.None;
					}
				}
				else if (control.Name == "全不选")
				{
					if (flag2)
					{
						control.BorderStyle = BorderStyle.Fixed3D;
					}
					else
					{
						control.BorderStyle = BorderStyle.None;
					}
				}
				else if (control.BorderStyle == BorderStyle.Fixed3D)
				{
					flag2 = false;
				}
				else
				{
					flag = false;
				}
			}
		}

		private void mapHelper1_MapMouseOver(double lat, double lng)
		{
			if (Icon_GUID.Equals(""))
			{
				return;
			}
			ifm.Close();
			TreeListNode focusedNode = treeList1.FocusedNode;
			Dictionary<string, List<double[]>> dictionary = Get_Border_Line(focusedNode["pguid"].ToString());
			bool flag = false;
			foreach (KeyValuePair<string, List<double[]>> item in dictionary)
			{
				Polygon polygon = new Polygon(item.Value);
				if (polygon.PointInPolygon(new dPoint(lat, lng)))
				{
					flag = true;
					break;
				}
			}
			if (!flag && XtraMessageBox.Show("添加图符不在" + focusedNode["Name"].ToString() + "的范围内!是否继续添加?", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				Icon_GUID = "";
				mapHelper1.SetBigIconPath("");
				return;
			}
			regfm.unLock = false;
			regfm.StartPosition = FormStartPosition.Manual;
			regfm.textName = "";
			regfm.Left = Control.MousePosition.X + 20;
			regfm.Top = Control.MousePosition.Y + 20;
			if (regfm.Left + regfm.Width > base.Width)
			{
				regfm.Left -= regfm.Width + 20;
			}
			if (regfm.Top + regfm.Height > base.Height)
			{
				regfm.Top -= regfm.Height + 20;
			}
			if (Icon_Reg(focusedNode["pguid"].ToString()))
			{
				treeList1.Focus();
				mapHelper1.Focus();
				dPoint dPoint = new dPoint(0.0, 0.0);
				string pa_guid = regfm.regguid;
				if (GL_POLY.ContainsKey(pa_guid))
				{
					dPoint = GL_POLY[pa_guid][GL_POLY[pa_guid].Keys.First()].GetAPoint();
				}
				else
				{
					Get_Border_Line(pa_guid);
					dPoint = GL_POLY[pa_guid][GL_POLY[pa_guid].Keys.First()].GetAPoint();
				}
				while (Math.Abs(dPoint.x + dPoint.y) < 0.01)
				{
					GL_Node gL_Node = GL_List.Find((GL_Node x) => x.pguid == pa_guid);
					pa_guid = gL_Node.upguid;
					if (GL_POLY.ContainsKey(pa_guid))
					{
						dPoint = GL_POLY[pa_guid][GL_POLY[pa_guid].Keys.First()].GetAPoint();
						continue;
					}
					Get_Border_Line(pa_guid);
					dPoint = GL_POLY[pa_guid][GL_POLY[pa_guid].Keys.First()].GetAPoint();
				}
				string textName = regfm.textName;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Icon_GUID);
				DataForm dataForm = new DataForm();
				dataForm.Icon_GUID = fileNameWithoutExtension;
				dataForm.Update_Data = false;
				dataForm.Load_Prop();
				dataForm.ReNew();
				dataForm.Close_Conn();
				FDName_Value = dataForm.FDName_Value;
				Icon_GUID = Icon_GUID.Replace('\\', '/');
				if (focusedNode["pguid"].ToString() == regfm.regguid)
				{
					mapHelper1.addMarker(string.Concat(lat), string.Concat(lng), textName, canedit: true, Icon_GUID, null);
				}
				else
				{
					mapHelper1.SetMapCenter(dPoint.x, dPoint.y);
					mapHelper1.addMarker(string.Concat(dPoint.x), string.Concat(dPoint.y), textName, canedit: true, Icon_GUID, null);
				}
			}
			Icon_GUID = "";
			mapHelper1.SetBigIconPath("");
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
            Process[] processesByName = Process.GetProcessesByName("TransMessage");
            Process[] array = processesByName;
            foreach (Process process2 in array)
            {
                process2.Kill();
                process2.WaitForExit();
            }
            processesByName = Process.GetProcessesByName("VideoConn");
            array = processesByName;
            foreach (Process process2 in array)
            {
                process2.Kill();
                process2.WaitForExit();
            }
            if (Permission)
			{
				FileReader.inip = new IniOperator(WorkPath + "RegInfo.ini");
				FileReader.inip.WriteString("Individuation", "skin", UserLookAndFeel.Default.ActiveSkinName);
				FileReader.inip.WriteInteger("Individuation", "listwidth", dockPanel1.Width);
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
            Process[] processesByName = Process.GetProcessesByName("TransMessage");
            Process[] array = processesByName;
            foreach (Process process2 in array)
            {
                process2.Kill();
                process2.WaitForExit();
            }
            processesByName = Process.GetProcessesByName("VideoConn");
            array = processesByName;
            foreach (Process process2 in array)
            {
                process2.Kill();
                process2.WaitForExit();
            }
            FileReader.often_ahp.CloseConn();
			FileReader.line_ahp.CloseConn();
			FileReader.log_ahp.CloseConn();
			FileReader.list_ahp.CloseConn();
		}

		private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
		{
			Process process = Process.Start(WorkPath + "DataBF.exe");
			process.WaitForExit();
		}

		private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
		{
			Process process = Process.Start(WorkPath + "DataHF.exe");
			process.WaitForExit();
		}

		private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
		{
			Process process = Process.Start(WorkPath + "YUNEvrDataUp.exe", "EnvirInfoSys.exe 0 2");
			process.WaitForExit();
		}

		private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
		{
			Process process = Process.Start(WorkPath + "OrgDataDown.exe");
			process.WaitForExit();
			treeList1.Nodes.Clear();
			Load_Unit_Level();
		}

		private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
		{
			for (int i = 0; i < FileReader.Authority.Length; i++)
			{
				if (FileReader.Authority[i] == "服务器IP设置权限")
				{
					CheckPwForm checkPwForm = new CheckPwForm();
					checkPwForm.unitid = UnitID;
					if (checkPwForm.ShowDialog() != DialogResult.OK)
					{
						XtraMessageBox.Show("未能获取管理员权限");
						return;
					}
					break;
				}
			}
			Process process = Process.Start(WorkPath + "SetIP.exe");
			process.WaitForExit();
		}

		private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
		{
			for (int i = 0; i < FileReader.Authority.Length; i++)
			{
				if (FileReader.Authority[i] == "边界线属性设置权限")
				{
					CheckPwForm checkPwForm = new CheckPwForm();
					checkPwForm.unitid = UnitID;
					if (checkPwForm.ShowDialog() != DialogResult.OK)
					{
						XtraMessageBox.Show("未能获取管理员权限");
						return;
					}
					break;
				}
			}
			BorderForm borderForm = new BorderForm();
			borderForm.IsPoint = false;
			borderForm.IsLine = false;
			borderForm.borData.Load_Line("边界线");
			if (borderForm.borData.line_data == null)
			{
				borderForm.borData.line_data = borData;
			}
			if (borderForm.ShowDialog() == DialogResult.OK)
			{
				borData = borderForm.borData.line_data;
				if (levelguid != string.Empty)
				{
					borderDic["type"] = borData.Type;
					borderDic["width"] = borData.Width;
					borderDic["color"] = borData.Color;
					borderDic["opacity"] = borData.Opacity;
					borData.Save_Line("边界线");
					EraseBorder();
					borderlines = DrawBorder();
				}
				string remark = "修改边界线属性";
				ComputerInfo.WriteLog("边界线属性设置", remark);
			}
		}

		private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
		{
			for (int i = 0; i < FileReader.Authority.Length; i++)
			{
				if (FileReader.Authority[i] == "图符管理设置权限")
				{
					CheckPwForm checkPwForm = new CheckPwForm();
					checkPwForm.unitid = UnitID;
					if (checkPwForm.ShowDialog() != DialogResult.OK)
					{
						XtraMessageBox.Show("未能获取管理员权限");
						return;
					}
					break;
				}
			}
			Classify_1Form classify_1Form = new Classify_1Form();
			classify_1Form.unitid = UnitID;
			classify_1Form.gxguid = GXguid;
			classify_1Form.ShowDialog();
			string str = "";
			foreach (BarItemLink itemLink in bar2.ItemLinks)
			{
				try
				{
					BarButtonItem barButtonItem = (BarButtonItem)itemLink.Item;
					if (barButtonItem.Tag != null && barButtonItem.Tag.ToString() == GXguid)
					{
						str = barButtonItem.Caption;
						goto IL_0136;
					}
				}
				catch
				{
					goto IL_0136;
				}
			}
			goto IL_0136;
			IL_0136:
			FileReader.often_ahp.CloseConn();
			FileReader.often_ahp = new AccessHelper(AccessPath);
			Load_Guan_Xia();
			string remark = "修改" + str + "分类设置";
			ComputerInfo.WriteLog("管辖分类设置", remark);
			Level_Icon = new Dictionary<string, List<string>>();
			GX_Icon = new Dictionary<string, List<string>>();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ENVIRDYDATA_H0001Z000E00.mdb");
			string sql = "select LEVELGUID, ICONGUID from ICONDUIYING_H0001Z000E00 where ISDELETE = 0 and UNITEID = '" + UnitID + "'";
			DataTable dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				if (!Level_Icon.ContainsKey(dataTable.Rows[i]["LEVELGUID"].ToString()))
				{
					Level_Icon[dataTable.Rows[i]["LEVELGUID"].ToString()] = new List<string>();
				}
				Level_Icon[dataTable.Rows[i]["LEVELGUID"].ToString()].Add(dataTable.Rows[i]["ICONGUID"].ToString());
			}
			FileReader.once_ahp.CloseConn();
			sql = "select ICONGUID, FLGUID from ENVIRGXDY_H0001Z000E00 where ISDELETE = 0 and UNITID = '" + UnitID + "'";
			dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				if (!GX_Icon.ContainsKey(dataTable.Rows[i]["FLGUID"].ToString()))
				{
					GX_Icon[dataTable.Rows[i]["FLGUID"].ToString()] = new List<string>();
				}
				GX_Icon[dataTable.Rows[i]["FLGUID"].ToString()].Add(dataTable.Rows[i]["ICONGUID"].ToString());
			}
			Icon_JDCode = new Dictionary<string, string>();
			Icon_Name = new Dictionary<string, string>();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ZSK_H0001Z000K00.mdb");
			sql = "select PGUID, JDNAME, JDCODE from ZSK_OBJECT_H0001Z000K00 where ISDELETE = 0 order by LEVELNUM, SHOWINDEX";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["PGUID"].ToString();
				Icon_Name.Add(text + ".png", dataTable.Rows[i]["JDNAME"].ToString());
				Icon_JDCode.Add(text, dataTable.Rows[i]["JDCODE"].ToString());
			}
			FileReader.once_ahp.CloseConn();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ZSK_H0001Z000E00.mdb");
			sql = "select PGUID, JDNAME, JDCODE from ZSK_OBJECT_H0001Z000E00 where ISDELETE = 0 order by LEVELNUM, SHOWINDEX";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["PGUID"].ToString();
				Icon_Name.Add(text + ".png", dataTable.Rows[i]["JDNAME"].ToString());
				Icon_JDCode.Add(text, dataTable.Rows[i]["JDCODE"].ToString());
			}
			FileReader.once_ahp.CloseConn();
			Get_All_Icon();
			Get_Icon_List();
			Get_All_Marker();
			if (classify_1Form.NeedShowMap)
			{
				Before_ShowMap = false;
			}
			Get_Marker_From_Access();
		}

		private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
		{
			for (int i = 0; i < FileReader.Authority.Length; i++)
			{
				if (FileReader.Authority[i] == "图符对应设置权限")
				{
					CheckPwForm checkPwForm = new CheckPwForm();
					checkPwForm.unitid = UnitID;
					if (checkPwForm.ShowDialog() != DialogResult.OK)
					{
						XtraMessageBox.Show("未能获取管理员权限");
						return;
					}
					break;
				}
			}
			Classify_2Form classify_2Form = new Classify_2Form();
			classify_2Form.unitid = UnitID;
			classify_2Form.ShowDialog();
			Get_Marker_From_Access();
			string remark = "修改图符对应设置";
			ComputerInfo.WriteLog("图符对应设置", remark);
		}

		private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
		{
			for (int i = 0; i < FileReader.Authority.Length; i++)
			{
				if (FileReader.Authority[i] == "图符扩展设置权限")
				{
					CheckPwForm checkPwForm = new CheckPwForm();
					checkPwForm.unitid = UnitID;
					if (checkPwForm.ShowDialog() != DialogResult.OK)
					{
						XtraMessageBox.Show("未能获取管理员权限");
						return;
					}
					break;
				}
			}
			Process process = Process.Start(WorkPath + "tfkzdy.exe");
			process.WaitForExit();
			Get_Marker_From_Access();
			Icon_JDCode = new Dictionary<string, string>();
			Icon_Name = new Dictionary<string, string>();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ZSK_H0001Z000K00.mdb");
			string sql = "select PGUID, JDNAME, JDCODE from ZSK_OBJECT_H0001Z000K00 where ISDELETE = 0 order by LEVELNUM, SHOWINDEX";
			DataTable dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["PGUID"].ToString();
				Icon_Name.Add(text + ".png", dataTable.Rows[i]["JDNAME"].ToString());
				Icon_JDCode.Add(text, dataTable.Rows[i]["JDCODE"].ToString());
			}
			FileReader.once_ahp.CloseConn();
			FileReader.once_ahp = new AccessHelper(WorkPath + "data\\ZSK_H0001Z000E00.mdb");
			sql = "select PGUID, JDNAME, JDCODE from ZSK_OBJECT_H0001Z000E00 where ISDELETE = 0 order by LEVELNUM, SHOWINDEX";
			dataTable = FileReader.once_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["PGUID"].ToString();
				Icon_Name.Add(text + ".png", dataTable.Rows[i]["JDNAME"].ToString());
				Icon_JDCode.Add(text, dataTable.Rows[i]["JDCODE"].ToString());
			}
			FileReader.once_ahp.CloseConn();
		}

		private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
		{
			CheckPwForm checkPwForm = new CheckPwForm();
			checkPwForm.unitid = UnitID;
			if (checkPwForm.ShowDialog() != DialogResult.OK)
			{
				XtraMessageBox.Show("未能获取管理员权限");
				return;
			}
			PasswordForm passwordForm = new PasswordForm();
			passwordForm.ShowDialog();
			string remark = "修改密码管理设置";
			ComputerInfo.WriteLog("密码管理", remark);
		}

		private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
		{
			for (int i = 0; i < FileReader.Authority.Length; i++)
			{
				if (FileReader.Authority[i] == "查看日志权限")
				{
					CheckPwForm checkPwForm = new CheckPwForm();
					checkPwForm.unitid = UnitID;
					if (checkPwForm.ShowDialog() != DialogResult.OK)
					{
						XtraMessageBox.Show("未能获取管理员权限");
						return;
					}
					break;
				}
			}
			LogForm logForm = new LogForm();
			logForm.unitid = UnitID;
			logForm.ShowDialog();
		}

		private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
		{
			HelpForm helpForm = new HelpForm();
			helpForm.ShowDialog();
		}

		private void mapHelper1_IconSelected(string level, string iconPath)
		{
			Icon_GUID = iconPath;
		}

		private void StopMarkerShine(string pguid)
		{
			mapHelper1.deleteMarker(pguid + "_line");
			mapHelper1.SetMarkerShine(pguid, shine: false);
		}

		private void DrawLine(string markerguid)
		{
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            FileReader.once_ahp = new AccessHelper(WorkPath + "data\\MOB_H0001Z000E00.mdb");
            string sql = "select POINTLAT, POINTLNG from M_ICONPOSITION_H0001Z000E00 where ISDELETE = 0 and UPGUID = '" + markerguid + "' and UNITID = '" + UnitID + "'";
            DataTable dataTable = FileReader.once_ahp.ExecuteDataTable(sql, null);
            if (dataTable.Rows.Count > 0)
            {
                dictionary["lat"] = double.Parse(dataTable.Rows[0]["POINTLAT"].ToString());
                dictionary["lng"] = double.Parse(dataTable.Rows[0]["POINTLNG"].ToString());
            }
            else
            {
                return;
            }
            
            FileReader.once_ahp.CloseConn();
            double num = 0.0;
			double num2 = 0.0;
            sql = "select MARKELAT, MARKELNG from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and PGUID = '" + markerguid + "' and UNITEID = '" + UnitID + "'";
            dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
            if (dataTable.Rows.Count > 0)
            {
                num = double.Parse(dataTable.Rows[0]["MARKELAT"].ToString());
                num2 = double.Parse(dataTable.Rows[0]["MARKELNG"].ToString());
            }
            
			sql = "select LINETYPE, LINEWIDTH, LINECOLOR, LINEOPACITY from ENVIRLINE_H0001Z000E00 where ISDELETE = 0 and UPGUID = '" + markerguid + "'";
			dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
            if (dataTable.Rows.Count > 0)
            {
                dictionary["type"] = dataTable.Rows[0]["LINETYPE"].ToString();
                dictionary["width"] = int.Parse(dataTable.Rows[0]["LINEWIDTH"].ToString());
                dictionary["color"] = dataTable.Rows[0]["LINECOLOR"].ToString();
                dictionary["opacity"] = double.Parse(dataTable.Rows[0]["LINEOPACITY"].ToString());
                dictionary["arrow"] = false;
            }
            else
            {
                dictionary["type"] = "实线";
                dictionary["width"] = 1;
                dictionary["color"] = "#FF0000";
                dictionary["opacity"] = 1.0;
                dictionary["arrow"] = false;
            }
            mapHelper1.DrawPointLine(markerguid, num, num2, dictionary);
        }

		private static void SendMessage(string strText, int data, string FormName)
		{
			IntPtr hWnd = ImportFromDLL.FindWindow(null, FormName);
			IntPtr wParam = Process.GetCurrentProcess().Handle;
			ImportFromDLL.COPYDATASTRUCT pcd = default(ImportFromDLL.COPYDATASTRUCT);
			pcd.cbData = 1000;
			pcd.lpData = strText;
			pcd.dwData = data;
			ImportFromDLL.SendMessage(hWnd, 74, wParam, ref pcd);
		}

		private void mapHelper1_MapMouseup(string Mousebutton, bool canedit, double lat, double lng, int x, int y, string markerguid, string iconPath)
		{
			if (markerguid.IndexOf("_circle") > 0 || markerguid == "fb66d40b-50fa-4d88-8156-c590328004cb")
			{
				return;
			}
			if (iconPath == WorkPath + "icon\\人.png")
			{
				Map_Person map_Person = Person_GUID[markerguid];
				SendMessage(map_Person.senderid + "," + map_Person.name, 1111, ProgName);
				return;
			}
			mapHelper1.deleteMarker(last_marker + "_line");
			if (markerguid != string.Empty)
			{
				ifm.Close();
				DrawLine(markerguid);
				mapHelper1.SetMarkerShine(markerguid, shine: true);
                ifm = new InfoForm
                {
                    Owner = this,
                    CanEdit = false,
                    Update_Data = true,
                    Node_GUID = markerguid,
                    Icon_GUID = GUID_Icon[markerguid]
                };
                ifm.JdCode = Icon_JDCode[ifm.Icon_GUID];
                ifm.Text = GUID_Name[markerguid];
                ifm.unitid = UnitID;
                ifm.StartPosition = FormStartPosition.Manual;
                ifm.Left = groupControl1.Right - ifm.Width;
                ifm.Top = groupControl1.Bottom - ifm.Height + 20;
                ifm.stopshine += StopMarkerShine;
				ifm.Show();
			}
			if (!markerguid.Equals("") || !Icon_GUID.Equals("") || !select_vector)
			{
				return;
			}
			last_marker = markerguid;
            BorderForm borderForm = new BorderForm
            {
                IsPoint = true,
                IsLine = false
            };
            borderForm.borData.Load_Line(Operator_GUID);
			if (borderForm.borData.line_data == null)
			{
				borderForm.borData.line_data = lineData;
			}
			borderForm.borData.lat = lat;
			borderForm.borData.lng = lng;
			if (borderForm.ShowDialog() == DialogResult.OK)
			{
				lineData = borderForm.borData.line_data;
				if (handle == 2)
				{
					mapHelper1.deleteMarker(Operator_GUID + "_line");
					last_marker = Operator_GUID;
					UpdateDelete(Operator_GUID + "_line");
				}
				Dictionary<string, object> dictionary = borderForm.borData.ToDic();
				mapHelper1.DrawPointLine(Operator_GUID, i_lat, i_lng, dictionary);
				borderForm.borData.Save_Line(Operator_GUID, lat, lng, isAdd: true);
				for (int i = 0; i < cur_lst.Count; i++)
				{
					if (cur_lst[i]["guid"].ToString() == Operator_GUID)
					{
						cur_lst[i]["topoint"] = dictionary;
						break;
					}
				}
				if (handle == 1)
				{
					string str = GUID_Icon[Operator_GUID];
					string text = GUID_Name[Operator_GUID];
					string remark = "添加" + Icon_Name[str + ".png"] + "标注" + text + "的指向位置到(" + lng.ToString() + ", " + lat.ToString() + ")";
					ComputerInfo.WriteLog("添加指向位置", remark);
				}
				else
				{
					string str = GUID_Icon[Operator_GUID];
					string text = GUID_Name[Operator_GUID];
					string remark = "修改" + Icon_Name[str + ".png"] + "标注" + text + "的指向位置到(" + lng.ToString() + ", " + lat.ToString() + ")";
					ComputerInfo.WriteLog("修改指向位置", remark);
				}
			}
			select_vector = false;
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

		private void mapHelper1_MarkerDragBegin(string markerguid, bool candrag)
		{
			mapHelper1.deleteMarker(last_marker + "_line");
			DrawLine(markerguid);
			last_marker = markerguid;
			Operator_GUID = "";
			select_vector = false;
			candrag = (Permission ? true : false);
		}

		private void mapHelper1_MarkerDragEnd(string markerguid, bool canedit, double lat, double lng)
		{
			if (markerguid == "")
			{
				return;
			}
			bool flag = false;
			dPoint dPoint = new dPoint(lat, lng);
			dPoint dPoint2 = dPoint;
			string sql = "select MARKELAT, MARKELNG, MAKRENAME, REGGUID from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and PGUID = '" + markerguid + "' and UNITEID = '" + UnitID + "'";
			DataTable dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				dPoint2 = new dPoint(double.Parse(dataTable.Rows[0]["MARKELAT"].ToString()), double.Parse(dataTable.Rows[0]["MARKELNG"].ToString()));
				string text = dataTable.Rows[0]["REGGUID"].ToString();
				if (GL_POLY.ContainsKey(text))
				{
					foreach (KeyValuePair<string, Polygon> item in GL_POLY[text])
					{
						flag = GL_POLY[text][item.Key].PointInPolygon(dPoint);
						if (flag)
						{
							break;
						}
					}
				}
				else
				{
					Dictionary<string, List<double[]>> dictionary = Get_Border_Line(text);
					foreach (KeyValuePair<string, Polygon> item2 in GL_POLY[text])
					{
						flag = GL_POLY[text][item2.Key].PointInPolygon(dPoint);
						if (flag)
						{
							break;
						}
					}
				}
			}
			if (!flag && XtraMessageBox.Show("移动超出注册范围!是否要完成移动?", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
			{
				if (dataTable.Rows.Count > 0)
				{
					mapHelper1.modifyMarker(markerguid, dataTable.Rows[0]["MAKRENAME"].ToString(), canEdit: true, dPoint2.x, dPoint2.y, null);
				}
				return;
			}
			for (int i = 0; i < cur_lst.Count; i++)
			{
				if (cur_lst[i]["guid"].ToString() == markerguid)
				{
					cur_lst[i]["lat"] = lat;
					cur_lst[i]["lng"] = lng;
					break;
				}
			}
			sql = "update ENVIRICONDATA_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', MARKELAT = '" + lat.ToString() + "', MARKELNG = '" + lng.ToString() + "' where ISDELETE = 0 and PGUID = '" + markerguid + "' and UNITEID = '" + UnitID + "'";
			FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
			string str = GUID_Icon[markerguid];
			string text2 = GUID_Name[markerguid];
			string remark = "移动" + Icon_Name[str + ".png"] + "标注" + text2 + "到(" + lng.ToString() + ", " + lat.ToString() + ")";
			ComputerInfo.WriteLog("移动标注", remark);
		}

		private void mapHelper1_MapRightClick(bool canedit, double lat, double lng, int x, int y)
		{
			Icon_GUID = "";
		}

		private void mapHelper1_MarkerRightClick(int sx, int sy, double lat, double lng, string level, string sguid, string name, bool canedit, string message)
		{
		}

		private void Map_Size_Change(int now_gl, int now_map, string szchange)
		{
			if (now_gl > GL_PGUID.Length)
			{
				return;
			}
			TreeListNode focusedNode = treeList1.FocusedNode;
			string key = GL_PGUID[now_gl];
			string[] array = GL_MAP[key].Split(',');
			string sql = "select MAPLEVEL from ENVIRMAPDY_H0001Z000E00 where ISDELETE = 0 and UNITID = '" + UnitID + "' and PGUID = '" + focusedNode["pguid"].ToString() + "'";
			DataTable dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				array = dataTable.Rows[0]["MAPLEVEL"].ToString().Split(',');
			}
			bool flag = true;
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] == folds[now_map]))
				{
					continue;
				}
				flag = false;
				cur_Level = int.Parse(array[i]);
				if (folds.Contains(cur_Level.ToString()))
				{
					if (cur_Level != now_Level)
					{
						if (!Before_ShowMap)
						{
							mapHelper1.ShowMap(cur_Level, GL_NAME[levelguid], Permission, map_type, Icon_Name, null, cur_lst, 1.0, 400);
							Before_ShowMap = true;
							continue;
						}
						now_Level = cur_Level;
						if (ifm != null)
						{
							ifm.Close();
						}
						string str = WorkPath + "ICONDER\\b_PNGICON\\";
						foreach (PictureBox control in flowLayoutPanel1.Controls)
						{
							if (!(control.Name == "全选") && !(control.Name == "全不选"))
							{
								string text = str + control.Name;
								text = text.Replace('\\', '/');
								if (control.BorderStyle == BorderStyle.Fixed3D && control.Visible)
								{
									mapHelper1.SetMarkerVisibleByIconPath(text, visible: true);
								}
								else
								{
									mapHelper1.SetMarkerVisibleByIconPath(text, visible: false);
								}
							}
						}
						mapHelper1.setMapLevel(cur_Level, "");
						mapHelper1.SetMapCenter(mapHelper1.centerlat, mapHelper1.centerlng);
						EraseBorder();
						borderlines = DrawBorder();
					}
					else
					{
						EraseBorder();
						borderlines = DrawBorder();
						mapHelper1.SetMapCenter(mapHelper1.centerlat, mapHelper1.centerlng);
					}
				}
				else
				{
					mapHelper1.ShowMap(cur_Level, cur_Level.ToString(), canEdit: false, map_type, null, null, null, 1.0, 400);
				}
			}
			if (flag)
			{
				TreeListNode treeListNode = null;
				foreach (TreeListNode node in treeList1.Nodes)
				{
					treeListNode = ((!(szchange == "up")) ? Select_Node(GL_PGUID[now_gl - 1], node) : Select_Node(GL_PGUID[now_gl + 1], node));
					if (treeListNode != null)
					{
						treeList1.FocusedNode = treeListNode;
						break;
					}
				}
			}
		}

		private void Map_Resize(bool IsEnlarge)
		{
			Operator_GUID = "";
			select_vector = false;
			int num = folds.Length;
			int num2 = GL_PGUID.Length;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < num; i++)
			{
				if (folds[i] == cur_Level.ToString())
				{
					num3 = i;
					break;
				}
			}
			for (int i = 0; i < num2; i++)
			{
				if (GL_PGUID[i] == levelguid)
				{
					num4 = i;
					break;
				}
			}
			if (IsEnlarge)
			{
				num3++;
				if (num3 >= num)
				{
					TreeListNode treeListNode = null;
					foreach (TreeListNode node in treeList1.Nodes)
					{
						if (num4 == num2)
						{
							break;
						}
						treeListNode = Select_Node(GL_PGUID[num4 + 1], node);
						if (treeListNode != null)
						{
							treeList1.FocusedNode = treeListNode;
							break;
						}
					}
				}
				else
				{
					Map_Size_Change(num4, num3, "up");
				}
			}
			else
			{
				num3--;
				if (num3 < 0)
				{
					TreeListNode treeListNode = null;
					foreach (TreeListNode node2 in treeList1.Nodes)
					{
						if (num4 == 0)
						{
							break;
						}
						treeListNode = Select_Node(GL_PGUID[num4 - 1], node2);
						if (treeListNode != null)
						{
							treeList1.FocusedNode = treeListNode;
							break;
						}
					}
				}
				else
				{
					Map_Size_Change(num4, num3, "down");
				}
			}
		}

		private void mapHelper1_MapDblClick(string button, bool canedit, double lat, double lng, int x, int y, string markerguid)
		{
			center_lat = lat;
			center_lng = lng;
			if (button == "left")
			{
				Map_Resize(IsEnlarge: true);
			}
			else
			{
				Map_Resize(IsEnlarge: false);
			}
		}

		private TreeListNode Select_Node(string levelguid, TreeListNode pNode)
		{
			if (pNode == null)
			{
				return null;
			}
			if (GL_NAME_PGUID[pNode["level"].ToString()] == levelguid && Check_relative(pNode))
			{
				return pNode;
			}
			TreeListNode treeListNode = null;
			foreach (TreeListNode node in pNode.Nodes)
			{
				treeListNode = Select_Node(levelguid, node);
				if (treeListNode != null)
				{
					break;
				}
			}
			return treeListNode;
		}

		private bool Check_relative(TreeListNode pNode)
		{
			dPoint p = new dPoint(center_lat, center_lng);
			string text = pNode["pguid"].ToString();
			bool flag;
			if (GL_POLY.ContainsKey(text))
			{
				flag = false;
				foreach (KeyValuePair<string, Polygon> item in GL_POLY[text])
				{
					flag = GL_POLY[text][item.Key].PointInPolygon(p);
					if (flag)
					{
						return true;
					}
				}
				return flag;
			}
			flag = false;
			Dictionary<string, List<double[]>> dictionary = Get_Border_Line(text);
			foreach (KeyValuePair<string, Polygon> item2 in GL_POLY[text])
			{
				flag = GL_POLY[text][item2.Key].PointInPolygon(p);
				if (flag)
				{
					return true;
				}
			}
			return flag;
		}

		private void mapHelper1_MapTypeChanged(string mapType)
		{
			map_type = mapType;
		}

		private void mapHelper1_MapMouseWheel(string direction)
		{
			double[] mapCenter = mapHelper1.GetMapCenter();
			center_lat = mapCenter[0];
			center_lng = mapCenter[1];
			if (direction == "up")
			{
				Map_Resize(IsEnlarge: true);
			}
			else
			{
				Map_Resize(IsEnlarge: false);
			}
		}

		private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!(Operator_GUID != ""))
			{
				return;
			}
			DrawLine(Operator_GUID);
			mapHelper1.SetMarkerShine(Operator_GUID, shine: true);
			string text = "";
			string sql = "select MAKRENAME, ICONGUID, MARKELAT, MARKELNG from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and PGUID = '" + Operator_GUID + "' and UNITEID = '" + UnitID + "'";
			DataTable dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count != 0)
			{
				text = dataTable.Rows[0]["ICONGUID"].ToString();
				DataForm dataForm = new DataForm();
				dataForm.CanEdit = Permission;
				dataForm.Update_Data = true;
				dataForm.Node_GUID = Operator_GUID;
				dataForm.Icon_GUID = text;
				dataForm.JdCode = Icon_JDCode[text];
				dataForm.Text = "编辑标注";
				dataForm.StartPosition = FormStartPosition.Manual;
				dataForm.Left = MapX;
				dataForm.Top = MapY;
				if (dataForm.Left + dataForm.Width > base.Width)
				{
					dataForm.Left -= dataForm.Width;
				}
				if (dataForm.Top + dataForm.Height > base.Height)
				{
					dataForm.Top -= dataForm.Height;
				}
				if (dataForm.ShowDialog() == DialogResult.OK)
				{
					string name = dataTable.Rows[0]["MAKRENAME"].ToString();
					FDName_Value = dataForm.FDName_Value;
					mapHelper1.modifyMarker(Operator_GUID, name, canEdit: true, double.Parse(dataTable.Rows[0]["MARKELAT"].ToString()), double.Parse(dataTable.Rows[0]["MARKELNG"].ToString()), null);
				}
				StopMarkerShine(Operator_GUID);
				Operator_GUID = "";
			}
		}

		private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!Permission)
			{
				XtraMessageBox.Show("您没有删除权限!");
			}
			else if (Operator_GUID != "")
			{
				mapHelper1.deleteMarker(Operator_GUID);
				UpdateDelete(Operator_GUID);
				Operator_GUID = "";
			}
		}

		private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!(Operator_GUID == ""))
			{
				select_vector = true;
				handle = 1;
			}
		}

		private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
		{
			select_vector = true;
			handle = 2;
		}

		private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (Operator_GUID != "")
			{
				mapHelper1.deleteMarker(Operator_GUID + "_line");
				UpdateDelete(Operator_GUID + "_line");
				Operator_GUID = "";
			}
		}

		private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
		{
		}

		private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!(Operator_GUID != ""))
			{
				return;
			}
			DrawLine(Operator_GUID);
			mapHelper1.SetMarkerShine(Operator_GUID, shine: true);
			string sql = "select MAKRENAME, MARKELAT, MARKELNG, REGGUID from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and PGUID = '" + Operator_GUID + "' and UNITEID = '" + UnitID + "'";
			DataTable dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
			if (dataTable.Rows.Count > 0)
			{
				regfm.unLock = true;
				regfm.StartPosition = FormStartPosition.Manual;
				regfm.textName = dataTable.Rows[0]["MAKRENAME"].ToString();
				regfm.markerguid = Operator_GUID;
				regfm.Left = Control.MousePosition.X + 20;
				regfm.Top = Control.MousePosition.Y + 20;
				if (regfm.Left + regfm.Width > base.Width)
				{
					regfm.Left -= regfm.Width + 20;
				}
				if (regfm.Top + regfm.Height > base.Height)
				{
					regfm.Top -= regfm.Height + 20;
				}
				string text = dataTable.Rows[0]["REGGUID"].ToString();
				if (Icon_Reg(text))
				{
					dPoint dPoint = new dPoint(center_lat, center_lng);
					string pa_guid = regfm.regguid;
					if (GL_POLY.ContainsKey(regfm.regguid))
					{
						dPoint = GL_POLY[pa_guid][GL_POLY[pa_guid].Keys.First()].GetAPoint();
					}
					else
					{
						Get_Border_Line(regfm.regguid);
						dPoint = GL_POLY[pa_guid][GL_POLY[pa_guid].Keys.First()].GetAPoint();
					}
					while (Math.Abs(dPoint.x + dPoint.y) < 0.01)
					{
						GL_Node gL_Node = GL_List.Find((GL_Node x) => x.pguid == pa_guid);
						pa_guid = gL_Node.upguid;
						if (GL_POLY.ContainsKey(pa_guid))
						{
							dPoint = GL_POLY[pa_guid][GL_POLY[pa_guid].Keys.First()].GetAPoint();
							continue;
						}
						Get_Border_Line(pa_guid);
						dPoint = GL_POLY[pa_guid][GL_POLY[pa_guid].Keys.First()].GetAPoint();
					}
					if (text != regfm.regguid)
					{
						mapHelper1.deleteMarker(Operator_GUID + "_line");
						UpdateDelete(Operator_GUID + "_line");
						mapHelper1.SetMapCenter(dPoint.x, dPoint.y);
						mapHelper1.modifyMarker(Operator_GUID, regfm.textName, canEdit: true, dPoint.x, dPoint.y, null);
					}
					else
					{
						mapHelper1.modifyMarker(Operator_GUID, regfm.textName, canEdit: true, double.Parse(dataTable.Rows[0]["MARKELAT"].ToString()), double.Parse(dataTable.Rows[0]["MARKELNG"].ToString()), null);
					}
					for (int i = 0; i < cur_lst.Count; i++)
					{
						if (cur_lst[i]["guid"].ToString() == Operator_GUID)
						{
							cur_lst[i]["name"] = regfm.textName;
							break;
						}
					}
					sql = "update ENVIRICONDATA_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', MAKRENAME = '" + regfm.textName + "', REGINFO = '" + regfm.regaddr + "', REGGUID = '" + regfm.regguid + "' where ISDELETE = 0 and PGUID = '" + Operator_GUID + "' and UNITEID = '" + UnitID + "'";
					FileReader.often_ahp.ExecuteSql(sql, (OleDbParameter[])null);
				}
			}
			StopMarkerShine(Operator_GUID);
			Operator_GUID = "";
			treeList1.Focus();
			mapHelper1.Focus();
		}

		private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
		{
            Process[] processesByName = Process.GetProcessesByName("TransMessage");
            Process[] array = processesByName;
            foreach (Process process2 in array)
            {
                process2.Kill();
                process2.WaitForExit();
            }
            processesByName = Process.GetProcessesByName("VideoConn");
            array = processesByName;
            foreach (Process process2 in array)
            {
                process2.Kill();
                process2.WaitForExit();
            }
            if (!Permission)
			{
				Environment.Exit(0);
				return;
			}
			FileReader.inip = new IniOperator(WorkPath + "RegInfo.ini");
			FileReader.inip.WriteString("Individuation", "skin", UserLookAndFeel.Default.ActiveSkinName);
			FileReader.inip.WriteInteger("Individuation", "listwidth", dockPanel1.Width);
			FileReader.often_ahp.CloseConn();
			FileReader.line_ahp.CloseConn();
			FileReader.log_ahp.CloseConn();
			FileReader.list_ahp.CloseConn();
			Environment.Exit(0);
		}

		private void mapHelper1_LevelChanged(int lastLevel, int currLevel, string showLevel)
		{
			now_Level = currLevel;
			string str = WorkPath + "ICONDER\\b_PNGICON\\";
			if (ifm != null)
			{
				ifm.Close();
			}
			borderlines = DrawBorder();
			foreach (PictureBox control in flowLayoutPanel1.Controls)
			{
				if (!(control.Name == "全选") && !(control.Name == "全不选"))
				{
					string text = str + control.Name;
					text = text.Replace('\\', '/');
					if (control.BorderStyle == BorderStyle.Fixed3D && control.Visible)
					{
						mapHelper1.SetMarkerVisibleByIconPath(text, visible: true);
					}
					else
					{
						mapHelper1.SetMarkerVisibleByIconPath(text, visible: false);
					}
				}
			}
			Before_ShowMap = true;
			HideMessage();
		}

		private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
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
			mapLevelForm.unitid = UnitID;
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

		private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
		{
			Process process = Process.Start(WorkPath + "YUNEvrDataUp.exe", "EnvirInfoSys.exe 1 1");
			process.WaitForExit();
		}

		private void barButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
		{
			for (int i = 0; i < FileReader.Authority.Length; i++)
			{
				if (FileReader.Authority[i] == "地图设置权限")
				{
					CheckPwForm checkPwForm = new CheckPwForm();
					checkPwForm.unitid = UnitID;
					if (checkPwForm.ShowDialog() != DialogResult.OK)
					{
						XtraMessageBox.Show("未能获取管理员权限");
						return;
					}
					break;
				}
			}
			MapSetForm mapSetForm = new MapSetForm();
			mapSetForm.unitid = UnitID;
			mapSetForm.ShowDialog();
			FileReader.line_ahp.CloseConn();
			FileReader.line_ahp = new AccessHelper(WorkPath + "data\\经纬度注册.mdb");
			foreach (TreeListNode node in treeList1.Nodes)
			{
				string text = node["pguid"].ToString();
				string sql = "select LAT, LNG from ORGCENTERDATA where ISDELETE = 0 and PGUID = '" + text + "' and UNITEID = '" + UnitID + "'";
				DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["lat"] = double.Parse(dataTable.Rows[0]["LAT"].ToString());
					node["lng"] = double.Parse(dataTable.Rows[0]["LNG"].ToString());
				}
				sql = "select MAPLEVEL from ENVIRMAPDY_H0001Z000E00 where ISDELETE = 0 and PGUID = '" + text + "' and UNITID = '" + UnitID + "'";
				dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["maps"] = dataTable.Rows[0]["MAPLEVEL"].ToString();
				}
				sql = "select PGUID from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and MAKRENAME = '" + node["Name"].ToString() + "' and UNITEID = '" + UnitID + "'";
				dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["reg"] = true;
				}
				Refresh_Nodes(node);
			}
			TreeListNode focusedNode = treeList1.FocusedNode;
			string[] array = null;
			array = ((focusedNode["maps"] != null && !(focusedNode["maps"].ToString() == "")) ? focusedNode["maps"].ToString().Split(',') : GL_MAP[levelguid].Split(','));
			if (array[0] != string.Empty)
			{
				cur_Level = int.Parse(array[0]);
			}
			else
			{
				cur_Level = 0;
			}
			if (cur_Level != now_Level)
			{
				if (!Before_ShowMap)
				{
					mapHelper1.ShowMap(cur_Level, GL_NAME[levelguid], Permission, map_type, Icon_Name, null, cur_lst, 1.0, 400);
					Before_ShowMap = true;
					return;
				}
				now_Level = cur_Level;
				if (ifm != null)
				{
					ifm.Close();
				}
				string str = WorkPath + "ICONDER\\b_PNGICON\\";
				foreach (PictureBox control in flowLayoutPanel1.Controls)
				{
					if (!(control.Name == "全选") && !(control.Name == "全不选"))
					{
						string text2 = str + control.Name;
						text2 = text2.Replace('\\', '/');
						if (control.BorderStyle == BorderStyle.Fixed3D && control.Visible)
						{
							mapHelper1.SetMarkerVisibleByIconPath(text2, visible: true);
						}
						else
						{
							mapHelper1.SetMarkerVisibleByIconPath(text2, visible: false);
						}
					}
				}
				mapHelper1.setMapLevel(cur_Level, "");
				mapHelper1.SetMapCenter(mapHelper1.centerlat, mapHelper1.centerlng);
				EraseBorder();
				borderlines = DrawBorder();
			}
			else
			{
				EraseBorder();
				borderlines = DrawBorder();
				mapHelper1.SetMapCenter(mapHelper1.centerlat, mapHelper1.centerlng);
			}
		}

		private void barButtonItem27_ItemClick(object sender, ItemClickEventArgs e)
		{
			Process process = Process.Start(WorkPath + "YUNDataUp.exe", "LoginForm.exe 0 2");
			process.WaitForExit();
		}

		private void Refresh_Nodes(TreeListNode pNode)
		{
			foreach (TreeListNode node in pNode.Nodes)
			{
				string text = node["pguid"].ToString();
				string sql = "select LAT, LNG from ORGCENTERDATA where ISDELETE = 0 and PGUID = '" + text + "' and UNITEID = '" + UnitID + "'";
				DataTable dataTable = FileReader.line_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["lat"] = double.Parse(dataTable.Rows[0]["LAT"].ToString());
					node["lng"] = double.Parse(dataTable.Rows[0]["LNG"].ToString());
				}
				sql = "select MAPLEVEL from ENVIRMAPDY_H0001Z000E00 where ISDELETE = 0 and PGUID = '" + text + "' and UNITID = '" + UnitID + "'";
				dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["maps"] = dataTable.Rows[0]["MAPLEVEL"].ToString();
				}
				sql = "select PGUID from ENVIRICONDATA_H0001Z000E00 where ISDELETE = 0 and MAKRENAME = '" + node["Name"].ToString() + "' and UNITEID = '" + UnitID + "'";
				dataTable = FileReader.often_ahp.ExecuteDataTable(sql, (OleDbParameter[])null);
				if (dataTable.Rows.Count > 0)
				{
					node["reg"] = true;
				}
				Refresh_Nodes(node);
			}
		}

		private void barButtonItem26_ItemClick(object sender, ItemClickEventArgs e)
		{
			xtraFolderBrowserDialog1.SelectedPath = MapPath;
			xtraFolderBrowserDialog1.Title = "请选择地图所在文件夹";
			if (xtraFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				if (string.IsNullOrEmpty(xtraFolderBrowserDialog1.SelectedPath))
				{
					XtraMessageBox.Show(this, "文件夹路径不能为空", "提示");
					return;
				}
				MapPath = xtraFolderBrowserDialog1.SelectedPath;
				FileReader.inip = new IniOperator(WorkPath + "RegInfo.ini");
				FileReader.inip.WriteString("Individuation", "mappath", MapPath);
				XtraMessageBox.Show("已成功导入地图路径，即将重启程序");
				Process process = Process.Start(WorkPath + "ReStart.exe", "LoginForm.exe");
			}
		}

        private int cnt = 0;

        private void ShowPerson(Map_Person person)
        {
            string iconpath = WorkPath + "icon\\人.png";
            mapHelper1.addMarker(string.Concat(person.lat), string.Concat(person.lng), person.name, canedit: false, iconpath, null, person.id);
            ++cnt;
        }

        private void HidePerson(string personid)
        {
            mapHelper1.deleteMarker(Person_GUID[personid].id);
            --cnt;
        }

		private void DrawPerson(Map_Person person)
		{
			string iconpath = WorkPath + "icon\\人.png";
			person.time = DateTime.Now;
			Person_GUID[person.id] = person;
            Person_GUID[person.id].timeclock = new System.Windows.Forms.Timer
            {
                Tag = person.id,
                Enabled = true,
                Interval = 1000
            };
            Person_GUID[person.id].timeclock.Tick += Timeclock_Tick;
			//mapHelper1.addMarker(string.Concat(person.lat), string.Concat(person.lng), person.name, canedit: false, iconpath, null, person.id);
		}

		private void DeletePerson(string personid)
		{
            if (Person_GUID[personid] != null && Person_GUID[personid].timeclock != null)
            {
                Person_GUID[personid].timeclock.Enabled = false;
            }
            mapHelper1.deleteMarker(Person_GUID[personid].id);
            --cnt;
            Person_GUID.Remove(personid);
		}

		private void GetPersonMessage(string msg)
		{
			if (Person_GUID == null || !Before_ShowMap)
			{
				return;
			}
			string[] array = msg.Split(',');
			if (array.Length != 6)
			{
				return;
			}
            Map_Person map_Person = new Map_Person
            {
                id = array[1],
                name = array[0],
                lat = double.Parse(array[2]),
                lng = double.Parse(array[3]),
                senderid = array[5],
                time = DateTime.Now,
                vis = true
            };
            if (array[4] == "1")
			{
				map_Person.lat = DeCodeGpsJwd(map_Person.lat);
				map_Person.lng = DeCodeGpsJwd(map_Person.lng);
				FileReader.once_ahp = new AccessHelper(WorkPath + "data\\mapdata.accdb", "zbxh2012base518");
				string sql = "select NAME from RAND_PERSONINFO where PERSONRAND = '" + map_Person.name + "'";
				DataTable dataTable = FileReader.once_ahp.ExecuteDataTable(sql);
				if (dataTable.Rows.Count > 0)
				{
					map_Person.name = dataTable.Rows[0]["NAME"].ToString();
				}
				FileReader.once_ahp.CloseConn();
                FileReader.once_ahp = new AccessHelper(WorkPath + "data\\PersonMange.mdb");
                sql = "select 所属单位 from RY_PERSONUNIT where PERSONID = " + map_Person.id;
                dataTable = FileReader.once_ahp.ExecuteDataTable(sql);
                if (dataTable.Rows.Count > 0)
                    map_Person.per_unit = dataTable.Rows[0]["所属单位"].ToString();
            }
			if (Person_GUID.ContainsKey(map_Person.id))
			{
				Person_GUID[map_Person.id] = map_Person;
			}
			else
			{
                map_Person.vis = false;
				DrawPerson(map_Person);
			}
		}

		private void Timeclock_Tick(object sender, EventArgs e)
		{
            string text = ((System.Windows.Forms.Timer)sender).Tag.ToString();
            if (!Person_GUID.ContainsKey(text))
            {
                ((System.Windows.Forms.Timer)sender).Enabled = false;
                return;
            }
            if ((DateTime.Now - Person_GUID[text].time).TotalSeconds > 20.0)
            {
                mapHelper1.deleteMarker(Person_GUID[text].id);
                mapHelper1.deleteMarker(Person_GUID[text].id);
                --cnt;
                Person_GUID.Remove(text);
                return;
            }
            string iconpath = WorkPath + "icon\\人.png";
            if (Person_GUID[text].vis == false)
            {
                if (Check_Person(Person_GUID[text]))
                {
                    Person_GUID[text].vis = true;
                    ShowPerson(Person_GUID[text]);
                }
                else
                    return;
            }
            else
            {
                if (!Check_Person(Person_GUID[text]))
                {
                    Person_GUID[text].vis = false;
                    HidePerson(Person_GUID[text].id);
                    return;
                }
            }
			
		    mapHelper1.deleteMarker(Person_GUID[text].id);
			mapHelper1.addMarker(string.Concat(Person_GUID[text].lat), string.Concat(Person_GUID[text].lng), Person_GUID[text].name, canedit: false, iconpath, null, Person_GUID[text].id);
		}

        private bool Check_Person(Map_Person map_Person)
        {
            TreeListNode pNode = treeList1.FocusedNode;
            string unit_guid = pNode["pguid"].ToString();
            if (map_Person.per_unit != unit_guid)
                return false;
            string sql = "select PGUID from GXPERSONDY_H0001Z000E00 where ISDELETE = 0 and PERSONID = '" + map_Person.id + "' and GXPGUID = '" + FLguid + "' and UNITID = '" + UnitID + "'";
            FileReader.once_ahp = new AccessHelper(WorkPath + "data\\PersonDy_H0001Z000E00.mdb");
            DataTable dt = FileReader.once_ahp.ExecuteDataTable(sql);
            FileReader.once_ahp.CloseConn();
            if (dt.Rows.Count <= 0)
                return false;
            return true;
        }

        private double EnCodeGpsJwd(double jw)
		{
			string text = jw.ToString();
			int num = text.IndexOf(".");
			text = text.Replace(".", "");
			if (text.Length < CodeN)
			{
				int num2 = CodeN - text.Length;
				for (int i = 0; i < num2; i++)
				{
					text += "0";
				}
			}
			if (text.Length > CodeN)
			{
				text = text.Substring(0, CodeN);
			}
			if (text.Length == CodeN)
			{
				string[] array = new string[CodeN];
				int i = 0;
				while (text.Length > 1)
				{
					array[i] = text.Substring(0, 1);
					text = text.Substring(1, text.Length - 1);
					i++;
				}
				array[i] = text;
				string[] array2 = new string[CodeN];
				for (i = 0; i < CodeN; i++)
				{
					array2[i] = array[s1[i] - 1];
				}
				for (i = 0; i < CodeN; i++)
				{
					array[i] = array2[s2[i] - 1];
				}
				for (i = 0; i < CodeN; i++)
				{
					array2[i] = array[s3[i] - 1];
				}
				for (i = 0; i < CodeN; i++)
				{
					if (i % 2 != 0)
					{
						array[i - 1] = array2[i];
						array[i] = array2[i - 1];
					}
				}
				array[CodeN - 1] = array2[CodeN - 1];
				text = string.Concat(num);
				for (i = 0; i < CodeN; i++)
				{
					if (i == num)
					{
						text += ".";
					}
					text += array[i];
				}
				jw = double.Parse(text);
				return jw;
			}
			return -1.0;
		}

		private double DeCodeGpsJwd(double jw)
		{
			string text = string.Concat(jw);
			text = text.Substring(1, text.Length - 1);
			int num = text.IndexOf(".");
			text = text.Replace(".", "");
			if (text.Length < CodeN)
			{
				int num2 = CodeN - text.Length;
				for (int i = 0; i < num2; i++)
				{
					text += "0";
				}
			}
			if (text.Length > CodeN)
			{
				text = text.Substring(0, CodeN);
			}
			if (text.Length == CodeN)
			{
				string[] array = new string[CodeN];
				int i = 0;
				while (text.Length > 1)
				{
					array[i] = text.Substring(0, 1);
					text = text.Substring(1, text.Length - 1);
					i++;
				}
				array[i] = text;
				string[] array2 = new string[CodeN];
				for (i = 0; i < CodeN; i++)
				{
					if (i % 2 != 0)
					{
						array2[i - 1] = array[i];
						array2[i] = array[i - 1];
					}
				}
				array2[CodeN - 1] = array[CodeN - 1];
				for (i = 0; i < CodeN; i++)
				{
					array[s3[i] - 1] = array2[i];
				}
				for (i = 0; i < CodeN; i++)
				{
					array2[s2[i] - 1] = array[i];
				}
				for (i = 0; i < CodeN; i++)
				{
					array[s1[i] - 1] = array2[i];
				}
				text = "";
				for (i = 0; i < CodeN; i++)
				{
					if (i == num)
					{
						text += ".";
					}
					text += array[i];
				}
				jw = double.Parse(text);
				return jw;
			}
			return -1.0;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem23 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem27 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem26 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem12 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem24 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.skinBarSubItem1 = new DevExpress.XtraBars.SkinBarSubItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem13 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem14 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem15 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem16 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem17 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem18 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem19 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem20 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem22 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem25 = new DevExpress.XtraBars.BarButtonItem();
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.dockPanel2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.pbMove = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mapHelper1 = new MapHelper.MapHelper();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.popupMenu2 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::EnvirInfoSys.WaitForm1), true, true);
            this.barButtonItem21 = new DevExpress.XtraBars.BarButtonItem();
            this.xtraFolderBrowserDialog1 = new DevExpress.XtraEditors.XtraFolderBrowserDialog(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            this.dockPanel2.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMove)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
            this.SuspendLayout();
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1,
            this.dockPanel2});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl"});
            // 
            // barManager1
            // 
            this.barManager1.AllowShowToolbarsPopup = false;
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barSubItem2,
            this.barButtonItem1,
            this.barButtonItem2,
            this.barButtonItem4,
            this.barButtonItem5,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barButtonItem8,
            this.barButtonItem9,
            this.barButtonItem10,
            this.barButtonItem11,
            this.barButtonItem12,
            this.skinBarSubItem1,
            this.barButtonItem3,
            this.barButtonItem13,
            this.barButtonItem14,
            this.barButtonItem15,
            this.barButtonItem16,
            this.barButtonItem17,
            this.barButtonItem18,
            this.barButtonItem19,
            this.barButtonItem20,
            this.barButtonItem22,
            this.barButtonItem23,
            this.barButtonItem24,
            this.barButtonItem25,
            this.barButtonItem26,
            this.barButtonItem27,
            this.barStaticItem1,
            this.barStaticItem2});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 43;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.Text = "Tools";
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.barButtonItem1, false),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.skinBarSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3)});
            this.bar2.OptionsBar.DrawBorder = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "数据管理(&D)";
            this.barSubItem1.Id = 0;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem23),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem27),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem26)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "数据备份(&B)";
            this.barButtonItem4.Id = 5;
            this.barButtonItem4.Name = "barButtonItem4";
            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "数据恢复(&R)";
            this.barButtonItem5.Id = 6;
            this.barButtonItem5.Name = "barButtonItem5";
            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "数据同步(&U)";
            this.barButtonItem6.Id = 7;
            this.barButtonItem6.Name = "barButtonItem6";
            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem23
            // 
            this.barButtonItem23.Caption = "数据上传(&L)";
            this.barButtonItem23.Id = 36;
            this.barButtonItem23.Name = "barButtonItem23";
            this.barButtonItem23.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem23_ItemClick);
            // 
            // barButtonItem27
            // 
            this.barButtonItem27.Caption = "下载基础数据(&D)";
            this.barButtonItem27.Id = 40;
            this.barButtonItem27.Name = "barButtonItem27";
            this.barButtonItem27.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem27_ItemClick);
            // 
            // barButtonItem26
            // 
            this.barButtonItem26.Caption = "设置地图路径(&P)";
            this.barButtonItem26.Id = 39;
            this.barButtonItem26.Name = "barButtonItem26";
            this.barButtonItem26.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem26_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "系统设置(&S)";
            this.barSubItem2.Id = 1;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem12),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.barButtonItem10, false),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.barButtonItem11, false),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem24),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barButtonItem12
            // 
            this.barButtonItem12.Caption = "密码管理(&P)";
            this.barButtonItem12.Id = 13;
            this.barButtonItem12.Name = "barButtonItem12";
            this.barButtonItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem12.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem12_ItemClick);
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "服务器IP设置(&S)";
            this.barButtonItem7.Id = 8;
            this.barButtonItem7.Name = "barButtonItem7";
            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "边界线属性设置(&B)";
            this.barButtonItem8.Id = 9;
            this.barButtonItem8.Name = "barButtonItem8";
            this.barButtonItem8.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem8_ItemClick);
            // 
            // barButtonItem10
            // 
            this.barButtonItem10.Caption = "图符对应设置(&C)";
            this.barButtonItem10.Id = 11;
            this.barButtonItem10.Name = "barButtonItem10";
            this.barButtonItem10.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem10_ItemClick);
            // 
            // barButtonItem11
            // 
            this.barButtonItem11.Caption = "图符扩展设置(&I)";
            this.barButtonItem11.Id = 12;
            this.barButtonItem11.Name = "barButtonItem11";
            this.barButtonItem11.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem11_ItemClick);
            // 
            // barButtonItem24
            // 
            this.barButtonItem24.Caption = "地图设置(&G)";
            this.barButtonItem24.Id = 37;
            this.barButtonItem24.Name = "barButtonItem24";
            this.barButtonItem24.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem24.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem24_ItemClick);
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "图符管理设置(&M)";
            this.barButtonItem9.Id = 10;
            this.barButtonItem9.Name = "barButtonItem9";
            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "查看日志(&L)";
            this.barButtonItem1.Id = 2;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "查看帮助(&H)";
            this.barButtonItem2.Id = 3;
            this.barButtonItem2.Name = "barButtonItem2";
            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // skinBarSubItem1
            // 
            this.skinBarSubItem1.Caption = "更换皮肤(&K)";
            this.skinBarSubItem1.Id = 19;
            this.skinBarSubItem1.Name = "skinBarSubItem1";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "退出(&Q)";
            this.barButtonItem3.Id = 20;
            this.barButtonItem3.Name = "barButtonItem3";
            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Custom 4";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem2)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawBorder = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Custom 4";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Id = 41;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Id = 42;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1712, 64);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 1067);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1712, 36);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 64);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 1003);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1712, 64);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 1003);
            // 
            // barButtonItem13
            // 
            this.barButtonItem13.Caption = "属性编辑";
            this.barButtonItem13.Id = 24;
            this.barButtonItem13.Name = "barButtonItem13";
            this.barButtonItem13.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem13_ItemClick);
            // 
            // barButtonItem14
            // 
            this.barButtonItem14.Caption = "删除当前标注";
            this.barButtonItem14.Id = 25;
            this.barButtonItem14.Name = "barButtonItem14";
            this.barButtonItem14.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem14_ItemClick);
            // 
            // barButtonItem15
            // 
            this.barButtonItem15.Caption = "添加指向位置";
            this.barButtonItem15.Id = 26;
            this.barButtonItem15.Name = "barButtonItem15";
            this.barButtonItem15.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem15_ItemClick);
            // 
            // barButtonItem16
            // 
            this.barButtonItem16.Caption = "修改指向位置";
            this.barButtonItem16.Id = 27;
            this.barButtonItem16.Name = "barButtonItem16";
            this.barButtonItem16.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem16_ItemClick);
            // 
            // barButtonItem17
            // 
            this.barButtonItem17.Caption = "删除指向位置";
            this.barButtonItem17.Id = 28;
            this.barButtonItem17.Name = "barButtonItem17";
            this.barButtonItem17.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem17_ItemClick);
            // 
            // barButtonItem18
            // 
            this.barButtonItem18.Caption = "导入当前单位边界线";
            this.barButtonItem18.Id = 29;
            this.barButtonItem18.Name = "barButtonItem18";
            this.barButtonItem18.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem18.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem18_ItemClick);
            // 
            // barButtonItem19
            // 
            this.barButtonItem19.Caption = "下载单位注册数据(&D)";
            this.barButtonItem19.Id = 30;
            this.barButtonItem19.Name = "barButtonItem19";
            this.barButtonItem19.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem19_ItemClick);
            // 
            // barButtonItem20
            // 
            this.barButtonItem20.Caption = "修改注册信息";
            this.barButtonItem20.Id = 31;
            this.barButtonItem20.Name = "barButtonItem20";
            this.barButtonItem20.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem20_ItemClick);
            // 
            // barButtonItem22
            // 
            this.barButtonItem22.Caption = "设置地图对应级别";
            this.barButtonItem22.Id = 35;
            this.barButtonItem22.Name = "barButtonItem22";
            this.barButtonItem22.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItem22.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem22_ItemClick);
            // 
            // barButtonItem25
            // 
            this.barButtonItem25.Caption = "发布系统(&P)";
            this.barButtonItem25.Id = 38;
            this.barButtonItem25.Name = "barButtonItem25";
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.dockPanel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dockPanel1.ID = new System.Guid("d955ddf5-94b6-49cb-840b-37e2f0994894");
            this.dockPanel1.Location = new System.Drawing.Point(0, 64);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Options.ShowCloseButton = false;
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.SavedSizeFactor = 0D;
            this.dockPanel1.Size = new System.Drawing.Size(200, 1003);
            this.dockPanel1.Text = "管辖范围";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeList1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(6, 33);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(185, 964);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeList1
            // 
            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList1.Location = new System.Drawing.Point(0, 0);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.Editable = false;
            this.treeList1.Size = new System.Drawing.Size(185, 964);
            this.treeList1.TabIndex = 0;
            this.treeList1.ViewStyle = DevExpress.XtraTreeList.TreeListViewStyle.TreeView;
            this.treeList1.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList1_FocusedNodeChanged);
            this.treeList1.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.treeList1_CustomDrawNodeCell);
            // 
            // dockPanel2
            // 
            this.dockPanel2.Controls.Add(this.dockPanel2_Container);
            this.dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            this.dockPanel2.ID = new System.Guid("ffe5c6be-7291-4281-896f-71a6444f9312");
            this.dockPanel2.Location = new System.Drawing.Point(1625, 64);
            this.dockPanel2.Name = "dockPanel2";
            this.dockPanel2.Options.ShowCloseButton = false;
            this.dockPanel2.OriginalSize = new System.Drawing.Size(87, 200);
            this.dockPanel2.SavedSizeFactor = 0D;
            this.dockPanel2.Size = new System.Drawing.Size(87, 1003);
            this.dockPanel2.Text = "图例";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.panelControl1);
            this.dockPanel2_Container.Location = new System.Drawing.Point(9, 33);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(72, 964);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.flowLayoutPanel1);
            this.panelControl1.Controls.Add(this.pbMove);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(72, 964);
            this.panelControl1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(68, 960);
            this.flowLayoutPanel1.TabIndex = 34;
            // 
            // pbMove
            // 
            this.pbMove.BackColor = System.Drawing.Color.Transparent;
            this.pbMove.Location = new System.Drawing.Point(73, 487);
            this.pbMove.Margin = new System.Windows.Forms.Padding(4);
            this.pbMove.Name = "pbMove";
            this.pbMove.Size = new System.Drawing.Size(39, 40);
            this.pbMove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMove.TabIndex = 33;
            this.pbMove.TabStop = false;
            this.pbMove.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.mapHelper1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1421, 970);
            this.panel2.TabIndex = 2;
            // 
            // mapHelper1
            // 
            this.mapHelper1.BackColor = System.Drawing.Color.Transparent;
            this.mapHelper1.centerlat = 0D;
            this.mapHelper1.centerlng = 0D;
            this.mapHelper1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapHelper1.iconspath = null;
            this.mapHelper1.Location = new System.Drawing.Point(0, 0);
            this.mapHelper1.maparr = null;
            this.mapHelper1.Margin = new System.Windows.Forms.Padding(4, 7, 4, 7);
            this.mapHelper1.Name = "mapHelper1";
            this.mapHelper1.roadmappath = null;
            this.mapHelper1.satellitemappath = null;
            this.mapHelper1.Size = new System.Drawing.Size(1421, 970);
            this.mapHelper1.TabIndex = 0;
            this.mapHelper1.webpath = null;
            this.mapHelper1.AddMarkerFinished += new MapHelper.MapHelper.DlAddMarkerFinished(this.mapHelper1_AddMarkerFinished);
            this.mapHelper1.ModifyMarkerFinished += new MapHelper.MapHelper.DlModifyMarkerFinished(this.mapHelper1_ModifyMarkerFinished);
            this.mapHelper1.MarkerDragBegin += new MapHelper.MapHelper.DlMarkerDragBegin(this.mapHelper1_MarkerDragBegin);
            this.mapHelper1.MarkerDragEnd += new MapHelper.MapHelper.DlMarkerDragEnd(this.mapHelper1_MarkerDragEnd);
            this.mapHelper1.MapMouseup += new MapHelper.MapHelper.DlMapMouseup(this.mapHelper1_MapMouseup);
            this.mapHelper1.MapRightClick += new MapHelper.MapHelper.DlMapRightClick(this.mapHelper1_MapRightClick);
            this.mapHelper1.MapDblClick += new MapHelper.MapHelper.DlMapDblClick(this.mapHelper1_MapDblClick);
            this.mapHelper1.RemoveMarkerFinished += new MapHelper.MapHelper.DlRemoveMarkerFinished(this.mapHelper1_RemoveMarkerFinished);
            this.mapHelper1.MarkerRightClick += new MapHelper.MapHelper.DlMarkerRightClick(this.mapHelper1_MarkerRightClick);
            this.mapHelper1.IconSelected += new MapHelper.MapHelper.DlIconSelected(this.mapHelper1_IconSelected);
            this.mapHelper1.LevelChanged += new MapHelper.MapHelper.DlLevelChanged(this.mapHelper1_LevelChanged);
            this.mapHelper1.MapTypeChanged += new MapHelper.MapHelper.DlMapTypeChanged(this.mapHelper1_MapTypeChanged);
            this.mapHelper1.MapMouseWheel += new MapHelper.MapHelper.DlMouseWheel(this.mapHelper1_MapMouseWheel);
            this.mapHelper1.MapMouseOver += new MapHelper.MapHelper.DlMapMouseOver(this.mapHelper1_MapMouseOver);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panel2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(200, 64);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1425, 1003);
            this.groupControl1.TabIndex = 24;
            this.groupControl1.Text = "地图显示";
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem20),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem13),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem14),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem15),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem16),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem17)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // popupMenu2
            // 
            this.popupMenu2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem18),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem22)});
            this.popupMenu2.Manager = this.barManager1;
            this.popupMenu2.Name = "popupMenu2";
            // 
            // xtraOpenFileDialog1
            // 
            this.xtraOpenFileDialog1.FileName = "xtraOpenFileDialog1";
            // 
            // splashScreenManager1
            // 
            this.splashScreenManager1.ClosingDelay = 500;
            // 
            // barButtonItem21
            // 
            this.barButtonItem21.Id = -1;
            this.barButtonItem21.Name = "barButtonItem21";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1712, 1103);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.dockPanel2);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            this.dockPanel2.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMove)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
