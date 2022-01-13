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
            //var content = new FormUrlEncodedContent(new[]
            //{
            //    new KeyValuePair<string, string>("name", tbUsername.Text),
            //    new KeyValuePair<string, string>("email", tbEmail.Text),
            //    new KeyValuePair<string, string>("password", pbPassword.Password),
            //    new KeyValuePair<string, string>("password_confirmation", pbConfirmPassword.Password),
            //    new KeyValuePair<string, string>("service-conditions", (checkbServiceConditions.IsChecked == true) ? "1" : "")
            //});

            //HttpResponseMessage response = await ApiHelper.Post("api/register", content);

            //if (((int)response.StatusCode) == 422)
            //{
            //    var result = await response.Content.ReadAsStringAsync();
            //    var jsonData = JsonConvert.DeserializeObject<dynamic>(result);

            //    string errorMessageName = string.Join(",", jsonData["errors"]["name"] ?? "");
            //    string errorMessageEmail = string.Join(",", jsonData["errors"]["email"] ?? "");
            //    string errorMessagePassword = string.Join(",", jsonData["errors"]["password"] ?? "");
            //    string errorMessageServiceConditions = string.Join(",", jsonData["errors"]["service-conditions"] ?? "");

            //    tbErrorUsername.Text = errorMessageName;
            //    tbErrorEmail.Text = errorMessageEmail;
            //    tbErrorPassword.Text = errorMessagePassword;
            //    tbErrorServiceConditions.Text = errorMessageServiceConditions;

            //}
            //else if (response.IsSuccessStatusCode)
            //{
            //    Login login = new Login();
            //    login.Show();

            //    //sluit de register form af
            //    Close();
            //}
            //else
            //{
            //    throw new Exception(response.ReasonPhrase);
            //}
        }

        private async void btnCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            //await SignUp();   
        }
    }
}
