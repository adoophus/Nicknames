using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace Nicknames.Module.API
{
    public class WebRequest
    {
        public string Url { get; set; }

        public Action<WebException> OnError;

        public void ApplyFetchArgs(List<Tuple<string, string>> FetchData)
        {
            for (int i = 0; i < FetchData.Count; i++)
            {
                Tuple<string, string> FetchArg = FetchData[i];
                if (i == 0)
                {
                    Url = $"{Url}?{FetchArg.Item1}={FetchArg.Item2}";
                }
                else
                {
                    Url = $"{Url}&{FetchArg.Item1}={FetchArg.Item2}";
                }
            }
        }

        public void SetRequestURL(string Url)
        {
            this.Url = Url;
        }

        public string GetResponse()
        {
            try
            {
                WebClient client = new WebClient();
                string contents = client.DownloadString(Url);

                return contents;
            }
            catch (WebException ex)
            {
                if (OnError == null)
                {
                    this.DefaultError(ex);
                }
                else
                {
                    OnError(ex);
                }

                return null;
            }
        }

        public async Task<string> GetResponseAsync()
        {
            try
            {
                WebClient client = new WebClient();
                string contents = await client.DownloadStringTaskAsync(Url);

                return contents;
            }
            catch (WebException ex)
            {
                if (OnError == null)
                {
                    this.DefaultError(ex);
                }
                else
                {
                    OnError(ex);
                }

                return null;
            }
        }

        public void DefaultError(WebException ex)
        {
            var error = $"An error occurred while retrieving the API. Error: {ex.Message}";

            Console.WriteLine(error);
            InformationManager.DisplayMessage(new InformationMessage(error));
        }
    }
}
