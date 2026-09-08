


using backend.api.handlers;
using backend.api.repo;
using backend.api.services;

public class Handlers{
    
    public AuthHandler auth;
    Repo repo;

    public Handlers(Services services,Repo repos)
    {
        this.auth=new(services,repos);
        this.repo=repos;

    }
}