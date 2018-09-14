using MyCircle.Services;
using System.Threading.Tasks;
using XamarinUniversity.Infrastructure;

namespace MyCircle.ViewModels
{
    public sealed class CircleMessageViewModel : SimpleViewModel
    {
        public CircleMessage Message { get; }
        public bool IsCurrentUser { get; }
        public long DetailCount
        {
            get => detailCount;
            set => SetPropertyValue(ref detailCount, value);
        }
        public string Author => $"{Message.CreatedDate:G} {Message.Author}";

        public CircleMessageViewModel(CircleMessage message)
        {
            Message = message;
            IsCurrentUser = message.Author == UserInfo.UserName;

            if (message.IsRoot)
            {
                UpdateDetailsCountAsync().ConfigureAwait(false);
            }
        }

        public CircleMessageViewModel(string message, string parentThreadId = null)
        {
            IsCurrentUser = true;
            Message = new CircleMessage(parentThreadId)
            {
                Author = UserInfo.UserName,
                IsRoot = parentThreadId == null,
                Text = message,
                Color = UserInfo.Color,
            };
        }

        public async Task UpdateDetailsCountAsync()
        {
            if (Message.IsRoot)
            {
                DetailCount = await App.Repository.GetDetailCountAsync(Message.ThreadId).ConfigureAwait(false);
            }
        }

        private long detailCount;
    }
}
