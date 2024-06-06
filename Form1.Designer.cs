namespace WinFormAPI_Practice
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            GetData = new Button();
            SpiderDataGridView = new DataGridView();
            textBox1 = new TextBox();
            comboBoxTitleFilter = new ComboBox();
            comboBoxValueFilter = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)SpiderDataGridView).BeginInit();
            SuspendLayout();
            // 
            // GetData
            // 
            GetData.Font = new Font("Microsoft JhengHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            GetData.Location = new Point(68, 141);
            GetData.Name = "GetData";
            GetData.Size = new Size(137, 74);
            GetData.TabIndex = 0;
            GetData.Text = "開始撈資料";
            GetData.UseVisualStyleBackColor = true;
            GetData.Click += GetData_Click;
            // 
            // SpiderDataGridView
            // 
            SpiderDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            SpiderDataGridView.Location = new Point(250, 141);
            SpiderDataGridView.Name = "SpiderDataGridView";
            SpiderDataGridView.RowTemplate.Height = 25;
            SpiderDataGridView.Size = new Size(735, 561);
            SpiderDataGridView.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Font = new Font("Microsoft JhengHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(68, 31);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(917, 89);
            textBox1.TabIndex = 2;
            textBox1.Text = "情境：練習從特定網站 API 撈取資料並整理\r\n可以針對該資料的標題、內容去分類，快速找出需要的內容";
            // 
            // comboBoxTitleFilter
            // 
            comboBoxTitleFilter.FormattingEnabled = true;
            comboBoxTitleFilter.IntegralHeight = false;
            comboBoxTitleFilter.Location = new Point(68, 276);
            comboBoxTitleFilter.Name = "comboBoxTitleFilter";
            comboBoxTitleFilter.Size = new Size(137, 23);
            comboBoxTitleFilter.TabIndex = 3;
            comboBoxTitleFilter.SelectedIndexChanged += comboBoxTitleFilter_SelectedIndexChanged;
            // 
            // comboBoxValueFilter
            // 
            comboBoxValueFilter.Enabled = false;
            comboBoxValueFilter.FormattingEnabled = true;
            comboBoxValueFilter.Items.AddRange(new object[] { "123 ", "12" });
            comboBoxValueFilter.Location = new Point(68, 360);
            comboBoxValueFilter.Name = "comboBoxValueFilter";
            comboBoxValueFilter.Size = new Size(137, 23);
            comboBoxValueFilter.TabIndex = 4;
            comboBoxValueFilter.SelectedIndexChanged += comboBoxValueFilter_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.ActiveCaption;
            label1.Font = new Font("Microsoft JhengHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(110, 236);
            label1.Name = "label1";
            label1.Size = new Size(48, 24);
            label1.TabIndex = 5;
            label1.Text = "標題";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.ActiveCaption;
            label2.Font = new Font("Microsoft JhengHei UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(110, 320);
            label2.Name = "label2";
            label2.Size = new Size(48, 24);
            label2.TabIndex = 6;
            label2.Text = "數值";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 729);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBoxValueFilter);
            Controls.Add(comboBoxTitleFilter);
            Controls.Add(textBox1);
            Controls.Add(SpiderDataGridView);
            Controls.Add(GetData);
            Name = "Form1";
            Text = "WinFormAPI_Practice";
            ((System.ComponentModel.ISupportInitialize)SpiderDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button GetData;
        private DataGridView SpiderDataGridView;
        private TextBox textBox1;
        private ComboBox comboBoxTitleFilter;
        private ComboBox comboBoxValueFilter;
        private Label label1;
        private Label label2;
    }
}