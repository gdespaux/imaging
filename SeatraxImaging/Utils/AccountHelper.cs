using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Pomelo.Data.MySql;
using SeatraxImaging.Models;

namespace SeatraxImaging.Utils {
    public class AccountHelper {
        private static AccountHelper accountHelper = new AccountHelper();
        private ObservableCollection<AccountModel> allAccounts = new ObservableCollection<AccountModel>();

        public ObservableCollection<AccountModel> AllAccounts {
            get { return accountHelper.allAccounts; }
        }

        public IEnumerable<AccountModel> GetAllAccounts() {
            try {
                using(MySqlConnection connection =
                    new MySqlConnection("Server=Server1; Database=seatrax; Uid=root; Pwd=admin;")) {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM users";
                    using(MySqlDataReader reader = getCommand.ExecuteReader()) {
                        while(reader.Read()) {
                            AccountModel user = new AccountModel(
                                reader.GetString("username"),
                                reader.GetString("memberName"),
                                reader.GetString("uid"));
                            accountHelper.allAccounts.Add(user);
                        }
                    }
                }
            } catch(MySqlException e) {
                //Do something
                Debug.WriteLine("Error getting account list!");
                Debug.WriteLine(e.ToString());
            }
            return accountHelper.AllAccounts;
        }

        public static AccountModel ValidateAccountCredentials(string username, string password) {
            string md5Password = CalculateMd5Hash(password);
            try {
                using(MySqlConnection connection =
                    new MySqlConnection("Server=Server1; Database=seatrax; Uid=root; Pwd=admin;")) {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM users WHERE username = @username AND password = @password";
                    getCommand.Parameters.AddWithValue("@username", username);
                    getCommand.Parameters.AddWithValue("@password", md5Password);
                    using(MySqlDataReader reader = getCommand.ExecuteReader()) {
                        if(reader.Read()) {
                            Debug.WriteLine("User authenticated!");
                            AccountModel user = new AccountModel(
                                reader.GetString("username"),
                                reader.GetString("memberName"),
                                reader.GetString("uid"));
                            accountHelper.allAccounts.Add(user);
                            return user;
                        }
                    }
                }
            } catch(MySqlException e) {
                //Do something
                Debug.WriteLine("Error getting account list!");
                Debug.WriteLine(e.ToString());
            }
            return null;
        }

        public static string CalculateMd5Hash(string input) {
            StringBuilder hash = new StringBuilder();
            MD5 md5Provider = MD5.Create();
            byte[] bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for(int i = 0; i < bytes.Length; i++) {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public AccountHelper() { }
    }
}
