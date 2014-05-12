using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Controls;

namespace ForgetME
{
    public partial class ForgetPassword : PhoneApplicationPage
    {


        private List<string> _questions = new List<string>
        {
            "What is your first phone number?",
            "Who is your childhood hero?",
            "Who is your first crush?",
            "My own secret question ___?"

        };

        public ForgetPassword()
        {
            InitializeComponent();
            PickQuestion.ItemsSource = _questions;
        }

        private void showToast(String title, String message)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.Title = title;
            toast.Message = message;
            //toast.ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));
            //toast.Completed += toast_Completed;
            toast.Show();
        }

        private void btnValidate_click(object sender, System.EventArgs e)
        {
            SettingsExt sett = new SettingsExt();
            Boolean validQ = false;
            Boolean validA = false;
            //check the question is correct

            if (PickQuestion.SelectedItem.ToString().Equals(sett.ForgetPwdQue, StringComparison.CurrentCulture))
            {
                validQ = true;
                
            }


            if (txtSecretAnswer.Password.Equals(sett.ForgetPwdAns, StringComparison.CurrentCulture))
            {
                validA = true;

            }

            if (validQ && validA)
            {
                NavigationService.Navigate((new Uri("/Assets/SettingsExt.xaml", UriKind.RelativeOrAbsolute)));
            }
            else
            {
                // show wrong Question or Answer message.
                showToast("Secure Notes", "Wrong Question Or Answer. Please try again.");
            }


        }
    }
}