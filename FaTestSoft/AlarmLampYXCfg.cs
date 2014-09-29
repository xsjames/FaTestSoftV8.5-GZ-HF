using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class AlarmLampYXCfg : Form
    {
        public AlarmLampYXCfg()
        {
            InitializeComponent();
        }
        int YXnum = 0;
        int[] YXAlarmNum = new int[32];
        private void AlarmLampYXCfg_Load(object sender, EventArgs e)
        {
        
            for(int i=0;i<32;i++)
            {
                comboBox1.Items.Add(string.Format("{0:d}", i));
              
            }
     
            for (int i = 0; i < PublicDataClass._YxConfigParam.num; i++)
            {
                ListViewItem lv = new ListViewItem(PublicDataClass._YxConfigParam.IndexTable[i]);
                lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[i]);
                listView1.Items.Add(lv);
            }

                for (int k = 0; k < 32; k++)
                { 
                    int num = 0;
                    for (int j = 1; j < 25; j++)
                    {
                        try
                        {
                           
                            if (PublicDataClass.TabCfg[PublicDataClass._FastParamRecord.ItemId].TabPageValue[j].ValueTable[k] != "65536")
                            {
                                num++;
                                //表中值加一为遥信索引号
                                ListViewItem lv = new ListViewItem(Convert.ToString(Convert.ToUInt32(PublicDataClass.TabCfg[PublicDataClass._FastParamRecord.ItemId].TabPageValue[j].ValueTable[k]) + 1));
                                lv.SubItems.Add(PublicDataClass._YxConfigParam.NameTable[Convert.ToUInt32(PublicDataClass.TabCfg[PublicDataClass._FastParamRecord.ItemId].TabPageValue[j].ValueTable[k])]);
                                lv.SubItems.Add(Convert.ToString(k));
                                lv.SubItems.Add(Convert.ToString(j));
                                listView2.Items.Add(lv);
                                YXAlarmNum[k] = num;
                                
                            }
                        }
                        catch
                        { }
                    }
                }
                comboBox1.SelectedIndex = 0;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string findstr = @"";
            findstr = textBox1.Text;
            listView1.Items.Clear();

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

        private void allRight_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection SettleOnItem = new ListView.SelectedListViewItemCollection(this.listView1);//定义一个选择项的集合
            if (SettleOnItem.Count == 0)
                MessageBox.Show("请选择配置项", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                int num = listView2.Items.Count;
             
                    for (int i = 0; i < SettleOnItem.Count; i++)//循环遍历选择的每一项
                    {
                        //2014.5.6封，去掉重复配置提示，功能需要允许重复配置
                        //bool flag = false;
                        //for (int j = 0; j < num; j++)
                        //{
                        //    if (listView2.Items[j].SubItems[0].Text == SettleOnItem[i].Text)
                        //    {
                        //        MessageBox.Show(SettleOnItem[i].SubItems[1].Text + "重复配置！", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //        flag = true;
                        //        break;
                        //    }
                        //}
                        //if (flag == false)
                        {
                            if (YXnum+1 > 24)
                            {
                                MessageBox.Show("超过最大关联遥信，请重新选择灯号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            } ListViewItem lv = new ListViewItem(SettleOnItem[i].Text);
                            lv.SubItems.Add(SettleOnItem[i].SubItems[1].Text);

                            lv.SubItems.Add(comboBox1.Text);
                            lv.SubItems.Add(Convert.ToString(++YXnum));

                            listView2.Items.Add(lv);
                        }

                    }
                
       


            }
        }

        private void allLeft_Click(object sender, EventArgs e)
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

            //    RefreshParamState();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            YXnum = YXAlarmNum[comboBox1.SelectedIndex];
        }

        private void buttonok_Click(object sender, EventArgs e)
        {
            PublicDataClass._FastParamRecord.num = listView2.Items.Count;
            PublicDataClass._FastParamRecord.index = new int[listView2.Items.Count];
            PublicDataClass._FastParamRecord.alarmY = new int[listView2.Items.Count];
            PublicDataClass._FastParamRecord.alarmX = new int[listView2.Items.Count];
         
    
            for (int i = 0; i < listView2.Items.Count; i++)
            {
                PublicDataClass._FastParamRecord.index[i] = Convert.ToInt32(listView2.Items[i].SubItems[0].Text);
                PublicDataClass._FastParamRecord.alarmY[i] = Convert.ToInt32(listView2.Items[i].SubItems[2].Text);
                PublicDataClass._FastParamRecord.alarmX[i] = Convert.ToInt32(listView2.Items[i].SubItems[3].Text);
            }

            this.DialogResult = DialogResult.OK;
        }


    }
}
