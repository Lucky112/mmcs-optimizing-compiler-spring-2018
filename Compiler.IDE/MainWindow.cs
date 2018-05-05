using Compiler.IDE.Handlers;
using System;
using System.IO;
using System.Windows.Forms;

namespace Compiler.IDE
{
    public partial class MainWindow : Form
    {
        public event EventHandler<string> FileSelected;

        OpenFileDialog fileDialog = new OpenFileDialog();

        ParseHandler parseHandler = new ParseHandler();
        ThreeAddrCodeHandler threeCodeHandler = new ThreeAddrCodeHandler();

        public MainWindow()
        {
            InitializeComponent();
            InitListeners();
        }

        private void InitListeners()
        {
            exitToolStripMenuItem.Click += (o, e) => Application.Exit();

            openToolStripMenuItem.Click += (o, e) => SelectFile();
            FileSelected += (o, e) => inputTextBox.Text = File.ReadAllText(fileDialog.FileName);

            inputTextBox.TextChanged += (o, e) => runButton.Enabled = false;
            inputTextBox.TextChanged += (o, e) => ClearAll();

            compileButton.Click += (o, e) => ClearOutput();
            compileButton.Click += (o, e) => parseHandler.Parse(inputTextBox.Text);

            parseHandler.ParsingCompleted += (o, e) => outTextBox.AppendText("Синтаксическое дерево построено\n");
            parseHandler.ParsingErrored += (o, e) => outTextBox.AppendText("Ошибка парсинга программы\n");
            parseHandler.ParsingSyntaxErrored += (o, e) => outTextBox.AppendText($"Синтаксическая ошибка. {e.Message}\n");
            parseHandler.ParsingLexErrored += (o, e) => outTextBox.AppendText($"Лексическая ошибка. {e.Message}\n");

            parseHandler.ParsingCompleted += (o, e) => runButton.Enabled = true;
            parseHandler.ParsingCompleted += threeCodeHandler.GenerateThreeAddrCode;

            threeCodeHandler.GenerationCompleted += (o, e) => threeAddrTextBox.Text = e;
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
