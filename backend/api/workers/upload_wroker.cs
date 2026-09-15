


using System.Threading.Channels;
using System.Threading.Tasks;
using backend.api.services;
using backend.api.services.filr_constructor;

namespace backend.api.workers;


public class Upload_worker_dispatcher
{
    int workers_count=3;//here

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
            new Upload_worker(dispatcher_reader, services);
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
            await foreach (var task in dispatcher_queue_reader.ReadAllAsync())
            {
                switch (task.is_context)
                {
                    case Allowed.upload_context:

                        break;
                    
                    case Allowed.chunk_context:

                        break;
                    
                    default:

                        break;
                }
            }
        });
    }

    void HandleUploadcontext(Upload_worker_task task)
    {
        UploadContext context = (UploadContext)task.context;
        switch (task.operation)
        {
            case "new":
                this.services.upload_service.SaveUploadContext(context);
                break;
            
            case "complete":
                UploadContext saved_context =this.services.upload_service.GetUploadContext(context.upload_id);

                List<int> missing_chunks =this.services.upload_service.CheckChunks(saved_context);
                
                break;
            
            default:
                break;
        }
    }

    async Task HandleChunkContext(Upload_worker_task task)
    {
        ChunkContext context = (ChunkContext)task.context;

        bool valid = await this.services.upload_service.ValidateChunk(Convert.ToHexString(context.Hash),context.Data);

        if (!valid)
        {
            return;
        }

        await this.services.upload_service.SaveChunk(
            context.Data,
            context.UploadId,
            context.ChunkId
        );

        this.services.upload_service.ChunkOk_UpdateUpload_ctx(
            context.UploadId,
            context.ChunkId
        );
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
    public ChannelWriter<object> response_sender;
}
