using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamAuth.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryEntriesPage : ContentPage
    {
        Account account;
        public DiaryEntriesPage(Account account)
        {
            InitializeComponent();

            this.account = account;

            listEntries.ItemTapped += ListEntries_ItemTapped;
            btnAddDiaryEntry.Clicked += BtnAddDiaryEntry_Clicked;
        }

        private async void BtnAddDiaryEntry_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new EditorPage(account));
        }

        protected async override void OnAppearing()
        {
            if (account != null)
                listEntries.ItemsSource = await App.Store.GetEntriesAsync(account.Username);
            base.OnAppearing();
        }

        private async void ListEntries_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var entry = listEntries.SelectedItem as DiaryEntry;

            await this.Navigation.PushAsync(new EditorPage(account, entry), true);
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var entry = (sender as MenuItem).BindingContext as DiaryEntry;

            await App.Store.DeleteEntryAsync(entry);

            listEntries.ItemsSource = await App.Store.GetEntriesAsync(account.Username);
        }
    }
}