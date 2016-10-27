namespace sandGlass
{
    partial class ApkForm
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
            this.text_new_package = new System.Windows.Forms.TextBox();
            this.but_getapk = new System.Windows.Forms.Button();
            this.but_ok = new System.Windows.Forms.Button();
            this.getApk = new System.Windows.Forms.OpenFileDialog();
            this.glog = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // text_new_package
            // 
            this.text_new_package.Location = new System.Drawing.Point(92, 51);
            this.text_new_package.Name = "text_new_package";
            this.text_new_package.Size = new System.Drawing.Size(305, 21);
            this.text_new_package.TabIndex = 0;
            // 
            // but_getapk
            // 
            this.but_getapk.Location = new System.Drawing.Point(92, 12);
            this.but_getapk.Name = "but_getapk";
            this.but_getapk.Size = new System.Drawing.Size(75, 23);
            this.but_getapk.TabIndex = 1;
            this.but_getapk.Text = "选择";
            this.but_getapk.UseVisualStyleBackColor = true;
            this.but_getapk.Click += new System.EventHandler(this.but_getapk_Click);
            // 
            // but_ok
            // 
            this.but_ok.Location = new System.Drawing.Point(92, 88);
            this.but_ok.Name = "but_ok";
            this.but_ok.Size = new System.Drawing.Size(75, 23);
            this.but_ok.TabIndex = 2;
            this.but_ok.Text = "确定";
            this.but_ok.UseVisualStyleBackColor = true;
            this.but_ok.Click += new System.EventHandler(this.but_ok_Click);
            // 
            // getApk
            // 
            this.getApk.Filter = "选择文件|*.apk";
            // 
            // glog
            // 
            this.glog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.glog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.glog.Location = new System.Drawing.Point(0, 195);
            this.glog.Name = "glog";
            this.glog.ReadOnly = true;
            this.glog.Size = new System.Drawing.Size(568, 195);
            this.glog.TabIndex = 66;
            this.glog.Text = "";
            // 
            // ApkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 390);
            this.Controls.Add(this.glog);
            this.Controls.Add(this.but_ok);
            this.Controls.Add(this.but_getapk);
            this.Controls.Add(this.text_new_package);
            this.Name = "ApkForm";
            this.Text = "ApkForm";
            this.Load += new System.EventHandler(this.ApkForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox text_new_package;
        private System.Windows.Forms.Button but_getapk;
        private System.Windows.Forms.Button but_ok;
        private System.Windows.Forms.OpenFileDialog getApk;
        private System.Windows.Forms.RichTextBox glog;
    }
}