using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TestApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            string password = "123";
            if (PasswordTxt.Text == password)
            {
                Hide();
                new Form1().ShowDialog();
                Close();
            }
            else
            {
                incorrectPassLbl.Visible = true;
            }
        }

    }
}
