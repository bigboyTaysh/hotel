import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.css']
})
export class CustomersComponent {
  public customers: Customer[];
  public filter: CustomerFilter;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.filter = {
      id: "",
      name: "",
      phone: "",
      email: "",
    }
    
    http.get<Customer[]>(baseUrl + 'api/customers').subscribe(result => {
      this.customers = result;
    }, error => console.error(error));
  }

  onDelete(id: string) {
    this.http.delete<Customer[]>(this.baseUrl + 'api/customers/' + id).subscribe(result => {
      let user = this.customers.find(x => x.id === id);
      let index = this.customers.indexOf(user, 0);
      this.customers.splice(index, 1);
    }, error => console.error(error));
  }

  onSearch(form: NgForm) {
    this.filter.id = form.value.id;
    this.filter.name = form.value.name;
    this.filter.email = form.value.email;
    this.filter.phone = form.value.phone;
    this.getCustomerByName();
  }

  async getCustomerByName() {
    this.http.post<Customer[]>(this.baseUrl + 'api/customers/customerByName', this.filter).subscribe(result => {
      this.customers = result;
    }, error => console.error(error));
  }
}

interface Customer {
  id: string;
  firstname: string;
  lastname: string;
  birthdate: Date;
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

interface CustomerFilter {
  id: string;
  name: string;
  phone: string;
  email: string;
}