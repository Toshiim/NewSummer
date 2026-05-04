using Hangfire.Dashboard;

namespace Presentation;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true; 
    }
}