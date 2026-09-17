

using System.Threading.Channels;
using backend.api.services;

namespace backend.api.workers;
public class Workers
{
    
    public Upload_worker_dispatcher upload_dispatcher;

    public Workers(Services services)
    {
        {
            Upload_worker_dispatcher upload_dispathcer=new(services);
            this.upload_dispatcher=upload_dispathcer;

            // this.upload_dispatcher.StartDispatcher();
        }
    }
}