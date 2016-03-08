using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace AeroplaneTree
{
    public partial class Form1 : Form
    {
        public static string COMPONENT = "Component";
        public static string AIRCRAFT = "Aircraft";

        string connectionString;
        SqlConnection connection;
        
        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["AeroplaneTree.Properties.Settings.ComponentsConnectionString"].ConnectionString;
            // set right mouse click to change selection
            treeView1.NodeMouseClick += (sender, args) => treeView1.SelectedNode = args.Node;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            populateTree();
            treeView1.ExpandAll();
        }

        private void populateTree()
        {
            string query = "SELECT * FROM component";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    treeView1.Nodes.Clear();
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        
                        while (reader.Read())
                        {
                            componentNode node = new componentNode(reader["ID"].ToString(), reader["name"].ToString(), (componentNode.Status)reader["status"], reader["description"].ToString());
                            int parentID = (int)reader["parentID"];
                            if (parentID == 0) // root node
                            {
                                treeView1.Nodes.Add(node);
                            }
                            else // find parent node
                            {
                                componentNode parent = (componentNode)treeView1.Nodes.Find(parentID.ToString(), true)[0];
                                parent.Nodes.Add(node);
                                 
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.ExitThread();
                }
            }
        }
        
        private void addComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                // show dialog: new component name
                NewComponentDialog dialog = new NewComponentDialog(this, COMPONENT);
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    //TODO: change new component flow: Insert into database then refresh list
                    componentNode newNode = new componentNode(dialog.newNodeName);
                    treeView1.SelectedNode.Nodes.Add(newNode);
                    treeView1.SelectedNode.Expand();
                }
            }
            else // show dialog: No aircraft or component selected
            {
                ErrorDialog errorDialog = new ErrorDialog("Please select a component or aircraft to add to.");
                errorDialog.ShowDialog();
            }
        }

        private void addNewAircraftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // show dialog: new aircraft
            NewComponentDialog dialog = new NewComponentDialog(this, AIRCRAFT);
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                componentNode newNode = new componentNode(dialog.newNodeName);
                treeView1.Nodes.Add(newNode);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

            // update the information panel to display the data from the selected node
            componentNode selected = (componentNode)treeView1.SelectedNode;
            // keep the selected item's image in the treeview from changing colour
            selected.SelectedImageIndex = (int)selected.CompletionStatus;
            this.labelDescription.Text = selected.Description;
            this.labelCompletion.Text = selected.CompletionStatus.ToString();
        }

        private void buttonChangeCompletion_Click(object sender, EventArgs e)
        {
            componentNode selected = (componentNode)treeView1.SelectedNode;

            // should not be able to change completion status of non-leaf jobs
            if (selected.GetNodeCount(false) == 0) // selected node is a leaf
            {
                // if not already completed...
                if (selected.CompletionStatus != componentNode.Status.Complete)
                {
                    // ...change the selected node's completion status to "Complete"
                    selected.CompletionStatus = componentNode.Status.Complete;
                    // update the information panel's Completion label to reflect this change
                    this.labelCompletion.Text = selected.CompletionStatus.ToString();
                    // update image in treeview
                    selected.SelectedImageIndex = (int)selected.CompletionStatus;
                }
                else
                {
                    MessageBox.Show("Job is already complete", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Cannot complete a job that is a combination of other jobs", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
