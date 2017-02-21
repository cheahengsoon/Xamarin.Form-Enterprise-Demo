using MyEvents.Models;
using MyEvents.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class ImageList : ContentPage
    {
       
        private User currentUser;
        UsersManager usermanager;
        private MediaFile _mediaFile;

        private List<PetPhoto> userList;
        private PetPhotoManager petphotoUserManager;
        public ImageList()
        {
            InitializeComponent();

            userList = new List<PetPhoto>();
            petphotoUserManager = new PetPhotoManager();

            currentUser = (User)Application.Current.Properties["user"];
            userListView.ItemTemplate = new DataTemplate(typeof(ImageRouteList));
            userListView.Refreshing += UserListView_Refreshing;
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
                    owneremail = currentUser.email,
                    petname = currentUser.petname,
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
                    owneremail = currentUser.email,
                    petname = currentUser.petname,
                    petimage = remoteURL,
                    datecreated = DateTime.Now,
                     nolike = "1"

                };
                await AddPetPhoto(petPhoto);
            };

        }

        private void UserListView_Refreshing(object sender, EventArgs e)
        {
            LoadImage();
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    LoadImage();
        //}

        private async void LoadImage()
        {
            userListView.IsRefreshing = true;
            userList = await petphotoUserManager.ListUserWhere(userSelect => userSelect.owneremail == currentUser.email);
            if (userList.Count != 0)
            {
                userListView.ItemsSource = userList;
            }
            else
            {
                txtnorecord.Text = "Sorry,No photo Founded.";
            }
            userListView.IsRefreshing = false;
        }

        async Task AddPetPhoto(PetPhoto petPhoto)
        {
            PetPhoto userResponse = await petphotoUserManager.SaveGetUserAsync(petPhoto);
        }
    }
}
