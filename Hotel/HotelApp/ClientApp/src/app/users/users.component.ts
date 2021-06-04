import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent{
  public users: User[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get<User[]>(baseUrl + 'api/users').subscribe(result => {
      this.users = result;
    }, error => console.error(error));
  }

  onDelete(id: string){
    this.http.delete<User[]>(this.baseUrl + 'api/users/' + id).subscribe(result => {
      let user = this.users.find(x => x.id === id);
      let index = this.users.indexOf(user, 0);
      this.users.splice(index, 1);
    }, error => console.error(error));
  }
}

interface User {
  id: string;
  login: string;
  role: string;
}
