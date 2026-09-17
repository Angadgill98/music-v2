


using backend.api.repo;
using backend.api.services;
using backend.api.services.filr_constructor;
using backend.Database;

public class MusicainHandler
{
    Services services;
    Repo repos;
    public MusicainHandler(Services services,Repo repos)
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


    //musician

    public async Task<IResult> NewMusicianReg(Postgres_Context db, Guid user_id, string musician_name)
    {
        var (musician_id, err) = await this.repos.musician_repo.CreateMusician(db, musician_name, user_id);

        if (err != null || musician_id == null)
        {
            return CreateResponse(false, 500, "Failed to create musician");
        }

        var (user, UserErr) = await this.repos.user_repo.GetUserById(db, user_id);

        if (UserErr != null || user == null)
        {
            return CreateResponse(false, 404, "User not found");
        }

        var (ok, registerErr) = await this.repos.user_repo.RegisterAsMusician(db, user, musician_id.Value);

        if (registerErr != null )
        {
            return CreateResponse(false, 500, "Failed to register as musician");
        }

        return CreateResponse(true, 200, "Successfully reg as a musician", musician_id);
    }
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