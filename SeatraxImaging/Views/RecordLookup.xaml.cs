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
using Telerik.UI.Xaml.Controls.Grid;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SeatraxImaging {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecordLookup : Page {
        public static ApplicationModel CurrentApplication;
        private ApplicationModel currentApplication;

        public RecordLookup() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null) {
                Debug.WriteLine("Sending application object...");
                currentApplication = (ApplicationModel) e.Parameter;
                CurrentApplication = currentApplication;
                RecordListView.ItemsSource = App.recordViewModel.GetAllRecords(currentApplication);
            }
        }

        void DeleteSelectedRecord() {
            Debug.WriteLine("Removed item");
        }

        void ChangeEditMode() {
            if (RecordListView.UserEditMode == DataGridUserEditMode.Inline) {
                RecordListView.UserEditMode = DataGridUserEditMode.None;
            }
            else {
                RecordListView.UserEditMode = DataGridUserEditMode.Inline;
            }
        }

        private void MenuButton_OnClick(object sender, RoutedEventArgs e) {
            LeftMenu.IsPaneOpen = !LeftMenu.IsPaneOpen;
        }

        private void AutoSuggestBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args) {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput) {
                //var matchingRecords = RecordViewModel.GetMatchingRecords(sender.Text);
                //RecordListView.ItemsSource = matchingRecords.ToList();
            }
        }

        private void AutoSuggestBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args) {
            Debug.WriteLine("Query Submitted");
        }

        private void AutoSuggestBox_OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) {
            Debug.WriteLine("Suggestion Chosen");
        }
    }
}

