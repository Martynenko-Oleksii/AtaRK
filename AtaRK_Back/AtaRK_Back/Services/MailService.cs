using AtaRK.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AtaRK_Back.Services
{
    public class MailService : IMailService
    {
        private readonly string _fromEmail = "goodgames.testing@gmail.com";
        private readonly IWebHostEnvironment _env;

        public MailService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public object SendApplication(ShopApplication application)
        {
            try
            {
                FranchiseContactInfo emailInfo = application.FastFoodFranchise.
                    FranchiseContactInfos.Find(x => x.IsEmail);

                string toEmail = null;
                if (emailInfo != null)
                {
                    toEmail = emailInfo.Value;
                }
                else
                {
                    toEmail = application.FastFoodFranchise.Email;
                }

                string body = GetApplicationBody(application);
                if (body == null)
                {
                    return "Body Is Empty";
                }

                const string fromName = "AtarkBot";
                MailMessage message = GetMessage(toEmail, body, fromName);
                SendEmail(message);

                return true;
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{ex.InnerException}";
            }
        }

        public object SendTechAnswer(TechMessage techMessage)
        {
            try
            {
                TechMessageAnswer techAnswer = techMessage.TechMessageAnswer;
                SystemAdmin admin = techAnswer.SystemAdmin;

                string toEmail = techMessage.ContactEmail;

                string body = GetTechAnswerBody(techAnswer);
                if (body == null)
                {
                    return "Body Is Empty";
                }

                string fromName = admin.Name;
                MailMessage message = GetMessage(toEmail, body, fromName);
                SendEmail(message);

                return true;
            }
            catch (Exception ex)
            {
                return $"{ex.Message}\n{ex.InnerException}";
            }
        }

        private string GetApplicationBody(ShopApplication application)
        {
            const string templateName = "application.txt";
            // TODO: UseTemplate
            return null;
        }

        private string GetTechAnswerBody(TechMessageAnswer techAnswer)
        {
            const string templateName = "answer.txt";
            // TODO: UseTemplate
            return null;
        }

        private MailMessage GetMessage(string toEmail, string body, string fromName)
        {
            MailAddress from = new MailAddress(_fromEmail, fromName);
            MailAddress to = new MailAddress(toEmail);
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Питання щодо франшизи";
            message.Body = body;
            message.IsBodyHtml = true;

            return message;
        }

        private void SendEmail(MailMessage message)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(_fromEmail, "frick4224");
            smtp.EnableSsl = true;
            smtp.Send(message);
        }
    }
}
