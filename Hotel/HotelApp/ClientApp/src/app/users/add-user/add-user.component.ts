import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  user: User;
  http: HttpClient;
  baseUrl: string;
  public message = new BehaviorSubject<string>(null);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _location: Location) { this.http = http; this.baseUrl = baseUrl; }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    let user = {
      id: "",
      login: form.value.login,
      password: form.value.password,
      role: form.value.role,
    }

    this.addUser(user, form);
  }

  async addUser(userInput, form) {
    console.log(userInput);
    this.http.post<User>(this.baseUrl + 'api/users', userInput).subscribe(result => {
      this.user = result;
      this.message.next('User "' + this.user.login + '" was added successfully.');
      form.reset();
    }, error => this.message.next(error.error));
  }

  back() {
    this._location.back();
  }
}

interface User {
  id: string;
  login: string;
  role: string;
}
