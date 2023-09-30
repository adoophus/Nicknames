using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Nicknames.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.ClientTest;

public static class APIManager
{
    static string baseUrl = "https://localhost:5001";
    static string signInEndpoint = "/signin";
    static string provider = "Steam";

    public delegate void LoginHandler();
    public static event LoginHandler OnLoginEvent;

    public static async Task Login(string ticket = null, string token = null)
    {
        Console.WriteLine("Login");

        string id = SteamAuth.GetSteamID();
        string name = SteamAuth.GetSteamName();

        using (HttpClient httpClient = new HttpClient())
        {
            var queryString = String.Empty;

            if (string.IsNullOrEmpty(ticket))
            {
                // Prepare the query string
                if (HasStoredToken())
                {
                    queryString = $"?provider={provider}&name={name}&id={id}&token={GetToken()}";
                }
                else
                {
                    return;
                }
            }
            else if (string.IsNullOrEmpty(token))
            {
                queryString = $"?provider={provider}&name={name}&id={id}&proof={ticket}";
            }
            else
            {
                Console.WriteLine("Couldn't resolve how to login. No ticket or token provided.");

                return;
            }

            // Construct the full URL
            var fullUrl = baseUrl + signInEndpoint + queryString;

            Console.WriteLine("URL: " + fullUrl);

            // Send the GET request to the authentication endpoint
            HttpResponseMessage response = await httpClient.GetAsync(fullUrl);

            // Check the response status code
            if (response.IsSuccessStatusCode)
            {
                // Successful authentication
                Console.WriteLine("Login successful!");

                if (response.Content != null)
                {
                    // Parse the response content to check for the token
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        dynamic responseData = JObject.Parse(responseContent);

                        // Check if the response contains a "token" field
                        if (responseData.token != null)
                        {
                            string recvToken = responseData.token;
                            Console.WriteLine("Token received: " + recvToken);

                            StoreToken(recvToken);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No token received.");
                    }
                }

                OnLoginEvent?.Invoke();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(responseContent))
                {
                    var wrapper = JsonConvert.DeserializeObject<AuthResponseWrapper>(responseContent);
                    if (wrapper != null && wrapper.Response != null)
                    {
                        Console.WriteLine($"Authentication failed. Message: {wrapper.Response}");

                        if (wrapper.Response == AuthResponse.TokenExpired)
                        {
                            // Generate the auth ticket again with steam values since
                            // whatever we have on the client is out of date or expired.
                            GenerateSteamTicket();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Authentication failed. No response content.");
                }
            }
            else
            {
                // Handle other error cases
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }

    public static async void GenerateSteamTicket()
    {
        SteamAuth.OnAuthCallbackEvent += SteamAuth_OnAuthCallbackEvent;
        SteamAuth.GenerateSteamTicket();
    }

    public static void SteamAuth_OnAuthCallbackEvent(string sessionTicket)
    {
        _ = APIManager.Login(ticket: sessionTicket);
        SteamAuth.OnAuthCallbackEvent -= SteamAuth_OnAuthCallbackEvent;
    }

    public static async Task<string> GetNickname()
    {
        using (var httpClient = new HttpClient())
        {
            // Add authorization token if needed
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {APIManager.GetToken()}");

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/Nicknames/GetNickname");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        Console.WriteLine($"Error: {response.StatusCode} / Message: " + responseContent);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        return null;
    }

    public static async Task<bool> SetNickname(string newNickname)
    {
        using (var httpClient = new HttpClient())
        {
            // Add authorization token if needed
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {APIManager.GetToken()}");

            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(newNickname), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync($"{baseUrl}/Nicknames/SetNickname", content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Nickname updated successfully.");
                    return true;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        Console.WriteLine($"Error: {response.StatusCode} / Message: " + responseContent);
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        return false;
    }


    public static void StoreToken(string token)
    {
        RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\BannerlordNicknames");
        key.SetValue("AuthToken", token);
        key.Close();
    }


    public static bool HasStoredToken()
    {
        RegistryKey readKey = Registry.CurrentUser.OpenSubKey("Software\\BannerlordNicknames");
        string storedToken = readKey.GetValue("AuthToken") as string;
        readKey.Close();

        if (string.IsNullOrEmpty(storedToken))
            return false;
        return true;
    }

    public static string GetToken()
    {
        RegistryKey readKey = Registry.CurrentUser.OpenSubKey("Software\\BannerlordNicknames");
        string storedToken = readKey.GetValue("AuthToken") as string;
        readKey.Close();

        return storedToken;
    }
}