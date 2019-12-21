namespace PointerSearcher
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonRead = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColumnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMainStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnMainEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHeapStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnHeapEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnTargetAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.textBoxDepth = new System.Windows.Forms.TextBox();
            this.textBoxOffsetNum = new System.Windows.Forms.TextBox();
            this.textBoxOffsetAddress = new System.Windows.Forms.TextBox();
            this.buttonNarrowDown = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRead
            // 
            this.buttonRead.Location = new System.Drawing.Point(14, 214);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(158, 23);
            this.buttonRead.TabIndex = 0;
            this.buttonRead.Text = "Read 1st Dump Data";
            this.buttonRead.UseVisualStyleBackColor = true;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 243);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(646, 138);
            this.textBox1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnPath,
            this.ColumnMainStart,
            this.ColumnMainEnd,
            this.ColumnHeapStart,
            this.ColumnHeapEnd,
            this.ColumnTargetAddress});
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(646, 150);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // ColumnPath
            // 
            this.ColumnPath.HeaderText = "Path";
            this.ColumnPath.Name = "ColumnPath";
            this.ColumnPath.ToolTipText = "Path of Noexs dump data";
            // 
            // ColumnMainStart
            // 
            this.ColumnMainStart.HeaderText = "MainStart";
            this.ColumnMainStart.Name = "ColumnMainStart";
            this.ColumnMainStart.ToolTipText = "main start address";
            // 
            // ColumnMainEnd
            // 
            this.ColumnMainEnd.HeaderText = "MainEnd";
            this.ColumnMainEnd.Name = "ColumnMainEnd";
            this.ColumnMainEnd.ToolTipText = "main end address";
            // 
            // ColumnHeapStart
            // 
            this.ColumnHeapStart.HeaderText = "HeapStart";
            this.ColumnHeapStart.Name = "ColumnHeapStart";
            this.ColumnHeapStart.ToolTipText = "heap start address";
            // 
            // ColumnHeapEnd
            // 
            this.ColumnHeapEnd.HeaderText = "HeapEnd";
            this.ColumnHeapEnd.Name = "ColumnHeapEnd";
            this.ColumnHeapEnd.ToolTipText = "heap end address";
            // 
            // ColumnTargetAddress
            // 
            this.ColumnTargetAddress.HeaderText = "TargetAddress";
            this.ColumnTargetAddress.Name = "ColumnTargetAddress";
            this.ColumnTargetAddress.ToolTipText = "address you want to find a pointer of this dump data";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(257, 214);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(158, 23);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "Reset and Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // textBoxDepth
            // 
            this.textBoxDepth.Location = new System.Drawing.Point(72, 178);
            this.textBoxDepth.Name = "textBoxDepth";
            this.textBoxDepth.Size = new System.Drawing.Size(100, 19);
            this.textBoxDepth.TabIndex = 4;
            // 
            // textBoxOffsetNum
            // 
            this.textBoxOffsetNum.Location = new System.Drawing.Point(315, 178);
            this.textBoxOffsetNum.Name = "textBoxOffsetNum";
            this.textBoxOffsetNum.Size = new System.Drawing.Size(100, 19);
            this.textBoxOffsetNum.TabIndex = 5;
            // 
            // textBoxOffsetAddress
            // 
            this.textBoxOffsetAddress.Location = new System.Drawing.Point(558, 178);
            this.textBoxOffsetAddress.Name = "textBoxOffsetAddress";
            this.textBoxOffsetAddress.Size = new System.Drawing.Size(100, 19);
            this.textBoxOffsetAddress.TabIndex = 6;
            // 
            // buttonNarrowDown
            // 
            this.buttonNarrowDown.Location = new System.Drawing.Point(500, 214);
            this.buttonNarrowDown.Name = "buttonNarrowDown";
            this.buttonNarrowDown.Size = new System.Drawing.Size(158, 23);
            this.buttonNarrowDown.TabIndex = 7;
            this.buttonNarrowDown.Text = "Narrow Down Result";
            this.buttonNarrowDown.UseVisualStyleBackColor = true;
            this.buttonNarrowDown.Click += new System.EventHandler(this.buttonNarrowDown_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "MaxDepth";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "OffsetNum";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(483, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "OffsetRange";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 388);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(565, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(583, 388);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 417);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonNarrowDown);
            this.Controls.Add(this.textBoxOffsetAddress);
            this.Controls.Add(this.textBoxOffsetNum);
            this.Controls.Add(this.textBoxDepth);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonRead);
            this.Name = "Form1";
            this.Text = "PointerSearcher 0.03";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRead;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.TextBox textBoxDepth;
        private System.Windows.Forms.TextBox textBoxOffsetNum;
        private System.Windows.Forms.TextBox textBoxOffsetAddress;
        private System.Windows.Forms.Button buttonNarrowDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMainStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnMainEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHeapStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnHeapEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTargetAddress;
    }
}

