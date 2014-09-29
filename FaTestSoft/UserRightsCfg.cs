using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class UserRightsCfg : Form
    {
        public UserRightsCfg()
        {
            InitializeComponent();
        
        }

        private void UserRightsCfg_Load(object sender, EventArgs e)
        {
            InitialTree();
        }
        private void InitialTree()
        {
            TreeNode newNode1 = treeView1.Nodes.Add("可选功能列表");              //根节点
   
            TreeNode newNode11 = new TreeNode("实时监视");                                           // 1级节点
            newNode1.Nodes.Add(newNode11);

     

              TreeNode newNode12 = new TreeNode("实时召测");                                           // 1级节点
            newNode1.Nodes.Add(newNode12);

            TreeNode newNode13 = new TreeNode("历史数据");                                           // 1级节点
            newNode1.Nodes.Add(newNode13);

            TreeNode[] paraNode = new TreeNode[9];
            paraNode[0] = new TreeNode("参数配置");        
             newNode1.Nodes.Add(paraNode[0]);
             paraNode[1] = new TreeNode("串口参数");
             paraNode[0].Nodes.Add(paraNode[1]);
             paraNode[2] = new TreeNode("遥测配置");
             paraNode[0].Nodes.Add(paraNode[2]);
             paraNode[3] = new TreeNode("遥信配置");
             paraNode[0].Nodes.Add(paraNode[3]);
             paraNode[4] = new TreeNode("遥控配置");
             paraNode[0].Nodes.Add(paraNode[4]);
             paraNode[5] = new TreeNode("模拟量接入配置");
             paraNode[0].Nodes.Add(paraNode[5]);
             paraNode[6] = new TreeNode("校准参数配置");
             paraNode[0].Nodes.Add(paraNode[6]);
             paraNode[7] = new TreeNode("模拟量配置");
             paraNode[0].Nodes.Add(paraNode[7]);
   


            ////动态选项卡

            TreeNode[] nodearry = new TreeNode[PublicDataClass.FILENAME.Length];



            for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
            {

                nodearry[k] = new TreeNode(PublicDataClass.TabCfg[k].PageName);
                nodearry[k].ImageKey = "mdf_ndf_dbfiles.ico";
                nodearry[k].SelectedImageIndex = 4;
                paraNode[0].Nodes.Add(nodearry[k]);

            }
                /////////////////////////////////////////////////////////////





            TreeNode[] controlNode = new TreeNode[4];
            controlNode[0] = new TreeNode("控制命令");
            newNode1.Nodes.Add(controlNode[0]);
            controlNode[1] = new TreeNode("遥控命令");
            controlNode[0].Nodes.Add(controlNode[1]);
            controlNode[2] = new TreeNode("复位命令");
            controlNode[0].Nodes.Add(controlNode[2]);
            controlNode[3] = new TreeNode("复归命令");
            controlNode[0].Nodes.Add(controlNode[3]);

       
     


      
            TreeNode[] otherNode = new TreeNode[5];
            otherNode[0] = new TreeNode("其它操作");
            newNode1.Nodes.Add(otherNode[0]);
            otherNode[1] = new TreeNode("对时");
            otherNode[0].Nodes.Add(otherNode[1]);
            otherNode[2] = new TreeNode("文件传输");
            otherNode[0].Nodes.Add(otherNode[2]);
            otherNode[3] = new TreeNode("程序升级");
            otherNode[0].Nodes.Add(otherNode[3]);
            otherNode[4] = new TreeNode("报文监视");
            otherNode[0].Nodes.Add(otherNode[4]);
        

               TreeNode newNode17 = new TreeNode("角色管理");                                           // 1级节点
            newNode17.ImageKey = "MYCOMP27.ICO";
            newNode17.SelectedImageIndex = 8;
            newNode1.Nodes.Add(newNode17);
            treeView1.ExpandAll();
   


   
                


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {   
                setNodeStateTrue(e.Node);        
            //if (e.Node.Text == "实时监视")
            //    PublicDataClass.CURRNTRIGHTCFG.MoniterFlag = e.Node.Checked;
            //if (e.Node.Text == "实时召测")
            //    PublicDataClass.CURRNTRIGHTCFG.CallDataFlag = e.Node.Checked;
            //if (e.Node.Text == "历史数据")
            //    PublicDataClass.CURRNTRIGHTCFG.HistoryDataFlag = e.Node.Checked;
            //if (e.Node.Text == "实时监视")
            //    PublicDataClass.CURRNTRIGHTCFG.MoniterFlag = e.Node.Checked;

            //if (e.Node.Text == "参数配置")
       
            //    PublicDataClass.CURRNTRIGHTCFG.AcqParamFlag[0] = e.Node.Checked;
            //for (int a = 0; a < treeView1.Nodes[0].Nodes[3].Nodes.Count; a++)
            //{
            //    if (e.Node.Index == treeView1.Nodes[0].Nodes[3].Nodes[a].Index)
            //    {
            //        if (a < 9)
            //            PublicDataClass.CURRNTRIGHTCFG.AcqParamFlag[a + 1] = e.Node.Checked;
            //        else
            //            PublicDataClass.CURRNTRIGHTCFG.DynOpt[a - 9] = e.Node.Checked;

            //    }

            //}

            //    if (e.Node.Text == "控制命令")
            //        PublicDataClass.CURRNTRIGHTCFG.ControlViewFlag[0] = e.Node.Checked;
            //    for (int a = 0; a < treeView1.Nodes[0].Nodes[4].Nodes.Count; a++)
            //    {
            //        if (e.Node.Index == treeView1.Nodes[0].Nodes[4].Nodes[a].Index)
            //        {
                 
            //                PublicDataClass.CURRNTRIGHTCFG.ControlViewFlag[a +1] = e.Node.Checked;

            //        }

            //    }


            //if (e.Node.Text == "其它操作")
            //    PublicDataClass.CURRNTRIGHTCFG.OtherTypeFlag[0]= e.Node.Checked;
            //for (int a = 0; a < treeView1.Nodes[0].Nodes[5].Nodes.Count; a++)
            //{
            //    if (e.Node.Index == treeView1.Nodes[0].Nodes[5].Nodes[a].Index)
            //    {

            //        PublicDataClass.CURRNTRIGHTCFG.OtherTypeFlag[a + 1] = e.Node.Checked;

            //    }

            //}
            //if (e.Node.Text == "角色管理")
            //    PublicDataClass.CURRNTRIGHTCFG.RoleMangerFlag = e.Node.Checked;
            
  

     

        }
   
          private void setNodeStateTrue(TreeNode selNote)
        {
            foreach (TreeNode node in selNote.Nodes)
            {
                node.Checked = selNote.Checked;
                setNodeStateTrue(node);
            }
        
        
        
        }

          private void button1_Click(object sender, EventArgs e)
          {
       
              //保存当前角色权限配置
              PublicDataClass.CURRNTRIGHTCFG.rightCfgState = true;

          }


     
             
    }

    
}
