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
using System.Text.RegularExpressions;

namespace CLINIC_Reservation
{
    public partial class Appointment : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;
        public Appointment()
        {
            InitializeComponent();
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Clinic.accdb;");
        }

        private void Appointment_Load(object sender, EventArgs e)
        {
            DisplayData();
            LoadDoctors();
            cmbDoctor.DropDownStyle = ComboBoxStyle.DropDownList;

        }

        private void LoadDoctors()
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT FullName FROM Doctors", conn);
                OleDbDataReader dr = cmd.ExecuteReader();

                cmbDoctor.Items.Clear();

                while (dr.Read())
                {
                    cmbDoctor.Items.Add(dr["FullName"].ToString());
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading doctors: " + ex.Message);
            }
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtMiddleName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtAge.Text) ||
                (!rbtnMale.Checked && !rbtnFemale.Checked) ||
                string.IsNullOrWhiteSpace(txtHouseNo.Text) ||
                string.IsNullOrWhiteSpace(txtStreet.Text) ||
                string.IsNullOrWhiteSpace(txtBarangay.Text) ||
                string.IsNullOrWhiteSpace(txtMunicipality.Text) ||
                string.IsNullOrWhiteSpace(txtProvince.Text) ||
                string.IsNullOrWhiteSpace(txtReason.Text) ||
                string.IsNullOrWhiteSpace(txtFee.Text) ||
                string.IsNullOrWhiteSpace(cmbDoctor.Text))   // ✅ Require doctor
            {
                MessageBox.Show("⚠️ Please fill out all fields including Doctor.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtAge.Text, @"^\d+$"))
            {
                MessageBox.Show("❌ Age must be a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Regex.IsMatch(txtPhone.Text, @"^\d{10,11}$"))
            {
                MessageBox.Show("❌ Phone number must contain only digits (10–11 numbers).", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtFee.Text, out decimal fee) || fee < 0)
            {
                MessageBox.Show("❌ Consultation Fee must be a valid positive number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to add this appointment?",
                "Confirm Add",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.No)
            {
                return;
            }

            string gender = rbtnMale.Checked ? "Male" : "Female";

            string fullName = $"{txtFirstName.Text} {txtMiddleName.Text} {txtLastName.Text}";
            string fullAddress = $"{txtHouseNo.Text} {txtStreet.Text}, {txtBarangay.Text}, {txtMunicipality.Text}, {txtProvince.Text}";

            conn.Open();

            string query = "INSERT INTO Appointment " +
               "(FullName, PhoneNumber, Age, Gender, FullAddress, AppointmentDate, Reason, ConsultationFee, Doctor, Status) " +
               "VALUES (@FullName, @PhoneNumber, @Age, @Gender, @FullAddress, @AppointmentDate, @Reason, @ConsultationFee, @Doctor, @Status)";

            cmd = new OleDbCommand(query, conn);

            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@PhoneNumber", txtPhone.Text);
            cmd.Parameters.AddWithValue("@Age", txtAge.Text);
            cmd.Parameters.AddWithValue("@Gender", gender);
            cmd.Parameters.AddWithValue("@FullAddress", fullAddress);
            cmd.Parameters.AddWithValue("@AppointmentDate", datePicker.Text);
            cmd.Parameters.AddWithValue("@Reason", txtReason.Text);
            cmd.Parameters.AddWithValue("@ConsultationFee", txtFee.Text);
            cmd.Parameters.AddWithValue("@Doctor", cmbDoctor.Text);

            cmd.Parameters.AddWithValue("@Status", "Pending"); // ✅ New status column

            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("✅ Appointment Added Successfully!");

            DisplayData();
            ClearData();
        }

        private void DisplayData()
        {
            conn.Open();
            adapter = new OleDbDataAdapter("SELECT * FROM Appointment", conn);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
            dataGridView1.Columns["ID"].Visible = false;

        }

        private void ClearData()
        {
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            txtPhone.Clear();
            txtAge.Clear();
            txtHouseNo.Clear();
            txtStreet.Clear();
            txtProvince.Clear();
            txtBarangay.Clear();
            txtMunicipality.Clear();
            txtReason.Clear();
            txtFee.Clear();
            cmbDoctor.SelectedIndex = -1;

            rbtnMale.Checked = false;
            rbtnFemale.Checked = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        

        private void txtMiddleName_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       
        
    }
}

