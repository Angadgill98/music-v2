


using backend.api.repo;
using backend.api.services;

public class DashboardHandler
{
    Services services;
    Repo repos;
    public DashboardHandler(Services services,Repo repos)
    {
        this.services=services;
        this.repos=repos;
    }
}