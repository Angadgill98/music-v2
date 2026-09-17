import { Injectable } from "@angular/core";
import { Auth } from "./auth";
import { dev_environment } from "../../environments/environment.development";
import { Musician } from "./musician";


@Injectable({
    providedIn:'root'
})
export class Api{

    url:string=dev_environment.server_addr;

    auth:Auth;
    musician:Musician

    constructor(){
        this.auth=new Auth(this.url);
        this.musician=new Musician(this.url)
    }




}