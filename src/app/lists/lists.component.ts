import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../Service/likes.service';
import { Member } from '../_models/member';
import { FormsModule } from '@angular/forms';
import { MemberCardComponent } from "../members/member-card/member-card.component";
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [FormsModule, MemberCardComponent,ButtonsModule, PaginationModule],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit, OnDestroy{
 
  likeservice = inject(LikesService);
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;

  ngOnInit(): void {
    this.loadLikes();
  }
  
  getTitle(){
    switch (this.predicate){
      case 'liked': return 'Members you Like';
      case 'likedby': return 'Members who likes you';
      default: return 'Mutual'
    }
  }


  loadLikes(){
    this.likeservice.getLikes(this.predicate, this.pageNumber, this.pageSize);
  }

  pageChanged(event:any){
    if(this.pageNumber !==event.page){
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }

  ngOnDestroy(): void {
    this.likeservice.paginatedResult.set(null);
  }
}
