# Mailer template and helper for asp.netcore 2.0 above


first step you need add and edit inside your appsettings.json

```
  "BsixMail": {
    "UserName": "yourmail@domain.com",
    "Password": "yourpassword",
    "Server": "smtp.yourservermail.com",
    "Port": "587",
    "UrlBase": "https://www.yourdomain.com" // your website domain (optional you can leave it empty)
  }
```

how to use it?

lets call the bsix inside your contoller or method
```
var mailer = new BsixMailer();
```
now you can use it, see the examples below

send on the fly variable `_Base` just optional if you want configure with approval, yes you need to configure your domain users can access ur public url not your localhost
```
            var _Base = _config.GetSection("BsixMail").GetSection("UrlBase").Value;
           
            mailer.BuildBody(new
            {
                Message = "hello from bsix aproval",
                UrlApprove = string.Format("{0}/{1}/{2}", _Base, "Approve", "yes"),
                UrlReject = string.Format("{0}/{1}/{2}", _Base, "Approve", "no") // your can bind this with your model, its just example
            }, "default.txt") // <--- your template mail located inside BsixMail folder
                .EmailTo("mailto@example.com")
                .EmailSubject("This is subject")
                .Send();
```

sending mail in background task
```
            // example
            mailer.BuildBody(new
            {
                Message = "hello from bsix" // your can bind this with your model, its just example
            }, "default.txt")  // <--- your template mail located inside BsixMail folder
                .EmailTo("mailto@example.com")
                .EmailSubject("This is subject")
                .SendBackground(); // <--- background task email
```
you can add or modify `default.txt` with your own template, and bind it to your model, example

```
public class Mymodel 
{
  public string Name {get; set;}
}
```
then your `defaul.txt` or `mytemplate.html` which is you just created with your own inside BsixMail Folder example template
```
Hi {{Name}} i`m from Bsixmail
```
you also can trace if email not sent in BsixMail folder

thats it

need help? just go to github repo and create issue https://github.com/ihsanbsix/Bsix-MailSender
