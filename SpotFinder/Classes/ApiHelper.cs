using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace SpotFinder.Classes
{
    public static class ApiHelper
    {
        private static HttpClient Client { get; set; }

        public static void InitializeClient()
        {
            //HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            Client = new HttpClient();
            Client.BaseAddress = new Uri("http://127.0.0.1:8000/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); //Only Json
            Client.DefaultRequestHeaders.Add("x-authorization", "2gIyuQlYBlVtm24wAQUCNuIIpEkROKRR4YO0Ib3Kr8FDurGXQYJHkAmbosk9GSdd");
        }

        //create - Post
        public static async Task<HttpResponseMessage> Post(string path, FormUrlEncodedContent content)
        {
            HttpResponseMessage response = await Client.PostAsync(path, content);

            return response;
        }

        //read - Get
        public static async Task<HttpResponseMessage> Get(string path)
        {
            HttpResponseMessage response = await Client.GetAsync(path);

            return response;
        }

        //update - Put
        public static async Task<HttpResponseMessage> Put(string path, StringContent content)
        {
            HttpResponseMessage response = await Client.PutAsync(path, content);

            return response;
        }

        //delete - Delete
        public static async Task<HttpResponseMessage> Delete(string path)
        {
            HttpResponseMessage response = await Client.DeleteAsync(path);

            return response;
        }
    }
}
