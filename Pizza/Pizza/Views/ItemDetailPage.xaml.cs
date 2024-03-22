using Pizza.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Pizza.Views
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