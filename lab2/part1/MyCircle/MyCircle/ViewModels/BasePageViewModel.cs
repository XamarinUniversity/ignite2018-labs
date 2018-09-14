using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinUniversity.Infrastructure;

namespace MyCircle.ViewModels
{
    public abstract class BasePageViewModel : SimpleViewModel
    {
        public string NewMessage
        {
            get => newMessage;
            set
            {
                if (SetPropertyValue(ref newMessage, value))
                {
                    AddMessage.ChangeCanExecute();
                }
            }
        }
        public RefreshingCollection<CircleMessageViewModel> Messages { get; }
        public Command AddMessage { get; }
        public Command RefreshData { get; }

        protected BasePageViewModel(Func<Task<IEnumerable<CircleMessageViewModel>>> refreshMethod)
        {
            AddMessage = new Command(async () => await OnAddMessageAsync(), () => !string.IsNullOrEmpty(NewMessage));
            RefreshData = new Command(async () => await Messages.RefreshAsync());
            Messages = new RefreshingCollection<CircleMessageViewModel>(refreshMethod)
            {
                Merge = OnMergeNewData,
                // TODO: move to UI or use service?
                RefreshFailed = async (s, ex) => await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK")
            };

            Messages.RefreshAsync().ConfigureAwait(false);
        }

        private static void OnMergeNewData(RefreshingCollection<CircleMessageViewModel> collection, IEnumerable<CircleMessageViewModel> newData)
        {
            int pos = 0;
            foreach (var item in newData)
            {
                if (!collection.Any(cd => cd.Message.Id == item.Message.Id))
                {
                    collection.Insert(pos, item);
                }
                pos++;
            }

            foreach (var item in collection)
            {
                item.UpdateDetailsCountAsync().ConfigureAwait(false);
            }
        }

        private async Task OnAddMessageAsync()
        {
            var vm = CreateNewMessageViewModel(NewMessage);
            await App.Repository.AddAsync(vm.Message);
            Messages.Insert(0, vm); // add to head
            NewMessage = string.Empty;
        }

        protected abstract CircleMessageViewModel CreateNewMessageViewModel(string messageText);
        private string newMessage;
    }
}
