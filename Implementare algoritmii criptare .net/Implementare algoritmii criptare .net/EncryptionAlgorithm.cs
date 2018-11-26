using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Implementare_algoritmii_criptare.net
{
    public enum Algorithm
    {
        Rijndael,
        Aes,
        DES,
        TripleDES,
        RC2
    }

    public static class EncryptionAlgorithm
    {
        public static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV, CipherMode cm,PaddingMode pm, Algorithm a,ref string SizeString)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (SymmetricAlgorithm sa = SymmetricAlgorithm.Create(a.ToString()))
            {
                sa.Key = Key;
                sa.IV = IV;
                sa.Mode = cm;
                sa.Padding = pm;

                //key size & block size
                SizeString = "";
                KeySizes[] ks = sa.LegalKeySizes;
                SizeString += "No empty fields!\n";
                SizeString += a.ToString() + " key and block sizes are: ";
                foreach (KeySizes k in ks)
                {
                    SizeString+="\n\nLegal min key size = " + k.MinSize+" bits";
                    SizeString += "\nLegal max key size = " + k.MaxSize + " bits";
                }
                ks = sa.LegalBlockSizes;
                foreach (KeySizes k in ks)
                {
                    SizeString += "\n\nLegal min block size = " + k.MinSize + " bits";
                    SizeString += "\nLegal max block size = " + k.MaxSize + " bits";
                }

                ICryptoTransform encryptor = sa.CreateEncryptor(sa.Key, sa.IV);
                
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV, CipherMode cm, PaddingMode pm, Algorithm a)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            
            string plaintext = null;
            
            using (SymmetricAlgorithm sa = SymmetricAlgorithm.Create(a.ToString()))
            {
                sa.Key = Key;
                sa.IV = IV;
                sa.Mode = cm;
                sa.Padding = pm;
                
                ICryptoTransform decryptor = sa.CreateDecryptor(sa.Key, sa.IV);
                
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
