using System.Threading.Tasks;
using backend.Database;
using Microsoft.EntityFrameworkCore;

public class User_repo
{
    public User_repo()
    {
        
    }

    public async Task<(User?, Exception?)> GetUserById(Postgres_Context db, Guid id)
    {
        try
        {
            User? user = db.UsersTable.FirstOrDefault(user => user.user_id == id);
            return (user, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to find user using id {id} and error is\n{err}");
            return (null, err);
        }
    }

    public async Task<(bool?, Exception?)> RegisterAsMusician(Postgres_Context db, User user, Guid musician_id)
    {
        try
        {
            user.musician_id = musician_id;
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to register user as musician, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> RegisterPlaylist(Postgres_Context db, User user, Guid playlist_id)
    {
        try
        {
            user.playlists.Add(playlist_id);
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to register playlist in user, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> RemovePlaylist(Postgres_Context db,User user,Guid playlist_id)
    {
        try
        {
            user.playlists.Remove(playlist_id);
            await db.SaveChangesAsync();

            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to remove playlist, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> LikeSong(Postgres_Context db, Guid song_id, User user)
    {
        try
        {
            user.liked_songs.Add(song_id);
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to like song, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> RemoveSongLike(Postgres_Context db, Guid song_id, User user)
    {
        try
        {
            user.liked_songs.Remove(song_id);
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to remove song like, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> LikeAlbum(Postgres_Context db, Guid album_id, User user)
    {
        try
        {
            user.liked_albums.Add(album_id);
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to like album, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> RemoveAlbumLike(Postgres_Context db, Guid album_id, User user)
    {
        try
        {
            user.liked_albums.Remove(album_id);
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to remove album like, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(List<Playlists>?,Exception?)> GetAllPLaylist(Postgres_Context db,HashSet<Guid> playlists)
    {
        try
        {
            var playlists1=await db.PlaylistsTable
            .Where(playlist => playlists.Contains(playlist.playlist_id))
            .ToListAsync();
            return (playlists1,null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to get playlists1, the error is\n{err}");
            return (null, err);
        }
    }


    public async Task<(List<Songs>?,Exception?)> GetAllLikedSongs(Postgres_Context db,HashSet<Guid> songs_id)
    {
        try
        {
            var songs=await db.SongsTable
            .Where(song => songs_id.Contains(song.song_id))
            .ToListAsync();
            return (songs,null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to get liked songs, the error is\n{err}");
            return (null, err);
        }
    }

    public async Task<(List<Albums>?,Exception?)> GetAllLikedAlbums(Postgres_Context db,HashSet<Guid> albums_id)
    {
        try
        {
            var albums=await db.AlbumsTable
            .Where(album => albums_id.Contains(album.album_id))
            .ToListAsync();
            return (albums,null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to get liked Albums, the error is\n{err}");
            return (null, err);
        }
    }
}