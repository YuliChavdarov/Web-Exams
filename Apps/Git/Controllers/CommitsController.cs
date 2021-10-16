using Git.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly ICommitsService commitsService;
        private readonly IRepositoriesService repositoriesService;

        public CommitsController(ICommitsService commitsService, IRepositoriesService repositoriesService)
        {
            this.commitsService = commitsService;
            this.repositoriesService = repositoriesService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var commitViewModels = commitsService.GetCommitsByUserId(this.GetUserId());

            return this.View(commitViewModels);
        }

        public HttpResponse Delete(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var commit = commitsService.GetCommitById(id);

            if(commit == null)
            {
                return this.Error(GlobalConstants.CommitNotFoundError);
            }

            if(commit.CreatorId != this.GetUserId())
            {
                return this.Error(GlobalConstants.OnlyOwnerCanDeleteCommitError);
            }

            commitsService.DeleteCommitById(id);

            return this.Redirect("/Commits/All");
        }

        public HttpResponse Create(string id)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var repositoryViewModel = repositoriesService.GetRepositoryById(id);

            return this.View(repositoryViewModel);
        }

        [HttpPost]
        public HttpResponse Create(string id, string description)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (repositoriesService.GetRepositoryById(id) == null)
            {
                return this.Error(GlobalConstants.RepositoryNotFoundError);
            }

            if(description.Length < GlobalConstants.CommitDescriptionMinLength)
            {
                return this.Error(GlobalConstants.CommitDescriptionLengthError);
            }

            commitsService.CreateCommit(description, this.GetUserId(), id);

            return this.Redirect("/Repositories/All");
        }
    }
}
