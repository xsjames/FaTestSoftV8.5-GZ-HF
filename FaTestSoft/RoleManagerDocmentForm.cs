using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;

namespace FaTestSoft
{
    public partial class RoleManagerDocmentForm : DockContent
    {
        public RoleManagerDocmentForm()
        {
            InitializeComponent();
        }

        private void RoleManagerDocmentForm_Load(object sender, EventArgs e)
        {
            for (byte i = 0; i < PublicDataClass._SaveRoleInfo.num; i++)
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", i));
                lv.SubItems.Add(PublicDataClass._SaveRoleInfo.RoleInfo[i].user);

                lv.SubItems.Add(PublicDataClass._SaveRoleInfo.RoleInfo[i].Password);
                if (PublicDataClass._SaveRoleInfo.RoleInfo[i].rightCfgState)
                    lv.SubItems.Add("<已配置>");
                else
                    lv.SubItems.Add("<未配置>");
                listView1.Items.Add(lv);
            }
            
            
            if (listView1.Items.Count == 0)
            {
                buttondelete.Enabled = false;
                buttonmodify.Enabled = false;
                buttonsave.Enabled   = false;

            }
        }

        private void buttonadd_Click(object sender, EventArgs e)
        {
            AddRoleForm rolefm = new AddRoleForm();
            rolefm.ShowDialog();
            if (rolefm.DialogResult == DialogResult.OK)
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.Items.Count));
                lv.SubItems.Add(PublicDataClass._AddRoleRecord.User);

                lv.SubItems.Add(PublicDataClass._AddRoleRecord.password);
                lv.SubItems.Add("<未配置>");
                listView1.Items.Add(lv);
            }
            EnabledOrDisableButton();
        }
        private void EnabledOrDisableButton()
        {
            if (this.listView1.Items.Count > 0)
            {
                buttondelete.Enabled = true;
                buttonmodify.Enabled = true;
                buttonsave.Enabled = true;
            }
            else
            {
                buttondelete.Enabled = false;
                buttonmodify.Enabled = false;
                buttonsave.Enabled   = false;
            }
        }
        private void buttondelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除此项吗?", "信  息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView1);

                if (SettleOnItem.Count <= 0)
                {
                    MessageBox.Show("记录项选择为空", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int i = 0; i < SettleOnItem.Count; )
                {

                    listView1.Items.Remove(SettleOnItem[i]);       //删除所选择的项
                }
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号

                }
                
            }    
        }

        private void buttonmodify_Click(object sender, EventArgs e)
        {

        }

        private void buttonsave_Click(object sender, EventArgs e)
        {
            PublicDataClass._SaveRoleInfo.num = Convert.ToByte(this.listView1.Items.Count);
            PublicDataClass._SaveRoleInfo.RoleInfo = new PublicDataClass._ROLEMANEGER[PublicDataClass._SaveRoleInfo.num];
            for (byte i = 0; i < PublicDataClass._SaveRoleInfo.num; i++)
            {

                PublicDataClass._SaveRoleInfo.RoleInfo[i].user     = listView1.Items[i].SubItems[1].Text;
                PublicDataClass._SaveRoleInfo.RoleInfo[i].Password = listView1.Items[i].SubItems[2].Text;
                PublicDataClass._SaveRoleInfo.RoleInfo[i].state = @"0";
            }


            string FileName = @"";
            FileStream afile;
            FileName = System.AppDomain.CurrentDomain.BaseDirectory + "\\User.txt";

            afile = new FileStream(FileName, FileMode.Create);


            StreamWriter sw = new StreamWriter(afile);


            sw.WriteLine(PublicDataClass._SaveRoleInfo.num);

            for (byte i = 0; i < PublicDataClass._SaveRoleInfo.num; i++)
            {
                sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].user);
                sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].Password);
                sw.WriteLine(PublicDataClass._SaveRoleInfo.RoleInfo[i].state);

            }
            sw.Close();
        }



        private void listView1_SubItemClicked(object sender, SubItemEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            if (e.SubItem == 0) // Password field
            {
                return;
            }

            else
            {

                if (e.SubItem == 3)
                {

                    UserRightsCfg userRight = new UserRightsCfg();

                    userRight.ShowDialog();
                    if (userRight.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
                    {
                        listView1.Items[e.Item.Index].SubItems[3].Text = "<已配置>";
                        //保存权限配置
                        PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].rightCfgState = true;
                       
                        for (int k = 0; k < 9; k++)
                        {                      
                           PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].AcqParamFlag[k]=PublicDataClass.CURRNTRIGHTCFG.AcqParamFlag[k];                                             
                        }
                         for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
                        {                      
                           PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].DynOpt[k]=PublicDataClass.CURRNTRIGHTCFG.DynOpt[k];                                             
                        }
                          PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].MoniterFlag=PublicDataClass.CURRNTRIGHTCFG.MoniterFlag;   
                          PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].CallDataFlag=PublicDataClass.CURRNTRIGHTCFG.CallDataFlag; 
                          PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].HistoryDataFlag=PublicDataClass.CURRNTRIGHTCFG.HistoryDataFlag; 
                          
                          for (int k = 0; k < 4; k++)
                        {                      
                           PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].ControlViewFlag[k]=PublicDataClass.CURRNTRIGHTCFG.ControlViewFlag[k];                                             
                        }
                         for (int k = 0; k < 5; k++)
                        {                      
                           PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].OtherTypeFlag[k]=PublicDataClass.CURRNTRIGHTCFG.OtherTypeFlag[k];                                             
                        }
                        
                        PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].RoleMangerFlag=PublicDataClass.CURRNTRIGHTCFG.RoleMangerFlag; 
                          PublicDataClass._SaveRoleInfo.RoleInfo[e.Item.Index].MoniterFlag=PublicDataClass.CURRNTRIGHTCFG.MoniterFlag; 

                    
                    }
                }
            }
        }
    }
}
