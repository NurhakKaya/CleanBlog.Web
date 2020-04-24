using CleanBlog.Core.Controllers;
using CleanBlog.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Logging;

namespace CleanBlog.Core.Services
{
    public class SmtpService : ISmtpService
    {
        // DI for ILogger, which is already registered by default in Umbraco v8
        private ILogger _logger;
        public SmtpService(ILogger logger)
        {
            _logger = logger;
        }

        public bool SendEmail(ContactViewModel model)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();

                string toAddress = "to@test.com"; // This can be moved to web.config etc.
                string fromAddress = "from@test.com";
                message.Subject = $"Enquiry from: {model.Name} - {model.Email}";
                message.Body = model.Message;
                message.To.Add(new MailAddress(toAddress, toAddress));
                message.From = new MailAddress(fromAddress, fromAddress);

                // Note: To send e-mails for dev tests, smtp4dev can be used - this package has been installed via chocolatey, might need some configuration 
                client.Send(message);
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.Error(typeof(ContactSurfaceController), "Error sending contact form.", ex);
                return false;
            }
        }
    }
}
