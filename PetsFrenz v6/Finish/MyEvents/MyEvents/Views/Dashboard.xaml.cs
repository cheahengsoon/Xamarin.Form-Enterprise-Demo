using Badge.Plugin;
using MyEvents.Card;
using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class Dashboard : ContentPage
    {
        CardStackView cardStack;
        MainPageViewModel viewModel = new MainPageViewModel();
        private User currentUser;

        public Dashboard()
        {
            InitializeComponent();


            currentUser = (User)Application.Current.Properties["user"];
            
            this.BindingContext = viewModel;
            this.BackgroundColor = Color.Black;

            RelativeLayout view = new RelativeLayout();

            cardStack = new CardStackView();
            cardStack.SetBinding(CardStackView.ItemsSourceProperty, "ItemsList");
            cardStack.SwipedLeft += SwipedLeft;
            cardStack.SwipedRight += SwipedRight;

            view.Children.Add(cardStack,
                              Constraint.Constant(30),
                              Constraint.Constant(60),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Width - 60;
                              }),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  return parent.Height - 140;
                              }));

            this.LayoutChanged += (object sender, System.EventArgs e) =>
            {
                cardStack.CardMoveDistance = (int)(this.Width * 0.60f);
            };

            this.Content = view;

            //ToolbarItem tbi = new ToolbarItem();
            //tbi.Icon = "frienz.png";
            //tbi.Text = "Friends";
            //this.ToolbarItems.Add(tbi);

            //Check No of Like
            if (!string.IsNullOrEmpty(currentUser.nolike))
            {
                int count = Int32.Parse(currentUser.nolike);
                CrossBadge.Current.SetBadge(count,"You got a "+count+" new Like!! ~ PetsFrenz");
            }
        }

        void SwipedLeft(int index)
        {
            // DisplayAlert("TEST", "Elemento rechazado", "ok", "ko");
        }

        void SwipedRight(int index)
        {
            // DisplayAlert("TEST", "Elemento agregado", "ok", "ko");
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