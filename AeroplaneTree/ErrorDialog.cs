using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AeroplaneTree
{
    public partial class ErrorDialog : Form
    {
        public ErrorDialog(string message)
        {
            InitializeComponent();
            ErrorText.Text = message;
        }

        private void ErrorDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
