import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeLeavesInfoComponent } from './employee-leaves-info.component';

describe('EmployeeLeavesInfoComponent', () => {
  let component: EmployeeLeavesInfoComponent;
  let fixture: ComponentFixture<EmployeeLeavesInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeLeavesInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeLeavesInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
