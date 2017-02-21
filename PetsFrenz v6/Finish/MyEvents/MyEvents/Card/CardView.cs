﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyEvents.Card
{
    public class CardView : ContentView
    {
        public Label Name { get; set; }
        public Image Photo { get; set; }
  

        public CardView()
        {
            RelativeLayout view = new RelativeLayout();

            BoxView boxView1 = new BoxView { Color = Color.FromRgb(190, 0, 0), InputTransparent = true };

            view.Children.Add(boxView1, Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return parent.Width; }),
                              Constraint.RelativeToParent((parent) => { return parent.Height; }));

            Photo = new Image() { InputTransparent = true, Aspect = Aspect.Fill };

            view.Children.Add(Photo,
                              Constraint.Constant(0),
                              Constraint.RelativeToParent((parent) =>
                              {
                                  double h = parent.Height * 0.80;
                                  return ((parent.Height - h) / 2) + 20;
                              }),
                              Constraint.RelativeToParent((parent) => { return parent.Width; }),
                              Constraint.RelativeToParent((parent) => { return parent.Height; })

                             );

            Name = new Label()
            {
                TextColor = Color.White,
                FontSize = 22,
                InputTransparent = true
            };

            view.Children.Add(Name,
                              Constraint.Constant(10),
                              Constraint.Constant(10),
                              Constraint.RelativeToParent((parent) => { return parent.Width; }),
                              Constraint.Constant(28)
                             );
            Image icon = new Image() { Source = "location.png", InputTransparent = true };
            view.Children.Add(icon, Constraint.Constant(10), Constraint.Constant(40));

       
            Image[] stars = new Image[5];
            StackLayout stack = new StackLayout() { Orientation = StackOrientation.Horizontal, Spacing = 2 };
            for (int i = 0; i < 5; i++)
            {
                stars[i] = new Image() { Source = "star_on", InputTransparent = true };
                stack.Children.Add(stars[i]);
            }

            view.Children.Add(stack,
                              Constraint.RelativeToParent((parent) => { return parent.Width - 90; }),
                              Constraint.Constant(40)
                             );

     

            Content = view;
        }


    }
}
