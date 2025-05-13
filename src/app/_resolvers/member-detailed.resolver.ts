import { ResolveFn } from '@angular/router';
import { Member } from '../_models/member';
import { MembersService } from '../Service/members.service';
import { inject } from '@angular/core';

export const memberDetailedResolver: ResolveFn<Member | null> = (route, state) => {
  const memberService = inject(MembersService);
  const username = route.paramMap.get('username');

  if(!username) return null;

  return memberService.getMember(username);
};
