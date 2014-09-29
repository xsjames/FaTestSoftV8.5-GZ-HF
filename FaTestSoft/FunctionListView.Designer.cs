namespace FaTestSoft
{
    partial class FunctionListView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FunctionListView));
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("楷体_GB2312", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.ItemHeight = 35;
            this.treeView1.LineColor = System.Drawing.Color.Red;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(292, 301);
            this.treeView1.TabIndex = 0;
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Tree_NodeMouseDoubleClick);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "MYCOMP27.ICO");
            this.imageList1.Images.SetKeyName(1, "ICO20.ICO");
            this.imageList1.Images.SetKeyName(2, "My Uploads XP.ico");
            this.imageList1.Images.SetKeyName(3, "mdf_ndf_dbfiles.ico");
            this.imageList1.Images.SetKeyName(4, "ICO7.ICO");
            this.imageList1.Images.SetKeyName(5, "iMac OS Find.ico");
            this.imageList1.Images.SetKeyName(6, "ICO48.ICO");
            this.imageList1.Images.SetKeyName(7, "ICO152.ICO");
            this.imageList1.Images.SetKeyName(8, "25.ico");
            this.imageList1.Images.SetKeyName(9, "ICO220.ICO");
            this.imageList1.Images.SetKeyName(10, "Be Icon World.ico");
            this.imageList1.Images.SetKeyName(11, "17.ico");
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FunctionListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 301);
            this.Controls.Add(this.treeView1);
            this.DockAreas = ((KD.WinFormsUI.Docking.DockAreas)((((KD.WinFormsUI.Docking.DockAreas.Float | KD.WinFormsUI.Docking.DockAreas.DockLeft)
                        | KD.WinFormsUI.Docking.DockAreas.DockTop)
                        | KD.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FunctionListView";
            this.Text = "功能视图";
            this.Load += new System.EventHandler(this.FunctionListView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
    }
}