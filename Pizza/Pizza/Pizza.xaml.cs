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

namespace Pizza
{
    
    public partial class Pizza : ContentPage
    {
        private string Products;
        private string mail;
    
        public Pizza()
        {
            InitializeComponent();
            Products = "Pizza";
            
            BindingContext = this;
            LoadDataIntoListView();


        }

        private void Order(object sender, EventArgs e)
        {
            // Navigation.PushModalAsync(new order_page());
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
            var tovar_list = new List<Pizzas>();
            tovar_list.Clear();
            
            foreach (var auto in tovars)
            {
                tovar_list.Add(auto.Object);
            }
            myListView.ItemsSource = tovar_list;
        }
        public async Task LoadDataIntoListViewFororzina()
        {
            var firebaseClient = new FirebaseClient("https://projectsd-4fe85-default-rtdb.firebaseio.com");
            var baskets = await firebaseClient
                .Child("Basket")
                .OnceAsync<Pizzas>();

            var tovar_list = new List<Pizzas>();

            foreach (var basket in baskets)
            {
                var products = await firebaseClient
                    .Child("Basket")
                    .Child(basket.Key) 
                    .OnceAsync<Pizzas>();

                foreach (var product in products)
                {
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
            public Pizzas() { 
                IsButtonVisible = true;
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
    }
    }