namespace WorkDivision
{
    partial class fEditOperByDivision
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
            this.lblID = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbCost = new System.Windows.Forms.TextBox();
            this.tbOperName = new System.Windows.Forms.TextBox();
            this.tbMatRate = new System.Windows.Forms.TextBox();
            this.tbTarif = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbWorkersCnt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbNVR = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbNVRbyItem = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbSumItem = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbSelectMat = new System.Windows.Forms.ComboBox();
            this.cbRank = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lbUCH = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btCalculate = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(16, 21);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(0, 13);
            this.lblID.TabIndex = 16;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(540, 195);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(288, 37);
            this.btnNext.TabIndex = 15;
            this.btnNext.Text = "Следующая операции   >>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(14, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Расценка на 1м:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Операция:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Расход ткани, м.:";
            // 
            // tbCost
            // 
            this.tbCost.Enabled = false;
            this.tbCost.Location = new System.Drawing.Point(16, 24);
            this.tbCost.Name = "tbCost";
            this.tbCost.Size = new System.Drawing.Size(95, 20);
            this.tbCost.TabIndex = 24;
            // 
            // tbOperName
            // 
            this.tbOperName.Enabled = false;
            this.tbOperName.Location = new System.Drawing.Point(88, 39);
            this.tbOperName.Name = "tbOperName";
            this.tbOperName.Size = new System.Drawing.Size(740, 20);
            this.tbOperName.TabIndex = 25;
            // 
            // tbMatRate
            // 
            this.tbMatRate.Location = new System.Drawing.Point(28, 138);
            this.tbMatRate.Name = "tbMatRate";
            this.tbMatRate.Size = new System.Drawing.Size(162, 20);
            this.tbMatRate.TabIndex = 26;
            this.tbMatRate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMatRate_KeyDown);
            this.tbMatRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMatRate_KeyPress);
            // 
            // tbTarif
            // 
            this.tbTarif.Enabled = false;
            this.tbTarif.Location = new System.Drawing.Point(207, 138);
            this.tbTarif.Name = "tbTarif";
            this.tbTarif.Size = new System.Drawing.Size(92, 20);
            this.tbTarif.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 27;
            this.label3.Text = "Разряд:";
            // 
            // tbWorkersCnt
            // 
            this.tbWorkersCnt.Location = new System.Drawing.Point(428, 137);
            this.tbWorkersCnt.Name = "tbWorkersCnt";
            this.tbWorkersCnt.Size = new System.Drawing.Size(100, 20);
            this.tbWorkersCnt.TabIndex = 30;
            this.tbWorkersCnt.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbWorkersCnt_KeyDown);
            this.tbWorkersCnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbWorkersCnt_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(426, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Кол-во работников:";
            // 
            // tbNVR
            // 
            this.tbNVR.Location = new System.Drawing.Point(312, 137);
            this.tbNVR.Name = "tbNVR";
            this.tbNVR.Size = new System.Drawing.Size(104, 20);
            this.tbNVR.TabIndex = 32;
            this.tbNVR.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbNVR_KeyDown);
            this.tbNVR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNVR_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(310, 120);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Норма времени, сек:";
            // 
            // tbNVRbyItem
            // 
            this.tbNVRbyItem.Enabled = false;
            this.tbNVRbyItem.Location = new System.Drawing.Point(129, 24);
            this.tbNVRbyItem.Name = "tbNVRbyItem";
            this.tbNVRbyItem.Size = new System.Drawing.Size(133, 20);
            this.tbNVRbyItem.TabIndex = 34;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(127, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(152, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "Норма времени на 1 ед:";
            // 
            // tbSumItem
            // 
            this.tbSumItem.Enabled = false;
            this.tbSumItem.Location = new System.Drawing.Point(16, 75);
            this.tbSumItem.Name = "tbSumItem";
            this.tbSumItem.Size = new System.Drawing.Size(246, 20);
            this.tbSumItem.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(14, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(141, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Стоимость 1 единицы:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 71);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 13);
            this.label9.TabIndex = 37;
            this.label9.Text = "Выберите ткань:";
            // 
            // cbSelectMat
            // 
            this.cbSelectMat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectMat.FormattingEnabled = true;
            this.cbSelectMat.Location = new System.Drawing.Point(28, 87);
            this.cbSelectMat.Margin = new System.Windows.Forms.Padding(2);
            this.cbSelectMat.Name = "cbSelectMat";
            this.cbSelectMat.Size = new System.Drawing.Size(162, 21);
            this.cbSelectMat.TabIndex = 38;
            this.cbSelectMat.SelectedIndexChanged += new System.EventHandler(this.cbSelectMat_SelectedIndexChanged);
            this.cbSelectMat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbSelectMat_KeyDown);
            // 
            // cbRank
            // 
            this.cbRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRank.FormattingEnabled = true;
            this.cbRank.Location = new System.Drawing.Point(207, 87);
            this.cbRank.Margin = new System.Windows.Forms.Padding(2);
            this.cbRank.Name = "cbRank";
            this.cbRank.Size = new System.Drawing.Size(92, 21);
            this.cbRank.TabIndex = 39;
            this.cbRank.SelectedIndexChanged += new System.EventHandler(this.cbRank_SelectedIndexChanged);
            this.cbRank.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbRank_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(205, 121);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 13);
            this.label10.TabIndex = 40;
            this.label10.Text = "Тарифная ставка:";
            // 
            // lbUCH
            // 
            this.lbUCH.AutoSize = true;
            this.lbUCH.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbUCH.Location = new System.Drawing.Point(86, 9);
            this.lbUCH.Name = "lbUCH";
            this.lbUCH.Size = new System.Drawing.Size(0, 13);
            this.lbUCH.TabIndex = 41;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(26, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 42;
            this.label11.Text = "Участок:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbSumItem);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbNVRbyItem);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbCost);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(539, 63);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(288, 106);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            // 
            // btCalculate
            // 
            this.btCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btCalculate.Location = new System.Drawing.Point(312, 195);
            this.btCalculate.Name = "btCalculate";
            this.btCalculate.Size = new System.Drawing.Size(215, 37);
            this.btCalculate.TabIndex = 46;
            this.btCalculate.Text = "Пересчитать";
            this.btCalculate.UseVisualStyleBackColor = true;
            this.btCalculate.Click += new System.EventHandler(this.btCalculate_Click);
            this.btCalculate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btCalculate_KeyDown);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(11, 195);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(288, 37);
            this.btnPrevious.TabIndex = 47;
            this.btnPrevious.Text = "<<  Предыдущая операции";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // fEditOperByDivision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 272);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.btCalculate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lbUCH);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbRank);
            this.Controls.Add(this.cbSelectMat);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbNVR);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbWorkersCnt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbTarif);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbMatRate);
            this.Controls.Add(this.tbOperName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.btnNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fEditOperByDivision";
            this.Text = "Добавить / Изменить запись";
            this.Load += new System.EventHandler(this.fAddOper_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbCost;
        private System.Windows.Forms.TextBox tbOperName;
        private System.Windows.Forms.TextBox tbMatRate;
        private System.Windows.Forms.TextBox tbTarif;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbWorkersCnt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbNVR;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbNVRbyItem;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbSumItem;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbSelectMat;
        private System.Windows.Forms.ComboBox cbRank;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbUCH;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btCalculate;
        private System.Windows.Forms.Button btnPrevious;
    }
}