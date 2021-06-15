import { Location } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent implements OnInit {
  public message = new BehaviorSubject<string>(null);
  customer: Customer;
  id: string;

  constructor(private route: ActivatedRoute,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private _location: Location) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');

    this.customer = {
      id: "",
      firstname: "",
      lastname: "",
      birthdate: new Date().toISOString().split('T')[0],
      phone: "",
      email: "",
      address: {
        street: "",
        zipCode: "",
        city: "",
        country: ""
      }
    }
    this.fetchCustomer();
  }

  onSubmit(form: NgForm) {
    let customer = {
      id: this.id,
      firstname: form.value.firstname,
      lastname: form.value.lastname,
      birthdate: this.customer.birthdate,
      phone: form.value.phone,
      email: form.value.email,
      address: {
        street: form.value.street,
        zipCode: form.value.zipcode,
        city: form.value.city,
        country: form.value.country
      }
    }

    this.editCustomer(customer);
  }

  async editCustomer(inputCustomer) {
    this.http.put<Customer>(this.baseUrl + 'api/customers', inputCustomer).subscribe(result => {
      this.customer = result;
      this.message.next('Customer "' + this.customer.firstname + " " + this.customer.lastname + '" was edited successfully.');
    }, error => this.message.next(error.error));
  }

  
  async fetchCustomer() {
    this.http.get<Customer>(this.baseUrl + 'api/customers/' + this.id).subscribe(result => {
      this.customer = result;
    }, error => console.error(error));
  }

  back() {
    this._location.back();
  }
}

interface Customer {
  id: string;
  firstname: string;
  lastname: string;
  birthdate: string;
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
