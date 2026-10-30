using Hangfire.Dashboard;

namespace CrescentSchool.Core.Enums.Jobs
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}
