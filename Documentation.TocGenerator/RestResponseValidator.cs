using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Documentation.TocGenerator
{
    public class RestResponseValidator
    {
        public void Validate(IRestResponse restResponse)
        {
            switch (restResponse.StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    throw new Exception("HTTP Forbidden received. It's possible the GitHub API rate limit has been exceeded.");
                case HttpStatusCode.Unauthorized:
                    throw new Exception("Bad Credentials");
                case HttpStatusCode.NotFound:
                    throw new Exception("Repository not found or given credentials not authorized to access respository.");
            }
        }
    }
}
