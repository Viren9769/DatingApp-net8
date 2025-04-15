import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { AccountService } from '../../Service/account.service';
import { environment } from '../../../environments/environment';
import { MembersService } from '../../Service/members.service';
import { Photo } from '../../_models/photo';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [NgIf, NgFor, NgStyle, NgClass, FileUploadModule, DecimalPipe ],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css'
})
export class PhotoEditorComponent implements OnInit{
private accountservice = inject(AccountService);
private memberservice = inject(MembersService);
 member = input.required<Member>();
 uploader?: FileUploader;
 hasBaseDropzoneOver = false;
 baseUrl = environment.apiUrl;
 memberchange = output<Member>();

 ngOnInit(): void {
  this.intializeUplaoder();
}

fileOverBase(e: any){
  this.hasBaseDropzoneOver =e;
}
deletePhoto(photo: Photo){
  this.memberservice.deletePhoto(photo).subscribe({
    next: _ => {
      const updatedMember = {...this.member()};
      updatedMember.photos = updatedMember.photos.filter(x => x.id !== photo.id);
      this.memberchange.emit(updatedMember);
    }
  })
}

setMainPhoto(photo: Photo){
this.memberservice.setMainPhoto(photo).subscribe({
  next: _ =>{
    const user = this.accountservice.currentUser();
    if(user) {
      user.photoUrl = photo.url;
      this.accountservice.setCurrentUser(user);
    }
    const updatedMember = {...this.member()}
    updatedMember.photoUrl = photo.url;
    updatedMember.photos.forEach(p => {
      if(p.isMain) p.isMain = false;
      if(p.id === photo.id) p.isMain = true;
    });
    this.memberchange.emit(updatedMember);
  }
})
}

intializeUplaoder(){
  this.uploader = new FileUploader({
    url: this.baseUrl + 'user/add-photo',
    authToken: 'Bearer ' + this.accountservice.currentUser()?.token,
    isHTML5: true,
    allowedFileType: ['image'],
    removeAfterUpload: true,
    autoUpload: false,
    maxFileSize: 10 * 1024 * 1024,
  });
  this.uploader.onAfterAddingFile = (file) => {
    file.withCredentials = false
  }
  this.uploader.onSuccessItem = (item, response, status, headers) => {
    const photo = JSON.parse(response);
    const updatedMember = {...this.member()}
    updatedMember.photos.push(photo);
    this.memberchange.emit(updatedMember);

  }
}
}
