using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WebUI.Data;

namespace WebUI.Services
{
    public class MailService : INotifyService
    {
        private readonly Helpers.MailSettings _mailSettings;

        public MailService(IOptions<Helpers.MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public void Send(TargetApp item, string destinationMailAddress)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress(destinationMailAddress, destinationMailAddress));
                    message.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);

                    message.Subject = "Notification";
                    message.Body = $"{item.Name} cannot be reached. URL: {item.Url}";
                    message.IsBodyHtml = _mailSettings.IsBodyHtml;

                    using (var client = new SmtpClient(_mailSettings.Host))
                    {
                        client.Port = _mailSettings.Port;
                        client.Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password);
                        client.EnableSsl = _mailSettings.EnableSsl;
                        client.Send(message);
                    }
                }
            }
            catch (Exception)
            {

            }


        }
    }
}
