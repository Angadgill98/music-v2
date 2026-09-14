import { Injectable, Service } from "@angular/core";


@Injectable({
    providedIn:'root'
})
class Upload{

    chunk_size:number;
    constructor (){
        //here
        //in bytes or in mb it is 10mb
        this.chunk_size=10485760;
    }

    async InitailizeUploadContext(file:File,song_name:string){
        let file_name=file.name;
        let file_size=file.size;
        let file_type=file.type;

        let total_chunk_size;
        let total_chunks = Math.ceil(file_size / this.chunk_size);

        let upload_id = crypto.randomUUID();

        let initialize_upload:UploadContext = {
            song_name,
            upload_id,
            file_name: file_name,
            file_size: file_size,
            file_type: file_type,
            total_chunks: total_chunks,
            chunks_ok: Array(total_chunks).fill(false),
            start:0,
            end:0,
        };

    }

    async StartUpload(file:File,context:UploadContext,start:number,end:number,chunk_id:number){
        //send the uploadcontext




        //create adn send the chunk context
        while(start<file.size){

            end=start+this.chunk_size;

            if(end>file.size){
                end=file.size;
            }``

            //create and send chunk here

            let chunk=await this.CreateChunkContext(file,start,end,chunk_id,context.upload_id)



            chunk_id++;
            start=end;
            context.start=start;
            context.end=end;
        }



        //send teh upload complete singal

    }

    async CreateChunkContext(file:File,start:number,end:number,chunk_id:number,upload_id:string):Promise<ChunckContext>{
        let chunk_data=file.slice(start,end);
        let data=await chunk_data.arrayBuffer();
        let hash = await crypto.subtle.digest("SHA-256", data);
        let chunk:ChunckContext={
            upload_id,
            chunk_id,
            chunk_size:chunk_data.size,
            hash,
            data:chunk_data
        }

        return chunk;
    }



}

interface UploadContext{
    upload_id:string
    song_name:String
    file_name: string;
    file_size: number;
    file_type: string;
    total_chunks: number;
    chunks_ok:boolean[];
    start:number,
    end:number
} 

interface ChunckContext{
    upload_id:string,
    chunk_id:number,
    chunk_size:number,
    data:Blob,
    hash:ArrayBuffer
}

