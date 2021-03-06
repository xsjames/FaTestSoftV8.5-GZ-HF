﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaTestSoft.CommonData;
using FaTestSoft.FUNCTIONCLASS;

namespace FaTestSoft
{
    public partial class YkCmdViewForm : Form
    {
        public YkCmdViewForm()
        {
            InitializeComponent();
        }

        Random ran = new Random();

        private int ty;
        static byte TriggeredType = 0;
        static byte YkCmdType     = 0;
        int YkStartPos = 0;
        int Data_L=0, Data_H = 0;
        byte rand1 = 0,rand2 = 0;

        private void YkCmdViewForm_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            //for (int i = 0; i < PublicDataClass._YkConfigParam.num; i++)
            //{
            //    ListViewItem lv = new ListViewItem(String.Format("{0:d}", i));
            //    lv.SubItems.Add(PublicDataClass._YkConfigParam.NameTable[i]);
            //    lv.SubItems.Add(PublicDataClass._YkConfigParam.AddrTable[i]);
            //    PublicDataClass.YkStartPos = Convert.ToInt32(PublicDataClass._YkConfigParam.AddrTable[i]);

            //    lv.SubItems.Add(String.Format("{0:x}", PublicDataClass.YkStartPos));
            //    lv.SubItems.Add("未知");
            //    listView1.Items.Add(lv);
            //}
            for (int i = 0; i < PublicDataClass.SaveText.Cfg[0].YkcfgNum; i++)
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", i));
                lv.SubItems.Add(PublicDataClass.SaveText.Cfg[0].YkcfgName[i]);
                lv.SubItems.Add(PublicDataClass.SaveText.Cfg[0].YkcfgAddr[i]);
                PublicDataClass.YkStartPos = Convert.ToInt32(PublicDataClass.SaveText.Cfg[0].YkcfgAddr[i]);

                lv.SubItems.Add(String.Format("{0:x}", PublicDataClass.YkStartPos));
                lv.SubItems.Add("未知");
                listView1.Items.Add(lv);
            }
             
            timer1.Enabled = true;
            TriggeredType  = 2;            //默认的是脉冲方式
            rdpulse.Checked = true;

            checkBoxDco.Checked = false;
            checkBoxSco.Checked = true;
            YkCmdType = 1;                   //默认的单点遥控
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            
                //this.listView1.ForeColor = SystemColors.WindowText;
                foreach (ListViewItem item in this.listView1.Items)
                {
                    item.ForeColor = SystemColors.WindowText;
     
                }
                this.listView1.SelectedItems[0].ForeColor = Color.Red;//设置当前选择项为红色

