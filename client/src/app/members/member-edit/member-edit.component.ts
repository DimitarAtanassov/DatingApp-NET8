import { Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/accounts.service';
import { MembersService } from '../../_services/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";
import { DatePipe } from '@angular/common';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-member-edit',
  standalone: true,
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css',
  imports: [TabsModule, FormsModule, PhotoEditorComponent, DatePipe, TimeagoModule]
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm; //Html template is the child of this component, @ViewChild(Selector that we want) varName: type.
  @HostListener('window:beforeunload', ['$event']) notify($event:any) {  //beforeunload is a browser event that we are listening for.
    if(this.editForm?.dirty)
    {
      $event.returnValue = true;
    }
  } // We are adding this here because the deactive route gaurd only works with angular routes and if a user has made unsaved changes to their profile and tries to go to another page on our angular app
  // They will be asked for conformation via our alert, but if the user goes to google.com for example the gaurd wont work
  // So we have this here to warn to user when their browser emits an event which it will if they try to go back to their browser homepage, so they get warned they are leaving with unsaved changes
 
 
  member?:Member;
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private toastr = inject(ToastrService);

  ngOnInit(): void {
      this.loadMember();
  }

  loadMember()
  {
    const user = this.accountService.currentUser();
    if(!user) return;
    this.memberService.getMember(user.username).subscribe({
      next: member => this.member = member
    })
  }

  updateMember()
  {
    
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        
        this.toastr.success("Profile updated successfully")
        this.editForm?.reset(this.member);  // Reseting the form, because once the form is submitted the member will be updated, and then we reset it with the updated info
      }
    })

  }

  onMemberChange(event: Member)
  {
    this.member = event;
  }

}
