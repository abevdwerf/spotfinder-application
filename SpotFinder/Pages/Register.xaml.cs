using SpotFinder.Classes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;


namespace SpotFinder.Pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        public async Task SignUp()
        { 
            User user = new User(tbUsername.Text, tbEmail.Text, pbPassword.Password, pbConfirmPassword.Password);
           
            string json = JsonConvert.SerializeObject(user);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await ApiHelper.Post("api/register", content);

            if (((int)response.StatusCode) == 401)
            {
                var result = await response.Content.ReadAsStringAsync();
                var jsonData = JsonConvert.DeserializeObject<dynamic>(result);

                string errorMessageName = string.Join(",", jsonData["error"]["name"] ?? "");
                string errorMessageEmail = string.Join(",", jsonData["error"]["email"] ?? "");
                string errorMessagePassword = string.Join(",", jsonData["error"]["password"] ?? "");
                string errorMessagePasswordConfirmation = string.Join(",", jsonData["error"]["PasswordConfirmation"] ?? "");

                tbErrorUsername.Text = errorMessageName;
                tbErrorEmail.Text = errorMessageEmail;
                tbErrorPassword.Text = errorMessagePassword;
                tbErrorPasswordConfirmation.Text = errorMessagePasswordConfirmation;
            }
            else if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var jsonData = JsonConvert.DeserializeObject<dynamic>(result);

                string token = jsonData["success"]["token"];

                ApiHelper.Token = token;

                MainWindow main = new MainWindow();
                main.Show();

                //sluit de register form af
                Close();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        private async void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (checkbServiceConditions.IsChecked == true)
            {
               tbErrorServiceConditions.Text = "";
               await SignUp();
            }
            else
            {
                tbErrorServiceConditions.Text = "The service conditions are required.";
            }
        }

        private void runLogin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            login.Show();

            //sluit de login form af
            Close();
        }
    }
}
