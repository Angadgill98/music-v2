


using backend.api.repo;
using backend.api.services;
using backend.Database;

public class DashboardHandler
{
    Services services;
    Repo repos;
    public DashboardHandler(Services services,Repo repos)
    {
        this.services=services;
        this.repos=repos;
    }

    IResult CreateResponse(bool success,int statusCode,string message,object? data = null)
    {
        return Results.Json(
            new
            {
                success,
                message,
                data
            },
            statusCode: statusCode
        );
    }
    
    public async Task<IResult> GetSongsByCategory(Postgres_Context db, string category, int limit, int offset = 0)
    {
        var (songs, err) = await this.repos.song_repo.GetSongsByCategory(db, category, limit, offset);

        if (err != null)
        {
            return CreateResponse(false, 500, "Failed to get songs");
        }

        return CreateResponse(true, 200, "Songs fetched successfully", songs);
    }

    public async Task<IResult> GetSongsByMusician(Postgres_Context db, Guid musician_id, int limit, int offset = 0)
    {
        var (songs, err) = await this.repos.song_repo.GetSongsByMusician(db, musician_id, limit, offset);

        if (err != null)
        {
            return CreateResponse(false, 500, "Failed to get songs");
        }

        return CreateResponse(true, 200, "Songs fetched successfully", songs);
    }
}