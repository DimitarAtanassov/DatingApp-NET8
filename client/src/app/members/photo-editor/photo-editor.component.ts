/*
Component for editing and uploading photos:
We want to display the user's images in this component b/c
We are giving User the ability to set a photo from their list as their MainImage (pfp),
We are also giving User the ability to delete an image from here, aswell 
*/
import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../_services/accounts.service';
import { environment } from '../../../environments/environment';
import { Photo } from '../../_models/photo';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgIf, NgFor, NgStyle,NgClass,FileUploadModule, DecimalPipe],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit
{
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  // Required input of type Member
  member = input.required<Member>();
  uploader?: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  memberChange = output<Member>();  //We will emit an event when our member has changed (been updated)

  ngOnInit(): void {
      this.initializeUploader();
  }

  fileOverBase(e: any)
  {
    this.hasBaseDropZoneOver = e;
  }

  deletePhoto(photo: Photo)
  {
    this.memberService.deletePhoto(photo).subscribe({
      next: _ => {
        const updatedMember = {...this.member()};
        updatedMember.photos = updatedMember.photos.filter(x => x.id !== photo.id);
        this.memberChange.emit(updatedMember);
      }
    })
  }

  setMainPhoto(photo: Photo)
  {
    this.memberService.setMainPhoto(photo).subscribe({
      next: _ =>
      {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user);
        }

        const updatedMember = {...this.member()}
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach(p => {
          if (p.isMain) p.isMain = false;
          if (p.id == photo.id) p.isMain = true;
        });
        this.memberChange.emit(updatedMember);

          
      }
    })
  }

  initializeUploader()
  {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      // 10 MB
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false  // We are sending up our authentication in a header (bc its jwt), rather than using cookies 
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {

      const photo = JSON.parse(response);
      const updatedMember = {...this.member()}; //Copying member Inputsignal object, this input is readonly so we shouldn't update it directly ()even though it is possible)

      updatedMember.photos.push(photo); // Update Inputsingal copy (updatedMember) by adding the photo.  
      
      // At this our member is updated and we need to output the updatedMember
      // memberChange is an output singal so we can use emit to emit and event and the event will be the updatedMember
      this.memberChange.emit(updatedMember);
    
    }
  }
}
