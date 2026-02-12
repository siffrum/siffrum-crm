import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompanyLettersComponent } from './company-letters.component';

describe('CompanyLettersComponent', () => {
  let component: CompanyLettersComponent;
  let fixture: ComponentFixture<CompanyLettersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompanyLettersComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompanyLettersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
