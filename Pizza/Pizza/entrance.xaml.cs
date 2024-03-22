using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Auth;

namespace Pizza
{
	
	public partial class entrance : ContentPage
	{
        public static string email;
        public entrance ()
		{
			InitializeComponent ();
		}
        private async void ButtonEntrance_Click(object sender, EventArgs e)
        {

            try
            {
                email = loginEntry.Text; 
                string password = passwordEntry.Text;
               
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyBPLVHy0u5UzSywYr5IKPIsUd_tMtLwIaE"));
                var authLink = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                
                await Navigation.PushModalAsync(new Pizza());
            }
            catch (FirebaseAuthException ex)
            {
                // Обработка ошибок аутентификации
                if (ex.Reason == AuthErrorReason.WrongPassword || ex.Reason == AuthErrorReason.UnknownEmailAddress)
                {
                    await DisplayAlert("Ошибка", "Неверный логин или пароль", "OK");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Ошибка аутентификации: " + ex.Reason, "OK");
                }
            }
        }

        private void ButtonRegistration_Click(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new registr());
        }
    }
}
