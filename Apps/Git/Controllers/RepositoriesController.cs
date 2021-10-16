using Git.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IRepositoriesService repositoriesService;

        public RepositoriesController(IRepositoriesService repositoriesService)
        {
            this.repositoriesService = repositoriesService;
        }

        public HttpResponse All()
        {
            var publicRepositories = repositoriesService.GetPublicRepositories();

            return this.View(publicRepositories);
        }

        public HttpResponse Create()
        {
            if(!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, string repositoryType)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if(name.Length < GlobalConstants.RepositoryNameMinLength || name.Length > GlobalConstants.RepositoryNameMaxLength)
            {
                return this.Error(GlobalConstants.RepositoryNameLengthError);
            }

            bool isPublic = false;

            if(repositoryType == "Public")
            {
                isPublic = true;
            }
            else if (repositoryType == "Private")
            {
                isPublic = false;
            }
            else
            {
                return this.Error("Invalid repository type.");
            }

            repositoriesService.CreateRepository(name, isPublic, this.GetUserId());

            return this.Redirect("/Repositories/All");
        }
    }
}
