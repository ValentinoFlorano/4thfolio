using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Education : Form
    {
        public Education()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Aboutme form3 = new Aboutme();
            form3.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Contactme form2 = new Contactme();
            form2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Projects form4 = new Projects();
            form4.Show();
            this.Hide();
        }
    }
}
