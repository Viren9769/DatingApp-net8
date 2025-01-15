import { CanActivateFn } from '@angular/router';
import { AccountService } from '../Service/account.service';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  if(accountService.currentUser()) {

  return true;
}
else{
  toastr.error('You are not authorized to access this page');
  return false;
}
};
