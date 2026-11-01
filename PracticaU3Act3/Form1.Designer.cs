namespace PracticaU3Act3
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            tabControl = new System.Windows.Forms.TabControl();
            tabBrowser = new System.Windows.Forms.TabPage();
            lblAddress = new System.Windows.Forms.Label();
            txtAddress = new System.Windows.Forms.TextBox();
            btnGo = new System.Windows.Forms.Button();
            btnBack = new System.Windows.Forms.Button();
            btnForward = new System.Windows.Forms.Button();
            btnClear = new System.Windows.Forms.Button();
            lblSamples = new System.Windows.Forms.Label();
            btnSample1 = new System.Windows.Forms.Button();
            btnSample2 = new System.Windows.Forms.Button();
            btnSample3 = new System.Windows.Forms.Button();
            btnSample4 = new System.Windows.Forms.Button();
            btnSample5 = new System.Windows.Forms.Button();
            lblStatus = new System.Windows.Forms.Label();
            pnlPage = new System.Windows.Forms.Panel();
            lblPageTitle = new System.Windows.Forms.Label();
            lblPageUrl = new System.Windows.Forms.Label();
            lblInfo = new System.Windows.Forms.Label();
            picStacks = new System.Windows.Forms.PictureBox();
            tabTests = new System.Windows.Forms.TabPage();
            txtTests = new System.Windows.Forms.TextBox();
            btnTests = new System.Windows.Forms.Button();
            tabControl.SuspendLayout();
            tabBrowser.SuspendLayout();
            pnlPage.SuspendLayout();
            tabTests.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picStacks).BeginInit();
            SuspendLayout();
            //
            // tabControl
            //
            tabControl.Controls.Add(tabBrowser);
            tabControl.Controls.Add(tabTests);
            tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl.Location = new System.Drawing.Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(1040, 620);
            tabControl.TabIndex = 0;
            //
            // tabBrowser
            //
            tabBrowser.Controls.Add(lblAddress);
            tabBrowser.Controls.Add(txtAddress);
            tabBrowser.Controls.Add(btnGo);
            tabBrowser.Controls.Add(btnBack);
            tabBrowser.Controls.Add(btnForward);
            tabBrowser.Controls.Add(btnClear);
            tabBrowser.Controls.Add(lblSamples);
            tabBrowser.Controls.Add(btnSample1);
            tabBrowser.Controls.Add(btnSample2);
            tabBrowser.Controls.Add(btnSample3);
            tabBrowser.Controls.Add(btnSample4);
            tabBrowser.Controls.Add(btnSample5);
            tabBrowser.Controls.Add(lblStatus);
            tabBrowser.Controls.Add(pnlPage);
            tabBrowser.Controls.Add(lblInfo);
            tabBrowser.Controls.Add(picStacks);
            tabBrowser.Location = new System.Drawing.Point(4, 24);
            tabBrowser.Name = "tabBrowser";
            tabBrowser.Padding = new System.Windows.Forms.Padding(3);
            tabBrowser.Size = new System.Drawing.Size(1032, 592);
            tabBrowser.TabIndex = 0;
            tabBrowser.Text = "Navegador";
            tabBrowser.UseVisualStyleBackColor = true;
            //
            // lblAddress
            //
            lblAddress.AutoSize = true;
            lblAddress.Location = new System.Drawing.Point(12, 16);
            lblAddress.Name = "lblAddress";
            lblAddress.TabIndex = 0;
            lblAddress.Text = "Dirección:";
            //
            // txtAddress
            //
            txtAddress.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            txtAddress.Location = new System.Drawing.Point(80, 13);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new System.Drawing.Size(545, 23);
            txtAddress.TabIndex = 1;
            txtAddress.KeyDown += txtAddress_KeyDown;
            //
            // btnGo
            //
            btnGo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnGo.Location = new System.Drawing.Point(630, 10);
            btnGo.Name = "btnGo";
            btnGo.Size = new System.Drawing.Size(60, 28);
            btnGo.TabIndex = 2;
            btnGo.Text = "Ir";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += btnGo_Click;
            //
            // btnBack
            //
            btnBack.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnBack.Location = new System.Drawing.Point(695, 10);
            btnBack.Name = "btnBack";
            btnBack.Size = new System.Drawing.Size(100, 28);
            btnBack.TabIndex = 3;
            btnBack.Text = "Atrás";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            //
            // btnForward
            //
            btnForward.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnForward.Location = new System.Drawing.Point(800, 10);
            btnForward.Name = "btnForward";
            btnForward.Size = new System.Drawing.Size(100, 28);
            btnForward.TabIndex = 4;
            btnForward.Text = "Adelante";
            btnForward.UseVisualStyleBackColor = true;
            btnForward.Click += btnForward_Click;
            //
            // btnClear
            //
            btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnClear.Location = new System.Drawing.Point(905, 10);
            btnClear.Name = "btnClear";
            btnClear.Size = new System.Drawing.Size(112, 28);
            btnClear.TabIndex = 5;
            btnClear.Text = "Vaciar historial";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            //
            // lblSamples
            //
            lblSamples.AutoSize = true;
            lblSamples.Location = new System.Drawing.Point(12, 54);
            lblSamples.Name = "lblSamples";
            lblSamples.TabIndex = 6;
            lblSamples.Text = "Enlaces de ejemplo:";
            //
            // btnSample1
            //
            btnSample1.Location = new System.Drawing.Point(140, 48);
            btnSample1.Name = "btnSample1";
            btnSample1.Size = new System.Drawing.Size(128, 28);
            btnSample1.TabIndex = 7;
            btnSample1.Text = "wikipedia.org";
            btnSample1.UseVisualStyleBackColor = true;
            btnSample1.Click += btnSample_Click;
            //
            // btnSample2
            //
            btnSample2.Location = new System.Drawing.Point(275, 48);
            btnSample2.Name = "btnSample2";
            btnSample2.Size = new System.Drawing.Size(128, 28);
            btnSample2.TabIndex = 8;
            btnSample2.Text = "github.com";
            btnSample2.UseVisualStyleBackColor = true;
            btnSample2.Click += btnSample_Click;
            //
            // btnSample3
            //
            btnSample3.Location = new System.Drawing.Point(410, 48);
            btnSample3.Name = "btnSample3";
            btnSample3.Size = new System.Drawing.Size(128, 28);
            btnSample3.TabIndex = 9;
            btnSample3.Text = "tecnm.mx";
            btnSample3.UseVisualStyleBackColor = true;
            btnSample3.Click += btnSample_Click;
            //
            // btnSample4
            //
            btnSample4.Location = new System.Drawing.Point(545, 48);
            btnSample4.Name = "btnSample4";
            btnSample4.Size = new System.Drawing.Size(128, 28);
            btnSample4.TabIndex = 10;
            btnSample4.Text = "mozilla.org";
            btnSample4.UseVisualStyleBackColor = true;
            btnSample4.Click += btnSample_Click;
            //
            // btnSample5
            //
            btnSample5.Location = new System.Drawing.Point(680, 48);
            btnSample5.Name = "btnSample5";
            btnSample5.Size = new System.Drawing.Size(128, 28);
            btnSample5.TabIndex = 11;
            btnSample5.Text = "ejemplo.com";
            btnSample5.UseVisualStyleBackColor = true;
            btnSample5.Click += btnSample_Click;
            //
            // lblStatus
            //
            lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblStatus.Location = new System.Drawing.Point(12, 86);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(1005, 24);
            lblStatus.TabIndex = 12;
            //
            // pnlPage
            //
            pnlPage.Controls.Add(lblPageTitle);
            pnlPage.Controls.Add(lblPageUrl);
            pnlPage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            pnlPage.BackColor = System.Drawing.Color.White;
            pnlPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlPage.Location = new System.Drawing.Point(12, 116);
            pnlPage.Name = "pnlPage";
            pnlPage.Size = new System.Drawing.Size(1005, 70);
            pnlPage.TabIndex = 13;
            //
            // lblPageTitle
            //
            lblPageTitle.AutoSize = true;
            lblPageTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            lblPageTitle.Location = new System.Drawing.Point(10, 8);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.TabIndex = 0;
            //
            // lblPageUrl
            //
            lblPageUrl.AutoSize = true;
            lblPageUrl.Location = new System.Drawing.Point(12, 42);
            lblPageUrl.Name = "lblPageUrl";
            lblPageUrl.TabIndex = 1;
            //
            // lblInfo
            //
            lblInfo.AutoSize = true;
            lblInfo.Location = new System.Drawing.Point(12, 196);
            lblInfo.Name = "lblInfo";
            lblInfo.TabIndex = 14;
            //
            // picStacks
            //
            picStacks.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            picStacks.BackColor = System.Drawing.Color.White;
            picStacks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            picStacks.Location = new System.Drawing.Point(12, 222);
            picStacks.Name = "picStacks";
            picStacks.Size = new System.Drawing.Size(1005, 360);
            picStacks.TabIndex = 15;
            picStacks.TabStop = false;
            picStacks.Paint += picStacks_Paint;
            picStacks.Resize += picStacks_Resize;
            //
            // tabTests
            //
            tabTests.Controls.Add(txtTests);
            tabTests.Controls.Add(btnTests);
            tabTests.Location = new System.Drawing.Point(4, 24);
            tabTests.Name = "tabTests";
            tabTests.Padding = new System.Windows.Forms.Padding(3);
            tabTests.Size = new System.Drawing.Size(1032, 592);
            tabTests.TabIndex = 1;
            tabTests.Text = "Pruebas y mediciones";
            tabTests.UseVisualStyleBackColor = true;
            //
            // txtTests
            //
            txtTests.Dock = System.Windows.Forms.DockStyle.Fill;
            txtTests.Font = new System.Drawing.Font("Consolas", 10F);
            txtTests.Location = new System.Drawing.Point(3, 35);
            txtTests.Multiline = true;
            txtTests.Name = "txtTests";
            txtTests.ReadOnly = true;
            txtTests.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            txtTests.Size = new System.Drawing.Size(1026, 554);
            txtTests.TabIndex = 1;
            txtTests.WordWrap = false;
            //
            // btnTests
            //
            btnTests.Dock = System.Windows.Forms.DockStyle.Top;
            btnTests.Location = new System.Drawing.Point(3, 3);
            btnTests.Name = "btnTests";
            btnTests.Size = new System.Drawing.Size(1026, 32);
            btnTests.TabIndex = 0;
            btnTests.Text = "Ejecutar pruebas y mediciones";
            btnTests.UseVisualStyleBackColor = true;
            btnTests.Click += btnTests_Click;
            //
            // Form1
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1040, 620);
            Controls.Add(tabControl);
            MinimumSize = new System.Drawing.Size(860, 520);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Historial de navegación con pilas";
            Load += Form1_Load;
            tabControl.ResumeLayout(false);
            tabBrowser.ResumeLayout(false);
            pnlPage.ResumeLayout(false);
            tabTests.ResumeLayout(false);
            tabBrowser.PerformLayout();
            pnlPage.PerformLayout();
            tabTests.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picStacks).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBrowser;
        private System.Windows.Forms.Label lblAddress;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblSamples;
        private System.Windows.Forms.Button btnSample1;
        private System.Windows.Forms.Button btnSample2;
        private System.Windows.Forms.Button btnSample3;
        private System.Windows.Forms.Button btnSample4;
        private System.Windows.Forms.Button btnSample5;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel pnlPage;
        private System.Windows.Forms.Label lblPageTitle;
        private System.Windows.Forms.Label lblPageUrl;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.PictureBox picStacks;
        private System.Windows.Forms.TabPage tabTests;
        private System.Windows.Forms.TextBox txtTests;
        private System.Windows.Forms.Button btnTests;
    }
}