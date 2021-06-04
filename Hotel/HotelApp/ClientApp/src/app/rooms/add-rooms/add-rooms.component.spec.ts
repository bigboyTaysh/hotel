import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddRoomsComponent } from './add-rooms.component';

describe('AddRoomsComponent', () => {
  let component: AddRoomsComponent;
  let fixture: ComponentFixture<AddRoomsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddRoomsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddRoomsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