                try
                {
           
                    //2013.10.10上海K型站修改，遥控功能全部开放
                            YkStartPos = Convert.ToInt16(this.listView1.SelectedItems[0].SubItems[2].Text);
                        /*           if (YkStartPos >= 24595 && YkStartPos <= 24597)
                                    buttonselfen.Enabled = false;
                                else
                                    buttonselfen.Enabled = true;*/ 
                }
                catch { }
                 
        }
        /********************************************************************************************
        *  函数名：    buttonselfen_Click                                                           *
        *  功能  ：    遥控选择 分处理                                                              *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2011-3-10                                                                    *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void buttonselfen_Click(object sender, EventArgs e)
        {
            try
            {
                if (PublicDataClass.LinSPointName == "无信息")
                {
                    MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                else
                {
                    ty = PublicFunction.CheckPointOfCommunicationEntrace(PublicDataClass.LinSPointName);
                    if (ty == 0)
                    {
                        MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }
                this.listView1.SelectedItems[0].SubItems[4].Text = "选择分等待中...";
                PublicDataClass.YkState = 1;   //分
                PublicDataClass._DataField.FieldLen = 0;

                if (YkCmdType == 2)  //双点
                {
                    if (TriggeredType == 1)
                    {
                        PublicDataClass.YkStartPos = YkStartPos;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x81;
                        PublicDataClass._DataField.FieldLen++;

                    }
                    else
                    {
                        Data_H = YkStartPos & 0xff00;
                        Data_L = YkStartPos & 0x00ff;
                        //PublicDataClass.YkStartPos = Data_H + (Data_L * 2);
                        PublicDataClass.YkStartPos = YkStartPos;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x81;
                        PublicDataClass._DataField.FieldLen++;

                    }


                    if (ty == 1)
                    {
                        //rand1 = Convert.ToByte(ran.Next(1, 255));
                        //rand2 = Convert.ToByte(ran.Next(1, 255));

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                        //PublicDataClass._DataField.FieldLen++;

                        PublicDataClass._ComTaskFlag.YK_Sel_1 = true;
                    }
                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.YK_Sel_1 = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.YK_Sel_1 = true;
                }
                else if (YkCmdType == 1)  //单点
                {

                    if (TriggeredType == 1)
                    {
                        PublicDataClass.YkStartPos = YkStartPos;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x80;
                        PublicDataClass._DataField.FieldLen++;

                    }
                    else
                    {
                        Data_H = YkStartPos & 0xff00;
                        Data_L = YkStartPos & 0x00ff;
                        //PublicDataClass.YkStartPos = Data_H + (Data_L * 2);
                        PublicDataClass.YkStartPos = YkStartPos;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x80;
                        PublicDataClass._DataField.FieldLen++;

                    }


                    if (ty == 1)
                    {
                        //rand1 = Convert.ToByte(ran.Next(1, 255));
                        //rand2 = Convert.ToByte(ran.Next(1, 255));

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                        //PublicDataClass._DataField.FieldLen++;

                        PublicDataClass._ComTaskFlag.YK_Sel_1_D = true;
                    }
                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.YK_Sel_1_D = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.YK_Sel_1_D = true;



                }
            }
            catch (ArgumentOutOfRangeException ee)
            {
                MessageBox.Show("请选择遥控路数", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            { 
            }
        }
        /********************************************************************************************
        *  函数名：    buttonselhe_Click                                                            *
        *  功能  ：    遥控选择 合处理                                                              *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2011-3-10                                                                    *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void buttonselhe_Click(object sender, EventArgs e)
        {
            try
            {
            if (PublicDataClass.LinSPointName == "无信息")
            {
                MessageBox.Show("无测量点信息可操作", "信息",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            else
            {
                ty = PublicFunction.CheckPointOfCommunicationEntrace(PublicDataClass.LinSPointName);
                if (ty == 0)
                {
                    MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }

            }
            this.listView1.SelectedItems[0].SubItems[4].Text = "选择合等待中...";
            PublicDataClass.YkState = 2;   //合
            PublicDataClass._DataField.FieldLen = 0;
            if (YkCmdType == 2)
            {
                if (TriggeredType == 1)
                {
                    PublicDataClass.YkStartPos = YkStartPos;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x82;
                    PublicDataClass._DataField.FieldLen++;
                }
                else        //脉冲式
                {
                    Data_H = YkStartPos & 0xff00;
                    Data_L = YkStartPos & 0x00ff;
                    //PublicDataClass.YkStartPos = Data_H + (Data_L * 2 -1);
                    PublicDataClass.YkStartPos = YkStartPos;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x82;
                    PublicDataClass._DataField.FieldLen++;

                }
                if (ty == 1)
                {

                    //rand1 = Convert.ToByte(ran.Next(1, 255));
                    //rand2 = Convert.ToByte(ran.Next(1, 255));

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                    //PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._ComTaskFlag.YK_Sel_1 = true;
                }
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.YK_Sel_1 = true;
                if (ty == 3)
                {
                    //rand1 = Convert.ToByte(ran.Next(1, 255));
                    //rand2 = Convert.ToByte(ran.Next(1, 255));

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                    //PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._GprsTaskFlag.YK_Sel_1 = true;
                }
            }
            else if (YkCmdType == 1) //单点
            {
                if (TriggeredType == 1)
                {
                    PublicDataClass.YkStartPos = YkStartPos;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x81;
                    PublicDataClass._DataField.FieldLen++;
                }
                else        //脉冲式
                {
                    Data_H = YkStartPos & 0xff00;
                    Data_L = YkStartPos & 0x00ff;
                    //PublicDataClass.YkStartPos = Data_H + (Data_L * 2 -1);
                    PublicDataClass.YkStartPos = YkStartPos;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x81;
                    PublicDataClass._DataField.FieldLen++;

                }
                if (ty == 1)
                {

                    //rand1 = Convert.ToByte(ran.Next(1, 255));
                    //rand2 = Convert.ToByte(ran.Next(1, 255));

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                    //PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._ComTaskFlag.YK_Sel_1_D = true;
                }
                if (ty == 2)
                    PublicDataClass._NetTaskFlag.YK_Sel_1_D = true;
                if (ty == 3)
                {
                    //rand1 = Convert.ToByte(ran.Next(1, 255));
                    //rand2 = Convert.ToByte(ran.Next(1, 255));

                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                    //PublicDataClass._DataField.FieldLen++;
                    //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                    //PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._GprsTaskFlag.YK_Sel_1_D = true;
                }



            }
            }
            catch (ArgumentOutOfRangeException ee)
            {
                MessageBox.Show("请选择遥控路数", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            { 
            }
        }
        /********************************************************************************************
        *  函数名：    buttonexe_Click                                                              *
        *  功能  ：    遥控执行   处理                                                              *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2011-3-10                                                                    *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void buttonexe_Click(object sender, EventArgs e)
        {
            try
            {
                if (PublicDataClass.LinSPointName == "无信息")
                {
                    MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                else
                {
                    ty = PublicFunction.CheckPointOfCommunicationEntrace(PublicDataClass.LinSPointName);
                    if (ty == 0)
                    {
                        MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }
                this.listView1.SelectedItems[0].SubItems[4].Text = "执行等待中...";
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass.YkStartPos = YkStartPos;
                if (YkCmdType == 2)
                {
                   
                    if (TriggeredType == 1)
                    {
                        if (PublicDataClass.YkState == 1)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x01;
                        else if (PublicDataClass.YkState == 2)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x02;
                        PublicDataClass._DataField.FieldLen++;
                    }
                    else
                    {
                        if (PublicDataClass.YkState == 1)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x01;
                        else if (PublicDataClass.YkState == 2)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x02;
                        PublicDataClass._DataField.FieldLen++;

                    }
                    if (ty == 1)
                    {
                        //rand1 = Convert.ToByte(ran.Next(1, 255));
                        //rand2 = Convert.ToByte(ran.Next(1, 255));

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                        //PublicDataClass._DataField.FieldLen++;
                        PublicDataClass._ComTaskFlag.YK_Exe_1 = true;
                    }
                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.YK_Exe_1 = true;
                    if (ty == 3)
                    {
                        //rand1 = Convert.ToByte(ran.Next(1, 255));
                        //rand2 = Convert.ToByte(ran.Next(1, 255));

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                        //PublicDataClass._DataField.FieldLen++;
                        PublicDataClass._GprsTaskFlag.YK_Exe_1 = true;
                    }
                }
                else if (YkCmdType == 1)
                {
                    if (TriggeredType == 1)
                    {
                        if (PublicDataClass.YkState == 1)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x00;
                        else if (PublicDataClass.YkState == 2)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x01;
                        PublicDataClass._DataField.FieldLen++;
                    }
                    else
                    {
                        if (PublicDataClass.YkState == 1)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x00;
                        else if (PublicDataClass.YkState == 2)
                            PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x01;
                        PublicDataClass._DataField.FieldLen++;

                    }
                    if (ty == 1)
                    {
                        //rand1 = Convert.ToByte(ran.Next(1, 255));
                        //rand2 = Convert.ToByte(ran.Next(1, 255));

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                        //PublicDataClass._DataField.FieldLen++;
                        PublicDataClass._ComTaskFlag.YK_Exe_1_D = true;
                    }
                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.YK_Exe_1_D = true;
                    if (ty == 3)
                    {
                        //rand1 = Convert.ToByte(ran.Next(1, 255));
                        //rand2 = Convert.ToByte(ran.Next(1, 255));

                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand1;     //随机数1
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = rand2;     //随机数2
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 | rand2); //或结果
                        //PublicDataClass._DataField.FieldLen++;
                        //PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = Convert.ToByte(rand1 & rand2); //与结果
                        //PublicDataClass._DataField.FieldLen++;
                        PublicDataClass._GprsTaskFlag.YK_Exe_1_D = true;
                    }



                }
            }
            catch
            { }
        }
        /********************************************************************************************
        *  函数名：    buttoncancel_Click                                                           *
        *  功能  ：    遥控撤销   处理                                                              *
        *  参数  ：    无                                                                           *
        *  返回值：    无                                                                           *
        *  修改日期：  2011-3-10                                                                    *
        *  作者    ：  cuibj                                                                        *
        * ******************************************************************************************/
        private void buttoncancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (PublicDataClass.LinSPointName == "无信息")
                {
                    MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                else
                {
                    ty = PublicFunction.CheckPointOfCommunicationEntrace(PublicDataClass.LinSPointName);
                    if (ty == 0)
                    {
                        MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;

                    }

                }
                this.listView1.SelectedItems[0].SubItems[4].Text = "撤销等待中...";
                PublicDataClass._DataField.FieldLen = 0;
                /*if (TriggeredType == 1)
                {
                    if (PublicDataClass.YkState == 1)
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x81;
                    else if (PublicDataClass.YkState == 2)
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x82;
                }
                else
                {
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x81;
                }*/
                if (YkCmdType == 2)
                {
                    if (PublicDataClass.YkState == 1)
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x81;
                    else if (PublicDataClass.YkState == 2)
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x82;

                    PublicDataClass._DataField.FieldLen++;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.YK_Cel_1 = true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.YK_Cel_1 = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.YK_Cel_1 = true;
                }
                else if (YkCmdType == 1)
                {
                    if (PublicDataClass.YkState == 1)
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x80;
                    else if (PublicDataClass.YkState == 2)
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x81;

                    PublicDataClass._DataField.FieldLen++;
                    if (ty == 1)
                        PublicDataClass._ComTaskFlag.YK_Cel_1_D= true;

                    if (ty == 2)
                        PublicDataClass._NetTaskFlag.YK_Cel_1_D = true;
                    if (ty == 3)
                        PublicDataClass._GprsTaskFlag.YK_Cel_1_D = true;


                }
            }
            catch
            { }
        }

        private void AddMenuItem_Click(object sender, EventArgs e)
        {
            AddYkRecordForm AddykFm = new AddYkRecordForm();
            AddykFm.ShowDialog();
            if (AddykFm.DialogResult == DialogResult.OK)               //判断是否按下-确定-按钮
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.Items.Count));
                lv.SubItems.Add(PublicDataClass._AddYkRecord.Name);

                lv.SubItems.Add(PublicDataClass._AddYkRecord.Pos);
                lv.SubItems.Add(String.Format("{0:x}", Convert.ToInt16(PublicDataClass._AddYkRecord.Pos)));
                lv.SubItems.Add("未知");
                listView1.Items.Add(lv);

            }
        }

        private void DeleteMenuItem_Click(object sender, EventArgs e)
        {
            
            if (MessageBox.Show("确定要删除此项吗?", "信  息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView1);

                if (SettleOnItem.Count <= 0)
                {
                    MessageBox.Show("记录项选择为空", "信息",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (byte i = 0; i < SettleOnItem.Count; )
                {

                    listView1.Items.Remove(SettleOnItem[i]);       //删除所选择的项
                }
                for (byte i = 0; i < listView1.Items.Count; i++)
                {
                    listView1.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号

                }
                //ReNewCfgState();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._Message.YkDocmentView == true)
            {
                PublicDataClass._Message.YkDocmentView = false;
                this.listView1.SelectedItems[0].SubItems[4].Text = "确认";

            }

            if (PublicDataClass._YkConfigParam.Ykinputflag == true)
            {
                PublicDataClass._YkConfigParam.Ykinputflag = false;
                listView1.Items.Clear();
                for (int i = 0; i < PublicDataClass.SaveText.Cfg[0].YkcfgNum; i++)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", i));
                    lv.SubItems.Add(PublicDataClass.SaveText.Cfg[0].YkcfgName[i]);
                    lv.SubItems.Add(PublicDataClass.SaveText.Cfg[0].YkcfgAddr[i]);
                    PublicDataClass.YkStartPos = Convert.ToInt32(PublicDataClass.SaveText.Cfg[0].YkcfgAddr[i]);

                    lv.SubItems.Add(String.Format("{0:x}", PublicDataClass.YkStartPos));
                    lv.SubItems.Add("未知");
                    listView1.Items.Add(lv);
                }
            }
        }

        private void YkCmdViewForm_VisibleChanged(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        /// <summary>
        /// 脉冲式---按钮的消息响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdpulse_CheckedChanged(object sender, EventArgs e)
        {
            TriggeredType = 2;
            //for (byte i = 0; i < listView1.Items.Count; i++)                         //12-06-28封
            //{
            //    if(i<18 ||i>20)
            //       listView1.Items[i].SubItems[1].Text = String.Format("遥控线路{0:d}", i + 1);


            //}
                
            buttonselfen.Enabled = true;
        }
        /// <summary>
        /// 电平式---按钮的消息响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rddping_CheckedChanged(object sender, EventArgs e)
        {
            TriggeredType = 1;

            //for (byte i = 0; i < listView1.Items.Count; i++)                         //12-06-28封
            //{
            //    if (i < 18 || i > 20)
            //        listView1.Items[i].SubItems[1].Text = String.Format("继电器{0:d}", i + 1);
            //}
            buttonselfen.Enabled = false;
        }

        private void checkBoxSco_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSco.Checked == true)
            {
                YkCmdType = 1;
                checkBoxDco.Checked = false;
            }
        }

        private void checkBoxDco_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxDco.Checked ==true)
            {
                YkCmdType = 2;
                checkBoxSco.Checked = false;
            }
        }

        private void YkCmdViewForm_Activated(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            for (int i = 0; i < PublicDataClass._YkConfigParam.num; i++)
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", i));
                lv.SubItems.Add(PublicDataClass._YkConfigParam.NameTable[i]);
                lv.SubItems.Add(PublicDataClass._YkConfigParam.AddrTable[i]);
                PublicDataClass.YkStartPos = Convert.ToInt32(PublicDataClass._YkConfigParam.AddrTable[i]);

                lv.SubItems.Add(String.Format("{0:x}", PublicDataClass.YkStartPos));
                lv.SubItems.Add("未知");
                listView1.Items.Add(lv);
            }
        }
    }
}
