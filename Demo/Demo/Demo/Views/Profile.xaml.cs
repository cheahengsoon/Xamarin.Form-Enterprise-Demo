using Demo.Models;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Demo.Views
{
    public partial class Profile : ContentPage
    {
        private User currentUser;
        UsersManager manager;

        public Profile()
        {
            InitializeComponent();

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
        }

         void loadData()
        {
            string[] genders = { "Male", "Female" };
            string[] types = { "Bird", "Cat", "Dog", "Fish", "Rabbit", "Small Pet", "Others" };

            if(currentUser!=null)
            {
                if (!string.IsNullOrEmpty(currentUser.ownername))
                    nameEntry.Text = currentUser.ownername;
                if (!string.IsNullOrEmpty(currentUser.petname))
                    petNameEntry.Text = currentUser.petname;
                if(currentUser.petage!=null)
                {
                    petAgeEntry.Text = currentUser.petage + "";
                }
                if(!string.IsNullOrEmpty(currentUser.petgender))
                {
                    PickerGenderCategory.SelectedIndex = Array.IndexOf(genders, currentUser.petgender);

                }

                if(!string.IsNullOrEmpty(currentUser.pettype))
                {
                    PickerTypeCategory.SelectedIndex = Array.IndexOf(types, currentUser.pettype);
                }


            }
        }

        async Task UpdateUser(User user)
        {
            User userResponse = await manager.SaveGetUserAsync(user);
            Application.Current.Properties["user"] = userResponse;
        }

        async void Dashboard_Clicked(object sender,EventArgs e)
        {
            string ownername = this.nameEntry.Text;
            string petname = this.petNameEntry.Text;
            string petage = this.petAgeEntry.Text;
            string genderSelected = PickerGenderCategory.SelectedIndex != -1 ? PickerGenderCategory.Items[PickerGenderCategory.SelectedIndex] : "";
            string typeSelected = PickerTypeCategory.SelectedIndex != -1 ? PickerTypeCategory.Items[PickerTypeCategory.SelectedIndex] : "";

            if(string.IsNullOrEmpty(ownername)||string.IsNullOrEmpty(petname)||string.IsNullOrEmpty(petage)||
                string.IsNullOrEmpty(genderSelected)||string.IsNullOrEmpty(typeSelected))
            {
                await DisplayAlert("Error", "Fill blank fields", "Accept");
            }

            else
            {
                var user = new User
                {
                    ID = currentUser.ID,
                    email = currentUser.email,
                    password = currentUser.password,
                    ownername = ownername + "",
                    petname = petname + "",
                    petage = petage + "",
                    petgender = genderSelected + "",
                    pettype = typeSelected + ""

                };

                activityIndicator.IsRunning = true;
                await UpdateUser(user);
                activityIndicator.IsRunning = false;
                Application.Current.MainPage = new NavigationPage(new Dashboard());
            }
        
        }
        async void lblSignUp_Tapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Photo ?", "Cancel", null, "Camera", "Folder");
            //Debug.WriteLine("Action: " + action);

            switch (action)
            {
                case "Camera": 
                  TakePhoto();
                    break;
                case "Folder":
                    FromFolder();
                    break;

            }

        }

        async void FromFolder()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            try
            {
                Stream stream = null;
                var file = await CrossMedia.Current.PickPhotoAsync().ConfigureAwait(true);


                if (file == null)
                    return;

                stream = file.GetStream();
                file.Dispose();

                img.Source = ImageSource.FromStream(() => stream);

            }
            catch //(Exception ex)
            {
                // Xamarin.Insights.Report(ex);
                // await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");
            }
        }

        async void TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }
            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "test.jpg",
                    //SaveToAlbum = saveToGallery.IsToggled
                });

                if (file == null)
                    return;

                //  await DisplayAlert("File Location", (saveToGallery.IsToggled ? file.AlbumPath : file.Path), "OK");

                img.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            catch //(Exception ex)
            {
                // Xamarin.Insights.Report(ex);
                // await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured it in Xamarin Insights! Thanks.", "OK");
            }
        }
    }
}
