using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCircle.Services
{
    public sealed class AzureMessageRepository : IAsyncMessageRepository
    {
        const string AzureServiceUrl = "https://build2018mycircle.azurewebsites.net";
        MobileServiceClient client;
        IMobileServiceTable<CircleMessage> messages;

        public AzureMessageRepository()
        {
            client = new MobileServiceClient(AzureServiceUrl);
            messages = client.GetTable<CircleMessage>();
        }

        public Task AddAsync(CircleMessage message)
        {
            Debug.Assert(message.Id == null);
            return messages.InsertAsync(message);
        }

        public async Task<IEnumerable<CircleMessage>> GetRootsAsync()
        {
            return await messages.Where(cm => cm.IsRoot)
                .OrderByDescending(cm => cm.CreatedAt)
                .ToEnumerableAsync();
        }

        public async Task<long> GetDetailCountAsync(string id)
        {
            var result = await messages
                           .Where(cm => cm.ThreadId == id && !cm.IsRoot)
                           .IncludeTotalCount().ToEnumerableAsync()
                           .ConfigureAwait(false) as IQueryResultEnumerable<CircleMessage>;
            return result.TotalCount;
        }

        public Task<IEnumerable<CircleMessage>> GetDetailsAsync(string id)
        {
            return messages.Where(cm => cm.ThreadId == id)
                .OrderBy(cm => cm.CreatedAt)
                .ToEnumerableAsync();
        }
    }
}
