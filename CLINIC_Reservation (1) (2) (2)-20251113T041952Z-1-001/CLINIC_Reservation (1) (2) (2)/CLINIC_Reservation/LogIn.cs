using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Drawing.Drawing2D;

namespace CLINIC_Reservation
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SignUp Form2 = new SignUp();
            Form2.Show();
           this.Hide();
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // Admin account (still local)
            if (email == "admin" && password == "1234")
            {
                MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                AdminMenu form1 = new AdminMenu();
                form1.Show();
                return;
            }

            // Otherwise, check from Signup table (database)
            try
            {
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Clinic.accdb;"))
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM SignUp WHERE Email = @Email AND [Password] = @Password";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Hide();

                            StaffMainForm form1 = new StaffMainForm();
                            form1.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Database Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;

            MakeRounded(button1, 30);
            MakeRounded(textBox2, 20);
            MakeRounded(textBox1, 20);

        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
            button3.BringToFront();
            button4.SendToBack();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            button4.BringToFront();
            button3.SendToBack();
        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
