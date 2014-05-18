using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Documentation.TocGenerator
{
    public class GitHubBranchInfoFetcher
    {
        private readonly RestClientWrapper _restClientWrapper;
        private readonly string _repoOwner;
        private readonly string _repoName;
        private readonly string _repoBranch;

        public GitHubBranchInfoFetcher(string repoOwner, string repoName, string repoBranch, RestClientWrapper restClientWrapper)
        {
            this._repoOwner = repoOwner;
            this._repoName = repoName;
            this._repoBranch = repoBranch;

            _restClientWrapper = restClientWrapper;
        }

        private BranchInfo _latestBranchInfo;
        public string GetLatestCommitSHA()
        {
            if (_latestBranchInfo == null)
            {
                string branchesUrl = string.Format("https://api.github.com/repos/{0}/{1}/branches", this._repoOwner, this._repoName);
                IRestResponse<List<BranchInfo>> restResponse = _restClientWrapper.GetRestResponse<List<BranchInfo>>(branchesUrl);
                List<BranchInfo> branches = restResponse.Data;
                BranchInfo currentBranchInfo = branches.FirstOrDefault(branch => branch.Name == this._repoBranch);
                if (currentBranchInfo == null)
                {
                    throw new Exception(string.Format("The branch name {0} does not exist on the remote repository {1}.",
                        this._repoBranch, this._repoName));
                }
                _latestBranchInfo = currentBranchInfo;
            }
            return _latestBranchInfo.Commit.SHA;
        }
    }
}
