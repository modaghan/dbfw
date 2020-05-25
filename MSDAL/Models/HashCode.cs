using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MS.BLL
{
    public class HashCode
    {
        private const string publicKey = "<RSAKeyValue><Modulus>vKzm/rFi2jPG6fumvhDWeZ49ZZhUh5EEXWW5fs1Y5iFW2poGA9I2sdeBBVgE16DshQY+VdW+e4uaXZJncuy+MdHoe9HWGx6iAVB3PSFUGNhaMQnX+bv+GWORBoFMBpx2ZGUqOSbazIxBpTxyg0DnXdpeFbVrWONq/A7RsmLWHZKk1cdoJ15YPI85IPFO3YwsfBDCYMHaaVcy7Ac9UvJGasopzfQq7dPM8d0xj93VUPq8La4psejJ/N56IYjs/+rvoFvACbt8U+a9UMWjNVbnBm4+MjcxASbxLkd3izRmZ5Jtv8YRoHOKkxlgG8vVVcNPNONoMNdE+YmxeZh7pH1w0Q==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private const string privateKey = "<RSAKeyValue><Modulus>vKzm/rFi2jPG6fumvhDWeZ49ZZhUh5EEXWW5fs1Y5iFW2poGA9I2sdeBBVgE16DshQY+VdW+e4uaXZJncuy+MdHoe9HWGx6iAVB3PSFUGNhaMQnX+bv+GWORBoFMBpx2ZGUqOSbazIxBpTxyg0DnXdpeFbVrWONq/A7RsmLWHZKk1cdoJ15YPI85IPFO3YwsfBDCYMHaaVcy7Ac9UvJGasopzfQq7dPM8d0xj93VUPq8La4psejJ/N56IYjs/+rvoFvACbt8U+a9UMWjNVbnBm4+MjcxASbxLkd3izRmZ5Jtv8YRoHOKkxlgG8vVVcNPNONoMNdE+YmxeZh7pH1w0Q==</Modulus><Exponent>AQAB</Exponent><P>3Wj62DgD6UFsoOxlgr7FpDM7Ev3/K4IfdHUkznGpVXWALawFMNMLmMIvp1NwFJ9U05+6IA3xnoq0Ue7mrJZ4HPqb7qwrJ9hG5gpXZzyMVUvcblJxchzet6N5fb3yMDX3A6zwfrsnaYucHVWmdf/MhWtkM/jQQN+Z1kAxkB+NTz0=</P><Q>2ia9ZGUKmw8Jsn0Ew13OkYzyjyw6E5ffwEDafpwJ8AojoKnFI1b4NSkYt3oAQbamLTyupoxL+3DQ7d7TeMoQqunM/tWOukZo3rAyaa/KRoNIcbDNLTdtzpdxgfZmZh6WrEHNkDx5y/UGqb/ceVlKeDCSQvE+sTuagbxCPoLkwSU=</Q><DP>xjGYAf66eY1oGPEjuRLeRqrZYZneVesIDy5hgS87flVNJRUMHHV+twJ0t9q3xK4Pt9QOP21b8SiGW6V39dxHruEivlZ91xAB/yAYtz/6+suKiXLhPF3dfBMoyMdESaW09SRUr40GrbMcTyIBfTU6td+49dDvUnMV+TTDaRjlXJ0=</DP><DQ>DJDUsfa8AKiCF3zqDFLX9jxXMHYMtlo2Mj3KGCbmz6PV34hH6bw1ueIvIUpuv1pFAjAPo1pLeiVKc5k1Nyz0ftPO0hL9EK/DlKgzjzDoBt3DC4FyoBskQRUqHaFSzqkOZse3jopdPalUg+ygR4EkL/4kPqTkxpK3WKe+bRlfEd0=</DQ><InverseQ>gSF7iEokHbW9m4IgZa3HL0fDcLAQUK6JO4uq/RM5/VgHtNR785OLU3pQjqHGgu2SmtwitNf+R8TKHkz8/777qKk3MUb9SRAApKFomSPQXU8Iu3m/WJuYCfFq+MeNCyNQ+UNsD7qqI6Xie/wsExMOa8+HP7VsxhOX7btVqErz5+0=</InverseQ><D>nW70zJ79ai98Ei/O4ZexLwgQGR7zoa8q4jgIgTsdq+Ez1PJihHu68chtuyTH3ZlE4nbkOsFA0VwasWuBcI8E4RNTF0Zvjm+QJOKcrGCMCLM3BuY81gC8tTi0gaYP5xBVZc5YXhoCxl1eRV9b+hOFO3YDvb+E1EXnNm2zIlOAcGlG44ezABU+MypPPEWr00DVG9AYSAjS93DoCPO01EMPJCf8eRcIQpnjJf22o0+0fgI3rkfkuJI6W1W16sqHHYJMv3KNo7kakL7OzKUcAZgCKx7IPQ9ilsYmKe0Ffj4qsEPrk2lIts/S9rYJWAoqBh1PdPh5F2P+adDz/A1HY4ZUsQ==</D></RSAKeyValue>";
        public static byte[] GetHash(string password)
        {
            HashAlgorithm algorithm = SHA1.Create();  // SHA1.Create()
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        public string GetHashString(string password)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(password))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        public string EncryptionConfig(string text)
        {
            byte[] textData = Encoding.UTF8.GetBytes(text);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(1024))
            {
                try
                {
                    rsa.FromXmlString(publicKey);
                    byte[] encryptedData = rsa.Encrypt(textData, true);
                    return Convert.ToBase64String(encryptedData);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        public string DecryptionConfig(string text)
        {
            byte[] textData = Encoding.UTF8.GetBytes(text);

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(8192))
            {
                try
                {
                    string base64Encrypted = text;
                    rsa.FromXmlString(privateKey);
                    byte[] resultBytes = Convert.FromBase64String(base64Encrypted);
                    byte[] decryptedBytes = rsa.Decrypt(resultBytes, true);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        public byte[] XmlToByte(XmlDocument xmlDocument)
        {
            return Encoding.UTF8.GetBytes(xmlDocument.OuterXml);
        }
        public string XmlToHash(byte[] xmlbytes)
        {
            MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();

            xmlbytes = md5CryptoServiceProvider.ComputeHash(xmlbytes);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte xmltobyte in xmlbytes) { stringBuilder.Append(xmltobyte.ToString("x2").ToLower()); }
            return stringBuilder.ToString();
        }
    }
}
