using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class ImageRouteList : ViewCell
    {
      private string stringFormat;

        //  private string stringFormat;

        public ImageRouteList()
        {
            InitializeComponent();
            lblPetName.SetBinding(Label.TextProperty, new Binding("petname",stringFormat:"Pet Name : {0}"));
         
            //lblPetName.SetBinding(TextCell.TextProperty, new Binding("petname"));
            imageRoute.SetBinding(Image.SourceProperty, new Binding("petimage"));
           lblnolike.SetBinding(Label.TextProperty,  new Binding("nolike", stringFormat: "Likes: {0}"));
            // imageRoute.Source = new UriImageSource { CachingEnabled = true };
        }
    }
}
