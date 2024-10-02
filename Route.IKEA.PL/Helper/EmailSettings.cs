using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Route.IKEA.PL.ViewModels.Identity;

namespace Route.IKEA.PL.Helper
{
	public  class EmailSettings :IMailSettings
	{
		private readonly MailSettings _options;
        public EmailSettings(IOptions<MailSettings> options)
        {
			_options = options.Value;
        }


        public  void SendEmail(Email email)
		{
			var mail = new MimeMessage()
			{
                Sender =MailboxAddress.Parse(_options.Email),
				Subject =email.Subject
			};
			mail.To.Add(MailboxAddress.Parse(email.To));
			mail.From.Add(MailboxAddress.Parse(_options.Email));

			var builder = new BodyBuilder();
			builder.TextBody=email.Body;
			mail.Body=builder.ToMessageBody();

			using var smtp = new SmtpClient();
			smtp.Connect(_options.Host,_options.Port,SecureSocketOptions.StartTls);
			smtp.Authenticate(_options.Email, _options.Password);
			smtp.Send(mail);
			smtp.Disconnect(true);
		}
	}
}
