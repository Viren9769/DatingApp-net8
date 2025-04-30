import { inject, Injectable, signal, WritableSignal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Member } from '../_models/member';
import { PaginatedResult } from '../_models/pagination';
import { setpaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class LikesService {
  baseurl = environment.apiUrl;
  private http = inject(HttpClient);
  likeIds = signal<number[]>([]);
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);

  toggleLike(targetId: number)
  {
    return this.http.post(`${this.baseurl}likes/${targetId}`,{})
  }
  getLikes(predicate: string, pageNumber: number, pageSize: number){
    let params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append('predicate', predicate);
    return this.http.get<Member[]>(`${this.baseurl}likes`,
      {observe: 'response', params}).subscribe({
        next: response => setpaginatedResponse(response, this.paginatedResult)
      })
  }
  getlikeIds(){
    return this.http.get<number[]>(`${this.baseurl}likes/list`).subscribe({
      next: ids => this.likeIds.set(ids)
    })
  }
}


