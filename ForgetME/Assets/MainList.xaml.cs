using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Controls;
using System.IO.IsolatedStorage;


namespace ForgetME
{

    public class ForgetMeNotes
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public ForgetMeNotes(string title)
        {
            this.Title = title;
        }
    }
    public class LongListSelectorEx : LongListSelector
    {

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            try
            {
                return base.MeasureOverride(availableSize);
            }
            catch (ArgumentException)
            {
                return this.DesiredSize;
            }
        }

    }
    public partial class MainList : PhoneApplicationPage
    {
        private Notes _notes;
        private SettingsExt sett = new SettingsExt();

        #region Constructor

        public MainList()
        {
            InitializeComponent();
            _notes = Notes.Instance;
        }
        #endregion

        #region Overrides
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            if (Notes.StatusCallBack != "")
            {
                showToast("Secure Notes", "Notes " + Notes.StatusCallBack);
                // clear the state, once showed. :)
                Notes.StatusCallBack = "";
            }
            loadLists();
            base.OnNavigatedTo(e);
        }


        #endregion

        #region Utils

        private void loadLists()
        {
            try
            {
                // sync the list.
                ObservableCollection<ForgetMeNotes> notes = new ObservableCollection<ForgetMeNotes>();
                ForgetMeNotes frmn = null;
                llsNotes.ItemsSource = notes;

                foreach (var item in _notes.GetKeys())
                {
                    frmn = new ForgetMeNotes(item.ToString().Substring(0, item.ToString().Length - ".txt".Length));

                    notes.Add(frmn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("unable to get settings" + ex.Message);
                Console.WriteLine(ex.Message + "Stack trace: " + ex.StackTrace.ToString());
            }
        }

        private void closeApplication()
        {
            IsolatedStorageSettings.ApplicationSettings.Save();
            Application.Current.Terminate();
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


        #endregion


        #region Events
        private void btnSync_click(object sender, System.EventArgs e)
        {
            loadLists();
        }

        private void btnAddNote_click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate((new Uri("/Assets/NotePage.xaml", UriKind.RelativeOrAbsolute)));

        }

        private void btnSettings_click(object sender, System.EventArgs e)
        {
            NavigationService.Navigate((new Uri("/Assets/SettingsExt.xaml", UriKind.RelativeOrAbsolute)));
        }

        private void item_tapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock keySel = (TextBlock)Convert.ChangeType(sender, e.OriginalSource.GetType());

            NavigationService.Navigate((new Uri("/Assets/NotePage.xaml?noteKey=" + keySel.Text, UriKind.RelativeOrAbsolute)));
        }

        private void LongListSelectorEx_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            LongListSelectorEx lls = sender as LongListSelectorEx;

            if (lls == null)
                return;
            ForgetMeNotes fmn = lls.SelectedItem as ForgetMeNotes;

            if (fmn == null)
                return;

            NavigationService.Navigate((new Uri("/Assets/NotePage.xaml?noteKey=" + fmn.Title, UriKind.RelativeOrAbsolute)));

        }


        /// <summary>
        /// back press stop.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void AddKeyClicked(object sender, System.EventArgs e)
        {
            // TODO: Add event handler implementation here.
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as MenuItem).DataContext;
            if (selected == null)
                return;
            ForgetMeNotes fmn = selected as ForgetMeNotes;

            if (fmn == null)
                return;

            try
            {
                Clipboard.SetText(Notes.Instance.GetNote(fmn.Title));
                showToast("Secure Notes", "Content Copied");
            }
            catch (Exception ex)
            {
                // swallow or log the exception or show the exception.
                Console.WriteLine(ex.Message);
            }
        }

        

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selected = (sender as MenuItem).DataContext;
            if (selected == null)
                return;
            ForgetMeNotes fmn = selected as ForgetMeNotes;

            if (fmn == null)
                return;

            NavigationService.Navigate((new Uri("/Assets/NotePage.xaml?noteKey=" + fmn.Title, UriKind.RelativeOrAbsolute)));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            var selected = (sender as MenuItem).DataContext;
            if (selected == null)
                return;
            ForgetMeNotes fmn = selected as ForgetMeNotes;

            if (fmn == null)
                return;

            Notes.Instance.DeleteNote(fmn.Title);
            showToast("Secure Notes", "Notes Deleted");

            loadLists();
        }



        #endregion
        

    }

}