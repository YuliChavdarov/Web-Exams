using Git.Data;
using Git.Data.Models;
using Git.ViewModels.Commits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Services
{
    public class CommitsService : ICommitsService
    {
        private readonly ApplicationDbContext context;

        public CommitsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string CreateCommit(string description, string creatorId, string repositoryId)
        {
            var commit = new Commit
            {
                Id = Guid.NewGuid().ToString(),
                Description = description,
                CreatedOn = DateTime.UtcNow,
                CreatorId = creatorId,
                RepositoryId = repositoryId
            };

            context.Commits.Add(commit);
            context.SaveChanges();

            return commit.Id;
        }

        public IEnumerable<CommitViewModel> GetCommitsByUserId(string userId)
        {
            return this.context.Commits
                .Where(x => x.CreatorId == userId)
                .Select(x => new CommitViewModel
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    Description = x.Description,
                    RepositoryName = x.Repository.Name
                })
                .ToList();
        }

        public void DeleteCommitById(string id)
        {
            var commitToDelete = context.Commits.FirstOrDefault(x => x.Id == id);
            if(commitToDelete != null)
            {
                context.Commits.Remove(commitToDelete);
            }

            context.SaveChanges();
        }

        public CommitViewModel GetCommitById(string id)
        {
            return this.context.Commits
                .Where(x => x.Id == id)
                .Select(x => new CommitViewModel
                {
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    CreatorId = x.CreatorId,
                    Description = x.Description,
                    RepositoryName = x.Repository.Name
                })
                .FirstOrDefault();
        }
    }
}
