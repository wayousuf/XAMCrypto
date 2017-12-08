using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using XamAuth.Pages;

namespace XamAuth
{
    public partial class App : Application
    {
        public static string DatabaseFolder { get; set; }

        static DiaryEntryStore store;

        public static DiaryEntryStore Store
        {
            get => store ?? (store = new DiaryEntryStore(DatabaseFolder, "DiaryEntries.db3"));
        }


        public App()
        {
            InitializeComponent();

            var loginPage = new LoginPage();
            loginPage.LoginSucceeded += async (account) => await loginPage.Navigation.PushAsync(new DiaryEntriesPage(account));

            MainPage = new NavigationPage(loginPage);
        }



        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
