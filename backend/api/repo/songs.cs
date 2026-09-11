using System.Threading.Tasks;
using backend.Database;

public class Songs_repo
{
    public Songs_repo(){}

    public async Task<(Guid?, Exception?)> CreateSong(Postgres_Context db, string song_name, Guid musician_id)
    {
        Songs song = new();
        var song_id = Guid.NewGuid();
        song.song_name = song_name;
        song.song_id = song_id;
        song.musician_id = musician_id;
        song.other_singers = [];
        song.likes = 0;

        try
        {
            db.SongsTable.Add(song);
            await db.SaveChangesAsync();
            return (song_id, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Error while creating song, the error is\n{err}");
            return (null, err);
        }
    }

    public async Task<(Songs?, Exception?)> GetSongById(Postgres_Context db, Guid song_id)
    {
        try
        {
            var song = await db.SongsTable.FindAsync(song_id);

            if (song == null)
            {
                return (null, null);
            }

            return (song, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to get song, the error is\n{err}");
            return (null, err);
        }
    }

    public async Task<(bool?, Exception?)> AddAdditionalSingers(Postgres_Context db, Songs song, Guid musician_id)
    {
        try
        {
            song.other_singers.Add(musician_id);
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to add additional singer, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> Addlike(Postgres_Context db, Songs song)
    {
        try
        {
            song.likes++;
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to like a song, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(bool?, Exception?)> RemoveLike(Postgres_Context db, Songs song)
    {
        try
        {
            song.likes--;
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to remove a like from song, the error is\n{err}");
            return (false, err);
        }
    }
}