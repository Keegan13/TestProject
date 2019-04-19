import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllDevelopersComponent } from './all-developers.component';

describe('AllDevelopersComponent', () => {
  let component: AllDevelopersComponent;
  let fixture: ComponentFixture<AllDevelopersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllDevelopersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllDevelopersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
