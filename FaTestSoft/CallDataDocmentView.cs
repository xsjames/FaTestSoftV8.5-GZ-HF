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
using FaTestSoft.FUNCTIONCLASS;
using System.IO;

namespace FaTestSoft
{
    public partial class CallDataDocmentView : DockContent
    {
        public CallDataDocmentView()
        {
            InitializeComponent();
        }
        private DataSet PaginationSet = new DataSet();  //定义存储数据的集合
        private DataTable PageTable = new DataTable();//定义一个数据表
        private DataTable LiShiTable = new DataTable();//定义一个数据表
        private DataTable SaveTable = new DataTable();
        private DataTable YcDataTable = new DataTable();
        private Page Pagefm = new Page();     //定义一个具有分页功能的类

        string[] datastr = new string[5];
        static int pagenum = 0;
        static int pagenumLine = 0;
        static int pagenumIndex = 0;
        static int pagenow = 1;                 //当前页号
        static int pagetotal = 0;                 //总页号
        static int datepos = 0;
        public int num = 0;
        public int index = 0;
        private int ty;
        string str = @"";
        bool First_Start;
        bool First_Frame = true;    //第一帧
        bool saveret = false;
        string savename;
        int delaytime;//循环召测间隔
        int ShowType = 1;//选择类型  0：按线路   1：按序号
     



        DataRow dr;
        /// <summary>
        /// 窗体的初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallDataDocmentView_Load(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            comboBoxData.SelectedIndex = 0;
            domainUpDownb.Enabled = true;
            checkBox1.Checked = false;
            comboBoxselete.Enabled = true;
            comboBoxselete.SelectedIndex = 1;





            if (PublicDataClass.SaveText.devicenum == 0)
            {
                comboBoxPoint.Text = "无信息";

            }
            else
            {
                for (byte i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    comboBoxPoint.Items.Add(PublicDataClass.SaveText.Device[i].PointName);
                }
                comboBoxPoint.Text = PublicDataClass.SaveText.Device[0].PointName;

            }
            num = PublicDataClass.SaveText.devicenum;


            DataSet PageSet = new DataSet();// 定义一个存储数据的集合

            PageSet.Clear();//清空数据集中原有内容
            //FillDataTable(selectString, ref PageSet);//型数据表中填充数据


            Pagefm.ItemsPerPage = 8;//设定每页显示多少行


            PageTable.Columns.Add("序号", System.Type.GetType("System.String"));
            PageTable.Columns.Add("名称", System.Type.GetType("System.String"));
            PageTable.Columns.Add("数值", System.Type.GetType("System.String"));
            PageTable.Columns.Add("单位", System.Type.GetType("System.String"));
            PageTable.Columns.Add("倍率", System.Type.GetType("System.String"));

            LiShiTable.Columns.Add("序号", System.Type.GetType("System.String"));
            LiShiTable.Columns.Add("名称", System.Type.GetType("System.String"));
            LiShiTable.Columns.Add("数值", System.Type.GetType("System.String"));
            LiShiTable.Columns.Add("单位", System.Type.GetType("System.String"));
            LiShiTable.Columns.Add("倍率", System.Type.GetType("System.String"));
            //③Add rows for DataTable
            //★Initialize the row
            /*DataRow dr = PageTable.NewRow();
            dr["序号"] = "A";
            dr["名称"] = "B";
            dr["数值"] = "C";
            dr["单位"] = "D";
            dr["倍率"] = "E";
            PageTable.Rows.Add(dr);*/


            //★Doesn't initialize the row
            /*DataRow dr1 = dt.NewRow();
            dt.Rows.Add(dr1);*/

            //if(PageSet.Tables.Count !=0)
            //Pagefm.SetDataSet(PageSet, out PageTable);//设置当前数据集中的内容
            //Display(PageTable);//显示数据

            ListData.Columns[1].Width = (splitContainer2.Panel1.Width - ListData.Columns[0].Width - ListData.Columns[3].Width - ListData.Columns[4].Width) / 2;
            ListData.Columns[2].Width = (splitContainer2.Panel1.Width - ListData.Columns[0].Width - ListData.Columns[3].Width - ListData.Columns[4].Width) / 2;
            //splitContainer2.Panel1.Width

            FistPage.Enabled = false;
            LastPage.Enabled = false;
            NextPage.Enabled = false;
            PreviousPage.Enabled = false;
            First_Start = true;

