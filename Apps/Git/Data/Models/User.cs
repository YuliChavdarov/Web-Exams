using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Git.Data.Models
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            this.Repositories = new HashSet<Repository>();
            this.Commits = new HashSet<Commit>();
        }
        public virtual ICollection<Repository> Repositories { get; set; }
        public virtual ICollection<Commit> Commits { get; set; }
    }
}
