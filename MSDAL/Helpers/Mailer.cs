using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace MS.BLL.Helpers
{

    public static class Mailer
    {
        /// <summary>
        /// Mail sender
        /// </summary>
        /// <param name="To">Receiver. Put ',' for multiple receivers.</param>
        /// <param name="Subject">Subject of mail</param>
        /// <param name="Body">Body of mail</param>
        /// <param name="isHtml">Make it true to send your mail in HTML form.</param>
        /// <returns>Returns true if mail has been successfully sent.</returns>
        public static async Task<bool> SendAsync(string To, string Subject, string Body, bool isHtml = false)
        {
            return await Task.Run(() =>
            {
                return SendMail(To, Subject, Body, isHtml);
            });
        }
        /// <summary>
        /// Mail sender to default user
        /// </summary>
        /// <param name="Subject">Subject of mail</param>
        /// <param name="Body">Body of mail</param>
        /// <param name="isHtml">Make it true to send your mail in HTML form.</param>
        /// <returns>Returns true if mail has been successfully sent.</returns>
        public static async Task<bool> SendAsync(string Subject, string Body, bool isHtml = false)
        {
            return await Task.Run(() =>
            {
                return SendMail(Credentials.MailCredentials().DefaultAddress, Subject, Body, isHtml);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">Mail Server</param>
        /// <param name="username">Mail Username</param>
        /// <param name="password">Mail Password</param>
        /// <param name="port">Mail Port (465, 587 ..etc)</param>
        /// <param name="To">Receiver. Put ',' for multiple receivers.</param>
        /// <param name="Subject">Subject of mail</param>
        /// <param name="Body">Body of mail</param>
        /// <param name="result">Return 'BAŞARILI' for successful mailing.</param>
        /// <param name="isHtml">Make it true to send your mail in HTML form.</param>
        /// <returns></returns>

        public static bool TestSend(string host, string username, string password, int port, string To, string Subject, string Body, ref string result, bool isHtml = false)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(username, username);
                SmtpClient client = new SmtpClient();
                client.Port = port;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = host;
                client.Credentials = new NetworkCredential(username, password);
                mail.Subject = Subject;
                mail.IsBodyHtml = isHtml;
                foreach (string receiver in To.Split(','))
                    mail.To.Add(receiver.Trim());
                mail.Body = Body;
                client.Send(mail);
                result = "BAŞARILI";
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                //Logger.Add(ex, Logger.LogPriority.Important, Logger.LogType.NetworkMistake, "E-Posta gönderilemedi.");
                return false;
            }
        }
        /// <summary>
        /// Mail sender
        /// </summary>
        /// <param name="To">Receiver. Put ',' for multiple receivers.</param>
        /// <param name="Subject">Subject of mail</param>
        /// <param name="Body">Body of mail</param>
        /// <param name="isHtml">Make it true to send your mail in HTML form.</param>
        /// <returns>Returns true if mail has been successfully sent.</returns>
        public static bool Send(string To, string Subject, string Body, bool isHtml = false)
        {
            return SendMail(To, Subject, Body, isHtml);
        }


        /// <summary>
        /// Mail sender
        /// </summary>
        /// <param name="To">Receiver. Put ',' for multiple receivers.</param>
        /// <param name="Subject">Subject of mail</param>
        /// <param name="Body">Body of mail</param>
        /// <param name="Attachments">Attachments</param>
        /// <returns>Returns true if mail has been successfully sent.</returns>
        public static bool Send(string To, string Subject, string Body, List<Attachment> Attachments)
        {
            return SendMail(To, Subject, Body, true, Attachments);
        }

        /// <summary>
        /// Mail sender to default user
        /// </summary>
        /// <param name="Subject">Subject of mail</param>
        /// <param name="Body">Body of mail</param>
        /// <param name="isHtml">Make it true to send your mail in HTML form.</param>
        /// <returns>Returns true if mail has been successfully sent.</returns>
        public static bool Send(string Subject, string Body, bool isHtml = false)
        {
            return SendMail(Credentials.MailCredentials().DefaultAddress, Subject, Body, isHtml);
        }
        public static async Task<string> SendMail(MailMessage mail, MailCredentials credentials)
        {
            try
            {
                await Task.Run(() =>
                {
                    SmtpClient client = new SmtpClient();
                    client.Port = credentials.Port;
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Host = credentials.Host;
                    client.Credentials = new NetworkCredential(credentials.Username, credentials.Password);
                   client.Send(mail);
                });
                return "Başarılı";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private static bool SendMail(string To, string Subject, string Body, bool isHtml, List<Attachment> Attachments = null)
        {
            try
            {
                Console.WriteLine(Credentials.MailCredentials().Host);
                Console.WriteLine(Credentials.MailCredentials().Port);
                Console.WriteLine(Credentials.MailCredentials().Username);
                Console.WriteLine(Credentials.MailCredentials().Password);

                To = To == "" ? Credentials.MailCredentials().DefaultAddress : To;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Credentials.MailCredentials().Username, Credentials.SystemCredentials.AppName);//new MailAddress(Credentials.CustomerCredentials().ShortName);
                SmtpClient client = new SmtpClient();
                client.Port = Credentials.MailCredentials().Port;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = Credentials.MailCredentials().Host;
                client.Credentials = new NetworkCredential(Credentials.MailCredentials().Username, Credentials.MailCredentials().Password);
                mail.Subject = Subject;
                mail.IsBodyHtml = isHtml;
                foreach (string receiver in To.Split(','))
                    mail.To.Add(receiver.Trim());
                mail.Body = Body;
                if(Attachments != null)
                    foreach (Attachment attachment in Attachments)
                        mail.Attachments.Add(attachment);
                client.Send(mail);
                return true;
            }
            catch (Exception ex)
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
