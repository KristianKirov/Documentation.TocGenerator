using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator
{
    public class RestClientInitializer
    {
        public RestClient CreateClient()
        {
            return new RestClient();

        }

        // HACK: Not performing authenticator setup in CreateClient due to an issue with RestSharp.
        // https://github.com/restsharp/RestSharp/issues/453
        private string _oAuthToken;
        public string OAuthToken
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_oAuthToken))
                {
                    Console.WriteLine("AccessToken: ");
                    string accessToken = Console.ReadLine();
                    
                    this._oAuthToken = accessToken;
                }
                return _oAuthToken;
            }
        }
    }
}
