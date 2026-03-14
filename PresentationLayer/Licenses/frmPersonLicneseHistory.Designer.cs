namespace DVLD.Licenses
{
    partial class frmPersonLicneseHistory
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
            this.btnClose = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ctrlLicneseHistory1 = new DVLD.Licenses.Control.ctrlLicneseHistory();
            this.ctrlPersonDetailsWithFilter1 = new DVLD.ctrlPersonDetailsWithFilter();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Image = global::DVLD.Properties.Resources.Close_32;
            this.btnClose.Location = new System.Drawing.Point(761, 607);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 40);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DVLD.Properties.Resources.PersonLicenseHistory_512;
            this.pictureBox1.Location = new System.Drawing.Point(12, 68);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(203, 183);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // ctrlLicneseHistory1
            // 
            this.ctrlLicneseHistory1.Location = new System.Drawing.Point(2, 306);
            this.ctrlLicneseHistory1.Name = "ctrlLicneseHistory1";
            this.ctrlLicneseHistory1.Size = new System.Drawing.Size(842, 312);
            this.ctrlLicneseHistory1.TabIndex = 2;
            // 
            // ctrlPersonDetailsWithFilter1
            // 
            this.ctrlPersonDetailsWithFilter1.FilterEnabled = true;
            this.ctrlPersonDetailsWithFilter1.Location = new System.Drawing.Point(221, 1);
            this.ctrlPersonDetailsWithFilter1.Name = "ctrlPersonDetailsWithFilter1";
            this.ctrlPersonDetailsWithFilter1.ShowAddPerson = true;
            this.ctrlPersonDetailsWithFilter1.Size = new System.Drawing.Size(618, 314);
            this.ctrlPersonDetailsWithFilter1.TabIndex = 0;
            this.ctrlPersonDetailsWithFilter1.OnPersonSelected += new System.Action<int>(this.ctrlPersonDetailsWithFilter1_OnPersonSelected);
            // 
            // frmPersonLicneseHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 650);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrlLicneseHistory1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ctrlPersonDetailsWithFilter1);
            this.Name = "frmPersonLicneseHistory";
            this.Text = "frmPersonLicneseHistory";
            this.Load += new System.EventHandler(this.frmPersonLicneseHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlPersonDetailsWithFilter ctrlPersonDetailsWithFilter1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Control.ctrlLicneseHistory ctrlLicneseHistory1;
        private System.Windows.Forms.Button btnClose;
    }
}