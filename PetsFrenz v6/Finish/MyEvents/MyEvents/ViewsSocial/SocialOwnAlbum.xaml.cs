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
    public partial class SocialOwnAlbum : ContentPage
    {
        private string owneremail;
        private User user;
        private UsersManager usersManager;


        private List<PetPhoto> userList;
        private PetPhotoManager petphotoUserManager;
        private MediaFile _mediaFile;
        public SocialOwnAlbum(string owneremail)
        {
            InitializeComponent();
            this.owneremail = owneremail;
            txtSocialName.Text = owneremail;

            user = new User
            {
                email = owneremail

            };
            usersManager = new UsersManager();

            userList = new List<PetPhoto>();
            petphotoUserManager = new PetPhotoManager();

            userListView.ItemTemplate = new DataTemplate(typeof(ImageRouteList));

            LoadImage();
            //txtcurrentuser.Text = currentUser.email;
            btntakePhoto.Clicked += async (sender, args) =>
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

                // txtfilePath.Text = _mediaFile.Path;

                // img.Source = ImageSource.FromStream(() =>
                // {
                //     return _mediaFile.GetStream();
                //  });


                //Cannot call it from here

                string remoteURL = "https://petfrenzblob.blob.core.windows.net/petuser/" + _mediaFile.Path;

                await BlobMan.Instance.UploadFileAsync(_mediaFile.Path);

                var petPhoto = new PetPhoto
                {
                    owneremail = user.email,
                    petname = user.petname,
                    petimage = remoteURL,
                    datecreated = DateTime.Now,
                    nolike = "1"

                };
                await AddPetPhoto(petPhoto);
            };

            btnPickPhoto.Clicked += async (sender, args) =>
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

                string remoteURL = "https://petfrenzblob.blob.core.windows.net/petuser/" + _mediaFile.Path;

                await BlobMan.Instance.UploadFileAsync(_mediaFile.Path);

                var petPhoto = new PetPhoto
                {
                    owneremail = user.email,
                    petname = user.petname,
                    petimage = remoteURL,
                    datecreated = DateTime.Now,
                    nolike = "1"

                };
                await AddPetPhoto(petPhoto);
            };
        }

        private async Task AddPetPhoto(PetPhoto petPhoto)
        {
            PetPhoto userResponse = await petphotoUserManager.SaveGetUserAsync(petPhoto);
        }

        private async void LoadImage()
        {
            userList = await petphotoUserManager.ListUserWhere(userSelect => userSelect.owneremail == user.petname);
            // user = await usersManager.GetUserWhere(userSelect => userSelect.email == user.email);
            if (userList.Count != 0)
            {
                userListView.ItemsSource = userList;

            }
            else
            {
                txtnorecord.Text = "No Photo Founded.";
            }
        }
    }
}
