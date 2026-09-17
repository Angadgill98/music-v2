import { HttpClient, HttpResponse } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { InfoLogger } from "../loggers/logger";
import { Observable } from "rxjs";




export class Auth{
    url:string;
    http:HttpClient=inject(HttpClient);
    logger:InfoLogger=new InfoLogger(true);

    constructor(url:string,){
        let post_fix="/auth"
        this.url=url+post_fix;
    }


    SignIn(mail:string,pass:string):Observable<HttpResponse<Object>>{
        let endpoint=this.url+"/sign-in";
        this.logger.log("Endpoint is "+endpoint);
        let body={
            mail,
            pass
        };
        this.logger.log("Body is "+JSON.stringify(body));

        let options={
            withCredentials:true,
            observe:'response'
        };
        return this.http.post(endpoint,body,{
            withCredentials:true,
            observe:'response'
        });
    }


    SignUp(name:string,mail:string,pass:string):Observable<HttpResponse<Object>>{
        let endpoint=this.url+"/sign-up";
        this.logger.log("Endpoint is "+endpoint);

        let body={
            name,
            mail,
            pass
        };
        this.logger.log("Body is "+JSON.stringify(body));

        return this.http.post(endpoint,body,{
            withCredentials:true,
            observe:'response'
        });
    }

    IsTokenValid(): Observable<boolean> {
        const endpoint = this.url + "/is-valid";

        this.logger.log("Endpoint is " + endpoint);

        return this.http.get<boolean>(endpoint, {
            withCredentials: true
        });

    }

}