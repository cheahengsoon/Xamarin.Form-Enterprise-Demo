using MyEvents.Models;
using MyEvents.Services;
using MyEvents.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.ViewsSocial
{
    public partial class SocialProfile : ContentPage
    {
     
        private string owneremail;
        private User user;
        private UsersManager usersManager;

        private MediaFile _mediaFile;
        public SocialProfile(string owneremail)
        {
            InitializeComponent();

            txtRemoteFilePath.IsVisible = false;
            txtfilePath.IsVisible = false;


            this.owneremail = owneremail;
          txtSocialName.Text = owneremail;

            user = new User
            {
                email = owneremail
                
            };

            //  txtPetName.Text = user.email;
            usersManager = new UsersManager();
            this.loadData();


            PickerGenderCategory.SelectedIndexChanged += (sender, args) =>
            {
                PickerGenderCategory.BackgroundColor = Color.FromHex("#009688");
            };

            PickerGenderCategory.SelectedIndexChanged += (sender, args) =>
            {
                PickerTypeCategory.BackgroundColor = Color.FromHex("#009688");
            };

            takePhoto.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    SaveToAlbum = true,
                    Directory = "Sample",
                    Name = Guid.NewGuid() + ".jpg"
                });

                if (_mediaFile == null)
                    return;

                txtfilePath.Text = _mediaFile.Path;

                img.Source = ImageSource.FromStream(() =>
                {
                    return _mediaFile.GetStream();
                });

                txtRemoteFilePath.Text = "https://petfrenzblob.blob.core.windows.net/petuser/" + _mediaFile.Path;

                await BlobMan.Instance.UploadFileAsync(_mediaFile.Path);

            };

            pickPhoto.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("No PickPhoto", ":( No PickPhoto available.", "OK");
                    return;
                }

                _mediaFile = await CrossMedia.Current.PickPhotoAsync();

                if (_mediaFile == null)
                    return;

                txtfilePath.Text = _mediaFile.Path;

                img.Source = ImageSource.FromStream(() =>
                {
                    return _mediaFile.GetStream();
                });

                txtRemoteFilePath.Text = "https://petfrenzblob.blob.core.windows.net/petuser/" + _mediaFile.Path;

                await BlobMan.Instance.UploadFileAsync(_mediaFile.Path);
            };
        }

        private async void loadData()
        {

            string[] genders = { "Male", "Female" };
            string[] types = { "Bird", "Cat", "Dog", "Fish", "Rabbit", "Small Pet", "Others" };

            user = await usersManager.GetUserWhere(userSelect => userSelect.email==user.email);
            //  txtPetName.Text = user.petname; //can display
            if(!string.IsNullOrEmpty(user.ID))
            {
                txtID.Text = user.ID;
            }

            if (!string.IsNullOrEmpty(user.ownername))
                nameEntry.Text = user.ownername;
            if (!string.IsNullOrEmpty(user.petname))
                petNameEntry.Text = user.petname;
            if (user.petage != null)
            {
                petAgeEntry.Text = user.petage + "";
            }
            if (!string.IsNullOrEmpty(user.petgender))
            {
                PickerGenderCategory.SelectedIndex = Array.IndexOf(genders, user.petgender);

            }

            if (!string.IsNullOrEmpty(user.pettype))
            {
                PickerTypeCategory.SelectedIndex = Array.IndexOf(types, user.pettype);
            }

            if (!string.IsNullOrEmpty(user.petimage))
            {
                img.Source = ImageSource.FromUri(new Uri(user.petimage));
            }
            else
            {
                img.Source = "person.png";
            }
        }

        async Task UpdateUser(User user)
        {
            User userResponse = await usersManager.SaveGetUserAsync(user);
            Application.Current.Properties["user"] = userResponse;
        }

        async void Dashboard_Clicked(object sender, EventArgs e)
        {
            string ownername = this.nameEntry.Text;
            string petname = this.petNameEntry.Text;
            string petage = this.petAgeEntry.Text;
            string genderSelected = PickerGenderCategory.SelectedIndex != -1 ? PickerGenderCategory.Items[PickerGenderCategory.SelectedIndex] : "";
            string typeSelected = PickerTypeCategory.SelectedIndex != -1 ? PickerTypeCategory.Items[PickerTypeCategory.SelectedIndex] : "";
            //  string remoteURL = txtRemoteFilePath.Text;
            if (string.IsNullOrEmpty(ownername) || string.IsNullOrEmpty(petname) || string.IsNullOrEmpty(petage) ||
                string.IsNullOrEmpty(genderSelected) || string.IsNullOrEmpty(typeSelected))
            {
                await DisplayAlert("Error", "Fill blank fields", "Accept");
            }

            else
            {

                var user = new User
                {
                    ID = txtID.Text,                
                    ownername = ownername + "",
                    petname = petname + "",
                    petage = petage + "",
                    petgender = genderSelected + "",
                    pettype = typeSelected + "",
                    petimage = txtRemoteFilePath.Text

                };

                activityIndicator.IsRunning = true;
                await UpdateUser(user);
                activityIndicator.IsRunning = false;
                Application.Current.MainPage = new NavigationPage(new FacebookProfilePage());
            }


        }
    }

}
