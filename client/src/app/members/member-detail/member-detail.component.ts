import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import {TabsModule} from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule,DatePipe ],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.css'
})
export class MemberDetailComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute); // When a user comes to this member detail page, their are activating a route, and we want to get something from that activated route
  member?: Member;
  images: GalleryItem[] = [];

  ngOnInit(): void 
  {
      this.loadMember();
  }

  loadMember()
  {
    // In app.routes we have this route {path: 'members/:username', component: MemberDetailComponent}, 
    // we need to match the param names (username must be spelled the exact same way as the username Query param) in order to get that param from the snapshot of the route
    const username = this.route.snapshot.paramMap.get('username');
    if(!username) return;

    this.memberService.getMember(username).subscribe({
      next: member => {
        this.member = member;
        member.photos.map(p => {
          this.images.push(new ImageItem({src: p.url, thumb: p.url})) //Mapping photos from member that comes with a photos array to our GalleryItem array called imagines (ImageItem comes from ngx-gallery)
        })
      }
    })
  }



}
