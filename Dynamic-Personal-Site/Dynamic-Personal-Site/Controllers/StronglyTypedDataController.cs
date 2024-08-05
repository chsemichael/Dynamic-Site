using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Options; //Grants access to IOptions<T>
using CORE1.Models; //Added for access to ContactViewModel
using MimeKit;
using MailKit.Net.Smtp;

namespace CORE1.Controllers
{
    public class StronglyTypedDataController : Controller
    {
        //We won't be using an Index Action/View for this controller,
        //so we can simply comment out/delete the one that is provided

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //Before creating any Actions or Views related to this Controller,
        //we will add a Credentials section to the appsettings.json file.
        //This lets us store the (sensitive) login information for our 
        //email account in that file so it does not have to be written here.

        //If you are using source control, you can then add the following line
        //to your .gitignore file to prevent the appsettings.json from being
        //pushed to the remote repo:

        // */appsettings.json


        //Create a field to store the CredentialSettings info
        private readonly CredentialSettings _credentialSettings;


        //Add a constructor for our Controller that accepts the necessary info as parameters
        public StronglyTypedDataController(IOptions<CredentialSettings> settings) 
        {
            //Since settings is of type IOptions<CredentialSettings>, we will need to 
            //access the Value property to get the info we want from CredentialSettings
            _credentialSettings = settings.Value;
        }

        //Controller Actions are meant to handle certain types of requests. The most 
        //common request is GET, which is used to request info to load a page. We will
        //also create actions to handle POST requests, which are used to send info to the app.

        //GET is the default request type to be handled, so no extra info is needed here.
        public IActionResult Contact()
        {
            ViewBag.contactRequestTest = "GET REQUEST";
            return View();

            //We want the info from our Contact form to use the ContactViewModel we created.
            //To do this, we can generate the necessary code using the following steps:

            #region Code Generation Steps

            //1. Go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution
            //2. Go to the Browse tab and search for Microsoft.VisualStudio.Web
            //3. Click Microsoft.VisualStudio.Web.CodeGeneration.Design
            //4. On the right, check the box next to the CORE1 project, then click "Install"
            //5. Once installed, return here and right click the Contact action
            //6. Select Add View, then select the Razor View template and click "Add"
            //7. Enter the following settings:
            //      - View Name: Contact
            //      - Template: Create
            //      - Model Class: ContactViewModel
            //8. Leave all other settings as-is and click "Add"

            #endregion
        }

        //Now we need to handle what to do when the user submits the form. For this,
        //we will make another Contact action, this time intended to handle the POST request.
        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            ViewBag.contactRequestTest = "POST REQUEST";

            if(!ModelState.IsValid)
            {
                return View(cvm);
            }

            //Create the format for the message content we will receive from the contact form
            string message = 
                $"You have received a new email from your site's contact form!<br/>" +
                $"Sender: {cvm.Name}<br/>" + 
                $"Email: {cvm.Email}<br/>" + 
                $"Subject: {cvm.Subject}<br/>" +
                $"Message: {cvm.Message}";

            //Create a MimeMessage object to assist with storing/transporting the email
            //information from the contact form.
            MimeMessage mm = new MimeMessage();

            //Even though the user is the one attempting to send a message to us, the actual sender 
            //of the email is the email user we set up with our hosting provider.

            //We can access the credentials for this email user from our appsettings.json file as shown below.
            mm.From.Add(new MailboxAddress("Sender", _credentialSettings.Email.Username));

            //The recipient of this email will be our personal email address, also stored in appsettings.json.
            mm.To.Add(new MailboxAddress("Personal", _credentialSettings.Email.Recipient));

            //The subject will be the one provided by the user, which we stored in our cvm object.
            mm.Subject = cvm.Subject;

            //The body of the message will be formatted with the string we created above.
            mm.Body = new TextPart("HTML") { Text = message };

            //We can set the priority of the message as "urgent" so it will be flagged in our email client.
            mm.Priority = MessagePriority.Urgent;

            //We can also add the user's provided email address to the list of ReplyTo addresses
            //so our replies can be sent directly to them instead of the email user on our hosting provider.
            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));


            using(SmtpClient client = new SmtpClient())
            {
                //It's possible the mail server may be down when the user attempts to contact us, 
                //so we can "encapsulate" our code to send the message in a try/catch.
                try
                {
                    //Connect to the mail server using credentials in our appsettings.json & port 8889
                    client.Connect(_credentialSettings.Email.Server, 8889);

                    //Log in to the mail server using the credentials for our email user.
                    client.Authenticate(_credentialSettings.Email.Username, _credentialSettings.Email.Password);
                
                    //Try to send the email
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    //If there is an issue, we can store an error message in a ViewBag variable 
                    //to be displayed in the View
                    ViewBag.EmailErrorMessage = $"There was an error when processing your email request." + $"Error Message:  {ex.Message}"
                      + $"Error StackTrace: {ex.StackTrace}";

                    // Return the user back to the contact page with the form intact
                    return View(cvm);
                    
                }
            }


            return View("EmailConfirmation", cvm);
        }
    }
}
