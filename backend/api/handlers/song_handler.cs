

using backend.api.repo;
using backend.api.services;
using backend.Database;

public class SongHandler
{
    Services services;
    Repo repos;
    public SongHandler(Services services,Repo repos)
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

     //songs

    // public async Task<IResult> GetSong(Postgres_Context db, Guid song_id)
    // {
        
    // }

    public async Task<IResult> AddToPlaylist(Postgres_Context db, Guid user_id, Guid song_id, Guid playlist_id)
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

        var (playlist, playlistErr) = await this.repos.playlist_repo.GetPlaylistById(db, playlist_id);

        if (playlistErr != null)
        {
            return CreateResponse(false, 500, "Failed to get playlist");
        }

        if (playlist == null)
        {
            return CreateResponse(false, 404, "Playlist not found");
        }

        if (playlist.user_id != user_id)
        {
            return CreateResponse(false, 403, "You do not own this playlist");
        }

        var (added, addErr) = await this.repos.playlist_repo.AddToPlaylist(db, song_id, playlist);

        if (addErr != null || added != true)
        {
            return CreateResponse(false, 500, "Failed to add song to playlist");
        }

        return CreateResponse(
            true,
            200,
            "Song added to playlist successfully",
            song_id
        );
    }

    public async Task<IResult> RemoveFromPlaylist(Postgres_Context db, Guid user_id, Guid song_id, Guid playlist_id)
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

        var (playlist, playlistErr) = await this.repos.playlist_repo.GetPlaylistById(db, playlist_id);

        if (playlistErr != null)
        {
            return CreateResponse(false, 500, "Failed to get playlist");
        }

        if (playlist == null)
        {
            return CreateResponse(false, 404, "Playlist not found");
        }

        if (playlist.user_id != user_id)
        {
            return CreateResponse(false, 403, "You do not own this playlist");
        }

        var (removed, removeErr) = await this.repos.playlist_repo.RemoveFromPlaylist(db, song_id, playlist);

        if (removeErr != null || removed != true)
        {
            return CreateResponse(false, 500, "Failed to remove song from playlist");
        }

        return CreateResponse(
            true,
            200,
            "Song removed from playlist successfully",
            song_id
        );
    }

    public async Task<IResult> RemoveLike(Postgres_Context db,Guid user_id,Guid song_id)
    {
        var (user,userErr)= await this.repos.user_repo.GetUserById(db,user_id);
        if (userErr != null || user==null)
        {
            return CreateResponse(false, 500, "Failed to get user");
        }

        var (removedFromUser, removeUserErr) = await this.repos.user_repo.RemoveSongLike(db, song_id, user);
        if (removeUserErr != null || removedFromUser != true)
        {
            return CreateResponse(false, 500, "Failed to remove song from liked songs");
        }

        var (song,SongErr)=await this.repos.song_repo.GetSongById(db,song_id);
        if (SongErr != null || song==null)
        {
            return CreateResponse(false, 500, "Failed to get song");
        }

        var (removedLike, removeLikeErr) = await this.repos.song_repo.RemoveLike(db, song);
        if (removeLikeErr != null || removedLike != true)
        {
            return CreateResponse(false, 500, "Failed to remove song like");
        }

        return CreateResponse(
            true,
            200,
            "Song like removed successfully",
            song_id
        );
        

    }
    
    public async Task<IResult> AddLike(Postgres_Context db, Guid user_id, Guid song_id)
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

        var (song, songErr) = await this.repos.song_repo.GetSongById(db, song_id);

        if (songErr != null)
        {
            return CreateResponse(false, 500, "Failed to get song");
        }

        if (song == null)
        {
            return CreateResponse(false, 404, "Song not found");
        }

        var (addedToUser, addUserErr) = await this.repos.user_repo.LikeSong(db, song_id, user);

        if (addUserErr != null || addedToUser != true)
        {
            return CreateResponse(false, 500, "Failed to add song to liked songs");
        }

        var (addedLike, addLikeErr) = await this.repos.song_repo.Addlike(db, song);

        if (addLikeErr != null || addedLike != true)
        {
            return CreateResponse(false, 500, "Failed to add song like");
        }

        return CreateResponse(
            true,
            200,
            "Song liked successfully",
            song_id
        );
    }

}