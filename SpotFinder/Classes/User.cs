using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SpotFinder.Classes
{
    public class User
    {
        public User()
        {

        }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public User(string name, string email, string password, string passwordConfirmation)
        {
            Name = name;
            Email = email;
            Password = password;
            PasswordConfirmation = passwordConfirmation;
        }

        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}
