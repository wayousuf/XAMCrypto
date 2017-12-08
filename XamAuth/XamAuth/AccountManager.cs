using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Auth;
using System.Threading.Tasks;

namespace XamAuth
{
    public class AccountManager
    {
        const string serviceID = "Diary";
        const string pwKey = "password";
        const string kmKey = "keymaterial";
        const string saltKey = "salt";

        public async Task<bool> CreateAndSaveAccount(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            AccountStore store = AccountStore.Create();
            if (await GetAccountFromStore(store, username) != null)
                return false;

            Account account = new Account(username);
            account.Properties.Add(pwKey, password);
            await store.SaveAsync(account, serviceID);

            return true;
        }

        public async Task<bool> LoginToAccount(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            Account account = await GetAccountFromStore(AccountStore.Create(), username);
            return account != null && account.Properties[pwKey] == password;
        }

        public Task<Account> GetAccount(string username) => GetAccountFromStore(AccountStore.Create(), username);

        private async Task<Account> GetAccountFromStore(AccountStore store, string username)
        {
            if (store == null || username == null)
                return null;

            var accounts = await store.FindAccountsForServiceAsync(serviceID);
            return accounts.FirstOrDefault(a => a.Username == username);
        }
    }
}
