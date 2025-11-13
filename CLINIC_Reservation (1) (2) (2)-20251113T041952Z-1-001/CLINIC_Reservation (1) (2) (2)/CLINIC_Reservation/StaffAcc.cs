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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CLINIC_Reservation
{
    public partial class StaffAcc : Form
    {
        private string connectionString = @"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = Clinic.accdb";
        private int selectedID = 0;

        public StaffAcc()
        {
            InitializeComponent();
        }


        private void StaffAcc_Load(object sender, EventArgs e)
        {
            LoadSignupData();
            dataGridView1.Columns["ID"].Visible = false;


            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;


            // ✅ Set visual and selection behaviors
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            // ✅ Remove blue color from headers (keep them default gray)
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = SystemColors.ControlText;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = SystemColors.ControlText;

            // ✅ Remove gray highlight on the row headers (left boxes)
            dataGridView1.RowHeadersDefaultCellStyle.SelectionBackColor = SystemColors.Control;
            dataGridView1.RowHeadersDefaultCellStyle.SelectionForeColor = SystemColors.ControlText;

            dataGridView1.DataBindingComplete += (s, ev) =>
            {
                dataGridView1.ClearSelection();
                dataGridView1.CurrentCell = null;
            };

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;

            // ✅ Event handler for manual selection
            dataGridView1.CellClick += dataGridView1_CellClick;

        }

        private void LoadSignupData()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // ⚠️ Make sure these column names exist in your Access table
                    string query = "SELECT ID, [First Name], [Last Name], [Phone Number], [Gender], [Email] FROM SignUp";

                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                    dataGridView1.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error loading data: " + ex.Message,
                                "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            TrySelectIDFromRow(e.RowIndex);
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            TrySelectIDFromRow(e.RowIndex);
        }

        private void TrySelectIDFromRow(int rowIndex)
        {
            try
            {
                var cellValue = dataGridView1.Rows[rowIndex].Cells["ID"].Value;

                if (cellValue != null && int.TryParse(cellValue.ToString(), out int idValue))
                {
                    selectedID = idValue;
                    Console.WriteLine("Selected ID: " + selectedID);
                }
                else
                {
                    selectedID = 0;
                }
            }
            catch
            {
                selectedID = 0;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedID == 0)
            {
                MessageBox.Show("⚠️ Please select a record first before deleting.",
                                "No Record Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show(
                "Are you sure you want to delete this account?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (confirm == DialogResult.Yes)
            {
                bool deleted = DeleteRecord(selectedID);

                if (deleted)
                {
                    MessageBox.Show("✅ Account deleted successfully!",
                                    "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    selectedID = 0;
                    LoadSignupData(); // ✅ refresh table
                }
                else
                {
                    MessageBox.Show("⚠️ No record was deleted. Please check your database.",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // ✅ Delete record by ID
        private bool DeleteRecord(int id)
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    connection.Open();

                    // 🔧 CHANGE TABLE NAME HERE if your table is SignUpAcc or another name
                    string deleteQuery = "DELETE FROM SignUp WHERE ID = ?";

                    using (OleDbCommand cmd = new OleDbCommand(deleteQuery, connection))
                    {
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = id;

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // ✅ returns true if record deleted
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error deleting record: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }


}
