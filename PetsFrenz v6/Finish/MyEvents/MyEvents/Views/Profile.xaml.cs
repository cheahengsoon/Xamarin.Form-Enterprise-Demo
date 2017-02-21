using Badge.Plugin;
using MyEvents.Models;
using MyEvents.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{

    public partial class Profile : ContentPage
    {
        private User currentUser;
        UsersManager manager;

       // byte[] byteData;
       // byte[] imageData;

        private MediaFile _mediaFile;
        public Profile()
        {
            InitializeComponent();

            txtRemoteFilePath.IsVisible = false;
            txtfilePath.IsVisible = false;

            manager = new UsersManager();
            currentUser = (User)Application.Current.Properties["user"];

            loadData();

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
                    Name = Guid.NewGuid()+".jpg"
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

        void loadData()
        {

            string[] genders = { "Male", "Female" };
            string[] types = { "Bird", "Cat", "Dog", "Fish", "Rabbit", "Small Pet", "Others" };

            if (currentUser != null)
            {
                if (!string.IsNullOrEmpty(currentUser.ownername))
                    nameEntry.Text = currentUser.ownername;
                if (!string.IsNullOrEmpty(currentUser.petname))
                    petNameEntry.Text = currentUser.petname;
                if (currentUser.petage != null)
                {
                    petAgeEntry.Text = currentUser.petage + "";
                }
                if (!string.IsNullOrEmpty(currentUser.petgender))
                {
                    PickerGenderCategory.SelectedIndex = Array.IndexOf(genders, currentUser.petgender);

                }

                if (!string.IsNullOrEmpty(currentUser.pettype))
                {
                    PickerTypeCategory.SelectedIndex = Array.IndexOf(types, currentUser.pettype);
                }

                if (!string.IsNullOrEmpty(currentUser.petimage))
                {
                    img.Source = ImageSource.FromUri(new Uri(currentUser.petimage));
                }
                else
                {
                    img.Source = "person.png";
                }
                //  if(!string.IsNullOrEmpty(currentUser.nolike))
                //{
                //    int count = Int32.Parse(currentUser.nolike);
                //    CrossBadge.Current.SetBadge(count);
                //}
            }
        }

        async Task UpdateUser(User user)
        {
            User userResponse = await manager.SaveGetUserAsync(user);
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
                //Blob Storage path
                //????
           
                //-------------------------------------------------
                // var byteStr = Convert.ToBase64String(imageData);

                var user = new User
                {
                    ID = currentUser.ID,
                    email = currentUser.email,
                    password = currentUser.password,
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
                Application.Current.MainPage = new NavigationPage(new Dashboard());
            }

        }
    }
}
