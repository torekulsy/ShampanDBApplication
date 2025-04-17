using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShampanDbApplication
{
    public partial class MDIMainInterface : Form
    {
        public MDIMainInterface()
        {
            InitializeComponent();
        }

        private void MDIMainInterface_Load(object sender, EventArgs e)
        {
            FormLogIn fmi = new FormLogIn();

            fmi.Show(this);
        }
    }
}
