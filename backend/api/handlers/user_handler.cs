



using backend.api.repo;
using backend.api.services;
using backend.api.services.filr_constructor;
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




    //musician

    public async Task<IResult> GetAlbums(Postgres_Context db, Guid musician_id)
    {
        var (albums, err) = await this.repos.album_repo.GetAlbumsByAuthor(db, musician_id);

        if (err != null)
            return CreateResponse(false, 500, "Failed to get albums");

        return CreateResponse(true, 200, "Albums retrieved successfully", albums);
    }

    public async Task<IResult> GetSongs(Postgres_Context db, Guid musician_id)
    {
        var (songs, err) = await this.repos.song_repo.GetSongsByAuthor(db, musician_id);

        if (err != null)
            return CreateResponse(false, 500, "Failed to get songs");

        return CreateResponse(true, 200, "Songs retrieved successfully", songs);
    }

    public async Task<IResult> CreateAlbum(Postgres_Context db, Guid musician_id, string album_name)
    {
        var (album_id, err) = await this.repos.album_repo.CreateAlbum(db, album_name, musician_id);

        if (err != null || album_id == null)
            return CreateResponse(false, 500, "Failed to create album");

        var (musician, musicianErr) = await this.repos.musician_repo.GetMusicianById(db, musician_id);

        if (musicianErr != null)
            return CreateResponse(false, 500, "Failed to get musician");

        if (musician == null)
            return CreateResponse(false, 404, "Musician not found");

        var (registered, registerErr) = await this.repos.musician_repo.RegisterAlbum(db, musician, album_id.Value);

        if (registerErr != null || registered != true)
            return CreateResponse(false, 500, "Failed to register album");

        return CreateResponse(true, 201, "Album created successfully", album_id);
    }

    public async Task<IResult> ChangeVisibilitySong(Postgres_Context db, Guid musician_id, Guid song_id, string visibility)
    {
        if (visibility != "public" && visibility != "private")
            return CreateResponse(false, 400, "Visibility must be public or private");

        var (song, err) = await this.repos.song_repo.GetSongById(db, song_id);

        if (err != null)
            return CreateResponse(false, 500, "Failed to get song");

        if (song == null)
            return CreateResponse(false, 404, "Song not found");

        if (song.musician_id != musician_id)
            return CreateResponse(false, 403, "You do not own this song");

        var (changed, changeErr) = await this.repos.song_repo.ChangeVisibilityofSong(db, song, visibility);

        if (changeErr != null || changed != true)
            return CreateResponse(false, 500, "Failed to change song visibility");

        return CreateResponse(true, 200, "Song visibility changed successfully", song_id);
    }

    public async Task<IResult> ChangeVisibilityAlbum(Postgres_Context db, Guid musician_id, Guid album_id, string visibility)
    {
        if (visibility != "public" && visibility != "private")
            return CreateResponse(false, 400, "Visibility must be public or private");

        var (album, err) = await this.repos.album_repo.GetAlbumById(db, album_id);

        if (err != null)
            return CreateResponse(false, 500, "Failed to get album");

        if (album == null)
            return CreateResponse(false, 404, "Album not found");

        var (musician, musicianErr) = await this.repos.musician_repo.GetMusicianById(db, musician_id);

        if (musicianErr != null)
            return CreateResponse(false, 500, "Failed to get musician");

        if (musician == null)
            return CreateResponse(false, 404, "Musician not found");

        if (!musician.albums.Contains(album_id))
            return CreateResponse(false, 403, "You do not own this album");

        var (changed, changeErr) = await this.repos.album_repo.ChangeVisibilityofAlbum(db, album, visibility);

        if (changeErr != null || changed != true)
            return CreateResponse(false, 500, "Failed to change album visibility");

        return CreateResponse(true, 200, "Album visibility changed successfully", album_id);
    }

    public async Task<IResult> AddSongToAlbum(Postgres_Context db, Guid musician_id, Guid album_id, Guid song_id)
    {
        var (album, err) = await this.repos.album_repo.GetAlbumById(db, album_id);

        if (err != null)
            return CreateResponse(false, 500, "Failed to get album");

        if (album == null)
            return CreateResponse(false, 404, "Album not found");

        var (musician, musicianErr) = await this.repos.musician_repo.GetMusicianById(db, musician_id);

        if (musicianErr != null)
            return CreateResponse(false, 500, "Failed to get musician");

        if (musician == null)
            return CreateResponse(false, 404, "Musician not found");

        if (!musician.albums.Contains(album_id))
            return CreateResponse(false, 403, "You do not own this album");

        var (song, songErr) = await this.repos.song_repo.GetSongById(db, song_id);

        if (songErr != null)
            return CreateResponse(false, 500, "Failed to get song");

        if (song == null)
            return CreateResponse(false, 404, "Song not found");

        if (song.musician_id != musician_id)
            return CreateResponse(false, 403, "You do not own this song");

        var (added, addErr) = await this.repos.album_repo.AddAlbumSong(db, album, song_id);

        if (addErr != null || added != true)
            return CreateResponse(false, 500, "Failed to add song to album");

        return CreateResponse(true, 200, "Song added to album successfully", song_id);
    }

    public async Task<IResult> SaveUploadContext(Postgres_Context db,UploadContext context)
    {
        this.services.upload_service.SaveUploadContext(context);
        return CreateResponse(
            true,
            StatusCodes.Status200OK,
            "Upload context saved successfully"
        );
    }

    public async Task HandleChunkContext(ChunkContext chunk)
    {
        
    }
}