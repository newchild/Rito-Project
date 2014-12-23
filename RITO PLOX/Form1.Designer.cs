namespace RITO_PLOX
{
    partial class Ritoconnector
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
            this.ConnectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.KeyBox = new System.Windows.Forms.TextBox();
            this.Matches = new System.Windows.Forms.ComboBox();
            this.GetInfoButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(263, 12);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(173, 21);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(249, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Custom Devkey (Leave empty to use standard one)";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(77, 13);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(180, 20);
            this.UsernameBox.TabIndex = 3;
            // 
            // KeyBox
            // 
            this.KeyBox.Location = new System.Drawing.Point(263, 45);
            this.KeyBox.Name = "KeyBox";
            this.KeyBox.Size = new System.Drawing.Size(173, 20);
            this.KeyBox.TabIndex = 4;
            // 
            // Matches
            // 
            this.Matches.FormattingEnabled = true;
            this.Matches.Location = new System.Drawing.Point(16, 117);
            this.Matches.Name = "Matches";
            this.Matches.Size = new System.Drawing.Size(153, 21);
            this.Matches.TabIndex = 5;
            // 
            // GetInfoButton
            // 
            this.GetInfoButton.Location = new System.Drawing.Point(175, 115);
            this.GetInfoButton.Name = "GetInfoButton";
            this.GetInfoButton.Size = new System.Drawing.Size(258, 23);
            this.GetInfoButton.TabIndex = 6;
            this.GetInfoButton.Text = "Get Info";
            this.GetInfoButton.UseVisualStyleBackColor = true;
            // 
            // Ritoconnector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 342);
            this.Controls.Add(this.GetInfoButton);
            this.Controls.Add(this.Matches);
            this.Controls.Add(this.KeyBox);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectButton);
            this.Name = "Ritoconnector";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Ritoconnector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.TextBox KeyBox;
        private System.Windows.Forms.ComboBox Matches;
        private System.Windows.Forms.Button GetInfoButton;
    }
}

