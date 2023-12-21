namespace InteractiveColorbar
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ilPanel1 = new ILNumerics.Drawing.Panel();
            this.SuspendLayout();


            // ComboBox
            this.comboBoxFunctions = new System.Windows.Forms.ComboBox();
            this.comboBoxFunctions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFunctions.FormattingEnabled = true;
            this.comboBoxFunctions.Items.AddRange(new object[] {
                "F1 / Rastrigin Function",
                "F2 / Rosenbrock Function",
                "F3 / Sphere Function",
                "F4 / Schwefel Function",
                "F5 / Michalewicz Function",
                "F6 / Styblinski – Tang Function",
                "F7 / Alpine01 Function",
                "F8 / Sum of different powers function",
                "F9 / Dixon - price function",
                "F10 / CosineMixture Function",
                "F11 / Mishra07 Function",
                "F12 / Mishra11 Function",
                "F13 / Plateau Function",
                "F14 / Qing Function",
                "F15 / Rana Function",
                "F16 / Trid Function",
                "F17 / YaoLiu09 Function",
                "F18 / Bent Cigar Function",
                "F19 / Deb's Function No.01",
                "F20 / Quartic Function",
                "F21 / Hyper-Ellipsoid Function",
                "F22 / Egg-Holder Function",
                "F23 / Chung-Reynolds' Function",
                "F24 / Moved-Axis Parallel Hyper-Ellipsoid Function",
                "F25 / Generalized Schwefel's Function No.2.26",
                 // ... další funkce ...
                });
            this.comboBoxFunctions.Location = new System.Drawing.Point(12, 12);
            this.comboBoxFunctions.Name = "comboBoxFunctions";
            this.comboBoxFunctions.Size = new System.Drawing.Size(300, 21);
            this.comboBoxFunctions.TabIndex = 1;
            this.comboBoxFunctions.SelectedIndex = 0;
            this.comboBoxFunctions.SelectedIndexChanged += new System.EventHandler(this.comboBoxFunctions_SelectedIndexChanged);




            // 
            // ilPanel1
            // 
            this.ilPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ilPanel1.RendererType = ILNumerics.Drawing.RendererTypes.OpenGL;
            this.ilPanel1.Editor = null;
            this.ilPanel1.Location = new System.Drawing.Point(0, 0);
            this.ilPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ilPanel1.Name = "ilPanel1";
            this.ilPanel1.Rectangle = ((System.Drawing.RectangleF)(resources.GetObject("ilPanel1.Rectangle")));
            this.ilPanel1.ShowUIControls = false;
            this.ilPanel1.Size = new System.Drawing.Size(751, 520);
            this.ilPanel1.TabIndex = 0;
            this.ilPanel1.Timeout = ((uint)(0u));
            this.ilPanel1.Load += new System.EventHandler(this.ilPanel1_Load);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 520);
            this.Controls.Add(this.comboBoxFunctions);
            this.Controls.Add(this.ilPanel1);
            this.Name = "Form1";
            this.Text = "Funkcions graph";
            this.ResumeLayout(false);

        }

        #endregion

        private ILNumerics.Drawing.Panel ilPanel1;
        private System.Windows.Forms.ComboBox comboBoxFunctions;
    }
}

