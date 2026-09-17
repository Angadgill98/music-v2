

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public class Postgres_Context : DbContext
{
    public Postgres_Context(DbContextOptions<Postgres_Context> options): base(options){}

    public DbSet<User> UsersTable{ get; set; }

    public DbSet<Songs> SongsTable{ get; set; }

    public DbSet<Musicians> MusiciansTable{ get; set; }

    public DbSet<Albums> AlbumsTable{ get; set; }
    public DbSet<Playlists> PlaylistsTable { get; set; }
}



public class User
{
    [Key]
    public Guid user_id{ get; set; }

    public string user_name{ get; set; } = "";
    public string user_mail{ get; set; } = "";
    public string user_pass{ get; set; } = "";

    public Guid? musician_id { get; set; }

    public List<Guid> liked_songs{ get; set; }=[];

    public List<Guid> liked_albums{ get; set; }=[];

    public List<Guid> playlists {get; set; }=[];

    public User()
    {
        
    }

}

public class Playlists
{
    [Key]
    public Guid playlist_id { get; set; }

    public Guid user_id { get; set; }

    public string name { get; set; } = "";

    public List<Guid> songs { get; set; } = [];
}


public class Musicians
{
    [Key]
    public Guid musician_id{get;set;}

    public Guid user_id{get;set;}

    public string musician_name{ get; set;}="";

    public List<Guid> songs {get;set;}=[];
 
    public List<Guid> albums {get;set;}=[];



    public Musicians()
    {
        
    }
}




public class Songs
{
    [Key]
    public Guid song_id{get;set;}

    public string song_name{ get; set;}="";

    public string file_name{ get; set;}="";

    public string category{ get; set;}="";

    public Guid musician_id{get;set;}

    public List<Guid> other_singers {get;set;}=[];

    public int likes{get;set;}=0;

    public string visibility{ get; set;}="";

    public Songs() 
    {
        
    }
}




public class Albums
{
    [Key]
    public Guid album_id{get;set;}

    public string album_name{ get; set;}="";

    public List<Guid> songs {get;set;}=[];

    public Guid musician_id{get;set;}

    public int likes{get;set;}=0;

    public string visibility{ get; set;}="";

    public Albums()
    {
        
    }
}


