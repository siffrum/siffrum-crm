import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SqlReportComponent } from './sql-report.component';

describe('SqlReportComponent', () => {
  let component: SqlReportComponent;
  let fixture: ComponentFixture<SqlReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SqlReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SqlReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
