using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
            Get_Tree_View();
            Get_Publish_Record();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            simpleButton1.Enabled = false;
            simpleButton2.Enabled = false;
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
    }
}
