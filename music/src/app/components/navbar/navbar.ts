import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  imports: [],
  selector: 'app-navbar',
  styleUrl: './navbar.scss',
  templateUrl: './navbar.html',
})
export class Navbar {
  router:Router=inject(Router)

  constructor(){}

  GotoProf(){
    this.router.navigate(['/profile']);
  }
}
