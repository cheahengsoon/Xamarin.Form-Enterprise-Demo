using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyEvents.Card
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        List<PetPhoto> items = new List<PetPhoto>();
        public List<PetPhoto> ItemsList
        {
            get
            {
                return items;
            }
            set
            {
                if (items == value)
                    return;

                items = value;
                OnPropertyChanged();
            }
        }

  

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPageViewModel()

        {

          //  items = ItemsList;
            var listphoto = new ListView();
            listphoto.ItemsSource = ItemsList;
            //BindingContext=new MainPageViewModel();
            //items.Add(new CardStackView.Item() { Name = "Cheah", Photo = "mainOne.png", Location = "", Description = "Pizza" });
            //items.Add(new CardStackView.Item() { Name = "Dragon & Peacock", Photo = "puppyTwo.jpg", Location = "", Description = "Sweet & Sour" });
            //items.Add(new CardStackView.Item() { Name = "Murrays Food Palace", Photo = "CoolCorgi.jpg", Location = "", Description = "Salmon Plate" });
            //items.Add(new CardStackView.Item() { Name = "Food to go", Photo = "racecorgi.jpg", Location = "", Description = "Salad Wrap" });
            //items.Add(new CardStackView.Item() { Name = "Mexican Joint", Photo = "five.jpg", Location = "", Description = "Chilli Bites" });
            //items.Add(new CardStackView.Item() { Name = "Mr Bens", Photo = "six.jpg", Location = "", Description = "Beef" });
            //items.Add(new CardStackView.Item() { Name = "Corner Shop", Photo = "seven.jpg", Location = "", Description = "Burger & Chips" });
            //items.Add(new CardStackView.Item() { Name = "Sarah's Cafe", Photo = "eight.jpg", Location = "", Description = "House Breakfast" });
            //items.Add(new CardStackView.Item() { Name = "Pata Place", Photo = "nine.jpg", Location = "", Description = "Chicken Curry" });
            //items.Add(new CardStackView.Item() { Name = "Jerrys", Photo = "ten.jpg", Location = "", Description = "Pasta Salad" });
        }
    }
}