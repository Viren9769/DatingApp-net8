import { Component, inject, OnInit, ViewChild, viewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MembersService } from '../../Service/members.service';
import { Member } from '../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessageComponent } from "../member-message/member-message.component";
import { Message } from '../../_models/message';
import { MessagesService } from '../../Service/messages.service';
@Component({
  selector: 'app-member-details',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessageComponent],
  templateUrl: './member-details.component.html',
  styleUrl: './member-details.component.css'
})
export class MemberDetailsComponent implements OnInit  {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
private memberservice = inject(MembersService);
private route = inject(ActivatedRoute);
private messageService = inject(MessagesService);
member: Member = {} as Member;
images: GalleryItem[] = [];
activeTab?: TabDirective;
messages: Message[] = [];

ngOnInit(): void {
  this.route.data.subscribe({
    next: data => {
      this.member = data['member'];
      this.member && this.member.photos.map(p => {
        this.images.push(new ImageItem({src: p.url, thumb: p.url}))
      })
    }
  })
  this.route.queryParams.subscribe({
    next: params => {
      params['tab'] && this.selectTab(params['tab'])
    }
  })
}

onUpdateMessages(event: Message){
this.messages.push(event);
}

selectTab(heading: string){
  if(this.memberTabs) {
    const messsageTab = this.memberTabs.tabs.find(x => x.heading === heading);
    if(messsageTab) messsageTab.active = true;
  }
}

onTabActivated(data: TabDirective){
  this.activeTab = data;
  if(this.activeTab.heading === 'Messages' && this.messages.length === 0 && this.member){
    this.messageService.getMessageThread(this.member.username).subscribe({
      next: messages => this.messages = messages
      })
  }
}

// loadMembers() {
//   const username = this.route.snapshot.paramMap.get('username');
//   if(!username) return;
//   this.memberservice.getMember(username).subscribe({
//     next: member =>{
//       this.member = member;
//       member.photos.map(p => {
//         this.images.push(new ImageItem({src: p.url, thumb: p.url}))
//       })
//     }
//   })
// }
}
