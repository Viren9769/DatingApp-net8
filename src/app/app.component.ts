import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
//import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  // constructor
  http = inject(HttpClient);
  title = 'Gunatit Sambandh';
  users: any;
 // constructor(private http: HttpClient){}
 // life cycle hook
 ngOnInit(): void {
  this.http.get('http://localhost:5000/api/user').subscribe({
    next: response => this.users = response,
    error: error => console.log(error),
    complete: () => console.log('Request has completed')  // handle completion of HTTP request
  })
 }
}
  