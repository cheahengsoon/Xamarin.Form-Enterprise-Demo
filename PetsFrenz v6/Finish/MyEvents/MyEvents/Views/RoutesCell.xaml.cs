using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class RoutesCell : ViewCell
    {
        public RoutesCell()
        {
            InitializeComponent();
            fromLabel.SetBinding(Label.TextProperty, new Binding("petname"));

            imageRoute.SetBinding(Image.SourceProperty, new Binding("petimage"));
        }

    }
}
