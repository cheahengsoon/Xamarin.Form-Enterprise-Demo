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
    public partial class SocialFrenzSuggest : ContentPage
    {
        private string owneremail;

        private User user;
        private List<User> userList;
        private UsersManager usersManager;

       //private FriendManager frenzManager;


        public SocialFrenzSuggest(string owneremail)
        {
            InitializeComponent();
            this.owneremail = owneremail;

            user = new User
            {
                email = owneremail

            };

            //  txtPetName.Text = user.email;
            usersManager = new UsersManager();


            userList = new List<User>();
            usersManager = new UsersManager();

            userListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));
            this.loadData();
        }

        private async void loadData()
        {
            userList = await usersManager.ListUserWhere(userSelect => userSelect.petname != user.petname);
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
            var user = e.Item as User;

            await Navigation.PushAsync(new SocialFrenzDetails(user,owneremail));
        }
    }
}
