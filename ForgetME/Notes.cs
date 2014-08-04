using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace ForgetME
{
    public sealed class Notes
    {
        IsolatedStorageSettings noteKeys;
        IsolatedStorageFile isoStore;

        private static volatile Notes _instance;
        private static object syncRoot = new object();

        private readonly object _readLock = new object();
        public static string StatusCallBack = "";
        public const string NotesDirectoryName = "Notes";
        public const string NotesDirectoryFormatedName = "/Notes/";
        private NoteSec notesSec;
        // single ton instance.
        public static Notes Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new Notes();
                        }
                    }
                }
                return _instance;
            }
        }
        // default constructor
        public Notes()
        {
            noteKeys = IsolatedStorageSettings.ApplicationSettings;
            notesSec = new NoteSec();

            using (isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isoStore.DirectoryExists(NotesDirectoryName))
                {
                    isoStore.CreateDirectory(NotesDirectoryName);
                }

                Console.WriteLine("instansiation done. \n Directory check done.");

                //isoStore.CreateFile("Notes/MyNotesFile.txt");
                //Console.WriteLine("Created a new file in the root.");


                //Console.WriteLine("Created a new file in the InsideDirectory.");
            }


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
            try
            {


                // If the key exists
                if (!noteKeys.Contains(Key))
                {
                    noteKeys.Add(Key, "");
                }

                // If the value has changed
                if (noteKeys[Key] != value)
                {
                    // Store the new value
                    using (isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        isoStore.CreateFile(NotesDirectoryFormatedName + Key + ".txt").Dispose();
                        // encrypt the value before you save.
                        byte[] encs = notesSec.EncryptString(value.ToString());
                        lock (_readLock)
                        {
                            using (var isoStream = new IsolatedStorageFileStream(NotesDirectoryFormatedName + Key + ".txt", FileMode.Open, FileAccess.Write, isoStore))
                            {
                                isoStream.Seek(0, SeekOrigin.Begin);
                                int offset = 0;
                                int total = encs.Length;
                                isoStream.Write(encs, offset, total - offset);
                                isoStream.Flush();
                            }
                        }


                    }
                    noteKeys[Key] = "";
                    Save();
                    valueChanged = true;
                }
            }
            catch (IsolatedStorageException isoex)
            {
                throw new Exception("Unable to get the storage, Try again");
            }
            catch (Exception ex)
            {

                throw ex;
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
            if (noteKeys.Contains(Key))
            {
                value = (T)noteKeys[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        public ICollection GetKeys()
        {
            ICollection notes = getNotes();
            return notes;
        }
        public String GetNote(String key)
        {
            String Note = "";
            Note = GetValue(key);
            return Note;
        }
        public bool SetNote(String key, String value)
        {
            bool Note = false;
            try
            {

                Note = AddOrUpdateValue(key, value);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Note;
        }
        public bool DeleteNote(String key)
        {

            return removeNote(key);
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        public void Save()
        {
            noteKeys.Save();

        }

        #region Private methods

        private string[] getNotes()
        {
            using (isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string mDir = NotesDirectoryName;
                return isoStore.GetFileNames(mDir + @"\*.*");
            }
        }



        private String GetValue(string Key)
        {
            String value = "";

            try
            {

                // If the key exists, retrieve the value.
                if (noteKeys.Contains(Key))
                {
                    // encrypt the value before you save.
                    try
                    {
                        using (isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (var stream = new IsolatedStorageFileStream(NotesDirectoryFormatedName + Key + ".txt", FileMode.Open, FileAccess.Read, isoStore))
                            {
                                Stream stmReader = new StreamReader(stream).BaseStream;
                                byte[] encData = new byte[stmReader.Length];
                                stmReader.Read(encData, 0, encData.Length); // read the values, from the stream, from the text file.
                                value = notesSec.DecryptString(encData); // decrypt the pin
                                stmReader.Close();
                            }
                        }

                    }
                    catch (FileNotFoundException fnfex)
                    {
                        // file not found
                        Console.WriteLine(fnfex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return value;
        }


        private bool removeNote(String key)
        {
            if (noteKeys.Contains(key))
            {
                noteKeys.Remove(key);
                using (isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    isoStore.DeleteFile(NotesDirectoryFormatedName + key + ".txt");
                }

            }
            Save();
            return true;
        }



        #endregion





    }
}
