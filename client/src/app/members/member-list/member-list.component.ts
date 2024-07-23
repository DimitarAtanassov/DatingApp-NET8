import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { MemberCardComponent } from "../member-card/member-card.component";

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css'
})
export class MemberListComponent implements OnInit {
  memberService = inject(MembersService);
  
  ngOnInit(): void {
    // Our signal is updated in our member service, but anything listenting to our signal such as this component will also react to those changes 
    if (this.memberService.members().length === 0) this.loadMembers();
    
  }

  loadMembers()
  {
    this.memberService.getMembers();
  }
}
