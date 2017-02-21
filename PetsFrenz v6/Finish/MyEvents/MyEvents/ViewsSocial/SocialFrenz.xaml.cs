using MyEvents.Models;
using MyEvents.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.ViewsSocial
{
    public partial class SocialFrenz : ContentPage
    {
        private string owneremail;
        private User user;
        private UsersManager usersManager;

        private User currentUser;

        private List<Friend> userList;
        private FriendManager frenzManager;
        public SocialFrenz(string owneremail)
        {
            InitializeComponent();
            this.owneremail = owneremail;

            txtSocialName.Text = owneremail;

            user = new User
            {
                email = owneremail
            };

            //  txtPetName.Text = user.email;
            usersManager = new UsersManager();

            userList = new List<Friend>();
            frenzManager = new FriendManager();

            userListView.ItemTemplate = new DataTemplate(typeof(RoutesCellFrienz));
            this.loadData();
        }

   

        private async void loadData()
        {
            user = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email);
            txtSocialPetName.Text = user.petname;

            userList = await frenzManager.ListUserWhere(userSelect => userSelect.userpetname == user.petname);
           // user = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email);
            if (userList.Count != 0)
            {
                userListView.ItemsSource = userList;

            }
            else
            {
                txtnorecord.Text = "No Friends Founded.";
            }
        }

        private async void SocialFrenzDetails_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new PetAlbum(txtSocialPetName.Text));
        }

        private async void SocialFrenzSuggest_Clicked(object sender, EventArgs e)
        {
            string owneremail = txtSocialName.Text;
            await Navigation.PushAsync(new SocialFrenzSuggest(owneremail));

        }
    }
}
