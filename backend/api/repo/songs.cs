using System.Threading.Tasks;
using backend.Database;
using Microsoft.EntityFrameworkCore;

public class Songs_repo
{
    public Songs_repo(){}

    public async Task<(Guid?, Exception?)> CreateSong(Postgres_Context db, string file_name,string song_name, Guid musician_id)
    {
        Songs song = new();
        var song_id = Guid.NewGuid();
        song.file_name=file_name;
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

    public async Task<(List<Songs>?, Exception?)> GetSongsByAuthor(Postgres_Context db, Guid musician_id)
    {
        try
        {
            var songs = await db.SongsTable
                .Where(song => song.musician_id == musician_id)
                .ToListAsync();

            return (songs, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to get songs by author, the error is\n{err}");
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

    public async Task<(bool?, Exception?)> ChangeVisibilityofSong(Postgres_Context db, Songs song, string visibility)
    {
        try
        {
            song.visibility = visibility;

            await db.SaveChangesAsync();

            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to change song visibility, the error is\n{err}");
            return (false, err);
        }
    }


    public async Task<(List<Songs>?, Exception?)> GetSongsByCategory(Postgres_Context db,string category,int limit,int offset = 0)
    {
        try
        {
            var songs = await db.SongsTable
                .Where(song => song.category == category)
                .OrderBy(song => song.song_id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return (songs, null);
        }
        catch (Exception err)
        {
            Console.WriteLine(
                $"Server_Exception: Failed to get songs by category, the error is\n{err}"
            );

            return (null, err);
        }
    }


    public async Task<(List<Songs>?, Exception?)> GetSongsByMusician(Postgres_Context db,Guid musician_id,int limit,int offset = 0)
    {
        try
        {
            var songs = await db.SongsTable
                .Where(song => song.musician_id == musician_id)
                .OrderBy(song => song.song_id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return (songs, null);
        }
        catch (Exception err)
        {
            Console.WriteLine(
                $"Server_Exception: Failed to get songs by musician, the error is\n{err}"
            );

            return (null, err);
        }
    }
}