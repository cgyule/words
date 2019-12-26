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
        public string title = "";
        public string message = "";
        public bool cancelVisible = false;
        public FormProg()
        {
            InitializeComponent();
            lblInfo.Text = message;
            lblTitle.Text =title;
            btnCancel.Visible = cancelVisible;

        }
    }
}
