import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { User } from 'src/api-authorization/authorize.service';

@Component({
  selector: 'app-add-reservation',
  templateUrl: './add-reservation.component.html',
  styleUrls: ['./add-reservation.component.css']
})
export class AddReservationComponent implements OnInit {
  public rooms: Room[] = [];
  public emptyRooms: Room[] = [];
  public startDate: string;
  public endDate: string;
  public selectedRooms: Room[] = [];
  public message = new BehaviorSubject<string>(null);
  public id: string;
  public customer: Customer;


  constructor(private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private _location: Location) {
      
    let startDate = new Date();
    startDate.setUTCHours(0, 0, 0, 0);
    this.startDate = startDate.toISOString().split('T')[0];

    let endDate = new Date();
    endDate.setUTCHours(0, 0, 0, 0);
    this.endDate = endDate.toISOString().split('T')[0];

    http.get<Room[]>(baseUrl + 'api/rooms').subscribe(result => {
      this.rooms = result;
      this.getEmptyRooms();
    }, error => console.error(error));
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');

    this.customer = {
      id: '',
      firstname: '',
      lastname: '',
      birthdate: new Date(),
      phone: '',
      email: '',
      address: {
        street: '',
        zipcode: '',
        city: '',
        country: '',
      }
    }

    this.http.get<Customer>(this.baseUrl + 'api/customers/' + this.id).subscribe(result => {
      this.customer = result;
    }, error => console.error(error));
  }

  async getEmptyRooms() {
    const request = {
      rooms: this.rooms,
      startDate: this.startDate,
      endDate: this.endDate
    };

    this.http.post<Room[]>(this.baseUrl + 'api/reservations/emptyRooms', request).subscribe(result => {
      this.emptyRooms = result;

      this.selectedRooms.forEach(room => {
        if (!this.emptyRooms.some(e => e.id === room.id)) {
          this.onRemove(room.id);
        }
      })
    }, error => console.error(error));
  }

  onAdd(id: string) {
    let room = this.emptyRooms.find(x => x.id === id);

    if (!this.selectedRooms.some(e => e.id === room.id)) {
      this.selectedRooms.push(room);
    }
  }

  onRemove(id: string) {
    let room = this.selectedRooms.find(x => x.id === id);
    let index = this.selectedRooms.indexOf(room, 0);
    this.selectedRooms.splice(index, 1);
  }

  onChangeStartDate(event) {
    this.startDate = event;
    this.getEmptyRooms();
  }

  onChangeEndDate(event) {
    this.endDate = event;
    if (new Date(this.endDate) > new Date(this.startDate)) {
      this.getEmptyRooms();
    }
  }

  async addReservation() {
    if (new Date(this.startDate).getTime() === new Date(this.endDate).getTime()) {
      this.message.next("Start date and end date can't be the same");
      return;
    }

    if (this.selectedRooms.length < 1) {
      this.message.next("Select the rooms to book");
      return;
    }

    const reservation = {
      id: '',
      customerId: this.customer.id,
      startDate: this.startDate,
      endDate: this.endDate,
      price: this.getPrice(),
      rooms: this.selectedRooms
    }

    this.http.post<Reservation>(this.baseUrl + 'api/reservations', reservation).subscribe(result => {
      let reservation = result;
      this.router.navigate(['reservation/' + reservation.id]);
    }, error => this.message.next(error.error));
  }

  getPrice() {
    const time = new Date(this.endDate).getTime() - new Date(this.startDate).getTime();
    const days = time / (1000 * 3600 * 24)

    return this.selectedRooms.reduce((a, b) => a + b.price * days, 0);
  }

  back() {
    this._location.back();
  }
}

interface Room {
  id: string;
  number: number;
  numberOfSeats: number;
  price: number;
  description: string;
}

interface Customer {
  id: string;
  firstname: string;
  lastname: string;
  birthdate: Date;
  phone: string;
  email: string;
  address: Address;
}

interface Address {
  street: string;
  zipcode: string;
  city: string;
  country: string;
}

interface Reservation {
  id: string;
  customerId: string;
  startDate: string;
  endDate: string;
  price: string;
}
