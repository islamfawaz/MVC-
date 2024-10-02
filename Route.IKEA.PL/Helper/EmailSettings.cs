using Route.IKEA.PL.ViewModels.Identity;
using System.Net;
using System.Net.Mail;

namespace Route.IKEA.PL.Helper
{
	public static class EmailSettings
	{

		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient(host:"smtp.gmail.com",587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential(userName:"fawazislam70@gmail.com",password: "zdhqzddqaojxgdtp");
			Client.Send("fawazislam70@gmail.com", email.To, email.Subject, email.Body);
		}

	}
}
