using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.Module.API.Endpoints
{
    public class GetNicknameEndpoint : Endpoint
    {
        public delegate void OnNicknameReceived(string response);
        public OnNicknameReceived OnNicknameReceivedHandler;

        public override string url => "Nicknames/GetNickname";

        public async Task StartAsync(string id)
        {
            this.SetArgs(new List<Tuple<string, string>>
            {
                Tuple.Create("id", id)
            });
            this.OnResponseHandler += (string response) =>
            {
                this.OnNicknameReceivedHandler(response);
            };
            this.request.OnError = async (WebException exception) => {

            };

            await this.RequestAsync();
        }
    }
}
