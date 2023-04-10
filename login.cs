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
using static LoginAndSignup.Registration;

namespace LoginAndSignup
{

    public partial class login : Form

    {
        private OleDbConnection conn;
        private OleDbCommand com;
        private OleDbDataReader dr;
        public login()
        {
            InitializeComponent();
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Lourdes\\source\\repos\\Updated-Log-in-\\LibSys.mdb");
        conn.Open();
        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            if (txtpassword.Text != string.Empty || txtusername.Text != string.Empty)
            {
                string encryptedPassword = EncryptionHelper.EncryptPassword(txtpassword.Text);

                com = new OleDbCommand("Select * from Login where username = '" + txtusername.Text + "' and password = '" + encryptedPassword + "'", conn);
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    this.Hide();
                    Form2 home = new Form2();
                    home.ShowDialog();
                }
                else
                {
                    dr.Close();
                    MessageBox.Show("Account Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("Please enter value in all field.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration reg = new Registration();
            this.Hide();
            reg.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            changepass cp = new changepass();
            cp.Show();
            this.Hide();
        }
    }
}
