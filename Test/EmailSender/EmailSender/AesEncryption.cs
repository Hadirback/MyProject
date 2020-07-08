using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace EmailSender
{
    internal class AesEncryption
    {
        private static readonly Byte[] IV = Encoding.UTF8.GetBytes("1234123412341234");

        public static Byte[] EncryptStringToBytes_Aes(Byte[] data)
        {
            Byte[] aesKey = null;

            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "cfg.xml")))
            {
                throw new ArgumentNullException($"There is no file in the given directory!");
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("cfg.xml");
            XmlElement xRoot = xDoc.DocumentElement;

            String aesString = xRoot.SelectSingleNode("descendant::section[@name='Notification']")
                            .SelectSingleNode("descendant::entry[@name = 'AesKey']").InnerText;

            if (String.IsNullOrEmpty(aesString))
            {
                throw new ArgumentNullException($"Failed to get line from file!");
            }

            aesKey = Encoding.UTF8.GetBytes(aesString);

            // Check arguments.
            if (data == null || data.Length <= 0)
            {
                throw new ArgumentNullException($"The input data is empty. {nameof(data)}");
            }

            if (aesKey == null || aesKey.Length <= 0)
            {
                throw new ArgumentNullException($"AES key is empty. {nameof(aesKey)}");
            }

            if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException($"IV is empty. {nameof(IV)}");
            }

            Byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = aesKey;
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}

