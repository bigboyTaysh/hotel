import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-add-reservation',
  templateUrl: './add-reservation.component.html',
  styleUrls: ['./add-reservation.component.css']
})
export class AddReservationComponent {
  public rooms: Room[];
  public emptyRooms: Room[];
  public startDate: string;
  public endDate: string;
  public selectedRooms: Room[] = [];
  public message = new BehaviorSubject<string>(null);


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    let startDate = new Date();
    startDate.setUTCHours(0,0,0,0);
    this.startDate = startDate.toISOString().split('T')[0];

    let endDate = new Date();
    endDate.setUTCHours(0,0,0,0);
    this.endDate = endDate.toISOString().split('T')[0];

    http.get<Room[]>(baseUrl + 'api/rooms').subscribe(result => {
      this.rooms = result;
      this.getEmptyRooms();
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
        if(!this.emptyRooms.some(e => e.id === room.id)){
          this.onRemove(room.id);
        }
      })
    }, error => console.error(error));
  }

  onAdd(id: string){
    let room = this.emptyRooms.find(x => x.id === id);

    if(!this.selectedRooms.some(e => e.id === room.id)){
      this.selectedRooms.push(room);
    }
  }

  onRemove(id: string){
    let room = this.selectedRooms.find(x => x.id === id);
    let index = this.selectedRooms.indexOf(room, 0);
    this.selectedRooms.splice(index, 1);
  }

  onChangeStartDate(event){
    this.startDate = event;
    this.getEmptyRooms();
  }

  onChangeEndDate(event){
    this.endDate = event;
    if(new Date(this.endDate) > new Date(this.startDate)){
      this.getEmptyRooms();
    }
  }

  /*
  onDelete(id: string){
    this.http.delete<Reservation[]>(this.baseUrl + 'api/reservations/' + id).subscribe(result => {
      
      let index = this.reservations.indexOf(reservation, 0);
      this.reservations.splice(index, 1);
    }, error => console.error(error));
  }*/
}

interface Room {
  id: string;
  number: number;
  numberOfSeats: number;
  price: number;
  description: string;
}