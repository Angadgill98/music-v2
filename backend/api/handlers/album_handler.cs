


using backend.api.repo;
using backend.api.services;
using backend.Database;

public class AlbumHandler
{
    Services services;
    Repo repos;
    public AlbumHandler(Services services,Repo repos)
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

    //albums
    public async Task<IResult> GetAlbum(Postgres_Context db,Guid user_id ,Guid album_id)
    {
        

        var (album, albumErr) = await this.repos.album_repo.GetAlbumById(db, album_id);

        if (albumErr != null)
        {
            return CreateResponse(false, 500, "Failed to get album");
        }

        if (album == null)
        {
            return CreateResponse(false, 404, "Album not found");
        }

        return CreateResponse(
            true,
            200,
            "Album retrieved successfully",
            album
        );
    }

    public async Task<IResult> RemoveLikeAlbum(Postgres_Context db, Guid user_id, Guid album_id)
    {
        var (user, userErr) = await this.repos.user_repo.GetUserById(db, user_id);
        if (userErr != null)
        {
            return CreateResponse(false, 500, "Failed to get user");
        }

        if (user == null)
        {
            return CreateResponse(false, 404, "User not found");
        }

        var (liked, likeErr) = await this.repos.user_repo.LikeAlbum(db, album_id, user);
        if (likeErr != null || liked != true)
        {
            return CreateResponse(false, 500, "Failed to add album to liked albums");
        }

        var (album, albumErr) = await this.repos.album_repo.GetAlbumById(db, album_id);
        if (albumErr != null)
        {
            return CreateResponse(false, 500, "Failed to get album");
        }
        if (album == null)
        {
            return CreateResponse(false, 404, "Album not found");
        }

        var (removed, removeErr) = await this.repos.album_repo.RemoveLike(db, album);
        if (removeErr != null || removed != true)
        {
            return CreateResponse(false, 500, "Failed to remove album like");
        }


        return CreateResponse(
            true,
            200,
            "Album like removed successfully",
            album_id
        );
    }

    public async Task<IResult> LikeAlbum(Postgres_Context db, Guid user_id, Guid album_id)
    {

        var (user, userErr) = await this.repos.user_repo.GetUserById(db, user_id);

        if (userErr != null)
        {
            return CreateResponse(false, 500, "Failed to get user");
        }

        if (user == null)
        {
            return CreateResponse(false, 404, "User not found");
        }

        var (liked, likeErr) = await this.repos.user_repo.LikeAlbum(db, album_id, user);

        if (likeErr != null || liked != true)
        {
            return CreateResponse(false, 500, "Failed to add album to liked albums");
        }

        var (album, albumErr) = await this.repos.album_repo.GetAlbumById(db, album_id);

        if (albumErr != null)
        {
            return CreateResponse(false, 500, "Failed to get album");
        }

        if (album == null)
        {
            return CreateResponse(false, 404, "Album not found");
        }

        var (added, addErr) = await this.repos.album_repo.Addlike(db, album);

        if (addErr != null || added != true)
        {
            return CreateResponse(false, 500, "Failed to add album like");
        }

        return CreateResponse(
            true,
            200,
            "Album liked successfully",
            album_id
        );
    }


}