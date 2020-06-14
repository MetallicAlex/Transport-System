using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace TransportSystem
{
    public partial class Form4 : Form
    {
        private TrasportSystem system;
        private bool fileSaved = false;
        public Form4(TrasportSystem system)
        {
            InitializeComponent();
            checkedListBox1.SetItemChecked(0, true);
            checkedListBox1.SetItemChecked(1, true);
            checkedListBox1.SetItemChecked(4, true);
            saveFileDialog1.DefaultExt = "docx";
            saveFileDialog1.Filter = "Все файлы (*.docx)|*.docx|Документы (*.doc)|*.doc";
            saveFileDialog1.FileName = "Report";
            this.system = system;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File file = new File();
                file.FileName = saveFileDialog1.FileName;
                file.CreateReport(system, checkedListBox1);
                fileSaved = true;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (fileSaved)
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
            else MessageBox.Show("Нужно сначала сохранить отчет", "Отчет", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
