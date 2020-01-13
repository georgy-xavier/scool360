using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace WC.WinerSchool.BL
{
    public class KnowinEncryption
    {
        private string passPhrase;
        private string saltValue, salt;
        private string hashAlgorithm;
        private int passwordIterations;
        private string initVector;
        private string Msg;
        private int keySize;

        public KnowinEncryption()
        {


            passPhrase = getticket();
            salt = getToken(out Msg);
            saltValue = Msg;
            hashAlgorithm = "SHA1";             // can be "MD5"
            passwordIterations = 2;
            initVector = getVectorValue();
            keySize = 128;                // can be 192 or 128


        }

        private string getVectorValue()
        {
            string _temp = "DRonACharYa:";
            string _val3;
            byte[] salt = new byte[] { 0x78, 0x57, 0x8E, 0x5A, 0x5D, 0x63, 0xCB, 0x06 };
            //string[] strArr = null;
            string sp, sp1;
            //string[] sphar = null;
            int val1 = System.Convert.ToInt32('A');
            string str = char.ConvertFromUtf32(75);
            _temp = _temp + str;
            sp = "@2B7c2D";
            string p = getTool(out _val3);
            System.Text.Encoding ascii = System.Text.Encoding.ASCII;
            Byte[] encodedBytes = ascii.GetBytes(_temp);
            sp1 = getZero();
            str = char.ConvertFromUtf32(97);
            _temp = _temp + str;
            str = char.ConvertFromUtf32(105);
            _temp = _temp + str + 5;
            _temp = _temp + p + _val3;
            ascii = System.Text.Encoding.ASCII;
            //encodedBytes = ascii.GetBytes(_temp);
            //_temp = strArr[1];        
            string str1 = char.ConvertFromUtf32(121);
            _temp = _temp + str1;
            Msg = _temp;
            _temp = sp + sp1;
            return _temp;

        }

        private string getZero()
        {
            string v1 = "2e8F5g7H8", v2 = "asdhjhgjhg", v4 = "mj^%$ MMJKihgdgdkkk";
            int temp = 1, temp3 = 200, temp4 = 9000;
            v2 = v1 + (100 * temp4 / 2);
            v4 = v1;
            v1 = v2 + v1 + (temp + temp3); v1 = v4;
            return v1;


        }





        public string Encrypt(string plainText)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            string cipherText = Convert.ToBase64String(cipherTextBytes);

            // Return encrypted string.
            return cipherText;
        }

        /// <summary>
        /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
        /// </summary>
        /// <param name="cipherText">
        /// Base64-formatted ciphertext value.
        /// </param>
        /// <param name="passPhrase">
        /// Passphrase from which a pseudo-random password will be derived. The
        /// derived password will be used to generate the encryption key.
        /// Passphrase can be any string. In this example we assume that this
        /// passphrase is an ASCII string.
        /// </param>
        /// <param name="saltValue">
        /// Salt value used along with passphrase to generate password. Salt can
        /// be any string. In this example we assume that salt is an ASCII string.
        /// </param>
        /// <param name="hashAlgorithm">
        /// Hash algorithm used to generate password. Allowed values are: "MD5" and
        /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
        /// </param>
        /// <param name="passwordIterations">
        /// Number of iterations used to generate password. One or two iterations
        /// should be enough.
        /// </param>
        /// <param name="initVector">
        /// Initialization vector (or IV). This value is required to encrypt the
        /// first block of plaintext data. For RijndaelManaged class IV must be
        /// exactly 16 ASCII characters long.
        /// </param>
        /// <param name="keySize">
        /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
        /// Longer keys are more secure than shorter keys.
        /// </param>
        /// <returns>
        /// Decrypted string value.
        /// </returns>
        /// <remarks>
        /// Most of the logic in this function is similar to the Encrypt
        /// logic. In order for decryption to work, all parameters of this function
        /// - except cipherText value - must match the corresponding parameters of
        /// the Encrypt function which was called to generate the
        /// ciphertext.
        /// </remarks>

        private string getticket()
        {
            string _val = "";
            _val = getpublickey();
            return _val;
        }

        private string getpublickey()
        {
            string _temp = "DRonACharYa:";
            string _val3;
            byte[] salt = new byte[] { 0x78, 0x57, 0x8E, 0x5A, 0x5D, 0x63, 0xCB, 0x06 };
            string[] strArr = null;
            char[] sphar = { ':' };
            int val1 = System.Convert.ToInt32('A');
            string str = char.ConvertFromUtf32(75);
            _temp = _temp + str;
            string p = getTool(out _val3);
            System.Text.Encoding ascii = System.Text.Encoding.ASCII;
            Byte[] encodedBytes = ascii.GetBytes(_temp);
            str = char.ConvertFromUtf32(97);
            _temp = _temp + str;
            str = char.ConvertFromUtf32(105);
            _temp = _temp + str + 5;
            _temp = _temp + p + _val3;
            strArr = _temp.Split(sphar);
            ascii = System.Text.Encoding.ASCII;
            encodedBytes = ascii.GetBytes(_temp);
            _temp = strArr[1];
            return _temp;

        }


        private string getToken(out string Msg)
        {
            string _temp = "StraiGht:", _val3, P1 = "s", vul1 = "hshshsdhasdgTTgag###hsgas";
            Msg = "";
            int i = 4;
            string[] strArr = null;
            char[] sphar = { 'n' };
            int val1 = System.Convert.ToInt32('A');
            string str1 = char.ConvertFromUtf32(121);
            _temp = _temp + str1;
            string p = getTool(out _val3);
            strArr = _val3.Split(sphar);
            System.Text.Encoding ascii = System.Text.Encoding.ASCII;
            Byte[] encodedBytes = ascii.GetBytes(_temp);
            getPrime(i, out Msg, out _temp);
            string str = char.ConvertFromUtf32(97);
            _temp = P1 + strArr[0] + _temp + Msg + str + sphar[0] + _temp + str1;
            Msg = _temp;
            vul1 = vul1 + 4 + (100 * 2);
            return _temp;
        }




        private string getTool(out  string _val3)
        {
            _val3 = "";
            string val = "AdQ<*33FfptWin~donmmnmR#@! rr jUdLP:hs'fr'kujff! !()@@%KLn'nnTijuAbrah'aragb JJ OO PP GG", val1 = "", val2 = "@ne", vul1 = "mew@^", vul3 = "KiMMGARa NUKK";
            string[] strArr = null;
            char[] splitchar = { ' ' };
            strArr = val.Split(splitchar);
            int count = 0;
            for (count = 0; count < 2; count++)
            {
                int temp = count + 1;
                if (count == 1)
                {
                    val1 = strArr[1];
                    _val3 = val2;
                    vul1 = val1 + vul3 + vul3;
                    vul1 = vul1 + 4 + (100 * 2);
                }
            }
            return val1;

        }
        private void getPrime(int p, out string Msg, out string _temp)
        {
            Msg = "";//tNan
            string val = "AdQ<*33Ff*ptWin~d*onmmn*tN*R#@! rr j*NdLP:hs'fr'kujff! !()@@%KLn'nnTijuAbrah'aragb JJ OO PP GG", val1 = "u", val2 = "@ne", vul1 = "mew@^", vul3 = "KiMMGARa NUKK";
            string[] strArr, strArr1, strArr2 = null;
            char[] sar = { '*' };
            char[] sar1 = { '#' };
            char[] sar3 = { ' ' };
            char[] sar4 = { '@' };
            strArr = val.Split(sar);
            _temp = val1;
            val2 = strArr[p];//tN
            vul3 = val2;
            vul1 = val1 + vul3 + vul3;
            Msg = val2;
            strArr1 = val.Split(sar1);
            vul1 = vul1 + 4 + (100 * 2);
            strArr2 = val.Split(sar3);


        }
        public string Decrypt(string cipherText)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                            passPhrase,
                                                            saltValueBytes,
                                                            hashAlgorithm,
                                                            passwordIterations);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(keySize / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                                                             keyBytes,
                                                             initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                          decryptor,
                                                          CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            string plainText = Encoding.UTF8.GetString(plainTextBytes,
                                                       0,
                                                       decryptedByteCount);

            // Return decrypted string.   
            return plainText;
        }

    }

}