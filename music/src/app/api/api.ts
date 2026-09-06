import { Injectable } from "@angular/core";
import { Auth } from "./auth";
import { dev_environment } from "../../environments/environment.development";


@Injectable({
    providedIn:'root'
})
export class Api{

    url:string=dev_environment.server_addr;



    constructor(){
        this.auth=new Auth(this.url);

    }



    auth:Auth;

}