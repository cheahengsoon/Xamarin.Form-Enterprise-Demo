using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.Email;

using Xamarin.Forms;

namespace Demo.TestFolder
{
    public partial class FF2 : ContentPage
    {
        private User currentUser;
        UsersManager manager;
        public FF2()
        {
            InitializeComponent();

            manager = new UsersManager();
            currentUser = (User)Application.Current.Properties["user"];

            loadData();
        }

        void loadData()
        {
           if(currentUser!=null)
            {
                if (!string.IsNullOrEmpty(currentUser.email))
                    txtEmail.Text = currentUser.email + "";
                if (!string.IsNullOrEmpty(currentUser.password))
                    txtPassword.Text = currentUser.password + "";
                if (!string.IsNullOrEmpty(currentUser.ownername))
                    txtOwer.Text = currentUser.ownername + "";

                SendEmail();
            }
        }

        private void SendEmail()
        {
            //string strMailTo = @"mailto:jeff.wei-lim@artesiansolutions.com?Subject=test&Body=testBody";
            // Device.OpenUri(new Uri(strMailTo));
            // Mail.SendMail("admin@abc.com", toEmail, "New Password", mailBody);

            Device.OpenUri(new Uri("mailto:ryan.hatfield@test.com"));
        }
    }
}
