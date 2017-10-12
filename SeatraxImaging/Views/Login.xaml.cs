using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SeatraxImaging.Models;
using SeatraxImaging.Utils;
using SeatraxImaging.Views;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SeatraxImaging {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page {

        private AccountModel currentAccount;

        public Login() {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            GetCurrentUser();
        }

        private async void GetCurrentUser() {
            string name = await GetUserName();
            Debug.WriteLine("Current User: " + name);
        }

        private async Task<string> GetUserName() {
            var users = await User.FindAllAsync();
            if (users != null) {
                var name = await users.FirstOrDefault().GetPropertyAsync(KnownUserProperties.PrincipalName);
                string userName = name.ToString().Substring(0, Math.Max(name.ToString().IndexOf('@'), 0));
                return userName;
            }
            else {
                return null;
            }
        }

        private void PassportSignInButton_Click(object sender, RoutedEventArgs e) {
            ErrorMessage.Text = "";
            UserLogin();
        }

        private void RegisterButtonTextBlock_OnPointerPressed(object sender, RoutedEventArgs e) {
            ErrorMessage.Text = "";
        }

        private void UserLogin() {
            currentAccount = AccountHelper.ValidateAccountCredentials(UsernameTextBox.Text, PasswordTextBox.Text);
            if (currentAccount != null) {
                Debug.WriteLine("Successfully logged in!");
                Frame.Navigate(typeof(ApplicationSelection), currentAccount);
            }
            else {
                ErrorMessage.Text = "Invalid credentials entered!";
            }
        }
    }
}
