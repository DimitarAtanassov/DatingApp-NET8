import { Component} from '@angular/core';
import { RegisterComponent } from "../register/register.component";

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})

// OnInit executes anything in the ngOnInit Function when the page is loaded
export class HomeComponent{
  registerMode = false;


  registerToggle() {
    this.registerMode = !this.registerMode
  }

  // We are passing a boolean parameter because that is what we are emmiting from the child component (the register component)
  cancelRegisterMode(event: boolean)
  {
    this.registerMode = event;
  }



}
