namespace sandGlass
{
    partial class Test
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
            this.panel33 = new System.Windows.Forms.Panel();
            this.btn_sdk_add = new System.Windows.Forms.Button();
            this.flowLayoutPanelSdk = new System.Windows.Forms.FlowLayoutPanel();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.sdk = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_fresh = new System.Windows.Forms.Button();
            this.installer11 = new sandGlass.Installer1();
            this.panel33.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel33
            // 
            this.panel33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel33.Controls.Add(this.btn_sdk_add);
            this.panel33.Controls.Add(this.flowLayoutPanelSdk);
            this.panel33.Controls.Add(this.label41);
            this.panel33.Controls.Add(this.label42);
            this.panel33.Location = new System.Drawing.Point(123, 12);
            this.panel33.Name = "panel33";
            this.panel33.Size = new System.Drawing.Size(344, 156);
            this.panel33.TabIndex = 107;
            // 
            // btn_sdk_add
            // 
            this.btn_sdk_add.Location = new System.Drawing.Point(164, 12);
            this.btn_sdk_add.Name = "btn_sdk_add";
            this.btn_sdk_add.Size = new System.Drawing.Size(75, 23);
            this.btn_sdk_add.TabIndex = 104;
            this.btn_sdk_add.Text = "添加";
            this.btn_sdk_add.UseVisualStyleBackColor = true;
            this.btn_sdk_add.Click += new System.EventHandler(this.btn_sdk_add_Click);
            // 
            // flowLayoutPanelSdk
            // 
            this.flowLayoutPanelSdk.AutoScroll = true;
            this.flowLayoutPanelSdk.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelSdk.Location = new System.Drawing.Point(18, 41);
            this.flowLayoutPanelSdk.Name = "flowLayoutPanelSdk";
            this.flowLayoutPanelSdk.Size = new System.Drawing.Size(298, 100);
            this.flowLayoutPanelSdk.TabIndex = 103;
            this.flowLayoutPanelSdk.WrapContents = false;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(129, 17);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(0, 12);
            this.label41.TabIndex = 102;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(16, 17);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(95, 12);
            this.label42.TabIndex = 100;
            this.label42.Text = "sdk强制版本控制";
            // 
            // sdk
            // 
            this.sdk.AutoSize = true;
            this.sdk.Location = new System.Drawing.Point(105, 256);
            this.sdk.Name = "sdk";
            this.sdk.Size = new System.Drawing.Size(23, 12);
            this.sdk.TabIndex = 105;
            this.sdk.Text = "sdk";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 256);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 108;
            this.label1.Text = "label1";
            // 
            // btn_fresh
            // 
            this.btn_fresh.Location = new System.Drawing.Point(501, 19);
            this.btn_fresh.Name = "btn_fresh";
            this.btn_fresh.Size = new System.Drawing.Size(75, 23);
            this.btn_fresh.TabIndex = 105;
            this.btn_fresh.Text = "刷新";
            this.btn_fresh.UseVisualStyleBackColor = true;
            this.btn_fresh.Click += new System.EventHandler(this.btn_fresh_Click);
            // 
            // Test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 476);
            this.Controls.Add(this.btn_fresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sdk);
            this.Controls.Add(this.panel33);
            this.Name = "Test";
            this.Text = "Test";
            this.Load += new System.EventHandler(this.Test_Load);
            this.panel33.ResumeLayout(false);
            this.panel33.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Installer1 installer11;
        private System.Windows.Forms.Panel panel33;
        private System.Windows.Forms.Button btn_sdk_add;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelSdk;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label sdk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_fresh;
    }
}