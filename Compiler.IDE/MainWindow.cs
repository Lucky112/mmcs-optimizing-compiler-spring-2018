using Compiler.IDE.Handlers;
using System;
using System.IO;
using System.Windows.Forms;

namespace Compiler.IDE
{
    public partial class MainWindow : Form
    {
        ParseHandler parseHandler = new ParseHandler();
        
        public MainWindow()
        {
            InitializeComponent();
            
            parseHandler.ParsingCompleted += (o, e) => outTextBox.AppendText("Синтаксическое дерево построено\n");
            parseHandler.ParsingErrored += (o, e) => outTextBox.AppendText("Ошибка парсинга программы\n");
            parseHandler.ParsingSyntaxErrored += (o, e) => outTextBox.AppendText($"Синтаксическая ошибка. {e.Message}\n");
            parseHandler.ParsingLexErrored += (o, e) => outTextBox.AppendText($"Лексическая ошибка. {e.Message}\n");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.FilterIndex = 2;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            string content = File.ReadAllText(fileDialog.FileName);
            inputTextBox.Text = content;
            outTextBox.Text = "";
        }

        private void compileButton_Click(object sender, EventArgs e)
        {
            outTextBox.Text = "";
            parseHandler.handle(inputTextBox.Text);
        }
    }
}
