import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListDevelopersComponent } from './list-developers.component';
import { AppModule } from '../app.module';

describe('ListDevelopersComponent', () => {
  let component: ListDevelopersComponent;
  let fixture: ComponentFixture<ListDevelopersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports:[AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListDevelopersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
