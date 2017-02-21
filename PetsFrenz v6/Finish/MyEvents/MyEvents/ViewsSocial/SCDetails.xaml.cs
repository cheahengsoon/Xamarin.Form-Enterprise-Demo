using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvents.Models;
using Xamarin.Forms;

namespace MyEvents.ViewsSocial
{
    public partial class SCDetails : ContentPage
    {
        private User user;
        private UsersManager usersManager;
        public SCDetails(User user)
        {
            InitializeComponent();
            this.user = user;

            user = new User()
            {
                petname = user.petname
            };
            txtName.Text = user.petname;

            usersManager = new UsersManager();
        }
    }
}
