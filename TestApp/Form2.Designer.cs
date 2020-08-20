namespace TestApp
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.PasswordTxt = new System.Windows.Forms.TextBox();
            this.ConfirmBtn = new System.Windows.Forms.Button();
            this.incorrectPassLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label1.Location = new System.Drawing.Point(106, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please enter your password";
            // 
            // PasswordTxt
            // 
            this.PasswordTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.PasswordTxt.Location = new System.Drawing.Point(30, 55);
            this.PasswordTxt.Name = "PasswordTxt";
            this.PasswordTxt.Size = new System.Drawing.Size(333, 21);
            this.PasswordTxt.TabIndex = 1;
            this.PasswordTxt.UseSystemPasswordChar = true;
            // 
            // ConfirmBtn
            // 
            this.ConfirmBtn.Location = new System.Drawing.Point(263, 98);
            this.ConfirmBtn.Name = "ConfirmBtn";
            this.ConfirmBtn.Size = new System.Drawing.Size(100, 23);
            this.ConfirmBtn.TabIndex = 2;
            this.ConfirmBtn.Text = "Confirm";
            this.ConfirmBtn.UseVisualStyleBackColor = true;
            this.ConfirmBtn.Click += new System.EventHandler(this.ConfirmBtn_Click);
            // 
            // incorrectPassLbl
            // 
            this.incorrectPassLbl.AutoSize = true;
            this.incorrectPassLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.incorrectPassLbl.ForeColor = System.Drawing.Color.Red;
            this.incorrectPassLbl.Location = new System.Drawing.Point(135, 79);
            this.incorrectPassLbl.Name = "incorrectPassLbl";
            this.incorrectPassLbl.Size = new System.Drawing.Size(122, 16);
            this.incorrectPassLbl.TabIndex = 3;
            this.incorrectPassLbl.Text = "Incorrect Password";
            this.incorrectPassLbl.Visible = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 133);
            this.Controls.Add(this.incorrectPassLbl);
            this.Controls.Add(this.ConfirmBtn);
            this.Controls.Add(this.PasswordTxt);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PasswordTxt;
        private System.Windows.Forms.Button ConfirmBtn;
        private System.Windows.Forms.Label incorrectPassLbl;
    }
}