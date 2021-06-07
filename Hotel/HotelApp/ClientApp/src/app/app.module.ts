import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { UsersComponent } from './users/users.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';
import { AddUserComponent } from './users/add-user/add-user.component';
import { RoomsComponent } from './rooms/rooms.component';
import { CustomersComponent } from './customers/customers.component';
import { AddCustomerComponent } from './customers/add-customer/add-customer.component';
import { EditCustomerComponent } from './customers/edit-customer/edit-customer.component';
import { EditRoomsComponent } from './rooms/edit-rooms/edit-rooms.component';
import { AddRoomsComponent } from './rooms/add-rooms/add-rooms.component';
import { ReservationsComponent } from './reservations/reservations.component';
import { AddReservationComponent } from './reservations/add-reservation/add-reservation.component';
import { CustomerDetailsComponent } from './customers/customer-details/customer-details.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    UsersComponent,
    EditUserComponent,
    AddUserComponent,
    RoomsComponent,
    CustomersComponent,
    AddCustomerComponent,
    EditCustomerComponent,
    EditRoomsComponent,
    AddRoomsComponent,
    ReservationsComponent,
    AddReservationComponent,
    CustomerDetailsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },

      { path: 'users', component: UsersComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin'] } },
      { path: 'users/add', component: AddUserComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin'] } },
      { path: 'users/edit/:id', component: EditUserComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin'] } },

      { path: 'rooms', component: RoomsComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },
      { path: 'rooms/add', component: AddRoomsComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },
      { path: 'rooms/edit/:id', component: EditRoomsComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },

      { path: 'customers', component: CustomersComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },
      { path: 'customers/add', component: AddCustomerComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },
      { path: 'customers/edit/:id', component: EditCustomerComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },
      { path: 'customer/:id', component: CustomerDetailsComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },

      { path: 'reservations', component: ReservationsComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },
      { path: 'reservations/add/:id', component: AddReservationComponent, canActivate: [AuthorizeGuard], data: { roles: ['Admin', 'Employee'] } },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
