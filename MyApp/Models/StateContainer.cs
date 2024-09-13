namespace MyApp.Models
{
    // The following class was adapted from the microsoft docs
    // Authors: guardrex, Rick-Anderson, ctrl-alt-d, rynowak
    // Link: https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management?view=aspnetcore-8.0&pivots=server
    internal class StateContainer
    {
        List<Issue> _issues = new List<Issue>();

        public List<Issue> Issues
        {
            get => Repository.GetIssues();
            set
            {
                // "value" isn't used as the Issues state should be 
                // derived from the repository (db)
                _issues = Repository.GetIssues();
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
