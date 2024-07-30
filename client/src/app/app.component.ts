import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/accounts.service';
import { HomeComponent } from "./home/home.component";
import { NgxSpinnerComponent } from 'ngx-spinner';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [RouterOutlet, NavComponent, HomeComponent, NgxSpinnerComponent]
})
export class AppComponent implements OnInit {
  private accountService = inject(AccountService); 


  // A callback function is a function in which:
  // accessible by another function, and is invoked after the first function if the first functoin completes.
  // A nice way to think of how a callback function works is that it is a function that is "called at the back" of the function it is passed into
  ngOnInit(): void {
    this.setCurrentUser();
  }
  
// This will keep our user presistent ever after a page refresh
  setCurrentUser()
  {
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }


} 
