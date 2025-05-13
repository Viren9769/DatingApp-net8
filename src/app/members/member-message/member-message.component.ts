import { Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
import { MessagesService } from '../../Service/messages.service';
import { Message } from '../../_models/message';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-message',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-message.component.html',
  styleUrl: './member-message.component.css'
})
export class MemberMessageComponent  {
  @ViewChild('messageForm') messageForm?: NgForm;
  private messageService = inject(MessagesService);
username = input.required<string>();
messages = input.required<Message[]>();
messagecontent = '';
updateMessages = output<Message>();
sendMessage(){
  this.messageService.sendMessage(this.username(), this.messagecontent).subscribe({
    next: message => {
      this.updateMessages.emit(message);
      this.messageForm?.reset();
    }
  })
}

}
