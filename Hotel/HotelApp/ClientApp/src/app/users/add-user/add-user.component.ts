import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  user: User;
  http: HttpClient;
  baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) { this.http = http; this.baseUrl = baseUrl; }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    let user = {
      id: "",
      login: form.value.login,
      password: form.value.password,
      role: form.value.role,
    }

    this.addUser(user);
  }

  async addUser(userInput) {
    console.log(userInput);
    this.http.post<User>(this.baseUrl + 'api/users', userInput).subscribe(result => {
      this.user = result;
    }, error => console.error(error));
  }
}

interface User {
  id: string;
  login: string;
  role: string;
}
