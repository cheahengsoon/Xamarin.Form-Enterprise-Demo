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
        private Friend friend;
        private User user;

        private FriendManager frenzManager;
        private UsersManager userManager;

        private List<PetPhoto> userList;
        private PetPhotoManager petphotoUserManager;

        public FrenzChat(Friend friend)
        {
            InitializeComponent();

           Title = friend.friendpetname + " Photos Album";

            txtownerName.Text = friend.friendpetname;

            userList = new List<PetPhoto>();

            petphotoUserManager = new PetPhotoManager();
            
           // friend=(Friend)Application.Current.Properties["user"];

            userListView.ItemTemplate = new DataTemplate(typeof(ImageRouteList));
            userListView.Refreshing += UserListView_Refreshing;
            
            loadData();
        }

        private void UserListView_Refreshing(object sender, EventArgs e)
        {
            loadData();
        }

        public async void loadData()
        {
            try
            {
                userList = await petphotoUserManager.ListUserWhere(userSelect => userSelect.petname == txtownerName.Text);
                if (userList.Count != 0)
                {
                    userListView.ItemsSource = userList;
                    
                }
                else
                {
                    txtnorecord.Text = "No records";
                }
            }catch(Exception ex)
            {
                txtnorecord.Text = ex.Message;
            }
          

        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    loadData();
        //}
    }
}