namespace CarShop.Services
{
    public interface IIssuesService
    {
        public string CreateIssue(string carId, string description);
        public void DeleteIssue(string issueId);
        public void FixIssue(string issueId);
    }
}