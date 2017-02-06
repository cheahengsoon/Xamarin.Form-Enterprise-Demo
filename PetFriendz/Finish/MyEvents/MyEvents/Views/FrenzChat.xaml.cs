using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class FrenzChat : ContentPage
    {
        private Friend user;


        private UsersManager usersManager;
        public FrenzChat(Friend user)
        {
            InitializeComponent();
            this.user = user;

            txtownerName.Text = user.friendpetname;

            this.loadData();
        }

        async void loadData()
        {
            try
            {
                string ownerpetname = txtownerName.Text;

                var user = new User { petname = ownerpetname };

                if (!string.IsNullOrEmpty(ownerpetname))
                {
                    User userResponse = await usersManager.GetUserWhere(userSelect => userSelect.petname == user.petname);

                    if (userResponse != null && userResponse.petname.Equals(ownerpetname, StringComparison.Ordinal))
                    {
                        txtRealOwnerName.Text = user.ownername;
                    }
                    else
                    {
                        await DisplayAlert("Incorrect", "Incorrent Data.", "Close");
                    }
                }
            }
            catch (Exception ex)
            {
                txtRealOwnerName.Text = ex.ToString();
                await DisplayAlert("Incorrect", ex.ToString(), "Close");
            }
        }
    }
}
