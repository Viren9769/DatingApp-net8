import { Component, inject } from '@angular/core';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './Service/account.service';
//import { HomeComponent } from "./home/home.component";
import { RouterOutlet } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NgxSpinnerComponent } from 'ngx-spinner';
//import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  // constructor
  
  private accountservice = inject(AccountService);
  title = 'Gunatit Sambandh';
  
 // constructor(private http: HttpClient){}
 // life cycle hook
 ngOnInit(): void {

this.setCurrentUser();
 }

 setCurrentUser(){
  const userString = localStorage.getItem('user');
  if(!userString) return;
  const user = JSON.parse(userString);
  this.accountservice.setCurrentUser(user);
  
 }
 
}
  