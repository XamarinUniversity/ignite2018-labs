using MyCircle.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyCircle.ViewModels
{
    public sealed class LoginViewModel
    {
        public IList<ProfileColorViewModel> ProfileColors { get; }
        public string UserName { get; set; }
        public Command Login { get; }
        public Command SelectProfileColor { get; }

        public LoginViewModel(ProfileColorViewModel[] colors)
        {
            if (colors == null)
                throw new System.ArgumentNullException(nameof(colors));

            ProfileColors = colors.ToList();
            Login = new Command(async () => await OnLoginAsync(), () => !string.IsNullOrWhiteSpace(UserName));
            SelectProfileColor = new Command<ProfileColorViewModel>(OnSelectProfileColor);
        }

        private void OnSelectProfileColor(ProfileColorViewModel preferredColor)
        {
            foreach (var pc in ProfileColors.Where(pc => pc.IsSelected && pc != preferredColor))
            {
                pc.IsSelected = false;
            }

            preferredColor.IsSelected = true;
        }

        private async Task OnLoginAsync()
        {
            string color = ProfileColors.Single(pc => pc.IsSelected).Color;

            await LoginService.LoginAsync(UserName, color);

            await App.Repository.AddAsync(new CircleMessage
            {
                Author = UserName,
                Color = color,
                IsRoot = true,
                Text = $"{UserName} entered the circle!",
            });
        }
    }
}
