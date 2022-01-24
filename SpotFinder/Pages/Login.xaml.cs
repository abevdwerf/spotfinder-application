using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Threading.Tasks;
using SpotFinder.Classes;
using System.Net.Http;

namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        public async Task SignIn()
        {
            try
            {
                User user = new User(tbUsername.Text, pbPassword.Password);

                string json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await ApiHelper.Post("api/login", content);

                if (((int)response.StatusCode) == 401)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<dynamic>(result);

                    string errorMessage = string.Join(",", jsonData["error"].Value ?? "");

                    tbError.Text = errorMessage;
                }
                else if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var jsonData = JsonConvert.DeserializeObject<dynamic>(result);

                    string token = jsonData["success"]["token"];

                    ApiHelper.Token = token;

                    MainWindow main = new MainWindow();
                    main.Username = jsonData["success"]["name"];
                    main.Show();

                    //sluit de login form af
                    Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            await SignIn();
        }

        private void runSignUp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Register register = new Register();
            register.Show();

            //sluit de login form af
            Close();
        }
    }
}
