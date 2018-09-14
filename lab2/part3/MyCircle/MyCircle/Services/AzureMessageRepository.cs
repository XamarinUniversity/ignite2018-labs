using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MyCircle.Services
{
    public sealed class AzureMessageRepository : IAsyncMessageRepository
    {
        const string AzureServiceUrl = "https://build2018mycircle.azurewebsites.net";
        MobileServiceClient client;
        IMobileServiceSyncTable<CircleMessage> messages;

        public AzureMessageRepository()
        {
            client = new MobileServiceClient(AzureServiceUrl);
        }

        async Task InitializeTableAsync()
        {
            if (messages == null)
            {
                // Define the database schema
                var store = new MobileServiceSQLiteStore("offlinecache.db");
                store.DefineTable<CircleMessage>();

                // Create the DB file
                await client.SyncContext.InitializeAsync(store);

                // Get the sync table
                messages = client.GetSyncTable<CircleMessage>();
            }
        }

        public async Task AddAsync(CircleMessage message)
        {
            await InitializeTableAsync();
            await messages.InsertAsync(message);
            await PushChangesAsync();
        }

        public async Task<IEnumerable<CircleMessage>> GetRootsAsync()
        {
            await InitializeTableAsync();
            await PullChangesAsync();

            return await messages.Where(cm => cm.IsRoot)
                .OrderByDescending(cm => cm.CreatedAt)
                .ToEnumerableAsync();
        }

        public async Task<long> GetDetailCountAsync(string id)
        {
            Debug.Assert(messages != null);
            var result = await messages
                           .Where(cm => cm.ThreadId == id && !cm.IsRoot)
                           .IncludeTotalCount().ToEnumerableAsync()
                           .ConfigureAwait(false) as IQueryResultEnumerable<CircleMessage>;
            return result.TotalCount;
        }

        public async Task<IEnumerable<CircleMessage>> GetDetailsAsync(string id)
        {
            Debug.Assert(messages != null);
            await PullChangesAsync();

            return await messages.Where(cm => cm.ThreadId == id)
                .OrderBy(cm => cm.CreatedAt)
                .ToEnumerableAsync();
        }

        Task<bool> IsOnlineAsync()
        {
            return CrossConnectivity.Current
                .IsRemoteReachable(client.MobileAppUri, 
                    TimeSpan.FromSeconds(5));
        }

        async Task PushChangesAsync()
        {
            if (!await IsOnlineAsync())
                return;

            try
            {
                // Push queued changes back to Azure
                await client.SyncContext.PushAsync();

            }
            catch (MobileServicePushFailedException ex)
            {
                foreach (var error in ex.PushResult?.Errors)
                {
                    await ResolveConflictAsync(error);
                }
            }
        }

        async Task PullChangesAsync()
        {
            if (!await IsOnlineAsync())
                return;

            // Pull changes from Azure back down to our copy
            // We pull the entire table down incrementally, unless a full sync is required.
            // We could also cache off specific queries - but that would require more round trips
            // to Azure which we want to avoid since we use all the data ..
            await messages.PullAsync($"sync_{nameof(CircleMessage)}", 
                                     messages.CreateQuery())
                          .ConfigureAwait(false);
        }

        Task ResolveConflictAsync(MobileServiceTableOperationError error)
        {
            var serverItem = error.Result.ToObject<CircleMessage>();
            var localItem = error.Item.ToObject<CircleMessage>();

            if (serverItem.Equals(localItem))
            {
                // Items are identical, ignore the conflict; server wins.
                return error.CancelAndDiscardItemAsync();
            }
            else
            {
                // otherwise, the client wins.
                localItem.Version = serverItem.Version;
                return error.UpdateOperationAsync(JObject.FromObject(localItem));
            }
        }
    }
}
