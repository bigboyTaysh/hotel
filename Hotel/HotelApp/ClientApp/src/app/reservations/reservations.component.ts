import { getLocaleDateFormat } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent{
  public reservations: Reservation[];
  public filter: ReservationFilter;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.filter = {
      id: "",
      customerId: "",
      startDate: undefined,
      endDate: undefined,
    }

    http.get<Reservation[]>(baseUrl + 'api/reservations').subscribe(result => {
      this.reservations = result;
    }, error => console.error(error));
  }

  onDelete(id: string){
    this.http.delete<Reservation[]>(this.baseUrl + 'api/reservations/' + id).subscribe(result => {
      let reservation = this.reservations.find(x => x.id === id);
      let index = this.reservations.indexOf(reservation, 0);
      this.reservations.splice(index, 1);
    }, error => console.error(error));
  }

  onSearch(form: NgForm) {
    this.filter.id = form.value.id;
    this.filter.customerId = form.value.customerId;
    this.filter.startDate = form.value.startDate;
    this.filter.endDate = form.value.endDate;
    this.getReservationByName();
  }

  async getReservationByName() {
    this.http.post<Reservation[]>(this.baseUrl + 'api/reservations/reservationByName', this.filter).subscribe(result => {
      this.reservations = result;
    }, error => console.error(error));
  }


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

interface ReservationFilter {
  id: string;
  customerId: string;
  startDate: Date;
  endDate: Date;
}
