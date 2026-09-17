import { Component, inject } from '@angular/core';
import { Navbar } from '../navbar/navbar';
import { Api } from '../../api/api';
import { InfoLogger } from '../../loggers/logger';
import { Musician } from './musician/musician';

@Component({
  imports: [Navbar, Musician],
  selector: 'app-profile',
  styleUrl: './profile.scss',
  templateUrl: './profile.html',
})
export class Profile {

  api=inject(Api);

  logger:InfoLogger=new InfoLogger(true);
  
  musician_profile=false







  
}
