

using System.Threading.Tasks;
using backend.Database;

public class Musician_repo
{
    public Musician_repo()
    {
        
    }

    public async Task<(Guid?, Exception?)> CreateMusician(Postgres_Context db, string musician_name, Guid user_id)
    {   
        var musician_id = Guid.NewGuid();
        
        Musicians musicion = new();
        musicion.musician_name = musician_name;
        musicion.musician_id = musician_id;
        musicion.user_id = user_id;
        musicion.albums = [];
        musicion.songs = [];
        
        try
        {
            db.MusiciansTable.Add(musicion);
            await db.SaveChangesAsync();
            return (musician_id, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Error while creating musician, the error is\n{err}");
            return (null, err);
        }
    }

    public async Task<(bool?, Exception?)> RegisterSong(Postgres_Context db, Musicians musician, Guid song_id)
    {
        try
        {
            musician.songs.Add(song_id);
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to register song to musician, the error is\n{err}");
            return (false, err);
        }
    }

    public async Task<(Musicians?, Exception?)> GetMusicianById(Postgres_Context db, Guid musician_id)
    {
        try
        {
            Musicians? musician = db.MusiciansTable.FirstOrDefault(musician => musician.musician_id == musician_id);

            if (musician == null)
            {
                return (null, null);
            }

            return (musician, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to get musician, the error is\n{err}");
            return (null, err);
        }
    }

    public async Task<(bool?, Exception?)> RegisterAlbum(Postgres_Context db, Musicians musician, Guid album_id)
    {
        try
        {
            musician.albums.Add(album_id);
            await db.SaveChangesAsync();
            return (true, null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to register album to musician, the error is\n{err}");
            return (false, err);
        }
    }
}