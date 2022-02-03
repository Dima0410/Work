using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Server.Services
{
    public interface IDataFromSNAHub
    {
        Task Start();
    }

    public class DataFromSNAHub : IDataFromSNAHub
    {
        static HubConnection hubConnection;

        public async Task Start()
        {
            await InitConnection();

            await hubConnection.StartAsync();
            Debug.WriteLine("Connection started!");

            //connection.On<Device>("GetDeviceByTag", device =>
            hubConnection.On<object>("GetAllDevices", device =>
            {
                //var _device = device;

                //var T_dv = _device.InputData.Analog.Find(x => x.Tagname == "engTemperature").Value;
                //Debug.WriteLine("\nDevice.Name = {0}, \n \nТемпература двигателя, T = {1} °C;", _device.Name, T_dv);
                //var F_dv = _device.InputData.Analog.Find(x => x.Tagname == "freq").Value;
                Debug.WriteLine("\nDevice.Name = {0}", device);

                var data = JsonConvert.SerializeObject(device);

                //var data2 = JsonConvert.DeserializeObject<List<Device>>(data);

            });
            
            await hubConnection.InvokeAsync("GetAllDevices");


        }

        private static async Task InitConnection()
        {
            var ServerURI = "http://88.81.213.35:5002/ioHub";
            AuthOptions auth = new AuthOptions("sub", "123456", "http://88.81.213.35:5002/");
            AccessToken _token = await GetToken(auth);
            var token = _token.Access_token;

            hubConnection = new HubConnectionBuilder()
               .WithUrl(ServerURI,
                      options => options.AccessTokenProvider = () => Task.FromResult(token))
               .AddNewtonsoftJsonProtocol()
               .Build();
        }


        public class AuthOptions
        {
            [JsonProperty("name")]
            public string Name = null;
            [JsonProperty("password")]
            public string Password = null;
            public string Uri = null;
            public string TokenEndPoint = "token";

            public AuthOptions(string name, string password, string uri)
            {
                Name = name;
                Password = password;
                Uri = uri;
            }
            public string GetTokenEndPoint()
            { return Uri + TokenEndPoint; }
        }
        public class AccessToken
        {
            [JsonProperty("access_token")]
            public string Access_token { get; set; }
            [JsonProperty("username")]
            public string Username { get; set; }
        }

        private static async Task<AccessToken> GetToken(AuthOptions auth)
        {
            using (var client = new HttpClient())
            {
                var jsonBody = JsonConvert.SerializeObject(auth);
                var response = await client.PostAsync(auth.GetTokenEndPoint(), new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<AccessToken>(content);
                return token;
            }
        }
    }
}
