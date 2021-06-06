import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-add-customer',
  templateUrl: './add-customer.component.html',
  styleUrls: ['./add-customer.component.css']
})
export class AddCustomerComponent implements OnInit {
  customer: Customer;
  http: HttpClient;
  baseUrl: string;
  public message = new BehaviorSubject<string>(null);

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) { this.http = http; this.baseUrl = baseUrl; }

  ngOnInit() {
  }

  onSubmit(form: NgForm) {
    let customer = {
      id: "",
      firstname: form.value.firstname,
      lastname: form.value.lastname,
      birthdate: form.value.birthdate,
      phone: form.value.phone,
      email: form.value.email,
      address: {
        street: form.value.street,
        zipcode: form.value.zipcode,
        city: form.value.city,
        country: form.value.country
      }
    }


    this.addCustomer(customer, form);
  }

  async addCustomer(customerInput, form) {
    console.log(customerInput);
    this.http.post<Customer>(this.baseUrl + 'api/customers', customerInput).subscribe(result => {
      this.customer = result;
      this.message.next('Customer "' + this.customer.firstname + " " + this.customer.lastname + '" was added successfully.');
      form.reset();
    }, error => this.message.next(error.error));
  }
}

interface Customer {
  id: string;
  firstname: string;
  lastname: string;
  birthdate: Date;
  phone: string;
  email: string;
  address: {
    street: string;
    zipcode: string;
    city: string;
    country: string;
  }
}
