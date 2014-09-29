using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HWLibs.Windows.Forms;
using KD.WinFormsUI.Docking;
using FaTestSoft.CommonData;
using System.Xml;
using System.IO;

namespace FaTestSoft
{
    public partial class XMLCfg : DockContent
    {
        public XMLCfg()
        {
            InitializeComponent();

        }
        TabPage[] tp;
        int rownum = 0;
        XmlDocument xmlDoc = new XmlDocument();
        string[] NodeNamePathBuffer;//各节点路径
        DataGridViewGroupCell node;//第一个节点
        XmlNode root;
        string CurFName = @"";
        private void XMLCfg_Load(object sender, EventArgs e)
        {

         
        //        tp = new TabPage[PublicDataClass.FILENAME.Length];//
         //       PublicDataClass.FILENAME = new string[0];
       
         
        }
        public void Init()
        { 
          tp = new TabPage[PublicDataClass.FILENAME.Length];//
        }
        private void AddItems(string path)
        {
            //      XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            root = xmlDoc.DocumentElement;
            //初始化行,计算节点总数
            int rootnum = 1;
            rootnum += CountNodeNum(root);
            for (int i = 0; i < rootnum; i++)
            {
                grid.Rows.Add();
            }
            //初始化节点路径存储数组
            NodeNamePathBuffer = new string[rootnum];
            rownum = 0;

            //此段代码为显示根节点------------------------------------------------------------------
            //显示第一个节点
            //node = grid.Rows[rownum].Cells[0] as DataGridViewGroupCell;
            //XmlElement xe = (XmlElement)root;
            ////设置属性值
            //xe.SetAttribute("RowNum", Convert.ToString(rownum));
            ////grid.Rows[rownum].Cells[0].Value = xe.Name + xe.GetAttribute("RowNum");
            //grid.Rows[rownum].Cells[0].Value = xe.Name;
            //NodeNamePathBuffer[rownum] = "/" + root.Name;
            //rownum++;
            ////各节点扩展显示
            //ShowNodes(root, node);
            //-------------------------------------------------------------------------------------------------------------------



            ///此段代码为不显示根节点------------------------------------------------------------------
            //指定一个节点
            XmlNode r = xmlDoc.SelectSingleNode("Root");
            //XmlElement xer = (XmlElement)r;
            //  XmlNodeList xnf = r.ChildNodes;
        
            XmlNodeList xnf = root.ChildNodes;
            
                DataGridViewGroupCell[] cell = new DataGridViewGroupCell[xnf.Count];
                int j= 0;
                foreach (XmlNode xn in xnf)
                {
                    switch (xn.NodeType)
                    {
                        case XmlNodeType.Element:
                            XmlElement xe = (XmlElement)xn;
                            //设置属性值
                            xe.SetAttribute("RowNum", Convert.ToString(rownum));
                            cell[j] = grid.Rows[rownum].Cells[0] as DataGridViewGroupCell;
                          grid.Rows[rownum].Cells[0].Value = xe.Name;
                           //   grid.Rows[rownum].Cells[0].Value = xe.GetAttribute("name");
                            grid.Rows[rownum].Cells[1].Value = xe.GetAttribute("name");
                            grid.Rows[rownum].Cells[2].Value = xe.Value ;
                            grid.Rows[rownum].Cells[3].Value = xe.GetAttribute("width");//字节
                            grid.Rows[rownum].Cells[4].Value = xe.GetAttribute("unit");//单位
                            grid.Rows[rownum].Cells[5].Value = xe.GetAttribute("type");//数据类型
                      //      grid.Rows[rownum].Cells[6].Value = xe.GetAttribute("scope");//范围
                            grid.Rows[rownum].Cells[6].Value = xe.GetAttribute("desc");//说明

                            NodeNamePathBuffer[rownum] = "/" + root.Name + "/" + xe.Name;
                            rownum++;
                            ShowNodes(xn, cell[j]);
                            break;
                        case XmlNodeType.Text:
                            //text 在上一行中显示值即可
                            rownum--;
                            grid.Rows[rownum].Cells[2].Value = xn.Value;
                            rownum++;
                            break;
                    }
                    j++;
                }

        }
        private int CountNodeNum(XmlNode root)
        {
            int num = 0;
            if (root.HasChildNodes)
            {
                XmlNodeList xnf = root.ChildNodes;
                num += xnf.Count;
                foreach (XmlNode xn in xnf)
                {

                    switch (xn.NodeType)
                    {
                        case XmlNodeType.Element:
                            XmlElement xe = (XmlElement)xn;
                            num += CountNodeNum(xn);
                            break;
                        case XmlNodeType.Text:
                            num -= 1;
                            break;
                    }
                }
            }


            return num;

        }
        private void ShowNodes(XmlNode root, DataGridViewGroupCell item)
        {
            if (root.HasChildNodes)
            {

                int i = 0;
                XmlNodeList xnf = root.ChildNodes;
                DataGridViewGroupCell[] cell = new DataGridViewGroupCell[xnf.Count];
                foreach (XmlNode xn in xnf)
                {

                    switch (xn.NodeType)
                    {
                        case XmlNodeType.Element:
                            XmlElement xe = (XmlElement)xn;
                            //设置属性值
                            xe.SetAttribute("RowNum", Convert.ToString(rownum));
                            cell[i] = grid.Rows[rownum].Cells[0] as DataGridViewGroupCell;
                            item.AddChildCell(cell[i]);
                         //   grid.Rows[rownum].Cells[0].Value = xe.Name + xe.GetAttribute("RowNum"); ;
                          grid.Rows[rownum].Cells[0].Value = xe.Name ; 
                        //      grid.Rows[rownum].Cells[0].Value = xe.GetAttribute("name");
                            grid.Rows[rownum].Cells[1].Value = xe.GetAttribute("name");
                            grid.Rows[rownum].Cells[2].Value = xe.Value ;
                            grid.Rows[rownum].Cells[3].Value = xe.GetAttribute("width");//字节
                            grid.Rows[rownum].Cells[4].Value = xe.GetAttribute("unit");//单位
                            grid.Rows[rownum].Cells[5].Value = xe.GetAttribute("type");//数据类型
                       //     grid.Rows[rownum].Cells[6].Value = xe.GetAttribute("scope");//范围
                            grid.Rows[rownum].Cells[6].Value = xe.GetAttribute("desc");//说明
                          
                            NodeNamePathBuffer[rownum] = NodeNamePathBuffer[item.RowIndex] + "/" + xe.Name;
                            rownum++;
                            ShowNodes(xn, cell[i]);
                            break;
                        case XmlNodeType.Text:
                            //text 在上一行中显示值即可
                            rownum--;
                            grid.Rows[rownum].Cells[2].Value = xn.Value;
                            rownum++;
                            break;
                    }

                    i++;

                }

            }
            else
                return;


        }

