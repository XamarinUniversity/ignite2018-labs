using MyCircle.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCircle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
        }

        async void OnMessageSelected(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as CircleMessageViewModel;
            await Navigation.PushAsync(new DetailsPage() { BindingContext = new DetailsViewModel(item) });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = new MainViewModel { NewMessage = ((MainViewModel)BindingContext)?.NewMessage };

            Messages.OnAppearing();
        }
    }
}
