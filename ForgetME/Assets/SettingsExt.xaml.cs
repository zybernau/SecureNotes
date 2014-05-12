using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Collections;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.IO;
namespace ForgetME
{
    public partial class SettingsExt : PhoneApplicationPage
    {

        // Our settings
        IsolatedStorageSettings settings;
        PhotoChooserTask photoChooser;
        NoteSec noteSec;

        // keys and default
        const string PasswordSettingKeyName = "PasswordSetting";
        const string PasswordSettingDefault = "";

        const string FirstTimeUseKeyName = "FirstTime";
        const bool FirstTimeUseDefault = false;

        const string OriginalTapPointsKeyName = "OriginalTapPoints";
        const Point[] OriginalTapPointsDefault = null;

        const string FirstUseGUIDKeyName = "FirstUseGUID";
        const string FirstUseGUIDDefault = "";

        const string ForgetPwdQueKeyName = "ForgetPwdQue";
        const string ForgetPwdQueDefault = "";

        const string ForgetPwdAnsKeyName = "ForgetPwdAns";
        const string ForgetPwdAnsDefault = "";


        // private keys
        private bool firstTime = false;

        private List<string> _questions = new List<string>
        {
            "What is your first phone number?",
            "Who is your childhood hero?",
            "Who is your first crush?",
            "My own secret question ___?"

        };
        public SettingsExt()
        {
            InitializeComponent();
            settings = IsolatedStorageSettings.ApplicationSettings;
            photoChooser = new PhotoChooserTask();
            photoChooser.Completed += photoChooser_Completed;
        }



        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
            return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        private String GetValue(string Key)
        {
            String value = "";

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (String)settings[Key];
            }

            return value;
        }



        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            settings.Save();

        }

        /// <summary>
        /// Property to get and set a Password Setting Key.
        /// </summary>
        public string PasswordSetting
        {
            get
            {

                return GetValueOrDefault<string>(PasswordSettingKeyName, PasswordSettingDefault);
            }
            set
            {
                if (AddOrUpdateValue(PasswordSettingKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a Password Setting Key.
        /// </summary>
        public string ForgetPwdQue
        {
            get
            {

                return GetValueOrDefault<string>(ForgetPwdQueKeyName, ForgetPwdQueDefault);
            }
            set
            {
                if (AddOrUpdateValue(ForgetPwdQueKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a Password Setting Key.
        /// </summary>
        public string ForgetPwdAns
        {
            get
            {

                return GetValueOrDefault<string>(ForgetPwdAnsKeyName, ForgetPwdAnsDefault);
            }
            set
            {
                if (AddOrUpdateValue(ForgetPwdAnsKeyName, value))
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string FirstUseGUID
        {
            get
            {
                return GetValueOrDefault<string>(FirstUseGUIDKeyName, FirstUseGUIDDefault);
            }
            set
            {
                if (FirstUse)
                {
                    if (AddOrUpdateValue(FirstUseGUIDKeyName, value))
                    {
                        Save();
                    }
                }
            }
        }

        public bool FirstUse
        {
            get
            {
                return GetValueOrDefault<bool>(FirstTimeUseKeyName, FirstTimeUseDefault);
            }
            set
            {
                if (AddOrUpdateValue(FirstTimeUseKeyName, value))
                {
                    Save();
                }
            }
        }

        public Point[] OriginalTapPoints
        {
            get
            {
                return GetValueOrDefault<Point[]>(OriginalTapPointsKeyName, OriginalTapPointsDefault);
            }
            set
            {
                if (AddOrUpdateValue(OriginalTapPointsKeyName, value))
                {
                    Save();
                }
            }
        }

        private void closeApplication()
        {
            //IsolatedStorageSettings.ApplicationSettings.Save();
            Application.Current.Terminate();

        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (FirstUse)
            {
                // exit the application.
                closeApplication();
            }
            else
            {
                base.OnBackKeyPress(e);
            }
        }

        private int pickerSelectedIndex = 0;
        public int PickerSelectedIndex
        {
            get
            {
                return this.pickerSelectedIndex;
            }
            set
            {
                this.pickerSelectedIndex = value;
            }
        }

        public List<string> PickerData
        {
            get
            {
                return this._questions;
            }
            set
            {
                this._questions = value;
            }
        }


        private int getSelectedIndexValue(String itemString)
        {
            int indPick=1;
            foreach (String item in _questions)
            {
                
                if (item.Equals(itemString, StringComparison.CurrentCulture))
                {
                    break;
                }
                indPick++;
            }
            return indPick;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {

                
                // set the default sdettings
                if (!FirstUse)
                {
                    txtPass.Password = PasswordSetting;
                    PickerSelectedIndex = getSelectedIndexValue(ForgetPwdQue);
                    //PickQuestion.SelectedIndex = getSelectedIndexValue(ForgetPwdQue);
                    txtSecretAnswer.Password = ForgetPwdAns;

                    // canImage.Source = loadImageFromStorage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            base.OnNavigatedTo(e);
            // set the drop down
            PickQuestion.ItemsSource = _questions;
        }
        private void btncancel_click(object sender, System.EventArgs e)
        {
            // TODO: Add event handler implementation here.
            txtPass.Password = "";
            NavigationService.GoBack();
        }

        private void btnsave_click(object sender, System.EventArgs e)
        {
            // save password
            PasswordSetting = txtPass.Password;
            ForgetPwdQue = PickQuestion.SelectedItem.ToString();
            ForgetPwdAns = txtSecretAnswer.Password;
            // for the first user
            if (FirstUse)
            {
                FirstUseGUID = new Guid().ToString();
                FirstUse = false;
            }
            // navigate to login page.
            NavigationService.Navigate((new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute)));

        }

        private void btnreest_click(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.

        }



        private void btnSelectPhoto_Click_1(object sender, RoutedEventArgs e)
        {
            photoChooser.Show();

        }

        void photoChooser_Completed(object sender, PhotoResult e)
        {
            try
            {
                WriteableBitmap writeableTile = new WriteableBitmap(1, 1);
                writeableTile.SetSource(e.ChosenPhoto);
                SaveToFile(writeableTile);

                // canImage.Source = writeableTile;

            }
            catch (Exception ex)
            {

                // swallow
            }

        }

        private BitmapImage loadImageFromStorage()
        {
            var bimg = new BitmapImage();
            try
            {
                using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var stream = iso.OpenFile(GlobalSetting.passFilePath, FileMode.Open, FileAccess.Read))
                    {
                        bimg.SetSource(stream);
                    }
                }
            }
            catch (IsolatedStorageException isoex)
            {

                throw new Exception(isoex.Message);
            }



            return bimg;

        }


        private void SaveToFile(WriteableBitmap bmImage)
        {
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {

                string url;
                //create a file name which is a GUID.


                url = GlobalSetting.passFilePath;

                if (!store.DirectoryExists(GlobalSetting.imagesDirectoryName))
                {
                    store.CreateDirectory(GlobalSetting.imagesDirectoryName);
                }
                using (IsolatedStorageFileStream isoStream = store.OpenFile(@url, FileMode.OpenOrCreate))
                {
                    Extensions.SaveJpeg(bmImage, isoStream, bmImage.PixelWidth, bmImage.PixelHeight, 0, 100);
                }

            }

        }

        private void canImage_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate((new Uri("/Assets/PhotoPassword.xaml", UriKind.RelativeOrAbsolute)));
        }
    }
}