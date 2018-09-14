using MyCircle.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCircle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MessagesView : ContentView
	{
        public static readonly BindableProperty ShowDetailsProperty = BindableProperty.Create(
            nameof(ShowDetails), typeof(bool), typeof(MessagesView), false);

        public bool ShowDetails
        {
            get { return (bool)GetValue(ShowDetailsProperty);  }
            set { SetValue(ShowDetailsProperty, value); }
        }

        private EventHandler<ItemTappedEventArgs> tapHandler;
        public event EventHandler<ItemTappedEventArgs> ItemTapped
        {
            add
            {
                tapHandler += value;
                ShowDetails = true;
            }

            remove
            {
                tapHandler -= value;
                ShowDetails = (tapHandler == null);
            }
        }

        public MessagesView ()
		{
			InitializeComponent ();
		}

        public void OnAppearing()
        {
            if (Device.Idiom == TargetIdiom.Desktop 
                || Device.Idiom == TargetIdiom.Tablet)
            {
                // On desktop apps, shift focus to the entry.
                // Don't do this on mobile devices as onscreen keyboard obscures data.
                messageEntry.Focus();
            }

        }

        void OnMessageSelected(object sender, ItemTappedEventArgs e)
        {
            tapHandler?.Invoke(sender, e);
        }
    }
}