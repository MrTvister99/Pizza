using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using static Google.Rpc.Help.Types;
using Xamarin.Essentials;

namespace Pizza
{
    
    public partial class Pizza : ContentPage
    {
        List<Pizzas> tovar_list;
        private string Products;
        private string mail;
    
        public Pizza()
        {
            InitializeComponent();
            Products = "Pizza";
            
            BindingContext = this;
            LoadDataIntoListView();


        }

      
        public async Task LoadDataIntoListView()
        {
            if (entrance.email != null)
            {
                string mail1 = entrance.email.ToString();
                mail = mail1.Replace(".", "");

            }
            else
            {
                string mail1 = registr.email.ToString();
                mail = mail1.Replace(".", "");


            }

            var firebaseClient = new FirebaseClient("https://projectsd-4fe85-default-rtdb.firebaseio.com");
            var prod = firebaseClient.Child(Products); 
            var tovars = await prod.OnceAsync<Pizzas>();
            tovar_list = new List<Pizzas>();
            tovar_list.Clear();
            
            foreach (var product in tovars)
            {
                if (Products == "Basket")
                {
                    product.Object.IsButtonVisible1 = true;
                }
                else
                {
                    product.Object.IsButtonVisible1 = false;
                }

                tovar_list.Add(product.Object);
            }
            myListView.ItemsSource = tovar_list;
        }
        public async Task LoadDataIntoListViewFororzina()
        {
            var firebaseClient = new FirebaseClient("https://projectsd-4fe85-default-rtdb.firebaseio.com");
            var baskets = await firebaseClient
                .Child("Basket")
                .OnceAsync<Pizzas>();

            tovar_list = new List<Pizzas>();

            foreach (var basket in baskets)
            {
                var products = await firebaseClient
                    .Child("Basket")
                    .Child(basket.Key) 
                    .OnceAsync<Pizzas>();

                foreach (var product in products)
                {
                    if (Products == "Basket")
                    {
                        product.Object.IsButtonVisible1 = true;
                    }
                    else
                    {
                        product.Object.IsButtonVisible1 = false;
                    }

                    if (basket.Key == mail)
                    {
                      
                        product.Object.IsButtonVisible = false;
                        tovar_list.Add(product.Object);

                    }
                }
            }

            myListView.ItemsSource = tovar_list;
        }

        public class Pizzas
        {
            public string Img { get; set; }

            public string Название { get; set; }
            public decimal Цена { get; set; }


            public string FormatedPrice => $"Цена: {Цена} руб";

            public bool IsButtonVisible { get; set; }
            public bool IsButtonVisible1 { get; set; }
            public Pizzas() { 
                IsButtonVisible = true;
                IsButtonVisible1 = false;
            }

        }

        private async void send_Clicked(object sender, EventArgs e)
        {

            var products = new Pizzas();
            Button button = (Button)sender;
            Pizzas selectedProduct = (Pizzas)button.CommandParameter;
            button.IsEnabled = false;
            button.Text = "Заказано";
            await Task.Delay(3000);
            button.IsEnabled = true;
            button.BackgroundColor = Color.Orange;
            button.Text = "Заказать";
            var firebaseClient = new FirebaseClient("https://projectsd-4fe85-default-rtdb.firebaseio.com/");
            if (entrance.email != null)
            {
                string mail1 = entrance.email.ToString();
                mail = mail1.Replace(".", "");
               
            }
            else
            {
                string mail1 = registr.email.ToString();
                mail = mail1.Replace(".", "");
                
                
            }

           
            var child = firebaseClient
                .Child("Basket")
                .Child(mail)
                .Child(selectedProduct.Название);



            await child
                .PutAsync(selectedProduct);
        }

        private void OrderButton_Clicked(object sender, EventArgs e)
        {
            Products = "Basket";
            LoadDataIntoListViewFororzina();
        }

        private void Button1_Clicked(object sender, EventArgs e)
        {
            Products = "Pizza";
            LoadDataIntoListView();
        }

        private void Button2_Clicked(object sender, EventArgs e)
        {
            Products = "Drink";
            LoadDataIntoListView();
        }

        private void Button3_Clicked(object sender, EventArgs e)
        {
            Products = "Sushi";
            LoadDataIntoListView();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var Button = (Button)sender;
            var item = (Pizzas)Button.BindingContext;
            int Index = mail.IndexOf('@');
            bool answer = await DisplayAlert("Подтверждение", "Вы уверены, что хотите отменить этот заказ?", "Да", "Нет");
            if (mail.Contains("ru") && Index < mail.IndexOf("ru"))
            {
                mail = mail.Replace("ru", ".ru");
            }
            else if (mail.Contains("com") && Index < mail.IndexOf("com"))
            {
                mail = mail.Replace("com", ".com");
            }

            var firebaseClient = new FirebaseClient("https://projectsd-4fe85-default-rtdb.firebaseio.com/");
            var toDeleteItem = (from Basket in tovar_list
                                where Basket.Название == item.Название && Basket.Цена == item.Цена
                                select Basket).FirstOrDefault();

            if (toDeleteItem != null)
            {
                int atIndex = mail.IndexOf('@');
                if (mail.Contains("ru") && atIndex < mail.IndexOf("ru"))
                {
                    mail = mail.Replace(".ru", "ru");
                }
                else if (mail.Contains("com") && atIndex < mail.IndexOf("com"))
                {
                    mail = mail.Replace(".com", "com");
                }
                var Snapshot = await firebaseClient
            .Child("Basket")
            .Child(mail.Replace(".", ""))
            .OnceAsync<Pizzas>();

                foreach (var ProductSnapshot in Snapshot)
                {
                    var Tovar = ProductSnapshot.Object;
                    if (Tovar.Название == item.Название && Tovar.Цена == item.Цена)
                    {

                        await firebaseClient
                            .Child("Basket")
                            .Child(mail.Replace(".", ""))
                            .Child(ProductSnapshot.Key)
                            .DeleteAsync();


                        tovar_list.Remove(Tovar);


                        myListView.ItemsSource = null;
                        myListView.ItemsSource = tovar_list;


                        break;
                    }
                }







                tovar_list.Remove(toDeleteItem);


                myListView.ItemsSource = null;
                myListView.ItemsSource = tovar_list;
            }
        }
    }
    }
    