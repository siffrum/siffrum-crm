import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDocumentInfoComponent } from './employee-document-info.component';

describe('EmployeeDocumentInfoComponent', () => {
  let component: EmployeeDocumentInfoComponent;
  let fixture: ComponentFixture<EmployeeDocumentInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmployeeDocumentInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmployeeDocumentInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
