import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { ListsComponent } from './lists/lists.component';
import { MessageComponent } from './messages/message/message.component';
import { authGuard } from './_guards/auth.guard';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            {path: 'members', component: MemberListComponent, canActivate:[authGuard]},
            {path: 'members/:id', component: MemberDetailsComponent},
            {path: 'lists', component: ListsComponent},
            {path: 'messages', component: MessageComponent},
        ]      
    }, 
    {path: '*', component: HomeComponent, pathMatch: 'full'}  
];
