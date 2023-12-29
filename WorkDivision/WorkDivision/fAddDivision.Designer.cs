namespace WorkDivision
{
    partial class fAddDivision
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
            this.tbProd = new System.Windows.Forms.TextBox();
            this.tbEI = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbModel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbProdCat = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbProdGRP = new System.Windows.Forms.TextBox();
            this.lbKMOD = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(23, 19);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(0, 13);
            this.lblID.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(401, 195);
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
            this.label2.Location = new System.Drawing.Point(23, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Наименование модели:";
            // 
            // tbProd
            // 
            this.tbProd.Enabled = false;
            this.tbProd.Location = new System.Drawing.Point(26, 109);
            this.tbProd.Name = "tbProd";
            this.tbProd.Size = new System.Drawing.Size(162, 20);
            this.tbProd.TabIndex = 25;
            // 
            // tbEI
            // 
            this.tbEI.Enabled = false;
            this.tbEI.Location = new System.Drawing.Point(26, 156);
            this.tbEI.Name = "tbEI";
            this.tbEI.Size = new System.Drawing.Size(162, 20);
            this.tbEI.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Ед. изм.:";
            // 
            // cbModel
            // 
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(26, 65);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(562, 21);
            this.cbModel.TabIndex = 34;
            this.cbModel.SelectedIndexChanged += new System.EventHandler(this.cbModel_SelectedIndexChanged);
            this.cbModel.TextUpdate += new System.EventHandler(this.cbModel_TextUpdate);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Вид изделия:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(214, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Категория 1:";
            // 
            // tbProdCat
            // 
            this.tbProdCat.Enabled = false;
            this.tbProdCat.Location = new System.Drawing.Point(217, 109);
            this.tbProdCat.Name = "tbProdCat";
            this.tbProdCat.Size = new System.Drawing.Size(180, 20);
            this.tbProdCat.TabIndex = 40;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(415, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Категория 2:";
            // 
            // tbProdGRP
            // 
            this.tbProdGRP.Enabled = false;
            this.tbProdGRP.Location = new System.Drawing.Point(417, 109);
            this.tbProdGRP.Name = "tbProdGRP";
            this.tbProdGRP.Size = new System.Drawing.Size(170, 20);
            this.tbProdGRP.TabIndex = 42;
            // 
            // lbKMOD
            // 
            this.lbKMOD.AutoSize = true;
            this.lbKMOD.Location = new System.Drawing.Point(23, 41);
            this.lbKMOD.Name = "lbKMOD";
            this.lbKMOD.Size = new System.Drawing.Size(0, 13);
            this.lbKMOD.TabIndex = 44;
            // 
            // fAddDivision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 244);
            this.Controls.Add(this.lbKMOD);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbProdGRP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbProdCat);
            this.Controls.Add(this.cbModel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbEI);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbProd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.button1);
            this.Name = "fAddDivision";
            this.Text = "Добавить / Изменить разделение";
            this.Load += new System.EventHandler(this.fAddModel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbProd;
        private System.Windows.Forms.TextBox tbEI;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbProdCat;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbProdGRP;
        private System.Windows.Forms.Label lbKMOD;
    }
}