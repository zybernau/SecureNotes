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
    public partial class NotePage : PhoneApplicationPage
    {
        private Notes sett;
        private bool unSaved = false;
        #region Constructor
        public NotePage()
        {
            InitializeComponent();
            sett = Notes.Instance;
            //notification_text.Text = "Add/Update notes";

        }

        #endregion

        #region Overrides

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (unSaved && !txtTitle.Text.Equals(""))
            {
                saveNote();
                Notes.StatusCallBack = "Saved";
            }
            clearControls();
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string querystringvalue = "";
            Notes.StatusCallBack = "";
            try
            {
                if (NavigationContext.QueryString.TryGetValue("noteKey", out querystringvalue))
                {
                    txtTitle.Text = querystringvalue;
                    // get the value from settings :)
                    if (txtTitle.Text.Equals(""))
                    {
                        showToast("Secure Notes", "Unable to find the selected text.");
                        NavigationService.GoBack();
                    }
                    txtNote.Text = sett.GetNote(txtTitle.Text);
                }
                else
                {
                    clearControls();
                }
            }
            catch (Exception ex)
            {
                // swallow or log the exception or show the exception.
                Console.WriteLine(ex.Message);
            }

        }

        #endregion

        #region Util Methods

        private void showToast(String title, String message)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.Title = title;
            toast.Message = message;
            //toast.ImageSource = new BitmapImage(new Uri("ApplicationIcon.png", UriKind.RelativeOrAbsolute));

            //toast.Completed += toast_Completed;
            toast.Show();
        }

        private void saveNote()
        {
            if (txtTitle.Text.Trim() != "")
            {
                sett.SetNote(txtTitle.Text.Trim(), txtNote.Text);
            }


        }

        private void clearControls()
        {
            txtTitle.Text = "";
            txtNote.Text = "";

        }

        #endregion

        #region Events
        private void btndelete(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the note", "Delete Secure Note", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                sett.DeleteNote(txtTitle.Text);
                
                clearControls();
                Notes.StatusCallBack = "Deleted";
                NavigationService.GoBack();

            }
            else
            {
                showToast("Secure Notes", "Nice decision :-)");
            }
            
        }

        private void btnsave_click(object sender, System.EventArgs e)
        {
            if (txtTitle.Text.Trim() == "")
            {
                // show error notification
                showToast("Secure Notes", "Please enter Note title");
                return;
            }
            saveNote();
            //NavigationContext.QueryString.Add("Saved", "true");
            Notes.StatusCallBack = "Saved";
            NavigationService.GoBack();
         //NavigationService.Navigate((new Uri("/Assets/MainPage.xaml?Saved=true", UriKind.RelativeOrAbsolute)));
        }

        private void btnback_click(object sender, System.EventArgs e)
        {
            Notes.StatusCallBack = "";
            NavigationService.GoBack();
        }

        private void Title_keyChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            unSaved = true;
            String notesLoaded = sett.GetNote(txtTitle.Text);
            if (!notesLoaded.Equals(""))
            { txtNote.Text = notesLoaded; }
        }

        private void NotesChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            unSaved = true;
        }

        private void PhoneApplicationPage_BackKeyPress_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            if (unSaved)
            {
                saveNote();
                Notes.StatusCallBack = "Saved";
            }
            NavigationService.GoBack();
            //NavigationService.Navigate((new Uri("/Assets/MainPage.xaml?Saved=true", UriKind.RelativeOrAbsolute)));

        }
        #endregion



    }
}