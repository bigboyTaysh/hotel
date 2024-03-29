import { Injectable } from '@angular/core';
//import { User, UserManager, WebStorageStateStore } from 'oidc-client';
import { BehaviorSubject, concat, from, Observable, ObservableInput } from 'rxjs';
import { filter, map, mergeMap, take, tap } from 'rxjs/operators';
import { ApplicationPaths, ApplicationName } from './api-authorization.constants';
import jwt_decode from 'jwt-decode';
import { Router } from '@angular/router';
import { HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';

export type IAuthenticationResult =
  SuccessAuthenticationResult |
  FailureAuthenticationResult |
  RedirectAuthenticationResult;

export interface SuccessAuthenticationResult {
  status: AuthenticationResultStatus.Success;
  state: any;
}

export interface FailureAuthenticationResult {
  status: AuthenticationResultStatus.Fail;
  message: string;
}

export interface RedirectAuthenticationResult {
  status: AuthenticationResultStatus.Redirect;
}

export enum AuthenticationResultStatus {
  Success,
  Redirect,
  Fail
}

export interface Token {
  access_token: string;
  refresh_token: string;
}

export interface UserSettings {
  name: string,
  token: string;
  session_state: string;
  access_token: string;
  refresh_token: string;
  token_type: string;
  scope: string;
  role: string;
  expires_at: number;
  state: any;
}

export class User {
  constructor(settings: UserSettings) {
    this.name = settings.name;

    this.token = settings.token;
    /** The session state value returned from the OIDC provider (opaque) */
    this.session_state = settings.session_state;
    /** The access token returned from the OIDC provider. */
    this.access_token = settings.access_token;
    /** Refresh token returned from the OIDC provider (if requested) */
    this.refresh_token = settings.refresh_token;
    /** The token_type returned from the OIDC provider */
    this.token_type = settings.token_type;
    /** The scope returned from the OIDC provider */
    this.scope = settings.scope;
    /** The claims represented by a combination of the id_token and the user info endpoint */
    this.role = settings.role;
    /** The expires at returned from the OIDC provider */
    this.expires_at = settings.expires_at;
    /** The custom state transferred in the last signin */
    this.state = settings.state;
  };

  name: string;
  /** The id_token returned from the OIDC provider */
  token: string;
  /** The session state value returned from the OIDC provider (opaque) */
  session_state?: string;
  /** The access token returned from the OIDC provider. */
  access_token: string;
  /** Refresh token returned from the OIDC provider (if requested) */
  refresh_token?: string;
  /** The token_type returned from the OIDC provider */
  token_type: string;
  /** The scope returned from the OIDC provider */
  scope: string;
  /** The claims represented by a combination of the id_token and the user info endpoint */
  role: string;
  /** The expires at returned from the OIDC provider */
  expires_at: number;
  /** The custom state transferred in the last signin */
  state: any;

  /** Calculated number of seconds the access token has remaining */
  readonly expires_in: number;
  /** Calculated value indicating if the access token is expired */
  readonly expired: boolean;
  /** Array representing the parsed values from the scope */
  readonly scopes: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  // By default pop ups are disabled because they don't work properly on Edge.
  // If you want to enable pop up authentication simply set this flag to false.

  constructor(private router: Router) {}

  private userSubject: BehaviorSubject<User | null> = new BehaviorSubject(null);

  public async signIn(credentials: any, state: any): Promise<IAuthenticationResult> {
    let user: User = null;
    
    const response = await fetch(ApplicationPaths.Login, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      },
      method: 'POST',
      body: JSON.stringify(credentials)
    });

    if (!response.ok) {
      return this.error('There was an error signing in.');
    }

    const token = await response.json();
    const tokenInfo = this.getDecodedAccessToken(token.accessToken);

    const settings: any = {
      name: tokenInfo.unique_name,
      role: tokenInfo.role,
      refresh_token: token.refreshToken,
      access_token: token.accessToken,
      token_type: 'Bearer',
    };
    
    user = new User(settings);
    this.userSubject.next(user);
    this.setUserToStorage(user);

    return this.success(state);
  }

  public async signOut(state: any): Promise<IAuthenticationResult> {
    let user;
    this.getUser().subscribe(value => user = value);
    
    await fetch(ApplicationPaths.LogOut, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      method: 'POST',
      body: JSON.stringify({
          token: user.refresh_token
        })
    });

    localStorage.removeItem('name');
    localStorage.removeItem('role');
    localStorage.removeItem('refresh_token');
    localStorage.removeItem('token_type');

    this.userSubject.next(null);
    return this.success(state);
  }

  public isAuthenticated(allowedRoles: string[]): Observable<boolean> {
    return this.getUser().pipe(map(u => this.isInRole(u, allowedRoles)));
  }

  private isInRole(user: User, allowedRoles: string[]){
    if (user == null){
      return false;
    }
    
    if (allowedRoles == null || allowedRoles.length === 0) {
      return true;
    }

    return allowedRoles.includes(user.role);
  }

  private error(message: string): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Fail, message };
  }

  private success(state: any): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Success, state };
  }

  private setUserToStorage(user: User) {
    localStorage.setItem("name", user.name);
    localStorage.setItem("role", user.role);
    localStorage.setItem("refresh_token", user.refresh_token);
    localStorage.setItem("token_type", user.token_type);
  }

  public getUser(): Observable<User | null> {
    return concat(
      //this.userSubject.pipe(filter(u => !!u)),
      this.userSubject.pipe(),
      //this.getUserFromStorage().pipe(filter(u => !!u), tap(u => this.userSubject.next(u))),
      this.userSubject.asObservable());
  }

  public getAccessToken(): Observable<string> {
    return this.userSubject
      .pipe(mergeMap(() => from(this.getUser())),
        map(user => user && user.access_token));
  }

  public getUserFromStorage() {
    const settings: any = {
      name: localStorage.getItem('name'),
      role: localStorage.getItem('role'),
      refresh_token: localStorage.getItem('refresh_token'),
      token_type: localStorage.getItem('token_type'),
    };

    if (this.isNotEmpty(settings)) {
      this.userSubject.next(new User(settings));
    }
  }

  private isNotEmpty(obj): boolean {
    for (var key in obj) {
      if (obj[key] === null || obj[key] === "" || obj[key] === undefined)
        return false;
    }
    return true;
  }

  getDecodedAccessToken(token: string): any {
    try{
        return jwt_decode(token);
    }
    catch(Error){
        return null;
    }
  }

  async sendReqWithNewToken(token: string, req: HttpRequest<any>, next: HttpHandler): Promise<any> {
    let user;
    this.getUser().subscribe(value => user = value);

    if(!this.isTokenExpired(token)){
      return;
    }

    if (user == null) {
      return;
    }

    const response = await fetch(ApplicationPaths.Token, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${user.refresh_token}`
      },
      method: 'POST',
    });

    if (!response.ok) {
      await this.signOut({ReturnUrlType: ""});
      await this.router.navigateByUrl("authentication/login", {
        replaceUrl: true
      });
    } else {
      const newToken = await response.json();
      const tokenInfo = this.getDecodedAccessToken(newToken.accessToken);
  
      const settings: any = {
        name: tokenInfo.unique_name,
        role: tokenInfo.role,
        refresh_token: newToken.refreshToken,
        access_token: newToken.accessToken,
        token_type: 'Bearer',
      };
      
      this.userSubject.next(new User(settings));
      this.setUserToStorage(user);
  
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${newToken.accessToken}`
        }
      });

      return next.handle(req).toPromise();
    }
  }
  
  isTokenExpired(token: string): boolean {
    const decoded = this.getDecodedAccessToken(token);
    if (!decoded) {
      return true;
    }

    const date = decoded.exp;
    if (!date) {
      return true;
    }

    return !(date > new Date().getTime() / 1000);
  }

  /*
  private popUpDisabled = true;
  private userManager: UserManager;
  private userSubject: BehaviorSubject<IUser | null> = new BehaviorSubject(null);

  public isAuthenticated(): Observable<boolean> {
    return this.getUser().pipe(map(u => !!u));
  }

  public getUser(): Observable<IUser | null> {
    return concat(
      this.userSubject.pipe(take(1), filter(u => !!u)),
      this.getUserFromStorage().pipe(filter(u => !!u), tap(u => this.userSubject.next(u))),
      this.userSubject.asObservable());
  }

  public getAccessToken(): Observable<string> {
    return from(this.ensureUserManagerInitialized())
      .pipe(mergeMap(() => from(this.userManager.getUser())),
        map(user => user && user.access_token));
  }

  // We try to authenticate the user in three different ways:
  // 1) We try to see if we can authenticate the user silently. This happens
  //    when the user is already logged in on the IdP and is done using a hidden iframe
  //    on the client.
  // 2) We try to authenticate the user using a PopUp Window. This might fail if there is a
  //    Pop-Up blocker or the user has disabled PopUps.
  // 3) If the two methods above fail, we redirect the browser to the IdP to perform a traditional
  //    redirect flow.
  public async signIn(state: any): Promise<IAuthenticationResult> {
    await this.ensureUserManagerInitialized();
    let user: User = null;
    try {
      user = await this.userManager.signinSilent(this.createArguments());
      this.userSubject.next(user.profile);
      return this.success(state);
    } catch (silentError) {
      // User might not be authenticated, fallback to popup authentication
      console.log('Silent authentication error: ', silentError);

      try {
        if (this.popUpDisabled) {
          throw new Error('Popup disabled. Change \'authorize.service.ts:AuthorizeService.popupDisabled\' to false to enable it.');
        }
        user = await this.userManager.signinPopup(this.createArguments());
        this.userSubject.next(user.profile);
        return this.success(state);
      } catch (popupError) {
        if (popupError.message === 'Popup window closed') {
          // The user explicitly cancelled the login action by closing an opened popup.
          return this.error('The user closed the window.');
        } else if (!this.popUpDisabled) {
          console.log('Popup authentication error: ', popupError);
        }

        // PopUps might be blocked by the user, fallback to redirect
        try {
          await this.userManager.signinRedirect(this.createArguments(state));
          return this.redirect();
        } catch (redirectError) {
          console.log('Redirect authentication error: ', redirectError);
          return this.error(redirectError);
        }
      }
    }
  }

  public async completeSignIn(url: string): Promise<IAuthenticationResult> {
    try {
      await this.ensureUserManagerInitialized();
      const user = await this.userManager.signinCallback(url);
      this.userSubject.next(user && user.profile);
      return this.success(user && user.state);
    } catch (error) {
      console.log('There was an error signing in: ', error);
      return this.error('There was an error signing in.');
    }
  }

  public async signOut(state: any): Promise<IAuthenticationResult> {
    try {
      if (this.popUpDisabled) {
        throw new Error('Popup disabled. Change \'authorize.service.ts:AuthorizeService.popupDisabled\' to false to enable it.');
      }

      await this.ensureUserManagerInitialized();
      await this.userManager.signoutPopup(this.createArguments());
      this.userSubject.next(null);
      return this.success(state);
    } catch (popupSignOutError) {
      console.log('Popup signout error: ', popupSignOutError);
      try {
        await this.userManager.signoutRedirect(this.createArguments(state));
        return this.redirect();
      } catch (redirectSignOutError) {
        console.log('Redirect signout error: ', redirectSignOutError);
        return this.error(redirectSignOutError);
      }
    }
  }

  public async completeSignOut(url: string): Promise<IAuthenticationResult> {
    await this.ensureUserManagerInitialized();
    try {
      const response = await this.userManager.signoutCallback(url);
      this.userSubject.next(null);
      return this.success(response && response.state);
    } catch (error) {
      console.log(`There was an error trying to log out '${error}'.`);
      return this.error(error);
    }
  }

  private createArguments(state?: any): any {
    return { useReplaceToNavigate: true, data: state };
  }

  private error(message: string): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Fail, message };
  }

  private success(state: any): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Success, state };
  }

  private redirect(): IAuthenticationResult {
    return { status: AuthenticationResultStatus.Redirect };
  }

  private async ensureUserManagerInitialized(): Promise<void> {
    if (this.userManager !== undefined) {
      return;
    }

    const response = await fetch(ApplicationPaths.ApiAuthorizationClientConfigurationUrl);
    if (!response.ok) {
      throw new Error(`Could not load settings for '${ApplicationName}'`);
    }


    const settings: any = {
      authority: 'https://localhost:44393/',
      client_id: 'spa',
      redirect_uri: 'https://localhost:44331/',
      post_logout_redirect_uri: 'https://localhost:44331/',
      response_mode: 'query',
      response_type:"Baerer token",
      scope: 'email role',
      filterProtocolClaims: true,
      loadUserInfo: true,
      
    };

    settings.automaticSilentRenew = true;
    settings.includeIdTokenInSilentRenew = true;
    this.userManager = new UserManager(settings);

    this.userManager.events.addUserSignedOut(async () => {
      await this.userManager.removeUser();
      this.userSubject.next(null);
    });
  }

  private getUserFromStorage(): Observable<IUser> {
    return from(this.ensureUserManagerInitialized())
      .pipe(
        mergeMap(() => this.userManager.getUser()),
        map(u => u && u.profile));
  }
  */
}
