

import { Injectable, Service } from "@angular/core";
import { Upload } from "./Upload/upload";


@Injectable({
    providedIn:'root'
})
export class Services{

    upload:Upload;
    constructor (){
        this.upload=new Upload();
    }

   


}
