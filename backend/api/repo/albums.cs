


using System.Data.SqlTypes;
using System.Threading.Tasks;
using backend.Database;

public class Albums_repo
{
    public Albums_repo()
    {
        
    }


    public async Task<(Guid?,Exception?)> CreateAlbum(Postgres_Context db,string album_name)
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
            return (album_id,null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Error while creating a album,the error is \n {err}");
            return (null,err);
        }
    }

    public async Task<(bool?,Exception?)> AddAlbumSong(Postgres_Context db,Albums album,Guid song_id)
    {
        try
        {
            album.songs.Add(song_id);
            await db.SaveChangesAsync();
            return (true,null);
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to add song to album ,the error is \n{err}");            
            return (false,err);
        }
    }

    public async Task<(bool,Exception?)> Addlike(Postgres_Context db,Albums album)
    {
        try
        {
            album.likes++;
            await db.SaveChangesAsync();
            return (true,null);            
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to like a album, the error is \n {err}");
            return (false,err);            
        }

    }

    public async Task<(bool,Exception?)> RemoveLike(Postgres_Context db,Albums album)
    {
        try
        {
            album.likes--;
            await db.SaveChangesAsync();
            return (true,null);            
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to remove a like from album, the error is \n {err}");
            return (false,err);            
        }

    }
}