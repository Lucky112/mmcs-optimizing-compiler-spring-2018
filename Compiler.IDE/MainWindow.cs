using Compiler.IDE.Handlers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Compiler.IDE
{
    public partial class MainWindow : Form
    {
        OpenFileDialog sourceDialog = new OpenFileDialog();
        SaveFileDialog saveGraphDialog = new SaveFileDialog();
        private Image cfgImage;
        private Image astImage;

        ParseHandler parseHandler = new ParseHandler();
        ThreeAddrCodeHandler threeCodeHandler = new ThreeAddrCodeHandler();
        CFGHandler cfgHandler = new CFGHandler();
        ASTHandler astHandler = new ASTHandler();

        public MainWindow()
        {
            InitializeComponent();
            
            InitCommonListeners();
            InitOutputListeners();
            InitInputTabListeners();
            InitCfgTabListeners();
            InitAstTabListeners();
        }

        private void InitCommonListeners()
        {
            // open/exit
            exitToolStripMenuItem.Click += (o, e) => Application.Exit();
            openToolStripMenuItem.Click += (o, e) => OpenSourceFile();

            // compile button
            compileButton.Click += (o, e) => ClearOutput();
            compileButton.Click += (o, e) => parseHandler.Parse(inputTextBox.Text);

            // successful compilation
            parseHandler.ParsingCompleted += (o, e) =>
            {
                runButton.Enabled = true;

                cfgSaveButton.Enabled = true;
                cfgScaleBar.Enabled = true;

                astSaveButton.Enabled = true;
                astTrackBar.Enabled = true;
            };

            // AST
            parseHandler.ParsingCompleted += (o, e) => astHandler.GenerateASTImage(e);
            astHandler.GenerationCompleted += (o, e) =>
            {
                astImage = e;
                ASTPictureBox.Image = astImage;
            };

            // 3-addr code
            parseHandler.ParsingCompleted += threeCodeHandler.GenerateThreeAddrCode;
            threeCodeHandler.PrintableCodeGenerated += (o, e) => threeAddrTextBox.Text = e;

            // CFG
            threeCodeHandler.GenerationCompleted += (o, e) => cfgHandler.GenerateCFGImage(e);
            cfgHandler.GenerationCompleted += (o, e) =>
            {
                cfgImage = e;
                CFGPictureBox.Image = cfgImage;
            };
        }

        private void InitOutputListeners()
        {
            parseHandler.ParsingErrored += (o, e) => outTextBox.AppendText("Ошибка парсинга программы" + Environment.NewLine);
            parseHandler.ParsingSyntaxErrored += (o, e) => outTextBox.AppendText($"Синтаксическая ошибка. {e.Message}" + Environment.NewLine);
            parseHandler.ParsingLexErrored += (o, e) => outTextBox.AppendText($"Лексическая ошибка. {e.Message}" + Environment.NewLine);

            parseHandler.ParsingCompleted += (o, e) => outTextBox.AppendText("Синтаксическое дерево построено" + Environment.NewLine);
            threeCodeHandler.PrintableCodeGenerated += (o, e) => outTextBox.AppendText("Создание трехадресного кода завершено" + Environment.NewLine);
            cfgHandler.GenerationCompleted += (o, e) => outTextBox.AppendText("Граф потока управления построен" + Environment.NewLine);
            astHandler.GenerationCompleted += (o, e) => outTextBox.AppendText("Граф AST построен" + Environment.NewLine);
        }

        private void InitInputTabListeners()
        {
            inputTextBox.TextChanged += (o, e) => runButton.Enabled = false;
        }

        private void InitCfgTabListeners()
        {
            cfgScaleBar.ValueChanged += (o, e) => 
                    CFGPictureBox.Image = Utils.ScaleImage(cfgImage, Utils.TrackBarToScale(cfgScaleBar));
            cfgSaveButton.Click += (o, e) => SaveGraphFile(CFGPictureBox);
        }

        private void InitAstTabListeners()
        {
            astTrackBar.ValueChanged += (o, e) =>
                    ASTPictureBox.Image = Utils.ScaleImage(astImage, Utils.TrackBarToScale(astTrackBar));
            astSaveButton.Click += (o, e) => SaveGraphFile(ASTPictureBox);
        }

        private void ClearOutput()
        {
            outTextBox.Text = "";
        }

        private void OpenSourceFile()
        {
            sourceDialog.InitialDirectory = Directory.GetCurrentDirectory();
            sourceDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sourceDialog.FilterIndex = 2;
            sourceDialog.RestoreDirectory = true;

            if (sourceDialog.ShowDialog() != DialogResult.OK)
                return;

            inputTextBox.Text = File.ReadAllText(sourceDialog.FileName);
        }

        private void SaveGraphFile(PictureBox picbox)
        {
            saveGraphDialog.Filter = "Images|*.png;*.bmp;*.jpg";
            saveGraphDialog.RestoreDirectory = true;
            ImageFormat format = ImageFormat.Png;
            if (saveGraphDialog.ShowDialog() != DialogResult.OK)
                return;

            string ext = Path.GetExtension(saveGraphDialog.FileName);
            switch (ext)
            {
                case ".jpg":
                    format = ImageFormat.Jpeg;
                    break;
                case ".bmp":
                    format = ImageFormat.Bmp;
                    break;
            }
            picbox.Image.Save(saveGraphDialog.FileName, format);
        }
    }
}
