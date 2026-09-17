


using System.Threading.Channels;
using System.Threading.Tasks;
using backend.api.services;
using backend.api.services.filr_constructor;

namespace backend.api.workers;


public class Upload_worker_dispatcher
{
    int workers_count=1;//here

    ChannelReader<Upload_worker_task> dispatcher_reader;
    ChannelWriter<Upload_worker_task> dispatcher_writer;
    List<ChannelWriter<Upload_worker_task>> workers=[];
    public Upload_worker_dispatcher(Services services)
    {   var channel=Channel.CreateUnbounded<Upload_worker_task>();
        this.dispatcher_reader=channel.Reader;
        this.dispatcher_writer=channel.Writer;
        
        this.dispatcher_reader = channel.Reader;
        this.dispatcher_writer = channel.Writer;

        foreach (var i in Enumerable.Range(0, this.workers_count))
        {
            var worker=new Upload_worker(dispatcher_reader, services);
            worker.StartWorker();
        }
    }

    public void StartDispatcher()
    {
        Task.Run(async ()=>{
            int current_worker = 0;

            await foreach (var task in dispatcher_reader.ReadAllAsync())
            {
                int worker = current_worker % workers_count;

                await workers[worker].WriteAsync(task);

                current_worker++;
            }
        });
    }

    public async Task Send(Upload_worker_task task)
    {

        await dispatcher_writer.WriteAsync(task);

    }
}


public class Upload_worker
{
    ChannelReader<Upload_worker_task> dispatcher_queue_reader;
    Services services;

    public Upload_worker(ChannelReader<Upload_worker_task> reader,Services services)
    {
        this.dispatcher_queue_reader=reader;
        this.services=services;
    }

    public void StartWorker()
    {
        Task.Run(async () =>
        {
            Console.WriteLine("WORKER STARTED");
            await foreach (var task in dispatcher_queue_reader.ReadAllAsync())
            {

                switch (task.is_context)
                {
                    case Allowed.upload_context:

                        await this.HandleUploadcontext(task);

                        break;
                    
                    case Allowed.chunk_context:

                        await this.HandleChunkContext(task);

                        break;
                    
                    default:

                        break;
                }
            }
        });
    }

    async Task HandleUploadcontext(Upload_worker_task task)
    {

        UploadContextReq context = (UploadContextReq)task.context;

        UploadContext upload_context = new UploadContext
        {
            upload_id = Guid.Parse(context.upload_id),
            song_name = context.song_name,
            file_name = context.file_name,
            file_size = context.file_size,
            file_type = context.file_type,
            total_chunks = context.total_chunks,
            chunks_ok = context.chunks_ok.ToList(),
            start = context.start,
            end = context.end
        };

        switch (task.operation)
        {
            case "new":
                this.services.upload_service.SaveUploadContext(upload_context);

                await task.response_sender.WriteAsync(true);

                break;
            
            case "complete":
                UploadContext saved_context =this.services.upload_service.GetUploadContext(upload_context.upload_id);

                List<int> missing_chunks =this.services.upload_service.CheckChunks(saved_context);
                
                if (missing_chunks.Count > 0)
                {
                    await task.response_sender.WriteAsync(false);
                    return;
                }

                this.services.upload_service.ReconstructOriginal(saved_context);
                this.services.upload_service.DeleteChunks(saved_context);


                
                await task.response_sender.WriteAsync(true);

                break;
            
            default:
                await task.response_sender.WriteAsync(false);

                break;
        }
    }

    async Task HandleChunkContext(Upload_worker_task task)
    {
        try
        {

            ChunkContextReq context = (ChunkContextReq)task.context;

            using var hash_stream = new MemoryStream();
            await context.hash.CopyToAsync(hash_stream);

            byte[] hash_bytes = hash_stream.ToArray();

            ChunkContext chunk_context = new ChunkContext
            {
                UploadId = Guid.Parse(context.upload_id),
                ChunkId = context.chunk_id,
                ChunkSize = context.chunk_size,
                Data = context.data,
                Hash = hash_bytes
            };

            bool valid = await this.services.upload_service.ValidateChunk(
                Convert.ToHexString(chunk_context.Hash),
                chunk_context.Data
            );

            if (!valid)
            {
                await task.response_sender.WriteAsync(false);
                return;
            }

            await this.services.upload_service.SaveChunk(
                chunk_context.Data,
                chunk_context.UploadId,
                chunk_context.ChunkId
            );

            this.services.upload_service.ChunkOk_UpdateUpload_ctx(
                chunk_context.UploadId,
                chunk_context.ChunkId
            );

            Console.WriteLine("Chunk handling completed");

            await task.response_sender.WriteAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex}");
            Console.WriteLine($"MESSAGE: {ex.Message}");
            Console.WriteLine($"STACK TRACE: {ex.StackTrace}");

            await task.response_sender.WriteAsync(false);
        }
    }

}



public enum Allowed
{
    upload_context,
    chunk_context
}

public record Upload_worker_task
{
    public Allowed is_context;
    public object context;
    public string operation;
    public ChannelWriter<bool> response_sender;
}
