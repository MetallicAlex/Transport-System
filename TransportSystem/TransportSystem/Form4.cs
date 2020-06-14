using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransportSystem
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            checkedListBox1.SetItemChecked(4, true);
            checkedListBox1.SetItemChecked(3, true);
            checkedListBox1.SetItemChecked(2, true);
            checkedListBox1.SetItemChecked(1, true);
        }
    }
}
