namespace PortObserverToAccessControl
{
    partial class FormPOTAC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPOTAC));
            this.WebAC = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // WebAC
            // 
            this.WebAC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebAC.IsWebBrowserContextMenuEnabled = false;
            this.WebAC.Location = new System.Drawing.Point(0, 0);
            this.WebAC.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebAC.Name = "WebAC";
            this.WebAC.ScriptErrorsSuppressed = true;
            this.WebAC.Size = new System.Drawing.Size(700, 378);
            this.WebAC.TabIndex = 0;
            this.WebAC.Url = new System.Uri("http://localhost:4034/", System.UriKind.Absolute);
            // 
            // FormPOTAC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 378);
            this.Controls.Add(this.WebAC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(720, 420);
            this.Name = "FormPOTAC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "POTAC (AccessControl)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser WebAC;
    }
}

