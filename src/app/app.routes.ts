import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { ListsComponent } from './lists/lists.component';
import { MessageComponent } from './messages/message/message.component';
import { authGuard } from './_guards/auth.guard';
import { ErrorsComponent } from '../errors/errors/errors.component';
import { NotFoundComponent } from '../errors/not-found/not-found.component';
import { ServerErrorComponent } from '../errors/server-error/server-error.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            {path: 'members', component: MemberListComponent, canActivate:[authGuard]},
            {path: 'members/:username', component: MemberDetailsComponent},
            {path: 'member/edit', component: MemberEditComponent, canDeactivate:[preventUnsavedChangesGuard]},
            {path: 'lists', component: ListsComponent},
            {path: 'messages', component: MessageComponent},
        ]      
    }, 
    {path: 'errors', component: ErrorsComponent},
    {path : 'not-found', component: NotFoundComponent},
    { path: 'server-error', component: ServerErrorComponent},
    {path: '*', component: HomeComponent, pathMatch: 'full'}  
];
