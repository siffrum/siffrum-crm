import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyAttendanceShiftComponent } from './company-attendance-shift.component';

describe('CompanyAttendanceShiftComponent', () => {
  let component: CompanyAttendanceShiftComponent;
  let fixture: ComponentFixture<CompanyAttendanceShiftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyAttendanceShiftComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompanyAttendanceShiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
