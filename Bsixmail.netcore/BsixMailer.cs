using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading;

namespace Bsixmail.netcore
{
    #region config
    class MailConfig
    {
        static IConfigurationRoot _root = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

        public static string UserName => _root.GetSection("BsixMail").GetSection("Username").Value;
        public static string Password => _root.GetSection("BsixMail").GetSection("Password").Value;
        public static string Server => _root.GetSection("BsixMail").GetSection("Server").Value;
        public static string Port => _root.GetSection("BsixMail").GetSection("Port").Value;
        public static string Folder => Path.Combine(Directory.GetCurrentDirectory(), "BsixMail");
        public static string DemoFile => "default.txt";
    }
    #endregion

    public class BsixMailer
    {
        #region Property
        private string Body { get; set; }
        private List<string> To { get; set; }
        private List<string> CC { get; set; }
        private List<string> Bcc { get; set; }
        private string Subject { get; set; }
        private List<Attachment> Attachment { get; set; }
        private MailPriority Priority { get; set; }

        #endregion

        #region Build Body
        public BsixMailer BuildBody(object yourObject, string yourTemplate)
        {
            string body = string.Empty;

            if (string.IsNullOrEmpty(yourTemplate)) yourTemplate = MailConfig.DemoFile;

            body = ReadFileFrom(yourTemplate);

            Type myType = yourObject.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            if (string.IsNullOrWhiteSpace(body))
            {
                body = "<html><head><title></title></head><body>";

                foreach (PropertyInfo prop in props)
                {
                    object propValue = prop.GetValue(yourObject, null);
                    object propName = prop.Name;

                    string propertyReplace = "{{" + propName.ToString() + "}}";

                    body += propName.ToString() + " : " + propertyReplace + "<hr/>";
                }
                body += "</body><html>";

                WriteTemplate(body, yourTemplate);
            }

            foreach (PropertyInfo prop in props)
            {
                object propValue = prop.GetValue(yourObject, null);
                object propName = prop.Name;

                string propertyReplace = "{{" + propName.ToString() + "}}";
                string propertyValueReplace = propValue == null ? "Unbind Model" : propValue.ToString();

                if (body.Contains(propertyReplace))
                {
                    body = body.Replace(propertyReplace, propertyValueReplace);
                }
            }

            this.Body = body;

            return this;
        }

        private string ReadFileFrom(string templateName)
        {
            bool folderExists = Directory.Exists(MailConfig.Folder);

            if (!folderExists) Directory.CreateDirectory(MailConfig.Folder);

            string filePath = MailConfig.Folder + "/" + templateName;

            if (!File.Exists(filePath))
            {
                (new FileInfo(filePath)).Directory.Create();
                using (TextWriter tw = new StreamWriter(filePath))
                {
                    tw.WriteLine(string.Empty);
                    tw.Close();
                }
            }

            string body = File.ReadAllText(filePath);

            return body;
        }

        private void WriteTemplate(string body, string templateName)
        {
            string filePath = MailConfig.Folder + "/" + templateName;

            string s = string.Empty;
            // Open the file to read from.
            using (StreamReader sr = File.OpenText(filePath))
            {
                while ((s = sr.ReadLine()) != null)
                {
                    s = s + s;
                }
            }

            if (!string.IsNullOrEmpty(body))
            {
                if (!File.Exists(filePath) || string.IsNullOrEmpty(s))
                {
                    // Create a file to write to.
                    using (StreamWriter tw = File.CreateText(filePath))
                    {
                        tw.WriteLine(body);
                        tw.Close();
                    }
                }
            }
        }
        #endregion

        #region To, CC, BCC, Subject, credentials


        public BsixMailer EmailTo(params string[] EmailTo)
        {
            this.To = EmailTo.ToList();
            return this;
        }

        public BsixMailer CCTo(params string[] CCTo)
        {
            this.CC = CCTo.ToList();
            return this;
        }

        public BsixMailer BCCTo(params string[] BCCTo)
        {
            this.Bcc = BCCTo.ToList();
            return this;
        }

        public BsixMailer EmailSubject(string Subject)
        {
            this.Subject = Subject;
            return this;
        }
        #endregion

