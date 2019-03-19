using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using DevExpress.XtraEditors;

namespace EnvirInfoSys
{
    public partial class IconListForm : DevExpress.XtraEditors.XtraForm
    {
        public Dictionary<string, string> guid_name = null;
        public string markerguid = null;

        public IconListForm()
        {
            InitializeComponent();
        }

        private void IconListForm_Shown(object sender, EventArgs e)
        {
            listBoxControl1.Items.Clear();
            List<string> guids = new List<string>(guid_name.Keys);
            
            for (int i = 0; i < guid_name.Count; ++i)
            {
                ListItem listItem = new ListItem
                {
                    Value = guids[i],
                    Text = guid_name[guids[i]]
                };
                listBoxControl1.Items.Add(listItem);
            }
            listBoxControl1.SelectedIndex = -1;
        }

        private void listBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBoxControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBoxControl1.IndexFromPoint(e.Location);
            if (index != -1)
            {
                ListItem listItem = (ListItem)listBoxControl1.SelectedItem;
                markerguid = listItem.Value;                
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                listBoxControl1.SelectedIndex = -1;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int index = listBoxControl1.SelectedIndex;
            if (index != -1)
            {
                ListItem listItem = (ListItem)listBoxControl1.SelectedItem;
                markerguid = listItem.Value;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                XtraMessageBox.Show("请选择所要选择图符的名称!");
            }
        }
    }
}