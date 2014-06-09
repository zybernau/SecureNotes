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
using Microsoft.Live;
using System.Threading.Tasks;
using System.Threading;
namespace ForgetME
{
    public partial class SettingsExt : PhoneApplicationPage
    {

        // Our settings
        IsolatedStorageSettings settings;
        PhotoChooserTask photoChooser;
        NoteSec noteSec;

        // live sdk connect session.
        LiveConnectSession _currentSession;

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

        //private List<string> _questions = new List<string>
        //{
        //    "What is your first phone number?",
        //    "Who is your childhood hero?",
        //    "Who is your first crush?",
        //    "My own secret question ___?"

        //};
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
                return GlobalSetting._questions;
            }

        }


        private int getSelectedIndexValue(String itemString)
        {
            int indPick = 0;
            foreach (String item in GlobalSetting._questions)
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
            PickQuestion.ItemsSource = GlobalSetting._questions;
            PickQuestion.SelectedIndex = PickerSelectedIndex;

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

        #region onedrive sync

        /**
         * 1. Read the IDs from the id file.
         * 2. match it with current system, there is no need to sync from the one drive, as there is going to be onie way comunuication.
         * 3. add the updated ids to the sync folder.
         * 4. 
         * lets have this way, file name be the key name and the contents of the file will be the part, just copy the files to sync folder if it doesn't exists there, then copy all 
         * the files from the sync folder to the notes folder so that it is synced here as well.
         * */
        private async void SignInButton_SessionChanged(object sender, Microsoft.Live.Controls.LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status == Microsoft.Live.LiveConnectSessionStatus.Connected)
            {
                _currentSession = e.Session;
                var coreContent = await CreateRootFolder(_currentSession);
                //get contents
                var res = GetFolderContents(_currentSession, coreContent);
                syncContents(_currentSession, coreContent);
            }

        }


        private async static void syncContents(LiveConnectSession session, string folderId)
        {
            // get the notes. instance
            ICollection keyNotes = Notes.Instance.GetKeys();

            IsolatedStorageFile isoStore;
            IsolatedStorageSettings noteKeys;
            noteKeys = IsolatedStorageSettings.ApplicationSettings;
            LiveConnectClient client = new LiveConnectClient(session);
            // LiveOperationResult result = await client.GetAsync(folderId + "/files");
            String fileNameOnly = string.Empty;

            try
            {
                // check if it is first time to sync down the notes, if some exists.
                //FirstUse
                if ((bool)noteKeys[FirstTimeUseKeyName] )
                {


                    LiveOperationResult operationResult = await client.GetAsync(folderId + "/files");
                    dynamic result = operationResult.Result;
                    if (result.data == null)
                    {

                    }
                    else
                    {
                        dynamic items = result.data;

                        foreach (dynamic item in items)
                        {
                            SkyDriveEntity entity = new SkyDriveEntity();

                            IDictionary<string, object> dictItem = (IDictionary<string, object>)item;


                            LiveDownloadOperationResult downloadOperationResult =
                        await client.DownloadAsync(dictItem["id"].ToString() + "/content");

                            Stream fileContent = downloadOperationResult.Stream;
                            fileNameOnly = dictItem["name"].ToString().Substring(0, dictItem["name"].ToString().LastIndexOf('.'));
                            noteKeys.Add(fileNameOnly, "");

                            SaveToStorage(fileContent, dictItem["name"].ToString());

                        }
                    }
                }
            }
            catch (LiveConnectException e)
            {
                Console.WriteLine(e.Message);

            }
            try
            {

                foreach (var noteKey in keyNotes)
                {
                    using (isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        using (var stream = new IsolatedStorageFileStream("/Notes/" + noteKey, FileMode.Open, FileAccess.Read, isoStore))
                        {
                            LiveOperationResult operationResult = await client.UploadAsync(folderId + "/", noteKey.ToString() , stream, OverwriteOption.Overwrite);
                        }
                    }
                }
            }
            catch (FileNotFoundException fnfex)
            {
                // file not found
                Console.WriteLine(fnfex.Message);
            }
            catch (Exception ex)
            {
                // file not found
                Console.WriteLine(ex.Message);
            }

        }

        public async static Task<List<SkyDriveEntity>> GetFolderContents(LiveConnectSession session, string folderId)
        {
            List<SkyDriveEntity> data = new List<SkyDriveEntity>();




            try
            {
                LiveConnectClient client = new LiveConnectClient(session);
                LiveOperationResult result = await client.GetAsync(folderId + "/files");

                List<object> container = (List<object>)result.Result["data"];

                foreach (var item in container)
                {
                    SkyDriveEntity entity = new SkyDriveEntity();

                    IDictionary<string, object> dictItem = (IDictionary<string, object>)item;
                    string type = dictItem["type"].ToString();
                    entity.IsFolder = type == "folder" || type == "album" ? true : false;
                    entity.ID = dictItem["id"].ToString();
                    entity.Name = dictItem["name"].ToString();

                    data.Add(entity);
                }




                // 


                return data;
            }
            catch
            {
                return null;
            }
        }

        private static void SaveToStorage(Stream file, String fileName)
        {
            using (IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var isoFileStream = new IsolatedStorageFileStream("/Notes/" + fileName, FileMode.OpenOrCreate, myStore))
                {
                    using (file)
                    {
                        long length = file.Length;
                        byte[] buffer = new byte[length];

                        int readLength = file.Read(buffer, 0, (int)length);

                        isoFileStream.Write(buffer, 0, readLength);
                    }
                }


            }
        }

        public async static Task<string> CreateRootFolder(LiveConnectSession session)
        {
            string folderId = string.Empty;
            try
            {
                var data = await GetFolderContents(session, "me/skydrive");

                if (data != null)
                {
                    var targetRootFolder = (from c in data where c.IsFolder && c.Name == "SYNC_SECURE_NOTES" select c).FirstOrDefault();

                    if (targetRootFolder == null)
                    {
                        LiveConnectClient client = new LiveConnectClient(session);
                        LiveOperationResult result = await client.PostAsync("me/skydrive", "{\"name\":\"SYNC_SECURE_NOTES\",\"description\":\"SYNCED FOLDER\"}");
                        folderId = result.Result["id"].ToString();
                    }
                    else
                    {
                        folderId = targetRootFolder.ID;
                    }
                }

                return folderId;
            }
            catch
            {
                return folderId;
            }
        }


        #endregion
    }

    public class SkyDriveEntity
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public bool IsFolder { get; set; }
    }
}