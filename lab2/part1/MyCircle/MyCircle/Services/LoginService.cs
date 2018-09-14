using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyCircle.Services
{
    public static class LoginService
    {
        static INavigation Navigation => App.Current.MainPage.Navigation;

        public static async Task LoginAsync(string userName, string color)
        {
            if (UserInfo.UserName != null
                && Navigation.ModalStack.Count == 0)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                UserInfo.UserName = userName;
                UserInfo.Color = color;
                await Navigation.PopModalAsync().ConfigureAwait(false);
            }
        }

        public static async Task LogoutAsync()
        {
            Debug.Assert(Navigation.ModalStack.Count == 0);
            UserInfo.UserName = null;
            UserInfo.Color = null;
            await Navigation.PushModalAsync(new LoginPage()).ConfigureAwait(false);
        }
    }
}
