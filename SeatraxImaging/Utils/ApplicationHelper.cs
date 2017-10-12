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
    public static class ApplicationHelper {
        private static List<ApplicationModel> allApplications = new List<ApplicationModel>();

        public static List<ApplicationModel> AllApplications {
            get { return allApplications; }
        }

        public static async Task<List<ApplicationModel>> GetAllApplications() {
            try {
                using(MySqlConnection connection =
                    new MySqlConnection("Server=Server1; Database=seatrax; Uid=root; Pwd=admin;")) {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM applications";
                    using(MySqlDataReader reader = getCommand.ExecuteReader()) {
                        while(reader.Read()) {
                            ApplicationModel application = new ApplicationModel(
                                reader.GetString("uid"),
                                reader.GetString("appName"),
                                reader.GetString("appNiceName"));
                            allApplications.Add(application);
                        }
                    }
                }
            } catch(MySqlException e) {
                //Do something
                Debug.WriteLine("Error getting application list!");
                Debug.WriteLine(e.ToString());
            }
            return AllApplications;
        }
    }
}
