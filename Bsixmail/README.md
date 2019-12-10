# Mailer template and helper for asp.net webform or mvc .net framework 4.0 or above


package dotnet cli (visual studio code)
```
dotnet add package Bsixmail
```

For visual studio user corner top, `CTRL+Q` type `package console` 
```
Install-Package Bsixmail
```


First step you need to add and edit this in your `web.config` 

```
  <system.net>
      <mailSettings>
          <smtp from="yourmail@yourmail.com">
              <network enableSsl="true" host="smtp.gmail.com" port="587" userName="yourmail@yourmail.com" password="yourpassword"/>
      </smtp>
    </mailSettings>
  </system.net>
```

Lets call the bsixmail inside your contoller or method
```
var mailer = new BsixMailer();
```
Now you can use it, see the examples below :

This is for send on the fly.
Variable `_Base` just optional if you want to configure approval, you need to configure your domain, so public can access your public url not your localhost
```
 var _Base = "https://yourdomain.com/controller/action";         
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

You also can trace if email not sent in BsixMail folder

If you want to take a look the source `https://github.com/ihsanbsix/Bsix-MailSender`

Thats it

Need help? just go to github repo and create issue 
