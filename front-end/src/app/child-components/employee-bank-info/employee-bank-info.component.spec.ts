import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeBankInfoComponent } from './employee-bank-info.component';

describe('EmployeeBankInfoComponent', () => {
  let component: EmployeeBankInfoComponent;
  let fixture: ComponentFixture<EmployeeBankInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeBankInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeBankInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
