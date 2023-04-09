using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LoginAndSignup.Registration;

namespace LoginAndSignup
{
    public partial class Change_password : Form
    {

        private OleDbConnection conn;
        private OleDbCommand com;
        private OleDbDataReader dr;


        public Change_password()
        {
            InitializeComponent();
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\LoginAndSignup\\LibSys.mdb");
            conn.Open();


        }

        private void Change_password_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtusername.Text;
            string password = txtpassword.Text;
            string newpass = txtnewpass.Text;
            string confirm = txtconfirm.Text;

            if (txtpassword.Text != string.Empty || txtusername.Text != string.Empty)
            {
                string encryptedPassword = EncryptionHelper.EncryptPassword(txtpassword.Text);
                string encryptedPass = EncryptionHelper.EncryptPassword(txtconfirm.Text);

                com = new OleDbCommand("Select * from Login where username = @username and password = @password", conn);
                com.Parameters.AddWithValue("@username", txtusername.Text);
                com.Parameters.AddWithValue("@password", encryptedPassword);
                dr = com.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    OleDbCommand cmd = new OleDbCommand("Update Login set password = @newpass where username = @username", conn);
                    cmd.Parameters.AddWithValue("@newpass", encryptedPass);
                    cmd.Parameters.AddWithValue("@username", username);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Password updated successfully.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
            login lg = new login();
            lg.Show();
            this.Hide();
        }
        public static class EncryptionHelper
        {
            public static string EncryptPassword(string password)
            {
                byte[] salt = Encoding.UTF8.GetBytes("somerandomsalt"); // Add a random salt for extra security
                using (var sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length];
                    passwordBytes.CopyTo(passwordWithSalt, 0);
                    salt.CopyTo(passwordWithSalt, passwordBytes.Length);
                    byte[] hashedPassword = sha256.ComputeHash(passwordWithSalt);
                    return Convert.ToBase64String(hashedPassword);
                }
            }
            public static bool DecryptPassword(string hashedPassword, string password)
            {
                byte[] salt = Encoding.UTF8.GetBytes("somerandomsalt"); // Add the same salt used for encryption
                byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
                using (var sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length];
                    passwordBytes.CopyTo(passwordWithSalt, 0);
                    salt.CopyTo(passwordWithSalt, passwordBytes.Length);
                    byte[] hashedPasswordToCheck = sha256.ComputeHash(passwordWithSalt);
                    return hashedPasswordBytes.SequenceEqual(hashedPasswordToCheck);
                }
            }
        }

    }
}
