public class MusicianRegReq
{
    public string musician_name { get; set; } = "";
}
public class UploadContextReq
{
    public string upload_id { get; set; } = "";
    public string song_name { get; set; } = "";
    public string file_name { get; set; } = "";
    public int file_size { get; set; }
    public string file_type { get; set; } = "";
    public int total_chunks { get; set; }
    public bool[] chunks_ok { get; set; } = [];
    public int start { get; set; }
    public int end { get; set; }
}

public class ChunkContextReq
{
    public string upload_id { get; set; } = "";
    public int chunk_id { get; set; }
    public int chunk_size { get; set; }
    public IFormFile data { get; set; } = null!;
    public IFormFile hash { get; set; } = null!;
}

public class CompleteUploadReq
{
    public string upload_id { get; set; } = "";
    public string song_name { get; set; } = "";
    public string file_name { get; set; } = "";
    public int file_size { get; set; }
    public string file_type { get; set; } = "";
    public int total_chunks { get; set; }
    public bool[] chunks_ok { get; set; } = [];
    public int start { get; set; }
    public int end { get; set; }
}










public class CreatePlaylistReq
{
    public string playlist_name { get; set; } = string.Empty;
}

public class DeletePlaylistReq
{
    public Guid playlist_id { get; set; }
}

public class SongLikeReq
{
    public Guid song_id { get; set; }
}

public class SongReq
{
    public Guid song_id { get; set; }
}

public class SongPlaylistReq
{
    public Guid song_id { get; set; }
    public Guid playlist_id { get; set; }
}

public class AlbumReq
{
    public Guid album_id { get; set; }
}

public class AlbumNameReq
{
    public string album_name { get; set; } = string.Empty;
}

public class SongVisibilityReq
{
    public Guid song_id { get; set; }
    public string visibility { get; set; } = string.Empty;
}

public class AlbumVisibilityReq
{
    public Guid album_id { get; set; }
    public string visibility { get; set; } = string.Empty;
}

public class AddSongToAlbumReq
{
    public Guid album_id { get; set; }
    public Guid song_id { get; set; }
}