using System;
using System.Collections.Generic;
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
using SeatraxImaging.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SeatraxImaging.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ApplicationSelection : Page {
        public ApplicationSelection() {
            this.InitializeComponent();
            Loaded += ApplicationSelection_Loaded;
        }

        private void ApplicationSelection_Loaded(object sender, RoutedEventArgs e) {
            ApplicationListView.ItemsSource = ApplicationHelper.AllApplications;
            ApplicationListView.SelectionChanged += ApplicationSelectionChanged;
        }

        /// <summary>
        /// Function called when an application is selected in the list of applications
        /// Navigates to the RecordLookup page and passes the chosen application
        /// </summary>
        private void ApplicationSelectionChanged(object sender, RoutedEventArgs e) {
            if(((ListView)sender).SelectedValue != null) {
                ApplicationModel application = (ApplicationModel)((ListView)sender).SelectedValue;
                if(application != null) {
                    Debug.WriteLine("Application " + application.AppNiceName + " selected!");
                }
                Frame.Navigate(typeof(RecordLookup), application);
            }
        }
    }
}
