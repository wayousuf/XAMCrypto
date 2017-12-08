using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace XamAuth
{
    public class DiaryEntryStore
    {
        string path;
        SQLiteAsyncConnection connection;

        public DiaryEntryStore(string folder, string filename)
        {
            if (string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(filename))
                return;

            path = Path.Combine(folder, filename);

            connection = new SQLiteAsyncConnection(path);

            connection.CreateTableAsync<DiaryEntry>().Wait();
        }

        public Task SaveEntryAsync(DiaryEntry entry)
        {
            if (entry.Id == -1)
                return connection.InsertAsync(entry);
            else
                return connection.InsertOrReplaceAsync(entry);
        }

        public Task DeleteEntryAsync(DiaryEntry entry) => connection.DeleteAsync(entry);

        public Task<List<DiaryEntry>> GetEntriesAsync(string accountName) => connection.Table<DiaryEntry>().Where(d => d.AccountName == accountName).ToListAsync();

        public Task<DiaryEntry> GetEntry(int id) => connection.Table<DiaryEntry>().Where(d => d.Id == id).FirstOrDefaultAsync();
    }
}
