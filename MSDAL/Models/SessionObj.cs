using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class SessionObj
    {
        public SessionObj(string programVersionCode, string runningMode, object currentUser
            , DateTime loginTime, DateTime processStartTime, DateTime processEndTime, string[] ip, string computerName, string folderPath, IList<string> networkAdapter, string adminNotes, Exception exception)
        {
            Exception = exception;
            ProgramVersionCode = programVersionCode;
            RunningMode = runningMode;
            CurrentUser = currentUser;
            LoginTime = loginTime;
            ProcessStartTime = processStartTime;
            ProcessEndTime = processEndTime;
            IP = ip;
            ComputerName = computerName;
            FolderPath = folderPath;
            NetworkAdapter = networkAdapter;
            AdminNotes = adminNotes;
        }
        public Exception Exception { get; set; }
        /// <summary>
        /// Program Sürüm Kodu
        /// </summary>
        public string ProgramVersionCode { get; set; }
        /// <summary>
        /// Notlar
        /// </summary>
        public string AdminNotes { get; set; }
        /// <summary>
        /// Yönetici mi yoksa Teknisyen modunda mı çalıştığı 
        /// </summary>
        public string RunningMode { get; set; }
        /// <summary>
        /// Login olmuş kullanıcı
        /// </summary>
        public object CurrentUser { get; set; }
        /// <summary>
        /// Login olma zamanı
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// İşlemin Başlangıç Zamanı
        /// </summary>
        public DateTime ProcessStartTime { get; set; }
        /// <summary>
        /// İşlemin Bitiş Zamanı
        /// </summary>
        public DateTime ProcessEndTime { get; set; }
        public string ProcessElapsedTime { get { return string.Format("{0} ms.", (ProcessEndTime.Millisecond - ProcessStartTime.Millisecond)); } }
        /// <summary>
        /// Bilgisayarın IP'si
        /// </summary>
        public string[] IP { get; set; }
        /// <summary>
        /// Bilgisayar Adı
        /// </summary>
        public string ComputerName { get; set; }
        /// <summary>
        /// Uygulamanın bulunduğu klasör
        /// </summary>
        public string FolderPath { get; set; }
        /// <summary>
        /// Network Bilgileri
        /// </summary>
        public IList<string> NetworkAdapter { get; set; }

    }
}
