using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginAndSignup
{
    public partial class changepass : Form
    {
        private OleDbConnection conn;
        public changepass()
        {
            InitializeComponent();
            conn = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Lourdes\\source\\repos\\Updated-Log-in-\\LibSys.mdb");
        }

        private void changepass_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtusername.Text;
            string currentPassword = txtoldpass.Text;
            string newPassword = txtnewpass.Text;
            if (ChangePassword(username, currentPassword, newPassword))
            {
                MessageBox.Show("Password changed successfully.");
            }
            else
            {
                MessageBox.Show("Failed to change password.");
            }
        }

        static bool ChangePassword(string username, string currentPassword, string newPassword)
        {
            using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\Lourdes\\source\\repos\\Updated-Log-in-\\LibSys.mdb"))
            {
                string query = "SELECT password FROM Login WHERE username = @username";
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    connection.Open();
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string hashedPassword = reader.GetString(0);
                            if (DecryptPassword(hashedPassword, currentPassword))
                            {
                                string newHashedPassword = EncryptPassword(newPassword);
                                query = "UPDATE Login SET password = @password WHERE username = @username";
                                using (OleDbCommand updateCommand = new OleDbCommand(query, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@username", username);
                                    updateCommand.Parameters.AddWithValue("@password", newHashedPassword); 
                                    int rowsAffected = updateCommand.ExecuteNonQuery();
                                    return rowsAffected > 0;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

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
