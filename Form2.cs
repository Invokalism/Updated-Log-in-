﻿using System;
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
    public partial class Form2 : Form
    {
        private OleDbConnection con;
        public Form2()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\LoginAndSignup\\LibSys.mdb");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            loadDataGrid();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void loadDataGrid()
        {
            con.Open();

            OleDbCommand com = new OleDbCommand("Select * from book order by accession_number asc", con);
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
            txtauthor.Text = "";
            txtno.Text = "";
            txttitle.Text = "";
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            con.Open();

            OleDbCommand com = new OleDbCommand("INSERT INTO book (accession_number, title, author) VALUES ('" + txtno.Text + "', '" + txttitle.Text + "', '" + txtauthor.Text + "')", con);
            com.ExecuteNonQuery();

            MessageBox.Show("Successfully SAVED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            con.Close();
            loadDataGrid();
        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            con.Open();
            int no;
            no = int.Parse(txtno.Text);

            OleDbCommand com = new OleDbCommand("Update book SET title= '" + txttitle.Text + "', author='" + txtauthor.Text + "' where accession_number= '" + no + "'", con);
            com.ExecuteNonQuery();

            MessageBox.Show("Successfully UPDATED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            con.Close();
            loadDataGrid();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            con.Open();
            string num = txtno.Text;

            DialogResult dr = MessageBox.Show("Are you sure you want to delete this?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                OleDbCommand com = new OleDbCommand("Delete from book where accession_number= '" + num + "'", con);
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

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            con.Open();

            OleDbCommand com = new OleDbCommand("Select * from book where title like'%" + txtSearch.Text + "%'", con);
            com.ExecuteNonQuery();

            OleDbDataAdapter adap = new OleDbDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
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

        private void grid1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            txtno.Text = grid1.Rows[e.RowIndex].Cells["accession_number"].Value.ToString();
            txttitle.Text = grid1.Rows[e.RowIndex].Cells["title"].Value.ToString();
            txtauthor.Text = grid1.Rows[e.RowIndex].Cells["author"].Value.ToString();
        }

        private void borrowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrower br = new Borrower();
            br.Show();
            this.Hide();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void transactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrow bw = new Borrow();
            bw.Show();
            this.Hide();
        }

        private void borrowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrow bb = new Borrow();
            bb.Show();
            this.Hide();
        }
    }
}
