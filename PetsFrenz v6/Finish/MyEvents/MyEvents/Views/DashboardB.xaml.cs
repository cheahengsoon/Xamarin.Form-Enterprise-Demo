using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class DashboardB : ContentPage
    {
        private User currentUser;
      
        private UsersManager usersManager;

        private List<PetPhoto> userList;
        private PetPhotoManager petphotoUserManager;
        public DashboardB()
        {
            InitializeComponent();

            currentUser = (User)Application.Current.Properties["user"];

            userList = new List<PetPhoto>();
           
            petphotoUserManager = new PetPhotoManager();

            userListView.ItemTemplate = new DataTemplate(typeof(ImageRouteList));
            userListView.Refreshing += UserListView_Refreshing;
            userListView.ItemTapped += userListView_ItemTapped;
            loadData();
        }

        private async void userListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = e.Item as PetPhoto;
            var nolikeuser = new PetPhoto
            {
                ID = user.ID,
                nolike = (Convert.ToInt32(user.nolike) + 1).ToString()
            };
            await UpdateUser(nolikeuser);
         
        
        }

        private async Task UpdateUser(PetPhoto nolikeuser)
        {
            PetPhoto userResponse = await petphotoUserManager.SaveGetUserAsync(nolikeuser);
        }

   

        private void UserListView_Refreshing(object sender, EventArgs e)
        {
            loadData();
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    loadData();
        //}

        public async void loadData()
        {
            try
            {
                userList = await petphotoUserManager.ListUserWhere(userSelect => userSelect.petname != currentUser.petname && userSelect.datecreated <= DateTime.Now);
           
                if (userList.Count != 0 )
                {
                   // userList.OrderByDescending(x => x.ID);
                    userListView.ItemsSource = userList.OrderByDescending(x => x.datecreated);
                   // userList.Sort();

                }
                else
                {
                    txtnorecord.Text = "No records";
                }
            }
            catch (Exception ex)
            {
                txtnorecord.Text = ex.Message;
            }


        }
        private async void ProfileDetails_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Profile());
        }

        private void LogOut_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new LoginMain());
        }

        private async void Settings_Clicked()
        {
            //Temporary
            await Navigation.PushAsync(new Profile());
        }

        private async void Friends_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new FriendList());
            await Navigation.PushAsync(new NotificationList());
        }

        private async void Notice_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificationList());
        }

        private async void Album_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ImageList());
        }
    }
}