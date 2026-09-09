


using System.Threading.Tasks;
using backend.Database;

public class Albums_repo
{
    public Albums_repo()
    {
        
    }


    public async Task CreateAlbum(Postgres_Context db,string album_name)
    {
        Albums album=new();
        var album_id=Guid.NewGuid();
        album.album_name=album_name;
        album.album_id=album_id;
        album.likes=0;
        album.songs=[];
        
        try
        {
            db.AlbumsTable.Add(album);
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task AddAlbumSong(Postgres_Context db,Albums album,Guid song_id)
    {
        try
        {
            album.songs.Add(song_id);
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public void Addlike(Postgres_Context db,Albums album)
    {
        try
        {
            album.likes++;
            db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }

    }

    public async Task RemoveLike(Postgres_Context db,Albums album)
    {
        try
        {
            album.likes--;
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }

    }
}