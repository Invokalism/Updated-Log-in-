using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginAndSignup
{
    public partial class Registration : Form
    {
        private OleDbConnection conn;
        public Registration()
        {
            InitializeComponent();
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\LoginAndSignup\\LibSys.mdb");
        }

        private void register1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtusername.Text;
            string password = txtpassword.Text;


            if (!UsernameStrong(username))
            {
                MessageBox.Show("Please enter a stronger username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!IsPasswordStrong(password))
            {
                MessageBox.Show("Please enter a stronger password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (txtpassword.Text != txtconfirmpassword.Text)
            {
                MessageBox.Show("Please enter same password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string query = "SELECT COUNT(*) FROM Login WHERE username = ?";
            OleDbCommand command = new OleDbCommand(query, conn);
            command.Parameters.AddWithValue("@username", username);

            // open the connection and execute the query
            conn.Open();
            int count = (int)command.ExecuteScalar();

            // check if the username already exists
            if (count > 0)
            {
                conn.Close();
                MessageBox.Show("Username already Exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {

                conn.Close();
                string encryptedPassword = EncryptionHelper.EncryptPassword(password);
                addrecord(username, encryptedPassword);
                MessageBox.Show("Sign Up Successfully. You can login now.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                login log = new login();
                log.ShowDialog();
            }
        }
        private bool IsPasswordStrong(string password)
        {
            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^\w\s]).{8,}$";


            return Regex.IsMatch(password, pattern);
            
        }
        private bool UsernameStrong(string username)
        {
            string pattern1 = @"^(?=.{5,})[a-zA-Z0-9_]+$";
            return Regex.IsMatch(username, pattern1);
        }

        private void addrecord(string username, string password)
        {
            conn.Open();
            OleDbCommand cmd = new OleDbCommand("INSERT INTO Login values(@username, @password)", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            login login = new login();
            login.Show();
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

        private void txtconfirmpassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
