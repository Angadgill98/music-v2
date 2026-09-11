


using System.Threading.Tasks;
using backend.Database;

public class Playlist_repo
{
    

    public Playlist_repo()
    {
        
    }


    public async Task<(Guid?, Exception?)> CreatePLaylist(Postgres_Context db,Guid user_id,string playlist_name)
    {
        var playlist_id = Guid.NewGuid();

        var playlist = new Playlists();
        playlist.name = playlist_name;
        playlist.playlist_id = playlist_id;
        playlist.user_id = user_id;
        playlist.songs = [];

        try
        {
            db.PlaylistsTable.Add(playlist);
            await db.SaveChangesAsync();

            return (playlist_id, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to create playlist, the error is\n{err}");
            return (null, err);
        }
    }

    public async Task<(Playlists?, Exception?)> GetPlaylistById(Postgres_Context db, Guid playlist_id)
    {
        try
        {
            var playlist = await db.PlaylistsTable.FindAsync(playlist_id);

            if (playlist == null)
            {
                return (null, null);
            }

            return (playlist, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to get playlist, the error is\n{err}");
            return (null, err);
        }
    }

    public async Task<(bool?, Exception?)> RemovePlaylist(Postgres_Context db,Playlists playlist)
    {
        try
        {
            
            db.PlaylistsTable.Remove(playlist);
            await db.SaveChangesAsync();

            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to remove playlist from teh table, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> AddToPlaylist(Postgres_Context db, Guid song_id, Playlists playlist)
    {
        try
        {
            playlist.songs.Add(song_id);
            await db.SaveChangesAsync();

            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to add song to playlist, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> RemoveFromPlaylist(Postgres_Context db, Guid song_id, Playlists playlist)
    {
        try
        {
            playlist.songs.Remove(song_id);
            await db.SaveChangesAsync();

            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to remove song from playlist, the error is\n{err}");
            return (false, err);
        }
    }
}