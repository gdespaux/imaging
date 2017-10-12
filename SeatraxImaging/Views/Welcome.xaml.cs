using System;
using System.Collections.Generic;
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

namespace SeatraxImaging.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Welcome : Page {
        private AccountModel currentAccountModel;

        public Welcome() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            currentAccountModel = (AccountModel) e.Parameter;
            if (currentAccountModel != null) {
                UserNameText.Text = currentAccountModel.RealName;
            }
        }

        private void Button_Restart_Click(object sender, RoutedEventArgs e) {
            
        }

        private void Button_Forget_User_Click(object sender, RoutedEventArgs e) {
            // Remove it from Microsoft Passport
            // MicrosoftPassportHelper.RemovePassportAccountAsync(_activeAccount);

            // Remove it from the local accounts list and resave the updated list
        }
    }
}
