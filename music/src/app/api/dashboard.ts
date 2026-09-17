import { HttpClient, HttpResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { InfoLogger } from "../loggers/logger";
import { Observable } from "rxjs";
import { ChunckContext, UploadContext } from "../services/Upload/upload";




export class DashBoard{
    url:string;
    http:HttpClient=inject(HttpClient);
    logger:InfoLogger=new InfoLogger(true);

    constructor(url:string,){
        let post_fix="/api/dashboard"
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

    GetSongs(category:string,limit: number,offset:number){
        let endpoint=this.url+"/get-songs";
        this.logger.log("Endpoint is "+endpoint);

        return this.http.get(endpoint, {
            params: {
                category: category,
                limit: limit
            }
        });
    }

}