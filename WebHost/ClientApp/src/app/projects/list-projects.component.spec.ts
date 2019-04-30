import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListProjectsComponent } from './list-projects.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AppModule } from '../app.module';

describe('ListProjectsComponent', () => {
  let component: ListProjectsComponent;
  let fixture: ComponentFixture<ListProjectsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports:[AppModule]
      
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListProjectsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
