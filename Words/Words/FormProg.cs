using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Words
{

    public partial class FormProg : Form
    {
        public BackgroundWorker bkWorker;
        public FormProg()
        {
            InitializeComponent();
            lblInfo.Text ="";
            lblTitle.Text ="";
            lblFound.Text = "";
            lblFound.Visible = false;
            btnCancel.Visible = false;
            bkWorker = null;

        }
        
        public void setup(string title, string message, bool cancelVisible,bool progressVisible)
        {
            lblTitle.Text = title;
            btnCancel.Visible = cancelVisible;
            progressBar1.Visible = progressVisible;
            progressBar1.Value = 0;
            if (!progressVisible)
            {
                lblFound.Visible = true;
                lblFound.Text = message;
                lblInfo.Visible = false;
            }
            else
            {
                lblFound.Visible = false;
                lblInfo.Text = message;
                lblInfo.Visible = true;
            }
        }
        public void setProgress(int prog,string msg)
        {
            if (progressBar1.Visible)
            {
                progressBar1.Value = prog;
                lblInfo.Text = msg;
                lblInfo.Refresh();
                progressBar1.Refresh();
            }
            else
            {
                lblFound.Text = msg;
                lblFound.Refresh();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bkWorker.CancelAsync();
        }
    }
}
