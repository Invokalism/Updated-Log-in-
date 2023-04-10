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

namespace LoginAndSignup
{
    public partial class Borrower : Form
    {
        private OleDbConnection con;
        public Borrower()
        {

            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Lourdes\\source\\repos\\Updated-Log-in-\\LibSys.mdb");
        }

        private void txtno_TextChanged(object sender, EventArgs e)
        {

        }

        private void txttitle_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtauthor_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtno.Text != String.Empty && txtfirst.Text != String.Empty && txtlast.Text != String.Empty)
            {


                con.Open();

                OleDbCommand com = new OleDbCommand("Insert into borrower values ('" + txtno.Text + "', '" + txtfirst.Text + "', '" + txtlast.Text + "')", con);
                com.ExecuteNonQuery();

                MessageBox.Show("Successfully SAVED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                con.Close();
                loadDataGrid();
            }
            else
            {
                MessageBox.Show("Please complete all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtno.Text != String.Empty && txtfirst.Text != String.Empty && txtlast.Text != String.Empty)
            {
                con.Open();
                int no;
                no = int.Parse(txtno.Text);

                OleDbCommand com = new OleDbCommand("Update borrower SET first_name= '" + txtfirst.Text + "', last_name='" + txtlast.Text + "' where id_num= '" + no + "'", con);
                com.ExecuteNonQuery();

                MessageBox.Show("Successfully UPDATED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                con.Close();
                loadDataGrid();
            }
            else
            {
                MessageBox.Show("Please complete all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtno.Text != String.Empty && txtfirst.Text != String.Empty && txtlast.Text != String.Empty)
            {
                con.Open();
                string num = txtno.Text;

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    OleDbCommand com = new OleDbCommand("Delete from borrower where id_num= '" + num + "'", con);
                    com.ExecuteNonQuery();

                    MessageBox.Show("Successfully DELETED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("CANCELLED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                con.Close();
                loadDataGrid();
            }
            else
            {
                MessageBox.Show("Please complete all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

            private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            con.Open();

            OleDbCommand com = new OleDbCommand("Select * from borrower where title like'%" + txtSearch.Text + "%'", con);
            com.ExecuteNonQuery();

            OleDbDataAdapter adap = new OleDbDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
        }

        private void grid1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtno.Text = grid1.Rows[e.RowIndex].Cells["id_num"].Value.ToString();
            txtfirst.Text = grid1.Rows[e.RowIndex].Cells["first_name"].Value.ToString();
            txtlast.Text = grid1.Rows[e.RowIndex].Cells["last_name"].Value.ToString();
        }

        private void Borrower_Load(object sender, EventArgs e)
        {
            loadDataGrid();
        }
        private void loadDataGrid()
        {
            con.Open();

            OleDbCommand com = new OleDbCommand("Select * from borrower order by id_num asc", con);
            com.ExecuteNonQuery();

            OleDbDataAdapter adap = new OleDbDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
            EmptyTextBox();
        }
        private void EmptyTextBox()
        {
            txtlast.Text = "";
            txtno.Text = "";
            txtfirst.Text = "";
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void borrowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrower br = new Borrower();
            br.Show();
            this.Hide();
        }

        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void transactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrow bw = new Borrow();
            bw.Show();
            this.Hide();
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reports rp = new reports();
            rp.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            login lg = new login();
            MessageBox.Show("Log out Successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lg.Show();
            this.Hide();
        }
    }
}
