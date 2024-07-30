// Child component of member-list component

import { Component, computed, inject, input } from '@angular/core';
import { Member } from '../../_models/member';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../_services/likes.service';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css'
})
export class MemberCardComponent 
{
  private likesService = inject(LikesService);
  member = input.required<Member>();
  // Anytime we add or remove to our likeIds, because this is a computed singal it will be recomputed and stored in hasLiked.
  hasLiked = computed(() => this.likesService.likeIds().includes(this.member().id));  //Checks if this member is in the likes of the currently logged in member

  toggleLike()
  {
    this.likesService.toggleLike(this.member().id).subscribe({
      next: () => {
        if (this.hasLiked())
        {
          this.likesService.likeIds.update(ids => ids.filter(x => x !== this.member().id));  // if hasLiked is true we will remove the like, if itself we will add a like (client side).
        }
        else
        {
          this.likesService.likeIds.update(ids => [...ids, this.member().id]);
        }
      }
    })
  }
}
