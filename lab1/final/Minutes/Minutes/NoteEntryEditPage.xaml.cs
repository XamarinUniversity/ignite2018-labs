using Minutes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Minutes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoteEntryEditPage : ContentPage
	{
        private NoteEntry entry;

        public NoteEntryEditPage (NoteEntry entry)
		{
			InitializeComponent ();
            BindingContext = this.entry = entry;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Device.Idiom == TargetIdiom.Desktop
                || Device.Idiom == TargetIdiom.Tablet)
            {
                textEditor.Focus();
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            if (entry != null)
            {
                await App.Entries.UpdateAsync(entry);
            }
        }

        private async void OnDeleteEntry(object sender, EventArgs e)
        {
            if (await DisplayAlert("Delete Entry", $"Are you sure you want to delete the entry {Title}?", "Yes", "No"))
            {
                await App.Entries.DeleteAsync(entry);
                entry = null; // deleted!
                await Navigation.PopAsync();
            }
        }
    }
}