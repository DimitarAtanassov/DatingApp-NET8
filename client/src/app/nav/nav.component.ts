import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/accounts.service';
import {BsDropdownModule} from 'ngx-bootstrap/dropdown';
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  model: any = {};
  
  // login returns an observable, observabales are lazy so we need to subscribe to them because if no one is listening, nothing will happen
  // Once you subscribe to an observable you can tell it what to do 'next', what to do on 'error', and what to do when it is completed '()'
  login() {
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error)
    });
  }

  logout() {
    this.accountService.logout();
  }
}
