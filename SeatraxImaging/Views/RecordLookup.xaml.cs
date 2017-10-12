using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SeatraxImaging.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SeatraxImaging {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecordLookup : Page {
        private ApplicationModel currentApplication;

        public RecordLookup() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null) {
                Debug.WriteLine("Sending application object...");
                currentApplication = (ApplicationModel) e.Parameter;
                RecordListView.ItemsSource = App.recordViewModel.GetAllRecords(currentApplication);
            }
        }

        void DeleteSelectedRecord() {
            Debug.WriteLine("Removed item");
        }

        void OnSelectionChanged(object sender, SelectionChangedEventArgs e) {

            RecordInfo removed = e.RemovedItems.FirstOrDefault() as RecordInfo;
            if(removed != null) {
                removed.IsReadOnly = true;
                removed.SaveButtonVisible = false;
            }

            RecordInfo added = e.AddedItems.FirstOrDefault() as RecordInfo;
            if(added != null) {
                added.IsReadOnly = false;
                added.SaveButtonVisible = true;
            }
        }
    }
}

