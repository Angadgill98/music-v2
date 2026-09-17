


using backend.api.handlers;
using backend.api.repo;
using backend.api.services;

public class Handlers{
    
    public AuthHandler auth;
    public UserHandler user;

    public AlbumHandler album;
    public DashboardHandler dashboard;
    public MusicainHandler musician;
    public SongHandler song;

    Repo repo;


    public Handlers(Services services,Repo repos)
    {
        this.auth=new(services,repos);
        this.repo=repos;
        this.user=new(services,repos);
        this.dashboard=new(services,repos);

        this.album=new(services,repos);

        this.musician=new(services,repos);

        this.song=new(services,repos);

        

    }
}