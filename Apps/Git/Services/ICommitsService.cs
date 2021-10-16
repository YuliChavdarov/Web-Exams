using Git.ViewModels.Commits;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Services
{
    public interface ICommitsService
    {
        public string CreateCommit(string description, string creatorId, string repositoryId);
        public IEnumerable<CommitViewModel> GetCommitsByUserId(string userId);
        public void DeleteCommitById(string id);
        public CommitViewModel GetCommitById(string id);
    }
}
