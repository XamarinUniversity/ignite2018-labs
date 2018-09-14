using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Minutes.Data
{
    public class FileEntryStore : INoteEntryStore
    {
        List<NoteEntry> loadedNotes;
        string filename;

        public FileEntryStore()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
            if (string.IsNullOrEmpty(folder))
               folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            this.filename = Path.Combine(folder, "minutes.xml");
        }

        private async Task InitializeAsync()
        {
            if (loadedNotes == null)
            {
                loadedNotes = (await ReadDataAsync(filename)).ToList();
            }
        }


        public async Task AddAsync(NoteEntry entry)
        {
            await InitializeAsync();

            if (!loadedNotes.Any(ne => ne.Id == entry.Id))
            {
                loadedNotes.Add(entry);
                await SaveDataAsync(filename, loadedNotes);
            }
        }

        public async Task DeleteAsync(NoteEntry entry)
        {
            await InitializeAsync();

            if (loadedNotes.Remove(entry))
            {
                await SaveDataAsync(filename, loadedNotes);
            }
        }

        public async Task<IEnumerable<NoteEntry>> GetAllAsync()
        {
            await InitializeAsync();
            return loadedNotes.OrderByDescending(n => n.CreatedDate);
        }

        public async Task<NoteEntry> GetByIdAsync(string id)
        {
            await InitializeAsync();
            return loadedNotes.SingleOrDefault(n => n.Id == id);
        }

        public async Task UpdateAsync(NoteEntry entry)
        {
            await InitializeAsync();

            if (!loadedNotes.Contains(entry))
            {
                throw new Exception($"NoteEntry {entry.Title} was not found in the {nameof(FileEntryStore)}. Did you forget to add it?");
            }

            await SaveDataAsync(filename, loadedNotes);
        }

        private static async Task<IEnumerable<NoteEntry>> ReadDataAsync(string filename)
        {
            if (!File.Exists(filename))
            {
                return Enumerable.Empty<NoteEntry>();
            }

            string text;
            using (var reader = new StreamReader(filename))
            {
                text = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return Enumerable.Empty<NoteEntry>();
            }

            IEnumerable<NoteEntry> result = XDocument.Parse(text)
                    .Root
                    .Elements("entry")
                    .Select(e =>
                        new NoteEntry
                        {
                            Title = e.Attribute("title").Value,
                            Text = e.Attribute("text").Value,
                            CreatedDate = (DateTime)e.Attribute("createdDate")
                        });

            return result;
        }

        static async Task SaveDataAsync(string filename, IEnumerable<NoteEntry> notes)
        {
            XDocument root = new XDocument(
                new XElement("minutes",
                    notes.Select(n =>
                        new XElement("entry",
                            new XAttribute("title", n.Title ?? ""),
                            new XAttribute("text", n.Text ?? ""),
                            new XAttribute("createdDate", n.CreatedDate)))));

            using (StreamWriter writer = new StreamWriter(filename))
            {
                await writer.WriteAsync(root.ToString()).ConfigureAwait(false);
            }
        }
    }
}
