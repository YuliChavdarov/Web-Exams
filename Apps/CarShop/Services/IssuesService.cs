using CarShop.Data;
using CarShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarShop.Services
{
    public class IssuesService : IIssuesService
    {
        private readonly ApplicationDbContext context;

        public IssuesService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public string CreateIssue(string carId, string description)
        {
            var issue = new Issue
            {
                Id = Guid.NewGuid().ToString(),
                CarId = carId,
                Description = description,
                IsFixed = false
            };

            this.context.Issues.Add(issue);
            context.SaveChanges();

            return issue.Id;
        }

        public void DeleteIssue(string issueId)
        {
            var issue = this.context.Issues.FirstOrDefault(x => x.Id == issueId);

            if(issue == null)
            {
                return;
            }

            context.Issues.Remove(issue);
            context.SaveChanges();
        }

        public void FixIssue(string issueId)
        {
            var issue = this.context.Issues.FirstOrDefault(x => x.Id == issueId);

            if(issue == null)
            {
                return;
            }

            issue.IsFixed = true;
            context.SaveChanges();
        }
    }
}
