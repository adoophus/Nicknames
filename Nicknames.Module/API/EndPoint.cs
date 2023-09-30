using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicknames.Module.API
{
    public class Endpoint
    {
        public string Url;

        public WebRequest request;

        public delegate void OnResponse(string response);
        public OnResponse OnResponseHandler;

        public string response;

        public virtual string url => null;

        public readonly static string host = "http://localhost:5002/";

        public Endpoint()
        {
            this.Prepare();
        }

        // Preparation of the web request so that it knows where to find the
        // endpoint.
        protected void Prepare()
        {
            this.request = new WebRequest();
            this.request.SetRequestURL(host + url);
        }

        // Gathering the information for the client to be sending to the server
        // including any auth tokens to verify steam account.
        public void Request()
        {
            string response = request.GetResponse();

            HandleResponse(response);
        }

        public async Task RequestAsync()
        {
            string response = await request.GetResponseAsync();

            HandleResponse(response);
        }

        private void HandleResponse(string response)
        {
            // We only want to handle it if the request was successful.
            if (response != null)
            {
                if (this.OnResponseHandler != null)
                {
                    this.OnResponseHandler(response);
                }

                // Sometimes we don't want to use a response handler.
                this.response = response;
            }
        }

        // Appends added arguments to the fetch request.
        public void SetArgs(List<Tuple<string, string>> args)
        {
            this.request.ApplyFetchArgs(args);
        }
    }
}
