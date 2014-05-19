
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ForgetME
{
    public static class GlobalSetting
    {
        public static SettingsExt settingsExt = new SettingsExt();

        public static Point[] originalTapPoints;

        public const string passFileName = "FME__PASS_IMG";

        public const string passFilePath = @"Images\FME_PASS_IMG";

        public const string imagesDirectoryName = "Images";


        public static  List<string> _questions = new List<string>
        {
            "What is your first phone number?",
            "Who is your childhood hero?",
            "Who is your first crush?",
            "My own secret question ___?"

        };

        //private static volatile Notes localNotes;

        ////TODO: create a singleton object for local notes.

        //public static Notes GetNotesInstance()
        //{
        //    if (localNotes == null)
        //    {
        //        localNotes = new Notes();
        //    }
        //    return localNotes;
        //}

    }
}
