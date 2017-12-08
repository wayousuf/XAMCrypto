using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamAuth
{
    [Table("DiaryEntry")]
    public class DiaryEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = -1;

        public DateTime Timestamp { get; set; }

        public byte[] InitalizationVector { get; set; }
        public byte[] CipherText { get; set; }

        public string AccountName { get; set; }

        public DiaryEntry()
        {
            Timestamp = DateTime.Now;
        }

        public DiaryEntry(byte[] initialicationVector, byte[] cipherText, string account) : this()
        {
            InitalizationVector = initialicationVector;
            CipherText = cipherText;
            AccountName = account;
        }
    }
}
