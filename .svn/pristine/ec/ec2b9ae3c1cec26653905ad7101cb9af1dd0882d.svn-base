using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FaTestSoft.CommonData;

namespace FaTestSoft
{
    public partial class ChannelClassViewForm : Form
    {
        public ChannelClassViewForm()
        {
            InitializeComponent();
        }

        private ComboBox cmb_Temp = new ComboBox();                                                       //建立一个组合框的对象
        private static int ColumnsMenu = 0;
        //组合框移动的顺序
        string[] str = new string[9];
        private static byte rows = 0;

        /// <summary>
        ///  窗体的初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelClassViewForm_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToResizeColumns = false;                                              //不允许改变列宽
            dataGridView1.RowHeadersWidthSizeMode  = DataGridViewRowHeadersWidthSizeMode.DisableResizing;  //行头不允许改变大小
            //dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.Red;


            cmb_Temp.Enabled       = false;                                                                     //组合框控件不允许
            cmb_Temp.DropDownStyle = ComboBoxStyle.DropDownList;                                          //设置组合框的样式 --下拉
            cmb_Temp.Visible       = false;                                                               //隐藏组合框 不可见
            
            cmb_Temp.MouseLeave += new System.EventHandler(cmb_Temp_MouseLeave);
            cmb_Temp.SelectedIndexChanged += new EventHandler(cmb_Temp_SelectedIndexChanged);             //添加组合框的事件
            this.dataGridView1.Controls.Add(cmb_Temp);                                                    //向datagridview中加入
            if (PublicDataClass.PrjType == 2)
            {
                for (byte i = 0; i < PublicDataClass.SaveText.channelnum; i++)
                {
                    str[0] = String.Format("通道{0:d}", i);
                    str[1] = PublicDataClass.SaveText.Channel[i].ChannelID;
                    str[2] = PublicDataClass.SaveText.Channel[i].potocol;
                    str[3] = PublicDataClass.SaveText.Channel[i].baud;
                    str[4] = PublicDataClass.SaveText.Channel[i].jy;
                    str[5] = PublicDataClass.SaveText.Channel[i].port;
                    str[6] = PublicDataClass.SaveText.Channel[i].IP;
                    str[7] = PublicDataClass.SaveText.Channel[i].RelayTime;
                    str[8] = "NUlL";
                    rows++;
                    this.dataGridView1.Rows.Add(str);
                    this.dataGridView1.AllowUserToAddRows = true;//设置用户不能手动给 DataGridView1 添加新行
                }
            }
        }
        /// <summary>
        ///  自定义的下拉组合框的鼠标离开事件的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_Temp_MouseLeave(object sender, EventArgs e)
        {
            cmb_Temp.Visible = false;  

        }
        /// <summary>
        /// 自定义的下拉组合框的选择索引改变事件的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_Temp_SelectedIndexChanged(object sender, EventArgs e)                    //下拉组合框的事件响应函数
        {

            if (ColumnsMenu == 1)
            {
                for (byte i = 0; i < PublicDataClass.PortIniInfo.num; i++)
                {
                    if (((ComboBox)sender).Text == PublicDataClass.PortIniInfo.IniTable[i])
                    {
                        dataGridView1.CurrentCell.Value = ((ComboBox)sender).Text;
                        break;
                    }
                }
                cmb_Temp.Visible = false;

            }
            else if (ColumnsMenu == 2)
            {
                for (byte i = 0; i < PublicDataClass.ProtocolIniInfo.num; i++)
                {
                    if (((ComboBox)sender).Text == PublicDataClass.ProtocolIniInfo.IniTable[i])
                    {
                        dataGridView1.CurrentCell.Value = ((ComboBox)sender).Text;
                        break;
                    }
                }
                cmb_Temp.Visible = false;
            }
            else if (ColumnsMenu == 3)
            {
                for (byte i = 0; i < PublicDataClass.BaudRateAJyIniInfo.Bnum; i++)
                {
                    if (((ComboBox)sender).Text == PublicDataClass.BaudRateAJyIniInfo.BIniTable[i])
                    {
                        dataGridView1.CurrentCell.Value = ((ComboBox)sender).Text;
                        break;
                    }
                }
                cmb_Temp.Visible = false;
            }
            else if (ColumnsMenu == 4)
            {
                for (byte i = 0; i < PublicDataClass.BaudRateAJyIniInfo.JYnum; i++)
                {
                    if (((ComboBox)sender).Text == PublicDataClass.BaudRateAJyIniInfo.JIniTable[i])
                    {
                        dataGridView1.CurrentCell.Value = ((ComboBox)sender).Text;
                        break;
                    }
                }
                cmb_Temp.Visible = false;

            }
            else if (ColumnsMenu == 5)
            {
                dataGridView1.CurrentCell.Value = ((ComboBox)sender).Text;
                cmb_Temp.Visible = false;

                if (Convert.ToString(dataGridView1.CurrentCell.Value) == "否")
                    return;
                else
                {
                    if (CheckCurrentCellValidity(this.dataGridView1.CurrentRow.Index) == false)
                    {
                        dataGridView1.CurrentCell.Value = "NULL";
                        return;
                    }
                }

                PublicDataClass.SaveText.Channel = new PublicDataClass.CHANNELINFO[this.dataGridView1.RowCount - 1];
                PublicDataClass.SaveText.channelnum = this.dataGridView1.RowCount - 1;

                for (byte i = 0; i < this.dataGridView1.RowCount - 1; i++)
                {
                    for (byte j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if (j == 1)
                            PublicDataClass.SaveText.Channel[i].ChannelID = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 2)
                            PublicDataClass.SaveText.Channel[i].potocol = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 3)
                            PublicDataClass.SaveText.Channel[i].baud = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 4)
                            PublicDataClass.SaveText.Channel[i].jy = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 5)
                            PublicDataClass.SaveText.Channel[i].port = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 6)
                            PublicDataClass.SaveText.Channel[i].IP = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 7)
                            PublicDataClass.SaveText.Channel[i].RelayTime = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                    }
                }
            }
        }
        /// <summary>
        ///  自定义的函数--检查单元格内容的合法性
        /// </summary>
        /// <param anthor="cuibj"></param>
        /// <param Time="11-04-28"></param>
        private bool CheckCurrentCellValidity(int index)
        {

            if (Convert.ToString(dataGridView1.Rows[index].Cells[1].Value) == "NUlL")           //先判断通道号为空?
            {
                MessageBox.Show("通道号为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else                                                                                //不为空 在判断是串口 网络 还是GPRS
            {
                Regex cm = new Regex("COM"); Regex nt = new Regex("NET"); Regex gs = new Regex("GPRS"); Regex ub = new Regex("USB");
                Match m = cm.Match(Convert.ToString(dataGridView1.Rows[index].Cells[1].Value));
                if (m.Success)//串口
                {
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[3].Value) == "NUlL")  //波特率为空
                    {
                        MessageBox.Show("波特率为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[4].Value) == "NUlL")  //校验为空
                    {
                        MessageBox.Show("校验为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[7].Value) == "NUlL")  //超时为空
                    {
                        MessageBox.Show("超时为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                        return true;
                }
                m = nt.Match(Convert.ToString(dataGridView1.Rows[index].Cells[1].Value));
                if (m.Success)  //网口
                {
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[5].Value) == "NUlL")  //波特率为空
                    {
                        MessageBox.Show("端口号为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[6].Value) == "NUlL")  //校验为空
                    {
                        MessageBox.Show("IP地址为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[7].Value) == "NUlL")  //超时为空
                    {
                        MessageBox.Show("超时为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                        return true;
                }
                m = gs.Match(Convert.ToString(dataGridView1.Rows[index].Cells[1].Value));
                if (m.Success)    //GPRS
                {
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[6].Value) == "NUlL")  //校验为空
                    {
                        MessageBox.Show("IP地址为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[7].Value) == "NUlL")  //超时为空
                    {
                        MessageBox.Show("超时为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                        return true;
                }
                m = ub.Match(Convert.ToString(dataGridView1.Rows[index].Cells[1].Value));
                if (m.Success)    //GPRS
                {
                    /*if (Convert.ToString(dataGridView1.Rows[index].Cells[6].Value) == "NUlL")  //校验为空
                    {
                        MessageBox.Show("IP地址为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[7].Value) == "NUlL")  //超时为空
                    {
                        MessageBox.Show("超时为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                        return true;*/
                }
            }
             return true;

        }
        /// <summary>
        ///  dataGridView1控件的单元格发生改变的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_User_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {

                Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex, false);
                cmb_Temp.Left = rect.Left;
                cmb_Temp.Top = rect.Top;
                cmb_Temp.Width = rect.Width;
                cmb_Temp.Height = rect.Height;
                if (this.dataGridView1.CurrentCell.ColumnIndex == 0)  //索引
                {
                    cmb_Temp.Visible = false;
                    this.dataGridView1.CurrentCell.ReadOnly = true;  //不可编辑

                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 1) //通道
                {
                    if (this.dataGridView1.CurrentCell.Value == null)     //判断当前单元格的内容是否为空
                    {
                        cmb_Temp.Enabled = false;                         //组合框不可用

                    }
                    else
                        cmb_Temp.Enabled = true;
                    ColumnsMenu = 1;
                    cmb_Temp.Items.Clear();
                    foreach (object tmp in PublicDataClass.PortIniInfo.IniTable)
                    {
                        cmb_Temp.Items.Add(tmp);

                    }
                    cmb_Temp.Visible = true;
                }

                else if (this.dataGridView1.CurrentCell.ColumnIndex == 2) //协议
                {
                    if (this.dataGridView1.CurrentCell.Value == null)
                    {
                        cmb_Temp.Enabled = false;

                    }
                    else
                        cmb_Temp.Enabled = true;
                    ColumnsMenu = 2;
                    cmb_Temp.Items.Clear();
                    foreach (object tmp in PublicDataClass.ProtocolIniInfo.IniTable)
                    {
                        cmb_Temp.Items.Add(tmp);

                    }
                    cmb_Temp.Visible = true;
                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 3) //波特率
                {
                    if (this.dataGridView1.CurrentCell.Value == null)
                    {
                        cmb_Temp.Enabled = false;

                    }
                    else
                        cmb_Temp.Enabled = true;
                    ColumnsMenu = 3;
                    cmb_Temp.Items.Clear();
                    foreach (object tmp in PublicDataClass.BaudRateAJyIniInfo.BIniTable)
                    {
                        cmb_Temp.Items.Add(tmp);

                    }
                    cmb_Temp.Visible = true;
                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 4) //jy
                {
                    if (this.dataGridView1.CurrentCell.Value == null)
                    {
                        cmb_Temp.Enabled = false;

                    }
                    else
                        cmb_Temp.Enabled = true;
                    ColumnsMenu = 4;
                    cmb_Temp.Items.Clear();
                    foreach (object tmp in PublicDataClass.BaudRateAJyIniInfo.JIniTable)
                    {
                        cmb_Temp.Items.Add(tmp);

                    }
                    cmb_Temp.Visible = true;
                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 5)  //端口
                {
                    cmb_Temp.Visible = false;
                    if (this.dataGridView1.CurrentCell.Value == null)   //判断当前单元格的内容 ==null的话
                    {
                        this.dataGridView1.CurrentCell.ReadOnly = true;  //不可编辑
                    }
                    else
                        this.dataGridView1.CurrentCell.ReadOnly = false;  //可编辑

                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 6) //IP地址
                {
                    cmb_Temp.Visible = false;
                    if (this.dataGridView1.CurrentCell.Value == null)   //判断当前单元格的内容 ==null的话
                    {
                        this.dataGridView1.CurrentCell.ReadOnly = true;  //不可编辑
                    }
                    else
                        this.dataGridView1.CurrentCell.ReadOnly = false;  //可编辑

                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 7)  //超时间隔
                {
                    cmb_Temp.Visible = false;
                    if (this.dataGridView1.CurrentCell.Value == null)   //判断当前单元格的内容 ==null的话
                    {
                        this.dataGridView1.CurrentCell.ReadOnly = true;  //不可编辑
                    }
                    else
                        this.dataGridView1.CurrentCell.ReadOnly = false;  //可编辑

                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 8) //
                {
                    if (this.dataGridView1.CurrentCell.Value == null)
                    {
                        cmb_Temp.Enabled = false;

                    }
                    else
                        cmb_Temp.Enabled = true;
                    ColumnsMenu = 5;
                    cmb_Temp.Items.Clear();

                    cmb_Temp.Items.Add("是");
                    cmb_Temp.Items.Add("否");
                    cmb_Temp.Visible = true;
                }

                else
                {
                    ColumnsMenu = 0;
                    cmb_Temp.Visible = false;
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// datagridView的鼠标移动事件的处理
        /// </summary>
        /// <param funtion="根据鼠标点击的行头 来显示下拉菜单 实现添加 删除的功能"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void datagridView_MouseDown(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.DataGridView.HitTestInfo hti = dataGridView1.HitTest(e.X, e.Y);
            if ((e.Button == MouseButtons.Right) && (hti.Type == DataGridViewHitTestType.RowHeader))
            {
                ContextMenuStrip contextMenuStrip1 = new ContextMenuStrip();

                contextMenuStrip1.Items.Add("添加");
                contextMenuStrip1.Items.Add("删除");
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);

                contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);

            }
        }
        /// <summary>
        /// 上下文菜单的选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem.Text == "添加")
            {
                //string[] str = { "NUlL", "NUlL", "NUlL", "NUlL", "NUlL", "8888", "192.168.17.12", "NULL" };

                str[0] = String.Format("通道{0:d}", rows);
                str[1] = "NUlL";
                str[2] = "NUlL";
                str[3] = "NUlL";
                str[4] = "NUlL";
                str[5] = "8888";
                str[6] = "192.168.17.12";
                str[7] = "NUlL";
                str[8] = "NUlL";
                rows++;
                this.dataGridView1.Rows.Add(str);
                this.dataGridView1.AllowUserToAddRows = true;//设置用户不能手动给 DataGridView1 添加新行

            }
            if (e.ClickedItem.Text == "删除")
            {
                if (MessageBox.Show("确定要删除此项吗?", "信  息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
                }

            }
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");

            if (!reg1.IsMatch(e.Value.ToString()) && (dgv.Columns[e.ColumnIndex].Name == "Column6"))
            {
                MessageBox.Show("对不起!必须是数字,请重新输入！", "提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Value = "NULL";
                e.ParsingApplied = true;
            }
            if (!reg1.IsMatch(e.Value.ToString()) && (dgv.Columns[e.ColumnIndex].Name == "Column9"))
            {
                MessageBox.Show("对不起!必须是数字,请重新输入！", "提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Value = "NULL";
                e.ParsingApplied = true;
            }
        }
    }
}
