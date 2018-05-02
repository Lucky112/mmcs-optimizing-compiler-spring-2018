namespace Compiler.IDE
{
    partial class MainWindow
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mainBox = new System.Windows.Forms.GroupBox();
            this.toggleOptsButton = new System.Windows.Forms.Button();
            this.optsList = new System.Windows.Forms.CheckedListBox();
            this.outBox = new System.Windows.Forms.GroupBox();
            this.outTextBox = new System.Windows.Forms.TextBox();
            this.tabsControl = new System.Windows.Forms.TabControl();
            this.inputPage = new System.Windows.Forms.TabPage();
            this.ILCodePage = new System.Windows.Forms.TabPage();
            this.threeAddrPage = new System.Windows.Forms.TabPage();
            this.CFGPage = new System.Windows.Forms.TabPage();
            this.ASTPage = new System.Windows.Forms.TabPage();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.button1 = new System.Windows.Forms.Button();
            this.compileButton = new System.Windows.Forms.Button();
            this.optimizationsLabel = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.mainBox.SuspendLayout();
            this.outBox.SuspendLayout();
            this.tabsControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1133, 38);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(56, 34);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(155, 34);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(155, 34);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(82, 34);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(161, 34);
            this.aboutToolStripMenuItem1.Text = "About";
            // 
            // mainBox
            // 
            this.mainBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainBox.Controls.Add(this.optimizationsLabel);
            this.mainBox.Controls.Add(this.compileButton);
            this.mainBox.Controls.Add(this.button1);
            this.mainBox.Controls.Add(this.toggleOptsButton);
            this.mainBox.Controls.Add(this.optsList);
            this.mainBox.Location = new System.Drawing.Point(12, 41);
            this.mainBox.Name = "mainBox";
            this.mainBox.Size = new System.Drawing.Size(264, 598);
            this.mainBox.TabIndex = 1;
            this.mainBox.TabStop = false;
            this.mainBox.Text = "Control Panel";
            // 
            // toggleOptsButton
            // 
            this.toggleOptsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleOptsButton.Location = new System.Drawing.Point(6, 28);
            this.toggleOptsButton.Name = "toggleOptsButton";
            this.toggleOptsButton.Size = new System.Drawing.Size(252, 40);
            this.toggleOptsButton.TabIndex = 3;
            this.toggleOptsButton.Text = "Toggle All";
            this.toggleOptsButton.UseVisualStyleBackColor = true;
            // 
            // optsList
            // 
            this.optsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.optsList.FormattingEnabled = true;
            this.optsList.Location = new System.Drawing.Point(6, 140);
            this.optsList.Name = "optsList";
            this.optsList.Size = new System.Drawing.Size(252, 412);
            this.optsList.TabIndex = 2;
            // 
            // outBox
            // 
            this.outBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outBox.Controls.Add(this.outTextBox);
            this.outBox.Location = new System.Drawing.Point(7, 15);
            this.outBox.Name = "outBox";
            this.outBox.Size = new System.Drawing.Size(824, 110);
            this.outBox.TabIndex = 2;
            this.outBox.TabStop = false;
            this.outBox.Text = "Output";
            // 
            // outTextBox
            // 
            this.outTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outTextBox.Location = new System.Drawing.Point(6, 28);
            this.outTextBox.Multiline = true;
            this.outTextBox.Name = "outTextBox";
            this.outTextBox.Size = new System.Drawing.Size(812, 76);
            this.outTextBox.TabIndex = 0;
            // 
            // tabsControl
            // 
            this.tabsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsControl.Controls.Add(this.inputPage);
            this.tabsControl.Controls.Add(this.ILCodePage);
            this.tabsControl.Controls.Add(this.threeAddrPage);
            this.tabsControl.Controls.Add(this.CFGPage);
            this.tabsControl.Controls.Add(this.ASTPage);
            this.tabsControl.Location = new System.Drawing.Point(3, 3);
            this.tabsControl.Name = "tabsControl";
            this.tabsControl.SelectedIndex = 0;
            this.tabsControl.Size = new System.Drawing.Size(832, 462);
            this.tabsControl.TabIndex = 3;
            // 
            // inputPage
            // 
            this.inputPage.Location = new System.Drawing.Point(4, 33);
            this.inputPage.Name = "inputPage";
            this.inputPage.Padding = new System.Windows.Forms.Padding(3);
            this.inputPage.Size = new System.Drawing.Size(824, 425);
            this.inputPage.TabIndex = 0;
            this.inputPage.Text = "Input";
            this.inputPage.UseVisualStyleBackColor = true;
            // 
            // ILCodePage
            // 
            this.ILCodePage.Location = new System.Drawing.Point(4, 33);
            this.ILCodePage.Name = "ILCodePage";
            this.ILCodePage.Padding = new System.Windows.Forms.Padding(3);
            this.ILCodePage.Size = new System.Drawing.Size(881, 361);
            this.ILCodePage.TabIndex = 1;
            this.ILCodePage.Text = "IL-Code";
            this.ILCodePage.UseVisualStyleBackColor = true;
            // 
            // threeAddrPage
            // 
            this.threeAddrPage.Location = new System.Drawing.Point(4, 33);
            this.threeAddrPage.Name = "threeAddrPage";
            this.threeAddrPage.Padding = new System.Windows.Forms.Padding(3);
            this.threeAddrPage.Size = new System.Drawing.Size(881, 361);
            this.threeAddrPage.TabIndex = 2;
            this.threeAddrPage.Text = "3-Address Code";
            this.threeAddrPage.UseVisualStyleBackColor = true;
            // 
            // CFGPage
            // 
            this.CFGPage.Location = new System.Drawing.Point(4, 33);
            this.CFGPage.Name = "CFGPage";
            this.CFGPage.Padding = new System.Windows.Forms.Padding(3);
            this.CFGPage.Size = new System.Drawing.Size(881, 361);
            this.CFGPage.TabIndex = 3;
            this.CFGPage.Text = "CFG";
            this.CFGPage.UseVisualStyleBackColor = true;
            // 
            // ASTPage
            // 
            this.ASTPage.Location = new System.Drawing.Point(4, 33);
            this.ASTPage.Name = "ASTPage";
            this.ASTPage.Padding = new System.Windows.Forms.Padding(3);
            this.ASTPage.Size = new System.Drawing.Size(881, 361);
            this.ASTPage.TabIndex = 4;
            this.ASTPage.Text = "AST";
            this.ASTPage.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(282, 41);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tabsControl);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.outBox);
            this.splitContainer.Size = new System.Drawing.Size(839, 598);
            this.splitContainer.SplitterDistance = 466;
            this.splitContainer.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(135, 556);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 36);
            this.button1.TabIndex = 4;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // compileButton
            // 
            this.compileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.compileButton.Location = new System.Drawing.Point(6, 556);
            this.compileButton.Name = "compileButton";
            this.compileButton.Size = new System.Drawing.Size(123, 36);
            this.compileButton.TabIndex = 5;
            this.compileButton.Text = "Compile";
            this.compileButton.UseVisualStyleBackColor = true;
            // 
            // optimizationsLabel
            // 
            this.optimizationsLabel.AutoSize = true;
            this.optimizationsLabel.Location = new System.Drawing.Point(6, 112);
            this.optimizationsLabel.Name = "optimizationsLabel";
            this.optimizationsLabel.Size = new System.Drawing.Size(130, 25);
            this.optimizationsLabel.TabIndex = 6;
            this.optimizationsLabel.Text = "Optimizations";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 651);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.mainBox);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainWindow";
            this.Text = "Compiler IDE";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainBox.ResumeLayout(false);
            this.mainBox.PerformLayout();
            this.outBox.ResumeLayout(false);
            this.outBox.PerformLayout();
            this.tabsControl.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
        private System.Windows.Forms.GroupBox mainBox;
        private System.Windows.Forms.Button toggleOptsButton;
        private System.Windows.Forms.CheckedListBox optsList;
        private System.Windows.Forms.GroupBox outBox;
        private System.Windows.Forms.TextBox outTextBox;
        private System.Windows.Forms.TabControl tabsControl;
        private System.Windows.Forms.TabPage inputPage;
        private System.Windows.Forms.TabPage ILCodePage;
        private System.Windows.Forms.TabPage threeAddrPage;
        private System.Windows.Forms.TabPage CFGPage;
        private System.Windows.Forms.TabPage ASTPage;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button compileButton;
        private System.Windows.Forms.Label optimizationsLabel;
    }
}