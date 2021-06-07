import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-customer-details',
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.css']
})
export class CustomerDetailsComponent implements OnInit {
  public id: string;
  public customer: Customer;
  public message = new BehaviorSubject<string>(null);

  constructor(private route: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.fetchCustomer();
  }

  async fetchCustomer() {
    this.http.get<Customer>(this.baseUrl + 'api/customers/' + this.id).subscribe(result => {
      this.customer = result;
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
