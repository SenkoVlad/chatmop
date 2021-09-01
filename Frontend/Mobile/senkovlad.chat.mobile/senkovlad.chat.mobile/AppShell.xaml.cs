using senkovlad.chat.mobile.ViewModels;
using senkovlad.chat.mobile.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace senkovlad.chat.mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
