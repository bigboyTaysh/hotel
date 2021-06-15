import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-add-rooms',
  templateUrl: './add-rooms.component.html',
  styleUrls: ['./add-rooms.component.css']
})

export class AddRoomsComponent implements OnInit {
  room: Room;
  http: HttpClient;
  baseUrl: string;
  public message = new BehaviorSubject<string>(null);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private _location: Location) { this.http = http; this.baseUrl = baseUrl; }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    let room = {
      id: "",
      number: form.value.number,
      numberOfSeats: form.value.numberOfSeats,
      price: form.value.price,
      description: form.value.description,
    }

    this.addRoom(room, form);
  }

  async addRoom(roomInput, form) {
    console.log(roomInput);
    this.http.post<Room>(this.baseUrl + 'api/rooms', roomInput).subscribe(result => {
      this.room = result;
      this.message.next('Room "' + this.room.number + '" was added successfully.');
      form.reset();
    }, error => this.message.next(error.error));
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
