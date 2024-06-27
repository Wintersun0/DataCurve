
namespace DataCurve
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataCurveEx1 = new UIResource.DataCurveEx();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemMarker = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataCurveEx1
            // 
            this.dataCurveEx1.BackColor = System.Drawing.Color.White;
            this.dataCurveEx1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataCurveEx1.Location = new System.Drawing.Point(13, 14);
            this.dataCurveEx1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataCurveEx1.Name = "dataCurveEx1";
            this.dataCurveEx1.Size = new System.Drawing.Size(982, 701);
            this.dataCurveEx1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemRestore,
            this.menuItemMarker,
            this.toolStripSeparator1,
            this.menuItemCopy,
            this.menuItemPrint});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 98);
            // 
            // menuItemRestore
            // 
            this.menuItemRestore.Name = "menuItemRestore";
            this.menuItemRestore.Size = new System.Drawing.Size(180, 22);
            this.menuItemRestore.Text = "还原";
            this.menuItemRestore.Click += new System.EventHandler(this.menuItemRestore_Click);
            // 
            // menuItemMarker
            // 
            this.menuItemMarker.Name = "menuItemMarker";
            this.menuItemMarker.Size = new System.Drawing.Size(180, 22);
            this.menuItemMarker.Text = "标记";
            this.menuItemMarker.Click += new System.EventHandler(this.menuItemMarker_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Name = "menuItemCopy";
            this.menuItemCopy.Size = new System.Drawing.Size(180, 22);
            this.menuItemCopy.Text = "复制";
            this.menuItemCopy.Click += new System.EventHandler(this.menuItemCopy_Click);
            // 
            // menuItemPrint
            // 
            this.menuItemPrint.Name = "menuItemPrint";
            this.menuItemPrint.Size = new System.Drawing.Size(180, 22);
            this.menuItemPrint.Text = "打印";
            this.menuItemPrint.Click += new System.EventHandler(this.menuItemPrint_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.dataCurveEx1);
            this.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Curve";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UIResource.DataCurveEx dataCurveEx1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemRestore;
        private System.Windows.Forms.ToolStripMenuItem menuItemMarker;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem menuItemPrint;
    }
}

