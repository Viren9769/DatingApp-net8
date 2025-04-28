import { Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../Service/account.service';
import { MembersService } from '../../Service/members.service';

import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';


@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [TabsModule, FormsModule, PhotoEditorComponent, TimeagoModule,DatePipe],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event:any){
    if(this.editForm?.dirty){
      $event.returnValue = true;
    }
  }
  members? : Member;
  private accountservice = inject(AccountService);
  private memberservice = inject(MembersService);
  private toasterService = inject(ToastrService);


  ngOnInit(): void {
    this.loadMembers(); 
  }

  loadMembers(){
    const user = this.accountservice.currentUser();
    if(!user) return;
    this.memberservice.getMember(user.username).subscribe({
      next: member => this.members = member
    })

  }

  updateMember(){
    this.memberservice.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toasterService.success('Profile updated successfully');
        this.editForm?.reset(this.members);
      }
    })

  }

onMemberChange(event: Member) {
  this.members = event;
}

}
