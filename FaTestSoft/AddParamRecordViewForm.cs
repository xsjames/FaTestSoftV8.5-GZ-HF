using System;
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
    public partial class AddParamRecordViewForm : Form
    {
        public AddParamRecordViewForm()
        {
            InitializeComponent();
        }
        static int themax = 64;
        static int thenum1 = 0;
        static int thenum2 = 0;
        static int thenum3 = 0;
        static int insertindex = 0;
        static bool inserflag = false;
        private Control[] Editors;

        private void AddParamRecordViewForm_Load(object sender, EventArgs e)
        {
            //Editors = new Control[] {
            //                        textBoxvalue,
            //                        textBoxvalue,			// for column 1
            //                        textBoxvalue,
            //                        textBoxvalue,           //
                             
            //                                // 
                                                                  
            //                        };

            checkBox1.Visible = false;
            if (PublicDataClass._FastParamRecord.ItemId == 4)
            {
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                button2.Visible = false;
                button4.Visible = false;

         


                for (int i = 0; i < PublicDataClass._YcConfigParam.num; i++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass._YcConfigParam.IndexTable[i]);
                    lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[i]);
                    listView1.Items.Add(lv);
                }

                int ycadr = 16385;
                for (int k = 0; k < PublicDataClass._YcConfigParam.num; k++)
                {
                    for (int j = 0; j < PublicDataClass._YcConfigParam.num; j++)
                    {
                        try
                        {
                            if (Convert.ToInt32(PublicDataClass._YcConfigParam.AddrTable[j]) == ycadr)
                            {
                                ListViewItem lv = new ListViewItem(PublicDataClass._YcConfigParam.IndexTable[j]);
                                lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[j]);
                                lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 16385));
                        
                                listView2.Items.Add(lv);
                                ycadr++;
                                break;
                            }
                        }
                        catch
                        { }
                    }
                }
            }
            else if (PublicDataClass._FastParamRecord.ItemId == 5)
            {
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                button2.Visible = false;
                button4.Visible = false;
                //ColumnHeader title = new ColumnHeader(); //声明标头，并创建对象。
                //title.Text = "遥信置数"; //标头一显示的名称。
                //title.Width = 120; //标头一名称 的宽度。
                //this.listView2.Columns.Add(title); //将标头添加到ListView控件。
                //checkBox1.Visible = true;
                //checkBox1.Checked = true;
                for (int i = 0; i < PublicDataClass._YxConfigParam.num; i++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass._YxConfigParam.IndexTable[i]);
                    lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[i]);
                    listView1.Items.Add(lv);
                }

                int ycadr = 1;
                for (int k = 0; k < PublicDataClass._YxConfigParam.num; k++)
                {
                    for (int j = 0; j < PublicDataClass._YxConfigParam.num; j++)
                    {
                        try
                        {
                            if (Convert.ToInt32(PublicDataClass._YxConfigParam.AddrTable[j]) == ycadr)
                            {
                                ListViewItem lv = new ListViewItem(PublicDataClass._YxConfigParam.IndexTable[j]);
                                lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[j]);
                                lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 1));
                                //lv.SubItems.Add("0");
                                listView2.Items.Add(lv);
                                ycadr++;
                                break;
                            }
                        }
                        catch
                        { }
                    }
                }
            }
            else if (PublicDataClass._FastParamRecord.ItemId == 104)
            {
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
                button2.Visible = true;
                button4.Visible = true;
                radioButton1.Checked = true;
                for (int i = 0; i < PublicDataClass._YcConfigParam.num; i++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass._YcConfigParam.IndexTable[i]);
                    lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[i]);
                    listView1.Items.Add(lv);
                }


            }
            else if (PublicDataClass.FILENAME[PublicDataClass._FastParamRecord.ItemId].IndexOf("电量配置") > 0)
            {
                int ItemId = PublicDataClass._FastParamRecord.ItemId;
                radioButton1.Visible = false;
                radioButton2.Visible = false;
                radioButton3.Visible = false;
                button2.Visible = false;
                button4.Visible = false;




                for (int i = 0; i < PublicDataClass.TabCfg[ItemId].LineNum; i++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass.TabCfg[ItemId].TabPageValue[2].ValueTable[i]);
                    lv.SubItems.Add(PublicDataClass.TabCfg[ItemId].TabPageValue[1].ValueTable[i]);
                    listView1.Items.Add(lv);
                }

                int ycadr = 25601;
                for (int k = 0; k < PublicDataClass.TabCfg[ItemId].LineNum; k++)
                {
                    for (int j = 0; j < PublicDataClass.TabCfg[ItemId].LineNum; j++)
                    {
                        try
                        {
                            if (Convert.ToInt32(PublicDataClass.TabCfg[ItemId].TabPageValue[3].ValueTable[j]) == ycadr)
                            {
                                ListViewItem lv = new ListViewItem(PublicDataClass.TabCfg[ItemId].TabPageValue[2].ValueTable[j]);
                                lv.SubItems.Add(PublicDataClass.TabCfg[ItemId].TabPageValue[1].ValueTable[j]);
                                lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 25601));

                                listView2.Items.Add(lv);
                                ycadr++;
                                break;
                            }
                        }
                        catch
                        { }
                    }
                }
            }
        }

        private void buttonok_Click(object sender, EventArgs e)
        {
            if (PublicDataClass._FastParamRecord.ItemId != 104)
            {
                PublicDataClass._FastParamRecord.num = listView2.Items.Count;
                PublicDataClass._FastParamRecord.index = new int[listView2.Items.Count];
                PublicDataClass._FastParamRecord.addr = new string[listView2.Items.Count];
                //PublicDataClass._FastParamRecord.yxzs = new string[listView2.Items.Count];
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    PublicDataClass._FastParamRecord.index[i] = Convert.ToInt32(listView2.Items[i].SubItems[0].Text);
                    PublicDataClass._FastParamRecord.addr[i] = listView2.Items[i].SubItems[2].Text;
                    //PublicDataClass._FastParamRecord.yxzs[i] = "0";
         
                }
            }
           //if( checkBox1.Checked == true)
            PublicDataClass._ChangeFlag.YxkjCfg = true;
            this.DialogResult = DialogResult.OK;

        }

        private void allRight_Click(object sender, EventArgs e)
        {
            if (PublicDataClass._FastParamRecord.ItemId != 104)
            {
                ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView1);//定义一个选择项的集合
                if (SettleOnItem.Count == 0)
                    MessageBox.Show("请选择配置项", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    int num = listView2.Items.Count;
                    if (inserflag == false)
                    {
                        for (int i = 0; i < SettleOnItem.Count; i++)//循环遍历选择的每一项
                        {
                            bool flag = false;
                            for (int j = 0; j < num; j++)
                            {
                                if (listView2.Items[j].SubItems[0].Text == SettleOnItem[i].Text)
                                {
                                    MessageBox.Show(SettleOnItem[i].SubItems[1].Text + "重复配置！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag == false)
                            {

                                ListViewItem lv = new ListViewItem(SettleOnItem[i].Text);
                                lv.SubItems.Add(SettleOnItem[i].SubItems[1].Text);
                                if (PublicDataClass._FastParamRecord.ItemId == 4)
                                    lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 16385));
                                else if (PublicDataClass._FastParamRecord.ItemId == 5)
                                {
                                    lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 1));
                                    lv.SubItems.Add("0");
                                }
                                else if (PublicDataClass.FILENAME[PublicDataClass._FastParamRecord.ItemId].IndexOf("电量配置") > 0)
                                    lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 25601));
                                    listView2.Items.Add(lv);
                            }

                        }
                    }
                    else
                    {
                        for (int i = SettleOnItem.Count - 1; i >= 0; i--)//循环遍历选择的每一项
                        {
                            bool flag = false;
                            for (int j = 0; j < num; j++)
                            {
                                if (listView2.Items[j].SubItems[0].Text == SettleOnItem[i].Text)
                                {
                                    MessageBox.Show(SettleOnItem[i].SubItems[1].Text + "重复配置！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag == false)
                            {

                                ListViewItem lv = new ListViewItem(SettleOnItem[i].Text);
                                lv.SubItems.Add(SettleOnItem[i].SubItems[1].Text);
                                if (PublicDataClass._FastParamRecord.ItemId == 4)
                                    lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 16385));
                                else if (PublicDataClass._FastParamRecord.ItemId == 5)
                                    lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 1));
                                //listView2.Items.Add(lv);
                              
                                    else if (PublicDataClass.FILENAME[PublicDataClass._FastParamRecord.ItemId].IndexOf("电量配置") > 0)
                                    lv.SubItems.Add(Convert.ToString(listView2.Items.Count + 25601));
                                    listView2.Items.Insert(insertindex, lv);
                            }

                        }
                        if (PublicDataClass._FastParamRecord.ItemId == 4)
                        {
                            for (int i = 0; i < listView2.Items.Count; i++)
                            {
                                listView2.Items[i].SubItems[2].Text = String.Format("{0:d}", i + 16385);   //重新调整序号
                            }
                        }
                        else if (PublicDataClass._FastParamRecord.ItemId == 5)
                        {
                            for (int i = 0; i < listView2.Items.Count; i++)
                            {
                                listView2.Items[i].SubItems[2].Text = String.Format("{0:d}", i + 1);   //重新调整序号
                            }
                        }
                        else if (PublicDataClass.FILENAME[PublicDataClass._FastParamRecord.ItemId].IndexOf("电量配置") > 0)
                        {
                            for (int i = 0; i < listView2.Items.Count; i++)
                            {
                                listView2.Items[i].SubItems[2].Text = String.Format("{0:d}", i + 25601);   //重新调整序号
                            }
                        }
                        inserflag = false;
                    }


                }
            }
            else
            {
                ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView1);//定义一个选择项的集合
                if (SettleOnItem.Count == 0)
                    MessageBox.Show("请选择配置项", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    int num = listView2.Items.Count;
                    if ((SettleOnItem.Count + num) > themax)
                    {
                        MessageBox.Show("配置点号个数越限!A帧最多64个;B帧最多64个;A、B、C帧个数和最多256个！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    for (int i = 0; i < SettleOnItem.Count; i++)//循环遍历选择的每一项
                    {
                        bool flag = false;
                        for (int j = 0; j < num; j++)
                        {
                            if (listView2.Items[j].SubItems[0].Text == SettleOnItem[i].Text)
                            {
                                MessageBox.Show(SettleOnItem[i].SubItems[1].Text + "重复配置！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                flag = true;
                                break;
                            }
                        }
                        if (flag == false)
                        {
                            ListViewItem lv = new ListViewItem(SettleOnItem[i].Text);
                            lv.SubItems.Add(SettleOnItem[i].SubItems[1].Text);
                            lv.SubItems.Add("");
                            listView2.Items.Add(lv);
                        }

                    }
                    if (radioButton1.Checked == true)
                        thenum1 = listView2.Items.Count;
                    else if (radioButton2.Checked == true)
                        thenum2 = listView2.Items.Count;
                    else if (radioButton3.Checked == true)
                        thenum3 = listView2.Items.Count;

                    RefreshParamState();
                }

            }
        }
        private void RefreshParamState()
        {
            if (radioButton1.Checked == true)
            {
                PublicDataClass._FastParamRecord.num1 = listView2.Items.Count;
                PublicDataClass._FastParamRecord.CDTindex1 = new string[PublicDataClass._FastParamRecord.num1];
                PublicDataClass._FastParamRecord.CDTname1 = new string[PublicDataClass._FastParamRecord.num1];
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    PublicDataClass._FastParamRecord.CDTindex1[i] = listView2.Items[i].SubItems[0].Text;
                    PublicDataClass._FastParamRecord.CDTname1[i] = listView2.Items[i].SubItems[1].Text;
                }
            }
            else if (radioButton2.Checked == true)
            {
                PublicDataClass._FastParamRecord.num2 = listView2.Items.Count;
                PublicDataClass._FastParamRecord.CDTindex2 = new string[PublicDataClass._FastParamRecord.num2];
                PublicDataClass._FastParamRecord.CDTname2 = new string[PublicDataClass._FastParamRecord.num2];
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    PublicDataClass._FastParamRecord.CDTindex2[i] = listView2.Items[i].SubItems[0].Text;
                    PublicDataClass._FastParamRecord.CDTname2[i] = listView2.Items[i].SubItems[1].Text;
                }
            }
            else if (radioButton3.Checked == true)
            {
                PublicDataClass._FastParamRecord.num3 = listView2.Items.Count;
                PublicDataClass._FastParamRecord.CDTindex3 = new string[PublicDataClass._FastParamRecord.num3];
                PublicDataClass._FastParamRecord.CDTname3 = new string[PublicDataClass._FastParamRecord.num3];
                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    PublicDataClass._FastParamRecord.CDTindex3[i] = listView2.Items[i].SubItems[0].Text;
                    PublicDataClass._FastParamRecord.CDTname3[i] = listView2.Items[i].SubItems[1].Text;
                }
            }
        }
        private void allLeft_Click(object sender, EventArgs e)
        {
            if (PublicDataClass._FastParamRecord.ItemId != 104)
            {
                ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView2);//定义一个选择项的集合
                if (SettleOnItem.Count == 0)
                    MessageBox.Show("请选择移除项", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    for (int i = 0; i < SettleOnItem.Count; )
                    {

                        listView2.Items.Remove(SettleOnItem[i]);       //删除所选择的项
                    }
                    for (int i = 0; i < listView2.Items.Count; i++)
                    {
                        if (PublicDataClass._FastParamRecord.ItemId == 4)
                            listView2.Items[i].SubItems[2].Text = String.Format("{0:d}", 16385 + i);   //重新调整序号
                        else if (PublicDataClass._FastParamRecord.ItemId == 5)
                            listView2.Items[i].SubItems[2].Text = String.Format("{0:d}", 1 + i);   //重新调整序号
                    }
                    if (radioButton1.Checked == true)
                        thenum1 = listView2.Items.Count;
                    else if (radioButton2.Checked == true)
                        thenum2 = listView2.Items.Count;
                    else if (radioButton3.Checked == true)
                        thenum3 = listView2.Items.Count;
                    RefreshParamState();
                }
            }
            else
            {
                ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView2);//定义一个选择项的集合
                if (SettleOnItem.Count == 0)
                    MessageBox.Show("请选择移除项", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    for (int i = 0; i < SettleOnItem.Count; )
                    {

                        listView2.Items.Remove(SettleOnItem[i]);       //删除所选择的项
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Focus();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Selected = true;   //

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView2.Focus();
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                listView2.Items[i].Selected = true;   //

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                themax = 64;
                PublicDataClass.ParamInfoAddr = 0x8001;
                listView2.Items.Clear();
                for (int i = 0; i < PublicDataClass._FastParamRecord.num1; i++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass._FastParamRecord.CDTindex1[i]);
                    lv.SubItems.Add(PublicDataClass._FastParamRecord.CDTname1[i]);
                    lv.SubItems.Add("");
                    listView2.Items.Add(lv);
                }
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                themax = 64;
                PublicDataClass.ParamInfoAddr = 0x8002;
                listView2.Items.Clear();
                for (int i = 0; i < PublicDataClass._FastParamRecord.num2; i++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass._FastParamRecord.CDTindex2[i]);
                    lv.SubItems.Add(PublicDataClass._FastParamRecord.CDTname2[i]);
                    lv.SubItems.Add("");
                    listView2.Items.Add(lv);
                }
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                themax = 256 - thenum1 - thenum2;
                PublicDataClass.ParamInfoAddr = 0x8003;
                listView2.Items.Clear();
                for (int i = 0; i < PublicDataClass._FastParamRecord.num3; i++)
                {
                    ListViewItem lv = new ListViewItem(PublicDataClass._FastParamRecord.CDTindex3[i]);
                    lv.SubItems.Add(PublicDataClass._FastParamRecord.CDTname3[i]);
                    lv.SubItems.Add("");
                    listView2.Items.Add(lv);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PublicDataClass.SQflag = 0;
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            int id = 0;
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                id = Convert.ToInt32(listView2.Items[i].SubItems[0].Text) - 1;

                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((id) & 0x00ff);
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((id) & 0xff00) >> 8);
                PublicDataClass._DataField.FieldLen += 2;

                PublicDataClass._DataField.FieldVSQ++;
            }
            PublicDataClass.seqflag = 0;
            PublicDataClass.seq = 1;

            int ty = PublicFunction.CheckPointOfCommunicationEntrace(PublicDataClass.SaveText.Device[0].PointName);
            if (ty == 1)
                PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;
            else if (ty == 2)
                PublicDataClass._NetTaskFlag.SET_PARAM_CON = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            PublicDataClass.seq = 1;
            PublicDataClass.seqflag = 0;
            PublicDataClass.SQflag = 0;
            int ty = PublicFunction.CheckPointOfCommunicationEntrace(PublicDataClass.SaveText.Device[0].PointName);
            if (ty == 1)
                PublicDataClass._ComTaskFlag.READ_P_1 = true;
            else if (ty == 2)
                PublicDataClass._NetTaskFlag.READ_P_1 = true;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PublicDataClass._Message.ReadParam == true)
            {
                PublicDataClass._Message.ReadParam = false;
                timer1.Enabled = false;
                int dex = 0;
                listView2.Items.Clear();
                for (int i = 0; i < PublicDataClass._DataField.FieldVSQ; i++)
                {
                    int id = 0;
                    id = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                    if (id != 65535)
                    {
                        ListViewItem lv = new ListViewItem(listView1.Items[id].SubItems[0].Text);
                        lv.SubItems.Add(listView1.Items[id].SubItems[1].Text);
                        lv.SubItems.Add("");
                        listView2.Items.Add(lv);
                    }
                    dex += 2;
                }
                RefreshParamState();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string findstr = @"";
            findstr = textBox1.Text;
            listView1.Items.Clear();
            if ((PublicDataClass._FastParamRecord.ItemId == 4) || (PublicDataClass._FastParamRecord.ItemId == 104))
            {
                if (findstr == "")
                {
                    for (int i = 0; i < PublicDataClass._YcConfigParam.num; i++)
                    {

                        ListViewItem lv = new ListViewItem(PublicDataClass._YcConfigParam.IndexTable[i]);
                        lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[i]);
                        listView1.Items.Add(lv);

                    }
                }
                else
                {
                    for (int i = 0; i < PublicDataClass._YcConfigParam.num; i++)
                    {
                        if (PublicDataClass._YcConfigParam.NameTable[i].IndexOf(findstr) >= 0)
                        {
                            ListViewItem lv = new ListViewItem(PublicDataClass._YcConfigParam.IndexTable[i]);
                            lv.SubItems.Add(PublicDataClass._YcConfigParam.NameTable[i]);
                            listView1.Items.Add(lv);
                        }
                    }
                }


            }
            else if (PublicDataClass._FastParamRecord.ItemId == 5)
            {
                if (findstr == "")
                {
                    for (int i = 0; i < PublicDataClass._YxConfigParam.num; i++)
                    {

                        ListViewItem lv = new ListViewItem(PublicDataClass._YxConfigParam.IndexTable[i]);
                        lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[i]);
                        listView1.Items.Add(lv);

                    }
                }
                else
                {
                    for (int i = 0; i < PublicDataClass._YxConfigParam.num; i++)
                    {
                        if (PublicDataClass._YxConfigParam.NameTable[i].IndexOf(findstr) >= 0)
                        {
                            ListViewItem lv = new ListViewItem(PublicDataClass._YxConfigParam.IndexTable[i]);
                            lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[i]);
                            listView1.Items.Add(lv);
                        }
                    }
                }

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (inserflag == true)
            {
                inserflag = false;
                return;
            }
            ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView2);//定义一个选择项的集合
            if (SettleOnItem.Count == 0)
                MessageBox.Show("请先在右边列表框中选择需要插入的位置", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                insertindex = SettleOnItem[0].Index;
                inserflag = true;
                MessageBox.Show("请在左边列表框中选择需要插入的条目，再单击>>按钮", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listView2_SubItemClicked(object sender, SubItemEventArgs e)
        {
            if (this.listView2.SelectedItems.Count == 0)
                return;
            if (e.SubItem == 0) // Password field
            {
                return;
            }


                if (e.SubItem == 3)
                {
                    listView2.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);
                }
   

        }

        private void 批量修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
