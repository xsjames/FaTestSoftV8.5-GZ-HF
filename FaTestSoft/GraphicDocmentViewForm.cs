using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KD.WinFormsUI.Docking;
using System.IO;
using FaTestSoft.CommonData;
using ZedGraph;
using System.Data.OleDb;
using System.Xml;
namespace FaTestSoft
{
    public partial class GraphicDocmentViewForm : DockContent
    {
        public GraphicDocmentViewForm()
        {
            InitializeComponent();
        }

        ToolBarManager _toolBarManager;

        //进行操作的位图
        private Image MyImg;
        //绘制位图实例对象
        private Graphics graphics;
        //绘图使用颜色
        private Color foreColor = Color.Black;
        private Color backColor = Color.White;

        LineItem[] curvemy ;
        LineItem hiscurvemy;
        PointPairList listemp = new PointPairList();
        private Color[] lineColor = new Color[10] { Color.Red, Color.Pink, Color.Gray, Color.Blue, Color.Purple, Color.SeaGreen, Color.SkyBlue, Color.SteelBlue, Color.PowderBlue, Color .RosyBrown};


        private void GraphicDocmentViewForm_Load(object sender, EventArgs e)
        {
            //Rectangle rect = pictureBox1.ClientRectangle;

            _toolBarManager = new ToolBarManager(this, this);
            _toolBarManager.AddControl(toolBar1, DockStyle.Top);
            _toolBarManager.AddControl(toolStrip1, DockStyle.Top);
            
     

            //this.zedGraphControl1.BackColor
            this.zedGraphControl1.GraphPane.Title.Text = "遥测值";
            this.zedGraphControl1.GraphPane.XAxis.Title.Text = "时间";
            this.zedGraphControl1.GraphPane.YAxis.Title.Text = "数值";
            this.zedGraphControl1.GraphPane.XAxis.Type = ZedGraph.AxisType.DateAsOrdinal;
            this.zedGraphControl1.GraphPane.XAxis.Scale.Format = "HH:mm:ss";
            this.zedGraphControl1.IsShowPointValues = true;
            this.zedGraphControl1.PointDateFormat = "HH:mm:ss";
            //this.zedGraphControl1.

            for (int j = 0; j < PublicDataClass.SaveText.Cfg[0].YccfgNum; j++)
            {
                comboBox1.Items.Add(16385+j);
            }
            comboBox1.SelectedIndex = 0;
            curvemy = new LineItem[PublicDataClass.SaveText.Cfg[0].YccfgNum];

                radioButton1.Checked = true;
                textBox1.Enabled = false;
                dateTimePickerbegindata.Enabled = false;
                dateTimePickerbegintime.Enabled = false;
                dateTimePickerenddata.Enabled = false;
                dateTimePickerendtime.Enabled = false;
            
        }

        private void GraphicDocmentViewForm_Activated(object sender, EventArgs e)
        {
            //timer1.Enabled = true;
        }

