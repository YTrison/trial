using System;
using System.IO;

namespace CoreEncryption
{
    public class EncryptHandler
    {
        private const string JTP_DEFAULT_PASSWORD = "";
        private static string password = "JTP-System123";
        private const int JTP_RANDOMBYTES_SIZE = 16;// 16 Bytes will give us 128 bits, 32 Bytes will give us 256 bits.
        private const int JTP_DERIVIATION_ITERATIONS = 1000;
        

        internal static string Encrypt(string plain_text, string password_old)
        {
            string cipher_text = "";
            try
            {
                Rijndael256.Settings.HashIterations = JTP_DERIVIATION_ITERATIONS;
                cipher_text = Rijndael256.Rijndael.Encrypt(plain_text, password, Rijndael256.KeySize.Aes256);
            }
            catch (Exception) { }
            return cipher_text;
        }

        internal static string Decrypt(string cipher_text, string password_old)
        {
            string plain_text = "";
            try
            {
                Rijndael256.Settings.HashIterations = JTP_DERIVIATION_ITERATIONS;
                plain_text = Rijndael256.Rijndael.Decrypt(cipher_text, password, Rijndael256.KeySize.Aes256);
            }
            catch (Exception) { }
            return plain_text;
        }

        internal static byte[] Encrypt(byte[] plain_byte, string password_old = JTP_DEFAULT_PASSWORD)
        {
            byte[] cipher_byte = null;
            byte[] initVectorBytes = Rijndael256.Rng.GenerateRandomBytes(JTP_RANDOMBYTES_SIZE);
            try
            {
                Rijndael256.Settings.HashIterations = JTP_DERIVIATION_ITERATIONS;
                cipher_byte = Rijndael256.Rijndael.EncryptByte(plain_byte, password, Rijndael256.KeySize.Aes256);
            }
            catch (Exception) { }
            return cipher_byte;
        }

        public static byte[] Decrypt(byte[] cipher_byte, string password_old = JTP_DEFAULT_PASSWORD)
        {
            byte[] plain_byte = null;
            try
            {
                Rijndael256.Settings.HashIterations = JTP_DERIVIATION_ITERATIONS;
                plain_byte = Rijndael256.Rijndael.DecryptByte(cipher_byte, password, Rijndael256.KeySize.Aes256);
            }
            catch (Exception e)
            {
                string message = e.Message.ToString();
            }
            finally
            {
            }
            return plain_byte;
        }

        public static string ProcessTextToText(string inputText, string passPhrase, string mode)
        {
            if (mode == "Encrypt")
            {
                return Encrypt(inputText, passPhrase);
            }
            else if (mode == "Decrypt")
            {
                return Decrypt(inputText, passPhrase);
            }
            else
            {
                return "";
            }
        }

        public static void ProcessTextToFile(string inputText, string OutputFileLocation, string passPhrase, string mode)
        {
            if (mode == "Encrypt")
            {
                File.WriteAllText(OutputFileLocation, Encrypt(inputText, passPhrase));
            }
            else if (mode == "Decrypt")
            {
                File.WriteAllText(OutputFileLocation, Decrypt(inputText, passPhrase));
            }
        }

        public static string ProcessFileToText(string InputFileLocation, string passPhrase, string mode)
        {
            string inputString = File.ReadAllText(InputFileLocation);

            if (mode == "Encrypt")
            {
                return Encrypt(inputString, passPhrase);
            }
            else if (mode == "Decrypt")
            {
                return Decrypt(inputString, passPhrase);
            }
            else
            {
                return "";
            }
        }

        public static void ProcessFileToFile(string InputFileLocation, string OutputFileLocation, string passPhrase, string mode)
        {
            byte[] inputByte = File.ReadAllBytes(InputFileLocation);

            if (mode == "Encrypt")
            {
                File.WriteAllBytes(OutputFileLocation, Encrypt(inputByte, passPhrase));
            }
            else if (mode == "Decrypt")
            {
                File.WriteAllBytes(OutputFileLocation, Decrypt(inputByte, passPhrase));
            }
        }

        public static byte[] ProcessFileToByte(string InputFileLocation, string passPhrase, string mode)
        {
            byte[] inputByte = File.ReadAllBytes(InputFileLocation);

            if (mode == "Encrypt")
            {
                return Encrypt(inputByte, passPhrase);
            }
            else if (mode == "Decrypt")
            {
                return Decrypt(inputByte, passPhrase);
            }
            else
            {
                return null;
            }
        }

        public static byte[] ProcessByteToByte(byte[] inputByte, string passPhrase, string mode)
        {
            if (mode == "Encrypt")
            {
                return Encrypt(inputByte, passPhrase);
            }
            else if (mode == "Decrypt")
            {
                return Decrypt(inputByte, passPhrase);
            }
            else
            {
                return null;
            }
        }

        private static string GetRightPartOfPath(string path, string startAfterPart)
        {
            // use the correct seperator for the environment
            var pathParts = path.Split(Path.DirectorySeparatorChar);

            // this assumes a case sensitive check. If you don't want this, you may want to loop through the pathParts looking
            // for your "startAfterPath" with a StringComparison.OrdinalIgnoreCase check instead
            int startAfter = Array.IndexOf(pathParts, startAfterPart);

            if (startAfter == -1)
            {
                // path path not found
                return null;
            }

            // try and work out if last part was a directory - if not, drop the last part as we don't want the filename
            var lastPartWasDirectory = pathParts[pathParts.Length - 1].EndsWith(Path.DirectorySeparatorChar.ToString());
            return string.Join(
                Path.DirectorySeparatorChar.ToString(),
                pathParts, startAfter,
                pathParts.Length - startAfter - (lastPartWasDirectory ? 0 : 1));
        }

        public static void ProcessFolderToFolder(string inputFolderLocation, string outputFolderLocation, string passPhrase, string mode)
        {
            //string[] Fname = Directory.GetFiles(txt_Lokasi.Text);
            String[] allfiles = System.IO.Directory.GetFiles(inputFolderLocation, "*.*", System.IO.SearchOption.AllDirectories);
            string Lfolder = Path.GetFileName(inputFolderLocation);
            foreach (string nama in allfiles)
            {
                FileInfo path = new FileInfo(nama);

                //buat directory bila belum ada
                if (!Directory.Exists(outputFolderLocation + "\\" + GetRightPartOfPath(nama, Lfolder)))
                    Directory.CreateDirectory(outputFolderLocation + "\\" + GetRightPartOfPath(nama, Lfolder));
                string lokasi = GetRightPartOfPath(nama, Lfolder);
                ProcessFileToFile(nama, (outputFolderLocation + "\\" + GetRightPartOfPath(nama, Lfolder) + "\\" + path.Name), passPhrase, mode);
            }
        }
    }
}
