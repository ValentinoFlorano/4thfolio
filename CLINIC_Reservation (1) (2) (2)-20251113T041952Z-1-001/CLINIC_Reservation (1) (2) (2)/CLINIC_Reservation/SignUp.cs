using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CLINIC_Reservation
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Clinic.accdb");
        private OleDbCommand cmd = new OleDbCommand();

        

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!chkAgree.Checked)
            {
                MessageBox.Show("Please agree to the terms before continuing.",
                    "Agreement Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

         
            string gender = "";
            if (rdoMale.Checked)
                gender = "Male";
            else if (rdoFemale.Checked)
                gender = "Female";

            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPass.Text;

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(gender))
            {
                MessageBox.Show("Please fill in all fields.",
                    "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please re-enter.",
                    "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtConfirmPass.Clear();
                txtPassword.Focus();
                return;
            }

            try
            {
                con.Open();

                string checkEmailQuery = "SELECT COUNT(*) FROM SignUp WHERE Email = ?";
                cmd = new OleDbCommand(checkEmailQuery, con);
                cmd.Parameters.AddWithValue("?", email);

                int emailCount = (int)cmd.ExecuteScalar();

                if (emailCount > 0)
                {
                    MessageBox.Show("Email already exists. Please use another.",
                        "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtEmail.Clear();
                    txtEmail.Focus();
                }
                else
                {
                    string insertQuery = @"INSERT INTO SignUp 
                        ([First Name], [Last Name], [Phone Number], [Gender], [Email], [password]) 
                        VALUES (?, ?, ?, ?, ?, ?)";

                    cmd = new OleDbCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("?", firstName);
                    cmd.Parameters.AddWithValue("?", lastName);
                    cmd.Parameters.AddWithValue("?", phone);
                    cmd.Parameters.AddWithValue("?", gender);
                    cmd.Parameters.AddWithValue("?", email);
                    cmd.Parameters.AddWithValue("?", password);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Account successfully created!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();

                    LogIn loginForm = new LogIn();
                    loginForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtConfirmPass.Clear();
            rdoMale.Checked = false;
            rdoFemale.Checked = false;
            chkAgree.Checked = false;
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPass.UseSystemPasswordChar = true;

            MakeRounded(button1, 30);
            MakeRounded(txtFirstName, 20);
            MakeRounded(txtLastName, 20);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
            button3.BringToFront();
            button4.SendToBack();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            button4.BringToFront();
            button3.SendToBack();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtConfirmPass.UseSystemPasswordChar = false;
            button5.BringToFront();
            button6.SendToBack();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtConfirmPass.UseSystemPasswordChar = true;
            button6.BringToFront();
            button5.SendToBack();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LogIn logIn = new LogIn();
            logIn.Show();
            this.Hide();
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        
    }

    
}
