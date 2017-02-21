using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class RoutesCellFrienz : ViewCell
    {
        public RoutesCellFrienz()
        {
            InitializeComponent();
            fromLabel.SetBinding(Label.TextProperty, new Binding("friendpetname"));
          imageRoute.SetBinding(Image.SourceProperty, new Binding("petimage"));
            // toLabel.SetBinding(Label.TextProperty, new Binding("userpetname"));
        }
    }
}
