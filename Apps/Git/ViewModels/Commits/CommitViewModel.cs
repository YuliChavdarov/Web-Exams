﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Git.ViewModels.Commits
{
    public class CommitViewModel
    {
        public string Id { get; set; }
        public string CreatorId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RepositoryName { get; set; }
    }
}
