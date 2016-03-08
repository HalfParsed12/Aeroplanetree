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
    public partial class NewComponentDialog : Form
    {
        Form1 mainForm;
        public string newNodeName { get; set; }
        private string nodeType;

        public NewComponentDialog(Form1 mainForm, string nodeType)
        {
            this.mainForm = mainForm;
            InitializeComponent();

            // add type of node (e.g. aircraft or component) to dialog strings
            this.Text += nodeType;
            label1.Text += nodeType;
            label1.Text += ":";
            // record node type
            this.nodeType = nodeType;
        }

        private void NewComponentDialog_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (nodeNameTextBox.Text != string.Empty)
            {
                newNodeName = nodeNameTextBox.Text;
            }
            this.Close();
        }

        private void nodeNameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(nodeNameTextBox.Text))
            {
                // cancel the "ok" event
                e.Cancel = true;
                // select entire text in box
                nodeNameTextBox.Select(0, nodeNameTextBox.Text.Length);
                // set the errorProvider error with the text to display
                string errorMessage = nodeType + " name required";
                errorProvider1.SetError(nodeNameTextBox, errorMessage);
            }
        }

        private void nodeNameTextBox_Validated(object sender, EventArgs e)
        {
            // If all conditions have been met, clear the ErrorProvider of errors.
            errorProvider1.SetError(nodeNameTextBox, "");
        }
    }
}
