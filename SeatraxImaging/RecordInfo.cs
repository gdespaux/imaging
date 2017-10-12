using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using SeatraxImaging.Properties;

namespace SeatraxImaging {
    public class RecordInfo : INotifyPropertyChanged {
        public RecordInfo(string[] recordFields) {
            RecordId = recordFields[0];
            RecordName = recordFields[1];
            RecordWeekEndingDate = recordFields[2];
            RecordCheckNumber = recordFields[3];
            RecordAmount = recordFields[4];
            RecordFilePath = recordFields[5];
            RecordNiceName = recordFields[6];
            if (recordFields.Length > 7) {
                //Debug.WriteLine(recordFields[7]);
                RecordUploadDate = UnixTimeStampToDateTime(double.Parse(recordFields[7])).ToString();
            }
        }

        public RecordInfo(string recordName) {
            //Used when adding new files, will be blank until indexed
            RecordName = recordName;
        }

        private bool isReadOnly = true;

        public bool IsReadOnly {
            get {
                return isReadOnly;
            }
            set {
                isReadOnly = value;
                //Debug.WriteLine("ReadOnly " + value);
                OnPropertyChanged();
            }
        }

        public string Value { get; set; }

        public string RecordId { get; }
        public string RecordName { get; set; }
        public string RecordWeekEndingDate { get; set; }
        public string RecordCheckNumber { get; set; }
        public string RecordAmount { get; set; }
        public string RecordFilePath { get; } //field1
        public string RecordNiceName { get; set; } //field2
        public string RecordUploadDate { get; }

        private bool saveButtonVisible = false;

        public bool SaveButtonVisible {
            get {
                return saveButtonVisible;
            }
            set {
                saveButtonVisible = value;
                //Debug.WriteLine("SaveButtonVisible " + value);
                OnPropertyChanged();
            }
        }

        public void SaveRecordToDatabase() {
            App.recordViewModel.UpdateRecordInfo(this);
        }

        public async void OpenFileToView() {
            if (RecordFilePath != null) {

                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(@"\\Server1\Imaging\test");
                StorageFile file = await folder.GetFileAsync("test.pdf");

                if (file != null) {
                    //Launch the file
                    var success = await Windows.System.Launcher.LaunchFileAsync(file);

                    if (success) {
                        Debug.WriteLine("File launched!");
                    }
                    else {
                        Debug.WriteLine("File launch failed!");
                    }
                }
                else {
                        Debug.WriteLine("File not found!");

                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
