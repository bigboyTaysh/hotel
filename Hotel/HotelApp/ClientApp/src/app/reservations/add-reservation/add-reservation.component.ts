import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-add-reservation',
  templateUrl: './add-reservation.component.html',
  styleUrls: ['./add-reservation.component.css']
})
export class AddReservationComponent {
  public rooms: Room[];
  public emptyRooms: Room[];
  public startDate: Date;
  public endDate: Date;


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    let startDate = new Date();
    startDate.setUTCHours(0,0,0,0);
    this.startDate = startDate;

    let endDate = new Date();
    endDate.setUTCHours(0,0,0,0);
    this.endDate = endDate;

    http.get<Room[]>(baseUrl + 'api/rooms').subscribe(result => {
      this.rooms = result;
      this.getEmptyRooms();
    }, error => console.error(error));
  }

  async getEmptyRooms() {
    const request = {
      rooms: this.rooms,
      startDate: this.startDate.toISOString(),
      endDate: this.endDate.toISOString()
    };
    
    this.http.post<Room[]>(this.baseUrl + 'api/reservations/emptyRooms', request).subscribe(result => {
      this.emptyRooms = result;
    }, error => console.error(error));
  }

}

interface Room {
  id: string;
  number: number;
  numberOfSeats: number;
  price: number;
  description: string;
}