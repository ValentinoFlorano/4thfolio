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

namespace CLINIC_Reservation
{
    public partial class Form3 : Form
    {
        OleDbConnection con;
        

        public Form3()
        {
            InitializeComponent();
            con = new OleDbConnection(
                @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Clinic.accdb;");

            LoadData();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LoadData()
        {
            try
            {
                con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM Doctors", con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                dataGridView1.Columns["ID"].Visible = false;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }

        private void btnInsert_Click_1(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            if (MessageBox.Show("Are you sure you want to insert this record?",
                "Confirm Insert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            string fullname = $"{txtFirst.Text} {txtMiddle.Text} {txtLast.Text}";
            string fulladdress =
                $"{txtHouse.Text} {txtStreet.Text}, {txtBrgy.Text}, {txtMunicipality.Text}, {txtProvince.Text}";
            string gender = rbMale.Checked ? "Male" : "Female";

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand(
                    "INSERT INTO Doctors (FullName, Age, Gender, Phone, Email, FullAddress) " +
                    "VALUES (@FullName, @Age, @Gender, @Phone, @Email, @FullAddress)", con);

                cmd.Parameters.AddWithValue("@FullName", fullname);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@FullAddress", fulladdress);

                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record inserted successfully.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a row to update.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to update this record?",
                "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int rowID = Convert.ToInt32(row.Cells["ID"].Value);

            string fullname = $"{txtFirst.Text} {txtMiddle.Text} {txtLast.Text}";
            string fulladdress =
                $"{txtHouse.Text} {txtStreet.Text}, {txtBrgy.Text}, {txtMunicipality.Text}, {txtProvince.Text}";
            string gender = rbMale.Checked ? "Male" : "Female";

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand(
                    "UPDATE Doctors SET FullName=@FullName, Age=@Age, Gender=@Gender, " +
                    "Phone=@Phone, Email=@Email, FullAddress=@FullAddress WHERE ID=@ID", con);

                cmd.Parameters.AddWithValue("@FullName", fullname);
                cmd.Parameters.AddWithValue("@Age", txtAge.Text);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@FullAddress", fulladdress);
                cmd.Parameters.AddWithValue("@ID", rowID);

                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record updated successfully.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a row to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this record?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.No)
                return;

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            int rowID = Convert.ToInt32(row.Cells["ID"].Value);

            try
            {
                con.Open();
                OleDbCommand cmd = new OleDbCommand(
                    "DELETE FROM Doctors WHERE ID=@ID", con);
                cmd.Parameters.AddWithValue("@ID", rowID);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Record deleted.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private bool ValidateInputs()
        {
            // ✅ Required fields check
            if (string.IsNullOrWhiteSpace(txtFirst.Text) ||
                string.IsNullOrWhiteSpace(txtLast.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                (!rbMale.Checked && !rbFemale.Checked) ||
                string.IsNullOrWhiteSpace(txtHouse.Text) ||
                string.IsNullOrWhiteSpace(txtStreet.Text) ||
                string.IsNullOrWhiteSpace(txtBrgy.Text) ||
                string.IsNullOrWhiteSpace(txtMunicipality.Text) ||
                string.IsNullOrWhiteSpace(txtProvince.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill all fields correctly.");
                return false;
            }


            if (!int.TryParse(txtAge.Text, out int age) || age < 1 || age > 120)
            {
                MessageBox.Show("Invalid age.");
                return false;
            }

            string phone = txtPhone.Text.Trim();

            if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^09\d{9}$"))
            {
                MessageBox.Show("Phone number must start with 09 and be exactly 11 digits.");
                return false;
            }


            if (!System.Text.RegularExpressions.Regex.IsMatch(txtFirst.Text, @"^[A-Za-z ]+$") ||
                !System.Text.RegularExpressions.Regex.IsMatch(txtMiddle.Text, @"^[A-Za-z ]*$") || // middle name optional
                !System.Text.RegularExpressions.Regex.IsMatch(txtLast.Text, @"^[A-Za-z ]+$"))
            {
                MessageBox.Show("Name must contain letters only.");
                return false;
            }


            if (!System.Text.RegularExpressions.Regex.IsMatch(txtHouse.Text, @"^[0-9]+$"))
            {
                MessageBox.Show("House number must be numbers only.");
                return false;
            }

            if (!txtEmail.Text.EndsWith("@gmail.com"))
            {
                MessageBox.Show("Email must end with @gmail.com");
                return false;
            }

            return true;
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            txtFirst.Text = row.Cells["FirstName"].Value.ToString();
            txtMiddle.Text = row.Cells["MiddleName"].Value.ToString();
            txtLast.Text = row.Cells["LastName"].Value.ToString();
            txtPhone.Text = row.Cells["Phone"].Value.ToString();
            txtAge.Text = row.Cells["Age"].Value.ToString();
            txtEmail.Text = row.Cells["Email"].Value.ToString();
        }


        private void ClearFields()
        {
            txtFirst.Clear();
            txtMiddle.Clear();
            txtLast.Clear();
            txtPhone.Clear();
            txtAge.Clear();
            txtEmail.Clear();
            txtHouse.Clear();
            txtStreet.Clear();
            txtBrgy.Clear();
            txtMunicipality.Clear();
            txtProvince.Clear();
            rbMale.Checked = false;
            rbFemale.Checked = false;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
    

