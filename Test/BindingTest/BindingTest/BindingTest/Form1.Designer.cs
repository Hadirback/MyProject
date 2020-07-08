namespace BindingTest
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.testLabel = new System.Windows.Forms.Label();
            this.testNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.totalInLabel = new System.Windows.Forms.Label();
            this.totalInValidLabel = new System.Windows.Forms.Label();
            this.checkButton = new System.Windows.Forms.Button();
            this.innnerNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.newObjButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.innnerNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.10021F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.89979F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 187F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.tableLayoutPanel1.Controls.Add(this.testLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.testNumericUpDown, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.totalInLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.totalInValidLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.innnerNumericUpDown, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.newObjButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textBox1, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 152F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 186F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // testLabel
            // 
            this.testLabel.AutoSize = true;
            this.testLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.testLabel.Location = new System.Drawing.Point(3, 0);
            this.testLabel.Name = "testLabel";
            this.testLabel.Size = new System.Drawing.Size(167, 56);
            this.testLabel.TabIndex = 0;
            this.testLabel.Text = "TEST";
            // 
            // testNumericUpDown
            // 
            this.testNumericUpDown.Location = new System.Drawing.Point(176, 3);
            this.testNumericUpDown.Name = "testNumericUpDown";
            this.testNumericUpDown.Size = new System.Drawing.Size(120, 22);
            this.testNumericUpDown.TabIndex = 1;
            // 
            // totalInLabel
            // 
            this.totalInLabel.AutoEllipsis = true;
            this.totalInLabel.AutoSize = true;
            this.totalInLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalInLabel.Location = new System.Drawing.Point(0, 56);
            this.totalInLabel.Margin = new System.Windows.Forms.Padding(0);
            this.totalInLabel.Name = "totalInLabel";
            this.totalInLabel.Size = new System.Drawing.Size(173, 56);
            this.totalInLabel.TabIndex = 3;
            this.totalInLabel.Text = "---";
            this.totalInLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // totalInValidLabel
            // 
            this.totalInValidLabel.AutoSize = true;
            this.totalInValidLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalInValidLabel.Location = new System.Drawing.Point(176, 56);
            this.totalInValidLabel.Name = "totalInValidLabel";
            this.totalInValidLabel.Size = new System.Drawing.Size(288, 56);
            this.totalInValidLabel.TabIndex = 4;
            this.totalInValidLabel.Text = "-";
            this.totalInValidLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkButton
            // 
            this.checkButton.Location = new System.Drawing.Point(470, 59);
            this.checkButton.Name = "checkButton";
            this.checkButton.Size = new System.Drawing.Size(166, 50);
            this.checkButton.TabIndex = 2;
            this.checkButton.Text = "CheckValue";
            this.checkButton.UseVisualStyleBackColor = true;
            this.checkButton.Click += new System.EventHandler(this.checkButton_Click);
            // 
            // innnerNumericUpDown
            // 
            this.innnerNumericUpDown.Location = new System.Drawing.Point(470, 3);
            this.innnerNumericUpDown.Name = "innnerNumericUpDown";
            this.innnerNumericUpDown.Size = new System.Drawing.Size(120, 22);
            this.innnerNumericUpDown.TabIndex = 5;
            this.innnerNumericUpDown.ValueChanged += new System.EventHandler(this.innnerNumericUpDown_ValueChanged);
            // 
            // newObjButton
            // 
            this.newObjButton.Location = new System.Drawing.Point(3, 115);
            this.newObjButton.Name = "newObjButton";
            this.newObjButton.Size = new System.Drawing.Size(167, 75);
            this.newObjButton.TabIndex = 6;
            this.newObjButton.Text = "Create new object";
            this.newObjButton.UseVisualStyleBackColor = true;
            this.newObjButton.Click += new System.EventHandler(this.newObjButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(470, 115);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.innnerNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label testLabel;
        private System.Windows.Forms.NumericUpDown testNumericUpDown;
        private System.Windows.Forms.Button checkButton;
        private System.Windows.Forms.Label totalInLabel;
        private System.Windows.Forms.Label totalInValidLabel;
        private System.Windows.Forms.NumericUpDown innnerNumericUpDown;
        private System.Windows.Forms.Button newObjButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}

