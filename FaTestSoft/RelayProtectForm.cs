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

namespace FaTestSoft
{
    public partial class RelayProtectForm : DockContent
    {
        public RelayProtectForm()
        {
            InitializeComponent();
        }
        private Control[] Editors;
        private int ty;
        private void RelayProtectForm_Load(object sender, EventArgs e)
        {

            //if (PublicDataClass.SaveText.devicenum == 0)
            //{
            //    comboBox1.Text = "无信息";

            //}
            //else
            //{
            //    for (int i = 0; i < PublicDataClass.SaveText.devicenum; i++)
            //    {
            //        comboBox1.Items.Add(PublicDataClass.SaveText.Device[i].PointName);
            //    }
            //    comboBox1.Text = PublicDataClass.SaveText.Device[0].PointName;

            //}

            radioButton1.Checked = true;



            comboBox2.Visible  = false;

            Editors = new Control[] {
	                                textBoxvalue,
									textBoxvalue,			// for column 1
                                    textBoxvalue,
                                    textBoxvalue,           //
									textBoxvalue,  	// 
                                                                  
									};
            ReadFile();
            if (PublicDataClass._RelayProtectParam.num > 0)
            {
                comboBox1.Items.Add(PublicDataClass._RelayProtectParam.LineTable[0]);

                for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                {

                    if (comboBox1.Items.Contains(PublicDataClass._RelayProtectParam.LineTable[j]) == false)
                        comboBox1.Items.Add(PublicDataClass._RelayProtectParam.LineTable[j]);

                }
                comboBox1.Text = PublicDataClass._RelayProtectParam.LineTable[0];
            }

        }

