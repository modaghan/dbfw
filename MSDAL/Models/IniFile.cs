using System.Runtime.InteropServices;
using System.Text;

namespace MS.BLL
{
    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);


        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            if (!Key.Equals("Crypto"))
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Value);
                WritePrivateProfileString(Section, Key, System.Convert.ToBase64String(plainTextBytes), this.path);
            }
            else
            {
                WritePrivateProfileString(Section, Key, Value, this.path);
            }

        }


        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(4000);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            4000, this.path);
            if (!Key.Equals("Crypto"))
            {
                var base64EncodedBytes = System.Convert.FromBase64String(temp.ToString());
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            else
            {
                return temp.ToString();
            }
           
        }
    }
}
