using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        public HttpResponse All()
        {
            return this.View();
        }

        public HttpResponse Create()
        {
            if(!this.IsUserSignedIn())
            {
                this.Redirect("/Users/Login");
            }

            return this.View();
        }
    }
}
