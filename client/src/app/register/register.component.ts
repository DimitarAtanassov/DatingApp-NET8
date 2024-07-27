import { Component,inject,OnInit,output } from '@angular/core';
import {AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/accounts.service';
import { NgIf } from '@angular/common';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatePickerComponent } from "../_forms/date-picker/date-picker.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, TextInputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
  private accountService = inject(AccountService);
  private fb = inject(FormBuilder);
  private router = inject(Router);
  cancelRegister =  output<boolean>(); // Output because this will go from child to parent and the output is of type boolean
  registerForm : FormGroup = new FormGroup({});
  maxDate = new Date();
  validationErrors: string[] | undefined;

  ngOnInit(): void {
      this.initalizeForm();
      this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }

  initalizeForm()
  {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(4)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    })
    // every time password field of registerform changes we are going to call the validator for confirmPassword
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  // If values match we return null, if they do not match we return True.
  // control.value = confirmPassword value in registerform, control.parent.get(matchTo) matchTo = "password" so we get the value of the password field in registerform
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(matchTo)?.value ? null : {isMatching: true}
    }
  }
  // When a user registers we log them in and redirect to /memebers, bc at this point they will be logged in since we are returning to them a token.
  register() {
    const dob = this.getDateOnly(this.registerForm.get('dateOfBirth')?.value);
    this.registerForm.patchValue({dateOfBirth: dob});
    this.accountService.register(this.registerForm.value).subscribe({
      next: _ => this.router.navigateByUrl("/members"),
      error: error => this.validationErrors = error
    })
  }

  cancel() {
    this.cancelRegister.emit(false);
  }

  private getDateOnly(dob: string | undefined)
  {
    if(!dob) return;
    return new Date(dob).toISOString().slice(0,10); // Start index 0 to index 10
  }
}
