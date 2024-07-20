import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.css'
})
export class ServerErrorComponent {
  error: any;

  // Injecting Router using consturctor injection
  constructor(private router: Router)
  {
    const navigation = this.router.getCurrentNavigation()
    this.error = navigation?.extras.state?.['error']; //We make our Navigation Extras state have an error key in our error.intercepor.ts file
  }
}
