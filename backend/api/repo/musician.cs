



using System.Threading.Tasks;
using backend.Database;

public class Musician_repo
{
    public Musician_repo()
    {
        
    }

    public async Task CreateMusician(Postgres_Context db,string musician_name,Guid user_id)
    {   
        
        var musician_id=Guid.NewGuid();
        
        Musicians musicion=new();
        musicion.musician_name=musician_name;
        musicion.musician_id=musician_id;
        musicion.user_id=user_id;
        musicion.albums=[];
        musicion.songs=[];
        
        try
        {
            db.MusiciansTable.Add(musicion);
            await db.SaveChangesAsync(); 
            return;   
        }
        catch (System.Exception)
        {
            
            throw;
        }
        
    }

    public async Task RegisterSong(Postgres_Context db,Musicians musician,Guid song_id)
    {
        try
        {
            musician.songs.Add(song_id);
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public void GetMusicianById(Postgres_Context db,Guid musician_id)
    {
        try
        {
            Musicians? musician=db.MusiciansTable.FirstOrDefault(musician=>musician.musician_id==musician_id);
            if (musician == null)
            {
                
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task RegisterAlbum(Postgres_Context db,Musicians musician,Guid album_id)
    {
        try
        {
            musician.albums.Add(album_id);
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    
} 