import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment'; //When in development mode because of our configuration in angular.json, this will get switched to environment.development when in dev mode 
import { Member } from '../_models/member';
@Injectable({
  providedIn: 'root'
})
// We will make http requests inside of here
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  // The routes in the two get functions require authentication because they under a [authorize] route class in the backend so we need to pass our token for user authentication with the request
  getMembers()
  {
    return this.http.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username:string)
  {
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

}
