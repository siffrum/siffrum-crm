import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicSqlReportsComponent } from './dynamic-sql-reports.component';

describe('DynamicSqlReportsComponent', () => {
  let component: DynamicSqlReportsComponent;
  let fixture: ComponentFixture<DynamicSqlReportsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DynamicSqlReportsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DynamicSqlReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