            for (int ch = 5; ch < 100; ch++)
            {
                domainUpDownb.Items.Add(ch);

            }
            domainUpDownb.SelectedIndex = 0;
        }
        private byte FindCfgInfo()
        {
            byte i = 0;
            for (i = 0; i < PublicDataClass.SaveText.cfgnum; i++)
            {
                if (comboBoxPoint.Text == PublicDataClass.SaveText.Device[i].PointName)
                {
                    break;
                }
            }
            return i;
        }
        /// <summary>
        /// 召测按钮---事件的处理过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCall_Click(object sender, EventArgs e)
        {
            datepos = 0;
            if (PublicDataClass.LinSPointName == "无信息")
            {
                MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                ty = PublicFunction.CheckPointOfCommunicationEntrace(comboBoxPoint.Text);
                if (ty == 0)
                {
                    MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }

            }
            if (comboBoxData.SelectedIndex == 0)    //F0_遥测
            {
                datepos = 0;
                //listView1.Items.Clear();
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.AloneCallYc_1 = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.AloneCallYc_1 = true;
                if (ty == 4)
                    PublicDataClass._UsbTaskFlag.AloneCallYc_1 = true;

            }
            else if (comboBoxData.SelectedIndex == 1)  //F1__遥信
            {
                datepos = 0;
                //listView1.Items.Clear();
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.AloneCallYx_1 = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;

                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.AloneCallYx_1 = true;

                if (ty == 4)
                    PublicDataClass._UsbTaskFlag.AloneCallYx_1 = true;
            }
            
            else if (comboBoxData.SelectedIndex == 2)          //版本号
            {
                //listView1.Items.Clear();
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.VERSION_1 = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.VERSION_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.VERSION_1 = true;

                if (ty == 4)
                    PublicDataClass._UsbTaskFlag.VERSION_1 = true;

            }
            else if (comboBoxData.SelectedIndex == 3)        //时间
            {
                //listView1.Items.Clear();
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.CALLTIME_1 = true;

                else if (ty == 2)
                    PublicDataClass._NetTaskFlag.CALLTIME_1 = true;
                else if (ty == 3)
                    PublicDataClass._GprsTaskFlag.CALLTIME_1 = true;
                else if (ty == 4)
                    PublicDataClass._UsbTaskFlag.CALLTIME_1 = true;

            }
            
            else if (comboBoxData.SelectedIndex == 4)        //硬件状态读取   zxl
            {
                datepos = 0;
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.READ_Hard_1 = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.READ_Hard_1 = true;
                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.READ_Hard_1= true;
                if (ty == 4)
                    PublicDataClass._UsbTaskFlag.READ_Hard_1 = true;
               

            }
            //else if (comboBoxData.SelectedIndex == 5)        //读索引记录
            //{
            //    PublicDataClass.seq = 1;
            //    PublicDataClass.seqflag = 0;
            //    PublicDataClass.SQflag = 0;
            //    PublicDataClass.ParamInfoAddr = 0x0600;
            //    if (ty == 1)
            //        PublicDataClass._ComTaskFlag.READ_P_1 = true;

            //    if (ty == 2)
            //        PublicDataClass._NetTaskFlag.READ_P_1 = true;
            //}
            
        }
        /// <summary>
        /// 定时器的事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param anthor="cuibj"></param>
        /// <param Time="11-04-28"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._WindowsVisable.CallDataUpdatVisable == true)  //窗体可见
            {

                if (PublicDataClass._Message.CallDataDocmentView == true)
                {
                    PublicDataClass._Message.CallDataDocmentView = false;


                    ProcessShowAllInfo();
                }
            }
            if (saveret)
            {
                saveret = false;
                ExportToText(savename);
                output.Text = "导出";
                
                    //MessageBox.Show("数据导出成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 窗体激活---事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallDataDocmentView_Activated(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.CallDataUpdatVisable = true;            //窗体可见
            timer1.Enabled = true;
            if (num == PublicDataClass.SaveText.devicenum)
                return;
            comboBoxPoint.Items.Clear();
            num = PublicDataClass.SaveText.devicenum;
            if (PublicDataClass.SaveText.devicenum == 0)
            {
                comboBoxPoint.Text = "无信息";

            }
            else
            {
                for (byte i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    comboBoxPoint.Items.Add(PublicDataClass.SaveText.Device[i].PointName);//添加测量点名称信息
                }
                comboBoxPoint.Text = PublicDataClass.SaveText.Device[0].PointName;

            }

        }
        /// <summary>
        /// 窗体失去焦点---不激活的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CallDataDocmentView_Deactivate(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.CallDataUpdatVisable = false;        //窗体不可见
            timer1.Enabled = false;
        
        }
        /// <summary>
        /// 处理设备应答的信息
        /// </summary>
        /// <param name="无"></param>
        /// <param name="无"></param>
        /// <param anthor="cuibj"></param>
        /// <param Time="11-04-28"></param>
        private void ProcessShowAllInfo()
        {
            if (comboBoxData.SelectedIndex == 0)
            {
                //if (PublicDataClass.RevNetFrameMsg == "遥测")
                ShowYcValueInfo();                      //显示遥测信息
             

            }
            else if (comboBoxData.SelectedIndex == 1)
            {
                //if (PublicDataClass.RevNetFrameMsg == "遥信")
                ShowYxValueInfo();                      //显示遥信信息

            }
            else if (comboBoxData.SelectedIndex == 2)                    //召测版本号
            {
                ShowDeviceInformation();
            }
            else if (comboBoxData.SelectedIndex == 3)   //时间的
            {
                ShowDeviceTime();

            }
            else if (comboBoxData.SelectedIndex == 4)
            {
                ShowHardInformation();                      //显示器件状态信息

            }
        }

        /// <summary>
        /// 自定义---函数  显示器件状态信息
        /// </summary>
        private void ShowHardInformation()
        {
            try
            {
                int pos = 0;
                //if (PageTable != null)
                //    PageTable.Clear();
                //if (SaveTable != null)
                //    SaveTable.Clear();
                //pagenum = 0;
                if (datepos == 0)
                {
                    pagenum = 0;
                    if (PageTable != null)
                        PageTable.Clear();
                    if (LiShiTable != null)
                        LiShiTable.Clear();
                    if (SaveTable != null)
                        SaveTable.Clear();
                }
                for (int j = 0; j < (PublicDataClass._DataField.FieldLen) / 11; j++)
                {
                    str = @"";
                    dr = PageTable.NewRow();


                    dr["序号"] = String.Format("{0:d}", j);
                    for (int i = 0; i < 10; i++)
                        str += Convert.ToChar(PublicDataClass._DataField.Buffer[pos + i]);
                    dr["名称"] = str;
                    pos += 10;
                    if (PublicDataClass._DataField.Buffer[pos] == 1)
                        str = @"正常";
                    else
                        str = @"故障";

                    dr["数值"] = str;
                    pos += 1;
                    dr["单位"] = "无";
                    dr["倍率"] = "无";
                    PageTable.Rows.Add(dr);
                    pagenum++;
                }
                if (PageTable != null)
                {
                    if (PageTable.DataSet != null)
                    {
                        PageTable.DataSet.Tables.Remove(PageTable);
                    }
                }
                CaligrationPageNum(pagenum);
                PaginationSet.Tables.Clear();
                PaginationSet.Tables.Add(PageTable);   // 
                Pagefm.SetDataSet(PaginationSet, out PageTable);
                Display(PageTable);

            }
            catch { }
        }
        /// <summary>
        /// 自定义---函数  显示版本信息
        /// </summary>
        private void ShowDeviceInformation()
        {
            try
            {
                byte[] bytes = new byte[4];
                float fdata = 0.0F;

                int type = 0;
                ////if (PageTable != null)
                ////    PageTable.Clear();

                ////if (SaveTable != null)
                ////    SaveTable.Clear();
                //pagenum = 0;
                if (datepos == 0)
                {
                    pagenum = 0;
                    if (PageTable != null)
                        PageTable.Clear();
                    if (LiShiTable != null)
                        LiShiTable.Clear();
                    if (SaveTable != null)
                        SaveTable.Clear();
                }
                if (PublicDataClass._DataField.FieldLen ==28)
                    type =1;
                else type=2;//国网
                for (byte j = 0; j < 5; j++)
                {
                    str = @"";
                    dr = PageTable.NewRow();


                    dr["序号"] = String.Format("{0:d}", j);

                    if (j == 0)
                    {
                        //lv.SubItems.Add("厂商代号:");
                        dr["名称"] = "厂商代号";//4
                        for (byte i = 0; i < 4; i++)
                            str += Convert.ToChar(PublicDataClass._DataField.Buffer[i]);
                            //str = "GKB";
                        //lv.SubItems.Add(str);
                        dr["数值"] = str;
                    }
                    else if (j == 1)
                    {

                        //lv.SubItems.Add("设备编号:");
                        if (type == 2)
                        {
                            dr["名称"] = "设备ID号";//24   
                            for (byte i = 4; i < 4 + 24; i++)
                                str += Convert.ToChar(PublicDataClass._DataField.Buffer[i]);
                        }
                        else if (type == 1)
                        {
                            dr["名称"] = "设备编号";
                            for (byte i = 4; i < 4 + 11; i++)
                                str += Convert.ToChar(PublicDataClass._DataField.Buffer[i]);
                        }

                        dr["数值"] = str;

                    }
                    else if (j == 2)
                    {
                        //lv.SubItems.Add("软件版本号:");
                        dr["名称"] = "软件版本号";//8
                        if (type == 2)
                        {
                            for (byte i = 28; i < 36; i++)
                                str += Convert.ToChar(PublicDataClass._DataField.Buffer[i]);
                        }
                        else if (type == 1)
                        {
                            for (byte i = 15; i < 21; i++)
                                str += Convert.ToChar(PublicDataClass._DataField.Buffer[i]);
                        }

                        dr["数值"] = str;

                    }
                    else if (j == 3)
                    {
                        //lv.SubItems.Add("终端软件发布时间:");
                        dr["名称"] = "终端软件发布时间";
                        if (type == 2)
                        {
                            str += String.Format("{0:x}", PublicDataClass._DataField.Buffer[38]);
                            str += "年";
                            str += String.Format("{0:x}", PublicDataClass._DataField.Buffer[37]);
                            str += "月";
                            str += String.Format("{0:x}", PublicDataClass._DataField.Buffer[36]);
                            str += "日";
                        }
                        else if (type == 1)
                        {
                            str += String.Format("{0:x}", PublicDataClass._DataField.Buffer[23]);
                            str += "年";
                            str += String.Format("{0:x}", PublicDataClass._DataField.Buffer[22]);
                            str += "月";
                            str += String.Format("{0:x}", PublicDataClass._DataField.Buffer[21]);
                            str += "日";
                        }

                        dr["数值"] = str;

                    }
                    //else if (j == 4)
                    //{
                    //    //lv.SubItems.Add("终端配置容量信息码:");
                    //    dr["名称"] = "终端配置容量信息码";
                    //    for (byte i = 22; i < 24; i++)
                    //        str += Convert.ToChar(PublicDataClass._DataField.Buffer[i]);
                    //    dr["数值"] = str;

                    //}
                    else if (j == 4)                                            //zxl120409添加
                    {
                        dr["名称"] = "温度";
                        //bytes[0] = PublicDataClass._DataField.Buffer[39];
                        //bytes[1] = PublicDataClass._DataField.Buffer[40];
                        //bytes[2] = PublicDataClass._DataField.Buffer[41];
                        //bytes[3] = PublicDataClass._DataField.Buffer[42];
                        //bytes[0] = PublicDataClass._DataField.Buffer[24];
                        //bytes[1] = PublicDataClass._DataField.Buffer[25];
                        //bytes[2] = PublicDataClass._DataField.Buffer[26];
                        //bytes[3] = PublicDataClass._DataField.Buffer[27];
                        if (type == 2)
                            Array.Copy(PublicDataClass._DataField.Buffer, 39, bytes, 0, 4);
                        else if (type == 1)
                            Array.Copy(PublicDataClass._DataField.Buffer, 24, bytes, 0, 4);
                        fdata = BitConverter.ToSingle(bytes, 0);
                        dr["数值"] = String.Format("{0:f4}", fdata);
                        //dr["数值"] = str; 
                    }
                    dr["单位"] = "无";
                    dr["倍率"] = "无";
                    PageTable.Rows.Add(dr);
                    pagenum++;
                    //lv.SubItems.Add("无");
                    // lv.SubItems.Add("无");

                    //listView1.Items.Add(lv);
                }
                if (PageTable != null)
                {
                    if (PageTable.DataSet != null)
                    {
                        PageTable.DataSet.Tables.Remove(PageTable);
                    }
                }
                CaligrationPageNum(pagenum);
                PaginationSet.Tables.Clear();
                PaginationSet.Tables.Add(PageTable);   // 
                Pagefm.SetDataSet(PaginationSet, out PageTable);
                Display(PageTable);



            }
            catch { }
        }
        private void ShowDeviceTime()
        {
            try
            {
                //pagenum = 0;
                //if (PageTable != null)
                //    PageTable.Clear();

                //if (SaveTable != null)
                //    SaveTable.Clear();
                if (datepos == 0)
                {
                    pagenum = 0;
                    if (PageTable != null)
                        PageTable.Clear();
                    if (LiShiTable != null)
                        LiShiTable.Clear();
                    if (SaveTable != null)
                        SaveTable.Clear();
                }
                str = @"";

                dr = PageTable.NewRow();

                dr["序号"] = String.Format("{0:g}", 0);
                dr["名称"] = "当前时间:";

                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 1]);
                str += "年";
                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 2]);
                str += "月";

                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 3] & 0x1f);  //日+星期
                str += "日";

                str += "[星期" + String.Format("{0:d}", (PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 3] & 0xe0) >> 5) + "]";

                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 4]);
                str += "时";
                str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 5]);
                str += "分";
                int ms = (PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 6] << 8) +
                      PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen - 7];

                str += String.Format("{0:d}", ms);
                str += "毫秒";

                dr["数值"] = str;
                dr["单位"] = "<时间>";
                dr["倍率"] = "无";
                PageTable.Rows.Add(dr);
                pagenum++;
                if (PageTable != null)
                {
                    if (PageTable.DataSet != null)
                    {
                        PageTable.DataSet.Tables.Remove(PageTable);
                    }
                }
                CaligrationPageNum(pagenum);
                PaginationSet.Tables.Clear();
                PaginationSet.Tables.Add(PageTable);   // 
                Pagefm.SetDataSet(PaginationSet, out PageTable);
                Display(PageTable);
            }
            catch
            { }
        }
        /// <summary>
        /// 显示遥测数值信息
        /// </summary>
        private void ShowYcValueInfo()
        {
            if (PublicDataClass._ChangeFlag.Clearflag == true)
            {
                datepos = 0;
                PublicDataClass._ChangeFlag.Clearflag = false;
            }
            try
            {



                int startp = 0;//其实点号位置
                int StartPos = 0;
                byte[] bytes = new byte[4];
                float fdata = 0.0F;
                int idate = 0;
                int k = 0;
                string StartName = @"";
                byte dx = PublicFunction.FindPointNameCorrelativeIndex(comboBoxPoint.Text);
                if (PublicDataClass.ChanelId == 2)
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }

                    StartPos = PublicDataClass._DataField.Buffer[2];
                    StartPos = StartPos << 16;
                    StartPos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);


                    StartName = PublicFunction.FindStartPosCorrelativeName(1, StartPos, dx);       //找起始点号
                    startp = PublicFunction.FindStartPos(1, StartPos, dx);//找起始点号位置
                    //if (startp==65535)
                    //{
                    //    MessageBox.Show("上传报文信息体地址不在配置表范围内！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    return;

                    //}
                    if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                    {

                        return;

                    }


                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @"";


                        dr = LiShiTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", datepos);

                        if (PublicDataClass.SaveText.Cfg[dx].YccfgState[datepos] == "是")
                        {
                            dr["名称"] = PublicDataClass.SaveText.Cfg[dx].YccfgName[datepos];

                            if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))//浮点型
                            {
                                
                                bytes[0] = PublicDataClass._DataField.Buffer[k + 3];
                                bytes[1] = PublicDataClass._DataField.Buffer[k + 4];
                                bytes[2] = PublicDataClass._DataField.Buffer[k + 5];
                                bytes[3] = PublicDataClass._DataField.Buffer[k + 6];

                                fdata = BitConverter.ToSingle(bytes, 0);
                                k += 5;
                                dr["数值"] = String.Format("{0:f4}", fdata);
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = String.Format("{0:f4}", fdata);
                                 
                            }
                            if ((PublicDataClass.DataTy == 36)|| (PublicDataClass.DataTy == 38))//整型  带品质描述
                            {
                                int date;
                                date = PublicDataClass._DataField.Buffer[k + 3] + (PublicDataClass._DataField.Buffer[k + 4] << 8);

                                k += 3;
                                if (date > 0x8000)
                                    date = date - 65536;
                                dr["数值"] = Convert.ToString(date);
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = Convert.ToString(date);

                            }
                            if (PublicDataClass.DataTy == 42)//整型 不带品质描述
                            {
                                int date;
                                date = PublicDataClass._DataField.Buffer[k + 3] + (PublicDataClass._DataField.Buffer[k + 4] << 8);

                                k += 2;
                                if (date > 0x8000)
                                    date = date - 65536;
                                dr["数值"] = Convert.ToString(date);
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = Convert.ToString(date);

                            }

                            dr["单位"] = "无";
                            dr["倍率"] = "无";
                            LiShiTable.Rows.Add(dr);
                            pagenum++;

                        }
                        datepos++;
                        startp++;
                        StartPos++;
                        if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                        {

                            break;

                        }


                    }
                    //if (First_Frame == true)  //首帧
                    //{
                    //    First_Frame = false;

                    //}
                    //if (datepos == 0) //首帧
                    //{
                    //    First_Frame = false;
                    // }

                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);

                    CaligrationPageNum(pagenum);
                    if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenumLine = pagenum;              //zxl 0416
                        pagenumIndex = pagenum;             //zxl 0416
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();


                        //if (ShowType == 0)   //选择类型  0：按线路   1：按序号
                        //{
                        //    byte po = FindCfgInfo();
                        //    if (SaveTable.Rows.Count == 0)
                        //        return;
                        //    object[] item = new object[SaveTable.Columns.Count];//定义一个object类型的数组
                        //    PageTable.Clear();
                        //    CaligrationPageNum(pagenumIndex);
                        //    pagenumLine = pagenumIndex;

                        //    PaginationSet.Tables.Clear();

                        //    for (int i = 0; i < pagenumIndex; i++)//循环遍历数据表中的每一行数据
                        //    {
                        //        for (int j = 0; j < pagenumIndex; j++)
                        //        {
                        //            //     if (Convert.ToInt16(PublicDataClass._YcInformationParam.IndexTable[j]) == i)
                        //            if (Convert.ToInt16(PublicDataClass.SaveText.Cfg[po].YccfgIndex[j]) == i)
                        //            {
                        //                for (int m = 0; m < SaveTable.Columns.Count; m++)//循环遍历数据表中每一列数据
                        //                {
                        //                    item[m] = SaveTable.Rows[j][m];

                        //                }
                        //                PageTable.Rows.Add(item);
                        //            }
                        //        }
                        //    }
                        //    if (PageTable.DataSet != null)
                        //    {
                        //        PageTable.DataSet.Tables.Remove(PageTable);
                        //    }

                        //    PaginationSet.Tables.Clear();
                        //    PaginationSet.Tables.Add(PageTable);
                        //    Pagefm.SetDataSet(PaginationSet, out PageTable);


                        //}
                    }
                    Display(PageTable);





                }
                else if (PublicDataClass.ChanelId == 1)
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }

                    StartPos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);


                    StartName = PublicFunction.FindStartPosCorrelativeName(1, StartPos, dx);       //找起始点号
                    startp = PublicFunction.FindStartPos(1, StartPos, dx);//找起始点号位置
                    
                    if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                    {

                        return;

                    }


                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @"";


                        dr = LiShiTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", datepos);

                        if (PublicDataClass.SaveText.Cfg[dx].YccfgState[datepos] == "是")
                        {
                            dr["名称"] = PublicDataClass.SaveText.Cfg[dx].YccfgName[datepos];

                            if ((PublicDataClass.DataTy == 35) || (PublicDataClass.DataTy == 40))//浮点型
                            {
                                if (PublicDataClass.inflen == 2)
                                {
                                    bytes[0] = PublicDataClass._DataField.Buffer[k + 2];
                                    bytes[1] = PublicDataClass._DataField.Buffer[k + 3];
                                    bytes[2] = PublicDataClass._DataField.Buffer[k + 4];
                                    bytes[3] = PublicDataClass._DataField.Buffer[k + 5];
                                }
                                else if (PublicDataClass.inflen == 3)
                                {
                                    bytes[0] = PublicDataClass._DataField.Buffer[k + 3];
                                    bytes[1] = PublicDataClass._DataField.Buffer[k + 4];
                                    bytes[2] = PublicDataClass._DataField.Buffer[k + 5];
                                    bytes[3] = PublicDataClass._DataField.Buffer[k + 6];
                                }

                                fdata = BitConverter.ToSingle(bytes, 0);
                                k += 5;
                                dr["数值"] = String.Format("{0:f4}", fdata);
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = String.Format("{0:f4}", fdata);
                                 
                            }
                            if ((PublicDataClass.DataTy == 36)|| (PublicDataClass.DataTy == 38))//整型  带品质描述
                            {
                                int date=0;
                                if (PublicDataClass.inflen == 2)
                                    date = PublicDataClass._DataField.Buffer[k + 2] + (PublicDataClass._DataField.Buffer[k + 3] << 8);
                                else if (PublicDataClass.inflen == 3)
                                    date = PublicDataClass._DataField.Buffer[k + 3] + (PublicDataClass._DataField.Buffer[k + 4] << 8);

                                k += 3;
                                if (date > 0x8000)
                                    date = date - 65536;
                                dr["数值"] = Convert.ToString(date);
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = Convert.ToString(date);

                            }
                            if (PublicDataClass.DataTy == 42)//整型 不带品质描述
                            {
                                int date=0;
                                if (PublicDataClass.inflen == 2)
                                    date = PublicDataClass._DataField.Buffer[k + 2] + (PublicDataClass._DataField.Buffer[k + 3] << 8);
                                else if (PublicDataClass.inflen == 3)
                                    date = PublicDataClass._DataField.Buffer[k + 3] + (PublicDataClass._DataField.Buffer[k + 4] << 8);

                                k += 2;
                                if (date > 0x8000)
                                    date = date - 65536;
                                dr["数值"] = Convert.ToString(date);
                                PublicDataClass.SaveText.Cfg[dx].Ycdata[datepos] = Convert.ToString(date);

                            }

                            dr["单位"] = "无";
                            dr["倍率"] = "无";
                            LiShiTable.Rows.Add(dr);
                            pagenum++;

                        }
                        datepos++;
                        startp++;
                        StartPos++;
                        if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                        {

                            break;

                        }


                    }
                    //if (First_Frame == true)  //首帧
                    //{
                    //    First_Frame = false;

                    //}
                    //if (datepos == 0) //首帧
                    //{
                    //    First_Frame = false;
                    // }

                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);

                    CaligrationPageNum(pagenum);
                    if (datepos >= PublicDataClass.SaveText.Cfg[dx].YccfgNum)
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenumLine = pagenum;              //zxl 0416
                        pagenumIndex = pagenum;             //zxl 0416
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();
                    }
                    Display(PageTable);


                }
            }
            catch { }
        }
        /// <summary>
        /// 显示遥信数值信息
        /// </summary>
        private void ShowYxValueInfo()
        {
            if (PublicDataClass._ChangeFlag.Clearflag == true)
            {   datepos = 0;
                PublicDataClass._ChangeFlag.Clearflag = false;
            }
            try
            {
                  int a = PublicDataClass._DataField.FieldVSQ;
                string StartName = @"";
                int startp = 0;//其实点号位置
                int StartPos = 0;
                byte yxdata=0;
                byte dx = PublicFunction.FindPointNameCorrelativeIndex(comboBoxPoint.Text);
                if (PublicDataClass.ChanelId == 2)
                {

                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }

                    StartPos = PublicDataClass._DataField.Buffer[2];
                    StartPos = StartPos << 16;
                    StartPos += PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);



                    StartName = PublicFunction.FindStartPosCorrelativeName(2, StartPos, dx);
                    startp = PublicFunction.FindStartPos(2, StartPos, dx);//找起始点号位置//找起始点号

                    if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                    {

                        return;

                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @"";

                        dr = LiShiTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", datepos);
                        if (PublicDataClass.SaveText.Cfg[dx].YxcfgState[datepos] == "是")
                        {

                            dr["名称"] = PublicDataClass.SaveText.Cfg[dx].YxcfgName[datepos];



                            yxdata = PublicDataClass._DataField.Buffer[j + 3];
                            if (PublicDataClass.DataTy == 53)
                            {
                                if (yxdata == 1)
                                {
                                    dr["数值"] = "分";
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "分";
                                }
                                else
                                {
                                    dr["数值"] = "合";
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "合";
                                }
                            }
                            else
                            {
                                if (yxdata == 0)
                                {
                                    dr["数值"] = "分";
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "分";
                                }
                                else
                                {
                                    dr["数值"] = "合";
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "合";
                                }
                            }
                            dr["单位"] = "无";
                            dr["倍率"] = "无";
                            LiShiTable.Rows.Add(dr);
                            pagenum++;
                        }

                        datepos++;
                        startp++;
                        if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                        {

                            break;

                        }
                        //if (startp >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                        //{

                        //    break;
                        //}
                        StartPos++;

                    }


                    //if (datepos == 0) //首帧
                    //{
                    //    First_Frame = false;
                    //}
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }

                    PaginationSet.Tables.Clear();
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);

                    CaligrationPageNum(pagenum);

                    if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();
                    }
                    Display(PageTable);
                }
                else if (PublicDataClass.ChanelId == 1)  //串口
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }

                    StartPos = PublicDataClass._DataField.Buffer[0] + (PublicDataClass._DataField.Buffer[1] << 8);



                    StartName = PublicFunction.FindStartPosCorrelativeName(2, StartPos, dx);
                    startp = PublicFunction.FindStartPos(2, StartPos, dx);//找起始点号位置//找起始点号

                    if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                    {

                        return;

                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @"";

                        dr = LiShiTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", datepos);
                        if (PublicDataClass.SaveText.Cfg[dx].YxcfgState[datepos] == "是")
                        {
                            dr["名称"] = PublicDataClass.SaveText.Cfg[dx].YxcfgName[datepos];
                            if (PublicDataClass.inflen == 2)
                                yxdata = PublicDataClass._DataField.Buffer[j + 2];
                            else if (PublicDataClass.inflen == 3)
                                yxdata = PublicDataClass._DataField.Buffer[j + 3];
                           
                            //dr["数值"] = String.Format("{0:d}", yxdata);
                            if (PublicDataClass.DataTy == 53)
                            {
                                if (yxdata == 1)
                                {
                                    dr["数值"] = "分";
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "分";
                                }
                                else
                                {
                                    dr["数值"] = "合";
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "合";
                                }
                            }
                            else
                            {
                                if (yxdata == 0)
                                {
                                    dr["数值"] = "分";
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "分";
                                }
                                else
                                {
                                    dr["数值"] = "合";
                                    PublicDataClass.SaveText.Cfg[dx].Yxdata[datepos] = "合";
                                }
                            }
                            dr["单位"] = "无";
                            dr["倍率"] = "无";
                            LiShiTable.Rows.Add(dr);
                            pagenum++;
                        }

                        datepos++;
                        startp++;
                        if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                        {

                            break;

                        }
                       
                        StartPos++;

                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }

                    PaginationSet.Tables.Clear();
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);

                    CaligrationPageNum(pagenum);

                    if (datepos >= PublicDataClass.SaveText.Cfg[dx].YxcfgNum)
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();
                    }
                    Display(PageTable);
                }
            }
            catch
            { }


        }

        #region Show Param Infomation
        /// <summary>
        /// 显示参数信息
        /// </summary>
        private void ShowParamInformation()
        {
           /* if (datepos == 0)
            {
                pagenum = 0;
                if (PageTable != null)
                    PageTable.Clear();
                if (LiShiTable != null)
                    LiShiTable.Clear();
                if (SaveTable != null)
                    SaveTable.Clear();
            }
            if (datepos >= PublicDataClass._RaoDong.num / 3)
            {

                return;

            }
            for (int j = 0; j < PublicDataClass._RaoDong.num / 3; j++)
            {
                str = @""; dr = PageTable.NewRow();
                dr["序号"] = String.Format("{0:d}", datepos);
                dr["名称"] = PublicDataClass._RaoDong.NameTable[datepos];
                if (PublicDataClass._RaoDong.ByteTable[datepos] == "4")
                {
                    byte[] bytes = new byte[4];
                    bytes[0] = PublicDataClass._DataField.Buffer[pos];
                    bytes[1] = PublicDataClass._DataField.Buffer[pos + 1];
                    bytes[2] = PublicDataClass._DataField.Buffer[pos + 2];
                    bytes[3] = PublicDataClass._DataField.Buffer[pos + 3];

                    float fdata = BitConverter.ToSingle(bytes, 0);

                    str += String.Format("{0:f4}", fdata);

                    pos += 4;
                }
                if (PublicDataClass._RaoDong.ByteTable[datepos] == "2")
                {
                    str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                    pos += 2;
                }
                if (PublicDataClass._RaoDong.ByteTable[datepos] == "1")
                {
                    str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                    pos += 1;
                }

                datepos++;
                dr["数值"] = str;
                dr["单位"] = "无";
                dr["倍率"] = "无";
                LiShiTable.Rows.Add(dr);
                pagenum++;
                if (datepos >= PublicDataClass._RaoDong.num / 3)
                {

                    break;

                }
            }
            if (First_Frame == true)  //首帧
            {
                First_Frame = false;

            }

            if (PageTable != null)
            {
                if (PageTable.DataSet != null)
                {
                    PageTable.DataSet.Tables.Remove(PageTable);
                }
            }
            PageTable = LiShiTable.Copy();
            PaginationSet.Tables.Clear();
            PaginationSet.Tables.Add(PageTable);   //
            Pagefm.SetDataSet(PaginationSet, out PageTable);


            CaligrationPageNum(pagenum);


            if (datepos >= (PublicDataClass._RaoDong.num / 3))
            {
                First_Frame = true;
                //datepos = 0;
                pagenum = 0;
                SaveTable = LiShiTable.Copy();
            }
            Display(PageTable);*/


            try
            {
                int pos = 0;
                //byte[] bytes = new byte[4];
                //float fdata = 0.0F;

                if ((PublicDataClass.ParamInfoAddr >= 0x7001) && (PublicDataClass.ParamInfoAddr <= 0x7fff))     //遥测参数
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    if (datepos >= PublicDataClass._YcParam.num / 3)
                    {

                        return;

                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @"";
                        dr = LiShiTable.NewRow();


                        //   dr["序号"] = String.Format("{0:d}", j);
                        dr["序号"] = String.Format("{0:d}", datepos);
                        dr["名称"] = PublicDataClass._YcParam.NameTable[datepos];
                        if (PublicDataClass._YcParam.ByteTable[datepos] == "4")
                        {
                            byte[] bytes = new byte[4];
                            bytes[0] = PublicDataClass._DataField.Buffer[pos];
                            bytes[1] = PublicDataClass._DataField.Buffer[pos + 1];
                            bytes[2] = PublicDataClass._DataField.Buffer[pos + 2];
                            bytes[3] = PublicDataClass._DataField.Buffer[pos + 3];

                            float fdata = BitConverter.ToSingle(bytes, 0);

                            str += String.Format("{0:f4}", fdata);

                            pos += 4;
                        }
                        if (PublicDataClass._YcParam.ByteTable[datepos] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._YcParam.ByteTable[datepos] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }
                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        LiShiTable.Rows.Add(dr);
                        pagenum++;
                        datepos++;

                        if (datepos >= PublicDataClass._YcParam.num / 3)
                        {

                            break;
                        }
                    }
                    if (First_Frame == true)  //首帧
                    {
                        First_Frame = false;

                    }

                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);


                    CaligrationPageNum(pagenum);


                    if (datepos >= (PublicDataClass._YcParam.num / 3))
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();
                    }
                    Display(PageTable);
                   
                }
                if ((PublicDataClass.ParamInfoAddr >= 0x8001) && (PublicDataClass.ParamInfoAddr <= 0x8fff))     //遥信参数
                {
                    
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    for (int j = 0; j < PublicDataClass._YxParam.num / 3; j++)
                    {
                        str = @"";
                        dr = PageTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", j);
                        dr["名称"] = PublicDataClass._YxParam.NameTable[j];

                        if (PublicDataClass._YxParam.ByteTable[j] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._YxParam.ByteTable[j] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }
                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        PageTable.Rows.Add(dr);
                        pagenum++;
                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    CaligrationPageNum(pagenum);
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);

                }
                if ((PublicDataClass.ParamInfoAddr >= 0x9001) && (PublicDataClass.ParamInfoAddr <= 0x9fff))  //遥控参数
                {
                    

                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    if (datepos >= PublicDataClass._YkParam.num / 3)
                    {

                        return;

                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @"";
                        dr = LiShiTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", datepos);
                        dr["名称"] = PublicDataClass._YkParam.NameTable[datepos];

                        if (PublicDataClass._YkParam.ByteTable[datepos] == "4")
                        {
                            byte[] bytes = new byte[4];
                            bytes[0] = PublicDataClass._DataField.Buffer[pos];
                            bytes[1] = PublicDataClass._DataField.Buffer[pos + 1];
                            bytes[2] = PublicDataClass._DataField.Buffer[pos + 2];
                            bytes[3] = PublicDataClass._DataField.Buffer[pos + 3];

                            float fdata = BitConverter.ToSingle(bytes, 0);

                            str += String.Format("{0:f4}", fdata);

                            pos += 4;
                        }
                        if (PublicDataClass._YkParam.ByteTable[datepos] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._YkParam.ByteTable[datepos] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }
                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        datepos++;
                        LiShiTable.Rows.Add(dr);
                        pagenum++;
                        if (datepos >= PublicDataClass._YkParam.num / 3)
                        {

                            break;

                        }

                    }
                    if (First_Frame == true)  //首帧
                    {
                        First_Frame = false;

                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);


                    CaligrationPageNum(pagenum);

                    if (datepos >= (PublicDataClass._YkParam.num / 3))
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();
                    }
                    Display(PageTable);

                }
                else if ((PublicDataClass.ParamInfoAddr >= 0x5001) && (PublicDataClass.ParamInfoAddr <= 0x50ff))    //网络参数
                {
                    byte ch = 0;
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    for (int k = 0; k < PublicDataClass._DataField.FieldVSQ; k++)
                    {

                        dr = PageTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", ch);
                        dr["名称"] = String.Format("网口{0:d}的信息", k);
                        dr["数值"] = "";
                        dr["单位"] = "";
                        dr["倍率"] = "";
                        PageTable.Rows.Add(dr);
                        pagenum++;
                        for (byte j = 0; j < 5; j++)
                        {
                            ch++;
                            dr = PageTable.NewRow();
                            dr["序号"] = String.Format("{0:d}", ch);

                            str = @"";

                            if (j == 0)
                            {

                                dr["名称"] = "IP地址:";
                                for (int i = pos; i < pos + 4; i++)
                                {
                                    str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[i]);
                                    if (i < (pos + 3))
                                        str += ".";
                                }
                                pos += 4;
                                dr["数值"] = str;
                            }
                            else if (j == 1)
                            {

                                dr["名称"] = "掩码:";
                                for (int i = pos; i < pos + 4; i++)
                                {
                                    str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[i]);
                                    if (i < (pos + 3))
                                        str += ".";
                                }
                                pos += 4;
                                dr["数值"] = str;
                            }
                            else if (j == 2)
                            {

                                dr["名称"] = "网关:";
                                for (int i = pos; i < pos + 4; i++)
                                {
                                    str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[i]);
                                    if (i < (pos + 3))
                                        str += ".";
                                }
                                pos += 4;
                                dr["数值"] = str;
                                //ptr += 4;
                            }
                            else if (j == 3)
                            {
                                dr["名称"] = "端口号:";
                                str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                                dr["数值"] = str;
                                pos += 2;

                            }
                            else
                            {
                                dr["名称"] = "MAC地址:";
                                for (int i = pos; i < pos + 6; i++)
                                {
                                    str += String.Format("{0:x2}", PublicDataClass._DataField.Buffer[i]);
                                    if (i < (pos + 5))
                                        str += "-";
                                }
                                pos += 6;
                                dr["数值"] = str;

                            }
                            dr["单位"] = "";
                            dr["倍率"] = "";
                            pagenum++;
                            PageTable.Rows.Add(dr);
                        }

                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    CaligrationPageNum(pagenum);
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);
                }
                else if ((PublicDataClass.ParamInfoAddr >= 0x5100) && (PublicDataClass.ParamInfoAddr <= 0x51ff))    //系统参数
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @"";
                        dr = PageTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", j);
                        dr["名称"] = PublicDataClass._SysParam.NameTable[j];


                        if (PublicDataClass._SysParam.ByteTable[j] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._SysParam.ByteTable[j] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }
                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        PageTable.Rows.Add(dr);
                        pagenum++;

                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    CaligrationPageNum(pagenum);
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);
                }
                
                else if ((PublicDataClass.ParamInfoAddr >= 0x5300) && (PublicDataClass.ParamInfoAddr <= 0x53ff))  //越限配置
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    if (datepos >= PublicDataClass._RaoDong.num / 3)
                    {

                        return;

                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @""; dr = LiShiTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", datepos);
                        dr["名称"] = PublicDataClass._RaoDong.NameTable[datepos];
                        if (PublicDataClass._RaoDong.ByteTable[datepos] == "4")
                        {
                            byte[] bytes = new byte[4];
                            bytes[0] = PublicDataClass._DataField.Buffer[pos];
                            bytes[1] = PublicDataClass._DataField.Buffer[pos + 1];
                            bytes[2] = PublicDataClass._DataField.Buffer[pos + 2];
                            bytes[3] = PublicDataClass._DataField.Buffer[pos + 3];

                            float fdata = BitConverter.ToSingle(bytes, 0);

                            str += String.Format("{0:f4}", fdata);

                            pos += 4;
                        }
                        if (PublicDataClass._RaoDong.ByteTable[datepos] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._RaoDong.ByteTable[datepos] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;
                        }
                        
                        datepos++;
                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        LiShiTable.Rows.Add(dr);
                        pagenum++;
                        if (datepos >= PublicDataClass._RaoDong.num / 3)
                        {

                            break;

                        }
                    }
                    if (First_Frame == true)  //首帧
                    {
                        First_Frame = false;

                    }

                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);


                    CaligrationPageNum(pagenum);


                    if (datepos >= (PublicDataClass._RaoDong.num / 3))
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();
                    }
                    Display(PageTable);
                    
                }
                else if ((PublicDataClass.ParamInfoAddr >= 0x5400) && (PublicDataClass.ParamInfoAddr <= 0x54ff))  //索引记录
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @""; dr = PageTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", j);
                        //if (j == 0)
                        //    dr["名称"] = "开关变位记录数目";
                        //else if (j == 1)
                        //    dr["名称"] = "SOE变位记录数目";
                        //else if (j == 2)
                        //    dr["名称"] = "遥控记录数目";
                        //else if (j == 3)
                        //    dr["名称"] = "电池遥控记录数目";
                        //else if (j == 4)
                        //    dr["名称"] = "电池遥信记录数目";
                        //else
                        //    dr["名称"] = "停电记录数目";

                        if (j == 0)
                            dr["名称"] = "遥信索引号";
                        else if (j == 1)
                            dr["名称"] = "遥控索引号";
                        else if (j == 2)
                            dr["名称"] = "遥信开始位值";
                        else if (j == 3)
                            dr["名称"] = "遥信结束位置";
                        else if (j == 4)
                            dr["名称"] = "遥控开始位置";
                        else
                            dr["名称"] = "遥控结束位置";

                        str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]); pos += 1;
                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        PageTable.Rows.Add(dr);
                        pagenum++;
                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    CaligrationPageNum(pagenum);
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);
                }

                else if ((PublicDataClass.ParamInfoAddr >= 0xa001) && (PublicDataClass.ParamInfoAddr <= 0xafff))  //遥信配置
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    //pagenum = 0;
                    //PageTable.Clear();
                    if (datepos >= PublicDataClass._YxLineCfgParam.num/3)
                    {

                        return;

                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {

                        str = @"";
                        dr = LiShiTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", datepos);
                        dr["名称"] = PublicDataClass._YxLineCfgParam.NameTable[datepos];



                        if (PublicDataClass._YxLineCfgParam.ByteTable[datepos] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._YxLineCfgParam.ByteTable[datepos] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }

                        datepos++;
                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        LiShiTable.Rows.Add(dr);
                        pagenum++;

                        if (datepos >= PublicDataClass._YxLineCfgParam.num/3)
                        {

                            break;

                        }

                    }
                    if (First_Frame == true)  //首帧
                    {
                        First_Frame = false;

                    }

                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);


                    CaligrationPageNum(pagenum);


                    if (datepos >= (PublicDataClass._YxLineCfgParam.num / 3))
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();
                    }
                    Display(PageTable);

                }

                else if ((PublicDataClass.ParamInfoAddr >= 0xb001) && (PublicDataClass.ParamInfoAddr <= 0xbfff))  //遥测配置
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    //pagenum = 0;
                    //PageTable.Clear();

                    if (datepos >= PublicDataClass._YcLineCfgParam.num / 3)
                    {

                        return;

                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        str = @"";

                        dr = LiShiTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", datepos);
                        dr["名称"] = PublicDataClass._YcLineCfgParam.NameTable[datepos];

                        if (PublicDataClass._YcLineCfgParam.ByteTable[datepos] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._YcLineCfgParam.ByteTable[datepos] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }
                        datepos++;
                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        LiShiTable.Rows.Add(dr);
                        pagenum++;
                        if (datepos >= PublicDataClass._YcLineCfgParam.num / 3)
                        {

                            break;

                        }

                    }
                    if (First_Frame == true)  //首帧
                    {
                        First_Frame = false;

                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    PageTable = LiShiTable.Copy();
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   //
                    Pagefm.SetDataSet(PaginationSet, out PageTable);


                    CaligrationPageNum(pagenum);

                    if (datepos >= (PublicDataClass._YcLineCfgParam.num / 3))
                    {
                        First_Frame = true;
                        //datepos = 0;
                        pagenum = 0;
                        SaveTable = LiShiTable.Copy();
                    }
                    Display(PageTable);

                }
                else if ((PublicDataClass.ParamInfoAddr >= 0x5200) && (PublicDataClass.ParamInfoAddr <= 0x52ff))  //串口参数
                {
                    byte ch = 0;
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    for (int k = 0; k < PublicDataClass._DataField.FieldVSQ; k++)
                    {
                        dr = PageTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", ch);
                        dr["名称"] = String.Format("串口{0:d}的信息", k);
                        dr["数值"] = "";
                        dr["单位"] = "";
                        dr["倍率"] = "";
                        PageTable.Rows.Add(dr);
                        pagenum++;

                        for (int j = 0; j < 3; j++)
                        {
                            ch++;
                            dr = PageTable.NewRow();
                            dr["序号"] = String.Format("{0:d}", ch);
                            str = @"";

                            if (j == 0)
                            {

                                dr["名称"] = "波特率:";
                                switch (PublicDataClass._DataField.Buffer[k * 3])
                                {
                                    case 0:
                                        str = "300";
                                        break;
                                    case 1:
                                        str = "600";
                                        break;
                                    case 2:
                                        str = "1200";
                                        break;
                                    case 3:
                                        str = "2400";
                                        break;
                                    case 4:
                                        str = "4800";
                                        break;
                                    case 5:
                                        str = "9600";
                                        break;
                                    case 6:
                                        str = "19200";
                                        break;
                                    case 7:
                                        str = "57600";
                                        break;
                                    case 8:
                                        str = "115200";
                                        break;
                                }
                                dr["数值"] = str;
                            }
                            else if (j == 1)
                            {


                                dr["名称"] = "校验:";
                                switch (PublicDataClass._DataField.Buffer[k * 3 + 1])
                                {
                                    case 0:
                                        str = "奇";
                                        break;
                                    case 1:
                                        str = "偶";
                                        break;
                                    case 2:
                                        str = "无";
                                        break;
                                }
                                dr["数值"] = str;
                            }
                            else if (j == 2)
                            {
                                dr["名称"] = "数据位:";
                                switch (PublicDataClass._DataField.Buffer[k * 3 + 2])
                                {
                                    case 0:
                                        str = "5";
                                        break;
                                    case 1:
                                        str = "6";
                                        break;
                                    case 2:
                                        str = "7";
                                        break;
                                    case 3:
                                        str = "8";
                                        break;
                                }
                                dr["数值"] = str;
                            }

                            dr["单位"] = "无";
                            dr["倍率"] = "无";
                            PageTable.Rows.Add(dr);
                            pagenum++;
                        }

                    }
                    CaligrationPageNum(pagenum);
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);
                }
                else if (PublicDataClass.ParamInfoAddr == 0xF101)   //软压板参数
                {
                    int s;
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    for (s = 0; s < PublicDataClass.FILENAME.Length; s++)
                    {
                        if (PublicDataClass.TabCfg[s].PageName == "软压板")
                            break;
                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {

                        str = @"";
                        dr = PageTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", j);


                        //dr["名称"] = PublicDataClass._FuncConfigParam.NameTable[j];
                        dr["名称"] = PublicDataClass.TabCfg[s].TabPageValue[1].ValueTable[j];


                        if (PublicDataClass.TabCfg[s].TabPageValue[3].ValueTable[j] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass.TabCfg[s].TabPageValue[3].ValueTable[j] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }

                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        PageTable.Rows.Add(dr);
                        pagenum++;

                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    CaligrationPageNum(pagenum);
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);

                }
                else if ((PublicDataClass.ParamInfoAddr >= 0xf201) && (PublicDataClass.ParamInfoAddr <= 0xf2ff))  //遥控逻辑配置参数
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {

                        str = @"";
                        dr = PageTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", j);
                        dr["名称"] = PublicDataClass._YkLogicInfo.NameTable[j];



                        if (PublicDataClass._YkLogicInfo.ByteTable[j] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._YkLogicInfo.ByteTable[j] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }

                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        PageTable.Rows.Add(dr);
                        pagenum++;

                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    CaligrationPageNum(pagenum);
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);

                }
                else if (PublicDataClass.ParamInfoAddr == 0x5900)  //反时限保护配置参数
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }

                    str = @"";
                    dr = PageTable.NewRow();
                    dr["序号"] = String.Format("{0:d}", 0);
                    dr["名称"] = "额定电流值";


                    byte[] bytes = new byte[4];
                    bytes[0] = PublicDataClass._DataField.Buffer[pos];
                    bytes[1] = PublicDataClass._DataField.Buffer[pos + 1];
                    bytes[2] = PublicDataClass._DataField.Buffer[pos + 2];
                    bytes[3] = PublicDataClass._DataField.Buffer[pos + 3];

                    float fdata = BitConverter.ToSingle(bytes, 0);

                    str += String.Format("{0:f4}", fdata);

                    pos += 4;


                    dr["数值"] = str;
                    dr["单位"] = "无";
                    dr["倍率"] = "无";
                    PageTable.Rows.Add(dr);
                    pagenum++;


                    str = @"";
                    dr = PageTable.NewRow();
                    dr["序号"] = String.Format("{0:d}", 1);
                    dr["名称"] = "保护方式";
                    if (PublicDataClass._DataField.Buffer[pos] == 0)
                        str = "不启用";
                    else if (PublicDataClass._DataField.Buffer[pos] == 1)
                        str = "一般";
                    else if (PublicDataClass._DataField.Buffer[pos] == 2)
                        str = "正常";
                    else if (PublicDataClass._DataField.Buffer[pos] == 3)
                        str = "极端";


                    dr["数值"] = str;
                    dr["单位"] = "无";
                    dr["倍率"] = "无";
                    PageTable.Rows.Add(dr);
                    pagenum++;


                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    CaligrationPageNum(pagenum);
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);

                }
                else if ((PublicDataClass.ParamInfoAddr >= 0xf301) && (PublicDataClass.ParamInfoAddr <= 0xffff))  //遥信取反
                {
                    if (datepos == 0)
                    {
                        pagenum = 0;
                        if (PageTable != null)
                            PageTable.Clear();
                        if (LiShiTable != null)
                            LiShiTable.Clear();
                        if (SaveTable != null)
                            SaveTable.Clear();
                    }
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {

                        str = @"";
                        dr = PageTable.NewRow();
                        dr["序号"] = String.Format("{0:d}", j);
                        dr["名称"] = PublicDataClass.ThreeYNameTable.YxTable[j];


                        if (PublicDataClass._YxDataQFParam.ByteTable[j] == "2")
                        {
                            str += String.Format("{0:d}", (PublicDataClass._DataField.Buffer[pos + 1] << 8) + PublicDataClass._DataField.Buffer[pos]);
                            pos += 2;
                        }
                        if (PublicDataClass._YxDataQFParam.ByteTable[j] == "1")
                        {
                            str += String.Format("{0:d}", PublicDataClass._DataField.Buffer[pos]);
                            pos += 1;

                        }

                        dr["数值"] = str;
                        dr["单位"] = "无";
                        dr["倍率"] = "无";
                        PageTable.Rows.Add(dr);
                        pagenum++;

                    }
                    if (PageTable != null)
                    {
                        if (PageTable.DataSet != null)
                        {
                            PageTable.DataSet.Tables.Remove(PageTable);
                        }
                    }
                    CaligrationPageNum(pagenum);
                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);   // 
                    Pagefm.SetDataSet(PaginationSet, out PageTable);
                    Display(PageTable);

                }
            }
            catch
            { }

        }
        #endregion
        /// <summary>
        /// 清空--按钮的事件响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonclear_Click(object sender, EventArgs e)
        {
            try
            {
                //listView1.Items.Clear();
                if (PageTable != null)
                    PageTable.Clear();
                Display(PageTable);
            }
            catch { }

        }
        /// <summary>
        /// 首页按钮--菜单的选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FistPage_Click(object sender, EventArgs e)
        {
            pagenow = 1;
            nowpage.Text = Convert.ToString(pagenow);
            CheckPageState();


            PageTable = null;//清空数据表中原有内容
            Pagefm.GoToFirstPage(out PageTable);//跳转到首页
            Display(PageTable);//显示数据
        }
        /// <summary>
        /// 上一页按钮-的响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousPage_Click(object sender, EventArgs e)
        {
            pagenow--;
            nowpage.Text = Convert.ToString(pagenow);

            CheckPageState();

            PageTable = null;//清空数据表中原有内容
            Pagefm.GoToPreviousPage(out PageTable);//跳转到上一页
            Display(PageTable);//显示数据
        }
        /// <summary>
        /// 下一页按钮--的响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextPage_Click(object sender, EventArgs e)
        {
            pagenow++;
            nowpage.Text = Convert.ToString(pagenow);

            CheckPageState();

            PageTable = null;//清空数据表中原有内容
            Pagefm.GoToNextPage(out PageTable);//跳转到下一列
            Display(PageTable);//显示数据
        }
        /// <summary>
        /// 最后一页按钮的--响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastPage_Click(object sender, EventArgs e)
        {
            pagenow = pagetotal;
            nowpage.Text = Convert.ToString(pagenow);
            CheckPageState();

            PageTable = null;//清空数据表中原有内容
            Pagefm.GoToLastPage(out PageTable);//跳转到尾页
            Display(PageTable);//显示数据
        }
        #region Custom Functions

        /// <summary>
        /// 自定义--显示函数
        /// </summary>
        /// <param name="PageTable"></param>
        private void Display(DataTable PageTable)
        {

            if (PageTable != null)//当数据表中存在记录时
            {
                ListData.Rows.Clear();
                object[] item = new object[PageTable.Columns.Count];//定义一个object类型的数组
                for (int i = 0; i < PageTable.Rows.Count; i++)//循环遍历数据表中的每一行数据
                {
                    for (int j = 0; j < PageTable.Columns.Count; j++)//循环遍历数据表中每一列数据
                    {
                        item[j] = PageTable.Rows[i][j];//保存数据表中的数据内容
                    }
                    ListData.Rows.Add(item);//向DataGridView中添加数据


                }
            }
        }

        private void CaligrationPageNum(int num)   //计算页数目
        {
            if (num < Pagefm.ItemsPerPage)
            {

                totalpage.Text = "1";
                nowpage.Text = "1";
                FistPage.Enabled = false;
                PreviousPage.Enabled = false;
                NextPage.Enabled = false;
                LastPage.Enabled = false;
            }
            else
            {
                FistPage.Enabled = false;
                PreviousPage.Enabled = false;
                NextPage.Enabled = true;
                LastPage.Enabled = true;
                nowpage.Text = "1";

                if ((num % Pagefm.ItemsPerPage) == 0)
                    pagetotal = num / Pagefm.ItemsPerPage;
                else
                    pagetotal = num / Pagefm.ItemsPerPage + 1;
                totalpage.Text = Convert.ToString(pagetotal);
            }

        }
        private void CheckPageState()   //判断页的状态
        {
            if (pagenow == pagetotal)   //当前页 ==总页数
            {
                LastPage.Enabled = false; NextPage.Enabled = false; PreviousPage.Enabled = true; FistPage.Enabled = true;
            }
            else if (pagenow == 1)  //当前页 ==首页
            {
                LastPage.Enabled = true; NextPage.Enabled = true; PreviousPage.Enabled = false; FistPage.Enabled = false;
            }
            else
            {

                LastPage.Enabled = true; NextPage.Enabled = true; PreviousPage.Enabled = true; FistPage.Enabled = true;
            }
        }
        #endregion
        /// <summary>
        /// 窗体大小 ---改变的响应函数
        /// </summary>
        static int Rows = 0;
        private void CallDataDocmentView_SizeChanged(object sender, EventArgs e)
        {
            if (First_Start == false)
            {
                return;
            }

            ListData.Columns[1].Width = (splitContainer2.Panel1.Width - ListData.Columns[0].Width - ListData.Columns[3].Width - ListData.Columns[4].Width) / 2;
            ListData.Columns[2].Width = (splitContainer2.Panel1.Width - ListData.Columns[0].Width - ListData.Columns[3].Width - ListData.Columns[4].Width) / 2;
            Pagefm.ItemsPerPage = (splitContainer2.Panel1.Height / ListData.RowTemplate.Height) - 2;

            if (Rows != Pagefm.ItemsPerPage)
            {
                /*if(pagenum ==0)
                   PageTable.Clear();
                for(int i=pagenum;i<Pagefm.ItemsPerPage;i++)
                {
                    dr = PageTable.NewRow();
                    dr["序号"] = "";
                    dr["名称"] = "";
                    dr["数值"] = "";
                    dr["单位"] = "";
                    dr["倍率"] = "";
                    PageTable.Rows.Add(dr);
                }


                Display(PageTable);*/
                Rows = Pagefm.ItemsPerPage;
            }

        }

        private void buttonseach_Click(object sender, EventArgs e)
        {
            if (seachpage.Text == "")
            {
                MessageBox.Show("内容为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int pageseach = Convert.ToInt16(seachpage.Text);
            if (pageseach <= 0 || pageseach > pagetotal)
            {
                MessageBox.Show("查询的页号非法", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (pageseach == pagenow)  //首页
            {
                pagenow = 1;
                nowpage.Text = Convert.ToString(pagenow);
                CheckPageState();

                PageTable = null;//清空数据表中原有内容
                Pagefm.GoToFirstPage(out PageTable);//跳转到首页
                Display(PageTable);//显示数据
            }
            else if (pageseach == pagetotal)  //未页
            {
                pagenow = pagetotal;
                nowpage.Text = Convert.ToString(pagenow);
                CheckPageState();

                PageTable = null;//清空数据表中原有内容
                Pagefm.GoToLastPage(out PageTable);//跳转到尾页
                Display(PageTable);//显示数据
            }
            else
            {
                pagenow = pageseach;
                nowpage.Text = Convert.ToString(pagenow);
                CheckPageState();
                PageTable = null;//清空数据表中原有内容
                Pagefm.GoToSeachPage(pageseach, out PageTable);//跳转到尾页
                Display(PageTable);//显示数据
            }
        }
        /// <summary>
        /// 导出-按钮的消息响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void output_Click(object sender, EventArgs e)
        {
            if (ListData.RowCount > 0)
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = "*.txt|*.*";
                saveret = savefile.ShowDialog() == DialogResult.OK;

                if (saveret)
                {
                    savename = savefile.FileName;
                    output.Text = "导出中..";
                    savefile.Dispose();
                }
            }
            else
            {
                MessageBox.Show("无可用的数据导出", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #region  ExportToText
        /// <summary>
        ///  导出--excel
        /// </summary>

        private void ExportToText(string path)
        {
            string s = "";
            int len = 0;
            string temp = "";
            try
            {
                //PageTable = LiShiTable.Copy;
                for (int i = 0; i < LiShiTable.Columns.Count; i++)
                {
                    temp = LiShiTable.Columns[i].ColumnName; 
                    len = 20 - Encoding.Default.GetByteCount(temp) + temp.Length; //考虑中英文的情况                  
                    temp = temp.PadRight( len, ' ');                    
                    s+= temp; 

                    //s += LiShiTable.Columns[i].ColumnName + "      ";
                }
                s += "\r\n";
                for (int i = 0; i < LiShiTable.Rows.Count; i++)
                {
                    for (int j = 0; j < LiShiTable.Columns.Count; j++)
                    {
                        temp = LiShiTable.Rows[i][j].ToString();
                        len = 20 - Encoding.Default.GetByteCount(temp) + temp.Length; //考虑中英文的情况                  
                        temp = temp.PadRight(len, ' ');
                        s += temp; 

                        //s += LiShiTable.Rows[i][j].ToString() + "      ";
                    }
                    s += "\r\n";
                }
                FileStream afile;
                StreamWriter sw;
                path += ".txt";
                afile = new FileStream(path, FileMode.Create);
                sw = new StreamWriter(afile);
                sw.Write(s);

                sw.Close();
                afile.Close();
                MessageBox.Show("数据导出成功！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("数据导出失败！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        private void seachpage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键
            {
                if ((e.KeyChar < '0') || (e.KeyChar > '9'))//这是允许输入0-9数字
                {

                    MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Handled = true;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                //  domainUpDownb.Enabled = true;
                delaytime = Convert.ToInt16(domainUpDownb.Text);
                timer2.Enabled = true;

            }
            else
            {
                //   domainUpDownb.Enabled = false;
                timer2.Enabled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._WindowsVisable.CallDataUpdatVisable == true)  //窗体可见
            {
                delaytime--;
                if (delaytime == 0)
                {

                    if (PublicDataClass.LinSPointName == "无信息")
                    {
                        MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        ty = PublicFunction.CheckPointOfCommunicationEntrace(comboBoxPoint.Text);
                        if (ty == 0)
                        {
                            MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        }

                    }
                    if (comboBoxData.SelectedIndex == 0)    //F0_遥测
                    {
                        datepos = 0;

                        //listView1.Items.Clear();
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.AloneCallYc_1 = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.AloneCallYc_1 = true;
                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.AloneCallYc_1 = true;
                        if (ty == 4)
                            PublicDataClass._UsbTaskFlag.AloneCallYc_1 = true;

                    }

                    else if (comboBoxData.SelectedIndex == 1)  //F1__遥信
                    {
                        datepos = 0;
                        //listView1.Items.Clear();
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.AloneCallYx_1 = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.AloneCallYx_1 = true;

                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.AloneCallYx_1 = true;

                        if (ty == 4)
                            PublicDataClass._UsbTaskFlag.AloneCallYx_1 = true;
                    }
                    else if (comboBoxData.SelectedIndex == 2)          //版本号
                    {
                        //listView1.Items.Clear();
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.VERSION_1 = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.VERSION_1 = true;
                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.VERSION_1 = true;

                        if (ty == 4)
                            PublicDataClass._UsbTaskFlag.VERSION_1 = true;

                    }
                    else if (comboBoxData.SelectedIndex == 3)        //时间
                    {
                        //listView1.Items.Clear();
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.CALLTIME_1 = true;

                        else if (ty == 2)
                            PublicDataClass._NetTaskFlag.CALLTIME_1 = true;
                        else if (ty == 3)
                            PublicDataClass._GprsTaskFlag.CALLTIME_1 = true;
                        else if (ty == 4)
                            PublicDataClass._UsbTaskFlag.CALLTIME_1 = true;

                    }

                    else if (comboBoxData.SelectedIndex == 4)        //硬件状态读取   zxl
                    {
                        datepos = 0;
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.READ_Hard_1 = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.READ_Hard_1 = true;
                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.READ_Hard_1 = true;
                        if (ty == 4)
                            PublicDataClass._UsbTaskFlag.READ_Hard_1 = true;


                    }
                    
                    delaytime = Convert.ToInt16(domainUpDownb.Text);

                }


            }

        }




        private void comboBoxselete_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (comboBoxselete.SelectedIndex == 1)
                {

                    ShowType = 1;   //选择类型  0：按线路   1：按序号
                    if (SaveTable.Rows.Count == 0)
                        return;

                    PageTable.Clear();
                    CaligrationPageNum(pagenumIndex);
                    pagenumLine = pagenumIndex;
                    PageTable = LiShiTable.Copy();
                    if (PageTable.DataSet != null)
                    {
                        PageTable.DataSet.Tables.Remove(PageTable);
                    }

                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);
                    Pagefm.SetDataSet(PaginationSet, out PageTable);


                    Display(PageTable);


                }

                else if (comboBoxselete.SelectedIndex == 0)
                {
                    ShowType = 0;   //选择类型  0：按线路   1：按序号
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    byte po = FindCfgInfo();
                    if (SaveTable.Rows.Count == 0)
                        return;
                    object[] item = new object[SaveTable.Columns.Count];//定义一个object类型的数组
                    PageTable.Clear();
                    CaligrationPageNum(pagenumIndex);
                    pagenumLine = pagenumIndex;



                    for (int i = 0; i < pagenumIndex; i++)//循环遍历数据表中的每一行数据
                    {
                        for (int j = 0; j < pagenumIndex; j++)
                        {
                            //     if (Convert.ToInt16(PublicDataClass._YcInformationParam.IndexTable[j]) == i)
                            if (Convert.ToInt16(PublicDataClass.SaveText.Cfg[po].YccfgIndex[j]) == i)
                            {
                                for (int m = 0; m < SaveTable.Columns.Count; m++)//循环遍历数据表中每一列数据
                                {
                                    item[m] = SaveTable.Rows[j][m];

                                }
                                PageTable.Rows.Add(item);
                            }
                        }
                    }


                    if (PageTable.DataSet != null)
                    {
                        PageTable.DataSet.Tables.Remove(PageTable);
                    }

                    PaginationSet.Tables.Clear();
                    PaginationSet.Tables.Add(PageTable);
                    Pagefm.SetDataSet(PaginationSet, out PageTable);

                    Display(PageTable);
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    /*   int i;
                       int j = 0;
                       if (SaveTable.Rows.Count == 0)
                           return;
                       object[] item = new object[SaveTable.Columns.Count];//定义一个object类型的数组
                       for (i = 0; i < 12; i++)//循环遍历数据表中的每一行数据
                       {
                           if (i < 8)
                           {

                               for (j = 0; j < SaveTable.Columns.Count; j++)//循环遍历数据表中每一列数据
                               {
                                   item[j] = SaveTable.Rows[i][j];

                               }
                               YcDataTable.Rows.Add(item);//向DataGridView中添加数据  
                           }
                           else
                           {

                               if (i == 8)
                                   j = 113;
                               else if (i == 9)
                                   j = 114;
                               else if (i == 10)
                                   j = 115;
                               else if (i == 11)
                                   j = 116;

                               for (int k = 0; k < SaveTable.Columns.Count; k++)
                                   item[k] = SaveTable.Rows[j][k];
                               YcDataTable.Rows.Add(item);//向DataGridView中添加数据

                           }
                       }
                       //电流
                       for (int k = 0; k < 15; k++)
                       {
                           for (i = (8 + k); i <= (98 + k); i += 15)
                           {
                               for (j = 0; j < SaveTable.Columns.Count; j++)//循环遍历数据表中每一列数据
                               {
                                   item[j] = SaveTable.Rows[i][j];

                               }
                               YcDataTable.Rows.Add(item);//向DataGridView中添加数据
                           }
                           for (i = (117 + k); i <= (387 + k); i += 15)
                           {
                               for (j = 0; j < SaveTable.Columns.Count; j++)//循环遍历数据表中每一列数据
                               {
                                   item[j] = SaveTable.Rows[i][j];

                               }
                               YcDataTable.Rows.Add(item);//向DataGridView中添加数据
                           }
                       }
                       for (j = 0; j < SaveTable.Columns.Count; j++)//循环遍历数据表中每一列数据
                       {
                           item[j] = SaveTable.Rows[SaveTable.Rows.Count - 2][j];

                       }
                       YcDataTable.Rows.Add(item);//向DataGridView中添加数据
                       for (j = 0; j < SaveTable.Columns.Count; j++)//循环遍历数据表中每一列数据
                       {
                           item[j] = SaveTable.Rows[SaveTable.Rows.Count - 1][j];

                       }
                       YcDataTable.Rows.Add(item);//向DataGridView中添加数据

                    //   PageTable = null;//清空数据表中原有内容
                       PageTable.Clear();
                       CaligrationPageNum(pagenumLine);
                       pagenumIndex = pagenumLine;
                       pagenumLine = 0;

                       PaginationSet.Tables.Clear();
                       PageTable = YcDataTable.Copy();
                       PaginationSet.Tables.Add(PageTable);   // 
                       Pagefm.SetDataSet(PaginationSet, out PageTable);

                       Display(PageTable);*/
                }
            }
            catch 
            { }
        }

        private void comboBoxData_SelectedIndexChanged(object sender, EventArgs e)
        {
                 if (comboBoxData.SelectedIndex == 0)    //F0_遥测
                     comboBoxselete.Enabled = true;
                 else comboBoxselete.Enabled = false;

        }

       
       
    }
}
