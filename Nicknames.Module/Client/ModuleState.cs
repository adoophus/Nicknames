using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Nicknames.Module;
using Nicknames.Module.API.Endpoints;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nicknames.Module.API;
using System.Net;
using Nicknames.Shared.Entities;

namespace Nicknames.Module.Client
{
    public class ModuleState : IModuleState
    {
        private Client.HarmonyPatcher _patcher;

        private string _nickname;

        public string GetStoredNickname() => _nickname;

        public ModuleState()
        {
            _patcher = new Client.HarmonyPatcher();
        }

        public async Task Load()
        {
            SigninEndpoint endpoint = new SigninEndpoint();

            endpoint.OnSigninSuccessfulHandler += OnSignInComplete;

            if (TokenManager.Get().HasStoredToken())
            {
                await endpoint.StartAsync(token: TokenManager.Get().GetToken());
            }
            else
            {
                await endpoint.StartAsync(ticket: await new SteamRequest().GenerateSteamTicket());
            }
        }
        
        public async void OnSignInComplete()
        {
            await GetNicknameAsync();
        }

        public async Task GetNicknameAsync()
        {
            GetNicknameEndpoint endpoint = new GetNicknameEndpoint();
            endpoint.OnNicknameReceivedHandler += (string nickname) =>
            {
                _nickname = nickname;
            };

            await endpoint.StartAsync(new SteamRequest().GetSteamID());
        }

        public async Task SetNicknameAsync(string nickname) => await new SetNicknameEndpoint().StartAsync(nickname);

        IHarmonyPatcher IModuleState.GetPatcher() => _patcher;
    }
}