        #region Attachment, Priority
        public BsixMailer Attacment(params Attachment[] attachment)
        {
            this.Attachment = attachment.ToList();
            return this;
        }

        public BsixMailer EmailPriority(MailPriority priority)
        {
            this.Priority = priority;
            return this;
        }
        #endregion

        #region sender
        public void SendBackground()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                MailMessage mail = new MailMessage();

                mail.Sender = new MailAddress(MailConfig.UserName);
                mail.From = new MailAddress(MailConfig.UserName);
                mail.Body = this.Body;
                mail.Subject = this.Subject;

                if (this.To != null)
                {
                    foreach (var item in this.To)
                    {
                        mail.To.Add(item);
                    }
                }

                if (this.CC != null)
                {
                    foreach (var item in this.CC)
                    {
                        mail.CC.Add(item);
                    }
                }

                if (this.Bcc != null)
                {
                    foreach (var item in this.Bcc)
                    {
                        mail.Bcc.Add(item);
                    }
                }

                if (this.Attachment != null)
                {
                    foreach (var item in this.Attachment)
                    {
                        mail.Attachments.Add(item);
                    }
                }

                mail.Priority = this.Priority;
                mail.IsBodyHtml = true;

                try
                {
                    using (var client = new SmtpClient(MailConfig.Server))
                    {
                        client.Port = Convert.ToInt32(MailConfig.Port);
                        client.Credentials = new NetworkCredential(MailConfig.UserName, MailConfig.Password);
                        client.EnableSsl = true;
                        client.Send(mail);
                    }
                }
                catch (Exception e)
                {
                    string path = MailConfig.Folder + "/" + "Error_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt";
                    if (!File.Exists(path))
                    {
                        (new FileInfo(path)).Directory.Create();
                        using (TextWriter tw = new StreamWriter(path))
                        {
                            tw.WriteLine(mail.Subject + "|" + mail.Body + "|" + e.ToString() + Environment.NewLine);
                            tw.Close();
                        }
                    }
                    else
                    {
                        using (StreamWriter tw = File.AppendText(path))
                        {
                            tw.WriteLine(mail.Subject + "|" + e.ToString() + Environment.NewLine);
                            tw.Close();
                        }
                    }
                }


            }).Start();
        }

        public int Send()
        {
            MailMessage mail = new MailMessage();

            mail.Sender = new MailAddress(MailConfig.UserName);
            mail.From = new MailAddress(MailConfig.UserName);
            mail.Body = this.Body;
            mail.Subject = this.Subject;

            if (this.To != null)
            {
                foreach (var item in this.To)
                {
                    mail.To.Add(item);
                }
            }

            if (this.CC != null)
            {
                foreach (var item in this.CC)
                {
                    mail.CC.Add(item);
                }
            }

            if (this.Bcc != null)
            {
                foreach (var item in this.Bcc)
                {
                    mail.Bcc.Add(item);
                }
            }

            if (this.Attachment != null)
            {
                foreach (var item in this.Attachment)
                {
                    mail.Attachments.Add(item);
                }
            }

            mail.Priority = this.Priority;
            mail.IsBodyHtml = true;

            try
            {
                using (var client = new SmtpClient(MailConfig.Server))
                {
                    client.Port = Convert.ToInt32(MailConfig.Port);
                    client.Credentials = new NetworkCredential(MailConfig.UserName, MailConfig.Password);
                    client.EnableSsl = true;
                    client.Send(mail);
                }
            }
            catch (Exception e)
            {
                string path = MailConfig.Folder + "/" + "Error_" + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt";
                if (!File.Exists(path))
                {
                    (new FileInfo(path)).Directory.Create();
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.WriteLine(mail.Subject + "|" + mail.Body + "|" + e.ToString() + Environment.NewLine);
                        tw.Close();
                    }
                }
                else
                {
                    using (StreamWriter tw = File.AppendText(path))
                    {
                        tw.WriteLine(mail.Subject + "|" + e.ToString() + Environment.NewLine);
                        tw.Close();
                    }
                }

                return 0;
            }

            return this.To.Count;
        }
        #endregion
    }
}
