// We are going to intercept requests going out of our client (frontend) to the api, so we can add our token to the request on its way out. 

import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AccountService } from '../_services/accounts.service';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const accountService = inject(AccountService);  // Token is stored here
  
  // The request passed in as a param is immutable, so we need to create a clone of it and add the authorization header to it and return it
  if (accountService.currentUser())
  {
    // We are setting the req param to the clone with the authorization header so we can jsut return the req param
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${accountService.currentUser()?.token}`
      }
    })
  }
  return next(req);
};

// Now we need to add interceptor to our app so we need to go to app.config.ts and add it to provideHttpClient(withInterceptorsArray)