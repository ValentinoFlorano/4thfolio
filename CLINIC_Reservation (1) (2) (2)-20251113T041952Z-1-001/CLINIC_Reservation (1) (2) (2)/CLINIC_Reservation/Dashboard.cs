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
using System.Drawing.Drawing2D;

namespace CLINIC_Reservation
{
    public partial class Dashboard : Form
    {
        OleDbConnection conn;

        public Dashboard()
        {
            InitializeComponent();
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=Clinic.accdb;");
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            CountRecords();

            MakeRounded(panel4, 35);
            MakeRounded(panel3, 35);
            MakeRounded(panel1, 35);
        }

        private void CountRecords()
        {
            try
            {
                conn.Open();

                // Count records from Signup table
                OleDbCommand cmdSignup = new OleDbCommand("SELECT COUNT(*) FROM SignUp", conn);
                int signupCount = Convert.ToInt32(cmdSignup.ExecuteScalar());

                // Count records from Appointment table
                OleDbCommand cmdAppointment = new OleDbCommand("SELECT COUNT(*) FROM Appointment", conn);
                int appointmentCount = Convert.ToInt32(cmdAppointment.ExecuteScalar());

                // Show counts in labels
                lblSignupCount.Text = signupCount.ToString();
                lblAppointmentCount.Text = appointmentCount.ToString();
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

        private void lblSignupCount_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
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
    }
}
