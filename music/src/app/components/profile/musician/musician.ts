import { Component, inject } from '@angular/core';
import { Api } from '../../../api/api';
import { InfoLogger } from '../../../loggers/logger';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Services } from '../../../services/service';
import { firstValueFrom } from 'rxjs';

@Component({
  imports: [FormsModule],
  selector: 'app-musician',
  styleUrl: './musician.scss',
  templateUrl: './musician.html',
})
export class Musician {

  api=inject(Api);

  logger:InfoLogger=new InfoLogger(true);
  
  router:Router=new Router();




  song_name: string = "";
  category: string = "";
  song_file: File | null = null;

  services=inject(Services)


  Regisgtee_as_Musician(){
    this.api.musician.Register("temp_musician_name").subscribe({
        next: (response) => {
            console.log("Musician registration successful");
            console.log("Response:", response);
            console.log("Relogin again");
            this.router.navigate(["/auth"])

        },
        error: (error) => {
            console.error("Musician registration failed");
            console.error("Error:", error);

        }
    });
  }





  onSongFileSelected(event: Event) {
      const input = event.target as HTMLInputElement;

      if (input.files && input.files.length > 0) {
          this.song_file = input.files[0];
      }
  }


  async UploadSong(){
    if (!this.song_name.trim()) {
        console.log("Song name is required");
        return;
    }

    if (!this.category.trim()) {
        console.log("Category is required");
        return;
    }

    if (this.song_file === null) {
        console.log("Song file is required");
        return;
    }

    let upload_context=await this.services.upload.InitailizeUploadContext(this.song_file,this.song_name)

    this.logger.log("The upload context is "+upload_context)

    let is_ok=await this.services.upload.StartUpload(this.song_file,upload_context,this.api)
    
    if(!is_ok){
        this.logger.log("Song upload failed");
        return;
    }

    this.logger.log("Song uploaded successfully");

  }
}
