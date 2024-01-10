namespace WorkDivision
{
    partial class fAddModel
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
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbKMOD = new System.Windows.Forms.TextBox();
            this.tbEI = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbProduct = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCatProd = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbProdGRP = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(32, 57);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(0, 13);
            this.lblID.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(277, 237);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(185, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Наименование модели:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "КОД (старое обозначение):";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(166, 58);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(296, 20);
            this.tbName.TabIndex = 25;
            // 
            // tbKMOD
            // 
            this.tbKMOD.Location = new System.Drawing.Point(166, 32);
            this.tbKMOD.Name = "tbKMOD";
            this.tbKMOD.Size = new System.Drawing.Size(101, 20);
            this.tbKMOD.TabIndex = 26;
            // 
            // tbEI
            // 
            this.tbEI.Location = new System.Drawing.Point(166, 202);
            this.tbEI.Name = "tbEI";
            this.tbEI.Size = new System.Drawing.Size(101, 20);
            this.tbEI.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(107, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Ед. изм.:";
            // 
            // cbProduct
            // 
            this.cbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProduct.FormattingEnabled = true;
            this.cbProduct.Location = new System.Drawing.Point(166, 94);
            this.cbProduct.Name = "cbProduct";
            this.cbProduct.Size = new System.Drawing.Size(296, 21);
            this.cbProduct.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(85, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Вид изделия:";
            // 
            // cbCatProd
            // 
            this.cbCatProd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCatProd.FormattingEnabled = true;
            this.cbCatProd.Location = new System.Drawing.Point(166, 128);
            this.cbCatProd.Name = "cbCatProd";
            this.cbCatProd.Size = new System.Drawing.Size(296, 21);
            this.cbCatProd.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Категория 1:";
            // 
            // cbProdGRP
            // 
            this.cbProdGRP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProdGRP.FormattingEnabled = true;
            this.cbProdGRP.Location = new System.Drawing.Point(166, 164);
            this.cbProdGRP.Name = "cbProdGRP";
            this.cbProdGRP.Size = new System.Drawing.Size(296, 21);
            this.cbProdGRP.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(87, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Категория 2:";
            // 
            // fAddModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 281);
            this.Controls.Add(this.cbProdGRP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbCatProd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbProduct);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbEI);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbKMOD);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fAddModel";
            this.Text = "Добавить / Изменить запись";
            this.Load += new System.EventHandler(this.fAddModel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbKMOD;
        private System.Windows.Forms.TextBox tbEI;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbProduct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCatProd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbProdGRP;
        private System.Windows.Forms.Label label5;
    }
}