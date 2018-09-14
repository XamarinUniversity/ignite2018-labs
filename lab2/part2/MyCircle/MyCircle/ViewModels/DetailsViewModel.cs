using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCircle.ViewModels
{
    public sealed class DetailsViewModel : BasePageViewModel
    {
        public CircleMessageViewModel Parent { get; }

        public DetailsViewModel(CircleMessageViewModel parent) 
            : base(() => LoadChildren(parent.Message.ThreadId))
        {
            Parent = parent;
        }

        private static async Task<IEnumerable<CircleMessageViewModel>> LoadChildren(string id)
        {
            var results = await App.Repository.GetDetailsAsync(id).ConfigureAwait(false);
            return results.Skip(1)
                          .Select(n => new CircleMessageViewModel(n));
        }

        protected override CircleMessageViewModel CreateNewMessageViewModel(string messageText)
        {
            return new CircleMessageViewModel(messageText, Parent.Message.ThreadId);
        }
    }
}
