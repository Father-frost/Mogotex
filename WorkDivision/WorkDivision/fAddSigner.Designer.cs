namespace WorkDivision
{
    partial class fAddSigner
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbPost = new System.Windows.Forms.TextBox();
            this.tbFIO = new System.Windows.Forms.TextBox();
            this.tbOrder = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Location = new System.Drawing.Point(35, 36);
            this.lblID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(0, 16);
            this.lblID.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(332, 149);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(247, 28);
            this.button1.TabIndex = 15;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 16);
            this.label1.TabIndex = 21;
            this.label1.Text = "Должность:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 16);
            this.label2.TabIndex = 22;
            this.label2.Text = "ФИО:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(35, 104);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 16);
            this.label4.TabIndex = 23;
            this.label4.Text = "Порядок:";
            // 
            // tbPost
            // 
            this.tbPost.Location = new System.Drawing.Point(109, 36);
            this.tbPost.Margin = new System.Windows.Forms.Padding(4);
            this.tbPost.Name = "tbPost";
            this.tbPost.Size = new System.Drawing.Size(470, 22);
            this.tbPost.TabIndex = 24;
            // 
            // tbFIO
            // 
            this.tbFIO.Location = new System.Drawing.Point(110, 68);
            this.tbFIO.Margin = new System.Windows.Forms.Padding(4);
            this.tbFIO.Name = "tbFIO";
            this.tbFIO.Size = new System.Drawing.Size(469, 22);
            this.tbFIO.TabIndex = 25;
            // 
            // tbOrder
            // 
            this.tbOrder.Location = new System.Drawing.Point(109, 101);
            this.tbOrder.Margin = new System.Windows.Forms.Padding(4);
            this.tbOrder.Name = "tbOrder";
            this.tbOrder.Size = new System.Drawing.Size(67, 22);
            this.tbOrder.TabIndex = 26;
            // 
            // fAddSigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 205);
            this.Controls.Add(this.tbOrder);
            this.Controls.Add(this.tbFIO);
            this.Controls.Add(this.tbPost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fAddSigner";
            this.Text = "Добавить / Изменить запись";
            this.Load += new System.EventHandler(this.fAddSigner_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblID;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPost;
        private System.Windows.Forms.TextBox tbFIO;
        private System.Windows.Forms.TextBox tbOrder;
    }
}