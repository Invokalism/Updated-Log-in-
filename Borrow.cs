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

namespace LoginAndSignup
{
    public partial class Borrow : Form
    {
        private OleDbConnection con;
        public Borrow()
        {
            InitializeComponent();
            con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\LoginAndSignup\\LibSys.mdb");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtidnum.Text != string.Empty && txtquantity.Text != string.Empty)
            {
                con.Open();
                int no;
                int value = 1;
                no = int.Parse(txtno.Text);
                string title = txttitle.Text;
                string author = txtauthor.Text;
                string lname = txtlname.Text;
                string fname = txtfname.Text;
                int id;
                id = int.Parse(txtidnum.Text);
                DateTime dt = DateTime.Now;
                string sqlFormattedDate = dt.ToString("yyyy-MMMM-dd hh:mm tt");
                string r = "BORROWED";

                int oldquantity = Convert.ToInt32(txtquantity.Text);
                int newquantity = oldquantity - value;

                if (newquantity >= 0)
                {
                    OleDbCommand com = new OleDbCommand("UPDATE book SET Quantity= '" + newquantity + "', title= '" + title + "', author='" + author + "'  WHERE book_number = '" + no + "'", con);
                    com.ExecuteNonQuery();

                    con.Close();
                    loadDataGrid();

                    MessageBox.Show("Successfully UPDATED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadDataGrid();

                    con.Open();
                    OleDbCommand com1 = new OleDbCommand("INSERT INTO records ([date], [id_num], [first_name], [last_name], [book_num], [title], [author], [remarks]) VALUES (@date, @id_num, @first_name, @last_name, @book_num, @title, @author, @remarks)", con);
                    com1.Parameters.AddWithValue("@date", sqlFormattedDate);
                    com1.Parameters.AddWithValue("@id_num", id);
                    com1.Parameters.AddWithValue("@first_name", fname);
                    com1.Parameters.AddWithValue("@last_name", lname);
                    com1.Parameters.AddWithValue("@book_num", no);
                    com1.Parameters.AddWithValue("@title", title);
                    com1.Parameters.AddWithValue("@author", author);
                    com1.Parameters.AddWithValue("@remarks", r);
                    com1.ExecuteNonQuery();

                    con.Close();
                    loadDataGrid();
                }
                else
                {
                    MessageBox.Show("The Book is not available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Please complete all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            txtno.Text = grid1.Rows[e.RowIndex].Cells["book_number"].Value.ToString();
            txttitle.Text = grid1.Rows[e.RowIndex].Cells["title"].Value.ToString();
            txtauthor.Text = grid1.Rows[e.RowIndex].Cells["author"].Value.ToString();
            txtquantity.Text = grid1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
            txtmaxquantity.Text = grid1.Rows[e.RowIndex].Cells["Max_quantity"].Value.ToString();
        }
        private void EmptyTextBox()
        {
            txtno.Text = "";
            txtauthor.Text = "";
            txttitle.Text = "";
            txtquantity.Text = "";
            txtmaxquantity.Text = "";
            txtidnum.Text = "";
            txtfname.Text = "";
            txtlname.Text = "";

        }

        private void Borrow_Load(object sender, EventArgs e)
        {
            loadDataGrid();
        }
        private void loadDataGrid()
        {
            con.Open();

            OleDbCommand com = new OleDbCommand("Select * from book order by book_number asc", con);
            com.ExecuteNonQuery();

            OleDbDataAdapter adap = new OleDbDataAdapter(com);
            DataTable tab = new DataTable();

            adap.Fill(tab);
            grid1.DataSource = tab;

            con.Close();
            EmptyTextBox();
        }

        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
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

        private void transactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Borrow bw = new Borrow();
            bw.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (txtidnum.Text != string.Empty && txtquantity.Text != string.Empty)
            {
                con.Open();
                int no;
                int value = 1;
                no = int.Parse(txtno.Text);
                string title = txttitle.Text;
                string author = txtauthor.Text;
                string lname = txtlname.Text;
                string fname = txtfname.Text;
                int id;
                id = int.Parse(txtidnum.Text);
                DateTime dt = DateTime.Now;
                string sqlFormattedDate = dt.ToString("yyyy-MMMM-dd hh:mm tt");
                string r = "RETURNED";

                int oldquantity = Convert.ToInt32(txtquantity.Text);
                int maxquantity = Convert.ToInt32(txtmaxquantity.Text);
                int newquantity = oldquantity + value;

                if (newquantity <= maxquantity)
                {
                    OleDbCommand com = new OleDbCommand("UPDATE book SET Quantity= '" + newquantity + "', title= '" + title + "', author='" + author + "'  WHERE book_number = '" + no + "'", con);
                    com.ExecuteNonQuery();

                    con.Close();
                    loadDataGrid();

                    MessageBox.Show("Successfully UPDATED!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadDataGrid();

                    con.Open();
                    OleDbCommand com1 = new OleDbCommand("INSERT INTO records ([date], [id_num], [first_name], [last_name], [book_num], [title], [author], [remarks]) VALUES (@date, @id_num, @first_name, @last_name, @book_num, @title, @author, @remarks)", con);
                    com1.Parameters.AddWithValue("@date", sqlFormattedDate);
                    com1.Parameters.AddWithValue("@id_num", id);
                    com1.Parameters.AddWithValue("@first_name", fname);
                    com1.Parameters.AddWithValue("@last_name", lname);
                    com1.Parameters.AddWithValue("@book_num", no);
                    com1.Parameters.AddWithValue("@title", title);
                    com1.Parameters.AddWithValue("@author", author);
                    com1.Parameters.AddWithValue("@remarks", r);
                    com1.ExecuteNonQuery();

                    con.Close();
                    loadDataGrid();
                }
                else
                {
                    MessageBox.Show("All the books have been returned.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();    
                }
            }
            else
            {
                MessageBox.Show("Please complete all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          




        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtidnum.Text != string.Empty )
            {
                con.Open();
                int idNum = int.Parse(txtidnum.Text);


                OleDbCommand com = new OleDbCommand("Select * from borrower where id_num = '" + idNum + "'", con);
                OleDbDataReader reader = com.ExecuteReader();
                if (reader.Read())
                {
                    txtfname.Text = reader.GetValue(1).ToString();
                    txtlname.Text = reader.GetValue(2).ToString();
                }
                reader.Close();
                con.Close();
            }
            else
            {
                MessageBox.Show("Please complete all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reports rp = new reports();
            rp.Show();
            this.Hide();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

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
