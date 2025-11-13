using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLINIC_Reservation
{
    public partial class StaffMainForm : Form
    {
        public StaffMainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StaffDashboard Form6 = new StaffDashboard();
            ShowFormInPanel(Form6);
        }

        private void ShowFormInPanel(Form frm)
        {
            panel1.Controls.Clear();
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;
            panel1.Controls.Add(frm);
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Appointment Form4 = new Appointment();
            ShowFormInPanel(Form4);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Report Form5 = new Report();
            ShowFormInPanel(Form5);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to Log Out?", "Confirm Log Out", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                LogIn form1 = new LogIn();
                form1.Show();


                this.Close();
            }
        }

        private void StaffMainForm_Load(object sender, EventArgs e)
        {
            StaffDashboard staffdashboard = new StaffDashboard();
            ShowFormInPanel(staffdashboard);

            MakeRounded(button1, 35);
            MakeRounded(button3, 35);
            MakeRounded(button4, 35);
            MakeRounded(button5, 35);
            MakeRounded(button2, 35);
        }

        private void MakeRounded(Control control, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(control.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(control.Width - radius, control.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, control.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Status Form7 = new Status();
            ShowFormInPanel(Form7);
        }
    }
}
