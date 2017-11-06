using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyRoommate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            
            InitializeComponent();
        /*    var semiTransparentColor = new Color(0, 0, 0, 0.5);
            ScrollView.BackgroundColor = semiTransparentColor;*/
        }
    }
}