import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/accounts.service';
import {BsDropdownModule} from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../_directives/has-role.directive';
@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule,RouterLink, RouterLinkActive, HasRoleDirective],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  accountService = inject(AccountService);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  model: any = {};
  
  // login returns an observable, observabales are lazy so we need to subscribe to them because if no one is listening, nothing will happen
  // Once you subscribe to an observable you can tell it what to do 'next', what to do on 'error', and what to do when it is completed '()'
  // Subscribing to an observable triggers the execution of the underlying HTTP request
  login() {
    this.accountService.login(this.model).subscribe({
      next: () => {
        this.router.navigateByUrl('/members')  //Navigates the user to the /members route when they login
      },
      error: error => this.toastr.error(error.error) // This error, the parameter one is returned as an object, so we need to access the error in the error object which is error.error
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl("/")
  }
}
