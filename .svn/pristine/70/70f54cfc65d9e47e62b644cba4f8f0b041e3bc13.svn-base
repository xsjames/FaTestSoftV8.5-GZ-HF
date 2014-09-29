using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FaTestSoft
{
    class Page
    {
        private int RowsPerPage = 5;//表示ItemsPerPage属性的默认值
        private DataSet TempSet = new DataSet();//定义一个存储数据的对象
        private int CurrentPage = 0;//表示当前处于第一页

        /// <summary>
        /// 表示每页显示多少条记录
        /// </summary>
        public int ItemsPerPage
        {
            get
            {
                return RowsPerPage;//返回当前页显示的多少数据
            }
            set
            {
                RowsPerPage = value;//设置当前页显示的数据数目
            }
        }
        public void SetDataSet(DataSet dataSet, out DataTable dataTable)
        {
            TempSet = dataSet;//向数据集中填充内容
            GoToPageNumber(1, out dataTable);//跳转到第一页
        }
        /// <summary>
        /// 跳转到某一页，如果该页不存在则什么也不做
        /// </summary>
        /// <param name="n">当前显示的页数</param>
        #region GoToPageNumber
        public void GoToPageNumber(int n, out DataTable pageTable)
        {
            DataSet TempSubSet = new DataSet();//初始化一个数据集对象
            DataRow[] Rows = new DataRow[RowsPerPage];//定义一个存储数据行的数组
            int AllPages = 0;//该变量表示所有页数
            AllPages = GetTotalPages();//为AllPages变量赋值

            if ((n > 0) && (n <= AllPages))//当变量n处于有效值范围内时
            {
                int PageIndex = (n - 1) * RowsPerPage;//设置页索引的值
                if (PageIndex >= TempSet.Tables[0].Rows.Count)//当页索引的值大于等于数据集中的所有记录总数时
                {
                    GoToFirstPage(out pageTable);//返回到第一页
                }
                else//当页索引的值小于数据集中的所有记录总数时
                {
                    int WholePages = TempSet.Tables[0].Rows.Count / RowsPerPage;//记录当前数据集按指定的分页方式分为多少页
                    if ((TempSet.Tables[0].Rows.Count % RowsPerPage) != 0 && n == AllPages)//当变量n为总页数且有些页的数据不足时
                    {
                        Rows = new DataRow[TempSet.Tables[0].Rows.Count - (WholePages * RowsPerPage)];//表示不足一页的记录数
                    }
                    for (int i = 0, j = PageIndex; i < Rows.Length && j < TempSet.Tables[0].Rows.Count; j++, i++)//循环遍历数据集中每一条记录
                    {
                        Rows[i] = TempSet.Tables[0].Rows[j];//为数组Rows赋值
                    }
                    TempSubSet.Merge(Rows);//将不足一页的数据附加到数据集中

                    CurrentPage = n;//设定当前处于第几页
                    pageTable = TempSubSet.Tables[0];//为pageTable赋值
                }
            }
            else//当变量n处于无效数据范围内时
            {
                pageTable = null;//清空数据表中的内容
            }
        }
        #endregion

        /// <summary>
        /// 该方法用来获取数据集分页后的页的总数
        /// </summary>
        /// <returns>返回当前数据集中按指定的显示方式所具有的页数 </returns>
        public int GetTotalPages()
        {
            if ((TempSet.Tables[0].Rows.Count % RowsPerPage) != 0)//当数据表中的行数除以每页显示的行数的余数不为0时
            {
                int x = TempSet.Tables[0].Rows.Count / RowsPerPage;//记录数据表中的所有行数除以每页显示的数据数
                return (x + 1);//返回该数据集所包含的所有页数
            }
            else//当数据表中的行数刚好能正处每页显示的页数时
            {
                return TempSet.Tables[0].Rows.Count / RowsPerPage;//返回两者相除后的值
            }
        }

        /// <summary>
        /// 跳转到最后一页
        /// </summary>
        #region GoToLastPage
        public void GoToLastPage(out DataTable pageTable)
        {
            GoToPageNumber(GetTotalPages(), out pageTable);//跳转到最后一页
        }
        #endregion

        /// <summary>
        /// 显示当前页的下一页记录，如果该页不存在，则不做任何操作
        /// </summary>
        #region GoToNextPage
        public void GoToNextPage(out DataTable pageTable)
        {
            GoToPageNumber(CurrentPage + 1, out pageTable);//跳转到当前页的下一页
        }
        #endregion

        /// <summary>
        /// Displays the first page.
        /// </summary>
        #region GoToFirstPage
        public void GoToFirstPage(out DataTable pageTable)
        {
            pageTable = null;//清空数据表中pageTable的记录
            DataSet TempSubSet = new DataSet();//初始化一个存储数据的数据集
            DataRow[] Rows = new DataRow[RowsPerPage];//声明一个DataRow数组

            if (TempSet.Tables[0].Rows.Count < RowsPerPage && CurrentPage != 1)//如果数据集中的记录总数不足一行则在第一行中显示全部
            {
                Rows = new DataRow[TempSet.Tables[0].Rows.Count];//重新定义DataRow数组的长度
                for (int i = 0; i < TempSet.Tables[0].Rows.Count; i++)//循环遍历数据集中的每一条记录
                {
                    Rows[i] = TempSet.Tables[0].Rows[i];//为DataRow数组赋值
                }
                TempSubSet.Merge(Rows);//将当前的数据记录添加进数据集
                pageTable = TempSubSet.Tables[0];//设定当前数据表的内容
            }

            if (TempSet.Tables[0].Rows.Count >= RowsPerPage && CurrentPage != 1)//当数据集中的数据记录大于每页显示记录数时
            {
                for (int i = 0; i < RowsPerPage; i++)//循环遍历每页显示的数据数
                {
                    Rows[i] = TempSet.Tables[0].Rows[i];//为DataRow数组赋值
                }
                TempSubSet.Merge(Rows);//将当前数据添加进数据集
                pageTable = TempSubSet.Tables[0];//设定当前数据表中的内容
            }
            CurrentPage = 1;//设定当前处于第1页

        }
        #endregion

        /// <summary>
        /// 显示上一条记录
        /// </summary>
        #region GoToPreviousPage
        public void GoToPreviousPage(out DataTable pageTable)
        {

            if (CurrentPage != 1)//当当前页没有处于第1页时
            {
                GoToPageNumber(CurrentPage - 1, out pageTable);//返回当前页的上一页

            }
            else//当处于第1页时
            {
                pageTable = null;//清空数据表中的内容
            }
        }
        #endregion
        /// <summary>
        /// 查询页
        /// </summary>
        /// <param name="pageTable"></param>
        #region GoToSeachPage
        public void GoToSeachPage(int cpage,out DataTable pageTable)
        {
            CurrentPage = cpage;
            GoToPageNumber(CurrentPage, out pageTable);//返回当前页的上一页
        }
        #endregion
    }
}
