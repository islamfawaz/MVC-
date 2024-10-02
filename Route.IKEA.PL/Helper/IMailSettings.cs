using Route.IKEA.PL.ViewModels.Identity;

namespace Route.IKEA.PL.Helper
{
    public interface IMailSettings
    {
        public void SendEmail(Email email);
    }
}
