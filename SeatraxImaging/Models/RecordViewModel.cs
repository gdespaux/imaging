using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Pomelo.Data.MySql;

namespace SeatraxImaging.Models {
    public class RecordViewModel {
        private static RecordViewModel recordViewModel = new RecordViewModel();
        private static List<RecordInfo> allRecordInfoList = new List<RecordInfo>();
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
                            allRecordInfoList.Add(new RecordInfo(recordFields));
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

            try {
                using(MySqlConnection connection =
                    new MySqlConnection("Server=Server1; Database=seatrax; Uid=root; Pwd=admin;")) {
                    connection.Open();
                    MySqlCommand insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = "INSERT INTO contract_labor (Name) VALUES (@Name)";
                    insertCommand.Parameters.AddWithValue("@Name", recordName);
                    insertCommand.ExecuteNonQuery();
                    //recordViewModel.allRecordInfo.Add(recordInfo);
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
                    updateCommand.Parameters.AddWithValue("@Name", recordInfo.RecordName);
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

        public bool UpdateSingleRecordField(ApplicationModel application, string recordId, string fieldName, string updatedInfo) {
            Debug.WriteLine("Attempt to update " + fieldName + " in database...");
            try {
                using(MySqlConnection connection =
                    new MySqlConnection("Server=Server1; Database=seatrax; Uid=root; Pwd=admin;")) {
                    connection.Open();
                    MySqlCommand updateCommand = connection.CreateCommand();
                    updateCommand.CommandText = "UPDATE " + application.AppName + " SET `" + fieldName + "` = '" + updatedInfo + "' WHERE uid = " + recordId;
                    updateCommand.ExecuteNonQuery();
                    //recordViewModel.allRecordInfo.Add(recordInfo);
                    Debug.WriteLine("Update success!");
                    return true;
                }
            } catch(MySqlException e) {
                //Do something
                Debug.WriteLine("Update failed!");
                Debug.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Do a fuzzy search on all records and order results based on a pre-defined rule set
        /// </summary>
        /// <param name="query">The part of the name or company to look for</param>
        /// <returns>An ordered list of records that matches the query</returns>
        public static IEnumerable<RecordInfo> GetMatchingRecords(string query) {
            var distinctRecords = allRecordInfoList.GroupBy(x => x.RecordName).Select(y => y.First());
            return distinctRecords.Where(c =>
                    c.RecordName.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                    c.RecordCheckNumber.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                    c.RecordWeekEndingDate.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                    c.RecordAmount.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.RecordName.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c =>
                    c.RecordCheckNumber.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
