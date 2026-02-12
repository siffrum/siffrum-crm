import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeAddressInfoComponent } from './employee-address-info.component';

describe('EmployeeAddressInfoComponent', () => {
  let component: EmployeeAddressInfoComponent;
  let fixture: ComponentFixture<EmployeeAddressInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeAddressInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeAddressInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
