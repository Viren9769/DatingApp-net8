import { Component, computed, inject, input } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../Service/likes.service';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent {
  private likeservice = inject(LikesService);
 member = input.required<Member>();
hasLiked = computed(() => this.likeservice.likeIds().includes(this.member().id))

toggleLike(){
  this.likeservice.toggleLike(this.member().id).subscribe({
    next: () => {
      if(this.hasLiked()){
        this.likeservice.likeIds.update(ids => ids.filter(x => x!== this.member().id))
      }
      else{
        this.likeservice.likeIds.update(ids => [...ids, this.member().id])
      }

    }
  })
}
}
