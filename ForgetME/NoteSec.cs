using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;


namespace ForgetME
{
    public sealed class NoteSec
    {

        private string fGuid = GlobalSetting.settingsExt.FirstUseGUID;

        private byte[] encKey;
        public NoteSec()
        {
            string keySec = GlobalSetting.settingsExt.PasswordSetting;

        }

        public String NotesSecKey
        {
            set
            {

                encKey = Convert.FromBase64String(value); ;

            }

        }

        public byte[] EncryptString(string plainText)
        {

            return EncryptStringToBytesProtected(plainText);
        }

        public string DecryptString(byte[] nonReadableText)
        {
            return DecryptStringFromBytesProtected(nonReadableText);
        }

        private byte[] EncryptStringToBytesProtected(string plainText)
        {
            try
            {


                // Convert the PIN to a byte[].
                byte[] PinByte = Encoding.UTF8.GetBytes(plainText);

                // Encrypt the PIN by using the Protect() method.
                byte[] ProtectedPinByte = ProtectedData.Protect(PinByte, null);

                return ProtectedPinByte;
            }
            catch (CryptographicException ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private string DecryptStringFromBytesProtected(byte[] cipherText)
        {

            try
            {


                // Decrypt the PIN by using the Unprotect method.
                byte[] PinByte = ProtectedData.Unprotect(cipherText, null);

                // Convert the PIN from byte to string and display it in the text box.
                return Encoding.UTF8.GetString(PinByte, 0, PinByte.Length);
            }
            catch (CryptographicException ex)
            {

                throw new Exception(ex.Message);
            }
        }



    }
}
