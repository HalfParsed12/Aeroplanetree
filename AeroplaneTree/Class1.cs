using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AeroplaneTree
{
    class componentNode : TreeNode
    {
        public enum Status { NotStarted, InProgress, Complete };

        public string Description { set; get; }
        private Status _CompletionStatus;
        public Status CompletionStatus
        {
            get { return _CompletionStatus; }
            set
            {
                _CompletionStatus = value;
                this.ImageIndex = (int)value;
                
                // update status of parent
                // TODO: find out whether status of children should be updated. Throw error?
                if (this.Parent != null)
                {
                    Status newParentStatus;
                    int numComplete = 0, numNotStarted = 0;
                    foreach (componentNode current in this.Parent.Nodes)
                    {
                        switch (current.CompletionStatus)
                        {
                            case Status.Complete: numComplete++; break;
                            case Status.NotStarted: numNotStarted++; break;
                            default: break;
                        };
                    }
                    if (numComplete == this.Parent.Nodes.Count) newParentStatus = Status.Complete;
                    else if (numNotStarted == this.Parent.Nodes.Count) newParentStatus = Status.NotStarted;
                    else newParentStatus = Status.InProgress;
                    if (((componentNode)this.Parent).CompletionStatus != newParentStatus)
                    {
                        ((componentNode)this.Parent).CompletionStatus = newParentStatus;
                    }
                }
            }
        }

        public componentNode(string name)
        {
            this.Text = name;
            CompletionStatus = Status.NotStarted;
            Description = "Write description here";
            
        }

        public componentNode(string id, string name, Status status, string Description)
        {
            this.Text = name;
            this.Name = id;
            CompletionStatus = status;
            this.Description = Description;
        }
        

    }
}
