

export interface Logger<T>{
  enabled: boolean;

  log(data:T): void;

}

export class InfoLogger implements Logger<String>{
    enabled: boolean;

    constructor(enabled:boolean){
        this.enabled=enabled;
    }

    log(data: String): void {
        if (this.enabled){
            console.log(data);
        }
    }
}