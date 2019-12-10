# Mailer template and helper for asp.netcore 2.0 above


First step you need to add and edit this in your `appsettings.json`

```
  "BsixMail": {
    "UserName": "yourmail@domain.com",
    "Password": "yourpassword",
    "Server": "smtp.yourservermail.com",
    "Port": "587",
    "UrlBase": "https://www.yourdomain.com" // your website domain (optional you can leave it empty)
  }
```

Lets call the bsixmail inside your contoller or method
```
var mailer = new BsixMailer();
```
Now you can use it, see the examples below :

This is for send on the fly.
Variable `_Base` just optional if you want to configure approval, you need to configure your domain, so public can access your public url not your localhost
```
var _Base = _config.GetSection("BsixMail").GetSection("UrlBase").Value;           
mailer.BuildBody(new
{
  Message = "hello from bsix aproval",
  UrlApprove = string.Format("{0}/{1}/{2}", _Base, "Approve", "yes"),
  UrlReject = string.Format("{0}/{1}/{2}", _Base, "Approve", "no") 
}, "default.txt") // Your template mail located inside BsixMail folder
.EmailTo("mailto@example.com")
.EmailSubject("This is subject")
.Send();
```

Sending mail in background task
```
mailer.BuildBody(new
{
  Message = "hello from bsix" // You can bind this with your model, its just example
}, "default.txt")  // Your template mail located inside BsixMail folder
.EmailTo("mailto@example.com")
.EmailSubject("This is subject")
.SendBackground();
```
You can add or edit `default.txt` with your own template, and bind it to your model, example

```
public class Mymodel 
{
  public string Name {get; set;}
}
```
Then your `defaul.txt` or `mytemplate.html` which is you just created with your own inside BsixMail Folder example template
```
Hi {{Name}} i`m from Bsixmail
```
And see how to use it
```
var myModel =  new Mymodel() { Name = "Ihsan" };

mailer.BuildBody(myModel, "mytemplate.txt")  // Your template mail located inside BsixMail folder
.EmailTo("mailto@example.com")
.EmailSubject("This is subject")
.SendBackground();
```

you also can trace if email not sent in BsixMail folder

if you want to take a look the source `https://github.com/ihsanbsix/Bsix-MailSender`

thats it

need help? just go to github repo and create issue 
