using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BaalPratibha.Logic
{
    public class Email
    {
        private string _baseUri = "https://api.mailgun.net/v3/baalpratibha.com.au";
        private string _apiKey = "key-6b0081b2374c3933e892c8b4623a5c6f";
        private readonly string _email;
        private readonly string _name;
        private readonly string _password;
        private readonly string _username;

        public Email(string email, string name, string username, string password)
        {
            _email = email;
            _name = name;
            _username = username;
            _password = password;
        }

        public async Task<HttpResponseMessage> SendMail()
        {

            using (var client = new HttpClient { BaseAddress = new Uri(_baseUri) })
            {

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes("api:" + _apiKey)));

                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("from", "donotreply@baalpratibha.com.au"),
            new KeyValuePair<string, string>("sender", "donotreply@baalpratibha.com.au"),
            new KeyValuePair<string, string>("to", _email),
            new KeyValuePair<string, string>("subject", "Baal Pratibha Login Details."),
            new KeyValuePair<string, string>("html", GetEmailBody())
        });

                return await client.PostAsync(_baseUri + "/messages", content).ConfigureAwait(false);
            }
        }

        private string GetEmailBody()
        {

            return "<h3>Congratulations " + _name + " on getting through the auditions and to the finals.</h3>" +
                   "<p>Welcome to Lahuri TV's NAYA presents Baal Pratibha Online Voting System. Please find your login details to the <a href='www.baalpratibha.com.au'>online voting system</a>. Please login using the details and update your profile.</p>" +
                   "<p>Find your profile at <a href='www.baalpratibha.com.au/" + _username + "'>www.baalpratibha.com.au/" + _username + "</a></p>" +
                   "<p>Login using: </p>" +
                   "<p><strong>UserName: " + _username +
                   "<br />" +
                   "Password: " + _password + "</strong></p>" +
                   "<p>Please update your profile with as much information as possible and also dont forget to upload a profile picture.</p>" +
                   "<p>If you face any issues feel free to contact Nabin Karki Thapa at 0449664244.</p>" +
                   "<p>All the best</p>" +
                   
                   "<p>Regards<br />" +
                   "Nabin Karki Thapa <br />" +
                   "<a href='www.facebook.com/lahuritv'>Lahuri TV</a></p>";
        }

    }
}
