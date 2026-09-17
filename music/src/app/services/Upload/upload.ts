import { Injectable, Service } from "@angular/core";
import { Api } from "../../api/api";
import { InfoLogger } from "../../loggers/logger";
import { firstValueFrom } from "rxjs";



export class Upload{

    chunk_size:number;
    logger:InfoLogger
    constructor (){
        //here
        //in bytes or in mb it is 10mb
        this.chunk_size=10485760;


        this.logger=new InfoLogger(true);
    }

    async InitailizeUploadContext(file:File,song_name:string):Promise<UploadContext>{
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

        return initialize_upload;

    }

    async StartUpload(file:File,context:UploadContext,api:Api):Promise<boolean>{
        //send the uploadcontext
        this.logger.log("Sending the Upload Context")
        let ok=await this.SendUploadContext(file,context,api);
        if (!ok) {
            this.logger.log("Failed to send Upload Context");
            return false;
        }

        this.logger.log("Upload Context sent successfully");

        

        let chunk_ok=await this.StartChunkContextChain(file,context,0,0,0,api);

        if(!chunk_ok){
            this.logger.log("Chunk upload failed");
            return false;
        }

        this.logger.log("All chunks uploaded successfully");

        //send teh upload complete singal

        this.logger.log("Sending teh complete signal");

        let response=await firstValueFrom(api.musician.SendComplete(context));

        if(!response.ok){
            this.logger.log("Upload completion failed");
            return false;
        }

        this.logger.log("Upload completed successfully");

        return true;
    }

    async SendUploadContext(file:File,context:UploadContext,api:Api):Promise<boolean>{
        try {
            let response = await firstValueFrom(api.musician.SendUploadContext(context));

            if (!response.ok) {
                this.logger.log("Failed to initialize the upload context")
                return false;
            }    
            return true;
        } catch (error) {
            this.logger.log("Error while tryin to intailze teh upload context Failed to start upload: "+error );
            return false;
        }
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

    async StartChunkContextChain(file:File,context:UploadContext,start:number,end:number,chunk_id:number,api:Api):Promise<boolean>{

        try {
            while(start<file.size){

                end=start+this.chunk_size;

                if(end>file.size){
                    end=file.size;
                }

                let chunk=await this.CreateChunkContext(file,start,end,chunk_id,context.upload_id);

                this.logger.log("Sending chunk "+chunk.chunk_id);

                let response=await firstValueFrom(api.musician.SendChunkContext(chunk));

                if(!response.ok){
                    this.logger.log("Chunk upload failed for chunk_id: "+chunk.chunk_id);
                    return false;
                }

                this.logger.log("Chunk "+chunk_id+" uploaded");

                chunk_id++;
                start=end;
                context.start=start;
                context.end=end;
            }

            return true;

        } catch(error) {
            this.logger.log("Error while uploading chunks");
            console.error(error);
            return false;
        }
    
    }


}

export interface UploadContext{
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

export interface ChunckContext{
    upload_id:string,
    chunk_id:number,
    chunk_size:number,
    data:Blob,
    hash:ArrayBuffer
}

