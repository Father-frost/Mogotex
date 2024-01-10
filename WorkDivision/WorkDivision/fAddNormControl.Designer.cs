namespace WorkDivision
{
    partial class fAddNormControl
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
            this.tbVIDTK = new System.Windows.Forms.TextBox();
            this.tbNORMVR = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(26, 29);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(0, 13);
            this.lblID.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(383, 106);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbVIDTK
            // 
            this.tbVIDTK.Location = new System.Drawing.Point(154, 27);
            this.tbVIDTK.Name = "tbVIDTK";
            this.tbVIDTK.Size = new System.Drawing.Size(410, 20);
            this.tbVIDTK.TabIndex = 25;
            // 
            // tbNORMVR
            // 
            this.tbNORMVR.Location = new System.Drawing.Point(154, 63);
            this.tbNORMVR.Name = "tbNORMVR";
            this.tbNORMVR.Size = new System.Drawing.Size(174, 20);
            this.tbNORMVR.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Затрата времени на 1м:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Вид ткани:";
            // 
            // fAddNormControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 148);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbNORMVR);
            this.Controls.Add(this.tbVIDTK);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fAddNormControl";
            this.Text = "Добавить / Изменить запись";
            this.Load += new System.EventHandler(this.fAddNormControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbVIDTK;
        private System.Windows.Forms.TextBox tbNORMVR;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}