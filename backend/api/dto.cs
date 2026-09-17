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