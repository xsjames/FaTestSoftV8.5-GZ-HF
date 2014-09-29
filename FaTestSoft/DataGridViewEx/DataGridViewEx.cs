using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace HWLibs.Windows.Forms
{
    /// <summary>
    /// ��չ�� DataGridView
    /// </summary>
    public class DataGridViewEx : DataGridView
    {
        bool showRowHeaderNumbers;

        /// <summary>
        /// �Ƿ���ʾ�к�
        /// </summary>
        [Description("�Ƿ���ʾ�к�"), DefaultValue(true)]
        public bool ShowRowHeaderNumbers
        {
            get { return showRowHeaderNumbers; }
            set 
            {
                if (showRowHeaderNumbers != value)
                    Invalidate();
                showRowHeaderNumbers = value; 
            }
        }

        public DataGridViewEx()
        {
            showRowHeaderNumbers = true;
            RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridViewEx_RowPostPaint);
        }

        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            if (CurrentCell != null && CurrentCell.OwningColumn is DataGridViewComboBoxColumnEx)
            {
                DataGridViewComboBoxColumnEx col = CurrentCell.OwningColumn as DataGridViewComboBoxColumnEx;
                //�޸���Ͽ����ʽ
                if (col.DropDownStyle != ComboBoxStyle.DropDownList)
                {
                    ComboBox combo = e.Control as ComboBox;
                    combo.DropDownStyle = col.DropDownStyle;
                    combo.Leave += new EventHandler(combo_Leave);
                }
            }
            base.OnEditingControlShowing(e);
        }

        /// <summary>
        /// �������뿪ʱ����Ҫ���������ֵ���뵽��Ͽ�� Items �б���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void combo_Leave(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            combo.Leave -= new EventHandler(combo_Leave);
            if (CurrentCell != null && CurrentCell.OwningColumn is DataGridViewComboBoxColumnEx)
            {
                DataGridViewComboBoxColumnEx col = CurrentCell.OwningColumn as DataGridViewComboBoxColumnEx;
                //һ��Ҫ���������ֵ���뵽��Ͽ��ֵ�б���
                //������һ������Ԫ��ֵ��ʱ��ᱨ����Ϊֵ������Ͽ��ֵ�б��У�
                col.Items.Add(combo.Text);
                CurrentCell.Value = combo.Text;
            }
        }

        void DataGridViewEx_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (showRowHeaderNumbers)
            {
                string title = (e.RowIndex + 1).ToString();
                Brush bru = Brushes.Black;
                e.Graphics.DrawString(title, DefaultCellStyle.Font,
                    bru, e.RowBounds.Location.X + RowHeadersWidth / 2 - 4, e.RowBounds.Location.Y + 4);
            }
        }
    }
}
