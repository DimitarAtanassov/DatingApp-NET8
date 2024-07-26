import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment'; //When in development mode because of our configuration in angular.json, this will get switched to environment.development when in dev mode 
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
@Injectable({
  providedIn: 'root'
})
// We will make http requests inside of here
export class MembersService {
  private http = inject(HttpClient);
  baseUrl = environment.apiUrl;
  // the components can use this singal to access the members
  members = signal<Member[]>([]); //We will store members from our response in this signal instead of the components that call the functions that return members 


  // The routes in the two get functions require authentication because they under a [authorize] route class in the backend so we need to pass our token for user authentication with the request
  getMembers()
  {
    return this.http.get<Member[]>(this.baseUrl + 'users').subscribe({
      next: members => this.members.set(members)
    })
  }

  getMember(username:string)
  {
    // Checking if member exists in our member array, and returning the member if it does before making request to backend.
    const member = this.members().find(x => x.username === username);
    if(member !== undefined) return of(member); //Converts member to an observable, because our member-detail component is subscribing to what this function retruns hence expecting and observable
      
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member)
  {
    /*
      We are working with an observable bc that is what we get back from an http request, so we use pipe
      We are using tap because we do not want to change or transfrom the observables value in any way
      Tap allows us to specifiy a callback function to do something with the observable without changing it
      members.update (members is a signal): Update the value of the signal based on its current value, and notify any dependents.
      members => members.map(m => m.username === member.username ? 
          member : m)): looping over each member in our array (m respresents the curr member being looped over)
      and we are saying if m.username === member.user (member in our function paramter on line 34), if they are equal we want to use the member that we are passing in on line 34,
      this member parameter has the member with the updated info, so we replace the entry with this newly updated entry, else we just replace the entry with itself by setting it to m

    */
    return this.http.put(this.baseUrl + 'users', member).pipe(
      tap(() => {
        this.members.update(members => members.map(m => m.username === member.username ? 
          member : m))
      })
    )
  }

  setMainPhoto(photo:Photo)
  {
    return this.http.put(this.baseUrl + 'users/set-main-photo/'+ photo.id, {}).pipe(
      tap(() => {
        this.members.update(members => members.map(m => {
          if(m.photos.includes(photo)) {
            m.photoUrl = photo.url
      }
          return m;
        }))
      })
    )
  }

  deletePhoto(photo: Photo)
  {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photo.id).pipe(
      tap(() => {
        this.members.update(members => members.map(m => {
          if (m.photos.includes(photo))
          {
            m.photos = m.photos.filter(x => x.id !== photo.id)
          }
          return m;
        }))
      })
    )
  }

}
