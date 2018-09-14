using Minutes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Minutes
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			entries.ItemTapped += OnItemTapped;
            newEntry.Completed += OnAddNewEntry;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			entries.ItemsSource = await App.Entries.GetAllAsync();
		}

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as NoteEntry;
            await Navigation.PushAsync(new NoteEntryEditPage(item));
        }

        private async void OnAddNewEntry(object sender, EventArgs e)
        {
            string text = newEntry.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                var item = new NoteEntry { Title = text };
                await App.Entries.AddAsync(item);
                await Navigation.PushAsync(new NoteEntryEditPage(item));
                newEntry.Text = string.Empty;
            }
        }
    }
}
