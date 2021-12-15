import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})

export class RoomsComponent {
  public rooms: Room[];
  public error: String;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.error = "";
    http.get<Room[]>(baseUrl + 'api/rooms').subscribe(result => {
      this.rooms = result;
      this.error = "";
    }, error => {
      console.error(error);
      this.error = "No access to data";
    });
  }

  onDelete(id: string) {
    this.http.delete<Room[]>(this.baseUrl + 'api/rooms/' + id).subscribe(result => {
      let room = this.rooms.find(x => x.id === id);
      let index = this.rooms.indexOf(room, 0);
      this.rooms.splice(index, 1);
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
