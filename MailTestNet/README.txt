
Change value mail setting in webconfig
Example

  <system.net>
      <mailSettings>
          <smtp from="yourmail@yourmail.com">
              <network enableSsl="true" host="smtp.gmail.com" port="587" userName="yourmail@yourmail.com" password="yourpassword"/>
      </smtp>
    </mailSettings>
  </system.net>

how to use?

    var _Base = "https://yourdomain.com/controller/action"; // Your domain and action to approve this is optional just example if you need approval process for your transactions
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