import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-edit',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.css']
})
export class EditUserComponent implements OnInit {
  public message = new BehaviorSubject<string>(null);
  user: User;
  id: string;


  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { this.http = http; this.baseUrl = baseUrl; }


  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.user = {
      id: "",
      login: "",
      password: "",
      role: "",
    }
    this.fetchUser();
  }

  onSubmit(form: NgForm) {
    let user = {
      id: this.user.id,
      login: form.value.login,
      password: form.value.password,
      role: form.value.role,
    }

    this.editUser(user);
  }

  async editUser(userInput) {
    this.http.put<User>(this.baseUrl + 'api/users', userInput).subscribe(result => {
      this.user = result;
      this.user.password = "";
      this.message.next('User "' + this.user.login + '" was edited successfully.');
    }, error => this.message.next(error.error));
  }

  async fetchUser(){
    this.http.get<User>(this.baseUrl + 'api/users/' + this.id).subscribe(result => {
      this.user = result;
      this.user.password = "";
    }, error => console.error(error));
  }
}

interface User {
  id: string;
  login: string;
  password: string;
  role: string;
}
