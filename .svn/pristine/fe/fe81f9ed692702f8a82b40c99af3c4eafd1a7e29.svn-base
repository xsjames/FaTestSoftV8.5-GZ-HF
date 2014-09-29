using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FaTestSoft
{
    class OperateExcel
    {
        #region Global variables
        public string mFilename;
        public Microsoft.Office.Interop.Excel.Application app;
        public Microsoft.Office.Interop.Excel.Workbooks   wbs;
        public Microsoft.Office.Interop.Excel.Workbook    wb;
        public Microsoft.Office.Interop.Excel.Worksheets  wss;
        public Microsoft.Office.Interop.Excel.Worksheet   ws;

        #endregion

        #region CreateNewExcel
        /// <summary>
        /// //创建一个Excel对象
        /// </summary>
        public void CreateNewExcel()
        {
            app = new Microsoft.Office.Interop.Excel.Application();
            wbs = app.Workbooks;
            wb  = wbs.Add(true);
            ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[1];
        }
        #endregion

        #region OpenExcel
        /// <summary>
        /// //打开一个Excel文件
        /// </summary>
        /// <param name="FileName"></param>
        public void OpenExcel(string FileName)
        {
            app = new Microsoft.Office.Interop.Excel.Application();
            wbs = app.Workbooks;
            wb  = wbs.Add(FileName);
            //wb = wbs.Open(FileName,  0, true, 5,"", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true,Type.Missing,Type.Missing);
            //wb = wbs.Open(FileName,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Excel.XlPlatform.xlWindows,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing,Type.Missing);
            mFilename = FileName;
        }
        #endregion

        #region GetSheet
        /// <summary>
        /// //获取一个工作表
        /// </summary>
        /// <param name="SheetName"></param>
        /// <returns></returns>
        public Microsoft.Office.Interop.Excel.Worksheet GetSheet(string SheetName)
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[SheetName];
            return s;
        }
        #endregion
        #region GetCellValue
        /// <summary>
        /// //ws：获取的工作表的名称 X行Y列 value 值
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public string GetCellValue(Microsoft.Office.Interop.Excel.Worksheet ws, int x, int y)//格式1
        {
            string value1 = @"";
            Microsoft.Office.Interop.Excel.Range range1;

            range1 = ws.get_Range(ws.Cells[x, y], ws.Cells[x, y]);
            value1 = Convert.ToString(range1.Value2);
            return value1;


        }
        #endregion

        #region GetSheetRows
        /// <summary>
        /// //ws：获取的工作表的名称 行数目
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public int GetSheetRows(Microsoft.Office.Interop.Excel.Worksheet ws)//格式1
        {

            int introwCount = ws.UsedRange.Rows.Count;
            return introwCount;
        }

        #endregion

        #region GetSheetColumns
        /// <summary>
        /// //ws：获取的工作表的名称 列数目
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public int GetSheetColumns(Microsoft.Office.Interop.Excel.Worksheet ws)//格式1
        {
            int intcolCount = ws.UsedRange.Columns.Count;

            return intcolCount;
        }

        #endregion

        #region AddSheet
        /// <summary>
        /// //添加一个工作表
        /// </summary>
        /// <param name="SheetName"></param>
        /// <returns></returns>
        public Microsoft.Office.Interop.Excel.Worksheet AddSheet(string SheetName)
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            s.Name = SheetName;
            return s;
        }
        #endregion

        #region DelSheet
        /// <summary>
        /// //删除一个工作表
        /// </summary>
        /// <param name="SheetName"></param>
        public void DelSheet(string SheetName)
        {
            ((Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[SheetName]).Delete();
        }
        #endregion

        #region ReNameSheet
        /// <summary>
        /// //重命名一个工作表一
        /// </summary>
        /// <param name="OldSheetName"></param>
        /// <param name="NewSheetName"></param>
        /// <returns></returns>
        public Microsoft.Office.Interop.Excel.Worksheet ReNameSheet(string OldSheetName, string NewSheetName)
        {
            Microsoft.Office.Interop.Excel.Worksheet s = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets[OldSheetName];
            s.Name = NewSheetName;
            return s;
        }
        #endregion

        #region SetCellValue
        /// <summary>
        /// //ws：要设值的工作表的名称 X行Y列 value 值
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="value"></param>
        public void SetCellValue(Microsoft.Office.Interop.Excel.Worksheet ws, int x, int y, object value)//格式1
        {
            ws.Cells[x, y] = value;
        }
        public void SetCellValue(string ws, int x, int y, object value)//格式2
        {

            GetSheet(ws).Cells[x, y] = value;
        }
        #endregion

        #region SetCellProperty

        /// <summary>
        /// //设置一个单元格的属性   字体，   大小，颜色   ，对齐方式
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="Startx"></param>
        /// <param name="Starty"></param>
        /// <param name="Endx"></param>
        /// <param name="Endy"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <param name="HorizontalAlignment"></param>
        public void SetCellProperty(Microsoft.Office.Interop.Excel.Worksheet ws, int Startx, int Starty, int Endx, int Endy, int size, string name, Microsoft.Office.Interop.Excel.Constants color, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment)//格式1
        {
            name = "宋体";
            size = 12;
            color = Microsoft.Office.Interop.Excel.Constants.xlAutomatic;
            HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }

        public void SetCellProperty(string wsn, int Startx, int Starty, int Endx, int Endy, int size, string name, Microsoft.Office.Interop.Excel.Constants color, Microsoft.Office.Interop.Excel.Constants HorizontalAlignment) //格式2
        {
            Microsoft.Office.Interop.Excel.Worksheet ws = GetSheet(wsn);
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Name = name;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Size = size;
            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).Font.Color = color;

            ws.get_Range(ws.Cells[Startx, Starty], ws.Cells[Endx, Endy]).HorizontalAlignment = HorizontalAlignment;
        }
        #endregion

        #region UniteCells
        /// <summary>
        /// //合并单元格
        /// </summary>
        /// <param name="ws"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public void UniteCells(Microsoft.Office.Interop.Excel.Worksheet ws, int x1, int y1, int x2, int y2)//格式1
        {
            ws.get_Range(ws.Cells[x1, y1], ws.Cells[x2, y2]).Merge(Type.Missing);
        }

        public void UniteCells(string ws, int x1, int y1, int x2, int y2)//格式2
        {
            GetSheet(ws).get_Range(GetSheet(ws).Cells[x1, y1], GetSheet(ws).Cells[x2, y2]).Merge(Type.Missing);

        }

        #endregion

        #region InsertTable
        /// <summary>
        /// 将内存中数据表格插入到Excel指定工作表的指定位置 为在使用模板时控制格式时
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        public void InsertTable(System.Data.DataTable dt, string ws, int startX, int startY)//使用一
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {
                    GetSheet(ws).Cells[startX + i, j + startY] = dt.Rows[i][j].ToString();

                }

            }

        }
        public void InsertTable(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet ws, int startX, int startY)//格式二
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    ws.Cells[startX + i, j + startY] = dt.Rows[i][j];

                }

            }

        }
        #endregion

        #region AddTable
        /// <summary>
        /// //将内存中数据表格添加到Excel指定工作表的指定位置一
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ws"></param>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        public void AddTable(System.Data.DataTable dt, string ws, int startX, int startY)//格式1
        {

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    GetSheet(ws).Cells[i + startX, j + startY] = dt.Rows[i][j];

                }

            }

        }
        public void AddTable(System.Data.DataTable dt, Microsoft.Office.Interop.Excel.Worksheet ws, int startX, int startY)//格式2
        {


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= dt.Columns.Count - 1; j++)
                {

                    ws.Cells[i + startX, j + startY] = dt.Rows[i][j];

                }
            }

        }
        #endregion

        #region InsertPictures
        /// <summary>
        ///  //插入图片操作一
        /// </summary>
        /// <param name="Filename"></param>
        /// <param name="ws"></param>
        public void InsertPictures(string Filename, string ws)
        {
            //GetSheet(ws).Shapes.AddPicture(Filename, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, 10, 10, 150, 150);//后面的数字表示位置
        }
        #endregion
        
        #region SaveAsExcel
        /// <summary>
        /// //文档另存为
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool SaveAsExcel(object FileName)
        {
            try
            {
                wb.SaveAs(FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                return true;

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }
        }
        #endregion

        #region CloseExcel
        /// <summary>
        /// 关闭一个Excel对象，销毁对象
        /// </summary>
        public void CloseExcel()
        {
            
            wb.Close(Type.Missing, Type.Missing, Type.Missing);
            wbs.Close();
            app.Quit();
            wb  = null;
            wbs = null;
            app = null;
            GC.Collect();//清理内存资源
        }
        #endregion
    }
}
