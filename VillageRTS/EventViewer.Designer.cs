
namespace VillageRTS
{
    partial class EventViewer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.Variant1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Variant2 = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Variant3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.Variant3);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.Variant2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.Variant1);
            this.panel1.Controls.Add(this.TitleLabel);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 487);
            this.panel1.TabIndex = 0;
            // 
            // TitleLabel
            // 
            this.TitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TitleLabel.Font = new System.Drawing.Font("Arial Black", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TitleLabel.Location = new System.Drawing.Point(3, 3);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(554, 40);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "EventTitle";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // Variant1
            // 
            this.Variant1.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Variant1.Location = new System.Drawing.Point(7, 192);
            this.Variant1.Name = "Variant1";
            this.Variant1.Size = new System.Drawing.Size(178, 40);
            this.Variant1.TabIndex = 1;
            this.Variant1.Text = "Variant1";
            this.Variant1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(7, 244);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(178, 240);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(191, 244);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(178, 240);
            this.panel3.TabIndex = 4;
            // 
            // Variant2
            // 
            this.Variant2.Enabled = false;
            this.Variant2.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Variant2.Location = new System.Drawing.Point(191, 192);
            this.Variant2.Name = "Variant2";
            this.Variant2.Size = new System.Drawing.Size(178, 40);
            this.Variant2.TabIndex = 3;
            this.Variant2.Text = "Variant2";
            this.Variant2.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(375, 244);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(178, 240);
            this.panel4.TabIndex = 6;
            // 
            // Variant3
            // 
            this.Variant3.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Variant3.ForeColor = System.Drawing.Color.Red;
            this.Variant3.Location = new System.Drawing.Point(375, 192);
            this.Variant3.Name = "Variant3";
            this.Variant3.Size = new System.Drawing.Size(178, 40);
            this.Variant3.TabIndex = 5;
            this.Variant3.Text = "Variant3";
            this.Variant3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(7, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 146);
            this.label1.TabIndex = 7;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // EventViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 514);
            this.Controls.Add(this.panel1);
            this.Name = "EventViewer";
            this.Text = "EventViewer";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button Variant3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button Variant2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button Variant1;
        private System.Windows.Forms.Label label1;
    }
}