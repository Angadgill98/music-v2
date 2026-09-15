


using System.Security.Cryptography;
using System.Threading.Tasks;

namespace backend.api.services.filr_constructor;


public class Upload_service
{

    public Dictionary<Guid, UploadContext> upload_context;

    public Upload_service()
    {
        
    }

    public void SaveUploadContext(UploadContext context)
    {
        this.upload_context.Add(context.upload_id,context);
    }

    public async Task<bool> ValidateChunk(string chunk_hash,IFormFile chunk_data)
    {
        using Stream input = chunk_data.OpenReadStream();
        byte[] hash = await SHA256.HashDataAsync(input);
        byte[] clientHash = Convert.FromHexString(chunk_hash);

        return CryptographicOperations.FixedTimeEquals(
            hash,
            clientHash
        );

    }
    
    public async Task<bool> SaveChunk(IFormFile chunk_data,Guid upload_id,int chunk_id)
    {
        var path="/uploads/"+upload_id+"_"+chunk_id;
        using (Stream input = chunk_data.OpenReadStream())
        using (FileStream output = File.Create(path))
        {
            await input.CopyToAsync(output);
        }

        return true;
    }

    public void ChunkOk_UpdateUpload_ctx(Guid upload_id,int chunk_id)
    {
        if (upload_context.TryGetValue(upload_id, out var context))
        {
            context.chunks_ok[chunk_id] = true;
        }
    }

    public UploadContext GetUploadContext(Guid upload_id)
    {
        var upload_context=this.upload_context[upload_id];
        return upload_context;
    }

    public List<int> CheckChunks(UploadContext context)
    {
        List<int> missing_chunks = new();

        for (int i = 0; i < context.chunks_ok.Count; i++)
        {
            if (!context.chunks_ok[i])
            {
                missing_chunks.Add(i);
            }
        }

        return missing_chunks;
    }

}


public class UploadContext
{
    public Guid upload_id { get; set; }
    public string song_name { get; set; } = string.Empty;
    public int file_size { get; set; }
    public string file_type { get; set; } = string.Empty;
    public int total_chunks { get; set; }
    public List<bool> chunks_ok { get; set; } = new();
    public int start { get; set; }
    public int end { get; set; }
}
public class ChunkContext
{
    public Guid UploadId { get; set; }

    public int ChunkId { get; set; }

    public long ChunkSize { get; set; }

    public IFormFile Data { get; set; } = null!;

    public byte[] Hash { get; set; } = null!;
}