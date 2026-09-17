import { HttpClient, HttpResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { InfoLogger } from "../loggers/logger";
import { Observable } from "rxjs";
import { ChunckContext, UploadContext } from "../services/Upload/upload";




export class Musician{
    url:string;
    http:HttpClient=inject(HttpClient);
    logger:InfoLogger=new InfoLogger(true);

    constructor(url:string,){
        let post_fix="/api/musician"
        this.url=url+post_fix;
    }


    Register(musician_name:string):Observable<HttpResponse<Object>>{
        let endpoint=this.url+"/musician-reg";
        this.logger.log("Endpoint is "+endpoint);

        let body={
            musician_name
        };

        this.logger.log("Body is "+JSON.stringify(body));

        return this.http.post(endpoint,body,{
            withCredentials:true,
            observe:'response'
        });
    }

    SendUploadContext(context: UploadContext): Observable<HttpResponse<Object>> {
        let endpoint = this.url + "/start-upload-context";

        this.logger.log("Endpoint is " + endpoint);
        this.logger.log("Upload Context: " + JSON.stringify(context));

        return this.http.post(endpoint, context, {
            withCredentials: true,
            observe: 'response'
        });
    }

    SendChunkContext(chunk_context:ChunckContext){
        let endpoint = this.url + "/chunk-context";

        this.logger.log("Endpoint is " + endpoint);

        let form = new FormData();

        form.append("upload_id", chunk_context.upload_id);
        form.append("chunk_id", chunk_context.chunk_id.toString());
        form.append("chunk_size", chunk_context.chunk_size.toString());

        form.append("data", chunk_context.data);

        form.append(
            "hash",
            new Blob([chunk_context.hash], {
                type: "application/octet-stream"
            })
        );

        return this.http.post(endpoint, form, {
            withCredentials: true,
            observe: "response"
        });
    }

    SendComplete(upload_context:UploadContext):Observable<HttpResponse<Object>>{
        let endpoint=this.url+"/complete-upload";

        this.logger.log("Endpoint is "+endpoint);
        this.logger.log("Upload Context: "+JSON.stringify(upload_context));

        return this.http.post(endpoint,upload_context,{
            withCredentials:true,
            observe:'response'
        });
    }

}