using Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Demo.Views
{
    public partial class ForgetPassword : ContentPage
    {
        private User currentUser;
        private UsersManager usersManager;
        public ForgetPassword()
        {
            InitializeComponent();


            usersManager = new UsersManager();
        }
        async void Reset_Clicked(object sender, EventArgs e)
        {

            string email = this.txtEmail.Text;
            var user = new User { email = email };

            User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email);

            if(userResponse!=null&&userResponse.email.Equals(email,StringComparison.Ordinal))
            {
                if(currentUser!=null)
                {

                    if(!string.IsNullOrEmpty(currentUser.ownername))
                    {
                        txtOwner.Text = currentUser.ownername + "";
                    }
                    if(!string.IsNullOrEmpty(currentUser.password))
                    {
                        txtPassword.Text = currentUser.password + "";
                    }
                }
            }
        }
    }
}
