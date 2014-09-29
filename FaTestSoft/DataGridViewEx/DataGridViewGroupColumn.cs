using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HWLibs.Windows.Forms
{
	/// <summary>
	/// �ɷ������ (���б����� Grid �ĵ�һ��)
	/// </summary>
	public class DataGridViewGroupColumn : DataGridViewTextBoxColumn
	{
		public DataGridViewGroupColumn()
		{
			CellTemplate = new DataGridViewGroupCell();
			ReadOnly = true;
		}

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if ((value != null) && !(value is DataGridViewGroupCell))
                {
                    throw new InvalidCastException("Need System.Windows.Forms.DataGridViewGroupCell");
                }
                base.CellTemplate = value;
            }

        }
	}

	/// <summary>
	/// �ɷ���ĵ�Ԫ��
	/// </summary>
	public class DataGridViewGroupCell : DataGridViewTextBoxCell
	{
		#region Variant

		/// <summary>
		/// ��ʾ�Ŀ��
		/// </summary>
		const int PLUS_WIDTH = 24;

		/// <summary>
		/// ��ʾ������
		/// </summary>
		Rectangle groupPlusRect;

		#endregion

		#region Init

		public DataGridViewGroupCell()
		{
			groupLevel = 1;
		}

		#endregion

		#region Property

		int groupLevel;

		/// <summary>
		/// �鼶��(��1��ʼ)
		/// </summary>
		public int GroupLevel
		{
			get { return groupLevel; }
			set { groupLevel = value; }
		}

		DataGridViewGroupCell parentCell;

		/// <summary>
		/// ���ڵ�
		/// </summary>
		public DataGridViewGroupCell ParentCell
		{
			get
			{
				return parentCell;
			}
			//set
			//{
			//    if (value == null)
			//        throw new NullReferenceException("���ڵ㲻��Ϊ��");
			//    if (!(value is DataGridViewGroupCell))
			//        throw new ArgumentException("���ڵ����Ϊ DataGridViewGroupCell ����");

			//    parentCell = value;
			//    parentCell.AddChildCell(this);				
			//}
		}

		private bool collapsed;

		/// <summary>
		/// �Ƿ�����
		/// </summary>
		public bool Collapsed
		{
			get { return collapsed; }
		}

		private List<DataGridViewGroupCell> childCells = null;

		/// <summary>
		/// ���е��ӽ��
		/// </summary>
		public DataGridViewGroupCell[] ChildCells
		{
			get 
			{
				if (childCells == null)
					return null;
				return childCells.ToArray(); 
			}
		}

		/// <summary>
		/// ȡ�����ʾ(��+��-�ŵĿ�)������
		/// </summary>
		public Rectangle GroupPlusRect
		{
			get
			{
				return groupPlusRect;
			}
		}

        bool bPaint = true;

        /// <summary>
        /// �Ƿ��ػ�
        /// </summary>
        public bool BPaint
        {
            get { return bPaint; }
            set { bPaint = value; }
        }

		#endregion

		#region ����ӽڵ�

		/// <summary>
		/// ����ӽ��
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		public int AddChildCell(DataGridViewGroupCell cell)
		{
            return AddChildCellRange(new DataGridViewGroupCell[] { cell });
		}

        public int AddChildCellRange(DataGridViewGroupCell[] cells)
        {
            bool needRedraw = false;
            if (childCells == null)
            {
                //��Ҫ��һ���Ӻ�
                childCells = new List<DataGridViewGroupCell>();
                needRedraw = true;
            }
            foreach (DataGridViewGroupCell  cell in cells)
            {
                childCells.Add(cell);
                cell.groupLevel = groupLevel + 1;
            }

            if (needRedraw)
                DataGridView.InvalidateCell(this);
            return childCells.Count;
        }

		#endregion

		#region ���ƽڵ�

		protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
		{
            if (!bPaint)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                return;
            }
			Pen gridPen = new Pen(DataGridView.GridColor);
			Brush bruBK = new SolidBrush(cellStyle.BackColor);
			int width = groupLevel * PLUS_WIDTH;
			Rectangle rectLeft = new Rectangle(cellBounds.Left, cellBounds.Top-1, width, cellBounds.Height);
			cellBounds.X += width;
			cellBounds.Width -= width;
			base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            PaintGroupPlus(graphics, gridPen, bruBK, cellStyle, rectLeft, collapsed);

			gridPen.Dispose();
			gridPen = null;

			bruBK.Dispose();
			bruBK = null;
		}

		private void PaintGroupPlus(Graphics graphics, Pen gridPen, Brush bruBK, DataGridViewCellStyle cellStyle, Rectangle rectLeft, bool collapsed)
		{
			graphics.FillRectangle(bruBK, rectLeft);
			int left = rectLeft.Left + (groupLevel-1) * PLUS_WIDTH;
			//�� Left, Top, Right ������
			graphics.DrawLine(gridPen, left, rectLeft.Top, left, rectLeft.Bottom);
			graphics.DrawLine(gridPen, left, rectLeft.Top, rectLeft.Right, rectLeft.Top);
			graphics.DrawLine(gridPen, rectLeft.Right, rectLeft.Top, rectLeft.Right, rectLeft.Bottom);

            //����ߵ�һ����
            graphics.DrawLine(gridPen, rectLeft.Left, rectLeft.Top, rectLeft.Left, rectLeft.Bottom);

			//����Ǹü�������һ���ڵ㣬����Ҫ��һ�����ߣ��Ա㽫������������
			bool drawBottomLine = false;
			for (int i = 1; i < groupLevel; i++)
			{
				graphics.DrawLine(gridPen, rectLeft.Left + i * PLUS_WIDTH, rectLeft.Top
				, rectLeft.Left + i * PLUS_WIDTH, rectLeft.Bottom);

				if (!drawBottomLine && IsLastCell(i))
				{
					graphics.DrawLine(gridPen, rectLeft.Left + (i - 1) * PLUS_WIDTH, rectLeft.Bottom
					, rectLeft.Left + groupLevel * PLUS_WIDTH, rectLeft.Bottom);
					drawBottomLine = true;
				}
			}
			
			//������ӽ�㣬 ����Ҫ��һ������, ������+�Ż�-��
			if (childCells != null && childCells.Count > 0)
			{
				groupPlusRect = new Rectangle((groupLevel - 1) * PLUS_WIDTH + rectLeft.Left + (PLUS_WIDTH - 12) / 2
					, rectLeft.Top + (rectLeft.Height - 12) / 2, 12, 12);
				graphics.DrawRectangle(gridPen, groupPlusRect);

				graphics.DrawLine(Pens.Black, groupPlusRect.Left + 3, groupPlusRect.Top + groupPlusRect.Height / 2
				, groupPlusRect.Right - 3, groupPlusRect.Top + groupPlusRect.Height / 2);
				if (collapsed)
				{
					graphics.DrawLine(Pens.Black, groupPlusRect.Left + groupPlusRect.Width / 2, groupPlusRect.Top + 3
				, groupPlusRect.Left + groupPlusRect.Width / 2, groupPlusRect.Bottom - 3);
				}
			}
		}

		#endregion

		#region �ж�

		/// <summary>
		/// �ýڵ��Ƿ�Ϊĳһ���ڵ�����һ���ӽ��
		/// </summary>
		/// <param name="level">�ڵ�㼶</param>
		/// <returns></returns>
		private bool IsLastCell(int level)
		{
			int row = DataGridView.Rows.GetNextRow(RowIndex, DataGridViewElementStates.None);
			if (row == -1)
				return true;
			DataGridViewGroupCell cel = DataGridView.Rows[row].Cells[0] as DataGridViewGroupCell;
			return (cel.GroupLevel == level);
		}

		#endregion

		#region ��� Cell

		protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
		{
			Rectangle rect = DataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, false);
			Point pt = new Point(rect.Left + e.Location.X, rect.Top + e.Location.Y);
			if (groupPlusRect.Contains(pt))
			{
				if (collapsed)
				{
					Expand();
				}
				else
				{
					Collapse();
				}
			}
			base.OnMouseDown(e);
		}

		#endregion

		#region չ��/����ڵ�

		/// <summary>
		/// չ���ڵ�
		/// </summary>
		public void Expand()
		{
			collapsed = false;
			ShowChild(true);
			base.DataGridView.InvalidateCell(this);
		}

		private void ShowChild(bool show)
		{
			if (childCells == null)
				return;
			foreach (DataGridViewGroupCell cel in childCells)
			{
                if (cel.RowIndex == -1)
                {
                    continue;
                }
				DataGridView.Rows[cel.RowIndex].Visible = show;
				if (!cel.collapsed)
					cel.ShowChild(show);
			}
		}

		/// <summary>
		/// ����ڵ�
		/// </summary>
		public void Collapse()
		{
			collapsed = true;
			ShowChild(false);
			base.DataGridView.InvalidateCell(this);
		}

		/// <summary>
		/// չ���ڵ㼰�ӽ��
		/// </summary>
		public void ExpandAll()
		{
			if (childCells == null)
				return;
			foreach (DataGridViewGroupCell cel in childCells)
			{
				cel.Expand();
				cel.ExpandAll();
			}
		}

		#endregion

	}
}
