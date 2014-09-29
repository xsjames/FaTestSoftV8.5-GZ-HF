using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class FunctionListView : DockContent
    {
        public FunctionListView()
        {
            InitializeComponent();
        }

        private void FunctionListView_Load(object sender, EventArgs e)
        {
            //treeView1.ExpandAll();
        }

        private void Tree_NodeMouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
     
                 if (treeView1.SelectedNode.Text == "参数配置")   //==================zxl
                {
                    //if (PublicDataClass._ChangeFlag.Setflag == false)
                    //{ //PublicDataClass._ChangeFlag.AcquisitionParamUpdate = true;
                    //    Login fm1 = new Login();
                    //    fm1.Show();
                    //    fm1.Visible = true;
                    //    PublicDataClass._ChangeFlag.AcquisitionParamUpdate1 = true;
                    //}
                    //else
                        PublicDataClass._ChangeFlag.AcquisitionParamUpdate = true;
                }
                else if (treeView1.SelectedNode.Text == "实时监视")
                {
                    PublicDataClass._ChangeFlag.MoniterUpdate = true;


                }
                else if (treeView1.SelectedNode.Text == "实时召测")
                {

                    PublicDataClass._ChangeFlag.CallDataUpdate = true;

                }
                else if (treeView1.SelectedNode.Text == "历史数据")
                {

                    PublicDataClass._ChangeFlag.HistoryDataUpdate = true;

                }
                else if (treeView1.SelectedNode.Text == "控制命令")   //==================zxl
                {
                    //if (PublicDataClass._ChangeFlag.Setflag == false)
                    //{//PublicDataClass._ChangeFlag.ControlViewUpdate = true;
                    //    Login fm1 = new Login();
                    //    fm1.Show();
                    //    fm1.Visible = true;
                    //    PublicDataClass._ChangeFlag.ControlViewUpdate1 = true;
                    //}
                    //else
                        PublicDataClass._ChangeFlag.ControlViewUpdate = true;

                }
                else if (treeView1.SelectedNode.Text == "其它操作")     //==================zxl
                {
                    //if (PublicDataClass._ChangeFlag.Setflag == false)
                    //{//PublicDataClass._ChangeFlag.OtherTypeViewUpdate = true;
                    //    Login fm1 = new Login();
                    //    fm1.Show();
                    //    fm1.Visible = true;
                    //    PublicDataClass._ChangeFlag.OtherTypeViewUpdate1 = true;
                    //}
                    //else
                        PublicDataClass._ChangeFlag.OtherTypeViewUpdate = true;

                }
                 else if (treeView1.SelectedNode.Text == "协处理器")
                 {

                     PublicDataClass._ChangeFlag.XieCPU = true;

                 }
                else if (treeView1.SelectedNode.Text == "用户名" || treeView1.SelectedNode.Text == "密码")
                {
                    PublicDataClass._ChangeFlag.RoleMangerViewUpdate = true;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._ChangeFlag.FunlistViewTreeNeedUpdate == true)
            {
                PublicDataClass._ChangeFlag.FunlistViewTreeNeedUpdate = false;
                InitialTree();
                timer1.Enabled = false;
            }
        }
        private void InitialTree()
        {
            TreeNode newNode1 = treeView1.Nodes.Add(PublicDataClass.PrjName);              //根节点
            newNode1.ImageKey = "My Uploads XP.ico";
            newNode1.SelectedImageIndex = 2;
            
            TreeNode newNode11 = new TreeNode("功能列表");                                           // 1级节点
            newNode11.ImageKey = "MYCOMP27.ICO";
            newNode11.SelectedImageIndex = 0;

                TreeNode newNode21 = new TreeNode("实时监视");
                newNode21.ImageKey = "ICO20.ICO";
                newNode21.SelectedImageIndex = 1;
                newNode11.Nodes.Add(newNode21);


                TreeNode newNode31 = new TreeNode("实时召测");
                newNode31.ImageKey = "ICO152.ICO";
                newNode31.SelectedImageIndex = 7;
                newNode11.Nodes.Add(newNode31);


                TreeNode newNode41 = new TreeNode("历史数据");
                newNode41.ImageKey = "mdf_ndf_dbfiles.ico";
                newNode41.SelectedImageIndex = 3;
                newNode11.Nodes.Add(newNode41);

                //TreeNode newNode51= new TreeNode("系统参数");
                //newNode51.ImageKey = "ICO48.ICO";
                //newNode51.SelectedImageIndex = 6;
                //newNode11.Nodes.Add(newNode51);

                TreeNode newNode61 = new TreeNode("参数配置");
                newNode61.ImageKey = "17.ICO";
                newNode61.SelectedImageIndex = 11;
                newNode11.Nodes.Add(newNode61);

                TreeNode newNode71 = new TreeNode("控制命令");
                newNode71.ImageKey = "ICO7.ICO";
                newNode71.SelectedImageIndex = 4;
                newNode11.Nodes.Add(newNode71);

                TreeNode newNode81 = new TreeNode("其它操作");
                newNode81.ImageKey = "iMac OS Find.ico";
                newNode81.SelectedImageIndex = 5;
                newNode11.Nodes.Add(newNode81);

                TreeNode newNodeA1 = new TreeNode("协处理器");
                newNodeA1.ImageKey = "Be Icon World.ico";
                newNodeA1.SelectedImageIndex = 10;
                newNode11.Nodes.Add(newNodeA1);



            newNode1.Nodes.Add(newNode11);

       
        


            TreeNode newNode12 = new TreeNode("角色管理");                                           // 1级节点
            newNode12.ImageKey = "ICO220.ICO";
            newNode12.SelectedImageIndex = 9;
                     TreeNode newNode91= new TreeNode("用户名");
                     newNode91.ImageKey = "25.ico";
                     newNode91.SelectedImageIndex = 8; 
                     newNode12.Nodes.Add(newNode91);


                     TreeNode newNode101 = new TreeNode("密码");
                     newNode101.ImageKey = "ICO48.ICO";
                     newNode101.SelectedImageIndex = 6;
                     newNode12.Nodes.Add(newNode101);
            newNode1.Nodes.Add(newNode12);
            
            treeView1.ExpandAll();

        }
    }
}
