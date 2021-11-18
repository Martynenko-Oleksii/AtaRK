using AtaRK.Models;
using AtaRK_Back.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK_Back.Services
{
    public class MailService : IMailService
    {
        private readonly string _fromEmail = "goodgames.testing@gmail.com";
        private readonly IWebHostEnvironment _env;
        private readonly IFileService _fileService;

        public MailService(IWebHostEnvironment env, IFileService fileService)
        {
            _env = env;
            _fileService = fileService;
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

                string body = GetTechAnswerBody(techMessage, techAnswer);
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
            const string templateName = "\\templates\\application.txt";
            string applicationTemplate = _fileService.ReadFile(templateName);

            StringBuilder finalApplication = new StringBuilder(applicationTemplate);
            finalApplication.Replace("{name}", $"{application.Name} {application.Surname}");
            finalApplication.Replace("{city}", application.City);
            finalApplication.Replace("{message}", application.Message);
            finalApplication.Replace("{phone}", application.ContactPhone);
            finalApplication.Replace("{email}", application.ContactEmail);

            return finalApplication.ToString();
        }

        private string GetTechAnswerBody(TechMessage techMessage, TechMessageAnswer techAnswer)
        {
            const string templateName = "\\templates\\answer.txt";
            string answerTemplate = _fileService.ReadFile(templateName);

            StringBuilder finalAnswer = new StringBuilder(answerTemplate);
            finalAnswer.Replace("{techMessageTitle}", techMessage.Title);
            finalAnswer.Replace("{techMessage}", techMessage.Message);
            finalAnswer.Replace("{deviceNumber}", techMessage.ClimateDevice.DeviceNumber.ToString());
            finalAnswer.Replace("{answer}", techAnswer.Answer);

            return finalAnswer.ToString();
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