        private void XMLCfg_Activated(object sender, EventArgs e)
        {
       //     if (PublicDataClass.DynOptCfgFlag == 1)
              DynOptProcess();//动态添加选项卡
        }
        public void DynOptProcess()               //动态选项卡处理函数
        {
            try
            {
                //for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
                //{
                //    if (this.tabControl1.Controls.Contains(tp[k]))
                //        this.tabControl1.Controls.Remove(tp[k]);
                //}

                //for (int k = 0; k < tp.Length; k++)
                //{
                //    if (this.tabControl1.Controls.Contains(tp[k]))
                //        this.tabControl1.Controls.Remove(tp[k]);
                //}
                this.tabControl1.Controls.Clear();
                for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
                {

                    //清除表
                    grid.Rows.Clear();
                    AddItems(PublicDataClass.FILENAME[k]);
        
                    tp[k] = new TabPage();

                    tp[k].Controls.Add(this.grid);
                    tp[k].Location = new System.Drawing.Point(4, 25);
                    string str = String.Format("tp_{0:d}", k);
                    tp[k].Name = str;
                    tp[k].Padding = new System.Windows.Forms.Padding(3);
                    tp[k].Size = new System.Drawing.Size(769, 375);
                 //   tp[k].TabIndex = 1+ k;//待定
                    tp[k].Text = Path.GetFileName(PublicDataClass.FILENAME[k]);
                    tp[k].UseVisualStyleBackColor = true;
                    tabControl1.Controls.Add(tp[k]);
                }
                tabControl1.SelectedIndex = 1;//显示第一个动态页
              //  tabPage1.Parent = null;//隐藏   tabPage1
            }

            catch 
            {
            }



        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            string str;
            try
            {
              
                             if (PublicDataClass.DynOptCfgFlag == 1)//动态选项卡已配置
                        {
                           
                            for (int k = 0; k < PublicDataClass.FILENAME.Length; k++)
                            {
                          
                            
                                str = String.Format("tp_{0:d}", k);
                                if (e.TabPage.Name == str)
                                {
                            
                                  //  ItemId = 0x14;
                                    grid.Rows.Clear();
                                    CurFName = PublicDataClass.FILENAME[k];
                                    AddItems(CurFName);
                                    tp[k].Controls.Add(grid );
                                  //  CheckNowDynOptParamState(k);//更新动态选项卡参数
                                }

                            }
                        }
                
            }
            catch
            { }
        }

        private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //读所有节点表               
            XmlNamespaceManager xnm = new XmlNamespaceManager(xmlDoc.NameTable);
            //根据路径选择节点    
            XmlNode node = xmlDoc.SelectSingleNode("//*[@RowNum='" + Convert.ToString(e.RowIndex) + "']");
           
            //       XmlNode node = xmlDoc.SelectSingleNode(NodeNamePathBuffer[e.RowIndex], xnm);   //node.InnerText 就是读取出来的值               
       //     if (node.NodeType == XmlNodeType.Text)
            {
                //将子节点类型转换为XmlElement类型
                XmlElement xe = (XmlElement)node;
                //修改值
                switch (e.ColumnIndex)
                {
                    //当前值行坐标DataGridView.CurrentCellAddress.Y
                    //当前值 .CurrentCell.Value
                    //  case 1: xe.SetAttribute("属性", (string)grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);//修改属性
                    case 1: xe.SetAttribute("name", (string)grid.CurrentCell.Value);//修改名称
                        break;
                    case 2: xe.Value = (string)grid.CurrentCell.Value;//修改值
                        break;
                    case 3: xe.SetAttribute("width", (string)grid.CurrentCell.Value);//修改字节
                        break;
                    case 4: xe.SetAttribute("unit", (string)grid.CurrentCell.Value);//修改单位
                        break;
                    case 5: xe.SetAttribute("type", (string)grid.CurrentCell.Value);//修改数据类型
                        break;
                    case 6: xe.SetAttribute("scope", (string)grid.CurrentCell.Value);//修改范围
                        break;
                    case 7: xe.SetAttribute("desc", (string)grid.CurrentCell.Value);//修改说明
                        break;


                }
            }
            xmlDoc.Save(CurFName);//保存。
        }
    }
}
