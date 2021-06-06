import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-reservations',
  templateUrl: './reservations.component.html',
  styleUrls: ['./reservations.component.css']
})
export class ReservationsComponent{
  public reservations: Reservation[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
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
