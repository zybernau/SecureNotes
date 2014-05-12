using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ForgetME.Resources;
using System.IO.IsolatedStorage;

namespace ForgetME
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            // check whether it is first run, check for the keys in local storage.
            // if there are no keys present, show the settings page.

           

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsExt sett = new SettingsExt();
            if (sett.FirstUse)
            {
                NavigationService.Navigate((new Uri("/Assets/SettingsExt.xaml", UriKind.RelativeOrAbsolute)));
                //btnSettings.IsEnabled = true;
            }
            base.OnNavigatedTo(e);
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            

           login();

        }
		private void login()
		{
            SettingsExt sett = new SettingsExt();

            if(txtPass.Password.Equals(sett.PasswordSetting, StringComparison.CurrentCulture))
            {
                txtPass.Password = "";
                NavigationService.Navigate((new Uri("/Assets/MainList.xaml", UriKind.RelativeOrAbsolute)));
            }
		}

        private void btnAppLogin_click(object sender, System.EventArgs e)
        {
        	login();
        }

        private void txtpass_changed(object sender, System.Windows.RoutedEventArgs e)
        {
            login();
        }

        /// <summary>
        /// Close by terminating process
        /// </summary>
        private void closeApplication()
        {

            IsolatedStorageSettings.ApplicationSettings.Save();
            Application.Current.Terminate();

        }

        private void PhoneApplicationPage_BackKeyPress_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // on back key press. warn user for exit the application, if the user presses the ok to exit, then exit.
            if (MessageBox.Show("Are you sure you want to exit", "Exit Secure Notes", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                
                closeApplication();
                
            }
        }

        private void btnimgpassword_tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate((new Uri("/PhotoPasswordLogin.xaml", UriKind.RelativeOrAbsolute)));
        }

        private void btn_ForgetPwd(object sender, System.EventArgs e)
        {
            NavigationService.Navigate((new Uri("/ForgetPassword.xaml", UriKind.RelativeOrAbsolute)));
        }


    }
}