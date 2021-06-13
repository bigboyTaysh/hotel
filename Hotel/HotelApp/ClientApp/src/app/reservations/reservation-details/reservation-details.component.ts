import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-reservation-details',
  templateUrl: './reservation-details.component.html',
  styleUrls: ['./reservation-details.component.css'],
})
export class ReservationDetailsComponent implements OnInit {
  public id: string;
  public customer: Customer;
  public reservation: Reservation;
  public message = new BehaviorSubject<string>(null);
  public customerIsExpanded = false;

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.fetchReservation();
  }

  fetchReservation () {
    this.http.get<Reservation>(this.baseUrl + 'api/reservations/' + this.id).subscribe(result => {
      this.reservation = result;

      this.http.get<Customer>(this.baseUrl + 'api/customers/' + this.reservation.customerId).subscribe(result => {
        this.customer = result;
      }, error => console.error(error));
    }, error => console.error(error));
  }

  toggleCustomer () {
    this.customerIsExpanded = !this.customerIsExpanded;
  }
}

interface Customer {
  id: string;
  firstname: string;
  lastname: string;
  birthdate: Date;
  phone: string;
  email: string;
  address: Address
}

interface Address {
  street: string;
  zipCode: string;
  city: string;
  country: string;
}

interface Reservation {
  id: string;
  customerId: string;
  startDate: string;
  endDate: string;
  price: string;
  rooms: Room[];
}

interface Room {
  id: string;
  number: number;
  numberOfSeats: number;
  price: number;
  description: string;
}