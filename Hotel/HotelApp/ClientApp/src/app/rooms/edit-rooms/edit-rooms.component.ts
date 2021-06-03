import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-edit-rooms',
  templateUrl: './edit-rooms.component.html',
  styleUrls: ['./edit-rooms.component.css']
})

export class EditRoomsComponent implements OnInit {
  public message = new BehaviorSubject<string>(null);
  room: Room;
  id: string;


  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { this.http = http; this.baseUrl = baseUrl; }


  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.room = {
      id: '',
      number: null,
      numberOfSeats: null,
      price: null,
      description: "",
    }
    this.fetchUser();
  }

  onSubmit(form: NgForm) {
    let room = {
      id: this.room.id,
      number: form.value.number,
      numberOfSeats: form.value.numberOfSeats,
      price: form.value.price,
      description: form.value.description,
    }

    this.editRoom(room);
  }

  async editRoom(roomInput) {
    this.http.put<Room>(this.baseUrl + 'api/rooms', roomInput).subscribe(result => {
      this.room = result;
      this.message.next('Room "' + this.room.number + '" was edited successfully.');
    }, error => this.message.next(error.error));
  }

  async fetchUser() {
    this.http.get<Room>(this.baseUrl + 'api/rooms/' + this.id).subscribe(result => {
      this.room = result;
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

