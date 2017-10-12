using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Pomelo.Data.MySql;

namespace SeatraxImaging.Models {
    public class RecordViewModel {
        private static RecordViewModel recordViewModel = new RecordViewModel();
        private ObservableCollection<RecordInfo> allRecordInfo = new ObservableCollection<RecordInfo>();

        public ObservableCollection<RecordInfo> AllRecordInfo {
            get { return recordViewModel.allRecordInfo; }
        }

        public IEnumerable<RecordInfo> GetAllRecords(ApplicationModel application = null) {
            Debug.WriteLine("Checking application...");
            if (application == null) return null;
            Debug.WriteLine("Fetching records...");

            try {
                using(MySqlConnection connection =
                    new MySqlConnection("Server=Server1; Database=seatrax; Uid=root; Pwd=admin;")) {
                    connection.Open();
                    MySqlCommand getCommand = connection.CreateCommand();
                    getCommand.CommandText = "SELECT * FROM " + application.AppName;
                    using(MySqlDataReader reader = getCommand.ExecuteReader()) {
                        //while(reader.Read()) {
                            //recordViewModel.allRecordInfo.Add(new RecordInfo(reader.GetString("Name"),
                            //    "An Imaging Application", reader.GetString("uid")));
                        //}
                        string[] recordFields = new string[reader.FieldCount];
                        while (reader.Read()) {
                            for (int i = 0; i < reader.FieldCount; i++) {
                                recordFields[i] = reader.GetString(i);
                            }
                            recordViewModel.allRecordInfo.Add(new RecordInfo(recordFields));
                        }
                        Debug.WriteLine("Records found!");
                    }
                }
            } catch(MySqlException e) {
                //Do something
                Debug.WriteLine("Error getting records!");
                Debug.WriteLine(e.ToString());
            }
            return recordViewModel.AllRecordInfo;
        }

        public bool InsertNewRecordInfo(string recordName) {
            RecordInfo recordInfo = new RecordInfo(recordName);

            try {
                using(MySqlConnection connection =
                    new MySqlConnection("Server=Server1; Database=seatrax; Uid=root; Pwd=admin;")) {
                    connection.Open();
                    MySqlCommand insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = "INSERT INTO contract_labor (Name) VALUES (@Name)";
                    insertCommand.Parameters.AddWithValue("@Name", recordInfo.RecordName);
                    insertCommand.ExecuteNonQuery();
                    recordViewModel.allRecordInfo.Add(recordInfo);
                    return true;
                }
            } catch(MySqlException) {
                //Do something
                return false;
            }
        }

        public bool UpdateRecordInfo(RecordInfo recordInfo) {
            Debug.WriteLine("Attempt to update database...");
            Debug.WriteLine("RecordId: " + recordInfo.RecordId + " RecordName: " + recordInfo.RecordName);
            try {
                using(MySqlConnection connection =
                    new MySqlConnection("Server=Server1; Database=seatrax; Uid=root; Pwd=admin;")) {
                    connection.Open();
                    MySqlCommand updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = "UPDATE contract_labor SET Name = @Name WHERE uid = @recordId";
                    updateCommand.Parameters.AddWithValue("@recordId", recordInfo.RecordId);
                    updateCommand.Parameters.AddWithValue("@appName", recordInfo.RecordName);
                    updateCommand.ExecuteNonQuery();
                    //recordViewModel.allRecordInfo.Add(recordInfo);
                    Debug.WriteLine("Update success!");
                    return true;
                }
            } catch(MySqlException) {
                //Do something
                Debug.WriteLine("Update failed!");
                return false;
            }
        }        

        public RecordViewModel() { }
    }
}
