using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nicknames.Module.Client;
using Nicknames.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.Module.API.Endpoints
{
    public class SigninEndpoint : Endpoint
    {
        public delegate void OnSigninSuccessful();
        public OnSigninSuccessful OnSigninSuccessfulHandler;

        public override string url => "signin";

        public async Task StartAsync(string ticket = null, string token = null)
        {
            string id = new SteamRequest().GetSteamID();
            string name = new SteamRequest().GetSteamName();

            var fetchArgs = new List<Tuple<string, string>>
            {
                Tuple.Create("provider", "Steam"),
                Tuple.Create("name", name),
                Tuple.Create("id", id)
            };

            if (string.IsNullOrEmpty(ticket))
            {
                if (TokenManager.Get().HasStoredToken())
                {
                    fetchArgs.Add(new Tuple<string, string>("token", TokenManager.Get().GetToken()));
                }
                else
                {
                    return;
                }
            }
            else if (string.IsNullOrEmpty(token))
            {
                fetchArgs.Add(new Tuple<string, string>("proof", ticket));
            }
            else
            {
                return;
            }

            this.SetArgs(fetchArgs);
            this.OnResponseHandler += (string response) =>
            {
                if (!string.IsNullOrEmpty(response))
                {
                    dynamic data = JObject.Parse(response);

                    if (data.token != null)
                        TokenManager.Get().StoreToken(data.token);
                }

                this.OnSigninSuccessfulHandler();
            };
            this.request.OnError = async (WebException exception) => {
                if (!string.IsNullOrEmpty(exception.Message))
                {
                    var wrapper = JsonConvert.DeserializeObject<AuthResponseWrapper>(exception.Message);
                    if (wrapper != null && wrapper.Response != null)
                    {
                        Console.WriteLine($"Authentication failed. Message: {wrapper.Response}");

                        if (wrapper.Response == AuthResponse.TokenExpired)
                        {
                            await this.StartAsync(ticket: await new SteamRequest().GenerateSteamTicket());
                        }
                    }
                }
            };

            await this.RequestAsync();
        }
    }
}
