using Git.Data;
using Git.Data.Models;
using Git.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Services
{
    public class RepositoriesService : IRepositoriesService
    {
        private readonly ApplicationDbContext context;

        public RepositoriesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string CreateRepository(string name, bool isPublic, string ownerId)
        {
            var repositoryToAdd = new Repository
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                CreatedOn = DateTime.UtcNow,
                IsPublic = isPublic,
                OwnerId = ownerId
            };

            this.context.Repositories.Add(repositoryToAdd);
            context.SaveChanges();

            return repositoryToAdd.Id;
        }

        public IEnumerable<RepositoryViewModel> GetPublicRepositories()
        {
            return this.context.Repositories
                .Where(x => x.IsPublic == true)
                .Select(x => new RepositoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    CreatedOn = x.CreatedOn,
                    Owner = x.Owner.Username,
                    CommitsCount = x.Commits.Count
                })
                .ToList();
        }

        public RepositoryViewModel GetRepositoryById(string id)
        {
            return this.context.Repositories
                .Where(x => x.Id == id)
                .Select(x => new RepositoryViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Owner = x.Owner.Username,
                    CreatedOn = x.CreatedOn,
                    CommitsCount = x.Commits.Count
                })
                .FirstOrDefault();
        }
    }
}
