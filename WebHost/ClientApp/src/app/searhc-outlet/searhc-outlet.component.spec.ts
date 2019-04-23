import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearhcOutletComponent } from './searhc-outlet.component';

describe('SearhcOutletComponent', () => {
  let component: SearhcOutletComponent;
  let fixture: ComponentFixture<SearhcOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearhcOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearhcOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
