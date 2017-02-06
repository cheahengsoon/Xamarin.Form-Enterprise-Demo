using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class FriendList : ContentPage
    {
        private User currentUser;
        private List<User> userList;
        private UsersManager usersManager;

        User userResponse;
        public FriendList()
        {
            InitializeComponent();

            currentUser = (User)Application.Current.Properties["user"];

            userList = new List<User>();
            usersManager = new UsersManager();

            userListView.ItemTemplate = new DataTemplate(typeof(RoutesCell));
            // userListView.ItemTapped += userListView_ItemTapped;
            userListView.Refreshing += userListView_Refreshing;

            LoadUsers();
        }

        private async void LoadUsers()
        {
            userListView.IsRefreshing = true;
            // userList = await usersManager.ListUserWhere(userSelect => userSelect.ID != currentUser.ID);
            userList = await usersManager.ListUserWhere(userSelect => userSelect.petname != currentUser.petname);

            if (userList.Count != 0)
            {
                userListView.ItemsSource = userList;
            }
            userListView.IsRefreshing = false;
        }

        private void userListView_Refreshing(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void friendlistProfile_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = e.Item as User;

            await Navigation.PushAsync(new FriendsDetails(user));

        }

        private async void Notice_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationList());
        }


    }
}