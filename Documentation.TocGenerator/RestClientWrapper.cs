using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator
{
    public class RestClientWrapper
    {
        private readonly RestClientInitializer _restClientInitializer;
        private readonly RestResponseValidator _restResponseValidator;

        private IRestClient _restClient;
        private IRestClient RestClient
        {
            get
            {
                if (_restClient == null)
                {
                    _restClient = _restClientInitializer.CreateClient();
                }
                return _restClient;
            }
        }

        public RestClientWrapper()
            : this(new RestClientInitializer(), new RestResponseValidator())
        { }

        public RestClientWrapper(RestClientInitializer restClientInitializer, RestResponseValidator restResponseValidator)
        {
            _restClientInitializer = restClientInitializer;
            _restResponseValidator = restResponseValidator;
        }

        public IRestResponse GetRestResponse(string requestUrl)
        {
            var restRequest = new RestRequest { Resource = requestUrl };
            return Execute(restRequest);
        }

        public IRestResponse<T> GetRestResponse<T>(string requestUrl)
            where T : class, new()
        {
            var restRequest = new RestRequest { Resource = requestUrl };
            return Execute<T>(restRequest);
        }

        public IRestResponse Execute(RestRequest request)
        {
            AppendOAuthTokenToRequest(request);
            IRestResponse restResponse = RestClient.Execute(request);
            _restResponseValidator.Validate(restResponse);
            return restResponse;
        }

        public IRestResponse<T> Execute<T>(RestRequest request)
            where T : class, new()
        {
            AppendOAuthTokenToRequest(request);
            IRestResponse<T> restResponse = RestClient.Execute<T>(request);
            _restResponseValidator.Validate(restResponse);
            return restResponse;
        }

        // HACK: RestSharp currently doesn't always append the token correctly, so we have to do it ourselves.
        private void AppendOAuthTokenToRequest(RestRequest request)
        {
            string queryParameterPrefix = "?";
            if (request.Resource.Contains("?"))
            {
                queryParameterPrefix = "&";
            }
            request.Resource += string.Format("{0}oauth_token={1}", queryParameterPrefix, _restClientInitializer.OAuthToken);
        }
    }
}
