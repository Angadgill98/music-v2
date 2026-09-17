import { Component, Inject, inject } from '@angular/core';
import { Api } from '../../api/api';
import { InfoLogger } from '../../loggers/logger';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  imports: [FormsModule],
  selector: 'app-auth',
  styleUrl: './auth.scss',
  templateUrl: './auth.html',
})
export class Auth {

  mode:boolean=false;

  name:string="";
  mail:string="";
  pass:string="";

  is_req_out:boolean=false;

  router:Router=inject(Router);
  logger:InfoLogger=new InfoLogger(true);

  api:Api=inject(Api);

  constructor(){

  }
  

  Signin(){

    if (this.mail === "" || this.pass === "") {
      return;
    }

    if (this.is_req_out==true) return;


    this.is_req_out=true;

  
    this.api.auth.SignIn(this.mail,this.pass).subscribe({
        next: (res) => {
            console.log("response is ", res);
            this.is_req_out = false;
            this.router.navigate(['/']);
        },

        error: (err) => {
            console.log("error is ", err);
            this.is_req_out = false;
        }
    });



  }

  SignUp(){
    if (this.name === "" || this.mail === "" || this.pass === "") {
      return;
    }

    if (this.is_req_out==true) return;

    this.is_req_out=true;


    this.api.auth.SignUp(this.name,this.mail,this.pass).subscribe({
        next: (res) => {
            console.log("response is ", res);
            this.is_req_out = false;
        },

        error: (err) => {
            console.log("error is ", err);
            this.is_req_out = false;
        }
    });


  }
}
