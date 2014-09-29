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
    public partial class CePointClassView : Form
    {
        public CePointClassView()
        {
            InitializeComponent();
        }
        private ComboBox cmb_Temp = new ComboBox();                                                       //建立一个组合框的对象

        private static int ColumnsMenu = 0;
        string[] str = new string[5];
        private static byte rows = 0;

        private void CePointClassView_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToResizeColumns = false;                                              //不允许改变列宽
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;  //行头不允许改变大小

            cmb_Temp.Enabled = false;                                                                     //组合框控件不允许

            cmb_Temp.DropDownStyle = ComboBoxStyle.DropDownList;                                          //设置组合框的样式 --下拉
            cmb_Temp.MouseLeave += new System.EventHandler(cmb_Temp_MouseLeave);
            cmb_Temp.SelectedIndexChanged += new EventHandler(cmb_Temp_SelectedIndexChanged);             //添加组合框的事件
            cmb_Temp.Visible = false;                                                                     //隐藏组合框 不可见
            this.dataGridView1.Controls.Add(cmb_Temp);                                                    //向datagridview中加入
            if (PublicDataClass.PrjType == 2)
            {
                for (byte i = 0; i < PublicDataClass.SaveText.devicenum; i++)
                {
                    str[0] = String.Format("{0:d}", i);
                    str[1] = PublicDataClass.SaveText.Device[i].PointName;
                    str[2] = PublicDataClass.SaveText.Device[i].ChannelID;
                    str[3] = PublicDataClass.SaveText.Device[i].Addr;
                    str[4] = "NUlL";
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
        private void cmb_Temp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColumnsMenu == 1)
            {
                for (byte i = 0; i < PublicDataClass.SaveText.channelnum; i++)
                {
                    if (((ComboBox)sender).Text == PublicDataClass.SaveText.Channel[i].ChannelID)
                    {
                        dataGridView1.CurrentCell.Value = ((ComboBox)sender).Text;
                        break;
                    }
                }
                cmb_Temp.Visible = false;

            }
            else if (ColumnsMenu == 2)
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

                PublicDataClass.SaveText.Device = new PublicDataClass.DEVICEINFO[this.dataGridView1.RowCount - 1];
                PublicDataClass.SaveText.devicenum = this.dataGridView1.RowCount - 1;
                PublicDataClass.SaveText.cfgnum = PublicDataClass.SaveText.devicenum;  //配置了一个测量点
                for (byte i = 0; i < this.dataGridView1.RowCount - 1; i++)
                {
                    for (byte j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        if (j == 1)
                            PublicDataClass.SaveText.Device[i].PointName = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 2)
                            PublicDataClass.SaveText.Device[i].ChannelID = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 3)
                            PublicDataClass.SaveText.Device[i].Addr = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);
                        else if (j == 4)
                            PublicDataClass.SaveText.Device[i].State = Convert.ToString(dataGridView1.Rows[i].Cells[j].Value);

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

            if (Convert.ToString(dataGridView1.Rows[index].Cells[1].Value) == "NUlL")           //先判断测量点为空?
            {
                MessageBox.Show("测量点为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else                                                                                //不为空 在判断其它项
            {
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[2].Value) == "NUlL")  //所在通道为空
                    {
                        MessageBox.Show("通道选择为null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (Convert.ToString(dataGridView1.Rows[index].Cells[3].Value) == "NUlL")  //地址为空
                    {
                        MessageBox.Show("地址null！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                        return true;
            }

        }
        /// <summary>
        /// dataGridView1 鼠标的按下事件的处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
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
        /// 自定义的菜单下拉的选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "添加")
            {

                str[0] = String.Format("{0:d}", rows);
                str[1] = "NUlL";
                str[2] = "NUlL";
                str[3] = "NUlL";
                str[4] = "NUlL";

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
        /// <summary>
        ///  dataGridView1控件的单元格发生改变的事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex, dataGridView1.CurrentCell.RowIndex, false);
                cmb_Temp.Left = rect.Left;
                cmb_Temp.Top = rect.Top;
                cmb_Temp.Width = rect.Width;
                cmb_Temp.Height = rect.Height;
                if (this.dataGridView1.CurrentCell.ColumnIndex == 0)
                {

                    this.dataGridView1.CurrentCell.ReadOnly = true;  //不可编辑

                }

                else if (this.dataGridView1.CurrentCell.ColumnIndex == 1)  //测量点名称
                {

                    cmb_Temp.Visible = false;
                    if (this.dataGridView1.CurrentCell.Value == null)   //判断当前单元格的内容 ==null的话
                    {
                        this.dataGridView1.CurrentCell.ReadOnly = true;  //不可编辑
                    }
                    else
                        this.dataGridView1.CurrentCell.ReadOnly = false;  //可编辑

                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 2) //所在通道号  从通道类节目配置中选择
                {
                    if (this.dataGridView1.CurrentCell.Value == null)
                    {
                        cmb_Temp.Enabled = false;                       //组合框禁用

                    }
                    else
                        cmb_Temp.Enabled = true;
                    ColumnsMenu = 1;
                    cmb_Temp.Items.Clear();
                    for (byte i = 0; i < PublicDataClass.SaveText.channelnum; i++)
                    {
                        cmb_Temp.Items.Add(PublicDataClass.SaveText.Channel[i].ChannelID);//从配置表中选择


                    }
                    cmb_Temp.Visible = true;
                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 3) //地址
                {
                    cmb_Temp.Visible = false;
                    if (this.dataGridView1.CurrentCell.Value == null)   //判断当前单元格的内容 ==null的话
                    {
                        this.dataGridView1.CurrentCell.ReadOnly = true;  //不可编辑
                    }
                    else
                        this.dataGridView1.CurrentCell.ReadOnly = false;  //可编辑
                }
                else if (this.dataGridView1.CurrentCell.ColumnIndex == 4) //使用标记
                {
                    if (this.dataGridView1.CurrentCell.Value == null)
                    {
                        cmb_Temp.Enabled = false;

                    }
                    else
                        cmb_Temp.Enabled = true;
                    ColumnsMenu = 2;
                    cmb_Temp.Items.Clear();

                    cmb_Temp.Items.Add("是");
                    cmb_Temp.Items.Add("否");
                    cmb_Temp.Visible = true;
                }
              
            }
            catch
            {

            }
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");

            if (!reg1.IsMatch(e.Value.ToString()) && (dgv.Columns[e.ColumnIndex].Name == "Column4"))
            {
                MessageBox.Show("对不起!必须是数字,请重新输入！", "提示",MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Value = "NULL";
                e.ParsingApplied = true;
            }
        }

    }
}
