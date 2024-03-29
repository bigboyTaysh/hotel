import { Component, OnInit } from '@angular/core';
import { AuthorizeService } from '../authorize.service';
import { Observable } from 'rxjs';
import { map, take, tap } from 'rxjs/operators';

@Component({
  selector: 'app-login-menu',
  templateUrl: './login-menu.component.html',
  styleUrls: ['./login-menu.component.css']
})
export class LoginMenuComponent implements OnInit {
  public isAuthenticated: Observable<boolean>;
  public userName: Observable<string>;
  public role: string;


  constructor(private authorizeService: AuthorizeService) { }

  ngOnInit() {
    if (!this.isUserAuthenticated()) {
      this.authorizeService.getUserFromStorage();
    }

    this.isAuthenticated = this.authorizeService.isAuthenticated([]);
    this.userName = this.authorizeService.getUser().pipe(map(u => u && u.name));
    this.authorizeService.getUser().pipe(map(u => u && u.role)).subscribe(value => this.role = value);
  }

  isUserAuthenticated() {
    let bool;
    this.authorizeService.isAuthenticated([]).subscribe(value => bool = value);
    return bool;
  }

  isInRole(role: string[]){
    
    return role.includes(this.role);
  }
}
