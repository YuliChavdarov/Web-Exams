using Git.ViewModels.Repositories;
using System.Collections.Generic;

namespace Git.Services
{
    public interface IRepositoriesService
    {
        public string CreateRepository(string name, bool isPublic, string ownerId);
        public IEnumerable<RepositoryViewModel> GetPublicRepositories();
        public RepositoryViewModel GetRepositoryById(string id);
    }
}