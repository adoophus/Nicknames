using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nicknames.Shared.Entities;
using System.Text.Json;

namespace Nicknames.Server.Auth;

public class SteamAuth : OIDHandler
{
    public SteamAuth() { }

    public async Task<AuthResult> CheckProof(IConfiguration config, string id, string proof)
    {
        using (var httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(config["auth:steam:baseUrl"]);

            var requestParameters = new Dictionary<string, string>
            {
                { "key", config["auth:steam:key"] },
                { "appid", config["auth:appid"]},
                { "ticket", proof }     
            };

            var queryString = new FormUrlEncodedContent(requestParameters);
            var response = await httpClient.GetAsync($"ISteamUserAuth/AuthenticateUserTicket/v1/?{await queryString.ReadAsStringAsync()}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var jsonResponse = JsonConvert.DeserializeObject<JObject>(responseContent);

                    if (jsonResponse.TryGetValue("response", out var responseObj) &&
                        responseObj is JObject responseObject &&
                        responseObject.TryGetValue("params", out var paramsObj) &&
                        paramsObj is JObject paramsObject)
                    {
                        if (paramsObject.TryGetValue("steamid", out var steamIdToken) &&
                            steamIdToken is JValue steamIdValue &&
                            ulong.TryParse(steamIdValue.ToString(), out ulong parsedSteamId))
                        {
                            if (parsedSteamId.ToString() == id)
                            {
                                return new AuthResult(Platform.Steam, true);
                            }
                        }
                    }
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    // Handle JSON parsing errors, if any.
                    Console.WriteLine("Error parsing JSON: " + ex.Message);
                }
            }
        }
        return new AuthResult(false);
    }
}
