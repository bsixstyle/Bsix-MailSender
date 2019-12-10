
Add this to your appsettings.json

  "BsixMail": {
    "UserName": "yourmail@domain.com", // your username
    "Password": "yourpassword", // your password
    "Server": "smtp.yourservermail.com", // your smtp server
    "Port": "587", // your port 
    "UrlBase":  "https://www.yourdomain.com" // your website domain (optional you can leave it empty)
  }

how to use?

  var _Base = _config.GetSection("BsixMail").GetSection("UrlBase").Value;
            var mailer = new BsixMailer();
            mailer.BuildBody(new
            {
                Message = "hello from bsix aproval",
                UrlApprove = string.Format("{0}/{1}/{2}", _Base, "Approve", "yes"),
                UrlReject = string.Format("{0}/{1}/{2}", _Base, "Approve", "no")
            }, "default.txt") // &lt;--- your template mail located inside BsixMail folder
                .EmailTo("mailto@example.com")
                .EmailSubject("This is subject")
                .Send();

            mailer.BuildBody(new
            {
                Message = "hello from bsix"
            }, "default.txt")  // &lt;--- your template mail located inside BsixMail folder
                .EmailTo("mailto@example.com")
                .EmailSubject("This is subject")
                .SendBackground(); // &lt;--- background task email

Need more details?
https://github.com/ihsanbsix/Bsix-MailSender/blob/master/Bsixmail.netcore/README.md