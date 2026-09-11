
namespace backend.api.repo;

public class Repo
{
    public Auth_repo auth_repo=new();
    public User_repo user_repo=new();
    public Musician_repo musician_repo=new();
    public Songs_repo song_repo=new();
    public Playlist_repo playlist_repo=new();
    public Albums_repo album_repo=new();

    public Repo()
    {
        
    }    
}