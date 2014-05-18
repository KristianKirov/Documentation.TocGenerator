using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Documentation.TocGenerator
{
    public class GitHubFilesDownloader
    {
        private readonly RestClientWrapper _restClientWrapper;
        private readonly GitHubBranchInfoFetcher _gitHubInfoFetcher;

        private readonly string _repoOwner;
        private readonly string _repoName;
        private readonly string _repoBranch;

        public GitHubFilesDownloader(string repoOwner, string repoName, string repoBranch)
        {
            this._restClientWrapper = new RestClientWrapper();
            this._gitHubInfoFetcher = new GitHubBranchInfoFetcher(repoOwner, repoName, repoBranch, this._restClientWrapper);

            this._repoOwner = repoOwner;
            this._repoName = repoName;
            this._repoBranch = repoBranch;
        }

        public string DownloadRepositoryFiles(string zipName)
        {
            string SHA = _gitHubInfoFetcher.GetLatestCommitSHA();
            GetLatestRepositoryZipFile(SHA, zipName);

            return SHA;
        }

        private void GetLatestRepositoryZipFile(string SHA, string zipName)
        {
            byte[] zipData = GetRawZipData(SHA);
            if (zipData == null)
            {
                const int numberOfRetries = 4;
                for (int i = 1; i <= numberOfRetries; i++)
                {
                    Thread.Sleep(1000);
                    zipData = GetRawZipData(null);
                    if (zipData != null)
                    {
                        break;
                    }
                }
                if (zipData == null)
                {
                    throw new Exception("Could not download repo");
                }
            }

            string zipNameWithExtension = string.Concat(zipName, ".zip");
            if (File.Exists(zipNameWithExtension))
            {
                File.Delete("zipNameWithExtension");
            }

            File.WriteAllBytes(zipNameWithExtension, zipData);
        }

        private byte[] GetRawZipData(string SHA)
        {
            string url = string.Format("https://api.github.com/repos/{0}/{1}/zipball/{2}", this._repoOwner, this._repoName, SHA);
            IRestResponse restResponse = _restClientWrapper.GetRestResponse(url);

            return restResponse.RawBytes;
        }

    }
}
