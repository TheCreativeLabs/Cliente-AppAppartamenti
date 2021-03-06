﻿using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AppAppartamentiApi.Providers
{
    public static class EmailService
    {
        public enum EmailType
        {
            Registration,
            Recovery
        }

        public static async Task SendAsync(string EmailTo, string Subject, string Body)
        {
            //var apiKey = ConfigurationManager.AppSettings["SendGridApiKey"];
            var apiKey = Properties.Settings.Default.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("AppAppartamenti@app.com", "AppAppartamenti");
            var subject = Subject;
            var to = new EmailAddress(EmailTo, "Example User");
            var plainTextContent = Body;
            var htmlContent = Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}