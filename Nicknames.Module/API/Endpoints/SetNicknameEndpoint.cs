using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Nicknames.Module.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.Module.API.Endpoints
{
    public class SetNicknameEndpoint : Endpoint
    {
        public delegate void OnNicknameSet(string response);
        public OnNicknameSet OnNicknameSetHandler;

        public override string url => "Nicknames/SetNickname";

        public async Task StartAsync(string nickname)
        {
            var fetchArgs = new List<Tuple<string, string>>
            {
                Tuple.Create("nickname", nickname),
                Tuple.Create("token", TokenManager.Get().GetToken())
            };

            this.SetArgs(fetchArgs);
            this.OnResponseHandler += (string response) =>
            {
                this.OnNicknameSetHandler(response);
            };
            this.request.OnError = async (WebException exception) => {

            };

            await this.RequestAsync();
        }
    }
}
