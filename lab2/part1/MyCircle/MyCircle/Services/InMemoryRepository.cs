using MyCircle.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCircle.Services
{
    sealed class InMemoryMessageRepository : IAsyncMessageRepository
    {
        const string SystemColor = "Black";
        Random rnd = new Random();
        List<CircleMessage> items = new List<CircleMessage>();

        public InMemoryMessageRepository()
        {
            items.Add(new CircleMessage
            {
                IsRoot = true,
                Author = "System",
                Text = "Welcome to Your Circle!",
                Color = SystemColor
            });
        }

        public Task<IEnumerable<CircleMessage>> GetRootsAsync()
        {
            // Each time we refresh, add a few new entries.
            AddRandomData();

            return Task.FromResult(items
                .Where(n => n.IsRoot)
                .OrderByDescending(n => n.CreatedAt)
                .AsEnumerable());
        }

        public Task<IEnumerable<CircleMessage>> GetDetailsAsync(string id)
        {
            return Task.FromResult(items
                .Where(n => n.ThreadId == id)
                .OrderBy(n => n.CreatedAt)
                .AsEnumerable());
        }

        public Task<long> GetDetailCountAsync(string id)
        {
            return Task.FromResult(items.LongCount(n => n.ThreadId == id && !n.IsRoot));
        }

        public Task AddAsync(CircleMessage message)
        {
            if (!items.Contains(message))
            {
                items.Add(message);
            }
            return Task.CompletedTask;
        }

        #region TestData
        private void AddRandomData()
        {
            int count = 1 + rnd.Next(10);

            for (int i = 0; i < count; i++)
            {
                string parent = null;
                if (rnd.Next(100) % 5 == 0)
                {
                    parent = items[rnd.Next(items.Count - 1)].ThreadId;
                }

                var message = new CircleMessage(parent)
                {
                    Author = "System",
                    Color = SystemColor,
                    IsRoot = (parent == null),
                    Text = String.Join(" ", Enumerable.Range(0, rnd.Next(20)).Select(_ => GetWord(rnd.Next(10))))
                };

                items.Add(message);
            }
        }

        string GetWord(int requestedLength)
        {
            const string vowels = "aeiou";
            const string consonants = "bcdfghjklmnpqrstvwxyz";
            char RandomLetter(string letters) => letters[rnd.Next(0, letters.Length - 1)];

            StringBuilder word = new StringBuilder();
            if (requestedLength == 1)
            {
                word.Append(RandomLetter(vowels));
            }
            else
            {
                for (int i = 0; i < requestedLength; i += 2)
                {
                    word.Append(RandomLetter(consonants));
                    word.Append(RandomLetter(vowels));
                }

                word.Replace("q", "qu");
            }

            return word.ToString().Substring(0, requestedLength);
        }
        #endregion
    }
}
