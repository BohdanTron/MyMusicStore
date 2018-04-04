using Domain.Abstract;
using Domain.Entities;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Domain.Concrete
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings _emailSettings;

        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using(var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _emailSettings.UseSsl;
                smtpClient.Port = _emailSettings.ServerPort;
                smtpClient.Host = _emailSettings.ServerName;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials
                    = new NetworkCredential(_emailSettings.MailToAddress, _emailSettings.Password);

                StringBuilder body = new StringBuilder()
                    .AppendLine("New order processed")
                    .AppendLine("---")
                    .AppendLine("Products:");

                foreach (var line in cart.CartLines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (total: {2:c})",
                        line.Quantity, line.Product.Name, subtotal);
                    body.AppendLine();
                }

                body.AppendFormat("Total sum: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Details of user:")
                    .AppendLine(shippingDetails.FirstName)
                    .AppendLine(shippingDetails.LastName ?? "")
                    .AppendLine(shippingDetails.Phone)
                    .AppendLine(shippingDetails.Email)
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Email)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine("---")
                    .AppendFormat("Gift box: {0}",
                        shippingDetails.GiftWrap ? "Yes" : "No");


                MailMessage mailMessage = new MailMessage(
                       _emailSettings.MailFromAddress,      // from
                       _emailSettings.MailToAddress,        // to
                       "New Order!",                        // theme
                       body.ToString()); 				    // body


                smtpClient.Send(mailMessage);
            }
        }
    }

    public class EmailSettings
    {
        public string MailToAddress = "tronbodya@gmail.com";
        public string MailFromAddress = "mymusicstore@example.com";
        public bool UseSsl = true;
        public string Password = "Bohdan301127";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
    }
}
