using senkovlad.chat.mobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace senkovlad.chat.mobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}