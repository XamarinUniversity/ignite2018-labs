using MyCircle.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCircle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			BindingContext = new LoginViewModel(Resources["ProfileColors"] as ProfileColorViewModel[]);
		}

        protected override bool OnBackButtonPressed()
        {
            // Do not allow back button to stop login.
            return true;
        }
    }
}