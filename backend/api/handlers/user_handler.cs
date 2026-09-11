



using backend.api.repo;
using backend.api.services;
using backend.Database;

public class UserHandler
{
    
    Services services;
    Repo repos;
    public UserHandler(Services services,Repo repos)
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

    public async Task<IResult> GetProfileUser(Postgres_Context db,Guid userid)
    {
        var (user,userErr)=await this.repos.user_repo.GetUserById(db,userid);
        if (userErr != null && user==null)
        {
            return CreateResponse(false, 500, "Failed to get user");
        }

        var (playlists, playlistErr) = await this.repos.user_repo.GetAllPLaylist(db, user.playlists);

        if (playlistErr != null)
        {
            playlists = [];
        }

        var (liked_songs,Songerr)=await this.repos.user_repo.GetAllLikedAlbums(db,user.liked_songs);
        if (Songerr == null)
        {
            liked_songs = [];
        }

        var (liked_albums,Albumerr)=await this.repos.user_repo.GetAllLikedAlbums(db,user.liked_albums);
        if (Albumerr == null)
        {
            liked_albums = [];
        }

        return CreateResponse(true,200,"Profile retrieved successfully",
            new
            {
                user,
                liked_songs,
                liked_albums,
                playlists
            }
        );

    }
    
    public async Task<IResult> GetProfileMusicianPrivate(Postgres_Context db,Guid musician_id)
    {
        var (musician,MusErr)=await this.repos.musician_repo.GetMusicianById(db,musician_id);
        if (MusErr != null && musician==null)
        {
            return CreateResponse(false, 500, "Failed to get musician");
        }

        var (songs, SongErr) = await this.repos.musician_repo.GetMusicianSongs(db, musician);

        if (SongErr != null)
        {
            songs = [];
        }

        var (albums, AlbumErr) = await this.repos.musician_repo.GetMusicianAlbums(db, musician);

        if (AlbumErr != null)
        {
            albums = [];
        }

        return CreateResponse(
            true,
            200,
            "Musician profile retrieved successfully",
            new
            {
                musician,
                songs,
                albums
            }
        );
        

    }
    
    public async Task<IResult> CreatePlaylist(Postgres_Context db,Guid user_id,string playlist_name)
    {
        var(playlist_id,playlistErr)=await this.repos.playlist_repo.CreatePLaylist(db,user_id,playlist_name);
        if (playlistErr != null || playlist_id == null)
        {
            return CreateResponse(false, 500, "Failed to create playlist");
        }

        var (user,userErr)= await this.repos.user_repo.GetUserById(db,user_id);
        if (userErr != null && user==null)
        {
            return CreateResponse(false, 500, "Failed to get user");
        }

        var (registered, registerErr) = await this.repos.user_repo.RegisterPlaylist(db,user,playlist_id.Value);
        if (registerErr != null || registered != true)
        {
            return CreateResponse(false, 500, "Failed to register playlist");
        }
        return CreateResponse(
            true,
            201,
            "Playlist created successfully",
            playlist_id
        );
        

    }

    public async Task<IResult> DeletePlaylist(Postgres_Context db,Guid user_id,Guid playlist_id)
    {
        var (user,userErr)= await this.repos.user_repo.GetUserById(db,user_id);
        if (userErr != null && user==null)
        {
            return CreateResponse(false, 500, "Failed to get user");
        }

        await this.repos.user_repo.RemovePlaylist(db,user,playlist_id);

        var(playlist,PLaylistErr)= await this.repos.playlist_repo.GetPlaylistById(db,playlist_id);

        await this.repos.playlist_repo.RemovePlaylist(db,playlist);
        
        return CreateResponse(
            true,
            201,
            "Playlist deleted successfully",
            playlist_id
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
     
    
}