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
    public partial class LoginPage : ContentPage
    {
        public event Action<Account> LoginSucceeded = s => { };

        AccountManager accountManager;

        public LoginPage()
        {
            InitializeComponent();

            accountManager = new AccountManager();

            btnLogin.Clicked += BtnLogin_Clicked;
            btnCreateAccount.Clicked += BtnCreateAccount_Clicked;
        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if (await accountManager.LoginToAccount(entryUserName.Text, entryPassword.Text) == false)
            {
                await DisplayAlert("Login failed", "Unable to login, please check your username and password", "OK");
            }
            else
            {
                LoginSucceeded(await accountManager.GetAccount(entryUserName.Text));
                entryUserName.Text = string.Empty;
                entryPassword.Text = string.Empty;
            }
        }

        private async void BtnCreateAccount_Clicked(object sender, EventArgs e)
        {
            if (await accountManager.CreateAndSaveAccount(entryUserName.Text, entryPassword.Text))
                LoginSucceeded(await accountManager.GetAccount(entryUserName.Text));
            else
                await DisplayAlert("Create account failed", "Unable to create a new account  - does this account already exist?", "OK");
        }


    }
}