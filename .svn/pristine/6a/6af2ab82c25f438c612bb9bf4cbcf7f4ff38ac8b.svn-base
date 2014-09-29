using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RichTextBoxExtended
{
    public partial class RichTextBoxExtended : UserControl
    {

     

        
        private RichTextBox rtbTemp =new RichTextBox();
        static  byte indexer =0;
        static bool LockFlag = false;   //不锁的
        static float Newfontsize = 0.0f;
        
        public RichTextBoxExtended()
        {
            InitializeComponent();
            UpdateToolbar();
        }
        #region Toolbar button click
        private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            bool add = e.Button.Pushed;
            switch (e.Button.Tag.ToString().ToLower())
            {

                case "bold":
                    ChangeFontStyle(FontStyle.Bold, add);
                    break;
                case "italic":
                    ChangeFontStyle(FontStyle.Italic, add);
                    indexer =1;
                    break;
                case "left":
                    //change horizontal alignment to left
                    rtb1.SelectionAlignment = HorizontalAlignment.Left;
                    tbcenter.Pushed = false;
                    tbright.Pushed = false;
                    break;
                case "center":
                    //change horizontal alignment to center
                    tbleft.Pushed = false;
                    rtb1.SelectionAlignment = HorizontalAlignment.Center;
                    tbright.Pushed = false;
                    break;
                case "right":
                    //change horizontal alignment to right
                    tbleft.Pushed = false;
                    tbcenter.Pushed = false;
                    rtb1.SelectionAlignment = HorizontalAlignment.Right;
                    break;
                case "undo":
                    rtb1.Undo();
                    break;
                case "redo":
                    rtb1.Redo();
                    break;
                case "seleteall":
                    rtb1.SelectAll();
                    tbseleteall.Pushed = false;
                    break;
                case "clearall":
                    rtb1.Clear();
                    tbclearall.Pushed = false;
                    break;
                case "unlock":
                    if (LockFlag ==true)
                    {
                        tblock.ImageIndex = 15;
                        rtb1.ReadOnly = false;
                        rtb1.BackColor = SystemColors.Window;
                        LockFlag = false;  
           
                    }
                    else if(LockFlag ==false)           
                    {
                        tblock.ImageIndex = 18;                 //锁住
                        rtb1.ReadOnly = true;
                        rtb1.BackColor = SystemColors.Info;
                        LockFlag = true;
                    }
                    tblock.Pushed = false;
                    break;
                case "open":
                    try
                    {
                        if (openfiledlg.ShowDialog() == DialogResult.OK && openfiledlg.FileName.Length > 0)
                            if (System.IO.Path.GetExtension(openfiledlg.FileName).ToLower().Equals(".rtf"))
                                rtb1.LoadFile(openfiledlg.FileName, RichTextBoxStreamType.RichText);
                            else
                                rtb1.LoadFile(openfiledlg.FileName, RichTextBoxStreamType.PlainText);
                    }
                    catch (ArgumentException ae)
                    {
                        if (ae.Message == "Invalid file format.")
                            MessageBox.Show("There was an error loading the file: " + openfiledlg.FileName);
                    }
                    break;
                case "save":
                    if (savefiledlg.ShowDialog() == DialogResult.OK && savefiledlg.FileName.Length > 0)
                        if (System.IO.Path.GetExtension(savefiledlg.FileName).ToLower().Equals(".rtf"))
                            rtb1.SaveFile(savefiledlg.FileName);
                        else
                            rtb1.SaveFile(savefiledlg.FileName, RichTextBoxStreamType.PlainText);
                    break;
                case "cut":
                    {
                        if (rtb1.SelectedText.Length <= 0) break;
                        rtb1.Cut();
                        break;
                    }

            }

        }
        #endregion

        #region Update Toolbar
        /// <summary>
        ///     Update the toolbar button statuses
        /// </summary>
        public void UpdateToolbar()
        {
            // Get the font, fontsize and style to apply to the toolbar buttons
            Font fnt = GetFontDetails();
            // Set font style buttons to the styles applying to the entire selection
            FontStyle style = fnt.Style;

            //Set all the style buttons using the gathered style
            tbBold.Pushed = fnt.Bold; //bold button
            toolBarButton20.Pushed = fnt.Italic; //italic button
            
    
            tbleft.Pushed = (rtb1.SelectionAlignment == HorizontalAlignment.Left); //justify left
            tbcenter.Pushed = (rtb1.SelectionAlignment == HorizontalAlignment.Center); //justify center
            tbright.Pushed = (rtb1.SelectionAlignment == HorizontalAlignment.Right); //justify right

            //Check the correct color
            foreach (MenuItem mi in cmcolor.MenuItems)
                mi.Checked = (rtb1.SelectionColor == Color.FromName(mi.Text));

            //Check the correct font
            foreach (MenuItem mi in cmfont.MenuItems)
                mi.Checked = (fnt.FontFamily.Name == mi.Text);

            //Check the correct font size
            foreach (MenuItem mi in cmfontsize.MenuItems)
                mi.Checked = ((int)fnt.SizeInPoints == float.Parse(mi.Text));
            Newfontsize = 9.0f;
        }
        #endregion

        #region Color Click
        private void Color_Click(object sender, EventArgs e)
        {
            ChangeFontColor(Color.FromName(((MenuItem)sender).Text));

        }
        #endregion
        #region Change font color
        /// <summary>
        ///     Change the richtextbox font color for the current selection
        /// </summary>
        public void ChangeFontColor(Color newColor)
        {
            //This method should handle cases that occur when multiple fonts/styles are selected
            // Parameters:-
            //	newColor - eg Color.Red

            int rtb1start = rtb1.SelectionStart;
            int len = rtb1.SelectionLength;
            int rtbTempStart = 0;

            //if len <= 1 and there is a selection font then just handle and return
            if (len <= 1 && rtb1.SelectionFont != null)
            {
                rtb1.SelectionColor = newColor;
                return;
            }

            // Step through the selected text one char at a time	
            rtbTemp.Rtf = rtb1.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);

                //change color
                rtbTemp.SelectionColor = newColor;
            }

            // Replace & reselect
            rtbTemp.Select(rtbTempStart, len);
            rtb1.SelectedRtf = rtbTemp.SelectedRtf;
            rtb1.Select(rtb1start, len);
            return;
        }
        #endregion

        #region Get Font Details
        /// <summary>
        ///     Returns a Font with:
        ///     1) The font applying to the entire selection, if none is the default font. 
        ///     2) The font size applying to the entire selection, if none is the size of the default font.
        ///     3) A style containing the attributes that are common to the entire selection, default regular.
        /// </summary>		
        /// 
        public Font GetFontDetails()
        {
            //This method should handle cases that occur when multiple fonts/styles are selected

            int rtb1start = rtb1.SelectionStart;
            int len = rtb1.SelectionLength;
            int rtbTempStart = 0;

            if (len <= 1)
            {
                // Return the selection or default font
                if (rtb1.SelectionFont != null)
                    return rtb1.SelectionFont;
                else
                    return rtb1.Font;
            }

            // Step through the selected text one char at a time	
            // after setting defaults from first char
            rtbTemp.Rtf = rtb1.SelectedRtf;

            //Turn everything on so we can turn it off one by one
            FontStyle replystyle =
                FontStyle.Bold | FontStyle.Italic | FontStyle.Strikeout | FontStyle.Underline;

            // Set reply font, size and style to that of first char in selection.
            rtbTemp.Select(rtbTempStart, 1);
            string replyfont = rtbTemp.SelectionFont.Name;
            float replyfontsize = rtbTemp.SelectionFont.Size;
            replystyle = replystyle & rtbTemp.SelectionFont.Style;

            // Search the rest of the selection
            for (int i = 1; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);

                // Check reply for different style
                replystyle = replystyle & rtbTemp.SelectionFont.Style;

                // Check font
                if (replyfont != rtbTemp.SelectionFont.FontFamily.Name)
                    replyfont = "";

                // Check font size
                if (replyfontsize != rtbTemp.SelectionFont.Size)
                    replyfontsize = (float)0.0;
            }

            // Now set font and size if more than one font or font size was selected
            if (replyfont == "")
                replyfont = rtbTemp.Font.FontFamily.Name;

            if (replyfontsize == 0.0)
                replyfontsize = rtbTemp.Font.Size;

            // generate reply font
            Font reply
                = new Font(replyfont, replyfontsize, replystyle);

            return reply;
        }
        #endregion

        #region Change font style
        /// <summary>
        ///     Change the richtextbox style for the current selection
        /// </summary>
        public void ChangeFontStyle(FontStyle style, bool add)
        {
            //This method should handle cases that occur when multiple fonts/styles are selected
            // Parameters:-
            //	style - eg FontStyle.Bold
            //	add - IF true then add else remove

            // throw error if style isn't: bold, italic, strikeout or underline
            if (style != FontStyle.Bold
                && style != FontStyle.Italic
                && style != FontStyle.Strikeout
                && style != FontStyle.Underline)
                throw new System.InvalidProgramException("Invalid style parameter to ChangeFontStyle");

            int rtb1start = rtb1.SelectionStart;
            int len = rtb1.SelectionLength;
            int rtbTempStart = 0;

            //if len <= 1 and there is a selection font then just handle and return
            if (len <= 1 && rtb1.SelectionFont != null)
            {
                //add or remove style 
                if (add)
                    rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style | style);
                else
                    rtb1.SelectionFont = new Font(rtb1.SelectionFont, rtb1.SelectionFont.Style & ~style);

                return;
            }

            // Step through the selected text one char at a time	
            rtbTemp.Rtf = rtb1.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);

                //add or remove style 
                if (add)
                    rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont, rtbTemp.SelectionFont.Style | style);
                else
                    rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont, rtbTemp.SelectionFont.Style & ~style);
            }

            // Replace & reselect
            rtbTemp.Select(rtbTempStart, len);
            rtb1.SelectedRtf = rtbTemp.SelectedRtf;
            rtb1.Select(rtb1start, len);
            return;
        }
        #endregion

        #region Show Text
        public void ShowText(string str)
        {
            rtb1.AppendText(str);

        }
        #endregion
        public void SeleteTextColor(Color color)
        {
            rtb1.SelectionColor = color;

        }
        public void ClearText()
        {
            rtb1.Clear();

        }
        #region 日志记录、支持其他线程访问
        public delegate void LogAppendDelegate(Color color, string text);
        /// <summary> 
        /// 追加显示文本 
        /// </summary> 
        /// <param name="color">文本颜色</param> 
        /// <param name="text">显示文本</param> 
        public void LogAppend(Color color, string text)
        {
            if (rtb1.ReadOnly == true)
                return;
          //  this.ActiveControl = this.rtb1;
         //   rtb1.Focus();
            rtb1.AppendText("\n");
            if(indexer ==1)
                ChangeFontStyle(FontStyle.Italic, true);

            rtb1.SelectionFont = new Font(rtb1.SelectionFont.FontFamily, Newfontsize, rtb1.SelectionFont.Style);
                    
            rtb1.SelectionColor = color;
            rtb1.AppendText(text);
            rtb1.ScrollToCaret();
        }
        /// <summary> 
        /// 显示错误日志 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogError(string text)
        {

            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtb1.Invoke(la, Color.Red, text);
        }
        /// <summary> 
        /// 显示警告信息 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogWarning(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtb1.Invoke(la, Color.Fuchsia, text);
        }
        /// <summary> 
        /// 显示信息 
        /// </summary> 
        /// <param name="text"></param> 
        public void LogMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtb1.Invoke(la, Color.Black, text);
        }
        /// <summary> 
        /// 显示连接信息
        /// </summary> 
        /// <param name="text"></param> 
        public void LinkMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtb1.Invoke(la, Color.DeepPink, text);
        }
        /// <summary> 
        /// 显示应答信息
        /// </summary> 
        /// <param name="text"></param> 
        public void AckMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtb1.Invoke(la, Color.ForestGreen, text);
        }
        /// <summary> 
        /// 显示3遥信息
        /// </summary> 
        /// <param name="text"></param> 
        public void ThreeYMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtb1.Invoke(la, Color.Blue, text);
        }
        /// <summary> 
        /// 显示遥控信息
        /// </summary> 
        /// <param name="text"></param> 
        public void YkAckMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtb1.Invoke(la, Color.Maroon, text);
        }
        /// <summary> 
        /// 显示遥控撤销信息
        /// </summary> 
        /// <param name="text"></param> 
        public void YkStopMessage(string text)
        {
            LogAppendDelegate la = new LogAppendDelegate(LogAppend);
            rtb1.Invoke(la, Color.LightPink, text);
        }
        #endregion 

        #region Font Size Click

        private void FontSize_Click(object sender, EventArgs e)
        {
            ChangeFontSize(float.Parse(((MenuItem)sender).Text));
            Newfontsize = float.Parse(((MenuItem)sender).Text);
            foreach (MenuItem mi in cmfontsize.MenuItems)
            {
                //mi.Checked = (mi.Text == (((MenuItem)sender).Text));
                if (mi.Text == ((MenuItem)sender).Text)
                {
                    mi.Checked = true;
                }
                else
                    mi.Checked = false;
            }

        }
        #endregion

        #region Change font size
        /// <summary>
        ///     Change the richtextbox font size for the current selection
        /// </summary>
        public void ChangeFontSize(float fontSize)
        {
            //This method should handle cases that occur when multiple fonts/styles are selected
            // Parameters:-
            // fontSize - the fontsize to be applied, eg 33.5
            if (fontSize <= 0.0)
                throw new System.InvalidProgramException("Invalid font size parameter to ChangeFontSize");

            int rtb1start = rtb1.SelectionStart;
            int len = rtb1.SelectionLength;
            int rtbTempStart = 0;

            // If len <= 1 and there is a selection font, amend and return
            if (len <= 1 && rtb1.SelectionFont != null)
            {
                rtb1.SelectionFont =
                    new Font(rtb1.SelectionFont.FontFamily, fontSize, rtb1.SelectionFont.Style);
                return;
            }

            // Step through the selected text one char at a time
            rtbTemp.Rtf = rtb1.SelectedRtf;
            for (int i = 0; i < len; ++i)
            {
                rtbTemp.Select(rtbTempStart + i, 1);
                rtbTemp.SelectionFont = new Font(rtbTemp.SelectionFont.FontFamily, fontSize, rtbTemp.SelectionFont.Style);
            }

            // Replace & reselect
            rtbTemp.Select(rtbTempStart, len);
            rtb1.SelectedRtf = rtbTemp.SelectedRtf;
            rtb1.Select(rtb1start, len);
            return;
        }
        #endregion
    }
}
