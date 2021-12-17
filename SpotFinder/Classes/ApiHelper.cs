﻿using System;
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
        public static async Task<HttpResponseMessage> Put(string path, HttpContent content)
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
