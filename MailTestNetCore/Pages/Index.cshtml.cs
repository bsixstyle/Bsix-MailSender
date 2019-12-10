using Bsixmail.netcore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace MailTestNetCore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;

        public IndexModel(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void OnGet()
        {
            // example with approval
            var _Base = _config.GetSection("BsixMail").GetSection("UrlBase").Value;
            var mailer = new BsixMailer();
            mailer.BuildBody(new
            {
                Message = "hello from bsix aproval",
                UrlApprove = string.Format("{0}/{1}/{2}", _Base, "Approve", "yes"),
                UrlReject = string.Format("{0}/{1}/{2}", _Base, "Approve", "no")
            }, "default.txt") // <--- your template mail located inside BsixMail folder
                .EmailTo("mailto@example.com")
                .EmailSubject("This is subject")
                .Send();

            // example
            mailer.BuildBody(new
            {
                Message = "hello from bsix"
            }, "default.txt")  // <--- your template mail located inside BsixMail folder
                .EmailTo("mailto@example.com")
                .EmailSubject("This is subject")
                .SendBackground(); // <--- background task email
        }
    }
}
