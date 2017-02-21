using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class FriendsDetails : ContentPage
    {
        //private string email;
        private User user;
        private UsersManager usersManager;
        private User currentUser;

        private Friend frenz;
        private FriendManager frenzManager;
        public FriendsDetails(User user)
        {
            InitializeComponent();

            currentUser = (User)Application.Current.Properties["user"];

            // txtCurrentUser.Text = currentUser.email;
            this.user = user;

            // user.email = txtdemo.Text;

            user = new User
            {
                //email = user.email,
                petname = user.petname


            };

            usersManager = new UsersManager();
            frenzManager = new FriendManager();

            this.LoadData();
        }

        private async void LoadData()
        {
            //user = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email);
            user = await usersManager.GetUserWhere(userSelect => userSelect.petname == user.petname);
            // txtemail.Text = user.email;
            txtPetName.Text = user.petname;
            txtPetAge.Text = user.petage;
            txtPetGender.Text = user.petgender;
            txtPetType.Text = user.pettype;
            txtOwnerName.Text = user.ownername;
            txtNoLike.Text = user.nolike;
            if (!string.IsNullOrEmpty(currentUser.petimage))
            {
                img.Source = ImageSource.FromUri(new Uri(user.petimage));
            }
            else
            {
                img.Source = "person.png";
            }
            
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    LoadData();
        //}

        private async void ReportDetails_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new ReportAbuse(txtOwnerName.Text));
        }

        private async void btnViewPhoto_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new PetAlbum(txtPetName.Text));
        }


        async Task AddFrenz(Friend frenz)
        {
            Friend userResponse = await frenzManager.SaveGetUserAsync(frenz);
            // Application.Current.Properties["user"] = userResponse;
        }
        async void OnAddFriend_Clicked(object sender, EventArgs e)
        {
            string currentuserowner = currentUser.petname.ToString();
            string frenzdetailsowner = user.petname.ToString();
            string frenzstatus = "request";
            if (!string.IsNullOrEmpty(currentuserowner))
            {
                var frenz = new Friend
                {
                    userpetname = currentuserowner,
                    friendpetname = frenzdetailsowner,
                    status = frenzstatus,
                   petimage= user.petimage
                };
                await AddFrenz(frenz);
               
              //  CrossBadge.Current.SetBadge(1);
                await DisplayAlert("Success", "Friend Request Sent!", "Cancel");

            }
        }



    }
}