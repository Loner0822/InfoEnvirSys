using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace PublishSys
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        private AccessHelper ahp = null;
        private IniOperator inip = null;
        private string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        private List<AdminUnit> AdminList = new List<AdminUnit>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
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
            simpleButton1.Enabled = false;
            simpleButton2.Enabled = false;
            Get_Tree_View();
            Get_Publish_Record();            
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            
        }

        private void Get_Tree_View()
        {
            treeList1.Nodes.Clear();
            treeList1.Appearance.FocusedCell.BackColor = Color.SteelBlue;
            treeList1.KeyFieldName = "Id";
            treeList1.ParentFieldName = "Pid";
            ahp = new AccessHelper(WorkPath + "Publish\\data\\PersonMange.mdb");
            string sql = "select PGUID, UPPGUID, ORGNAME, ULEVEL from RG_单位注册 where ISDELETE = 0 and UPPGUID = '0'";
            AdminList = new List<AdminUnit>();
            DataTable dt = ahp.ExecuteDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                string pguid = dt.Rows[i]["PGUID"].ToString();
                string upguid = dt.Rows[i]["UPPGUID"].ToString();
                string name = dt.Rows[i]["ORGNAME"].ToString();
                string level = dt.Rows[i]["ULEVEL"].ToString();
                AdminUnit adminUnit = new AdminUnit(pguid, upguid, level, name);
                AdminList.Add(adminUnit);
                Add_Unit_Node(adminUnit);
            }
            treeList1.DataSource = AdminList;
            treeList1.HorzScrollVisibility = ScrollVisibility.Auto;
            treeList1.Columns[0].Visible = false;
            foreach (TreeListNode tln in treeList1.Nodes)
                Expand_Tree(tln);
            ahp.CloseConn();
        }

        private void Expand_Tree(TreeListNode pa)
        {
            if (pa["Level"].ToString() == "国" || pa["Level"].ToString() == "省" || pa["Level"].ToString() == "市")
            {
                pa.Expand();
                foreach (TreeListNode tln in pa.Nodes)
                    Expand_Tree(tln);
            }
        }

        private void Add_Unit_Node(AdminUnit pa)
        {
            string sql = "select PGUID, UPPGUID, ORGNAME, ULEVEL from RG_单位注册 where ISDELETE = 0 and UPPGUID = '" + pa.Id + "'";
            DataTable dt = ahp.ExecuteDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string pguid = dt.Rows[i]["PGUID"].ToString();
                string upguid = dt.Rows[i]["UPPGUID"].ToString();
                string name = dt.Rows[i]["ORGNAME"].ToString();
                string level = dt.Rows[i]["ULEVEL"].ToString();
                AdminUnit adminUnit = new AdminUnit(pguid, upguid, level, name);
                AdminList.Add(adminUnit);
                Add_Unit_Node(adminUnit);
            }
        }

        private void Get_Publish_Record()
        {
            DataTable pr_dt = new DataTable();
            pr_dt.Columns.Add("序号", typeof(int));
            pr_dt.Columns.Add("发布单位", typeof(string));
            pr_dt.Columns.Add("发布时间", typeof(string));
            pr_dt.Columns.Add("发布系统", typeof(string));
            pr_dt.Columns.Add("系统版本", typeof(string));
            ahp = new AccessHelper(WorkPath + "data\\PublishData.mdb");
            string sql = "select * from ENVIR_PUBLISH_H0001Z000E00 where ISDELETE = 0 order by S_UDTIME";
            DataTable dt = ahp.ExecuteDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
                pr_dt.Rows.Add(i + 1, dt.Rows[i]["UNITNAME"].ToString(), dt.Rows[i]["S_UDTIME"].ToString(),
                    dt.Rows[i]["UNITNAME"].ToString() + "环境信息化系统", dt.Rows[i]["VERSION"].ToString());
            gridControl1.DataSource = pr_dt;
            GridView gridView = gridControl1.MainView as GridView;
            gridView.OptionsBehavior.Editable = false;
            for (int i = 0; i < 5; ++i)
                gridView1.Columns[i].OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            gridView1.Columns[0].Width = 60;
            gridView1.Columns[1].Width = 80;
            gridView1.Columns[2].Width = 180;
            gridView1.Columns[3].Width = 180;
            ahp.CloseConn();
        }

        private void treeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            simpleButton1.Enabled = true;
            simpleButton2.Enabled = true;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process process = Process.Start(WorkPath + "Publish\\DataUp.exe", "-1 1 2");
            process.WaitForExit();
            Get_Tree_View();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process process = Process.Start(WorkPath + "Publish\\YUNDataUp.exe", "0 1 1");
            process.WaitForExit();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process process = Process.Start(WorkPath + "DataBF.exe");
            process.WaitForExit();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process process = Process.Start(WorkPath + "DataHF.exe");
            process.WaitForExit();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process process = Process.Start(WorkPath + "DataUP.exe", "PublishSys.exe 0 2");
            process.WaitForExit();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process process = Process.Start(WorkPath + "DataUP.exe", "PublishSys.exe 1 1");
            process.WaitForExit();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Process process = Process.Start(WorkPath + "SetIP.exe");
            process.WaitForExit();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ahp.CloseConn();
            Environment.Exit(0);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            TreeListNode pNode = treeList1.FocusedNode;
            string pguid = pNode["Id"].ToString();
            string lvlist = Get_Level_List(pguid);
            if (lvlist == "")
                return;
            if (XtraMessageBox.Show("是否要下载单位地图到打包目录?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Process p = Process.Start(WorkPath + "Publish\\DownOrgMapByBorder.exe", pguid + " " + lvlist + " 1");
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    XtraMessageBox.Show("下载地图失败!请检查服务器后重新下载");
                    return;
                }
            }
            else
            {
                if (xtraFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    Process p = Process.Start(WorkPath + "Publish\\DownOrgMapByBorder.exe", pguid + " " + lvlist + " 1 " + xtraFolderBrowserDialog1.SelectedPath);
                    p.WaitForExit();
                    if (p.ExitCode != 0)
                    {
                        XtraMessageBox.Show("下载地图失败!请检查服务器后重新下载");
                        return;
                    }
                }
            }
        }

        private string Get_Level_List(string pguid)
        {
            TreeListNode pNode = treeList1.FocusedNode;
            ahp = new AccessHelper(WorkPath + "Publish\\data\\PersonMange.mdb");
            string sql = "select UPPGUID, ULEVEL from RG_单位注册 where ISDELETE = 0 and PGUID = '" + pNode["Id"].ToString() + "'";
            DataTable dataTable = ahp.ExecuteDataTable(sql);
            string tmp_str = pguid;
            List<string> left_level = new List<string>();
            if (dataTable.Rows.Count > 0)
            {
                while (dataTable.Rows[0]["ULEVEL"].ToString() != "县")
                {
                    tmp_str = dataTable.Rows[0]["UPPGUID"].ToString();
                    sql = "select UPPGUID, ULEVEL from RG_单位注册 where ISDELETE = 0 and PGUID = '" + tmp_str + "'";
                    dataTable = ahp.ExecuteDataTable(sql);
                    left_level.Add(dataTable.Rows[0]["ULEVEL"].ToString());
                    if (dataTable.Rows.Count <= 0)
                    {
                        MessageBox.Show("读取单位列表时出错!");
                        return "";
                    }
                }
            }
            ahp.CloseConn();
            ahp = new AccessHelper(WorkPath + "Publish\\data\\ZSK_H0001Z000K01.mdb");
            for (int i = 0; i < left_level.Count; ++i)
            {
                sql = "select PGUID from ZSK_OBJECT_H0001Z000K01 where ISDELETE = 0 and JDNAME = '" + left_level[i] + "'";
                dataTable = ahp.ExecuteDataTable(sql, null);
                if (dataTable.Rows.Count > 0)
                    left_level[i] = dataTable.Rows[0]["PGUID"].ToString();
            }
            ahp.CloseConn();
            string lvlist = "";
            ahp = new AccessHelper(WorkPath + "Publish\\data\\ENVIRDYDATA_H0001Z000E00.mdb");
            sql = "select MAPLEVEL, LEVELGUID from MAPDUIYING_H0001Z000E00 where ISDELETE = 0 and UNITEID = '" + tmp_str + "'";
            DataTable dt = ahp.ExecuteDataTable(sql);
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                bool flag = true;
                foreach (string l_level in left_level)
                {
                    if (dt.Rows[i]["LEVELGUID"].ToString() == l_level)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                    lvlist += dt.Rows[i]["MAPLEVEL"].ToString() + ",";
            }
            ahp.CloseConn();
            List<string> tmp = new List<string>(lvlist.Split(','));
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
            if (lvlist == "")
            {
                MessageBox.Show("当前单位尚未对应地图级别!");
                return "";
            }
            return lvlist.Substring(0, lvlist.Length - 1);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            TreeListNode pNode = treeList1.FocusedNode;
            if (pNode == null)
                return;
            inip = new IniOperator(WorkPath + "Publish\\RegInfo.ini");
            if (XtraMessageBox.Show("即将发布《" + pNode["Name"].ToString() + "区域经济大数据平台" + textEdit1.Text + "》", "提示", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            /*string levellist = Get_Level_List(selectedNode.Tag.ToString());
            Process process = Process.Start(WorkPath + "Publish\\DownOrgMapByBorder.exe", selectedNode.Tag.ToString() + " " + levellist + " 1");// 单位guid，级别逗号隔开，1 删除下载，0 不删就下
            process.WaitForExit();
            if (process.ExitCode == 1)
            {
                MessageBox.Show("地图获取失败!");
                return;
            }*/

            inip.WriteString("Public", "UnitName", pNode["Name"].ToString());
            inip.WriteString("Public", "UnitLevel", pNode["Level"].ToString());
            inip.WriteString("Public", "UnitID", pNode["Id"].ToString());
            string text = pNode["Id"].ToString();
            ahp = new AccessHelper(WorkPath + "Publish\\data\\PersonMange.mdb");
            string sql = "select UPPGUID, ULEVEL from RG_单位注册 where ISDELETE = 0 and PGUID = '" + pNode["Id"].ToString() + "'";
            DataTable dataTable = ahp.ExecuteDataTable(sql);
            if (dataTable.Rows.Count > 0)
            {
                while (dataTable.Rows[0]["ULEVEL"].ToString() != "县")
                {
                    text = dataTable.Rows[0]["UPPGUID"].ToString();
                    sql = "select UPPGUID, ULEVEL from RG_单位注册 where ISDELETE = 0 and PGUID = '" + text + "'";
                    dataTable = ahp.ExecuteDataTable(sql);
                    if (dataTable.Rows.Count <= 0)
                    {
                        XtraMessageBox.Show("读取单位列表时出错!");
                        return;
                    }
                }
                ahp.CloseConn();
                inip.WriteString("Public", "UnitCounty", text);
                inip.WriteString("Public", "AppName", "区域经济大数据平台");
                inip.WriteString("版本号", "VerNum", textEdit1.Text);
                inip = new IniOperator(WorkPath + "PackUp.ini");
                inip.WriteString("packup", "my_app_name", "区域经济大数据平台");
                inip.WriteString("packup", "my_app_version", textEdit1.Text);
                inip.WriteString("packup", "my_app_publisher", pNode["Name"].ToString());
                inip.WriteString("packup", "my_app_exe_name", "LoginForm.exe");
                inip.WriteString("packup", "my_app_id", "{" + Guid.NewGuid().ToString("B"));
                inip.WriteString("packup", "source_exe_path", WorkPath + "Publish\\EnvirInfoSys.exe");
                inip.WriteString("packup", "source_path", WorkPath + "Publish");
                inip.WriteString("packup", "registry_subkey", "区域经济大数据平台");
                ahp = new AccessHelper(WorkPath + "Publish\\data\\PASSWORD_H0001Z000E00.mdb");
                sql = "select PGUID from PASSWORD_H0001Z000E00 where ISDELETE = 0 and PWNAME = '管理员密码' and UNITID = '" + pNode["Id"].ToString() + "'";
                dataTable = ahp.ExecuteDataTable(sql);
                if (dataTable.Rows.Count > 0)
                {
                    sql = "update PASSWORD_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', PWMD5 = '95d565ef66e7dff9' where PWNAME = '管理员密码' and UNITID = '" + pNode["Id"].ToString() + "'";
                    ahp.ExecuteSql(sql);
                }
                else
                {
                    sql = "insert into PASSWORD_H0001Z000E00 (PGUID, S_UDTIME, PWNAME, PWMD5, UNITID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '管理员密码', '95d565ef66e7dff9', '" + pNode["Id"].ToString() + "')";
                    ahp.ExecuteSql(sql);
                }
                sql = "select PGUID from PASSWORD_H0001Z000E00 where ISDELETE = 0 and PWNAME = '编辑模式' and UNITID = '" + pNode["Id"].ToString() + "'";
                dataTable = ahp.ExecuteDataTable(sql);
                if (dataTable.Rows.Count > 0)
                {
                    sql = "update PASSWORD_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', PWMD5 = 'a0b923820dcc509a' where PWNAME = '编辑模式' and UNITID = '" + pNode["Id"].ToString() + "'";
                    ahp.ExecuteSql(sql);
                }
                else
                {
                    sql = "insert into PASSWORD_H0001Z000E00 (PGUID, S_UDTIME, PWNAME, PWMD5, UNITID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '编辑模式', 'a0b923820dcc509a', '" + pNode["Id"].ToString() + "')";
                    ahp.ExecuteSql(sql);
                }
                sql = "select PGUID from PASSWORD_H0001Z000E00 where ISDELETE = 0 and PWNAME = '查看模式' and UNITID = '" + pNode["Id"].ToString() + "'";
                dataTable = ahp.ExecuteDataTable(sql);
                if (dataTable.Rows.Count > 0)
                {
                    sql = "update PASSWORD_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', PWMD5 = '9d4c2f636f067f89' where PWNAME = '查看模式' and UNITID = '" + pNode["Id"].ToString() + "'";
                    ahp.ExecuteSql(sql);
                }
                else
                {
                    sql = "insert into PASSWORD_H0001Z000E00 (PGUID, S_UDTIME, PWNAME, PWMD5, UNITID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '查看模式', '9d4c2f636f067f89', '" + pNode["Id"].ToString() + "')";
                    ahp.ExecuteSql(sql);
                }
                ahp.CloseConn();
                ahp = new AccessHelper(WorkPath + "Publish\\data\\ENVIR_H0001Z000E00.mdb");
                sql = "select PGUID from ENVIRGXFL_H0001Z000E00 where ISDELETE = 0 and UPGUID = '-1' and UNITID = '" + pNode["Id"].ToString() + "'";
                dataTable = ahp.ExecuteDataTable(sql);
                if (dataTable.Rows.Count > 0)
                {
                    sql = "update ENVIRGXFL_H0001Z000E00 set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', FLNAME = '管辖', SHOWINDEX = 0 where ISDELETE = 0 and UPGUID = '-1' and UNITID = '" + pNode["Id"].ToString() + "'";
                    ahp.ExecuteSql(sql);
                }
                else
                {
                    sql = "insert into ENVIRGXFL_H0001Z000E00 (PGUID, S_UDTIME, FLNAME, UNITID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '管辖', '" + pNode["Id"].ToString() + "')";
                    ahp.ExecuteSql(sql);
                }
                ahp.CloseConn();
                ahp = new AccessHelper(WorkPath + "Publish\\data\\ZSK_AppInfo.mdb");
                sql = "update APPINFO set S_UDTIME = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', UNITID = '" + pNode["Id"].ToString() + "' where ISDELETE = 0 and PGUID = '{8C3B99C5-26D3-48B2-A676-250189FCEA2F}'";
                ahp.ExecuteSql(sql);
                ahp.CloseConn();
                ahp = new AccessHelper(WorkPath + "Publish\\data\\ENVIRLIST_H0001Z000E00.mdb");
                sql = "select PGUID from ENVIRLIST_H0001Z000E00 where ISDELETE = 0 and UNITID = '" + pNode["Id"].ToString() + "' and MARKERID = 'all'";
                dataTable = ahp.ExecuteDataTable(sql);
                if (dataTable.Rows.Count <= 0)
                {
                    sql = "insert into ENVIRLIST_H0001Z000E00 (PGUID, S_UDTIME, FUNCNAME, FUNCTION, SHOWINDEX, UNITID, MARKERID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '基本信息', 'info', -2, '" + pNode["Id"].ToString() + "', 'all')";
                    ahp.ExecuteSql(sql);
                    sql = "insert into ENVIRLIST_H0001Z000E00 (PGUID, S_UDTIME, FUNCNAME, FUNCTION, SHOWINDEX, UNITID, MARKERID) values ('" + Guid.NewGuid().ToString("B") + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '照片', 'pic', -1, '" + pNode["Id"].ToString() + "', 'all')";
                    ahp.ExecuteSql(sql);
                }
                ahp.CloseConn();
                Process process = Process.Start(WorkPath + "PackUp.exe");
                process.WaitForExit();
                if (process.ExitCode == -1)
                {
                    XtraMessageBox.Show("发布失败!");
                    return;
                }
                string text2 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string text3 = Guid.NewGuid().ToString("B");
                ahp = new AccessHelper(WorkPath + "data\\PublishData.mdb");
                sql = "insert into ENVIR_PUBLISH_H0001Z000E00 (PGUID, S_UDTIME, UNITID, UNITNAME, VERSION, SYSTEMNAME) values ('" + text3 + "', '" + text2 + "', '" + pNode["Id"].ToString() + "', '" + pNode["Name"].ToString() + "', '" + textEdit1.Text + "', '" + pNode["Name"].ToString() + "区域经济大数据平台')";
                ahp.ExecuteSql(sql);
                ahp.CloseConn();
                XtraMessageBox.Show("发布成功!");
                Get_Publish_Record();
                Process p = Process.Start(WorkPath + "Publish\\DownOrgMapByBorder.exe", "2 2 2 2");
                p.WaitForExit();
            }
            else
            {
                XtraMessageBox.Show("读取单位列表时出错!");
            }
        }
    }
}
