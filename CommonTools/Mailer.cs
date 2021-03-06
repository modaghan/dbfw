using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace BLL.Helpers
{

    public static class Mailer
    {
        public static async Task SendAsync(string To, string Subject, string Body, bool isHtml = false)
        {
            await Task.Run(() =>
            {
                return SendMail(To, Subject, Body, isHtml);
            });
        }
        public static bool Send(string To, string Subject, string Body, bool isHtml = false)
        {
            return SendMail(To, Subject, Body, isHtml);
        }

        private static bool SendMail(string To, string Subject, string Body, bool isHtml)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Credentials.MailCredentials.Username);
                SmtpClient client = new SmtpClient();
                client.Port = Credentials.MailCredentials.Port;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = Credentials.MailCredentials.Host;
                client.Credentials = new NetworkCredential(Credentials.MailCredentials.Username, Credentials.MailCredentials.Password);
                mail.Subject = Subject;
                mail.IsBodyHtml = isHtml;
                mail.To.Add(To);
                mail.Body = Body;
                client.Send(mail);
                return true;
            }
            catch (Exception)
            {
                //Logger.Add(ex, Logger.LogPriority.Important, Logger.LogType.NetworkMistake, "E-Posta gönderilemedi.");
                return false;
            }
        }


    }
    public static class MailVerifier
    {
        static bool invalid = false;
        public static bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
