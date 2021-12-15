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
            Client = new HttpClient();
            Client.BaseAddress = new Uri("http://127.0.0.1:8000");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")); //Only Json
            Client.DefaultRequestHeaders.Add("x-authorization", "tnUK9vo1l7DxRwZR246yl7xCRKcs7ZxEpwYPyjuNJlowcnhxdqvjIjxVCWf75FVb");
        }

        //create - Post
        public static async Task<HttpResponseMessage> Post(string path, HttpContent content)
        {
            string apiUrl = Client.BaseAddress + path;
            Client.BaseAddress = new Uri(apiUrl);
            HttpResponseMessage response = await Client.PostAsync(apiUrl, content);

            return response;
        }

        //read - Get
        public static async Task<HttpResponseMessage> Get(string path)
        {
            string apiUrl = Client.BaseAddress + path;
            Client.BaseAddress = new Uri(apiUrl);
            HttpResponseMessage response = await Client.GetAsync(apiUrl);

            return response;
        }

        //update - Put
        public static async Task<HttpResponseMessage> Put(string path, HttpContent content)
        {
            string apiUrl = Client.BaseAddress + path;
            Client.BaseAddress = new Uri(apiUrl);
            HttpResponseMessage response = await Client.PutAsync(apiUrl, content);

            return response;
        }

        //delete - Delete
        public static async Task<HttpResponseMessage> Delete(string path)
        {
            string apiUrl = Client.BaseAddress + path;
            Client.BaseAddress = new Uri(apiUrl);
            HttpResponseMessage response = await Client.DeleteAsync(apiUrl);

            return response;
        }
    }
}
