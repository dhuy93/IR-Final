namespace IR_Final
{
    partial class Form1
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
            this.queryTb = new System.Windows.Forms.RichTextBox();
            this.executeBtn = new System.Windows.Forms.Button();
            this.resultsTb = new System.Windows.Forms.RichTextBox();
            this.modeCbx = new System.Windows.Forms.ComboBox();
            this.modeLb = new System.Windows.Forms.Label();
            this.queryLb = new System.Windows.Forms.Label();
            this.resultLb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // queryTb
            // 
            this.queryTb.Location = new System.Drawing.Point(71, 50);
            this.queryTb.Name = "queryTb";
            this.queryTb.Size = new System.Drawing.Size(400, 100);
            this.queryTb.TabIndex = 0;
            this.queryTb.Text = "Input your query here";
            // 
            // executeBtn
            // 
            this.executeBtn.Location = new System.Drawing.Point(546, 50);
            this.executeBtn.Name = "executeBtn";
            this.executeBtn.Size = new System.Drawing.Size(75, 23);
            this.executeBtn.TabIndex = 1;
            this.executeBtn.Text = "Search";
            this.executeBtn.UseVisualStyleBackColor = true;
            this.executeBtn.Click += new System.EventHandler(this.searchBtn_Click);
            // 
            // resultsTb
            // 
            this.resultsTb.Location = new System.Drawing.Point(71, 177);
            this.resultsTb.Name = "resultsTb";
            this.resultsTb.ReadOnly = true;
            this.resultsTb.Size = new System.Drawing.Size(600, 300);
            this.resultsTb.TabIndex = 2;
            this.resultsTb.Text = "";
            // 
            // modeCbx
            // 
            this.modeCbx.FormattingEnabled = true;
            this.modeCbx.Location = new System.Drawing.Point(71, 12);
            this.modeCbx.Name = "modeCbx";
            this.modeCbx.Size = new System.Drawing.Size(121, 21);
            this.modeCbx.TabIndex = 3;
            this.modeCbx.SelectedIndexChanged += new System.EventHandler(this.modeCbx_SelectedIndexChanged);
            // 
            // modeLb
            // 
            this.modeLb.AutoSize = true;
            this.modeLb.Location = new System.Drawing.Point(12, 15);
            this.modeLb.Name = "modeLb";
            this.modeLb.Size = new System.Drawing.Size(34, 13);
            this.modeLb.TabIndex = 4;
            this.modeLb.Text = "Mode";
            // 
            // queryLb
            // 
            this.queryLb.AutoSize = true;
            this.queryLb.Location = new System.Drawing.Point(15, 59);
            this.queryLb.Name = "queryLb";
            this.queryLb.Size = new System.Drawing.Size(35, 13);
            this.queryLb.TabIndex = 5;
            this.queryLb.Text = "Query";
            // 
            // resultLb
            // 
            this.resultLb.AutoSize = true;
            this.resultLb.Location = new System.Drawing.Point(18, 207);
            this.resultLb.Name = "resultLb";
            this.resultLb.Size = new System.Drawing.Size(42, 13);
            this.resultLb.TabIndex = 6;
            this.resultLb.Text = "Results";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.resultLb);
            this.Controls.Add(this.queryLb);
            this.Controls.Add(this.modeLb);
            this.Controls.Add(this.modeCbx);
            this.Controls.Add(this.resultsTb);
            this.Controls.Add(this.executeBtn);
            this.Controls.Add(this.queryTb);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox queryTb;
        private System.Windows.Forms.Button executeBtn;
        private System.Windows.Forms.RichTextBox resultsTb;
        private System.Windows.Forms.ComboBox modeCbx;
        private System.Windows.Forms.Label modeLb;
        private System.Windows.Forms.Label queryLb;
        private System.Windows.Forms.Label resultLb;

    }
}

