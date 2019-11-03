namespace SimpleClient
{
    partial class ClientForm
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
            this.btnSubmit = new System.Windows.Forms.Button();
            this.messageDisplay = new System.Windows.Forms.RichTextBox();
            this.inputChat = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(580, 376);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(208, 62);
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // messageDisplay
            // 
            this.messageDisplay.Location = new System.Drawing.Point(13, 13);
            this.messageDisplay.Name = "messageDisplay";
            this.messageDisplay.Size = new System.Drawing.Size(775, 357);
            this.messageDisplay.TabIndex = 1;
            this.messageDisplay.Text = "";
            // 
            // inputChat
            // 
            this.inputChat.Location = new System.Drawing.Point(13, 377);
            this.inputChat.Name = "inputChat";
            this.inputChat.Size = new System.Drawing.Size(561, 61);
            this.inputChat.TabIndex = 2;
            this.inputChat.Text = "";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.inputChat);
            this.Controls.Add(this.messageDisplay);
            this.Controls.Add(this.btnSubmit);
            this.Name = "ClientForm";
            this.Text = "Chat Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.RichTextBox messageDisplay;
        private System.Windows.Forms.RichTextBox inputChat;
    }
}