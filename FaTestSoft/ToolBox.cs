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
    public partial class ToolBox :DockContent
    {
        public ToolBox()
        {
            InitializeComponent();
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._ChangeFlag.ToolBoxTreeNeedUpdate == true)
            {
                PublicDataClass._ChangeFlag.ToolBoxTreeNeedUpdate = false;
                InitialTree();
                timer1.Enabled = false;
            }
        }
        /********************************************************************************************
        *  函数名：    InitialTree                                                                  *
        *  功能  ：    初始化树形控件                                                               *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-11-09                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void InitialTree()
        {
            TreeNode newNode1 = treeView1.Nodes.Add(PublicDataClass.PrjName);              //根节点
            //TreeNode newNode11 = new TreeNode("通道类");                                           // 1级节点
            //newNode1.Nodes.Add(newNode11);
            //TreeNode newNode12 = new TreeNode("设备类");                                           // 1级节点
            //newNode1.Nodes.Add(newNode12);
            TreeNode newNode13 = new TreeNode("规约类");                                           // 1级节点
            for (int i = 0; i < PublicDataClass.ProtocolIniInfo.num; i++)
            {
                TreeNode newNode21 = new TreeNode(PublicDataClass.ProtocolIniInfo.IniTable[i]);
                newNode13.Nodes.Add(newNode21);                                                      //为-规约类-增加2级节点
            }
            newNode1.Nodes.Add(newNode13);
            TreeNode newNode14 = new TreeNode("配置类");                                           // 1级节点

            newNode1.Nodes.Add(newNode14);
            treeView1.ExpandAll();

        }
        /********************************************************************************************
        *  函数名：    Tree_NodeMouseDoubleClick                                                    *
        *  功能  ：    鼠标双击树中节点的事件处理函数                                               *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2010-12-07                                                                   *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void Tree_NodeMouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode.Text == "通道类")
                {
                    //PublicDataClass._ChangeFlag.DocmentViewUpdate  = true;
                    //PublicDataClass._ChangeFlag.ChannelClassUpdata = true;
                }
                else if (treeView1.SelectedNode.Text == "设备类")
                {
                    //PublicDataClass._ChangeFlag.DocmentViewUpdate = true;
                    //PublicDataClass._ChangeFlag.CePointClassUpdate = true;

                }
                else  if (treeView1.SelectedNode.Text == "配置类")
                {
                    PublicDataClass._ChangeFlag.DocmentViewUpdate = true;
                    PublicDataClass._ChangeFlag.DeviceCalssUpdate = true;
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void ToolBox_Load(object sender, EventArgs e)
        {
            
        }
    }
}