        private void 插入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.Items.Count));
          //  for (int i = 0; i < listView1.Columns.Count - 1; i++)
            {
                lv.SubItems.Add(Convert.ToString(this.listView1.Items.Count));
                lv.SubItems.Add("0");
                lv.SubItems.Add(Convert.ToString(12289 + this.listView1.Items.Count));
                lv.SubItems.Add(Convert.ToString(1 + this.listView1.Items.Count / 2));
            }
            listView1.Items.Add(lv);
            RefreshParamState();
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
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
                RefreshParamState();
            }
        }
        private void RefreshParamState()
        {
            PublicDataClass._RelayProtectParam.num = listView1.Items.Count;
            PublicDataClass._RelayProtectParam.NameTable = new string[listView1.Items.Count];
            PublicDataClass._RelayProtectParam.ValueTable = new string[listView1.Items.Count];
            PublicDataClass._RelayProtectParam.AddrTable = new string[listView1.Items.Count];
            PublicDataClass._RelayProtectParam.LineTable = new string[listView1.Items.Count];


                    for (int j = 0; j < listView1.Items.Count; j++)
                    {
                        PublicDataClass._RelayProtectParam.NameTable[j] = listView1.Items[j].SubItems[1].Text;//取得listview某行某列的值

                        PublicDataClass._RelayProtectParam.ValueTable[j] = listView1.Items[j].SubItems[2].Text;

                        PublicDataClass._RelayProtectParam.AddrTable[j] = listView1.Items[j].SubItems[3].Text;

                        PublicDataClass._RelayProtectParam.LineTable[j] = listView1.Items[j].SubItems[4].Text;
                    }
        }

        private void 插入ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ListViewItem lv = new ListViewItem(String.Format("{0:d}", this.listView1.SelectedItems[0].Index));
            for (int i = 0; i < listView1.Columns.Count - 1; i++)
            {
                lv.SubItems.Add("");
            }
            listView1.Items.Insert(this.listView1.SelectedItems[0].Index, lv);

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].SubItems[0].Text = String.Format("{0:d}", i);   //重新调整序号
            }
            RefreshParamState();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string FileName = "";
            string path = PublicDataClass.PrjPath + "\\ini";
            FileName = path + "\\RelayProtectparam.ini";
            WriteReadAllFile.WriteReadParamIniFile(FileName, 1, 11);
            //Form1.WriteIniFilek = 11;
            //Form1.WriteIniFileName = FileName;
            //Form1.WriteIniFileType = 1;
            //Form1.WriteIniflag = true;
            MessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < listView1.Items.Count; j++)
                listView1.Items[j].Checked = true;
        }

        private void buttonsave_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < listView1.Items.Count; j++)
                listView1.Items[j].Checked = false ;
        }

        private void readbutton_Click(object sender, EventArgs e)
        {
          

            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            int num = 0;
            if (radioButton1.Checked == true)//召唤具体定值
            {
                for (int j = 0; j < listView1.Items.Count; j++)
                {

                    if (listView1.Items[j].Checked == true)
                    {
                        num++;
                        if (num > 1)
                        {
                            MessageBox.Show("召唤单一定值功能，请重新选择！", "信息",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;

                        }


                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(listView1.Items[j].SubItems[3].Text)) & 0x00ff);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(listView1.Items[j].SubItems[3].Text)) & 0xff00) >> 8);
                        PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = Convert.ToByte(listView1.Items[j].SubItems[4].Text);
                        PublicDataClass._DataField.FieldLen += 3;
                        PublicDataClass._DataField.FieldVSQ++;

                    }

                }
     if (num == 0)
                        {
                            MessageBox.Show("请选择召唤的具体定值！", "信息",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
            }
            else if (radioButton2.Checked == true)//召唤线路所有定值
            {
                if (comboBox2.Text != "")
                {
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0xff;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = 0xff;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = Convert.ToByte(comboBox2.Text);
                    PublicDataClass._DataField.FieldLen += 3;
                    PublicDataClass._DataField.FieldVSQ++;
                }
                else 
                {
                           MessageBox.Show("请选择线路号！", "信息",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                           return;
                            }
            
            }
            timer2.Enabled = true;
           // if (ty == 1)
           //     PublicDataClass._ComTaskFlag.READ_P_1 = true;

        //    if (ty == 2)
                PublicDataClass._NetTaskFlag.READ_ReP_1 = true;

        }

        private void downloadbutton_Click(object sender, EventArgs e)
        {
            //if (comboBox1.Text == "无信息")
            //{
            //    MessageBox.Show("无测量点信息可操作", "信息",
            //       MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;

            //}
            //else
            //{
            //    ty = PublicFunction.CheckPointOfCommunicationEntrace(comboBox1.Text);
            //    if (ty == 0)
            //    {
            //        MessageBox.Show("无测量点信息可操作", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;

            //    }

            //}
            int num = 0;
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            for (int j = 0; j < listView1.Items.Count; j++)
            {
                if (listView1.Items[j].Checked == true)
                {
                    num++;
               
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(listView1.Items[j].SubItems[3].Text)) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(listView1.Items[j].SubItems[3].Text)) & 0xff00) >> 8);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = Convert.ToByte(listView1.Items[j].SubItems[4].Text);
                    PublicDataClass._DataField.FieldLen += 3;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = (byte)((Convert.ToInt32(listView1.Items[j].SubItems[2].Text)) & 0x00ff);
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = (byte)(((Convert.ToInt32(listView1.Items[j].SubItems[2].Text)) & 0xff00) >> 8);

                    PublicDataClass._DataField.FieldLen += 2;
                    PublicDataClass._DataField.FieldVSQ++;

                }


            }
            if (num == 0)
            {
                MessageBox.Show("请选择下装的具体定值！", "信息",
              MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            timer2.Enabled = true;
           // if (ty == 1)
              //  PublicDataClass._ComTaskFlag.SET_PARAM_CON = true;

            //if (ty == 2)
                PublicDataClass._NetTaskFlag.SET_ReP= true;
          //  if (ty == 3)
            //    PublicDataClass._GprsTaskFlag.SET_PARAM_CON = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
         
            int dex = 0;
            if (PublicDataClass._WindowsVisable.RelayProtectVisable== true)  //窗体可见
            {
        
                if (PublicDataClass._Message.CallRelayProtectView == true)
                {
                    PublicDataClass._Message.CallRelayProtectView = false;
                    timer2.Enabled = false;
                    listView1.Items.Clear();
                    for (int j = 0; j < PublicDataClass._DataField.FieldVSQ; j++)
                    {
                        int add = 0;
                       add = PublicDataClass._DataField.Buffer[dex] + (PublicDataClass._DataField.Buffer[dex + 1] << 8);
                       int line = 0;
                       line = PublicDataClass._DataField.Buffer[dex+2];
       int value = 0;
                         value += PublicDataClass._DataField.Buffer[dex+3] + (PublicDataClass._DataField.Buffer[dex + 4] << 8);
                     
                         ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                         int num = 0;
                         for (int i = 0; i < PublicDataClass._RelayProtectParam.num; i++)
                         {
                             num++;
                             if (Convert.ToString(add) == PublicDataClass._RelayProtectParam.AddrTable[i])
                             {
                                 lv.SubItems.Add(PublicDataClass._RelayProtectParam.NameTable[i]);//按地址查找name
                                 break;
                             }
                             else
                             {
                                 if (num == PublicDataClass._RelayProtectParam.num)
                                     lv.SubItems.Add("该信息体地址未配置");
                             }


                         }

                         lv.SubItems.Add(Convert.ToString(value));
                         lv.SubItems.Add(Convert.ToString(add));
                         lv.SubItems.Add(Convert.ToString(line));
                         listView1.Items.Add(lv);
                         dex += 5;
                         if (dex > 1024)
                         {
                             MessageBox.Show("接收buffer溢出！", "信息",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                             return;
                         }
                        
                         //for (int i = 0; i < listView1.Items.Count; i++)
                         //{
                         //    if (Convert.ToInt32(listView1.Items[i].SubItems[3].Text) == add)
                         //    {
                         //        if (Convert.ToString(value) != listView1.Items[i].SubItems[2].Text)
                         //        {
                         //            listView1.Items[i].UseItemStyleForSubItems = false;
                         //            listView1.Items[i].SubItems[2].ForeColor = Color.Red;
                         //        }
                         //        else
                         //        {
                         //            listView1.Items[i].UseItemStyleForSubItems = false;
                         //            listView1.Items[i].SubItems[2].ForeColor = Color.Green;

                         //        }
                         //        listView1.Items[i].SubItems[2].Text = Convert.ToString(value);
                         //        dex += 5;
                         //        if (dex > 1024)
                         //        {
                         //            MessageBox.Show("接受buffer溢出！", "信息",
                         //          MessageBoxButtons.OK, MessageBoxIcon.Error);
                         //            return;
                         //        }
                         //    }
                        
                         //}
                    }
                
                    
              
                }

            }
        }

        private void RelayProtectForm_Activated(object sender, EventArgs e)
        {
            PublicDataClass._WindowsVisable.RelayProtectVisable= true;       
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                comboBox2.Visible = true;
                comboBox2.Items.Clear();
                if (PublicDataClass._RelayProtectParam.num > 0)
                {
                    comboBox2.Items.Add(PublicDataClass._RelayProtectParam.LineTable[0]);
                   
                    for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
                    {

                        if (comboBox2.Items.Contains(PublicDataClass._RelayProtectParam.LineTable[j]) == false)
                            comboBox2.Items.Add(PublicDataClass._RelayProtectParam.LineTable[j]);

                    }
                    comboBox2.Text = PublicDataClass._RelayProtectParam.LineTable[0];
                }
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
                comboBox2.Visible = false ;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReadFile();
        }

        private void ReadFile()
        {
            listView1.Items.Clear();
            string FileName = "";
            string path = PublicDataClass.PrjPath + "\\ini";
            FileName = path + "\\RelayProtectparam.ini";
            WriteReadAllFile.WriteReadParamIniFile(FileName, 0, 11);
            //Form1.WriteIniFilek = 11;
            //Form1.WriteIniFileName = FileName;
            //Form1.WriteIniFileType = 0;
            //Form1.WriteIniflag = true;
            for (int j = 0; j < PublicDataClass._RelayProtectParam.num; j++)
            {
                ListViewItem lv = new ListViewItem(String.Format("{0:d}", j));
                lv.SubItems.Add(PublicDataClass._RelayProtectParam.NameTable[j]);
                lv.SubItems.Add(PublicDataClass._RelayProtectParam.ValueTable[j]);
                lv.SubItems.Add(PublicDataClass._RelayProtectParam.AddrTable[j]);
                lv.SubItems.Add(PublicDataClass._RelayProtectParam.LineTable[j]);
                listView1.Items.Add(lv);
            }
        }

        private void textBoxvalue_Leave(object sender, EventArgs e)
        {
            listView1.EndEditing(true);
      
            RefreshParamState();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;           
         
                if (comboBox1.Text != "")
                {
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x00;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = 0x00;
                    PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = Convert.ToByte(comboBox1.Text);
                    PublicDataClass._DataField.FieldLen += 3;
                    PublicDataClass._DataField.FieldVSQ++;
                }
                else 
                {
                           MessageBox.Show("请选择线路号！", "信息",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                           return;
                            }
            
            
            
                PublicDataClass._NetTaskFlag.ACT_ReP  = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PublicDataClass._DataField.FieldLen = 0;
            PublicDataClass._DataField.FieldVSQ = 0;
            if (comboBox1.Text != "")
            {
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen] = 0x00;
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 1] = 0x00;
                PublicDataClass._DataField.Buffer[PublicDataClass._DataField.FieldLen + 2] = Convert.ToByte(comboBox1.Text);
                PublicDataClass._DataField.FieldLen += 3;
                PublicDataClass._DataField.FieldVSQ++;
            }
            else
            {
                MessageBox.Show("请选择线路号！", "信息",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            PublicDataClass._NetTaskFlag.CancelACT_ReP = true;
        }

        private void listView1_SubItemClicked_1(object sender, SubItemEventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0)
                return;
            if (e.SubItem == 0) // Password field
            {
                return;
            }

            else
            {

                //if (e.SubItem != 2) // Password field

                //    return;

                //else
                listView1.StartEditing(Editors[e.SubItem], e.Item, e.SubItem);

            }
        }
    }
}
