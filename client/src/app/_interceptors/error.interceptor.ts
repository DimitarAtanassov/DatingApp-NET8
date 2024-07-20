import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

// Function not a component
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toastr = inject(ToastrService);
  
  // next returns an observable, so we need to use pipe 
  return next(req).pipe(
    catchError(error => {
      if(error)
      {
        switch (error.status) {
            case 400:
              
            // We have a few requests that return 400 as their status, one has an 'errors' object which can be accessed w error.error.errors
            // First error is our HTTP error(the param) which contains an error object, which contains the 'errors' object
            // Validation errors are considered Model State Errors in our API, hence the name modalStateErrors
            // Our errors object contains two objects Username and password which contain arrays 
            /*
            Postman:
              {
                "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                "title": "One or more validation errors occurred.",
                "status": 400,
                "errors": {
                    "Password": [
                        "The Password field is required.",
                        "The field Password must be a string with a minimum length of 4 and a maximum length of 8."
                    ],
                    "Username": [
                        "The Username field is required."
                    ]
                },
                "traceId": "00-ca1e039f153fbea5f4229f9bf1d6fd81-5a2ac53872a8e5e9-00"
              } 
            */
              if (error.error.errors)
                {
                  const modalStateErrors = [];
                  for (const key in error.error.errors) {
                    if (error.error.errors[key])
                    {
                      modalStateErrors.push(error.error.errors[key]);
                    }
                  }
                  throw modalStateErrors.flat(); // Flattening them into an array so its easy in our code to work with
                }
                else // There is no errors object in our error object of the response so its the other 400 error not the register one
                {
                  toastr.error(error.error, error.status);
                }
                break;
          case 401:
            toastr.error('Unauthorized', error.status);    
            break;
        
          case 404:
            router.navigateByUrl('/not-found');
            break;
        
          case 500:
            // We are going to use a feature of the router that allows us to pass states, along w our navigation
            // We want to get the details of the error, and pass them into another component (The details of the error are called Navigation Extras) and this below is how that is done.
            // :NavigationExtras sets our const to a NvaigationExtras type
            const navigationExtras: NavigationExtras = {state: {error:error.error}};  // Specify states and what we are going to pass to them and then we specify we are passing an key called error, w value error.error
            router.navigateByUrl('/server-error', navigationExtras);
            break;
          default:
                toastr.error('Something unexpected went wrong');

            break;
        }
      }
      throw error;
    })
  )
};

// Now we need to go to app.config and add our interceptor
