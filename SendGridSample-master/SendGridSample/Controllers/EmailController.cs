using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using SendGrid;

namespace SendGridSample.Controllers
{
    public class EmailController : ApiController
    {
        [HttpGet]
        [Route("api/SendMail")]
        public string SendMail(string email, string content)
        {

            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress("cheahengsoon@live.com.my");

            // Add multiple addresses to the To field.
            List<String> recipients = new List<String> { email };

            myMessage.AddTo(recipients);

            myMessage.Subject = "Testing the SendGrid Library";

            myMessage.Text = content;

            //Add Text bodies
           // myMessage.Text = "Hello World from " + Environment.MachineName;

            // Create a Web transport, using API Key
            NetworkCredential nc = new NetworkCredential("azure_e0baa2ffbc46c2b71b833e6bc5f51931@azure.com", "CESoon1012");
            var transportWeb = new Web("This string is a SendGrid API key", nc, TimeSpan.FromSeconds(15));

            // Send the email.
            transportWeb.DeliverAsync(myMessage);

            return "done";
        }
    }
}