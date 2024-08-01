// Services are singletons and are created when our application starts
// Good place to store states and HTTP requests
import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { User } from '../_models/user';
import { map } from 'rxjs';
import { environment } from '../../environments/environment';
import { LikesService } from './likes.service';

// We can inject our services into our components
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  private likeService = inject(LikesService);
  baseUrl =  environment.apiUrl;
  currentUser = signal<User | null>(null); // currentUser inital value is set to null bc of the (null)
  
  // Computed signal based on the current value of currentUser signal.
  roles = computed(() => {
    const user = this.currentUser();
    if (user && user.token)
    {
      const role = JSON.parse(atob(user.token.split('.')[1])).role;
      return Array.isArray(role) ? role : [role];
    }
    return [null];
  })

  login(model: any) {
    // .post<User> gives our post request a User return type
    // the model param is the body of the request
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map(user => {
        if (user) {
          this.setCurrentUser(user);
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
          this.setCurrentUser(user);
        }
        return user; // We are returning from the map function to the .post request function above it (the user is part of the map function)
      })
    )
  }

  setCurrentUser(user:User)
  {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
    this.likeService.getLikeIds();
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  
}
