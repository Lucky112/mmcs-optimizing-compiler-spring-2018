using Compiler.IDE.Handlers;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Compiler.IDE
{
    public partial class MainWindow : Form
    {
        public event EventHandler<string> FileSelected;

        OpenFileDialog fileDialog = new OpenFileDialog();
        private Image cfgImage;

        ParseHandler parseHandler = new ParseHandler();
        ThreeAddrCodeHandler threeCodeHandler = new ThreeAddrCodeHandler();
        CFGHandler cfgHandler = new CFGHandler();
        
        public MainWindow()
        {
            InitializeComponent();
            
            InitCommonListeners();
            InitOutputListeners();
            InitInputTabListeners();
            InitCfgTabListeners();
        }

        private void InitCommonListeners()
        {
            exitToolStripMenuItem.Click += (o, e) => Application.Exit();
            openToolStripMenuItem.Click += (o, e) => SelectFile();
            FileSelected += (o, e) => inputTextBox.Text = File.ReadAllText(fileDialog.FileName);

            compileButton.Click += (o, e) => ClearOutput();
            compileButton.Click += (o, e) => parseHandler.Parse(inputTextBox.Text);

            parseHandler.ParsingCompleted += (o, e) =>
            {
                runButton.Enabled = true;

                cfgSaveButton.Enabled = true;
                cfgScaleBar.Enabled = true;
            };

            parseHandler.ParsingCompleted += threeCodeHandler.GenerateThreeAddrCode;
            threeCodeHandler.PrintableCodeGenerated += (o, e) => threeAddrTextBox.Text = e;

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
        }

        private void InitInputTabListeners()
        {
            inputTextBox.TextChanged += (o, e) => runButton.Enabled = false;
            inputTextBox.TextChanged += (o, e) => ClearAll();
        }

        private void InitCfgTabListeners()
        {
            cfgScaleBar.ValueChanged += (o, e) => 
                    CFGPictureBox.Image = Utils.ScaleImage(cfgImage, Utils.TrackBarToScale(cfgScaleBar));
        }

        private void ClearAll()
        {
            ClearOutput();
            ILCodeTextBox.Text = "";
            threeAddrTextBox.Text = "";
        }

        private void ClearOutput()
        {
            outTextBox.Text = "";
        }

        private void SelectFile()
        {
            fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            FileSelected(null, fileDialog.FileName);
        }
    }
}
