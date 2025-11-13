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
    public partial class Report : Form
    {
        OleDbConnection conn = new OleDbConnection(
           @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=clinic.accdb");


        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            comboFilter.DropDownStyle = ComboBoxStyle.DropDownList;

            // ✅ Load ComboBox Items
            comboFilter.Items.Add("All");
            comboFilter.Items.Add("Completed");
            comboFilter.Items.Add("Cancelled");

            comboFilter.SelectedIndex = 0; // ✅ Default = ALL

            LoadStatusTable("All");
        }

        private void LoadStatusTable(string filter)
        {
            try
            {
                conn.Open();

                string query = "SELECT * FROM Status";

                if (filter == "Completed")
                    query = "SELECT * FROM Status WHERE [Status] = 'Completed'";
                else if (filter == "Cancelled")
                    query = "SELECT * FROM Status WHERE [Status] = 'Cancelled'";

                OleDbDataAdapter da = new OleDbDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridReport.DataSource = dt;

                conn.Close();

                if (dataGridReport.Columns.Contains("ID"))
                    dataGridReport.Columns["ID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message);
            }

            dataGridReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridReport.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

        }

        private void comboFilter_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LoadStatusTable(comboFilter.SelectedItem.ToString());
        }
    }
}
