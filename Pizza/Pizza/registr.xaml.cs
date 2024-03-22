using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Pizza
{
    
    public partial class registr : ContentPage
    {
        private const string FirebaseUrl = "https://projectsd-4fe85-default-rtdb.firebaseio.com/";

        private readonly FirebaseClient firebase;
        public static string email;
        public registr()
        {
            InitializeComponent();
            
            firebase = new FirebaseClient(FirebaseUrl);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private async void ButtenAccount_registration_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(loginEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text) || string.IsNullOrEmpty(EnterpasswordEntry.Text))
            {
                await DisplayAlert("Ошибка регистрации", "Заполните все поля", "OK");
                return;
            }

            email = loginEntry.Text;
            string password = passwordEntry.Text;
            string confirmPassword = EnterpasswordEntry.Text;

            if (email.Length < 6)
            {
                await DisplayAlert("Ошибка регистрации", "Недопустимый адрес электронной почты", "OK");
                return;
            }

            if (password.Length < 6)
            {
                await DisplayAlert("Ошибка регистрации", "Пароль не должен быть короче 6 символов, попробуйте другой пароль", "OK");
                await DisplayAlert("Ошибка регистрации", "Пароль не должен быть короче 6 символов, попробуйте другой пароль", "OK");
                return;
            }

            if (password == confirmPassword)
            {
                string apiKey = "AIzaSyBPLVHy0u5UzSywYr5IKPIsUd_tMtLwIaE"; 

                FirebaseAuthProvider firebaseAuthProvider = new FirebaseAuthProvider(new FirebaseConfig(apiKey));



                try
                {
                    FirebaseAuthLink authLink = await firebaseAuthProvider.CreateUserWithEmailAndPasswordAsync(email, password);



                    Navigation.PushModalAsync(new Pizza());

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ошибка регистрации", ex.Message, "OK");
                }
            }
            else
            {
                await DisplayAlert("Ошибка регистрации", "Пароли не совпадают", "OK");
            }
        }

        

        private void ButtonRefund_Click(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new entrance()); 
        }
    }
}