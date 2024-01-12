namespace WorkDivision
{
    partial class fSelectDivisionToCopy
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
            this.lvDivisionsToCopy = new System.Windows.Forms.ListView();
            this.btnSelect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lvDivisionsToCopy
            // 
            this.lvDivisionsToCopy.HideSelection = false;
            this.lvDivisionsToCopy.Location = new System.Drawing.Point(2, 2);
            this.lvDivisionsToCopy.Name = "lvDivisionsToCopy";
            this.lvDivisionsToCopy.Size = new System.Drawing.Size(582, 547);
            this.lvDivisionsToCopy.TabIndex = 0;
            this.lvDivisionsToCopy.UseCompatibleStateImageBehavior = false;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(191, 555);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(206, 37);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "Выбрать";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // fSelectDivisionToCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 604);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lvDivisionsToCopy);
            this.Name = "fSelectDivisionToCopy";
            this.Text = "Скопировать разделение";
            this.Load += new System.EventHandler(this.fSelectDivisionToCopy_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvDivisionsToCopy;
        private System.Windows.Forms.Button btnSelect;
    }
}