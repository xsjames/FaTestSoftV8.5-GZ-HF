using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace HWLibs.Windows.Forms
{
    /// <summary>
    /// ���޸� DropDownStyle �� DataGridViewComboBoxColumn
    /// </summary>
    public class DataGridViewComboBoxColumnEx : DataGridViewComboBoxColumn
    {
        ComboBoxStyle dropDownStyle;

        /// <summary>
        /// ������Ͽ����ۺ͹���
        /// </summary>
        [Description("������Ͽ����ۺ͹���"), DefaultValue(ComboBoxStyle.DropDownList)]
        public ComboBoxStyle DropDownStyle
        {
            get { return dropDownStyle; }
            set { dropDownStyle = value; }
        }

        public DataGridViewComboBoxColumnEx()
        {
            dropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
