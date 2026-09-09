


using System.Threading.Tasks;
using backend.Database;

public class Songs_repo
{
    public Songs_repo(){}


    public async Task CreateSong(Postgres_Context db,string song_name,Guid musician_id)
    {
        Songs song=new();
        var song_id=Guid.NewGuid();
        song.song_name=song_name;
        song.song_id=song_id;
        song.musician_id=musician_id;
        song.other_singers=[];
        song.likes=0;

        try
        {
            db.SongsTable.Add(song);
            await db.SaveChangesAsync();    
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task AddAdditionalSingers(Postgres_Context db,Songs song,Guid musician_id)
    {
        try
        {
            song.other_singers.Add(musician_id);
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public void Addlike(Postgres_Context db,Songs song)
    {
        try
        {
            song.likes++;
            db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }

    }

    public async Task RemoveLike(Postgres_Context db,Songs song)
    {
        try
        {
            song.likes--;
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }

    }
}