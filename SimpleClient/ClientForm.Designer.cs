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
            this.connectButton = new System.Windows.Forms.Button();
            this.nicknameTextBox = new System.Windows.Forms.RichTextBox();
            this.nicknameLabel = new System.Windows.Forms.Label();
            this.clientConnectLB = new System.Windows.Forms.ListBox();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.tankGameBtn = new System.Windows.Forms.Button();
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
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(13, 5);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(107, 41);
            this.connectButton.TabIndex = 4;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
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
            // clientConnectLB
            // 
            this.clientConnectLB.FormattingEnabled = true;
            this.clientConnectLB.Location = new System.Drawing.Point(864, 52);
            this.clientConnectLB.Name = "clientConnectLB";
            this.clientConnectLB.Size = new System.Drawing.Size(140, 446);
            this.clientConnectLB.TabIndex = 9;
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(864, 5);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(140, 41);
            this.disconnectButton.TabIndex = 10;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.disconnectButton_Click);
            // 
            // tankGameBtn
            // 
            this.tankGameBtn.Location = new System.Drawing.Point(865, 505);
            this.tankGameBtn.Name = "tankGameBtn";
            this.tankGameBtn.Size = new System.Drawing.Size(139, 55);
            this.tankGameBtn.TabIndex = 11;
            this.tankGameBtn.Text = "Play Tanks";
            this.tankGameBtn.UseVisualStyleBackColor = true;
            this.tankGameBtn.Visible = false;
            this.tankGameBtn.Click += new System.EventHandler(this.tankGameBtn_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 573);
            this.Controls.Add(this.tankGameBtn);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.clientConnectLB);
            this.Controls.Add(this.nicknameLabel);
            this.Controls.Add(this.nicknameTextBox);
            this.Controls.Add(this.connectButton);
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
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.RichTextBox nicknameTextBox;
        private System.Windows.Forms.Label nicknameLabel;
        private System.Windows.Forms.ListBox clientConnectLB;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button tankGameBtn;
    }
}