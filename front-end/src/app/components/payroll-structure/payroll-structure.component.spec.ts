import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollStructureComponent } from './payroll-structure.component';

describe('PayrollStructureComponent', () => {
  let component: PayrollStructureComponent;
  let fixture: ComponentFixture<PayrollStructureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PayrollStructureComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PayrollStructureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
