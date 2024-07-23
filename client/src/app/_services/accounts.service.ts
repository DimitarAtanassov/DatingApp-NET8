// Services are singletons and are created when our application starts
// Good place to store states and HTTP requests
import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';

// We can inject our services into our components
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl =  environment.apiUrl;
  currentUser = signal<User | null>(null); // currentUser inital value is set to null bc of the (null)

  login(model: any) {
    // .post<User> gives our post request a User return type
    // the model param is the body of the request
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    )
  }

  // When a user register we are also logging them in 
  register(model: any) {
    // .post<User> gives our post request a User return type
    return this.http.post<User>(this.baseUrl + 'account/register',model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
        return user; // We are returning from the map function to the .post request function above it (the user is part of the map function)
      })
    )
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  
}
