using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace CLINIC_Reservation
{
    public partial class Status : Form
    {
        OleDbConnection conn = new OleDbConnection(
           @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=clinic.accdb");

        public Status()
        {
            InitializeComponent();
            
        }

        private void Status_Load(object sender, EventArgs e)
        {
            LoadAppointmentStatus();
        }

        private void LoadAppointmentStatus()
        {
            try
            {
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(
                    "SELECT * FROM Appointment", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridStatus.DataSource = dt;
                conn.Close();

                if (dataGridStatus.Columns.Contains("ID"))
                    dataGridStatus.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message);
            }
        }

        private void ProcessStatus(string newStatus)
        {
            if (dataGridStatus.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment.");
                return;
            }

            DataGridViewRow row = dataGridStatus.SelectedRows[0];
            int id = Convert.ToInt32(row.Cells["ID"].Value);

            try
            {
                conn.Open();

                // ✅ 1. UPDATE STATUS IN APPOINTMENT TABLE
                OleDbCommand update = new OleDbCommand(
                    "UPDATE Appointment SET Status=@stat WHERE ID=@id", conn);
                update.Parameters.AddWithValue("@stat", newStatus);
                update.Parameters.AddWithValue("@id", id);
                update.ExecuteNonQuery();

                // ✅ 2. INSERT INTO STATUS TABLE
                OleDbCommand insert = new OleDbCommand(
                    "INSERT INTO Status (FullName, PhoneNumber, Age, Gender, FullAddress, AppointmentDate, Reason, ConsultationFee, Doctor, Status) " +
                    "VALUES (@FullName, @PhoneNumber, @Age, @Gender, @FullAddress, @AppointmentDate, @Reason, @ConsultationFee, @Doctor, @Status)", conn);

                insert.Parameters.AddWithValue("@FullName", row.Cells["FullName"].Value.ToString());
                insert.Parameters.AddWithValue("@PhoneNumber", row.Cells["PhoneNumber"].Value.ToString());
                insert.Parameters.AddWithValue("@Age", row.Cells["Age"].Value.ToString());
                insert.Parameters.AddWithValue("@Gender", row.Cells["Gender"].Value.ToString());
                insert.Parameters.AddWithValue("@FullAddress", row.Cells["FullAddress"].Value.ToString());
                insert.Parameters.AddWithValue("@AppointmentDate", row.Cells["AppointmentDate"].Value.ToString());
                insert.Parameters.AddWithValue("@Reason", row.Cells["Reason"].Value.ToString());
                insert.Parameters.AddWithValue("@ConsultationFee", row.Cells["ConsultationFee"].Value.ToString());
                insert.Parameters.AddWithValue("@Doctor", row.Cells["Doctor"].Value.ToString());
                insert.Parameters.AddWithValue("@Status", newStatus);

                insert.ExecuteNonQuery();

                // ✅ 3. DELETE FROM APPOINTMENT TABLE
                OleDbCommand delete = new OleDbCommand(
                    "DELETE FROM Appointment WHERE ID=@id", conn);
                delete.Parameters.AddWithValue("@id", id);
                delete.ExecuteNonQuery();

                conn.Close();

                MessageBox.Show("✅ Moved to archive as " + newStatus);

                LoadAppointmentStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProcessStatus("Completed");
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            ProcessStatus("Cancelled");
        }
    }
}