        private void GraphicDocmentViewForm_Deactivate(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            //this.zedGraphControl1.GraphPane.CurveList.Clear();
            int pos = Convert.ToInt16(comboBox1.Text);
            if (radioButton1.Checked == true)
            {//if (curvemy[pos - 16385] == null)
                //{
                curvemy[pos - 16385] = zedGraphControl1.GraphPane.AddCurve(PublicDataClass.SaveText.Cfg[0].YccfgName[pos - 16385], PublicDataClass._Curve.listemp[pos - 16385], lineColor[(pos - 16385) % 10], SymbolType.None);
                //    curvemy[pos - 16385].IsVisible = true;
                //}
                //else
                //    curvemy[pos - 16385].IsVisible = true;
                //curvemy[pos - 16385].Line.IsSmooth = true;
                this.zedGraphControl1.AxisChange();
                this.zedGraphControl1.Refresh();
                timer1.Enabled = true;
            }
            else
            {
                timer1.Enabled = false;
                int interval = Convert.ToInt32(textBox1.Text);
                TimeSpan tsan = dateTimePickerendtime.Value.TimeOfDay-dateTimePickerbegintime.Value.TimeOfDay;
                int n = tsan.Hours * 3600 + tsan.Minutes * 60 + tsan.Seconds;

             //   string datapath = PublicDataClass.PrjPath  + "\\HistoryDate" + "\\" + dateTimePickerbegindata.Value.ToShortDateString() + ".xml";
                string datapath = PublicDataClass.PrjPath + "\\HistoryDate"+"\\"+"data.xml";
              //  string datapath = PublicDataClass.PrjPath + "\\DATA" + "\\HistoryCurve" + "\\" +dateTimePickerbegindata.Value.ToShortDateString() + ".cur";
                //string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Jet OLEDB:DataBase Password='csgcsg ';User Id=admin;Data source='" + datapath + "'";
             if (!File.Exists(datapath))
                {
                   MessageBox.Show("无该天数据记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    //lhc xml格式历史数据文件操作部分代码
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(datapath);

                    XmlNode root = xmlDoc.DocumentElement;
                    XmlNodeList xnf = root.ChildNodes;
                //    XmlNode nodex, nodey;
                    double x, y;
                    int i = 0;
                    foreach (XmlNode xn in xnf)
                    {
                        i++;
                        if (i != 1)
                        {
                            //nodex = xmlDoc.SelectSingleNode(" /HistoryDate/Node[" + Convert.ToString(i) + "]/x");
                            //nodey = xmlDoc.SelectSingleNode(" /HistoryDate/Node[" + Convert.ToString(i) + "]/y");
                            XmlElement xe = (XmlElement)xn;
                            string a = xe.GetAttribute("time");
                            // x = Convert.ToDouble(Convert.ToDateTime (xe.GetAttribute ("time")) );
                            x = (double)new XDate(Convert.ToDateTime(xe.GetAttribute("time")));

                            y = Convert.ToDouble(xe.InnerText);
                            listemp.Add(x, y);
                        }
                    }
            

                    //zxl cur格式历史数据文件操作部分代码
                    //string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data source='" + datapath + "'";
                    //OleDbConnection oleCon = new OleDbConnection(ConStr);
                    //oleCon.Open();

                    //using (OleDbCommand cmd = new OleDbCommand())
                    //{
                    //    listemp.Clear();
                    //    int t = dateTimePickerbegintime.Value.Hour * 3600 + dateTimePickerbegintime.Value.Minute * 60 + dateTimePickerbegintime.Value.Second;
                    //    //string stastr = "10:12";              //test
                    //    cmd.CommandText = "select * from 曲线";
                    //    cmd.Connection = oleCon;
                    //    OleDbDataReader dr = cmd.ExecuteReader();
                    //    int tempt = 0;
                    //    while (dr.Read())
                    //    {
                    //        tempt++;
                    //        if (tempt>t)
                    //        {
                    //            break;
                    //        }
                    //    }
                    //    for (int i = 0; i <= n; i++)
                    //    {
                    //        if (i % interval == 0)
                    //        {
                    //            double x = (double)new XDate(dateTimePickerbegintime.Value.AddSeconds(i));
                    //            double y = Convert.ToDouble(dr[comboBox1.SelectedIndex + 1].ToString());
                    //            listemp.Add(x, y);
                    //        }
                    //        dr.Read();
                    //    }
                             
                    //    oleCon.Close();
                    //}
                    zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;
                    hiscurvemy = zedGraphControl1.GraphPane.AddCurve("历史曲线", listemp, Color.Red, SymbolType.None);
                    //hiscurvemy.Line.IsSmooth = true;
                    this.zedGraphControl1.AxisChange();
                    this.zedGraphControl1.Refresh();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;
            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //int pos = Convert.ToInt16(comboBox1.Text);
            //if (curvemy[pos - 16385] != null)
            //{
            //    curvemy[pos - 16385].IsVisible = false;
            //    //curvemy[pos - 16385]=null;

               
            //}
            this.zedGraphControl1.GraphPane.CurveList.Clear();
            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();
            timer1.Enabled = false;
        }

        private void dateTimePickerbegindata_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerenddata.Value = dateTimePickerbegindata.Value;
        }

        private void dateTimePickerenddata_ValueChanged(object sender, EventArgs e)
        {
            dateTimePickerbegindata.Value = dateTimePickerenddata.Value;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox1.Enabled = false;
                dateTimePickerbegindata.Enabled = false;
                dateTimePickerbegintime.Enabled = false;
                dateTimePickerenddata.Enabled = false;
                dateTimePickerendtime.Enabled = false;
                comboBox1.Items.Clear();
                for (int j = 0; j < PublicDataClass.SaveText.Cfg[0].YccfgNum; j++)
                {
                    comboBox1.Items.Add(16385 + j);
                }
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                textBox1.Enabled = true;
                dateTimePickerbegindata.Enabled = true;
                dateTimePickerbegintime.Enabled = true;
                dateTimePickerenddata.Enabled = true;
                dateTimePickerendtime.Enabled = true;
                comboBox1.Items.Clear();
                for (int j = 0; j < PublicDataClass._Curve.savenum; j++)
                {
                    comboBox1.Items.Add(PublicDataClass._Curve.SavePosTable[j]);
                }
                comboBox1.SelectedIndex = 0;
            }

        }

        /*private void toolBar1_ButtonClick_1(object sender, ToolBarButtonClickEventArgs e)
        {
             bool add = e.Button.Pushed;
             switch (e.Button.Tag.ToString().ToLower())
             {

                 case "new":
                     GraphicToolForm toolfm = new GraphicToolForm();
                     toolfm.ShowDialog() ;
                     break;
             }
            //Graphics g = this.panel2.CreateGraphics();
            //g.Clear(backColor);
            //MyImg = new Bitmap(this.panel2.ClientRectangle.Width, this.panel2.ClientRectangle.Height);
            //graphics = Graphics.FromImage(MyImg);
            //graphics.Clear(backColor);
        }
       
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }*/
    }
}
