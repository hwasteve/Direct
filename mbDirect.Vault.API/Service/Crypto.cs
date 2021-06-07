using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace mbDirect.Vault.API.Service
{
    /// <summary>Provide Enryption, Hashing and random utility
    /// 
    /// </summary>
    public class Crypto
    {
        public enum PwdMethod { HSHA0 }
        public enum TypeAlgorithm {AES}

        private SymmetricAlgorithm CryptoProvider;

        public Crypto(TypeAlgorithm t)
        {
            switch (t)
            {
                case TypeAlgorithm.AES:
                    CryptoProvider = new AesManaged();
                    break;
            }
        }
        public String Encrypt(String ClearText, Byte[] KeyByte, Byte[] Vector)
        {
            byte[] ClearTextByte = System.Text.ASCIIEncoding.ASCII.GetBytes(ClearText);

            CryptoProvider.Mode = CipherMode.CBC;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ICryptoTransform encryptor = CryptoProvider.CreateEncryptor(KeyByte, Vector);
            CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(ClearTextByte, 0, ClearTextByte.Length);
            cs.FlushFinalBlock();

            Byte[] OutByte = ms.ToArray();
            ms.Close();
            cs.Close();

            return System.Convert.ToBase64String(OutByte);
        }
        public String Encrypt(String ClearText, String EncryptKey, String Vector)
        {

            byte[] KeyByte = LegalKey(EncryptKey);
            byte[] VectorByte = LegalKey(Vector);
            return Encrypt(ClearText, KeyByte, VectorByte);
        }

        public String Decrypt(String EncryptedString, Byte[] KeyByte, Byte[] Vector)
        {
            Byte[] InByte = System.Convert.FromBase64String(EncryptedString);
            CryptoProvider.Mode = CipherMode.CBC;

            if (Vector.Length == 0)
                Vector = new Byte[8];

            MemoryStream ms = null;
            CryptoStream cs = null;
            StreamReader sr = null;

            try
            {
                ms = new MemoryStream(InByte, 0, InByte.Length);
                ICryptoTransform decryptor = CryptoProvider.CreateDecryptor(KeyByte, Vector);
                using (cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    sr = new StreamReader(cs);
                    return sr.ReadToEnd();
                }
            }
            catch (CryptographicException ex)
            {
                 return String.Empty;
            }
            finally
            {
                if (ms != null) ms.Close();
            }
        }


        public String Decrypt(String EncryptedString, String EncryptKey, String Vector)
        {
            Byte[] KeyByte = LegalKey(EncryptKey);
            Byte[] VectorByte = LegalKey(Vector);
            return Decrypt(EncryptedString, KeyByte, VectorByte);
        }

        private Byte[] LegalKey(String key)
        {
            String TempKeyString = String.Empty;
            if (CryptoProvider.LegalKeySizes.Length > 0)
            {
                int lessSize = 0, moreSize = CryptoProvider.LegalKeySizes[0].MinSize;
                // key sizes are in bits

                while ((key.Length * 8 > moreSize) && (CryptoProvider.LegalKeySizes[0].SkipSize > 0))
                {
                    lessSize = moreSize;
                    moreSize += CryptoProvider.LegalKeySizes[0].SkipSize;
                }
                if (key.Length * 8 < moreSize)
                    TempKeyString = key.PadRight(moreSize / 8, ' ');
                else if (key.Length * 8 > CryptoProvider.LegalKeySizes[0].MaxSize)
                    TempKeyString = key.Substring(0, CryptoProvider.LegalKeySizes[0].MaxSize / 8);
                else TempKeyString = key;
            }
            else TempKeyString = key;

            return ASCIIEncoding.ASCII.GetBytes(TempKeyString);
        }

        public static String EncryptString(String ClearText, String key, String Vector, Crypto.TypeAlgorithm t)
        {
            Crypto c = new Crypto(t);
            return c.Encrypt(ClearText, key, Vector);
        }

        public static String DecryptString(String EncryptedText, String key, String Vector, Crypto.TypeAlgorithm t)
        {
            Crypto c = new Crypto(t);
            return c.Decrypt(EncryptedText, key, Vector);
        }

        /// <summary>Hashing
        /// 
        /// </summary>
        /// <param name="ClearText"></param>
        /// <returns></returns>
        public static String Hash(String ClearText, HashAlgorithm t)
        {
            Byte[] ByteOut = t.ComputeHash(Encoding.Default.GetBytes(ClearText));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ByteOut.Length; i++)
                sb.Append(ByteOut[i].ToString("x2"));

            return sb.ToString();
        }

        public static String Hash(String ClearText, HashAlgorithm t, Encoding encoding)
        {
            Byte[] ByteOut = t.ComputeHash(encoding.GetBytes(ClearText));

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ByteOut.Length; i++)
                sb.Append(ByteOut[i].ToString("x2"));

            return sb.ToString();
        }

        /// <summary>Get Crypto generated random text string
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String CreateRandomText(int size)
        {
            return CreateRandomText(size, false);
        }
        /// <summary>Get randomly generated string
        /// 
        /// </summary>
        /// <param name="Size"></param>
        /// <param name="DigitsOnly">if true, the resulted string only contains numeric digits and the first digit will not be 0 </param>
        /// <returns></returns>
        public static String CreateRandomText(int Size, Boolean DigitsOnly)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            Byte[] Seed = new Byte[8];
            rng.GetBytes(Seed);
            Random random = new Random(BitConverter.ToInt32(Seed, 0));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Size; i++)
            {
                int Min = DigitsOnly && (i == 0) ? 49 :
                            DigitsOnly ? 48 : 65;
                int Range = DigitsOnly && (i == 0) ? 9 :
                            DigitsOnly ? 10 : 58;

                char c = Convert.ToChar(Convert.ToInt32(Math.Floor(Range * random.NextDouble() + Min)));
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}
