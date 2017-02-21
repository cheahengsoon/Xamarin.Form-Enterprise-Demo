using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvents.Models;
using Xamarin.Forms;
using MyEvents.Views;

namespace MyEvents.ViewsSocial
{
    public partial class SocialFrenzDetails : ContentPage
    {
        private User user;
        private User currentUser;
        private UsersManager usersManager;
        

        private Friend frenz;
        private FriendManager frenzManager;
        private string owneremail;

        //public SocialFrenzDetails(User user)
        //{
        //    InitializeComponent();
        //    this.user = user;
        //    user = new User
        //    {
        //        //email = user.email,
        //        petname = user.petname


        //    };

        //    frenzManager = new FriendManager();
        //    usersManager = new UsersManager();

         
        //        this.LoadData();
         
        //}

        public SocialFrenzDetails(User user, string owneremail) 
        {
            InitializeComponent();
            
            this.user = user;
            user = new User
            {
                //email = user.email,
                petname = user.petname

            };

            Title = user.petname + " Details ";
           

            frenzManager = new FriendManager();
            usersManager = new UsersManager();

            this.LoadData();

            this.owneremail = owneremail;
            currentUser = new User()
            {
                email=owneremail
            };
            this.LoadCurrentUserData();
           // txtcurrentuser.Text = owneremail;
        }

        private async void LoadCurrentUserData()
        {
            currentUser = await usersManager.GetUserWhere(userSelect => userSelect.email == currentUser.email);
            txtcurrentuserPetName.Text = currentUser.petname;
        }

        private async void LoadData()
        {
            try
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
                if (!string.IsNullOrEmpty(user.petimage))
                {
                    img.Source = ImageSource.FromUri(new Uri(user.petimage));
                }
                else
                {
                    img.Source = "person.png";
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        //-----------------------------------------------------------------------------
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
                    petimage = user.petimage
                };
                await AddFrenz(frenz);

                //  CrossBadge.Current.SetBadge(1);
                await DisplayAlert("Success", "Friend Request Sent!", "Cancel");

            }
        }



    }
}
