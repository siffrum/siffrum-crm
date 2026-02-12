import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeGenerateLetterInfoComponent } from './employee-generate-letter-info.component';

describe('EmployeeGenerateLetterInfoComponent', () => {
  let component: EmployeeGenerateLetterInfoComponent;
  let fixture: ComponentFixture<EmployeeGenerateLetterInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeGenerateLetterInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeGenerateLetterInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
