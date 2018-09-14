using MyCircle.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinUniversity.Infrastructure;

namespace MyCircle.ViewModels
{
    public sealed class MainViewModel : BasePageViewModel
    {
        public ICommand Logout { get; }

        public MainViewModel() : base(LoadAllRoots)
        {
            Logout = new Command(async () => await LoginService.LogoutAsync());
        }

        private static async Task<IEnumerable<CircleMessageViewModel>> LoadAllRoots()
        {
            var results = await App.Repository
                .GetRootsAsync()
                .ConfigureAwait(false);

            return results.Select(c => new CircleMessageViewModel(c));
        }

        protected override CircleMessageViewModel CreateNewMessageViewModel(string messageText)
        {
            return new CircleMessageViewModel(messageText);
        }
    }
}
