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
    public partial class StaffDashboard : Form
    {
        OleDbConnection conn;
        public StaffDashboard()
        {
            InitializeComponent();
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Clinic.accdb;");
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void StaffDashboard_Load(object sender, EventArgs e)
        {
            CountRecords();
        }

        private void CountRecords()
        {
            try
            {
                conn.Open();

                // Count records from Appointment table
                OleDbCommand cmdAppointment = new OleDbCommand("SELECT COUNT(*) FROM Appointment", conn);
                int appointmentCount = Convert.ToInt32(cmdAppointment.ExecuteScalar());

                // Show counts in labels
                AppointmentCount.Text = appointmentCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error counting records: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
