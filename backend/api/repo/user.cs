


using System.Threading.Tasks;
using backend.Database;

public class User_repo
{
    public User_repo()
    {
        
    }

    public async Task<User?> GetUserById(Postgres_Context db,Guid id)
    {
        try
        {
            User? user=db.UsersTable.FirstOrDefault(user=>user.user_id==id);
            return user;
        }
        catch (System.Exception err)
        {
            Console.WriteLine($"Server_Exception: Failed to find user using id {id} and error is \n {err}");
            return null;
        }
    }

    public async Task RegisterAsMusician(Postgres_Context db,User user,Guid musician_id)
    {   
        try
        {
            user.musician_id=musician_id;
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }        
        
    }

    public async Task LikeSong(Postgres_Context db,Guid song_id,User user)
    {
        try
        {
            user.liked_songs.Add(song_id);
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task RemoveSongLike(Postgres_Context db,Guid song_id,User user)
    {
        try
        {
            user.liked_albums.Remove(song_id);   
            await db.SaveChangesAsync();
            
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task LikeAlbum(Postgres_Context db,Guid album_id,User user)
    {
        try
        {
            user.liked_albums.Add(album_id);
            await db.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task RemoveAlbumLike(Postgres_Context db,Guid album_id,User user)
    {
        try
        {
            user.liked_albums.Remove(album_id);   
            await db.SaveChangesAsync();

        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    


}