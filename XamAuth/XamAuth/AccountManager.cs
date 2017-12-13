using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xamarin.Auth;
using System.Threading.Tasks;
using Xamarin.Auth.Cryptography;

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

            var salt = CryptoUtilities.Get256BitSalt();
            var hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(password), salt);

            var materialKey = CryptoUtilities.GetAES256KeyMaterial();

            AccountStore store = AccountStore.Create();
            if (await GetAccountFromStore(store, username) != null)
                return false;

            Account account = new Account(username);
            account.Properties.Add(pwKey, Convert.ToBase64String(hashedPassword));
            account.Properties.Add(saltKey, Convert.ToBase64String(salt));
            account.Properties.Add(kmKey, Convert.ToBase64String(materialKey));

            await store.SaveAsync(account, serviceID);

            return true;
        }

        public async Task<bool> LoginToAccount(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            AccountStore store = AccountStore.Create();
            Account account = await GetAccountFromStore(AccountStore.Create(), username);

            if (account == null)
                return false;

            byte[] salt, hashedPassword;

            if (!account.Properties.ContainsKey(saltKey))
            {
                salt = CryptoUtilities.Get256BitSalt();
                hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(account.Properties[pwKey]), salt);

                account.Properties[pwKey] = Convert.ToBase64String(hashedPassword);
                account.Properties.Add(saltKey, Convert.ToBase64String(salt));

                await store.SaveAsync(account, serviceID);
            }

            salt = Convert.FromBase64String(account.Properties[saltKey]);
            hashedPassword = CryptoUtilities.GetHash(CryptoUtilities.StringToByteArray(password), salt);

            return account != null && account.Properties[pwKey] == Convert.ToBase64String(hashedPassword);
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
