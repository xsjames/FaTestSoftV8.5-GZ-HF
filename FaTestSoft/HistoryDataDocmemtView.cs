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
using FaTestSoft.FUNCTIONCLASS;
using System.Text.RegularExpressions;
using System.IO;



namespace FaTestSoft
{
    public partial class HistoryDataDocmemtView : DockContent
    {
        public HistoryDataDocmemtView()
        {
            InitializeComponent();
        }
        private byte[] _HisDataField = new byte[11];

        static int FrameSeq = 0;
        public int num      = 0;
        private int ty;
        bool saveret        = false;
        string savename;
        int YCdatapos = 0;//数据个数标志

        private void HistoryDataDocmemtView_Load(object sender, EventArgs e)
        {
            dateTimePickerendtime.Enabled = false;
            textBoxfrom.Text="1";
            textBoxto.Text = "10";
            comboBoxData.SelectedIndex = 0;
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
            for (int i = 1; i < 60; i++)
                dUpDown.Items.Add(i);
            dUpDown.SelectedIndex = 0;
        }
        /// <summary>
        /// 召测 ---按钮的消息响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttoncall_Click(object sender, EventArgs e)
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


            if (Regex.IsMatch(textBoxfrom.Text, @"^\d+$", RegexOptions.Singleline) == false)
            {
                MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Regex.IsMatch(textBoxto.Text, @"^\d+$", RegexOptions.Singleline) == false)
            {
                MessageBox.Show("只能输入数字！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((Convert.ToInt16(textBoxfrom.Text) < 1) || (Convert.ToInt16(textBoxfrom.Text) > 255))//这是允许输入0-9数字
            {

                MessageBox.Show("范围错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((Convert.ToInt16(textBoxto.Text) <0) || (Convert.ToInt16(textBoxto.Text) > 65535))//这是允许输入0-9数字
            {

                MessageBox.Show("范围错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt16(textBoxfrom.Text) > Convert.ToInt16(textBoxto.Text))
            {
                MessageBox.Show("范围错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dateTimePickerbegindata.Enabled == false)
            {
                PublicDataClass._DataField.FieldLen = 0;
                PublicDataClass._DataField.FieldVSQ = 0;
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToInt16(textBoxfrom.Text)&0xff);
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)((Convert.ToInt16(textBoxfrom.Text) & 0x00ff) >> 8);
                PublicDataClass._DataField.FieldLen+=2;
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToInt16(textBoxto.Text)&0xff);
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)((Convert.ToInt16(textBoxto.Text) & 0xff00) >> 8);
                PublicDataClass._DataField.FieldLen+=2;
                //for (byte i = 0; i < 1; i++)
                //    _HisDataField[i] = PublicDataClass._DataField.Buffer[i];
                //FrameSeq = 0;
                FrameSeq = Convert.ToInt16(textBoxfrom.Text);
                PublicDataClass._DataField.FieldVSQ = 2;

            }

            else if ((dateTimePickerbegindata.Enabled == true) && (dateTimePickerenddata.Enabled == true) && (dateTimePickerbegintime.Enabled == false) && (dateTimePickerendtime.Enabled == false))
            { 
                if(dateTimePickerbegindata.Value.Year>dateTimePickerenddata.Value.Year)
                    {
                        MessageBox.Show("结束年份<开始年份", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                else if ((dateTimePickerbegindata.Value.Year==dateTimePickerenddata.Value.Year)&&(dateTimePickerbegindata.Value.Month>dateTimePickerenddata.Value.Month))
                {
                    MessageBox.Show("结束月份<开始月份", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ((dateTimePickerbegindata.Value.Year == dateTimePickerenddata.Value.Year) && (dateTimePickerbegindata.Value.Month == dateTimePickerenddata.Value.Month) && (dateTimePickerbegindata.Value.Day > dateTimePickerenddata.Value.Day))
                {
                    MessageBox.Show("结束天数<开始天数", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    PublicDataClass._DataField.FieldLen = 1;
                    PublicDataClass._DataField.FieldVSQ = 0;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerbegindata.Value.Year - 2000));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerbegindata.Value.Month));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerbegindata.Value.Day));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] =0;
                    PublicDataClass._DataField.FieldLen++;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerenddata.Value.Year - 2000));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerenddata.Value.Month));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerenddata.Value.Day));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0;
                    PublicDataClass._DataField.FieldLen++;
                  
                    for (byte i = 0; i < 11; i++)
                        _HisDataField[i] = PublicDataClass._DataField.Buffer[i];

                    PublicDataClass._DataField.FieldVSQ = 11;
                    datapos = 0;
                    pos = 0;
                }

            }
            else if ((dateTimePickerbegindata.Enabled == true) && (dateTimePickerenddata.Enabled == true) && (dateTimePickerbegintime.Enabled == true) && (dateTimePickerendtime.Enabled == true))
            {
                int begintime = (dateTimePickerbegintime.Value.Hour) * 3600 + (dateTimePickerbegintime.Value.Minute) * 60 + dateTimePickerbegintime.Value.Second;
                int endtime = (dateTimePickerendtime.Value.Hour) * 3600 + (dateTimePickerendtime.Value.Minute) * 60 + dateTimePickerendtime.Value.Second;
                if (dateTimePickerbegindata.Value.Year > dateTimePickerenddata.Value.Year)
                {
                    MessageBox.Show("结束年份<开始年份", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ((dateTimePickerbegindata.Value.Year == dateTimePickerenddata.Value.Year) && (dateTimePickerbegindata.Value.Month > dateTimePickerenddata.Value.Month))
                {
                    MessageBox.Show("结束月份<开始月份", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ((dateTimePickerbegindata.Value.Year == dateTimePickerenddata.Value.Year) && (dateTimePickerbegindata.Value.Month == dateTimePickerenddata.Value.Month) && (dateTimePickerbegindata.Value.Day > dateTimePickerenddata.Value.Day))
                {
                    MessageBox.Show("结束天数<开始天数", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if ((dateTimePickerbegindata.Value.Year == dateTimePickerenddata.Value.Year) && (dateTimePickerbegindata.Value.Month == dateTimePickerenddata.Value.Month) && (dateTimePickerbegindata.Value.Day == dateTimePickerenddata.Value.Day) && (begintime > endtime))
                {

                    MessageBox.Show("结束时间<开始时间", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    PublicDataClass._DataField.FieldLen = 1;
                    PublicDataClass._DataField.FieldVSQ = 0;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerbegindata.Value.Year - 2000));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerbegindata.Value.Month));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerbegindata.Value.Day));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerbegintime.Value.Hour));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerbegintime.Value.Minute));
                    PublicDataClass._DataField.FieldLen++;

                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerenddata.Value.Year - 2000));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerenddata.Value.Month));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerenddata.Value.Day));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerendtime.Value.Hour));
                    PublicDataClass._DataField.FieldLen++;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = PublicFunction.HextoBCD(Convert.ToByte(dateTimePickerendtime.Value.Minute));
                    PublicDataClass._DataField.FieldLen++;

                    for (byte i = 0; i <11; i++)
                        _HisDataField[i] = PublicDataClass._DataField.Buffer[i];

                    PublicDataClass._DataField.FieldVSQ = 11;
                    datapos = 0;
                    pos = 0;
                }

            }  
            

            if (comboBoxData.SelectedIndex == 0)    //H0_遥信变位记录
            {
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.CallRecordData = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.CallRecordData = true;

                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.CallRecordData = true;

                datapos = 0;

                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x8000;
            }
            else if (comboBoxData.SelectedIndex == 1)  //变位记录
            {
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.CallRecordData = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.CallRecordData = true;

                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.CallRecordData = true;

                datapos = 0;

                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass.ParamInfoAddr = 0x8002;
            }
            else if (comboBoxData.SelectedIndex == 2) //遥控操作
            {

                if (ty == 1)
                    PublicDataClass._ComTaskFlag.CallRecordData = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.CallRecordData = true;

                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.CallRecordData = true;
                datapos = 0;
                pos = 0;
                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
       
                PublicDataClass.ParamInfoAddr = 0x8001;
            }
            else if (comboBoxData.SelectedIndex == 3)//历史记录
            {
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.CallRecordData = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.CallRecordData = true;

                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.CallRecordData = true;
                datapos = 0;
                pos = 0;

                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass._DataField.Buffer[0] = 0;
                PublicDataClass.ParamInfoAddr = 0x8003;

            }
            else if (comboBoxData.SelectedIndex == 4)//统计数据-最大最小值
            {
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.CallRecordData = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.CallRecordData = true;

                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.CallRecordData = true;
                datapos = 0;
                pos = 0;

                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass._DataField.Buffer[0] = 0;
                PublicDataClass.ParamInfoAddr = 0x8004;


            }
            else if (comboBoxData.SelectedIndex == 5)//电压日合格率
            {
               
                if (ty == 1)
                    PublicDataClass._ComTaskFlag.CallRecordData = true;

                if (ty == 2)
                    PublicDataClass._NetTaskFlag.CallRecordData = true;

                if (ty == 3)
                    PublicDataClass._GprsTaskFlag.CallRecordData = true;
                datapos = 0;
                pos = 0;

                PublicDataClass.seq = 1;
                PublicDataClass.seqflag = 0;
                PublicDataClass.SQflag = 0;
                PublicDataClass._DataField.Buffer[0] = 0;
                PublicDataClass.ParamInfoAddr = 0x8005;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._WindowsVisable.HistoryDataUpdateVisable == true)  //窗体可见
            {

                if (PublicDataClass._Message.CallHisDataDocmentView == true)
                {
                    PublicDataClass._Message.CallHisDataDocmentView = false;
                    ProcessHisData();
                }
            }
            if (saveret)
            {
                saveret = false;
                ExportToText(savename);
                buttonoutput.Text = "导出";
                //MessageBox.Show("数据导出成功", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void HistoryDataDocmemtView_Deactivate(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.HistoryDataUpdateVisable = false;        //窗体不可见
            timer1.Enabled = false;
        }

        private void HistoryDataDocmemtView_Activated(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.HistoryDataUpdateVisable = true;        //窗体可见
            timer1.Enabled = true;
        }

        private void ProcessHisData()
        {
            if (comboBoxData.SelectedIndex == 0)
            {
                ShowHisRecordData(1);
            }
            else if (comboBoxData.SelectedIndex == 1)
            {
                ShowHisRecordData(2); 
            }
            else if (comboBoxData.SelectedIndex == 2)
            {
                ShowHisRecordData(3);
            }
           else if (comboBoxData.SelectedIndex == 3)
               ShowYcHisData();
           else if (comboBoxData.SelectedIndex == 4)
                ShowTjHisData();
            else if (comboBoxData.SelectedIndex == 5)
                ShowDYqualityData();
        }
        static int datapos = 0;
        static int pos = 0;
        private void ShowYcHisData()
        {      
            try
            {
                int dex = 0; string strdata = @"";
                if (PublicDataClass._DataField.FieldVSQ < 5)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", 0));
                    lv.SubItems.Add("无数据记录");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    return;
                }
                if (datapos == 0)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));

                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8));
                    strdata += "年";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 2] + (PublicDataClass._DataField.Buffer[dex + 3] << 8));
                    strdata += "月";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 4] + (PublicDataClass._DataField.Buffer[dex + 5] << 8));  //日+星期
                    strdata += "日";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 6] + (PublicDataClass._DataField.Buffer[dex + 7] << 8));
                    strdata += "时";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 8] + (PublicDataClass._DataField.Buffer[dex + 9] << 8));
                    strdata += "分";
                    lv.SubItems.Add(strdata);
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    datapos++;
                }
                dex += 10;
                for (int j = 0; j < PublicDataClass._DataField.FieldVSQ - 5; j++)
                {

                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));
                    lv.SubItems.Add(PublicDataClass._HisData.YcNameTable[pos]);

                    int data = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                    dex += 2;
                    lv.SubItems.Add(Convert.ToString(data));
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    pos++;
                    datapos++;
                }
            }
            catch
            {
                MessageBox.Show("上传报文有误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }

        private void ShowTjHisData()
        {
            try
            {
                int dex = 0; string strdata = @"";
                if (PublicDataClass._DataField.FieldVSQ < 5)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", 0));
                    lv.SubItems.Add("无数据记录");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    return;
                }
                if (datapos == 0)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));

                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8));
                    strdata += "年";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 2] + (PublicDataClass._DataField.Buffer[dex + 3] << 8));
                    strdata += "月";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 4] + (PublicDataClass._DataField.Buffer[dex + 5] << 8));  //日+星期
                    strdata += "日";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 6] + (PublicDataClass._DataField.Buffer[dex + 7] << 8));
                    strdata += "时";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 8] + (PublicDataClass._DataField.Buffer[dex + 9] << 8));
                    strdata += "分";
                    lv.SubItems.Add(strdata);
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    datapos++;
                }
                dex += 10;
                for (int j = 0; j < PublicDataClass._DataField.FieldVSQ - 5; j++)
                {

                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));
                    lv.SubItems.Add(PublicDataClass._HisData.TjNameTable[pos]);

                    int data = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                    dex += 2;
                    lv.SubItems.Add(Convert.ToString(data));
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    pos++;
                    datapos++;
                }
            }
            catch
            {
                MessageBox.Show("上传报文有误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowDYqualityData()
        {
            try
            {
                int dex = 0; string strdata = @"";
                if (PublicDataClass._DataField.FieldVSQ < 1)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", 0));
                    lv.SubItems.Add("无数据记录");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    return;
                }
                if (datapos == 0)
                {
                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));

                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8));
                    strdata += "年";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 2] + (PublicDataClass._DataField.Buffer[dex + 3] << 8));
                    strdata += "月";
                    strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 4] + (PublicDataClass._DataField.Buffer[dex + 5] << 8));  //日+星期
                    strdata += "日";

                    lv.SubItems.Add(strdata);
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    datapos++;
                }
                dex += 6;
                for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                {

                    ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));
                    lv.SubItems.Add("电压日合格率");

                    //int data = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                    byte[] bytes = new byte[4];
                    bytes[0] = PublicDataClass._DataField.Buffer[dex];
                    bytes[1] = PublicDataClass._DataField.Buffer[dex + 1];
                    bytes[2] = PublicDataClass._DataField.Buffer[dex + 2];
                    bytes[3] = PublicDataClass._DataField.Buffer[dex + 3];

                    float fdata = BitConverter.ToSingle(bytes, 0);
                    string str = "";
                    str += String.Format("{0:f4}", fdata * 100) + "%";
                    lv.SubItems.Add(str);
                    dex += 4;
                    lv.SubItems.Add("无");
                    lv.SubItems.Add("无");
                    listView1.Items.Add(lv);
                    pos++;
                    datapos++;
                }
            }
            catch
            {
                MessageBox.Show("上传报文有误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        
        private void ShowHisRecordData(byte ty)
        {
            try
            {
                //listView1.Items.Clear();
                int dex = 0; string strdata = @"";
                if (ty == 1)
                {
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        strdata = @"";
                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));
                        

                        int channel = PublicDataClass._DataField.Buffer[dex + 2];
                        try
                        {
                            lv.SubItems.Add(Convert.ToString(PublicDataClass.SaveText.Cfg[0].YxcfgName[channel - 1]));
                        }
                        catch 
                        {
                            lv.SubItems.Add("配置表无对应点号！");
                        }
                        strdata += String.Format("[状态]:{0:d}", PublicDataClass._DataField.Buffer[dex + 3]) + "---故障值:" ;
                        int faultvalue = (PublicDataClass._DataField.Buffer[dex + 1] << 8) +
                                      PublicDataClass._DataField.Buffer[dex + 0];
                        strdata += String.Format("{0:d}", faultvalue);
                        strdata += "---发生的时间:";

                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 4]);
                        strdata += "年";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 5]);
                        strdata += "月";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 6]);  //日+星期
                        strdata += "日";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 7]);
                        strdata += "时";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 8]);
                        strdata += "分";
                        int ms = (PublicDataClass._DataField.Buffer[dex + 10] << 8) +
                                  PublicDataClass._DataField.Buffer[dex + 9];

                        strdata += String.Format("{0:d}", ms);
                        strdata += "毫秒";
                        lv.SubItems.Add(strdata);
                        lv.SubItems.Add("无");
                        lv.SubItems.Add("无");
                        listView1.Items.Add(lv);
                       
                        datapos++;
                        dex += 11;
                    }
                    if (PublicDataClass.seqflag == 1)
                    {
                        PublicDataClass.seq = 1;
                        PublicDataClass.seqflag = 0;
                        PublicDataClass.SQflag = 0;

                        FrameSeq += PublicDataClass._DataField.FieldVSQ;
                        PublicDataClass._DataField.FieldLen = 0;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((FrameSeq) & 0xff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((FrameSeq) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToInt16(textBoxto.Text) & 0xff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)((Convert.ToInt16(textBoxto.Text) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;
                        PublicDataClass._DataField.FieldVSQ = 2;
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.CallRecordData = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.CallRecordData = true;

                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.CallRecordData = true;
                    }
                }
                else if (ty == 2)     //复位记录
                {
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        strdata = @"";
                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));                       

                        int channel = PublicDataClass._DataField.Buffer[dex + 0];
                        lv.SubItems.Add(String.Format("[复位类型]:{0:d}", PublicDataClass._DataField.Buffer[dex + 0]));

                        strdata += String.Format("[状态]:{0:d}", PublicDataClass._DataField.Buffer[dex + 1]);
                        strdata += "---发生的时间:";

                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 2]);
                        strdata += "年";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 3]);
                        strdata += "月";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 4]);  //日+星期
                        strdata += "日";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 5]);
                        strdata += "时";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 6]);
                        strdata += "分";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 7]);
                        strdata += "秒";
                        //int ms = (PublicDataClass._DataField.Buffer[dex + 10] << 8) +
                        //          PublicDataClass._DataField.Buffer[dex + 9];

                        //strdata += String.Format("{0:d}", ms);
                        //strdata += "毫秒";
                        lv.SubItems.Add(strdata);
                        lv.SubItems.Add("无");
                        lv.SubItems.Add("无");
                        listView1.Items.Add(lv);
                        datapos++;
                        dex += 8;
                    }
                    if (PublicDataClass.seqflag == 1)
                    {
                        PublicDataClass.seq = 1;
                        PublicDataClass.seqflag = 0;
                        PublicDataClass.SQflag = 0;

                        FrameSeq += PublicDataClass._DataField.FieldVSQ;
                        PublicDataClass._DataField.FieldLen = 0;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((FrameSeq) & 0xff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((FrameSeq) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToInt16(textBoxto.Text) & 0xff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)((Convert.ToInt16(textBoxto.Text) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;
                        PublicDataClass._DataField.FieldVSQ = 2;
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.CallRecordData = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.CallRecordData = true;

                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.CallRecordData = true;
                    }
                }

                else if (ty == 3)
                {
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        strdata = @"";
                        string a = @"";
                        ListViewItem lv = new ListViewItem(String.Format("{0:d}", datapos));

                        int channel = PublicDataClass._DataField.Buffer[dex];
                        a += String.Format("遥控线路:{0:d}", channel);
                        lv.SubItems.Add(a);
                        strdata += "[状态]";
                        if (PublicDataClass._DataField.Buffer[dex + 1] == 0)
                            strdata += "分";
                        else if (PublicDataClass._DataField.Buffer[dex + 1] == 1)
                            strdata += "合";
                        //strdata += String.Format("[状态]:{0:x}", PublicDataClass._DataField.Buffer[dex + 1]);//+ "---发生的时间:";
                        if (PublicDataClass._DataField.Buffer[dex + 2] == 0)
                            strdata += "[---本地---发生的时间:]";
                        else if (PublicDataClass._DataField.Buffer[dex + 2] == 1)
                            strdata += "[---远方---发生的时间:]";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 3]);
                        strdata += "年";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 4]);
                        strdata += "月";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 5]);  //日+星期
                        strdata += "日";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 6]);
                        strdata += "时";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 7]);
                        strdata += "分";
                        strdata += String.Format("{0:d}", PublicDataClass._DataField.Buffer[dex + 8]);
                        strdata += "秒";
                        //int ms = (PublicDataClass._DataField.Buffer[dex + 8] << 8) +
                        //          PublicDataClass._DataField.Buffer[dex + 9];

                        //strdata += String.Format("{0:d}", ms);
                        //strdata += "毫秒";

                        lv.SubItems.Add(strdata);
                        lv.SubItems.Add("无");
                        lv.SubItems.Add("无");
                        listView1.Items.Add(lv);
                        datapos++;
                        dex += 9;
                    }
                    if (PublicDataClass.seqflag == 1)
                    {
                        PublicDataClass.seq = 1;
                        PublicDataClass.seqflag = 0;
                        PublicDataClass.SQflag = 0;

                        FrameSeq += PublicDataClass._DataField.FieldVSQ;
                        PublicDataClass._DataField.FieldLen = 0;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((FrameSeq) & 0xff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((FrameSeq) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)(Convert.ToInt16(textBoxto.Text) & 0xff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)((Convert.ToInt16(textBoxto.Text) & 0xff00) >> 8);
                        PublicDataClass._DataField.FieldLen += 2;
                        PublicDataClass._DataField.FieldVSQ = 2;
                        if (ty == 1)
                            PublicDataClass._ComTaskFlag.CallRecordData = true;

                        if (ty == 2)
                            PublicDataClass._NetTaskFlag.CallRecordData = true;

                        if (ty == 3)
                            PublicDataClass._GprsTaskFlag.CallRecordData = true;
                    }
                }
            }
            catch
            {
                //MessageBox.Show("上传报文有误","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
               
            }

        }



        private void buttonclear_Click(object sender, EventArgs e)
        {
            datapos = 0;
            pos     = 0;
            listView1.Items.Clear();
        }

        private void buttonoutput_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("记录项内容为空", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.Filter = "*.txt|*.*";
                saveret = savefile.ShowDialog() == DialogResult.OK;
                if (saveret)
                {
                    savename = savefile.FileName;
                    buttonoutput.Text = "导出中..";
                    savefile.Dispose();
                }

            }
        }
        #region ExportToText
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
                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    temp = listView1.Columns[i].Text;
                    len = 20 - Encoding.Default.GetByteCount(temp) + temp.Length; //考虑中英文的情况                  
                    temp = temp.PadRight(len, ' ');
                    s += temp;
                    //s += listView1.Columns[i].Text + "      ";
                }
                s += "\r\n";
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    for (int j = 0; j < listView1.Items[i].SubItems.Count; j++)
                    {
                        temp = listView1.Items[i].SubItems[j].Text;
                        len = 20 - Encoding.Default.GetByteCount(temp) + temp.Length; //考虑中英文的情况                  
                        temp = temp.PadRight(len, ' ');
                        s += temp;
                        //s += listView1.Items[i].SubItems[j].Text + "      ";
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

        private void comboBoxData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxData.SelectedIndex < 3)
            {
                dateTimePickerbegindata.Enabled = false;
                dateTimePickerbegintime.Enabled = false;
                dateTimePickerenddata.Enabled   = false;
                dateTimePickerendtime.Enabled   = false;
                dUpDown.Enabled = false;
                textBoxto.Enabled = true;
                textBoxfrom.Enabled = true;
            }
            else if (comboBoxData.SelectedIndex < 5)
            {
    

                dateTimePickerbegindata.Enabled = true;
                dateTimePickerbegintime.Enabled = true;
                dateTimePickerenddata.Enabled = true;
                dateTimePickerendtime.Enabled = true;
                dUpDown.Enabled = true;
                textBoxto.Enabled = false;
                textBoxfrom.Enabled = false;
               // comboBoxaddr.Enabled = false;
              //  checkBox1.Enabled = false;
            }
            else 
            {


                dateTimePickerbegindata.Enabled = true;
                dateTimePickerbegintime.Enabled = false;
                dateTimePickerenddata.Enabled = true;
                dateTimePickerendtime.Enabled = false;
                dUpDown.Enabled = true;
                textBoxto.Enabled = false;
                textBoxfrom.Enabled = false;
                // comboBoxaddr.Enabled = false;
                //  checkBox1.Enabled = false;
            }
        }
    }
}
