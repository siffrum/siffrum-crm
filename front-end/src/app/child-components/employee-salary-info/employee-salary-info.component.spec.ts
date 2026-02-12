import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeSalaryInfoComponent } from './employee-salary-info.component';

describe('EmployeeSalaryInfoComponent', () => {
  let component: EmployeeSalaryInfoComponent;
  let fixture: ComponentFixture<EmployeeSalaryInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeSalaryInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeSalaryInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
