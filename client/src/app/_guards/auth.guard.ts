import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/accounts.service';
import { ToastrService } from 'ngx-toastr';

// Function not a component
export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  if (accountService.currentUser())
  {
    return true;
  }
  else
  {
    toastr.error("You shall not pass!");
    return false;
  }  
};
