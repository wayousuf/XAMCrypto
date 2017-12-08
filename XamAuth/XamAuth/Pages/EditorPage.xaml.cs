using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Auth.Cryptography;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamAuth.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : ContentPage
    {
        DiaryEntry entry;
        Account account;

        const string kmKey = "keymaterial";

        public EditorPage(Account account, DiaryEntry entry = null)
        {
            this.account = account;
            this.entry = entry ?? new DiaryEntry { AccountName = account?.Username };

            InitializeComponent();

            if (this.entry.CipherText != null)
                editorEntry.Text = GetDiaryText(this.entry.CipherText);

            btnSave.Clicked += BtnSave_Clicked;
            btnCancel.Clicked += BtnCancel_Clicked;
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PopAsync();
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(editorEntry.Text))
            {
                entry.CipherText = GetChiperText(editorEntry.Text);

                if (entry.CipherText != null)
                    await App.Store.SaveEntryAsync(entry);
            }

            await this.Navigation.PopAsync();
        }

        private byte[] GetChiperText(string diaryText)
        {
            return CryptoUtilities.StringToByteArray(diaryText);
        }

        private string GetDiaryText(byte[] cipherText)
        {
            return CryptoUtilities.ByteArrayToString(cipherText);
        }
    }
}