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
            this.chatRelay = new System.Windows.Forms.RichTextBox();
            this.inputChat = new System.Windows.Forms.RichTextBox();
            this.connectDcBtn = new System.Windows.Forms.Button();
            this.nicknameTextBox = new System.Windows.Forms.RichTextBox();
            this.nicknameLabel = new System.Windows.Forms.Label();
            this.activeClientList = new System.Windows.Forms.RichTextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(650, 499);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(208, 62);
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Visible = false;
            this.btnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // chatRelay
            // 
            this.chatRelay.Location = new System.Drawing.Point(12, 52);
            this.chatRelay.Name = "chatRelay";
            this.chatRelay.ReadOnly = true;
            this.chatRelay.Size = new System.Drawing.Size(846, 441);
            this.chatRelay.TabIndex = 3;
            this.chatRelay.Text = "";
            // 
            // inputChat
            // 
            this.inputChat.Location = new System.Drawing.Point(12, 499);
            this.inputChat.Name = "inputChat";
            this.inputChat.Size = new System.Drawing.Size(632, 61);
            this.inputChat.TabIndex = 2;
            this.inputChat.Text = "";
            this.inputChat.Visible = false;
            // 
            // connectDcBtn
            // 
            this.connectDcBtn.Location = new System.Drawing.Point(13, 5);
            this.connectDcBtn.Name = "connectDcBtn";
            this.connectDcBtn.Size = new System.Drawing.Size(107, 41);
            this.connectDcBtn.TabIndex = 4;
            this.connectDcBtn.Text = "Connect";
            this.connectDcBtn.UseVisualStyleBackColor = true;
            this.connectDcBtn.Click += new System.EventHandler(this.ConnectDcBtn_Click);
            // 
            // nicknameTextBox
            // 
            this.nicknameTextBox.Location = new System.Drawing.Point(127, 24);
            this.nicknameTextBox.Name = "nicknameTextBox";
            this.nicknameTextBox.Size = new System.Drawing.Size(170, 22);
            this.nicknameTextBox.TabIndex = 6;
            this.nicknameTextBox.Text = "";
            // 
            // nicknameLabel
            // 
            this.nicknameLabel.AutoSize = true;
            this.nicknameLabel.Location = new System.Drawing.Point(149, 5);
            this.nicknameLabel.Name = "nicknameLabel";
            this.nicknameLabel.Size = new System.Drawing.Size(122, 13);
            this.nicknameLabel.TabIndex = 7;
            this.nicknameLabel.Text = "Enter Desired Nickname";
            // 
            // activeClientList
            // 
            this.activeClientList.Location = new System.Drawing.Point(864, 52);
            this.activeClientList.Name = "activeClientList";
            this.activeClientList.ReadOnly = true;
            this.activeClientList.Size = new System.Drawing.Size(140, 441);
            this.activeClientList.TabIndex = 8;
            this.activeClientList.Text = "";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(581, 190);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 95);
            this.listBox1.TabIndex = 9;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 573);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.activeClientList);
            this.Controls.Add(this.nicknameLabel);
            this.Controls.Add(this.nicknameTextBox);
            this.Controls.Add(this.connectDcBtn);
            this.Controls.Add(this.inputChat);
            this.Controls.Add(this.chatRelay);
            this.Controls.Add(this.btnSubmit);
            this.Name = "ClientForm";
            this.Text = "Chat Application";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.RichTextBox chatRelay;
        private System.Windows.Forms.RichTextBox inputChat;
        private System.Windows.Forms.Button connectDcBtn;
        private System.Windows.Forms.RichTextBox nicknameTextBox;
        private System.Windows.Forms.Label nicknameLabel;
        private System.Windows.Forms.RichTextBox activeClientList;
        private System.Windows.Forms.ListBox listBox1;
    }
}