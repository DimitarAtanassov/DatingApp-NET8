// This is where we define routes for our app
import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChagesGuard } from './_guards/prevent-unsaved-chages.guard';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    
    // Dummy Route so we do not need to set canActivate on every route we want to be protected on our site, this will apply auth gaurd to all the routes in children array
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            {path: 'members', component: MemberListComponent},
            {path: 'members/:username', component: MemberDetailComponent}, // Dynamic Route (bc of :username)
            {path: 'member/edit', component: MemberEditComponent, canDeactivate: [preventUnsavedChagesGuard]},
            {path: 'lists', component: ListsComponent},
            {path: 'messages', component: MessagesComponent}
        ]
    },
    {path: 'errors', component: TestErrorsComponent},
    {path: 'not-found', component:NotFoundComponent},
    {path: 'server-error', component:ServerErrorComponent},
    {path: '**', component: HomeComponent, pathMatch: 'full'}  // Wildcard route, if none of the routes match this one will run, reccommend to use pathMatch = full in wildcard routes
];
