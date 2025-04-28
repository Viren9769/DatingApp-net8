import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { inject, Injectable, model, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParam';
import { AccountService } from './account.service';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http  = inject(HttpClient);
  private accountservice = inject(AccountService);
  baseUrl = environment.apiUrl;  
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map();
  user = this.accountservice.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));
  


  resetUserParams(){
    this.userParams.set(new UserParams(this.user));
  }

  getmembers(){

    const response = this.memberCache.get(Object.values(this.userParams()).join('-'));
    if(response) return this.setpaginatedResponse(response);
    let params = this.setPaginationHeaders(this.userParams().pagNumber, this.userParams().pageSize);
    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);
    return this.http.get<Member[]>(this.baseUrl + 'user', {observe: 'response', params}).subscribe({
      next: response => {
       this.setpaginatedResponse(response);
       this.memberCache.set(Object.values(this.userParams()).join('-'),response)
      } 
    })
  }

  private setpaginatedResponse(response: HttpResponse<Member[]>)
  {
    this.paginatedResult.set({
      items: response.body as Member[],
      pagination: JSON.parse(response.headers.get('Pagination')!)
    })
  }

  private setPaginationHeaders(pageNumber: number, pageSize: number){
    let params = new HttpParams();
    if(pageNumber && pageSize){
      params = params.append('pageNumber', pageNumber);
      params = params.append('pageSize', pageSize);
    }
    return params;
  }


  getMember(username: string){
    // const member = this.members().find(x => x.username === username);
    // if(member !== undefined) return of(member);
    const member: Member = [...this.memberCache.values()]
    .reduce((arr, elem) => arr.concat(elem.body), [])
    .find((m: Member) => m.username === username);
    if(member) return of(member);
    return this.http.get<Member>(this.baseUrl + 'user/' + username);
  }
  updateMember(member: Member){
    return this.http.put(this.baseUrl + 'user', member).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(x => x.username === member.username
      //     ? member : x))
      // })
    )
  }

  setMainPhoto(photo: Photo){
    return this.http.put(this.baseUrl + 'user/set-main-photo/' + photo.id,{}).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if(m.photos.includes(photo)){
      //       m.photoUrl = photo.url
      //     }
      //     return m;
      //   }))
      // })
    )
  }

  deletePhoto(photo: Photo){
    return this.http.delete(this.baseUrl + 'user/delete-photo/' + photo).pipe(
      // tap(() => {
      //   this.members.update(members => members.map(m => {
      //     if(m.photos.includes(photo)){
      //       m.photoUrl = photo.url
      //     }
      //     return m;
      //   }))
      // })

    )
  }
}
