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
            this.optimizationsLabel = new System.Windows.Forms.Label();
            this.compileButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.toggleOptsButton = new System.Windows.Forms.Button();
            this.optsList = new System.Windows.Forms.CheckedListBox();
            this.outBox = new System.Windows.Forms.GroupBox();
            this.outTextBox = new System.Windows.Forms.TextBox();
            this.tabsControl = new System.Windows.Forms.TabControl();
            this.inputPage = new System.Windows.Forms.TabPage();
            this.inputTextBox = new System.Windows.Forms.RichTextBox();
            this.ILCodePage = new System.Windows.Forms.TabPage();
            this.ILCodeTextBox = new System.Windows.Forms.RichTextBox();
            this.threeAddrPage = new System.Windows.Forms.TabPage();
            this.threeAddrTextBox = new System.Windows.Forms.RichTextBox();
            this.CFGPage = new System.Windows.Forms.TabPage();
            this.CFGPictureBox = new System.Windows.Forms.PictureBox();
            this.ASTPage = new System.Windows.Forms.TabPage();
            this.ASTPictureBox = new System.Windows.Forms.PictureBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.menuStrip1.SuspendLayout();
            this.mainBox.SuspendLayout();
            this.outBox.SuspendLayout();
            this.tabsControl.SuspendLayout();
            this.inputPage.SuspendLayout();
            this.ILCodePage.SuspendLayout();
            this.threeAddrPage.SuspendLayout();
            this.CFGPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CFGPictureBox)).BeginInit();
            this.ASTPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ASTPictureBox)).BeginInit();
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
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(618, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            // 
            // mainBox
            // 
            this.mainBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.mainBox.Controls.Add(this.optimizationsLabel);
            this.mainBox.Controls.Add(this.compileButton);
            this.mainBox.Controls.Add(this.runButton);
            this.mainBox.Controls.Add(this.toggleOptsButton);
            this.mainBox.Controls.Add(this.optsList);
            this.mainBox.Location = new System.Drawing.Point(7, 22);
            this.mainBox.Margin = new System.Windows.Forms.Padding(2);
            this.mainBox.Name = "mainBox";
            this.mainBox.Padding = new System.Windows.Forms.Padding(2);
            this.mainBox.Size = new System.Drawing.Size(144, 324);
            this.mainBox.TabIndex = 1;
            this.mainBox.TabStop = false;
            this.mainBox.Text = "Control Panel";
            // 
            // optimizationsLabel
            // 
            this.optimizationsLabel.AutoSize = true;
            this.optimizationsLabel.Location = new System.Drawing.Point(4, 46);
            this.optimizationsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.optimizationsLabel.Name = "optimizationsLabel";
            this.optimizationsLabel.Size = new System.Drawing.Size(69, 13);
            this.optimizationsLabel.TabIndex = 6;
            this.optimizationsLabel.Text = "Optimizations";
            // 
            // compileButton
            // 
            this.compileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.compileButton.Location = new System.Drawing.Point(3, 301);
            this.compileButton.Margin = new System.Windows.Forms.Padding(2);
            this.compileButton.Name = "compileButton";
            this.compileButton.Size = new System.Drawing.Size(67, 20);
            this.compileButton.TabIndex = 5;
            this.compileButton.Text = "Compile";
            this.compileButton.UseVisualStyleBackColor = true;
            // 
            // runButton
            // 
            this.runButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.runButton.Enabled = false;
            this.runButton.Location = new System.Drawing.Point(74, 301);
            this.runButton.Margin = new System.Windows.Forms.Padding(2);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(67, 20);
            this.runButton.TabIndex = 4;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            // 
            // toggleOptsButton
            // 
            this.toggleOptsButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleOptsButton.Location = new System.Drawing.Point(3, 15);
            this.toggleOptsButton.Margin = new System.Windows.Forms.Padding(2);
            this.toggleOptsButton.Name = "toggleOptsButton";
            this.toggleOptsButton.Size = new System.Drawing.Size(137, 22);
            this.toggleOptsButton.TabIndex = 3;
            this.toggleOptsButton.Text = "Toggle All";
            this.toggleOptsButton.UseVisualStyleBackColor = true;
            // 
            // optsList
            // 
            this.optsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.optsList.FormattingEnabled = true;
            this.optsList.Location = new System.Drawing.Point(3, 61);
            this.optsList.Margin = new System.Windows.Forms.Padding(2);
            this.optsList.Name = "optsList";
            this.optsList.Size = new System.Drawing.Size(139, 229);
            this.optsList.TabIndex = 2;
            // 
            // outBox
            // 
            this.outBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outBox.Controls.Add(this.outTextBox);
            this.outBox.Location = new System.Drawing.Point(4, 8);
            this.outBox.Margin = new System.Windows.Forms.Padding(2);
            this.outBox.Name = "outBox";
            this.outBox.Padding = new System.Windows.Forms.Padding(2);
            this.outBox.Size = new System.Drawing.Size(449, 68);
            this.outBox.TabIndex = 2;
            this.outBox.TabStop = false;
            this.outBox.Text = "Output";
            // 
            // outTextBox
            // 
            this.outTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outTextBox.Location = new System.Drawing.Point(3, 15);
            this.outTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.outTextBox.Multiline = true;
            this.outTextBox.Name = "outTextBox";
            this.outTextBox.ReadOnly = true;
            this.outTextBox.Size = new System.Drawing.Size(445, 51);
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
            this.tabsControl.Location = new System.Drawing.Point(2, 2);
            this.tabsControl.Margin = new System.Windows.Forms.Padding(2);
            this.tabsControl.Name = "tabsControl";
            this.tabsControl.SelectedIndex = 0;
            this.tabsControl.Size = new System.Drawing.Size(454, 249);
            this.tabsControl.TabIndex = 3;
            // 
            // inputPage
            // 
            this.inputPage.Controls.Add(this.inputTextBox);
            this.inputPage.Location = new System.Drawing.Point(4, 22);
            this.inputPage.Margin = new System.Windows.Forms.Padding(2);
            this.inputPage.Name = "inputPage";
            this.inputPage.Padding = new System.Windows.Forms.Padding(2);
            this.inputPage.Size = new System.Drawing.Size(446, 223);
            this.inputPage.TabIndex = 0;
            this.inputPage.Text = "Input";
            this.inputPage.UseVisualStyleBackColor = true;
            // 
            // inputTextBox
            // 
            this.inputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputTextBox.Location = new System.Drawing.Point(2, 2);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(442, 219);
            this.inputTextBox.TabIndex = 0;
            this.inputTextBox.Text = "";
            // 
            // ILCodePage
            // 
            this.ILCodePage.Controls.Add(this.ILCodeTextBox);
            this.ILCodePage.Location = new System.Drawing.Point(4, 22);
            this.ILCodePage.Margin = new System.Windows.Forms.Padding(2);
            this.ILCodePage.Name = "ILCodePage";
            this.ILCodePage.Padding = new System.Windows.Forms.Padding(2);
            this.ILCodePage.Size = new System.Drawing.Size(446, 223);
            this.ILCodePage.TabIndex = 1;
            this.ILCodePage.Text = "IL-Code";
            this.ILCodePage.UseVisualStyleBackColor = true;
            // 
            // ILCodeTextBox
            // 
            this.ILCodeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ILCodeTextBox.Location = new System.Drawing.Point(2, 2);
            this.ILCodeTextBox.Name = "ILCodeTextBox";
            this.ILCodeTextBox.ReadOnly = true;
            this.ILCodeTextBox.Size = new System.Drawing.Size(442, 219);
            this.ILCodeTextBox.TabIndex = 0;
            this.ILCodeTextBox.Text = "";
            // 
            // threeAddrPage
            // 
            this.threeAddrPage.Controls.Add(this.threeAddrTextBox);
            this.threeAddrPage.Location = new System.Drawing.Point(4, 22);
            this.threeAddrPage.Margin = new System.Windows.Forms.Padding(2);
            this.threeAddrPage.Name = "threeAddrPage";
            this.threeAddrPage.Padding = new System.Windows.Forms.Padding(2);
            this.threeAddrPage.Size = new System.Drawing.Size(446, 223);
            this.threeAddrPage.TabIndex = 2;
            this.threeAddrPage.Text = "3-Address Code";
            this.threeAddrPage.UseVisualStyleBackColor = true;
            // 
            // threeAddrTextBox
            // 
            this.threeAddrTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.threeAddrTextBox.Location = new System.Drawing.Point(2, 2);
            this.threeAddrTextBox.Name = "threeAddrTextBox";
            this.threeAddrTextBox.ReadOnly = true;
            this.threeAddrTextBox.Size = new System.Drawing.Size(442, 219);
            this.threeAddrTextBox.TabIndex = 0;
            this.threeAddrTextBox.Text = "";
            // 
            // CFGPage
            // 
            this.CFGPage.Controls.Add(this.CFGPictureBox);
            this.CFGPage.Location = new System.Drawing.Point(4, 22);
            this.CFGPage.Margin = new System.Windows.Forms.Padding(2);
            this.CFGPage.Name = "CFGPage";
            this.CFGPage.Padding = new System.Windows.Forms.Padding(2);
            this.CFGPage.Size = new System.Drawing.Size(446, 223);
            this.CFGPage.TabIndex = 3;
            this.CFGPage.Text = "CFG";
            this.CFGPage.UseVisualStyleBackColor = true;
            // 
            // CFGPictureBox
            // 
            this.CFGPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CFGPictureBox.Location = new System.Drawing.Point(2, 2);
            this.CFGPictureBox.Name = "CFGPictureBox";
            this.CFGPictureBox.Size = new System.Drawing.Size(442, 219);
            this.CFGPictureBox.TabIndex = 0;
            this.CFGPictureBox.TabStop = false;
            // 
            // ASTPage
            // 
            this.ASTPage.Controls.Add(this.ASTPictureBox);
            this.ASTPage.Location = new System.Drawing.Point(4, 22);
            this.ASTPage.Margin = new System.Windows.Forms.Padding(2);
            this.ASTPage.Name = "ASTPage";
            this.ASTPage.Padding = new System.Windows.Forms.Padding(2);
            this.ASTPage.Size = new System.Drawing.Size(446, 223);
            this.ASTPage.TabIndex = 4;
            this.ASTPage.Text = "AST";
            this.ASTPage.UseVisualStyleBackColor = true;
            // 
            // ASTPictureBox
            // 
            this.ASTPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ASTPictureBox.Location = new System.Drawing.Point(2, 2);
            this.ASTPictureBox.Name = "ASTPictureBox";
            this.ASTPictureBox.Size = new System.Drawing.Size(442, 219);
            this.ASTPictureBox.TabIndex = 0;
            this.ASTPictureBox.TabStop = false;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(154, 22);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(2);
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
            this.splitContainer.Size = new System.Drawing.Size(458, 324);
            this.splitContainer.SplitterDistance = 251;
            this.splitContainer.SplitterWidth = 2;
            this.splitContainer.TabIndex = 4;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 353);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.mainBox);
            this.Controls.Add(this.menuStrip1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainWindow";
            this.Text = "Compiler IDE";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.mainBox.ResumeLayout(false);
            this.mainBox.PerformLayout();
            this.outBox.ResumeLayout(false);
            this.outBox.PerformLayout();
            this.tabsControl.ResumeLayout(false);
            this.inputPage.ResumeLayout(false);
            this.ILCodePage.ResumeLayout(false);
            this.threeAddrPage.ResumeLayout(false);
            this.CFGPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CFGPictureBox)).EndInit();
            this.ASTPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ASTPictureBox)).EndInit();
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
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.Button compileButton;
        private System.Windows.Forms.Label optimizationsLabel;
        private System.Windows.Forms.RichTextBox inputTextBox;
        private System.Windows.Forms.RichTextBox ILCodeTextBox;
        private System.Windows.Forms.RichTextBox threeAddrTextBox;
        private System.Windows.Forms.PictureBox CFGPictureBox;
        private System.Windows.Forms.PictureBox ASTPictureBox;
    }
}